using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DepTrailersApp.Models;
using DepTrailersApp.Models.Configuration;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace DepTrailersApp.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {

        private readonly MovieConfig _movieConfig;
        private readonly string TMDB_API_URL = "https://api.themoviedb.org/3/";
        private readonly HttpClient http = new HttpClient();
        
        public MoviesController(IOptions<MovieConfig> movieConfig)
        {
            // MovieConfig injected into the constructor   
            // Verify existence of API Keys in Configuration
            _movieConfig = movieConfig.Value ?? throw new ArgumentException(nameof(movieConfig));
        }

        /**
        * List movies matching with the string send as a parameter in the query
        * 
        * Cache the content for every different query ('q') value.
        * **/
        // GET api/movies/find?q={query}
        [HttpGet("find/")]
        [ResponseCache(Duration = 30, VaryByQueryKeys = new string[] { "q" })] 
        public async Task<ActionResult<Movie>> getSearchResults([FromQuery(Name = "q")]string query)
        {
             var responseString="";

            // Handling empty || null query Exception, sending empty result to client-side
            try {  
            // Request TmDB Api movies matching query string
            responseString = await http.GetStringAsync(
                new Uri($"{TMDB_API_URL}search/movie?api_key={_movieConfig.TmdbApiKey}&query={query}"));
            }
            catch (HttpRequestException)
            {
                return Ok();
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }

            var jsonString = JsonConvert.SerializeObject(buildMovieList(responseString));
            
            return Ok(jsonString);
        }
        
        /**
         * List popular movies to display in the homepage        
         * **/
        // GET api/movies/popular
        [HttpGet("popular/")]
        public async Task<ActionResult<Movie>> getPopularMovies()
        {
            var responseString = "";
            // Request TmDB Api popular movies
            try { 
                 responseString = await http.GetStringAsync(
                     new Uri($"{TMDB_API_URL}movie/popular?api_key={_movieConfig.TmdbApiKey}&language=en-US&page=1"));
            }
            catch(Exception e)
            {
                if(e is ArgumentNullException || e is HttpRequestException)
                {
                    return BadRequest();
                }

            }
            var jsonString = JsonConvert.SerializeObject(buildMovieList(responseString));
            
            return Ok(jsonString);
        }

        
        /**
         * Get specific movie details
         * **/
        // Get api/v1.0/movies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> getMovie(int id)
        {
            var responseString = "";
            try { 
                responseString = await http.GetStringAsync(
                    new Uri($"{TMDB_API_URL}movie/{id}?api_key={_movieConfig.TmdbApiKey}&language=en-US"));
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException || e is HttpRequestException) { return BadRequest(); }
            }
            dynamic body = JsonConvert.DeserializeObject(responseString);
            
            // Build movie object with all the info
            Movie movie = new Movie
            {
                Id = body.id,
                Title = body.original_title,
                Poster = body.poster_path,
                Popularity = body.popularity,
                Rating = body.vote_average,
                Release = body.release_date,
                Overview = body.overview,
                Trailer = this.getTrailer(body.original_title, body.release_date)
            };

            var jsonString = JsonConvert.SerializeObject(movie);
            
            return Ok(jsonString);
        }

        /**
         * Build movie object and add it to 
         * list of movies
         * 
         * @return list of movies with movie objects
         * **/
        private List<Movie> buildMovieList(string responseString)
        {
            // Deserialize response JSON string to an object and extract array of results
            dynamic body = JsonConvert.DeserializeObject(responseString);
            var results = body["results"];

            //Create list of movies
            var movieList = new List<Movie>();

            foreach (var movieItem in results)
            {
                // Build movie object
                Movie movie = new Movie
                {
                    Id = movieItem.id,
                    Title = movieItem.original_title,
                    Poster = movieItem.poster_path,
                    Popularity = movieItem.popularity,
                    Rating = movieItem.vote_average,
                    Release = movieItem.release_date
                };
                // Add movie to list of movies
                movieList.Add(movie);
            }
            return movieList;
        }

        /**
         * Request Youtube Api for movie trailer
         * **/
        private string getTrailer(dynamic original_title, dynamic release_date)
        {
            // Creation of string to search for trailer on Youtube 
            var search = original_title + " official trailer " + Convert.ToDateTime(release_date).Year;
            
            YouTubeService youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _movieConfig.YtApiKey
            });

            SearchResource.ListRequest listRequest = youtube.Search.List("snippet");
            listRequest.Q = search; // search term
            listRequest.Type = "video"; 
            listRequest.MaxResults = 1; // I decided to keep the first result as the choosen one to associate with the movie

            SearchListResponse searchResponse = listRequest.Execute();
            
            // Handle possible occurrence of no results
            if (searchResponse.Items.Count < 0)
            {
                return "";
            }
            return searchResponse.Items[0].Id.VideoId;
        }
    }
}
