using System;
using NLog;
using MvvmCross.Platform.Platform;

namespace GodSpeak.iOS
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
            try
            {
                _log.Debug(text, args);        
            }
            catch (Exception ex)
            {

            }
		}

		public void Error(string text, params object[] args)
		{
            try
            {
                _log.Error(text, args);
            }
            catch (Exception ex)
            {

            }
		}

		public void Fatal(string text, params object[] args)
		{
            try
            {
                _log.Fatal(text, args);
            }
            catch (Exception ex)
            {

            }
		}

		public void Info(string text, params object[] args)
		{
            try
            {
                _log.Info(text, args);
            }
            catch (Exception ex)
            {

            }
		}

		public void Trace(string text, params object[] args)
		{
            try
            {
                _log.Trace(text, args);
            }
            catch (Exception ex)
            {

            }
		}

		public void Warn(string text, params object[] args)
		{
            try
            {
                _log.Warn(text, args);
            }
            catch (Exception ex)
            {

            }
		}

		public void Trace(MvxTraceLevel level, string tag, Func<string> message)
		{
            try
            {
                _log.Trace(tag + ":" + level + ":" + message());
			}
			catch (Exception ex)
			{

			}
		}

		public void Trace(MvxTraceLevel level, string tag, string message)
		{
            try
            {
                _log.Trace(tag + ":" + level + ":" + message);
            }
			catch (Exception ex)
			{

			}
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
            catch (Exception)
            {
                
            }
		}

		public void Exception(Exception ex)
		{
			try
			{
				_log.Error(string.Format("{0} {1} {2} {3}", ex.Message, ex, ex.Data, ex.StackTrace));
			}
			catch (System.Exception ex2)
			{

			}
		}
	}
}
