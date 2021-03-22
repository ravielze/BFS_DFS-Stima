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

        public string ExploreFriend(string source, string target, bool found, string hasil)
        {       
            // Kamus
            bool sudah;
            
            // Menambahkan akun acuan pada queue BFS
            this.Antrian.Enqueue(source);

            // Menambahkan akun acuan pada Set pembacaan BFS
            this.Cek.Add(source);

            // Iterasi hingga ditemukan akun tujuan
            // atau queue BFS sudah kosong
            while ((!found) && (this.Antrian.Count > 0))
            {
                // Mengambil antrian untuk dilakukan pencarian explore
                string jalur = this.Antrian.Dequeue();

                // Split jalur untuk mendapatkan nama akun
                // yang akan dilakukan pencarian explore
                var nama = jalur.Split(" → ");

                // Mencari akunTujuan dari pertemanan akun yang sedang dilakukan pencarian
                foreach (string tersangka in akun[nama[nama.Length - 1]].GetFriend())
                {
                    // Tersangka merupakan akunTujuan
                    if (tersangka == target)
                    {
                        // Berhasil menemukan akun tujuan
                        found = true;

                        // Menambahkan data jalur menuju tersangka
                        jalur += " → " + tersangka;

                        // Memindahkan jalur yang didapatkan pada hasil explore friend
                        hasil = jalur;

                        // Memasukkan data jalur ke dalam queue BFS
                        this.Antrian.Enqueue(jalur);

                        // Memasukkan akun tersangka pada set pembacaan BFS
                        this.Cek.Add(tersangka);

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
                            // Console.WriteLine("Akan memasukkan " + tersangka + " ke dalam queue");

                            // Menambahkan jalur menuju tersangka
                            jalur += " → " + tersangka;

                            // Memasukkan data jalur pada queue untuk dicek pada waktu yang akan datang
                            this.Antrian.Enqueue(jalur);

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
                var banyak = hasil.Split(" → ");

                // Mengubah fotmat penulisan hasil
                hasil = " (" + hasil + ", ";

                if ((banyak.Length - 2) % 100 > 20)
                {
                    // Jika terdapat 1 (mod 10) kali perpindahan akun
                    if ((banyak.Length - 2) % 10 == 1)
                    {
                        // Menambahkan keterangan Nst degree pada hasil
                        hasil += (banyak.Length - 2) + "st Degree)";
                    }
                    // Jika terdapat 2 (mod 10) kali perpindahan akun
                    else if ((banyak.Length - 2) % 10 == 2)
                    {
                        // Menambahkan keterangan Nnd degree pada hasil
                        hasil += (banyak.Length - 2) + "nd Degree)";
                    }
                    // Jika terdapat 3 (mod 10) kali perpindahan akun
                    else if ((banyak.Length - 2) % 10 == 3)
                    {
                        // Menambahkan keterangan Nrd degree pada hasil
                        hasil += (banyak.Length - 2) + "rd Degree)";
                    }
                    // sisa
                    else
                    {
                        // Menambahkan keterangan Nth degree pada hasil
                        hasil += (banyak.Length - 2) + "th Degree)";
                    }
                }
                else
                {
                    // Jika terdapat 1 (mod 20) kali perpindahan akun
                    if ((banyak.Length - 2) % 20 == 1)
                    {
                        // Menambahkan keterangan Nst degree pada hasil
                        hasil += (banyak.Length - 2) + "st Degree)";
                    }
                    // Jika terdapat 2 (mod 20) kali perpindahan akun
                    else if ((banyak.Length - 2) % 20 == 2)
                    {
                        // Menambahkan keterangan Nnd degree pada hasil
                        hasil += (banyak.Length - 2) + "nd Degree)";
                    }
                    // Jika terdapat 3 (mod 20) kali perpindahan akun
                    else if ((banyak.Length - 2) % 20 == 3)
                    {
                        // Menambahkan keterangan Nrd degree pada hasil
                        hasil += (banyak.Length - 2) + "rd Degree)";
                    }
                    // sisa
                    else
                    {
                        // Menambahkan keterangan Nth degree pada hasil
                        hasil += (banyak.Length - 2) + "th Degree)";
                    }
                }
            }
            // Menampilkan hasil explore
            return hasil;
            }
        }

        public string RecommendedFriend(string source, string target)
        {
            // Kamus
            bool lolos;
            bool found;
            string hasil;
            bool berteman;
            string Recommended;

            // Mencari account yang akan direkomendasikan
            foreach (string friendAcuan in akun[source].GetFriend())
            {
                // Mencari teman rekomendasi dari teman yang dimiliki akun acuan
                foreach (string kandidat in akun[friendAcuan].GetFriend())
                {
                    // Inisiasi
                    lolos = true;

                    // Mengecek apakah akun yang direkomendasikan bukan
                    // akun yang dipilih atau teman dari akun yang dipilih
                    foreach (string temanAcuan in akun[source].GetFriend())
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

            // Menambahkan akun explore ke dalam set rekomendasi
            this.Recommendation.Add(target);

            // Inisiasi
            hasil = " (Explore Between " + source + " and " + target + " Not Found)";

            // Menampilkan rekomendasi pertemanan beserta hasil explore
            Console.WriteLine("\nDaftar rekomendasi untuk akun " + source + " :");
            foreach (string rekomendasi in this.Recommendation)
            {
                // Inisiasi
                found = false;

                // Menampilkan pada layar nama akun yang direkomendasikan
                Console.Write("Nama akun: " + rekomendasi);                
               
                // Akun yang akan direkomendasikan merupakan akunTujuan explore friends
                if (rekomendasi == target)
                {
                    hasil = this.ExploreFriend(source, target, found, hasil)
                }

                // Melihat teman yang dimiliki oleh akun rekomendasi
                foreach (string temanRekomendasi in akun[rekomendasi].GetFriend())
                {
                    // Mencari mutual friends
                    foreach (string temanAcuan in akun[source].GetFriend())
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
                    // Menampilkan jumlah mutual friends
                    Recommended = "\n" + this.Mutual.Count + " mutual friend(s): ";

                    // Menampilkan mutual friends
                    foreach (string nama in this.Mutual)
                    {
                        Recommended += nama + " ";
                    }

                    // Memberikan space tampilan
                    Recommended += "\n\n";
                }

                // Jika tidak memiliki mutual friends
                else
                {
                    // Jika jalur explore ditemukan
                    if (found)
                    {
                        // Insiasi
                        berteman = false;

                        // Mengecek apakah akunRekomendasi adalah teman dari akun acuan
                        foreach (string friend in akun[source].GetFriend())
                        {
                            // Akun rekomendasi berteman dengan akun acuan
                            if (friend == rekomendasi)
                            {
                                berteman = true;
                            }
                        }

                        // Jika akun rekomendasi dengan akun acuan berteman
                        if (berteman)
                        {
                            // Menampilkan pada layar bahwa akun rekomnedasi
                            // dengan akun acuan sudah berteman
                            Recommended += source + " berteman dengan " + rekomendasi + "\n\n";
                        }

                        // Jika akun rekomendasi dengan akun acuan tidak berteman
                        else
                        {
                            // Menampilkan pada layar akun acuan tidak memiliki
                            // mutual friend dengan akun rekomendasi
                            Recommended = source + " tidak memiliki mutual friend dengan " + rekomendasi + "\n\n");
                        }
                    }

                    // Jalur explore tidak ditemukan
                    else
                    {
                        // Menampilkan keterangan pada layar
                        Recommended += source + " tidak terhubung dengan " + rekomendasi + "\n";
                        Recommended += "Buatlah koneksi baru untuk menghubungkan mereka\n");
                    }
                }
                // Membersihkan data set mutual friend
                this.Mutual.Clear();
            }
            return Recommended;
        }

    }
}
