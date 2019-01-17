using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models
{
  [Serializable]
  public class Contact
  {
    public Guid Id { get; set; }
    public string Title {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string CompanyName { get; set; }

    public string Email {get; set;}
    public string FaxNumber {get; set;}
    public string MobileNumber {get; set;}
    public string PhoneNumber {get; set;}

    public ContactVisibility Type { get; set; }
    public Guid? OwnerId { get; set; }

    public Contact()
    { }

    public Contact(Internal.ContactItem item)
    {
      this.Id = item.Id;
      this.Email = item.Email;
      this.FaxNumber = item.Fax;
      this.FirstName = item.FirstName;
      this.LastName = item.LastName;
      this.CompanyName = item.CompanyName;
      this.MobileNumber = item.MobilePhone;
      this.PhoneNumber = item.Phone;
      this.Title = item.Title;
      this.Type = (ContactVisibility)Enum.Parse(typeof(ContactVisibility), item.Type, true);
      this.OwnerId = item.OwnerId;
    }

    
  }

  public static class ContactExtensionMethods
  {
    public static string ToFullName(this Contact obj)
    {
      if (obj == null) { return ""; }
      return (obj.FirstName + " " + obj.LastName).Trim();
    }

    public static Internal.ContactItem ToContactItem(this Contact obj)
    {
      var item = new Internal.ContactItem();

      item.Id = obj.Id;
      item.Email = obj.Email;
      item.Fax = obj.FaxNumber;
      item.FirstName = obj.FirstName;
      item.LastName = obj.LastName;
      item.CompanyName = obj.CompanyName;
      item.MobilePhone = obj.MobileNumber;
      item.Phone = obj.PhoneNumber;
      item.Title = obj.Title;
      item.Type = obj.Type.ToString();
      item.OwnerId = obj.OwnerId;

      return item;
    }

  }

}
