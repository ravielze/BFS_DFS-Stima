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
                ExploreFriendsAccount.Items.Add(n.LabelText);
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

        private void Process(object sender, RoutedEventArgs e)
        {
            if (Accounts.SelectedIndex == -1)
            {
                MessageBox.Show("Mohon untuk memilih account!", "Hanya Penggemar", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int algorithm = AlgorithmPicker.SelectedIndex;
            string account = Accounts.SelectedValue.ToString();
            string exploreaccount = "";
            if (ExploreFriendsAccount.SelectedIndex == -1)
            {
                ExploreFriends.Text = "Anda belum mengisi dropdown di atas.";
            } else
            {
                ExploreFriends.Text = "";
                exploreaccount = ExploreFriendsAccount.SelectedValue.ToString();
            }

            BFS bfs = new BFS(this.G);
            DFS dfs = new DFS(this.G);
            FriendRecommendation.Text = account + "\n" + exploreaccount;
            //Menampilkan friend recommendation
            FriendRecommendation.Text = bfs.RecommendedFriend(account);
            if (algorithm == 0)
            {
                //Menampilkan explore friend dari account ke exploreaccount
                //Menggunakan algoritma dfs

                // DFS dfs = new DFS(this.G);
                if (ExploreFriendsAccount.SelectedIndex != -1)
                    ExploreFriends.Text = dfs.ExploreFriend(account, exploreaccount);
                    //FriendRecommendation.Text = bfs.RecommendedFriend(account);
            } else if (algorithm == 1)
            {
                //Menampilkan explore friend dari account ke exploreaccount
                //Menggunakan algoritma bfs

                // BFS bfs = new BFS(this.G);
                if (ExploreFriendsAccount.SelectedIndex != -1)
                    ExploreFriends.Text = bfs.ExploreFriend(account, exploreaccount);
                    //FriendRecommendation.Text = bfs.RecommendedFriend(account);
            }
        }
    }
}
