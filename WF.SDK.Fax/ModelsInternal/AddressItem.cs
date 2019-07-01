using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{

  [Serializable]
  public class AddressItem
  {
    public string Company;
    public string Attn;
    public string Address1;
    public string Address2;
    public string City;
    public string State;
    public string Zip;
    public string Country;

    public AddressItem()
    { }
  }

}
