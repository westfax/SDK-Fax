using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WF.SDK.Common;

namespace WF.SDK.Models
{
  [Serializable]
  public class AccountInfo 
  {
    public Guid AccountId { get; set; }
    public string AccountName { get; set; }
    public string AccountNumber { get; set; }
    public string AccountState { get; set; }
    public bool Active { get; set; }
    public TimeZoneName TimeZone {get; set;}

    public User Contact { get; set; }
    public Address BillingAddress { get; set; }

    public AccountInfo()
    { this.AccountId = Guid.Empty; this.AccountName = ""; this.AccountNumber = ""; this.AccountState = ""; this.Active = false; this.TimeZone = TimeZoneName.Eastern; }
  }

  public static class AccountInfoExtensions
  {
    public static AccountInfo ToAccountInfo(this Internal.AccountItem obj)
    {
      if (obj == null) { return null; }
      
      var ret = new AccountInfo();
      ret.AccountId = obj.AccountId;
      ret.AccountName = obj.AccountName;
      ret.AccountNumber = obj.AccountNumber;
      ret.AccountState = obj.AccountState;
      ret.Active = obj.Active;

      try { ret.TimeZone = (TimeZoneName)Enum.Parse(typeof(TimeZoneName), obj.TimeZone, true); }
      catch { ret.TimeZone = TimeZoneName.Eastern; }

      ret.BillingAddress = new Address(obj.BillingAddr);
      ret.Contact = new User(obj.Contact);
     
      return ret;
    }

    public static Internal.AccountItem ToAccountItem(this AccountInfo obj)
    {
      if (obj == null) { return null; }

      var ret = new Internal.AccountItem();
      ret.AccountId = obj.AccountId;
      ret.AccountName = obj.AccountName;
      ret.AccountNumber = obj.AccountNumber;
      ret.AccountState = obj.AccountState;
      ret.Active = obj.Active;
      ret.TimeZone = obj.TimeZone.ToString();

      ret.BillingAddr = obj.BillingAddress.ToAddressItem();
      ret.Contact = obj.Contact.ToUserItem();

      return ret;
    }
  }

}
