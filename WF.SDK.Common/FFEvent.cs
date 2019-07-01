using System;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;

namespace WF.SDK.Common
{
  [Serializable]
  public class FFEvent
  {
    public static void Publish(string message, string logname = "Application", string source = "WF.SDK")
    {
      //EventLogEntryType type = EventLogEntryType.Information;

      //EventLog log = FFEvent.GetEventLogByName(logname);

      //try
      //{
      //  if (!EventLog.SourceExists(source))
      //  {
      //    EventLog.CreateEventSource(source, log.LogDisplayName);
      //  }
      //}
      //catch { }

      //log.Source = source;
      //log.WriteEntry(message, type);
      //log.Close();
      //log.Dispose();
    }

    public static void Publish(Exception exception, string logname = "Application", string source = "WF.SDK")
    {
      //EventLogEntryType type = EventLogEntryType.Information;

      //EventLog log = FFEvent.GetEventLogByName(logname);

      //try
      //{
      //  if (!EventLog.SourceExists(source))
      //  {
      //    EventLog.CreateEventSource(source, log.LogDisplayName);
      //  }
      //}
      //catch { }

      //log.Source = source;
      //log.WriteEntry(exception.ToString(), type);
      //log.Close();
      //log.Dispose();
    }

    private static EventLog GetEventLogByName(string logName)
    {
      EventLog[] logs = EventLog.GetEventLogs();
      foreach (EventLog log in logs)
      {
        if (log.LogDisplayName.ToLower() == logName.ToLower()) { return log; }
      }
      return FFEvent.GetEventLogByName("Application");
    }

    public static void FixUpEventLog(bool clear)
    {
      System.Diagnostics.EventLog[] logs = System.Diagnostics.EventLog.GetEventLogs();
      foreach (System.Diagnostics.EventLog log in logs)
      {
        log.MaximumKilobytes = 1024 * 10;  //10 MB
        log.ModifyOverflowPolicy(System.Diagnostics.OverflowAction.OverwriteAsNeeded, 3);
        if (clear)
        {
          log.Clear();
        }
      }
    }



  }
}
