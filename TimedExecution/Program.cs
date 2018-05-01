using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimedExecution
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("Start application");

			AutoResetEvent autoResetEvent = new AutoResetEvent(false);
			TimedAction timedAction = new TimedAction();

			TimerCallback timerCallback1 = timedAction.PublicAction;
			Timer timer1 = new Timer(timerCallback1, autoResetEvent, 1000, 1000);

			TimerCallback timerCallback2 = timedAction.MainAction;
			Timer timer2 = new Timer(timerCallback2, autoResetEvent, 2000, 2000);

			Console.WriteLine("Press key...");
			Console.ReadKey();

			//Destroy timed actions
			autoResetEvent.WaitOne(0, false);
			timer1.Dispose();
			timer2.Dispose();

			Console.WriteLine("Finished application");
		}

		public static void MyAction()
		{
			Console.WriteLine(string.Format("Main action at {0} without async",
				DateTime.Now.ToString()));

			MyActionAsync();
		}

		public static async Task MyActionAsync()
		{
			Console.WriteLine(string.Format("Main asynced action at {0}",
				DateTime.Now.ToString()));

			await Log.WriteAsync("Timed async action");
		}
	}

	class TimedAction
	{
		public void PublicAction(Object StatusInfo)
		{
			Console.WriteLine(string.Format("Timed public action at {0}",
				DateTime.Now.ToString()));
		}

		public void MainAction(Object StatusInfo)
		{
			Program.MyAction();
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
	}//class Log

	/* 2017-11-28 11:15
	class Program
	{
		static void Main(string[] args)
		{
			// Create an event to signal the timeout count threshold in the 
			// timer callback.
			AutoResetEvent autoEvent = new AutoResetEvent(false);

			//StatusChecker statusChecker = new StatusChecker();

			TimedAction ItIs = new TimedAction();
			// Create an inferred delegate that invokes methods for the timer.
			//TimerCallback tcb = statusChecker.CheckStatus;
			TimerCallback tcb = ItIs.Action;

			// Create a timer that signals the delegate to invoke  
			// CheckStatus after one second, and every 1/4 second  
			// thereafter.
			Console.WriteLine("{0} Creating timer.\n",
					DateTime.Now.ToString("h:mm:ss.fff"));
			Timer stateTimer = new Timer(tcb, autoEvent, 3000, 1000);

			//// When autoEvent signals, change the period to every 
			//// 1/2 second.
			//autoEvent.WaitOne(5000, false);
			//stateTimer.Change(0, 500);
			//Console.WriteLine("\nChanging period.\n");

			Console.WriteLine("Press a key...");
			Console.ReadKey();

			// When autoEvent signals the second time, dispose of  
			// the timer.
			autoEvent.WaitOne(0, false);

			stateTimer.Dispose();
			Console.WriteLine("\nDestroying timer.");

			Console.WriteLine("Press a key...");
			Console.ReadKey();

		}

		public static void MyAction()
		{
			Console.WriteLine(DateTime.Now.ToString());
		}
	}

	class TimedAction
	{
		public void Action(Object stateInfo)
		{
			Program.MyAction();
		}
	}

	class StatusChecker
	{
		public StatusChecker()
		{
		}

		// This method is called by the timer delegate. 
		public void CheckStatus(Object stateInfo)
		{
			AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
			Console.WriteLine("{0} Checking status.",
					DateTime.Now.ToString("h:mm:ss.fff"));
		}
	}
	*/
}