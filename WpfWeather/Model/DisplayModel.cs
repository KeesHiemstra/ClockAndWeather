using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfWeather.Logic;

namespace WpfWeather.Model
{
	public class DisplayData : WeatherData
	{
		public string CurrentTemperatureCelcius { get { return string.Format("{0} ºC", CurrentTemperature); } }
		public string MinimumTemperatureCelcius { get { return string.Format("{0} ºC", MinimumTemperature); } }
		public string MaximumTemperatureCelcius { get { return string.Format("{0} ºC", MaximumTemperature); } }

		public DisplayData(WeatherInternetData iw)
		{
			Initializer(iw);
		}
	}
}
