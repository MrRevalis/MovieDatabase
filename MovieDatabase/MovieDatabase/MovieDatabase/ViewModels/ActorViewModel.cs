using MovieDatabase.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MovieDatabase.ViewModels
{
    [QueryProperty("ID", "id")]
    public class ActorViewModel : BaseViewModel
    {
        public ICommand AppearingCommand { get; private set; }

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

        public ActorViewModel()
        {
            AppearingCommand = new Command(async () => await OnAppearing());
            ActorMovies = new ObservableRangeCollection<SearchItem>();
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
