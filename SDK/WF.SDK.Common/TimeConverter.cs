using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Common
{
  public enum TimeZoneName
  {
    [DisplayName("Pacific Time")]
    Pacific,
    [DisplayName("Mountain Time")]
    Mountain,
    [DisplayName("Central Time")]
    Central,
    [DisplayName("Eastern Time")]
    Eastern,
    [DisplayName("Atlantic Time")]
    Atlantic,
    [DisplayName("Hawaii Time")]
    Hawaii,
    [DisplayName("Alaska Time")]
    Alaska,
    [DisplayName("Universal Time Coordinated")]
    UTC,
  }

  public static class TimeConverter
  {
    public static DateTime UtcToTimeZone(DateTime utcDate, string destTimeZone = "Mountain")
    {
      var tz = TimeConverter.GetTimeZone(destTimeZone);
      var temp = new DateTime(utcDate.Ticks, DateTimeKind.Utc);
      return TimeZoneInfo.ConvertTimeFromUtc(temp, tz);
    }

    public static DateTime TimeZoneToUtc(DateTime srcDate, string srcTimeZone = "Mountain")
    {
      var tz = TimeConverter.GetTimeZone(srcTimeZone);
      var temp = new DateTime(srcDate.Ticks, DateTimeKind.Unspecified);
      return TimeZoneInfo.ConvertTime(temp, tz, TimeConverter.GetUTCTimeZone());
    }

    private static List<TimeZoneInfo> TimeZoneList()
    {
      return TimeZoneInfo.GetSystemTimeZones().Where(i => i.DisplayName.Contains("US & Canada")
        || i.DisplayName.Contains("Hawaii")
        || i.DisplayName.Contains("Alaska")
        || i.DisplayName.Contains("Atlantic Time")
        || i.DisplayName.Contains("(UTC) Coordinated Universal Time")).ToList();
    }

    private static TimeZoneInfo GetTimeZone(string name = "mountain")
    {
      var tzlist = TimeConverter.TimeZoneList();
      var tz = tzlist.FirstOrDefault(i => i.DisplayName.ToLower().Contains(name.ToLower()));
      if (tz == null) { tz = tzlist.FirstOrDefault(i => i.DisplayName.ToLower().Contains("mountain")); }
      return tz;
    }

    private static TimeZoneInfo GetUTCTimeZone()
    {
      var tzlist = TimeConverter.TimeZoneList();
      return tzlist.FirstOrDefault(i => i.DisplayName.Contains("(UTC) Coordinated Universal Time"));
    }

    public static TimeZoneName GuessTimeZone(DateTime srcNow)
    {
      try
      {
        var utcNow = DateTime.UtcNow;
        var ret = Enum.GetValues(typeof(TimeZoneName)).OfType<TimeZoneName>().ToList().FirstOrDefault(i =>
        {
          var conv = TimeConverter.TimeZoneToUtc(srcNow, i.ToString());
          return conv > utcNow.AddMinutes(-10) && conv < utcNow.AddMinutes(10);
        });
        return ret;
      }
      catch { return TimeZoneName.Eastern; }
    }

  }
}
