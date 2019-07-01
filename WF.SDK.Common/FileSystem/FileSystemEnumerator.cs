using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace WF.SDK.Common
{
  public enum EnumertorType
  {
    FilesOnly,
    FoldersOnly,
    FilesAndFolders,
    Recursive_FilesOnly,
    Recursive_FoldersOnly,
    Recursive_FilesAndFolders,
  }

  /// <summary>
  /// A fast file system enumerator class that allows for recursive enumeration.
  /// When files are returned by the enumerator, they are returned in order.  When folders are enumerated
  /// with recursion, and a folder is encoundered, the contents of the folder are returned prior to the folder itself.  Thus,
  /// deletion of files and folders along the way will be safe.
  /// </summary>
  public class FileSystemEnumerator : IEnumerable<FileSystemInfo>
  {
    private string _rootFolder = "";
    private EnumertorType _type = EnumertorType.FilesOnly;

    public FileSystemEnumerator(string rootFolder, EnumertorType type = EnumertorType.FilesOnly)
    {
      this._type = type;
      this._rootFolder = rootFolder;
    }

    public IEnumerator<FileSystemInfo> GetEnumerator()
    {
      return new FSEnumerator(this._rootFolder, this._type);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return new FSEnumerator(this._rootFolder, this._type);
    }
  }

  /// <summary>
  /// An enumerator that provides fast iteration over a directory tree.  The root folder is not returned as part of the enumerated values.
  /// When recursive, the enumerator will descend into a folder when it finds one, and enumerate the files and folders there and so on.
  /// </summary>
  public class FSEnumerator : IEnumerator<FileSystemInfo>
  {
    private EnumertorType _type = EnumertorType.FilesOnly;

    private Stack<FolderEnumerator> _enumerators = new Stack<FolderEnumerator>();
    private FolderEnumerator _currentEnumerator = null;

    internal FSEnumerator(string rootFolder, EnumertorType type = EnumertorType.FilesOnly)
    {
      this._type = type;
      this._currentEnumerator = new FolderEnumerator(rootFolder);
    }

    public FileSystemInfo Current { get { return this._currentEnumerator.Current; } }

    public void Dispose()
    {
      if (this._currentEnumerator != null)
      {
        this._currentEnumerator.Dispose();
        this._currentEnumerator = null;
      }
      while (this._enumerators.Count > 0)
      {
        FolderEnumerator e = this._enumerators.Pop();
        e.Dispose();
      }
    }

    object System.Collections.IEnumerator.Current
    {
      get { return this.Current; }
    }

    public bool MoveNext()
    {
      switch (this._type)
      {
        case EnumertorType.FilesOnly: { return this.MoveNext_NonRecursive_FilesOnly(); break; }
        case EnumertorType.FoldersOnly: { return this.MoveNext_NonRecursive_FoldersOnly(); break; }
        case EnumertorType.FilesAndFolders: { return this.MoveNext_NonRecursive_FilesAndFolders(); break; }
        case EnumertorType.Recursive_FilesOnly: { return this.MoveNext_Recursive_FilesOnly(); break; }
        case EnumertorType.Recursive_FoldersOnly: { return this.MoveNext_Recursive_FoldersOnly(); break; }
        case EnumertorType.Recursive_FilesAndFolders: { return this.MoveNext_Recursive_FilesAndFolders(); break; }
        default: { return false; break; }
      }
    }

    private bool MoveNext_NonRecursive_FilesOnly()
    {
      bool result = false;
      result = this._currentEnumerator.MoveNext();
      //Keep going till we have something other than a directory or no more file system entries
      while (result && this.Current is DirectoryInfo) { result = this._currentEnumerator.MoveNext(); }
      return result;
    }

    private bool MoveNext_NonRecursive_FoldersOnly()
    {
      bool result = false;
      result = this._currentEnumerator.MoveNext();
      //Keep going till we have something other than a file or no more file system entries
      while (result && this.Current is FileInfo) { result = this._currentEnumerator.MoveNext(); }
      return result;
    }

    private bool MoveNext_NonRecursive_FilesAndFolders()
    {
      bool result = this._currentEnumerator.MoveNext();
      return result;
    }

    private bool MoveNext_Recursive_FilesOnly()
    {
      bool result = false;
      result = this._currentEnumerator.MoveNext();
      //If success and we have a file, just return it
      if (result && this.Current is FileInfo) { return result; }
      //If success and we have a folder - descend into it
      if (result && this.Current is DirectoryInfo)
      {
        //Push the enumerator on the stack
        this._enumerators.Push(this._currentEnumerator);
        //Create a new enumerator and set it as the current one.
        this._currentEnumerator = new FolderEnumerator(this.Current.FullName);
        //Call back into this method
        return this.MoveNext_Recursive_FilesOnly();
      }
      //If not success, then we're at the end of the current folder
      //If there are no more enumerators then we're done.
      if (!result && this._enumerators.Count == 0) { this._currentEnumerator.Dispose(); return result; }
      //If there are enumerators then we need to pop one off the stack
      //Keep going till we have something other than a directory or no more file system entries
      while (!result && this._enumerators.Count > 0)
      {
        //Clean up the current before letting go of it.
        this._currentEnumerator.Dispose();
        //Get one off the stack
        this._currentEnumerator = this._enumerators.Pop();
        //Call back into this method
        return this.MoveNext_Recursive_FilesOnly();
      }
      return result;
    }

    private bool MoveNext_Recursive_FoldersOnly()
    {
      bool result = false;
      //Run till we get another folder.
      while ((result = this._currentEnumerator.MoveNext()) && this.Current is FileInfo) { }
      //If success, we're guaranteed to have a folder, descend into the folder without returning it at this point.
      if (result)
      {
        //Push it on to the stack
        this._enumerators.Push(this._currentEnumerator);
        //Set the curretn enumerator o this folder
        this._currentEnumerator = new FolderEnumerator(this.Current.FullName);
        //Call back into this method
        return this.MoveNext_Recursive_FoldersOnly();
      }
      //Not success and there are none left on the stack, then just return false.  we're done
      if (!result && this._enumerators.Count == 0) { this._currentEnumerator.Dispose(); return false; }
      //Not success, pop the enumerator off the stack if there is one
      if (!result && this._enumerators.Count > 0)
      {
        //Clean up the current before letting go of it.
        this._currentEnumerator.Dispose();
        //Get one off the stack
        this._currentEnumerator = this._enumerators.Pop();
        //This one should have a folder on it waiting for us, return it.
        if (this.Current is DirectoryInfo) { return true; }
      }
      return result;
    }

    private bool MoveNext_Recursive_FilesAndFolders()
    {
      bool result = false;
      result = this._currentEnumerator.MoveNext();
      //If success and we have a file, just return it
      if (result && this.Current is FileInfo) { return result; }
      //If success and we have a folder - descend into it
      if (result && this.Current is DirectoryInfo)
      {
        //Push the enumerator on the stack
        this._enumerators.Push(this._currentEnumerator);
        //Create a new enumerator and set it as the current one.
        this._currentEnumerator = new FolderEnumerator(this.Current.FullName);
        //Call back into this method
        return this.MoveNext_Recursive_FilesAndFolders();
      }
      //If not success, then we're at the end of the current folder
      //If there are no more enumerators then we're done.
      if (!result && this._enumerators.Count == 0) { this._currentEnumerator.Dispose(); return result; }
      //Not success, pop the enumerator off the stack if there is one
      if (!result && this._enumerators.Count > 0)
      {
        //Clean up the current before letting go of it.
        this._currentEnumerator.Dispose();
        //Get one off the stack
        this._currentEnumerator = this._enumerators.Pop();
        //This one should have a folder on it waiting for us, return it.
        if (this.Current is DirectoryInfo) { return true; }
      }
      return result;
    }

    public void Reset()
    {
      //This one is for COM interoperability.  Don't implement.
      throw new NotImplementedException();
    }

  }

  /// <summary>
  /// A class that enumerates the file system entries in a folder.
  /// </summary>
  internal class FolderEnumerator : IEnumerator<FileSystemInfo>
  {

    private string _rootFolder = null;
    private FileSystemInfo _current = null;
    //WinAPI stuff
    private WIN32_FIND_DATA _findData;
    private IntPtr _hFind = FileHelperWinAPI.INVALID_HANDLE_VALUE;

    public FolderEnumerator(string rootFolder)
    {
      if (Directory.Exists(rootFolder))
      {
        this._rootFolder = rootFolder;
      }
      else { throw new Exception("No folder exists at: " + rootFolder); }
    }

    public FileSystemInfo Current
    {
      get
      {
        if (this._current == null) { throw new InvalidOperationException(); }
        return _current;
      }
    }

    public void Dispose()
    {
      if (this._hFind != FileHelperWinAPI.INVALID_HANDLE_VALUE) { FileHelperWinAPI.FindClose(this._hFind); this._hFind = FileHelperWinAPI.INVALID_HANDLE_VALUE; }
    }

    object System.Collections.IEnumerator.Current
    {
      get { return this.Current; }
    }

    public bool MoveNext()
    {
      bool result;
      if (this._current == null)
      {
        this._hFind = FileHelperWinAPI.FindFirstFile(this._rootFolder + "\\*", out this._findData);
        if (this._hFind == FileHelperWinAPI.INVALID_HANDLE_VALUE) { throw new Exception("Failed to get first file in directory: " + this._rootFolder, Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error())); }
        result = true;
      }
      else
      {
        result = FileHelperWinAPI.FindNextFile(this._hFind, out this._findData);
      }
      //Get out if there are no files
      if (!result) { this._current = null; return result; }

      //Jump over self and parent pointers
      while (this._findData.cFileName == "." || this._findData.cFileName == "..")
      {
        result = FileHelperWinAPI.FindNextFile(this._hFind, out this._findData);
      }

      //Get out if there are no files
      if (!result) { this._current = null; return result; }

      //Form the file or folder name
      var temp = Path.Combine(this._rootFolder, this._findData.cFileName);

      //Decide what the file system entry is
      switch (this._findData.dwFileAttributes)
      {
        case FileAttributes.FILE_ATTRIBUTE_DIRECTORY: { this._current = new DirectoryInfo(temp); break; }
        case FileAttributes.FILE_ATTRIBUTE_NORMAL: { this._current = new FileInfo(temp); break; }
        case FileAttributes.FILE_ATTRIBUTE_ARCHIVE: { this._current = new FileInfo(temp); break; }
        default: { throw new Exception("Can't determine if this is a folder or a file: " + temp); break; }
      }

      return true;
    }

    public void Reset()
    {
      //This one is for COM interoperability.  Don't implement.
      throw new NotImplementedException();
    }
  }
}
