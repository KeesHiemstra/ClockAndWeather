using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;
using System.Threading;
using System.IO;

namespace ClockAndWeather
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Log.Write("Start application");

			//Prepare automated tasks
			AutoResetEvent autoEvent = new AutoResetEvent(false);
			TimedAction timedAction = new TimedAction();
			TimerCallback timerCallback = timedAction.Action;
			Timer timer = new Timer(timerCallback, autoEvent, 5 * 60 * 1000, 15 * 60 * 1000);

			BuildScreenAsync();
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			Log.Write("RereshButton_Click()");
			BuildScreenAsync();
		}

		public static void AutomaticScreen()
		{
			Log.Write("AutomacticScreen()");
			AutomaticScreenAsync();
		}

		public static async Task AutomaticScreenAsync()
		{
			await Log.Write("AutomaticScreenAsync()");
			await SayText("Refresh");
			//await BuildScreenAsync();
		}

		
	}

	class TimedAction
	{
		public void Action(Object StatusInfo)
		{
			MainWindow.AutomaticScreen();
		}
	}

	class Log
	{
		public Log()
		{

		}

		public static async Task Write(string Message)
		{
			using (StreamWriter sw = File.AppendText(@"C:\Temp\ClockAndWeather.log"))
			{
				await sw.WriteLineAsync(string.Format("{0} {1}",
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					Message));
			}
		}
	}
}
