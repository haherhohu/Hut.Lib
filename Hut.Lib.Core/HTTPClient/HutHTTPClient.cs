/******************************************************************************
 * Hut HTTP Client
 *
 * - HTTP Client for download web source
 * - change httpagilitypack to system base
 *
 * Author : Daegung Kim
 * Version: 1.0.3
 * Update : 2020-04-27
 ******************************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace Hut
{
    public class HutHTTPClient
    {
        private HttpWebRequest request;
        private HttpWebResponse response;

        private void get(string address)
        {
            // create web request
            request = (HttpWebRequest)WebRequest.Create(address);

            // get response
            response = (HttpWebResponse)request.GetResponse();
        }

        // get response string
        private string getResponseString()
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            try
            {
                return reader.ReadToEnd();
            }
            catch (WebException e)
            {
                //if (e.Status == WebExceptionStatus.Timeout)
                //    DeepSouthLogger.Instance.logE(@"connection timeout: " + e.Message);

                return null;
            }
        }

        public bool getMonthlyTET(string url, string filepath)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(url, filepath);
                    return true;
                }
                catch (WebException e)
                {
                    //                    DeepSouthLogger.Instance.logE(@"download error: " + e.Message);
                    return false;
                }
            }
        }

        // get daily mpc tet
        public string getMPCTET(string address, string target, string year, string month, string day, string numberoflist, string interval, string unit, string observationcode)
        {
            string fullpath = address + @"/cgi-bin/mpeph2.cgi?" + @"ty=e";
            string query = @"";

            // set variables
            if (target != null)
                query = @"&TextArea=" + target;
            if (year != null && month != null && day != null)
                query += @"&d=" + year + @"-" + month + @"-" + day;
            if (numberoflist != null)
                query += @"&l=" + numberoflist;
            if (interval != null)
                query += @"&i=" + interval;
            if (unit != null)
                query += @"&u=" + unit + @"&uto=0";
            if (observationcode != null)
                query += @"&c=" + observationcode;

            // the others
            query += @"&long=&lat=&alt=&raty=a&s=t&m=m&adir=S&oed=&e=0&resoc=&tit=&bu=&ch=c&ce=f&js=f";

            this.get(fullpath + query);

            return this.getResponseString();
        }

        // get mpc PDNumber Text
        public string getMPCPDText(string address, string target)
        {
            string fullpath = address + @"/cgi-bin/pdes.cgi?";
            string query = @"";

            if (target != null)
                query = @"pm=" + target;

            this.get(fullpath + query);

            // tagged with javascript
            //            HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(WebUtility.HtmlDecode(this.getResponseString()));
            //HtmlNodeCollection center = doc.DocumentNode.SelectNodes("//center");

            //if (center.Count == 1)
            //    return center[0].InnerText;
            //else
            return null;
        }

        // TODO: 정리
        public string getJPLPDText(string address, string target)
        {
            string fullpath = address + @"/sbdb.cgi?";
            string query = @"";
            string result = string.Empty;

            string search = @"Search:";
            string alternatedesigations = @"Alternate Designations";

            List<string> linelist = new List<string>();

            if (target != null)
                query = @"sstr=" + target;
            else
                return null;

            this.get(fullpath + query);

            string[] lines = removeTag(this.getResponseString()).Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string test = Regex.Replace(lines[i].Replace(@"&nbsp;", @" "), @"\s+", @" ");

                if (test.Length > 1)
                    linelist.Add(test);
            }

            lines = linelist.ToArray();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IndexOf(search) > -1 || lines[i].IndexOf(alternatedesigations) > -1)
                {
                    result = lines[i + 1].Split('=')[0];
                    break;
                }
            }

            lines = null;
            linelist.Clear();

            return result;
        }

        // tag remover
        public static string removeTag(string tagged)
        {
            string regex = @"\<[^\<\>]*\>"; // 정규표현식 선언
            string tagless = Regex.Replace(tagged, regex, string.Empty); // 정규표현식 적용된 문자열 선언

            // remove rogue leftovers
            tagless = tagless.Replace("<", string.Empty).Replace(">", string.Empty); // 정규표현식 문자열 적용
            return tagless; // 정규표현식 리턴
        }
    }
}