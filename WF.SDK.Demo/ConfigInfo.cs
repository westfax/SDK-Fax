using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Fax.Demo
{

  public static class ConfigInfo
  {

    private static string _url = null;
    public static string Url
    {
      get
      {
        if (ConfigInfo._url == null) { ConfigInfo._url = (string)(System.Configuration.ConfigurationManager.AppSettings["APIEndpoint"]); }
        return ConfigInfo._url;
      }
    }

    private static string _encoding = null;
    public static string Encoding
    {
      get
      {
        if (ConfigInfo._encoding == null) { ConfigInfo._encoding = (string)(System.Configuration.ConfigurationManager.AppSettings["APIEncoding"]); }
        return ConfigInfo._encoding;
      }
    }

    //https://api2.westfax.com/REST/{0}/json
    public static string RestUrlTemplate
    {
      get { return ConfigInfo.Url + "/REST/{0}/" + ConfigInfo.Encoding; }
    }

    private static string _user = null;
    public static string User
    {
      get
      {
        if (ConfigInfo._user == null) { ConfigInfo._user = (string)(System.Configuration.ConfigurationManager.AppSettings["Username"]); }
        return ConfigInfo._user;
      }
    }

    private static string _pass = null;
    public static string Pass
    {
      get
      {
        if (ConfigInfo._pass == null) { ConfigInfo._pass = (string)(System.Configuration.ConfigurationManager.AppSettings["Password"]); }
        return ConfigInfo._pass;
      }
    }

    private static Guid _prodId = Guid.Empty;
    public static Guid ProductId
    {
      get
      {
        if (ConfigInfo._prodId == Guid.Empty)
        {
          try { var id = (string)(System.Configuration.ConfigurationManager.AppSettings["ProductId"]); ConfigInfo._prodId = new Guid(id); }
          catch { }
        }
        return ConfigInfo._prodId;
      }
    }

    /// <summary>
    /// Test fax number
    /// </summary>
    private static string _testFax = null;
    public static string TestFax
    {
      get
      {
        if (ConfigInfo._testFax == null) { ConfigInfo._testFax = (string)(System.Configuration.ConfigurationManager.AppSettings["TestFax"]); }
        return ConfigInfo._testFax;
      }
    }

    /// <summary>
    /// Test fax number
    /// </summary>
    private static string _email = null;
    public static string Email
    {
      get
      {
        if (ConfigInfo._email == null) { ConfigInfo._email = (string)(System.Configuration.ConfigurationManager.AppSettings["TestEmail"]); }
        return ConfigInfo._email;
      }
    }







  }
}
