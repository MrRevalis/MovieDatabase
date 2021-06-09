using MovieDatabase.Models;
using MovieDatabase.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MovieDatabase.ViewModels
{
    public class BrowseViewModel : BaseViewModel
    {
        private IMovieDB movieDB;
        private IFirebaseDatabase firebaseDatabase;

        private string username;

        public ICommand AppearingCommand { get; private set; }
        public ICommand SignOutCommand { get; private set; }
        public ICommand ChangePageCommand { get; private set; }
        public ICommand AddRealisedCommand { get; private set; }
        public ICommand AddFavouriteCommand { get; private set; }

        private ObservableRangeCollection<BrowseItem> favouriteItems;
        public ObservableRangeCollection<BrowseItem> FavouriteItems
        {
            get => favouriteItems;
            set => SetProperty(ref favouriteItems, value);
        }

        private ObservableRangeCollection<BrowseItem> realisedItems;
        public ObservableRangeCollection<BrowseItem> RealisedItems
        {
            get => realisedItems;
            set => SetProperty(ref realisedItems, value);
        }


        public BrowseViewModel()
        {
            movieDB = DependencyService.Get<IMovieDB>();
            firebaseDatabase = DependencyService.Get<IFirebaseDatabase>();

            SignOutCommand = new Command(async () => await SignOut());
            AppearingCommand = new Command(async () => await OnAppearing());
            ChangePageCommand = new Command<BrowseItem>(async (sender) => await ChangePage(sender));
            AddRealisedCommand = new Command<BrowseItem>(async (sender) => await AddRealisedItem(sender));
            AddFavouriteCommand = new Command<BrowseItem>(async (sender) => await AddToFavouriteItem(sender));

            FavouriteItems = new ObservableRangeCollection<BrowseItem>();
            RealisedItems = new ObservableRangeCollection<BrowseItem>();

            username = FirebaseAuth.GetUserName();
        }

        private async Task AddToFavouriteItem(BrowseItem item)
        {
            int index = FavouriteItems.IndexOf(item);
            int realisedIndex = RealisedItems.IndexOf(item);
            item.ToWatch = !item.ToWatch;
            if (item.ToWatch == true)
            {
                item.Watched = false;
                RealisedItems.RemoveAt(realisedIndex);
                FavouriteItems.Add(item);
            }
            else if (item.ToWatch == false)
            {
                if (index >= 0)
                {
                    FavouriteItems.RemoveAt(index);
                }
            }

            FirebaseItem firebaseItem = new FirebaseItem() { ID = item.ID, Owner = username, Watched = item.Watched, ToWatch = item.ToWatch, Type = item.Type };
            await firebaseDatabase.UpdateItem(firebaseItem);
        }

        private async Task AddRealisedItem(BrowseItem item)
        {
            int index = RealisedItems.IndexOf(item);
            int favouriteIndex = FavouriteItems.IndexOf(item);
            item.Watched = !item.Watched;
            if (item.Watched == true)
            {
                item.ToWatch = false;
                FavouriteItems.RemoveAt(favouriteIndex);
                RealisedItems.Add(item);
            }
            else if (item.Watched == false)
            {
                if (index >= 0)
                {
                    RealisedItems.RemoveAt(index);
                }
            }

            FirebaseItem firebaseItem = new FirebaseItem() { ID = item.ID, Owner = username, Watched = item.Watched, ToWatch = item.ToWatch, Type = item.Type };
            await firebaseDatabase.UpdateItem(firebaseItem);
        }

        private async Task SignOut()
        {
            bool signOut = FirebaseAuth.SignOut();

            if (signOut)
            {
                await Shell.Current.GoToAsync($"//login");
            }
        }
        private async Task ChangePage(BrowseItem item)
        {
            if (item == null)
                return;

            switch (item.Type)
            {
                case "movies": await Shell.Current.GoToAsync($"detail?type={item.Type}&id={item.ID}"); break;
                case "tv series": await Shell.Current.GoToAsync($"detail?type={item.Type}&id={item.ID}"); break;
                default:
                    await Shell.Current.GoToAsync("//main/browse"); break;
            }
        }
        private async Task OnAppearing()
        {
            IsBusy = true;

            List<FirebaseItem> firebaseItems = await firebaseDatabase.GetItemsForUser(username);
            if (RealisedItems.Count > 0 || FavouriteItems.Count > 0)
            {
                await RefreshData(firebaseItems);
            }
            else
            {
                List<BrowseItem> favourite = new List<BrowseItem>();
                List<BrowseItem> realised = new List<BrowseItem>();

                foreach (FirebaseItem item in firebaseItems)
                {
                    BrowseItem browseItem;
                    switch (item.Type)
                    {
                        case "movies":
                            browseItem = await movieDB.BrowseMovie(item);
                            break;
                        case "tv series":
                            browseItem = await movieDB.BrowseTV(item);
                            break;
                        default:
                            continue;
                    }
                    if (browseItem != null)
                    {
                        if (browseItem.Watched == true)
                            realised.Add(browseItem);
                        if (browseItem.ToWatch == true)
                            favourite.Add(browseItem);
                    }
                }

                FavouriteItems.AddRange(favourite);
                RealisedItems.AddRange(realised);

            }

            IsBusy = false;
        }

        private async Task RefreshData(List<FirebaseItem> firebase)
        {
            List<BrowseItem> realised = new List<BrowseItem>();
            List<BrowseItem> favourite = new List<BrowseItem>();

            foreach (FirebaseItem item in firebase)
            {
                if (item.Watched == true)
                {
                    BrowseItem oldItem = RealisedItems.FirstOrDefault(x => x.ID == item.ID);
                    if (oldItem == null)
                    {
                        BrowseItem browseItem;
                        switch (item.Type)
                        {
                            case "movies":
                                browseItem = await movieDB.BrowseMovie(item);
                                break;
                            case "tv series":
                                browseItem = await movieDB.BrowseTV(item);
                                break;
                            default:
                                continue;
                        }
                        realised.Add(browseItem);
                    }
                    else
                    {
                        oldItem.Watched = item.Watched;
                        oldItem.ToWatch = item.ToWatch;
                        realised.Add(oldItem);
                    }
                }
                if (item.ToWatch == true)
                {
                    BrowseItem oldItem = FavouriteItems.FirstOrDefault(x => x.ID == item.ID);
                    if (oldItem == null)
                    {
                        BrowseItem browseItem;
                        switch (item.Type)
                        {
                            case "movies":
                                browseItem = await movieDB.BrowseMovie(item);
                                break;
                            case "tv series":
                                browseItem = await movieDB.BrowseTV(item);
                                break;
                            default:
                                continue;
                        }
                        favourite.Add(browseItem);
                    }
                    else
                    {
                        oldItem.Watched = item.Watched;
                        oldItem.ToWatch = item.ToWatch;
                        favourite.Add(oldItem);
                    }
                }
            }

            RealisedItems.Clear();
            RealisedItems.AddRange(realised);

            FavouriteItems.Clear();
            FavouriteItems.AddRange(favourite);
        }
    }
}