using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Common
{
  public static class wwHttpHelper
  {

    public static void AddParameterList<T>(wwHttp http, List<T> items, string name)
    {
      if (items is List<Guid> || items is List<String>)
      {
        for (int i = 0; i < items.Count; i++)
        {
          var key = name + (i + 1).ToString();
          var sval = items[i].ToString();
          http.AddPostKey(key, sval);
        }
      }
      else
      {
        for (int i = 0; i < items.Count; i++)
        {
          var key = name + (i + 1).ToString();
          var sval = WF.SDK.Common.JSONSerializerHelper.SerializeToString<T>(items[i]);
          http.AddPostKey(key, sval);
        }
      }
    }

   



  }

}
