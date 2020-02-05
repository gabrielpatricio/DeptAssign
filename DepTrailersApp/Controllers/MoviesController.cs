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
        private readonly string YT_API_KEY = "AIzaSyDRQG6vA0maCr9G6Cfy0ABCc_e5cO51q5E";

        private readonly HttpClient http = new HttpClient();
        // GET: api/movies
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/movies/{searchString}
        [HttpGet("{searchString}")]
        public async Task<ActionResult<Movie>> Get(string searchString)
        {

            Uri url = new Uri($"{TMDB_API_URL}search/movie?api_key={TMDB_API_KEY}&query={searchString}");

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
                    Release = entry.release_date,
                    Trailer = new MoviesController().getTrailer(entry.original_title, entry.release_date)
                };

                movieList.Add(movie);
            }
            var json = JsonConvert.SerializeObject(movieList);

            return Ok(json);
        }

        private string getTrailer(dynamic original_title, dynamic release_date)
        {
            var search = original_title + " official trailer " + Convert.ToDateTime(release_date).Year;
            var videoId = "";
            YouTubeService youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = YT_API_KEY

            });
            SearchResource.ListRequest listRequest = youtube.Search.List("snippet");
            listRequest.Q = search;
            listRequest.Type = "video";
            listRequest.MaxResults = 2;

            SearchListResponse searchResponse = listRequest.Execute();

            foreach (var searchResult in searchResponse.Items)
            {
                videoId = searchResult.Id.VideoId;
            }

            return videoId;
        }


        // POST api/movies
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/movies/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/movies/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
