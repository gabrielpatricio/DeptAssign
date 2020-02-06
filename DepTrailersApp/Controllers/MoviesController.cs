using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DepTrailersApp.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DepTrailersApp.Controllers
{
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly string TMDB_API_KEY = "24ef18a9a2a6be1292a50289a8b49004";
        private readonly string TMDB_API_URL = "https://api.themoviedb.org/3/";
        private readonly string YT_API_KEY = "AIzaSyCi3fLy92sRIlSNoh3lteI0IALE0dqOG2k";

        private readonly HttpClient http = new HttpClient();

        // GET api/movies/find?q={query}
        [HttpGet("find/")]
        public async Task<ActionResult<Movie>> getSearchResults([FromQuery(Name = "q")]string query)
        {
            Uri url = new Uri($"{TMDB_API_URL}search/movie?api_key={TMDB_API_KEY}&query={query}");

            var responseString = await http.GetStringAsync(url);
            dynamic body = JsonConvert.DeserializeObject(responseString);

            var movieList = new List<Movie>();
            var searchResults = body["results"];

            foreach (var entry in searchResults)
            {
                // Use getters and setters

                Movie movie = new Movie
                {
                    Id = entry.id,
                    Title = entry.original_title,
                    Poster = entry.poster_path,
                    Popularity = entry.popularity,
                    Rating = entry.vote_average,
                    Release = entry.release_date
                };

                movieList.Add(movie);
            }
            var json = JsonConvert.SerializeObject(movieList);

            return Ok(json);
        }
        // GET /api/movies/popular
        [HttpGet("popular/")]
        public async Task<ActionResult<Movie>> getPopularMovies()
        {

            Uri url = new Uri($"{TMDB_API_URL}movie/popular?api_key={TMDB_API_KEY}&language=en-US&page=1");

            var responseString = await http.GetStringAsync(url);
            dynamic body = JsonConvert.DeserializeObject(responseString);

            var movieList = new List<Movie>();
            var searchResults = body["results"];

            foreach (var entry in searchResults)
            {
                Movie movie = new Movie
                {
                    Id = entry.id,
                    Title = entry.original_title,
                    Poster = entry.poster_path,
                    Popularity = entry.popularity,
                    Rating = entry.vote_average,
                    Release = entry.release_date
                };

                movieList.Add(movie);
            }
            var json = JsonConvert.SerializeObject(movieList);

            return Ok(json);
        }
        // Get api/movies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> getMovie(int id)
        {

            Uri url = new Uri($"{TMDB_API_URL}movie/{id}?api_key={TMDB_API_KEY}&language=en-US");

            var responseString = await http.GetStringAsync(url);
            dynamic body = JsonConvert.DeserializeObject(responseString);

            Movie movie = new Movie
            {
                Id = body.id,
                Title = body.original_title,
                Poster = body.poster_path,
                Popularity = body.popularity,
                Rating = body.vote_average,
                Release = body.release_date,
                Trailer = new MoviesController().getTrailer(body.original_title, body.release_date)
            };


            var json = JsonConvert.SerializeObject(movie);
            return Ok(json);
        }


        private string getTrailer(dynamic original_title, dynamic release_date)
        {
            var search = original_title + " official trailer " + Convert.ToDateTime(release_date).Year;
            var videoId = "";
            YouTubeService youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = YT_API_KEY,
                ApplicationName = "deptrailers"
            });

            SearchResource.ListRequest listRequest = youtube.Search.List("snippet");
            listRequest.Q = search;
            listRequest.Type = "video";
            listRequest.MaxResults = 1;

            SearchListResponse searchResponse = listRequest.Execute();

            foreach (var searchResult in searchResponse.Items)
            {
                videoId = searchResult.Id.VideoId;
            }

            return videoId;
        }
    }
}
