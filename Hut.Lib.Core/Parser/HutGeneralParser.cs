/******************************************************************************
 * Hut General Parser
 *
 * - General File Parser Interface
 *
 * Author : Youngsoo Ryu
 * Version: 1.0.2
 * Update : 2020-04-27
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Hut
{
    public abstract class HutGeneralParser
    {
        protected object objects;

        //public const string ASTEROID_PROVISIONAL_DESIGNATION_REGEX = @"[0-9A-Z]{1}[0-9]{3}[\s]*[A-Z][A-Z\-]*[A-Z0-9][0-9]*"; // 1948 OA

        public HutGeneralParser(string filename = null)
        {
            objects = new object();

            if (filename != null)
                Read(filename);
        }

        public abstract void Read(string filename);

        public void Clear()
        {
            ((List<object>)objects).Clear();
        }

        public int NumberOfObjects()
        {
            return ((List<object>)objects).Count;
        }

        public object[] ObjectArray()
        {
            return ((List<object>)objects).ToArray();
        }

        // check regular expression
        protected string FindExpression(string token, string regulerexpression)
        {
            string result = null;
            Regex regex = new Regex(regulerexpression);
            Match match = regex.Match(token);

            if (match.Success)
                result = match.Value;

            return result;
        }

        protected string RemoveSpaces(string token, string replace = @" ")
        {
            return Regex.Replace(token, @"\s+", replace);
        }

        protected string[] SplitTextBySpaces(string text)
        {
            return RemoveSpaces(text).Split(' ');
        }

        public static string ConvertFormattedHMS(string time)
        {
            string hour = time.Substring(0, 2);
            string minute = time.Substring(2, 2);
            string second = time.Substring(4, 2);

            return ConvertFormattedHMS(hour, minute, second);
        }

        public static string ConvertFormattedYMD(string date)
        {
            string year = date.Substring(0, 4);
            string month = date.Substring(4, 2);
            string day = date.Substring(6, 2);

            return ConvertFormattedYMD(year, month, day);
        }

        public static string ConvertFormattedHMS(string hour, string minute, string second)
        {
            return ConvertDateTimeFormat(hour, minute, second, @":");
        }

        public static string ConvertFormattedYMD(string year, string month, string day)
        {
            return ConvertDateTimeFormat(year, month, day, @"-");
        }

        protected static string ConvertDateTimeFormat(string s1, string s2, string s3, string connector)
        {
            return (s1 + connector + s2 + connector + s3);
        }

        //public static void writeText(string filename, string text)
        //{
        //    File.WriteAllText(filename, text);
        //}
    }
}