using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WF.SDK.Fax;
using WF.SDK.Models;

namespace WF.SDK.Fax
{
  public static class FaxInterface
  {
    //Pass through the value on the Raw Interface
    public static string RestUrlTemplate
    {
      get { return Internal.FaxInterfaceRaw.RestUrlTemplate; }
      set { Internal.FaxInterfaceRaw.RestUrlTemplate = value; }
    }

    static FaxInterface()
    {
      //You can configure this statically here and compile, or set the URL template above from another source such as confguration
      //FaxInterface.RestUrlTemplate = System.Configuration.ConfigurationManager.AppSettings["APIEncoding"];  //From config?
      //FaxInterface.RestUrlTemplate = "https://api2.westfax.com/REST/{0}/json";  //Statically?
      //Encoding can be json, json2, xml.
    }

    #region Ping

    public static ApiResult<string> Ping(string pingStr = "")
    {
      var rstr = Internal.FaxInterfaceRaw.Ping(pingStr);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    #endregion

    #region Authenticate

    public static ApiResult<string> Authenticate(string username, string password, Guid? productId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.Authenticate(username, password, productId);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }
            
    #endregion

    #region GetProductList

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// </summary>
    public static ApiResult<List<Product>> GetProductList(string username, string password)
    {
      var rstr = Internal.FaxInterfaceRaw.GetProductList(username, password);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.ProductItem>>>(rstr);
      var ret = new ApiResult<List<Product>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => new Product(i)).ToList();
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = new List<Product>();
      }

      return ret;
    }
        
    #endregion

    #region GetF2EProductList

    /// /// <summary>
    /// ProductType can be FaxForward only
    /// </summary>
    public static ApiResult<List<Product>> GetF2EProductList(string username, string password)
    {
      var rstr = Internal.FaxInterfaceRaw.GetF2EProductList(username, password);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.F2EProductItem>>>(rstr);
      var ret = new ApiResult<List<Product>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => new Product(i)).ToList();
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = new List<Product>();
      }

      return ret;
    }
    
    #endregion

    #region GetF2EProductDetail

    /// <summary>
    /// ProductType can be FaxForward only
    /// </summary>
    public static ApiResult<Product> GetF2EProductDetail(string username, string password, Guid productId)
    {
      var rstr = Internal.FaxInterfaceRaw.GetF2EProductDetail(username, password, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Models.Internal.F2EProductItem>>(rstr);
      var ret = new ApiResult<Product>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = new Product(result.Result);
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = null;
      }

      return ret;
    }

    #endregion

    #region GetAccountInfo

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay.  
    /// </summary>
    public static ApiResult<AccountInfo> GetAccountInfo(string username, string password, Guid? productId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.GetAccountInfo(username, password, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Models.Internal.AccountItem>>(rstr);
      var ret = new ApiResult<AccountInfo>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = AccountInfoExtensions.ToAccountInfo(result.Result);
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = null;
      }

      return ret;
    }

    #endregion

    #region GetUserProfile

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// </summary>
    public static ApiResult<User> GetUserProfile(string username, string password, Guid? productId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.GetUserProfile(username, password, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Models.Internal.UserItem>>(rstr);
      var ret = new ApiResult<User>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = new User(result.Result);
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = null;
      }

      return ret;
    }

    #endregion

    #region GetLoginInfo

    public static ApiResult<LoginInfo> GetLoginInfo(string username, string password)
    {
      var rstr = Internal.FaxInterfaceRaw.GetLoginInfo(username, password);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Models.Internal.LoginInfoItem>>(rstr);

      var ret = new ApiResult<LoginInfo>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = new LoginInfo(result.Result);
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = null;
      }

      return ret;
    }

    #endregion

    #region GetContactList
    /// <summary>
    /// Retrieves a list of the contacts that are available to the user, based on user role.
    /// Contacts appropriate for the product will be filtered if the ProductId is given.
    /// </summary>
    public static ApiResult<List<Models.Contact>> GetContactList(string username, string password, Guid? productId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.GetContactList(username, password, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.ContactItem>>>(rstr);
      var ret = new ApiResult<List<Models.Contact>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => new Models.Contact(i)).ToList();
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = new List<Models.Contact>();
      }

      return ret;
    }

    #endregion

    #region GetFaxIds (Inbound and Outbound)
    /// <summary>
    /// Gets all the faxes.
    /// </summary>
    public static ApiResult<List<IFaxId>> GetFaxIds(string username, string password, Guid productId)
    {
      var rstr = Internal.FaxInterfaceRaw.GetFaxIds(username, password, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.FaxIdItem>>>(rstr);

      var ret = new ApiResult<List<IFaxId>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => (IFaxId)(new FaxDesc(i))).ToList();
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = new List<IFaxId>();
      }

      return ret;
    }

    /// <summary>
    /// Get The inbound Faxes.
    /// </summary>
    public static ApiResult<List<IFaxId>> GetInboundFaxIds(string username, string password, Guid productId)
    {
      var rstr = Internal.FaxInterfaceRaw.GetInboundFaxIds(username, password, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.FaxIdItem>>>(rstr);

      var ret = new ApiResult<List<IFaxId>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => (IFaxId)(new FaxDesc(i))).ToList();
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = new List<IFaxId>();
      }

      return ret;
    }

    /// <summary>
    /// Get The outbound Faxes.
    /// </summary>
    public static ApiResult<List<IFaxId>> GetOutboundFaxIds(string username, string password, Guid productId)
    {
      var rstr = Internal.FaxInterfaceRaw.GetOutboundFaxIds(username, password, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.FaxIdItem>>>(rstr);

      var ret = new ApiResult<List<IFaxId>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => (IFaxId)(new FaxDesc(i))).ToList();
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = new List<IFaxId>();
      }

      return ret;
    }

    #endregion

    #region GetFaxDescriptions (Inbound and outbound)

    /// <summary>
    /// Get The inbound Faxes
    /// </summary>
    public static ApiResult<List<IFaxId>> GetInboundFaxDescriptions(string username, string password, Guid productId)
    {
      var rstr = Internal.FaxInterfaceRaw.GetInboundFaxDescriptions(username, password, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.FaxDescItem>>>(rstr);

      var ret = new ApiResult<List<IFaxId>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => (IFaxId)(new FaxDesc(i))).ToList();
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = new List<IFaxId>();
      }

      return ret;
    }

    /// <summary>
    /// Get The outbound Faxes
    /// </summary>
    public static ApiResult<List<IFaxId>> GetOutboundFaxDescriptions(string username, string password, Guid productId)
    {
      var rstr = Internal.FaxInterfaceRaw.GetOutboundFaxDescriptions(username, password, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.FaxDescItem>>>(rstr);

      var ret = new ApiResult<List<IFaxId>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => (IFaxId)(new FaxDesc(i))).ToList();
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = new List<IFaxId>();
      }

      return ret;
    }

    /// <summary>
    /// Get the requested Faxes
    /// </summary>
    public static ApiResult<List<IFaxId>> GetFaxDescriptions(string username, string password, Guid productId, List<IFaxId> items)
    {
      var rstr = Internal.FaxInterfaceRaw.GetFaxDescriptions(username, password, productId, FaxDesc.ToFaxIdItemList(items));
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.FaxDescItem>>>(rstr);

      var ret = new ApiResult<List<IFaxId>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        try { ret.Result = result.Result.Select(i => (IFaxId)(new FaxDesc(i))).ToList(); }
        catch { ret.Result = new List<IFaxId>(); }
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = new List<IFaxId>();
      }

      return ret;
    }

    #endregion

    #region GetFaxDocuments
    /// <summary>
    /// Get the requested Faxes
    /// </summary>
    public static ApiResult<List<IFaxId>> GetFaxDocuments(string username, string password, Guid productId, List<IFaxId> items, FileFormat format = FileFormat.Pdf)
    {

      var rstr = Internal.FaxInterfaceRaw.GetFaxDocuments(username, password, productId, FaxDesc.ToFaxIdItemList(items), format.ToString().ToLower());
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.FaxFileItem>>>(rstr);

      var ret = new ApiResult<List<IFaxId>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => (IFaxId)(new FaxDesc(i))).ToList();
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = new List<IFaxId>();
      }

      return ret;
    }

    #endregion

    #region GetProductsWithInboundFaxes
    /// <summary>
    /// Get the products that have faxes matching the given filter.  Usually used to 
    /// determine if there are faxes waiting for download, and what products may have them.
    /// </summary>
    public static ApiResult<List<Product>> GetProductsWithInboundFaxes(string username, string password, string filter = "None")
    {
      var rstr = Internal.FaxInterfaceRaw.GetProductsWithInboundFaxes(username, password, filter);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.ProductItem>>>(rstr);
      var ret = new ApiResult<List<Product>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = result.ErrorString;
        ret.InfoString = result.InfoString;
        ret.Result = result.Result.Select(i => new Product(i)).ToList();
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.InfoString = result.InfoString;
        ret.Result = new List<Product>();
      }

      return ret;

    }
    #endregion

    #region SendFax
    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static ApiResult<string> SendFax(string username, string password, Guid productId,
      List<string> numbers, HttpFileCollection files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "", 
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys1 = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(username, password, productId, numbers, files, csid, ani, startDate, faxQuality, jobname, header, billingCode, feedbackEmail, callbackUrl, custKeys1);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static ApiResult<string> SendFax(string username, string password, Guid productId,
      List<string> numbers, FileDetail file,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "", 
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys1 = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(username, password, productId, numbers, file.FileContents, file.Filename, csid, ani, startDate, faxQuality, jobname, header, billingCode, feedbackEmail, callbackUrl, custKeys1);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static ApiResult<string> SendFax(string username, string password, Guid productId,
      List<string> numbers, List<string> filePaths,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys1 = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(username, password, productId, numbers, filePaths, csid, ani, startDate, faxQuality, jobname, header, billingCode, feedbackEmail, callbackUrl, custKeys1);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }
    #endregion

    #region MarkAsRead

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// Must have a contact object defined.
    /// </summary>
    public static ApiResult<bool> MarkAsRead(string username, string password, Guid productId, List<IFaxId> items)
    {
      var rstr = Internal.FaxInterfaceRaw.ChangeFaxFilterValue(username, password, productId, FaxDesc.ToFaxIdItemList(items), "Retrieved");
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    #endregion

    #region MarkAsUnRead

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// Must have a contact object defined.
    /// </summary>
    public static ApiResult<bool> MarkAsUnRead(string username, string password, Guid productId, List<IFaxId> items)
    {
      var rstr = Internal.FaxInterfaceRaw.ChangeFaxFilterValue(username, password, productId, FaxDesc.ToFaxIdItemList(items), "None");
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    #endregion

    #region MarkAsDeleted

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// Must have a contact object defined.
    /// </summary>
    public static ApiResult<bool> MarkAsDeleted(string username, string password, Guid productId, List<IFaxId> items)
    {
      var rstr = Internal.FaxInterfaceRaw.ChangeFaxFilterValue(username, password, productId, FaxDesc.ToFaxIdItemList(items), "Removed");
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    #endregion

    #region SendFaxAsEmail
    /// <summary>
    /// Send the fax as an email.  Works on inbound and outbound.
    /// </summary>
    public static ApiResult<bool> SendFaxAsEmail(string username, string password, Guid productId, IFaxId item, string emailAddress)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFaxAsEmail(username, password, productId, FaxDesc.ToFaxIdItem(item), emailAddress);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }
    #endregion

    #region ResendFaxNotification
    /// <summary>
    /// Send the fax as an email.  Works on inbound only.
    /// </summary>
    public static ApiResult<bool> ResendFaxNotification(string username, string password, Guid productId, IFaxId item)
    {
      var rstr = Internal.FaxInterfaceRaw.ResendFaxNotification(username, password, productId, FaxDesc.ToFaxIdItem(item));
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }
    #endregion

    #region ConvertToFaxDocument
    /// <summary>
    /// Converts files to Fax files.  
    /// </summary>
    public static ApiResult<FaxFileInfo> ConvertToFaxDocument(string username, string password, 
      HttpFileCollection files, string faxQuality = "Fine", Models.FileFormat format = FileFormat.Tiff)
    {
      var rstr = Internal.FaxInterfaceRaw.ConvertToFaxDocument(username, password, files, faxQuality, format.ToString());
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Models.Internal.FaxFileItem>>(rstr);

      var ret = new ApiResult<FaxFileInfo>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = new FaxFileInfo(result.Result);
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = null;
      }

      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static ApiResult<FaxFileInfo> ConvertToFaxDocument(string username, string password,
      FileDetail file, string faxQuality = "Fine", Models.FileFormat format = FileFormat.Tiff)
    {
      var rstr = Internal.FaxInterfaceRaw.ConvertToFaxDocument(username, password, file.FileContents, file.Filename, faxQuality, format.ToString());
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Models.Internal.FaxFileItem>>(rstr);

      var ret = new ApiResult<FaxFileInfo>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = new FaxFileInfo(result.Result);
      }
      else
      {
        ret.Success = false;
        ret.ErrorString = result.ErrorString;
        ret.Result = null;
      }

      return ret;
    }
    #endregion



  }
}