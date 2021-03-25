using Microsoft.Msagl.Drawing;
using System.Collections.Generic;

namespace HanyaPenggemar
{
    class DFS
    {
        //Stack yang menyimpan jalur penelusuran DFS
        public Stack<string> stackDFS { get; set; }

        //Dictionary untuk assign setiap nama akun dengan suatu int
        public Dictionary<string, int> accountAssignedInt { get; set; }
        
        //Representasi graf pertemanan
        public FriendshipGraph friendshipGraph { get; set; } 

        public DFS (Graph graph)
        {
            this.Graph = graph;
            this.stackDFS = new Stack<string>();
            this.accountAssignedInt = new Dictionary<string, int>();
            this.friendshipGraph = new FriendshipGraph(graph.NodeCount);
            this.LoadData();
        }

        public Graph Graph { get; set; }

        private void LoadData()
        {
            int accValue; //int yang di assign untuk setiap akun pada dictionary accountAssignedInt
            accValue = 0;
            foreach (var each in Graph.Edges)
            {
                var x = each.SourceNode.LabelText;
                var y = each.TargetNode.LabelText;
                //Menambahkan akun ke dictionary bila key nya belum ada
                if (!accountAssignedInt.ContainsKey(x))
                {
                    accountAssignedInt.Add(x, accValue);
                    accValue = accValue + 1;
                }
                if (!accountAssignedInt.ContainsKey(y))
                {
                    accountAssignedInt.Add(y, accValue);
                    accValue = accValue + 1;
                }
				//Menambahkan akun ke graf
                //THIS PART DEPENDENT ON FRIENDSHIP GRAPH INITIALIZATION
				friendshipGraph.addEdge(x, y, accountAssignedInt);
                friendshipGraph.addEdge(y, x, accountAssignedInt);
            }
        }

        private string ConvertIntToOrdinal(int i)
        {
            if (((i % 100 > 20) && (i % 10 == 1)) || ((i % 100 <= 20) && (i % 20 == 1)))
            {
                return i + "st-degree connection";
            }
            else if (((i % 100 > 20) && (i % 10 == 2)) || ((i % 100 <= 20) && (i % 20 == 2)))
            {
                return i + "nd-degree connection";
            }
            else if (((i % 100 > 20) && (i % 10 == 3)) || ((i % 100 <= 20) && (i % 20 == 3)))
            {
                return i + "rd-degree connection";
            }
            return i + "th-degree connection";
        }

        //Fungsi yang mengembalikan true jika semua akun telah visited
        //Mengembalikan false jika ada akun yang belum visited
        private bool allVisited (bool[] visited)
        {
            foreach(bool x in visited){
                if (x == false){
                    return false;
                }
            }
            return true;
        }

        //Fungsi yang mengembalikan true jika
        //semua friend dari suatu akun telah visited
        //Mengembalikan false jika ada friend dari
        //suatu akun yang belum visited
        private bool allFriendsVisited (string source, bool [] visited, Dictionary<string, int> accountAssignedInt)
        {
            foreach (var friend in friendshipGraph.graphLinkedList[accountAssignedInt[source]])
            {
                if (!visited[accountAssignedInt[friend]] == true)
                {
                    return false;
                }
            }
            return true;
        }

        //Fungsi DFS untuk mencari jalur dari akun source ke akun target
        private Stack<string> DFSforExploreFriends (string source, string target, bool [] visited, Dictionary<string, int> accountAssignedInt)
        {
            //Basis bila target telah ditemukan
            if (source == target)
            {
                stackDFS.Push(source);
                return stackDFS;
            }
            //Basis bila semua node telah visited
            //Mengembalikan stack kosong (untuk menandakan
            //akun source dan target tidak dapat terhubung)
            else if (allVisited(visited) == true)
            {
                Stack<string> emptyStack = new Stack<string>();
                return emptyStack;
            }
            //Rekurens
            else
            {
                visited[accountAssignedInt[source]] = true;
                //Mengecek apakah source memiliki friend
                if (friendshipGraph.graphLinkedList[accountAssignedInt[source]] != null)
                {
                    //Mengecek apakah semua friend dari source sudah visited atau belum
                    //Kasus belum semua friend dari source visited
                    if (allFriendsVisited(source, visited, accountAssignedInt) != true)
                    {
                        //Push source ke dalam stack jalur penulusuran DFS
                        stackDFS.Push(source);
                        foreach (var friend in friendshipGraph.graphLinkedList[accountAssignedInt[source]])
                        {
                            //Memanggil kembali fungsi DFSforExploreFriends 
                            //pada akun friend yang belum visited
                            if (!visited[accountAssignedInt[friend]] == true)
                            {
                                return DFSforExploreFriends(friend, target, visited, accountAssignedInt);
                            }
							else
							{
								continue;
							}
                        }
                    }
                    //Kasus semua friend dari source visited
                    //sehingga DFS melakukan backtracking
                    else
                    {
                        //Mengecek apakah stack kosong
                        //Jika iya, maka telah dilakukan backtracking
                        //sampai pada akun source yang paling pertama dipanggil (root)
                        //sehingga dikembalikan stack kosong sebagai tanda bahwa
                        //akun source pada pemanggilan paling pertama dan
                        //akun target tidak dapat terhubung
                        if (stackDFS.Count == 0)
                        {
                            Stack<string> emptyStack = new Stack<string>();
                            return emptyStack;
                        }
                        //Backtracking dilaksanakan
                        var backTrack = stackDFS.Pop();
                        return DFSforExploreFriends(backTrack, target, visited, accountAssignedInt);
                    }
                    
                }
                //Kasus akun source tidak memiliki friend
                //Sehingga melakukan backtracking
                else
                {
                    if (stackDFS.Count == 0)
                    {
                        Stack<string> emptyStack = new Stack<string>();
                        return emptyStack;
                    }
                    var backTrack = stackDFS.Pop();
                    return DFSforExploreFriends(backTrack, target, visited, accountAssignedInt);
                }
            }
            //Kasus semua yang di atas terlewati, sehingga
            //Mengembalikan stack kosong sebagai penanda
            //bahwa akun source dan target tidak dapat terhubung
            Stack<string> emptyStack_ = new Stack<string>();
            return emptyStack_;
        }

        public string ExploreFriend(string source, string target)
        {
            //visited merupakan array yang menandakan apakah suatu node telah divisit
            bool[] visited = new bool[friendshipGraph.graphLinkedList.Length + 1];
            //Stack yang menyimpan jalur DFS
            Stack<string> stackHasilExploreFriend = new Stack<string>();
            //Memanggil fungsi DFSforExploreFriends untuk
            stackHasilExploreFriend = DFSforExploreFriends(source, target, visited, accountAssignedInt);
            //Kasus stackHasilExploreFriend kosong, sehingga menandakan
            //akun source dan target tidak dapat terhubung
            if (stackHasilExploreFriend.Count == 0)
            {
                //barisPertama, barisKedua, dan barisKetiga mengikuti
                //spesifikasi yang diberikan ketika kedua akun source dan
                //target tidak dapat terhubung.
                //barisPertama untuk nama akun source dan target
                //barisKedua dan barisKetiga untuk keterangan kedua akun tidak dapat
                //terhubung dan harus memulai koneksi baru sendiri
                string barisPertama, barisKedua, barisKetiga, hasilAkhir;
                barisPertama = "Nama akun: " + source + " dan " + target + "\n";
                barisKedua = "Tidak ada jalur koneksi yang tersedia\n";
                barisKetiga = "Anda harus memulai koneksi baru itu sendiri.\n";
                //Penggabungan barisPertama, barisKedua, dan barisKetiga
                hasilAkhir = barisPertama + barisKedua + barisKetiga;
                return hasilAkhir;
            }
            //Kasus akun source dan target dapat terhubung
            else
            {
                //barisPertama untuk nama akun source dan target
                //barisKedua untuk degree connection
                //barisKetiga untuk jalur penelusuran DFS
                string barisPertama, barisKedua, barisKetiga, hasilAkhir;
				int pathDegree;
                barisPertama = "Nama akun: " + source + " dan " + target + "\n";
				barisKedua = "";
                barisKetiga = "";

                //Mengurus baris kedua, degree connection antara
                //akun source dan target
                barisKedua = ConvertIntToOrdinal(stackHasilExploreFriend.Count - 2) + "\n";

                //Mengurus baris ketiga, jalur penulusuran DFS
				pathDegree = stackHasilExploreFriend.Count;
                for(int i = 0; i < pathDegree; i++)
                {
                    string jalur = stackHasilExploreFriend.Pop();
                    barisKetiga = jalur + barisKetiga;
                    if (stackHasilExploreFriend.Count > 0)
                    {
                        barisKetiga = " → " + barisKetiga;
                    }
                }
                barisKetiga = barisKetiga + "\n";

                //Penggabungan barisPertama, barisKedua, dan barisKetiga
                hasilAkhir = barisPertama + barisKedua + barisKetiga;
                return hasilAkhir;
            }
        }
    }
}