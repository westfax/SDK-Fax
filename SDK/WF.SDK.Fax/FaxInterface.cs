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

    public static AppType AppType
    {
      get { return Internal.FaxInterfaceRaw.AppType; }
      set { Internal.FaxInterfaceRaw.AppType = value; }
    }

    public static string AppSource
    {
      get { return Internal.FaxInterfaceRaw.AppSource; }
      set { Internal.FaxInterfaceRaw.AppSource = value; }
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

    public static ApiResult<string> Authenticate(string apiKey, Guid? productId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.Authenticate(apiKey, productId);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    #endregion

    #region GetProductList

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// </summary>
    public static ApiResult<List<Product>> GetProductList(string username, string password, Guid? accountId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.GetProductList(username, password,accountId);
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

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// </summary>
    public static ApiResult<List<Product>> GetProductList(string apiKey, Guid? accountId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.GetProductList(apiKey, accountId);
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

    /// /// <summary>
    /// ProductType can be FaxForward only
    /// </summary>
    public static ApiResult<List<Product>> GetF2EProductList(string apiKey)
    {
      var rstr = Internal.FaxInterfaceRaw.GetF2EProductList(apiKey);
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

    /// <summary>
    /// ProductType can be FaxForward only
    /// </summary>
    public static ApiResult<Product> GetF2EProductDetail(string apiKey, Guid productId)
    {
      var rstr = Internal.FaxInterfaceRaw.GetF2EProductDetail(apiKey, productId);
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

    #region SearchF2EProducts

    /// <summary>
    /// Can search for F2E product type.  Returns a page of products.
    /// Sort value can be "name" or "number".
    /// </summary>
    public static ApiResult<Models.Page<Models.Product>> SearchF2EProducts(string username, string password, int page = 1, int itemsPerPage = 10,
      string phoneMatch = "", string nameMatch = "", string nameSearch = "", UserRoleLevel level = UserRoleLevel.User, string sortCol = "number", string sortDir = "asc")
    {
      return FaxInterface.SearchAllProducts(username, password, page, itemsPerPage, ProductType.FaxToEmail, phoneMatch, nameMatch, nameSearch, level, sortCol, sortDir);
    }

    /// <summary>
    /// Can search for F2E product type.  Returns a page of products.
    /// Sort value can be "name" or "number".
    /// </summary>
    public static ApiResult<Models.Page<Models.Product>> SearchF2EProducts(string apiKey, int page = 1, int itemsPerPage = 10,
      string phoneMatch = "", string nameMatch = "", string nameSearch = "", UserRoleLevel level = UserRoleLevel.User, string sortCol = "number", string sortDir = "asc")
    {
      return FaxInterface.SearchAllProducts(apiKey, page, itemsPerPage, ProductType.FaxToEmail, phoneMatch, nameMatch, nameSearch, level, sortCol, sortDir);
    }

    #endregion

    #region SearchAllProducts

    /// <summary>
    /// Can search for any product type. Returns a page of products.
    /// Sort value can be "name" or "number".
    /// </summary>
    public static ApiResult<Models.Page<Models.Product>> SearchAllProducts(string username, string password,
      int page = 1, int itemsPerPage = 10, Models.ProductType type = Models.ProductType.FaxToEmail,
      string phoneMatch = "", string nameMatch = "", string nameSearch = "", UserRoleLevel level = UserRoleLevel.User, string sortCol = "number", string sortDir = "asc", Guid? accountId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SearchAllProducts(username, password, page, itemsPerPage, type.ConvertToPolkaProdString(), phoneMatch, nameMatch, nameSearch, level.ToString(), sortCol, sortDir, accountId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Models.Page<Models.Internal.ProductItem>>>(rstr);
      var ret = new ApiResult<Models.Page<Models.Product>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.ToProductPage();
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
    /// Can search for any product type. Returns a page of products.
    /// Sort value can be "name" or "number".
    /// </summary>
    public static ApiResult<Models.Page<Models.Product>> SearchAllProducts(string apiKey,
      int page = 1, int itemsPerPage = 10, Models.ProductType type = Models.ProductType.FaxToEmail,
      string phoneMatch = "", string nameMatch = "", string nameSearch = "", UserRoleLevel level = UserRoleLevel.User, string sortCol = "number", string sortDir = "asc", Guid? accountId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SearchAllProducts(apiKey, page, itemsPerPage, type.ConvertToPolkaProdString(), phoneMatch, nameMatch, nameSearch, level.ToString(), sortCol, sortDir, accountId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Models.Page<Models.Internal.ProductItem>>>(rstr);
      var ret = new ApiResult<Models.Page<Models.Product>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.ToProductPage();
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
    public static ApiResult<AccountInfo> GetAccountInfo(string username, string password, Guid? productId = null, Guid? accountId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.GetAccountInfo(username, password, productId, accountId);
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

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay.  
    /// </summary>
    public static ApiResult<AccountInfo> GetAccountInfo(string apiKey, Guid? productId = null, Guid? accountId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.GetAccountInfo(apiKey, productId, accountId);
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
    /// <summary>
    /// Retrieves a list of the contacts that are available to the user, based on user role.
    /// Contacts appropriate for the product will be filtered if the ProductId is given.
    /// </summary>
    public static ApiResult<List<Models.Contact>> GetContactList(string apiKey, Guid? productId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.GetContactList(apiKey, productId);
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

    #region GetAllContactList
    /// <summary>
    /// Retrieves a list of the contacts that are available to the user, based on user role.
    /// Contacts appropriate for the product will be filtered if the ProductId is given.
    /// </summary>
    public static ApiResult<List<Models.Contact>> GetAllContactList(string username, string password)
    {
      var rstr = Internal.FaxInterfaceRaw.GetAllContactList(username, password);
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

    /// <summary>
    /// Retrieves a list of the contacts that are available to the user, based on user role.
    /// Contacts appropriate for the product will be filtered if the ProductId is given.
    /// </summary>
    public static ApiResult<List<Models.Contact>> GetAllContactList(string apiKey)
    {
      var rstr = Internal.FaxInterfaceRaw.GetAllContactList(apiKey);
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

    #region SearchContactList_ForSend
    /// <summary>
    /// This method returns the viewable or usable contacts.  Editing may noit be possible on all of these.  
    /// Use this to get or filter a list of contacts when sending a fax.
    /// </summary>
    public static ApiResult<Models.Page<Models.Contact>> SearchContactList_ForSend(string username, string password, Guid productId, int page = 1, int itemsPerPage = 10,
      string searchString = null, List<ContactVisibility> filters = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SearchContactList_ForSend(username, password, productId, page, itemsPerPage, searchString, filters);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Page<Models.Internal.ContactItem>>>(rstr);

      var ret = new ApiResult<Models.Page<Models.Contact>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.ToContactPage();
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
    /// This method returns the viewable or usable contacts.  Editing may noit be possible on all of these.  
    /// Use this to get or filter a list of contacts when sending a fax.
    /// </summary>
    public static ApiResult<Models.Page<Models.Contact>> SearchContactList_ForSend(string apiKey, Guid productId, int page = 1, int itemsPerPage = 10,
      string searchString = null, List<ContactVisibility> filters = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SearchContactList_ForSend(apiKey, productId, page, itemsPerPage, searchString, filters);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Page<Models.Internal.ContactItem>>>(rstr);

      var ret = new ApiResult<Models.Page<Models.Contact>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.ToContactPage();
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

    #region GetFaxIds (Inbound and Outbound)
    /// <summary>
    /// Gets all the faxes.
    /// </summary>
    public static ApiResult<List<IFaxId>> GetFaxIds(string username, string password, Guid productId, bool include0PageCalls)
    {
      var rstr = Internal.FaxInterfaceRaw.GetFaxIds(username, password, productId, include0PageCalls);
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
    /// Gets all the faxes.
    /// </summary>
    public static ApiResult<List<IFaxId>> GetFaxIds(string apiKey, Guid productId, bool include0PageCalls)
    {
      var rstr = Internal.FaxInterfaceRaw.GetFaxIds(apiKey, productId, include0PageCalls);
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
    public static ApiResult<List<IFaxId>> GetInboundFaxIds(string username, string password, Guid productId, bool include0PageCalls)
    {
      var rstr = Internal.FaxInterfaceRaw.GetInboundFaxIds(username, password, productId, include0PageCalls);
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
    public static ApiResult<List<IFaxId>> GetInboundFaxIds(string apiKey, Guid productId, bool include0PageCalls)
    {
      var rstr = Internal.FaxInterfaceRaw.GetInboundFaxIds(apiKey, productId, include0PageCalls);
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

    /// <summary>
    /// Get The outbound Faxes.
    /// </summary>
    public static ApiResult<List<IFaxId>> GetOutboundFaxIds(string apiKey, Guid productId)
    {
      var rstr = Internal.FaxInterfaceRaw.GetOutboundFaxIds(apiKey, productId);
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
    public static ApiResult<List<IFaxId>> GetInboundFaxDescriptions(string username, string password, Guid productId, bool include0PageCalls)
    {
      var rstr = Internal.FaxInterfaceRaw.GetInboundFaxDescriptions(username, password, productId, include0PageCalls);
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
    /// Get The inbound Faxes
    /// </summary>
    public static ApiResult<List<IFaxId>> GetInboundFaxDescriptions(string apiKey, Guid productId, bool include0PageCalls)
    {
      var rstr = Internal.FaxInterfaceRaw.GetInboundFaxDescriptions(apiKey, productId, include0PageCalls);
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
    /// Get The outbound Faxes
    /// </summary>
    public static ApiResult<List<IFaxId>> GetOutboundFaxDescriptions(string apiKey, Guid productId)
    {
      var rstr = Internal.FaxInterfaceRaw.GetOutboundFaxDescriptions(apiKey, productId);
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

    /// <summary>
    /// Get the requested Faxes
    /// </summary>
    public static ApiResult<List<IFaxId>> GetFaxDescriptions(string apiKey, Guid productId, List<IFaxId> items)
    {
      var rstr = Internal.FaxInterfaceRaw.GetFaxDescriptions(apiKey, productId, FaxDesc.ToFaxIdItemList(items));
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

    #region SearchF2EFaxDescriptions (Commented)
    ///// <summary>
    ///// Get the requested Faxes
    ///// </summary>
    //public static ApiResult<Models.Page<Models.FaxDesc>> SearchF2EFaxDescriptions(string username, string password, Guid productId, int page = 1, int itemsPerPage = 10,
    //  Direction direction = Direction.Inbound, string filter = null, string numberMatch = null, string csidSearch = null, string stateMatch = null, string cityMatch = null,
    //  DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    //{

    //  var rstr = Internal.FaxInterfaceRaw.SearchF2EFaxDescriptions(username, password, productId, null, page, itemsPerPage, direction.ToString(),
    //    filter, numberMatch, csidSearch, stateMatch, cityMatch, startUtc, endUtc, include0Page);
    //  var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Page<Models.Internal.FaxDescItem>>>(rstr);

    //  var ret = new ApiResult<Models.Page<Models.FaxDesc>>();

    //  if (result.Success)
    //  {
    //    ret.Success = true;
    //    ret.ErrorString = "";
    //    ret.Result = result.Result.ToFaxDescPage();
    //  }
    //  else
    //  {
    //    ret.Success = false;
    //    ret.ErrorString = result.ErrorString;
    //    ret.Result = null;
    //  }

    //  return ret;
    //}

    ///// <summary>
    ///// Get the requested Faxes
    ///// </summary>
    //public static ApiResult<Models.Page<Models.FaxDesc>> SearchF2EFaxDescriptions_ByOwnerLoginId(string username, string password, Guid productId, Guid? ownerLoginId, int page = 1, int itemsPerPage = 10,
    //  Direction direction = Direction.Inbound, string filter = null, string numberMatch = null, string csidSearch = null, string stateMatch = null, string cityMatch = null,
    //  DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    //{

    //  var rstr = Internal.FaxInterfaceRaw.SearchF2EFaxDescriptions(username, password, productId, ownerLoginId, page, itemsPerPage, direction.ToString(),
    //    filter, numberMatch, csidSearch, stateMatch, cityMatch, startUtc, endUtc, include0Page);
    //  var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Page<Models.Internal.FaxDescItem>>>(rstr);

    //  var ret = new ApiResult<Models.Page<Models.FaxDesc>>();

    //  if (result.Success)
    //  {
    //    ret.Success = true;
    //    ret.ErrorString = "";
    //    ret.Result = result.Result.ToFaxDescPage();
    //  }
    //  else
    //  {
    //    ret.Success = false;
    //    ret.ErrorString = result.ErrorString;
    //    ret.Result = null;
    //  }

    //  return ret;
    //}
    #endregion

    #region GetF2EFaxDescriptions_PagedSearch

    /// <summary>
    /// Get the requested Faxes
    /// </summary>
    public static ApiResult<Models.Page<Models.FaxDesc>> GetF2EFaxDescriptions_PagedSearch(string username, string password, Guid productId, Guid? ownerLoginId, int page = 1, int itemsPerPage = 10,
      Direction direction = Direction.Inbound, List<string> filterList = null, string numberMatch = null, string stringMatch = null, MatchType match = MatchType.Or,
      DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {

      var rstr = Internal.FaxInterfaceRaw.GetF2EFaxDescriptions_PagedSearch(username, password, productId, ownerLoginId, page, itemsPerPage, direction.ToString(),
                filterList, numberMatch, stringMatch, match.ToString(), startUtc, endUtc, include0Page);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Page<Models.Internal.FaxDescItem>>>(rstr);

      var ret = new ApiResult<Models.Page<Models.FaxDesc>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.ToFaxDescPage();
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
    /// Get the requested Faxes
    /// </summary>
    public static ApiResult<Models.Page<Models.FaxDesc>> GetF2EFaxDescriptions_PagedSearch(string apiKey, Guid productId, Guid? ownerLoginId, int page = 1, int itemsPerPage = 10,
      Direction direction = Direction.Inbound, List<string> filterList = null, string numberMatch = null, string stringMatch = null, MatchType match = MatchType.Or,
      DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {

      var rstr = Internal.FaxInterfaceRaw.GetF2EFaxDescriptions_PagedSearch(apiKey, productId, ownerLoginId, page, itemsPerPage, direction.ToString(),
                filterList, numberMatch, stringMatch, match.ToString(), startUtc, endUtc, include0Page);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Page<Models.Internal.FaxDescItem>>>(rstr);

      var ret = new ApiResult<Models.Page<Models.FaxDesc>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.ToFaxDescPage();
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

    /// <summary>
    /// Get the requested Faxes
    /// </summary>
    public static ApiResult<List<IFaxId>> GetFaxDocuments(string apiKey, Guid productId, List<IFaxId> items, FileFormat format = FileFormat.Pdf)
    {
      var rstr = Internal.FaxInterfaceRaw.GetFaxDocuments(apiKey, productId, FaxDesc.ToFaxIdItemList(items), format.ToString().ToLower());
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
    public static ApiResult<List<Product>> GetProductsWithInboundFaxes(string username, string password, string filter = "None", bool include0PageCalls = false)
    {
      var rstr = Internal.FaxInterfaceRaw.GetProductsWithInboundFaxes(username, password, filter, include0PageCalls);
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

    /// <summary>
    /// Get the products that have faxes matching the given filter.  Usually used to 
    /// determine if there are faxes waiting for download, and what products may have them.
    /// 
    /// This is identical to <see cref="GetProductsWithInboundFaxes(string, string, string, bool)"/> except for the ApiKey vs Username/Password. 
    /// This needed _ApiKey added to the name because calling with (string, string) would be ambiguous.
    /// </summary>
    public static ApiResult<List<Product>> GetProductsWithInboundFaxes_ApiKey(string apiKey, string filter = "None", bool include0PageCalls = false)
    {
      var rstr = Internal.FaxInterfaceRaw.GetProductsWithInboundFaxes(apiKey, filter, include0PageCalls);
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
    /// Takes a HttpFileCollection.
    /// ------------------
    /// Extra Params is a Dictionary<string, string> and can contain the following keys:
    /// impersonateLoginId (Guid) - The login Id To impersonate for the submission.  Requires Account Admin to impersonate another user.
    /// createdByName (string) - In the absense of a login, this shows in the Web portal as the sender name.
    /// ------------------
    /// Cover page options (in the Extra Params Dictionary):
    /// coverPageId (Guid) - The Id of the coverpage to use if any.  If the system cannot find the coverpage, the fax will fail.
    /// coverPageUseDefault (bool) - Search for a "default" starting at product level, and then searching account level.
    /// coverPageMergeFirstFile (bool) - Directs the merge system to merge the data onto the first file in the supplied file list.  This allows customers to supply a cover page of their choosing.
    /// Cover page merge fields - These are the values to merge into the document.  Not all documents contain all of these fields.
    /// coverPageDate - The date string to use on the cover page (optional - will default to the current date and time if omitted)
    /// coverPagePageCount - The number of pages in the fax including the cover page (optional - it is good to omit this.  The page count will autopopulate if ommitted)
    /// coverPageSenderName - The Sender Name to merge on to the cover page (optional - will use the name on the sender's login - impersonation will be honored)
    /// coverPageMessage = The Message to merge on to the cover page (optional - will default nothing if omitted)
    /// coverPageRecipientCompany - The company name of the recipient (optional - will default nothing if omitted)
    /// coverPageRecipientFax - The fax number name of the recipient (optional - will default to the dialed number if omitted)
    /// coverPageRecipientName - The person name of the recipient (optional - will default nothing if omitted)
    /// coverPageSubject - A subject field (optional - will default nothing if omitted)
    /// coverPageReference - A fax reference.  (optional - will default nothing if omitted)
    /// coverPageIdCode - A fax identifier - future use.  Will render a QR or Bar code.  (optional - will default nothing if omitted
    /// ------------------
    /// </summary>
    public static ApiResult<string> SendFax(string username, string password, Guid productId, List<string> numbers,
      List<HttpPostedFile> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(username, password, productId, numbers, files, csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail, callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a HttpFileCollection.
    /// This has enhanced Feedback email object
    /// </summary>
    public static ApiResult<string> SendFax(string username, string password, Guid productId, List<string> numbers,
      List<HttpPostedFile> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmail feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(username, password, productId, numbers, files, csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail.ToFeedbackEmailItem(), callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a HttpFileCollection.
    /// </summary>
    public static ApiResult<string> SendFax(string username, string password, Guid productId, List<string> numbers,
      HttpFileCollection files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(username, password, productId, numbers, files, csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail, callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a HttpFileCollection.
    /// This has enhanced Feedback email object
    /// </summary>
    public static ApiResult<string> SendFax(string username, string password, Guid productId, List<string> numbers,
      HttpFileCollection files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmail feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(username, password, productId, numbers, files, csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail.ToFeedbackEmailItem(), callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a File Detail object.
    /// </summary>
    public static ApiResult<string> SendFax(string username, string password, Guid productId, List<string> numbers,
      List<FileDetail> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(username, password, productId, numbers, files.ToFileItems(), csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail, callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a File Detail object.
    /// This has enhanced Feedback email object
    /// </summary>
    public static ApiResult<string> SendFax(string username, string password, Guid productId, List<string> numbers,
      List<FileDetail> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmail feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(username, password, productId, numbers, files.ToFileItems(), csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail.ToFeedbackEmailItem(), callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a list of local file paths.
    /// </summary>
    public static ApiResult<string> SendFax(string username, string password, Guid productId, List<string> numbers,
      List<string> filePaths,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(username, password, productId, numbers, filePaths, csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail, callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a list of local file paths.
    /// This has enhanced Feedback email object
    /// </summary>
    public static ApiResult<string> SendFax(string username, string password, Guid productId, List<string> numbers,
      List<string> filePaths,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmail feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(username, password, productId, numbers, filePaths, csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail.ToFeedbackEmailItem(), callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }
    #endregion

    #region SendFax (Api Key)

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a HttpFileCollection.
    /// ------------------
    /// Extra Params is a Dictionary<string, string> and can contain the following keys:
    /// impersonateLoginId (Guid) - The login Id To impersonate for the submission.  Requires Account Admin to impersonate another user.
    /// createdByName (string) - In the absense of a login, this shows in the Web portal as the sender name.
    /// ------------------
    /// Cover page options (in the Extra Params Dictionary):
    /// coverPageId (Guid) - The Id of the coverpage to use if any.  If the system cannot find the coverpage, the fax will fail.
    /// coverPageUseDefault (bool) - Search for a "default" starting at product level, and then searching account level.
    /// coverPageMergeFirstFile (bool) - Directs the merge system to merge the data onto the first file in the supplied file list.  This allows customers to supply a cover page of their choosing.
    /// Cover page merge fields - These are the values to merge into the document.  Not all documents contain all of these fields.
    /// coverPageDate - The date string to use on the cover page (optional - will default to the current date and time if omitted)
    /// coverPagePageCount - The number of pages in the fax including the cover page (optional - it is good to omit this.  The page count will autopopulate if ommitted)
    /// coverPageSenderName - The Sender Name to merge on to the cover page (optional - will use the name on the sender's login - impersonation will be honored)
    /// coverPageMessage = The Message to merge on to the cover page (optional - will default nothing if omitted)
    /// coverPageRecipientCompany - The company name of the recipient (optional - will default nothing if omitted)
    /// coverPageRecipientFax - The fax number name of the recipient (optional - will default to the dialed number if omitted)
    /// coverPageRecipientName - The person name of the recipient (optional - will default nothing if omitted)
    /// coverPageSubject - A subject field (optional - will default nothing if omitted)
    /// coverPageReference - A fax reference.  (optional - will default nothing if omitted)
    /// coverPageIdCode - A fax identifier - future use.  Will render a QR or Bar code.  (optional - will default nothing if omitted
    /// ------------------
    /// </summary>
    public static ApiResult<string> SendFax(string apiKey, Guid productId, List<string> numbers,
      List<HttpPostedFile> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(apiKey, productId, numbers, files, csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail, callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a HttpFileCollection.
    /// This has enhanced Feedback email object
    /// </summary>
    public static ApiResult<string> SendFax(string apiKey, Guid productId, List<string> numbers,
      List<HttpPostedFile> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmail feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(apiKey, productId, numbers, files, csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail.ToFeedbackEmailItem(), callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a HttpFileCollection.
    /// </summary>
    public static ApiResult<string> SendFax(string apiKey, Guid productId, List<string> numbers,
      HttpFileCollection files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(apiKey, productId, numbers, files, csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail, callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a HttpFileCollection.
    /// This has enhanced Feedback email object
    /// </summary>
    public static ApiResult<string> SendFax(string apiKey, Guid productId, List<string> numbers,
      HttpFileCollection files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmail feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(apiKey, productId, numbers, files, csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail.ToFeedbackEmailItem(), callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a File Detail object.
    /// </summary>
    public static ApiResult<string> SendFax(string apiKey, Guid productId, List<string> numbers,
      List<FileDetail> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null, string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(apiKey, productId, numbers, files.ToFileItems(), csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail, callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a File Detail object.
    /// This has enhanced Feedback email object
    /// </summary>
    public static ApiResult<string> SendFax(string apiKey, Guid productId, List<string> numbers,
      List<FileDetail> files,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmail feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(apiKey, productId, numbers, files.ToFileItems(), csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail.ToFeedbackEmailItem(), callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a list of local file paths.
    /// </summary>
    public static ApiResult<string> SendFax(string apiKey, Guid productId, List<string> numbers,
      List<string> filePaths,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      string feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(apiKey, productId, numbers, filePaths, csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail, callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// Takes a list of local file paths.
    /// This has enhanced Feedback email object
    /// </summary>
    public static ApiResult<string> SendFax(string apiKey, Guid productId, List<string> numbers,
      List<string> filePaths,
      string csid, string ani, DateTime? startDate = null, string faxQuality = "Fine",
      string jobname = "", string header = "", string billingCode = "",
      FeedbackEmail feedbackEmail = null,
      string callbackUrl = null, List<string> custKeys = null, Dictionary<string, string> extraParams = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFax(apiKey, productId, numbers, filePaths, csid, ani, startDate, faxQuality, jobname ?? "", header ?? "", billingCode ?? "", feedbackEmail.ToFeedbackEmailItem(), callbackUrl, custKeys, extraParams);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<string>>(rstr);
      return ret;
    }
    #endregion

    #region CancelFax
    /// <summary>
    /// Get the fax statuses
    /// </summary>
    public static ApiResult<FaxId> CancelFax(string username, string password, Guid productId, Guid id)
    {
      var rstr = Internal.FaxInterfaceRaw.CancelFax(username, password, productId, id);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<FaxId>>(rstr);
      return result;
    }

    /// <summary>
    /// Get the fax statuses
    /// </summary>
    public static ApiResult<FaxId> CancelFax(string apiKey, Guid productId, Guid id)
    {
      var rstr = Internal.FaxInterfaceRaw.CancelFax(apiKey, productId, id);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<FaxId>>(rstr);
      return result;
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

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// Must have a contact object defined.
    /// </summary>
    public static ApiResult<bool> MarkAsRead(string apiKey, Guid productId, List<IFaxId> items)
    {
      var rstr = Internal.FaxInterfaceRaw.ChangeFaxFilterValue(apiKey, productId, FaxDesc.ToFaxIdItemList(items), "Retrieved");
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

    /// <summary>
    /// ProductType can be FaxForward, BroadcastFax, FaxRelay
    /// Must have a contact object defined.
    /// </summary>
    public static ApiResult<bool> MarkAsUnRead(string apiKey, Guid productId, List<IFaxId> items)
    {
      var rstr = Internal.FaxInterfaceRaw.ChangeFaxFilterValue(apiKey, productId, FaxDesc.ToFaxIdItemList(items), "None");
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    #endregion

    #region MarkAsRemoved

    /// <summary>
    /// Product Type must be FaxForward - Marks as Removed - Moves to Deleted Items Folder
    /// </summary>
    public static ApiResult<bool> MarkAsRemoved(string username, string password, Guid productId, List<IFaxId> items)
    {
      var rstr = Internal.FaxInterfaceRaw.ChangeFaxFilterValue(username, password, productId, FaxDesc.ToFaxIdItemList(items), "Removed");
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    /// <summary>
    /// Product Type must be FaxForward - Marks as Removed - Moves to Deleted Items Folder
    /// </summary>
    public static ApiResult<bool> MarkAsRemoved(string apiKey, Guid productId, List<IFaxId> items)
    {
      var rstr = Internal.FaxInterfaceRaw.ChangeFaxFilterValue(apiKey, productId, FaxDesc.ToFaxIdItemList(items), "Removed");
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    #endregion

    #region MarkAsDeleted

    /// <summary>
    /// Product Type must be FaxForward - Marks as Deleted - Will never show in interface again.
    /// </summary>
    public static ApiResult<bool> MarkAsDeleted(string username, string password, Guid productId, List<IFaxId> items)
    {
      var rstr = Internal.FaxInterfaceRaw.ChangeFaxFilterValue(username, password, productId, FaxDesc.ToFaxIdItemList(items), "Deleted");
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    /// <summary>
    /// Product Type must be FaxForward - Marks as Deleted - Will never show in interface again.
    /// </summary>
    public static ApiResult<bool> MarkAsDeleted(string apiKey, Guid productId, List<IFaxId> items)
    {
      var rstr = Internal.FaxInterfaceRaw.ChangeFaxFilterValue(apiKey, productId, FaxDesc.ToFaxIdItemList(items), "Deleted");
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    #endregion

    #region SendFaxAsEmail
    /// <summary>
    /// Send the fax as an email.  Works on inbound and outbound.
    /// </summary>
    public static ApiResult<bool> SendFaxAsEmail(string username, string password, Guid productId, IFaxId item, string emailAddress, string subject = null, string message = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFaxAsEmail(username, password, productId, FaxDesc.ToFaxIdItem(item), emailAddress, subject, message);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send the fax as an email.  Works on inbound and outbound.
    /// </summary>
    public static ApiResult<bool> SendFaxAsEmail(string apiKey, Guid productId, IFaxId item, string emailAddress, string subject = null, string message = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SendFaxAsEmail(apiKey, productId, FaxDesc.ToFaxIdItem(item), emailAddress, subject, message);
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

    /// <summary>
    /// Send the fax as an email.  Works on inbound only.
    /// </summary>
    public static ApiResult<bool> ResendFaxNotification(string apiKey, Guid productId, IFaxId item)
    {
      var rstr = Internal.FaxInterfaceRaw.ResendFaxNotification(apiKey, productId, FaxDesc.ToFaxIdItem(item));
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }
    #endregion

    #region ResendFaxFeedbackEmail
    /// <summary>
    /// Send the fax as an email.  Works on inbound only.
    /// </summary>
    public static ApiResult<bool> ResendFaxFeedbackEmail(string username, string password, Guid productId, Guid id, string emailAddress)
    {
      if (string.IsNullOrEmpty(emailAddress)) { return new ApiResult<bool>(false, "Email Address is required.", false); }
      var rstr = Internal.FaxInterfaceRaw.ResendFaxFeedbackEmail(username, password, productId, id, emailAddress);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    /// <summary>
    /// Send the fax as an email.  Works on inbound only.
    /// </summary>
    public static ApiResult<bool> ResendFaxFeedbackEmail(string apiKey, Guid productId, Guid id, string emailAddress)
    {
      if (string.IsNullOrEmpty(emailAddress)) { return new ApiResult<bool>(false, "Email Address is required.", false); }
      var rstr = Internal.FaxInterfaceRaw.ResendFaxFeedbackEmail(apiKey, productId, id, emailAddress);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }
    #endregion

    #region ConvertToFaxDocument
    /// <summary>
    /// Converts files to Fax files.  
    /// </summary>
    public static ApiResult<FaxFileInfo> ConvertToFaxDocument(string username, string password,
      System.Web.HttpFileCollection files, string faxQuality = "Fine", Models.FileFormat format = FileFormat.Tiff)
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
    /// Converts files to Fax files.  
    /// </summary>
    public static ApiResult<FaxFileInfo> ConvertToFaxDocument(string apiKey,
      System.Web.HttpFileCollection files, string faxQuality = "Fine", Models.FileFormat format = FileFormat.Tiff)
    {
      var rstr = Internal.FaxInterfaceRaw.ConvertToFaxDocument(apiKey, files, faxQuality, format.ToString());
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

    /// <summary>
    /// Send a Fax now.
    /// Fax quality is Fine or Normal.
    /// </summary>
    public static ApiResult<FaxFileInfo> ConvertToFaxDocument(string apiKey,
      FileDetail file, string faxQuality = "Fine", Models.FileFormat format = FileFormat.Tiff)
    {
      var rstr = Internal.FaxInterfaceRaw.ConvertToFaxDocument(apiKey, file.FileContents, file.Filename, faxQuality, format.ToString());
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

    #region GetFaxCoverPages
    /// <summary>
    /// Retrieves the Fax Cover Pages that can be used on the indicated product. This should be used in 
    /// conjunction with a fax send operation.  These are the usable cover pages for the product.  This 
    /// should not be used to get the cover pages that can be edited.  Use the profile method for that.
    /// </summary>
    public static ApiResult<List<FaxCoverPage>> GetFaxCoverPages(string username, string password, Guid? productId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.GetFaxCoverPages(username, password, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.FaxCoverPageItem>>>(rstr);
      var ret = new ApiResult<List<FaxCoverPage>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => new FaxCoverPage(i)).ToList();
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
    /// Retrieves the Fax Cover Pages that can be used on the indicated product. This should be used in 
    /// conjunction with a fax send operation.  These are the usable cover pages for the product.  This 
    /// should not be used to get the cover pages that can be edited.  Use the profile method for that.
    /// </summary>
    public static ApiResult<List<FaxCoverPage>> GetFaxCoverPages(string apiKey, Guid? productId = null)
    {
      var rstr = Internal.FaxInterfaceRaw.GetFaxCoverPages(apiKey, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.FaxCoverPageItem>>>(rstr);
      var ret = new ApiResult<List<FaxCoverPage>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => new FaxCoverPage(i)).ToList();
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
    public static ApiResult<bool> SaveFaxFields(string username, string password, Guid productId, IFaxId item, string jobName = null, string reference = null, string filterValue = null, Guid? ownerLoginId = null, List<string> tagList = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SaveFaxFields(username, password, productId, FaxDesc.ToFaxIdItem(item), jobName, reference, filterValue, ownerLoginId, tagList);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
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
    public static ApiResult<bool> SaveFaxFields(string apiKey, Guid productId, IFaxId item, string jobName = null, string reference = null, string filterValue = null, Guid? ownerLoginId = null, List<string> tagList = null)
    {
      var rstr = Internal.FaxInterfaceRaw.SaveFaxFields(apiKey, productId, FaxDesc.ToFaxIdItem(item), jobName, reference, filterValue, ownerLoginId, tagList);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }
    #endregion

    #region GetAllF2EFaxDescriptions_Paged

    /// <summary>
    /// Get the requested Faxes
    /// </summary>
    public static ApiResult<Models.Page<Models.FaxDesc>> GetAllF2EFaxDescriptions_Paged(string username, string password, Guid productId, Guid? ownerLoginId, int page = 1, int itemsPerPage = 10,
        Direction? direction = null, List<string> filter = null,
        DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {

      var rstr = Internal.FaxInterfaceRaw.GetAllF2EFaxDescriptions_Paged(username, password, productId, ownerLoginId, page, itemsPerPage, direction.HasValue ? direction.ToString() : null,
          filter, startUtc, endUtc, include0Page);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Page<Models.Internal.FaxDescItem>>>(rstr);

      var ret = new ApiResult<Models.Page<Models.FaxDesc>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.ToFaxDescPage();
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
    /// Get the requested Faxes
    /// </summary>
    public static ApiResult<Models.Page<Models.FaxDesc>> GetAllF2EFaxDescriptions_Paged(string apiKey, Guid productId, Guid? ownerLoginId, int page = 1, int itemsPerPage = 10,
        Direction? direction = null, List<string> filter = null,
        DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {

      var rstr = Internal.FaxInterfaceRaw.GetAllF2EFaxDescriptions_Paged(apiKey, productId, ownerLoginId, page, itemsPerPage, direction.HasValue ? direction.ToString() : null,
          filter, startUtc, endUtc, include0Page);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Page<Models.Internal.FaxDescItem>>>(rstr);

      var ret = new ApiResult<Models.Page<Models.FaxDesc>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.ToFaxDescPage();
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

    #region GetF2EFaxDescriptions_Paged

    /// <summary>
    /// Get the requested Faxes
    /// </summary>
    public static ApiResult<Models.Page<Models.FaxDesc>> GetF2EFaxDescriptions_Paged(string username, string password, Guid productId, Models.Folder folder, Guid? ownerLoginId, int page = 1, int itemsPerPage = 10,
        Direction? direction = null, List<string> filter = null,
        DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {

      var rstr = Internal.FaxInterfaceRaw.GetF2EFaxDescriptions_Paged(username, password, productId, folder != null ? folder.ToFolderItem() : null, ownerLoginId, page, itemsPerPage, direction.HasValue ? direction.ToString() : null,
          filter, startUtc, endUtc, include0Page);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Page<Models.Internal.FaxDescItem>>>(rstr);

      var ret = new ApiResult<Models.Page<Models.FaxDesc>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.ToFaxDescPage();
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
    /// Get the requested Faxes
    /// </summary>
    public static ApiResult<Models.Page<Models.FaxDesc>> GetF2EFaxDescriptions_Paged(string apiKey, Guid productId, Models.Folder folder, Guid? ownerLoginId, int page = 1, int itemsPerPage = 10,
        Direction? direction = null, List<string> filter = null,
        DateTime? startUtc = null, DateTime? endUtc = null, bool include0Page = false)
    {

      var rstr = Internal.FaxInterfaceRaw.GetF2EFaxDescriptions_Paged(apiKey, productId, folder != null ? folder.ToFolderItem() : null, ownerLoginId, page, itemsPerPage, direction.HasValue ? direction.ToString() : null,
          filter, startUtc, endUtc, include0Page);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Page<Models.Internal.FaxDescItem>>>(rstr);

      var ret = new ApiResult<Models.Page<Models.FaxDesc>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.ToFaxDescPage();
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

    #region MoveFaxToFolder

    /// <summary>
    /// Move Faxes to a different Folder
    /// </summary>
    public static ApiResult<bool> MoveFaxToFolder(string username, string password, Guid productId, List<IFaxId> faxIds, Models.Folder folder = null)
    {
      var rstr = Internal.FaxInterfaceRaw.MoveFaxToFolder(username, password, productId, FaxDesc.ToFaxIdItemList(faxIds), folder != null ? folder.ToFolderItem() : null);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    /// <summary>
    /// Move Faxes to a different Folder
    /// </summary>
    public static ApiResult<bool> MoveFaxToFolder(string apiKey, Guid productId, List<IFaxId> faxIds, Models.Folder folder = null)
    {
      var rstr = Internal.FaxInterfaceRaw.MoveFaxToFolder(apiKey, productId, FaxDesc.ToFaxIdItemList(faxIds), folder != null ? folder.ToFolderItem() : null);
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }
    #endregion

    #region CheckFolderName

    /// <summary>
    /// Checks to see if the folder name is available
    /// </summary>
    public static ApiResult<bool> CheckFolderName(string username, string password, Guid productId, Models.Folder folder)
    {
      var rstr = Internal.FaxInterfaceRaw.CheckFolderName(username, password, productId, folder.ToFolderItem());
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    /// <summary>
    /// Checks to see if the folder name is available
    /// </summary>
    public static ApiResult<bool> CheckFolderName(string apiKey, Guid productId, Models.Folder folder)
    {
      var rstr = Internal.FaxInterfaceRaw.CheckFolderName(apiKey, productId, folder.ToFolderItem());
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }
    #endregion

    #region DeleteFolder

    /// <summary>
    /// Deletes an existing folder
    /// </summary>
    public static ApiResult<bool> DeleteFolder(string username, string password, Guid productId, Models.Folder folder)
    {
      var rstr = Internal.FaxInterfaceRaw.DeleteFolder(username, password, productId, folder.ToFolderItem());
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }

    /// <summary>
    /// Deletes an existing folder
    /// </summary>
    public static ApiResult<bool> DeleteFolder(string apiKey, Guid productId, Models.Folder folder)
    {
      var rstr = Internal.FaxInterfaceRaw.DeleteFolder(apiKey, productId, folder.ToFolderItem());
      var ret = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<bool>>(rstr);
      return ret;
    }
    #endregion

    #region SaveFolder

    /// <summary>
    /// Save a new or existing folder
    /// </summary>
    public static ApiResult<Models.Folder> SaveFolder(string username, string password, Guid productId, Models.Folder folder)
    {

      var rstr = Internal.FaxInterfaceRaw.SaveFolder(username, password, productId, folder.ToFolderItem());
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Models.Internal.FolderItem>>(rstr);

      var ret = new ApiResult<Models.Folder>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = new Models.Folder(result.Result);
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
    /// Save a new or existing folder
    /// </summary>
    public static ApiResult<Models.Folder> SaveFolder(string apiKey, Guid productId, Models.Folder folder)
    {

      var rstr = Internal.FaxInterfaceRaw.SaveFolder(apiKey, productId, folder.ToFolderItem());
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<Models.Internal.FolderItem>>(rstr);

      var ret = new ApiResult<Models.Folder>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = new Models.Folder(result.Result);
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

    #region GetFolderList

    /// <summary>
    /// Get the requested Folders
    /// </summary>
    public static ApiResult<List<Models.Folder>> GetFolderList(string username, string password, Guid productId)
    {

      var rstr = Internal.FaxInterfaceRaw.GetFolderList(username, password, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.FolderItem>>>(rstr);

      var ret = new ApiResult<List<Models.Folder>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => new Models.Folder(i)).ToList();
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
    /// Get the requested Folders
    /// </summary>
    public static ApiResult<List<Models.Folder>> GetFolderList(string apiKey, Guid productId)
    {

      var rstr = Internal.FaxInterfaceRaw.GetFolderList(apiKey, productId);
      var result = WF.SDK.Common.JSONSerializerHelper.Deserialize<ApiResult<List<Models.Internal.FolderItem>>>(rstr);

      var ret = new ApiResult<List<Models.Folder>>();

      if (result.Success)
      {
        ret.Success = true;
        ret.ErrorString = "";
        ret.Result = result.Result.Select(i => new Models.Folder(i)).ToList();
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