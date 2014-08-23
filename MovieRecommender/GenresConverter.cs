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
using System.Windows.Data;
using System.Collections.Generic;
using System.Text;

namespace MovieRecommender
{
    public class GenresConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<Genre> genres = value as List<Genre>;
            StringBuilder sb = new StringBuilder();
            foreach (Genre genre in genres)
            {
                sb.Append(genre.name + "\n");
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
