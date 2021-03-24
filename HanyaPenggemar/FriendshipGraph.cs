using System;
using System.Collections.Generic;
using System.Text;

namespace HanyaPenggemar
{
    class FriendshipGraph
    {
        //Representasi graf
        public LinkedList<string> [] graphLinkedList;

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
            }
        }





    }
}