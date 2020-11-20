using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyVeryOwnSpotifyApi.Models
{

    public class ExternalUrls
    {
        [JsonPropertyName("spotify")]
        public string Spotify { get; set; }
    }

    public class Followers
    {
        [JsonPropertyName("href")]
        public object Href { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }
    }

    public class SearchItem
    {
        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonPropertyName("followers")]
        public Followers Followers { get; set; }

        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("images")]
        public List<Image> Images { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("popularity")]
        public int Popularity { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }

    public class Artists
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("items")]
        public List<SearchItem> searchItems { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("next")]
        public object Next { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("previous")]
        public object Previous { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class SearchResult

    {
        [JsonPropertyName("artists")]
        public Artists Artists { get; set; }
    }



}
