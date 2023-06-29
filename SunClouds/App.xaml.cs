using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SunClouds
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string theme;
        public static string Theme
        {
            get { return theme; }
            set
            { 
                theme = value;
                var dict = new ResourceDictionary { Source = new Uri($"/Resources/{value}.xaml", UriKind.Relative) };

                Current.Resources.MergedDictionaries.RemoveAt(0);
                Current.Resources.MergedDictionaries.Add(dict);
            }
        }
    }
}
