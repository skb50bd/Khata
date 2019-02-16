using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedLibrary
{
    public static class StringExtensions
    {
        public static DateTime ParseDate(this string date) =>
            DateTime.ParseExact(date, @"dd/MM/yyyy", CultureInfo.InvariantCulture.DateTimeFormat);

        public static string FromPascalCase(this string str)
        {
            List<string> words = new List<string>();

            var word = new StringBuilder();
            foreach (char c in str)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    words.Add(word.ToString());
                    word = new StringBuilder();
                }
                word.Append(c);
            }

            words.Add(word.ToString());

            var final = new StringBuilder();
            foreach (string s in words)
                final.Append(s)
                     .Append(" ");

            return final.ToString().Trim();
        }

        public static string ToPascalCase(this string str)
        {
            var words = str.Split();
            var output = new StringBuilder();

            var textInfo = new CultureInfo("en-US", false).TextInfo;

            foreach (var word in words)
                output.Append(textInfo.ToTitleCase(word));

            return output.ToString();
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);

            return result;
        }

        /// <summary>
        /// Given a Camel Case string, it will return the string in plain spaced fashion
        /// </summary>
        /// <param name="camelCase">The input camelCase string</param>
        /// <returns></returns>
        public static string ToFriendlyCase(this string camelCase) =>
            Regex.Replace( // Remove Extra Spaces
                Regex.Replace( // Split The Camel Case Expression
                    camelCase,
                    "(?!^)([A-Z])",
                    " $1"),
                "/ +/ g",
                " ");

        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static (string, string, string, string) DeconstructName(this string line)
        {
            string firstName = string.Empty;
            string middleName = string.Empty;
            string lastName = string.Empty;
            string nickName = string.Empty;

            line = line.Trim();
            string[] words = line.Split();

            switch (words.Length)
            {
                case 0:
                    {
                        break;
                    }

                case 1:
                    {
                        firstName = line;

                        break;
                    }
                case 2:
                    {
                        (firstName, lastName) = (words[0], words[1]);

                        break;
                    }
                case 3 when words[2].Contains("("):
                    {
                        (firstName, lastName, nickName) =
                        (words[0], words[1], words[2].Replace(",", "").Replace(")", ""));

                        break;
                    }
                case 3:
                    {
                        (firstName, middleName, lastName) = (words[0], words[1], words[2]);

                        break;
                    }
                case 4:
                    {
                        (firstName, middleName, lastName, nickName) =
                        (words[0], words[1], words[2], words[3].Replace(",", "").Replace(")", ""));

                        break;
                    }
                default:
                    {
                        firstName = string.Join(" ", words.SubArray(0, 2));
                        lastName = string.Join(" ", words[2], words.Length - 2);

                        break;
                    }
            }

            return (firstName, middleName, lastName, nickName);
        }
    }
}
