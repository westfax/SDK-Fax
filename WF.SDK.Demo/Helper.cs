using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using WF.SDK.Fax;
using WF.SDK.Models;

namespace WF.SDK.Fax.Demo
{
  public static class Helper
  {
    public static List<string> WriteFiles(IFaxId item)
    {
      var ret = new List<string>();
      foreach (var f in ((FaxDesc)item).FaxFileList)
      {
        var filename = Path.GetTempFileName();
        filename = filename + "." + f.FileFormat.ToString();
        File.WriteAllBytes(filename, f.FaxFiles[0].FileContents);
        ret.Add(filename);
      }
      return ret;
    }

    public static List<string> WriteFiles(Models.Internal.FaxFileItem item)
    {
      var ret = new List<string>();
      foreach (var f in item.FaxFiles)
      {
        var file = WriteFile(f, item.Format);
        ret.Add(file);
      }
      return ret;
    }

    public static List<string> WriteFiles(List<Models.Internal.FileItem> items, string format)
    {
      var ret = new List<string>();
      foreach (var f in items)
      {
        var file = WriteFile(f, format);
        ret.Add(file);
      }
      return ret;
    }

    public static List<string> WriteFiles(List<Models.FileDetail> items, string format)
    {
      var ret = new List<string>();
      foreach (var f in items)
      {
        var file = WriteFile(f, format);
        ret.Add(file);
      }
      return ret;
    }

    public static string WriteFile(Models.Internal.FileItem item, string format)
    {
      var filename = Path.GetTempFileName();
      filename = filename + "." + format;
      File.WriteAllBytes(filename, item.FileContents);
      
      return filename;
    }

    public static string WriteFile(Models.FileDetail item, string format)
    {
      var filename = Path.GetTempFileName();
      filename = filename + "." + format;
      File.WriteAllBytes(filename, item.FileContents);

      return filename;
    }

    public static void DisplayFiles(List<string> files)
    {
      foreach (string file in files)
      {
        DisplayFile(file);
      }
    }

    public static void DisplayFile(string file)
    {
        System.Diagnostics.Process.Start(file);
    }
  }
}
