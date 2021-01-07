using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using Npgsql;
using System.Web;
using System.Data.SqlClient;

namespace RefBOT.Controllers
{
    public static class AutoStructReferencesWCSearch
    {
        public static bool IsPublisherNameInWC(string sPublisherName)
        {
            bool bReturnValue = false;
            try
            {
                string sPubliserSearchURL = string.Empty;
                string WebPageSource_InitailSearch = string.Empty;

                sPublisherName = PublisherNameCleanup4Original(sPublisherName);

                if (!string.IsNullOrEmpty(sPublisherName))
                {
                    //http://www.worldcat.org/search?q=pb%3ANASA+Technical+Memorandum&qt=owc_search
                    sPubliserSearchURL = string.Format("http://www.worldcat.org/search?q=pb%3A{0}&qt=owc_search", System.Web.HttpUtility.UrlEncode(sPublisherName));
                }
                else
                {
                    return false;
                }

                WebPageSource_InitailSearch = ReadWebpageSource(sPubliserSearchURL);
                List<Tuple<string, string, string, string, string, string>> lstSearchData1 = new List<Tuple<string, string, string, string, string, string>>();
                GetResultPageInformation(WebPageSource_InitailSearch, ref lstSearchData1);

                string sRisUrl = string.Empty;
                string sOCLC_Number = string.Empty;
                string sTitleLink = string.Empty;
                string sRisFileUrl = string.Empty;
                string sRisFilePath = string.Empty;
                string sRisFileContent = string.Empty;

                string sQuery = string.Empty;
                SqlCommand cmd = new SqlCommand();
                DataTable dtResult = new DataTable();

                foreach (var eachItem in lstSearchData1)
                {
                    sOCLC_Number = eachItem.Item1.ToString();
                    sTitleLink = eachItem.Item2.ToString();

                    if (!string.IsNullOrEmpty(sOCLC_Number) && !string.IsNullOrEmpty(sTitleLink))
                    {
                        sRisFileUrl = GetRisDownloadURL(sOCLC_Number, sTitleLink);
                        sRisFileContent = ReadRisFile(sRisFileUrl);
                        int RowCount = 0;
                        sQuery = string.Empty;
                        dtResult = new DataTable();

                        if (!string.IsNullOrEmpty(sRisFileContent))
                        {
                            cmd = new SqlCommand();
                            sQuery = string.Format("select count(*) from data_dict_publisher_learn");
                            //sQuery = string.Format("select max(id) from data_dict_publisher_learn");
                            Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                            dtResult = General.GeneralInstance.dataTable;
                            RowCount = Convert.ToInt32(dtResult.Rows[0][0].ToString());

                            string sOriginalPubNameInRis = GetPublisherNameFromRis(sRisFileContent);

                            if (string.IsNullOrEmpty(sOriginalPubNameInRis))
                            {
                                continue;
                            }

                            string sOriginalPubNameInRis_Cleaned = General.DatabaseIndexCleanup(sOriginalPubNameInRis).ToLower();

                            cmd = new SqlCommand();
                            sQuery = string.Format(string.Format("select * from data_dict_publisher_learn where data1 = '{0}'", Cleanup4QueryContent(sOriginalPubNameInRis)));
                            Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                            dtResult = General.GeneralInstance.dataTable;

                            string sDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string sUserName = "eBot-Automatic";

                            if (dtResult.Rows.Count <= 0)
                            {
                                sQuery = string.Format("insert into data_dict_publisher_learn(id, index1, data1, source, created_by, created_date)  values({0}, '{1}', '{2}', 'WorldCat', '{3}', '{4}')", RowCount + 1,
                                    sOriginalPubNameInRis_Cleaned, Cleanup4QueryContent(sOriginalPubNameInRis), sUserName, sDateTime);
                                Database.GetInstance.ExecuteNonQueryFunction(sQuery);
                            }
                        }
                    }
                }

                //Again Local DB Search
                //string sPublisherNameCleaned = PublisherNameCleanup4DBSearch(sPublisherName);
                string sPublisherNameCleaned = General.DatabaseIndexCleanup(sPublisherName).ToLower();

                cmd = new SqlCommand();
                dtResult = new DataTable();

                sQuery = string.Format("select distinct(index1) from data_dict_publisher_learn where index1 = '{0}' and created_by = 'eBot-Automatic'", sPublisherNameCleaned.ToLower());
                Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                dtResult = General.GeneralInstance.dataTable;

                if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesWCSearch.cs\IsPublisherNameInWC", ex.Message, true, "");
            }
            return bReturnValue;
        }

        static string Cleanup4QueryContent(string sContent)
        {
            try
            {
                sContent = Regex.Replace(sContent, "[']", "''");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesWCSearch.cs\Cleanup4QueryContent", ex.Message, true, "");
            }
            return sContent;
        }


        static string PublisherNameCleanup4Original(string sPublisherName)
        {
            try
            {
                sPublisherName = Regex.Replace(sPublisherName, @"[\:\.;, ]+$", "");
                NormalizeSpaces(ref sPublisherName);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesWCSearch.cs\PublisherNameCleanup4Original", ex.Message, true, "");
            }
            return sPublisherName;
        }

        static string PublisherNameCleanup4DBSearch(string sPublisherName)
        {
            try
            {
                sPublisherName = Regex.Replace(sPublisherName, "</?[^<>]+>", "");
                sPublisherName = Regex.Replace(sPublisherName, @"[:,;\(\)\[\]\$=""\'\?\!\/\u201C\u201D\u2019\.]", "");
                sPublisherName = Regex.Replace(sPublisherName, @"[\-]", " ");
                sPublisherName = Regex.Replace(sPublisherName, @"\b(?:of|the|an|a|the|in|and|to|for|on)\b", "", RegexOptions.IgnoreCase);
                sPublisherName = Regex.Replace(sPublisherName, @" & ", " ", RegexOptions.IgnoreCase);
                NormalizeSpaces(ref sPublisherName);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesWCSearch.cs\PublisherNameCleanup4DBSearch", ex.Message, true, "");
            }
            return sPublisherName;
        }

        static bool NormalizeSpaces(ref string sRefContent)
        {
            try
            {
                do
                {
                    sRefContent = Regex.Replace(sRefContent, "(<[^/][^<>]+>)[ ]+", " $1");
                    sRefContent = Regex.Replace(sRefContent, "[ ]+(</[^<>]+>)", "$1 ");
                } while (Regex.IsMatch(sRefContent, "(?:(?:(<[^/][^<>]+>)[ ]+)|(?:[ ]+(</[^<>]+>)))"));

                do
                {
                    sRefContent = Regex.Replace(sRefContent, "[ ]{2,}", " ");
                } while (Regex.IsMatch(sRefContent, "[ ]{2,}"));

                sRefContent = Regex.Replace(sRefContent, "<skip_space/>", " ");
                sRefContent = sRefContent.Trim();
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesWCSearch.cs\NormalizeSpaces", ex.Message, true, "");
            }
            return true;
        }

        static string GetPublisherNameFromRis(string sRisFileContent)
        {
            string sPublisherInRis = string.Empty;

            try
            {
                if (Regex.IsMatch(sRisFileContent, @"^(?:PB[ \t]+[\-][ \t]+)(.+)$", RegexOptions.Multiline))
                {
                    Match mPubName = Regex.Match(sRisFileContent, @"^(?:PB[ \t]+[\-][ \t]+)(.+)$", RegexOptions.Multiline);
                    if (mPubName != null)
                    {
                        sPublisherInRis = mPubName.Groups[1].Value;
                        sPublisherInRis = PublisherNameCleanup4Original(sPublisherInRis);
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesWCSearch.cs\GetPublisherNameFromRis", ex.Message, true, "");
            }
            return sPublisherInRis;
        }

        static string GetUrl(string sTitle, string sAuthors, string sPubYear)
        {
            string sURL = string.Empty;

            try
            {
                //http://www.worldcat.org/search?q=ti%3AFIDIC+users+guide%3A+A+practical+guide+to+the+1999+red+and+yellow+books+au%3ATotterdill%2C+B.+W.&fq=yr%3A2006..2006+%3E&qt=advanced&dblist=638

                if (!string.IsNullOrEmpty(sTitle) && !string.IsNullOrEmpty(sAuthors) && !string.IsNullOrEmpty(sPubYear))
                {
                    sURL = string.Format("http://www.worldcat.org/search?q=ti%3A{0}+au%3A{1}&fq=yr%3A{2}..{2}+%3E&qt=advanced&dblist=638", System.Web.HttpUtility.UrlEncode(sTitle), System.Web.HttpUtility.UrlEncode(sAuthors), System.Web.HttpUtility.UrlEncode(sPubYear));
                }
                //http://www.worldcat.org/search?q=ti%3AFIDIC+users+guide%3A+A+practical+guide+to+the+1999+red+and+yellow+books&fq=yr%3A2013..2013+%3E&qt=advanced&dblist=638
                else if (!string.IsNullOrEmpty(sTitle) && string.IsNullOrEmpty(sAuthors) && !string.IsNullOrEmpty(sPubYear))
                {
                    sURL = string.Format("http://www.worldcat.org/search?q=ti%3A{0}&fq=yr%3A{1}..{1}+%3E&qt=advanced&dblist=638", System.Web.HttpUtility.UrlEncode(sTitle), System.Web.HttpUtility.UrlEncode(sPubYear));
                }
                ////http://www.worldcat.org/search?q=ti%3AFIDIC+users+guide%3A+A+practical+guide+to+the+1999+red+and+yellow+books&qt=advanced&dblist=638
                else if (!string.IsNullOrEmpty(sTitle) && string.IsNullOrEmpty(sAuthors) && string.IsNullOrEmpty(sPubYear))
                {
                    sURL = string.Format("http://www.worldcat.org/search?q=ti%3A{0}&qt=advanced&dblist=638", System.Web.HttpUtility.UrlEncode(sTitle));
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesWCSearch.cs\GetUrl", ex.Message, true, "");
            }

            return sURL;
        }
        static string GetRisDownloadURL(string sOCLC_Number, string sTitleLink)
        {
            string sRisDownloadURL = string.Empty;
            string sExactTitle4Link = string.Empty;
            try
            {

                if (Regex.IsMatch(sTitleLink, "((?<=(?:/title/))(?:[^/]+)(?=/))"))
                {
                    sExactTitle4Link = Regex.Match(sTitleLink, "((?<=(?:/title/))(?:[^/]+)(?=/))").Value.ToString();

                    //http://www.worldcat.org/title/chapter-3-monolithic-silicon-microbolometer-arrays/oclc/7327839871&referer=brief_results/oclc/7327839871?page=endnotealt&client=worldcat.org-detailed_record
                    sRisDownloadURL = string.Format("http://www.worldcat.org/title/{0}/oclc/{1}&referer=brief_results/oclc/{1}?page=endnotealt&client=worldcat.org-detailed_record", sExactTitle4Link, sOCLC_Number);
                }

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesWCSearch.cs\GetRisDownloadURL", ex.Message, true, "");
            }
            return sRisDownloadURL;
        }
        static string ReadRisFile(string sUrl)
        {
            string sRisContent = string.Empty;
            try
            {
                //string urlTest = @"http://www.worldcat.org/title/engineering-for-a-changing-world-59th-iwk-ilmenau-scientific-colloquium-technische-universitat-ilmenau-september-11-15-2017-programme/oclc/1003298779&referer=brief_results/oclc/1003298779?page=endnotealt&client=worldcat.org-detailed_record";

                WebClient myWebClient = new WebClient();
                myWebClient.Encoding = System.Text.Encoding.UTF8;
                myWebClient.Headers.Add("user-agent", "Chrome/66.0.3359.181)"); // If you need to 
                myWebClient.Proxy.Credentials = new System.Net.NetworkCredential("dakshinamoorthy.g", "xxxxxxxxx");
                string sTempFileName = System.IO.Path.GetTempFileName();
                sTempFileName = Path.ChangeExtension(sTempFileName, "html");
                myWebClient.DownloadFile(sUrl, sTempFileName);
                sRisContent = File.ReadAllText(sTempFileName, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesWCSearch.cs\ReadRisFile", ex.Message, true, "");
            }
            return sRisContent;
        }

        static string ReadWebpageSource(string url)
        {
            string download = string.Empty;

            try
            {
                WebClient myWebClient = new WebClient();
                //myWebClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)"); // If you need to 
                myWebClient.Headers.Add("user-agent", "Chrome/66.0.3359.181)"); // If you need to 
                myWebClient.Encoding = System.Text.Encoding.UTF8;
                myWebClient.Proxy.Credentials = new System.Net.NetworkCredential("dakshinamoorthy.g", "Subha%2018");

                //simulate a specific browser
                byte[] myDataBuffer = myWebClient.DownloadData(url);
                download = Encoding.ASCII.GetString(myDataBuffer);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesWCSearch.cs\ReadWebpageSource", ex.Message, true, "");
            }
            return download;
        }

        static bool GetResultPageInformation(string sWebpageSource, ref List<Tuple<string, string, string, string, string, string>> lstSearchData)
        {
            string sResultInfo = string.Empty;
            string sOCLC_Number = string.Empty;
            string sTitleLinkValue = string.Empty;
            string sTitleValue = string.Empty;
            string sAuthors = string.Empty;
            string sLanguage = string.Empty;
            string sPublisher = string.Empty;

            sWebpageSource = Regex.Replace(sWebpageSource, "[ ]{2,}", " ");

            try
            {
                MatchCollection mcResulTable = Regex.Matches(sWebpageSource, "<table [^<>]* class=\"table-results\"[^<>]*>(.+)</table>", RegexOptions.Singleline);
                if (mcResulTable.Count > 0)
                {
                    string sTableContent = mcResulTable[0].Value.ToString();
                    MatchCollection mcResulRow = Regex.Matches(sTableContent, "<tr class=\"menuElem\">(?:(?!</tr>).)+</tr>", RegexOptions.Singleline);

                    if (mcResulRow.Count > 0)
                    {
                        string sRowContent = string.Empty;
                        foreach (Match trMatch in mcResulRow)
                        {
                            sRowContent = trMatch.Value.ToString();
                            MatchCollection mcResultCell = Regex.Matches(sRowContent, "<td class=\"result details\">(?:(?!</td>).)+</td>", RegexOptions.Singleline);

                            if (mcResultCell.Count > 0)
                            {
                                string sCellContent = string.Empty;
                                foreach (Match tdMatch in mcResultCell)
                                {
                                    sOCLC_Number = sTitleLinkValue = sTitleValue = sAuthors = sLanguage = sPublisher = string.Empty;
                                    sCellContent = tdMatch.Value.ToString();

                                    if (Regex.IsMatch(sCellContent, "<div class=\"oclc_number\">((?:(?!</div>).)+)</div>", RegexOptions.Singleline))
                                    {
                                        sOCLC_Number = Regex.Match(sCellContent, "<div class=\"oclc_number\">((?:(?!</div>).)+)</div>", RegexOptions.Singleline).Groups[1].Value.ToString();
                                    }

                                    if (Regex.IsMatch(sCellContent, "<div class=\"name\">((?:(?!</div>).)+)</div>", RegexOptions.Singleline))
                                    {
                                        string sNameContent = Regex.Match(sCellContent, "<div class=\"name\">((?:(?!</div>).)+)</div>", RegexOptions.Singleline).Groups[1].Value.ToString();

                                        if (Regex.IsMatch(sCellContent, "<a id=\"[^\"]*\" href=\"([^\"]+)\">((?:(?!</a>).)+)</a>", RegexOptions.Singleline))
                                        {
                                            sTitleLinkValue = Regex.Match(sCellContent, "<a id=\"[^\"]*\" href=\"([^\"]+)\">((?:(?!</a>).)+)</a>", RegexOptions.Singleline).Groups[1].Value.ToString();
                                            sTitleValue = Regex.Match(sCellContent, "<a id=\"[^\"]*\" href=\"([^\"]+)\">((?:(?!</a>).)+)</a>", RegexOptions.Singleline).Groups[2].Value.ToString();
                                        }
                                    }

                                    if (Regex.IsMatch(sCellContent, "<div class=\"author\">((?:(?!</div>).)+)</div>", RegexOptions.Singleline))
                                    {
                                        sAuthors = Regex.Match(sCellContent, "<div class=\"author\">((?:(?!</div>).)+)</div>", RegexOptions.Singleline).Groups[1].Value.ToString();
                                    }

                                    if (Regex.IsMatch(sCellContent, "<div class=\"type language\">((?:(?!</div>).)+)</div>", RegexOptions.Singleline))
                                    {
                                        sLanguage = Regex.Match(sCellContent, "<div class=\"type language\">((?:(?!</div>).)+)</div>", RegexOptions.Singleline).Groups[1].Value.ToString();
                                    }

                                    if (Regex.IsMatch(sCellContent, "<div class=\"publisher\">((?:(?!</div>).)+)</div>", RegexOptions.Singleline))
                                    {
                                        sPublisher = Regex.Match(sCellContent, "<div class=\"publisher\">((?:(?!</div>).)+)</div>", RegexOptions.Singleline).Groups[1].Value.ToString();
                                    }

                                    lstSearchData.Add(
                                new Tuple<string, string, string, string, string, string>(
                                    sOCLC_Number,
                                    sTitleLinkValue,
                                    sTitleValue,
                                    sAuthors,
                                    sLanguage,
                                    sPublisher));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesWCSearch.cs\GetResultPageInformation", ex.Message, true, "");
            }
            return true;
        }

        static string Cleanup4ReultContent(string sContent)
        {
            try
            {
                sContent = Regex.Replace(sContent, "<[^<>]+>", "");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesWCSearch.cs\GetResultPageInformation", ex.Message, true, "");
            }
            return sContent;
        }

    }
}
