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
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.Phone.Shell;
using System.Text;

namespace MovieRecommender
{
    public partial class RateMovie : PhoneApplicationPage
    {
        private const string strConnectionString = @"DataSource=isostore:/MoviesDB.sdf";
        Movie movie;
        long movieID;
        public RateMovie()
        {
            InitializeComponent();
            movieID = 0;
            movie = new Movie(-1);
            var k = PhoneApplicationService.Current.State["movieId"];
            movieID = Convert.ToInt64(k);
            movie.movieId = movieID;
            CheckVoting();
            GetMovie();
        }

        public void CheckVoting()
        {
            IList<RatedMovies> ratedMovies = null;

            using (MovieDataContext Empdb = new MovieDataContext(strConnectionString))
            {
                if (Empdb.DatabaseExists())
                {
                    IQueryable<RatedMovies> EmpQuery = from RatedMovies mv in Empdb.RatedMovies where mv.movieId == movieID select mv;
                    ratedMovies = EmpQuery.ToList();
                    foreach (RatedMovies movie in ratedMovies)
                    {
                        if (movie.movieVote)
                        {
                            rateUpBtn.IsEnabled = false;

                        }
                        else
                        {
                            rateUpBtn.IsEnabled = false;
                        }
                    }
                    IList<FavoriteGenres> favGenres = null;
                    IQueryable<FavoriteGenres> favQuery = from FavoriteGenres mv in Empdb.FavoriteGenres select mv;
                    favGenres = favQuery.ToList();
                    foreach (FavoriteGenres gen in favGenres)
                    {
                        FavoriteGenres gen1 = new FavoriteGenres();
                        gen1.genreId = gen.genreId;
                    }
                }

            }
        }
                
               
        public void GetMovie()
        {
            try
            {
                WebClient webClient = new WebClient();
                string url = "http://api.themoviedb.org/3/movie/" + movieID + "?api_key=9d8233dd037a14ac32e473b3147e67f0";
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
                    foreach (Genre gen in rootObject.genres)
                    {
                        Genre genre = new Genre(gen.id, gen.name);
                        movie.movieGenres.Add(genre);
                    }
                    movie.movieHomepage = rootObject.homepage;
                    LayoutRoot.DataContext = movie;
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void createDb()
        {
            using (MovieDataContext MvDb = new MovieDataContext(strConnectionString))
            {
                try
                {
                    if (MvDb.DatabaseExists() == false)
                    {
                        MvDb.CreateDatabase();  
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

       

        private void rateDownBtn_Click(object sender, RoutedEventArgs e)
        {
            createDb();

            using (MovieDataContext MvDb = new MovieDataContext(strConnectionString))
            {
                
                IQueryable<RatedMovies> EmpQuery = from RatedMovies mv in MvDb.RatedMovies where mv.movieId == movieID select mv;
                RatedMovies ratedMovies = EmpQuery.FirstOrDefault();
                if (ratedMovies != null)
                {
                    MvDb.RatedMovies.DeleteOnSubmit(ratedMovies);
                    MvDb.SubmitChanges();
                }
                RatedMovies newMovie = new RatedMovies
                {
                    movieId = movieID,
                    movieVote = false
                };
                MvDb.RatedMovies.InsertOnSubmit(newMovie);
                MvDb.SubmitChanges();
                MessageBox.Show("You gave negative rating for this movie!");
            }

        }

        private void rateUpBtn_click(object sender, RoutedEventArgs e)
        {
            createDb();
            using (MovieDataContext MvDb = new MovieDataContext(strConnectionString))
            {
                IQueryable<RatedMovies> EmpQuery = from RatedMovies mv in MvDb.RatedMovies where mv.movieId == movieID select mv;
                RatedMovies ratedMovies = EmpQuery.FirstOrDefault();
                if (ratedMovies != null)
                {
                    MvDb.RatedMovies.DeleteOnSubmit(ratedMovies);
                    MvDb.SubmitChanges();
                }
                RatedMovies newMovie = new RatedMovies
                {
                    movieId = movieID,
                    movieVote = true
                };
                MvDb.RatedMovies.InsertOnSubmit(newMovie);
                MvDb.SubmitChanges();
                foreach (Genre genre in movie.movieGenres)
                {
                    
                    IList<FavoriteGenres> gen = null;
                    IQueryable<FavoriteGenres> Query = from FavoriteGenres mv in MvDb.FavoriteGenres where mv.genreId==genre.id select mv;
                    gen = Query.ToList();
                    
                    if (gen == null)
                    {
                        FavoriteGenres fav = new FavoriteGenres
                        {
                            genreId = genre.id
                        };
                        MvDb.FavoriteGenres.InsertOnSubmit(fav);
                    }
                }

                MvDb.SubmitChanges();
                MessageBox.Show("You gave positive rating for this movie!");
            }
        }

       
    }
}