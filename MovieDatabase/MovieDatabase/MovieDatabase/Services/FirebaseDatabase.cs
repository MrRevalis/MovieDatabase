using Firebase.Database;
using Firebase.Database.Query;
using MovieDatabase.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class FirebaseDatabase : IFirebaseDatabase
    {
        private FirebaseClient client;
        public FirebaseDatabase()
        {
            client = new FirebaseClient("https://moviedatabase-5e229-default-rtdb.firebaseio.com/");
        }

        public async Task AddItem(FirebaseItem item)
        {
            await client.Child("Database").PostAsync(JsonConvert.SerializeObject(item));
        }

        public async Task DeleteItem(string key)
        {
            await client.Child("Database").Child(key).DeleteAsync();
        }

        public async Task<FirebaseItem> CheckItem(string owner, string id)
        {
            var getItem = await client.Child("Database").OnceAsync<FirebaseItem>();
            if (getItem != null)
            {
                var item = getItem.FirstOrDefault(x => x.Object.Owner == owner && x.Object.ID == id);
                if (item != null)
                {
                    return item.Object;
                }
                else
                {
                    return new FirebaseItem();
                }
            }
            return new FirebaseItem();
        }

        public async Task<List<FirebaseItem>> GetItemsForUser(string owner)
        {
            var getItems = await client.Child("Database").OnceAsync<FirebaseItem>();
            if (getItems != null)
            {
                var items = getItems.Where(x => x.Object.Owner == owner).Select(y => y.Object).ToList();
                return items;
            }
            return new List<FirebaseItem>();
        }

        public async Task UpdateItem(FirebaseItem item)
        {
            var itemToUpdate = await client.Child("Database").OnceAsync<FirebaseItem>();
            if (itemToUpdate != null)
            {
                var specificItem = itemToUpdate.FirstOrDefault(x => x.Object.Owner == item.Owner && x.Object.ID == item.ID);
                if (specificItem != null)
                {
                    if (item.Watched == false && item.ToWatch == false)
                    {
                        await DeleteItem(specificItem.Key);
                    }
                    else
                    {
                        await client.Child("Database").Child(specificItem.Key).PutAsync(JsonConvert.SerializeObject(item));
                    }

                }
                else
                {
                    await AddItem(item);
                }
            }

        }
    }
}
