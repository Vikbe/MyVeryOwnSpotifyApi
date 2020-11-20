using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVeryOwnSpotifyApi.Models.Config
{
    //Same as AuthConfig.Json
    public class AuthConfig
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
    }
}
