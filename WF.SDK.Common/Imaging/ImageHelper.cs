using System;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace WF.SDK.Common.Imaging
{
	/// <summary>
	/// Summary description for ImageHelper.
	/// </summary>
	public class ImageHelper
	{
		private static Hashtable ImageFormats;
		private static Hashtable EncoderParams;

		private static readonly Guid ImageFormatUndefined = new Guid(0xb96b3ca9, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
		private static readonly Guid ImageFormatMemoryBMP = new Guid(0xb96b3caa, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
		private static readonly Guid ImageFormatBMP = new Guid(0xb96b3cab, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
		private static readonly Guid ImageFormatEMF = new Guid(0xb96b3cac, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
		private static readonly Guid ImageFormatWMF = new Guid(0xb96b3cad, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
		private static readonly Guid ImageFormatJPEG = new Guid(0xb96b3cae, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
		private static readonly Guid ImageFormatPNG = new Guid(0xb96b3caf, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
		private static readonly Guid ImageFormatGIF = new Guid(0xb96b3cb0, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
		private static readonly Guid ImageFormatTIFF = new Guid(0xb96b3cb1, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
		private static readonly Guid ImageFormatEXIF = new Guid(0xb96b3cb2, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);
		private static readonly Guid ImageFormatIcon = new Guid(0xb96b3cb5, 0x0728, 0x11d3, 0x9d, 0x7b, 0x00, 0x00, 0xf8, 0x1e, 0xf3, 0x2e);

		private static readonly Guid frameDimensionTime = new Guid(0x6aedbd6d, 0x3fb5, 0x418a, 0x83, 0xa6, 0x7f, 0x45, 0x22, 0x9d, 0xc8, 0x72);
		private static readonly Guid frameDimensionResolution = new Guid(0x84236f7b, 0x3bd3, 0x428f, 0x8d, 0xab, 0x4e, 0xa1, 0x43, 0x9c, 0xa3, 0x15);
		private static readonly Guid frameDimensionPage = new Guid(0x7462dc86, 0x6180, 0x4c7e, 0x8e, 0x3f, 0xee, 0x73, 0x33, 0xa7, 0xa4, 0x83);

		public static readonly Guid CodecIImageBytes = new Guid(0x025d1823, 0x6c7d, 0x447b, 0xbb, 0xdb, 0xa3, 0xcb, 0xc3, 0xdf, 0xa2, 0xfc);

		public static FrameDimension FrameDimensionPage = new FrameDimension(frameDimensionPage);
		public static FrameDimension FrameDimensionResolution = new FrameDimension(frameDimensionResolution);
		public static FrameDimension FrameDimensionTime = new FrameDimension(frameDimensionTime);

		private ImageHelper()
		{
		}

		static ImageHelper()
		{
			ImageHelper.ImageFormats = new Hashtable();
			ImageHelper.ImageFormats[ImageFormatUndefined] = ImageFormatNames.Undefined;
			ImageHelper.ImageFormats[ImageFormatMemoryBMP] = ImageFormatNames.MemoryBmp;
			ImageHelper.ImageFormats[ImageFormatBMP] = ImageFormatNames.Bmp;
			ImageHelper.ImageFormats[ImageFormatEMF] = ImageFormatNames.Emf;
			ImageHelper.ImageFormats[ImageFormatWMF] = ImageFormatNames.Wmf;
			ImageHelper.ImageFormats[ImageFormatJPEG] = ImageFormatNames.Jpeg;
			ImageHelper.ImageFormats[ImageFormatPNG] = ImageFormatNames.Png;
			ImageHelper.ImageFormats[ImageFormatGIF] = ImageFormatNames.Gif;
			ImageHelper.ImageFormats[ImageFormatTIFF] = ImageFormatNames.Tiff;
			ImageHelper.ImageFormats[ImageFormatEXIF] = ImageFormatNames.Exif;
			ImageHelper.ImageFormats[ImageFormatIcon] = ImageFormatNames.Icon;
			ImageHelper.EncoderParams = new Hashtable();
			ImageHelper.EncoderParams[System.Drawing.Imaging.Encoder.ChrominanceTable.Guid] = EncoderParameterNames.EncoderChrominanceTable;
			ImageHelper.EncoderParams[System.Drawing.Imaging.Encoder.ColorDepth.Guid] = EncoderParameterNames.EncoderColorDepth;
			ImageHelper.EncoderParams[System.Drawing.Imaging.Encoder.Compression.Guid] = EncoderParameterNames.EncoderCompression;
			ImageHelper.EncoderParams[System.Drawing.Imaging.Encoder.LuminanceTable.Guid] = EncoderParameterNames.EncoderLuminanceTable;
			ImageHelper.EncoderParams[System.Drawing.Imaging.Encoder.Quality.Guid] = EncoderParameterNames.EncoderQuality;
			ImageHelper.EncoderParams[System.Drawing.Imaging.Encoder.RenderMethod.Guid] = EncoderParameterNames.EncoderRenderMethod;
			ImageHelper.EncoderParams[System.Drawing.Imaging.Encoder.SaveFlag.Guid] = EncoderParameterNames.EncoderSaveFlag;
			ImageHelper.EncoderParams[System.Drawing.Imaging.Encoder.ScanMethod.Guid] = EncoderParameterNames.EncoderScanMethod;
			ImageHelper.EncoderParams[System.Drawing.Imaging.Encoder.Transformation.Guid] = EncoderParameterNames.EncoderTransformation;
			ImageHelper.EncoderParams[System.Drawing.Imaging.Encoder.Version.Guid] = EncoderParameterNames.EncoderVersion;
		}

		#region Image Format Helpers
		public static ImageFormatNames GetImageFormat_Name(Image img)
		{
			return (ImageFormatNames)ImageHelper.ImageFormats[img.RawFormat.Guid];
		}

		public static ImageFormatNames GetImageFormat_Name(Guid formatId)
		{
			return (ImageFormatNames)ImageHelper.ImageFormats[formatId];
		}

		public static Guid GetImageFormat_Guid(ImageFormatNames formatName)
		{
			foreach (Guid key in ImageHelper.ImageFormats.Keys)
			{
				if ((ImageFormatNames)ImageHelper.ImageFormats[key] == formatName)
				{
					return key;
				}
			}
			return ImageHelper.ImageFormatUndefined;
		}

		public static ImageFormat GetImageFormat_Instance(ImageFormatNames formatName)
		{
			foreach (Guid key in ImageHelper.ImageFormats.Keys)
			{
				if ((ImageFormatNames)ImageHelper.ImageFormats[key] == formatName)
				{
					return new ImageFormat(key);
				}
			}
			return new ImageFormat(ImageHelper.ImageFormatUndefined);
		}

		public static ImageFormat GetImageFormat_Instance(Guid formatId)
		{
			return new ImageFormat(formatId);
		}

		#endregion
		#region FrameDimension Helpers

		public static FrameDimension GetFrameDimension(FrameDimensionType type)
		{
			FrameDimension ret = null;
			switch (type)
			{
				case FrameDimensionType.None: { break; }
				case FrameDimensionType.Page: { ret = ImageHelper.FrameDimensionPage; break; }
				case FrameDimensionType.Resolution: { ret = ImageHelper.FrameDimensionResolution; break; }
				case FrameDimensionType.Time: { ret = ImageHelper.FrameDimensionTime; break; }
			}
			return ret;
		}

		public static FrameDimensionType GetFrameDimensionTypes(Guid[] framesDims)
		{
			FrameDimensionType ret = FrameDimensionType.None;
			foreach (Guid dim in framesDims)
			{
				if (dim == ImageHelper.frameDimensionPage) { ret = (FrameDimensionType)((int)ret + (int)FrameDimensionType.Page); }
				if (dim == ImageHelper.frameDimensionResolution) { ret = ret & FrameDimensionType.Resolution; }
				if (dim == ImageHelper.frameDimensionTime) { ret = ret & FrameDimensionType.Time; }
			}
			return ret;
		}

		public static FrameDimension FrameDimension_Page()
		{
			return ImageHelper.FrameDimensionPage;
		}

		public static FrameDimension FrameDimension_Resolution()
		{
			return ImageHelper.FrameDimensionResolution;
		}

		public static FrameDimension FrameDimension_Time()
		{
			return ImageHelper.FrameDimensionTime;
		}
		#endregion
		#region Encoder Parameter Helpers
		public static string GetEncoderName(Guid enc)
		{
			return ((EncoderParameterNames)ImageHelper.EncoderParams[enc]).ToString();
		}

		public static string GetEncoderName(System.Drawing.Imaging.Encoder enc)
		{
			return ((EncoderParameterNames)ImageHelper.EncoderParams[enc.Guid]).ToString();
		}
		#endregion
		#region Codec Helpers
		public static ImageCodecInfo GetEncoder(ImageFormatNames formatName)
		{
			ImageCodecInfo[] inflist = ImageCodecInfo.GetImageEncoders();
			foreach (ImageCodecInfo inf in inflist)
			{
				if (inf.FormatID == ImageHelper.GetImageFormat_Guid(formatName))
				{
					return inf;
				}
			}
			return null;
		}

		public static string ShowImageDecoders()
		{
			StringBuilder sb = new StringBuilder();

			ImageCodecInfo[] inflist = ImageCodecInfo.GetImageDecoders();

			sb.Append("-------------------\r\n");
			sb.Append("ImageDecoders Count: " + inflist.Length.ToString() + "\r\n");

			foreach (ImageCodecInfo inf in inflist)
			{
				ShowCodecInfo(inf, sb);
			}
			return sb.ToString();
		}

		public static string ShowImageEncoders()
		{
			StringBuilder sb = new StringBuilder();

			ImageCodecInfo[] inflist = ImageCodecInfo.GetImageEncoders();

			sb.Append("-------------------\r\n");
			sb.Append("ImageEncoders Count: " + inflist.Length.ToString() + "\r\n");

			foreach (ImageCodecInfo inf in inflist)
			{
				ShowCodecInfo(inf, sb);
				ShowEncoderParameters(inf, sb);
			}
			return sb.ToString();
		}

		private static void ShowCodecInfo(ImageCodecInfo inf, StringBuilder sb)
		{
			sb.Append("---------Codec Info----------\r\n");
			sb.Append("Codec Name : " + inf.CodecName + "\r\n");
			sb.Append("Codec Dll : " + inf.DllName + "\r\n");
			sb.Append("Codec FileName Extension : " + inf.FilenameExtension + "\r\n");
			sb.Append("Codec Flags : " + Enum.Format(typeof(ImageCodecFlags), inf.Flags, "g") + "\r\n");
			sb.Append("Codec Format Description : " + inf.FormatDescription + "\r\n");
			sb.Append("Codec Format ID : " + ImageHelper.GetImageFormat_Name(inf.FormatID) + "\r\n");
			sb.Append("Codec Mime Type : " + inf.MimeType + "\r\n");
			sb.Append("Codec Version : " + inf.Version.ToString() + "\r\n");
		}

		private static void ShowEncoderParameters(ImageCodecInfo inf, StringBuilder sb)
		{
			Bitmap b = new Bitmap(1, 1);
			EncoderParameters list = null;
			try
			{
				list = b.GetEncoderParameterList(inf.Clsid);
			}
			catch (Exception e)
			{
				string s = e.Message;
				return;
			}

			sb.Append("++++++++Encoder Parameters Info++++++++\r\n");

			foreach (EncoderParameter param in list.Param)
			{
				sb.Append("Encoder Category: " + ImageHelper.GetEncoderName(param.Encoder) + "\r\n");
				sb.Append("Number of Values: " + param.NumberOfValues + "\r\n");
				sb.Append("Type: " + param.Type.ToString() + "\r\n");
				sb.Append("Value Type: " + param.ValueType.ToString() + "\r\n");
			}
		}

		private static Guid GetImageEncoderID(ImageFormatNames formatName)
		{
			Guid FormatId = ImageHelper.GetImageFormat_Guid(formatName);

			ImageCodecInfo[] inflist = ImageCodecInfo.GetImageEncoders();

			foreach (ImageCodecInfo inf in inflist)
			{
				if (inf.FormatID == FormatId) { return inf.Clsid; }
			}
			return Guid.Empty;
		}

		#endregion

		public static bool IsIndexedFormat(Image img)
		{
			return IsIndexedFormat((Bitmap)img);
		}

		public static bool IsIndexedFormat(Bitmap bmp)
		{
			if (bmp.PixelFormat == PixelFormat.Indexed ||
				bmp.PixelFormat == PixelFormat.Format1bppIndexed ||
				bmp.PixelFormat == PixelFormat.Format4bppIndexed ||
				bmp.PixelFormat == PixelFormat.Format8bppIndexed)
			{ return true; }
			else { return false; }
		}
	}
}
