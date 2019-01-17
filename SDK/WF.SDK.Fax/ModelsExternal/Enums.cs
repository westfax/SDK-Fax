using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WF.SDK.Common;

namespace WF.SDK.Models
{
  public enum UserRoleLevel
  {
    None,
    [DisplayName("Accounting")]
    Accounting,
    [DisplayName("User")]
    User,
    [DisplayName("Super User")]
    SuperUser,
    [DisplayName("Administrator")]
    Admin,
  }

  public enum ACLType
  {
    Product,
    Account,
  }

  public enum ProductType
  {
    [DisplayName(DisplayName = "None", Tag = "None")]
    None,
    [DisplayName(DisplayName = "Fax To Email", Tag = "FaxForward")]
    FaxToEmail,
    [DisplayName(DisplayName = "Broadcast Fax", Tag = "BroadcastFax")]
    BroadcastFax,
    [DisplayName(DisplayName = "Fax Relay", Tag = "FaxRelay")]
    FaxRelay,
  }

  public enum ProductState
  {
    OK,
    OK_Cancelled,
    Suspended,
    Closed,
  }

  public enum ContactVisibility
  {
    Global,   //Visible to everyone who uses the account
    Public,   //Visible to everyone who uses the product
    Private,  //Visible to the user only
  }

  /// <summary>
  /// The direction of the fax call.
  /// </summary>
  public enum Direction
  {
    Inbound,
    Outbound,
  }

  /// <summary>
  /// The quality of the fax Tif.
  /// </summary>
  public enum FaxQuality
  {
    Fine,
    Normal,
  }

  /// <summary>
  /// The quality of the fax Tif.
  /// </summary>
  public enum FileFormat
  {
    Pdf,
    Tiff,
    Jpeg,
    Png,
    Gif,
  }

  /// <summary>
  /// Job status - This is not a comment on the result of the calls.  This describes the
  /// status of a job (that can have a number of calls on it).  It can be in production, while 
  /// some of its calls are completed, and others are dialing...
  /// </summary>
  public enum FaxStatus
  {
    Complete,   //Job is done dialing.  We are not working on it anymore.  Most will be here.
    Submitted,   //Production pipeline.  Pre-processing.
    Production,  //Production pipeline.   Generally indicates that it is dialing.
    UnSubmitted,   //It is created, but not actually submitted for production.  
    UnAssigned,   //Generally will not see this state.
    Failed,    //Probably will never see this.  Make the same as completed.
    Cancelled,   //Cancelled 
    Paused,   //Paused.  In Production pipeline.
    Submitter,  //Production pipeline.
  }

  /// <summary>
  /// The result of the individual call.
  /// </summary>
  public enum CallResult
  {
    Success,
    Dialing,
    Removed,
    BadNumber,
    Busy,
    NoAnswer,
    NoFaxDevice,
    Cancelled,
    Failed,
    InvalidNumber,
    Unknown,
  }

  /// <summary>
  /// The Amount of Hydration that has occurred.  Once it occurs, the data
  /// can be relied on and kept in the cache.  
  /// Some updates to the State for in work faxes is possible.
  /// </summary>
  [Flags]
  public enum HydrationFlag
  {
    None = 0,
    Id = 1,
    Desc = 2,
    FilePdf = 4,
    FileTif = 8,
    FileAll = HydrationFlag.FileTif | HydrationFlag.FilePdf,

    Id_Desc = HydrationFlag.Id | HydrationFlag.Desc,

    Id_Desc_FilePdf = HydrationFlag.Id | HydrationFlag.Desc | HydrationFlag.FilePdf,
    Id_Desc_FileTif = HydrationFlag.Id | HydrationFlag.Desc | HydrationFlag.FileTif,
    Id_Desc_FileAll_ = HydrationFlag.Id | HydrationFlag.Desc | HydrationFlag.FileAll,

    Id_FilePdf = HydrationFlag.Id | HydrationFlag.FilePdf,
    Id_FileTif = HydrationFlag.Id | HydrationFlag.FileTif,
    Id_FileAll = HydrationFlag.Id | HydrationFlag.FileAll,
  }

  public static class EnumExtensionMethods
  {

    public static ProductType ConvertToSdkProdType(string type)
    {
      if (type.ToLower() == "faxforward") { return ProductType.FaxToEmail; }
      else{return (ProductType)Enum.Parse(typeof(ProductType), type, true);}
    }

    public static string ConvertToPolkaProdString(this ProductType type)
    {
      return type.GetDisplayNameAttribute().Tag;
    }


  }

}
