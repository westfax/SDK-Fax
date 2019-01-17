using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{

  [Serializable]
  public class FaxIdItem
  {
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Direction { get; set; }
    public string Tag { get; set; }

    public FaxIdItem()
    { }
  }

}
