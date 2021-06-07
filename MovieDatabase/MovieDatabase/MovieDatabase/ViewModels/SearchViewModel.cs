using MovieDatabase.Extensions;
using MovieDatabase.Models;
using MovieDatabase.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MovieDatabase.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private IMovieDB movieDB;

        private int batchSize = 30;
        private int currentIndex = 0;

        #region Commands
        public ICommand AppearingCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand LoadMoreItems { get; private set; }
        public ICommand ChangePageCommand { get; private set; }
        #endregion
        #region Properties
        private string id;
        public string ID
        {
            get => id;
            set => SetProperty(ref id, value);
        }
        private string type;
        public string Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }
        private string searchedItem;
        public string SearchedItem
        {
            get => searchedItem;
            set => SetProperty(ref searchedItem, value);
        }

        private ObservableRangeCollection<SearchItem> searchList;
        public ObservableRangeCollection<SearchItem> SearchList
        {
            get => searchList;
            set => SetProperty(ref searchList, value);
        }

        private List<SearchItem> itemsCollection;
        public List<SearchItem> ItemsCollection
        {
            get => itemsCollection;
            set => SetProperty(ref itemsCollection, value);
        }
        #endregion
        public SearchViewModel()
        {
            movieDB = DependencyService.Get<IMovieDB>();

            ID = Preferences.Get("LastItem", null);
            Type = Preferences.Get("LastType", null);

            AppearingCommand = new Command(async () => await OnAppearing());
            SearchCommand = new Command<string>(async (sender) => await Search(sender));
            LoadMoreItems = new Command(LoadMore);
            ChangePageCommand = new Command<SearchItem>(async (sender) => await ChangePage(sender));
            SearchList = new ObservableRangeCollection<SearchItem>();
            ItemsCollection = new List<SearchItem>();
        }

        private async Task ChangePage(SearchItem item)
        {
            if (item == null)
                return;

            switch (item.Type)
            {
                case "movies": await Shell.Current.GoToAsync($"detail?type={item.Type}&id={item.ID}"); break;
                case "tv series": await Shell.Current.GoToAsync($"detail?type={item.Type}&id={item.ID}"); break;
                default:
                    await Shell.Current.GoToAsync("//main"); break;
            }
        }

        private async Task Search(string title)
        {
            if (IsBusy)
                return;

            if (String.IsNullOrEmpty(title))
                return;

            IsBusy = true;
            try
            {
                List<SearchItem> movies = await movieDB.SearchMovie(title);
                List<SearchItem> tv = await movieDB.SearchTV(title);

                List<SearchItem> concat = movies.Concat(tv).OrderBy(x => x.ID).ToList();
                if (concat.Any())
                {
                    SearchedItem = title;
                    SearchList.RemoveAll();
                    ItemsCollection = concat;
                    AddToObservable();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e.Message}");
            }

            IsBusy = false;
        }
        private void LoadMore()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            if (currentIndex < ItemsCollection.Count())
            {
                int itemsNumber = ItemsCollection.Count() - currentIndex < batchSize ? ItemsCollection.Count() - currentIndex : batchSize;

                SearchList.AddRange(
                    ItemsCollection.Skip(currentIndex).Take(itemsNumber)
                    );

                currentIndex += itemsNumber;
            }

            IsBusy = false;
        }

        private async Task OnAppearing()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            if (!String.IsNullOrEmpty(Type) && !String.IsNullOrEmpty(ID))
            {
                if (!SearchList.Any())
                {
                    List<SearchItem> similarItems = new List<SearchItem>();
                    switch (Type.ToLower())
                    {
                        case "movies": similarItems = await movieDB.SimilarMovies(ID); break;
                        case "tv series": similarItems = await movieDB.SimilarTV(ID); break;
                    }

                    if (similarItems.Any())
                    {
                        ItemsCollection = similarItems;
                        AddToObservable();
                    }
                }
            }
            IsBusy = false;
        }

        private void AddToObservable()
        {
            int itemsNumber = ItemsCollection.Count() - currentIndex < batchSize ? ItemsCollection.Count() - currentIndex : batchSize;
            SearchList.AddRange(ItemsCollection.Take(itemsNumber));
            currentIndex = itemsNumber;
        }
    }
}
