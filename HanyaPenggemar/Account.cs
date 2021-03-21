using System;
using System.Collections.Generic;
using System.Text;

namespace HanyaPenggemar
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
    }
}
