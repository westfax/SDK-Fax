using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Management;

namespace WF.SDK.Common
{
  public class WfDriveInfo
  {
    public string LocalName { get; private set; }
    public string MappedUncPath { get; private set; }
    public long AvailableFreeSpace { get; private set; }

    public DriveType DriveType { get; private set; }
    public string DriveFormat { get; private set; }

    public bool IsLocal { get { return this.DriveType != DriveType.Network; } }
    public bool IsMapped { get { return !this.IsLocal; } }

    public WfDriveInfo(DriveInfo info)
    {
      this.LocalName = new WfPath(info.Name).FullPath;
      this.DriveType = info.DriveType;
      if (info.IsReady) { this.AvailableFreeSpace = info.AvailableFreeSpace; }
      else { this.AvailableFreeSpace = -1; }
      if (info.IsReady) { this.DriveFormat = info.DriveFormat; } else { this.DriveFormat = "The device is not ready."; }
      this.MappedUncPath = "Drive is local.";
    }

    /// <summary>
    /// Turns the given path into a localized path on this machine if it can be.
    /// </summary>
    public string LocalizePath(string fullpath)
    {
      return this.LocalizePath(new WfPath(fullpath));
    }

    /// <summary>
    /// Turns the given path into a localized path on this machine if it can be.
    /// </summary>
    public string LocalizePath(WfPath fullpath)
    {
      var temp = fullpath.FullPath;
      return temp.Replace(this.MappedUncPath, this.LocalName);
    }

    /// <summary>
    /// Turns the given path into the remotized path on this machine if it can be.
    /// </summary>
    public string RemotizePath(string fullpath)
    {
      return this.RemotizePath(new WfPath(fullpath));
    }

    /// <summary>
    /// Turns the given path into the remotized path on this machine if it can be.
    /// </summary>
    public string RemotizePath(WfPath fullpath)
    {
      var temp = fullpath.FullPath;
      return temp.Replace(this.LocalName, this.MappedUncPath);
    }

    #region Static
    /// <summary>
    /// Gets the WfDriveInfo which exists at the root of the path given.  If a null is returned, and the path really exists
    /// then this is a pure unc path with no local drive mapping.
    /// </summary>
    public static WfDriveInfo GetWfDriveInfo(string path)
    {
      return WfDriveInfo.FindMatchingDrive(WfDriveInfo.GetDrives(), path);
    }

    /// <summary>
    /// Returns a WfDriveInfo object for every drive on the local system. Includes local and mapped disks.
    /// </summary>
    public static List<WfDriveInfo> GetDrives()
    {
      List<WfDriveInfo> ret = new List<WfDriveInfo>();
      //Load up the drive information
      DriveInfo.GetDrives().ToList().ForEach(i => ret.Add(new WfDriveInfo(i)));
      //Get the 
      var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MappedLogicalDisk");
      var items = searcher.Get().OfType<ManagementObject>().ToList();
      //For network drives, get the path
      ret.ForEach(i =>
      {
        if (!i.IsLocal)
        {
          try
          {
            i.MappedUncPath = new WfPath(items.FirstOrDefault(j => j["Name"].ToString().ToLower() + "\\" == i.LocalName)["ProviderName"].ToString()).FullPath;
          }
          catch { }
        }
      });

      return ret;
    }

    /// <summary>
    /// Attempts to find the WfDriveInfo object that the path is rooted on.
    /// </summary>
    private static WfDriveInfo FindMatchingDrive(List<WfDriveInfo> list, string path)
    {
      WfDriveInfo ret = null;
      if (list == null) { return ret; }
      if (list.Count == 0) { return ret; }
      var temp = new WfPath(path).FullPath;

      ret = list.FirstOrDefault(i => temp.StartsWith(i.MappedUncPath.ToLower()) || temp.StartsWith(i.LocalName.ToLower()));
      return ret;
    }
    #endregion
  }
}
