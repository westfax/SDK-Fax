using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WF.SDK.Common
{
  public enum CardType
  {
    [DisplayName("UnAssigned")]
    UnAssigned = 0,
    [DisplayName("Visa")]
    Visa = 1,
    [DisplayName("Master Card")]
    MasterCard = 2,
    [DisplayName("American Express")]
    AmericanExpress = 3,
    [DisplayName("Discover")]
    Discover = 4,
    [DisplayName("Diners Club")]
    DinersClub = 5,
  }

	public class CreditCardHelper
	{
		#region CardInfo Class to hold Regex instances

		private class CardInfo
		{
			public CardInfo(string regEx, int length, CardType type)
			{
				RegEx = regEx;
				Length = length;
				Type = type;
			}

			public string RegEx { get; set; }
			public int Length { get; set; }
			public CardType Type { get; set; }
		}

		private static CardInfo[] _cardInfos =
		{
		  new CardInfo("^(51|52|53|54|55)", 16, CardType.MasterCard),
		  new CardInfo("^(4)", 16, CardType.Visa),
		  new CardInfo("^(4)", 13, CardType.Visa),
		  new CardInfo("^(34|37)", 15, CardType.AmericanExpress),
		  new CardInfo("^(6011)", 16, CardType.Discover),
		  new CardInfo("^(300|301|302|303|304|305|36|38)", 14, CardType.DinersClub),
		};

		#endregion

		public static CardType Parse(string cardNumber)
		{
			try
			{

				foreach (CardInfo cardInfo in _cardInfos)
				{
					if (cardNumber.Length == cardInfo.Length && Regex.IsMatch(cardNumber, cardInfo.RegEx))
					{
						return cardInfo.Type;
					}
				}
			}
			catch { }
			return CardType.UnAssigned;
		}

		public static CardType? TryParse(string cardNumber)
		{
			foreach (CardInfo cardInfo in _cardInfos)
			{
				if (cardNumber.Length == cardInfo.Length && Regex.IsMatch(cardNumber, cardInfo.RegEx))
				{
					return cardInfo.Type;
				}
			}
			return null;
		}

		public static string Normalize(string cardNumber)
		{
			try
			{
				if (cardNumber == null || cardNumber.Trim().Length == 0) { return String.Empty; }

				StringBuilder sb = new StringBuilder();

				foreach (char c in cardNumber)
				{
					if (char.IsDigit(c)) { sb.Append(c); }
				}
				return sb.ToString();
			}
			catch { return String.Empty; }
		}

		public static bool IsValidNumber(string cardNumber)
		{
			try
			{
				int[] DELTAS = new int[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
				int checksum = 0;
				char[] chars = cardNumber.ToCharArray();
				for (int i = chars.Length - 1; i > -1; i--)
				{
					int j = ((int)chars[i]) - 48;
					checksum += j;
					if (((i - chars.Length) % 2) == 0) { checksum += DELTAS[j]; }
				}
				return ((checksum % 10) == 0);
			}
			catch { return false; }
		}
	}
}
