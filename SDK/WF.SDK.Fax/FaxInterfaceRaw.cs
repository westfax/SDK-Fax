using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WF.SDK.Models.Internal;

namespace WF.SDK.Fax.Internal
{
  public static class FaxInterfaceRaw
  {
    //The method name will be inserted in each call.
    public static string RestUrlTemplate { get; set; } //https://api2.westfax.com/REST/{0}/json

    static FaxInterfaceRaw()
    {
      //You can configure this statically here and compile, or set the URL template above from another source such as confguration
      //FaxInterfaceRaw.RestUrlTemplate = System.Configuration.ConfigurationManager.AppSettings["APIEncoding"];  //From config?
      //FaxInterfaceRaw.RestUrlTemplate = "https://api2.westfax.com/REST/{0}/json";  //Statically?
      //Encoding can be json, json2, xml.
    }

    #region GetHttp (Private Helper)
    private static wwHttp GetHttp(string username, string password, Guid? productId = null, bool cookies = false)
    {
      wwHttp http = new wwHttp();
      http.PostMode = 2;
      if (username != null) { http.AddPostKey("Username", username); }
      if (password != null) { http.AddPostKey("Password", password); }
      http.AddPostKey("Cookies", cookies.ToString());
      if (productId.HasValue) { http.AddPostKey("ProductId", productId.Value.ToString()); }
      return http;
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
    #endregion

    #region GetProductList
    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// </summary>
    public static string GetProductList(string username, string password)
    {
      string method = "Profile_GetProductList";
      var http = FaxInterfaceRaw.GetHttp(username, password);
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
    #endregion

    #region GetAccountInfo
    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay.  
    /// </summary>
    public static string GetAccountInfo(string username, string password, Guid? productId = null)
    {
      string method = "Profile_GetAccountInfo";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);
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
    #endregion

    #region GetFaxIds (Inbound and Outbound)
    /// <summary>
    /// Get The inbound Faxes
    /// </summary>
    public static string GetFaxIds(string username, string password, Guid productId)
    {
      string method = "Fax_GetFaxIdentifiers";

      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region GetInboundFaxIds
    /// <summary>
    /// Get The inbound Faxes
    /// </summary>
    public static string GetInboundFaxIds(string username, string password, Guid productId)
    {
      string method = "Fax_GetFaxIdentifiers";

      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());
      http.AddPostKey("FaxDirection", "Inbound");

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
    #endregion

    #region GetInboundFaxDescriptions
    /// <summary>
    /// Get The inbound Faxes
    /// </summary>
    public static string GetInboundFaxDescriptions(string username, string password, Guid productId)
    {
      string method = "Fax_GetFaxDescriptions";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("StartDate", DateTime.Now.AddDays(-32).ToShortDateString());
      http.AddPostKey("FaxDirection", "Inbound");

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
    #endregion

    #region SendFax
    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId,
      List<string> numbers, HttpFileCollection files,
       string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "", string feedbackEmail = null)
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

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region SendFax - Overload for Byte []
    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId,
      List<string> numbers, byte[] fileContent, string fileName,
       string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "", string feedbackEmail = null)
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

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
    }
    #endregion

    #region SendFax - Overload for List<String> file names
    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static string SendFax(string username, string password, Guid productId,
      List<string> numbers, List<string> filePaths,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "", string feedbackEmail = null)
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

      http.AddPostKey("JobName", jobname);  //Required
      http.AddPostKey("Header", header);  //Required
      http.AddPostKey("BillingCode", billingCode);  //Required

      if (!String.IsNullOrEmpty(csid)) { http.AddPostKey("CSID", csid); }  //Optional
      if (!String.IsNullOrEmpty(ani)) { http.AddPostKey("ANI", ani); }  //Optional
      if (startDate.HasValue) { http.AddPostKey("StartDate", startDate.Value.ToString("G")); }  //Optional
      if (!String.IsNullOrEmpty(faxQuality)) { http.AddPostKey("FaxQuality", faxQuality); }  //Optional
      if (!String.IsNullOrEmpty(feedbackEmail)) { http.AddPostKey("FeedbackEmail", feedbackEmail); }  //Optional

      return FaxInterfaceRaw.GetResponseStr(http, String.Format(FaxInterfaceRaw.RestUrlTemplate, method));
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

    #endregion

    #region SendFaxAsEmail
    /// <summary>
    /// Send the fax as an email.  Works on inbound and outbound faxes.
    /// </summary>
    public static string SendFaxAsEmail(string username, string password, Guid productId, FaxIdItem item, string emailAddress)
    {
      string method = "Fax_SendFaxAsEmail";
      var http = FaxInterfaceRaw.GetHttp(username, password, productId);

      http.AddPostKey("FeedbackEmail", emailAddress);

      //Put them on the http object
      Common.wwHttpHelper.AddParameterList<FaxIdItem>(http, new List<FaxIdItem>() { item }, "FaxIds");

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
    #endregion
    

  }
}