using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Common
{
  public static class StringCrypto
  {
    private static readonly byte[] entropy = Encoding.Unicode.GetBytes("Don't mess with this. Just a Salt string for some random entropy. 98023542316649klgfjhseiouanflkn;@#$&$*");

    public static string EncryptString(this SecureString input)
    {
      if (input == null)
      {
        return null;
      }

      var encryptedData = ProtectedData.Protect(
          Encoding.Unicode.GetBytes(input.ToInsecureString()),
          entropy,
          DataProtectionScope.CurrentUser);

      return Convert.ToBase64String(encryptedData);
    }

    public static SecureString DecryptString(this string encryptedData)
    {
      if (encryptedData == null)
      {
        return null;
      }

      try
      {
        var decryptedData = ProtectedData.Unprotect(
            Convert.FromBase64String(encryptedData),
            entropy,
            DataProtectionScope.CurrentUser);

        return Encoding.Unicode.GetString(decryptedData).ToSecureString();
      }
      catch
      {
        return new SecureString();
      }
    }

    public static SecureString ToSecureString(this IEnumerable<char> input)
    {
      if (input == null)
      {
        return null;
      }

      var secure = new SecureString();

      foreach (var c in input)
      {
        secure.AppendChar(c);
      }

      secure.MakeReadOnly();
      return secure;
    }

    public static string ToInsecureString(this SecureString input)
    {
      if (input == null)
      {
        return null;
      }

      var ptr = Marshal.SecureStringToBSTR(input);

      try
      {
        return Marshal.PtrToStringBSTR(ptr);
      }
      finally
      {
        Marshal.ZeroFreeBSTR(ptr);
      }
    }



  }
}

