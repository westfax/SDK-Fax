using System;
using System.Net;
using System.IO;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace WF.SDK
{
  /// <summary>
  /// Summary description for wwHttp.
  /// </summary>
  public class wwHttp
  {
    /// <summary>
    /// Fires progress events when using GetUrlEvents() to retrieve a URL.
    /// </summary>
    public event OnReceiveDataHandler OnReceiveData;

    /// <summary>
    /// Determines how data is POSTed when cPostBuffer is set.
    /// 1 - UrlEncoded
    /// 2 - Multi-Part form vars
    /// 4 - XML (raw buffer content type: text/xml)
    /// </summary>
    public int PostMode
    {
      get { return this.nPostMode; }
      set { this.nPostMode = value; }
    }

    /// <summary>
    ///  User name used for Authentication. 
    ///  To use the currently logged in user when accessing an NTLM resource you can use "AUTOLOGIN".
    /// </summary>
    public string Username
    {
      get { return this.cUsername; }
      set { cUsername = value; }
    }

    /// <summary>
    /// Password for Authentication.
    /// </summary>
    public string Password
    {
      get { return this.cPassword; }
      set { this.cPassword = value; }
    }

    /// <summary>
    /// Address of the Proxy Server to be used.
    /// Use optional DEFAULTPROXY value to specify that you want to IE's Proxy Settings
    /// </summary>
    public string ProxyAddress
    {
      get { return this.cProxyAddress; }
      set { this.cProxyAddress = value; }
    }

    /// <summary>
    /// Semicolon separated Address list of the servers the proxy is not used for.
    /// </summary>
    public string ProxyBypass
    {
      get { return this.cProxyBypass; }
      set { this.cProxyBypass = value; }
    }

    /// <summary>
    /// Username for a password validating Proxy. Only used if the proxy info is set.
    /// </summary>
    public string ProxyUsername
    {
      get { return this.cProxyUsername; }
      set { this.cProxyUsername = value; }
    }
    /// <summary>
    /// Password for a password validating Proxy. Only used if the proxy info is set.
    /// </summary>
    public string ProxyPassword
    {
      get { return this.cProxyPassword; }
      set { this.cProxyPassword = value; }
    }

    /// <summary>
    /// Timeout for the Web request in seconds. Times out on connection, read and send operations.
    /// Default is 30 seconds.
    /// </summary>
    public int Timeout
    {
      get { return this.nConnectTimeout; }
      set { this.nConnectTimeout = value; }
    }

    /// <summary>
    /// Error Message if the Error Flag is set or an error value is returned from a method.
    /// </summary>
    public string ErrorMsg
    {
      get { return this.cErrorMsg; }
      set { this.cErrorMsg = value; }
    }

    /// <summary>
    /// Error flag if an error occurred.
    /// </summary>
    public bool Error
    {
      get { return this.bError; }
      set { this.bError = value; }
    }

    /// <summary>
    /// Determines whether errors cause exceptions to be thrown. By default errors 
    /// are handled in the class and the Error property is set for error conditions.
    /// </summary>
    public bool ThrowExceptions
    {
      get { return bThrowExceptions; }
      set { this.bThrowExceptions = value; }
    }

    /// <summary>
    /// If set to a non-zero value will automatically track cookies. The number assigned is the cookie count.
    /// </summary>
    public bool HandleCookies
    {
      get { return this.bHandleCookies; }
      set { this.bHandleCookies = value; }
    }
    public CookieCollection Cookies
    {
      get { return this.oCookies; }
      set { this.oCookies = value; }
    }
    public bool HandleHeaders
    {
      get { return this.bHandleHeaders; }
      set { this.bHandleHeaders = value; }
    }
    public NameValueCollection Headers
    {
      get { return this.oHeaders; }
      set { this.oHeaders = value; }
    }
    public HttpWebResponse WebResponse
    {
      get { return this.oWebResponse; }
      set { this.oWebResponse = value; }
    }
    public HttpWebRequest WebRequest
    {
      get { return this.oWebRequest; }
      set { this.oWebRequest = value; }
    }

    // *** member properties
    //string cPostBuffer = "";
    MemoryStream oPostStream;
    BinaryWriter oPostData;

    int nPostMode = 1;

    int nConnectTimeout = 45;
    string cUserAgent = "West Fax HTTP .NET";

    string cUsername = "";
    string cPassword = "";

    string cProxyAddress = "";
    string cProxyBypass = "";
    string cProxyUsername = "";
    string cProxyPassword = "";

    bool bThrowExceptions = false;
    bool bHandleCookies = false;
    bool bHandleHeaders = false;

    string cErrorMsg = "";
    bool bError = false;

    NameValueCollection oHeaders;
    HttpWebResponse oWebResponse;
    HttpWebRequest oWebRequest;
    CookieCollection oCookies;

    string cMultiPartBoundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");

    public void wwHTTP()
    {
      //
      // TODO: Add constructor logic here
      //

    }

    /// <summary>
    /// Adds POST form variables to the request buffer.
    /// HttpPostMode determines how parms are handled.
    /// 1 - UrlEncoded Form Variables. Uses key and value pairs (ie. "Name","Rick") to create URLEncoded content
    /// 2 - Multi-Part Forms - not supported
    /// 4 - XML block - Post a single XML block. Pass in as Key (1st Parm)
    /// other - raw content buffer. Just assign to Key.
    /// </summary>
    /// <param name="Key">Key value or raw buffer depending on post type</param>
    /// <param name="Value">Value to store. Used only in key/value pair modes</param>
    public void AddPostKey(string Key, byte[] Value)
    {

      if (this.oPostData == null)
      {
        this.oPostStream = new MemoryStream();
        this.oPostData = new BinaryWriter(this.oPostStream);
      }

      if (Key == "RESET")
      {
        this.oPostStream = new MemoryStream();
        this.oPostData = new BinaryWriter(this.oPostStream);
      }

      switch (this.nPostMode)
      {
        case 1:
          this.oPostData.Write(Encoding.GetEncoding(1252).GetBytes(
                    Key + "=" + System.Web.HttpUtility.UrlEncode(Value) + "&"));
          break;
        case 2:
          this.oPostData.Write(Encoding.GetEncoding(1252).GetBytes(
            "--" + this.cMultiPartBoundary + "\r\n" +
            "Content-Disposition: form-data; name=\"" + Key + "\"\r\n\r\n"));

          this.oPostData.Write(Value);

          this.oPostData.Write(Encoding.GetEncoding(1252).GetBytes("\r\n"));
          break;
        default:
          this.oPostData.Write(Value);
          break;
      }
    }

    public void AddPostKey(string Key, string Value)
    {
      this.AddPostKey(Key, Encoding.GetEncoding(1252).GetBytes(Value));
    }

    /// <summary>
    /// Adds a fully self contained POST buffer to the request.
    /// Works for XML or previously encoded content.
    /// </summary>
    /// <param name="PostBuffer"></param>
    public void AddPostKey(string FullPostBuffer)
    {
      this.oPostData.Write(Encoding.GetEncoding(1252).GetBytes(FullPostBuffer));
    }

    public bool AddPostFile(string Key, string FileName)
    {
      byte[] lcFile;

      if (this.oPostData == null)
      {
        this.oPostStream = new MemoryStream();
        this.oPostData = new BinaryWriter(this.oPostStream);
      }

      if (this.nPostMode != 2)
      {
        this.cErrorMsg = "File upload allowed only with Multi-part forms";
        this.bError = true;
        return false;
      }

      try
      {
        FileStream loFile = new FileStream(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

        lcFile = new byte[loFile.Length];
        loFile.Read(lcFile, 0, (int)loFile.Length);
        loFile.Close();
      }
      catch (Exception e)
      {
        this.cErrorMsg = e.Message;
        this.bError = true;
        return false;
      }

      return this.AddPostFile(Key, lcFile, new FileInfo(FileName).Name);
    }

    public bool AddPostFile(string Key, FileStream fileStream, string FileName)
    {
      byte[] lcFile;

      if (this.oPostData == null)
      {
        this.oPostStream = new MemoryStream();
        this.oPostData = new BinaryWriter(this.oPostStream);
      }

      if (this.nPostMode != 2)
      {
        this.cErrorMsg = "File upload allowed only with Multi-part forms";
        this.bError = true;
        return false;
      }

      try
      {
        lcFile = new byte[fileStream.Length];
        fileStream.Read(lcFile, 0, (int)fileStream.Length);
        fileStream.Close();
      }
      catch (Exception e)
      {
        this.cErrorMsg = e.Message;
        this.bError = true;
        return false;
      }

      return this.AddPostFile(Key, lcFile, new FileInfo(FileName).Name);
    }

    public bool AddPostFile(string Key, byte[] fileBytes, string FileName)
    {
      if (this.oPostData == null)
      {
        this.oPostStream = new MemoryStream();
        this.oPostData = new BinaryWriter(this.oPostStream);
      }

      if (this.nPostMode != 2)
      {
        this.cErrorMsg = "File upload allowed only with Multi-part forms";
        this.bError = true;
        return false;
      }

      this.oPostData.Write(Encoding.GetEncoding(1252).GetBytes(
          "--" + this.cMultiPartBoundary + "\r\n" +
          "Content-Disposition: form-data; name=\"" + Key + "\" filename=\"" +
          FileName + "\"\r\n\r\n"));

      this.oPostData.Write(fileBytes);

      this.oPostData.Write(Encoding.GetEncoding(1252).GetBytes("\r\n"));

      return true;
    }


    /// <summary>
    /// Return a the result from an HTTP Url into a StreamReader.
    /// Client code should call Close() on the returned object when done reading.
    /// </summary>
    /// <param name="Url">Url to retrieve.</param>
    /// <param name="WebRequest">An HttpWebRequest object that can be passed in with properties preset.</param>
    /// <returns></returns>
    protected StreamReader GetUrlStream(string Url, HttpWebRequest Request)
    {
      try
      {
        this.bError = false;
        this.cErrorMsg = "";


        if (Request == null)
        {
          Request = (HttpWebRequest)System.Net.WebRequest.Create(Url);
        }

        Request.AutomaticDecompression = DecompressionMethods.Deflate;
        Request.UserAgent = this.cUserAgent;
        Request.Timeout = this.nConnectTimeout * 1000;

        // *** Save for external access
        this.oWebRequest = Request;

        // *** Handle Security for the request
        if (this.cUsername.Length > 0)
        {
          if (this.cUsername == "AUTOLOGIN")
            Request.Credentials = CredentialCache.DefaultCredentials;
          else
            Request.Credentials = new NetworkCredential(this.cUsername, this.cPassword);
        }

        // *** Handle Proxy Server configuration
        if (this.cProxyAddress.Length > 0)
        {
          if (this.cProxyAddress == "DEFAULTPROXY")
          {
            Request.Proxy = new WebProxy();
            Request.Proxy = WebProxy.GetDefaultProxy();
          }
          else
          {
            WebProxy loProxy = new WebProxy(this.cProxyAddress, true);
            if (this.cProxyBypass.Length > 0)
            {
              loProxy.BypassList = this.cProxyBypass.Split(';');
            }

            if (this.cProxyUsername.Length > 0)
              loProxy.Credentials = new NetworkCredential(this.cProxyUsername, this.cProxyPassword);

            Request.Proxy = loProxy;
          }
        }

        // *** Handle cookies - automatically re-assign 
        if (this.bHandleCookies)
        {
          Request.CookieContainer = new CookieContainer();
          if (this.oCookies != null && this.oCookies.Count > 0)
          {
            Request.CookieContainer.Add(this.oCookies[0]);
          }
        }

        // *** Deal with the POST buffer if any
        if (this.oPostData != null)
        {
          Request.Method = "POST";
          switch (this.nPostMode)
          {
            case 1:
              Request.ContentType = "application/x-www-form-urlencoded";
              // strip off any trailing & which can cause problems with some 
              // http servers
              //							if (this.cPostBuffer.EndsWith("&"))
              //								this.cPostBuffer = this.cPostBuffer.Substring(0,this.cPostBuffer.Length-1);
              break;
            case 2:
              Request.ContentType = "multipart/form-data; boundary=" + this.cMultiPartBoundary;
              this.oPostData.Write(Encoding.GetEncoding(1252).GetBytes("--" + this.cMultiPartBoundary + "\r\n"));
              break;
            case 4:
              Request.ContentType = "text/xml";
              break;
            default:
              goto case 1;
          }


          Stream loPostData = Request.GetRequestStream();
          //loPostData.Write(lcPostData,0,lcPostData.Length);
          this.oPostStream.WriteTo(loPostData);

          //*** Close the memory stream
          this.oPostStream.Close();
          this.oPostStream = null;

          //*** Close the Binary Writer
          this.oPostData.Close();
          this.oPostData = null;

          //*** Close Request Stream
          loPostData.Close();

          // *** clear the POST buffer
          //this.cPostBuffer = "";
        }

        // *** Handle headers - automatically re-assign 
        if (this.bHandleHeaders)
        {
          if (this.oHeaders != null && this.oHeaders.Count > 0)
          {
            Request.Headers.Add(this.Headers);
          }
        }

        // *** Retrieve the response headers 
        HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();

        // ** Save cookies the server sends
        if (this.bHandleCookies)
        {
          if (Response.Cookies.Count > 0)
          {
            if (this.oCookies == null)
            {
              this.oCookies = Response.Cookies;
            }
            else
            {
              // ** If we already have cookies update the list
              foreach (Cookie oRespCookie in Response.Cookies)
              {
                bool bMatch = false;
                foreach (Cookie oReqCookie in this.oCookies)
                {
                  if (oReqCookie.Name == oRespCookie.Name)
                  {
                    oReqCookie.Value = oRespCookie.Name;
                    bMatch = true;
                    break; // 
                  }
                } // for each ReqCookies
                if (!bMatch)
                  this.oCookies.Add(oRespCookie);
              } // for each Response.Cookies
            }  // this.Cookies == null
          } // if Response.Cookie.Count > 0
        }  // if this.bHandleCookies = 0


        // *** Save the response object for external access
        this.oWebResponse = Response;

        Encoding enc;
        try
        {
          if (Response.ContentEncoding.Length > 0)
            enc = Encoding.GetEncoding(Response.ContentEncoding);
          else
            enc = Encoding.GetEncoding(1252);
        }
        catch
        {
          // *** Invalid encoding passed
          enc = Encoding.GetEncoding(1252);
        }

        // *** drag to a stream
        StreamReader strResponse =
          new StreamReader(Response.GetResponseStream(), enc);
        return strResponse;
      }
      catch (Exception e)
      {
        if (this.bThrowExceptions)
          throw;

        this.cErrorMsg = e.Message;
        this.bError = true;
        return null;
      }
    }

    /// <summary>
    /// Return a the result from an HTTP Url into a StreamReader.
    /// Client code should call Close() on the returned object when done reading.
    /// </summary>
    /// <param name="Url">Url to retrieve.</param>
    /// <returns></returns>
    public StreamReader GetUrlStream(string Url)
    {
      HttpWebRequest oHttpWebRequest = this.WebRequest;
      return this.GetUrlStream(Url, oHttpWebRequest);
    }

    /// <summary>
    /// Return a the result from an HTTP Url into a StreamReader.
    /// Client code should call Close() on the returned object when done reading.
    /// </summary>
    /// <param name="Request">A Request object</param>
    /// <returns></returns>
    public StreamReader GetUrlStream(HttpWebRequest Request)
    {
      return this.GetUrlStream(Request.RequestUri.AbsoluteUri, Request);
    }



    /// <summary>
    /// Return a the result from an HTTP Url into a string.
    /// </summary>
    /// <param name="Url">Url to retrieve.</param>
    /// <returns></returns>
    public string GetUrl(string Url)
    {
      StreamReader oHttpResponse = this.GetUrlStream(Url);
      if (oHttpResponse == null)
        return "";

      string lcResult = oHttpResponse.ReadToEnd();
      oHttpResponse.Close();

      return lcResult;
    }

    /// <summary>
    /// Return a the result from an HTTP Url into a string.
    /// </summary>
    /// <param name="Url">Url to retrieve.</param>
    /// <returns></returns>
    public byte[] GetUrlBytes(string Url)
    {
      StreamReader oHttpResponse = this.GetUrlStream(Url);

      if (oHttpResponse == null)
      {
        return null;
      }

      string lcResult = oHttpResponse.ReadToEnd();
      oHttpResponse.Close();

      return Encoding.UTF8.GetBytes(lcResult);
    }

    /// <summary>
    /// Retrieves URL with events in the OnReceiveData event.
    /// </summary>
    /// <param name="Url"></param>
    /// <param name="BufferSize"></param>
    /// <returns></returns>
    public string GetUrlEvents(string Url, long BufferSize)
    {

      StreamReader oHttpResponse = this.GetUrlStream(Url);
      if (oHttpResponse == null)
        return "";

      long lnSize = BufferSize;
      if (this.oWebResponse.ContentLength > 0)
        lnSize = this.oWebResponse.ContentLength;
      else
        lnSize = 0;

      Encoding enc = Encoding.GetEncoding(1252);

      StringBuilder loWriter = new StringBuilder((int)lnSize);

      char[] lcTemp = new char[BufferSize];

      OnReceiveDataEventArgs oArgs = new OnReceiveDataEventArgs();
      oArgs.TotalBytes = lnSize;

      lnSize = 1;
      int lnCount = 0;
      long lnTotalBytes = 0;

      while (lnSize > 0)
      {
        lnSize = oHttpResponse.Read(lcTemp, 0, (int)BufferSize);
        if (lnSize > 0)
        {
          loWriter.Append(lcTemp, 0, (int)lnSize);
          lnCount++;
          lnTotalBytes += lnSize;

          // *** Raise an event if hooked up
          if (this.OnReceiveData != null)
          {
            /// *** Update the event handler
            oArgs.CurrentByteCount = lnTotalBytes;
            oArgs.NumberOfReads = lnCount;
            oArgs.CurrentChunk = lcTemp;
            this.OnReceiveData(this, oArgs);

            // *** Check for cancelled flag
            if (oArgs.Cancel)
              goto CloseDown;
          }
        }
      } // while


    CloseDown:
      oHttpResponse.Close();

      // *** Send Done notification
      if (this.OnReceiveData != null && !oArgs.Cancel)
      {
        // *** Update the event handler
        oArgs.Done = true;
        this.OnReceiveData(this, oArgs);
      }

      //			return lcHtml;
      return loWriter.ToString();
    }


    public delegate void OnReceiveDataHandler(object sender, OnReceiveDataEventArgs e);
    public class OnReceiveDataEventArgs
    {
      public long CurrentByteCount = 0;
      public long TotalBytes = 0;
      public int NumberOfReads = 0;
      public char[] CurrentChunk;
      public bool Done = false;
      public bool Cancel = false;
    }

  }


}
