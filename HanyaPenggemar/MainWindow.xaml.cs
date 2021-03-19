using AvaloniaGraphControl;
using Microsoft.Win32;
using System.IO;
using System.Windows;
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
        }

        public void Exit(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
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
                //FlowDocument document = new FlowDocument(paragraph);
                //Previewer.Document = document;
                Graph g = new Graph();
                g.Edges.Add(new Edge("A", "B"));
                g.Edges.Add(new Edge("A", "C"));
                g.Edges.Add(new Edge("C", "D"));
            }
        }

        public string[] GetReadedFileLines()
        {
            return lines;
        }

        private void Minimize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
