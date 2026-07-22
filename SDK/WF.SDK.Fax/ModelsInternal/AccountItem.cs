using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{
 
  [Serializable]
  public class AccountItem
  {
    public Guid AccountId;
    public Guid ResellerId;
    public string AccountName;
    public string AccountNumber;
    public string AccountState;
    public bool Active;
    public string TimeZone;
    public string MfaPolicy;
    public string FaxConsoleReferenceDropDown;
    public bool AllowMultipleWebsiteSessions;

    public WF.SDK.Models.AccountRestrictions AccountRestrictions;

    public AddressItem BillingAddr;
    public UserItem Contact;

    public AccountItem()
    { }

  }

 

}
