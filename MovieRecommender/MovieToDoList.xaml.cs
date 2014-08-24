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

namespace MovieRecommender
{
    public partial class MovieToDoList : PhoneApplicationPage
    {
        private const string strConnectionString = @"DataSource=isostore:/MoviesDB.sdf";
        public MovieToDoList()
        {
            InitializeComponent();
            
            CheckToDoList();
        }

        public void CheckToDoList()
        {

            using (MovieDataContext Empdb = new MovieDataContext(strConnectionString))
            {
                if (Empdb.DatabaseExists())
                {
                    IList<ToDoList> toDoList = null;
                    IQueryable<ToDoList> EmpQuery = from ToDoList mv in Empdb.ToDoList select mv;
                    toDoList = EmpQuery.ToList();
                    List<Movie> movies = new List<Movie>();
                    if (toDoList != null)
                    {
                        foreach (ToDoList list in toDoList)
                        {
                            Movie mov = new Movie(-1);
                            mov.movieImage = list.movieImage;
                            mov.movieTitle = list.movieTitle;
                            movies.Add(mov);
                            
                        }
                        toDoListMovies.ItemsSource = movies;
                    }
                    

                }
            }
        }
    }
}