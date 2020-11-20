using Microsoft.AspNetCore.Http.Extensions;
using MyVeryOwnSpotifyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyVeryOwnSpotifyApi.Services
{
    public class SpotifyApi
    {
        private readonly HttpClient _httpClient;


        public SpotifyApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<SearchItem> Search(string search)
        {
           

            string searchQuery = $"q={Uri.EscapeDataString(search)}";
            string searchType = "type=artist";

            //Create the uri query
            UriBuilder baseUri = new UriBuilder();

            if (baseUri.Query != null && baseUri.Query.Length > 1)
                baseUri.Query = baseUri.Query.Substring(1) + "&" + searchQuery;
            else
                baseUri.Query = searchQuery;


            baseUri.Query = baseUri.Query.Substring(1) + "&" + searchType;

         


            var response = await _httpClient.GetAsync(baseUri.Query);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error retrieving from spotify search api.");

            }

            var searchResult = JsonSerializer.Deserialize<SearchResult>(await response.Content.ReadAsStringAsync());


            

            if (searchResult.Artists.searchItems.Count >= 1)
            {
                //Remove all punctuation and whitespaces convert to lowercase and compare it to the artist name for the best possible match.
                var trimmedSearch = Regex.Replace(search, @"[^\w]", "").ToLower();

                var artistItem = searchResult.Artists.searchItems
                    .Where(i => Regex.Replace(i.Name, @"[^\w]", "").ToLower().Contains(trimmedSearch))
                    .OrderByDescending(i => i.Popularity).FirstOrDefault(); //Sorting by popularity seems like a reasonable idea at first glance.

                return artistItem;
            }


            return null;
           

        }


    }
}

