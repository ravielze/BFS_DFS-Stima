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
        private int lastSelectedAccount = -1;

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
            ClearButton.Visibility = Visibility.Hidden;
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
        private void Clear(object sender, RoutedEventArgs e)
        {
            ResetAllTabState();
            Accounts.SelectedIndex = -1;
        }

        private void Minimize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void AlgoPickSelect(object sender, EventArgs e)
        {
            if (Accounts.SelectedIndex != -1 && ExploreFriendsAccount.SelectedIndex != -1)
            {
                this.Process();
            }
        }

        private void ResetAllTabState()
        {
            GraphControl.Graph = null;
            GraphControl.Graph = this.G;
            ClearButton.Visibility = Visibility.Hidden;
            ExploreFriends.Text = null;
            FriendRecommendation.Text = null;
            ExploreFriendsAccount.SelectedIndex = -1;
        }

        private void ExploreAccountSelect(object sender, EventArgs e)
        {
            if (ExploreFriendsAccount.IsDropDownOpen == false)
            {
                this.Process();
            } else
            {
                if (Accounts.SelectedIndex == -1)
                {
                    MessageBox.Show("Anda harus mengisi dropdown Account terlebih dahulu!", "Hanya Penggemar", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (ExploreFriendsAccount.Items.Count == 0)
                {
                    MessageBox.Show("Semua akun sudah saling berteman.", "Hanya Penggemar", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
        }

        private void AccountSelect(object sender, EventArgs e)
        {
            if (Accounts.IsDropDownOpen == false)
            {
                if (lastSelectedAccount != Accounts.SelectedIndex)
                {
                    ResetAllTabState();
                    lastSelectedAccount = Accounts.SelectedIndex;
                    if (Accounts.SelectedIndex != -1)
                    {
                        ExploreFriendsAccount.Items.Clear();
                        string account = Accounts.SelectedValue.ToString();
                        List<String> friends = new List<string>();
                        foreach (Edge edg in this.G.Edges)
                        {
                            if (edg.SourceNode.LabelText.Equals(account) && !edg.TargetNode.LabelText.Equals(account))
                            {
                                friends.Add(edg.TargetNode.LabelText);
                            } else if (edg.TargetNode.LabelText.Equals(account) && !edg.SourceNode.LabelText.Equals(account))
                            {
                                friends.Add(edg.SourceNode.LabelText);
                            }
                        }
                        foreach (Node n in this.G.Nodes)
                        {
                            if (!n.LabelText.Equals(account) && !friends.Contains(n.LabelText))
                            {
                                ExploreFriendsAccount.Items.Add(n.LabelText);
                            }
                        }
                    }
                    this.Process();
                }
            }
        }

        private Graph CopyGraph(Graph source)
        {
            Graph result = new Graph();
            foreach(Edge edg in source.Edges)
            {
                Edge redg = result.AddEdge(edg.SourceNode.LabelText, edg.TargetNode.LabelText);
                redg.Attr.ArrowheadAtTarget = ArrowStyle.None;
                redg.Attr.ArrowheadAtSource = ArrowStyle.None;
            }
            return result;
        }

        private void ColorizeFriendRecommendation(Graph source, string center, string data)
        {
            var arrData = data.Split("\n");
            Node centerNode = source.FindNode(center);
            centerNode.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;
            centerNode.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
            List<String> friends = new List<string>();
            foreach (Edge edg in source.Edges)
            {
                if (edg.SourceNode.LabelText.Equals(center) && !edg.TargetNode.LabelText.Equals(center))
                {
                    friends.Add(edg.TargetNode.LabelText);
                }
                else if (edg.TargetNode.LabelText.Equals(center) && !edg.SourceNode.LabelText.Equals(center))
                {
                    friends.Add(edg.SourceNode.LabelText);
                }
            }
            foreach(string friendData in friends)
            {
                Node friendNode = source.FindNode(friendData);
                friendNode.Attr.FillColor = Microsoft.Msagl.Drawing.Color.DarkOrange;
                friendNode.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
            }
            foreach (string eachData in arrData)
            {
                if (eachData.StartsWith("Nama akun: "))
                {
                    string friendRecommended = eachData.Replace("Nama akun: ", "");
                    Node friendRecommendedNode = source.FindNode(friendRecommended);
                    friendRecommendedNode.Attr.FillColor = Microsoft.Msagl.Drawing.Color.OrangeRed;
                    friendRecommendedNode.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                }
            }
        }

        private void ColorizeExploreFriend(Graph source, string data)
        {
            var arrData = data.Split("\n");
            if (data.Contains("degree connection")){
                var path = arrData[2];
                var pathData = path.Split(" → ");
                for(int i = 0; i < pathData.Length-1; i++)
                {
                    foreach(Edge edg in source.Edges)
                    {
                        if ((edg.SourceNode.LabelText.Equals(pathData[i])
                            && edg.TargetNode.LabelText.Equals(pathData[i+1]))
                            || (edg.SourceNode.LabelText.Equals(pathData[i + 1])
                            && edg.TargetNode.LabelText.Equals(pathData[i])))
                        {
                            edg.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                            edg.Attr.LineWidth += 1.65;
                        }
                    }
                }
            }
        }

        private void Process()
        {
            int algorithm = AlgorithmPicker.SelectedIndex;
            string account = Accounts.SelectedValue.ToString();
            string exploreaccount;
            ClearButton.Visibility = Visibility.Visible;

            BFS bfs = new BFS(this.G);
            DFS dfs = new DFS(this.G);
            Graph viewGraph = CopyGraph(this.G);
            // Menampilkan friend recommendation, methodnya nitip di class BFS heheh
            // mager mindahin ke class baru
            string friendRecommend = bfs.RecommendedFriend(account);
            FriendRecommendation.Text = friendRecommend;
            ColorizeFriendRecommendation(viewGraph, account, friendRecommend);

            if (ExploreFriendsAccount.SelectedIndex == -1)
            {
                if (ExploreFriendsAccount.Items.Count == 0 && Accounts.SelectedIndex != -1)
                {
                    ExploreFriends.Text = "Semua akun sudah saling berteman.";
                } else
                {
                    ExploreFriends.Text = "Dropdown di atas belum terisi.";
                }
            } else
            {
                ExploreFriends.Text = "";
                exploreaccount = ExploreFriendsAccount.SelectedValue.ToString();
                if (algorithm == 0)
                {
                    //Menampilkan explore friend dari account ke exploreaccount
                    //Menggunakan algoritma dfs
                    string exploreFriend = dfs.ExploreFriend(account, exploreaccount);
                    ExploreFriends.Text = exploreFriend;
                    ColorizeExploreFriend(viewGraph, exploreFriend);

                } else if (algorithm == 1)
                {
                    //Menampilkan explore friend dari account ke exploreaccount
                    //Menggunakan algoritma bfs
                    string exploreFriend = bfs.ExploreFriend(account, exploreaccount);
                    ExploreFriends.Text = exploreFriend;
                    ColorizeExploreFriend(viewGraph, exploreFriend);
                }
            }

            GraphControl.Graph = null;
            GraphControl.Graph = viewGraph;
        }
    }
}
