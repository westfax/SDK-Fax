using System;

namespace WF.SDK.Common.Imaging
{

  /// <summary>
  /// Converter options for Tuespeking HTML to PDF
  /// </summary>
  public enum HtmlToPSConverterOption
  {
    ColorMode_GreyScale,
    ColorMode_Color,

    Resolution_72,
    Resolution_100,
    Resolution_200,
    Resolution_300,
    Resolution_400,
    Resolution_600,

    Margin_Top_00,
    Margin_Top_10,
    Margin_Top_20,
    Margin_Top_25,
    Margin_Top_30,
    Margin_Top_40,
    Margin_Top_50,
    Margin_Top_60,
    Margin_Top_70,
    Margin_Top_75,
    Margin_Top_80,
    Margin_Top_90,
    Margin_Top_100,

    Margin_Btm_00,
    Margin_Btm_10,
    Margin_Btm_20,
    Margin_Btm_25,
    Margin_Btm_30,
    Margin_Btm_40,
    Margin_Btm_50,
    Margin_Btm_60,
    Margin_Btm_70,
    Margin_Btm_75,
    Margin_Btm_80,
    Margin_Btm_90,
    Margin_Btm_100,

    Margin_Left_00,
    Margin_Left_10,
    Margin_Left_20,
    Margin_Left_25,
    Margin_Left_30,
    Margin_Left_40,
    Margin_Left_50,
    Margin_Left_60,
    Margin_Left_70,
    Margin_Left_75,
    Margin_Left_80,
    Margin_Left_90,
    Margin_Left_100,

    Margin_Right_00,
    Margin_Right_10,
    Margin_Right_20,
    Margin_Right_25,
    Margin_Right_30,
    Margin_Right_40,
    Margin_Right_50,
    Margin_Right_60,
    Margin_Right_70,
    Margin_Right_75,
    Margin_Right_80,
    Margin_Right_90,
    Margin_Right_100,

    Zoom_125,
    Zoom_120,
    Zoom_115,
    Zoom_110,
    Zoom_105,
    Zoom_100,
    Zoom_95,
    Zoom_90,
    Zoom_85,
    Zoom_80,
    Zoom_75,
  }

  /// <summary>
  /// Needed so that the tiffer knows the class of file and
  /// so it knows where in the tiff procedure chain to begin.
  /// Enum types can be anded together but the only supported
  /// combination is Image, Office, or PostScript anded with List.
  /// Combine them any other way at your own risk!
  /// </summary>
  [Flags]
  public enum FileClass
  {
    None = 0,
    /// <summary>
    /// Jpg, bmp, png, tif, etc.
    /// </summary>
    Image = 1,
    /// <summary>
    /// Doc, Ppt, Pub, Xls, Txt
    /// </summary>
    Office = 2,
    /// <summary>
    /// Ps, Pdf, Eps
    /// </summary>
    PostScript = 4,
    /// <summary>
    /// Xls, Dbf, Csv, Txt
    /// </summary>
    List = 8,
  }

  /// <summary>
  /// The method of reducing a High Tiff to a Low Tiff. 
  /// </summary>
  public enum HighToLowScaleMethod
  {/// <summary>
   /// Don't scale, keep in high fax format.
   /// </summary>
    NoScale = 0,
    /// <summary>
    /// Elimination deletes everyother line - good for dark fonts
    /// </summary>
    Elimination = 1,
    /// <summary>
    /// ORs every other line together
    /// </summary>
    ORing = 2,
    /// <summary>
    /// Averaging averages two together - good for dithered images
    /// </summary>
    Averaging = 3,
    /// <summary>
    /// ANDs every other line together
    /// </summary>
    ANDing = 4,
    NearestNeighbor = 5,
    Bilinear = 6,
  }

  public enum ConvertTo1BppMethod
  {
    Threshold = 1,
    Aforge = 2,
  }

  /// <summary>
  /// Defines the width and height of the created tiff.
  /// Auto will convert based on the size of the source document.
  /// </summary>
  public enum PaperSize
  {
    /// <summary>
    /// Determine paper size by image size.
    /// </summary>
    Auto = 0,
    /// <summary>
    /// Legal letter size.
    /// </summary>
    Legal = 1,
    /// <summary>
    /// Letter size.
    /// </summary>
    Letter = 2,
    /// <summary>
    /// No Value.
    /// </summary>
    Undefined = 3,
  }

  /// <summary>
  /// Paper orientation
  /// </summary>
  public enum PaperOrientation
  {
    /// <summary>
    /// Determine paper size by image size.
    /// </summary>
    None = 0,
    /// <summary>
    /// Legal letter size.
    /// </summary>
    Landscape = 1,
    /// <summary>
    /// Letter size.
    /// </summary>
    Portrait = 2,
  }

  /// <summary>
  /// Procesisng Options.
  /// </summary>
  [Flags]
  public enum ProcessingOption
  {
    /// <summary>
    /// No option
    /// </summary>
    None = 0,
    /// <summary>
    /// Fit to page - Absence is NoResize
    /// </summary>
    FitToPage = 1,
    /// <summary>
    /// Rotate the image - Absence is NoRotate
    /// </summary>
    Rotate90 = 2,

  }

  /// <summary>
  /// Defines the quality of the tiff.
  /// </summary>
  public enum FaxQuality
  {
    /// <summary>
    /// 204x98 pixels per square inch.
    /// </summary>
    Normal = 0,
    /// <summary>
    /// 204x196 pixels per square inch.
    /// </summary>
    Fine = 1,
    /// <summary>
    /// Defaults to Normal (204x98 pixels per square inch).
    /// </summary>
    Default = 0,
    /// <summary>
    /// No value
    /// </summary>
    Undefined = 3,
  }

  [Flags]
  public enum FrameDimensionType
  {
    None = 0,
    Page = 1,
    Resolution = 2,
    Time = 4,
  }

  [Flags]
  public enum ImageOperationWarning
  {
    None = 0,
    DramaticScalingRequired = 1,
    InconsistentSourceGeometry = 2,
    FillRatioThresholdExceeded = 4,
    LegalSizePageRendered = 8,
  }

  public enum ImageFormatNames
  {
    Bmp = 1,
    Emf = 2,
    Exif = 3,
    Gif = 4,
    Icon = 5,
    Jpeg = 6,
    MemoryBmp = 7,
    Png = 8,
    Tiff = 9,
    Wmf = 10,
    Undefined = 11,
    Unknown = 12,
  }

  public enum EncoderParameterNames
  {
    EncoderCompression = 1,
    EncoderColorDepth = 2,
    EncoderScanMethod = 3,
    EncoderVersion = 4,
    EncoderRenderMethod = 5,
    EncoderQuality = 6,
    EncoderTransformation = 7,
    EncoderLuminanceTable = 8,
    EncoderChrominanceTable = 9,
    EncoderSaveFlag = 10,
  }


  public enum PropertyIdType
  {
    /// <summary>
    /// Specifies that the value data member is an array of bytes.
    /// </summary>
    PropertyTagTypeByte = 1,
    /// <summary>
    /// Specifies that the value data member is a null-terminated ASCII string. If you set the type data member of a PropertyItem object to PropertyTagTypeASCII, you should set the length data member to the length of the string including the NULL terminator. For example, the string HELLO would have a length of 6.
    /// </summary>
    PropertyTagTypeASCII = 2,
    /// <summary>
    /// Specifies that the value data member is an array of unsigned short (16-bit) integers.
    /// </summary>
    PropertyTagTypeShort = 3,
    /// <summary>
    /// Specifies that the value data member is an array of unsigned long (32-bit) integers.
    /// </summary>
    PropertyTagTypeLong = 4,
    /// <summary>
    /// Specifies that the value data member is an array of pairs of unsigned long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator.
    /// </summary>
    PropertyTagTypeRational = 5,
    /// <summary>
    /// Specifies that the value data member is an array of bytes that can hold values of any data type.  
    /// </summary>
    PropertyTagTypeUndefined = 7,
    /// <summary>
    /// Specifies that the value data member is an array of signed long (32-bit) integers.
    /// </summary>
    PropertyTagTypeSLONG = 9,
    /// <summary>
    /// Specifies that the value data member is an array of pairs of signed long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator.
    /// </summary>
    PropertyTagTypeSRational = 10,
  }

  public enum PropertyId
  {

    //---------------------------------------------------------------------------
    // Image property ID tags
    //---------------------------------------------------------------------------
    PropertyTagExifIFD = 0x8769,
    PropertyTagGpsIFD = 0x8825,

    PropertyTagNewSubfileType = 0x00FE,
    PropertyTagSubfileType = 0x00FF,
    PropertyTagImageWidth = 0x0100,
    PropertyTagImageHeight = 0x0101,
    PropertyTagBitsPerSample = 0x0102,
    PropertyTagCompression = 0x0103,
    PropertyTagPhotometricInterp = 0x0106,
    PropertyTagThreshHolding = 0x0107,
    PropertyTagCellWidth = 0x0108,
    PropertyTagCellHeight = 0x0109,
    PropertyTagFillOrder = 0x010A,
    PropertyTagDocumentName = 0x010D,
    PropertyTagImageDescription = 0x010E,
    PropertyTagEquipMake = 0x010F,
    PropertyTagEquipModel = 0x0110,
    PropertyTagStripOffsets = 0x0111,
    PropertyTagOrientation = 0x0112,
    PropertyTagSamplesPerPixel = 0x0115,
    PropertyTagRowsPerStrip = 0x0116,
    PropertyTagStripBytesCount = 0x0117,
    PropertyTagMinSampleValue = 0x0118,
    PropertyTagMaxSampleValue = 0x0119,
    PropertyTagXResolution = 0x011A, // Image resolution in width direction
    PropertyTagYResolution = 0x011B, // Image resolution in height direction
    PropertyTagPlanarConfig = 0x011C, // Image data arrangement
    PropertyTagPageName = 0x011D,
    PropertyTagXPosition = 0x011E,
    PropertyTagYPosition = 0x011F,
    PropertyTagFreeOffset = 0x0120,
    PropertyTagFreeByteCounts = 0x0121,
    PropertyTagGrayResponseUnit = 0x0122,
    PropertyTagGrayResponseCurve = 0x0123,
    PropertyTagT4Option = 0x0124,
    PropertyTagT6Option = 0x0125,
    PropertyTagResolutionUnit = 0x0128, // Unit of X and Y resolution
    PropertyTagPageNumber = 0x0129,
    PropertyTagTransferFuncition = 0x012D,
    PropertyTagSoftwareUsed = 0x0131,
    PropertyTagDateTime = 0x0132,
    PropertyTagArtist = 0x013B,
    PropertyTagHostComputer = 0x013C,
    PropertyTagPredictor = 0x013D,
    PropertyTagWhitePoint = 0x013E,
    PropertyTagPrimaryChromaticities = 0x013F,
    PropertyTagColorMap = 0x0140,
    PropertyTagHalftoneHints = 0x0141,
    PropertyTagTileWidth = 0x0142,
    PropertyTagTileLength = 0x0143,
    PropertyTagTileOffset = 0x0144,
    PropertyTagTileByteCounts = 0x0145,
    PropertyTagInkSet = 0x014C,
    PropertyTagInkNames = 0x014D,
    PropertyTagNumberOfInks = 0x014E,
    PropertyTagDotRange = 0x0150,
    PropertyTagTargetPrinter = 0x0151,
    PropertyTagExtraSamples = 0x0152,
    PropertyTagSampleFormat = 0x0153,
    PropertyTagSMinSampleValue = 0x0154,
    PropertyTagSMaxSampleValue = 0x0155,
    PropertyTagTransferRange = 0x0156,

    PropertyTagJPEGProc = 0x0200,
    PropertyTagJPEGInterFormat = 0x0201,
    PropertyTagJPEGInterLength = 0x0202,
    PropertyTagJPEGRestartInterval = 0x0203,
    PropertyTagJPEGLosslessPredictors = 0x0205,
    PropertyTagJPEGPointTransforms = 0x0206,
    PropertyTagJPEGQTables = 0x0207,
    PropertyTagJPEGDCTables = 0x0208,
    PropertyTagJPEGACTables = 0x0209,

    PropertyTagYCbCrCoefficients = 0x0211,
    PropertyTagYCbCrSubsampling = 0x0212,
    PropertyTagYCbCrPositioning = 0x0213,
    PropertyTagREFBlackWhite = 0x0214,

    PropertyTagICCProfile = 0x8773, // This TAG is defined by ICC for embedded ICC in TIFF
    PropertyTagGamma = 0x0301,
    PropertyTagICCProfileDescriptor = 0x0302,
    PropertyTagSRGBRenderingIntent = 0x0303,

    PropertyTagImageTitle = 0x0320,
    PropertyTagCopyright = 0x8298,

    // Extra TAGs (Like Adobe Image Information tags etc.)

    PropertyTagResolutionXUnit = 0x5001,
    PropertyTagResolutionYUnit = 0x5002,
    PropertyTagResolutionXLengthUnit = 0x5003,
    PropertyTagResolutionYLengthUnit = 0x5004,
    PropertyTagPrintFlags = 0x5005,
    PropertyTagPrintFlagsVersion = 0x5006,
    PropertyTagPrintFlagsCrop = 0x5007,
    PropertyTagPrintFlagsBleedWidth = 0x5008,
    PropertyTagPrintFlagsBleedWidthScale = 0x5009,
    PropertyTagHalftoneLPI = 0x500A,
    PropertyTagHalftoneLPIUnit = 0x500B,
    PropertyTagHalftoneDegree = 0x500C,
    PropertyTagHalftoneShape = 0x500D,
    PropertyTagHalftoneMisc = 0x500E,
    PropertyTagHalftoneScreen = 0x500F,
    PropertyTagJPEGQuality = 0x5010,
    PropertyTagGridSize = 0x5011,
    PropertyTagThumbnailFormat = 0x5012, // 1 = JPEG, 0 = RAW RGB
    PropertyTagThumbnailWidth = 0x5013,
    PropertyTagThumbnailHeight = 0x5014,
    PropertyTagThumbnailColorDepth = 0x5015,
    PropertyTagThumbnailPlanes = 0x5016,
    PropertyTagThumbnailRawBytes = 0x5017,
    PropertyTagThumbnailSize = 0x5018,
    PropertyTagThumbnailCompressedSize = 0x5019,
    PropertyTagColorTransferFunction = 0x501A,
    PropertyTagThumbnailData = 0x501B, // RAW thumbnail bits in
                                       // JPEG format or RGB format
                                       // depends on
                                       // PropertyTagThumbnailFormat

    // Thumbnail related TAGs

    PropertyTagThumbnailImageWidth = 0x5020,  // Thumbnail width
    PropertyTagThumbnailImageHeight = 0x5021,  // Thumbnail height
    PropertyTagThumbnailBitsPerSample = 0x5022,  // Number of bits per component
    PropertyTagThumbnailCompression = 0x5023,  // Compression Scheme
    PropertyTagThumbnailPhotometricInterp = 0x5024, // Pixel composition
    PropertyTagThumbnailImageDescription = 0x5025,  // Image Tile
    PropertyTagThumbnailEquipMake = 0x5026,  // Manufacturer of Image Input equipment
    PropertyTagThumbnailEquipModel = 0x5027,  // Model of Image input equipment
    PropertyTagThumbnailStripOffsets = 0x5028,  // Image data location
    PropertyTagThumbnailOrientation = 0x5029,  // Orientation of image
    PropertyTagThumbnailSamplesPerPixel = 0x502A,  // Number of components
    PropertyTagThumbnailRowsPerStrip = 0x502B,  // Number of rows per strip
    PropertyTagThumbnailStripBytesCount = 0x502C,  // Bytes per compressed strip
    PropertyTagThumbnailResolutionX = 0x502D,  // Resolution in width direction
    PropertyTagThumbnailResolutionY = 0x502E,  // Resolution in height direction
    PropertyTagThumbnailPlanarConfig = 0x502F,  // Image data arrangement
    PropertyTagThumbnailResolutionUnit = 0x5030,  // Unit of X and Y Resolution
    PropertyTagThumbnailTransferFunction = 0x5031,  // Transfer function
    PropertyTagThumbnailSoftwareUsed = 0x5032,  // Software used
    PropertyTagThumbnailDateTime = 0x5033,  // File change date and time
    PropertyTagThumbnailArtist = 0x5034,  // Person who created the image
    PropertyTagThumbnailWhitePoint = 0x5035,  // White point chromaticity
    PropertyTagThumbnailPrimaryChromaticities = 0x5036,   // Chromaticities of primaries
    PropertyTagThumbnailYCbCrCoefficients = 0x5037, // Color space transforma-tion coefficients
    PropertyTagThumbnailYCbCrSubsampling = 0x5038,  // Subsampling ratio of Y to C
    PropertyTagThumbnailYCbCrPositioning = 0x5039,  // Y and C position
    PropertyTagThumbnailRefBlackWhite = 0x503A,  // Pair of black and white reference values
    PropertyTagThumbnailCopyRight = 0x503B,  // CopyRight holder
    PropertyTagLuminanceTable = 0x5090,
    PropertyTagChrominanceTable = 0x5091,

    PropertyTagFrameDelay = 0x5100,
    PropertyTagLoopCount = 0x5101,

    PropertyTagPixelUnit = 0x5110,  // Unit specifier for pixel/unit
    PropertyTagPixelPerUnitX = 0x5111,  // Pixels per unit in X
    PropertyTagPixelPerUnitY = 0x5112,  // Pixels per unit in Y
    PropertyTagPaletteHistogram = 0x5113,  // Palette histogram
                                           // EXIF specific tag
    PropertyTagExifExposureTime = 0x829A,
    PropertyTagExifFNumber = 0x829D,

    PropertyTagExifExposureProg = 0x8822,
    PropertyTagExifSpectralSense = 0x8824,
    PropertyTagExifISOSpeed = 0x8827,
    PropertyTagExifOECF = 0x8828,

    PropertyTagExifVer = 0x9000,
    PropertyTagExifDTOrig = 0x9003, // Date & time of original
    PropertyTagExifDTDigitized = 0x9004, // Date & time of digital data generation

    PropertyTagExifCompConfig = 0x9101,
    PropertyTagExifCompBPP = 0x9102,

    PropertyTagExifShutterSpeed = 0x9201,
    PropertyTagExifAperture = 0x9202,
    PropertyTagExifBrightness = 0x9203,
    PropertyTagExifExposureBias = 0x9204,
    PropertyTagExifMaxAperture = 0x9205,
    PropertyTagExifSubjectDist = 0x9206,
    PropertyTagExifMeteringMode = 0x9207,
    PropertyTagExifLightSource = 0x9208,
    PropertyTagExifFlash = 0x9209,
    PropertyTagExifFocalLength = 0x920A,
    PropertyTagExifMakerNote = 0x927C,
    PropertyTagExifUserComment = 0x9286,
    PropertyTagExifDTSubsec = 0x9290,  // Date & Time subseconds
    PropertyTagExifDTOrigSS = 0x9291,  // Date & Time original subseconds
    PropertyTagExifDTDigSS = 0x9292,  // Date & TIme digitized subseconds

    PropertyTagExifFPXVer = 0xA000,
    PropertyTagExifColorSpace = 0xA001,
    PropertyTagExifPixXDim = 0xA002,
    PropertyTagExifPixYDim = 0xA003,
    PropertyTagExifRelatedWav = 0xA004,  // related sound file
    PropertyTagExifInterop = 0xA005,
    PropertyTagExifFlashEnergy = 0xA20B,
    PropertyTagExifSpatialFR = 0xA20C,  // Spatial Frequency Response
    PropertyTagExifFocalXRes = 0xA20E,  // Focal Plane X Resolution
    PropertyTagExifFocalYRes = 0xA20F,  // Focal Plane Y Resolution
    PropertyTagExifFocalResUnit = 0xA210,  // Focal Plane Resolution Unit
    PropertyTagExifSubjectLoc = 0xA214,
    PropertyTagExifExposureIndex = 0xA215,
    PropertyTagExifSensingMethod = 0xA217,
    PropertyTagExifFileSource = 0xA300,
    PropertyTagExifSceneType = 0xA301,
    PropertyTagExifCfaPattern = 0xA302,

    PropertyTagGpsVer = 0x0000,
    PropertyTagGpsLatitudeRef = 0x0001,
    PropertyTagGpsLatitude = 0x0002,
    PropertyTagGpsLongitudeRef = 0x0003,
    PropertyTagGpsLongitude = 0x0004,
    PropertyTagGpsAltitudeRef = 0x0005,
    PropertyTagGpsAltitude = 0x0006,
    PropertyTagGpsGpsTime = 0x0007,
    PropertyTagGpsGpsSatellites = 0x0008,
    PropertyTagGpsGpsStatus = 0x0009,
    PropertyTagGpsGpsMeasureMode = 0x000A,
    PropertyTagGpsGpsDop = 0x000B,  // Measurement precision
    PropertyTagGpsSpeedRef = 0x000C,
    PropertyTagGpsSpeed = 0x000D,
    PropertyTagGpsTrackRef = 0x000E,
    PropertyTagGpsTrack = 0x000F,
    PropertyTagGpsImgDirRef = 0x0010,
    PropertyTagGpsImgDir = 0x0011,
    PropertyTagGpsMapDatum = 0x0012,
    PropertyTagGpsDestLatRef = 0x0013,
    PropertyTagGpsDestLat = 0x0014,
    PropertyTagGpsDestLongRef = 0x0015,
    PropertyTagGpsDestLong = 0x0016,
    PropertyTagGpsDestBearRef = 0x0017,
    PropertyTagGpsDestBear = 0x0018,
    PropertyTagGpsDestDistRef = 0x0019,
    PropertyTagGpsDestDist = 0x001A,
  }


}
