using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Xml;

namespace RefBOT.Controllers
{
    public static class AutoStructRefAPI
    {
        #region For GROBID
        public static string sGROBID_API_BaseAddress = @"https://submissionchecker.luminad.com/grobid/api/processCitation";
        #endregion
        public static string sAnyStyleAPI_BaseAddress = @"https://luminadrefstruct.herokuapp.com/ref_struct/?q=";
        public static string sWebRequestResultContent = string.Empty;

        public static string gRefInputContent = string.Empty;
        public static string gRefOutputContent = string.Empty;

        #region For GROBID
        public static bool IdentifyRefElements_GROBID(string sRefInputContent, ref string sRefTaggedContent)
        {
            var dicParam = new Dictionary<string, string>();
            string sGROBID_Output = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(sRefInputContent))
                {
                    return false;
                }

                sRefInputContent = Regex.Replace(sRefInputContent, "</?(?:b|i|u|sup|sub|sc|ac)>", "");
                //added by Dakshinamoorthy on 2020-Dec-12
                sRefInputContent = HexUnicodeToCharConvertor(sRefInputContent);

                dicParam.Add("citations", sRefInputContent);
                sGROBID_Output = HttpPostRequest(sGROBID_API_BaseAddress, dicParam);
                sRefTaggedContent = GetTaggedRefContent(sGROBID_Output, sRefInputContent);

                //exclude some elements
                sRefTaggedContent = ExcludeRefElements(sRefTaggedContent);

                sRefTaggedContent = NormalizeTitleInformation(sRefTaggedContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\IdentifyRefElements_GROBID", ex.Message, true, "");
                return false;
            }
            return true;
        }

        private static string HexUnicodeToCharConvertor(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "&#x([A-F0-9]+);", Hex2Char, RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\HexUnicodeToCharConvertor", ex.Message, true, "");
            }
            return sRefContent;
        }

        private static string Hex2Char(Match myHexMatch)
        {
            string sHexValue = myHexMatch.Groups[1].Value.ToString();
            try
            {
                sHexValue = ((char)Convert.ToInt32(sHexValue, 16)).ToString();
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\Hex2Char", ex.Message, true, "");
            }
            return sHexValue;
        }


        private static string NormalizeTitleInformation(string sRefTaggedContent)
        {
            try
            {
                string sTitleFullPattern = @"(?:<(Article_Title|Journal_Title|otherTitle)>((?:(?!</\1>).)+)</\1>)";
                MatchCollection mcTitles = Regex.Matches(sRefTaggedContent, sTitleFullPattern);
                List<Tuple<string, string>> lstTitles = new List<Tuple<string, string>>();

                int nArticleTitleCount = 0;
                int nJournalTitleCount = 0;
                int nOtherTitleCount = 0;


                if (mcTitles != null && mcTitles.Count > 0)
                {
                    foreach (Match myTitleMatch in mcTitles)
                    {
                        lstTitles.Add(new Tuple<string, string>(myTitleMatch.Groups[1].Value, myTitleMatch.Groups[2].Value));
                    }

                    nArticleTitleCount = lstTitles.FindAll(t => t.Item1.Equals("Article_Title")).Count;
                    nJournalTitleCount = lstTitles.FindAll(t => t.Item1.Equals("Journal_Title")).Count;
                    nOtherTitleCount = lstTitles.FindAll(t => t.Item1.Equals("otherTitle")).Count;

                    if (nArticleTitleCount == 0 && nJournalTitleCount == 0 && nOtherTitleCount == 1)
                    {
                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<otherTitle>", "<Article_Title>");
                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</otherTitle>", "</Article_Title>");
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\NormalizeTitleInformation", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        private static string HttpPostRequest(string url, Dictionary<string, string> postParameters)
        {
            string pageContent = string.Empty;

            try
            {
                string postData = "";

                foreach (string key in postParameters.Keys)
                {
                    postData += HttpUtility.UrlEncode(key) + "="
                          + HttpUtility.UrlEncode(postParameters[key]) + "&";
                }

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                myHttpWebRequest.Method = "POST";

                byte[] data = Encoding.ASCII.GetBytes(postData);

                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream responseStream = myHttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
                pageContent = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                responseStream.Close();
                myHttpWebResponse.Close();
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\HttpPostRequest", ex.Message, true, "");
            }
            return pageContent;
        }

        private static string GetTaggedRefContent(string sGROBID_XmlContent, string sRefOriginalContent)
        {
            string sTaggedRefContent = string.Empty;
            List<string> lstTaggedRefContent = new List<string>();
            string sRefType = "refTypeOther";

            try
            {
                sGROBID_XmlContent = Regex.Replace(sGROBID_XmlContent, "<biblStruct[ ]+>", "<biblStruct>");
                sGROBID_XmlContent = Regex.Replace(sGROBID_XmlContent, "</biblStruct>", "</biblStruct>");
                sGROBID_XmlContent = Regex.Replace(sGROBID_XmlContent, " xmlns=[\u0022][^\u0022]+[\u0022]", "");

                XmlDocument docXml = new XmlDocument();
                docXml.LoadXml(sGROBID_XmlContent);

                #region Author Information
                //tagging "author"
                XmlNodeList nlAuthor = null;
                XmlNodeList nlAuthor1 = docXml.SelectNodes("//biblStruct/analytic/author");
                XmlNodeList nlAuthor2 = docXml.SelectNodes("//biblStruct/monogr/author");

                if (nlAuthor1 != null && nlAuthor1.Count > 0)
                {
                    nlAuthor = nlAuthor1;
                }
                else
                {
                    nlAuthor = nlAuthor2;
                }

                StringBuilder sbAuthorInfo = new StringBuilder();
                if (nlAuthor != null && nlAuthor.Count > 0)
                {
                    //sbAuthorInfo.Append("<authors>");
                    foreach (XmlNode nAuthor in nlAuthor)
                    {
                        string sAuthorXml = nAuthor.OuterXml;
                        XmlDocument docAuthorXml = new XmlDocument();
                        docAuthorXml.LoadXml(sAuthorXml);

                        XmlNode nPersName = docAuthorXml.SelectSingleNode(".//persName");

                        if (nPersName != null)
                        {
                            XmlNodeList nlAuthorElements = nPersName.ChildNodes;

                            sbAuthorInfo.Append("<author>");
                            foreach (XmlNode nAuEle in nlAuthorElements)
                            {
                                sbAuthorInfo.Append(string.Format("<{0}>{1}</{0}> ", nAuEle.LocalName, nAuEle.InnerText));
                            }
                            sbAuthorInfo.Append("</author> ");
                        }
                    }
                    //sbAuthorInfo.Append("</authors>");
                    if (string.IsNullOrEmpty(sbAuthorInfo.ToString()) == false)
                    {
                        lstTaggedRefContent.Add(sbAuthorInfo.ToString());
                    }
                }
                #endregion

                #region Editor Information
                StringBuilder sbEditorInfo = new StringBuilder();
                XmlNodeList nlEditor = null;
                XmlNodeList nlEditor1 = docXml.SelectNodes("//biblStruct/analytic/editor");
                XmlNodeList nlEditor2 = docXml.SelectNodes("//biblStruct/monogr/editor");

                if (nlEditor1 != null && nlEditor1.Count > 0)
                {
                    nlEditor = nlEditor1;
                }
                else
                {
                    nlEditor = nlEditor2;
                }

                if (nlEditor != null && nlEditor.Count > 0)
                {
                    foreach (XmlNode nodeEditor in nlEditor)
                    {
                        sbEditorInfo.Append(string.Format("<{0}>{1}</{0}> ", "editor", nodeEditor.InnerText));
                    }
                }

                if (string.IsNullOrEmpty(sbEditorInfo.ToString()) == false)
                {
                    lstTaggedRefContent.Add(sbEditorInfo.ToString());
                }
                #endregion

                #region Title Information
                StringBuilder sbTitleInfo = new StringBuilder();
                XmlNode nodeArticleTitle = null;
                XmlNode nodeArticleTitle1 = docXml.SelectNodes("//biblStruct/analytic/title[@level='a' and @type='main']")[0];
                XmlNode nodeArticleTitle2 = docXml.SelectNodes("//biblStruct/monogr/title[@level='a' and @type='main']")[0];

                if (nodeArticleTitle1 != null)
                {
                    nodeArticleTitle = nodeArticleTitle1;
                }
                else
                {
                    nodeArticleTitle = nodeArticleTitle2;
                }

                if (nodeArticleTitle != null)
                {
                    sbTitleInfo.Append(string.Format("<{0}>{1}</{0}> ", "articleTitle", nodeArticleTitle.InnerText));
                }

                XmlNode nodeJournalTitle = null;
                XmlNode nodeJournalTitle1 = docXml.SelectNodes("//biblStruct/analytic/title[@level='j']")[0];
                XmlNode nodeJournalTitle2 = docXml.SelectNodes("//biblStruct/monogr/title[@level='j']")[0];

                if (nodeJournalTitle1 != null)
                {
                    nodeJournalTitle = nodeJournalTitle1;
                }
                else
                {
                    nodeJournalTitle = nodeJournalTitle2;
                }

                if (nodeJournalTitle != null)
                {
                    sbTitleInfo.Append(string.Format("<{0}>{1}</{0}> ", "journalTitle", nodeJournalTitle.InnerText));
                }

                XmlNode nodeOtherTitle = null;
                XmlNode nodeOtherTitle1 = docXml.SelectNodes("//biblStruct/analytic/title[@level='m']")[0];
                XmlNode nodeOtherTitle2 = docXml.SelectNodes("//biblStruct/monogr/title[@level='m']")[0];

                if (nodeOtherTitle1 != null)
                {
                    nodeOtherTitle = nodeOtherTitle1;
                }
                else
                {
                    nodeOtherTitle = nodeOtherTitle2;
                }

                if (nodeOtherTitle != null)
                {
                    sbTitleInfo.Append(string.Format("<{0}>{1}</{0}> ", "otherTitle", nodeOtherTitle.InnerText));
                }

                if (string.IsNullOrEmpty(sbTitleInfo.ToString()) == false)
                {
                    lstTaggedRefContent.Add(sbTitleInfo.ToString());
                }
                #endregion

                #region Volume, Issue, and Page Number Information
                StringBuilder sbNumberInfo = new StringBuilder();
                //volume number
                XmlNode nodeVolumeNumber = docXml.SelectSingleNode("//biblStruct/monogr/imprint/biblScope[@unit='volume']");
                if (nodeVolumeNumber != null)
                {
                    sbNumberInfo.Append(string.Format("<{0}>{1}</{0}> ", "volume", nodeVolumeNumber.InnerText));
                }
                //issue number
                XmlNode nodeIssueNumber = docXml.SelectSingleNode("//biblStruct/monogr/imprint/biblScope[@unit='issue']");
                if (nodeIssueNumber != null)
                {
                    sbNumberInfo.Append(string.Format("<{0}>{1}</{0}> ", "issue", nodeIssueNumber.InnerText));
                }
                //page number
                XmlNode nodePageNumber = docXml.SelectSingleNode("//biblStruct/monogr/imprint/biblScope[@unit='page']");
                if (nodePageNumber != null)
                {
                    sbNumberInfo.Append("<pageRange>");
                    if (nodePageNumber.Attributes != null && nodePageNumber.Attributes["from"] != null && nodePageNumber.Attributes["to"] != null)
                    {
                        foreach (XmlAttribute attPage in nodePageNumber.Attributes)
                        {
                            if (attPage.LocalName.ToLower().Equals("from"))
                            {
                                sbNumberInfo.Append(string.Format("<{0}>{1}</{0}> ", "FirstPage", attPage.Value));
                            }
                            else if (attPage.LocalName.ToLower().Equals("to"))
                            {
                                sbNumberInfo.Append(string.Format("<{0}>{1}</{0}> ", "LastPage", attPage.Value));
                            }
                        }
                    }
                    else
                    {
                        sbNumberInfo.Append(string.Format("<{0}>{1}</{0}> ", "FirstPage", nodePageNumber.InnerText));
                    }
                    sbNumberInfo.Append("</pageRange> ");
                }

                if (string.IsNullOrEmpty(sbNumberInfo.ToString()) == false)
                {
                    lstTaggedRefContent.Add(sbNumberInfo.ToString());
                }
                #endregion

                #region Date Information
                StringBuilder sbDateInfo = new StringBuilder();
                //published date
                XmlNode nodePublishedDate = docXml.SelectSingleNode("//biblStruct/monogr/imprint/date[@type='published']");
                if (nodePublishedDate != null)
                {
                    sbDateInfo.Append("<publishedDate>");
                    foreach (XmlAttribute attDate in nodePublishedDate.Attributes)
                    {
                        if (attDate.LocalName.ToLower().Equals("when"))
                        {
                            sbDateInfo.Append(string.Format("<{0}>{1}</{0}> ", "year", attDate.Value));
                        }
                    }
                    sbDateInfo.Append("</publishedDate> ");
                }

                if (string.IsNullOrEmpty(sbDateInfo.ToString()) == false)
                {
                    lstTaggedRefContent.Add(sbDateInfo.ToString());
                }
                #endregion

                #region Website Infomration
                StringBuilder sbWebsite = new StringBuilder();
                XmlNode nodeWebsite = docXml.SelectSingleNode("//biblStruct/analytic/ptr");
                if (nodeWebsite != null)
                {
                    foreach (XmlAttribute attWebsite in nodeWebsite.Attributes)
                    {
                        if (attWebsite.LocalName.ToLower().Equals("target"))
                        {
                            sbWebsite.Append(string.Format("<{0}>{1}</{0}> ", "website", attWebsite.Value));
                        }
                    }
                }

                if (string.IsNullOrEmpty(sbWebsite.ToString()) == false)
                {
                    lstTaggedRefContent.Add(sbWebsite.ToString());
                }
                #endregion

                #region Publisher Location
                StringBuilder sbPubLocationInfo = new StringBuilder();
                XmlNode nodePubLocation = null;
                XmlNode nodePubLocation1 = docXml.SelectSingleNode("//biblStruct/monogr/imprint/pubPlace");
                XmlNode nodePubLocation2 = docXml.SelectSingleNode("//biblStruct/monogr/meeting/address/addrLine");

                if (nodePubLocation1 != null)
                {
                    nodePubLocation = nodePubLocation1;
                }
                else
                {
                    nodePubLocation = nodePubLocation2;
                }

                if (nodePubLocation != null)
                {
                    sbPubLocationInfo.Append(string.Format("<{0}>{1}</{0}> ", "pubPlace", nodePubLocation.InnerText));
                }

                if (string.IsNullOrEmpty(sbPubLocationInfo.ToString()) == false)
                {
                    lstTaggedRefContent.Add(sbPubLocationInfo.ToString());
                }
                #endregion

                #region Publisher Name
                StringBuilder sbPublisherNameInfo = new StringBuilder();
                XmlNode nodePublisherName = docXml.SelectSingleNode("//biblStruct/monogr/imprint/publisher");
                XmlNode nodePublisherName1 = docXml.SelectSingleNode("//biblStruct/monogr/imprint/publisher");
                XmlNode nodePublisherName2 = docXml.SelectSingleNode("//biblStruct/monogr/respStmt/orgName");

                if (nodePublisherName1 != null)
                {
                    nodePublisherName = nodePublisherName1;
                }
                else
                {
                    nodePublisherName = nodePublisherName2;
                }

                if (nodePublisherName != null)
                {
                    sbPublisherNameInfo.Append(string.Format("<{0}>{1}</{0}> ", "PublisherName", nodePublisherName.InnerText));
                }

                if (string.IsNullOrEmpty(sbPublisherNameInfo.ToString()) == false)
                {
                    lstTaggedRefContent.Add(sbPublisherNameInfo.ToString());
                }
                #endregion

                #region DOI Number
                StringBuilder sbDOI_Number = new StringBuilder();
                XmlNode nodeDOI_Number = docXml.SelectSingleNode("//biblStruct/analytic/idno[@type='DOI']");

                if (nodeDOI_Number != null)
                {
                    sbDOI_Number.Append(string.Format("<{0}>{1}</{0}> ", "Doi", nodeDOI_Number.InnerText));
                }

                if (string.IsNullOrEmpty(sbDOI_Number.ToString()) == false)
                {
                    lstTaggedRefContent.Add(sbDOI_Number.ToString());
                }
                #endregion

                #region Note (type="report_type")

                StringBuilder sbNoteReportType = new StringBuilder();
                XmlNode nodeNoteReportType = docXml.SelectSingleNode("//biblStruct/note[@type='report_type']");

                if (nodeNoteReportType != null)
                {
                    sbNoteReportType.Append(string.Format("<{0}>{1}</{0}> ", "NoteReportType", nodeNoteReportType.InnerText));
                }

                if (string.IsNullOrEmpty(sbNoteReportType.ToString()) == false)
                {
                    lstTaggedRefContent.Add(sbNoteReportType.ToString());
                }
                #endregion

                sTaggedRefContent = string.Join(" ", lstTaggedRefContent);
                sTaggedRefContent = RefTagCleanup(sTaggedRefContent);
                sTaggedRefContent = string.Format("<{0}>{1}</{0}>", sRefType, sTaggedRefContent);

                sTaggedRefContent = ConvertGrobidTagTo_eBOT_Tag(sTaggedRefContent, sRefOriginalContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\GetTaggedRefContent", ex.Message, true, "");
            }
            return sTaggedRefContent;
        }

        private static string RefTagCleanup(string sRefContent)
        {
            try
            {
                do
                {
                    sRefContent = Regex.Replace(sRefContent, "([ ]+)(</[^<>]+>)", "$2$1");
                    sRefContent = Regex.Replace(sRefContent, "(<[^/][^<>]+>)([ ]+)", "$2$1");
                    sRefContent = Regex.Replace(sRefContent, "[ ]{2,}", " ");
                } while (Regex.IsMatch(sRefContent, "(?:(?:([ ]+)(</[^<>]+>))|(?:(<[^/][^<>]+>)([ ]+))|(?:[ ]{2,}))"));

                sRefContent = sRefContent.Trim();
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\RefTagCleanup", ex.Message, true, "");
            }
            return sRefContent;
        }

        private static string ConvertGrobidTagTo_eBOT_Tag(string sGrobidTaggedContent, string sRefInputContent)
        {
            try
            {
                //pre cleanup
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, @"</(forename|surname)> <\1>", " ");

                //updated by Dakshinamoorthy on 2020-Dec-12
                //sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, @"<pageRange><FirstPage>((?:(?!</?FirstPage>).)+)</FirstPage> <LastPage>((?:(?!</?LastPage>).)+)</LastPage></pageRange>", "<PageRange>$1-$2</PageRange>");
                #region Page Range matching with original content
                string sPageRangePattern = @"<pageRange><FirstPage>((?:(?!</?FirstPage>).)+)</FirstPage> <LastPage>((?:(?!</?LastPage>).)+)</LastPage></pageRange>";
                if (Regex.IsMatch(sGrobidTaggedContent, sPageRangePattern))
                {
                    Match matchPageRange = Regex.Match(sGrobidTaggedContent, sPageRangePattern);
                    string sPageRangeValue = matchPageRange.Value;
                    string sFirstPage = matchPageRange.Groups[1].Value;
                    string sLastPage = matchPageRange.Groups[2].Value;

                    string sActualPageRangePattern = string.Format(@" {0}[ ]?(?:[\u2013]|\-)[ ]?{1}[\:.;, ]+", sFirstPage, sLastPage);

                    if (Regex.IsMatch(sRefInputContent, sActualPageRangePattern))
                    {
                        string sOriginalPageRange = Regex.Match(sRefInputContent, sActualPageRangePattern).Value;
                        sGrobidTaggedContent = sGrobidTaggedContent.Replace(sPageRangeValue, string.Format("<{0}>{1}</{0}>", "PageRange", sOriginalPageRange));
                        sGrobidTaggedContent = RefTagCleanup(sGrobidTaggedContent);
                    }
                }
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<pageRange>", "<PageRange>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</pageRange>", "</PageRange>");
                #endregion


                //Reference Type
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<refTypeBook>", "<Book>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</refTypeBook>", "</Book>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<refTypeCommunication>", "<Communication>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</refTypeCommunication>", "</Communication>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<refTypeConference>", "<Conference>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</refTypeConference>", "</Conference>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<refTypeJournal>", "<Journal>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</refTypeJournal>", "</Journal>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<refTypeOther>", "<Other>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</refTypeOther>", "</Other>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<refTypePatent>", "<Patent>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</refTypePatent>", "</Patent>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<refTypeReferences>", "<References>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</refTypeReferences>", "</References>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<refTypeReport>", "<Report>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</refTypeReport>", "</Report>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<refTypeThesis>", "<Thesis>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</refTypeThesis>", "</Thesis>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<refTypeWeb>", "<Web>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</refTypeWeb>", "</Web>");

                //Author
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<author>", "<Author>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</author>", "</Author>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<forename>", "<Forename>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</forename>", "</Forename>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<surname>", "<Surname>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</surname>", "</Surname>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<genName>", "<Suffix>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</genName>", "</Suffix>");

                //Title Information
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<articleTitle>", "<Article_Title>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</articleTitle>", "</Article_Title>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<journalTitle>", "<Journal_Title>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</journalTitle>", "</Journal_Title>");

                //Volume
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<volume>", "<Vol_No>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</volume>", "</Vol_No>");

                //Issue
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<issue>", "<Issue_No>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</issue>", "</Issue_No>");

                //Publisher Location
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<pubPlace>", "<PublisherLocation>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</pubPlace>", "</PublisherLocation>");

                //Website
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<website>", "<Website>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</website>", "</Website>");

                //Date Information
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "<publishedDate>", "<DateInfo>");
                sGrobidTaggedContent = Regex.Replace(sGrobidTaggedContent, "</publishedDate>", "</DateInfo>");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\ConvertGrobidTagTo_eBOT_Tag", ex.Message, true, "");
            }
            return sGrobidTaggedContent;
        }

        private static string ExcludeRefElements(string sRefContent)
        {
            try
            {
                string sExclusionPattern = string.Empty;

                //Exclude "Author"
                sExclusionPattern = string.Format("(?:<{0}>(?:(?!</?{0}>).)+</{0}>)", "Author");
                sRefContent = Regex.Replace(sRefContent, sExclusionPattern, "");

                //Exclude "Date Info"
                sExclusionPattern = string.Format("(?:<{0}>(?:(?!</?{0}>).)+</{0}>)", "DateInfo");
                sRefContent = Regex.Replace(sRefContent, sExclusionPattern, "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\ExcludeRefElements", ex.Message, true, "");
            }
            sRefContent = RefTagCleanup(sRefContent);
            return sRefContent;
        }

        #endregion

        public static bool IdentifyRefElements(string sRefInputContent, ref string sRefTaggedContent)
        {
            try
            {
                //gRefInputContent = sRefInputContent;
                //IdentifyRefElements_API();
                //sRefTaggedContent = gRefOutputContent;

                if (string.IsNullOrEmpty(sRefInputContent))
                {
                    return false;
                }

                //string sWebRequestUrl = string.Format("{0}{1}", sAPI_BaseAddress, System.Web.HttpUtility.UrlEncode(sRefInputContent));
                string sWebRequestUrl = string.Format("{0}{1}", sAnyStyleAPI_BaseAddress, sRefInputContent);

                if (ReadAPI_Result(sWebRequestUrl, ref sWebRequestResultContent) == true && string.IsNullOrEmpty(sWebRequestResultContent) == false)
                {
                    sRefTaggedContent = JsonRessultToTaggedContent(sWebRequestResultContent);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\IdentifyRefElements", ex.Message, true, "");
                return false;
            }
            return true;
        }

        private static bool ReadAPI_Result(string sWebRequestUrl, ref string sResultContent)
        {
            try
            {
                DateTime startTime = DateTime.Now;

                HttpWebRequest request = WebRequest.Create(sWebRequestUrl) as HttpWebRequest;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                WebHeaderCollection header = response.Headers;

                //var encoding = ASCIIEncoding.ASCII;
                var encoding = Encoding.UTF8;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    sResultContent = reader.ReadToEnd();
                }

                DateTime endTime = DateTime.Now;
                //System.Windows.Forms.MessageBox.Show("API Time Duration: " + ((int)((TimeSpan)(endTime - startTime)).TotalSeconds).ToString());
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\ReadAPI_Result", ex.Message, true, "");
                return false;
            }
            return true;
        }

        private static string JsonRessultToTaggedContent(string sJsonResultContent)
        {
            string sOutputTaggedContent = string.Empty;

            try
            {
                var obj = JsonConvert.DeserializeObject(sJsonResultContent);
                string json = JsonConvert.SerializeObject(obj);

                JArray reference_data = JArray.Parse(json);
                {
                    foreach (JObject refernce in reference_data)
                    {
                        string type = refernce.GetValue("type").ToString();

                        if (type == "article-journal")
                        {
                            var List = new List<string>();
                            string refoutput = "";
                            DataTable dt = referencejournal(json);
                            foreach (DataRow dr in dt.Rows)
                            {
                                for (int i = 1; i < dr.ItemArray.Length; i++)
                                {
                                    refoutput = dr.ItemArray[i].ToString();
                                    List.Add(refoutput);

                                }
                            }
                            sOutputTaggedContent = string.Join(" ", List);
                            sOutputTaggedContent = string.Format("<{0}>{1}</{0}>", "Journal", sOutputTaggedContent);
                        }
                        else if (type == "paper-conference")
                        {
                            var List = new List<string>();
                            string refoutput = "";
                            DataTable dt = referenceconference(json);
                            foreach (DataRow dr in dt.Rows)
                            {
                                for (int i = 1; i < dr.ItemArray.Length; i++)
                                {
                                    refoutput = dr.ItemArray[i].ToString();
                                    List.Add(refoutput);
                                }
                            }
                            sOutputTaggedContent = string.Join(" ", List);
                            sOutputTaggedContent = string.Format("<{0}>{1}</{0}>", "Conference", sOutputTaggedContent);
                        }
                        else if (type == "book")
                        {
                            var List = new List<string>();

                            string refoutput = "";
                            DataTable dt = referencebook(json);
                            foreach (DataRow dr in dt.Rows)
                            {
                                for (int i = 1; i < dr.ItemArray.Length; i++)
                                {
                                    refoutput = dr.ItemArray[i].ToString();
                                    List.Add(refoutput);

                                }
                            }
                            sOutputTaggedContent = string.Join(" ", List);
                            sOutputTaggedContent = string.Format("<{0}>{1}</{0}>", "Book", sOutputTaggedContent);
                        }
                        else if (type == "chapter")
                        {
                            var List = new List<string>();

                            string refoutput = "";
                            DataTable dt = referencechapter(json);
                            foreach (DataRow dr in dt.Rows)
                            {
                                for (int i = 1; i < dr.ItemArray.Length; i++)
                                {
                                    refoutput = dr.ItemArray[i].ToString();
                                    List.Add(refoutput);

                                }
                            }
                            sOutputTaggedContent = string.Join(" ", List);
                            sOutputTaggedContent = string.Format("<{0}>{1}</{0}>", "Book", sOutputTaggedContent);
                        }
                        else if (type == "report")
                        {
                            var List = new List<string>();

                            string refoutput = "";
                            DataTable dt = referencereport(json);
                            foreach (DataRow dr in dt.Rows)
                            {
                                for (int i = 1; i < dr.ItemArray.Length; i++)
                                {
                                    refoutput = dr.ItemArray[i].ToString();
                                    List.Add(refoutput);
                                }
                            }
                            sOutputTaggedContent = string.Join(" ", List);
                            sOutputTaggedContent = string.Format("<{0}>{1}</{0}>", "Report", sOutputTaggedContent);
                        }
                        else if (type == "thesis")
                        {
                            var List = new List<string>();

                            string refoutput = "";
                            DataTable dt = referencethesis(json);
                            foreach (DataRow dr in dt.Rows)
                            {
                                for (int i = 1; i < dr.ItemArray.Length; i++)
                                {
                                    refoutput = dr.ItemArray[i].ToString();
                                    List.Add(refoutput);

                                }
                            }
                            sOutputTaggedContent = string.Join(" ", List);
                            sOutputTaggedContent = string.Format("<{0}>{1}</{0}>", "Thesis", sOutputTaggedContent);
                        }
                        else if (type == "web") //need to check
                        {

                        }
                        else
                        {
                            var List = new List<string>();

                            string refoutput = "";
                            DataTable dt = referencethesis(json);
                            foreach (DataRow dr in dt.Rows)
                            {
                                for (int i = 1; i < dr.ItemArray.Length; i++)
                                {
                                    refoutput = dr.ItemArray[i].ToString();
                                    List.Add(refoutput);

                                }
                            }
                            sOutputTaggedContent = string.Join(" ", List);
                            sOutputTaggedContent = string.Format("<{0}>{1}</{0}>", "References", sOutputTaggedContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\JsonRessultToTaggedContent", ex.Message, true, "");
            }
            return sOutputTaggedContent;
        }


        private static async void IdentifyRefElements_API()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(sAnyStyleAPI_BaseAddress + gRefInputContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;


                        JsonConvert.DeserializeObject<dynamic>(result);
                        result = await response.Content.ReadAsStringAsync();
                        var obj = JsonConvert.DeserializeObject(result);
                        string json = JsonConvert.SerializeObject(obj);

                        JArray reference_data = JArray.Parse(json);
                        {
                            foreach (JObject refernce in reference_data)
                            {
                                string type = refernce.GetValue("type").ToString();
                                if (type == "article-journal")
                                {
                                    var List = new List<string>();

                                    string refoutput = "";
                                    DataTable dt = referencejournal(json);
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        for (int i = 1; i < dr.ItemArray.Length; i++)
                                        {
                                            refoutput = dr.ItemArray[i].ToString();
                                            List.Add(refoutput);

                                        }
                                    }

                                    string refContent = string.Join(" ", List);
                                    Console.WriteLine("\n" + refContent);
                                }
                                else if (type == "paper-conference")
                                {

                                    var List = new List<string>();

                                    string refoutput = "";
                                    DataTable dt = referenceconference(json);
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        for (int i = 1; i < dr.ItemArray.Length; i++)
                                        {
                                            refoutput = dr.ItemArray[i].ToString();
                                            List.Add(refoutput);

                                        }
                                    }
                                    string refContent = string.Join(" ", List);
                                    Console.WriteLine("\n" + refContent);
                                }
                                else if (type == "book")
                                {
                                    var List = new List<string>();

                                    string refoutput = "";
                                    DataTable dt = referencebook(json);
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        for (int i = 1; i < dr.ItemArray.Length; i++)
                                        {
                                            refoutput = dr.ItemArray[i].ToString();
                                            List.Add(refoutput);

                                        }
                                    }
                                    string refContent = string.Join(" ", List);
                                    Console.WriteLine("\n" + refContent);
                                }
                                else if (type == "chapter")
                                {
                                    var List = new List<string>();

                                    string refoutput = "";
                                    DataTable dt = referencechapter(json);
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        for (int i = 1; i < dr.ItemArray.Length; i++)
                                        {
                                            refoutput = dr.ItemArray[i].ToString();
                                            List.Add(refoutput);

                                        }
                                    }
                                    string refContent = string.Join(" ", List);
                                    Console.WriteLine("\n" + refContent);
                                }
                                else if (type == "report")
                                {
                                    var List = new List<string>();

                                    string refoutput = "";
                                    DataTable dt = referencereport(json);
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        for (int i = 1; i < dr.ItemArray.Length; i++)
                                        {
                                            refoutput = dr.ItemArray[i].ToString();
                                            List.Add(refoutput);

                                        }
                                    }



                                    string refContent = string.Join(" ", List);
                                    Console.WriteLine("\n" + refContent);

                                }
                                else if (type == "thesis")
                                {
                                    var List = new List<string>();

                                    string refoutput = "";
                                    DataTable dt = referencethesis(json);
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        for (int i = 1; i < dr.ItemArray.Length; i++)
                                        {
                                            refoutput = dr.ItemArray[i].ToString();
                                            List.Add(refoutput);

                                        }
                                    }


                                    string refContent = string.Join(" ", List);
                                    Console.WriteLine("\n" + refContent);
                                }
                                else
                                {

                                }
                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine("Internal server Error");
                    }
                }

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\IdentifyRefElements_API", ex.Message, true, "");
            }
        }


        static DataTable referencejournal(string json)
        {
            DataTable dt = new DataTable();
            try
            {
                
                var myList = new List<KeyValuePair<string, string>>();
                dt.Columns.Add("Reference Key", typeof(string));
                dt.Columns.Add("Reference value", typeof(string));
                JArray reference_data = JArray.Parse(json);
                {
                    foreach (JObject refernce in reference_data)
                    {


                        string type = refernce.GetValue("type").ToString();


                        if (type == "article-journal")
                        {
                            if (json.Contains("citation-number"))
                            {
                                string citationNumber = refernce.GetValue("citation-number").ToString();
                                citationNumber = Regex.Replace(citationNumber, @"(\[)", "");
                                citationNumber = Regex.Replace(citationNumber, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(citationNumber);
                                citationNumber = match.Groups[1].Value;
                                dt.Rows.Add("number", "<RefLabel>" + citationNumber + "</RefLabel>");

                            }
                            if (json.Contains("author"))
                            {
                                string authorGroup = refernce.GetValue("author").ToString();

                                JArray authorGroupitem = JArray.Parse(authorGroup);
                                {
                                    foreach (JObject item in authorGroupitem)
                                    {
                                        string surname = item.GetValue("family").ToString();
                                        string givenname = item.GetValue("given").ToString();

                                        dt.Rows.Add("author", "<Author>" + "<Surname>" + givenname + " </Surname> " + "<Forename>" + surname + "</Forename>" + "</author>");
                                    }
                                }
                            }
                            if (json.Contains("date"))
                            {
                                string date = refernce.GetValue("date").ToString();
                                date = Regex.Replace(date, @"(\[)", "");
                                date = Regex.Replace(date, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(date);
                                date = match.Groups[1].Value;
                                dt.Rows.Add("year", "<PubDate>" + date + "</PubDate>");
                            }

                            if (json.Contains("title"))
                            {
                                string articletitle = refernce.GetValue("title").ToString();
                                articletitle = Regex.Replace(articletitle, @"(\[)", "");
                                articletitle = Regex.Replace(articletitle, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(articletitle);
                                articletitle = match.Groups[1].Value;
                                dt.Rows.Add("articletitle", "<Article_Title>" + articletitle + "</Article_Title>");

                            }
                            if (json.Contains("container-title"))
                            {
                                string journaltitle = refernce.GetValue("container-title").ToString();
                                journaltitle = Regex.Replace(journaltitle, @"(\[)", "");
                                journaltitle = Regex.Replace(journaltitle, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(journaltitle);
                                journaltitle = match.Groups[1].Value;
                                dt.Rows.Add("journaltitle", "<Journal_Title>" + journaltitle + "</Journal_Title>");



                                //myList.Add(new KeyValuePair<string, string>("journaltitle", journaltitle));


                            }
                            if (json.Contains("volume"))
                            {

                                string volume = refernce.GetValue("volume").ToString();
                                volume = Regex.Replace(volume, @"(\[)", "");
                                volume = Regex.Replace(volume, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(volume);
                                volume = match.Groups[1].Value;
                                dt.Rows.Add("journaltitle", "<Vol_No>" + volume + "</Vol_No>");


                            }
                            if (json.Contains("issue"))
                            {

                                string issue = refernce.GetValue("issue").ToString();
                                issue = Regex.Replace(issue, @"(\[)", "");
                                issue = Regex.Replace(issue, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(issue);
                                issue = match.Groups[1].Value;
                                dt.Rows.Add("issueno", "<Issue_No>" + issue + "</Issue_No>");

                            }
                            if (json.Contains("pages"))
                            {
                                string pages = refernce.GetValue("pages").ToString();
                                pages = Regex.Replace(pages, @"(\[)", "");
                                pages = Regex.Replace(pages, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(pages);
                                pages = match.Groups[1].Value;
                                string pagearange = Regex.Replace(pages, @"([0-9]+)(–)([0-9]+)", "<ldlFirstPageNumber>$1</ldlFirstPageNumber>$2<ldlLastPageNumber>$3</ldlLastPageNumber>");
                                dt.Rows.Add("pagerange", "<PageRange>" + pagearange + "</PageRange>");
                            }
                            if (json.Contains("doi"))
                            {
                                string doi = refernce.GetValue("doi").ToString();
                                doi = Regex.Replace(doi, @"(\[)", "");
                                doi = Regex.Replace(doi, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(doi);
                                doi = match.Groups[1].Value;
                                dt.Rows.Add("doino", "<Doi>" + doi + "</Doi>");



                            }
                            if (json.Contains("url"))
                            {
                                string url = refernce.GetValue("url").ToString();
                                url = Regex.Replace(url, @"(\[)", "");
                                url = Regex.Replace(url, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(url);
                                url = match.Groups[1].Value;
                                dt.Rows.Add("url", "<Website>" + url + "</Website>");

                            }

                        }

                    }

                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\referencejournal", ex.Message, true, "");
            }
            return dt;
        }
        static DataTable referenceconference(string json)
        {
            DataTable dt = new DataTable();
            try
            {

                dt.Columns.Add("Reference Key", typeof(string));
                dt.Columns.Add("Reference value", typeof(string));
                JArray reference_data = JArray.Parse(json);
                {
                    foreach (JObject refernce in reference_data)
                    {

                        string type = refernce.GetValue("type").ToString();
                        if (type == "paper-conference")
                        {

                            if (json.Contains("citation-number"))
                            {
                                string citationNumber = refernce.GetValue("citation-number").ToString();
                                citationNumber = Regex.Replace(citationNumber, @"(\[)", "");
                                citationNumber = Regex.Replace(citationNumber, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(citationNumber);
                                citationNumber = match.Groups[1].Value;
                                dt.Rows.Add("number", "<RefLabel>" + citationNumber + "</RefLabel>");

                            }
                            if (json.Contains("author"))
                            {
                                string authorGroup = refernce.GetValue("author").ToString();

                                JArray authorGroupitem = JArray.Parse(authorGroup);
                                {
                                    foreach (JObject item in authorGroupitem)
                                    {
                                        string surname = item.GetValue("family").ToString();
                                        string givenname = item.GetValue("given").ToString();

                                        dt.Rows.Add("author", "<Author>" + "<Surname>" + givenname + " </Surname> " + "<Forename>" + surname + "</Forename>" + "</Author>");
                                    }
                                }
                            }
                            if (json.Contains("date"))
                            {
                                string date = refernce.GetValue("date").ToString();
                                date = Regex.Replace(date, @"(\[)", "");
                                date = Regex.Replace(date, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(date);
                                date = match.Groups[1].Value;
                                dt.Rows.Add("year", "<PubDate>" + date + "</PubDate>");
                            }

                            if (json.Contains("title"))
                            {
                                string articleTitle = refernce.GetValue("title").ToString();
                                articleTitle = Regex.Replace(articleTitle, @"(\[)", "");
                                articleTitle = Regex.Replace(articleTitle, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(articleTitle);
                                articleTitle = match.Groups[1].Value;
                                dt.Rows.Add("articletitle", "<Article_Title>" + articleTitle + "</Article_Title>");

                            }
                            if (json.Contains("container-title"))
                            {
                                string conference = refernce.GetValue("container-title").ToString();
                                conference = Regex.Replace(conference, @"(\[)", "");
                                conference = Regex.Replace(conference, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(conference);
                                conference = match.Groups[1].Value;
                                dt.Rows.Add("conferencename", "<ldlConferenceName>" + conference + "</ldlConferenceName>");
                            }
                            if (json.Contains("editor"))
                            {
                                string editor = refernce.GetValue("editor").ToString();
                                JArray editoritem = JArray.Parse(editor);
                                {
                                    foreach (JObject item in editoritem)
                                    {
                                        string surname = item.GetValue("family").ToString();
                                        string givenname = item.GetValue("given").ToString();

                                        dt.Rows.Add("editor", "<Editor>" + "<ESurname>" + givenname + " </ESurname> " + "<EForename>" + surname + "</EForename>" + "</Editor>");

                                    }
                                }
                            }
                            if (json.Contains("publisher"))
                            {
                                string publisher = refernce.GetValue("publisher").ToString();
                                publisher = Regex.Replace(publisher, @"(\[)", "");
                                publisher = Regex.Replace(publisher, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(publisher);
                                publisher = match.Groups[1].Value;
                                dt.Rows.Add("publisher", "<ldlPublisherName>" + publisher + "</ldlPublisherName>");

                            }
                            if (json.Contains("location"))
                            {
                                string location = refernce.GetValue("location").ToString();
                                location = Regex.Replace(location, @"(\[)", "");
                                location = Regex.Replace(location, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(location);
                                location = match.Groups[1].Value;
                                dt.Rows.Add("location", "<ldlPublisherLocation>" + location + "</ldlPublisherLocation>");

                            }
                            if (json.Contains("pages"))
                            {
                                string pages = refernce.GetValue("pages").ToString();

                                pages = Regex.Replace(pages, @"(\[)", "");
                                pages = Regex.Replace(pages, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(pages);
                                pages = match.Groups[1].Value;
                                string pagearange = Regex.Replace(pages, @"([0-9]+)(–)([0-9]+)", "<ldlFirstPageNumber>$1</ldlFirstPageNumber>$2<ldlLastPageNumber>$3</ldlLastPageNumber>");
                                dt.Rows.Add("pagerange", "<PageRange>" + pagearange + "</PageRange>");
                            }
                            if (json.Contains("url"))
                            {
                                string url = refernce.GetValue("url").ToString();
                                url = Regex.Replace(url, @"(\[)", "");
                                url = Regex.Replace(url, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(url);
                                url = match.Groups[1].Value;
                                dt.Rows.Add("url", "<Website>" + url + "</Website>");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\referenceconference", ex.Message, true, "");
            }
            return dt;
        }


        static DataTable referencebook(string json)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Reference Key", typeof(string));
            dt.Columns.Add("Reference value", typeof(string));

            try
            {


                JArray reference_data = JArray.Parse(json);
                {
                    foreach (JObject refernce in reference_data)
                    {

                        string type = refernce.GetValue("type").ToString();
                        if (type == "book")
                        {

                            if (json.Contains("citation-number"))
                            {
                                string citationNumber = refernce.GetValue("citation-number").ToString();
                                citationNumber = Regex.Replace(citationNumber, @"(\[)", "");
                                citationNumber = Regex.Replace(citationNumber, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(citationNumber);
                                citationNumber = match.Groups[1].Value;
                                dt.Rows.Add("number", "<RefLabel>" + citationNumber + "</RefLabel>");

                            }
                            if (json.Contains("author"))
                            {
                                string authorGroup = refernce.GetValue("author").ToString();

                                JArray authorGroupitem = JArray.Parse(authorGroup);
                                {
                                    foreach (JObject item in authorGroupitem)
                                    {
                                        string surname = item.GetValue("family").ToString();
                                        string givenname = item.GetValue("given").ToString();

                                        dt.Rows.Add("author", "<Author>" + "<Surname>" + givenname + " </Surname> " + "<Forename>" + surname + "</Forename>" + "</Author>");
                                    }
                                }
                            }
                            if (json.Contains("date"))
                            {
                                string date = refernce.GetValue("date").ToString();
                                date = Regex.Replace(date, @"(\[)", "");
                                date = Regex.Replace(date, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(date);
                                date = match.Groups[1].Value;
                                dt.Rows.Add("year", "<PubDate>" + date + "</PubDate>");
                            }


                            if (json.Contains("title"))
                            {
                                string chapterTitle = refernce.GetValue("title").ToString();
                                chapterTitle = Regex.Replace(chapterTitle, @"(\[)", "");
                                chapterTitle = Regex.Replace(chapterTitle, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(chapterTitle);
                                chapterTitle = match.Groups[1].Value;
                                dt.Rows.Add("chapterTitle", "<ldlChapterTitle>" + chapterTitle + "</ldlChapterTitle>");


                            }
                            if (json.Contains("container-title"))
                            {
                                string bookTitle = refernce.GetValue("container-title").ToString();
                                bookTitle = Regex.Replace(bookTitle, @"(\[)", "");
                                bookTitle = Regex.Replace(bookTitle, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(bookTitle);
                                bookTitle = match.Groups[1].Value;
                                dt.Rows.Add("bookTitle", "<ldlBookTitle>" + bookTitle + "</ldlBookTitle>");


                            }
                            if (json.Contains("editor"))
                            {
                                string editor = refernce.GetValue("editor").ToString();
                                JArray editoritem = JArray.Parse(editor);
                                {
                                    foreach (JObject item in editoritem)
                                    {
                                        string surname = item.GetValue("family").ToString();
                                        string givenname = item.GetValue("given").ToString();
                                        dt.Rows.Add("editor", "<Editor>" + "<ESurname>" + givenname + " </ESurname> " + "<EForename>" + surname + "</EForename>" + "</Editor>");

                                    }
                                }

                            }

                            if (json.Contains("publisher"))
                            {
                                string publisher = refernce.GetValue("publisher").ToString();
                                publisher = Regex.Replace(publisher, @"(\[)", "");
                                publisher = Regex.Replace(publisher, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(publisher);
                                publisher = match.Groups[1].Value;
                                dt.Rows.Add("publisher", "<ldlPublisherName>" + publisher + "</ldlPublisherName>");

                            }
                            if (json.Contains("location"))
                            {
                                string location = refernce.GetValue("location").ToString();
                                location = Regex.Replace(location, @"(\[)", "");
                                location = Regex.Replace(location, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(location);
                                location = match.Groups[1].Value;
                                dt.Rows.Add("location", "<ldlPublisherLocation>" + location + "</ldlPublisherLocation>");

                            }
                            if (json.Contains("pages"))
                            {
                                string pages = refernce.GetValue("pages").ToString();
                                pages = Regex.Replace(pages, @"(\[)", "");
                                pages = Regex.Replace(pages, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(pages);
                                pages = match.Groups[1].Value;
                                string pagearange = Regex.Replace(pages, @"([0-9]+)(–)([0-9]+)", "<ldlFirstPageNumber>$1</ldlFirstPageNumber>$2<ldlLastPageNumber>$3</ldlLastPageNumber>");
                                dt.Rows.Add("pagerange", "<PageRange>" + pagearange + "</PageRange>");
                            }
                            if (json.Contains("url"))
                            {
                                string url = refernce.GetValue("url").ToString();
                                url = Regex.Replace(url, @"(\[)", "");
                                url = Regex.Replace(url, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(url);
                                url = match.Groups[1].Value;
                                dt.Rows.Add("url", "<Website>" + url + "</Website>");

                            }
                            if (json.Contains("volume"))
                            {
                                string volume = refernce.GetValue("volume").ToString();
                                volume = Regex.Replace(volume, @"(\[)", "");
                                volume = Regex.Replace(volume, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(volume);
                                volume = match.Groups[1].Value;
                                dt.Rows.Add("volume", "<Vol_No>" + volume + "</Vol_No>");
                            }
                            if (json.Contains("pages"))
                            {
                                string pages = refernce.GetValue("pages").ToString();

                                pages = Regex.Replace(pages, @"(\[)", "");
                                pages = Regex.Replace(pages, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(pages);
                                pages = match.Groups[1].Value;
                                string pagearange = Regex.Replace(pages, @"([0-9]+)(–|\?\?\?)([0-9]+)", "<ldlFirstPageNumber>$1</ldlFirstPageNumber>$2<ldlLastPageNumber>$3</ldlLastPageNumber>");
                                dt.Rows.Add("pagerange", "<PageRange>" + pagearange + "</PageRange>");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\referencebook", ex.Message, true, "");
            }
            return dt;
        }



        static DataTable referencechapter(string json)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Reference Key", typeof(string));
            dt.Columns.Add("Reference value", typeof(string));

            try
            {
                var myList = new List<KeyValuePair<string, string>>();
                JArray reference_data = JArray.Parse(json);

                {
                    foreach (JObject refernce in reference_data)
                    {

                        string type = refernce.GetValue("type").ToString();
                        if (type == "chapter")
                        {


                            if (json.Contains("citation-number"))
                            {
                                string citationNumber = refernce.GetValue("citation-number").ToString();
                                citationNumber = Regex.Replace(citationNumber, @"(\[)", "");
                                citationNumber = Regex.Replace(citationNumber, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(citationNumber);
                                citationNumber = match.Groups[1].Value;
                                dt.Rows.Add("number", "<RefLabel>" + citationNumber + "</RefLabel>");

                            }
                            if (json.Contains("author"))
                            {
                                string authorGroup = refernce.GetValue("author").ToString();

                                JArray authorGroupitem = JArray.Parse(authorGroup);
                                {
                                    foreach (JObject item in authorGroupitem)
                                    {
                                        string surname = item.GetValue("family").ToString();
                                        string givenname = item.GetValue("given").ToString();

                                        dt.Rows.Add("author", "<Author>" + "<Surname>" + givenname + " </Surname> " + "<Forename>" + surname + "</Forename>" + "</Author>");
                                    }
                                }
                            }
                            if (json.Contains("date"))
                            {
                                string date = refernce.GetValue("date").ToString();
                                date = Regex.Replace(date, @"(\[)", "");
                                date = Regex.Replace(date, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(date);
                                date = match.Groups[1].Value;
                                dt.Rows.Add("year", "<PubDate>" + date + "</PubDate>");
                            }


                            if (json.Contains("title"))
                            {
                                string chapterTitle = refernce.GetValue("title").ToString();
                                chapterTitle = Regex.Replace(chapterTitle, @"(\[)", "");
                                chapterTitle = Regex.Replace(chapterTitle, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(chapterTitle);
                                chapterTitle = match.Groups[1].Value;
                                dt.Rows.Add("chapterTitle", "<ldlChapterTitle>" + chapterTitle + "</ldlChapterTitle>");


                            }
                            if (json.Contains("container-title"))
                            {
                                string bookTitle = refernce.GetValue("container-title").ToString();
                                bookTitle = Regex.Replace(bookTitle, @"(\[)", "");
                                bookTitle = Regex.Replace(bookTitle, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(bookTitle);
                                bookTitle = match.Groups[1].Value;
                                dt.Rows.Add("bookTitle", "<ldlBookTitle>" + bookTitle + "</ldlBookTitle>");


                            }
                            if (json.Contains("editor"))
                            {
                                string editor = refernce.GetValue("editor").ToString();
                                JArray editoritem = JArray.Parse(editor);
                                {
                                    foreach (JObject item in editoritem)
                                    {
                                        string surname = item.GetValue("family").ToString();
                                        string givenname = item.GetValue("given").ToString();
                                        dt.Rows.Add("editor", "<Editor>" + "<ESurname>" + givenname + " </ESurname> " + "<EForename>" + surname + "</EForename>" + "</Editor>");

                                    }
                                }

                            }

                            if (json.Contains("publisher"))
                            {
                                string publisher = refernce.GetValue("publisher").ToString();
                                publisher = Regex.Replace(publisher, @"(\[)", "");
                                publisher = Regex.Replace(publisher, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(publisher);
                                publisher = match.Groups[1].Value;
                                dt.Rows.Add("publisher", "<ldlPublisherName>" + publisher + "</ldlPublisherName>");

                            }
                            if (json.Contains("location"))
                            {
                                string location = refernce.GetValue("location").ToString();
                                location = Regex.Replace(location, @"(\[)", "");
                                location = Regex.Replace(location, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(location);
                                location = match.Groups[1].Value;
                                dt.Rows.Add("location", "<ldlPublisherLocation>" + location + "</ldlPublisherLocation>");

                            }
                            if (json.Contains("pages"))
                            {
                                string pages = refernce.GetValue("pages").ToString();
                                pages = Regex.Replace(pages, @"(\[)", "");
                                pages = Regex.Replace(pages, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(pages);
                                pages = match.Groups[1].Value;
                                string pagearange = Regex.Replace(pages, @"([0-9]+)(–)([0-9]+)", "<ldlFirstPageNumber>$1</ldlFirstPageNumber>$2<ldlLastPageNumber>$3</ldlLastPageNumber>");
                                dt.Rows.Add("pagerange", "<PageRange>" + pagearange + "</PageRange>");
                            }
                            if (json.Contains("url"))
                            {
                                string url = refernce.GetValue("url").ToString();
                                url = Regex.Replace(url, @"(\[)", "");
                                url = Regex.Replace(url, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(url);
                                url = match.Groups[1].Value;
                                dt.Rows.Add("url", "<Website>" + url + "</Website>");

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\referencechapter", ex.Message, true, "");
            }
            return dt;
        }
        static DataTable referencereport(string json)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Reference Key", typeof(string));
            dt.Columns.Add("Reference value", typeof(string));
            try
            {
                var myList = new List<KeyValuePair<string, string>>();
                JArray reference_data = JArray.Parse(json);
                {
                    foreach (JObject refernce in reference_data)
                    {

                        string type = refernce.GetValue("type").ToString();
                        if (type == "report")
                        {


                            if (json.Contains("citation-number"))
                            {
                                string citationNumber = refernce.GetValue("citation-number").ToString();
                                citationNumber = Regex.Replace(citationNumber, @"(\[)", "");
                                citationNumber = Regex.Replace(citationNumber, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(citationNumber);
                                citationNumber = match.Groups[1].Value;
                                dt.Rows.Add("number", "<RefLabel>" + citationNumber + "</RefLabel>");

                            }
                            if (json.Contains("author"))
                            {
                                string authorGroup = refernce.GetValue("author").ToString();

                                JArray authorGroupItem = JArray.Parse(authorGroup);
                                {
                                    foreach (JObject item in authorGroupItem)
                                    {
                                        string surname = item.GetValue("family").ToString();
                                        string givenname = item.GetValue("given").ToString();

                                        dt.Rows.Add("author", "<Author>" + "<Surname>" + givenname + " </Surname> " + "<Forename>" + surname + "</Forename>" + "</Author>");
                                    }
                                }
                            }
                            if (json.Contains("date"))
                            {
                                string date = refernce.GetValue("date").ToString();
                                date = Regex.Replace(date, @"(\[)", "");
                                date = Regex.Replace(date, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(date);
                                date = match.Groups[1].Value;
                                dt.Rows.Add("year", "<PubDate>" + date + "</PubDate>");
                            }

                            if (json.Contains("title"))
                            {
                                string articleTitle = refernce.GetValue("title").ToString();
                                articleTitle = Regex.Replace(articleTitle, @"(\[)", "");
                                articleTitle = Regex.Replace(articleTitle, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(articleTitle);
                                articleTitle = match.Groups[1].Value;
                                dt.Rows.Add("articletitle", "<Article_Title>" + articleTitle + "</Article_Title>");

                            }
                            if (json.Contains("genre"))
                            {
                                string report = refernce.GetValue("genre").ToString();
                                report = Regex.Replace(report, @"(\[)", "");
                                report = Regex.Replace(report, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(report);
                                report = match.Groups[1].Value;
                                dt.Rows.Add("report", "<ldlReportKeyword>" + report + "</ldlReportKeyword>");



                            }
                            if (json.Contains("publisher"))
                            {
                                string publisher = refernce.GetValue("publisher").ToString();
                                publisher = Regex.Replace(publisher, @"(\[)", "");
                                publisher = Regex.Replace(publisher, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(publisher);
                                publisher = match.Groups[1].Value;
                                dt.Rows.Add("publisher", "<ldlPublisherName>" + publisher + "</ldlPublisherName>");

                            }
                            if (json.Contains("location"))
                            {
                                string location = refernce.GetValue("location").ToString();
                                location = Regex.Replace(location, @"(\[)", "");
                                location = Regex.Replace(location, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(location);
                                location = match.Groups[1].Value;
                                dt.Rows.Add("location", "<ldlPublisherLocation>" + location + "</ldlPublisherLocation>");

                            }
                            if (json.Contains("pages"))
                            {
                                string pages = refernce.GetValue("pages").ToString();
                                pages = Regex.Replace(pages, @"(\[)", "");
                                pages = Regex.Replace(pages, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(pages);
                                pages = match.Groups[1].Value;
                                string pagearange = Regex.Replace(pages, @"([0-9]+)(–)([0-9]+)", "<ldlFirstPageNumber>$1</ldlFirstPageNumber>$2<ldlLastPageNumber>$3</ldlLastPageNumber>");
                                dt.Rows.Add("pagerange", "<PageRange>" + pagearange + "</PageRange>");
                            }
                            if (json.Contains("url"))
                            {
                                string url = refernce.GetValue("url").ToString();
                                url = Regex.Replace(url, @"(\[)", "");
                                url = Regex.Replace(url, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(url);
                                url = match.Groups[1].Value;
                                dt.Rows.Add("url", "<Website>" + url + "</Website>");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\referencereport", ex.Message, true, "");
            }
            return dt;
        }
        static DataTable referencethesis(string json)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Reference Key", typeof(string));
            dt.Columns.Add("Reference value", typeof(string));
            try
            {
                var myList = new List<KeyValuePair<string, string>>();
                JArray reference_data = JArray.Parse(json);
                {
                    foreach (JObject refernce in reference_data)
                    {

                        string type = refernce.GetValue("type").ToString();
                        if (type == "thesis")
                        {


                            if (json.Contains("citation-number"))
                            {
                                string citationNumber = refernce.GetValue("citation-number").ToString();
                                citationNumber = Regex.Replace(citationNumber, @"(\[)", "");
                                citationNumber = Regex.Replace(citationNumber, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(citationNumber);
                                citationNumber = match.Groups[1].Value;
                                dt.Rows.Add("number", "<RefLabel>" + citationNumber + "</RefLabel>");

                            }
                            if (json.Contains("author"))
                            {
                                string authorGroup = refernce.GetValue("author").ToString();

                                JArray authorGroupitem = JArray.Parse(authorGroup);
                                {
                                    foreach (JObject item in authorGroupitem)
                                    {
                                        string surname = item.GetValue("family").ToString();
                                        string givenname = item.GetValue("given").ToString();

                                        dt.Rows.Add("author", "<Author>" + "<Surname>" + givenname + " </Surname> " + "<Forename>" + surname + "</Forename>" + "</Author>");
                                    }
                                }
                            }
                            if (json.Contains("date"))
                            {
                                string date = refernce.GetValue("date").ToString();
                                date = Regex.Replace(date, @"(\[)", "");
                                date = Regex.Replace(date, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(date);
                                date = match.Groups[1].Value;
                                dt.Rows.Add("year", "<PubDate>" + date + "</PubDate>");
                            }

                            if (json.Contains("title"))
                            {
                                string articleTitle = refernce.GetValue("title").ToString();
                                articleTitle = Regex.Replace(articleTitle, @"(\[)", "");
                                articleTitle = Regex.Replace(articleTitle, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(articleTitle);
                                articleTitle = match.Groups[1].Value;
                                dt.Rows.Add("articletitle", "<Article_Title>" + articleTitle + "</Article_Title>");

                            }
                            if (json.Contains("genre"))
                            {
                                string thesis = refernce.GetValue("genre").ToString();

                                thesis = Regex.Replace(thesis, @"(\[)", "");
                                thesis = Regex.Replace(thesis, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(thesis);
                                thesis = match.Groups[1].Value;
                                dt.Rows.Add("thesis", "<ldlThesisKeyword>" + thesis + "</ldlThesisKeyword>");

                            }
                            if (json.Contains("publisher"))
                            {
                                string publisher = refernce.GetValue("publisher").ToString();
                                publisher = Regex.Replace(publisher, @"(\[)", "");
                                publisher = Regex.Replace(publisher, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(publisher);
                                publisher = match.Groups[1].Value;
                                dt.Rows.Add("publisher", "<ldlPublisherName>" + publisher + "</ldlPublisherName>");

                            }
                            if (json.Contains("location"))
                            {
                                string location = refernce.GetValue("location").ToString();
                                location = Regex.Replace(location, @"(\[)", "");
                                location = Regex.Replace(location, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(location);
                                location = match.Groups[1].Value;
                                dt.Rows.Add("location", "<ldlPublisherLocation>" + location + "</ldlPublisherLocation>");

                            }
                            if (json.Contains("pages"))
                            {
                                string pages = refernce.GetValue("pages").ToString();
                                pages = Regex.Replace(pages, @"(\[)", "");
                                pages = Regex.Replace(pages, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(pages);
                                pages = match.Groups[1].Value;
                                string pagearange = Regex.Replace(pages, @"([0-9]+)(–)([0-9]+)", "<ldlFirstPageNumber>$1</ldlFirstPageNumber>$2<ldlLastPageNumber>$3</ldlLastPageNumber>");
                                dt.Rows.Add("pagerange", "<PageRange>" + pagearange + "</PageRange>");
                            }
                            if (json.Contains("url"))
                            {
                                string url = refernce.GetValue("url").ToString();
                                url = Regex.Replace(url, @"(\[)", "");
                                url = Regex.Replace(url, @"(\])", "");
                                Regex refdata = new Regex(@"(?:[\u0022]((?:(?!(?:[\u0022]|[\u0022])).)+)[\u0022])");
                                Match match = refdata.Match(url);
                                url = match.Groups[1].Value;
                                dt.Rows.Add("url", "<Website>" + url + "</Website>");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAPI.cs\referencethesis", ex.Message, true, "");
            }
            return dt;
        }
    }
}