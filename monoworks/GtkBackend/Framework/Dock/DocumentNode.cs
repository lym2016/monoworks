// DocumentNode.cs - Slate Mono Application Framework
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

namespace MonoWorks.GtkBackend.Framework.Dock
{
	
	/// <summary>
	/// The node in the docking framework for documents.
	/// </summary>
	public class DocumentNode : DockableNode
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="document"> A <see cref="Dockable"/>. </param>
		public DocumentNode(Dockable document) : base(document)
		{
		}
		
//		public override Node Parent {
//			get { return parent; }
//			set
//			{
//				if (value is BookNode) // the parent is already a book
//				{
//					parent = value;
//				}
//				else if (value == null)
//				{
//					parent = value;
//				}
//				else
//				{
//					parent = new BookNode();
//					Node[] children = new Node[value.NumChildren];
//					value.Children.CopyTo(children, 0);
//					foreach (Node child in children)
//						value.Remove(child);
//					value.Add(parent);
//					parent.Add(this);
//				}
//			}
//		}

	}
}
