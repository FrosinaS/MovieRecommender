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
using System.Data.Linq.Mapping;
using System.Data.Linq;
 

namespace MovieRecommender
{
    [Table]
    public class RatedMovies
    {
        [Column(IsPrimaryKey = true, IsDbGenerated=true, CanBeNull = false)]
        public int id
        {
            get;
            set;
        }
        [Column(IsDbGenerated = false)]
        public long movieId
        {
            get;
            set;
        }
        [Column(CanBeNull = false, IsDbGenerated=false)]
        public bool movieVote
        {
            get;
            set;
        }
    }
    [Table]
    public class FavoriteGenres
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int id
        {
            get;
            set;
        }
        [Column(IsDbGenerated = false)]
        public int genreId
        {
            get;
            set;
        }
    }
    [Table]
    public class ToDoList
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int id
        {
            get;
            set;
        }
        [Column(IsDbGenerated = false)]
        public long movieId
        {
            get;
            set;
        }
        [Column(IsDbGenerated = false)]
        public String movieTitle
        {
            get;
            set;
        }
        [Column(CanBeNull = false, IsDbGenerated = false)]
        public String movieImage
        {
            get;
            set;
        }
    }

    public class MovieDataContext:DataContext
    {
        private const string DbConnectionString = @"DataSource=isostore:/MoviesDB.sdf";
        public MovieDataContext():this(DbConnectionString)
        {

         }
        public MovieDataContext(string connectionString):base(connectionString)
        {

        }
        public Table<RatedMovies> RatedMovies
        {
            get
            {
                return this.GetTable<RatedMovies>();
            }
        }
        public Table<FavoriteGenres> FavoriteGenres
        {
            get
            {
                return this.GetTable<FavoriteGenres>();
            }
        }
        public Table<ToDoList> ToDoList
        {
            get
            {
                return this.GetTable<ToDoList>();
            }
        }
       }
}
