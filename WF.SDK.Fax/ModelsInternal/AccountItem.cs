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
    public string AccountName;
    public string AccountNumber;
    public string AccountState;
    public bool Active;
    public string TimeZone;

    public AddressItem BillingAddr;
    public UserItem Contact;

    public AccountItem()
    { }
  }

}
