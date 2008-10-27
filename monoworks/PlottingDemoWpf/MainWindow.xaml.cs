﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using MonoWorks.GuiWpf;
using MonoWorks.Rendering;
using MonoWorks.Plotting;

namespace MonoWorks.PlottingDemoWpf
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			book = new TabControl();
			dockPanel.Children.Add(book);

			// create the basic 3D tab
			Pane3D pane3d = new Pane3D();
			TabItem item3d = new TabItem();
			item3d.Header = "Basic 3D";
			item3d.Content = pane3d;
			book.Items.Add(item3d);


			// create the basic 2D tab
			Pane2D pane2d = new Pane2D();
			TabItem item2d = new TabItem();
			item2d.Header = "Basic 2D";
			item2d.Content = pane2d;
			book.Items.Add(item2d);

			book.SelectionChanged += OnPageChanged;
		}

		TabControl book;

		void OnPageChanged(object sender, SelectionChangedEventArgs e)
		{
			//((book.SelectedItem as TabItem).Content as PaneBase).OnUpdated();
			Console.WriteLine("page changed");
		}


	}
}
