﻿using MovieDatabase.Models;
using MovieDatabase.Models.Actor;
using MovieDatabase.Models.ActorMovies;
using MovieDatabase.Models.Cast;
using MovieDatabase.Models.Movie;
using MovieDatabase.Models.MovieDetail;
using MovieDatabase.Models.MovieTrailers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class MovieDB : IMovieDB
    {
        private const string api = "fbfd2d53b7504d595ee9c450e52d4026";
        private const string imageSource = "https://image.tmdb.org/t/p/w500";
        private HttpClient client;
        public MovieDB()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://api.theMovieDB.org/3/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<BrowseItem> BrowseMovie(FirebaseItem item)
        {
            HttpResponseMessage response = await client.GetAsync($"movie/{item.ID}?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                MovieDetail movieResponse = JsonConvert.DeserializeObject<MovieDetail>(content);

                if (movieResponse != null)
                {
                    return new BrowseItem
                    {
                        ID = item.ID,
                        Image = imageSource + movieResponse.backdrop_path,
                        Title = movieResponse.title,
                        Type = "movies",
                        Watched = item.Watched,
                        ToWatch = item.ToWatch
                    };
                }
            }
            return new BrowseItem();
        }

        public async Task<BrowseItem> BrowseTV(FirebaseItem item)
        {
            HttpResponseMessage response = await client.GetAsync($"tv/{item.ID}?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                MovieDetail movieResponse = JsonConvert.DeserializeObject<MovieDetail>(content);

                if (movieResponse != null)
                {
                    return new BrowseItem
                    {
                        ID = item.ID,
                        Image = imageSource + movieResponse.backdrop_path,
                        Title = movieResponse.original_title ?? movieResponse.name,
                        Type = "tv series",
                        Watched = item.Watched,
                        ToWatch = item.ToWatch
                    };
                }
            }
            return new BrowseItem();
        }

        public async Task<List<CastDetail>> MovieCast(string ID)
        {
            HttpResponseMessage response = await client.GetAsync($"movie/{ID}/credits?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Credits castResponse = JsonConvert.DeserializeObject<Credits>(content);

                if (castResponse != null)
                {
                    return castResponse.cast.Select(x => new CastDetail
                    {
                        ID = x.id,
                        Name = x.name,
                        Image = imageSource + x.profile_path
                    }).ToList();
                }
            }

            return null;
        }

        public async Task<DetailItem> MovieDetail(string ID)
        {
            HttpResponseMessage response = await client.GetAsync($"movie/{ID}?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                MovieDetail movieResponse = JsonConvert.DeserializeObject<MovieDetail>(content);

                if (movieResponse != null)
                {
                    string genres = String.Join(", ", movieResponse.genres.Select(x => x.name));

                    return new DetailItem()
                    {
                        Title = movieResponse.original_title,
                        VoteAverage = movieResponse.vote_average,
                        Runtime = TimeConverter(movieResponse.runtime),
                        Release = movieResponse.release_date != null ? Convert.ToDateTime(movieResponse.release_date).ToShortDateString() : "",
                        Poster = imageSource + movieResponse.poster_path,
                        Background = imageSource + movieResponse.backdrop_path,
                        Genres = genres,
                        Overview = movieResponse.overview,
                        Cast = await MovieCast(ID)
                    };
                }
            }

            return null;
        }

        public async Task<List<Genres>> MoviesGenres()
        {
            HttpResponseMessage response = await client.GetAsync($"genre/movie/list?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                ListGenres genresResponse = JsonConvert.DeserializeObject<ListGenres>(content);

                if (genresResponse != null)
                {
                    return genresResponse.genres.Select(x => new Genres
                    {
                        ID = x.id,
                        Name = x.name
                    }).ToList();
                }
            }
            return null;
        }

        public async Task<List<SearchItem>> MoviesList(int ID)
        {
            HttpResponseMessage response = await client.GetAsync($"discover/movie?api_key={api}&language=en-US&sort_by=popularity.desc&with_genres={ID}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Movie movieResponse = JsonConvert.DeserializeObject<Movie>(content);
                if (movieResponse.results.Any())
                {
                    List<SearchItem> movies = movieResponse.results.Select(x => new SearchItem
                    {
                        ID = x.id.ToString(),
                        Type = "movies",
                        Title = x.title,
                        Background = imageSource + x.poster_path,
                    }).ToList();

                    return movies;
                }
            }
            return new List<SearchItem>();
        }

        public async Task<List<Video>> MoviesTrailers(string ID, string type)
        {
            string link = type == "movies" ? $"movie/{ID}/videos" : $"tv/{ID}/videos";
            HttpResponseMessage response = await client.GetAsync($"{link}?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                MoviesVideo moviesVideo = JsonConvert.DeserializeObject<MoviesVideo>(content);
                if (moviesVideo != null)
                {
                    return moviesVideo.results.Where(x => x.site == "YouTube").Select(y => new Video
                    {
                        YouTube = y.key,
                        Thumbail = $"https://i1.ytimg.com/vi/{y.key}/maxresdefault.jpg"
                    }).ToList();
                }
            }
            return null;
        }

        public async Task<List<SearchItem>> SearchMovie(string title)
        {
            HttpResponseMessage response = await client.GetAsync($"search/movie?api_key={api}&query={title}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Movie movieResponse = JsonConvert.DeserializeObject<Movie>(content);
                if (movieResponse.results.Any())
                {
                    List<SearchItem> movies = movieResponse.results.Select(x => new SearchItem
                    {
                        ID = x.id.ToString(),
                        Type = "movies",
                        Title = x.title,
                        Background = imageSource + x.poster_path,
                    }).ToList();

                    return movies;
                }
            }
            return new List<SearchItem>();
        }

        public async Task<List<SearchItem>> SearchTV(string title)
        {
            HttpResponseMessage response = await client.GetAsync($"search/tv?api_key={api}&query={title}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Movie movieResponse = JsonConvert.DeserializeObject<Movie>(content);
                if (movieResponse.results.Any())
                {
                    List<SearchItem> movies = movieResponse.results.Select(x => new SearchItem
                    {
                        ID = x.id.ToString(),
                        Type = "tv series",
                        Title = x.name,
                        Background = imageSource + x.poster_path,
                    }).ToList();

                    return movies;
                }
            }
            return new List<SearchItem>();
        }

        public async Task<List<SearchItem>> SimilarMovies(string ID)
        {
            HttpResponseMessage response = await client.GetAsync($"movie/{ID}/similar?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Movie movieResponse = JsonConvert.DeserializeObject<Movie>(content);
                if (movieResponse.results.Any())
                {
                    List<SearchItem> movies = movieResponse.results.Select(x => new SearchItem
                    {
                        ID = x.id.ToString(),
                        Type = "movies",
                        Title = x.title,
                        Background = imageSource + x.poster_path,
                    }).ToList();

                    return movies;
                }
            }
            return new List<SearchItem>();
        }

        public async Task<List<SearchItem>> SimilarTV(string ID)
        {
            HttpResponseMessage response = await client.GetAsync($"tv/{ID}/similar?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Movie movieResponse = JsonConvert.DeserializeObject<Movie>(content);
                if (movieResponse.results.Any())
                {
                    List<SearchItem> movies = movieResponse.results.Select(x => new SearchItem
                    {
                        ID = x.id.ToString(),
                        Type = "tv series",
                        Title = x.name,
                        Background = imageSource + x.poster_path,
                    }).ToList();

                    return movies;
                }
            }
            return new List<SearchItem>();
        }

        public async Task<List<SearchItem>> TrendingList(string mediaType, string time)
        {
            HttpResponseMessage response = await client.GetAsync($"trending/{mediaType}/{time}?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Movie movieResponse = JsonConvert.DeserializeObject<Movie>(content);
                if (movieResponse.results.Any())
                {
                    List<SearchItem> movies = movieResponse.results.Select(x => new SearchItem
                    {
                        ID = x.id.ToString(),
                        Type = x.media_type == "movie" ? "movies" : "tv series",
                        Title = x.title ?? x.name,
                        Background = imageSource + x.backdrop_path,
                        Poster = imageSource + x.poster_path
                    }).ToList();

                    return movies;
                }
            }
            return new List<SearchItem>();
        }

        public async Task<List<CastDetail>> TvCast(string ID)
        {
            HttpResponseMessage response = await client.GetAsync($"tv/{ID}/credits?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Credits castResponse = JsonConvert.DeserializeObject<Credits>(content);

                if (castResponse != null)
                {
                    return castResponse.cast.Select(x => new CastDetail
                    {
                        ID = x.id,
                        Name = x.name,
                        Image = imageSource + x.profile_path
                    }).ToList();
                }
            }

            return null;
        }

        public async Task<DetailItem> TvDetail(string ID)
        {
            HttpResponseMessage response = await client.GetAsync($"tv/{ID}?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                MovieDetail movieResponse = JsonConvert.DeserializeObject<MovieDetail>(content);

                if (movieResponse != null)
                {
                    string genres = String.Join(", ", movieResponse.genres.Select(x => x.name));

                    return new DetailItem()
                    {
                        Title = movieResponse.original_title ?? movieResponse.name,
                        VoteAverage = movieResponse.vote_average,
                        Runtime = TimeConverter(movieResponse.episode_run_time[0]),
                        Release = movieResponse.first_air_date != null ? Convert.ToDateTime(movieResponse.first_air_date).ToShortDateString() : "",
                        Poster = imageSource + movieResponse.poster_path,
                        Background = imageSource + movieResponse.backdrop_path,
                        Genres = genres,
                        Overview = movieResponse.overview,
                        Cast = await TvCast(ID)
                    };
                }
            }

            return null;
        }
        public async Task<ActorDetail> Actor(int ID)
        {
            HttpResponseMessage response = await client.GetAsync($"person/{ID}?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Actor actor = JsonConvert.DeserializeObject<Actor>(content);
                if(actor != null)
                {
                    return new ActorDetail
                    {
                        Birthday = actor.birthday,
                        Name = actor.name,
                        Picture = imageSource + actor.profile_path,
                        PlaceOfBirth = actor.place_of_birth,
                        Biography = actor.biography
                    };
                }
            }
            return null;
        }

        public string TimeConverter(int time)
        {
            TimeSpan newTime = TimeSpan.FromMinutes(time);
            if (newTime.Hours == 0)
            {
                return $"{newTime.Minutes % 60}m";
            }
            else
                return $"{newTime.Hours}h {newTime.Minutes % 60}m";
        }

        public async Task<List<SearchItem>> MovieCredits(int ID)
        {
            HttpResponseMessage response = await client.GetAsync($"person/{ID}/movie_credits?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                ActorMovies actorResponse = JsonConvert.DeserializeObject<ActorMovies>(content);

                if (actorResponse != null)
                {
                    return actorResponse.cast.Select(x => new SearchItem
                    {
                        ID = x.id.ToString(),
                        Type = "movies",
                        Title = x.original_title,
                        Background = imageSource + x.backdrop_path,
                        Poster = imageSource + x.poster_path
                    }).ToList();
                }
            }
            return new List<SearchItem>();
        }

        public async Task<List<SearchItem>> TvCredits(int ID)
        {
            HttpResponseMessage response = await client.GetAsync($"person/{ID}/tv_credits?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                ActorMovies actorResponse = JsonConvert.DeserializeObject<ActorMovies>(content);

                if (actorResponse != null)
                {
                    return actorResponse.cast.Select(x => new SearchItem
                    {
                        ID = x.id.ToString(),
                        Type = "tv series",
                        Title = x.original_name,
                        Background = imageSource + x.backdrop_path,
                        Poster = imageSource + x.poster_path
                    }).ToList();
                }
            }
            return new List<SearchItem>();
        }
    }
}
