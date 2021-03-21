using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private Graph g = null;
        private int selectedAlgorithm = -1;

        private Graph G { get => g; set => g = value; }
        private int SelectedAlgorithm { get => selectedAlgorithm; set => selectedAlgorithm = value; }

        public SecondWindow(Graph draw, int selectedAlgorithm)
        {
            this.G = draw;
            this.SelectedAlgorithm = selectedAlgorithm;
            InitializeComponent();
            Startup();
        }

        private void Startup()
        {
            GraphControl.Graph = this.G;
            AlgorithmPicker.SelectedIndex = this.SelectedAlgorithm;
            foreach(Node n in this.G.Nodes)
            {
                Accounts.Items.Add(n.LabelText);
            }
        }

        private void OnClose(object sender, CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            this.Owner.Top = this.Top;
            this.Owner.Left = this.Left;
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        private void PrevPage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
