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
                    IQueryable<ToDoList> EmpQuery = from ToDoList mv in Empdb.ToDoList where mv.movieTitle == movie.movieTitle select mv;
                    toDoList = EmpQuery.ToList();
                    if (toDoList != null)
                    {
                        addToDoList.IsEnabled = false;
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
                
                    ToDoList newMovie = new ToDoList
                    {
                        movieTitle = movie.movieTitle,
                        movieImage = movie.movieImage
                    };
                    MvDb.ToDoList.InsertOnSubmit(newMovie);
                    MvDb.SubmitChanges();
                    MessageBox.Show("You added this movie to your to do list!");
                
                
               
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



        
      

       

     
        }

       
    }
