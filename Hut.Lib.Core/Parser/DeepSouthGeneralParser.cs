/******************************************************************************
 * Deep South TET
 * 
 * - General File Parser Interface
 * 
 * Author : Youngsoo Ryu
 * Version: 1.0.2
 * Update : 2015-06-19
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;



namespace DeepSouthData
{
    public abstract class DeepSouthGeneralParser
    {
        protected Object objects;

        public const string ASTEROID_PROVISIONAL_DESIGNATION_REGEX = @"[0-9A-Z]{1}[0-9]{3}[\s]*[A-Z][A-Z\-]*[A-Z0-9][0-9]*"; // 1948 OA

        public DeepSouthGeneralParser(string filename = null)
        {
            objects = new Object();

            if (filename != null)
                readFile(filename);
        }

        public abstract void readFile(string filename);

        public void Clear()
        {
            ((List<Object>)objects).Clear();
        }

        public int getNumberOfObjects()
        {
            return ((List<Object>)objects).Count;
        }

        public Object[] getObjectArray()
        {
            return ((List<Object>)objects).ToArray();
        }

        // check regular expression
        protected string findExpression(string token, string regulerexpression)
        {
            string result = null;
            Regex regex = new Regex(regulerexpression);
            Match match = regex.Match(token);

            if (match.Success)
                result = match.Value;

            return result;
        }

        protected string removeSpaces(string token, string replace = @" ")
        {
            return Regex.Replace(token, @"\s+", replace);
        }

        protected string[] splitTextBySpaces(string text )
        {
            return removeSpaces(text).Split(' ');
        }

        public static string convertFormattedHMS( string time )
        {
            string hour = time.Substring(0, 2);
            string minute = time.Substring(2, 2);
            string second = time.Substring(4, 2);

            return convertFormattedHMS(hour, minute, second);
        }

        public static string convertFormattedYMD( string date )
        {
            string year = date.Substring(0, 4);
            string month = date.Substring(4, 2);
            string day = date.Substring(6, 2);

            return convertFormattedYMD(year, month, day);
        }

        public static string convertFormattedHMS(string hour, string minute, string second)
        {
            return convertDateTimeFormat(hour, minute, second, @":");
        }

        public static string convertFormattedYMD( string year, string month, string day )
        {
            return convertDateTimeFormat(year, month, day, @"-");
        }

        protected static string convertDateTimeFormat( string s1, string s2, string s3, string connector )
        {
            return (s1 + connector + s2 + connector + s3);
        }

        //public static void writeText(string filename, string text)
        //{
        //    File.WriteAllText(filename, text);
        //}
    }
}