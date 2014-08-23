using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;

namespace MovieRecommender
{
    public partial class RateMovie : PhoneApplicationPage
    {
        Movie movie;
        public RateMovie()
        {
            InitializeComponent();
            movie = new Movie(-1);
            GetMovie();
        }

        public void GetMovie()
        {
            try
            {
                WebClient webClient = new WebClient();
                string url = "http://api.themoviedb.org/3/movie/" + 279181 + "?api_key=9d8233dd037a14ac32e473b3147e67f0";
                Uri uri = new Uri(url);
                webClient.DownloadStringCompleted += webClient_DownloadStringCompleted;
                webClient.DownloadStringAsync(uri);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Result.ToString()))
                {
                    var rootObject = (FullMovie)JsonConvert.DeserializeObject<FullMovie>(e.Result.ToString());
                    movie.movieId = rootObject.id;
                    movie.movieImage = "http://image.tmdb.org/t/p/w500" + rootObject.poster_path;
                    movie.movieOriginalTitle = rootObject.original_title;
                    movie.movieReleaseDate = rootObject.release_date;
                    movie.movieStatus = rootObject.status;
                    movie.movieOverview = rootObject.overview;
                    movie.movieTitle = rootObject.title;
                    movie.movieGenres = rootObject.genres;
                    movie.movieHomepage = rootObject.homepage;
                    LayoutRoot.DataContext = movie;
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
    }
}