using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{

  [Serializable]
  public class ProductItem
  {
    public Guid Id;
    public string Name;
    public string Detail;

    public string ProductType;

    public string InboundCSID;
    public string InboundNumber;

    public string OutboundCSID;
    public string OutboundANI;

    public string FaxHeader;
    public string TimeZone;
    public string ProductState;

    public ProductItem()
    { }
  }

}
