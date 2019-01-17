using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WF.SDK.Common
{

  public class FFResult<T>
  {
    private Exception _exception = null;
    private string _userString = "";
    private string _url = "";
    private string _warning = "";
    private T _result;

    public FFResult()
    {
    }

    public FFResult(T result)
    {
      this._result = result;
    }

    public FFResult(Exception exception)
    {
      this._exception = exception;
    }

    public FFResult(T result, Exception exception)
    {
      this._result = result;
      this._exception = exception;
    }

    public FFResult(T result, Exception exception, string warning)
    {
      this._result = result;
      this._exception = exception;
      this._warning = warning;
    }

    public FFResult(Exception exception, string warning)
    {
      this._exception = exception;
      this._warning = warning;
    }

    /// <summary>
    /// Optional. The operation did not return an error, but a warning is associated with
    /// the operation.
    /// </summary>
    public string Warning
    {
      get { return this._warning; }
      set { this._warning = value; }
    }

    /// <summary>
    /// The error string to be presented to the user.
    /// </summary>
    public string UserString
    {
      get { return this._userString; }
      set { this._userString = value; }
    }

    /// <summary>
    /// The url that failed in the case of a web service call.  Optional.
    /// </summary>
    public string Url
    {
      get { return this._url; }
      set { this._url = value; }
    }

    /// <summary>
    /// The result object of type <T>
    /// </summary>
    public T Result
    {
      get { return this._result; }
      set { this._result = value; }
    }

    /// <summary>
    /// An exception associated with the operation.
    /// </summary>
    public Exception Exception
    {
      get { return this._exception; }
      set { this._exception = value; }
    }

    /// <summary>
    /// Converts on result type to another.  Leaves default(T) in the result field of the current object.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public void ConvertFrom<U>(FFResult<U> result)
    {
      this._exception = result._exception;
      this._url = result._url;
      this._userString = result._userString;
      this._warning = result._warning;
      this._result = default(T);
    }

    /// <summary>
    /// Get only.  True if no exception ocurred.
    /// </summary>
    [XmlIgnore]
    public bool Success
    {
      get { return this._exception == null; }
    }

    /// <summary>
    /// Get only.  Returns a boolean indicating if a warning is associated with the operation.
    /// </summary>
    [XmlIgnore]
    public bool HasWarning
    {
      get { return this._warning != ""; }
    }


    /// <summary>
    /// Get only.  Returns a bool indicating if an exception ocurred.
    /// </summary>
    [XmlIgnore]
    public bool Failure
    {
      get { return this._exception != null; }
    }


  }
}
