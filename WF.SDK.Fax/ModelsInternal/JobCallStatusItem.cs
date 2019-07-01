using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{
  /// <summary>
  /// Represents the result of a job status request.  (GetFaxStatus method call)
  /// This is somewhat similar to a FaxDesc.  This job status contains different
  /// info from the FaxDesc.  This is for Outbound only.
  /// </summary>
  [Serializable]
  public class JobCallStatusItem
  {
    public DateTime Date { get; set; }
    public string Direction { get { return "Outbound"; } set { } }  //Outbound only.  You do not use this method on inbound.
    public string Tag { get; set; }

    public string PhoneNumber { get; set; }
    public string CallResult { get; set; }    //Complete, in work, etc....
    public int JobPages { get; set; }  //Pages Submitted
    public int PagesSent { get; set; }  //Pages Sent
    public int DurationSeconds { get; set; }
    public DateTime CompleteUtc { get { return this.Date; } set { this.Date = value; } }
    public int FaxConnectionSpeed { get; set; }


    public JobCallStatusItem()
    { }
  }

}
