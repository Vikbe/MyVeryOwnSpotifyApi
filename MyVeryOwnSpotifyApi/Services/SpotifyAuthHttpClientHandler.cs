using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MyVeryOwnSpotifyApi.Services
{
    public sealed class SpotifyAuthHttpClientHandler : DelegatingHandler
    {
   
        private readonly SpotifyAuthService _spotifyAuthService;

        public SpotifyAuthHttpClientHandler(SpotifyAuthService spotifyAuthService)
        {
            
            _spotifyAuthService = spotifyAuthService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var token = await _spotifyAuthService.RetrieveToken();
            
            // Use the token to make the call.
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
