using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepTrailersApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public float Popularity { get; set; }
        public float Rating { get; set; }
        public string Release { get; set; }
        public string Trailer { get; set; }
    }
}
