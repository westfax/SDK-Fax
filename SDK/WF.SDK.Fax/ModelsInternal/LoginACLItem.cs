using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{

  [Serializable]
  public class LoginACLItem
  {
    public Guid? LoginId;
    public Guid? SrcAclId;
    public bool Inherited;
    public string ACLType;
    public string UserRoleLevel;
    public string EntityName;
    public Guid EntityId;

    public LoginACLItem()
    { }
  }

}
