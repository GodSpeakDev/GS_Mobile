using System;
using MvvmCross.Platform.Platform;

namespace GodSpeak
{
	public interface ILoggingService : IMvxTrace
	{
		void Trace(string text, params object[] args);
		void Debug(string text, params object[] args);
		void Info(string text, params object[] args);
		void Warn(string text, params object[] args);
		void Error(string text, params object[] args);
		void Fatal(string text, params object[] args);
		void Exception(Exception ex);
	}
}
