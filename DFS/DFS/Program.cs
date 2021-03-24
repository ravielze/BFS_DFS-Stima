using System;
using System.Collections.Generic;

namespace DFS
{
    public class FriendshipGraph
    {
        //Representasi graf
        LinkedList<string> [] graphLinkedList;

        //Stack yang menyimpan jalur penelusuran DFS
        Stack<string> stackDFS = new Stack<string>();

		//Default Constructor graf
		public FriendshipGraph()
		{
			graphLinkedList = new LinkedList<string>[10];
		}

        //User-defined constructor graf
        public FriendshipGraph(int N)
        {
            graphLinkedList = new LinkedList<string>[N];
        }

        //Menambahkan busur ke graf yang menghubungkan a dan b
        public void addEdge (string a, string b, Dictionary<string, int> accountAssignedInt)
        {
            //Kasus node a belum ada sebelumnya
            if (graphLinkedList[accountAssignedInt[a]] == null)
            {
                graphLinkedList[accountAssignedInt[a]] = new LinkedList<string>();
                graphLinkedList[accountAssignedInt[a]].AddFirst(b);
            }
            //Kasus node a sudah ada sebelumnya
            else
            {
                //node b akan ditambahkan pada linkedlist
                //sedemikian rupa sehingga linkedlist tetap terurut
                //secara abjad

                //Previous menyimpan node sebelumnya dari graphLinkedList
                //Current menyimpan node saat ini dari graphLinkedList
				string previous = "";
				string current = "";
                foreach (var curr in graphLinkedList[accountAssignedInt[a]])
                {
					current = curr;
                    //Mengecek apakah abjad b masih lebih dibelakang abjad curr
                    if (string.Compare(b, curr) > 0)
                    {
                        previous = curr;
                        continue;
                    }
                    //Jika abjad b berada di depan abjad curr, hentikan iterasi
                    else
                    {
						break;
                    }
                }
                //Kasus abjad b merupakan abjad paling awal di graphLinkedList
                //Ditandai dengan isi previous yang tidak berubah (tetap "")
				if (previous == "")
				{
					graphLinkedList[accountAssignedInt[a]].AddFirst(b);
				}
                //Kasus abjad b bukan merupakan abjad paling awal di graphLinkedList
				else
				{
                    //currentNode sebagai node yang mengandung abjad sebelum abjad b
					LinkedListNode<string> currentNode = graphLinkedList[accountAssignedInt[a]].Find(current);
                    //Menambahkan abjad b setelah currentNode
					graphLinkedList[accountAssignedInt[a]].AddAfter(currentNode, b);
				}
                // graphLinkedList[accountAssignedInt[a]].AddLast(b);
                // foreach (var curr in graphLinkedList[accountAssignedInt[a]])
                // {
                //     Console.Write("friend of " + a + ": \n");
                //     Console.Write(curr + "\n");
                // }
            }
        }

		//Fungsi yang mengembalikan true jika semua akun telah visited
        //Mengembalikan false jika ada akun yang belum visited
        public bool allVisited (bool[] visited, Dictionary<string, int> accountAssignedInt)
        {
            for (int i = 0; i < accountAssignedInt.Count; i ++)
			{
				if (!visited[i] == true)
				{
					return false;
				}
			}
			return true;

            // bool allAreVisited = true;
            // if (graphLinkedList[accountAssignedInt[source]] != null)
            // {
            //     foreach (var friend in graphLinkedList[accountAssignedInt[source]])
            //     {
            //         if (!visited[friend] == true)
            //         {
            //             allAreVisited = false;
            //             return allAreVisited;
            //         }
            //         else
            //         {
            //             continue;
            //         }
            //     }
            //     foreach (var friend in graphLinkedList[accountAssignedInt[source]])
            //     {
            //         return allAreVisited & allVisited(friend, visited, accountAssignedInt);
            //     }
            // }
            // return allAreVisited;
            
            // foreach (var friend in graphLinkedList[accountAssignedInt[source]])
            // {
            //     allAreVisited = allAreVisited & allFriendsVisited(friend, visited, accountAssignedInt);
            // }
            // return allAreVisited;

            //Jika semua friend dari akun source telah visited, rekursi
            //fungsi allVisited ke friend dari akun source
        }

        //Fungsi yang mengembalikan true jika
        //semua friend dari suatu akun telah visited
        //Mengembalikan false jika ada friend dari
        //suatu akun yang belum visited
        public bool allFriendsVisited (string source, bool [] visited, Dictionary<string, int> accountAssignedInt)
        {
            foreach (var friend in graphLinkedList[accountAssignedInt[source]])
            {
                if (!visited[accountAssignedInt[friend]] == true)
                {
                    return false;
                }
            }
            return true;
        }

        //Fungsi DFS untuk mencari jalur dari akun source ke akun target
        public Stack<string> DFSforExploreFriends (string source, string target, bool [] visited, Dictionary<string, int> accountAssignedInt)
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
            else if(allVisited(visited, accountAssignedInt) == true)
            {
                Stack<string> emptyStack = new Stack<string>();
                return emptyStack;
            }
            //Rekurens
            else
            {
                visited[accountAssignedInt[source]] = true;
                //Mengecek apakah source memiliki friend
                if (graphLinkedList[accountAssignedInt[source]] != null)
                {
                    //Mengecek apakah semua friend dari source sudah visited atau belum
                    //Kasus belum semua friend dari source visited
                    if (allFriendsVisited(source, visited, accountAssignedInt) != true)
                    {
                        //Push source ke dalam stack jalur penulusuran DFS
                        stackDFS.Push(source);
                        foreach (var friend in graphLinkedList[accountAssignedInt[source]])
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


            // Stack<string> stackDFS = new Stack<string>();
            // foreach (var friend in graphLinkedList[source])
            // {
            //     stackDFS.Push(friend);
            // }
            // visited[source] = true;

            // while (stackDFS.Count > 0)
            // {
            //     if (!visited[stackDFS.Peek()])
            //     {
            //         var person = stackDFS.Pop();
            //         visited[person] = true;
            //         if (graphLinkedList[person] != null)
            //         {
            //             foreach (var friend in graphLinkedList[person])
            //             {
            //                 stackDFS.Push(friend);
            //             }
            //         }
            //     }
            // }
        }

        public string ExploreFriend(string source, string target, Dictionary<string, int> accountAssignedInt)
        {
            //visited merupakan array yang menandakan apakah suatu node telah divisit
            bool[] visited = new bool[graphLinkedList.Length + 1];
            //Stack yang menyimpan jalur DFS
            Stack<string> stackHasilExploreFriend = new Stack<string>();
            //Memanggil fungsi DFSforExploreFriends untuk
            stackHasilExploreFriend = DFSforExploreFriends(source, target, visited, accountAssignedInt);
            //Kasus stackHasilExploreFriend kosong, sehingga menandakan
            //akun source dan target tidak dapat terhubung
            if (stackHasilExploreFriend.Count == 0)
            {
                return "Kedua akun tidak dapat terhubung";
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
                if ((stackHasilExploreFriend.Count - 2) % 100 > 20)
                {
                    // Jika terdapat 1 (mod 10) kali perpindahan akun
                    if ((stackHasilExploreFriend.Count - 2) % 10 == 1)
                    {
                        // Menambahkan keterangan Nst degree pada hasil
                        barisKedua += (stackHasilExploreFriend.Count - 2) + "st";
                    }
                    // Jika terdapat 2 (mod 10) kali perpindahan akun
                    else if ((stackHasilExploreFriend.Count - 2) % 10 == 2)
                    {
                        // Menambahkan keterangan Nnd degree pada hasil
                        barisKedua += (stackHasilExploreFriend.Count - 2) + "nd";
                    }
                    // Jika terdapat 3 (mod 10) kali perpindahan akun
                    else if ((stackHasilExploreFriend.Count - 2) % 10 == 3)
                    {
                        // Menambahkan keterangan Nrd degree pada hasil
                        barisKedua += (stackHasilExploreFriend.Count - 2) + "rd";
                    }
                    // sisa
                    else
                    {
                        // Menambahkan keterangan Nth degree pada hasil
                        barisKedua += (stackHasilExploreFriend.Count - 2) + "th";
                    }
                }
                else
                {
                    // Jika terdapat 1 (mod 20) kali perpindahan akun
                    if ((stackHasilExploreFriend.Count - 2) % 20 == 1)
                    {
                        // Menambahkan keterangan Nst degree pada hasil
                        barisKedua += (stackHasilExploreFriend.Count - 2) + "st";
                    }
                    // Jika terdapat 2 (mod 20) kali perpindahan akun
                    else if ((stackHasilExploreFriend.Count - 2) % 20 == 2)
                    {
                        // Menambahkan keterangan Nnd degree pada hasil
                        barisKedua += (stackHasilExploreFriend.Count - 2) + "nd";
                    }
                    // Jika terdapat 3 (mod 20) kali perpindahan akun
                    else if ((stackHasilExploreFriend.Count - 2) % 20 == 3)
                    {
                        // Menambahkan keterangan Nrd degree pada hasil
                        barisKedua += (stackHasilExploreFriend.Count - 2) + "rd";
                    }
                    // sisa
                    else
                    {
                        // Menambahkan keterangan Nth degree pada hasil
                        barisKedua += (stackHasilExploreFriend.Count - 2) + "th";
                    }
                }
                barisKedua = barisKedua + " degree connection \n";

                //Mengurus baris ketiga, jalur penulusuran DFS
				pathDegree = stackHasilExploreFriend.Count;
                for(int i = 0; i < pathDegree; i++)
                {
                    string jalur = stackHasilExploreFriend.Pop();
                    barisKetiga = jalur + barisKetiga;
                    if (stackHasilExploreFriend.Count > 0)
                    {
                        barisKetiga = " -> " + barisKetiga;
                    }
                }
                barisKetiga = barisKetiga + "\n";

                //Penggabungan barisPertama, barisKedua, dan barisKetiga
                hasilAkhir = barisPertama + barisKedua + barisKetiga;
                return hasilAkhir;
            }
        }

        public void Main()
        {
            int N;  //Banyak baris input pertemanan
            int accValue; //int yang di assign untuk setiap akun pada dictionary accountAssignedInt
            string akun1, akun2; //hasil split(spasi) dari baris input pertemanan
            string source, target; //source dan target merupakan akun yang akan dicari jalur pertemanannya
            string hasilExploreFriend; //penyimpan hasil penggunaan fitur explore friend
            Console.Write("Banyak pertemanan : ");
            N = Convert.ToInt32(Console.ReadLine());
            FriendshipGraph graph = new FriendshipGraph(2 * N);
            //Dictionary untuk assign setiap nama akun dengan suatu int
            Dictionary<string, int> accountAssignedInt = new Dictionary<string, int>();
            //Value int yang di assign pada dictionary
            accValue = 0;
            for (int i = 0; i< N; i++)
            {
				//Membaca input pertemanan
                var baca = Console.ReadLine().Split(' ');
                akun1 = baca[0];
                akun2 = baca[1];
				//Menambahkan akun ke dictionary bila key nya belum ada
                if (!accountAssignedInt.ContainsKey(akun1))
                {
                    accountAssignedInt.Add(akun1, accValue);
                    accValue = accValue + 1;
                }
                if (!accountAssignedInt.ContainsKey(akun2))
                {
                    accountAssignedInt.Add(akun2, accValue);
                    accValue = accValue + 1;
                }
				//Menambahkan akun ke graf
				graph.addEdge(akun1, akun2, accountAssignedInt);
                graph.addEdge(akun2, akun1, accountAssignedInt);
            }

            // Memilih akun acuan
            Console.Write("Choose Account     : ");
            source = Console.ReadLine();

            // Memilih akun tujuan
            Console.Write("Explore friends with : ");
            target = Console.ReadLine();
            
            //Memanggil DFS untuk menggunakan fitur explore friend
            hasilExploreFriend = graph.ExploreFriend(source, target, accountAssignedInt);
            Console.Write(hasilExploreFriend);
        }

    }

}