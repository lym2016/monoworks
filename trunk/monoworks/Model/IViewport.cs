// IViewport.cs - MonoWorks Project
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

namespace MonoWorks.Model
{
	
	/// <summary>
	/// The IViewport interface defines an interface for MonoWorks viewports. 
	/// This lets the model interact with viewports in a GUI-independant manner.
	/// </summary>
	public interface IViewport
	{
		/// <summary>
		/// Returns the viewport width.
		/// </summary>
		int Width();
		
		/// <summary>
		/// Returns the viewport height.
		/// </summary>
		int Height();
		
		/// <value>
		/// Access the viewport camera.
		/// </value>
		Camera Camera
		{
			get;
		}
		
		/// <summary>
		/// Initializes the rendering.
		/// </summary>
		void Initialize();
		
		/// <summary>
		/// Performs the rendering for one frame.
		/// </summary>
		void Paint();

		/// <value>
		/// Access the viewport's render manager.
		/// </value>
		RenderManager RenderManager
		{
			get;
		}
	}
}