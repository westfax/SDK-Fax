using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models
{
  [Serializable]
  public class User
  {
    public string Title {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string CompanyName { get; set; }

    public string Email {get; set;}
    public bool EmailValidated { get; set; }
    public string Fax {get; set;}
    public string MobilePhone {get; set;}
    public string Phone {get; set;}

    public string Address1 {get; set;}
    public string Address2 {get; set;}
    public string City {get; set;}
    public string State {get; set;}
    public string Zip {get; set;}
    public string Country {get; set;}

    public WF.SDK.Common.TimeZoneName TimeZone { get; set; }

    public User()
    { }

    public User(Internal.UserItem item)
    {
      this.Address1 = item.Address1;
      this.Address2 = item.Address2;
      this.City = item.City;
      this.Country = item.Country;
      this.Email = item.Email;
      this.EmailValidated = item.EmailValidated;
      this.Fax = item.Fax;
      this.FirstName = item.FirstName;
      this.LastName = item.LastName;
      this.CompanyName = item.CompanyName;
      this.MobilePhone = item.MobilePhone;
      this.Phone = item.Phone;
      this.State = item.State;
      this.Title = item.Title;
      this.Zip = item.Zip;

      try { this.TimeZone = (WF.SDK.Common.TimeZoneName)Enum.Parse(typeof(WF.SDK.Common.TimeZoneName), item.TimeZone); }
      catch { this.TimeZone = WF.SDK.Common.TimeZoneName.Eastern; }
    }

    public Internal.UserItem ToUserItem()
    {
      var item = new Internal.UserItem();

      item.Address1 = this.Address1;
      item.Address2 = this.Address2;
      item.City = this.City;
      item.Country = this.Country;
      item.Email = this.Email;
      item.Fax = this.Fax;
      item.FirstName = this.FirstName;
      item.LastName = this.LastName;
      item.CompanyName = this.CompanyName;
      item.MobilePhone = this.MobilePhone;
      item.Phone = this.Phone;
      item.State = this.State;
      item.Title = this.Title;
      item.Zip = this.Zip;

      return item;
    }
  }

  public static class UserExtensionMethods
  {
    public static string ToFullName(this User obj)
    {
      if (obj == null) { return ""; }
      return (obj.FirstName + " " + obj.LastName).Trim();
    }
  }

}
