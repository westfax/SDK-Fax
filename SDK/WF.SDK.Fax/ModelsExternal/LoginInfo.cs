using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models
{
  [Serializable]
  public class LoginInfo
  {
    public Guid Id;
    public Guid PrimaryAccountId;
    public string UserName;
    public string PassWord;
    public string DigitUserName;
    public string DigitPassWord;
    public User Contact;
    public List<LoginACL> AclList;

    public LoginInfo()
    { }

    public LoginInfo(Internal.LoginInfoItem item)
    {
      this.Id = item.Id;
      this.PrimaryAccountId = item.PrimaryAccountId;
      this.UserName = item.UserName;
      this.PassWord = item.PassWord;
      this.DigitUserName = item.DigitUserName;
      this.DigitPassWord = item.DigitPassWord;
      this.Contact = new User(item.contact);
      this.AclList = new List<LoginACL>();
      item.aclList.ForEach(i => this.AclList.Add(new LoginACL(i)));
    }


    public LoginInfo Clone()
    {
      return WF.SDK.Common.JSONSerializerHelper.Deserialize<LoginInfo>(WF.SDK.Common.JSONSerializerHelper.SerializeToString(this));
    }

  }


  public static class LoginInfoExtensions
  {
    public static Models.Internal.LoginInfoItem ToLoginInfoItem(this LoginInfo obj)
    {
      var ret = new Models.Internal.LoginInfoItem();

      ret.Id = obj.Id;
      ret.PrimaryAccountId = obj.PrimaryAccountId;
      ret.UserName = obj.UserName;
      ret.PassWord = obj.PassWord;
      ret.DigitPassWord = obj.DigitPassWord;
      ret.DigitUserName = obj.DigitUserName;

      ret.contact = obj.Contact.ToUserItem();
      ret.aclList = obj.AclList.Select(i => i.ToLoginACLItem()).ToList();

      return ret;
    }

    public static LoginInfo ToLoginInfo(this Models.Internal.LoginInfoItem obj)
    {
      if (obj == null) { return null; }
      return new LoginInfo(obj);
    }
  }

}
