﻿using FabLab.DeviceManagement.DesktopApplication.Views.Project;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FabLab.DeviceManagement.DesktopApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }

       
       

        //private void TabItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (Tag1.Visibility == Visibility.Collapsed)
        //    {
        //        Tag1.Visibility = Visibility.Visible;
        //        Tag2.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        Tag1.Visibility = Visibility.Collapsed;
        //        Tag2.Visibility = Visibility.Collapsed;
        //    }
        //}
    }
}
