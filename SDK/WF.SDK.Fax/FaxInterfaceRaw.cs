using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WF.SDK.Models;
using WF.SDK.Models.Internal;

namespace WF.SDK.Fax.Internal
{
  public static class FaxInterfaceRaw
  {
    //The method name will be inserted in each call.
    public static string RestUrlTemplate { get; set; } //https://api2.westfax.com/REST/{0}/json

    public static AppType AppType { get; set; } = AppType.UnKnown;

    public static string AppSource { get; set; } //

    public static int HttpRequestTimeoutSecs = 1100;

    static FaxInterfaceRaw()
    {
      //You can configure this statically here and compile, or set the URL template above from another source such as confguration
      //FaxInterfaceRaw.RestUrlTemplate = System.Configuration.ConfigurationManager.AppSettings["APIEncoding"];  //From config?
      //FaxInterfaceRaw.RestUrlTemplate = "https://api2.westfax.com/REST/{0}/json";  //Statically?
      //Encoding can be json, json2, xml.
    }

    #region GetHttp (Private Helper)
    private static wwHttp GetHttp(string username, string password, Guid? productId = null, bool cookies = false, Guid? accountId = null)
    {
      wwHttp http = wwHttpApiHelper.GetHttp(username, password, productId, cookies, accountId, null, FaxInterfaceRaw.AppType, FaxInterfaceRaw.HttpRequestTimeoutSecs);
      return http;

      //wwHttp http = new wwHttp();
      //http.Timeout = FaxInterfaceRaw.HttpRequestTimeoutSecs;
      //http.PostMode = 2;
      //if (username != null) { http.AddPostKey("Username", username); }
      //if (password != null) { http.AddPostKey("Password", password); }
      //http.AddPostKey("Cookies", cookies.ToString());
      //if (productId.HasValue) { http.AddPostKey("ProductId", productId.Value.ToString()); }
      //if (accountId.HasValue) { http.AddPostKey("AccountId", accountId.Value.ToString()); }

      //http.AddPostKey("AppType", FaxInterfaceRaw.AppType.ToString());
      //return http;
    }

    private static wwHttp GetHttp(string apiKey, Guid? productId = null, bool cookies = false, Guid? accountId = null)
    {
      wwHttp http = wwHttpApiHelper.GetHttp(apiKey, productId, cookies, accountId, null, FaxInterfaceRaw.AppType, FaxInterfaceRaw.HttpRequestTimeoutSecs);
      return http;

      //wwHttp http = new wwHttp();
      //http.Timeout = FaxInterfaceRaw.HttpRequestTimeoutSecs;
      //http.PostMode = 2;
      //if (apiKey != null) { http.AddPostKey("ApiKey", apiKey); }
      //http.AddPostKey("Cookies", cookies.ToString());
      //if (productId.HasValue) { http.AddPostKey("ProductId", productId.Value.ToString()); }
      //if (accountId.HasValue) { http.AddPostKey("AccountId", accountId.Value.ToString()); }
      //return http;
    }
    #endregion

    #region GetResponseStr (Private Helper)
    private static string GetResponseStr(wwHttp http, string url)
    {
      var ret = http.GetUrl(url);
      if (http.Error) { ret = "{\"Success\":false,\"ErrorString\":\"" + http.ErrorMsg + "\",\"InfoString\":\"Error Calling API.\"}"; }
      return ret;
    }
    #endregion

    #region Ping
    public static string Ping(string pingStr)
    {
      string method = "Security_Ping";
      var http = FaxInterfaceRaw.GetHttp(null, null, null);

      List<string> arg = new List<string>() { pingStr };

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<string>(http, arg, "StringParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region Authenticate
    public static string Authenticate(string username, string password, Guid? productId = null)
    {
      string method = "Security_Authenticate";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    public static string Authenticate(string apiKey, Guid? productId = null)
    {
      string method = "Security_Authenticate";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetProductList
    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// </summary>
    public static string GetProductList(string username, string password, Guid? accountId = null)
    {
      string method = "Profile_GetProductList";
      var http = FaxInterfaceRaw.GetHttp(username, password, accountId: accountId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// </summary>
    public static string GetProductList(string apiKey, Guid? accountId = null)
    {
      string method = "Profile_GetProductList";
      var http = FaxInterfaceRaw.GetHttp(apiKey, accountId: accountId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetF2EProductList
    /// <summary>
    /// ProductType can be Fax 2 Email Only
    /// </summary>
    public static string GetF2EProductList(string username, string password)
    {
      string method = "Profile_GetF2EProductList";
      var http = FaxInterfaceRaw.GetHttp(username, password);
      http.Timeout = 180;
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// ProductType can be Fax 2 Email Only
    /// </summary>
    public static string GetF2EProductList(string apiKey)
    {
      string method = "Profile_GetF2EProductList";
      var http = FaxInterfaceRaw.GetHttp(apiKey);
      http.Timeout = 180;
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetF2EProductDetail
    /// <summary>
    /// Detail of product with counts
    /// </summary>
    public static string GetF2EProductDetail(string username, string password, Guid productId)
    {
      string method = "Profile_GetF2EProductDetail";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Detail of product with counts
    /// </summary>
    public static string GetF2EProductDetail(string apiKey, Guid productId)
    {
      string method = "Profile_GetF2EProductDetail";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region SearchAllProducts
    /// <summary>
    /// ProductType can be Fax 2 Email Only
    /// </summary>
    public static string SearchAllProducts(string username, string password,
      int page = 1, int itemsPerPage = 10, string type = "faxforward",
      string phoneMatch = "", string nameMatch = "", string nameSearch = "", string level = "User", string sortCol = "number", string sortDir = "asc",
      Guid? accountId = null)
    {
      string method = "Profile_GetProductList_PagedSearch";
      var http = FaxInterfaceRaw.GetHttp(username, password, accountId: accountId);

      http.AddPostKey("ProductType", type.ToString());

      //Paramater Names can be 
      // page - value is int - the page to get
      // count - value is int - the count per page
      // type - value is string - the product type to match
      // sort - value is string - the sort column (fax, name, etc)
      // phoneMatch - value is string - match string starting from first digit 
      // nameMatch - value is string - match string starting from beginning of product name
      // nameSearch - value is string - search product name for anywhere in string

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "page", Value = page.ToString() });
      items.Add(new NameValueItem() { Name = "count", Value = itemsPerPage.ToString() });
      items.Add(new NameValueItem() { Name = "type", Value = type.ToString() });
      items.Add(new NameValueItem() { Name = "sortCol", Value = sortCol });
      items.Add(new NameValueItem() { Name = "sortDir", Value = sortDir });
      items.Add(new NameValueItem() { Name = "phoneMatch", Value = phoneMatch });
      items.Add(new NameValueItem() { Name = "nameMatch", Value = nameMatch });
      items.Add(new NameValueItem() { Name = "nameSearch", Value = nameSearch });
      items.Add(new NameValueItem() { Name = "level", Value = level });

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// ProductType can be Fax 2 Email Only
    /// </summary>
    public static string SearchAllProducts(string apiKey,
      int page = 1, int itemsPerPage = 10, string type = "faxforward",
      string phoneMatch = "", string nameMatch = "", string nameSearch = "", string level = "User", string sortCol = "number", string sortDir = "asc",
      Guid? accountId = null)
    {
      string method = "Profile_GetProductList_PagedSearch";
      var http = FaxInterfaceRaw.GetHttp(apiKey, accountId: accountId);

      http.AddPostKey("ProductType", type.ToString());

      //Paramater Names can be 
      // page - value is int - the page to get
      // count - value is int - the count per page
      // type - value is string - the product type to match
      // sort - value is string - the sort column (fax, name, etc)
      // phoneMatch - value is string - match string starting from first digit 
      // nameMatch - value is string - match string starting from beginning of product name
      // nameSearch - value is string - search product name for anywhere in string

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "page", Value = page.ToString() });
      items.Add(new NameValueItem() { Name = "count", Value = itemsPerPage.ToString() });
      items.Add(new NameValueItem() { Name = "type", Value = type.ToString() });
      items.Add(new NameValueItem() { Name = "sortCol", Value = sortCol });
      items.Add(new NameValueItem() { Name = "sortDir", Value = sortDir });
      items.Add(new NameValueItem() { Name = "phoneMatch", Value = phoneMatch });
      items.Add(new NameValueItem() { Name = "nameMatch", Value = nameMatch });
      items.Add(new NameValueItem() { Name = "nameSearch", Value = nameSearch });
      items.Add(new NameValueItem() { Name = "level", Value = level });

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetAccountInfo
    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay.  
    /// </summary>
    public static string GetAccountInfo(string username, string password, Guid? productId = null, Guid? accountId = null)
    {
      string method = "Profile_GetAccountInfo";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId, accountId: accountId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay.  
    /// </summary>
    public static string GetAccountInfo(string apiKey, Guid? productId = null, Guid? accountId = null)
    {
      string method = "Profile_GetAccountInfo";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId, accountId: accountId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetUserProfile
    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// </summary>
    public static string GetUserProfile(string username, string password, Guid? productId = null)
    {
      string method = "Security_GetUserInfo";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetLoginInfo
    public static string GetLoginInfo(string username, string password)
    {
      string method = "Security_GetLoginInfo";
      var http = FaxInterfaceRaw.GetHttp(username, password);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetContactList
    public static string GetContactList(string username, string password, Guid? productId = null)
    {
      string method = "Contact_GetContactList";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    public static string GetContactList(string apiKey, Guid? productId = null)
    {
      string method = "Contact_GetContactList";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetAllContactList
    public static string GetAllContactList(string username, string password)
    {
      string method = "Contact_GetAllContactList";
      var http = FaxInterfaceRaw.GetHttp(username, password);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    public static string GetAllContactList(string apiKey)
    {
      string method = "Contact_GetAllContactList";
      var http = FaxInterfaceRaw.GetHttp(apiKey);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region SearchContactList_ForSend
    /// <summary>
    /// This method returns the viewable or usable contacts.  Editing may noit be possible on all of these.  
    /// Use this to get or filter a list of contacts when sending a fax.
    /// </summary>
    public static string SearchContactList_ForSend(string username, string password, Guid productId, int page = 1, int itemsPerPage = 10,
      string searchString = null, List<Models.ContactVisibility> filters = null)
    {
      string method = "Contact_GetContactList_ForSend_PagedSearch";
      string url = String.Format(FaxInterfaceRaw.RestUrlTemplate, method);

      wwHttp http = GetHttp(username, password, productId, false);
      //wwHttp http = new wwHttp();
      //http.PostMode = 2;
      //http.AddPostKey("Username", username);
      //http.AddPostKey("Password", password);
      //http.AddPostKey("ProductId", productId.ToString());
      //http.AddPostKey("Cookies", "false");

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "page", Value = page.ToString() });  // Mandatory
      items.Add(new NameValueItem() { Name = "count", Value = itemsPerPage.ToString() }); // Mandatory
      if (!String.IsNullOrEmpty(searchString)) { items.Add(new NameValueItem() { Name = "searchString", Value = searchString }); }//Optional
      if (filters != null && filters.Count > 0)
      {
        string filter = string.Join(",", filters.Select(i => i.ToString()).ToArray());
        items.Add(new NameValueItem() { Name = "filters", Value = filter }); //Optional
      }

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return http.GetUrl(url);
    }

    /// <summary>
    /// This method returns the viewable or usable contacts.  Editing may noit be possible on all of these.  
    /// Use this to get or filter a list of contacts when sending a fax.
    /// </summary>
    public static string SearchContactList_ForSend(string apiKey, Guid productId, int page = 1, int itemsPerPage = 10,
      string searchString = null, List<Models.ContactVisibility> filters = null)
    {
      string method = "Contact_GetContactList_ForSend_PagedSearch";
      string url = String.Format(FaxInterfaceRaw.RestUrlTemplate, method);

      wwHttp http = GetHttp(apiKey, productId, false);
      //wwHttp http = new wwHttp();
      //http.PostMode = 2;
      //http.AddPostKey("ApiKey", apiKey);
      //http.AddPostKey("ProductId", productId.ToString());
      //http.AddPostKey("Cookies", "false");

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "page", Value = page.ToString() });  // Mandatory
      items.Add(new NameValueItem() { Name = "count", Value = itemsPerPage.ToString() }); // Mandatory
      if (!String.IsNullOrEmpty(searchString)) { items.Add(new NameValueItem() { Name = "searchString", Value = searchString }); }//Optional
      if (filters != null && filters.Count > 0)
      {
        string filter = string.Join(",", filters.Select(i => i.ToString()).ToArray());
        items.Add(new NameValueItem() { Name = "filters", Value = filter }); //Optional
      }

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return http.GetUrl(url);
    }
    #endregion

    #region GetFaxIds (Inbound and Outbound)
    /// <summary>
    /// Get The inbound Faxes
    /// </summary>
    public static string GetFaxIds(string username, string password, Guid productId, bool include0PageCalls)
    {
      string method = "Fax_GetFaxIdentifiers";

      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "include0PageCalls", Value = include0PageCalls.ToString() });

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get The inbound Faxes
    /// </summary>
    public static string GetFaxIds(string apiKey, Guid productId, bool include0PageCalls)
    {
      string method = "Fax_GetFaxIdentifiers";

      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "include0PageCalls", Value = include0PageCalls.ToString() });

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetInboundFaxIds
    /// <summary>
    /// Get The inbound Faxes
    /// </summary>
    public static string GetInboundFaxIds(string username, string password, Guid productId, bool include0PageCalls)
    {
      string method = "Fax_GetFaxIdentifiers";

      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());
      http.AddPostKey("FaxDirection", "Inbound");

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "include0PageCalls", Value = include0PageCalls.ToString() });

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));

    }

    /// <summary>
    /// Get The inbound Faxes
    /// </summary>
    public static string GetInboundFaxIds(string apiKey, Guid productId, bool include0PageCalls)
    {
      string method = "Fax_GetFaxIdentifiers";

      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());
      http.AddPostKey("FaxDirection", "Inbound");

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "include0PageCalls", Value = include0PageCalls.ToString() });

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));

    }
    #endregion

    #region GetOutboundFaxIds
    /// <summary>
    /// Get The outbound Faxes
    /// </summary>
    public static string GetOutboundFaxIds(string username, string password, Guid productId)
    {
      string method = "Fax_GetFaxIdentifiers";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());
      http.AddPostKey("FaxDirection", "Outbound");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get The outbound Faxes
    /// </summary>
    public static string GetOutboundFaxIds(string apiKey, Guid productId)
    {
      string method = "Fax_GetFaxIdentifiers";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());
      http.AddPostKey("FaxDirection", "Outbound");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetInboundFaxDescriptions
    /// <summary>
    /// Get The inbound Faxes
    /// </summary>
    public static string GetInboundFaxDescriptions(string username, string password, Guid productId, bool include0PageCalls)
    {
      string method = "Fax_GetFaxDescriptions";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());
      http.AddPostKey("FaxDirection", "Inbound");

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "include0PageCalls", Value = include0PageCalls.ToString() });

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get The inbound Faxes
    /// </summary>
    public static string GetInboundFaxDescriptions(string apiKey, Guid productId, bool include0PageCalls)
    {
      string method = "Fax_GetFaxDescriptions";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());
      http.AddPostKey("FaxDirection", "Inbound");

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "include0PageCalls", Value = include0PageCalls.ToString() });

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetOutboundFaxDescriptions
    /// <summary>
    /// Get The outbound Faxes
    /// </summary>
    public static string GetOutboundFaxDescriptions(string username, string password, Guid productId)
    {
      string method = "Fax_GetFaxDescriptions";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());
      http.AddPostKey("FaxDirection", "Outbound");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get The outbound Faxes
    /// </summary>
    public static string GetOutboundFaxDescriptions(string apiKey, Guid productId)
    {
      string method = "Fax_GetFaxDescriptions";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());
      http.AddPostKey("FaxDirection", "Outbound");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetFaxDescriptions
    /// <summary>
    /// Get the requested Faxes
    /// </summary>
    public static string GetFaxDescriptions(string username, string password, Guid productId, List<FaxIdItem> items)
    {
      string method = "Fax_GetFaxDescriptionsUsingIds";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, items, "FaxIds");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get the requested Faxes
    /// </summary>
    public static string GetFaxDescriptions(string apiKey, Guid productId, List<FaxIdItem> items)
    {
      string method = "Fax_GetFaxDescriptionsUsingIds";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, items, "FaxIds");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region SearchF2EFaxDescriptions
    /// <summary>
    /// Get the requested Faxes
    /// Only use the ownerLoginId parameter to returned owned faxes only.
    /// For all faxes, leave the parameter null.
    /// </summary>
    public static string SearchF2EFaxDescriptions(string username, string password, Guid productId, Guid? ownerLoginId = null, int page = 1, int itemsPerPage = 10,
      string direction = "Inbound", string filter = null, string numberMatch = null, string csidSearch = null, string stateMatch = null, string cityMatch = null,
      DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {
      string method = "Fax_GetF2EFaxDescriptions_PagedSearch";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("FaxDirection", direction);
      if (!String.IsNullOrEmpty(csidSearch)) { http.AddPostKey("CSID", csidSearch); }  //Optional
      if (!String.IsNullOrEmpty(filter)) { http.AddPostKey("Filter", filter); }  //Optional
      if (startUtc.HasValue) { http.AddPostKey("StartDate", startUtc.Value.ToString("G")); }  //Optional
      if (endUtc.HasValue) { http.AddPostKey("StartDate", endUtc.Value.ToString("G")); }  //Optional

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "page", Value = page.ToString() });  // Mandatory
      items.Add(new NameValueItem() { Name = "count", Value = itemsPerPage.ToString() }); // Mandatory
      items.Add(new NameValueItem() { Name = "include0Page", Value = include0Page.ToString() }); // Optional
      if (ownerLoginId.HasValue) { items.Add(new NameValueItem() { Name = "ownerLoginId", Value = ownerLoginId.ToString() }); } // Optional
      if (!String.IsNullOrEmpty(numberMatch)) { items.Add(new NameValueItem() { Name = "numberMatch", Value = numberMatch }); }//Optional
      if (!String.IsNullOrEmpty(stateMatch)) { items.Add(new NameValueItem() { Name = "stateMatch", Value = stateMatch }); }//Optional
      if (!String.IsNullOrEmpty(cityMatch)) { items.Add(new NameValueItem() { Name = "cityMatch", Value = cityMatch }); }//Optional

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get the requested Faxes
    /// Only use the ownerLoginId parameter to returned owned faxes only.
    /// For all faxes, leave the parameter null.
    /// </summary>
    public static string SearchF2EFaxDescriptions(string apiKey, Guid productId, Guid? ownerLoginId = null, int page = 1, int itemsPerPage = 10,
      string direction = "Inbound", string filter = null, string numberMatch = null, string csidSearch = null, string stateMatch = null, string cityMatch = null,
      DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {
      string method = "Fax_GetF2EFaxDescriptions_PagedSearch";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      http.AddPostKey("FaxDirection", direction);
      if (!String.IsNullOrEmpty(csidSearch)) { http.AddPostKey("CSID", csidSearch); }  //Optional
      if (!String.IsNullOrEmpty(filter)) { http.AddPostKey("Filter", filter); }  //Optional
      if (startUtc.HasValue) { http.AddPostKey("StartDate", startUtc.Value.ToString("G")); }  //Optional
      if (endUtc.HasValue) { http.AddPostKey("StartDate", endUtc.Value.ToString("G")); }  //Optional

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "page", Value = page.ToString() });  // Mandatory
      items.Add(new NameValueItem() { Name = "count", Value = itemsPerPage.ToString() }); // Mandatory
      items.Add(new NameValueItem() { Name = "include0Page", Value = include0Page.ToString() }); // Optional
      if (ownerLoginId.HasValue) { items.Add(new NameValueItem() { Name = "ownerLoginId", Value = ownerLoginId.ToString() }); } // Optional
      if (!String.IsNullOrEmpty(numberMatch)) { items.Add(new NameValueItem() { Name = "numberMatch", Value = numberMatch }); }//Optional
      if (!String.IsNullOrEmpty(stateMatch)) { items.Add(new NameValueItem() { Name = "stateMatch", Value = stateMatch }); }//Optional
      if (!String.IsNullOrEmpty(cityMatch)) { items.Add(new NameValueItem() { Name = "cityMatch", Value = cityMatch }); }//Optional

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetF2EFaxDescriptions_PagedSearch
    /// <summary>
    /// Get the requested Faxes
    /// Only use the ownerLoginId parameter to returned owned faxes only.
    /// For all faxes, leave the parameter null.
    /// </summary>
    public static string GetF2EFaxDescriptions_PagedSearch(string username, string password, Guid productId, Guid? ownerLoginId = null, int page = 1, int itemsPerPage = 10,
      string direction = "Inbound", List<string> filterList = null, string numberMatch = null, string stringMatch = null, string matchType = "Or",
      DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {
      string method = "Fax_GetF2EFaxDescriptions_PagedSearch";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("FaxDirection", direction);
      if (filterList != null) { Common.wwHttpHelper.AddParameterList<string>(http, filterList, "FilterList"); }
      if (startUtc.HasValue) { http.AddPostKey("StartDate", startUtc.Value.ToString("G")); }  //Optional
      if (endUtc.HasValue) { http.AddPostKey("EndDate", endUtc.Value.ToString("G")); }  //Optional

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "page", Value = page.ToString() });  // Mandatory
      items.Add(new NameValueItem() { Name = "count", Value = itemsPerPage.ToString() }); // Mandatory
      items.Add(new NameValueItem() { Name = "include0Page", Value = include0Page.ToString() }); // Optional
      if (ownerLoginId.HasValue) { items.Add(new NameValueItem() { Name = "ownerLoginId", Value = ownerLoginId.ToString() }); } // Optional
      if (!String.IsNullOrEmpty(numberMatch)) { items.Add(new NameValueItem() { Name = "numberMatch", Value = numberMatch }); }//Optional
      if (!String.IsNullOrEmpty(stringMatch)) { items.Add(new NameValueItem() { Name = "stringMatch", Value = stringMatch }); }//Optional
      items.Add(new NameValueItem() { Name = "matchType", Value = matchType });

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get the requested Faxes
    /// Only use the ownerLoginId parameter to returned owned faxes only.
    /// For all faxes, leave the parameter null.
    /// </summary>
    public static string GetF2EFaxDescriptions_PagedSearch(string apiKey, Guid productId, Guid? ownerLoginId = null, int page = 1, int itemsPerPage = 10,
      string direction = "Inbound", List<string> filterList = null, string numberMatch = null, string stringMatch = null, string matchType = "Or",
      DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {
      string method = "Fax_GetF2EFaxDescriptions_PagedSearch";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      http.AddPostKey("FaxDirection", direction);
      if (filterList != null) { Common.wwHttpHelper.AddParameterList<string>(http, filterList, "FilterList"); }
      if (startUtc.HasValue) { http.AddPostKey("StartDate", startUtc.Value.ToString("G")); }  //Optional
      if (endUtc.HasValue) { http.AddPostKey("EndDate", endUtc.Value.ToString("G")); }  //Optional

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "page", Value = page.ToString() });  // Mandatory
      items.Add(new NameValueItem() { Name = "count", Value = itemsPerPage.ToString() }); // Mandatory
      items.Add(new NameValueItem() { Name = "include0Page", Value = include0Page.ToString() }); // Optional
      if (ownerLoginId.HasValue) { items.Add(new NameValueItem() { Name = "ownerLoginId", Value = ownerLoginId.ToString() }); } // Optional
      if (!String.IsNullOrEmpty(numberMatch)) { items.Add(new NameValueItem() { Name = "numberMatch", Value = numberMatch }); }//Optional
      if (!String.IsNullOrEmpty(stringMatch)) { items.Add(new NameValueItem() { Name = "stringMatch", Value = stringMatch }); }//Optional
      items.Add(new NameValueItem() { Name = "matchType", Value = matchType });

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetFaxDocuments
    /// <summary>
    /// Get the requested Faxes
    /// </summary>
    public static string GetFaxDocuments(string username, string password, Guid productId, List<FaxIdItem> items, string format = "pdf")
    {
      string method = "Fax_GetFaxDocuments";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("Format", format);
      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, items, "FaxIds");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get the requested Faxes
    /// </summary>
    public static string GetFaxDocuments(string apiKey, Guid productId, List<FaxIdItem> items, string format = "pdf")
    {
      string method = "Fax_GetFaxDocuments";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      http.AddPostKey("Format", format);
      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, items, "FaxIds");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetProductsWithInboundFaxes
    /// <summary>
    /// Get the products that have faxes matching the given filter.  Usually used to 
    /// determine if there are faxes waiting for download, and what products may have them.
    /// </summary>
    public static string GetProductsWithInboundFaxes(string username, string password, string filter = "None", bool include0PageCalls = false)
    {
      string method = "Fax_GetProductsWithInboundFaxes";
      var http = FaxInterfaceRaw.GetHttp(username, password);

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "include0PageCalls", Value = include0PageCalls.ToString() });

      http.AddPostKey("Filter", filter);

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get the products that have faxes matching the given filter.  Usually used to 
    /// determine if there are faxes waiting for download, and what products may have them.
    /// </summary>
    public static string GetProductsWithInboundFaxes(string apiKey, string filter = "None", bool include0PageCalls = false)
    {
      string method = "Fax_GetProductsWithInboundFaxes";
      var http = FaxInterfaceRaw.GetHttp(apiKey);

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "include0PageCalls", Value = include0PageCalls.ToString() });

      http.AddPostKey("Filter", filter);

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region SendFax - HttpFileCollection
    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId, List<string> numbers,
      HttpFileCollection files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {

      //Username : <input name="Username" type="text" value="<?php print $username; ?>"><br>
      //Password : <input name="Password" type="text" value="<?php print $password; ?>"><br>
      //ProductId : <input name="ProductId" type="text" value="<?php print $prodid;?>"><br>
      //Number 1 : <input name="Numbers1" type="text" value="8002075529"><br>
      //Number 2 : <input name="Numbers2" type="text" value="8002075529"><br>
      //JobName : <input name="JobName" type="text" value="Test 1234"><br>
      //Header : <input name="Header" type="text" value="Test Header"><br>
      //Billing Code : <input name="BillingCode" type="text" value="Billing Code 123"><br>
      //Csid : <input name="CSID" type="text" value="My Company 1234567890"><br>
      //Quality : <input name="FaxQuality" type="text" value="Normal"><br>
      //Start : <input name="StartDate" type="text" value="6/1/2015"><br>
      //Feedback : <input name="FeedbackEmail" type="text" value="<?php print $feedback;?>"><br>
      //File 1 : <input name="Files1" type="file"><br>
      //File 2 : <input name="Files2" type="file"><br>

      string method = "Fax_SendFax";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      //Add the numbers collection
      Common.wwHttpHelper.AddParameterList(http, numbers, "Numbers");  //Required

      //These will get tacked on to the CustomerKey1
      //Make sure the Names and numbers collections are the same.  Otherwise don't include.
      if (custKeys != null && custKeys.Count == numbers.Count)
      {
        Common.wwHttpHelper.AddParameterList(http, custKeys, "StringParams");  //Not Required
      }

      //Add the files collection 
      //Required
      for (int i = 0; i < files.Count; i++)
      {
        byte[] lcFile = new byte[files[i].ContentLength];
        files[i].InputStream.Read(lcFile, 0, (int)files[i].InputStream.Length);
        http.AddPostFile("Files" + (i + 1).ToString(), lcFile, files[i].FileName);
      }

      http.AddPostKey("JobName", jobname);  //Required
      http.AddPostKey("Header", header);  //Required
      http.AddPostKey("BillingCode", billingCode);  //Required

      if (!String.IsNullOrEmpty(csid)) { http.AddPostKey("CSID", csid); }  //Optional
      if (!String.IsNullOrEmpty(ani)) { http.AddPostKey("ANI", ani); }  //Optional
      if (startDate.HasValue) { http.AddPostKey("StartDate", startDate.Value.ToString("G")); }  //Optional
      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(feedbackEmail)) { http.AddPostKey("FeedbackEmail", feedbackEmail); }  //Optional
      if (!String.IsNullOrEmpty(callbackUrl)) { http.AddPostKey("CallbackUrl", callbackUrl); }  //Optional
      if (!String.IsNullOrEmpty(FaxInterfaceRaw.AppSource)) { http.AddPostKey("JobSource", FaxInterfaceRaw.AppSource); }  //Optional

      //Add the extra params if there are any
      if (extraParams != null && extraParams.Count > 0)
      {
        List<NameValueItem> items = new List<NameValueItem>();
        foreach (var param in extraParams) { items.Add(new NameValueItem() { Name = param.Key, Value = param.Value }); }
        Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");
      }

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string apiKey, Guid productId, List<string> numbers,
      HttpFileCollection files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {

      //Username : <input name="Username" type="text" value="<?php print $username; ?>"><br>
      //Password : <input name="Password" type="text" value="<?php print $password; ?>"><br>
      //ProductId : <input name="ProductId" type="text" value="<?php print $prodid;?>"><br>
      //Number 1 : <input name="Numbers1" type="text" value="8002075529"><br>
      //Number 2 : <input name="Numbers2" type="text" value="8002075529"><br>
      //JobName : <input name="JobName" type="text" value="Test 1234"><br>
      //Header : <input name="Header" type="text" value="Test Header"><br>
      //Billing Code : <input name="BillingCode" type="text" value="Billing Code 123"><br>
      //Csid : <input name="CSID" type="text" value="My Company 1234567890"><br>
      //Quality : <input name="FaxQuality" type="text" value="Normal"><br>
      //Start : <input name="StartDate" type="text" value="6/1/2015"><br>
      //Feedback : <input name="FeedbackEmail" type="text" value="<?php print $feedback;?>"><br>
      //File 1 : <input name="Files1" type="file"><br>
      //File 2 : <input name="Files2" type="file"><br>

      string method = "Fax_SendFax";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      //Add the numbers collection
      Common.wwHttpHelper.AddParameterList(http, numbers, "Numbers");  //Required

      //These will get tacked on to the CustomerKey1
      //Make sure the Names and numbers collections are the same.  Otherwise don't include.
      if (custKeys != null && custKeys.Count == numbers.Count)
      {
        Common.wwHttpHelper.AddParameterList(http, custKeys, "StringParams");  //Not Required
      }

      //Add the files collection 
      //Required
      for (int i = 0; i < files.Count; i++)
      {
        byte[] lcFile = new byte[files[i].ContentLength];
        files[i].InputStream.Read(lcFile, 0, (int)files[i].InputStream.Length);
        http.AddPostFile("Files" + (i + 1).ToString(), lcFile, files[i].FileName);
      }

      http.AddPostKey("JobName", jobname);  //Required
      http.AddPostKey("Header", header);  //Required
      http.AddPostKey("BillingCode", billingCode);  //Required

      if (!String.IsNullOrEmpty(csid)) { http.AddPostKey("CSID", csid); }  //Optional
      if (!String.IsNullOrEmpty(ani)) { http.AddPostKey("ANI", ani); }  //Optional
      if (startDate.HasValue) { http.AddPostKey("StartDate", startDate.Value.ToString("G")); }  //Optional
      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(feedbackEmail)) { http.AddPostKey("FeedbackEmail", feedbackEmail); }  //Optional
      if (!String.IsNullOrEmpty(callbackUrl)) { http.AddPostKey("CallbackUrl", callbackUrl); }  //Optional
      if (!String.IsNullOrEmpty(FaxInterfaceRaw.AppSource)) { http.AddPostKey("JobSource", FaxInterfaceRaw.AppSource); }  //Optional

      //Add the extra params if there are any
      if (extraParams != null && extraParams.Count > 0)
      {
        List<NameValueItem> items = new List<NameValueItem>();
        foreach (var param in extraParams) { items.Add(new NameValueItem() { Name = param.Key, Value = param.Value }); }
        Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");
      }

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId, List<string> numbers,
      HttpFileCollection files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmailItem feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      return FaxInterfaceRaw.SendFax(username, password, productId, numbers, files, csid, ani, startDate, faxQuality, jobname, header, billingCode,
        WF.SDK.Common.JSONSerializerHelper.SerializeToString(feedbackEmail),
        callbackUrl, custKeys, extraParams);
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string apiKey, Guid productId, List<string> numbers,
      HttpFileCollection files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmailItem feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      return FaxInterfaceRaw.SendFax(apiKey, productId, numbers, files, csid, ani, startDate, faxQuality, jobname, header, billingCode,
        WF.SDK.Common.JSONSerializerHelper.SerializeToString(feedbackEmail),
        callbackUrl, custKeys, extraParams);
    }

    #endregion

    #region SendFax - Overload for List<HttpPostedFile>
    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId, List<string> numbers,
      List<HttpPostedFile> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {

      //Username : <input name="Username" type="text" value="<?php print $username; ?>"><br>
      //Password : <input name="Password" type="text" value="<?php print $password; ?>"><br>
      //ProductId : <input name="ProductId" type="text" value="<?php print $prodid;?>"><br>
      //Number 1 : <input name="Numbers1" type="text" value="8002075529"><br>
      //Number 2 : <input name="Numbers2" type="text" value="8002075529"><br>
      //JobName : <input name="JobName" type="text" value="Test 1234"><br>
      //Header : <input name="Header" type="text" value="Test Header"><br>
      //Billing Code : <input name="BillingCode" type="text" value="Billing Code 123"><br>
      //Csid : <input name="CSID" type="text" value="My Company 1234567890"><br>
      //Quality : <input name="FaxQuality" type="text" value="Normal"><br>
      //Start : <input name="StartDate" type="text" value="6/1/2015"><br>
      //Feedback : <input name="FeedbackEmail" type="text" value="<?php print $feedback;?>"><br>
      //File 1 : <input name="Files1" type="file"><br>
      //File 2 : <input name="Files2" type="file"><br>

      string method = "Fax_SendFax";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      //Add the numbers collection
      Common.wwHttpHelper.AddParameterList(http, numbers, "Numbers");  //Required

      //These will get tacked on to the CustomerKey1
      //Make sure the Names and numbers collections are the same.  Otherwise don't include.
      if (custKeys != null && custKeys.Count == numbers.Count)
      {
        Common.wwHttpHelper.AddParameterList(http, custKeys, "StringParams");  //Not Required
      }

      //Add the files collection 
      //Required
      for (int i = 0; i < files.Count; i++)
      {
        byte[] lcFile = new byte[files[i].ContentLength];
        files[i].InputStream.Read(lcFile, 0, (int)files[i].InputStream.Length);
        http.AddPostFile("Files" + (i + 1).ToString(), lcFile, files[i].FileName);
      }

      http.AddPostKey("JobName", jobname);  //Required
      http.AddPostKey("Header", header);  //Required
      http.AddPostKey("BillingCode", billingCode);  //Required

      if (!String.IsNullOrEmpty(csid)) { http.AddPostKey("CSID", csid); }  //Optional
      if (!String.IsNullOrEmpty(ani)) { http.AddPostKey("ANI", ani); }  //Optional
      if (startDate.HasValue) { http.AddPostKey("StartDate", startDate.Value.ToString("G")); }  //Optional
      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(feedbackEmail)) { http.AddPostKey("FeedbackEmail", feedbackEmail); }  //Optional
      if (!String.IsNullOrEmpty(callbackUrl)) { http.AddPostKey("CallbackUrl", callbackUrl); }  //Optional
      if (!String.IsNullOrEmpty(FaxInterfaceRaw.AppSource)) { http.AddPostKey("JobSource", FaxInterfaceRaw.AppSource); }  //Optional

      //Add the extra params if there are any
      if (extraParams != null && extraParams.Count > 0)
      {
        List<NameValueItem> items = new List<NameValueItem>();
        foreach (var param in extraParams) { items.Add(new NameValueItem() { Name = param.Key, Value = param.Value }); }
        Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");
      }

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string apiKey, Guid productId, List<string> numbers,
      List<HttpPostedFile> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {

      //Username : <input name="Username" type="text" value="<?php print $username; ?>"><br>
      //Password : <input name="Password" type="text" value="<?php print $password; ?>"><br>
      //ProductId : <input name="ProductId" type="text" value="<?php print $prodid;?>"><br>
      //Number 1 : <input name="Numbers1" type="text" value="8002075529"><br>
      //Number 2 : <input name="Numbers2" type="text" value="8002075529"><br>
      //JobName : <input name="JobName" type="text" value="Test 1234"><br>
      //Header : <input name="Header" type="text" value="Test Header"><br>
      //Billing Code : <input name="BillingCode" type="text" value="Billing Code 123"><br>
      //Csid : <input name="CSID" type="text" value="My Company 1234567890"><br>
      //Quality : <input name="FaxQuality" type="text" value="Normal"><br>
      //Start : <input name="StartDate" type="text" value="6/1/2015"><br>
      //Feedback : <input name="FeedbackEmail" type="text" value="<?php print $feedback;?>"><br>
      //File 1 : <input name="Files1" type="file"><br>
      //File 2 : <input name="Files2" type="file"><br>

      string method = "Fax_SendFax";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      //Add the numbers collection
      Common.wwHttpHelper.AddParameterList(http, numbers, "Numbers");  //Required

      //These will get tacked on to the CustomerKey1
      //Make sure the Names and numbers collections are the same.  Otherwise don't include.
      if (custKeys != null && custKeys.Count == numbers.Count)
      {
        Common.wwHttpHelper.AddParameterList(http, custKeys, "StringParams");  //Not Required
      }

      //Add the files collection 
      //Required
      for (int i = 0; i < files.Count; i++)
      {
        byte[] lcFile = new byte[files[i].ContentLength];
        files[i].InputStream.Read(lcFile, 0, (int)files[i].InputStream.Length);
        http.AddPostFile("Files" + (i + 1).ToString(), lcFile, files[i].FileName);
      }

      http.AddPostKey("JobName", jobname);  //Required
      http.AddPostKey("Header", header);  //Required
      http.AddPostKey("BillingCode", billingCode);  //Required

      if (!String.IsNullOrEmpty(csid)) { http.AddPostKey("CSID", csid); }  //Optional
      if (!String.IsNullOrEmpty(ani)) { http.AddPostKey("ANI", ani); }  //Optional
      if (startDate.HasValue) { http.AddPostKey("StartDate", startDate.Value.ToString("G")); }  //Optional
      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(feedbackEmail)) { http.AddPostKey("FeedbackEmail", feedbackEmail); }  //Optional
      if (!String.IsNullOrEmpty(callbackUrl)) { http.AddPostKey("CallbackUrl", callbackUrl); }  //Optional
      if (!String.IsNullOrEmpty(FaxInterfaceRaw.AppSource)) { http.AddPostKey("JobSource", FaxInterfaceRaw.AppSource); }  //Optional

      //Add the extra params if there are any
      if (extraParams != null && extraParams.Count > 0)
      {
        List<NameValueItem> items = new List<NameValueItem>();
        foreach (var param in extraParams) { items.Add(new NameValueItem() { Name = param.Key, Value = param.Value }); }
        Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");
      }

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId, List<string> numbers,
      List<HttpPostedFile> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmailItem feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      return FaxInterfaceRaw.SendFax(username, password, productId, numbers, files, csid, ani, startDate, faxQuality, jobname, header, billingCode,
        WF.SDK.Common.JSONSerializerHelper.SerializeToString(feedbackEmail),
        callbackUrl, custKeys, extraParams);
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string apiKey, Guid productId, List<string> numbers,
      List<HttpPostedFile> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmailItem feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      return FaxInterfaceRaw.SendFax(apiKey, productId, numbers, files, csid, ani, startDate, faxQuality, jobname, header, billingCode,
        WF.SDK.Common.JSONSerializerHelper.SerializeToString(feedbackEmail),
        callbackUrl, custKeys, extraParams);
    }
    #endregion

    #region SendFax - Overload for List<FileItem>
    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId, List<string> numbers,
      List<FileItem> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys1 = null, Dictionary<string, string> extraParams = null)
    {

      //Username : <input name="Username" type="text" value="<?php print $username; ?>"><br>
      //Password : <input name="Password" type="text" value="<?php print $password; ?>"><br>
      //ProductId : <input name="ProductId" type="text" value="<?php print $prodid;?>"><br>
      //Number 1 : <input name="Numbers1" type="text" value="8002075529"><br>
      //Number 2 : <input name="Numbers2" type="text" value="8002075529"><br>
      //JobName : <input name="JobName" type="text" value="Test 1234"><br>
      //Header : <input name="Header" type="text" value="Test Header"><br>
      //Billing Code : <input name="BillingCode" type="text" value="Billing Code 123"><br>
      //Csid : <input name="CSID" type="text" value="My Company 1234567890"><br>
      //Quality : <input name="FaxQuality" type="text" value="Normal"><br>
      //Start : <input name="StartDate" type="text" value="6/1/2015"><br>
      //Feedback : <input name="FeedbackEmail" type="text" value="<?php print $feedback;?>"><br>
      //File 1 : <input name="Files1" type="file"><br>
      //File 2 : <input name="Files2" type="file"><br>

      string method = "Fax_SendFax";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      //Add the numbers collection
      Common.wwHttpHelper.AddParameterList(http, numbers, "Numbers");  //Required

      //These will get tacked on to the CustomerKey1
      //Make sure the Names and numbers collections are the same.  Otherwise don't include.
      if (custKeys1 != null && custKeys1.Count == numbers.Count)
      {
        Common.wwHttpHelper.AddParameterList(http, custKeys1, "StringParams");  //Not Required
      }

      http.AddPostKey("JobName", jobname);  //Required
      http.AddPostKey("Header", header);  //Required
      http.AddPostKey("BillingCode", billingCode);  //Required

      if (!String.IsNullOrEmpty(csid)) { http.AddPostKey("CSID", csid); }  //Optional
      if (!String.IsNullOrEmpty(ani)) { http.AddPostKey("ANI", ani); }  //Optional
      if (startDate.HasValue) { http.AddPostKey("StartDate", startDate.Value.ToString("G")); }  //Optional
      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(feedbackEmail)) { http.AddPostKey("FeedbackEmail", feedbackEmail); }  //Optional
      if (!String.IsNullOrEmpty(callbackUrl)) { http.AddPostKey("CallbackUrl", callbackUrl); }  //Optional
      if (!String.IsNullOrEmpty(FaxInterfaceRaw.AppSource)) { http.AddPostKey("JobSource", FaxInterfaceRaw.AppSource); }  //Optional

      //Add the extra params if there are any
      if (extraParams != null && extraParams.Count > 0)
      {
        List<NameValueItem> items = new List<NameValueItem>();
        foreach (var param in extraParams) { items.Add(new NameValueItem() { Name = param.Key, Value = param.Value }); }
        Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");
      }

      // This is last so that the most important information comes first.
      //Add the files collection 
      //Required
      for (int i = 0; i < files.Count; i++)
      {
        http.AddPostFile("Files" + (i + 1).ToString(), files[i].FileContents, files[i].Filename);
      }

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string apiKey, Guid productId, List<string> numbers,
      List<FileItem> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys1 = null, Dictionary<string, string> extraParams = null)
    {

      //Username : <input name="Username" type="text" value="<?php print $username; ?>"><br>
      //Password : <input name="Password" type="text" value="<?php print $password; ?>"><br>
      //ProductId : <input name="ProductId" type="text" value="<?php print $prodid;?>"><br>
      //Number 1 : <input name="Numbers1" type="text" value="8002075529"><br>
      //Number 2 : <input name="Numbers2" type="text" value="8002075529"><br>
      //JobName : <input name="JobName" type="text" value="Test 1234"><br>
      //Header : <input name="Header" type="text" value="Test Header"><br>
      //Billing Code : <input name="BillingCode" type="text" value="Billing Code 123"><br>
      //Csid : <input name="CSID" type="text" value="My Company 1234567890"><br>
      //Quality : <input name="FaxQuality" type="text" value="Normal"><br>
      //Start : <input name="StartDate" type="text" value="6/1/2015"><br>
      //Feedback : <input name="FeedbackEmail" type="text" value="<?php print $feedback;?>"><br>
      //File 1 : <input name="Files1" type="file"><br>
      //File 2 : <input name="Files2" type="file"><br>

      string method = "Fax_SendFax";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      //Add the numbers collection
      Common.wwHttpHelper.AddParameterList(http, numbers, "Numbers");  //Required

      //These will get tacked on to the CustomerKey1
      //Make sure the Names and numbers collections are the same.  Otherwise don't include.
      if (custKeys1 != null && custKeys1.Count == numbers.Count)
      {
        Common.wwHttpHelper.AddParameterList(http, custKeys1, "StringParams");  //Not Required
      }

      http.AddPostKey("JobName", jobname);  //Required
      http.AddPostKey("Header", header);  //Required
      http.AddPostKey("BillingCode", billingCode);  //Required

      if (!String.IsNullOrEmpty(csid)) { http.AddPostKey("CSID", csid); }  //Optional
      if (!String.IsNullOrEmpty(ani)) { http.AddPostKey("ANI", ani); }  //Optional
      if (startDate.HasValue) { http.AddPostKey("StartDate", startDate.Value.ToString("G")); }  //Optional
      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(feedbackEmail)) { http.AddPostKey("FeedbackEmail", feedbackEmail); }  //Optional
      if (!String.IsNullOrEmpty(callbackUrl)) { http.AddPostKey("CallbackUrl", callbackUrl); }  //Optional
      if (!String.IsNullOrEmpty(FaxInterfaceRaw.AppSource)) { http.AddPostKey("JobSource", FaxInterfaceRaw.AppSource); }  //Optional

      //Add the extra params if there are any
      if (extraParams != null && extraParams.Count > 0)
      {
        List<NameValueItem> items = new List<NameValueItem>();
        foreach (var param in extraParams) { items.Add(new NameValueItem() { Name = param.Key, Value = param.Value }); }
        Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");
      }

      // This is last so that the most important information comes first.
      //Add the files collection 
      //Required
      for (int i = 0; i < files.Count; i++)
      {
        http.AddPostFile("Files" + (i + 1).ToString(), files[i].FileContents, files[i].Filename);
      }

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }


    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId, List<string> numbers,
      List<FileItem> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmailItem feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      return FaxInterfaceRaw.SendFax(username, password, productId, numbers, files, csid, ani, startDate, faxQuality, jobname, header, billingCode,
        WF.SDK.Common.JSONSerializerHelper.SerializeToString(feedbackEmail),
        callbackUrl, custKeys, extraParams);
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string apiKey, Guid productId, List<string> numbers,
      List<FileItem> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmailItem feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      return FaxInterfaceRaw.SendFax(apiKey, productId, numbers, files, csid, ani, startDate, faxQuality, jobname, header, billingCode,
        WF.SDK.Common.JSONSerializerHelper.SerializeToString(feedbackEmail),
        callbackUrl, custKeys, extraParams);
    }
    #endregion

    #region SendFax - Overload for Byte []
    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId, List<string> numbers,
      byte[] fileContent, string fileName,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys1 = null, Dictionary<string, string> extraParams = null)
    {

      //Username : <input name="Username" type="text" value="<?php print $username; ?>"><br>
      //Password : <input name="Password" type="text" value="<?php print $password; ?>"><br>
      //ProductId : <input name="ProductId" type="text" value="<?php print $prodid;?>"><br>
      //Number 1 : <input name="Numbers1" type="text" value="8002075529"><br>
      //Number 2 : <input name="Numbers2" type="text" value="8002075529"><br>
      //JobName : <input name="JobName" type="text" value="Test 1234"><br>
      //Header : <input name="Header" type="text" value="Test Header"><br>
      //Billing Code : <input name="BillingCode" type="text" value="Billing Code 123"><br>
      //Csid : <input name="CSID" type="text" value="My Company 1234567890"><br>
      //Quality : <input name="FaxQuality" type="text" value="Normal"><br>
      //Start : <input name="StartDate" type="text" value="6/1/2015"><br>
      //Feedback : <input name="FeedbackEmail" type="text" value="<?php print $feedback;?>"><br>
      //File 1 : <input name="Files1" type="file"><br>
      //File 2 : <input name="Files2" type="file"><br>

      string method = "Fax_SendFax";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      //Add the numbers collection
      Common.wwHttpHelper.AddParameterList(http, numbers, "Numbers");  //Required

      //These will get tacked on to the CustomerKey1
      //Make sure the Names and numbers collections are the same.  Otherwise don't include.
      if (custKeys1 != null && custKeys1.Count == numbers.Count)
      {
        Common.wwHttpHelper.AddParameterList(http, custKeys1, "StringParams");  //Not Required
      }

      //Add the files collection 
      //Required
      if (fileName == null) { fileName = "FaxDocument.pdf"; }
      http.AddPostFile("Files1", fileContent, fileName);

      http.AddPostKey("JobName", jobname);  //Required
      http.AddPostKey("Header", header);  //Required
      http.AddPostKey("BillingCode", billingCode);  //Required

      if (!String.IsNullOrEmpty(csid)) { http.AddPostKey("CSID", csid); }  //Optional
      if (!String.IsNullOrEmpty(ani)) { http.AddPostKey("ANI", ani); }  //Optional
      if (startDate.HasValue) { http.AddPostKey("StartDate", startDate.Value.ToString("G")); }  //Optional
      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(feedbackEmail)) { http.AddPostKey("FeedbackEmail", feedbackEmail); }  //Optional
      if (!String.IsNullOrEmpty(callbackUrl)) { http.AddPostKey("CallbackUrl", callbackUrl); }  //Optional
      if (!String.IsNullOrEmpty(FaxInterfaceRaw.AppSource)) { http.AddPostKey("JobSource", FaxInterfaceRaw.AppSource); }  //Optional

      //Add the extra params if there are any
      if (extraParams != null && extraParams.Count > 0)
      {
        List<NameValueItem> items = new List<NameValueItem>();
        foreach (var param in extraParams) { items.Add(new NameValueItem() { Name = param.Key, Value = param.Value }); }
        Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");
      }

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string apiKey, Guid productId, List<string> numbers,
      byte[] fileContent, string fileName,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys1 = null, Dictionary<string, string> extraParams = null)
    {

      //Username : <input name="Username" type="text" value="<?php print $username; ?>"><br>
      //Password : <input name="Password" type="text" value="<?php print $password; ?>"><br>
      //ProductId : <input name="ProductId" type="text" value="<?php print $prodid;?>"><br>
      //Number 1 : <input name="Numbers1" type="text" value="8002075529"><br>
      //Number 2 : <input name="Numbers2" type="text" value="8002075529"><br>
      //JobName : <input name="JobName" type="text" value="Test 1234"><br>
      //Header : <input name="Header" type="text" value="Test Header"><br>
      //Billing Code : <input name="BillingCode" type="text" value="Billing Code 123"><br>
      //Csid : <input name="CSID" type="text" value="My Company 1234567890"><br>
      //Quality : <input name="FaxQuality" type="text" value="Normal"><br>
      //Start : <input name="StartDate" type="text" value="6/1/2015"><br>
      //Feedback : <input name="FeedbackEmail" type="text" value="<?php print $feedback;?>"><br>
      //File 1 : <input name="Files1" type="file"><br>
      //File 2 : <input name="Files2" type="file"><br>

      string method = "Fax_SendFax";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      //Add the numbers collection
      Common.wwHttpHelper.AddParameterList(http, numbers, "Numbers");  //Required

      //These will get tacked on to the CustomerKey1
      //Make sure the Names and numbers collections are the same.  Otherwise don't include.
      if (custKeys1 != null && custKeys1.Count == numbers.Count)
      {
        Common.wwHttpHelper.AddParameterList(http, custKeys1, "StringParams");  //Not Required
      }

      //Add the files collection 
      //Required
      if (fileName == null) { fileName = "FaxDocument.pdf"; }
      http.AddPostFile("Files1", fileContent, fileName);

      http.AddPostKey("JobName", jobname);  //Required
      http.AddPostKey("Header", header);  //Required
      http.AddPostKey("BillingCode", billingCode);  //Required

      if (!String.IsNullOrEmpty(csid)) { http.AddPostKey("CSID", csid); }  //Optional
      if (!String.IsNullOrEmpty(ani)) { http.AddPostKey("ANI", ani); }  //Optional
      if (startDate.HasValue) { http.AddPostKey("StartDate", startDate.Value.ToString("G")); }  //Optional
      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(feedbackEmail)) { http.AddPostKey("FeedbackEmail", feedbackEmail); }  //Optional
      if (!String.IsNullOrEmpty(callbackUrl)) { http.AddPostKey("CallbackUrl", callbackUrl); }  //Optional
      if (!String.IsNullOrEmpty(FaxInterfaceRaw.AppSource)) { http.AddPostKey("JobSource", FaxInterfaceRaw.AppSource); }  //Optional

      //Add the extra params if there are any
      if (extraParams != null && extraParams.Count > 0)
      {
        List<NameValueItem> items = new List<NameValueItem>();
        foreach (var param in extraParams) { items.Add(new NameValueItem() { Name = param.Key, Value = param.Value }); }
        Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");
      }

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }


    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId, List<string> numbers,
      byte[] fileContent, string fileName,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmailItem feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      return FaxInterfaceRaw.SendFax(username, password, productId, numbers, fileContent, fileName, csid, ani, startDate, faxQuality, jobname, header, billingCode,
        WF.SDK.Common.JSONSerializerHelper.SerializeToString(feedbackEmail),
        callbackUrl, custKeys, extraParams);
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string apiKey, Guid productId, List<string> numbers,
      byte[] fileContent, string fileName,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmailItem feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      return FaxInterfaceRaw.SendFax(apiKey, productId, numbers, fileContent, fileName, csid, ani, startDate, faxQuality, jobname, header, billingCode,
        WF.SDK.Common.JSONSerializerHelper.SerializeToString(feedbackEmail),
        callbackUrl, custKeys, extraParams);
    }
    #endregion

    #region SendFax - Overload for List<String> file names
    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId, List<string> numbers,
      List<string> filePaths,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {

      //Username : <input name="Username" type="text" value="<?php print $username; ?>"><br>
      //Password : <input name="Password" type="text" value="<?php print $password; ?>"><br>
      //ProductId : <input name="ProductId" type="text" value="<?php print $prodid;?>"><br>
      //Number 1 : <input name="Numbers1" type="text" value="8002075529"><br>
      //Number 2 : <input name="Numbers2" type="text" value="8002075529"><br>
      //JobName : <input name="JobName" type="text" value="Test 1234"><br>
      //Header : <input name="Header" type="text" value="Test Header"><br>
      //Billing Code : <input name="BillingCode" type="text" value="Billing Code 123"><br>
      //Csid : <input name="CSID" type="text" value="My Company 1234567890"><br>
      //Quality : <input name="FaxQuality" type="text" value="Normal"><br>
      //Start : <input name="StartDate" type="text" value="6/1/2015"><br>
      //Feedback : <input name="FeedbackEmail" type="text" value="<?php print $feedback;?>"><br>
      //File 1 : <input name="Files1" type="file"><br>
      //File 2 : <input name="Files2" type="file"><br>

      string method = "Fax_SendFax";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      //Add the numbers collection
      Common.wwHttpHelper.AddParameterList(http, numbers, "Numbers");  //Required

      //These will get tacked on to the CustomerKey1
      //Make sure the Names and numbers collections are the same.  Otherwise don't include.
      if (custKeys != null && custKeys.Count == numbers.Count)
      {
        Common.wwHttpHelper.AddParameterList(http, custKeys, "StringParams");  //Not Required
      }

      //Add the files collection 
      //Required
      int counter = 0;
      foreach (var file in filePaths)
      {
        if (!System.IO.File.Exists(file)) { return Common.JSONSerializerHelper.SerializeToString(new Models.ApiResult<string>(false, "File does not exist.", "")); }
        //Get the bytes from the file.
        var content = System.IO.File.ReadAllBytes(file);
        //Get the fileName from the file.
        var filename = System.IO.Path.GetFileName(file);
        //Add it to the post collection
        counter++;
        http.AddPostFile("Files" + counter.ToString(), content, filename);
      }

      http.AddPostKey("JobName", jobname ?? "");  //Required
      http.AddPostKey("Header", header ?? "");  //Required
      http.AddPostKey("BillingCode", billingCode ?? "");  //Required

      if (!String.IsNullOrEmpty(csid)) { http.AddPostKey("CSID", csid); }  //Optional
      if (!String.IsNullOrEmpty(ani)) { http.AddPostKey("ANI", ani); }  //Optional
      if (startDate.HasValue) { http.AddPostKey("StartDate", startDate.Value.ToString("G")); }  //Optional
      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(feedbackEmail)) { http.AddPostKey("FeedbackEmail", feedbackEmail); }  //Optional
      if (!String.IsNullOrEmpty(callbackUrl)) { http.AddPostKey("CallbackUrl", callbackUrl); }  //Optional
      if (!String.IsNullOrEmpty(FaxInterfaceRaw.AppSource)) { http.AddPostKey("JobSource", FaxInterfaceRaw.AppSource); }  //Optional

      //Add the extra params if there are any
      if (extraParams != null && extraParams.Count > 0)
      {
        List<NameValueItem> items = new List<NameValueItem>();
        foreach (var param in extraParams) { items.Add(new NameValueItem() { Name = param.Key, Value = param.Value }); }
        Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");
      }

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string apiKey, Guid productId, List<string> numbers,
      List<string> filePaths,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {

      //Username : <input name="Username" type="text" value="<?php print $username; ?>"><br>
      //Password : <input name="Password" type="text" value="<?php print $password; ?>"><br>
      //ProductId : <input name="ProductId" type="text" value="<?php print $prodid;?>"><br>
      //Number 1 : <input name="Numbers1" type="text" value="8002075529"><br>
      //Number 2 : <input name="Numbers2" type="text" value="8002075529"><br>
      //JobName : <input name="JobName" type="text" value="Test 1234"><br>
      //Header : <input name="Header" type="text" value="Test Header"><br>
      //Billing Code : <input name="BillingCode" type="text" value="Billing Code 123"><br>
      //Csid : <input name="CSID" type="text" value="My Company 1234567890"><br>
      //Quality : <input name="FaxQuality" type="text" value="Normal"><br>
      //Start : <input name="StartDate" type="text" value="6/1/2015"><br>
      //Feedback : <input name="FeedbackEmail" type="text" value="<?php print $feedback;?>"><br>
      //File 1 : <input name="Files1" type="file"><br>
      //File 2 : <input name="Files2" type="file"><br>

      string method = "Fax_SendFax";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      //Add the numbers collection
      Common.wwHttpHelper.AddParameterList(http, numbers, "Numbers");  //Required

      //These will get tacked on to the CustomerKey1
      //Make sure the Names and numbers collections are the same.  Otherwise don't include.
      if (custKeys != null && custKeys.Count == numbers.Count)
      {
        Common.wwHttpHelper.AddParameterList(http, custKeys, "StringParams");  //Not Required
      }

      //Add the files collection 
      //Required
      int counter = 0;
      foreach (var file in filePaths)
      {
        if (!System.IO.File.Exists(file)) { return Common.JSONSerializerHelper.SerializeToString(new Models.ApiResult<string>(false, "File does not exist.", "")); }
        //Get the bytes from the file.
        var content = System.IO.File.ReadAllBytes(file);
        //Get the fileName from the file.
        var filename = System.IO.Path.GetFileName(file);
        //Add it to the post collection
        counter++;
        http.AddPostFile("Files" + counter.ToString(), content, filename);
      }

      http.AddPostKey("JobName", jobname ?? "");  //Required
      http.AddPostKey("Header", header ?? "");  //Required
      http.AddPostKey("BillingCode", billingCode ?? "");  //Required

      if (!String.IsNullOrEmpty(csid)) { http.AddPostKey("CSID", csid); }  //Optional
      if (!String.IsNullOrEmpty(ani)) { http.AddPostKey("ANI", ani); }  //Optional
      if (startDate.HasValue) { http.AddPostKey("StartDate", startDate.Value.ToString("G")); }  //Optional
      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(feedbackEmail)) { http.AddPostKey("FeedbackEmail", feedbackEmail); }  //Optional
      if (!String.IsNullOrEmpty(callbackUrl)) { http.AddPostKey("CallbackUrl", callbackUrl); }  //Optional
      if (!String.IsNullOrEmpty(FaxInterfaceRaw.AppSource)) { http.AddPostKey("JobSource", FaxInterfaceRaw.AppSource); }  //Optional

      //Add the extra params if there are any
      if (extraParams != null && extraParams.Count > 0)
      {
        List<NameValueItem> items = new List<NameValueItem>();
        foreach (var param in extraParams) { items.Add(new NameValueItem() { Name = param.Key, Value = param.Value }); }
        Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");
      }

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }


    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId, List<string> numbers,
      List<string> filePaths,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmailItem feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      return FaxInterfaceRaw.SendFax(username, password, productId, numbers, filePaths, csid, ani, startDate, faxQuality, jobname, header, billingCode,
        WF.SDK.Common.JSONSerializerHelper.SerializeToString(feedbackEmail),
        callbackUrl, custKeys, extraParams);
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string apiKey, Guid productId, List<string> numbers,
      List<string> filePaths,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmailItem feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      return FaxInterfaceRaw.SendFax(apiKey, productId, numbers, filePaths, csid, ani, startDate, faxQuality, jobname, header, billingCode,
        WF.SDK.Common.JSONSerializerHelper.SerializeToString(feedbackEmail),
        callbackUrl, custKeys, extraParams);
    }
    #endregion

    #region GetFaxStatus
    /// <summary>
    /// Get the fax statuses
    /// </summary>
    public static string GetFaxStatus(string username, string password, Guid productId, List<Guid> ids)
    {
      string method = "Fax_GetFaxStatus";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<Guid>(http, ids, "Ids");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get the fax statuses
    /// </summary>
    public static string GetFaxStatus(string apiKey, Guid productId, List<Guid> ids)
    {
      string method = "Fax_GetFaxStatus";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<Guid>(http, ids, "Ids");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region CancelFax
    /// <summary>
    /// Get the fax statuses
    /// </summary>
    public static string CancelFax(string username, string password, Guid productId, Guid id)
    {
      string method = "Fax_CancelFax";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<Guid>(http, new List<Guid>() { id }, "Ids");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    public static string CancelFax(string apiKey, Guid productId, Guid id)
    {
      string method = "Fax_CancelFax";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<Guid>(http, new List<Guid>() { id }, "Ids");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetBroadcastFaxStatus
    /// <summary>
    /// Get the fax statuses
    /// </summary>
    public static string GetBroadcastFaxStatus(string username, string password, Guid productId, List<Guid> ids)
    {
      string method = "Fax_GetBroadcastFaxStatus";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<Guid>(http, ids, "Ids");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get the fax statuses
    /// </summary>
    public static string GetBroadcastFaxStatus(string apiKey, Guid productId, List<Guid> ids)
    {
      string method = "Fax_GetBroadcastFaxStatus";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<Guid>(http, ids, "Ids");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region ChangeFaxFilterValue (Inbound and Outbound)
    /// <summary>
    /// Changes the filter value to prevent them from showing -  Filter can
    /// "Removed" = deleted
    /// "Retrieved" = read
    /// "None" = unread (reset)
    /// This is an unauthenticated method.
    /// </summary>
    public static string ChangeFaxFilterValue(string username, string password, Guid productId, List<FaxIdItem> items, string filter = "Retrieved")
    {
      string method = "Fax_ChangeFaxFilterValue";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, items, "FaxIds");
      //Add the filter value
      http.AddPostKey("Filter", filter);

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Changes the filter value to prevent them from showing -  Filter can
    /// "Removed" = deleted
    /// "Retrieved" = read
    /// "None" = unread (reset)
    /// This is an unauthenticated method.
    /// </summary>
    public static string ChangeFaxFilterValue(string apiKey, Guid productId, List<FaxIdItem> items, string filter = "Retrieved")
    {
      string method = "Fax_ChangeFaxFilterValue";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, items, "FaxIds");
      //Add the filter value
      http.AddPostKey("Filter", filter);

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    #endregion

    #region SendFaxAsEmail
    /// <summary>
    /// Send the fax as an email.  Works on inbound and outbound faxes.
    /// </summary>
    public static string SendFaxAsEmail(string username, string password, Guid productId, FaxIdItem item, string emailAddress, string subject = null, string message = null)
    {
      string method = "Fax_SendFaxAsEmail";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("FeedbackEmail", emailAddress);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, new List<FaxIdItem>() { item }, "FaxIds");
      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, new List<NameValueItem>() {
        new NameValueItem("Subject", subject),
        new NameValueItem("Message", message),
      }, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Send the fax as an email.  Works on inbound and outbound faxes.
    /// </summary>
    public static string SendFaxAsEmail(string apiKey, Guid productId, FaxIdItem item, string emailAddress, string subject = null, string message = null)
    {
      string method = "Fax_SendFaxAsEmail";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      http.AddPostKey("FeedbackEmail", emailAddress);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, new List<FaxIdItem>() { item }, "FaxIds");
      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, new List<NameValueItem>() {
        new NameValueItem("Subject", subject),
        new NameValueItem("Message", message),
      }, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region ResendFaxNotification
    /// <summary>
    /// Re-sends the notification for an inbound fax just as when it was first sent. Only for inbound faxes.
    /// </summary>
    public static string ResendFaxNotification(string username, string password, Guid productId, FaxIdItem item)
    {
      string method = "Fax_ResendFaxNotification";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, new List<FaxIdItem>() { item }, "FaxIds");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Re-sends the notification for an inbound fax just as when it was first sent. Only for inbound faxes.
    /// </summary>
    public static string ResendFaxNotification(string apiKey, Guid productId, FaxIdItem item)
    {
      string method = "Fax_ResendFaxNotification";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, new List<FaxIdItem>() { item }, "FaxIds");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region ResendFaxFeedbackEmail
    /// <summary>
    /// Re-sends the notification for an inbound fax just as when it was first sent. Only for inbound faxes.
    /// </summary>
    public static string ResendFaxFeedbackEmail(string username, string password, Guid productId, Guid id, string email)
    {
      string method = "Fax_ResendFaxFeedback";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("EmailAddress", email ?? "");  //Required

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<Guid>(http, new List<Guid>() { id }, "Ids");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Re-sends the notification for an inbound fax just as when it was first sent. Only for inbound faxes.
    /// </summary>
    public static string ResendFaxFeedbackEmail(string apiKey, Guid productId, Guid id, string email)
    {
      string method = "Fax_ResendFaxFeedback";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      http.AddPostKey("EmailAddress", email ?? "");  //Required

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<Guid>(http, new List<Guid>() { id }, "Ids");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region ConvertToFaxDocument
    /// <summary>
    /// Create a Fax Tiff, and send it back in the format we want.
    /// </summary>
    public static string ConvertToFaxDocument(string username, string password,
      HttpFileCollection files, string faxQuality = "Fine", string format = "png")
    {
      string method = "Fax_ConvertToFaxDocument";
      var http = FaxInterfaceRaw.GetHttp(username, password);

      //Add the files collection 
      //Required
      for (int i = 0; i < files.Count; i++)
      {
        byte[] lcFile = new byte[files[i].ContentLength];
        files[i].InputStream.Read(lcFile, 0, (int)files[i].InputStream.Length);
        http.AddPostFile("Files" + (i + 1).ToString(), lcFile, files[i].FileName);
      }

      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(format)) { http.AddPostKey("Format", format); } //Optional

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Create a Fax Tiff, and send it back in the format we want.
    /// </summary>
    public static string ConvertToFaxDocument(string apiKey,
      HttpFileCollection files, string faxQuality = "Fine", string format = "png")
    {
      string method = "Fax_ConvertToFaxDocument";
      var http = FaxInterfaceRaw.GetHttp(apiKey);

      //Add the files collection 
      //Required
      for (int i = 0; i < files.Count; i++)
      {
        byte[] lcFile = new byte[files[i].ContentLength];
        files[i].InputStream.Read(lcFile, 0, (int)files[i].InputStream.Length);
        http.AddPostFile("Files" + (i + 1).ToString(), lcFile, files[i].FileName);
      }

      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(format)) { http.AddPostKey("Format", format); } //Optional

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region ConvertToFaxDocument - Overload for Byte []
    /// <summary>
    /// Create a Fax Tiff, and send it back in the format we want.
    /// </summary>
    public static string ConvertToFaxDocument(string username, string password,
      byte[] fileContent, string fileName, string faxQuality = "Fine", string format = "png")
    {
      string method = "Fax_ConvertToFaxDocument";
      var http = FaxInterfaceRaw.GetHttp(username, password);

      //Add the files collection 
      //Required
      if (fileName == null) { fileName = "FaxDocument"; }
      http.AddPostFile("Files1", fileContent, fileName);

      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(format)) { http.AddPostKey("Format", format); } //Optional

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Create a Fax Tiff, and send it back in the format we want.
    /// </summary>
    public static string ConvertToFaxDocument(string apiKey,
      byte[] fileContent, string fileName, string faxQuality = "Fine", string format = "png")
    {
      string method = "Fax_ConvertToFaxDocument";
      var http = FaxInterfaceRaw.GetHttp(apiKey);

      //Add the files collection 
      //Required
      if (fileName == null) { fileName = "FaxDocument"; }
      http.AddPostFile("Files1", fileContent, fileName);

      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(format)) { http.AddPostKey("Format", format); } //Optional

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetFaxCoverPages
    /// <summary>
    /// Retrieves the Fax Cover Pages that can be used on the indicated product. This should be used in 
    /// conjunction with a fax send operation.  These are the usable cover pages for the product.  This 
    /// should not be used to get the cover pages that can be edited.  Use the profile method for that.
    /// </summary>
    public static string GetFaxCoverPages(string username, string password, Guid? productId = null)
    {
      string method = "Fax_GetCoverPages";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Retrieves the Fax Cover Pages that can be used on the indicated product. This should be used in 
    /// conjunction with a fax send operation.  These are the usable cover pages for the product.  This 
    /// should not be used to get the cover pages that can be edited.  Use the profile method for that.
    /// </summary>
    public static string GetFaxCoverPages(string apiKey, Guid? productId = null)
    {
      string method = "Fax_GetCoverPages";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);
      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region SaveFaxFields
    /// <summary>
    /// Alters some fields on a fax.
    /// The behavior here is to update only the fields below if they are set on the object we receive.
    /// Some of this can be achieved using other methods.
    ///   -jobName     // Subject displayed to customer
    ///   -reference   // Job Reference displayed to customer
    ///   -loginId     // The "owner" Login Id
    ///   -tagList     // This is a comma separated list of string names.  Searchable.
    ///   -filterValue // (None, Retrieved, Removed)
    /// </summary>
    public static string SaveFaxFields(string username, string password, Guid productId, FaxIdItem item, string jobName, string reference, string filterValue, Guid? ownerLoginId, List<string> tagList)
    {
      string method = "Fax_SaveFaxFields";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, new List<FaxIdItem>() { item }, "FaxIds");

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      if (!string.IsNullOrEmpty(jobName)) { items.Add(new NameValueItem() { Name = "jobName", Value = jobName }); }
      if (!string.IsNullOrEmpty(reference)) { items.Add(new NameValueItem() { Name = "reference", Value = reference }); }
      if (!string.IsNullOrEmpty(filterValue)) { items.Add(new NameValueItem() { Name = "filterValue", Value = filterValue }); }
      if (ownerLoginId.HasValue && ownerLoginId.Value != Guid.Empty) { items.Add(new NameValueItem() { Name = "ownerLoginId", Value = ownerLoginId.Value.ToString() }); }
      if (tagList != null && tagList.Count > 0) { items.Add(new NameValueItem() { Name = "tagList", Value = string.Join(",", tagList.ToArray()) }); }

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Alters some fields on a fax.
    /// The behavior here is to update only the fields below if they are set on the object we receive.
    /// Some of this can be achieved using other methods.
    ///   -jobName     // Subject displayed to customer
    ///   -reference   // Job Reference displayed to customer
    ///   -loginId     // The "owner" Login Id
    ///   -tagList     // This is a comma separated list of string names.  Searchable.
    ///   -filterValue // (None, Retrieved, Removed)
    /// </summary>
    public static string SaveFaxFields(string apiKey, Guid productId, FaxIdItem item, string jobName, string reference, string filterValue, Guid? ownerLoginId, List<string> tagList)
    {
      string method = "Fax_SaveFaxFields";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, new List<FaxIdItem>() { item }, "FaxIds");

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      if (!string.IsNullOrEmpty(jobName)) { items.Add(new NameValueItem() { Name = "jobName", Value = jobName }); }
      if (!string.IsNullOrEmpty(reference)) { items.Add(new NameValueItem() { Name = "reference", Value = reference }); }
      if (!string.IsNullOrEmpty(filterValue)) { items.Add(new NameValueItem() { Name = "filterValue", Value = filterValue }); }
      if (ownerLoginId.HasValue && ownerLoginId.Value != Guid.Empty) { items.Add(new NameValueItem() { Name = "ownerLoginId", Value = ownerLoginId.Value.ToString() }); }
      if (tagList != null && tagList.Count > 0) { items.Add(new NameValueItem() { Name = "tagList", Value = string.Join(",", tagList.ToArray()) }); }

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetAllF2EFaxDescriptions_Paged
    /// <summary>
    /// Get the requested Faxes
    /// Only use the ownerLoginId parameter to returned owned faxes only.
    /// For all faxes, leave the parameter null.
    /// </summary>
    public static string GetAllF2EFaxDescriptions_Paged(string username, string password, Guid productId, Guid? ownerLoginId = null, int page = 1, int itemsPerPage = 10,
        string direction = null, List<string> filterList = null,
        DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {
      string method = "Fax_GetAllF2EFaxDescriptions_Paged";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      if (!String.IsNullOrEmpty(direction)) { http.AddPostKey("FaxDirection", direction); }
      if (filterList != null) { Common.wwHttpHelper.AddParameterList<string>(http, filterList, "FilterList"); }  //Optional
      if (startUtc.HasValue) { http.AddPostKey("StartDate", startUtc.Value.ToString("G")); }  //Optional
      if (endUtc.HasValue) { http.AddPostKey("StartDate", endUtc.Value.ToString("G")); }  //Optional

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "page", Value = page.ToString() });  // Mandatory
      items.Add(new NameValueItem() { Name = "count", Value = itemsPerPage.ToString() }); // Mandatory
      items.Add(new NameValueItem() { Name = "include0Page", Value = include0Page.ToString() }); // Optional
      if (ownerLoginId.HasValue) { items.Add(new NameValueItem() { Name = "ownerLoginId", Value = ownerLoginId.ToString() }); } // Optional

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get the requested Faxes
    /// Only use the ownerLoginId parameter to returned owned faxes only.
    /// For all faxes, leave the parameter null.
    /// </summary>
    public static string GetAllF2EFaxDescriptions_Paged(string apiKey, Guid productId, Guid? ownerLoginId = null, int page = 1, int itemsPerPage = 10,
        string direction = null, List<string> filterList = null,
        DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {
      string method = "Fax_GetAllF2EFaxDescriptions_Paged";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      if (!String.IsNullOrEmpty(direction)) { http.AddPostKey("FaxDirection", direction); }
      if (filterList != null) { Common.wwHttpHelper.AddParameterList<string>(http, filterList, "FilterList"); }  //Optional
      if (startUtc.HasValue) { http.AddPostKey("StartDate", startUtc.Value.ToString("G")); }  //Optional
      if (endUtc.HasValue) { http.AddPostKey("StartDate", endUtc.Value.ToString("G")); }  //Optional

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "page", Value = page.ToString() });  // Mandatory
      items.Add(new NameValueItem() { Name = "count", Value = itemsPerPage.ToString() }); // Mandatory
      items.Add(new NameValueItem() { Name = "include0Page", Value = include0Page.ToString() }); // Optional
      if (ownerLoginId.HasValue) { items.Add(new NameValueItem() { Name = "ownerLoginId", Value = ownerLoginId.ToString() }); } // Optional

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetF2EFaxDescriptions_Paged
    /// <summary>
    /// Get the requested Faxes
    /// Only use the ownerLoginId parameter to returned owned faxes only.
    /// For all faxes, leave the parameter null.
    /// </summary>
    public static string GetF2EFaxDescriptions_Paged(string username, string password, Guid productId, Models.Internal.FolderItem folder = null, Guid? ownerLoginId = null, int page = 1, int itemsPerPage = 10,
        string direction = null, List<string> filterList = null,
        DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {
      string method = "Fax_GetF2EFaxDescriptions_Paged";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      if (!String.IsNullOrEmpty(direction)) { http.AddPostKey("FaxDirection", direction); }
      if (filterList != null) { Common.wwHttpHelper.AddParameterList<string>(http, filterList, "FilterList"); }  //Optional
      if (startUtc.HasValue) { http.AddPostKey("StartDate", startUtc.Value.ToString("G")); }  //Optional
      if (endUtc.HasValue) { http.AddPostKey("StartDate", endUtc.Value.ToString("G")); }  //Optional

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "page", Value = page.ToString() });  // Mandatory
      items.Add(new NameValueItem() { Name = "count", Value = itemsPerPage.ToString() }); // Mandatory
      items.Add(new NameValueItem() { Name = "include0Page", Value = include0Page.ToString() }); // Optional
      if (ownerLoginId.HasValue) { items.Add(new NameValueItem() { Name = "ownerLoginId", Value = ownerLoginId.ToString() }); } // Optional
      if (folder != null) { http.AddPostKey("ProductFolder", WF.SDK.Common.JSONSerializerHelper.SerializeToString<Models.Internal.FolderItem>(folder)); } // Optional

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get the requested Faxes
    /// Only use the ownerLoginId parameter to returned owned faxes only.
    /// For all faxes, leave the parameter null.
    /// </summary>
    public static string GetF2EFaxDescriptions_Paged(string apiKey, Guid productId, Models.Internal.FolderItem folder = null, Guid? ownerLoginId = null, int page = 1, int itemsPerPage = 10,
        string direction = null, List<string> filterList = null,
        DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {
      string method = "Fax_GetF2EFaxDescriptions_Paged";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      if (!String.IsNullOrEmpty(direction)) { http.AddPostKey("FaxDirection", direction); }
      if (filterList != null) { Common.wwHttpHelper.AddParameterList<string>(http, filterList, "FilterList"); }  //Optional
      if (startUtc.HasValue) { http.AddPostKey("StartDate", startUtc.Value.ToString("G")); }  //Optional
      if (endUtc.HasValue) { http.AddPostKey("StartDate", endUtc.Value.ToString("G")); }  //Optional

      //NameValue - MethodParam list
      List<NameValueItem> items = new List<NameValueItem>();
      items.Add(new NameValueItem() { Name = "page", Value = page.ToString() });  // Mandatory
      items.Add(new NameValueItem() { Name = "count", Value = itemsPerPage.ToString() }); // Mandatory
      items.Add(new NameValueItem() { Name = "include0Page", Value = include0Page.ToString() }); // Optional
      if (ownerLoginId.HasValue) { items.Add(new NameValueItem() { Name = "ownerLoginId", Value = ownerLoginId.ToString() }); } // Optional
      if (folder != null) { http.AddPostKey("ProductFolder", WF.SDK.Common.JSONSerializerHelper.SerializeToString<Models.Internal.FolderItem>(folder)); } // Optional

      Common.wwHttpHelper.AddParameterList<NameValueItem>(http, items, "MethodParams");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region MoveFaxToFolder
    /// <summary>
    /// Move Faxes to a different Product Folder
    /// </summary>
    public static string MoveFaxToFolder(string username, string password, Guid productId, List<Models.Internal.FaxIdItem> faxIds, Models.Internal.FolderItem folder = null)
    {
      string method = "Fax_MoveFaxToFolder";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      // add the folder item
      if (folder != null) { http.AddPostKey("ProductFolder", WF.SDK.Common.JSONSerializerHelper.SerializeToString<Models.Internal.FolderItem>(folder)); }

      // add the fax ids
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, faxIds, "FaxIds");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Move Faxes to a different Product Folder
    /// </summary>
    public static string MoveFaxToFolder(string apiKey, Guid productId, List<Models.Internal.FaxIdItem> faxIds, Models.Internal.FolderItem folder = null)
    {
      string method = "Fax_MoveFaxToFolder";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      // add the folder item
      if (folder != null) { http.AddPostKey("ProductFolder", WF.SDK.Common.JSONSerializerHelper.SerializeToString<Models.Internal.FolderItem>(folder)); }

      // add the fax ids
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, faxIds, "FaxIds");

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region DeleteFolder
    /// <summary>
    /// Delete a Product Folder
    /// Must have all of its Non-deleted faxes moved to another folder before this will work
    /// </summary>
    public static string DeleteFolder(string username, string password, Guid productId, Models.Internal.FolderItem folder)
    {
      string method = "Fax_DeleteProductFolder";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      // add the folder item
      http.AddPostKey("ProductFolder", WF.SDK.Common.JSONSerializerHelper.SerializeToString<Models.Internal.FolderItem>(folder));

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Delete a Product Folder
    /// Must have all of its Non-deleted faxes moved to another folder before this will work
    /// </summary>
    public static string DeleteFolder(string apiKey, Guid productId, Models.Internal.FolderItem folder)
    {
      string method = "Fax_DeleteProductFolder";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      // add the folder item
      http.AddPostKey("ProductFolder", WF.SDK.Common.JSONSerializerHelper.SerializeToString<Models.Internal.FolderItem>(folder));

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region CheckFolderName
    /// <summary>
    /// Checks if a product folder name is available
    /// false - it is not available
    /// true - it is available
    /// </summary>
    public static string CheckFolderName(string username, string password, Guid productId, Models.Internal.FolderItem folder)
    {
      string method = "Fax_CheckProductFolderName";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      // add the folder item
      http.AddPostKey("ProductFolder", WF.SDK.Common.JSONSerializerHelper.SerializeToString<Models.Internal.FolderItem>(folder));

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Checks if a product folder name is available
    /// false - it is not available
    /// true - it is available
    /// </summary>
    public static string CheckFolderName(string apiKey, Guid productId, Models.Internal.FolderItem folder)
    {
      string method = "Fax_CheckProductFolderName";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      // add the folder item
      http.AddPostKey("ProductFolder", WF.SDK.Common.JSONSerializerHelper.SerializeToString<Models.Internal.FolderItem>(folder));

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region SaveFolder
    /// <summary>
    /// Saves a Product Folder
    /// Folder Id should be missing when creating a new folder, otherwise you're updating an existing one
    /// </summary>
    public static string SaveFolder(string username, string password, Guid productId, Models.Internal.FolderItem folder)
    {
      string method = "Fax_SaveProductFolder";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      // add the folder item
      http.AddPostKey("ProductFolder", WF.SDK.Common.JSONSerializerHelper.SerializeToString<Models.Internal.FolderItem>(folder));

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Saves a Product Folder
    /// Folder Id should be missing when creating a new folder, otherwise you're updating an existing one
    /// </summary>
    public static string SaveFolder(string apiKey, Guid productId, Models.Internal.FolderItem folder)
    {
      string method = "Fax_SaveProductFolder";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      // add the folder item
      http.AddPostKey("ProductFolder", WF.SDK.Common.JSONSerializerHelper.SerializeToString<Models.Internal.FolderItem>(folder));

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetFolderList
    /// <summary>
    /// Get the Folder List for a Product
    /// </summary>
    public static string GetFolderList(string username, string password, Guid productId)
    {
      string method = "Fax_GetProductFolderList";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }

    /// <summary>
    /// Get the Folder List for a Product
    /// </summary>
    public static string GetFolderList(string apiKey, Guid productId)
    {
      string method = "Fax_GetProductFolderList";
      var http = FaxInterfaceRaw.GetHttp(apiKey, productId);

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

  }
}