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
        public List<Genre> movieGenres { get; set; }
        public String movieOverview { get; set; }
        public String movieStatus { get; set; }
        
        public Movie(long movieId)
        {
            this.movieId = movieId;
            movieTitle = "Unknown";
            movieOriginalTitle = "Unknown";
            movieOverview = "Unknown";
            movieImage = "filmimage.png";
            movieReleaseDate = "Unknown";
            movieStatus = "Unknown";
            movieGenres = new List<Genre>();

        }

    }
    

}
