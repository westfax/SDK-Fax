using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models
{
  [Serializable]
  public class UserSettings
  {
    public bool Mfa_DontAskToConfigAgain { get; set; }
    public DateTime Mfa_ConfigRemindAgainDate { get; set; }

    public UserSettings() { }
  }

  [Serializable]
  public class LoginInfo : IId
  {
    public Guid Id { get; set; }
    public Guid PrimaryAccountId { get; set; }
    public Guid DefaultProductId { get; set; }
    public Guid DefaultCoverPageId { get; set; }

    public string UserName { get; set; }
    public string PassWord { get; set; }
    public bool ImmutableUsername { get; set; }

    public string DigitUserName { get; set; }
    public string DigitPassWord { get; set; }

    public string EMRUserId { get; set; }
    public string EMRGroupId { get; set; }

    public string ADSyncKey { get; set; }
    public string IdentityName { get; set; }
    public string NameId { get; set; }
    public User Contact { get; set; }
    public MfaPolicyType MfaPolicy { get; set; }
    public List<LoginACL> AclList { get; set; }

    public UserSettings UserSettings { get; set; }

    public LoginInfo()
    { }

    public LoginInfo(Internal.LoginInfoItem item)
    {
      this.Id = item.Id;
      this.PrimaryAccountId = item.PrimaryAccountId;
      this.DefaultProductId = item.DefaultProductId;
      this.DefaultCoverPageId = item.DefaultCoverPageId;

      this.UserName = item.UserName;
      this.PassWord = item.PassWord;
      this.ImmutableUsername = item.ImmutableUsername;

      this.DigitUserName = item.DigitUserName;
      this.DigitPassWord = item.DigitPassWord;

      this.EMRUserId = item.EMRUserId;
      this.EMRGroupId = item.EMRGroupId;
      this.ADSyncKey = item.ADSyncKey;

      this.IdentityName = item.IdentityName;
      this.NameId = item.NameId;
      this.Contact = new User(item.contact);
      this.AclList = new List<LoginACL>();
      item.aclList.ForEach(i => this.AclList.Add(new LoginACL(i)));
      this.UserSettings = item.UserSettings;
      try { this.MfaPolicy = (MfaPolicyType)Enum.Parse(typeof(MfaPolicyType), item.MfaPolicy); }
      catch { this.MfaPolicy = MfaPolicyType.None; }
    }


    public LoginInfo Clone()
    {
      return WF.SDK.Common.JSONSerializerHelper.Deserialize<LoginInfo>(WF.SDK.Common.JSONSerializerHelper.SerializeToString(this));
    }

    public override string ToString()
    {
      return this.UserName;
    }
  }


  public static class LoginInfoExtensions
  {
    public static Models.Internal.LoginInfoItem ToLoginInfoItem(this LoginInfo obj)
    {
      var ret = new Models.Internal.LoginInfoItem();

      ret.Id = obj.Id;
      ret.PrimaryAccountId = obj.PrimaryAccountId;
      ret.DefaultProductId = obj.DefaultProductId;
      ret.DefaultCoverPageId = obj.DefaultCoverPageId;

      ret.UserName = obj.UserName;
      ret.PassWord = obj.PassWord;
      ret.ImmutableUsername = obj.ImmutableUsername;

      ret.DigitPassWord = obj.DigitPassWord;
      ret.DigitUserName = obj.DigitUserName;

      ret.EMRUserId = obj.EMRUserId;
      ret.EMRGroupId = obj.EMRGroupId;
      ret.ADSyncKey = obj.ADSyncKey;

      ret.IdentityName = obj.IdentityName;
      ret.NameId = obj.NameId;
      ret.MfaPolicy = obj.MfaPolicy.ToString();
      ret.contact = obj.Contact.ToUserItem();
      ret.aclList = obj.AclList?.Select(i => i.ToLoginACLItem()).ToList();
      ret.UserSettings = obj.UserSettings;
      return ret;
    }

    public static LoginInfo ToLoginInfo(this Models.Internal.LoginInfoItem obj)
    {
      if (obj == null) { return null; }
      return new LoginInfo(obj);
    }
  }
}
