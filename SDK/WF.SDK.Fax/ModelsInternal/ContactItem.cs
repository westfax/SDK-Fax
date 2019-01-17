using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{
 
  [Serializable]
  public class ContactItem
  {
    public Guid Id;
    public string Title;
    public string FirstName;
    public string LastName;
    public string CompanyName;

    public string Email;
    public string Fax;
    public string MobilePhone;
    public string Phone;

    public string Type;
    public Guid? OwnerId;

    public ContactItem()
    { }
  }

}
