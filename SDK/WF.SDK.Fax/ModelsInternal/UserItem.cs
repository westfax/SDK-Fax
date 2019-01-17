using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{


  [Serializable]
  public class UserItem
  {
    public Guid Id;
    public string Title;
    public string FirstName;
    public string LastName;
    public string CompanyName;

    public string Email;
    public bool EmailValidated;
    public string Fax;
    public string MobilePhone;
    public string Phone;

    public string Address1;
    public string Address2;
    public string City;
    public string State;
    public string Zip;
    public string Country;

    public string TimeZone;

    public UserItem()
    { }
  }


}
