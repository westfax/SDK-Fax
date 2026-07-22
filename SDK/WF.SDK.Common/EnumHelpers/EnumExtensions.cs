using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Common
{
  public static class EnumExtensions
  {
    public static bool In(this Enum value, params Enum[] list) { return list.Contains(value); }

    public static bool In(this Enum value, List<Enum> list) { return list.Contains(value); }

    public static bool NotIn(this Enum value, params Enum[] list) { return !value.In(list); }

    public static bool NotIn(this Enum value, List<Enum> list) { return !value.In(list); }

    /// <summary>
    /// If an attribute cannot be found then the default will be the given default.  A null 
    /// give default will reverto the the ToString() value.
    /// </summary>
    public static string ToDisplayName(this Enum value, string defaultDisplayName = null)
    {
      DisplayNameAttribute att = EnumExtensions.GetDisplayNameAttribute(value);
      if (att != null) { return att.DisplayName; }
      if (defaultDisplayName == null) { defaultDisplayName = value.ToString(); }
      return defaultDisplayName;
    }

    /// <summary>
    /// If an attribute cannot be found then the default will be the given default.  A null 
    /// give default will reverto the the ToString() value.
    /// </summary>
    public static string ToDisplayDesc(this Enum value, string defaultDisplayDesc = null)
    {
      DisplayNameAttribute att = EnumExtensions.GetDisplayNameAttribute(value);
      if (att != null) { return att.DisplayDesc; }
      if (defaultDisplayDesc == null) { defaultDisplayDesc = value.ToString(); }
      return defaultDisplayDesc;
    }

    public static DisplayNameAttribute GetDisplayNameAttribute(this Enum value)
    {
      try
      {
        DisplayNameAttribute ret = null;
        System.Reflection.MemberInfo[] memberInfos = value.GetType().GetMember(value.ToString());
        if (memberInfos.Length > 0)
        {
          ret = (DisplayNameAttribute)Attribute.GetCustomAttribute(memberInfos[0], typeof(DisplayNameAttribute));
        }
        return ret;
      }
      catch { return null; }
    }


  }
}
