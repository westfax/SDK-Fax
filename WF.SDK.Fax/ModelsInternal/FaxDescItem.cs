using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{

  [Serializable]
  public class FaxDescItem
  {
    //IFaxIdentifier
    public Guid Id { get; set; } //the fileresource id or Job Id
    public DateTime Date { get; set; }
    public string Direction { get; set; }  //Inbound or Outbound
    public string Tag { get; set; }

    public int PageCount { get; set; }
    public string FaxQuality { get; set; }
    public string Status { get; set; }    //Complete, in work, etc....
    public bool UnRead { get; set; }    //Whether it has been retrieved or not.

    public string Reference { get; set; }
    public string JobName { get; set; }

    public List<FaxCallInfoItem> FaxCallInfoList = new List<FaxCallInfoItem>();

    public FaxDescItem()
    { }
  }

}
