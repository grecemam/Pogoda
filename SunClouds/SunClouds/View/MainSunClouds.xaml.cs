using Newtonsoft.Json;
using SunClouds.Models;
using SunClouds.ModelView;
using SunClouds.Resources;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SunClouds.View
{
    /// <summary>
    /// Логика взаимодействия для MainSunClouds.xaml
    /// </summary>
    public partial class MainSunClouds : Window
    {
        //Скрытый список городов, используемый для сери/десе-реализации JSON.
        private List<FavoriteCitiesModel> cityList = new List<FavoriteCitiesModel>();
        public MainSunClouds()
        {
            InitializeComponent();
            DataContext = new[] { new SunCloudsModelView() };
            MainCity.Text = Properties.Settings.Default.CurrentCity;
            if (Properties.Settings.Default.IsCelsius == true) Cel_RB.IsChecked = true;
            else Far_RB.IsChecked = true;

            Deserialize();
        }
        //Десериализация JSON
        private void Deserialize()
        {
            using (StreamReader reader = new StreamReader("cities.json"))
            {
                string text = reader.ReadToEnd();
                cityList = JsonConvert.DeserializeObject<List<FavoriteCitiesModel>>(text);
                //Динамическое создание UserControl на Grid с заполненной информацией 
                for (int i = 0; i < cityList.Count; i++)
                {
                    FavCityCard card = new FavCityCard();
                    card.CityName = cityList[i].city;
                    card.Coords = cityList[i].lan + " " + cityList[i].lon;
                    card.ClickB1 += SelectCard;
                    card.ClickB2 += DeleteCard;
                    card.Margin = new Thickness(4);
                    PanelOfCities.Children.Add(card);
                }
            }
        }
        //Отключение приложения
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
        //Скрытие кнопок закрытия, увеличения и скрытия окна при слишком маленьком размере окна
        private void MainSunCloudsWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MainSunCloudsWindow.ActualWidth < 550)
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
        //Переключение графиков
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TempChart.Visibility = Visibility.Hidden;
            FeelChart.Visibility = Visibility.Visible;
            PressureChart.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TempChart.Visibility = Visibility.Visible;
            FeelChart.Visibility = Visibility.Hidden;
            PressureChart.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TempChart.Visibility = Visibility.Hidden;
            FeelChart.Visibility = Visibility.Hidden;
            PressureChart.Visibility = Visibility.Visible;
        }
        //Переключение правой части основного окна между Grid настроек и погоды
        private void Label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RightPanelMain.Visibility = Visibility.Visible;
            RightPanelSettings.Visibility = Visibility.Hidden;
        }

        private void Label_MouseDoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RightPanelMain.Visibility = Visibility.Hidden;
            RightPanelSettings.Visibility = Visibility.Visible;
        }
        //Очистка TextBox
        private void ClearCityName_Click(object sender, RoutedEventArgs e)
        {
            MainCity.Text = "";
        }
        //Сохранение настроек и их применение (Смена города, вид отображения температуры)
        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            if(MainCity.Text != "") Properties.Settings.Default.CurrentCity = MainCity.Text;
            if (Cel_RB.IsChecked == true) Properties.Settings.Default.IsCelsius = true;
            else Properties.Settings.Default.IsCelsius = false;
            Properties.Settings.Default.Save();

            View.MainSunClouds window = new MainSunClouds();
            window.Show();
            this.Close();
        }
        //Выбор карты, отображение температуры выбранной карточки
        private void SelectCard(object sender, RoutedEventArgs e)
        {
            var obj = sender as FavCityCard;
            Properties.Settings.Default.CurrentCity = obj.CityName;
            Properties.Settings.Default.Save();

            View.MainSunClouds window = new MainSunClouds();
            window.Show();
            this.Close();
        }
        //Удаление карточки
        private void DeleteCard(object sender, RoutedEventArgs e)
        {
            var obj = sender as FavCityCard;
            for (int i = cityList.Count - 1; i >= 0; i--)
            {
                if (cityList[i].city == obj.CityName)
                {
                    cityList.RemoveAt(i);
                }
            }
            var json = JsonConvert.SerializeObject(cityList, Formatting.Indented);
            File.Delete("cities.json");
            FileStream fs = new FileStream("cities.json", FileMode.OpenOrCreate);
            using (StreamWriter file = new StreamWriter(fs))
            {
                file.Write(json);
            }
            PanelOfCities.Children.Clear();
            Deserialize();
        }
        //Очистка TextBox
        private void ClearAddCity_Click(object sender, RoutedEventArgs e)
        {
            AddCity.Text = "";
        }
        //Проверка наличия города и добавление его в избранное
        private void AddCity_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApiHelper api = new ApiHelper();
                WeatherModel.Root obj = api.GetData(AddCity.Text);
                FavoriteCitiesModel city = new FavoriteCitiesModel() { city = AddCity.Text, lan = obj.location.lat.ToString() + "с.ш.", lon = obj.location.lon.ToString() + "в.д." };
                cityList.Add(city);
                var json = JsonConvert.SerializeObject(cityList, Formatting.Indented);
                File.Delete("cities.json");
                FileStream fs = new FileStream("cities.json", FileMode.OpenOrCreate);
                using (StreamWriter file = new StreamWriter(fs))
                {
                    file.Write(json);
                }
                PanelOfCities.Children.Clear();
                Deserialize();
            }
            catch // Если город не был найден, выйдет ошибка
            {
                MessageBox.Show("Не удалось добавить указанный город. Убедитесь, что данные были введены верно и повторите попытку.", "Add Exception.");
            }
        }
    }
}
