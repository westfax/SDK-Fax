using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{
  [Serializable]
  public class LoginInfoItem
  {
    public Guid Id;
    public Guid PrimaryAccountId;
    public Guid DefaultProductId;
    public Guid DefaultCoverPageId;

    public string UserName;
    public string PassWord;
    public bool ImmutableUsername;

    public string DigitUserName;
    public string DigitPassWord;

    public string EMRUserId;
    public string EMRGroupId;
    public string ADSyncKey;

    public string IdentityName;
    public string NameId;
    public UserItem contact;
    public List<LoginACLItem> aclList;
    public string MfaPolicy;
    //public string LoginExtras;
    public UserSettings UserSettings;



    public LoginInfoItem()
    { }

  }
}
