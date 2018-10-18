#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Syncfusion.Windows.Shared;
using Syncfusion.ThriftHive.Base;

namespace Asynchronous
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
 		    this.dataPager.PageCount = 2;
            
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Maximize(object sender, RoutedEventArgs e)
        {
            btnRestore.Visibility = Visibility.Visible;
            btnMaximize.Visibility = Visibility.Collapsed;
            this.WindowState = WindowState.Maximized;
        }
        private void Restore(object sender, RoutedEventArgs e)
        {
            btnRestore.Visibility = Visibility.Collapsed;
            btnMaximize.Visibility = Visibility.Visible;
            this.WindowState = WindowState.Normal;
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(this.WindowState==WindowState.Maximized)
            {
                btnRestore.Visibility = Visibility.Visible;
                btnMaximize.Visibility = Visibility.Collapsed;
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                btnRestore.Visibility = Visibility.Collapsed;
                btnMaximize.Visibility = Visibility.Visible;
                this.WindowState = WindowState.Normal;
            }
        }
    }  
}
