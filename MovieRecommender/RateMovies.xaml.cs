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
using System.Text;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace MovieRecommender
{
    public partial class RateMovies : PhoneApplicationPage
    {
        List<Movie> movies;
        public RateMovies()
        {
            InitializeComponent();
            movies = new List<Movie>();
            listMovies.SelectedIndex = -1;
            GetMovies();
        }

        public void GetMovies()
        {
            try
            {
                WebClient webClient = new WebClient();
                Uri uri = new Uri("https://api.themoviedb.org/3/discover/movie?api_key=9d8233dd037a14ac32e473b3147e67f0&with_genres=35&primary_release_year=2014");
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
                    var rootObject = JsonConvert.DeserializeObject<RootObject>(e.Result.ToString());
                    foreach (var res in rootObject.results)
                    {
                        Movie movie = new Movie(res.id);
                        movie.movieImage = "http://image.tmdb.org/t/p/w500" + res.poster_path;
                        movie.movieTitle = res.title;
                        movie.movieReleaseDate = res.release_date;
                        movies.Add(movie);
                       
                    }
                    listMovies.ItemsSource = movies;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void listMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Movie movie = listMovies.SelectedItem as Movie;
            PhoneApplicationService.Current.State["movieId"] = movie.movieId;
            PhoneApplicationService.Current.State["which"] = 0;
            NavigationService.Navigate(new Uri("/MovieReview.xaml", UriKind.Relative));
        }
    }
}