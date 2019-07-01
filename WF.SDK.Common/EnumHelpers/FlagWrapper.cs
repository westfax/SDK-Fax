using System;
using System.Collections.Generic;
using System.Text;

namespace WF.SDK.Common
{
  public class FlagHelper
  {
    public static bool IsSet(int valueToCheckIn, int flagToCheckFor)
    {
      if ((int)(valueToCheckIn & flagToCheckFor) == flagToCheckFor)
      {
        return true;
      }
      else
      {
        return false;
      }
    }
  }

  public class FlagWrapper<T> where T : struct, System.IConvertible
  {
    private T _value;

    public FlagWrapper(T enumVal)
    {
      this._value = enumVal;
    }

    public T Value
    {
      get
      {
        return this._value;
      }
      set
      {
        this._value = value;
      }
    }

    #region Instance Methods
    public bool IsSet(T checkForEnumVal)
    {
      return FlagWrapper<T>.IsSet(this._value, checkForEnumVal);
    }

    public bool IsUnSet(T checkForEnumVal)
    {
      return FlagWrapper<T>.IsUnSet(this._value, checkForEnumVal);
    }

    public void Set(T setEnumVal)
    {
      this._value = FlagWrapper<T>.Set(this._value, setEnumVal);
    }

    public void UnSet(T unsetEnumVal)
    {
      this._value = FlagWrapper<T>.UnSet(this._value, unsetEnumVal);
    }

    #endregion

    static FlagWrapper()
    {
      FlagsAttribute[] atts = (FlagsAttribute[])typeof(T).GetCustomAttributes(typeof(FlagsAttribute), false);
      if (atts.Length == 1)
      {

      }
      else { throw new Exception("The Enum is not decorated with the Flags Attribute."); }
    }

    #region Static Methods
    public static bool IsSet(T enumVal, T checkForEnumVal)
    {
      if ((Convert.ToInt64(enumVal) & Convert.ToInt64(checkForEnumVal)) == Convert.ToInt64(checkForEnumVal)) { return true; }
      else { return false; }
    }

    public static bool IsUnSet(T enumVal, T checkForEnumVal)
    {
      return !FlagWrapper<T>.IsSet(enumVal, checkForEnumVal);
    }

    public static T Set(T enumVal, T setEnumVal)
    {
      return (T)Enum.ToObject(typeof(T), (Convert.ToInt64(enumVal) | Convert.ToInt64(setEnumVal)));
    }

    public static T UnSet(T enumVal, T unsetEnumVal)
    {
      return (T)Enum.ToObject(typeof(T), (Convert.ToInt64(enumVal) & ~Convert.ToInt64(unsetEnumVal)));
    }

    public static T Flip(T enumVal, T flipEnumVal)
    {
      return (T)Enum.ToObject(typeof(T), ((~Convert.ToInt64(enumVal) & Convert.ToInt64(flipEnumVal)) | (Convert.ToInt64(enumVal) & ~Convert.ToInt64(flipEnumVal))));
    }

    #endregion

  }
}
