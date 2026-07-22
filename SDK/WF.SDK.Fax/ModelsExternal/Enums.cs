using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WF.SDK.Common;

namespace WF.SDK.Models
{
  public enum UserRoleLevel
  {
    [DisplayName("None")]
    None = 0,
    [DisplayName("Read Only")]
    Read = 1,
    [DisplayName("Send Only User")]
    SendOnly = 2,
    [DisplayName("User")]
    User = 3,
    [DisplayName("Accounting")]
    Accounting = 4,
    [DisplayName("Super User")]
    SuperUser = 5,
    [DisplayName("Administrator")]
    Admin = 6,
  }

  public enum AppType
  {
    UnKnown,
    WebOrderClassic,
    FFWeb,
    Android,
    iOS,
    WF_Printer,
    WF_Console_Web,
    WF_Admin_Web,
    WF_Send_Web,
    WF_Reseller_Web,

    CFT,
    SDK,
  }

  public enum ACLType
  {
    [DisplayName("Fax Line")]
    Product,
    [DisplayName("Account")]
    Account,
    [DisplayName("Reseller")]
    Reseller,
  }

  public enum MatchType
  {
    And,
    Or,
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
    [DisplayName(DisplayName = "OK", Tag = "OK")]
    OK,
    [DisplayName(DisplayName = "Cancelled", Tag = "OK_Cancelled")]
    OK_Cancelled,
    [DisplayName(DisplayName = "Suspended", Tag = "Suspended")]
    Suspended,
    [DisplayName(DisplayName = "Closed", Tag = "Closed")]
    Closed,
  }

  public enum ContactVisibility
  {
    [DisplayName(DisplayName = "Global")]
    Global = 1,   //Visible to everyone who uses the account
    [DisplayName(DisplayName = "Shared")]
    Public = 10,   //Visible to everyone who uses the product
    [DisplayName(DisplayName = "Private")]
    Private = 20,  //Visible to the user only
  }

  public enum FolderRole
  {
    Custom = 1,
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
  /// The format of the file to retrieve
  /// </summary>
  public enum FileFormat
  {

    [DisplayName("PDF", ".pdf")]
    Pdf,
    //Images
    [DisplayName("TIFF", ".tif")]
    Tiff,
    [DisplayName("JPEG", ".jpeg")]
    Jpeg,
    [DisplayName("Portable Network Graphic", ".png")]
    Png,
    [DisplayName("Graphics Interchange Format", ".gif")]
    Gif,
    //OCR Types
    [DisplayName("OCR XML", ".ocr.xml")]
    Xml_Ocr = 1000,
    [DisplayName("OCR JSON", ".ocr.json")]
    Json_Ocr = 1100,
    [DisplayName("OCR Text", ".ocr.txt")]
    Txt_Ocr = 1200,
    [DisplayName("Pdf Searchable", ".ocr.pdf")]
    Pdf_Ocr = 1300,
    [DisplayName("Html Ocr", ".hocr.htm")]
    Html_Ocr = 1400,
    //Workflow results
    [DisplayName("Workflow Classification", ".class.json")]
    Json_Classify = 6001,
    [DisplayName("Workflow Extraction", ".extract.json")]
    Json_Extraction = 6002,

    //Txt,
    //Html,
    //Json,
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
    Cancelled,   //Canceled 
    Paused,   //Paused.  In Production pipeline.
    Submitter,  //Production pipeline.
  }

  /// <summary>
  /// The result of the individual call.
  /// </summary>
  public enum CallResult
  {
    Success,
    Sending,
    Removed,
    BadNumber,
    Busy,
    NoAnswer,
    NoFaxDevice,
    Cancelled,
    Failed,
    InvalidNumber,
    Unknown,
    ConnectionInterrupt,
  }

  /// <summary>
  /// Filter enum
  /// </summary>
  public enum FilterFlag
  {
    None = 0,  //No Flags set - Default.

    Read = 1 << 0,  //Read or Un-Read

    Web_Retrieved = 1 << 1, //Retrieved using Fax Console
    FT_Retrieved = 1 << 2,  //Retrieved using Fax Tools
    CFT_Retrieved = 1 << 3, //Retrieved using CloudFaxToolkit
    RFC_Retrieved = 1 << 4, //Retrieved uinsg RFC (FaxConnector)

    Retrieved = FilterFlag.Web_Retrieved | FilterFlag.FT_Retrieved | FilterFlag.CFT_Retrieved | FilterFlag.RFC_Retrieved,  //Removed from all

    Web_Removed = 1 << 5,  //Removed from Fax Console View (deleted)
    FT_Removed = 1 << 6,  //Removed from Api View (deleted)
    CFT_Removed = 1 << 7, //Removed using CloudFaxToolkit
    RFC_Removed = 1 << 8,  //Removed from RFC View (deleted)

    Removed = FilterFlag.Web_Removed | FilterFlag.FT_Removed | FilterFlag.CFT_Removed | FilterFlag.RFC_Removed,  //Removed from all
  }

  /// <summary>
	/// Search type when matching the filter enum flags
	/// </summary>
	public enum SearchType
  {
    /// <summary>
    /// Returns a search where there is an exact flag match.
    /// </summary>
    Equal = 1,
    /// <summary>
    /// Returns a search where there is NOT an exact flag match.
    /// </summary>
    NotEqual = 2,
    /// <summary>
    /// Returns a search where there is at least one flag set on the object.
    /// </summary>
    OneOf = 3,
    /// <summary>
    /// Returns a search where there is no flag set 
    /// </summary>
    NotOneOf = 4,
    /// <summary>
    /// Ususally used when a single flag is searched for. True if the flag is set on the search target.
    /// </summary>
    IsSet = 5,
    /// <summary>
    /// Ususally used when a single flag is searched for. True if the flag is NOT set on the search target.
    /// </summary>
    IsNotSet = 6,
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

  /// <summary>
  /// Feedback Report Types
  /// </summary>
  public enum FeedbackReportType
  {
    [DisplayName("None")]
    None = 0,
    [DisplayName("Detail")]
    Detail = 1,
  }

  /// <summary>
  /// Feedback Report Options
  /// </summary>
  [Flags]
  public enum AutoReportOption
  {
    [DisplayName("None")]
    None = 0,
    [DisplayName("Inline Thumbnail")]
    InlineThumb = 1,
    [DisplayName("Attach Thumbnail")]
    AttachThumb = 2,
    [DisplayName("Fail Only")]
    FailedOnly = 4,
  }

  public enum Acct2FASetting
  {
    [DisplayName("2FA is Disabled")]
    Disabled = 0,
    [DisplayName("2FA is Optional")]
    Optional = 1,
    [DisplayName("2FA is Required")]
    Required = 2,
  }

  public enum MfaPolicyType
  {
    [DisplayName("2FA is not Set")]
    None = 0,
    [DisplayName("2FA is Disabled")]
    Disabled = 1,
    [DisplayName("2FA is Optional")]
    Optional = 2,
    [DisplayName("2FA is Required")]
    Required = 4,
  }

  public static class EnumExtensionMethods
  {

    public static ProductType ConvertToSdkProdType(string type)
    {
      if (type.ToLower().Contains("forward")) { return ProductType.FaxToEmail; }
      if (type.ToLower().Contains("broadcast")) { return ProductType.BroadcastFax; }
      if (type.ToLower().Contains("relay")) { return ProductType.FaxRelay; }
      else { return (ProductType)Enum.Parse(typeof(ProductType), type, true); }
    }

    public static string ConvertToPolkaProdString(this ProductType type)
    {
      return type.GetDisplayNameAttribute().Tag;
    }
  }

  public enum DetailedOutboundCallResult
  {
    Unassigned = 0,

    [DetailedCallResultInfo(
      cause: "The number is on a removal list. This means that the owner of the number requested not to be called.",
      effect: "The call is blocked to prevent unwanted contact.",
      suggestions: new[] { "Contact support for more information." }
    )]
    RemovalList,

    [DetailedCallResultInfo(
      cause: "This fax line is not allowed to place calls to this tier/region.",
      effect: "The fax was not attempted.",
      suggestions: new[] { "Contact support to allow this fax line to send to this tier/region." }
    )]
    TierBlocked,

    [DetailedCallResultInfo(DetailedOutboundCallResult.TierBlocked)]
    Blocked,

    [DetailedCallResultInfo(
      cause:"The number is invalid.",
      effect: "The fax was not attempted.",
      suggestions: new[] { "Verify the number format.", "Check for typos or missing digits." }
    )]
    InvalidNumber,

    [DetailedCallResultInfo(
      cause: "The call schedule is invalid.",
      effect: "The call could not be made according to the schedule.",
      suggestions: new[] { "Review the call schedule settings.", "Adjust the schedule if needed." })]
    InvalidSchedule,

    [DetailedCallResultInfo(
      cause: "The fax was canceled.",
      effect: "The fax was canceled before it completed. It could have been canceled before the call started or during the call.",
      suggestions: new[] { "Resend the fax." }
    )]
    Canceled,
    
    [DetailedCallResultInfo(
      cause: "The document could not be converted to a fax TIFF.",
      effect: "The fax was not attempted.",
      suggestions: new[] { 
        "Verify the document is valid and not corrupt.",
        "Convert the document to a PDF and try again.",
        "Flatten PDF fields/forms.",
      }
    )]
    DocumentConversionError,

    [DetailedCallResultInfo(
      cause: "The CSID is invalid.",
      effect: "The fax was not attempted.",
      suggestions: new[] { "Verify the CSID configuration.", "Correct any formatting issues." }
    )]
    InvalidCsid,

    [DetailedCallResultInfo(
      cause: "The fax is in progress.",
      suggestions: new string[] { "Wait. The fax could take a while if it has to retry.", "Contact support if the fax has been stuck sending for too long."}
    )]
    WaitingToDial,
    
    [DetailedCallResultInfo(DetailedOutboundCallResult.NoAnswer)]
    Busy,
    [DetailedCallResultInfo(
      cause: "None of the fax attempts were answered.",
      effect: "The fax was not sent.",
      suggestions: new[] { "Try resending the fax.", "Contact the recipient and verify their fax machine is functioning correctly." }
    )]
    NoAnswer,
   
    [DetailedCallResultInfo(
      cause: "None of the fax attempts were able to establish a fax connection. This can mean that the recipient was not a fax machine or had a poor connection.",
      effect: "The fax was not sent.",
      suggestions: new[] { "Verify that the number is correct.", "Contact the recipient and verify their fax machine is functioning correctly." }
    )]
    NoFaxDevice,

    [DetailedCallResultInfo(
      cause: "The call was disconnected before the fax document was fully sent.",
      effect: "The fax was not sent successfully. The recipient may have a partial fax document or no fax document."
    )]
    ConnectionInterrupt,

    [DetailedCallResultInfo(DetailedOutboundCallResult.NoFaxDevice)]
    FaxOrModemDetected,

    [DetailedCallResultInfo(
      cause: "There was an issue connecting to the recipient.",
      effect: "The fax was not sent.",
      suggestions: new[] { 
        "Try resending the fax.",
        "Contact the recipient and verify their fax machine is functioning correctly.",
        "Contact support to see if their is an issue with the recipient's telecommunication carrier."
      }
      )]
    ConnectionFailure,


    [DetailedCallResultInfo(
      cause: "The number could not be reached. The number may have been deactivated. If the sender or recipient recently ported a number, it may take a day or two for this error to be automatically resolved.",
      effect: "The fax was not sent.",
      suggestions: new[] { "Verify that the number is correct.", "Contact the recipient and verify their number is active." }
    )]
    OperatorIntercept,
    [DetailedCallResultInfo(DetailedOutboundCallResult.OperatorIntercept)]
    UnreachableNumber,
    
    [DetailedCallResultInfo(
      effect: "The fax was successfully sent."  
    )]
    Sent,
    [DetailedCallResultInfo(DetailedOutboundCallResult.DocumentConversionError)]
    FileError,
    [DetailedCallResultInfo(DetailedOutboundCallResult.RemovalList)]
    AreaCodeBlocked,
    
    [DetailedCallResultInfo(
      cause: "This number was specified multiple times in the fax recipient list.",
      effect: "This number was removed so that this fax recipient would only receive one copy of the fax."
    )]
    DuplicateNumber,
    [DetailedCallResultInfo(DetailedOutboundCallResult.DuplicateNumber)]
    DuplicateNumberMax,
  }

  public static class DetailedCallResultExtensions
  {
    /// <summary>
    /// Converts the string representation of the name or numeric value of a DetailedOutboundCallResult to a DetailedOutboundCallResult object.
    /// This operation is case-insensitive.
    /// The return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="rawResult">
    /// The string representation of the DetailedOutboundCallResult to convert. 
    /// </param>
    /// <param name="detailedCallResult">
    /// When this method returns, contains a DetailedOutboundCallResult whose value is represented by rawValue. 
    /// This parameter is passed uninitialized.
    /// </param>
    /// <returns>true if the value parameter was converted successfully; otherwise, false.</returns>
    public static bool TryFrom(string rawResult, out DetailedOutboundCallResult detailedCallResult)
    {
      detailedCallResult = default;

      if (String.IsNullOrEmpty(rawResult)) { return false; }

      // Remove spaces and leading/trailing whitespace.
      string formattedResult = rawResult.Trim().Replace(" ", "");

      // Try to parse the rawResult into a DetailedOutboundCallResult.
      if (Enum.TryParse(formattedResult, ignoreCase: true, out detailedCallResult)) { return true; }

      // If the formattedResult failed to parse and is for an inbound fax, default to sent.
      if (formattedResult.ToLower() == "inboundfaxreceived") { detailedCallResult = DetailedOutboundCallResult.Sent; return true; }

      // Handle misspelled word.
      if (formattedResult.ToLower() == "cancelled") { detailedCallResult = DetailedOutboundCallResult.Canceled; return true; }
      // Failed to convert the rawResult to a DetailedOutboundCallResult.
      return false;
    }

    /// <summary>
    /// This will convert the provided DetailedOutboundCallResult to a human readable string representation that can be shown to a user.
    /// </summary>
    public static string ToDisplayResult(this DetailedOutboundCallResult detailedCallResult)
    {
      switch (detailedCallResult)
      {
        case DetailedOutboundCallResult.Sent: return "Sent";

        case DetailedOutboundCallResult.NoFaxDevice:
        case DetailedOutboundCallResult.FaxOrModemDetected: return "No Fax Device";

        case DetailedOutboundCallResult.OperatorIntercept:
        case DetailedOutboundCallResult.InvalidNumber:
        case DetailedOutboundCallResult.UnreachableNumber: return "Number Unreachable";

        case DetailedOutboundCallResult.RemovalList:
        case DetailedOutboundCallResult.AreaCodeBlocked:
        case DetailedOutboundCallResult.TierBlocked:
        case DetailedOutboundCallResult.Blocked:
        case DetailedOutboundCallResult.DuplicateNumber:
        case DetailedOutboundCallResult.DuplicateNumberMax: return "Removed";

        case DetailedOutboundCallResult.NoAnswer: return "No Answer";
        case DetailedOutboundCallResult.Busy: return "Busy";

        case DetailedOutboundCallResult.FileError:
        case DetailedOutboundCallResult.DocumentConversionError: return "Conversion Error";

        case DetailedOutboundCallResult.Canceled: return "Canceled";
        case DetailedOutboundCallResult.WaitingToDial: return "Sending";
        case DetailedOutboundCallResult.ConnectionInterrupt: return "Connection Interrupted";

        case DetailedOutboundCallResult.InvalidSchedule:
        case DetailedOutboundCallResult.InvalidCsid:
        case DetailedOutboundCallResult.ConnectionFailure: return "Failed";

        default: return "Unknown";
      }
    }

    /// <summary>
    /// This will convert the provided DetailedOutboundCallResult to a human readable string representation that can be shown to a user.
    /// If the result has a specific type, that will be returned.
    /// Ex: TierBlocked would return "Removed" with ToDisplayResult, but here it will return "Removed -- Tier Blocked".
    /// </summary>
    public static string ToSpecificDisplayResult(this DetailedOutboundCallResult detailedCallResult)
    {
      switch (detailedCallResult)
      {
        case DetailedOutboundCallResult.Sent: return "Sent";

        case DetailedOutboundCallResult.NoFaxDevice:
        case DetailedOutboundCallResult.FaxOrModemDetected: return "No Fax Device";

        case DetailedOutboundCallResult.OperatorIntercept:
        case DetailedOutboundCallResult.InvalidNumber:
        case DetailedOutboundCallResult.UnreachableNumber: return "Number Unreachable";

        case DetailedOutboundCallResult.TierBlocked: return "Removed -- Tier Blocked";

        case DetailedOutboundCallResult.RemovalList:
        case DetailedOutboundCallResult.AreaCodeBlocked:
        case DetailedOutboundCallResult.Blocked:
        case DetailedOutboundCallResult.DuplicateNumber:
        case DetailedOutboundCallResult.DuplicateNumberMax: return "Removed";

        case DetailedOutboundCallResult.NoAnswer:
        case DetailedOutboundCallResult.Busy: return "No Answer";

        case DetailedOutboundCallResult.FileError:
        case DetailedOutboundCallResult.DocumentConversionError: return "Conversion Error";

        case DetailedOutboundCallResult.Canceled: return "Canceled";
        case DetailedOutboundCallResult.WaitingToDial: return "Sending";
        case DetailedOutboundCallResult.ConnectionInterrupt: return "Connection Interrupted";

        case DetailedOutboundCallResult.InvalidSchedule:
        case DetailedOutboundCallResult.InvalidCsid: return "Failed";

        case DetailedOutboundCallResult.ConnectionFailure: return "Failed -- Connection Failure";

        default: return "Unknown";
      }
    }
  }

  public class ResultHelpMessages
  {
    public readonly DetailedOutboundCallResult Result;
    public readonly string DisplayResult;

    /// <summary>
    /// This is a message about potential causes for this result.
    /// </summary>
    public readonly string Cause;

    /// <summary>
    /// This is a message about the outcome of this result.
    /// </summary>
    public readonly string Effect;

    /// <summary>
    /// This is a list of suggestions on how to fix the failed call result.
    /// </summary>
    public readonly System.Collections.ObjectModel.ReadOnlyCollection<string> Suggestions;

    /// <summary>
    /// Whether or not there is a cause message.
    /// </summary>
    public readonly bool HasCause;
    /// <summary>
    /// Whether or not there is an effect message.
    /// </summary>
    public readonly bool HasEffect;
    /// <summary>
    /// Whether or not there is at least 1 suggestion.
    /// </summary>
    public readonly bool HasSuggestions;
    /// <summary>
    /// Whether or not their are any messages (cause, effect, or suggestion).
    /// </summary>
    public readonly bool HasInformationToDisplay;

    private readonly string[] _suggestions;
    private static readonly Dictionary<DetailedOutboundCallResult, ResultHelpMessages> _resultHelpMessages;

    static ResultHelpMessages()
    {
      // Get all the detailed results.
      Array detailedResults = Enum.GetValues(typeof(DetailedOutboundCallResult));

      // Create a dictionary mapping detailed result to its messages.
      _resultHelpMessages = new Dictionary<DetailedOutboundCallResult, ResultHelpMessages>(detailedResults.Length);
      foreach (DetailedOutboundCallResult result in detailedResults)
      {
        if (_resultHelpMessages.ContainsKey(result)) { continue; }
        _resultHelpMessages.Add(result, new ResultHelpMessages(result));
      }
    }

    private ResultHelpMessages(DetailedOutboundCallResult result)
    {
      this.Result = result;
      this.DisplayResult = result.ToSpecificDisplayResult();

      // Populate the messages.
      {
        DetailedCallResultInfoAttribute attribute = DetailedCallResultInfoAttribute.GetAttribute(result);
        List<Enum> visitedValues = new List<Enum>() { result };

        int linkCount = 0;
        while (attribute != null)
        {
          // If the cause is still needed
          if (String.IsNullOrWhiteSpace(this.Cause))
          {
            // Use the current attribute's cause.
            this.Cause = attribute.Cause;
          }

          // If the effect is still needed
          if (String.IsNullOrWhiteSpace(this.Effect))
          {
            // Use the current attribute's effect.
            this.Effect = attribute.Effect;
          }

          // If the suggestions are still needed
          if (this.Suggestions == null)
          {
            // Use the current attribute's suggestions.
            this._suggestions = attribute.Suggestions?.ToArray();
          }

          if (attribute.LinkedMember == null) { break; }

          // Make sure there is not an infinite link.
          if (visitedValues.Contains(attribute.LinkedMember))
          {
            //throw new Exception("Circular Linked Attribute Detected.");
            break;
          }

          visitedValues.Add(attribute.LinkedMember);
          attribute = attribute.LinkedAttribute;

          // Give up if there were too many linked attributes.
          if (++linkCount > 100)
          {
            //throw new Exception("Too Many Linked Attributes Detected.");
            break;
          }
        }
      }

      this.Suggestions = this._suggestions != null ? new System.Collections.ObjectModel.ReadOnlyCollection<string>(this._suggestions) : null;

      // Populate all the 'Has' fields.
      this.HasEffect = !String.IsNullOrWhiteSpace(this.Effect);
      this.HasCause = !String.IsNullOrWhiteSpace(this.Cause);
      this.HasSuggestions = this.Suggestions?.FirstOrDefault(msg => !String.IsNullOrWhiteSpace(msg)) != null;
      this.HasInformationToDisplay = this.HasCause || this.HasEffect || this.HasSuggestions;
    }

    public static ResultHelpMessages FromDetailedCallResult(DetailedOutboundCallResult callResult)
    {
      ResultHelpMessages helpMessages;
      _resultHelpMessages.TryGetValue(callResult, out helpMessages);
      return helpMessages;
    }

    public static ResultHelpMessages FromDetailedCallResult(DetailedOutboundCallResult? callResult)
    {
      if (callResult.HasValue) { return FromDetailedCallResult(callResult.Value); }
      return null;
    }
  }

  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
  public class DetailedCallResultInfoAttribute : Attribute
  {
    public readonly string Cause;
    public readonly string Effect;
    public readonly string[] Suggestions;

    /// <summary>
    /// If specified, this links to another enum field whose message will be copied.
    /// If this attribute has any messages specified, they will override any linked attributes.
    /// </summary>
    public readonly DetailedOutboundCallResult LinkedMember;

    /// <summary>
    /// If there is a LinkedMember, this will return its DisplayResultHelpAttribute (if it has one).
    /// </summary>
    public DetailedCallResultInfoAttribute LinkedAttribute { get { return GetAttribute(this.LinkedMember); } }

    internal DetailedCallResultInfoAttribute(DetailedOutboundCallResult linkedMember = default, string cause = null, string effect = null, params string[] suggestions)
    {
      this.Cause = cause;
      this.Effect = effect;
      this.LinkedMember = linkedMember;
      this.Suggestions = suggestions;
    }

    internal DetailedCallResultInfoAttribute(DetailedOutboundCallResult linkedValue) { this.LinkedMember = linkedValue; }

    /// <summary>
    /// This will return the attribute if it exists for the provided enum value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    internal static DetailedCallResultInfoAttribute GetAttribute(Enum value)
    {
      try
      {
        if (value == null) { return null; }
        Type type = value.GetType();
        System.Reflection.MemberInfo[] memberInfos = type.GetMember(value.ToString());
        if (memberInfos.Length > 0)
        {
          System.Reflection.MemberInfo info = memberInfos[0];
          if (info != null)
          {
            Attribute attribute = Attribute.GetCustomAttribute(info, typeof(DetailedCallResultInfoAttribute));
            if (attribute is DetailedCallResultInfoAttribute displayHelpAttribute) { return displayHelpAttribute; }
          }
        }
      }
      catch { }
      return null;
    }
  }
  /// <summary>
  /// Enum for fax console inbound viewer reference sorting
  /// </summary>
  public enum FaxConsoleReferenceDropDown
  {
    /// <summary>
    /// Default no drop down
    /// </summary>
    None,
    /// <summary>
    /// Drop down is populated with user logins
    /// </summary>
    Users,
  }
}
