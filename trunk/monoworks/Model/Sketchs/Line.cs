////   Sketchable.cs - MonoWorks Project
////
////    Copyright Andy Selvig 2008
////
////    This program is free software: you can redistribute it and/or modify
////    it under the terms of the GNU Lesser General Public License as published 
////    by the Free Software Foundation, either version 3 of the License, or
////    (at your option) any later version.
////
////    This program is distributed in the hope that it will be useful,
////    but WITHOUT ANY WARRANTY; without even the implied warranty of
////    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
////    GNU Lesser General Public License for more details.
////
////    You should have received a copy of the GNU Lesser General Public 
////    License along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using MonoWorks.Base;

using gl = Tao.OpenGl.Gl;

namespace MonoWorks.Model
{
	
	/// <summary>
	/// The Line class represents a line in 3D space connected by two point.
	/// </summary>
	public class Line : Sketchable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Line() : base()
		{
			
		}
		
		
		/// <summary>
		/// Initialization constructor.
		/// Sets the values of the first two points.
		/// </summary>
		/// <param name="p1"> The first <see cref="Point"/>. </param>
		/// <param name="p2"> The second <see cref="Point"/>. </param>
		public Line(Point p1, Point p2) : this()
		{
			Points.Add(p1);
			Points.Add(p2);
		}
	

#region Momentos
				
		/// <summary>
		/// Appends a momento to the momento list.
		/// </summary>
		protected override void AddMomento()
		{
			base.AddMomento();
			Momento momento = momentos[momentos.Count-1];
			momento["points"] = new List<Point>();
		}
		
#endregion
		
		
#region Points

		/// <value>
		/// The list of points.
		/// </value>
		public List<Point> Points
		{
			get {return (List<Point>)CurrentMomento["points"];}
		}
			
#endregion
		
		
#region Geometry
		
		/// <summary>
		/// Computes the raw points needed to draw the sketch.
		/// </summary>
		public override void ComputeGeometry()
		{
			solidPoints = new Vector[Points.Count];
			for (int i=0; i<Points.Count; i++)
			{
				solidPoints[i] = Points[i].ToVector();
				bounds.Resize(solidPoints[i]);
			}
			
			// for lines, the solid and wireframe points are the same
			wireframePoints = solidPoints;
			
		}
		
		
		/// <summary>
		/// Draws the vertices to the current GL context.
		/// Updates the geometry if the number of points has changes.
		/// </summary>
		public override void DrawVertices()
		{
			if (Points.Count != solidPoints.Length)
				ComputeGeometry();
			base.DrawVertices();
		}
		
#endregion
		
	}
	
}