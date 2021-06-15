using MovieDatabase.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MovieDatabase.ViewModels
{
    [QueryProperty("ID", "id")]
    public class ActorViewModel : BaseViewModel
    {
        public ICommand AppearingCommand { get; private set; }
        public ICommand MovieClickedCommand { get; private set; }

        private int id;
        public int ID
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private ActorDetail actor;
        public ActorDetail Actor
        {
            get => actor;
            set => SetProperty(ref actor, value);
        }

        private ObservableRangeCollection<SearchItem> actorMovies;
        public ObservableRangeCollection<SearchItem> ActorMovies
        {
            get => actorMovies;
            set => SetProperty(ref actorMovies, value);
        }

        private SearchItem selectedMovie;
        public SearchItem SelectedMovie
        {
            get => selectedMovie;
            set => SetProperty(ref selectedMovie, value);
        }
        public ActorViewModel()
        {
            AppearingCommand = new Command(async () => await OnAppearing());
            ActorMovies = new ObservableRangeCollection<SearchItem>();
            MovieClickedCommand = new Command<SearchItem>(async (sender) => await MovieClicked(sender));
        }

        private async Task MovieClicked(SearchItem movie)
        {
            if (movie == null)
                return;

            SelectedMovie = null;

            Preferences.Set("LastItem", movie.ID);
            Preferences.Set("LastType", movie.Type);

            await Shell.Current.GoToAsync($"detail?type={movie.Type}&id={movie.ID}");
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            if (ID < 0)
                await Shell.Current.GoToAsync("..");

            ActorDetail tempActor = await MovieDB.Actor(ID);
            if(tempActor != null)
            {
                Actor = tempActor;
            }
            else
            {
                await Shell.Current.GoToAsync("..");
            }

            List<SearchItem> movies = await MovieDB.MovieCredits(ID);
            List<SearchItem> tvSeries = await MovieDB.TvCredits(ID);
            ActorMovies.AddRange(movies.Concat(tvSeries).OrderBy(x => x.ID).ToList());


            IsBusy = false;
        }
    }
}
