using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Management;

namespace WF.SDK.Common
{
  public class FileActionResultInfo
  {
    public FileActionResultInfo()
    { }

    internal FileActionResultInfo(
      int CountFoldersSearched,
      int CountFoldersDeleted,
      int CountFilesSearched,
      int CountFilesDeleted,
      int CountFoldersRemaining,
      int CountFilesRemaining,
      List<Tuple<string, int>> a,
      List<Tuple<string, int>> b
      )
    {
      this.CountFoldersSearched = CountFoldersSearched;
      this.CountFoldersDeleted = CountFoldersDeleted;

      this.CountFilesSearched = CountFilesSearched;
      this.CountFilesDeleted = CountFilesDeleted;

      this.CountFoldersRemaining = CountFoldersRemaining;
      this.CountFilesRemaining = CountFilesRemaining;

      this.FolderFileCounts.AddRange(a);
      this.FolderFileCounts.AddRange(b);
    }

    public int CountFoldersSearched = 0;
    public int CountFoldersDeleted = 0;
    public int CountFilesSearched = 0;
    public int CountFilesDeleted = 0;

    public int CountFoldersRemaining = 0;
    public int CountFilesRemaining = 0;

    public List<Tuple<string, int>> FolderFileCounts = new List<Tuple<string, int>>();

    public static FileActionResultInfo operator +(FileActionResultInfo a, FileActionResultInfo b)
    {
      if (a == null && b == null) { return new FileActionResultInfo(); }
      if (a == null) { return b; }
      if (b == null) { return a; }
      return new FileActionResultInfo(
        a.CountFoldersSearched + b.CountFoldersSearched,
        a.CountFoldersDeleted + b.CountFoldersDeleted,
        a.CountFilesSearched + b.CountFilesSearched,
        a.CountFilesDeleted + b.CountFilesDeleted,
        a.CountFoldersRemaining + b.CountFoldersRemaining,
        a.CountFilesRemaining + b.CountFilesRemaining,
        a.FolderFileCounts, b.FolderFileCounts);
    }

  }

  /// <summary>
  /// This class provides a set of static methods to assist with deleting old files and other file activity.
  /// </summary>
  public static class FileHelper
  {

    public static bool IsLocal(string filePath)
    {
      return !FileHelper.IsRemote(filePath);
    }

    public static bool IsRemote(string filePath)
    {
      return filePath.StartsWith(@"\\");
    }

    public static string FormFileSystemSafeString(DateTime dtm)
    {
      return dtm.ToString("yyyy-MM-dd_HHmm");
    }

    public static string FormFileSystemSafeString(DateTime dtm, bool includeTime)
    {
      if (includeTime) { return FileHelper.FormFileSystemSafeString(dtm); }
      return dtm.ToString("yyyy-MM-dd");
    }

    public static string FormFileSystemSafeString(DateTime dtm1, DateTime dtm2)
    {
      return dtm1.ToString("yyyy-MM-dd_HHmm") + "_to_" + dtm2.ToString("yyyy-MM-dd_HHmm");
    }

    public static string FormFileSystemSafeString(DateTime dtm1, DateTime dtm2, bool includeTime)
    {
      if (includeTime) { return FileHelper.FormFileSystemSafeString(dtm1, dtm2); }
      return dtm1.ToString("yyyy-MM-dd") + "_to_" + dtm2.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// Copies the source file to the destination and updates the file modified date
    /// time to the current system date time.  The destination directory will 
    /// be created if it doesn't exist.  If the source file does not exist, returns "".
    /// </summary>
    /// <param name="source">The full path to the source File</param>
    /// <param name="dest">The full path to the source File</param>
    public static string CopyAndFixDateTime(string source, string dest)
    {
      if (!File.Exists(source)) { return ""; }
      if (!Directory.Exists(Path.GetDirectoryName(dest))) { Directory.CreateDirectory(Path.GetDirectoryName(dest)); }
      File.Copy(source, dest, true);
      File.SetLastWriteTime(dest, DateTime.Now);
      return dest;
    }

    /// <summary>
    /// Copies the source file to the destination and updates the file modified date
    /// time to the supplied DateTime.  The destination directory will 
    /// be created if it doesn't exist.  If the source file does not exist, returns "".
    /// </summary>
    /// <param name="source">The full path to the source File</param>
    /// <param name="dest">The full path to the source File</param>
    /// <param name="dtm">The destination file date time.</param>
    public static string CopyAndFixDateTime(string source, string dest, DateTime dtm)
    {
      if (!File.Exists(source)) { return ""; }
      if (!Directory.Exists(Path.GetDirectoryName(dest))) { Directory.CreateDirectory(Path.GetDirectoryName(dest)); }
      File.Copy(source, dest, true);
      File.SetLastWriteTime(dest, dtm);
      return dest;
    }

    /// <summary>
    /// Deletes files and folders in accordance with the FileActionInfo.  FileActionInfo
    /// is polulated with the result of the operation.
    /// </summary>
    public static FileActionResultInfo GetTreeInfo(string path, bool recursive, bool populateList)
    {
      FileActionResultInfo ret = new FileActionResultInfo();
      FileActionResultInfo child = new FileActionResultInfo();

      string[] dirs = new string[] { };
      string[] files = new string[] { };

      if (!Directory.Exists(path)) { return ret; }

      //We're searching this folder, so increment.
      ret.CountFoldersSearched = 1;
      ret.CountFoldersDeleted = 0;
      ret.CountFoldersRemaining = 0;

      if (recursive)
      {
        dirs = Directory.GetDirectories(path);
        foreach (string dir in dirs)
        {
          FileActionResultInfo childInfo = FileHelper.GetTreeInfo(dir, recursive, populateList);
          child += childInfo;
        }
      }

      //Files to be visited
      files = Directory.GetFiles(path);
      //We're searching these files, set the searched count
      ret.CountFilesSearched = files.Length;
      ret.CountFilesDeleted = 0;
      ret.CountFilesRemaining = 0;

      if (populateList)
      {
        if (ret.CountFilesSearched != 0)
        {
          ret.FolderFileCounts.Add(new Tuple<string, int>(path, ret.CountFilesSearched));
        }
      }

      return ret + child;
    }

    /// <summary>
    /// Deletes files and folders in accordance with the FileActionInfo.  FileActionInfo
    /// is populated with the result of the operation.
    /// </summary>
    public static FileActionResultInfo DeleteOldFiles(string path, DateTime olderThan, bool recursive, bool populateList)
    {
      FileActionResultInfo ret = new FileActionResultInfo();
      FileActionResultInfo child = new FileActionResultInfo();

      string[] dirs = new string[] { };
      string[] files = new string[] { };

      if (!Directory.Exists(path)) { return ret; }

      //We're searching this folder, so increment.
      ret.CountFoldersSearched = 1;

      if (recursive)
      {
        dirs = Directory.GetDirectories(path);
        foreach (string dir in dirs)
        {
          FileActionResultInfo childInfo = FileHelper.DeleteOldFiles(dir, olderThan, recursive, populateList);
          if (childInfo.CountFilesRemaining == 0 && childInfo.CountFoldersRemaining == 0
            && Directory.GetFiles(dir).Length == 0 && Directory.GetDirectories(dir).Length == 0)
          {
            Directory.Delete(dir);
            //Directory Deleted.  Increment.
            ret.CountFoldersDeleted++;
          }
          else
          {
            //Directory Remains.  Increment.
            ret.CountFoldersRemaining++;
          }
          child += childInfo;
        }
      }
      else
      {
        ret.CountFoldersRemaining = Directory.GetDirectories(path).Length;
        ret.CountFoldersDeleted = 0;
      }

      //Files to be visited
      files = Directory.GetFiles(path);
      //We're searching these files, set the searched count
      ret.CountFilesSearched = files.Length;
      ret.CountFilesDeleted = 0;
      ret.CountFilesRemaining = 0;

      //Walk the files
      foreach (string file in files)
      {
        if (File.GetLastWriteTime(file) < olderThan)
        {
          File.Delete(file);
          //File Deleted.  Increment.
          ret.CountFilesDeleted++;
        }
        else
        {
          //File Remains.  Increment.
          ret.CountFilesRemaining++;
        }
      }

      if (populateList)
      {
        if (ret.CountFilesRemaining != 0)
        {
          ret.FolderFileCounts.Add(new Tuple<string, int>(path, ret.CountFilesRemaining));
        }
      }

      return ret + child;
    }

    /// <summary>
    /// Deletes files (and folders) whose modified DateTime is older than the number of hours indicated.
    /// Not recursive.
    /// </summary>
    /// <param name="path">The fully-rooted path to search for files.</param>
    /// <param name="ageHours">The age of the files in hours.</param>
    /// <returns>The number of deleted items. (Files + Directories)</returns>
    public static int DeleteOldFiles(string path, int ageHours)
    {
      return FileHelper.DeleteOldFiles(path, ageHours, false);
    }

    /// <summary>
    /// Deletes files (and folders) whose modified DateTime is older than the number of hours indicated.
    /// </summary>
    /// <param name="path">The fully-rooted path to search for files.</param>
    /// <param name="ageHours">The age of the files in hours.</param>
    /// <param name="recursive">Whether sub directories are searched.  If true, then 
    /// sub-directories that are empty after the file delete will be deleted also.</param>
    /// <returns>The number of deleted items. (Files + Directories)</returns>
    public static int DeleteOldFiles(string path, int ageHours, bool recursive)
    {
      int ret = 0;
      string[] dirs = new string[] { };
      string[] files = new string[] { };

      if (!Directory.Exists(path)) { return ret; }

      if (recursive)
      {
        dirs = Directory.GetDirectories(path);
        foreach (string dir in dirs)
        {
          ret += FileHelper.DeleteOldFiles(dir, ageHours, recursive);
        }

      }

      files = Directory.GetFiles(path);

      foreach (string file in files)
      {
        if (File.GetLastWriteTime(file) < DateTime.Now.AddHours(-ageHours))
        {
          File.Delete(file);
          ret++;
        }
      }

      if (recursive)
      {
        foreach (string dir in dirs)
        {
          try
          {
            if (Directory.GetFiles(dir).Length == 0 && Directory.GetDirectories(dir).Length == 0)
            {
              Directory.Delete(dir);
              ret++;
            }
          }
          catch (DirectoryNotFoundException) { } //Ignore DirectoryNotFoundExceptions (occurs when DeleteOldFiles is called multiple times simultaneously)
        }
      }

      return ret;
    }

    public static FileActionResultInfo DeleteOldFiles_Fast(string path, DateTime olderThan, bool pruneemptyFolders = true, bool recursive = true, int fileDeleteLimit = 0)
    {
      FileSystemEnumerator e = null;
      if (recursive) { e = new FileSystemEnumerator(path, EnumertorType.Recursive_FilesAndFolders); }
      else { e = new FileSystemEnumerator(path, EnumertorType.FilesAndFolders); }

      var ret = new FileActionResultInfo();
      foreach (FileSystemInfo info in e)
      {
        if (info is DirectoryInfo)
        {
          ret.CountFoldersSearched++;
          if (pruneemptyFolders && FileHelper.IsDirectoryEmpty_Fast(info.FullName)) { info.Delete(); ret.CountFoldersDeleted++; }
          else { ret.CountFoldersRemaining++; }
        }
        if (info is FileInfo)
        {
          ret.CountFilesSearched++;
          if (info.LastWriteTime < olderThan) { info.Delete(); ret.CountFilesDeleted++; }
          else { ret.CountFilesRemaining++; }
        }
        if (fileDeleteLimit != 0 && ret.CountFilesDeleted >= fileDeleteLimit) { break; }
      }
      return ret;
    }

    /// <summary>
    /// Searches a root path for folders matching the search criteria.  Wildcards are accepted.
    /// </summary>
    /// <returns></returns>
    public static List<string> FindFolders(string rootPath, string search)
    {
      if (!Directory.Exists(rootPath)) { return new List<string>(); }
      return Directory.GetDirectories(rootPath, search).ToList();
    }

    /// <summary>
    /// Localizes a folder path if it can be.  Takes any path (to a folder or a file).  
    /// Generally this is a unc path, but not required. 
    /// Uses the locally mapped drives tries to find the equivalent local path.
    /// </summary>
    /// <returns>
    /// A string with the resulting localized path.  If the path cannot be 
    /// localized, then the original uncFullPath is returned.
    /// </returns>
    public static string LocalizePath(string fullPath)
    {
      return new WfPath(fullPath).LocalizedPath;
    }

    /// <summary>
    /// Remotizes a folder path if it can be.  Takes any path (to a folder or a file).  
    /// Generally this is a local path, but not required. 
    /// Uses the locally mapped drives tries to find the equivalent remote path.
    /// </summary>
    /// <returns>
    /// A string with the resulting remotized path.  If the path cannot be 
    /// remotized, then the original local path is returned.
    /// </returns>
    public static string RemotizePath(string fullPath)
    {
      return new WfPath(fullPath).RemotizedPath;
    }

    /// <summary>
    /// Takes a folder Path, and turns it into a UNC path.  It will always
    /// assume that you have the default admin shares turned on.  
    /// c:\Somepath = > \\machine\\c$\SomePath
    /// Throws an exception if the path is not rooted.
    /// </summary>
    public static string ToUncPath(string machineRelativeLocalPath, string machineName)
    {
      //Path is not rooted.  We cant do anything.
      if (!Path.IsPathRooted(machineRelativeLocalPath)) { throw new Exception("Path must be rooted."); }
      //This path is already a UNC path return it
      if (machineRelativeLocalPath.StartsWith("\\\\")) { return machineRelativeLocalPath; }

      string template = "\\\\{0}\\{1}$\\";
      string path = machineRelativeLocalPath;
      string drive = machineRelativeLocalPath.Substring(0, 1);
      string ret = String.Format(template, machineName, drive);

      ret = Path.Combine(ret, machineRelativeLocalPath.Substring(3));

      return ret;
    }

    /// <summary>
    /// Look for file/s.
    /// </summary>
    /// <param name="path">The base path to look.</param>
    /// <param name="toFind">The string to look for, wildcards accepted.</param>
    /// <returns>String array of all files matching toFind.</returns>
    public static string[] FindFiles(string path, string toFind)
    {
      if (!Directory.Exists(Path.GetDirectoryName(path))) { return new string[0]; }

      Stack folders = new Stack();
      folders.Push(path);
      ArrayList al = new ArrayList();
      al = FindFiles(toFind, ref folders, ref al);

      return (string[])al.ToArray(typeof(string));
    }

    /// <summary>
    /// Recursive loop to find files.
    /// </summary>
    /// <param name="toFind"></param>
    /// <param name="folders"></param>
    /// <param name="foundFiles"></param>
    /// <returns></returns>
    private static ArrayList FindFiles(string toFind, ref Stack folders, ref ArrayList foundFiles)
    {
      if (folders.Count > 0)
      {
        string currentDir = folders.Pop().ToString();
        string[] dirs = Directory.GetDirectories(currentDir);

        foreach (string s in dirs) { folders.Push(s); }

        string[] files = Directory.GetFiles(currentDir, toFind);

        if (files.Length > 0) { foreach (string s in files) { foundFiles.Add(s); } }

        FindFiles(toFind, ref folders, ref foundFiles);
      }

      return foundFiles;
    }

    public static string GetLocalWorkingFolder(string folderName, string preferDrive = "c")
    {
      string ret = @"";

      try
      {
        ret = Path.Combine(@"E:\", folderName);
        //Make sure the drive exists
        if (Directory.Exists(ret)) { return ret; }
      }
      catch { ret = @""; }

      try
      {
        ret = Path.Combine(@"D:\", folderName);
        //Make sure the drive exists
        if (Directory.Exists(ret)) { return ret; }
      }
      catch { ret = @""; }

      try
      {
        ret = Path.Combine(@"C:\", folderName);
        //Make sure the drive exists
        if (Directory.Exists(ret)) { return ret; }
      }
      catch { ret = @""; }

      //Try Creating using prefered drive
      try
      {
        var drive = string.Format("{0}:\\", preferDrive);

        if (WfDriveInfo.GetWfDriveInfo(drive).DriveType == DriveType.Fixed)
        {
          ret = Path.Combine(drive, folderName);
          if (Directory.Exists(ret)) { return ret; }
          else
          {
            Directory.CreateDirectory(ret);
            if (Directory.Exists(ret)) { return ret; }
          }
        }
      }
      catch { ret = @""; }

      //Ok, back to the basics....
      preferDrive = "c";
      try
      {
        var drive = string.Format("{0}:\\", preferDrive);

        if (WfDriveInfo.GetWfDriveInfo(drive).DriveType == DriveType.Fixed)
        {
          ret = Path.Combine(drive, folderName);
          if (Directory.Exists(ret)) { return ret; }
          else
          {
            Directory.CreateDirectory(ret);
            if (Directory.Exists(ret)) { return ret; }
          }
        }
      }
      catch { ret = @""; }

      //Just try soemthing!!!
      try
      {
        ret = folderName;
        ret = Path.GetFullPath(ret);
        if (!Directory.Exists(ret)) { Directory.CreateDirectory(ret); }
      }
      catch { ret = @""; }

      return "";
      //try
      //{
      //  ret = @"D:\" + folderName;
      //  if (Directory.Exists(ret))  //Make sure the drive exists
      //  {
      //    ret = @"D:\" + folderName;
      //    if (!Directory.Exists(ret)) { Directory.CreateDirectory(ret); }
      //  }
      //  else { ret = ""; }
      //}
      //catch { ret = ""; }

      //if (ret != @"") { return ret; }

      //try
      //{
      //  ret = @"C:\" + folderName;
      //  if (!Directory.Exists(ret)) { Directory.CreateDirectory(ret); }
      //}
      //catch { ret = @""; }

      //if (ret != @"") { return ret; }

      //try
      //{
      //  ret = folderName;
      //  ret = Path.GetFullPath(ret);
      //  if (!Directory.Exists(ret)) { Directory.CreateDirectory(ret); }
      //}
      //catch { ret = @""; }

      //return ret;
    }

    public static string GetLocalTempFullFilePath(string fileExtension)
    {
      string local = Path.GetTempPath();
      if (!Directory.Exists(local)) { Directory.CreateDirectory(local); }
      return Path.Combine(local, Guid.NewGuid().ToString() + fileExtension);
    }

    public static string GetLocalTempFolder()
    {
      string local = Path.GetTempPath();
      if (!Directory.Exists(local)) { Directory.CreateDirectory(local); }
      return local;
    }

    public static int PurgeLocalTempFolder()
    {
      return FileHelper.PurgeLocalTempFolder(0);
    }

    public static int PurgeLocalTempFolder(int ageHours)
    {
      string local = Path.GetTempPath();
      return FileHelper.DeleteOldFiles(local, ageHours);
    }


    #region WinAPI Fast Detection File Methods

    /// <summary>
    /// Checks if a folder is Empty.  It will throw an exception if the folder does not exist.
    /// </summary>
    public static bool IsDirectoryEmpty_Fast(string path)
    {
      //Bad path throw exception
      if (string.IsNullOrEmpty(path)) { throw new ArgumentNullException(path); }

      //Get out if the folder is missing
      if (!Directory.Exists(path)) { throw new DirectoryNotFoundException(); }

      //Fix up the path for searching
      if (path.EndsWith(Path.DirectorySeparatorChar.ToString())) { path += "*"; }
      else { path += Path.DirectorySeparatorChar + "*"; }

      //Data Instance.  This gets reused.
      WIN32_FIND_DATA findData;
      IntPtr hFind = FileHelperWinAPI.INVALID_HANDLE_VALUE;
      try
      {
        //Find the first
        hFind = FileHelperWinAPI.FindFirstFile(path, out findData);
        if (hFind == FileHelperWinAPI.INVALID_HANDLE_VALUE) { throw new Exception("Failed to get directory first file", Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error())); }

        bool empty = true;
        do
        {
          //Reject the . and .. directory entries.
          if (findData.cFileName != "." && findData.cFileName != "..") { empty = false; }
        }
        while (empty && FileHelperWinAPI.FindNextFile(hFind, out findData));

        return empty;
      }
      catch (Exception e) { throw e; }
      finally { if (hFind != FileHelperWinAPI.INVALID_HANDLE_VALUE) { FileHelperWinAPI.FindClose(hFind); } }
    }

    public static int CountFolders_Fast(string path)
    {
      return FileHelper.CountItems_Fast(path, FileAttributes.FILE_ATTRIBUTE_DIRECTORY, true);
    }

    public static int CountFiles_Fast(string path)
    {
      return FileHelper.CountItems_Fast(path, FileAttributes.FILE_ATTRIBUTE_DIRECTORY, false);
    }

    private static int CountItems_Fast(string path, uint filter, bool set)
    {
      int ret = 0;

      //Bad path throw exception
      if (string.IsNullOrEmpty(path)) { throw new ArgumentNullException(path); }

      //Get out if the folder is missing
      if (!Directory.Exists(path)) { throw new DirectoryNotFoundException(); }

      //Fix up the path for searching
      if (path.EndsWith(Path.DirectorySeparatorChar.ToString())) { path += "*"; }
      else { path += Path.DirectorySeparatorChar + "*"; }

      //Data Instance.  This gets reused.
      WIN32_FIND_DATA findData;
      IntPtr hFind = FileHelperWinAPI.INVALID_HANDLE_VALUE;

      try
      {
        //Find the first
        hFind = FileHelperWinAPI.FindFirstFile(path, out findData);
        if (hFind == FileHelperWinAPI.INVALID_HANDLE_VALUE) { throw new Exception("Failed to get first file in directory", Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error())); }

        do
        {
          //Special directories
          if (findData.cFileName == "." || findData.cFileName == "..") { continue; }
          //Filter
          if (((findData.dwFileAttributes & filter) != 0) == set) { ret++; }
        }
        while (FileHelperWinAPI.FindNextFile(hFind, out findData));
      }
      catch (Exception e) { throw e; }
      finally { if (hFind != FileHelperWinAPI.INVALID_HANDLE_VALUE) { FileHelperWinAPI.FindClose(hFind); } }

      return ret;
    }

    #endregion





  }
}
