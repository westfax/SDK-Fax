using System;
using System.Collections.Generic;
using System.Text;

namespace WF.SDK.Common
{
  public class DisplayWrapper<T> where T : struct, System.IConvertible
  {
    #region GetDisplayName
    public static string GetDisplayName(T enumValue)
    {

        System.Reflection.MemberInfo[] memberInfos = typeof(T).GetMember(enumValue.ToString());
        if (memberInfos.Length > 0)
        {
          DisplayNameAttribute att =
            (DisplayNameAttribute)Attribute.GetCustomAttribute(memberInfos[0], typeof(DisplayNameAttribute));
          if (att != null) { return att.DisplayName; }
        }
        return (enumValue.ToString().Replace("_", " ")).Trim();
    

    }
    #endregion

    #region GetDisplayDesc
    public static string GetDisplayDesc(T enumValue)
    {
      System.Reflection.MemberInfo[] memberInfos = typeof(T).GetMember(enumValue.ToString());
      if (memberInfos.Length > 0)
      {
        DisplayNameAttribute att =
          (DisplayNameAttribute)Attribute.GetCustomAttribute(memberInfos[0], typeof(DisplayNameAttribute));
        if (att != null) { return att.DisplayDesc; }
      }
      return (enumValue.ToString().Replace("_", " ")).Trim();
    }
    #endregion

    #region GetTag
    public static string GetTag(T enumValue)
    {
      System.Reflection.MemberInfo[] memberInfos = typeof(T).GetMember(enumValue.ToString());
      if (memberInfos.Length > 0)
      {
        DisplayNameAttribute att =
          (DisplayNameAttribute)Attribute.GetCustomAttribute(memberInfos[0], typeof(DisplayNameAttribute));
        if (att != null) { return att.Tag; }
      }
      return (enumValue.ToString().Replace("_", " ")).Trim();
    }
    #endregion

    #region GetDisplayNames
    public static List<string> GetDisplayNames()
    {
      List<string> memberDisplayNames = new List<string>();

      System.Reflection.FieldInfo[] fields = typeof(T).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

      foreach (System.Reflection.FieldInfo fieldInfo in fields)
      {
        object[] memberInfos = fieldInfo.GetCustomAttributes(false);

        string displayName = "";
        foreach (object obj in memberInfos)
        {
          if (!(obj is DisplayNameAttribute))
            continue;

          DisplayNameAttribute att = (DisplayNameAttribute)obj;
          if (att != null) { displayName = att.DisplayName; break; }
        }

        if (displayName == String.Empty) displayName = (fieldInfo.Name.ToString().Replace("_", " ")).Trim();

        memberDisplayNames.Add(displayName);
      }

      return memberDisplayNames;
    }

    public static List<string> GetDisplayNames(List<T> enumValList)
    {
      List<string> memberDisplayNames = new List<string>();

      System.Reflection.FieldInfo[] fields = typeof(T).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

      foreach (System.Reflection.FieldInfo fieldInfo in fields)
      {
        if (!enumValList.Contains((T)Enum.Parse(typeof(T), fieldInfo.Name))) { continue; }

        object[] memberInfos = fieldInfo.GetCustomAttributes(false);

        string displayName = "";
        foreach (object obj in memberInfos)
        {
          if (!(obj is DisplayNameAttribute))
            continue;

          DisplayNameAttribute att = (DisplayNameAttribute)obj;
          if (att != null) { displayName = att.DisplayName; break; }
        }

        if (displayName == String.Empty) displayName = (fieldInfo.Name.ToString().Replace("_", " ")).Trim();

        memberDisplayNames.Add(displayName);
      }

      return memberDisplayNames;
    }
    #endregion

    #region GetDisplayDesc
    public static List<string> GetDisplayDesc()
    {
      List<string> memberDisplayDesc = new List<string>();

      System.Reflection.FieldInfo[] fields = typeof(T).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

      foreach (System.Reflection.FieldInfo fieldInfo in fields)
      {
        object[] memberInfos = fieldInfo.GetCustomAttributes(false);

        string displayDesc = "";
        foreach (object obj in memberInfos)
        {
          if (!(obj is DisplayNameAttribute))
            continue;

          DisplayNameAttribute att = (DisplayNameAttribute)obj;
          if (att != null) { displayDesc = att.DisplayDesc; break; }
        }

        if (displayDesc == String.Empty) displayDesc = (fieldInfo.Name.ToString().Replace("_", " ")).Trim();

        memberDisplayDesc.Add(displayDesc);
      }

      return memberDisplayDesc;
    }
    #endregion

    #region GetTags
    public static List<string> GetTags()
    {
      List<string> tags = new List<string>();

      System.Reflection.FieldInfo[] fields = typeof(T).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

      foreach (System.Reflection.FieldInfo fieldInfo in fields)
      {
        object[] memberInfos = fieldInfo.GetCustomAttributes(false);

        string tag = "";
        foreach (object obj in memberInfos)
        {
          if (!(obj is DisplayNameAttribute))
            continue;

          DisplayNameAttribute att = (DisplayNameAttribute)obj;
          if (att != null) { tag = att.Tag; break; }
        }

        if (tag == String.Empty) tag = (fieldInfo.Name.ToString().Replace("_", " ")).Trim();

        tags.Add(tag);
      }

      return tags;
    }
    #endregion

    #region GetDisplayNameDictionary
    public static Dictionary<T, string> GetDisplayNameDictionary()
    {
      Dictionary<T, string> ret = new Dictionary<T, string>();
      foreach (T val in Enum.GetValues(typeof(T)))
      {
        ret.Add(val, DisplayWrapper<T>.GetDisplayName(val));
      }
      return ret;
    }
    #endregion

    #region GetDisplayDescDictionary
    public static Dictionary<T, string> GetDisplayDescDictionary()
    {
      Dictionary<T, string> ret = new Dictionary<T, string>();
      foreach (T val in Enum.GetValues(typeof(T)))
      {
        ret.Add(val, DisplayWrapper<T>.GetDisplayDesc(val));
      }
      return ret;
    }
    #endregion

    #region GetTagsDictionary
    public static Dictionary<T, string> GetTagsDictionary()
    {
      Dictionary<T, string> ret = new Dictionary<T, string>();
      foreach (T val in Enum.GetValues(typeof(T)))
      {
        ret.Add(val, DisplayWrapper<T>.GetTag(val));
      }
      return ret;
    }
    #endregion

    #region DisplayNameToEnumValue
    public static T ToEnumValue(string enumDisplayName)
    {
      return DisplayWrapper<T>.DisplayNameToEnumValue(enumDisplayName);
    }

    public static T DisplayNameToEnumValue(string enumDisplayName)
    {
      string[] enumValues = Enum.GetNames(typeof(T));
      foreach (string s in enumValues)
      {
        System.Reflection.MemberInfo[] memberInfos = typeof(T).GetMember(s.ToString());
        if (memberInfos.Length > 0)
        {
          DisplayNameAttribute att = (DisplayNameAttribute)Attribute.GetCustomAttribute(memberInfos[0], typeof(DisplayNameAttribute));
          if (att != null && att.DisplayName == enumDisplayName) { return (T)Enum.Parse(typeof(T), s); }
          if (att == null && enumDisplayName == s.Replace("_", " ").Trim()) { return (T)Enum.Parse(typeof(T), s); }
        }
      }
      return default(T);
    }
    #endregion

    #region DisplayDescToEnumValue
    public static T DisplayDescToEnumValue(string enumDisplayDesc)
    {
      string[] enumValues = Enum.GetNames(typeof(T));
      foreach (string s in enumValues)
      {
        System.Reflection.MemberInfo[] memberInfos = typeof(T).GetMember(s.ToString());
        if (memberInfos.Length > 0)
        {
          DisplayNameAttribute att = (DisplayNameAttribute)Attribute.GetCustomAttribute(memberInfos[0], typeof(DisplayNameAttribute));
          if (att != null && att.DisplayDesc == enumDisplayDesc) { return (T)Enum.Parse(typeof(T), s); }
        }
      }
      return default(T);
    }
    #endregion

    #region TagToEnumValue
    public static T TagToEnumValue(string enumTag)
    {
      string[] enumValues = Enum.GetNames(typeof(T));
      foreach (string s in enumValues)
      {
        System.Reflection.MemberInfo[] memberInfos = typeof(T).GetMember(s.ToString());
        if (memberInfos.Length > 0)
        {
          DisplayNameAttribute att = (DisplayNameAttribute)Attribute.GetCustomAttribute(memberInfos[0], typeof(DisplayNameAttribute));
          if (att != null && att.Tag == enumTag) { return (T)Enum.Parse(typeof(T), s); }
          if (att == null && enumTag == s.Replace("_", " ").Trim()) { return (T)Enum.Parse(typeof(T), s); }
        }
      }
      return default(T);
    }
    #endregion

    #region SplitDelimitedTagMatchToEnumValue - when the tag is a delimited list
    public static T SplitDelimitedTagMatchToEnumValue(char delimiter, string match)
    {
      Dictionary<T, string> temp = DisplayWrapper<T>.GetTagsDictionary();
      foreach (T key in temp.Keys)
      {
        string[] arr = temp[key].Split(delimiter);
        foreach (string s in arr)
        {
          if (s.ToLower() == match.Replace(".", "").ToLower())
          {
            return key;
          }
        }
      }

      return default(T);
    }
    #endregion

    #region GetLinkValue
    public static Enum GetLinkValue(T enumValue)
    {
      System.Reflection.MemberInfo[] memberInfos = typeof(T).GetMember(enumValue.ToString());
      DisplayNameAttribute att = null;
      if (memberInfos.Length > 0)
      {
        att = (DisplayNameAttribute)Attribute.GetCustomAttribute(memberInfos[0], typeof(DisplayNameAttribute));
        if (att != null)
        {
          foreach (Enum e in Enum.GetValues(att.LinkType))
          {
            if (Convert.ToInt32(e) == att.LinkVal) { return e; }
          }
        }
      }
      return null;
    }
    #endregion
  }
}
