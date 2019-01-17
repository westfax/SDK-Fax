using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{

  [Serializable]
  public class FileItem
  {
    public string ContentDisposition { get; set; }
    public string ContentEncoding { get; set; }
    public int ContentLength { get; set; }
    public string ContentType { get; set; }
    public string Filename { get; set; }
    public byte[] FileContents { get; set; }
    public string Url { get; set; }

    public FileItem() { }
  }
 
}
