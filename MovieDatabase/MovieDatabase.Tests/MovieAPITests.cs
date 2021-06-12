using MovieDatabase.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.Tests
{
    [TestFixture]
    public class MovieAPITests
    {
        IMovieDB movieDB;

        [SetUp]
        public void Setup()
        {
            movieDB = new MovieDB();
        }

        [Test]
        public async Task GetMovieDetails()
        {
            string id = "124";

            var movie = await movieDB.MovieDetail(id);

            Assert.IsNotNull(movie);
        }

        [Test]
        public void TimeConverter()
        {
            int runtime = 139;

            string newTime = movieDB.TimeConverter(runtime);

            Assert.AreEqual("2h 19m", newTime);
        }

        [Test]
        public async Task MoviesGenres()
        {
            var genres = await movieDB.MoviesGenres();

            Assert.Greater(genres.Count, 0);
            Assert.AreEqual("Action", genres[0].Name);
        }

        [Test]
        public async Task CorrectMovie()
        {
            string id = "550";

            var movie = await movieDB.MovieDetail(id);

            Assert.AreEqual("Fight Club", movie.Title);
        }
    }
}
