using System;
using System.Diagnostics.Contracts;

namespace GodSpeak
{
	public interface ILogManager
	{
		ILoggingService GetLog([System.Runtime.CompilerServices.CallerFilePath]string callerFilePath = "");		
	}
}
