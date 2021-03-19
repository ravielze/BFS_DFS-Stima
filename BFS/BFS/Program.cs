using System;
using System.Collections.Generic;

namespace Tugas_Besar_2
{
    class Account
    {
        public SortedSet<string> friends;

        // Constructor Account Baru
        public Account()
        {
            friends = new SortedSet<string>();
        }

        // Fungsi menampilkan semua teman
        public SortedSet<string> GetFriend()
        {
            return friends;
        }

        // Prosedur menambahkan y pada teman x
        public void AddFriend(string y)
        {
            friends.Add(y);
        }

        // MAIN PROGRAM
        static void Main(string[] args)
        {
            // KAMUS
            int N; // Banyaknya penambahan data teman
            string x, y; // x dan y account yang berteman
            string acuan; // Nama akun acuan (untuk explore)
            string akunTujuan; // Nama akun tujuan (untuk explore)
            bool lolos; // Menunjukkan bahwa akun rekomendasi lolos seleksi atau tidak
            bool found; // Menunjukkan pencarian akun tujuan sudah ditemukan atau belum
            string hasil; // Hasil pencarian explore
            string jalur; // Jalur pencarian explore dengan BFS
            bool sudah; // Menunjukkan akun sudah terdaftar pada set cek pembacaan BFS atau belum
            bool berteman; // Menunjukkan akun acuan berteman dengan akun tujuan

            // Set akun yang terdaftar
            SortedSet<string> ListAkun = new SortedSet<string>();

            // Queue pembacaan BFS
            Queue<string> antrian = new Queue<string>();

            // List pembacaan BFS
            List<string> cek = new List<string>();

            // Set akun-akun yang akan direkomendasikan (friend recommendation)
            SortedSet<string> recommendation = new SortedSet<string>();

            // Set mutual friend yang ditemukan
            SortedSet<string> mutual = new SortedSet<string>();

            // Dictionary untuk membaca string sebagai nama sebuah account
            Dictionary<string, Account> akun = new Dictionary<string, Account>(StringComparer.CurrentCultureIgnoreCase);

            // Menerima banyaknya pertemanan yang akan diinput
            Console.Write("Banyak pertemanan : ");
            N = Convert.ToInt32(Console.ReadLine());

            // Membaca penambahan teman sebanyak N kali
            for (int i = 0; i < N; i++)
            {
                // Memisahkan data kedua akun
                var baca = Console.ReadLine().Split(' ');
                x = baca[0];
                y = baca[1];

                // Account x sudah terdaftar
                if (akun.ContainsKey(x))
                {
                    // Account y sudah terdaftar
                    if (akun.ContainsKey(y))
                    {
                        // Menambahkan pertemanan x dan y
                        akun[x].AddFriend(y);
                        akun[y].AddFriend(x);
                    }
                    // Account y belum terdaftar
                    else
                    {
                        // Mendaftarkan akun y
                        akun[y] = new Account();
                        ListAkun.Add(y);
                        // Menambahkan pertemanan x dan y
                        akun[x].AddFriend(y);
                        akun[y].AddFriend(x);
                    }
                }
                // Account x belum terdaftar
                else
                {
                    // Account y sudah terdaftar
                    if (akun.ContainsKey(y))
                    {
                        // Mendaftarkan akun x
                        akun[x] = new Account();
                        ListAkun.Add(x);
                        // Menambahkan pertemanan x dan y
                        akun[x].AddFriend(y);
                        akun[y].AddFriend(x);
                    }
                    // Account y belum terdaftar
                    else
                    {
                        // Mendaftarkan akun x
                        akun[x] = new Account();
                        ListAkun.Add(x);
                        // Mendaftarkan akun y
                        akun[y] = new Account();
                        ListAkun.Add(y);
                        // Menambahkan pertemanan x dan y
                        akun[x].AddFriend(y);
                        akun[y].AddFriend(x);
                    }
                }
            }

            // Menampilkan teman setiap akun
            Console.WriteLine("\nPertemanan");
            foreach (string person in ListAkun)
            {
                // Menampilkan nama akun
                Console.Write(person + " :");
                foreach (string friend in akun[person].GetFriend())
                {
                    // Menampilkan teman-teman akun
                    Console.Write(" " + friend);
                }
                Console.WriteLine();
            }

            // Menampilkan akun-akun yang terdaftar
            Console.WriteLine("\nAkun yang terdaftar:");
            foreach (string list in ListAkun)
            {
                Console.Write(list + " ");
            }

            // Memilih akun acuan
            Console.Write("\nChoose Account     : ");
            acuan = Console.ReadLine();

            // Memilih akun tujuan
            Console.Write("Explore friends with : ");
            akunTujuan = Console.ReadLine();

            // Mencari account yang akan direkomendasikan
            foreach (string friendAcuan in akun[acuan].GetFriend())
            {
                // Mencari teman rekomendasi dari teman yang dimiliki akun acuan
                foreach (string kandidat in akun[friendAcuan].GetFriend())
                {
                    // Inisiasi
                    lolos = true;

                    // Mengecek apakah akun yang direkomendasikan bukan
                    // akun yang dipilih atau teman dari akun yang dipilih
                    foreach (string temanAcuan in akun[acuan].GetFriend())
                    {
                        if ((kandidat == acuan) || (kandidat == temanAcuan))
                        {
                            lolos = false;
                        }
                    }

                    if (lolos)
                    {
                        Console.WriteLine("Cek rekomen");
                        // Mengecek apakah akun yang direkomendasikan sudah
                        // pernah direkomendasikan sebelumnya
                        foreach (string rekomendasi in recommendation)
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
                        recommendation.Add(kandidat);
                    }
                }
            }

            // Menambahkan akun explore ke dalam set rekomendasi
            recommendation.Add(akunTujuan);

            // Inisiasi
            hasil = " (Explore Between " + acuan + " and " + akunTujuan + " Not Found)";

            // Menampilkan rekomendasi pertemanan beserta hasil explore
            Console.WriteLine("\nDaftar rekomendasi untuk akun " + acuan + " :");
            foreach (string rekomendasi in recommendation)
            {
                // Inisiasi
                found = false;

                // Menampilkan pada layar nama akun yang direkomendasikan
                Console.Write("Nama akun: " + rekomendasi);

                // Akun yang akan direkomendasikan merupakan akunTujuan explore friends
                if (rekomendasi == akunTujuan)
                {
                    // Menambahkan akun acuan pada queue BFS
                    antrian.Enqueue(acuan);

                    // Menambahkan akun acuan pada Set pembacaan BFS
                    cek.Add(acuan);

                    // Iterasi hingga ditemukan akun tujuan
                    // atau queue BFS sudah kosong
                    while ((!found) && (antrian.Count > 0))
                    {
                        // Pengecekan BFS
                        // Console.WriteLine();
                        // foreach (string jalur1 in antrian)
                        // {
                        //     Console.Write(jalur1 + ", ");
                        // }
                        // Console.WriteLine();
                        // foreach (string cek1 in cek)
                        // {
                        //     Console.Write(cek1 + ", ");
                        // }
                        // Console.WriteLine();

                        // Mengambil antrian untuk dilakukan pencarian explore
                        jalur = antrian.Dequeue();

                        // Split jalur untuk mendapatkan nama akun
                        // yang akan dilakukan pencarian explore
                        var nama = jalur.Split(" → ");

                        // Console.WriteLine("Akan mencari " + akunTujuan + " dari jalur " + jalur);

                        // Mencari akunTujuan dari pertemanan akun yang sedang dilakukan pencarian
                        foreach (string tersangka in akun[nama[nama.Length - 1]].GetFriend())
                        {
                            //Console.WriteLine("Mencari temannya " + nama[nama.Length - 1] + " menemukan " + tersangka);

                            // Tersangka merupakan akunTujuan
                            if (tersangka == akunTujuan)
                            {
                                // Berhasil menemukan akun tujuan
                                found = true;

                                // Menambahkan data jalur menuju tersangka
                                jalur += " → " + tersangka;

                                // Memindahkan jalur yang didapatkan pada hasil explore friend
                                hasil = jalur;

                                // Memasukkan data jalur ke dalam queue BFS
                                antrian.Enqueue(jalur);

                                // Memasukkan akun tersangka pada set pembacaan BFS
                                cek.Add(tersangka);

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
                                foreach (string cekcek in cek)
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
                                    antrian.Enqueue(jalur);

                                    // Memasukkan data tersangka pada pembacaan BFS
                                    cek.Add(tersangka);
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
                    Console.WriteLine(hasil);
                }

                // Melihat teman yang dimiliki oleh akun rekomendasi
                foreach (string temanRekomendasi in akun[rekomendasi].GetFriend())
                {
                    // Mencari mutual friends
                    foreach (string temanAcuan in akun[acuan].GetFriend())
                    {
                        // Menemukan mutual friends
                        if (temanRekomendasi == temanAcuan)
                        {
                            // Menambahkan mutual friends
                            mutual.Add(temanRekomendasi);
                        }
                    }
                }

                // Jika memiliki mutual friends
                if (mutual.Count != 0)
                {
                    // Menampilkan jumlah mutual friends
                    Console.Write("\n" + mutual.Count + " mutual friend(s): ");

                    // Menampilkan mutual friends
                    foreach (string nama in mutual)
                    {
                        Console.Write(nama + " ");
                    }

                    // Memberikan space tampilan
                    Console.WriteLine();
                    Console.WriteLine();
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
                        foreach (string friend in akun[acuan].GetFriend())
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
                            Console.WriteLine(acuan + " berteman dengan " + rekomendasi + "\n");
                        }

                        // Jika akun rekomendasi dengan akun acuan tidak berteman
                        else
                        {
                            // Menampilkan pada layar akun acuan tidak memiliki
                            // mutual friend dengan akun rekomendasi
                            Console.WriteLine(acuan + " tidak memiliki mutual friend dengan " + rekomendasi + "\n");
                        }
                    }

                    // Jalur explore tidak ditemukan
                    else
                    {
                        // Menampilkan keterangan pada layar
                        Console.WriteLine(acuan + " tidak terhubung dengan " + rekomendasi);
                        Console.WriteLine("Buatlah koneksi baru untuk menghubungkan mereka\n");
                    }
                }
                // Membersihkan data set mutual friend
                mutual.Clear();
            }
        }
    }
}
