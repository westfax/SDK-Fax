using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{

  [Serializable]
  public class FaxFileItem
  {
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Direction { get; set; }  //Inbound or Outbound
    public string Tag { get; set; }
    public string Status { get; set; }

    
    public string Format { get; set; }
    public int PageCount { get; set; }
    public List<FileItem> FaxFiles;

    public FaxFileItem()
    {
      this.FaxFiles = new List<FileItem>();
    }
  }

}
