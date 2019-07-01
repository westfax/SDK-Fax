using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Management;

namespace WF.SDK.Common
{
  public class WfPath
  {
    private string originalPath = "";
    private string normalizedPath = "";
    private WfDriveInfo driveInfo = null;

    public string FullPath { get { return this.normalizedPath; } }
    public bool Exists { get; private set; }
    public bool IsFile { get; private set; }
    public bool IsFolder { get; private set; }
    public string Extension { get; private set; }

    #region Instance
    //No Default Constructor
    private WfPath() { }

    public WfPath(string path)
    {
      this.DiscoverPath(path);
    }

    private void DiscoverPath(string path)
    {
      this.originalPath = path;

      this.normalizedPath = path.ToLower();

      if (File.Exists(this.normalizedPath)) { this.IsFile = true; this.IsFolder = false; this.Exists = true; }
      else if (Directory.Exists(this.normalizedPath)) { this.IsFile = false; this.IsFolder = true; this.Exists = true; }
      else
      {
        this.Exists = false;
        this.IsFile = false;
        this.IsFolder = false;
      }
      //Make sure the folder ends with a separator character
      if (this.IsFolder && !this.normalizedPath.EndsWith("\\")) { this.normalizedPath += "\\"; }
      //Extension
      if (this.IsFile) { this.Extension = Path.GetExtension(this.normalizedPath); } else { this.Extension = null; }
    }

    /// <summary>
    /// If this path is a remote path, it will yeild the local version via the mapped drive.
    /// </summary>
    public string LocalizedPath
    {
      get { try { return this.WfDriveInfo.LocalizePath(this.normalizedPath); } catch { return this.normalizedPath; } }
    }

    /// <summary>
    /// If this path is a local path, but a mapped drive, will return the remote version of the path.
    /// </summary>
    public string RemotizedPath
    {
      get { if (this.IsLocal) { return this.normalizedPath; } else { try { return this.WfDriveInfo.RemotizePath(this.normalizedPath); } catch { return this.normalizedPath; } } }
    }

    /// <summary>
    /// Whether the file is on a remote malchine.  This will work even if the pat is a mapped network drive.
    /// </summary>
    public bool IsRemote
    {
      get { if (this.WfDriveInfo == null || this.WfDriveInfo.DriveType == DriveType.Network) { return true; } else return false; }
    }

    /// <summary>
    /// Whether the file is physically on this machine.
    /// </summary>
    public bool IsLocal
    {
      get { return !this.IsRemote; }
    }

    /// <summary>
    /// Returns the WfDriveInfo object that is at the root of the path for this instance.  A null means that the 
    /// path is not rooted on a local drive (mapped or local disk).  The path may still be reachable if this is null.
    /// </summary>
    public WfDriveInfo WfDriveInfo
    {
      get { if (this.driveInfo == null) { this.driveInfo = WfDriveInfo.GetWfDriveInfo(this.normalizedPath); } return this.driveInfo; }
    }

    #endregion
    #region Static






    #endregion
  }
}
