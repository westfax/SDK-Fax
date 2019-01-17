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
  public class JobStatusItem
  {
    //IFaxIdentifier
    public Guid JobId { get; set; } //the Job Id
    public string JobState { get; set; }
    public bool QuerySuccess { get; set; }
    public decimal JobEstimate { get; set; }

    public Guid Id { get { return this.JobId; } set{this.JobId = value;} }
    public DateTime Date { get; set; }
    public string Direction { get { return "Outbound"; } set{} }  //Outbound only.  You do not use this method on inbound.
    public string Tag { get { return this.JobState; } set { } }

    public List<JobCallStatusItem> Calls = new List<JobCallStatusItem>();

    public JobStatusItem()
    { }
  }

}
