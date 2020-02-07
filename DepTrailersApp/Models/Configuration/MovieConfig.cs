using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepTrailersApp.Models.Configuration
{
    /**
     * Model created to map the user secrets values
     * and enable its easier access within the controllers
     * **/
    public class MovieConfig
    {
        public string TmdbApiKey { get; set; }
        public string YtApiKey { get; set; }
        public string TmdbApiUrl { get; set; }
    }
}
