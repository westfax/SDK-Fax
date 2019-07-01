using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace WF.SDK.Common
{
  public static class JSONSerializerHelper
  {
    public static List<Newtonsoft.Json.JsonConverter> SerializeConverters { get; set; }
    public static List<Newtonsoft.Json.JsonConverter> DeserializeConverters { get; set; }

    static JSONSerializerHelper()
    {
      JSONSerializerHelper.SerializeConverters = new List<Newtonsoft.Json.JsonConverter>();
      JSONSerializerHelper.DeserializeConverters = new List<Newtonsoft.Json.JsonConverter>();
    }

    public static T Deserialize<T>(string item, bool errorOnMissingMember = true)
    {
      if (item == null || item.Length == 0) { return default(T); }

      T ret = default(T);

      try
      {
        var ser = new Newtonsoft.Json.JsonSerializer();
        if (!errorOnMissingMember)
        {
          ser.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
        }
        //Add the needed converters
        JSONSerializerHelper.DeserializeConverters.ForEach(i => ser.Converters.Add(i));
        //Make a reader
        var sr = new StringReader(item);
        var reader = new Newtonsoft.Json.JsonTextReader(sr);
        ret = (T)ser.Deserialize(reader, typeof(T));
      }
      catch (Exception e)
      {
        Exception ex = new Exception("Deserialize Failed.", e);
        throw ex;
      }
      return ret;
    }


    public static string SerializeToString<T>(T item)
    {
      return JSONSerializerHelper.SerializeToString<T>(item, "");
    }

    public static string SerializeToString<T>(T item, string defaultIfNull)
    {
      if (item == null) { return defaultIfNull; }
      string ret = "";
      try
      {
        Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer();
        //Add the needed converters
        JSONSerializerHelper.SerializeConverters.ForEach(i => ser.Converters.Add(i));
        //Make a writer
        var sw = new StringWriter();
        ser.Serialize(sw, item);

        ret = sw.GetStringBuilder().ToString();
      }
      catch (Exception e)
      {
        Exception ex = new Exception("Serialize Failed.", e);
        throw ex;
      }
      return ret;
    }
  }

}
    

