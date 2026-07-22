using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{

  [Serializable]
  public class NameValueItem
  {
    public string Name;
    public string Value;

    public NameValueItem() { }

		public NameValueItem(string name, string val) { this.Name = name; this.Value = val; }
  }

  public static class NameValueItemExtensions
  {
    public static string ValueOrDefault(this NameValueItem obj, string def = "")
    {
      if (obj == null)
      { return def; }
      return obj.Value;
    }
  }

}
