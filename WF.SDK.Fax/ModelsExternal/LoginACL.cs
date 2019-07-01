using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models
{

  [Serializable]
  public class LoginACL
  {
    public Guid? LoginId;
    public Guid? SrcAclId;
    public bool Inherited;
    public ACLType ACLType;
    public UserRoleLevel UserRoleLevel;
    public string EntityName;
    public Guid EntityId;

    public LoginACL()
    { }

    public LoginACL(Internal.LoginACLItem item)
    {
      this.LoginId = item.LoginId;
      this.SrcAclId = item.SrcAclId;
      this.Inherited = item.Inherited;
      this.EntityName = item.EntityName;
      this.EntityId = item.EntityId;
      this.UserRoleLevel = (UserRoleLevel)Enum.Parse(typeof(UserRoleLevel), item.UserRoleLevel);
      this.ACLType = (ACLType)Enum.Parse(typeof(ACLType), item.ACLType);
    }
  }

  public static class LoginACLExtensions
  {
    public static Models.Internal.LoginACLItem ToLoginACLItem(this LoginACL obj)
    {
      var ret = new Models.Internal.LoginACLItem();

      ret.ACLType = obj.ACLType.ToString();
      ret.EntityId = obj.EntityId;
      ret.EntityName = obj.EntityName;
      ret.Inherited = obj.Inherited;
      ret.SrcAclId = obj.SrcAclId;
      ret.LoginId = obj.LoginId;
      ret.UserRoleLevel = obj.UserRoleLevel.ToString();

      return ret;
    }

    public static LoginACL ToLoginACL(this Models.Internal.LoginACLItem obj)
    {
      if (obj == null) { return null; }
      return new LoginACL(obj);
    }
  }
}
