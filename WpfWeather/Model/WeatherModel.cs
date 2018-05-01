using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWeather.Model
{
	public class WeatherData
	{
		public DateTime StationDate { get; set; }
		public string StationName { get; set; }
		public DateTime Sunrise { get; set; }
		public DateTime Sunset { get; set; }

		public double CurrentTemperature { get; set; }
		public double MinimumTemperature { get; set; }
		public double MaximumTemperature { get; set; }

		public double WindStrength { get; set; }
		public int WindDirection { get; set; }

		public int Humidity { get; set; }
		public int Pressure { get; set; }

		public int Visibility { get; set; }
		public int CloudsCover { get; set; }

		public WeatherData()
		{
		}

		public WeatherData(WeatherInternetData iw)
		{
			Initializer(iw);
		}

		internal void Initializer(WeatherInternetData iw)
		{
			StationDate = ConvertUnixTimeToDate(iw.dt);
			StationName = iw.name;

			Sunrise = ConvertUnixTimeToDate(iw.sys.sunrise);
			Sunset = ConvertUnixTimeToDate(iw.sys.sunset);

			CurrentTemperature = KelvinToCelsius(iw.main.temp);
			MinimumTemperature = KelvinToCelsius(iw.main.temp_min);
			MaximumTemperature = KelvinToCelsius(iw.main.temp_max);

			WindStrength = iw.wind.speed;
			WindDirection = iw.wind.deg;

			Humidity = iw.main.humidity;
			Pressure = iw.main.pressure;

			Visibility = iw.visibility;
			CloudsCover = iw.clouds.all;
		}

		#region Time conversion
		/// <summary>
		/// Convert Unix timestamp (number of seconds since epoch to date/time.
		/// </summary>
		/// <param name="unixTimeStamp"></param>
		/// <returns></returns>
		public static DateTime ConvertUnixTimeToDate(int unixTime)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			return dateTime.AddSeconds(unixTime).ToLocalTime();
		}
		#endregion

		#region Temperature conversion
		/// <summary>
		/// Calculate temperature Kalvin to Celcius in 1 decimal.
		/// </summary>
		/// <param name="Kelvin"></param>
		/// <returns>Celcius</returns>
		public static double KelvinToCelsius(double Kelvin)
		{
			return Math.Floor((Kelvin - 273.15) * 10) / 10;
		}
		#endregion

	}
}
