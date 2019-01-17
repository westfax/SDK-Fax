using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace WF.SDK.Common
{
  [Serializable]
  [XmlRoot("dateTime")]
  public class WfDateTime : IComparable, IFormattable, IConvertible,
  ISerializable, IComparable<DateTime>, IComparable<WfDateTime>,
  IEquatable<DateTime>, IEquatable<WfDateTime>, IXmlSerializable
  {
    private DateTime _dtm = new DateTime(1850, 7, 1);

    //Constructors
    public WfDateTime()
    {
    }

    public WfDateTime(DateTime dateTime)
    {
      if (WfDateTime.IsUnAssigned(dateTime)) { return; }
      this._dtm = dateTime;
    }

    public WfDateTime(int year, int month, int day)
      : this(new DateTime(year, month, day))
    { }

    public WfDateTime(int year, int month)
      : this(new DateTime(year, month, 1))
    { }

    //For serialization
    protected WfDateTime(SerializationInfo info, StreamingContext context)
    {
      DateTime dtm = info.GetDateTime("i");
      this._dtm = dtm;
    }

    #region Static Members

    public readonly static WfDateTime MaxValue = (WfDateTime)DateTime.MaxValue;
    public readonly static WfDateTime MinValue = (WfDateTime)new DateTime(1850, 7, 1);
    public readonly static WfDateTime UnAssignedValue = (WfDateTime)new DateTime(1850, 7, 1);

    public static DateTime UnAssigned
    {
      get { return new DateTime(1850, 7, 1); }
    }

    public static bool IsUnAssigned(DateTime dtm)
    {
      if (dtm <= new DateTime(1900, 1, 1)) { return true; }
      else { return false; }
    }

    public static bool IsUnAssigned(WfDateTime wfdtm)
    {
      if (wfdtm._dtm <= new DateTime(1900, 1, 1)) { return true; }
      else { return false; }
    }

    public static WfDateTime StartOfDay(WfDateTime wfdtm)
    {
      return new WfDateTime(new DateTime(wfdtm._dtm.Year, wfdtm._dtm.Month, wfdtm._dtm.Day, 0, 0, 0));
    }

    public static WfDateTime EndOfDay(WfDateTime wfdtm)
    {
      return wfdtm.StartOfDay().AddDays(1);
    }

    public static WfDateTime StartOfMonth(WfDateTime wfdtm)
    {
      return new WfDateTime(new DateTime(wfdtm._dtm.Year, wfdtm._dtm.Month, 1, 0, 0, 0));
    }

    public static WfDateTime EndOfMonth(WfDateTime wfdtm)
    {
      return wfdtm.StartOfMonth().AddMonths(1);
    }

    public static string ToShortDateTimeString(WfDateTime wfdtm)
    {
      return WfDateTime.ToShortDateTimeString(wfdtm._dtm);
    }

    public static string ToShortDateTimeString(DateTime dtm)
    {
      if (WfDateTime.IsUnAssigned(dtm)) { return "UnAssigned"; }
      return dtm.ToShortDateString() + " " + dtm.ToShortTimeString();
    }

    public static string ToLongDateTimeString(WfDateTime wfdtm)
    {
      return WfDateTime.ToLongDateTimeString(wfdtm._dtm);
    }

    public static string ToLongDateTimeString(DateTime dtm)
    {
      if (WfDateTime.IsUnAssigned(dtm)) { return "UnAssigned"; }
      return dtm.ToLongDateString() + " " + dtm.ToLongTimeString();
    }

    public static string ToShortFileSystemString(WfDateTime wfdtm)
    {
      return WfDateTime.ToShortFileSystemString(wfdtm._dtm);
    }

    public static string ToShortFileSystemString(DateTime dtm)
    {
      return dtm.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");
    }

    public static string ToLongFileSystemString(WfDateTime wfdtm)
    {
      return WfDateTime.ToLongFileSystemString(wfdtm._dtm);
    }

    public static string ToLongFileSystemString(DateTime dtm)
    {
      return dtm.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss.ffff");
    }

    public static string ToXmlString(WfDateTime wfdtm)
    {
      return WfDateTime.ToXmlString(wfdtm._dtm);
    }

    public static string ToXmlString(DateTime dtm)
    {
      return dtm.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK");
    }

    /// <summary>
    /// Method ignores the DateTime.Kind value.  Reassigns it to the Local kind and
    /// converts to utc using the local machine timezone setting.
    /// </summary>
    public static DateTime LocalToUniversalTime(DateTime localTime)
    {
      DateTime dtm = DateTime.SpecifyKind(localTime, DateTimeKind.Local);
      return dtm.ToUniversalTime();
    }

    /// <summary>
    /// Method ignores the DateTime.Kind value.  Reassigns it to the Utc kind and
    /// converts to local using the local machine timezone setting.
    /// </summary>
    public static DateTime UniversalToLocalTime(DateTime utcTime)
    {
      DateTime dtm = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
      return dtm.ToLocalTime();
    }

    #endregion

    #region DateTime Static Members
    public static int Compare(WfDateTime wfdtm1, WfDateTime wfdtm2)
    {
      return wfdtm1.CompareTo(wfdtm2);
    }

    public static int DaysInMonth(int year, int month)
    {
      return DateTime.DaysInMonth(year, month);
    }

    public static bool Equals(WfDateTime wfdtm1, WfDateTime wfdtm2)
    {
      return wfdtm1.Equals(wfdtm2);
    }

    public static new bool Equals(object objA, object objB)
    {
      return ((WfDateTime)objA).Equals(objB);
    }

    public static WfDateTime FromBinary(long dateData)
    {
      return (WfDateTime)DateTime.FromBinary(dateData);
    }

    public static WfDateTime FromFileTime(long fileTime)
    {
      return (WfDateTime)DateTime.FromFileTime(fileTime);
    }

    public static WfDateTime FromFileTimeUtc(long fileTime)
    {
      return (WfDateTime)DateTime.FromFileTimeUtc(fileTime);
    }

    public static WfDateTime FromOADate(double d)
    {
      return (WfDateTime)DateTime.FromOADate(d);
    }

    public static bool IsLeapYear(int year)
    {
      return DateTime.IsLeapYear(year);
    }

    public static WfDateTime Now
    {
      get { return (WfDateTime)DateTime.Now; }
    }

    public static WfDateTime Parse(string s)
    {
      return (WfDateTime)DateTime.Parse(s);
    }

    public static WfDateTime Parse(string s, IFormatProvider provider)
    {
      return (WfDateTime)DateTime.Parse(s, provider);
    }

    public static WfDateTime Parse(string s, IFormatProvider provider, System.Globalization.DateTimeStyles style)
    {
      return (WfDateTime)DateTime.Parse(s, provider, style);
    }

    public static WfDateTime ParseExact(string s, string format, IFormatProvider provider)
    {
      return (WfDateTime)DateTime.ParseExact(s, format, provider);
    }

    public static WfDateTime ParseExact(string s, string format, IFormatProvider provider, System.Globalization.DateTimeStyles style)
    {
      return (WfDateTime)DateTime.ParseExact(s, format, provider, style);
    }

    public static WfDateTime ParseExact(string s, string[] formats, IFormatProvider provider, System.Globalization.DateTimeStyles style)
    {
      return (WfDateTime)DateTime.ParseExact(s, formats, provider, style);
    }

    public static new bool ReferenceEquals(object objA, object objB)
    {
      return objA.GetHashCode() == objB.GetHashCode();
    }

    public static WfDateTime SpecifyKind(DateTime value, DateTimeKind kind)
    {
      return (WfDateTime)DateTime.SpecifyKind(value, kind);
    }

    public static WfDateTime Today
    {
      get { return (WfDateTime)DateTime.Today; }
    }

    public static bool TryParse(string s, out WfDateTime result)
    {
      DateTime dtm;
      bool ret = DateTime.TryParse(s, out dtm);
      result = (WfDateTime)dtm;
      return ret;
    }

    public static bool TryParse(string s, IFormatProvider provider, System.Globalization.DateTimeStyles style, out WfDateTime result)
    {
      DateTime dtm;
      bool ret = DateTime.TryParse(s, provider, style, out dtm);
      result = (WfDateTime)dtm;
      return ret;
    }

    public static WfDateTime UtcNow
    {
      get { return (WfDateTime)DateTime.UtcNow; }
    }

    public static TimeSpan DateDiff(DateTime dtmStart, DateTime dtmEnd, bool absValue = true)
    {
      var ticks = dtmEnd.Ticks - dtmStart.Ticks;
      if (absValue) { ticks = Math.Abs(ticks); }
      return new TimeSpan(ticks);
    }

    #endregion

    #region Static Operators
    //Equivalence
    public static bool operator ==(WfDateTime wfdtm1, WfDateTime wfdtm2)
    {
      if (wfdtm1.IsUnAssigned() && wfdtm2.IsUnAssigned()) { return true; }
      return wfdtm1._dtm == wfdtm2._dtm;
    }
    public static bool operator !=(WfDateTime wfdtm1, WfDateTime wfdtm2)
    {
      if (wfdtm1.IsUnAssigned() && wfdtm2.IsUnAssigned()) { return false; }
      return wfdtm1._dtm != wfdtm2._dtm;
    }
    public static bool operator ==(DateTime dtm, WfDateTime wfdtm) { return (WfDateTime)dtm == wfdtm; }
    public static bool operator !=(DateTime dtm, WfDateTime wfdtm) { return (WfDateTime)dtm != wfdtm; }
    public static bool operator ==(WfDateTime wfdtm, DateTime dtm) { return wfdtm == (WfDateTime)dtm; }
    public static bool operator !=(WfDateTime wfdtm, DateTime dtm) { return wfdtm != (WfDateTime)dtm; }

    public static bool operator >=(WfDateTime wfdtm1, WfDateTime wfdtm2)
    {
      if (wfdtm1.IsUnAssigned() && wfdtm2.IsUnAssigned()) { return true; }
      return wfdtm1._dtm >= wfdtm2._dtm;
    }
    public static bool operator <=(WfDateTime wfdtm1, WfDateTime wfdtm2)
    {
      if (wfdtm1.IsUnAssigned() && wfdtm2.IsUnAssigned()) { return true; }
      return wfdtm1._dtm <= wfdtm2._dtm;
    }
    public static bool operator >=(DateTime dtm, WfDateTime wfdtm) { return (WfDateTime)dtm >= wfdtm; }
    public static bool operator <=(DateTime dtm, WfDateTime wfdtm) { return (WfDateTime)dtm <= wfdtm; }
    public static bool operator >=(WfDateTime wfdtm, DateTime dtm) { return wfdtm >= (WfDateTime)dtm; }
    public static bool operator <=(WfDateTime wfdtm, DateTime dtm) { return wfdtm <= (WfDateTime)dtm; }

    public static bool operator >(WfDateTime wfdtm1, WfDateTime wfdtm2)
    {
      if (wfdtm1.IsUnAssigned() && wfdtm2.IsUnAssigned()) { return true; }
      return wfdtm1._dtm > wfdtm2._dtm;
    }
    public static bool operator <(WfDateTime wfdtm1, WfDateTime wfdtm2)
    {
      if (wfdtm1.IsUnAssigned() && wfdtm2.IsUnAssigned()) { return true; }
      return wfdtm1._dtm < wfdtm2._dtm;
    }
    public static bool operator >(DateTime dtm, WfDateTime wfdtm) { return (WfDateTime)dtm > wfdtm; }
    public static bool operator <(DateTime dtm, WfDateTime wfdtm) { return (WfDateTime)dtm < wfdtm; }
    public static bool operator >(WfDateTime wfdtm, DateTime dtm) { return wfdtm > (WfDateTime)dtm; }
    public static bool operator <(WfDateTime wfdtm, DateTime dtm) { return wfdtm < (WfDateTime)dtm; }

    //Implicit Conversions
    public static implicit operator WfDateTime(DateTime dtm)
    {
      if (WfDateTime.IsUnAssigned(dtm)) { return new WfDateTime(); }
      return new WfDateTime(dtm);
    }

    public static implicit operator DateTime(WfDateTime wfdtm)
    {
      return wfdtm._dtm;
    }
    #endregion

    //Instance 
    #region DateTime properties
    [XmlIgnore]
    public DateTime Date
    {
      get { return this._dtm.Date; }
    }
    [XmlIgnore]
    public int Day
    {
      get { return this._dtm.Day; }
    }
    [XmlIgnore]
    public DayOfWeek DayOfWeek
    {
      get { return this._dtm.DayOfWeek; }
    }
    [XmlIgnore]
    public int DayOfYear
    {
      get { return this._dtm.DayOfYear; }
    }
    [XmlIgnore]
    public int Hour
    {
      get { return this._dtm.Hour; }
    }
    [XmlIgnore]
    public DateTimeKind Kind
    {
      get { return this._dtm.Kind; }
    }
    [XmlIgnore]
    public int Millisecond
    {
      get { return this._dtm.Millisecond; }
    }
    [XmlIgnore]
    public int Minute
    {
      get { return this._dtm.Minute; }
    }
    [XmlIgnore]
    public int Month
    {
      get { return this._dtm.Month; }
    }
    [XmlIgnore]
    public int Second
    {
      get { return this._dtm.Second; }
    }
    [XmlIgnore]
    public long Ticks
    {
      get { return this._dtm.Ticks; }
    }
    [XmlIgnore]
    public TimeSpan TimeOfDay
    {
      get { return this._dtm.TimeOfDay; }
    }
    [XmlIgnore]
    public int Year
    {
      get { return this._dtm.Year; }
    }
    #endregion

    #region DateTime Add* Subtract methods

    public WfDateTime Add(TimeSpan value)
    {
      return new WfDateTime(this._dtm.Add(value));
    }

    public WfDateTime AddDays(double value)
    {
      return new WfDateTime(this._dtm.AddDays(value));
    }

    public WfDateTime AddHours(double value)
    {
      return new WfDateTime(this._dtm.AddHours(value));
    }

    public WfDateTime AddMilliseconds(double value)
    {
      return new WfDateTime(this._dtm.AddMilliseconds(value));
    }

    public WfDateTime AddMinutes(double value)
    {
      return new WfDateTime(this._dtm.AddMinutes(value));
    }

    public WfDateTime AddMonths(int value)
    {
      return new WfDateTime(this._dtm.AddMonths(value));
    }

    public WfDateTime AddSeconds(double value)
    {
      return new WfDateTime(this._dtm.AddSeconds(value));
    }

    public WfDateTime AddTicks(long value)
    {
      return new WfDateTime(this._dtm.AddTicks(value));
    }

    public WfDateTime AddYears(int value)
    {
      return new WfDateTime(this._dtm.AddYears(value));
    }

    public TimeSpan Subtract(DateTime value)
    {
      return this._dtm.Subtract(value);
    }

    public WfDateTime Subtract(TimeSpan value)
    {
      return new WfDateTime(this._dtm.Subtract(value));
    }

    #endregion

    #region DateTime Other Methods

    public string[] GetDateTimeFormats()
    {
      return this._dtm.GetDateTimeFormats();
    }

    public bool IsDaylightSavingTime()
    {
      return this._dtm.IsDaylightSavingTime();
    }

    #endregion

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj == null) { return false; }
      if (obj is DateTime || obj is WfDateTime)
      {
        return this == (WfDateTime)obj;
      }
      else { return false; }
    }

    public bool IsUnAssigned()
    {
      if (this._dtm <= new DateTime(1900, 1, 1)) { return true; }
      else { return false; }
    }

    public WfDateTime StartOfDay()
    {
      return new WfDateTime(new DateTime(this._dtm.Year, this._dtm.Month, this._dtm.Day, 0, 0, 0));
    }

    public WfDateTime EndOfDay()
    {
      return this.StartOfDay().AddDays(1);
    }

    public WfDateTime StartOfMonth()
    {
      return new WfDateTime(new DateTime(this._dtm.Year, this._dtm.Month, 1, 0, 0, 0));
    }

    public WfDateTime EndOfMonth()
    {
      return this.StartOfMonth().AddMonths(1);
    }

    public WfDateTime EndOfMonthLastDay()
    {
      return this.EndOfMonth().AddDays(-1);
    }

    #region IEquatable<DateTime> Members

    public bool Equals(DateTime other)
    {
      return this.Equals((object)other);
    }

    #endregion

    #region IEquatable<WfDateTime> Members

    public bool Equals(WfDateTime other)
    {
      return this.Equals((object)other);
    }

    #endregion

    #region ToString*() Instance Methods
    public override string ToString()
    {
      return this._dtm.ToString();
    }

    public string ToString(string format)
    {
      return this._dtm.ToString(format);
    }

    public string ToShortDateTimeString()
    {
      return WfDateTime.ToShortDateTimeString(this);
    }

    public string ToLongDateTimeString()
    {
      return WfDateTime.ToLongDateTimeString(this);
    }

    public string ToShortFileSystemString()
    {
      return WfDateTime.ToShortFileSystemString(this);
    }

    public string ToLongFileSystemString()
    {
      return WfDateTime.ToLongFileSystemString(this);
    }

    public string ToXmlString()
    {
      return WfDateTime.ToXmlString(this);
    }
    #endregion

    #region DateTime To* methods

    public long ToBinary()
    {
      return this._dtm.ToBinary();
    }

    public long ToFileTime()
    {
      return this._dtm.ToFileTime();
    }

    public long ToFileTimeUtc()
    {
      return this._dtm.ToFileTimeUtc();
    }

    public WfDateTime ToLocalTime()
    {
      return new WfDateTime(this._dtm.ToLocalTime());
    }

    public string ToLongDateString()
    {
      return this._dtm.ToLongDateString();
    }

    public string ToLongTimeString()
    {
      return this._dtm.ToLongTimeString();
    }

    public double ToOADate()
    {
      return this._dtm.ToOADate();
    }

    public string ToShortDateString()
    {
      return this._dtm.ToShortDateString();
    }

    public string ToShortTimeString()
    {
      return this._dtm.ToShortTimeString();
    }

    public WfDateTime ToUniversalTime()
    {
      return new WfDateTime(this._dtm.ToUniversalTime());
    }

    #endregion

    #region IFormattable Members

    public string ToString(string format, IFormatProvider formatProvider)
    {
      return this._dtm.ToString(format, formatProvider);
    }

    #endregion

    #region IComparable Members

    public int CompareTo(object obj)
    {
      if (obj == null) { return 0; }
      if (obj is DateTime || obj is WfDateTime)
      {
        if (this == (WfDateTime)obj) { return 0; }
        if (this >= (WfDateTime)obj) { return 1; }
        if (this < (WfDateTime)obj) { return -1; }
      }
      return 0;
    }

    #endregion

    #region IComparable<WfDateTime> Members

    public int CompareTo(WfDateTime other)
    {
      return this.CompareTo((object)other);
    }

    #endregion

    #region IComparable<DateTime> Members

    public int CompareTo(DateTime other)
    {
      return this.CompareTo((object)other);
    }

    #endregion

    #region IConvertible Members

    TypeCode IConvertible.GetTypeCode()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    bool IConvertible.ToBoolean(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    byte IConvertible.ToByte(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    char IConvertible.ToChar(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
      return (DateTime)this;
    }

    decimal IConvertible.ToDecimal(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    double IConvertible.ToDouble(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    short IConvertible.ToInt16(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    int IConvertible.ToInt32(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    long IConvertible.ToInt64(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    sbyte IConvertible.ToSByte(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    float IConvertible.ToSingle(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    //Non-Explicit interface Implementation
    public string ToString(IFormatProvider provider)
    {
      return this._dtm.ToString(provider);
    }

    object IConvertible.ToType(Type conversionType, IFormatProvider provider)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    ushort IConvertible.ToUInt16(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    uint IConvertible.ToUInt32(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    ulong IConvertible.ToUInt64(IFormatProvider provider)
    {
      throw new InvalidCastException("The method or operation is not implemented.");
    }

    #endregion

    #region ISerializable Members

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("i", this._dtm);
    }

    #endregion

    #region IXmlSerializable Members

    System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
    {
      return (System.Xml.Schema.XmlSchema)null;
    }

    void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
    {
      this._dtm = reader.ReadElementContentAsDateTime();
    }

    void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
    {
      writer.WriteRaw(this._dtm.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"));
    }

    #endregion
  }
}
