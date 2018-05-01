using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingWithASync
{
	class Program
	{
		static void Main(string[] args)
		{
			Log.WriteAsync("Starting program");
			RunAsync();
		}

		public static async Task RunAsync()
		{
			await Log.WriteAsync("Start Run()");
		}
	}

	class Log
	{
		public static async Task WriteAsync(string Message)
		{
			Console.WriteLine(Message);
			using (StreamWriter sw = File.AppendText(@"F:\Temp\WorkingWithASync.log"))
			{
				await sw.WriteLineAsync(string.Format("{0} {1}",
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					Message));
			}
		}

		/*
		public static async Task Write(string Message)
		{
			using (StreamWriter sw = File.AppendText(@"C:\Temp\ClockAndWeather.log"))
			{
				await sw.WriteLineAsync(string.Format("{0} {1}",
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					Message));
			}
		}

		 */
	}
}
