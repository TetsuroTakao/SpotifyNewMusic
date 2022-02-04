using System;
using System.Collections.Generic;

namespace SpotifyNewMusic.Models
{
    public class Artist
    {
	    public SpotifyUrl external_urls { get; set; }
        public FollowersProperties followers { get; set; }
        public List<string> genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<SpotityImage> images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
    public class Album
    {
        public string album_type { get; set; }
        public List<Artist> artists { get; set; }
        public List<string> available_markets { get; set; }
        public SpotifyUrl external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<SpotityImage> images { get; set; }
        public string name { get; set; }
        public DateTime release_date { get; set; }
        public string release_date_precision { get; set; }
        public int total_tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
    public class SpotifyUrl
    {
        public string spotify { get; set; }
    }
    public class FollowersProperties
    {
        public string href { get; set; }
        public int total { get; set; }
    }
    public class SpotityImage
    {
        public Nullable<int> height { get; set; }
        public string url { get; set; }
        public Nullable<int> width { get; set; }
    }
}
