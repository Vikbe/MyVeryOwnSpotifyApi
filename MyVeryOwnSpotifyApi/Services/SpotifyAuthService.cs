using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;
using MyVeryOwnSpotifyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyVeryOwnSpotifyApi.Services
{
    public class SpotifyAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;


        public SpotifyAuthService(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }

        //Get and refresh bearer token from spotify's auth api.
        //This is called with every request from the SpotifyApi service through SpotifyAuthenticationHttpClientHandler
        public async Task<string> RetrieveToken()
        {

            //See if the bearer token already exists in memory cache and has not expired, if so gets a new one.
            if (!_memoryCache.TryGetValue("Token", out string access_token))
            {
                
                var form = new Dictionary<string, string>();
                form.Add("grant_type", "client_credentials");

                //Using PostAsync instead of SendAsync would not let me set Content-Type: application/x-www-form-urlencoded
                var req = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token") //URL should probably be in a config or similar.
                {
                    Content = new FormUrlEncodedContent(form)
                };
                
                var response = await _httpClient.SendAsync(req);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Error getting authentication from Spotify API.");
                }
                
                response.EnsureSuccessStatusCode();

                var responseData = JsonSerializer.Deserialize<AuthResponse>(await response.Content.ReadAsStringAsync());

                access_token = responseData.AccessToken;



                _memoryCache.Set("Token", access_token, new DateTimeOffset(DateTime.Now).AddSeconds(responseData.ExpiresIn - 60));
            }

            return access_token;
        }


    }
}

