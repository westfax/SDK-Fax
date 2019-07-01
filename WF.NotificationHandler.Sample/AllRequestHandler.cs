using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Linq;

namespace WF.NotificationHandler.Sample
{
  public class AllRequestHandler : IHttpHandler
  {
    string itemkey = "requestLog";

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
    /// For the sample we'll handle any request coming into the url.
    /// The handler will do the following:
    ///   --Check to see if the request is asking for the "ShowRequestLog" page.  If so, then it will dump the request Log to an info page and return that.
    ///   --Add a record of the request to the Application Cache.
    ///   --Reply with a body content of "Got it!", and a response of 200 OK.
    /// </summary>
    public void ProcessRequest(HttpContext context)
    {
      //If the url includes this string, then just send the status report back.
      if (context.Request.RawUrl.ToLower().Contains("showrequestlog")) { this.SendRequestLog(context); return; }

      //Log the request.
      this.LogRequest(context);

      //Send the default response.
      this.SendResponse(context);
    }

    /// <summary>
    /// Put the request into the request log.
    /// </summary>
    private void LogRequest(HttpContext context)
    {
      //Make sure there is an object there.
      if (!context.Application.AllKeys.Contains(this.itemkey)) { context.Application.Add(this.itemkey, new List<string>()); }

      string item = "";
      //For a get, just add the url.
      if (context.Request.HttpMethod.ToLower() == "get") { item = "[GET]" + context.Request.RawUrl; }
      //For a post, add the post collection
      else if (context.Request.HttpMethod.ToLower() == "post")
      {
        var vals = string.Join("&", context.Request.Form.AllKeys.ToList().Select(i => i + "=" + context.Request.Form[i]).ToArray());
        item = "[POST]" + context.Request.RawUrl + "?" + vals;
      }
      //Everything else - Shouldn't have this.
      else { item = "[" + context.Request.HttpMethod + "]" + context.Request.RawUrl; }
      //Add the request info
      ((List<string>)context.Application[itemkey]).Add(item);
    }
    
    /// <summary>
    /// This sends a web page that shows the requests that have been logged.  Used for testing.
    /// </summary>
    /// <param name="context"></param>
    private void SendRequestLog(HttpContext context)
    {
      string itemkey = "requestLog";
      var items = (List<string>)context.Application[itemkey];

      context.Response.StatusCode = 200;
      context.Response.Write("<html><body>" + string.Join("<p/>", items.ToArray()) + "</body></html>");
      context.Response.End();

    }

    /// <summary>
    /// Send a default respopnse.  Really no content is necessary. This will look for a "code" argument and 
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
      context.Response.End();
      return;
    }

    #endregion
  }
}
