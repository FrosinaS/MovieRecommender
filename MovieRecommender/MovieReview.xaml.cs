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
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

namespace MovieRecommender
{
    public partial class MovieReview : PhoneApplicationPage
    {
        private const string strConnectionString = @"DataSource=isostore:/MoviesDB.sdf";
        Movie movie;
        long movieID;
        public MovieReview()
        {
            InitializeComponent();
            movieID = 0;
            movie = new Movie(-1);
            var k = PhoneApplicationService.Current.State["movieId"];
            movieID = Convert.ToInt64(k);
            movie.movieId = movieID;
            int a = Convert.ToInt32(PhoneApplicationService.Current.State["which"]);
            if (a == 0)
            {
                addToDoList.Visibility = Visibility.Collapsed;
            }
            else if (a == 1)
            {
                rateDownBtn.Visibility = Visibility.Collapsed;
                rateUpBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                addToDoList.Visibility = Visibility.Visible;
                rateDownBtn.Visibility = Visibility.Visible;
                rateUpBtn.Visibility = Visibility.Visible;
            }

            
            GetMovie();
            CheckVoting();
            
        }




        public void CheckVoting()
        {
           
            using (MovieDataContext Empdb = new MovieDataContext(strConnectionString))
            {
                if (Empdb.DatabaseExists())
                {
                    IList<ToDoList> toDoList = null;
                    IQueryable<ToDoList> EmpQuery = from ToDoList mv in Empdb.ToDoList where mv.movieId == movieID select mv;
                    toDoList = EmpQuery.ToList();
                    foreach (ToDoList lst in toDoList)
                    {
                        if (lst.movieId == movie.movieId)
                        {
                            addToDoList.Content = "Remove from my watch list";
                        }
                        else
                        {
                            addToDoList.Content = "Add to my watch list";
                        }
                    }

                    IList<RatedMovies> ratedMovies = null;
                    IQueryable<RatedMovies> Query = from RatedMovies mv in Empdb.RatedMovies where mv.movieId == movieID select mv;
                    ratedMovies = Query.ToList();
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

        private void addToDoList_click(object sender, RoutedEventArgs e)
        {
            createDb();

            using (MovieDataContext MvDb = new MovieDataContext(strConnectionString))
            {
               
                if (addToDoList.Content.ToString() == "Remove from my watch list")
                {
                    ToDoList movie = null;
                    IQueryable<ToDoList> EmpQuery = from ToDoList mv in MvDb.ToDoList where mv.movieId == movieID select mv;
                    movie= EmpQuery.FirstOrDefault();
                    MvDb.ToDoList.DeleteOnSubmit(movie);
                    MvDb.SubmitChanges();
                    addToDoList.Content = "Add to my watch list";
                    MessageBox.Show("You removed this movie from your watch list!");
                }
                else
                {

                    ToDoList newMovie = new ToDoList
                        {
                            movieId = movie.movieId,
                            movieTitle = movie.movieTitle,
                            movieImage = movie.movieImage
                        };
                    MvDb.ToDoList.InsertOnSubmit(newMovie);
                    MvDb.SubmitChanges();
                    addToDoList.Content = "Remove from my watch list";
                    MessageBox.Show("You added this movie to your watch list!");
                }
                
               
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
                rateDownBtn.IsEnabled = false;
                rateUpBtn.IsEnabled = true;
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

                    
                    IQueryable<FavoriteGenres> Query = from FavoriteGenres mv in MvDb.FavoriteGenres where mv.genreId == genre.id select mv;
                    FavoriteGenres gen = Query.FirstOrDefault();
                    
                     if (gen == null)
                        {
                            FavoriteGenres fav = new FavoriteGenres
                            {
                                genreId = genre.id
                            };
                            MvDb.FavoriteGenres.InsertOnSubmit(fav);
                            MvDb.SubmitChanges();
                        }
                    }
                }

                
                rateUpBtn.IsEnabled = false;
                rateDownBtn.IsEnabled = true;
                MessageBox.Show("You gave positive rating for this movie!");
            }
        }



        
      

       

     
        }

       
    
