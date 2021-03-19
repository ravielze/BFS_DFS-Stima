using Microsoft.Msagl.Drawing;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace HanyaPenggemar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] lines;
        public MainWindow()
        {
            InitializeComponent();
            Startup();
        }

        private void Startup()
        {
            ChangeSecondTabVisibility(Visibility.Hidden);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void ChangeSecondTabVisibility(Visibility v)
        {
            graphControl.Visibility = v;
            NextButton.Visibility = v;
            ClearButton.Visibility = v;
            AlgorithmPicker.Visibility = v;
        }

        private void ClearFirstTab()
        {
            this.lines = null;
            Previewer.Document = null;
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Text files (*.txt)|*.txt",
                InitialDirectory = Directory.GetCurrentDirectory()
            };
            if (openFileDialog.ShowDialog() == true)
            {
                lines = System.IO.File.ReadAllLines(openFileDialog.FileName);
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(System.IO.File.ReadAllText(openFileDialog.FileName));
                FlowDocument document = new FlowDocument(paragraph);
                Previewer.Document = document;

                AlgorithmPicker.SelectedIndex = -1;
                graphControl.Graph = null;
                graphControl.Graph = Draw();
                if (graphControl.Graph == null)
                {
                    ClearFirstTab();
                    ChangeSecondTabVisibility(Visibility.Hidden);
                    MessageBox.Show("Format file tidak valid.", "Hanya Penggemar", MessageBoxButton.OK, MessageBoxImage.Error);
                } else
                {
                    ChangeSecondTabVisibility(Visibility.Visible);
                }
            }
        }

        private Graph Draw()
        {
            if (this.lines == null) return null;
            if (this.lines.Length < 1) return null;

            Graph result = new Graph();
            foreach (string each in lines)
            {
                string[] eachsplit = each.Split(" ", 2);
                if (eachsplit.Length == 2)
                {
                    result.AddEdge(eachsplit[0], eachsplit[1]);
                } else
                {
                    ChangeSecondTabVisibility(Visibility.Hidden);
                    return null;
                }
            }
            return result;
        }

        public string[] GetReadedFileLines()
        {
            return lines;
        }

        private void Minimize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Clear(object sender, RoutedEventArgs e)
        {
            ClearFirstTab();
            AlgorithmPicker.SelectedIndex = -1;
            ChangeSecondTabVisibility(Visibility.Hidden);
        }

        private void NextPage(object sender, RoutedEventArgs e)
        {
            if (AlgorithmPicker.SelectedIndex == -1)
            {
                MessageBox.Show("Mohon untuk memilih algoritma terlebih dahulu!", "Hanya Penggemar", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ComboBoxItem val = (ComboBoxItem) AlgorithmPicker.SelectedValue;
            Debug.WriteLine(val.Content);
            this.Visibility = Visibility.Hidden;
            SecondWindow sw = new SecondWindow
            {
                Owner = this
            };
            sw.Top = this.Top;
            sw.Left = this.Left;
            sw.ShowDialog();
        }
    }
}
