using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace HanyaPenggemar
{
    class DecendingComparer<TKey> : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (x == y)
            {
                return y;
            }
            else
            {
                return y.CompareTo(x);
            }
        }
    }

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

        // Set Sorting banyaknya mutual
        public SortedList<int, string> TotalMutual { get; set; }

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
            this.Akun = new Dictionary<string, Account>(StringComparer.CurrentCultureIgnoreCase);
            this.TotalMutual = new SortedList<int, string>(new DecendingComparer<int>());
            this.LoadData();
        }

        public Graph Graph { get; set; }

        private void LoadData()
        {
            foreach (var each in Graph.Edges)
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
                    Akun[y] = new Account();
                    ListAkun.Add(y);
                }
                Akun[x].AddFriend(y);
                Akun[y].AddFriend(x);
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

        public string ExploreFriend(string source, string target)
        {
            // Kamus
            bool sudah;
            bool found = false;
            string jalur = "";
            string jalurNew;
            string hasil = "Nama akun: " + source + " dan " + target + "\n" + "Tidak ada jalur koneksi yang tersedia\n" + "Anda harus memulai koneksi baru itu sendiri.\n";

            // Menambahkan akun acuan pada queue BFS
            this.Antrian.Enqueue(source);

            // Menambahkan akun acuan pada Set pembacaan BFS
            this.Cek.Add(source);

            // Iterasi hingga ditemukan akun tujuan
            // atau queue BFS sudah kosong
            while ((!found) && (this.Antrian.Count > 0))
            {
                // Mengambil antrian untuk dilakukan pencarian explore
                jalur = this.Antrian.Dequeue();

                // Split jalur untuk mendapatkan nama akun
                // yang akan dilakukan pencarian explore
                var nama = jalur.Split(" → ");

                // Mencari akunTujuan dari pertemanan akun yang sedang dilakukan pencarian
                foreach (string tersangka in this.Akun[nama[^1]].GetFriend())
                {
                    // Tersangka merupakan akunTujuan
                    if (tersangka == target)
                    {
                        // Berhasil menemukan akun tujuan
                        found = true;

                        // Menambahkan data jalur menuju tersangka
                        jalur += " → " + tersangka;

                        // Memindahkan jalur yang didapatkan pada hasil explore friend
                        hasil = "Nama akun: " + source + " dan " + target + "\n";

                        // Menghentikan pencarian pertemanan akun yang sedang dilakukan pencarian
                        break;
                    }

                    // Tersangka bukan akunTujuan
                    else
                    {
                        // Inisiasi
                        sudah = false;

                        // Mengecek apakah tersangka sudah pernah ditemukan
                        // melalui set pembacaan BFS
                        foreach (string cekcek in this.Cek)
                        {
                            // Tersangka sudah terdaftar pada set cek pembacaan BFS
                            if (tersangka == cekcek)
                            {
                                sudah = true;
                            }
                        }

                        // Tersangka belum terdaftar pada set cek pembacaan BFS
                        if (!sudah)
                        {
                            // Menambahkan jalur menuju tersangka
                            jalurNew = jalur + " → " + tersangka;

                            // Memasukkan data jalur pada queue untuk dicek pada waktu yang akan datang
                            this.Antrian.Enqueue(jalurNew);

                            // Memasukkan data tersangka pada pembacaan BFS
                            this.Cek.Add(tersangka);
                        }
                    }
                }
            }

            // Pencarian ditemukan
            if (found)
            {
                // Menghitung banyaknya perpindahan akun yang dilakukan
                int banyak = jalur.Split(" → ").Length - 2;

                // Mengubah fotmat penulisan hasil
                hasil += ConvertIntToOrdinal(banyak) + "\n" + jalur;
            }
            // Menampilkan hasil explore
            return hasil;
        }

        public string RecommendedFriend(string source)
        {
            // Kamus
            bool lolos;
            string Recommended = "Tidak ada rekomendasi teman";

            // Mencari account yang akan direkomendasikan
            foreach (string friendAcuan in this.Akun[source].GetFriend())
            {
                // Mencari teman rekomendasi dari teman yang dimiliki akun acuan
                foreach (string kandidat in this.Akun[friendAcuan].GetFriend())
                {
                    // Inisiasi
                    lolos = true;

                    // Mengecek apakah akun yang direkomendasikan bukan
                    // akun yang dipilih atau teman dari akun yang dipilih
                    foreach (string temanAcuan in this.Akun[source].GetFriend())
                    {
                        if ((kandidat == source) || (kandidat == temanAcuan))
                        {
                            lolos = false;
                        }
                    }

                    if (lolos)
                    {
                        // Mengecek apakah akun yang direkomendasikan sudah
                        // pernah direkomendasikan sebelumnya
                        foreach (string rekomendasi in this.Recommendation)
                        {
                            if (kandidat == rekomendasi)
                            {
                                lolos = false;
                            }
                        }
                    }

                    if (lolos)
                    {
                        // Menambahkan kandidat pada Set akun yang akan direkomendasi
                        this.Recommendation.Add(kandidat);
                    }
                }
            }

            if (this.Recommendation.Count > 0)
            {
                Recommended = "Daftar rekomendasi teman untuk akun " + source + ":\n";
            }
            // Menampilkan rekomendasi pertemanan beserta hasil explore
            foreach (string rekomendasi in this.Recommendation)
            {
                int count = 0;

                // Melihat teman yang dimiliki oleh akun rekomendasi
                foreach (string temanRekomendasi in this.Akun[rekomendasi].GetFriend())
                {
                    // Mencari mutual friends
                    foreach (string temanAcuan in this.Akun[source].GetFriend())
                    {
                        // Menemukan mutual friends
                        if (temanRekomendasi == temanAcuan)
                        {
                            // Menambahkan mutual friends
                            count += 1;
                        }
                    }
                }
                this.TotalMutual.Add(count, rekomendasi);
            }

            for (int i = 0; i < this.TotalMutual.Count; i++)
            {
                string rekomendasi = this.TotalMutual.Values[i];

                // Melihat teman yang dimiliki oleh akun rekomendasi
                foreach (string temanRekomendasi in this.Akun[rekomendasi].GetFriend())
                {
                    // Mencari mutual friends
                    foreach (string temanAcuan in this.Akun[source].GetFriend())
                    {
                        // Menemukan mutual friends
                        if (temanRekomendasi == temanAcuan)
                        {
                            // Menambahkan mutual friends
                            this.Mutual.Add(temanRekomendasi);
                        }
                    }
                }

                // Jika memiliki mutual friends
                if (this.Mutual.Count != 0)
                {
                    Recommended += "Nama akun: " + rekomendasi;
                    // Menampilkan jumlah mutual friends
                    Recommended += "\n" + this.Mutual.Count + " mutual friend(s):\n";

                    // Menampilkan mutual friends
                    foreach (string nama in this.Mutual)
                    {
                        Recommended += nama + "\n";
                    }

                    // Memberikan space tampilan
                    Recommended += "\n";
                }

                // Jika tidak memiliki mutual friends
                else
                {
                    // Menampilkan keterangan pada layar
                    Recommended += source + " tidak terhubung dengan " + rekomendasi + "\n";
                    Recommended += "Buatlah koneksi baru untuk menghubungkan mereka\n";
                }
                // Membersihkan data set mutual friend
                this.Mutual.Clear();
            }

            this.TotalMutual.Clear();
            return Recommended;
        }

    }
}
