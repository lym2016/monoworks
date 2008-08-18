// MainWindow.cs - MonoWorks Project
//
//  Copyright (C) 2008 Andy Selvig
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

using MonoWorks.Plotting;
using MonoWorks.GuiGtk;

namespace MonoWorks.PlottingDemoGtk
{
	
	/// <summary>
	/// Main window for the Gtk port of the plotting demo.
	/// </summary>
	public class MainWindow : Gtk.Window
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public MainWindow() : base(Gtk.WindowType.Toplevel)
		{
			SetSizeRequest(800,800);
			Title = "MonoWorks Plotting Demo";
			
			DeleteEvent += OnDeleteEvent;
			
			// add the viewport
			Viewport viewport = new Viewport();
			Add(viewport);
			TestAxes axes = new TestAxes();
			viewport.AddRenderable(axes);
			
			viewport.Camera.Center = new MonoWorks.Base.Vector(0, 0, 0);
			viewport.Camera.Position = new MonoWorks.Base.Vector(7, 0, 0);
			viewport.Camera.UpVector = new MonoWorks.Base.Vector(0, 0, 1);
			viewport.Camera.RecomputeUpVector();
			
			ShowAll();
		}
		
		
		protected void OnDeleteEvent(object sender, Gtk.DeleteEventArgs args)
		{
			args.RetVal = true;
			Gtk.Application.Quit();
		}
	}
}
