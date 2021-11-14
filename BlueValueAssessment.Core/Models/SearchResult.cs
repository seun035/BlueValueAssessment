﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BlueValueAssessment.Core.Models
{
    public class SearchResult
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }

        [JsonProperty("imdbRating")]
        public string ImdbRating { get; set; }
        [JsonProperty("imdbVotes")]
        public string ImdbVotes { get; set; }
        [JsonProperty("imdbID")]
        public string ImdbID { get; set; }
    }
}
