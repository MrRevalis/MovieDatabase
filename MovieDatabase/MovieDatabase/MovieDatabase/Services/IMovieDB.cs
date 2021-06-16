using MovieDatabase.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public interface IMovieDB
    {
        Task<List<SearchItem>> TrendingList(string mediaType, string time);
        Task<List<SearchItem>> SearchMovie(string title);
        Task<List<SearchItem>> SearchTV(string title);
        Task<DetailItem> MovieDetail(string ID);
        Task<DetailItem> TvDetail(string ID);
        Task<List<Genres>> MoviesGenres();
        Task<List<SearchItem>> MoviesList(int ID);
        Task<List<CastDetail>> MovieCast(string ID);
        Task<List<CastDetail>> TvCast(string ID);
        Task<List<SearchItem>> SimilarMovies(string ID);
        string TimeConverter(int time);
        Task<List<SearchItem>> SimilarTV(string ID);
        Task<List<Video>> MoviesTrailers(string ID, string type);
        Task<BrowseItem> BrowseMovie(FirebaseItem item);
        Task<BrowseItem> BrowseTV(FirebaseItem item);

        //Egzamin
        Task<ActorDetail> Actor(int ID);
        Task<List<SearchItem>> MovieCredits(int ID);
        Task<List<SearchItem>> TvCredits(int ID);

        Task<List<SearchItem>> DiscoverMovies();
    }
}
