// Viewport.cs - MonoWorks Project
//
// Copyright (C) 2008 Andy Selvig
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using System.Collections.Generic;

using Qyoto;

using gl = Tao.OpenGl.Gl;
using glu = Tao.OpenGl.Glu;

using mwb = MonoWorks.Base;
using MonoWorks.Model;


namespace MonoWorks.Gui
{
	
	// Dictionary that maps the mouse event mask to an interaction action.
	using MouseMap = Dictionary<int, MouseAction>;
		
	
	// Dictionary that maps the mouse event mask to a cursor.
	using CursorMap = Dictionary<MouseAction, QCursor>;
	
	
	// Dictionary that maps the scroll event mask to an interaction action.
	using ScrollMap = Dictionary<long, ScrollAction>;
	
	
	/// <summary>
	/// Possible mouse interaction actions.
	/// </summary>
	public enum MouseAction :short {NONE, PAN, ZOOM, DOLLY, ROTATE, SPIN};
	
	
	/// <summary>
	/// Possible scroll actions.
	/// </summary>
	public enum ScrollAction :short {NONE, DOLLY, VERTICAL_PAN, HORIZONTAL_PAN};
	
	
	/// <summary>
	/// The Viewport class represents a viewable area for rendering models,
	/// </summary>
	public class Viewport : QGLWidget, IViewport
	{

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Viewport() : base()
		{
			// ensure the resource manager is initialized
			ResourceManager.Initialize();
			
			camera = new Camera(this);
			mouseAction = MouseAction.NONE;

			// assign default cursor positions
			lastX = 0.0;
			lastY = 0.0;
			currentX = 0.0;
			currentY = 0.0;
			
			lastExposed = DateTime.Now;

			// create the rubber band
			rubberBand = new RubberBand();
			
			// initialize the mouse map
			mouseMap = new MouseMap();
			mouseMap[(int)Qt.MouseButton.LeftButton] = MouseAction.ROTATE;
			mouseMap[(int)Qt.MouseButton.MidButton] = MouseAction.ZOOM;
			mouseMap[(int)Qt.MouseButton.RightButton] = MouseAction.PAN;
			mouseMap[(int)Qt.MouseButton.MidButton | (int)Qt.Modifier.CTRL] = MouseAction.DOLLY;
			
			// create the cursor map
			cursorMap = new CursorMap();
			cursorMap[MouseAction.PAN] = new QCursor(Qt.CursorShape.OpenHandCursor);
			cursorMap[MouseAction.ZOOM] = ResourceManager.GetCursor("zoom");
			cursorMap[MouseAction.DOLLY] = ResourceManager.GetCursor("dolly");
			cursorMap[MouseAction.ROTATE] =  new QCursor(Qt.CursorShape.ClosedHandCursor);
			cursorMap[MouseAction.SPIN] = ResourceManager.GetCursor("spin");
			
			
			// initialize the scroll map
			scrollMap = new ScrollMap();
			scrollMap[0] = ScrollAction.DOLLY;
			scrollMap[(int)Qt.Modifier.CTRL] = ScrollAction.VERTICAL_PAN;
			scrollMap[(int)Qt.Modifier.SHIFT] = ScrollAction.HORIZONTAL_PAN;

			// initialize the render manager
			renderManager = new RenderManager();
			
			this.SetAttribute(Qt.WidgetAttribute.WA_NoSystemBackground);
		}
		
		
		/// <summary>
		/// Constructs the viewport and sets the document.
		/// </summary>
		/// <param name="document"> A <see cref="Document"/>. </param>
		public Viewport(Document document) : this()
		{
			this.document = document;
		}
		
		
#region Document
		
		/// <value>
		/// The document associated with this viewport.
		/// </value>
		protected Document document;		
		/// <value>
		/// The document associated with this viewport.
		/// </value>
		public Document Document
		{
			get {return document;}
			set {document = value;}
		}
		
#endregion

		
#region Attributes
		
		/// <value>
		/// The camera.
		/// </value>
		protected Camera camera;
		/// <value>
		/// Accesses the viewport camera.
		/// </value>
		public Camera Camera
		{
			get {return camera;}
			set {camera = value;}
		}
		

		/// <value>
		/// The current mouse interaction action.
		/// </value>
		protected MouseAction mouseAction;
		/// <value>
		/// The current mouse interaction action.
		/// </value>
		public MouseAction MouseAction
		{
			get {return mouseAction;}
		}
		
		/// <value>
		/// The cursor map.
		/// </value>
		protected CursorMap cursorMap;
		
		
		/// <value>
		/// Maps mouse event masks to interaction actions.
		/// </value>
		protected MouseMap mouseMap;
		/// <value>
		/// Maps mouse event masks to interaction actions.
		/// </value>
		public MouseMap MouseMap
		{
			get {return mouseMap;}
		}
		
		
		/// <value>
		/// Maps scroll event masks to interaction actions.
		/// </value>
		protected ScrollMap scrollMap;
		/// <value>
		/// Maps scroll event masks to interaction actions.
		/// </value>
		public ScrollMap ScrollMap
		{
			get {return scrollMap;}
		}
		
		/// <value>
		/// Last x position.
		/// </value>
		protected double lastX;
		/// <value>
		/// The x value of the last mouse position.
		/// </value>
		public double LastX
		{
			get {return lastX;}
		}
		
		/// <value>
		/// Last y position.
		/// </value>
		protected double lastY;
		/// <value>
		/// The y value of the last mouse position.
		/// </value>
		public double LastY
		{
			get {return lastY;}
		}
		
		
		/// <value>
		/// Current x position.
		/// </value>
		protected double currentX;
		/// <value>
		/// The x value of the current mouse position.
		/// </value>
		public double CurrentX
		{
			get {return currentX;}
		}
		
		/// <value>
		/// Current y position.
		/// </value>
		protected double currentY;
		/// <value>
		/// The y value of the current mouse position.
		/// </value>
		public double CurrentY
		{
			get {return currentY;}
		}
		
		/// <value>
		/// Time of the last rendering.
		/// </value>
		protected DateTime lastExposed;
		
		
		protected RenderManager renderManager;
		/// <value>
		/// The viewport render manager.
		/// </value>
		public RenderManager RenderManager
		{
			get {return renderManager;}
		}
		
#endregion
		
		
#region Event Handlers
		
		/// <summary>
		/// Handles button press events.
		/// </summary>
		/// <param name="evt">The <see cref="QMouseEvent"/>. </param>
		protected override void MousePressEvent(QMouseEvent evt)
		{
			// get the position
			lastX = evt.X();
			lastY = evt.Y();
			
			// get the mask for this button event
//			Console.WriteLine("mouse event modifiers: {0}", evt.Modifiers());
//			int mask = (int)evt.Button() | (int)evt.Modifiers();
			int mask = (int)evt.Button();
				
			// determine if the mask is present in the map
			if (mouseMap.ContainsKey(mask))
			{
				// store the current action
				mouseAction = mouseMap[mask];
				
				// set the override cursor
				if (cursorMap.ContainsKey(mouseAction))
				{
					QApplication.SetOverrideCursor(new QCursor(cursorMap[mouseAction]));
				}
				
				// special stuff for zooming
				if (mouseAction==MouseAction.ZOOM)
				{
					rubberBand.StartX = (int)lastX;
					rubberBand.StartY = (int)lastY;
					rubberBand.Enabled = true;
				}
			}
			UpdateGL();

		}
			
		
		/// <summary>
		/// Handles mouse double-click events.
		/// </summary>
		/// <param name="evt">The <see cref="QMouseEvent"/>. </param>
		protected override void MouseDoubleClickEvent(QMouseEvent evt)
		{
			camera.Reset();
			UpdateGL();
		}
		
		/// <summary>
		/// Handles button release events.
		/// </summary>
		/// <param name="evt">The <see cref="QMouseEvent"/>. </param>	
		protected override void MouseReleaseEvent(QMouseEvent evt)
		{

			// special handling for zoom
			if (mouseAction==MouseAction.ZOOM)
			{
				camera.Zoom(rubberBand.StartX, rubberBand.StartY,
				              rubberBand.StopX, rubberBand.StopY);
			}
			
			mouseAction = MouseAction.NONE;
			rubberBand.Enabled = false;					
			
			// reset the override cursor
			QApplication.SetOverrideCursor(new QCursor(Qt.CursorShape.ArrowCursor));
			
			UpdateGL();
		}
		

		/// <summary>
		/// Handles mouse motion.
		/// </summary>
		/// <param name="evt">The <see cref="QMouseEvent"/>. </param>	
		protected override void MouseMoveEvent(QMouseEvent evt)
		{
			// get the new mouse coordinates
			currentX = evt.X();
			currentY = evt.Y();
			
			// decide what to do based on the mouse mode
			bool queueDraw = true;
			switch (mouseAction)
			{
			case MouseAction.DOLLY:
				OnDolly();
				break;
			case MouseAction.PAN:
				OnPan();
				break;
			case MouseAction.ROTATE:
				OnRotate();
				break;
			case MouseAction.ZOOM:
				OnZoom();
				break;
			case MouseAction.NONE:
				queueDraw = false;
				break;
			}
			lastX = currentX;
			lastY = currentY;
			if (queueDraw)
				UpdateGL();
		}
		
		
		/// <summary>
		/// Handles mouse scrolling.
		/// </summary>
		/// <param name="evt">The <see cref="WheelEvent"/>. </param>
		protected override void WheelEvent( QWheelEvent evt)
		{
			
			// determine what action to do based on the scroll map
//			ScrollAction action = scrollMap[evt.Buttons()];
			ScrollAction action = scrollMap[0];
			
			// decide what to do based on the action
			switch (action)
			{
			case ScrollAction.DOLLY:
				if (evt.Delta()<0)
					camera.DollyOut();
				else
					camera.DollyIn();
				break;
			case ScrollAction.VERTICAL_PAN:
				if (evt.Delta()<0)
					camera.PanDown();
				else
					camera.PanUp();
				break;
			case ScrollAction.HORIZONTAL_PAN:
				if (evt.Delta()<0)
					camera.PanRight();
				else
					camera.PanLeft();
				break;
			}
			UpdateGL();	
		}
		
		
#endregion
		
			
			
#region Interaction
				
		/// <summary>
		/// Callback for dolly motion.
		/// </summary>
		protected void OnDolly()
		{
			camera.Dolly((currentY-lastY) / (double)Height() * 5.0);
		}
		
		
		/// <summary>
		/// Callback for pan motion.
		/// </summary>
		protected void OnPan()
		{
			camera.Pan(currentX-lastX, currentY-lastY);
		}
		
		
		/// <summary>
		/// Callback for rotate motion.
		/// </summary>
		protected void OnRotate()
		{
			camera.Rotate(currentX-lastX, currentY-lastY);
		}
		
		
		/// <summary>
		/// Callback for zoom motion.
		/// </summary>
		protected void OnZoom()
		{
			rubberBand.StopX = currentX;
			rubberBand.StopY = currentY;
		}
					
#endregion
			
				
		
#region Views
		
		/// <summary>
		/// Sets the camera to the standard view.
		/// </summary>
		[Q_SLOT("StandardView()")]
		public void StandardView()
		{
			camera.StandardView();
		}
		
		/// <summary>
		/// Sets the camera to the front view.
		/// </summary>
		[Q_SLOT("FrontView()")]
		public void FrontView()
		{
			camera.FrontView();
		}
		
		/// <summary>
		/// Sets the camera to the back view.
		/// </summary>
		[Q_SLOT("BackView()")]
		public void BackView()
		{
			camera.BackView();
		}
		
		/// <summary>
		/// Sets the camera to the top view.
		/// </summary>
		[Q_SLOT("TopView()")]
		public void TopView()
		{
			camera.TopView();
		}
		
		/// <summary>
		/// Sets the camera to the bottom view.
		/// </summary>
		[Q_SLOT("BottomView()")]
		public void BottomView()
		{
			camera.BottomView();
		}
		
		/// <summary>
		/// Sets the camera to the right view.
		/// </summary>
		[Q_SLOT("RightView()")]
		public void RightView()
		{
			camera.RightView();
		}
		
		/// <summary>
		/// Sets the camera to the left view.
		/// </summary>
		[Q_SLOT("LeftView()")]
		public void LeftView()
		{
			camera.LeftView();
		}
		
#endregion
		
		
		
#region Rendering
		
		/// <value>
		/// Rubberband object.
		/// </value>
		protected RubberBand rubberBand;
		/// <value>
		/// The rubber band used for zooming and seleting.
		/// </value>
		public RubberBand RubberBand
		{
			get {return rubberBand;}
			set {rubberBand = value;}
		}
		
		
		/// <summary>
		/// OpenGL resize event handler.
		/// </summary>
		protected override void ResizeGL(int width, int height)
		{							
			camera.Configure();
		}

		
		
		/// <summary>
		/// Initializes the OpenGL rendering.
		/// </summary>
		protected override void InitializeGL()
		{
			RenderManager.SetupSolidMode();
			
			gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);			// Black Background
			gl.glClearDepth(1.0f);								// Depth Buffer Setup
			gl.glEnable(gl.GL_DEPTH_TEST);						// Enables Depth Testing
			gl.glDepthFunc(gl.GL_LEQUAL);						// The Type Of Depth Test To Do
			// Really Nice Perspective Calculations
			gl.glHint(gl.GL_PERSPECTIVE_CORRECTION_HINT, gl.GL_NICEST);
			
			// enable color tracking
			gl.glEnable(gl.GL_COLOR_MATERIAL);
			// set material properties which will be assigned by glColor
			gl.glColorMaterial(gl.GL_FRONT, gl.GL_AMBIENT_AND_DIFFUSE);

			
			// initialize Qt overlay
//			image = new QImage(Width(), Height(), QImage.Format.Format_ARGB32_Premultiplied);
//			image.Fill(Qt.QRgba(0, 0, 0, 127));
			
		}
				
		/// <summary>
		/// Initialize drawing.
		/// Calls InitializeGL().
		/// </summary>
		public void Initialize()
		{
			InitializeGL();
			UpdateGL();
		}
				
		
		/// <summary>
		/// Repaints the OpenGL surface.
		/// </summary>
		protected override void PaintGL()
		{
			base.PaintGL();
			
			// Clear The Screen And The Depth Buffer
			gl.glClear(gl.GL_COLOR_BUFFER_BIT | gl.GL_DEPTH_BUFFER_BIT);
			
			document.Render(this);

			// render the rubber band
			rubberBand.Render(this);
			
			// keep track of frame rate
			double frameRate = 10000000.0 / (double)(DateTime.Now.Ticks - lastExposed.Ticks);
			lastExposed = DateTime.Now;
			gl.glColor3f(1f, 1f, 1f);
			this.RenderText(5, Height()-5, String.Format("{0:f2} fps", frameRate));
			
			
			// test out the Qt overlay
//			QPainter painter = new QPainter();
//			painter.Begin(this);
//			painter.SetRenderHint( QPainter.RenderHint.Antialiasing );
//			painter.End();
		}
		
				
		/// <summary>
		/// Paint to the current context.
		/// Calls UpdateGL().
		/// </summary>
		public void Paint()
		{
			UpdateGL();
		}
		


		
		
#endregion
		
	}
}