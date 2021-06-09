using MovieDatabase.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public interface IFirebaseDatabase
    {
        Task<List<FirebaseItem>> GetItemsForUser(string owner);
        Task<FirebaseItem> CheckItem(string owner, string id);
        Task AddItem(FirebaseItem item);
        Task UpdateItem(FirebaseItem item);
        Task DeleteItem(string key);
    }
}
