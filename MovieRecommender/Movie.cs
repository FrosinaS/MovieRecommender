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

namespace MovieRecommender
{
    public class Movie
    {
        public long movieId { get; set; }
        public String movieTitle { get; set; }
        public String movieOriginalTitle { get; set; }
        public String movieImage { get; set; }
        public String movieReleaseDate { get; set; }
        public List<Genres> movieGenres { get; set; }
        public String movieOverview { get; set; }
        public String movieStatus { get; set; }
        public String movieHomepage{get; set;}
        public Movie(long movieId)
        {
            this.movieId = movieId;
            movieTitle = "Unknown";
            movieOriginalTitle = "Unknown";
            movieOverview = "Unknown";
            movieImage = "filmimage.png";
            movieReleaseDate = "Unknown";
            movieStatus = "Unknown";
            movieGenres = new List<Genres>();
            movieHomepage="/";
        }

    }
    public class FullMovie
    {
        public String adult { get; set; }
        public String backdrop_path { get; set; }
        public String belongs_to_collection { get; set; }
        public long budget { get; set; }
        public List<Genres> genres { get; set; }
        public String homepage { get; set; }
        public long id { get; set; }
        public String imdb_id { get; set; }
        public String original_title { get; set; }
        public String overview { get; set; }
        public float popularity { get; set; }
        public String poster_path { get; set; }
        public List<ProductionCompanies> production_companies { get; set; }
        public List<ProductionCountries> production_countries { get; set; }
        public String release_date { get; set; }
        public long revenue { get; set; }
        public float runtime { get; set; }
        public List<ProductionCompanies> spoken_languages { get; set; }
        public String status { get; set; }
        public String tagline { get; set; }
        public String title { get; set; }
        public float vote_average { get; set; }
        public long vote_count { get; set; }
    }
   
    public class ProductionCompanies
    {
        public String name { get; set; }
        public long id { get; set; }
        public ProductionCompanies(String name, long id)
        {
            this.name = name;
            this.id = id;
        }
    }

    public class ProductionCountries
    {
        public String iso { get; set; }
        public String name { get; set; }
        public ProductionCountries(String name, String iso)
        {
            this.iso = iso;
            this.name = name;
        }
    }

}
