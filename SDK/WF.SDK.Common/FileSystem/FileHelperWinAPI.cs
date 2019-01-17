using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Management;

namespace WF.SDK.Common
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  internal struct WIN32_FIND_DATA
  {
    public uint dwFileAttributes;
    public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
    public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
    public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
    public uint nFileSizeHigh;
    public uint nFileSizeLow;
    public uint dwReserved0;
    public uint dwReserved1;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string cFileName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
    public string cAlternateFileName;
  }

  internal static class FileAttributes
  {
    public const uint FILE_ATTRIBUTE_READONLY = 1;
    public const uint FILE_ATTRIBUTE_HIDDEN = 2;
    public const uint FILE_ATTRIBUTE_SYSTEM = 4;

    public const uint FILE_ATTRIBUTE_DIRECTORY = 16;

    public const uint FILE_ATTRIBUTE_ARCHIVE = 32;
    public const uint FILE_ATTRIBUTE_DEVICE = 64;
    public const uint FILE_ATTRIBUTE_NORMAL = 128;
    public const uint FILE_ATTRIBUTE_TEMPORARY = 256;
    public const uint FILE_ATTRIBUTE_SPARSE_FILE = 512;
    public const uint FILE_ATTRIBUTE_REPARSE_POINT = 1024;
    public const uint FILE_ATTRIBUTE_COMPRESSED = 2048;
    public const uint FILE_ATTRIBUTE_OFFLINE = 4096;
    public const uint FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 8192;
    public const uint FILE_ATTRIBUTE_ENCRYPTED = 16384;
    public const uint FILE_ATTRIBUTE_INTEGRITY_STREAM = 32768;
    public const uint FILE_ATTRIBUTE_VIRTUAL = 65536;
    public const uint FILE_ATTRIBUTE_NO_SCRUB_DATA = 131072;
  }

  internal static class FileHelperWinAPI
  {
    internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    internal static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    internal static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

    [DllImport("kernel32.dll")]
    internal static extern bool FindClose(IntPtr hFindFile);
  }
}
