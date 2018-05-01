using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ClockAndWeather
{
	public partial class MainWindow : Window
	{
		private async Task BuildScreenAsync()
		{
			await Log.Write("BuidScreenAsync()");
			DateTime RefreshTime = DateTime.Now;

			//RootObject ThisWeather = await WeatherInfo.GetWeather();
			WeatherData RefreshWeather = await WeatherDataWrapper.GetWeather();

			TemperatureTextBlock.Text = DisplayTemperature(RefreshWeather.main.temp);
			TemperatureMaxTextBlock.Text = DisplayTemperature(RefreshWeather.main.temp_max);
			TemperatureMinTextBlock.Text = DisplayTemperature(RefreshWeather.main.temp_min);

			PressureTextBlock.Text = DisplayPressure(RefreshWeather.main.pressure);
			HumidityTextBlock.Text = DisplayHumidity(RefreshWeather.main.humidity);

			SunRiseTextBlock.Text = DisplaySunRise(RefreshWeather.sys.sunrise);
			SunSetTextBlock.Text = DisplaySunSet(RefreshWeather.sys.sunset);

			WindBeaufortTextBlock.Text = DisplayBeafort(RefreshWeather.wind.speed);

			WindSpeedTextBlock.Text = DisplayWindSpeed(RefreshWeather.wind.speed);
			WindDescriptionTextBlock.Text = Weather.ConvertWindBeaufortToName(RefreshWeather.wind.speed);
			WindDescriptionTextBlock.ToolTip = Weather.ConvertWindBeaufortToDescription(RefreshWeather.wind.speed);

			WindDirectionTextBlock.Text = DisplayWindDirection(RefreshWeather.wind.deg);

			WeatherTextBlock.Text = RefreshWeather.weather[0].main;
			DescriptionTextBlock.Text = RefreshWeather.weather[0].description;

			WeatherTimeTextBlock.Text = WeatherStationTime(RefreshWeather.dt);
			WeatherLocationTextBlock.Text = RefreshWeather.name;

			if (DateTime.Now >= Weather.ConvertUnixTimeToDate(RefreshWeather.sys.sunrise) &&
				DateTime.Now <= Weather.ConvertUnixTimeToDate(RefreshWeather.sys.sunset))
				this.Background = Brushes.LightBlue;
			else
				this.Background = Brushes.LightGray;

		}

		private static async Task SayText(string Saying)
		{
			// Initialize a new instance of the SpeechSynthesizer.
			SpeechSynthesizer synth = new SpeechSynthesizer();

			// Configure the audio output. 
			synth.SetOutputToDefaultAudioDevice();

			// Speak a string asynchronously.
			synth.SpeakAsync(Saying);
		}

		/// <summary>
		/// Display tempurature with trailing ºC.
		/// </summary>
		/// <param name="Temperature"></param>
		/// <returns>string</returns>
		private static string DisplayTemperature(double Temperature)
		{
			return string.Format("{0} ºC",
				Weather.ToCelsius(Temperature));
		}

		/// <summary>
		/// Display pressure with trailing hPa.
		/// </summary>
		/// <param name="Pressure"></param>
		/// <returns>string</returns>
		private static string DisplayPressure(int Pressure)
		{
			return string.Format("{0} hPa",
				Pressure);
		}

		private static string DisplayHumidity(int Humidity)
		{
			return string.Format("{0} %",
				Humidity);
		}

		private static string DisplayWindSpeed(double WindSpeed)
		{
			return string.Format("{0} km/h",
				Math.Round(WindSpeed * 3.6));
		}

		private static string DisplayBeafort(double WindSpeed)
		{
			return string.Format("{0} bft",
				Weather.ConvertWindSpeedToBeaufort(WindSpeed));
		}

		private static string DisplayWindDirection(int WindDirection)
		{
			return string.Format("{0} / {1}",
				Weather.ConvertDirectionToName(WindDirection),
				WindDirection);
		}

		private static string DisplaySunRise(int SunRise)
		{
			return string.Format("{0} sun up",
				Weather.ConvertUnixTimeToDate(SunRise).ToString("dd HH:mm"));
		}

		private static string DisplaySunSet(int SunSet)
		{
			return string.Format("{0} sun down",
				Weather.ConvertUnixTimeToDate(SunSet).ToString("dd HH:mm"));
		}

		private static string WeatherStationTime(int StationTime)
		{
			DateTime stationTime = Weather.ConvertUnixTimeToDate(StationTime);
			return string.Format("Station time: {0} ({1} minutes ago)",
				stationTime.ToString("yyyy-MM-dd HH-mm"),
				(DateTime.Now - stationTime).Minutes);
		}
	}
}
