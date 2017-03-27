using System;
using System.Collections.Generic;
using System.Diagnostics;
using MvvmCross.Platform.Platform;

namespace GodSpeak.iOS
{
    public class DebugTrace : IMvxTrace
    {




        public void Trace (MvxTraceLevel level, string tag, Func<string> message)
        {
            Debug.WriteLine (tag + ":" + level + ":" + message ());

        }

        public void Trace (MvxTraceLevel level, string tag, string message)
        {
            Debug.WriteLine (tag + ":" + level + ":" + message);
            if (level == MvxTraceLevel.Error)
                ReportToHockey (tag, message);
        }

        public void Trace (MvxTraceLevel level, string tag, string message, params object [] args)
        {
            try {
                Debug.WriteLine (tag + ":" + level + ":" + message, args);
            } catch (FormatException) {
                Trace (MvxTraceLevel.Error, tag, "Exception during trace of {0} {1}", level, message);
            }
        }

        void ReportToHockey (string tag, string message)
        {
            HockeyApp.MetricsManager.TrackEvent (
                tag,
                new Dictionary<string, string> { { "message", "greetings from MVX" }, { "date-time", DateTime.Now.ToString ("g") } },
                new Dictionary<string, double> { { "time", 1.0 } }
            );

        }
    }
}
