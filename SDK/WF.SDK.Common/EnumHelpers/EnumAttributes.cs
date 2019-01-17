using System;
using System.Collections.Generic;
using System.Text;

namespace WF.SDK.Common
{

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class DisplayNameAttribute : Attribute
	{
		public string DisplayName = "";
		public string DisplayDesc = "";
		public string Tag = "";
    public Type LinkType = null;
    public int LinkVal = 0;

		public DisplayNameAttribute()
		{
		}

		public DisplayNameAttribute(string DisplayName)
		{
			this.DisplayName = DisplayName;
		}

		public DisplayNameAttribute(string DisplayName, string DisplayDesc)
		{
			this.DisplayName = DisplayName;
			this.DisplayDesc = DisplayDesc;
		}

		public DisplayNameAttribute(string DisplayName, string DisplayDesc, string Tag)
		{
			this.DisplayName = DisplayName;
			this.DisplayDesc = DisplayDesc;
			this.Tag = Tag;
		}

    public DisplayNameAttribute(string DisplayName, string DisplayDesc, string Tag, Type LinkType, int LinkVal)
    {
      this.DisplayName = DisplayName;
      this.DisplayDesc = DisplayDesc;
      this.Tag = Tag;
      this.LinkType = LinkType;
      this.LinkVal = LinkVal;
    }

	}

}
