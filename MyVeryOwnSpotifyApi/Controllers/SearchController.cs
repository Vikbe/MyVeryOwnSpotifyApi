using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyVeryOwnSpotifyApi.Models;
using MyVeryOwnSpotifyApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyVeryOwnSpotifyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {

        private SpotifyApi _spotifyApi;
        private readonly ILogger<SearchController> _logger;

        public SearchController(SpotifyApi spotifyApi, ILogger<SearchController> logger)
        {
            _spotifyApi = spotifyApi;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string search)
        {
            
            var artist = await _spotifyApi.Search(search);
            
            if(artist != null)
            {
                var result = new
                {
                    artist_id = artist.Id,
                    artist_name = artist.Name
                };

                return Ok(result);
            }
            
            string noMatch = 
                $" Sorry! :( \r\n Could not find a match for \"{search}\"";
          
            //No match should probably return a 200? seeing as there was no actual error and the search worked. Maybe a 204 would be more or equally approriate.
            return Ok(noMatch);
        }
    }
}
