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
    public string UserName;
    public string PassWord;
    public string DigitUserName;
    public string DigitPassWord;
    public UserItem contact;
    public List<LoginACLItem> aclList;

    public LoginInfoItem()
    { }

  }
}
