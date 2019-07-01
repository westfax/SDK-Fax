using System;
using System.Reflection;
using System.Collections.Generic;

namespace WF.SDK.Common
{
  [Flags]
  public enum GenerationStyle
  {
    Alpha = 1,
    Numeric = 2,
    AlphaNumeric = 3,
    UpperCase = 4,
    LowerCase = 8,
    MixedCase = 12,
    WackyChars = 16,
    HexString = 32,
  }

  /// <summary>
  /// This class provides a set of static methods to perform various operations on Strings.
  /// </summary>
  public sealed class StringHelper
  {
    //Purely static class - private constructor
    private StringHelper()
    { }

    public const string Zeroto9 = "0123456789";
    public const string AtoZ = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    public const string HexChars = "0123456789abcdefABCDEF";

    public const string AtoZand0to9 = AtoZ + Zeroto9;
    public const string AtoZand0to9andSpace = AtoZand0to9 + " ";
    public const string LegalFileSystemChars = AtoZand0to9andSpace + "-_.";

    /// <summary>
    /// Returns the name in lastName, firstName format unless one of them is zero length
    /// </summary>
    public static string FormatPersonName(string firstName, string lastName)
    {
      if (firstName == null) { firstName = ""; }
      if (lastName == null) { lastName = ""; }
      firstName = firstName.Trim();
      lastName = lastName.Trim();

      if (firstName.Length > 0 && lastName.Length > 0)
      {
        return string.Format("{0}, {1}", lastName, firstName);
      }
      if (firstName.Length > 0) { return firstName; }
      if (lastName.Length > 0) { return lastName; }
      return "";
    }

    /// <summary>
    /// Checks the length of a string and returns a boolean.  True if input.Lenght() == len, false otherwise.
    /// </summary>
    /// <param name="input">The String to check.</param>
    /// <param name="len">The length to check for.</param>
    public static bool CheckLength(string input, int len)
    {
      if (input.Length == len) { return true; }
      else { return false; }
    }

    /// <summary>
    /// Checks if the input string is in a valid domestic or international phonenumber format.
    /// Does not characterize the phone number.
    /// </summary>
    /// <param name="phoneNum"></param>
    public static bool CheckIsPhoneNumberValidFormat(string phoneNum)
    {
      phoneNum = StringHelper.CleanPhoneNumber(phoneNum);
      if (phoneNum.StartsWith("011") && phoneNum.Length > 7) { return true; }
      if (phoneNum.Length == 10) { return true; }
      return false;
    }

    /// <summary>
    ///Does a strict check on the 10 digit phone number
    /// </summary>
    /// <param name="phoneNum"></param>
    public static FFResult<bool> CheckNpaNxxNumberStrict(string phoneNum)
    {
      var temp = phoneNum;
      temp = StringHelper.CleanPhoneNumber(temp);
      temp = StringHelper.StripLeadingChars(temp, '0', '1');
      if (temp.Length != 10) { return new FFResult<bool>(false, new Exception("Length is not 10 digits.")); }
      if (temp[0] == '0' || temp[0] == '1') { return new FFResult<bool>(false, new Exception("Invalid NPA digits.  N cannot be 0 or 1.")); }
      if (temp[3] == '0' || temp[3] == '1') { return new FFResult<bool>(false, new Exception("Invalid NXX digits.  N cannot be 0 or 1.")); }
      return new FFResult<bool>(true);
    }

    /// <summary>
    ///Does a strict check on the 3 digit NPA of a number
    /// </summary>
    /// <param name="phoneNum"></param>
    public static FFResult<bool> CheckNpaStrict(string npa)
    {
      var temp = npa;
      temp = StringHelper.CleanPhoneNumber(temp);
      temp = StringHelper.StripLeadingChars(temp, '0', '1');
      if (temp.Length < 3) { return new FFResult<bool>(false, new Exception("Length is not at least 3 digits.")); }
      if (temp[0] == '0' || temp[0] == '1') { return new FFResult<bool>(false, new Exception("Invalid NPA digits.  N cannot be 0 or 1.")); }
      return new FFResult<bool>(true);
    }

    /// <summary>
    ///Does a strict check on the 6 digit NPANXX of a number
    /// </summary>
    /// <param name="phoneNum"></param>
    public static FFResult<bool> CheckNpaNxxStrict(string npanxx)
    {
      var temp = npanxx;
      temp = StringHelper.CleanPhoneNumber(temp);
      temp = StringHelper.StripLeadingChars(temp, '0', '1');
      if (temp.Length < 6) { return new FFResult<bool>(false, new Exception("Length is not at least 6 digits.")); }
      if (temp[0] == '0' || temp[0] == '1') { return new FFResult<bool>(false, new Exception("Invalid NPA digits.  N cannot be 0 or 1.")); }
      if (temp[3] == '0' || temp[3] == '1') { return new FFResult<bool>(false, new Exception("Invalid NXX digits.  N cannot be 0 or 1.")); }
      if (temp[4] == '1' && temp[5] == '1') { return new FFResult<bool>(false, new Exception("Invalid NXX digits.  NXX cannot be N11.")); }
      return new FFResult<bool>(true);
    }

    /// <summary>
    /// Strips leading 1 and non-numeric characters from the input string.
    /// </summary>
    /// <param name="phoneNum">The string to clean</param>
    /// <returns>The clean string</returns>
    public static string CleanPhoneNumber(string phoneNum)
    {
      phoneNum = StringHelper.StripNonNumeric(phoneNum);
      phoneNum = StringHelper.StripLeading1(phoneNum);
      return phoneNum;
    }

    /// <summary>
    /// Strips all non-numeric characters from the input string.
    /// </summary>
    /// <param name="input">The string to strip.</param>
    public static string StripNonNumeric(string input)
    {
      char[] c = input.ToCharArray();
      for (int i = 0; i < c.Length; i++)
      {
        if (!(c[i] == '0' ||
          c[i] == '1' ||
          c[i] == '2' ||
          c[i] == '3' ||
          c[i] == '4' ||
          c[i] == '5' ||
          c[i] == '6' ||
          c[i] == '7' ||
          c[i] == '8' ||
          c[i] == '9'))
        {
          c[i] = 'x';
        }
      }
      string ret = new String(c);
      ret = ret.Replace("x", "");
      return ret;
    }

    /// <summary>
    /// Strips all non-numeric characters from the input string (leaves the decimal point)
    /// </summary>
    /// <param name="input">The string to strip.</param>
    public static string StripNonNumericCurrency(string input)
    {
      char[] c = input.ToCharArray();
      for (int i = 0; i < c.Length; i++)
      {
        if (!(c[i] == '0' ||
          c[i] == '1' ||
          c[i] == '2' ||
          c[i] == '3' ||
          c[i] == '4' ||
          c[i] == '5' ||
          c[i] == '6' ||
          c[i] == '7' ||
          c[i] == '8' ||
          c[i] == '9' ||
          c[i] == '.'))
        {
          c[i] = 'x';
        }
      }
      string ret = new String(c);
      ret = ret.Replace("x", "");
      return ret;
    }

    /// <summary>
    /// Strips all numeric characters from the input string. (removes digits only)
    /// </summary>
    /// <param name="input">The string to strip.</param>
    public static string StripNumeric(string input)
    {
      return System.Text.RegularExpressions.Regex.Replace(input, @"\d", "");
    }

    /// <summary>
    /// Strips leading 1 digits from the string (after all whitespace is trimmed with String.Trim())
    /// </summary>
    /// <param name="input">The string to strip.</param>
    public static string StripLeading1(string input)
    {
      return StringHelper.StripLeadingChars(input, '1');
    }

    /// <summary>
    /// Strips leading chars from the string.  
    /// </summary>
    /// <param name="input">The string to strip.</param>
    public static string StripLeadingChars(string input, params char[] chars)
    {
      string temp = input;
      temp = temp.Trim();
      bool done = false;
      while (!done)
      {
        done = true;
        if (temp.Length == 0) { continue; }  //We're done.  the string is empty.
        foreach (char x in chars)
        {
          if (temp.StartsWith(x.ToString()))
          {
            temp = temp.Remove(0, 1);
            done = false;
          }
        }
      }
      return temp;
    }

    /// <summary>
    /// Removes weird characters (Vertical Tabs, Form Feed, Escapees, etc...)
    /// </summary>
    /// <param name="input">The string to clean.</param>
    public static string RemoveSpecialCharacters(string input)
    {
      string temp = input;
      temp = temp.Replace("\v", "");   //Vertical Tab
      temp = temp.Replace("\f", "");   //Formfeed
      temp = temp.Replace("\b", "");   //Backspace
      temp = temp.Replace("\a", "");   //Bell (alert)
      temp = temp.Replace("\033", ""); //Escape character

      return temp;
    }

    #region StripStringLeave...legal characters
    /// <summary>
    /// Removes the all but the legal characters
    /// </summary>
    public static string StripStringLeaveLegalCharacters(char[] legalCharacters, string stringToStrip)
    {
      string ret = "";
      foreach (char c in stringToStrip.ToCharArray())
      {
        foreach (char test in legalCharacters)
        {
          if (c == test) { ret += c; }
        }
      }
      return ret;
    }

    /// <summary>
    /// Removes the all but the legal characters
    /// </summary>
    public static string StripStringLeaveLegalCharacters(string legalCharacters, string stringToStrip)
    {
      return StringHelper.StripStringLeaveLegalCharacters(legalCharacters.ToCharArray(), stringToStrip);
    }

    /// <summary>
    /// Removes the all but the legal characters
    /// </summary>
    public static string StripStringLeaveHexChars(string stringToStrip)
    {
      return StringHelper.StripStringLeaveLegalCharacters(StringHelper.HexChars.ToCharArray(), stringToStrip);
    }

    /// <summary>
    /// Removes the all but the legal characters
    /// </summary>
    public static string StripStringLeaveAtoZ(string stringToStrip)
    {
      return StringHelper.StripStringLeaveLegalCharacters(StringHelper.AtoZ.ToCharArray(), stringToStrip);
    }

    /// <summary>
    /// Removes the all but the legal characters
    /// </summary>
    public static string StripStringLeaveAtoZand0to9andSpace(string stringToStrip)
    {
      return StringHelper.StripStringLeaveLegalCharacters(StringHelper.AtoZand0to9andSpace.ToCharArray(), stringToStrip);
    }

    /// <summary>
    /// Removes the all but the legal characters
    /// </summary>
    public static string StripStringLeaveAtoZand0to9andReplaceSpace(string stringToStrip, char replaceSpaceChar)
    {
      return StringHelper.StripStringLeaveLegalCharacters(StringHelper.AtoZand0to9andSpace.ToCharArray(), stringToStrip).Replace(' ', replaceSpaceChar);
    }

    /// <summary>
    /// Removes the all but the legal characters
    /// </summary>
    public static string StripStringLeaveAtoZand0to9andReplaceSpace(string stringToStrip, string replaceSpaceStr)
    {
      return StringHelper.StripStringLeaveLegalCharacters(StringHelper.AtoZand0to9andSpace.ToCharArray(), stringToStrip).Replace(" ", replaceSpaceStr);
    }

    /// <summary>
    /// Removes the all but the legal characters
    /// </summary>
    public static string StripStringLeaveAtoZand0to9(string stringToStrip)
    {
      return StringHelper.StripStringLeaveLegalCharacters(StringHelper.AtoZand0to9.ToCharArray(), stringToStrip);
    }

    #endregion

    #region StripIllegalCharactersFrom....various strings
    /// <summary>
    /// Removes the illegal characters from a string
    /// </summary>
    public static string StripIllegalCharactersFromString(char[] illegalCharacters, string stringToStrip)
    {
      string temp = stringToStrip;
      foreach (char c in illegalCharacters)
      {
        temp = temp.Replace(c.ToString(), ""); //Remove the illegal Character
      }
      return temp;
    }

    /// <summary>
    /// Removes the illegal characters from a FileName
    /// </summary>
    public static string StripIllegalCharactersFromFileName(string fileName)
    {
      return StringHelper.StripIllegalCharactersFromString(System.IO.Path.GetInvalidFileNameChars(), fileName);
    }

    /// <summary>
    /// Removes the illegal characters from a FullPath
    /// </summary>
    public static string StripIllegalCharactersFromFullPath(string fullPath)
    {
      //Split off the directory part
      string dir = System.IO.Path.GetDirectoryName(fullPath);
      dir = StringHelper.StripIllegalCharactersFromString(System.IO.Path.GetInvalidPathChars(), dir);
      //Escape the FileName Part
      string file = System.IO.Path.GetFileName(fullPath);
      file = StringHelper.StripIllegalCharactersFromString(System.IO.Path.GetInvalidFileNameChars(), file);
      //Bring together and return.
      return System.IO.Path.Combine(dir, file);
    }

    #endregion

    /// <summary>
    /// Returns a SQL safe value string from the input string (Removes '|'s and replaces 's with ''s).
    /// Use to escape strings before sending to SQL in raw SQL strings.  Not necessary for SProc calls.
    /// </summary>
    /// <param name="input">The string to escape.</param>
    /// <returns>The Safe String</returns>
    public static string SqlEscape(string input)
    {
      string ret = input;
      ret = ret.Replace("'", "''");
      ret = ret.Replace("|", "");
      return ret;
    }

    /// <summary>
    /// Truncates the input string to the desired length or leaves the string as is if its short enough.
    /// </summary>
    /// <param name="input">The string to truncate.</param>
    /// <param name="len">The final length of the string.</param>
    public static string Truncate(string input, int len)
    {
      if (input.Length > len)
      {
        return input.Substring(0, len);
      }
      return input;
    }

    #region GenerateRandomString
    public static string GenerateRandomString(int length)
    {
      return StringHelper.GenerateRandomString(length, GenerationStyle.AlphaNumeric | GenerationStyle.MixedCase);
    }

    public static string GenerateRandomString(int length, GenerationStyle style)
    {
      string allowedChars = "";

      if (FlagWrapper<GenerationStyle>.IsSet(style, GenerationStyle.Numeric))
      {
        allowedChars += "0123456789";
      }
      if (FlagWrapper<GenerationStyle>.IsSet(style, GenerationStyle.WackyChars))
      {
        allowedChars += "!@#$%^&*()_+-=[]{};':<>?~,.";
      }
      if (FlagWrapper<GenerationStyle>.IsSet(style, GenerationStyle.Alpha))
      {
        string alphas = "";
        if (FlagWrapper<GenerationStyle>.IsSet(style, GenerationStyle.LowerCase))
        {
          alphas += "abcdefghijklmnopqrstuvwxyz";
        }
        if (FlagWrapper<GenerationStyle>.IsSet(style, GenerationStyle.UpperCase))
        {
          alphas += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }
        if (alphas.Length == 0) { alphas = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"; }

        allowedChars += alphas;
      }
      if (FlagWrapper<GenerationStyle>.IsSet(style, GenerationStyle.HexString))
      {
        //Can have uppercase hexstring I suppose
        if (FlagWrapper<GenerationStyle>.IsSet(style, GenerationStyle.UpperCase))
        {
          allowedChars = "0123456789ABCDEF";
        }
        else
        {
          allowedChars = "0123456789abcdef";
        }
      }
      return StringHelper.GenerateRandomString(length, allowedChars);
    }

    public static string GenerateRandomString(int length, string allowedChars)
    {
      return StringHelper.GenerateRandomString(length, allowedChars.ToCharArray());
    }

    public static string GenerateRandomString(int length, char[] allowedChars)
    {
      //Return nothing if len is zero
      if (length == 0) { return ""; }
      if (allowedChars.Length == 0) { return ""; }
      if (allowedChars.Length == 1) { return new string(allowedChars[0], length); }

      char[] temp = new char[length];

      for (int j = 0; j < length; j++)
      {
        temp[j] = allowedChars[CommonRandom.Next(0, allowedChars.Length)];
      }
      return new string(temp);
    }
    #endregion

    #region Rot13Transform ...poor mans encryption
    /// <summary>
    /// Performs the ROT13 character rotation.
    /// </summary>
    public static string Rot13Transform(string value)
    {
      char[] array = value.ToCharArray();
      for (int i = 0; i < array.Length; i++)
      {
        int number = (int)array[i];

        if (number >= 'a' && number <= 'z')
        {
          if (number > 'm')
          {
            number -= 13;
          }
          else
          {
            number += 13;
          }
        }
        else if (number >= 'A' && number <= 'Z')
        {
          if (number > 'M')
          {
            number -= 13;
          }
          else
          {
            number += 13;
          }
        }
        array[i] = (char)number;
      }
      return new string(array);
    }
    #endregion

    /// <summary>
    /// Merges public string fields in fieldsObj with the string template.
    /// The string names in fieldsObj should be uniquely named and not found elsewhere in the template.
    /// The String names in the fieldsObj and the names inside the template delimiters are case sensitive.
    /// 
    /// Example: 
    /// 
    /// Given: 
    ///		fieldsObj.value = "Test";
    ///		template = "This is a {value}..."
    ///		
    /// A Call To: StringFromat(template, "{", "}", fieldsObj);
    /// 
    /// Will Return: "This is Test..."
    /// </summary>
    /// <param name="template">The string to do the merge on.</param>
    /// <param name="fieldsObj">An object with public string fields to merge.</param>
    /// <returns>The merged string.</returns>
    public static string StringFormat(string template, string startDelimiter, string endDelimiter, object fieldsObj)
    {
      string ret = template;
      //for each string type in fieldsObj
      FieldInfo[] fields = fieldsObj.GetType().GetFields();
      PropertyInfo[] properties = fieldsObj.GetType().GetProperties();

      foreach (FieldInfo inf in fields)
      {
        //if GetValue returns an exception the value is most likely unassigned, in this case
        //ignore it and move on with life.
        try
        {
          ret = ret.Replace(
            String.Format("{0}{1}{2}", startDelimiter, inf.Name, endDelimiter),
            inf.GetValue(fieldsObj).ToString()
            );
        }
        catch { }
      }

      foreach (PropertyInfo inf in properties)
      {
        //Be careful of index properties
        if (inf.GetIndexParameters().Length != 0) { continue; }
        //if GetValue returns an exception the value is most likely unassigned, in this case
        //ignore it and move on with life.
        try
        {
          ret = ret.Replace(
            String.Format("{0}{1}{2}", startDelimiter, inf.Name, endDelimiter),
            inf.GetValue(fieldsObj, null).ToString()
            );
        }
        catch { }

      }

      return ret;
    }

    /// <summary>
    /// Cleans up the number for display...
    /// Strips nonnumeric characters, converts it to a double, and formats it based on the following rules:
    /// Length = 0 - Return value parameter
    /// Length = 7 - XXX-XXXX
    /// Length = 10 - (XXX) XXX-XXXX parens = true
    /// Length = 10 - XXX-XXX-XXXX parens = false
    /// Length > 10 - +XXXXXXXXXXXXX
    /// </summary>
    public static string GetDisplayPhoneNumber(string value, bool parens = true)
    {
      try
      {
        string clean = StringHelper.StripNonNumeric(value);

        double number = 0;
        if (!double.TryParse(clean, out number)) { return value; }

        if (clean.Length == 0)
          return value;
        else if (clean.Length == 7)
          return String.Format("{0:###-####}", number);
        else if (clean.Length == 10 && parens)
          return String.Format("{0:(###) ###-####}", number);
        else if (clean.Length == 10 && !parens)
          return String.Format("{0:###-###-####}", number);
        else if (clean.Length > 10)
          return String.Concat("+", number.ToString());
        else
          return value;
      }
      catch (Exception ex)
      {
        return value;
      }
    }

    //Private regex is cached for compile only once! - Initialized only when needed.
    private static System.Text.RegularExpressions.Regex ipRegex = null;

    //Checks for a valid ip address
    public static bool IsValidIp(string ipAddress)
    {
      bool ret = false;
      if (!string.IsNullOrEmpty(ipAddress))
      {
        //Initialize and compile regex.
        if (StringHelper.ipRegex == null)
        {
          StringHelper.ipRegex = new System.Text.RegularExpressions.Regex(@"^((0|1[0-9]{0,2}|2[0-9]{0,1}|2[0-4][0-9]|25[0-5]|[3-9][0-9]{0,1})\.){3}(0|1[0-9]{0,2}|2[0-9]{0,1}|2[0-4][0-9]|25[0-5]|[3-9][0-9]{0,1})$");
        }
        ret = StringHelper.ipRegex.IsMatch(ipAddress, 0);
      }
      return ret;
    }
  }
}
