using Newtonsoft.Json;
using SunClouds.Models;
using SunClouds.ModelView;
using SunClouds.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SunClouds
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Инициализация таймера.
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            //Проверка на первичный запуск (если город был выбран, то запускает основное окно).
            if(Settings.Default.CurrentCity != "")
            {
                this.Hide();
                View.MainSunClouds window = new View.MainSunClouds();
                window.Show();
            }
        }
        //Таймер, который проверяет текущее время и сменяет тему
        private void Timer_Tick(object sender, EventArgs e)
        {
            int time = DateTime.Now.Hour;
            if(time > 0 && time < 4)
            {
                App.Theme = "NightTheme";
            }
            else if(time > 12 && time < 17)
            {
                App.Theme = "DayTheme";
            }
            else
            {
                App.Theme = "MorningEvening";
            }
        }
        //Кнопка, очищающая TextBox
        private void ClearCityName_Click(object sender, RoutedEventArgs e)
        {
            CityName.Clear();
        }
        //Скрытие кнопок закрытия, увеличения и сворачивания окна, если окно слишком маленькое
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(SunCloudsWindow.ActualHeight < 250 && SunCloudsWindow.ActualWidth < 400)
            {
                CloseBtn.Visibility = Visibility.Hidden;
                FullSizeBtn.Visibility = Visibility.Hidden;
                HideBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                CloseBtn.Visibility = Visibility.Visible;
                FullSizeBtn.Visibility = Visibility.Visible;
                HideBtn.Visibility = Visibility.Visible;
            }
        }
        //Закрытие приложения
        private void CloseBtn_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        //Метод, позволяющий менять цвета элемента при покидании курсора элемента
        private void SpeacialBtn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Label obj = e.Source as Label;
            obj.Foreground = (SolidColorBrush)this.TryFindResource("Buttons");
        }
        //Метод, позволяющий менять цвет элемента при наведении курсора на элемент
        private void SpecialBtn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Label obj = e.Source as Label;
            obj.Foreground = (SolidColorBrush)this.TryFindResource("Hover");
        }
        //Метод, скрывающий окно при нажатии
        private void HideBtn_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        //Метод, делающий окно полноэкранным/возврающим в исходное состояние
        private void FullSizeBtn_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized) this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }
        //Кнопка, получающая информацию о городе и открывающее основное окно, добавление города в избранное по умолчанию.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApiHelper api = new ApiHelper();
                WeatherModel.Root obj = api.GetData(CityName.Text);
                FavoriteCitiesModel city = new FavoriteCitiesModel() { city = CityName.Text, lan = obj.location.lat.ToString() + "с.ш.", lon = obj.location.lon.ToString() + "в.д." };
                List<FavoriteCitiesModel> cities = new List<FavoriteCitiesModel>();
                cities.Add(city);
                var json = JsonConvert.SerializeObject(cities, Formatting.Indented);
                FileStream fs = new FileStream("cities.json", FileMode.OpenOrCreate);
                using (StreamWriter file = new StreamWriter(fs))
                {
                    file.Write(json);
                }
                this.Hide();
                Settings.Default.CurrentCity = CityName.Text;
                Settings.Default.Save();
                View.MainSunClouds window = new View.MainSunClouds();
                window.Show();
            }
            catch
            {
                MessageBox.Show("Не получилось получить данные. Убедитесь, что данные были введены верно.", "Getting data exception.");
            }
        }
    }
}
