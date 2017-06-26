using System;
using NLog;
using Xamarin.Forms;
using MvvmCross.Platform.Platform;

namespace GodSpeak.Droid
{
	public class LoggingService : ILoggingService
	{
		private Logger _log;

		public LoggingService(Logger log)
		{
			this._log = log;
		}

		public void Debug(string text, params object[] args)
		{
			_log.Debug(text, args);
		}

		public void Error(string text, params object[] args)
		{
			_log.Error(text, args);
		}

		public void Fatal(string text, params object[] args)
		{
			_log.Fatal(text, args);
		}

		public void Info(string text, params object[] args)
		{
			_log.Info(text, args);
		}

		public void Trace(string text, params object[] args)
		{
			_log.Trace(text, args);
		}

		public void Warn(string text, params object[] args)
		{
			_log.Warn(text, args);
		}

		public void Trace(MvxTraceLevel level, string tag, Func<string> message)
		{
			_log.Trace(tag + ":" + level + ":" + message());
		}

		public void Trace(MvxTraceLevel level, string tag, string message)
		{
			_log.Trace(tag + ":" + level + ":" + message);
		}

		public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
		{
			try
			{
				_log.Trace(tag + ":" + level + ":" + message, args);
			}
			catch (FormatException)
			{
				Trace(MvxTraceLevel.Error, tag, "Exception during trace of {0} {1}", level, message);
			}
		}
	}
}
