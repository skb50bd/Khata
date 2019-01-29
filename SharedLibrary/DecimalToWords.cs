using System;
using System.Globalization;

namespace SharedLibrary
{
    public static class DecimalToWords
    {
        #region Decimal to Words
        /// <summary>
        /// Converts decimal number to words (string)
        /// </summary>
        /// <param name="value">The decimal number</param>
        /// <param name="basicMonetaryUnit">Basic monetary unit to show.
        /// E.g. Dollars, Taka, Euro etc</param>
        /// <param name="fractionMonetaryUnit">1/100th fraction unit name of the basic monetary unit.
        /// E.g. Cents, Penny, Poisa etc.</param>
        /// <returns>In words representation of the decimal value</returns>
        public static string ToMoney(this decimal value,
            string basicMonetaryUnit,
            string fractionMonetaryUnit)
        {
            string decimals = String.Empty;
            string input = Math.Round(value, 2).ToString(CultureInfo.InvariantCulture);

            if (input.Contains("."))
            {
                decimals = input.Substring(input.IndexOf(".", StringComparison.Ordinal) + 1);

                // remove decimal part from input
                input = input.Remove(input.IndexOf(".", StringComparison.Ordinal));
            }

            // Convert input into words. save it into strWords
            string strWords = $"{GetWords(input)} {basicMonetaryUnit}";

            if (decimals.Length > 0)
            {
                // if there is any decimal part convert it to words and add it to strWords.
                strWords += $" and {GetWords(decimals)} {fractionMonetaryUnit}";
            }

            return strWords;
        }

        private static string GetWords(string input)
        {
            // these are seperators for each 3 digit in numbers. 
            // you can add more if you want convert beigger numbers.
            string[] seperators = { "", " Thousand ", " Million ", " Billion ", " Trillion " };

            // Counter is indexer for seperators.
            // each 3 digit converted this will count.
            int i = 0;

            string strWords = "";

            while (input.Length > 0)
            {
                // get the 3 last numbers from input and store it. 
                // if there is not 3 numbers just use take it.
                string threeDigits = input.Length < 3 ? input : input.Substring(input.Length - 3);

                // remove the 3 last digits from input. if there is not 3 numbers just remove it.
                input = input.Length < 3 ? "" : input.Remove(input.Length - 3);

                int no = Int32.Parse(threeDigits);

                // Convert 3 digit number into words.
                threeDigits = GetWord(no);

                // apply the seperator.
                threeDigits += seperators[i];

                // since we are getting numbers from right to left then we must append resault to strWords like this.
                strWords = threeDigits + strWords;

                // 3 digits converted. count and go for next 3 digits
                i++;
            }

            return strWords;
        }

        // method just to convert 3digit number into words.
        private static string GetWord(int no)
        {
            string[] ones = {
                "One",
                "Two",
                "Three",
                "Four",
                "Five",
                "Six",
                "Seven",
                "Eight",
                "Nine",
                "Ten",
                "Eleven",
                "Twelve",
                "Thirteen",
                "Fourteen",
                "Fifteen",
                "Sixteen",
                "Seventeen",
                "Eighteen",
                "Ninteen"
            };

            string[] tens = {
                "Ten",
                "Twenty",
                "Thirty",
                "Fourty",
                "Fifty",
                "Sixty",
                "Seventy",
                "Eighty",
                "Ninty"
            };

            string word = "";

            if (no > 99 && no < 1000)
            {
                int i = no / 100;
                word = word + ones[i - 1] + " Hundred ";
                no = no % 100;
            }

            if (no > 19 && no < 100)
            {
                int i = no / 10;
                word = word + tens[i - 1] + " ";
                no = no % 10;
            }

            if (no > 0 && no < 20)
            {
                word = word + ones[no - 1];
            }

            return word;
        }
        #endregion
    }
}
