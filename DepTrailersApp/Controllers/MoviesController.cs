using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DepTrailersApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DepTrailersApp.Controllers
{
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly string API_KEY = "24ef18a9a2a6be1292a50289a8b49004";
        private readonly string API_URL = "https://api.themoviedb.org/3/";
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
            Uri url = new Uri($"{API_URL}search/movie?api_key={API_KEY}&query={searchString}");

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
