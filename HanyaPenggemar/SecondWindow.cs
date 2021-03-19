﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HanyaPenggemar
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        public SecondWindow()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            this.Owner.Top = this.Top;
            this.Owner.Left = this.Left;
        }
    }
}
