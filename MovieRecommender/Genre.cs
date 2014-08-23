using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace MovieRecommender
{
    public class Genre
    {
        public int id { get; set; }
        public String name { get; set; }
        public Genre(int genreId, String genreName)
        {
            this.id = genreId;
            this.name = genreName;
        }
    }

    public class MyMovieList
    {
        public string adult { get; set; }
        public string backdrop_path { get; set; }
        public long id { get; set; }
        public string original_title { get; set; }
        public string release_date { get; set; }
        public string poster_path { get; set; }
        public double popularity{get; set;}
        public string title{get; set;}
        public float vote_average { get; set; }
        public int vote_count { get; set; }

    }

    public class RootObject
    {
        public int page { get; set; }
        public List<MyMovieList> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }
}
