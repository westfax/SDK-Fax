using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models
{
  /// <summary>
  /// This is the basic info for a fax.
  /// A structure similar to this is used to communicate with the API to Identify a fax
  /// and to get more info about it.  This works for both inbound and outbound faxes.
  /// </summary>
  public interface IFaxId
  {
    Guid Id { get; set; }
    Direction Direction { get; set; }
    DateTime DateUTC { get; set; }
    string Tag { get; set; }
  }

  /// <summary>
  /// Simple implementation for the IFaxId interface.  Used for querying the API
  /// </summary>
  public class FaxId : IFaxId
  {
    public Guid Id { get; set; }
    public Direction Direction { get; set; }
    public DateTime DateUTC { get; set; }
    public string Tag { get; set; }

    public FaxId() { this.Direction = Models.Direction.Outbound; }

    public FaxId(Guid id) : this() { this.Id = id; }
  }

  /// <summary>
  /// This is a cache control interface.
  /// </summary>
  public interface ICacheControl
  {
    HydrationFlag HydrationFlag { get; set; }
    DateTime HydrationUTC { get; set; }
    //Same as marking an item as final - It won't change.
    bool RequiresTimeExpiration { get; set; }
  }

  /// <summary>
  /// This all the information for a fax.  From this we can contain any information needed.
  /// This class is emitted by the PolkaDotApiInterface.  It may be partially or fully populated
  /// based on the method that was called to hydrate it.  At minimum, the IFaxId properties will
  /// be filled, and it is returned from the PolkaDotApiInterface as an IFaxId.
  /// </summary>
  public class FaxDesc : IFaxId , ICacheControl
  {
    //Cache Control properties - these do not go to the client.
    [Newtonsoft.Json.JsonIgnore]
    public HydrationFlag HydrationFlag { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public DateTime HydrationUTC { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public bool RequiresTimeExpiration { get; set; }
    //End Cache Control

    public Guid Id { get; set; }
    public Direction Direction { get; set; }
    public DateTime DateUTC { get; set; }
    public string Tag { get; set; }

    public int PageCount { get; set; }
    public FaxQuality FaxQuality { get; set; }
    public FaxStatus FaxStatus { get; set; }

    public string Reference { get; set; }
    public string JobName { get; set; }

    public List<FaxCallInfo> FaxCallInfoList { get; set; }
    public List<FaxFileInfo> FaxFileList { get; set; }

    public bool IsUnRead { get { return this.Tag.ToLower() == "none"; } }

    public FaxDesc()
    {
      this.HydrationFlag = HydrationFlag.None;
      this.FaxCallInfoList = new List<FaxCallInfo>();
      this.FaxFileList = new List<FaxFileInfo>();
    }
    

    internal FaxDesc(Internal.FaxIdItem item)
      : this()
    {
      this.Id = item.Id;
      this.Direction = (Direction)Enum.Parse(typeof(Direction), item.Direction, true);
      this.DateUTC = item.Date;
      this.Tag = item.Tag;

      //Cache Control
      this.HydrationFlag = HydrationFlag.Id;
      this.HydrationUTC = DateTime.UtcNow;
      this.RequiresTimeExpiration = true;
    }

    internal FaxDesc(Internal.FaxFileItem item)
      : this()
    {
      this.Id = item.Id;
      this.Direction = (Direction)Enum.Parse(typeof(Direction), item.Direction, true);
      this.DateUTC = item.Date;
      this.Tag = item.Tag;
      var ff = new FaxFileInfo();
      ff.FileFormat = (FileFormat)Enum.Parse(typeof(FileFormat), item.Format, true);
      ff.FaxFiles = item.FaxFiles.Select(i => new FileDetail(i)).ToList();
      this.FaxFileList.Add(ff);
      //Cache Control
      if (ff.FileFormat == FileFormat.Pdf) { this.HydrationFlag = HydrationFlag.Id_FilePdf; }
      if (ff.FileFormat == FileFormat.Tiff) { this.HydrationFlag = HydrationFlag.Id_FileTif; }
      this.HydrationUTC = DateTime.UtcNow;
      this.RequiresTimeExpiration = true;
    }

    internal FaxDesc(Internal.FaxDescItem item)
      : this()
    {
      this.Id = item.Id;
      this.Direction = (Direction)Enum.Parse(typeof(Direction), item.Direction, true);
      this.DateUTC = item.Date;
      this.Tag = item.Tag;
      this.FaxQuality = (FaxQuality)Enum.Parse(typeof(FaxQuality), item.FaxQuality, true);
      this.FaxStatus = (FaxStatus)Enum.Parse(typeof(FaxStatus), item.Status, true);
      this.JobName = item.JobName;
      this.PageCount = item.PageCount;
      this.Reference = item.Reference;
      this.FaxCallInfoList = new List<FaxCallInfo>();
      item.FaxCallInfoList.ForEach(i =>
      {
        var inf = new FaxCallInfo();
        inf.CallId = i.CallId;

        //Call result can fail parsing
        try { inf.CallResult = (CallResult)Enum.Parse(typeof(CallResult), i.Result, true); }
        catch
        {
          switch (i.Result.ToLower().Replace(" ", "").Trim())
          {
            case "inboundfaxreceived":
            case "sent": { inf.CallResult = CallResult.Success; break; }
            case "waitingtodial": { inf.CallResult = CallResult.Dialing; break; }
            case "noanswer":
            case "busy": { inf.CallResult = CallResult.NoAnswer; break; }
            case "invalidcsid":
            case "faxormodemdetected":
            case "fileerror":
            case "connectionfailure":
            case "documentconversionerror":
            case "connectioninterrupt": { inf.CallResult = CallResult.Failed; break; }
            case "areacodeblocked":
            case "tierblocked":
            case "duplicatenumber":
            case "duplicatenumbermax":
            case "blocked": { inf.CallResult = CallResult.Removed; break; }
            case "operatorintercept":
            case "invalidnumber": { inf.CallResult = CallResult.InvalidNumber; break; }
            case "cancelled": { inf.CallResult = CallResult.Cancelled; break; }
            default: { inf.CallResult = CallResult.Unknown; break; }
          }
        }
        inf.CompletedUTC = i.CompletedUTC;
        inf.TermCSID = i.TermCSID;
        inf.TermNumber = i.TermNumber;
        inf.OrigCSID = i.OrigCSID;
        inf.OrigNumber = i.OrigNumber;
        this.FaxCallInfoList.Add(inf);
      });
      //Cache Control
      this.HydrationFlag = HydrationFlag.Id_Desc;
      this.HydrationUTC = DateTime.UtcNow;
      if (this.FaxStatus == FaxStatus.Complete) { this.RequiresTimeExpiration = false; }
      else { this.RequiresTimeExpiration = true; }
    }

    internal Internal.FaxIdItem ToFaxIdItem()
    {
      var ret = new Internal.FaxIdItem();
      ret.Id = this.Id;
      ret.Direction = this.Direction.ToString();
      ret.Date =  this.DateUTC;
      ret.Tag = this.Tag;
      return ret;
    }

    /// <summary>
    /// Static conversion methods.  Just put them here instead of creating a new class.
    /// They are internal only.
    /// </summary>
    internal static Internal.FaxIdItem ToFaxIdItem(IFaxId item)
    {
      return new Internal.FaxIdItem() { Id = item.Id, Direction = item.Direction.ToString(), Date = item.DateUTC, Tag = item.Tag };
    }

    /// <summary>
    /// Static conversion methods.  Just put them here instead of creating a new class.
    /// They are internal only.
    /// </summary>
    internal static List<Internal.FaxIdItem> ToFaxIdItemList(List<IFaxId> items)
    {
      return items.Select(i => FaxDesc.ToFaxIdItem(i)).ToList();
    }

  }

  public class FaxCallInfo
  {
    public Guid CallId { get; set; }
    public DateTime CompletedUTC { get; set; }
    public string TermNumber { get; set; }
    public string OrigNumber { get; set; }
    public string TermCSID { get; set; }
    public string OrigCSID { get; set; }
    public CallResult CallResult { get; set; }
  }

  [Serializable]
  public class FaxFileInfo
  {
    public FileFormat FileFormat { get; set; }
    public int PageCount { get; set; }
    public List<FileDetail> FaxFiles { get; set; }

    public FaxFileInfo()
    {
      this.FaxFiles = new List<FileDetail>();
    }

    public FaxFileInfo(Models.Internal.FaxFileItem item)
    {
      try { this.FileFormat = (FileFormat)Enum.Parse(typeof(FileFormat), item.Format, true); }
      catch { this.FileFormat = Models.FileFormat.Pdf; }

      this.PageCount = item.PageCount;
      this.FaxFiles = item.FaxFiles.Select(i => new FileDetail(i)).ToList();
    }
  }

  [Serializable]
  public class FileDetail
  {
    public string ContentDisposition;
    public string ContentEncoding;
    public int ContentLength;
    public string ContentType;
    public string Filename;
    public byte[] FileContents;
    public string Url;
    
    public string LocalPath;

    public FileDetail() { }

    internal FileDetail(Internal.FileItem item) 
    {
      this.ContentDisposition = item.ContentDisposition;
      this.ContentEncoding = item.ContentEncoding;
      this.ContentLength = item.ContentLength;
      this.ContentType = item.ContentType;
      this.FileContents = item.FileContents;
      this.Filename = item.Filename;
      this.Url = item.Url;
      this.LocalPath = "";
    }
  }


}
