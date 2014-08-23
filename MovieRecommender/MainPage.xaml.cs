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

namespace MovieRecommender
{
    public partial class MainPage : PhoneApplicationPage
    {
        public List<Movie> movies;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            
        }

        private void rateMovies_click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri("/RateMovies.xaml", UriKind.Relative));
        }
        
    }
}