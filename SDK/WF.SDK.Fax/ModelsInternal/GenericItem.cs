using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models.Internal
{

  [Serializable]
  public class GenericItem
  {
    public string ItemTypeName { get; set; }

    public string String1Name { get; set; }
    public string String1Value { get; set; }

    public string String2Name { get; set; }
    public string String2Value { get; set; }

    public string String3Name { get; set; }
    public string String3Value { get; set; }

    public string String4Name { get; set; }
    public string String4Value { get; set; }

    public GenericItem()
    { }

    [Newtonsoft.Json.JsonIgnore]
    [System.Xml.Serialization.XmlIgnore]
    public string this[string name]
    {
      get { return this.GetValueByName(name); }
      set { this.SetValueByName(name, value); }
    }

    [Newtonsoft.Json.JsonIgnore]
    [System.Xml.Serialization.XmlIgnore]
    public string this[string name, string def]
    {
      get { return this.GetValueByName(name, false, def); }
      set { this.SetValueByName(name, value); }
    }

    private string GetValueByName(string name, bool caseInsensitive = true, string defIfNotFound = null)
    {
      if (this.Check(name, this.String1Name, caseInsensitive)) { return this.String1Value; }
      if (this.Check(name, this.String2Name, caseInsensitive)) { return this.String2Value; }
      if (this.Check(name, this.String3Name, caseInsensitive)) { return this.String3Value; }
      if (this.Check(name, this.String4Name, caseInsensitive)) { return this.String4Value; }

      return defIfNotFound;
    }

    private void SetValueByName(string name, string val, bool caseInsensitive = true)
    {
      if (this.Check(name, this.String1Name, caseInsensitive)) { this.String1Value = val; }
      if (this.Check(name, this.String2Name, caseInsensitive)) { this.String2Value = val; }
      if (this.Check(name, this.String3Name, caseInsensitive)) { this.String3Value = val; }
      if (this.Check(name, this.String4Name, caseInsensitive)) { this.String4Value = val; }
    }

    private bool Check(string str1, string str2, bool caseInsensitive = true)
    {
      try
      {
        if (caseInsensitive) { return str1.ToLower() == str2.ToLower(); }
        else { return str1 == str2; }
      }
      catch { return false; }
    }
  }


}
