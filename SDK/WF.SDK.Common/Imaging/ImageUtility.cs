using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;


namespace WF.SDK.Common.Imaging
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
  public class ImageUtility
  {
    public const float FAX_TIF_HOR_RES = 204;
    public const float FAX_TIF_VER_RES_LOW = 98;
    public const float FAX_TIF_VER_RES_HI = 196;

    public const int FAX_TIF_HOR_PX = 1728;

    public const int FAX_TIF_VER_PX_LTR_LOW = 1078;
    public const int FAX_TIF_VER_PX_LTR_HI = 2156;

    public const int FAX_TIF_VER_PX_LGL_LOW = 1372;
    public const int FAX_TIF_VER_PX_LGL_HI = 2744;

    public const float FAX_TIF_HOR_IN = FAX_TIF_HOR_PX / FAX_TIF_HOR_RES;  // 8.47
    public const float FAX_TIF_VER_IN_LTR = FAX_TIF_VER_PX_LTR_LOW / FAX_TIF_VER_RES_LOW; //11
    public const float FAX_TIF_VER_IN_LGL = FAX_TIF_VER_PX_LGL_LOW / FAX_TIF_VER_RES_LOW; //14

    public const float FAX_TIF_HOR_POINTS = FAX_TIF_HOR_IN * 72;
    public const float FAX_TIF_VER_POINTS_LTR = FAX_TIF_VER_IN_LTR * 72;
    public const float FAX_TIF_VER_POINTS_LGL = FAX_TIF_VER_IN_LGL * 72;

    public const float ASPECT_RATIO_FAX_LEGAL_POR = FAX_TIF_HOR_IN / FAX_TIF_VER_IN_LGL; //0.6050
    public const float ASPECT_RATIO_FAX_LETTER_POR = FAX_TIF_HOR_IN / FAX_TIF_VER_IN_LTR; //0.7701

    public const float ASPECT_RATIO_FAX_LETTER_LSC = 1 / ASPECT_RATIO_FAX_LETTER_POR;     //1.2986
    public const float ASPECT_RATIO_FAX_LEGAL_LSC = 1 / ASPECT_RATIO_FAX_LEGAL_POR;      //1.6528

    //The interpolation mode to use for all graphics operations
    private static InterpolationMode _interpolationMode = InterpolationMode.Low;
    private static HighToLowScaleMethod _scaleMethod = HighToLowScaleMethod.ANDing;
    private static ConvertTo1BppMethod _1BppMethod = ConvertTo1BppMethod.Aforge;
    //private static ErrorDiffusionToAdjacentNeighbors _diffusionFilter = new FloydSteinbergDithering();
    //private static Grayscale _grayscaleFilter = Grayscale.CommonAlgorithms.BT709;
    private static int _threshold = 500;

    private ImageUtility()
    {
    }

    static ImageUtility()
    {
    }

    #region Static Properties

    public static InterpolationMode InterpolationMode { get { return ImageUtility._interpolationMode; } set { ImageUtility._interpolationMode = value; } }

    public static HighToLowScaleMethod HighToLowScaleMethod { get { return ImageUtility._scaleMethod; } set { ImageUtility._scaleMethod = value; } }

    public static ConvertTo1BppMethod ConvertTo1BppMethod { get { return ImageUtility._1BppMethod; } set { ImageUtility._1BppMethod = value; } }

    public static int Threshold { get { return ImageUtility._threshold; } set { ImageUtility._threshold = value; } }

    #endregion

    #region ImageDiscovery
    public static ImageInfo DiscoverImage(Bitmap bmp)
    {
      return new ImageInfo(bmp);
    }

    public static ImageInfo DiscoverImage(Image image)
    {
      return new ImageInfo(image);
    }

    public static ImageInfo DiscoverImage(string imageFileName)
    {
      return new ImageInfo(imageFileName);
    }
    #endregion

    #region LoadImage

    /// <summary>
    /// Creates a new Page image list from a file on disk. Pages
    /// will be extracted into the Page image list if the source is multipage.
    /// </summary>
    /// <param name="imageFileNames">The source file.</param>
    /// <returns>A new Page Image List</returns>
    public static List<PageImage> LoadImage(string[] imageFileNames)
    {
      return ImageUtility.LoadImage(imageFileNames, true);
    }

    /// <summary>
    /// Creates a new Page image list from a file on disk. Pages
    /// will be extracted into the Page image list if the source is multipage.
    /// </summary>
    /// <param name="imageFileName">The source file.</param>
    /// <returns>A new Page Image List</returns>
    public static List<PageImage> LoadImage(string imageFileName)
    {
      return ImageUtility.LoadImage(imageFileName, true);
    }

    /// <summary>
    /// Creates a new Page image list from the given Bitmap object.  Pages
    /// will be extracted into the Page image list if the source is multipage.
    /// </summary>
    /// <param name="bmp">The given Bitmap object.</param>
    /// <returns>A new Page Image List</returns>
    public static List<PageImage> LoadImage(Bitmap bmp)
    {
      return ImageUtility.LoadImage(bmp, true);
    }

    /// <summary>
    /// Creates a new Page image list from the given Bitmap object.  Pages
    /// will be extracted into the Page image list if the source is multipage.
    /// </summary>
    /// <param name="image">The given Bitmap object.</param>
    /// <returns>A new Page Image List</returns>
    public static List<PageImage> LoadImage(Image image)
    {
      return ImageUtility.LoadImage(image, true);
    }

    /// <summary>
    /// Creates a new Page image list from the given page image list.
    /// </summary>
    /// <param name="pages">The given Page image list.</param>
    /// <returns>A new page image list</returns>
    public static List<PageImage> LoadImage(List<PageImage> pages)
    {
      return ImageUtility.LoadImage(pages, true);
    }

    /// <summary>
    /// Creates a new Page image list from a file on disk. Pages
    /// will be extracted into the Page image list if the source is multipage.
    /// </summary>
    /// <param name="imageFileNames">The source files.</param>
    /// <param name="convertTo32Bit">Whether to coerce into a 32 bit</param>
    /// <returns>A new Page Image List</returns>
    public static List<PageImage> LoadImage(string[] imageFileNames, bool convertTo32Bit)
    {
      List<PageImage> ret = new List<PageImage>();
      foreach (string file in imageFileNames)
      {
        ret.AddRange(ImageUtility.LoadImage(file, convertTo32Bit));
      }
      return ret;
    }

    /// <summary>
    /// Creates a new Page image list from a file on disk. Pages
    /// will be extracted into the Page image list if the source is multipage.
    /// </summary>
    /// <param name="imageFileName">The source file.</param>
    /// <param name="convertTo32Bit">Whether to coerce into a 32 bit</param>
    /// <returns>A new Page Image List</returns>
    public static List<PageImage> LoadImage(string imageFileName, bool convertTo32Bit)
    {
      if (String.IsNullOrEmpty(imageFileName.Trim())) { return null; }
      Image img = Image.FromFile(imageFileName);
      List<PageImage> ret = ImageUtility.LoadImage(img, convertTo32Bit);
      img.Dispose();
      return ret;
    }

    /// <summary>
    /// Creates a new Page image list from the given Bitmap object.  Pages
    /// will be extracted into the Page image list if the source is multipage.
    /// </summary>
    /// <param name="bmp">The given Bitmap object.</param>
    /// <param name="convertTo32Bit">Whether to coerce into a 32 bit</param>
    /// <returns>A new Page Image List</returns>
    public static List<PageImage> LoadImage(Bitmap bmp, bool convertTo32Bit)
    {
      return ImageUtility.LoadImage((Image)bmp, convertTo32Bit);
    }

    /// <summary>
    /// Creates a new Page image list from the given Bitmap object.  Pages
    /// will be extracted into the Page image list if the source is multipage.
    /// </summary>
    /// <param name="image">The given Image object.</param>
    /// <param name="convertTo32Bit">Whether to coerce into a 32 bit</param>
    /// <returns>A new Page Image List</returns>
    public static List<PageImage> LoadImage(Image image, bool convertTo32Bit)
    {
      return ImageUtility.InternalLoadImage(image, convertTo32Bit);
    }

    /// <summary>
    /// Creates a new Page image list from the given page image list.
    /// </summary>
    /// <param name="pages">The given Page image list.</param>
    /// <param name="convertTo32Bit">Whether to coerce into a 32 bit</param>
    /// <returns>A new page image list</returns>
    public static List<PageImage> LoadImage(List<PageImage> pages, bool convertTo32Bit)
    {
      List<PageImage> ret = new List<PageImage>();
      foreach (PageImage page in pages)
      {
        ret.AddRange(ImageUtility.LoadImage((Image)page.Bitmap, convertTo32Bit));
      }
      return ret;
    }


    /// <summary>
    /// Creates a new Page image list from a file on disk. Pages
    /// will be extracted into the Page image list if the source is multipage.
    /// </summary>
    /// <param name="imageFileName">The source file.</param>
    /// <param name="quality">Quality Conversion</param>
    /// <returns>A new Page Image List</returns>
    public static List<PageImage> LoadImage(string imageFileName, FaxQuality quality)
    {
      Image img = Image.FromFile(imageFileName);
      List<PageImage> ret = ImageUtility.LoadImage(img, true);
      img.Dispose();
      return ret;
    }

    /// <summary>
    /// Creates a new Page image list from the given Bitmap object.  Pages
    /// will be extracted into the Page image list if the source is multipage.
    /// </summary>
    /// <param name="bmp">The given Bitmap object.</param>
    /// <param name="quality"></param>
    /// <returns>A new Page Image List</returns>
    public static List<PageImage> LoadImage(Bitmap bmp, FaxQuality quality)
    {
      return ImageUtility.LoadImage((Image)bmp);
    }

    /// <summary>
    /// Creates a new Page image list from the given image object.  Pages
    /// will be extracted into the Page image list if the source is multipage.
    /// </summary>
    /// <param name="image">The given image object.</param>
    /// <param name="quality"></param>
    /// <returns>A new Page Image List</returns>
    public static List<PageImage> LoadImage(Image image, FaxQuality quality)
    {
      List<PageImage> ret = new List<PageImage>();
      FrameDimensionType frameDimTypeFlags = ImageHelper.GetFrameDimensionTypes(image.FrameDimensionsList);
      int pages = 0;
      //Get the Frame Dimensions if they exist
      if (FlagHelper.IsSet((int)frameDimTypeFlags, (int)FrameDimensionType.Page))
      {
        pages = image.GetFrameCount(ImageHelper.FrameDimensionPage);
      }

      //Foreach frame make a PageInfo
      for (int i = 0; i < pages; i++)
      {
        image.SelectActiveFrame(ImageHelper.FrameDimensionPage, i);
        ret.Add(new PageImage((Bitmap)image));
      }

      return ret;
    }

    /// <summary>
    /// Creates a new Page image list from the given image object.  Pages
    /// will be extracted into the Page image list if the source is multipage.
    /// </summary>
    /// <param name="image">The given image object.</param>
    /// <param name="convertTo32Bit">Whether to coerce into a 32 bit</param>
    /// <returns>A new Page Image List</returns>
    private static List<PageImage> InternalLoadImage(Image image, bool convertTo32Bit)
    {
      List<PageImage> ret = new List<PageImage>();
      FrameDimensionType frameDimTypeFlags = ImageHelper.GetFrameDimensionTypes(image.FrameDimensionsList);
      int pages = 0;

      FrameDimension frameType = ImageHelper.FrameDimensionPage;
      //Get the Frame Dimensions if they exist
      if (FlagHelper.IsSet((int)frameDimTypeFlags, (int)FrameDimensionType.Page))
      {
        pages = image.GetFrameCount(ImageHelper.FrameDimensionPage);
        if (pages > 0) { frameType = ImageHelper.FrameDimensionPage; }
      }

      if (FlagHelper.IsSet((int)frameDimTypeFlags, (int)FrameDimensionType.Time))
      {
        pages = image.GetFrameCount(ImageHelper.FrameDimensionTime);
        if (pages > 0) { frameType = ImageHelper.FrameDimensionTime; }
      }

      if (FlagHelper.IsSet((int)frameDimTypeFlags, (int)FrameDimensionType.Resolution))
      {
        pages = image.GetFrameCount(ImageHelper.FrameDimensionResolution);
        if (pages > 0) { frameType = ImageHelper.FrameDimensionResolution; }
      }

      if (pages == 0)  //Just try to load the single image
      {
        ret.Add(new PageImage((Bitmap)image, convertTo32Bit));
      }
      else //Otherwise load each page in the proper dimension
      {
        //Foreach frame make a PageInfo
        for (int i = 0; i < pages; i++)
        {
          image.SelectActiveFrame(frameType, i);
          ret.Add(new PageImage((Bitmap)image, convertTo32Bit));
        }
      }

      return ret;
    }

    #endregion

    #region SaveAsFaxTiff
    /// <summary>
    /// Reads and saves the indicated file as a fax tiff at the outputFilePath location using the quality option specified with AutoPageSize
    /// </summary>
    /// <param name="inputFilePath">The file to work on.</param>
    /// <param name="quality">Tiff Normal or Fine</param>
    /// <param name="paperSize">Letter, Legal or Auto</param>
    /// <param name="outputFilePath">Fully qualified output file name and path.</param>
    /// <returns></returns>
    public static ImageOperationResult SaveAsFaxTiff(string inputFilePath, string outputFilePath, FaxQuality quality = FaxQuality.Normal, PaperSize paperSize = PaperSize.Auto)
    {
      ImageOperationResult ret = new ImageOperationResult();

      List<PageImage> pages = ImageUtility.LoadImage(inputFilePath);

      ret = ImageUtility.InternalSaveAsFaxTiff(pages, quality, paperSize, outputFilePath);

      ImageUtility.Dispose(pages);

      return ret;
    }

    /// <summary>
    /// Saves the indicated Page Image List as a fax tiff at the outputFilePath location using the quality option specified with AutoPageSize
    /// </summary>
    /// <param name="inputFilePaths">The Images to save.</param>
    /// <param name="quality">Tiff Normal or Fine</param>
    /// <param name="paperSize">Letter, Legal or Auto</param>
    /// <param name="outputFilePath">Fully qualified file name and path.</param>
    /// <returns></returns>
    public static ImageOperationResult SaveAsFaxTiff(string[] inputFilePaths, string outputFilePath, FaxQuality quality = FaxQuality.Normal, PaperSize paperSize = PaperSize.Auto)
    {
      ImageOperationResult ret = new ImageOperationResult();

      List<PageImage> pages = ImageUtility.LoadImage(inputFilePaths);

      ret = ImageUtility.InternalSaveAsFaxTiff(pages, quality, paperSize, outputFilePath);

      ImageUtility.Dispose(pages);

      return ret;
    }

    /// <summary>
    /// Saves the indicated Bitmap object as a fax tiff at the outputFilePath location using the quality option specified with AutoPageSize
    /// </summary>
    /// <param name="bmp">The Bitmap object to save.</param>
    /// <param name="quality">Tiff Normal or Fine</param>
    /// <param name="paperSize">Letter, Legal or Auto</param>
    /// <param name="outputFilePath">Fully qualified file name and path.</param>
    /// <returns></returns>
    public static ImageOperationResult SaveAsFaxTiff(Bitmap bmp, string outputFilePath, FaxQuality quality = FaxQuality.Normal, PaperSize paperSize = PaperSize.Auto)
    {
      ImageOperationResult ret = new ImageOperationResult();

      List<PageImage> pages = ImageUtility.LoadImage(bmp);

      ret = ImageUtility.InternalSaveAsFaxTiff(pages, quality, paperSize, outputFilePath);

      ImageUtility.Dispose(pages);

      return ret;
    }

    /// <summary>
    /// Saves the indicated Image object as a fax tiff at the outputFilePath location using the quality option specified with AutoPageSize
    /// </summary>
    /// <param name="img">The Image object to save.</param>
    /// <param name="quality">Tiff Normal or Fine</param>
    /// <param name="paperSize">Letter, Legal or Auto</param>
    /// <param name="outputFilePath">Fully qualified file name and path.</param>
    /// <returns></returns>
    public static ImageOperationResult SaveAsFaxTiff(Image img, string outputFilePath, FaxQuality quality = FaxQuality.Normal, PaperSize paperSize = PaperSize.Auto)
    {
      ImageOperationResult ret = new ImageOperationResult();

      List<PageImage> pages = ImageUtility.LoadImage(img);

      ret = ImageUtility.InternalSaveAsFaxTiff(pages, quality, paperSize, outputFilePath);

      ImageUtility.Dispose(pages);

      return ret;
    }

    /// <summary>
    /// Saves the indicated Page Image List as a fax tiff at the outputFilePath location using the quality option specified with AutoPageSize
    /// </summary>
    /// <param name="pages">The Images to save.</param>
    /// <param name="quality">Tiff Normal or Fine</param>
    /// <param name="paperSize">Letter, Legal or Auto</param>
    /// <param name="outputFilePath">Fully qualified file name and path.</param>
    /// <returns></returns>
    public static ImageOperationResult SaveAsFaxTiff(List<PageImage> pages, string outputFilePath, FaxQuality quality = FaxQuality.Normal, PaperSize paperSize = PaperSize.Auto)
    {
      ImageOperationResult ret = new ImageOperationResult();

      ret = ImageUtility.InternalSaveAsFaxTiff(pages, quality, paperSize, outputFilePath);

      return ret;
    }

    /// <summary>
    /// Saves the indicated Page Image as a fax tiff at the outputFilePath location using the quality option specified with AutoPageSize
    /// </summary>
    /// <param name="page">The Image to save.</param>
    /// <param name="quality">Tiff Normal or Fine</param>
    /// <param name="paperSize">Letter, Legal or Auto</param>
    /// <param name="outputFilePath">Fully qualified file name and path.</param>
    /// <returns></returns>
    public static ImageOperationResult SaveAsFaxTiff(PageImage page, string outputFilePath, FaxQuality quality = FaxQuality.Normal, PaperSize paperSize = PaperSize.Auto)
    {
      ImageOperationResult ret = new ImageOperationResult();

      ret = ImageUtility.InternalSaveAsFaxTiff(new List<PageImage>() { page}, quality, paperSize, outputFilePath);

      return ret;
    }

    /// <summary>
    /// Saves the indicated Page Image List as a fax tiff at the outputFilePath location using the quality option specified with AutoPageSize
    /// </summary>
    /// <param name="pages">The Images to save.</param>
    /// <param name="quality">Tiff Normal or Fine</param>
    /// <param name="paperSize">Letter, Legal or Auto</param>
    /// <param name="outputFilePath">Fully qualified file name and path.</param>
    /// <returns></returns>
    internal static ImageOperationResult InternalSaveAsFaxTiff(List<PageImage> pages, FaxQuality quality, PaperSize paperSize, string outputFilePath)
    {
      ImageOperationResult result = new ImageOperationResult();

      List<PageImage> newPages = ImageConverter.ConvertToFaxablePageImageList(pages, quality, paperSize, result);

      ImageUtility.SavePagesAsFaxTiffFile(newPages, outputFilePath);

      result.Pages = newPages.Count;

      ImageUtility.Dispose(newPages);

      return result;
    }

    internal static void SavePagesAsFaxTiffFile(List<PageImage> pages, string outputFilePath)
    {
      if (pages.Count == 0) { return; }
      if (!Directory.Exists(Path.GetDirectoryName(outputFilePath))) { Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath)); }
      if (File.Exists(outputFilePath)) { File.Delete(outputFilePath); }

      ImageCodecInfo imageCodecInfo = ImageHelper.GetEncoder(ImageFormatNames.Tiff);

      System.Drawing.Imaging.Encoder enc1 = System.Drawing.Imaging.Encoder.SaveFlag;
      System.Drawing.Imaging.Encoder enc2 = System.Drawing.Imaging.Encoder.Compression;

      EncoderParameter encoderParameter11 = new EncoderParameter(enc1, (long)EncoderValue.MultiFrame);
      EncoderParameter encoderParameter12 = new EncoderParameter(enc1, (long)EncoderValue.FrameDimensionPage);
      EncoderParameter encoderParameter13 = new EncoderParameter(enc1, (long)EncoderValue.Flush);
      EncoderParameter encoderParameter21 = new EncoderParameter(enc2, (long)EncoderValue.CompressionCCITT3);

      EncoderParameters encoderParameters = new EncoderParameters(2);

      // Save the bitmap as a TIFF file with compression.
      encoderParameters.Param[0] = encoderParameter11;
      encoderParameters.Param[1] = encoderParameter21;

      FileStream strm = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);

      pages[0].Bitmap.Save(strm, imageCodecInfo, encoderParameters);

      encoderParameters.Param[0] = encoderParameter12;

      for (int i = 1; i < pages.Count; i++)
      {
        pages[0].Bitmap.SaveAdd(pages[i].Bitmap, encoderParameters);
      }

      encoderParameters.Param[0] = encoderParameter13;

      pages[0].Bitmap.SaveAdd(encoderParameters);

      strm.Close();
    }

    #endregion

    #region SaveAs32BitTiff

    public static void SaveAs32BitTiff(List<PageImage> pages, string outputFilePath)
    {
      ImageUtility.InternalSaveAs32BitTiff(pages, outputFilePath);
    }

    public static void SaveAs32BitTiff(Bitmap bmp, string outputFilePath)
    {
      ImageUtility.SaveAs32BitTiff(ImageUtility.LoadImage(bmp), outputFilePath);
    }

    public static void SaveAs32BitTiff(Image image, string outputFilePath)
    {
      ImageUtility.SaveAs32BitTiff(ImageUtility.LoadImage(image), outputFilePath);
    }

    public static void SaveAs32BitTiff(string inputFilePath, string outputFilePath)
    {
      ImageUtility.SaveAs32BitTiff(ImageUtility.LoadImage(inputFilePath), outputFilePath);
    }

    internal static void InternalSaveAs32BitTiff(List<PageImage> pages, string outputFilePath)
    {
      if (!Directory.Exists(Path.GetDirectoryName(outputFilePath))) { Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath)); }
      if (File.Exists(outputFilePath)) { File.Delete(outputFilePath); }

      ImageCodecInfo imageCodecInfo = ImageHelper.GetEncoder(ImageFormatNames.Tiff);

      System.Drawing.Imaging.Encoder enc = System.Drawing.Imaging.Encoder.SaveFlag;

      EncoderParameter encoderParameter1 = new EncoderParameter(enc, (long)EncoderValue.MultiFrame);
      EncoderParameter encoderParameter2 = new EncoderParameter(enc, (long)EncoderValue.FrameDimensionPage);
      EncoderParameter encoderParameter3 = new EncoderParameter(enc, (long)EncoderValue.Flush);

      EncoderParameters encoderParameters = new EncoderParameters(1);

      // Save the bitmap as a TIFF file with no compression.
      encoderParameters.Param[0] = encoderParameter1;

      FileStream strm = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);

      pages[0]._sourceBmp.Save(strm, imageCodecInfo, encoderParameters);

      encoderParameters.Param[0] = encoderParameter2;

      for (int i = 1; i < pages.Count; i++)
      {
        pages[0]._sourceBmp.SaveAdd(pages[i]._sourceBmp, encoderParameters);
      }

      encoderParameters.Param[0] = encoderParameter3;

      pages[0]._sourceBmp.SaveAdd(encoderParameters);

      strm.Close();
    }

    #endregion

    #region ConvertTo32BitBitmap

    public static Image ConvertTo32BitBitmap(string inputFilePath)
    {
      return ConvertTo32BitBitmap(ImageUtility.LoadImage(inputFilePath));
    }

    public static Image ConvertTo32BitBitmap(Bitmap bmp)
    {
      return ConvertTo32BitBitmap(ImageUtility.LoadImage(bmp));
    }

    public static Image ConvertTo32BitBitmap(Image image)
    {
      return ConvertTo32BitBitmap(ImageUtility.LoadImage(image));
    }

    public static Image ConvertTo32BitBitmap(List<PageImage> pageImageList)
    {
      string newfile = Path.GetTempFileName();
      ImageUtility.SaveAs32BitTiff(pageImageList, newfile);
      ImageUtility.Dispose(pageImageList);
      Image ret = Image.FromFile(newfile);
      return ret;
    }

    #endregion

    #region VerifyAllFilesAndWriteFaxTiff
    public static ImageOperationResult VerifyAllTiffFilesAndWriteFaxTiff(List<PageImage> pages, string outputFilePath, PaperSize paperSize, FaxQuality faxQuality)
    {
      ImageOperationResult ret = new ImageOperationResult();

      List<PageImage> newpages = ImageConverter.CreateFaxTiff(pages, paperSize, faxQuality, ret);

      ImageUtility.Dispose(pages);

      ImageUtility.SavePagesAsFaxTiffFile(newpages, outputFilePath);

      ret.Pages = newpages.Count;

      ImageUtility.Dispose(newpages);

      return ret;
    }

    public static ImageOperationResult VerifyAllTiffFilesAndWriteFaxTiff(string inputFilePath, string outputFilePath, PaperSize paperSize, FaxQuality faxQuality)
    {
      return ImageUtility.VerifyAllTiffFilesAndWriteFaxTiff(new string[] { inputFilePath }, outputFilePath, paperSize, faxQuality);
    }

    public static ImageOperationResult VerifyAllTiffFilesAndWriteFaxTiff(string[] inputFilePaths, string outputFilePath, PaperSize paperSize, FaxQuality faxQuality)
    {
      ImageOperationResult ret = new ImageOperationResult();

      List<PageImage> pages = ImageUtility.LoadImage(inputFilePaths, false);

      List<PageImage> newpages = ImageConverter.CreateFaxTiff(pages, paperSize, faxQuality, ret);

      ImageUtility.Dispose(pages);

      ImageUtility.SavePagesAsFaxTiffFile(newpages, outputFilePath);

      ret.Pages = newpages.Count;

      ImageUtility.Dispose(newpages);

      return ret;
    }
    #endregion

    #region CropAndVerifyAllTiffFilesAndWriteFaxTiff
    public static ImageOperationResult CropAndVerifyAllTiffFilesAndWriteFaxTiff(string inputFilePath, string outputFilePath)
    {
      return ImageUtility.CropAndVerifyAllTiffFilesAndWriteFaxTiff(new List<string>() { inputFilePath }, outputFilePath);
    }

    public static ImageOperationResult CropAndVerifyAllTiffFilesAndWriteFaxTiff(List<string> inputFilePaths, string outputFilePath)
    {
      ImageOperationResult ret = new ImageOperationResult();

      List<PageImage> pages = ImageUtility.LoadImage(inputFilePaths.ToArray(), false);

      var newPages = ImageConverter.CreateCroppedPageImageList(pages);

      ImageUtility.Dispose(pages);

      ImageUtility.SavePagesAsFaxTiffFile(newPages, outputFilePath);

      ret.Pages = newPages.Count;

      ImageUtility.Dispose(newPages);

      return ret;
    }
    #endregion

    public static List<PageImage> ConvertToFaxPageImageList(List<PageImage> pages, FaxQuality quality, PaperSize paperSize)
    {
      ImageOperationResult result = new ImageOperationResult();
      return ImageConverter.ConvertToFaxablePageImageList(pages, quality, paperSize, result);
    }

    public static void Dispose(List<PageImage> pages)
    {
      pages.ForEach(i => i.Dispose());
      pages.Clear();
    }

  }
}
