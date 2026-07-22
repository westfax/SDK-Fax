using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WF.SDK.Common;

namespace WF.SDK.Models
{
  [Serializable]
  public class AccountInfo : IId
  {
    /// <summary>
    ///  Interface Implementation - same as AccountId
    /// </summary>
    public Guid Id { get{return this.AccountId ;} set{this.AccountId = value;} }

    public Guid AccountId { get; set; }
    public Guid ResellerId { get; set; }
    public string AccountName { get; set; }
    public string AccountNumber { get; set; }
    public string AccountState { get; set; }
    public bool Active { get; set; }
    public TimeZoneName TimeZone {get; set;}
    public Acct2FASetting MfaPolicy { get; set; }

    public User Contact { get; set; }

    public Address BillingAddress { get; set; }

    public AccountRestrictions AccountRestrictions { get; set; }

    public FaxConsoleReferenceDropDown FaxConsoleReferenceDropDown { get; set; } = FaxConsoleReferenceDropDown.None;

    public bool AllowMultipleWebsiteSessions { get; set;  }

    public AccountInfo()
    { this.AccountId = Guid.Empty; this.AccountName = ""; this.AccountNumber = ""; this.AccountState = ""; this.Active = false; this.TimeZone = TimeZoneName.Eastern; }
  }

  /// <summary>
  /// Has the Account Restrictions. Internal and External Models share the same type.
  /// </summary>
  [Serializable]
  public class AccountRestrictions
  {
    public bool ResellerHasUserAdmin;

    public AccountRestrictions() { }
  }

  public static class AccountInfoExtensions
  {

    public static AccountInfo ToAccountInfo(this Internal.AccountItem obj)
    {
      if (obj == null) { return null; }
      
      var ret = new AccountInfo();
      ret.AccountId = obj.AccountId;
      ret.ResellerId = obj.ResellerId;
      ret.AccountName = obj.AccountName;
      ret.AccountNumber = obj.AccountNumber;
      ret.AccountState = obj.AccountState;
      ret.Active = obj.Active;

      try { ret.TimeZone = (TimeZoneName)Enum.Parse(typeof(TimeZoneName), obj.TimeZone, true); }
      catch { ret.TimeZone = TimeZoneName.Eastern; }

      try { ret.MfaPolicy = (Acct2FASetting) Enum.Parse(typeof(Acct2FASetting), obj.MfaPolicy); }
      catch { ret.MfaPolicy = Acct2FASetting.Disabled; }

      ret.BillingAddress = new Address(obj.BillingAddr);
      ret.Contact = new User(obj.Contact);
      ret.AccountRestrictions = obj.AccountRestrictions;
      Enum.TryParse(obj.FaxConsoleReferenceDropDown, out FaxConsoleReferenceDropDown faxRef);
      ret.FaxConsoleReferenceDropDown = faxRef;
      ret.AllowMultipleWebsiteSessions = obj.AllowMultipleWebsiteSessions;

      return ret;
    }

    public static Internal.AccountItem ToAccountItem(this AccountInfo obj)
    {
      if (obj == null) { return null; }

      var ret = new Internal.AccountItem();
      ret.AccountId = obj.AccountId;
      ret.ResellerId = obj.ResellerId;
      ret.AccountName = obj.AccountName;
      ret.AccountNumber = obj.AccountNumber;
      ret.AccountState = obj.AccountState;
      ret.Active = obj.Active;
      ret.TimeZone = obj.TimeZone.ToString();
      ret.MfaPolicy = obj.MfaPolicy.ToString();
      ret.BillingAddr = obj.BillingAddress.ToAddressItem();
      ret.Contact = obj.Contact.ToUserItem();
      ret.AccountRestrictions = obj.AccountRestrictions;
      ret.FaxConsoleReferenceDropDown = obj.FaxConsoleReferenceDropDown.ToString();
      ret.AllowMultipleWebsiteSessions = obj.AllowMultipleWebsiteSessions;
      return ret;
    }
  }

}
