using System;
using System.Drawing.Imaging;
using System.Text;


namespace WF.SDK.Common.Imaging
{
	/// <summary>
	/// Summary description for ImageProperty.
	/// </summary>
	public class ImageProperty
	{
		private PropertyItem _prop = null;
		private bool _valueParsed = false;
		private bool _valueParseErr = false;
		private object _val = null;
		private string _valueParseErrStr = "";

		internal ImageProperty(PropertyItem prop)
		{
			this._prop = prop;
			this.ParsePropValue();
		}

		public PropertyId PropertyId
		{
			get
			{
				return (PropertyId)this._prop.Id;
			}
		}

		public PropertyIdType PropertyIdType
		{
			get
			{
				return (PropertyIdType)this._prop.Type;
			}
		}

		public string Description
		{
			get
			{
				string s = "";
				s += "Property ID : " + this._prop.Id.ToString() + " (" + this.PropertyId.ToString() + ")\r\n";
				s += "Property Type : " + this._prop.Type.ToString() + " (" + this.PropertyIdType.ToString() + ")\r\n";
				s += "Property Length : " + this._prop.Len.ToString() + "\r\n";

				switch (this.PropertyIdType)
				{
					case PropertyIdType.PropertyTagTypeLong:
						{
							s += "Property Value(s): ";
							foreach (int i in this.PropertyValue_Long)
							{
								s += "<" + i.ToString() + ">";
							}
							s += " (Array of Uint32)\r\n";
							break;
						}
					case PropertyIdType.PropertyTagTypeShort:
						{
							s += "Property Value(s): ";
							foreach (int i in this.PropertyValue_Long)
							{
								s += "<" + i.ToString() + ">";
							}
							s += " (Array of Uint16)\r\n";
							break;
						}
					case PropertyIdType.PropertyTagTypeByte:
						{
							s += "Property Value(s): ";
							foreach (byte i in this.PropertyValue_Byte)
							{
								s += "<" + i.ToString() + ">";
							}
							s += " (Array of Byte)\r\n";
							break;
						}
					case PropertyIdType.PropertyTagTypeASCII:
						{
							s += "Property Value(s): ";
							string propertyValue = this.PropertyValue_String.Replace("\0", @"\0");
							s += "<" + propertyValue + ">";
							s += " (String)\r\n";
							break;
						}
					case PropertyIdType.PropertyTagTypeRational:
						{
							s += "Property Value(s): ";
							for (int i = 0; i < this.PropertyValue_Rational.Length; i = i + 2)
							{
								s += "<" + this.PropertyValue_Rational[i].ToString() + "/" + this.PropertyValue_Rational[i + 1].ToString() + ">";
							}
							s += " (Array of Pairs a/b)\r\n";
							break;
						}
				}
				return s;
			}
		}

		public bool ParseError
		{
			get
			{
				return this._valueParseErr;
			}
		}

		public string ParseErrorString
		{
			get
			{
				return this._valueParseErrStr;
			}
		}


		public object PropertyValue
		{
			get
			{
				return this._val;
			}
		}

		public int[] PropertyValue_Long
		{
			get
			{
				return (int[])this._val;
			}
		}

		public int[] PropertyValue_Short
		{
			get
			{
				return (int[])this._val;
			}
		}

		public byte[] PropertyValue_Byte
		{
			get
			{
				return (byte[])this._val;
			}
		}

		public string PropertyValue_String
		{
			get
			{
				return (string)this._val;
			}
		}

		public int[] PropertyValue_Rational
		{
			get
			{
				return (int[])this._val;
			}
		}

		private void ParsePropValue()
		{
			if (this._valueParsed) { return; }
			switch (this.PropertyIdType)
			{
				case PropertyIdType.PropertyTagTypeLong:
					{
						int remainder;
						int count = Math.DivRem(this._prop.Len, 4, out remainder);
						if (remainder == 0)
						{
							int[] result = new int[count];

							for (int i = 0; i < count; i++)
							{
								result[i] = (int)BitConverter.ToUInt32(this._prop.Value, i * 4);
							}
							this._val = result;
							this._valueParseErr = false;
							this._valueParseErrStr = "";
						}
						break;
					}
				case PropertyIdType.PropertyTagTypeShort:
					{
						int remainder;
						int count = Math.DivRem(this._prop.Len, 2, out remainder);
						if (remainder == 0)
						{
							int[] result = new int[count];

							for (int i = 0; i < count; i++)
							{
								result[i] = (int)BitConverter.ToUInt16(this._prop.Value, i * 2);
							}
							this._val = result;
							this._valueParseErr = false;
							this._valueParseErrStr = "";
						}
						break;
					}
				case PropertyIdType.PropertyTagTypeByte:
					{
						this._val = this._prop.Value;
						this._valueParseErr = false;
						this._valueParseErrStr = "";
						break;
					}
				case PropertyIdType.PropertyTagTypeASCII:
					{
						this._val = Encoding.ASCII.GetString(this._prop.Value);
						this._valueParseErr = false;
						this._valueParseErrStr = "";
						break;
					}
				case PropertyIdType.PropertyTagTypeRational:
					{
						int remainder;
						int count = Math.DivRem(this._prop.Len, 4, out remainder);
						if (remainder == 0)
						{
							int[] result = new int[count];

							for (int i = 0; i < count; i++)
							{
								result[i] = (int)BitConverter.ToUInt32(this._prop.Value, i * 4);
							}
							this._val = result;
							this._valueParseErr = false;
							this._valueParseErrStr = "";
						}
						break;
					}
			}
			this._valueParsed = true;
		}

	}
}
