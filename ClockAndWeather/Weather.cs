using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Net.Http;
using System.IO;

namespace ClockAndWeather
{
	public class WeatherDataWrapper
	{
		public async static Task<WeatherData> GetWeather()
		{
			//Get JSon string from the web
			var http = new HttpClient();
			var httpRespond = await http.GetAsync("http://api.openweathermap.org/data/2.5/weather?lat=51.913580&lon=4.999771&appid=8b6fbc4fab942058a2e1967b913460a4&lang=nl");
			var httpResult = await httpRespond.Content.ReadAsStringAsync();

			using (StreamWriter sw = File.AppendText(@"C:\Temp\Weather.log"))
			{
				await sw.WriteLineAsync(String.Format("{0}\t{1}",
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					httpResult));
			}

			//Translate data
			var serializer = new DataContractJsonSerializer(typeof(WeatherData));
			var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(httpResult));

			var weatherData = (WeatherData)serializer.ReadObject(memoryStream);

			return weatherData;
		}

	}

	public class Weather
	{
		#region Temperature
		/// <summary>
		/// Calculate temperature Kalvin to Celcius in 1 decimal.
		/// </summary>
		/// <param name="Kelvin"></param>
		/// <returns>Celcius</returns>
		public static double ToCelsius(double Kelvin)
		{
			return Math.Floor((Kelvin - 273.15) * 10) / 10;
		}
		#endregion

		#region Wind
		/// <summary>
		/// Convert wind speed to scale of Beaufort.
		/// </summary>
		/// <param name="WindSpeed"></param>
		/// <returns></returns>
		public static int ConvertWindSpeedToBeaufort(double WindSpeed)
		{
			if (WindSpeed <= 0.2) { return 0; }
			if (WindSpeed <= 1.5) { return 1; }
			if (WindSpeed <= 3.3) { return 2; }
			if (WindSpeed <= 5.4) { return 3; }
			if (WindSpeed <= 7.9) { return 4; }
			if (WindSpeed <= 10.7) { return 5; }
			if (WindSpeed <= 13.8) { return 6; }
			if (WindSpeed <= 17.1) { return 7; }
			if (WindSpeed <= 20.7) { return 8; }
			if (WindSpeed <= 24.4) { return 9; }
			if (WindSpeed <= 28.4) { return 10; }
			if (WindSpeed <= 32.6) { return 11; }
			return 12;
		}

		/// <summary>
		/// Convert Beaufort to short description of the wind.
		/// </summary>
		/// <param name="WindBeaufort"></param>
		/// <returns></returns>
		public static string ConvertWindBeaufortToName(int WindBeaufort)
		{
			string[] WindName = new string[13] {
				"Wind stil",
				"Flauw en stil",
				"Zwakke wind",
				"Lichte koelte",
				"Matige koelte",
				"Vrij krachtige wind",
				"Krachtige wind",
				"Harde wind",
				"Stormachtige wind",
				"Storm",
				"Zware storm",
				"Zeer zware storm",
				"Orkaan"};
			return WindName[WindBeaufort];
		}

		/// <summary>
		/// Convert Beaufort to short description of the wind.
		/// </summary>
		/// <param name="WindBeaufort"></param>
		/// <returns></returns>
		public static string ConvertWindBeaufortToName(double WindSpeed)
		{
			return ConvertWindBeaufortToName(ConvertWindSpeedToBeaufort(WindSpeed));
		}

		/// <summary>
		/// Convert Beaufort to description of the wind.
		/// </summary>
		/// <param name="WindBeaufort"></param>
		/// <returns></returns>
		public static string ConvertWindBeaufortToDescription(int WindBeaufort)
		{
			string[] WindDescription = new string[13] {
				"Rook stijgt recht of bijna recht omhoog.",
				"Windrichting goed herkenbaar aan rookpluimen.",
				"Bladeren beginnen te ritselen en windvanen kunnen gaan bewegen. Wind begint merkbaar te worden in het gelaat.",
				"Bladeren en twijgen zijn voortdurend in beweging.",
				"Kleine takken beginnen te bewegen. Stof en papier beginnen van de grond op te dwarrelen.",
				"Kleine bebladerde takken maken zwaaiende bewegingen. Er vormen zich gekuifde golven op meren en kanalen.",
				"Grote takken bewegen. Paraplu’s kunnen slechts met moeite worden vastgehouden.",
				"Gehele bomen bewegen. De wind is hinderlijk wanneer men er tegen in loopt.",
				"Twijgen breken af. Fietsen en lopen wordt bemoeilijkt.",
				"Lichte schade aan gebouwen. Schoorsteenkappen en dakpannen worden afgerukt.",
				"Ontwortelde bomen. Aanzienlijke schade aan gebouwen enz. Komt boven land zelden voor.",
				"Uitgebreide schade.",
				"Komt boven land zeer zelden voor."};
			return WindDescription[WindBeaufort];
		}

		/// <summary>
		/// Convert Beaufort to description of the wind.
		/// </summary>
		/// <param name="WindBeaufort"></param>
		/// <returns></returns>
		public static string ConvertWindBeaufortToDescription(double WindSpeed)
		{
			return ConvertWindBeaufortToDescription(ConvertWindSpeedToBeaufort(WindSpeed));
		}

		/// <summary>
		/// Convert direction to direction name.
		/// </summary>
		/// <param name="Direction"></param>
		/// <returns></returns>
		public static string ConvertDirectionToName(int Direction)
		{
			string[] DirectionName = new string[16]
			{
				"N", "NNO", "NO", "ONO",
				"O", "OZO", "ZO", "ZZO",
				"Z", "ZZW", "ZW", "WZW",
				"W", "WNW", "NW", "NNW"
			};
			int direction = (int)Math.Round(((float)Direction / 22.5));
			if (direction == 16) { direction = 0; }
			return DirectionName[direction];
		}
		#endregion

		#region Time
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
	}//Weather

	#region WeatherData
	[DataContract]
	public class Coord
	{
		[DataMember]
		public int lon { get; set; }

		[DataMember]
		public double lat { get; set; }
	}


	[DataContract]
	public class WeatherI
	{
		[DataMember]
		public int id { get; set; }

		[DataMember]
		public string main { get; set; }

		[DataMember]
		public string description { get; set; }

		[DataMember]
		public string icon { get; set; }
	}


	[DataContract]
	public class Main
	{
		[DataMember]
		public double temp { get; set; }

		[DataMember]
		public int pressure { get; set; }

		[DataMember]
		public int humidity { get; set; }

		[DataMember]
		public double temp_min { get; set; }

		[DataMember]
		public double temp_max { get; set; }
	}


	[DataContract]
	public class Wind
	{
		[DataMember]
		public double speed { get; set; }

		[DataMember]
		public int deg { get; set; }
	}


	[DataContract]
	public class Clouds
	{
		[DataMember]
		public int all { get; set; }
	}


	[DataContract]
	public class Sys
	{
		[DataMember]
		public int type { get; set; }

		[DataMember]
		public int id { get; set; }

		[DataMember]
		public double message { get; set; }

		[DataMember]
		public string country { get; set; }

		[DataMember]
		public int sunrise { get; set; }

		[DataMember]
		public int sunset { get; set; }
	}


	[DataContract]
	public class WeatherData
	{
		[DataMember]
		public Coord coord { get; set; }

		[DataMember]
		public List<WeatherI> weather { get; set; }

		[DataMember]
		public string @base { get; set; }

		[DataMember]
		public Main main { get; set; }

		[DataMember]
		public int visibility { get; set; }

		[DataMember]
		public Wind wind { get; set; }

		[DataMember]
		public Clouds clouds { get; set; }

		[DataMember]
		public int dt { get; set; }

		[DataMember]
		public Sys sys { get; set; }

		[DataMember]
		public int id { get; set; }

		[DataMember]
		public string name { get; set; }

		[DataMember]
		public int cod { get; set; }
	}
	#endregion
}
