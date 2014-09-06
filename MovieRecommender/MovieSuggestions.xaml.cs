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
using Microsoft.Phone.Shell;
using System.Text;
using System.Diagnostics;

namespace MovieRecommender
{
    public partial class MovieSuggestions : PhoneApplicationPage
    {
        private const string strConnectionString = @"DataSource=isostore:/MoviesDB.sdf";
        List<Movie> movies;
        long genreId;
        public MovieSuggestions()
        {
            InitializeComponent();
            movies = new List<Movie>();
            listMovies.SelectedIndex = -1;
            genreId = 1;
            GetMovies();
            listMovies.ItemsSource = movies;
        }

        public void GetMovies()
        {
            using (MovieDataContext Empdb = new MovieDataContext(strConnectionString))
            {
                if (Empdb.DatabaseExists())
                {
                    IList<FavoriteGenres> gen = null;
                    IQueryable<FavoriteGenres> Query = from FavoriteGenres mv in Empdb.FavoriteGenres select mv;
                    gen = Query.ToList();
                    
                    foreach (FavoriteGenres genre in gen)
                    {
                        genreId = genre.genreId;
                        WebClient webClient = new WebClient();
                        webClient.DownloadStringCompleted += webClient_DownloadStringCompleted;

                        Uri uri = new Uri("https://api.themoviedb.org/3/discover/movie?api_key=9d8233dd037a14ac32e473b3147e67f0&with_genres=" + genre.genreId);
                        webClient.DownloadStringAsync(uri);
                    }

                }

            }

        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Result.ToString()))
                {
                    var rootObject = JsonConvert.DeserializeObject<RootObject>(e.Result.ToString());
                    Random rnd = new Random();
                    int newPage = rnd.Next(1, 10);
                    WebClient webClient = new WebClient();
                    Uri uri = new Uri("https://api.themoviedb.org/3/discover/movie?api_key=9d8233dd037a14ac32e473b3147e67f0&with_genres=" + genreId + "&page=" + newPage);
                    webClient.DownloadStringCompleted += webClient_DownloadStringComplete;
                    webClient.DownloadStringAsync(uri);
                    Debug.WriteLine(uri.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        void webClient_DownloadStringComplete(object sender, DownloadStringCompletedEventArgs e)
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
                        listMovies.ItemsSource = null;
                        listMovies.ItemsSource = movies;
                        Debug.WriteLine("MOVIE ADDED: " + movie.movieTitle);
                    }

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
            PhoneApplicationService.Current.State["which"] = 1;
            NavigationService.Navigate(new Uri("/MovieReview.xaml", UriKind.Relative));
        }
    }
}