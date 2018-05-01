using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWeather.Logic
{
	public class Log
	{
		public static async Task Write(string Message)
		{
			using (StreamWriter sw = File.AppendText(@"C:\Temp\WpfWeather.log"))
			{
				await sw.WriteLineAsync(string.Format("{0} {1}",
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					Message));
			}
		}

	}
}
