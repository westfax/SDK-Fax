using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Linq;
using System.IO;

namespace WF.NotificationHandler.Sample
{
  public class AllRequestHandler : IHttpHandler
  {
    private const string ITEM_KEY = "requestLog";

    /// <summary>
    /// Thi is just a sample HttpHandler that will receive all requests.  It logs the request, and
    /// makes the log available if the string "showrequestlog" is anywhere in the request url.
    /// </summary>
    #region IHttpHandler Members

    public bool IsReusable
    {
      // Return false in case your Managed Handler cannot be reused for another request.
      // Usually this would be false in case you have some state information preserved per request.
      get { return true; }
    }

    /// <summary>
    /// Gets the Items list stored in the Application
    /// </summary>
    public List<string> GetItems(HttpContext context)
    {
      if (!context.Application.AllKeys.Contains(ITEM_KEY)) { context.Application.Add(ITEM_KEY, new List<string>()); }
      return (List<string>)context.Application[ITEM_KEY];
    }

    /// <summary>
    /// For the sample we'll handle any request coming into the url.
    /// The handler will do the following:
    ///   --Check to see if the request is asking for the "ShowRequestLog" page.  If so, then it will dump the request Log to an info page and return that.
    ///   --Add a record of the request to the Application Cache.
    ///   --Reply with a body content with a log link, and a response of 200 OK.
    /// </summary>
    public void ProcessRequest(HttpContext context)
    {
      if (context.Request.IsSecureConnection)
      {
        var result = this.CheckClientCertificate(context);
        if (!result)
        {
          //If we decide not to accept the client certificate for any reason we can fail this way.
          context.Response.ClearContent();
          context.Response.StatusCode = 401;  //Unauthorized
          context.Response.End();
          return;
        }
      }
      else
      {
        //Maybe fail here if there is not SSL connection, but not in this sample.
      }
      //If the url includes this string, then just send the status report back.  Don't log it.
      if (context.Request.RawUrl.ToLower().Contains("showrequestlog")) { this.SendRequestLog(context); return; }

      //Log the request.
      this.LogRequest(context);

			//var str = this.ReadRequestBody(context);

      //Send the default response.
      this.SendResponse(context);
    }

    private bool CheckClientCertificate(HttpContext context)
    {
      try
      {
        var cert = context.Request.ClientCertificate;
        string issuer = cert.Issuer;
        string subject = cert.Subject;
      }
      catch { }
      //Always accept the certificate in this sample.
      return true;
    }

    private string ClientCertSubject(HttpContext context)
    {
      try      {        return context.Request.ClientCertificate.Subject;      }
      catch { return "No Cert"; }
    }


		/// <summary>
		/// Put the request into the request log.
		/// </summary>
		private string ReadRequestBody(HttpContext context)
		{
			using(var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream))
			{
    bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
    var bodyText = bodyStream.ReadToEnd();
    return bodyText;
			}

			//context.Request.InputStream.r
			//string item = "";
			////For a get, just add the url.
			//if (context.Request.HttpMethod.ToLower() == "get") { item = "[GET]" + context.Request.RawUrl; }
			////For a post, add the post collection
			//else if (context.Request.HttpMethod.ToLower() == "post")
			//{
			//  var vals = string.Join("&", context.Request.Form.AllKeys.ToList().Select(i => i + "=" + context.Request.Form[i]).ToArray());
			//  item = "[POST]" + context.Request.RawUrl + "?" + vals;
			//}
			////Everything else - Probably wont ever use this code path.
			//else { item = "[" + context.Request.HttpMethod + "]" + context.Request.RawUrl; }
			////Cert Subject if there is one
			//item += "\t" + this.ClientCertSubject(context);
			////Add the request info
			//this.GetItems(context).Add(item);
		}


    /// <summary>
    /// Put the request into the request log.
    /// </summary>
    private void LogRequest(HttpContext context)
    {
      string item = "";
      //For a get, just add the url.
      if (context.Request.HttpMethod.ToLower() == "get") { item = "[GET]" + context.Request.RawUrl; }
      //For a post, add the post collection
      else if (context.Request.HttpMethod.ToLower() == "post")
      {
        var vals = string.Join("&", context.Request.Form.AllKeys.ToList().Select(i => i + "=" + context.Request.Form[i]).ToArray());
        item = "[POST]" + context.Request.RawUrl + "?" + vals;
      }
      //Everything else - Probably wont ever use this code path.
      else { item = "[" + context.Request.HttpMethod + "]" + context.Request.RawUrl; }
      //Cert Subject if there is one
      item += "\t" + this.ClientCertSubject(context);
      //Add the request info
      this.GetItems(context).Add(item);
    }
    
    /// <summary>
    /// This sends a web page that shows the requests that have been logged.  Used for testing.
    /// </summary>
    /// <param name="context"></param>
    private void SendRequestLog(HttpContext context)
    {
      var items = this.GetItems(context);

      context.Response.StatusCode = 200;
      context.Response.Write("<html><body>" + string.Join("<p/>", items.ToArray()) + "</body></html>");
      context.Response.End();
    }

    /// <summary>
    /// Send a default respopnse.  Really no content is necessary. This will look for a "code" argument and reply with that HTTP code.
    /// </summary>
    public void SendResponse(HttpContext context)
    {
      int code = 200;
      try { if (context.Request.QueryString["code"] != null) { try { code = int.Parse(context.Request.QueryString["code"]); } catch { } } }
      catch { }
      try { if (context.Request.Form["code"] != null) { try { code = int.Parse(context.Request.Form["code"]); } catch { } } }
      catch { }
      if (code != 200) 
      {
        context.Response.ClearContent();
        context.Response.StatusCode = code;
        context.Response.End();
        return;
      }

      int itemCount = 0;
      try { itemCount = this.GetItems(context).Count; }
      catch { }
      context.Response.Write(string.Format("Log Entries: {0} <a href='showrequestlog'>Show</a>", itemCount.ToString()));
      context.Response.End();
      return;
    }

    #endregion
  }
}
