﻿// ControlExtensions.cs - MonoWorks Project
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
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;
using sd = System.Drawing;
using swf = System.Windows.Forms;


namespace MonoWorks.Wpf.Backend
{
	/// <summary>
	/// Extensions for the combo box control.
	/// </summary>
	public static class ComboBoxExtensions
	{
		/// <summary>
		/// Convenience method for adding a text item.
		/// </summary>
		/// <param name="combo"></param>
		/// <param name="text"></param>
		public static void AddItem(this ComboBox combo, string text)
		{
			ComboBoxItem item = new ComboBoxItem();
			item.Content = text;
			combo.Items.Add(item);
		}

		/// <summary>
		/// Convenience method to get the selected item as a string.
		/// </summary>
		/// <param name="combo"></param>
		/// <returns></returns>
		public static string GetSelectedText(this ComboBox combo)
		{
			return (string)((combo.SelectedItem as ComboBoxItem).Content); 
		}
	}


	/// <summary>
	/// Extensions for the WPF grid to get rid of some of those lame static methods.
	/// </summary>
	public static class GridExtensions
	{
		/// <summary>
		/// Adds element to the grid at the given row and column.
		/// </summary>
		public static void AddAt(this Grid grid, UIElement element, int row, int col)
		{
			grid.Children.Add(element);
			Grid.SetColumn(element, col);
			Grid.SetRow(element, row);
		}

		/// <summary>
		/// Adds element to the grid at the given row and column.
		/// </summary>
		/// <remarks>Also define row and column span.</remarks>
		public static void AddAt(this Grid grid, UIElement element, int row, int col, int rowspan, int colspan)
		{
			grid.Children.Add(element);
			Grid.SetColumn(element, col);
			Grid.SetColumnSpan(element, colspan);
			Grid.SetRow(element, row);
			Grid.SetRowSpan(element, rowspan);
		}

		/// <summary>
		/// Adds a row definition.
		/// </summary>
		public static RowDefinition AddRow(this Grid grid)
		{
			RowDefinition rowDef = new RowDefinition();
			grid.RowDefinitions.Add(rowDef);
			return rowDef;
		}

		/// <summary>
		/// Adds a row definition with a defined height.
		/// </summary>
		public static RowDefinition AddRow(this Grid grid, double height)
		{
			RowDefinition rowDef = new RowDefinition();
			rowDef.Height = new GridLength(height);
			grid.RowDefinitions.Add(rowDef);
			return rowDef;
		}

		/// <summary>
		/// Adds a row definition with automatic height.
		/// </summary>
		public static RowDefinition AddAutoRow(this Grid grid)
		{
			RowDefinition rowDef = new RowDefinition();
			rowDef.Height = GridLength.Auto;
			grid.RowDefinitions.Add(rowDef);
			return rowDef;
		}

		/// <summary>
		/// Adds a column definition.
		/// </summary>
		public static ColumnDefinition AddColumn(this Grid grid)
		{
			ColumnDefinition colDef = new ColumnDefinition();
			grid.ColumnDefinitions.Add(colDef);
			return colDef;
		}

		/// <summary>
		/// Adds a column definition with a defined width.
		/// </summary>
		public static ColumnDefinition AddColumn(this Grid grid, double width)
		{
			ColumnDefinition colDef = new ColumnDefinition();
			colDef.Width = new GridLength(width);
			grid.ColumnDefinitions.Add(colDef);
			return colDef;
		}

		/// <summary>
		/// Adds a column definition with automatic width.
		/// </summary>
		/// <param name="grid"></param>
		public static ColumnDefinition AddAutoColumn(this Grid grid)
		{
			ColumnDefinition colDef = new ColumnDefinition();
			colDef.Width = GridLength.Auto;
			grid.ColumnDefinitions.Add(colDef);
			return colDef;
		}

	}



	public static class LabelExtensions
	{
		/// <summary>
		/// Adds a label to a panel.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="text"></param>
		public static void AddLabel(this Panel panel, string text)
		{
			Label label = new Label();
			label.Content = text;
			panel.Children.Add(label);
		}

	}

}