using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LenoApp.Models
{
    public class Word
    {
        [JsonProperty("word", NullValueHandling = NullValueHandling.Ignore)]
        public string word { get; set; }   //Comment

        [JsonProperty("translation", NullValueHandling = NullValueHandling.Ignore)]
        public string translation { get; set; }   //Comment

    }
}
