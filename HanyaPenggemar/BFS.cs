using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace HanyaPenggemar
{
    class BFS
    {
        // Set akun yang terdaftar
        public SortedSet<string> ListAkun { get; set; }

        // Queue pembacaan BFS
        public Queue<string> Antrian { get; set; }

        // List pembacaan BFS
        public List<string> Cek { get; set; }

        // Set akun-akun yang akan direkomendasikan (friend recommendation)
        public SortedSet<string> Recommendation { get; set; }

        // Set mutual friend yang ditemukan
        public SortedSet<string> Mutual { get; set; }

        // Dictionary untuk membaca string sebagai nama sebuah account
        public Dictionary<string, Account> Akun { get; set; }
        public BFS(Graph graph)
        {
            this.Graph = graph;
            this.ListAkun = new SortedSet<string>();
            this.Antrian = new Queue<string>();
            this.Cek = new List<string>();
            this.Recommendation = new SortedSet<string>();
            this.Mutual = new SortedSet<string>();
            this.Akun = new Dictionary<string, Account>(StringComparer.CurrentCulture);
            this.LoadData();
        }

        public Graph Graph { get; set; }

        private void LoadData()
        {
            foreach(var each in Graph.Edges)
            {
                var x = each.SourceNode.LabelText;
                var y = each.TargetNode.LabelText;
                if (!Akun.ContainsKey(x))
                {
                    ListAkun.Add(x);
                    Akun[x] = new Account();
                }
                if (!Akun.ContainsKey(y))
                {
                    Akun[x] = new Account();
                    ListAkun.Add(y);
                }
                Akun[x].AddFriend(y);
                Akun[y].AddFriend(x);
            }
        }

        public string ExploreFriend(string source, string target)
        {
            return "";
        }

        public string RecommendedFriend(string source, string target)
        {
            return "";
        }

    }
}
