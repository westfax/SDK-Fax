using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models
{
  [Serializable]
  public class Address
  {
    public string Company { get; set; }
    public string Attn { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }

    public Address()
    { }

    public Address(Internal.AddressItem addr)
    {
      this.Company = addr.Company;
      this.Attn = addr.Attn;
      this.Address1 = addr.Address1;
      this.Address2 = addr.Address2;
      this.City = addr.City;
      this.Country = addr.Country;
      this.State = addr.State;
      this.Zip = addr.Zip;
    }

    public Internal.AddressItem ToAddressItem()
    {
      var item = new Internal.AddressItem();

      item.Company = this.Company;
      item.Attn = this.Attn;
      item.Address1 = this.Address1;
      item.Address2 = this.Address2;
      item.City = this.City;
      item.Country = this.Country;
      item.State = this.State;
      item.Zip = this.Zip;

      return item;
    }
  }

}
