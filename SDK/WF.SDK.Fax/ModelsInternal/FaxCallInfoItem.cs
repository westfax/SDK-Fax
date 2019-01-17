using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{


  [Serializable]
  public class FaxCallInfoItem
  {
    public Guid CallId { get; set; }
    public DateTime CompletedUTC { get; set; }
    public string TermNumber { get; set; }
    public string OrigNumber { get; set; }
    public string TermCSID { get; set; }
    public string OrigCSID { get; set; }
    public string Result { get; set; }

    public FaxCallInfoItem()
    { }
  }


}
