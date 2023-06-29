using LiveCharts;
using SunClouds.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;

namespace SunClouds.ModelView
{
    class SunCloudsModelView : INotifyPropertyChanged
    {
        ApiHelper helper = new ApiHelper();
        public SunCloudsModelView()
        {
            WeatherModel.Root obj = helper.GetData(Properties.Settings.Default.CurrentCity);
            CityWeather = obj;
            image = GetImages();
            temperatureCards = GetTemperature();
            feelsCards = GetFeels();
            hoursCards = GetHours();
        }
        //Получение модели, содержащее информацию о городе, температуре
        private WeatherModel.Root cityWeather;
        public WeatherModel.Root CityWeather
        {
            get { return cityWeather; }
            set
            {
                cityWeather = value;
                OnPropertyChanged("CityWeather");
            }
        }
        //Получение температуры на данный момент
        public string currentTemp { get => GetCurrentTemp(); }
        private string GetCurrentTemp()
        {
            if (Properties.Settings.Default.IsCelsius) return cityWeather.current.temp_c.ToString() + "°";
            else return cityWeather.current.temp_f.ToString() + "°";
        }
        //Получение ощущаемой температуры на данный момент
        public string currentFeel { get => GetCurrentFeel(); }
        private string GetCurrentFeel()
        {
            if (Properties.Settings.Default.IsCelsius) return cityWeather.current.feelslike_c.ToString() + "°";
            else return cityWeather.current.feelslike_f.ToString() + "°";
        }
        //Получение минимальной температуры на данный момент
        public string currentMin { get => GetCurrentMin(); }
        private string GetCurrentMin()
        {
            if (Properties.Settings.Default.IsCelsius) return cityWeather.forecast.forecastday[0].day.mintemp_c.ToString() + "°";
            else return cityWeather.forecast.forecastday[0].day.mintemp_f.ToString() + "°";
        }
        //Получение максимальной температуры на данный момент
        public string currentMax { get => GetCurrentMax(); }

        private string GetCurrentMax()
        {
            if (Properties.Settings.Default.IsCelsius) return cityWeather.forecast.forecastday[0].day.maxtemp_c.ToString() + "°";
            else return cityWeather.forecast.forecastday[0].day.maxtemp_f.ToString() + "°";
        }
        //Список для нижней части графиков
        private List<string> hours = new List<string>() { "0:00", "1:00", "2:00", "3:00", "4:00", "5:00", "6:00", "7:00", "8:00", "9:00",
            "10:00","11:00","12:00","13:00","14:00","15:00","16:00","17:00","18:00","19:00","20:00","21:00","22:00","23:00" };
        public List<string> Hours { get { return hours; } }
        //Поля, привязанные к графикам, получающие информацию от модели
        public ChartValues<double> Temperature
        {
            get => GetTemp();
        }

        public ChartValues<double> FeelTemp
        {
            get => GetFeelTemp();
        }
        public ChartValues<double> Pressure
        {
            get => GetPressure();
        }
        //Событие при изменении данных
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        //Метод, получающий информацию о погоде (исходя из настроек (фаренгейт/цельсий))
        private ChartValues<double> GetTemp()
        {
            ChartValues<double> temp = new ChartValues<double>();
            for (int i = 0; i < 24; i++)
            {
                if (Properties.Settings.Default.IsCelsius) temp.Add(cityWeather.forecast.forecastday[0].hour[i].temp_c);
                else temp.Add(cityWeather.forecast.forecastday[0].hour[i].temp_f);
            }
            return temp;
        }
        //Метод, получающий информацию об ощущаемой погоде (исходя из настроек (фаренгейт/цельсий))
        private ChartValues<double> GetFeelTemp()
        {
            ChartValues<double> temp = new ChartValues<double>();
            for (int i = 0; i < 24; i++)
            {
                if (Properties.Settings.Default.IsCelsius) temp.Add(cityWeather.forecast.forecastday[0].hour[i].feelslike_c);
                else temp.Add(cityWeather.forecast.forecastday[0].hour[i].feelslike_f);
            }
            return temp;
        }
        //Метод, получающий информацию о давлении
        private ChartValues<double> GetPressure()
        {
            ChartValues<double> temp = new ChartValues<double>();
            for (int i = 0; i < 24; i++)
            {
                temp.Add(cityWeather.forecast.forecastday[0].hour[i].pressure_mb);
            }
            return temp;
        }
        //Поля для карточек в основном меню.
        private string[] image;
        public string[] Image { get { return image; } }

        private string[] temperatureCards;
        public string[] TemperatureCards { get { return temperatureCards; } }

        private string[] feelsCards;
        public string[] FeelsCards { get { return feelsCards; } }

        private string[] hoursCards;
        public string[] HoursCards { get { return hoursCards; } }

        private string[] GetHours()
        {
            string[] temp = new string[24];
            for (int i = 0; i < temp.Length; i++)
            {
                string[] words = cityWeather.forecast.forecastday[0].hour[i].time.Split(' ');
                temp[i] = words[1];
            }
            return temp;
        }

        private string[] GetFeels()
        {
            string[] temp = new string[24];
            for (int i = 0; i < temp.Length; i++)
            {
                if (Properties.Settings.Default.IsCelsius) temp[i] = cityWeather.forecast.forecastday[0].hour[i].feelslike_c.ToString() + "°";
                else temp[i] = cityWeather.forecast.forecastday[0].hour[i].feelslike_f.ToString() + "°";
            }
            return temp;
        }

        private string[] GetTemperature()
        {
            string[] temp = new string[24];
            for (int i = 0; i < temp.Length; i++)
            {
                if (Properties.Settings.Default.IsCelsius) temp[i] = cityWeather.forecast.forecastday[0].hour[i].temp_c.ToString() + "°";
                else temp[i] = cityWeather.forecast.forecastday[0].hour[i].temp_f.ToString() + "°";
            }
            return temp;
        }
        //Метод, позволяющий заполнить поле с изображением для карточек исходя из кода погоды.
        private string[] GetImages()
        {
            string[] array = new string[24];
            for (int i = 0; i < array.Length; i++)
            {
                switch (cityWeather.forecast.forecastday[0].hour[i].condition.code)
                {
                    case 1000:
                        array[i] = "pack://siteoforigin:,,,/Resources/Sunny.png";
                        break;
                    case 1003:
                    case 1006:
                        array[i] = "pack://siteoforigin:,,,/Resources/Cloudy.png";
                        break;
                    case 1063:
                    case 1180:
                    case 1183:
                    case 1186:
                    case 1189:
                    case 1192:
                    case 1195:
                        array[i] = "pack://siteoforigin:,,,/Resources/Rainy.png";
                        break;
                    case 1117:
                        array[i] = "pack://siteoforigin:,,,/Resources/Blizzard.png";
                        break;
                    case 1210:
                    case 1213:
                    case 1216:
                    case 1219:
                    case 1222:
                    case 1225:
                        array[i] = "pack://siteoforigin:,,,/Resources/Snow.png";
                        break;
                    case 1273:
                    case 1276:
                    case 1279:
                    case 1282:
                        array[i] = "pack://siteoforigin:,,,/Resources/Thunderstorm.png";
                        break;
                    case 1135:
                    case 1147:
                        array[i] = "pack://siteoforigin:,,,/Resources/Wind.png";
                        break;
                    default:
                        array[i] = "pack://siteoforigin:,,,/Resources/Downpour.png";
                        break;
                }
            }
            return array;
        }
        //Поля для левой панели приложения
        private string[] leftTemp = new string[4];
        private string[] leftFeel = new string[4];
        public string[] leftImg
        {
            get => getLeftImage();
        }
        public string[] LeftPanelText
        {
            get => getLeftPanel();
        }
        //Получение информации исходя из текущего времени
        private string[] getLeftPanel()
        {
            int currentHour = DateTime.Now.Hour;
            switch (currentHour)
            {
                case 21:
                    for (int i = 0; i < 3; i++)
                    {
                        leftTemp[i] = temperatureCards[currentHour + i];
                        leftFeel[i] = feelsCards[currentHour + i];
                    }
                    leftTemp[3] = temperatureCards[0];
                    leftFeel[3] = feelsCards[0];
                    break;
                case 22:
                    for (int i = 0; i < 2; i++)
                    {
                        leftTemp[i] = temperatureCards[currentHour + i];
                        leftFeel[i] = feelsCards[currentHour + i];
                    }
                    for (int i = 2; i < 4; i++)
                    {
                        leftTemp[i] = temperatureCards[0];
                        leftFeel[i] = feelsCards[0];
                    }
                    break;
                case 23:
                    for (int i = 1; i < 4; i++)
                    {
                        leftTemp[i] = temperatureCards[0];
                        leftFeel[i] = feelsCards[0];
                    }
                    leftTemp[0] = temperatureCards[currentHour];
                    leftFeel[0] = feelsCards[currentHour];
                    break;
                default:
                    for (int i = 0; i < 4; i++)
                    {
                        leftTemp[i] = temperatureCards[currentHour + i];
                        leftFeel[i] = feelsCards[currentHour + i];
                    }
                    break;
            }
            string[] temp = new string[4];
            for (int i = 0; i < 4; i++)
            {
                temp[i] = leftTemp[i] + ".\nОщущается как " + leftFeel[i];
            }
            return temp;
        }
        private string[] getLeftImage()
        {
            int currentHour = DateTime.Now.Hour;
            string[] temp = new string[4];
            switch (currentHour)
            {
                case 21:
                    for (int i = 0; i < 3; i++)
                    {
                        temp[i] = image[currentHour + i];
                    }
                    temp[3] = image[0];
                    break;
                case 22:
                    for (int i = 0; i < 2; i++)
                    {
                        temp[i] = image[currentHour + i];
                    }
                    for (int i = 2; i < 4; i++)
                    {
                        temp[i] = image[0];
                    }
                    break;
                case 23:
                    for (int i = 1; i < 4; i++)
                    {
                        temp[i] = image[0];
                    }
                    temp[0] = image[currentHour];
                    break;
                default:
                    for (int i = 0; i < 4; i++)
                    {
                        temp[i] = image[currentHour + i];
                    }
                    break;
            }
            return temp;
        }
    }
}