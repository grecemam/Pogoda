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
    /// Логика взаимодействия для FavCityCard.xaml
    /// </summary>
    public partial class FavCityCard : UserControl
    {
        //Создание ивентов для привязки команд.
        public static readonly RoutedEvent ClickB1Event = EventManager.RegisterRoutedEvent(
            "ClickB1", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FavCityCard));
        public static readonly RoutedEvent ClickB2Event = EventManager.RegisterRoutedEvent(
            "ClickB2", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FavCityCard));

        public event RoutedEventHandler ClickB1
        {
            add { AddHandler(ClickB1Event, value); }
            remove { RemoveHandler(ClickB1Event, value); }
        }

        public event RoutedEventHandler ClickB2
        {
            add { AddHandler(ClickB2Event, value); }
            remove { RemoveHandler(ClickB2Event, value); }
        }
        public FavCityCard()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        //Поля для манипуляции с элементами управления
        public string CityName { get; set; }
        public string Coords { get; set; }
        //Привязка новых команд
        void button1_Click(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(ClickB1Event));
        void button2_Click(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(ClickB2Event));
    }
}
