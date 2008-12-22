// PaneControls.cs - MonoWorks Project
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

using MonoWorks.Base;
using MonoWorks.Rendering;
using MonoWorks.Rendering.Controls;
using MonoWorks.GuiGtk;

namespace MonoWorks.PlottingDemoGtk
{
	
	/// <summary>
	/// Contains the rendering controls demo.
	/// </summary>
	public class PaneControls : Gtk.HBox
	{
		
		public PaneControls() : base()
		{
			// add the viewport
			TooledViewport tooledViewport = new TooledViewport(ViewportUsage.Plotting, false);
			PackEnd(tooledViewport);

			Viewport viewport = tooledViewport.Viewport;

			Button button1 = new Button("Hello Blah");
			button1.Position = new Coord(300, 300);
			viewport.RenderList.AddOverlay(button1);

			Button button2 = new Button("Button 2");
			button2.Position = new Coord(350, 350);
			viewport.RenderList.AddOverlay(button2);
		}
	}
}