using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfWeather.Model;
using WpfWeather.Logic;

namespace WpfWeather.Logic
{
	public class LogicViewModel
	{
		//DisplayData Weather = new DisplayData();

		public async Task CollectData()
		{
			WeatherInternetData wid = await InternetModelControler.GetInternetWeatherAsync();
			//Weather = DisplayData(wid);
		}
	}
}
