using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SunClouds.Resources
{
    /// <summary>
    /// Логика взаимодействия для Card.xaml
    /// </summary>
    public partial class Card : UserControl
    {
        //Привязка данных из внешних окон
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("time", typeof(string), typeof(Card));
        public static readonly DependencyProperty FeelProperty =
            DependencyProperty.Register("feels", typeof(string), typeof(Card));
        public static readonly DependencyProperty HumidityProperty =
            DependencyProperty.Register("humidity", typeof(string), typeof(Card));
        public static readonly DependencyProperty TempProperty =
            DependencyProperty.Register("temp", typeof(string), typeof(Card));
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("img", typeof(string), typeof(Card));
        public Card()
        {
            InitializeComponent();
        }
        //Геттеры и сеттеры, основанные на методах DependencyProperty
        public string time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        
        public string img 
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public string feels
        {
            get { return (string)GetValue(FeelProperty); }
            set { SetValue(FeelProperty, value); }
        }
        public string humidity 
        {
            get { return (string)GetValue(HumidityProperty); }
            set { SetValue(HumidityProperty, value); }
        }
        public string temp 
        {
            get { return (string)GetValue(TempProperty); }
            set { SetValue(TempProperty, value); }
        }
    }
}
