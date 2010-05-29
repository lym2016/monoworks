// ActorPane.cs - MonoWorks Project
//
//  Copyright (C) 2009 Andy Selvig
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 

using System;

using MonoWorks.Base;
using MonoWorks.Rendering;
using MonoWorks.Rendering.Events;

using Tao.OpenGl;

namespace MonoWorks.Controls
{
	
	/// <summary>
	/// The ActorPane is an 3D plane that can be placed directly into the scene.
	/// </summary>
	public class ActorPane : Actor, IPane, IPlane
	{
		
		public ActorPane() : base()
		{
			Origin = new Vector();
			XAxis = new Vector(1, 0, 0);
			Normal = new Vector(0, 0, 1);
			_scaling = 1;
		}
				
		public ActorPane(Control2D control) : this()
		{
			Control = control;
		}
		
		
		/// <value>
		/// The size of the pane.
		/// </value>
		public Coord RenderSize
		{
			get
			{
				if (Control != null)
					return Control.RenderSize;
				else
					return new Coord();
			}
		}
		
		public double RenderWidth
		{
			get {return RenderSize.X;}
		}
		
		public double RenderHeight
		{
			get {return RenderSize.Y;}
		}
		
		[MwxProperty]
		public Vector Origin { get; set; }
		
		[MwxProperty]
		public Vector Normal { get; set; }
		
		[MwxProperty]
		public Vector XAxis { get; set; }
		
		private Control2D control;
		
		public Control2D Control
		{
			get {return control;}
			set
			{
				if (control != null)
					control.Pane = null;
				if (value != null)
				{
					control = value;
					control.Pane = this;
				}
			}
		}
		
		
		
#region Interaction
		
		private Control2D _inFocus;
		/// <summary>
		/// Sets the contro currently in focus.
		/// </summary>
		public Control2D InFocus
		   {
			get { return _inFocus; }
			set {
				if (_inFocus == value)
					return;
				if (_inFocus != null)
					_inFocus.IsFocused = false;
				_inFocus = value;
				if (_inFocus == null)
					return;
				if (!_inFocus.IsFocused)
					_inFocus.IsFocused = true;
			}
		}
		
		/// <summary>
		/// Gets a point in control-space corresponding to the hit line in 3D space.
		/// </summary>
		private Coord GetControlPoint(HitLine hitLine)
		{
			if (RenderSize == null)
				return new Coord();
			var intersection = hitLine.GetIntersection(this);
			var point = this.Project(intersection) / _scaling;
			point.Y = RenderHeight - point.Y;
			return point;
		}
		
		public override void OnButtonPress(MouseButtonEvent evt)
		{
			base.OnButtonPress(evt);
			
			if (Control != null)
			{
				var controlEvt = new MouseButtonEvent(evt.Scene, GetControlPoint(evt.HitLine), evt.Button, evt.Modifier, evt.Multiplicity);
				if (evt.IsHandled)
					controlEvt.Handle(this);
				Control.OnButtonPress(controlEvt);
				if (controlEvt.IsHandled)
					evt.Handle(this);
			}
		}

		public override void OnButtonRelease(MouseButtonEvent evt)
		{
			base.OnButtonRelease(evt);
			
			if (Control != null)
			{
				var controlEvt = new MouseButtonEvent(evt.Scene, GetControlPoint(evt.HitLine), evt.Button, evt.Modifier, evt.Multiplicity);
				if (evt.IsHandled)
					controlEvt.Handle(this);
				Control.OnButtonRelease(controlEvt);
				if (controlEvt.IsHandled)
					evt.Handle(this);
			}
		}

		public override void OnMouseMotion(MouseEvent evt)
		{
			base.OnMouseMotion(evt);
			
			if (Control != null)
			{
				var controlEvt = new MouseEvent(evt.Scene, GetControlPoint(evt.HitLine), evt.Modifier);
				if (evt.IsHandled)
					controlEvt.Handle(this);
				Control.OnMouseMotion(controlEvt);
				if (controlEvt.IsHandled)
					evt.Handle(this);
			}
		}	
		
		public override void OnKeyPress(KeyEvent evt)
		{
			base.OnKeyPress(evt);
			
			if (InFocus != null)
			{
				InFocus.OnKeyPress(evt);
			}
		}		

#endregion

		
#region Rendering
		
		/// <summary>
		/// If null, the pane will always be scaled so that the control's coordinates map closely to screen coordinates.
		/// Otherwise, this scaling will be used to go between world and screen coordinates.
		/// </summary>
		[MwxProperty]
		public double? Scaling { get; set; }
		
		/// <summary>
		/// Actual scaling used in geometry calculations.
		/// </summary>
		/// <remarks>Could be the one specified in Scaling, or the default on computed.</remarks>
		private double _scaling;
		
		/// <summary>
		/// This is set true if the pane was dirty last render cycle.
		/// </summary>
		private bool wasDirty = false;

		public void QueueRender()
		{
			wasDirty = true;
		}
		
		/// <summary>
		/// Handle to the OpenGL texture that the control will be rendered to.
		/// </summary>
		private uint texture = 0;
		
		public override void ComputeGeometry()
		{
			base.ComputeGeometry();
			
			if (Control == null)
				return;
						
			if (texture == 0)
				Gl.glGenTextures(1, out texture);
			
			wasDirty = true;
			
		}
		
		public override void RenderTransparent(Scene scene)
		{
			if (Control == null)
				return;
			
			base.RenderTransparent(scene);
			
			if (Control.IsDirty)
				ComputeGeometry();
			
			// render the control to the texture
			if (wasDirty)
			{
				Gl.glBindTexture(Gl.GL_TEXTURE_RECTANGLE_ARB, texture);
				Control.RenderImage(scene);
				Gl.glTexImage2D(Gl.GL_TEXTURE_RECTANGLE_ARB,
			                0, 
			                Gl.GL_RGBA, 
			                Control.IntWidth, 
			                Control.IntHeight, 
			                0, 
			                Gl.GL_BGRA, 
			                Gl.GL_UNSIGNED_BYTE, 
			                Control.ImageData);
				wasDirty = false;
			}
			
			// determine how big the control should be 
			double width = RenderWidth;
			double height = RenderHeight;
			if (Scaling == null) {
				_scaling = scene.Camera.SceneToWorldScaling;
			}
			
			else
			{
				_scaling = (double)Scaling;
			}
			if (_scaling != 1) {
				width *= _scaling;
				height *= _scaling;
			}
			
			bounds.Reset();
			
			// render the texture
			scene.Lighting.Disable();
			Gl.glEnable(Gl.GL_TEXTURE_RECTANGLE_ARB);
			Gl.glBindTexture(Gl.GL_TEXTURE_RECTANGLE_ARB, texture);
			Gl.glBegin(Gl.GL_QUADS);
			Gl.glColor3f(1f, 1f, 1f);
			
			Gl.glTexCoord2d(0.0, Control.RenderHeight);
			var vert = Origin;
			vert.glVertex();
			bounds.Resize(vert);
			
			Gl.glTexCoord2d(Control.RenderWidth, Control.RenderHeight);
			vert += XAxis * width;
			vert.glVertex();
			bounds.Resize(vert);
			
			Gl.glTexCoord2d(Control.RenderWidth, 0.0);
			vert += this.YAxis() * height;
			vert.glVertex();
			bounds.Resize(vert);
			
			Gl.glTexCoord2d(0.0, 0.0);
			vert -= XAxis * width;
			vert.glVertex();
			bounds.Resize(vert);
			
			Gl.glEnd();
			Gl.glDisable(Gl.GL_TEXTURE_RECTANGLE_ARB);
			scene.Lighting.Enable();
		}

#endregion
		
	}
}
