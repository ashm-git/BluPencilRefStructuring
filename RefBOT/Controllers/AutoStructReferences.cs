using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Text.RegularExpressions;
using System.Data;
using Npgsql;
using System.Security.Policy;
using System.Security;

namespace RefBOT.Controllers
{

    public class clsStylesInformation
    {
        public bool bIsBold = false;
        public bool bIsItalic = false;
        public bool bIsUnderline = false;
        public bool bIsSmallCaps = false;
        public bool bIsAllCaps = false;
        public bool bIsHidden = false;
        public bool bIsSup = false;
        public bool bIsSub = false;
        public bool bIsInsterted = false;
        public bool bIsDeleted = false;
    }

    public class AutoStructReferences
    {
        public string[] arrRefRootTags = new string[] { "Journal", "Book", "References", "Conference", "Web", "Report", "Thesis" };
        public string[] arrRefGeneRootTags = new string[] { "ldlRefTypeJournal", "ldlRefTypeBook", "ldlRefTypeWeb", "ldlRefTypePaper", "ldlRefTypeConference", "ldlRefTypePatent", "ldlRefTypeReport", "ldlRefTypeThesis", "ldlRefTypeOther", "ldlRefTypeSubRef" };

        public string sNonKeyboardCharPattern = "[^a-z0-9~\\!\\@\\#\\$\\%\\^\\*\\(\\)\\\\\\-_\\+\\{\\}><\\[\\]:;,\\.\\/\\?\"\\' \\n\\r\\s]";
        public string sUnicode4DecimalUniConv = "[\u2013]";

        private bool CreateLogInformation(System.Data.DataTable dtLogInfo, ref Dictionary<string, string> dicLogInfoFiltered)
        {
            try
            {
                int nTotalRefs = dtLogInfo.Rows.Count;
                int nTotalJournalRefs = dtLogInfo.Select("RefType = 'ldlRefTypeJournal'").Length;
                int nTotalBookRefs = dtLogInfo.Select("RefType = 'ldlRefTypeBook'").Length;
                int nTotalWebRefs = dtLogInfo.Select("RefType = 'ldlRefTypeWeb'").Length;
                int nTotalPaperRefs = dtLogInfo.Select("RefType = 'ldlRefTypePaper'").Length;
                int nTotalConferenceRefs = dtLogInfo.Select("RefType = 'ldlRefTypeConference'").Length;
                int nTotalPatentRefs = dtLogInfo.Select("RefType = 'ldlRefTypePatent'").Length;
                int nTotalReportRefs = dtLogInfo.Select("RefType = 'ldlRefTypeReport'").Length;
                int nTotalThesisRefs = dtLogInfo.Select("RefType = 'ldlRefTypeThesis'").Length;
                int nTotalOtherRefs = dtLogInfo.Select("RefType = 'ldlRefTypeOther'").Length;

                StringBuilder sbRefTypeCount = new StringBuilder();
                StringBuilder sbRefTypePercentage = new StringBuilder();

                int nTotalRawRefWordCount = 0;
                int nTotalMatchedWordCount = 0;
                double nPercentage = 0.0;

                #region Appending sbTypeRefs
                if (nTotalJournalRefs > 0)
                {
                    sbRefTypeCount.Append(string.Format("Journal = {0}; ", nTotalJournalRefs.ToString()));

                    nTotalRawRefWordCount = dtLogInfo.Select("RefType = 'ldlRefTypeJournal'").AsEnumerable().Sum(x => x.Field<int>("TotalWords"));
                    nTotalMatchedWordCount = dtLogInfo.Select("RefType = 'ldlRefTypeJournal'").AsEnumerable().Sum(x => x.Field<int>("MatchedWords"));
                    nPercentage = ((double)nTotalMatchedWordCount / (double)nTotalRawRefWordCount) * 100;

                    sbRefTypePercentage.Append(string.Format("Journal = {0}; ", nPercentage.ToString("00.00")));
                }

                if (nTotalBookRefs > 0)
                {
                    sbRefTypeCount.Append(string.Format("Book = {0}; ", nTotalBookRefs));

                    nTotalRawRefWordCount = dtLogInfo.Select("RefType = 'ldlRefTypeBook'").AsEnumerable().Sum(x => x.Field<int>("TotalWords"));
                    nTotalMatchedWordCount = dtLogInfo.Select("RefType = 'ldlRefTypeBook'").AsEnumerable().Sum(x => x.Field<int>("MatchedWords"));
                    nPercentage = ((double)nTotalMatchedWordCount / (double)nTotalRawRefWordCount) * 100;

                    sbRefTypePercentage.Append(string.Format("Book = {0}; ", nPercentage.ToString("00.00")));
                }

                if (nTotalWebRefs > 0)
                {
                    sbRefTypeCount.Append(string.Format("Web = {0}; ", nTotalWebRefs));

                    nTotalRawRefWordCount = dtLogInfo.Select("RefType = 'ldlRefTypeWeb'").AsEnumerable().Sum(x => x.Field<int>("TotalWords"));
                    nTotalMatchedWordCount = dtLogInfo.Select("RefType = 'ldlRefTypeWeb'").AsEnumerable().Sum(x => x.Field<int>("MatchedWords"));
                    nPercentage = ((double)nTotalMatchedWordCount / (double)nTotalRawRefWordCount) * 100;

                    sbRefTypePercentage.Append(string.Format("Web = {0}; ", nPercentage.ToString("00.00")));
                }

                if (nTotalPaperRefs > 0)
                {
                    sbRefTypeCount.Append(string.Format("Paper = {0}; ", nTotalPaperRefs));

                    nTotalRawRefWordCount = dtLogInfo.Select("RefType = 'ldlRefTypePaper'").AsEnumerable().Sum(x => x.Field<int>("TotalWords"));
                    nTotalMatchedWordCount = dtLogInfo.Select("RefType = 'ldlRefTypePaper'").AsEnumerable().Sum(x => x.Field<int>("MatchedWords"));
                    nPercentage = ((double)nTotalMatchedWordCount / (double)nTotalRawRefWordCount) * 100;

                    sbRefTypePercentage.Append(string.Format("Paper = {0}; ", nPercentage.ToString("00.00")));
                }

                if (nTotalConferenceRefs > 0)
                {
                    sbRefTypeCount.Append(string.Format("Conference = {0}; ", nTotalConferenceRefs));

                    nTotalRawRefWordCount = dtLogInfo.Select("RefType = 'ldlRefTypeConference'").AsEnumerable().Sum(x => x.Field<int>("TotalWords"));
                    nTotalMatchedWordCount = dtLogInfo.Select("RefType = 'ldlRefTypeConference'").AsEnumerable().Sum(x => x.Field<int>("MatchedWords"));
                    nPercentage = ((double)nTotalMatchedWordCount / (double)nTotalRawRefWordCount) * 100;

                    sbRefTypePercentage.Append(string.Format("Conference = {0}; ", nPercentage.ToString("00.00")));
                }

                if (nTotalPatentRefs > 0)
                {
                    sbRefTypeCount.Append(string.Format("Patent = {0}; ", nTotalPatentRefs));

                    nTotalRawRefWordCount = dtLogInfo.Select("RefType = 'ldlRefTypePatent'").AsEnumerable().Sum(x => x.Field<int>("TotalWords"));
                    nTotalMatchedWordCount = dtLogInfo.Select("RefType = 'ldlRefTypePatent'").AsEnumerable().Sum(x => x.Field<int>("MatchedWords"));
                    nPercentage = ((double)nTotalMatchedWordCount / (double)nTotalRawRefWordCount) * 100;

                    sbRefTypePercentage.Append(string.Format("Patent = {0}; ", nPercentage.ToString("00.00")));
                }

                if (nTotalReportRefs > 0)
                {
                    sbRefTypeCount.Append(string.Format("Report = {0}; ", nTotalReportRefs));

                    nTotalRawRefWordCount = dtLogInfo.Select("RefType = 'ldlRefTypeReport'").AsEnumerable().Sum(x => x.Field<int>("TotalWords"));
                    nTotalMatchedWordCount = dtLogInfo.Select("RefType = 'ldlRefTypeReport'").AsEnumerable().Sum(x => x.Field<int>("MatchedWords"));
                    nPercentage = ((double)nTotalMatchedWordCount / (double)nTotalRawRefWordCount) * 100;

                    sbRefTypePercentage.Append(string.Format("Report = {0}; ", nPercentage.ToString("00.00")));
                }

                if (nTotalThesisRefs > 0)
                {
                    sbRefTypeCount.Append(string.Format("Thesis = {0}; ", nTotalThesisRefs));

                    nTotalRawRefWordCount = dtLogInfo.Select("RefType = 'ldlRefTypeThesis'").AsEnumerable().Sum(x => x.Field<int>("TotalWords"));
                    nTotalMatchedWordCount = dtLogInfo.Select("RefType = 'ldlRefTypeThesis'").AsEnumerable().Sum(x => x.Field<int>("MatchedWords"));
                    nPercentage = ((double)nTotalMatchedWordCount / (double)nTotalRawRefWordCount) * 100;

                    sbRefTypePercentage.Append(string.Format("Thesis = {0}; ", nPercentage.ToString("00.00")));
                }

                if (nTotalOtherRefs > 0)
                {
                    sbRefTypeCount.Append(string.Format("Other = {0}; ", nTotalOtherRefs.ToString()));
                }
                #endregion

                nTotalRawRefWordCount = dtLogInfo.AsEnumerable().Sum(x => x.Field<int>("TotalWords"));
                nTotalMatchedWordCount = dtLogInfo.AsEnumerable().Sum(x => x.Field<int>("MatchedWords"));
                nPercentage = ((double)nTotalMatchedWordCount / (double)nTotalRawRefWordCount) * 100;

                //int sTotalPercentage = dtLogInfo.AsEnumerable().Sum(x => x.Field<int>("MatchPercentage"));
                //double dAverangePercentage = (double)sTotalPercentage / (double)sTotalRefs;
                //Total References: 5, Type of References: Journal = 3; Book = 2; Others=1; Quality: Journal = 100%; Book = 90%; Others = 50%; Process Type: Selection.

                string sRefLogInfo1 = string.Format("Total References: {0}, Type of References: {1}, Quality: {2}, Process Type: {3}",
                    nTotalRefs.ToString(),
                    sbRefTypeCount.ToString().Trim(),
                    sbRefTypePercentage.ToString().Trim(),
                    "Selection");

                string sRefLogInfo2 = string.Empty;

                //if (System.Environment.UserName.ToLower().Equals("dakshinamoorthy.g"))
                //{
                //    sRefLogInfo2 = string.Format("Identified Percentage: {0} / {1} = {2}%", nTotalMatchedWordCount, nTotalRawRefWordCount, nPercentage.ToString("00.00"));
                //}
                //else
                //{
                //    sRefLogInfo2 = string.Format("Identified Percentage: {0}%", nPercentage.ToString("00.00"));
                //}

                sRefLogInfo2 = string.Format("Identified Percentage: {0}%", nPercentage.ToString("00.00"));

                dicLogInfoFiltered.Add("RefLogInfo1", sRefLogInfo1);
                dicLogInfoFiltered.Add("RefLogInfo2", sRefLogInfo2);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\CreateLogInformation", ex.Message, true, "");
            }
            return true;
        }

        private string DoHexaDecimalUnicodeConversion(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, sNonKeyboardCharPattern, HexaEntityConversion, RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\DoHexaDecimalUnicodeConversion", ex.Message, true, "");
            }
            return sRefContent;
        }


        public string DoReferenceConversion_File(string sRefContent, string sUserName, string sBatchId, string sDocFullPath, string sSelectionMode, string sAppName, string sAppVersion, string sCustomerName)
        {
            string sOutputContent_File = sRefContent;
            StringBuilder sbOutputCotent = new StringBuilder();

            try
            {
                if (!string.IsNullOrEmpty(sRefContent.Trim()))
                {
                    sRefContent = sRefContent.Trim();
                    List<string> lstInputReferences = Regex.Split(sRefContent, "[\n\r]+", RegexOptions.Singleline).ToList();
                    foreach (string sEachInputRef in lstInputReferences)
                    {
                        sbOutputCotent.AppendLine(DoReferenceConversion(sEachInputRef, sUserName, sBatchId, sDocFullPath, sSelectionMode, sAppName, sAppVersion, sCustomerName));
                    }

                    sOutputContent_File = sbOutputCotent.ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\DoReferenceConversion_File", ex.Message, true, "");
            }
            return sOutputContent_File;
        }

        public string DoReferenceConversion(string sRefContent, string sUserName, string sBatchId, string sDocFullPath, string sSelectionMode, string sAppName, string sAppVersion, string sCustomerName)
        {
            string sRefTaggedContent = string.Empty;
            string sOutputRefContent = string.Empty;
            string sRefContent4DB = string.Empty;

            //added by Dakshinamoorthy on 2020-Nov-24
            string sprogramFiles = General.ProgramFiles;
            File.AppendAllText(@"D:\Projects\RefBOTDev\Log\Log.txt", sprogramFiles);
            General.GeneralInstance.CustomerName = sCustomerName;

            string sInputRefContent = sRefContent;
            try
            {
                if (!string.IsNullOrEmpty(sInputRefContent))
                {
                    sOutputRefContent = sInputRefContent.Trim();
                    AutoStructReferencesHelper objRefHelper = new AutoStructReferencesHelper();
                    string sQuery = string.Empty;
                    string sOutputRefType = string.Empty;
                    string sOutputPercentage = string.Empty;
                    string sRefInput = string.Empty;
                    int nTotalWords = 0;
                    int nMatchedWords = 0;

                    DateTime startTime = DateTime.Now;

                    sRefTaggedContent = sInputRefContent.Trim();
                    sRefInput = sInputRefContent.Trim();
                    sRefContent4DB = sRefInput;
                    sRefInput = HexUnicodeToCharConvertor(sRefInput);
                    sRefTaggedContent = HexUnicodeToCharConvertor(sRefTaggedContent);

                    Dictionary<string, string> dicLogInfo = new Dictionary<string, string>();
                    sRefTaggedContent = objRefHelper.ImproveLocalConversionWithSubRef(sRefInput, sRefTaggedContent, ref dicLogInfo);

                    //updated by Dakshinamoorthy on 2020-Dec-04
                    if (dicLogInfo.ContainsKey("RefTypeForAPI"))
                    {
                        sOutputRefType = dicLogInfo["RefTypeForAPI"].ToString();
                    }
                    else
                    {
                        if (dicLogInfo.ContainsKey("RefType"))
                        {
                            sOutputRefType = dicLogInfo["RefType"].ToString();
                        }
                        else
                        {
                            sOutputRefType = "Other";
                        }
                    }

                    if (dicLogInfo.ContainsKey("MatchedPercentage"))
                    {
                        sOutputPercentage = dicLogInfo["MatchedPercentage"].ToString();
                    }
                    else
                    {
                        sOutputPercentage = "0";
                    }

                    if (dicLogInfo.ContainsKey("TotalWords"))
                    {
                        nTotalWords = Convert.ToInt32(dicLogInfo["TotalWords"].ToString());
                    }
                    else
                    {
                        nTotalWords = 0;
                    }

                    if (dicLogInfo.ContainsKey("MatchedWords"))
                    {
                        nMatchedWords = Convert.ToInt32(dicLogInfo["MatchedWords"].ToString());
                    }
                    else
                    {
                        nMatchedWords = 0;
                    }

                    //added by Dakshinamoorthy on 2020-Apr-13
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:</?LQ_[0-9A-Z]{6}>)", HandleQueryTag);
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:<(authorQueryOpen|authorQuerySelf|authorQueryClose|procIntruc)>((?:(?!</?\1).)+)</\1>)", HandleAuthorQuery);

                    //commented by Dakshinamoorthy on 2019-June-14
                    //sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<(b|i|u|sup|sub|ac|sc|hi)>", "&lt;$1&gt;");
                    //sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</(b|i|u|sup|sub|ac|sc|hi)>", "&lt;/$1&gt;");

                    //added by Dakshinamoorthy on 2019-June-14
                    sRefTaggedContent = GroupAuthorEditor(sRefTaggedContent);

                    //added by Dakshinamoorthy on 2020-Nov-24
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"([ ]*<ldlAuthorDelimiterAnd>&#x0026;</ldlAuthorDelimiterAnd>)(</ldlAuthor>)", "$2$1");
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"([ ]*<ldlEditorDelimiterAnd>&#x0026;</ldlEditorDelimiterAnd>)(</ldlEditor>)", "$2$1");

                    //added by Dakshinamoorthy on 2020-Nov-17
                    #region Updated_Code
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(<(ldlAuthorGivenName|ldlEditorGivenName)>(?:(?:(?!</?\2).)+)</\2>),</ldlAuthor>", "$1</ldlAuthor>,");
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(<(ldlAuthorGivenName|ldlEditorGivenName)>(?:(?:(?!</?\2).)+)</\2>)\.</ldlAuthor>", "$1</ldlAuthor>.");
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(<(ldlAuthorGivenName|ldlEditorGivenName)>(?:(?:(?!</?\2).)+)</\2>), (<(ldlAuthorDelimiterAnd|ldlEditorDelimiterChar)>(?:(?!</?\4>).)+</\4>)</ldlAuthor>", "$1</ldlAuthor>, $3");
                    #endregion

                    sRefTaggedContent = ApplicationSpecificTagMapping(sRefTaggedContent, sAppName);

                    DateTime endTime = DateTime.Now;
                    int totalSeconds = Convert.ToInt32((endTime - startTime).TotalSeconds);

                    //Write log information to Database
                    sQuery = string.Format("insert into ref_struct_log(username, doc_path, batch_id, selection_type, start_time, end_time, ref_type, identified_percent, input_content, output_content, software_name, software_version)  values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}')", EscapeApostrophe(sUserName), EscapeApostrophe(sDocFullPath), EscapeApostrophe(sBatchId), EscapeApostrophe(sSelectionMode), EscapeApostrophe(startTime.ToString("yyyy-MM-dd HH:mm:ss")), EscapeApostrophe(endTime.ToString("yyyy-MM-dd HH:mm:ss")), EscapeApostrophe(GetRefTypeFromGenericType(sOutputRefType)), EscapeApostrophe(Convert.ToInt32(sOutputPercentage).ToString()), EscapeApostrophe(sRefContent4DB), EscapeApostrophe(sRefTaggedContent), EscapeApostrophe(sAppName), EscapeApostrophe(sAppVersion));
                    Database.GetInstance.ExecuteNonQueryFunction(sQuery);
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\ConvertRefWithInternalTool", ex.Message, true, "");
            }

            return sRefTaggedContent;
        }

        private string HandleAuthorQuery(Match aqMatch)
        {
            string sOutputContent = aqMatch.Groups[2].Value;
            try
            {
                sOutputContent = HexUnicodeToCharConvertor(sOutputContent);
                AutoStructReferencesHelper objRefHelper = new AutoStructReferencesHelper();
                sOutputContent = objRefHelper.DecodeFrom64(sOutputContent);

                //this is not applicable for RefBOT API
                //sOutputContent = sOutputContent.Replace("<", "&lt;").Replace(">", "&gt;");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\HandleAuthorQuery", ex.Message, true, "");
            }
            return sOutputContent;
        }


        //added by Dakshinamoorthy on 2020-Apr-13
        private string HandleQueryTag(Match myQueryTagMatch)
        {
            string sReturnValue = myQueryTagMatch.Value.ToString();
            try
            {
                sReturnValue = sReturnValue.Replace("<", "&lt;").Replace(">", "&gt;");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\HandleQueryTag", ex.Message, true, "");
            }
            return sReturnValue;
        }

        private string GroupAuthorEditor(string sRefTaggedContent)
        {
            try
            {
                string sAUG_Pattern = @"(?:(?:(?:<ldlAuthor>(?:(?:(?!</?ldlAuthor>).)+)</ldlAuthor>(?:(?:<(?:sb|i|b|u|sup|sub)>)?(?:[\:\.,; ]+)(?:</(?:sb|i|b|u|sup|sub)>)?))+)(?:(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>(?:(?:<(?:sb|i|b|u|sup|sub)>)?(?:[\:\.,; ]+)(?:</(?:sb|i|b|u|sup|sub)>)?))(?:(?:<ldlAuthor>(?:(?:(?!</?ldlAuthor>).)+)</ldlAuthor>(?:(?:<(?:sb|i|b|u|sup|sub)>)?(?:[\:\.,; ]+)(?:</(?:sb|i|b|u|sup|sub)>)?))+))*)";
                string sEDG_Pattern = @"(?:(?:(?:<ldlEditor>(?:(?:(?!</?ldlEditor>).)+)</ldlEditor>(?:(?:<(?:sb|i|b|u|sup|sub)>)?(?:[\:\.,; ]+)(?:</(?:sb|i|b|u|sup|sub)>)?))+)(?:(?:<ldlEditorDelimiterAnd>(?:(?!</?ldlEditorDelimiterAnd>).)+</ldlEditorDelimiterAnd>(?:(?:<(?:sb|i|b|u|sup|sub)>)?(?:[\:\.,; ]+)(?:</(?:sb|i|b|u|sup|sub)>)?))(?:(?:<ldlEditor>(?:(?:(?!</?ldlEditor>).)+)</ldlEditor>(?:(?:<(?:sb|i|b|u|sup|sub)>)?(?:[\:\.,; ]+)(?:</(?:sb|i|b|u|sup|sub)>)?))+))*)";

                sRefTaggedContent = Regex.Replace(sRefTaggedContent, sAUG_Pattern, string.Format("<{0}>{1}</{0}>", "ldlAuthorGroup", "$&"));
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, sEDG_Pattern, string.Format("<{0}>{1}</{0}>", "ldlEditorGroup", "$&"));
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "[ ]+(</(?:ldlAuthorGroup|ldlEditorGroup)>)", "$1 ");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\GroupAuthorEditor", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }


        private string ApplicationSpecificTagMapping(string sRefTaggedContent, string sApplicationName)
        {
            try
            {
                if (sApplicationName.ToLower().Equals("blupencil"))
                {

                    Dictionary<string, string> dicBluPencilTagMapping = new Dictionary<string, string>();
                    //Label
                    dicBluPencilTagMapping.Add("ldlRefLabel", "label");

                    //Ref Type
                    dicBluPencilTagMapping.Add("ldlRefTypeBook", "refbook");
                    dicBluPencilTagMapping.Add("ldlRefTypeConference", "refconference");
                    //dicBluPencilTagMapping.Add("", "refcommunication"); -> Check
                    dicBluPencilTagMapping.Add("ldlRefTypeJournal", "refjournal");
                    dicBluPencilTagMapping.Add("ldlRefTypeOther", "refother");
                    dicBluPencilTagMapping.Add("ldlRefTypePaper", "refpaper"); //check
                    dicBluPencilTagMapping.Add("ldlRefTypePatent", "refpatent");
                    dicBluPencilTagMapping.Add("ldlRefTypeReport", "refreport");
                    dicBluPencilTagMapping.Add("ldlRefTypeSubRef", "subref");
                    dicBluPencilTagMapping.Add("ldlRefTypeThesis", "refthesis");
                    dicBluPencilTagMapping.Add("ldlRefTypeWeb", "refweb");

                    dicBluPencilTagMapping.Add("ldlSubRefTypeBook", "subrefbook");
                    dicBluPencilTagMapping.Add("ldlSubRefTypeConference", "subrefconference"); //check
                    dicBluPencilTagMapping.Add("ldlSubRefTypeJournal", "subrefjournal");
                    dicBluPencilTagMapping.Add("ldlSubRefTypeOther", "subrefother");
                    dicBluPencilTagMapping.Add("ldlSubRefTypePaper", "subrefpaper"); //check
                    dicBluPencilTagMapping.Add("ldlSubRefTypePatent", "subrefpatent");
                    dicBluPencilTagMapping.Add("ldlSubRefTypeReport", "subrefreport"); //check
                    dicBluPencilTagMapping.Add("ldlSubRefTypeThesis", "subrefthesis");
                    dicBluPencilTagMapping.Add("ldlSubRefTypeWeb", "subrefweb");

                    //Formatting Tag
                    //dicBluPencilTagMapping.Add("b", "bold");
                    //dicBluPencilTagMapping.Add("i", "italic");
                    //dicBluPencilTagMapping.Add("sub", "sub");
                    //dicBluPencilTagMapping.Add("sup", "sup");
                    //dicBluPencilTagMapping.Add("sc", "sc"); //check

                    //Title
                    dicBluPencilTagMapping.Add("ldlArticleTitle", "articletitle");
                    dicBluPencilTagMapping.Add("ldlBookTitle", "bktitle");
                    dicBluPencilTagMapping.Add("ldlChapterTitle", "chaptertitle");
                    dicBluPencilTagMapping.Add("ldlJournalTitle", "jrtitle");
                    dicBluPencilTagMapping.Add("ldlSourceTitle", "source");

                    //Author and Editor Tags
                    dicBluPencilTagMapping.Add("ldlAuthorGroup", "authors");
                    dicBluPencilTagMapping.Add("ldlEditorGroup", "editors");
                    dicBluPencilTagMapping.Add("ldlCollab", "collab");
                    dicBluPencilTagMapping.Add("ldlEditor", "editor");
                    dicBluPencilTagMapping.Add("ldlAuthor", "author");
                    dicBluPencilTagMapping.Add("ldlEditorGivenName", "egivenname");
                    dicBluPencilTagMapping.Add("ldlEditorSurName", "esurname");
                    dicBluPencilTagMapping.Add("ldlEditorSuffix", "esuffix");
                    dicBluPencilTagMapping.Add("ldlAuthorGivenName", "givenname");
                    dicBluPencilTagMapping.Add("ldlAuthorSurName", "surname");
                    dicBluPencilTagMapping.Add("ldlAuthorSuffix", "suffix");
                    dicBluPencilTagMapping.Add("ldlAuthorDelimiterEtal", "etal");
                    dicBluPencilTagMapping.Add("ldlEditorDelimiterEtal", "etal");
                    dicBluPencilTagMapping.Add("ldlAuthorDelimiterChar", "connect");
                    dicBluPencilTagMapping.Add("ldlEditorDelimiterChar", "connect");
                    dicBluPencilTagMapping.Add("ldlEditorDelimiterEds", "");
                    dicBluPencilTagMapping.Add("ldlEditorDelimiterEds_Back", "");
                    dicBluPencilTagMapping.Add("ldlEditorDelimiterEds_Front", "");
                    dicBluPencilTagMapping.Add("ldlAuthorDelimiterAnd", "");

                    //Conference Type Tags
                    dicBluPencilTagMapping.Add("ldlConferenceDate", "confdate");
                    dicBluPencilTagMapping.Add("ldlConferenceDay", "");
                    dicBluPencilTagMapping.Add("ldlConferenceMonth", "");
                    dicBluPencilTagMapping.Add("ldlConferenceYear", "");
                    dicBluPencilTagMapping.Add("ldlConferenceLocation", "confloc");
                    dicBluPencilTagMapping.Add("ldlConferenceName", "confname");
                    dicBluPencilTagMapping.Add("ldlConferenceSponsor", "confsponsor");

                    //Publisher Tags
                    dicBluPencilTagMapping.Add("ldlInstitutionName", "institution");
                    dicBluPencilTagMapping.Add("ldlPublisherLocation", "publisherloc");
                    dicBluPencilTagMapping.Add("ldlPublisherName", "publishername");

                    //Date Tags
                    dicBluPencilTagMapping.Add("ldlPublicationDay", "day");
                    dicBluPencilTagMapping.Add("ldlPublicationMonth", "month");
                    dicBluPencilTagMapping.Add("ldlPublicationYear", "year");
                    dicBluPencilTagMapping.Add("ldlSeason", "unknownprefix");

                    //Others
                    //sRefTaggedContent = Regex.Replace(sRefTaggedContent, "", "<coden>");
                    //dicBluPencilTagMapping.Add("", "comment");
                    dicBluPencilTagMapping.Add("ldlDOINumber", "doi");
                    //dicBluPencilTagMapping.Add("", "doicross");
                    dicBluPencilTagMapping.Add("ldlEditionNumber", "edition");
                    dicBluPencilTagMapping.Add("ldlELocationId", "elocationid");
                    dicBluPencilTagMapping.Add("ldlEmail", "");
                    dicBluPencilTagMapping.Add("ldlFirstPageNumber", "fpage");
                    //dicBluPencilTagMapping.Add("", "issn");
                    dicBluPencilTagMapping.Add("ldlIssueNumber", "issue");

                    dicBluPencilTagMapping.Add("ldlLastPageNumber", "lpage");
                    //dicBluPencilTagMapping.Add("", "patentnumber");
                    dicBluPencilTagMapping.Add("ldlPubMedIdNumber", "pmid");
                    //dicBluPencilTagMapping.Add("", "prefix");
                    //dicBluPencilTagMapping.Add("", "pubid");
                    //dicBluPencilTagMapping.Add("", "pubidother");
                    //dicBluPencilTagMapping.Add("", "series");
                    dicBluPencilTagMapping.Add("ldlReportKeyword", "std");
                    dicBluPencilTagMapping.Add("ldlReportNumber", "unknowncomment");
                    dicBluPencilTagMapping.Add("ldlThesisKeyword", "unknowncomment");
                    dicBluPencilTagMapping.Add("ldlUnknownElement", "unidentified");
                    dicBluPencilTagMapping.Add("ldlMisc", "unknowncomment");
                    dicBluPencilTagMapping.Add("ldlVolumeNumber", "volume");
                    dicBluPencilTagMapping.Add("ldlURL", "website");
                    dicBluPencilTagMapping.Add("ldlAccessedDate", "");
                    dicBluPencilTagMapping.Add("ldlAccessedDateLabel", "");
                    dicBluPencilTagMapping.Add("ldlAccessedDay", "day");
                    dicBluPencilTagMapping.Add("ldlAccessedMonth", "month");
                    dicBluPencilTagMapping.Add("ldlAccessedYear", "year");
                    dicBluPencilTagMapping.Add("ldlUpdatedDay", "day");
                    dicBluPencilTagMapping.Add("ldlUpdatedMonth", "month");
                    dicBluPencilTagMapping.Add("ldlUpdatedYear", "year");
                    dicBluPencilTagMapping.Add("ldlTestingLog", "");

                    //added by Dakshinamoorthy on 2019-Dec-23
                    dicBluPencilTagMapping.Add("ldlTitleLabel", "");
                    dicBluPencilTagMapping.Add("ldlURLLabel", "");
                    dicBluPencilTagMapping.Add("ldlEmailLabel", "");
                    dicBluPencilTagMapping.Add("ldlVolumeLabel", "");
                    dicBluPencilTagMapping.Add("ldlIssueLabel", "");
                    dicBluPencilTagMapping.Add("ldlEditionLabel", "");
                    dicBluPencilTagMapping.Add("ldlPageLabel", "");
                    dicBluPencilTagMapping.Add("ldlDOILabel", "");
                    dicBluPencilTagMapping.Add("ldlPubMedIdLabel", "");
                    dicBluPencilTagMapping.Add("ldlISBNLabel", "");
                    dicBluPencilTagMapping.Add("ldlISSNLabel", "");

                    string sReplaceContent_Open = string.Empty;
                    string sReplaceContent_Close = string.Empty;

                    foreach (var eachReplacement in dicBluPencilTagMapping)
                    {
                        sReplaceContent_Open = string.Empty;
                        sReplaceContent_Close = string.Empty;

                        if (string.IsNullOrEmpty(eachReplacement.Key.ToString().Trim()) == false)
                        {
                            if(string.IsNullOrEmpty(eachReplacement.Value.ToString().Trim()) == true)
                            {
                                sReplaceContent_Open = "";
                                sReplaceContent_Close = "";
                            }
                            else
                            {
                                sReplaceContent_Open = string.Format("<{0}>", eachReplacement.Value);
                                sReplaceContent_Close = string.Format("</{0}>", eachReplacement.Value);
                            }

                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, string.Format("<{0}>", eachReplacement.Key), sReplaceContent_Open);
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, string.Format("</{0}>", eachReplacement.Key), sReplaceContent_Close);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\ApplicationSpecificTagMapping", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }


        private string EscapeApostrophe(string sContent)
        {
            try
            {
                sContent = Regex.Replace(sContent, "[']", "''");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\ConvertRefWithInternalTool", ex.Message, true, "");
            }
            return sContent;
        }


        private string GetRefTypeFromGenericType(string sRefType)
        {
            string sRefGenericType = string.Empty;
            try
            {
                //updated by Dakshinamoorthy on 2020-Dec-04
                if (sRefType.Contains("-") && Regex.IsMatch(sRefType, @"(?:\-[0-9]+)$"))
                {
                    return sRefType;
                }

                switch (sRefType)
                {
                    case "ldlRefTypeJournal":
                        sRefGenericType = "Journal";
                        break;
                    case "ldlRefTypeBook":
                        sRefGenericType = "Book";
                        break;
                    case "ldlRefTypeConference":
                        sRefGenericType = "Conference";
                        break;
                    case "ldlRefTypeWeb":
                        sRefGenericType = "Web";
                        break;
                    case "ldlRefTypeReport":
                        sRefGenericType = "Report";
                        break;
                    case "ldlRefTypeThesis":
                        sRefGenericType = "Thesis";
                        break;

                    //added by Dakshinamoorthy on 2019-Feb-07
                    case "ldlRefTypeSubRef":
                        sRefGenericType = "SubRef";
                        break;

                    case "ldlRefTypeOther":
                    default:
                        sRefGenericType = "Other";
                        break;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\GetGenericRefType_LocalExe", ex.Message, true, "");
            }
            return sRefGenericType;
        }


        private string HandleRefLabel(string sRefTaggedContent)
        {
            string sOutputContent = string.Empty;
            string sRefRootTagPattern = string.Format("(?:{0})", string.Join("|", arrRefRootTags));

            try
            {
                if (Regex.IsMatch(sRefTaggedContent, string.Format("^(?:<{0}>)", sRefRootTagPattern)))
                {
                    sOutputContent = sRefTaggedContent;
                }
                else if (Regex.IsMatch(sRefTaggedContent, string.Format("(?:<{0}>)", sRefRootTagPattern)))
                {
                    sOutputContent = Regex.Replace(sRefTaggedContent, string.Format("^((?:[^<> ]+)[ ]*)(<{0}>)", sRefRootTagPattern), "$2<Label>$1</Label>");
                    sOutputContent = Regex.Replace(sOutputContent, "([ ]+)</Label>", "</Label>$1");
                }
                else
                {
                    sOutputContent = string.Format("<Other>{0}</Other>", sOutputContent);
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\sRefTaggedContent", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private string ConvertDecimalToHexDecimal(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "&#([0-9]+);", Decimal2Hexa, RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\ConvertDecimalToHexDecimal", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string Decimal2Hexa(Match myDecimalMatch)
        {
            string sOutputContent = myDecimalMatch.Groups[1].Value.ToString();
            try
            {
                int number = int.Parse(sOutputContent);
                string hex = number.ToString("x");
                sOutputContent = string.Format("&#x{0};", hex);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\Decimal2Hexa", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private string HexUnicodeToCharConvertor(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "&#x([A-F0-9]+);", Hex2Char, RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\HexUnicodeToCharConvertor", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string Hex2Char(Match myHexMatch)
        {
            string sHexValue = myHexMatch.Groups[1].Value.ToString();
            try
            {
                sHexValue = ((char)Convert.ToInt32(sHexValue, 16)).ToString();
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\Hex2Char", ex.Message, true, "");
            }
            return sHexValue;
        }

        private string GetStyleIDFromTagName(string sTagName)
        {
            string sStyleID = string.Empty;
            try
            {
                switch (sTagName)
                {
                    case "ldlRefTypeJournal": sStyleID = "RefJournal"; break;
                    case "ldlRefTypeBook": sStyleID = "RefBook"; break;
                    case "ldlRefTypeWeb": sStyleID = "RefWeb"; break;
                    case "ldlRefTypePaper": sStyleID = "RefPaper"; break;
                    case "ldlRefTypeConference": sStyleID = "RefConference"; break;
                    case "ldlRefTypePatent": sStyleID = "RefPatent"; break;
                    case "ldlRefTypeReport": sStyleID = "RefReport"; break;
                    case "ldlRefTypeThesis": sStyleID = "RefThesis"; break;
                    case "ldlRefTypeOther": sStyleID = "RefOther"; break;

                    //added by Dakshinamoorthy on 2019-Feb-07
                    case "ldlRefTypeSubRef": sStyleID = "SubRef"; break;
                    case "ldlSubRefTypeJournal": sStyleID = "subRefJournal"; break;
                    case "ldlSubRefTypeBook": sStyleID = "subRefBook"; break;
                    case "ldlSubRefTypeWeb": sStyleID = "subRefWeb"; break;
                    case "ldlSubRefTypePaper": sStyleID = "subRefPaper"; break;
                    case "ldlSubRefTypeConference": sStyleID = "subRefConference"; break;
                    case "ldlSubRefTypePatent": sStyleID = "subRefPatent"; break;
                    case "ldlSubRefTypeReport": sStyleID = "subRefReport"; break;
                    case "ldlSubRefTypeThesis": sStyleID = "subRefThesis"; break;
                    case "ldlSubRefTypeOther": sStyleID = "subRefOther"; break;

                    case "ldlUnknownElement": sStyleID = "unidentified"; break;
                    case "ldlRefLabel": sStyleID = "label"; break;
                    case "ldlAuthorInPrevoiusRef": sStyleID = ""; break;
                    case "ldlCollab": sStyleID = "collab"; break;
                    case "ldlAuthorSurName": sStyleID = "surname"; break;
                    case "ldlAuthorGivenName": sStyleID = "givenName"; break;
                    case "ldlAuthorSuffix": sStyleID = "suffix"; break;
                    case "ldlAuthorDelimiterEtal": sStyleID = "etal"; break;
                    case "ldlAuthorDelimiterAnd": sStyleID = ""; break;
                    case "ldlEditorSurName": sStyleID = "eSurname"; break;
                    case "ldlEditorGivenName": sStyleID = "eGivenName"; break;
                    case "ldlEditorSuffix": sStyleID = "suffix"; break;
                    case "ldlEditorDelimiterEtal": sStyleID = "etal"; break;
                    case "ldlEditorDelimiterAnd": sStyleID = ""; break;
                    case "ldlEditorDelimiterEds": sStyleID = ""; break;
                    case "ldlEditorDelimiterEds_Front": sStyleID = "edsFront"; break;
                    case "ldlEditorDelimiterEds_Back": sStyleID = "edsBack"; break;


                    case "ldlArticleTitle": sStyleID = "articleTitle"; break;
                    case "ldlJournalTitle": sStyleID = "jrTitle"; break;
                    case "ldlChapterTitle": sStyleID = "chapterTitle"; break;
                    case "ldlBookTitle": sStyleID = "bkTitle"; break;
                    case "ldlSourceTitle": sStyleID = "source"; break;
                    case "ldlConferenceName": sStyleID = "confName"; break;
                    case "ldlConferenceLocation": sStyleID = "confLoc"; break;
                    case "ldlConferenceSponsor": sStyleID = "confSponsor"; break;
                    case "ldlConferenceDate": sStyleID = "confDate"; break;

                    case "ldlPublicationYear": sStyleID = "year"; break;
                    case "ldlPublicationMonth": sStyleID = "month"; break;
                    case "ldlPublicationDay": sStyleID = "day"; break;
                    case "ldlAccessedYear": sStyleID = "year"; break;
                    case "ldlAccessedMonth": sStyleID = "month"; break;
                    case "ldlAccessedDay": sStyleID = "day"; break;
                    case "ldlUpdatedYear": sStyleID = "year"; break;
                    case "ldlUpdatedMonth": sStyleID = "month"; break;
                    case "ldlUpdatedDay": sStyleID = "day"; break;

                    case "ldlConferenceYear": sStyleID = "year"; break;
                    case "ldlConferenceMonth": sStyleID = "month"; break;
                    case "ldlConferenceDay": sStyleID = "day"; break;
                    case "ldlSeason": sStyleID = "unknownPrefix"; break;

                    case "ldlAccessedDate": sStyleID = "accessDate"; break;
                    case "ldlAccessedDateLabel": sStyleID = "accessDate"; break;

                    //case "ldlTitleLabel": sStyleID = ""; break;
                    //case "ldlURLLabel": sStyleID = ""; break;
                    //case "ldlEmailLabel": sStyleID = ""; break;
                    //case "ldlVolumeLabel": sStyleID = ""; break;
                    //case "ldlIssueLabel": sStyleID = ""; break;
                    //case "ldlEditionLabel": sStyleID = ""; break;
                    //case "ldlPageLabel": sStyleID = ""; break;
                    //case "ldlDOILabel": sStyleID = ""; break;
                    //case "ldlPubMedIdLabel": sStyleID = ""; break;
                    //case "ldlISBNLabel": sStyleID = ""; break;
                    //case "ldlISSNLabel": sStyleID = ""; break;

                    case "ldlVolumeNumber": sStyleID = "volume"; break;
                    case "ldlIssueNumber": sStyleID = "issue"; break;
                    case "ldlEditionNumber": sStyleID = "edition"; break;
                    case "ldlFirstPageNumber": sStyleID = "fPage"; break;
                    case "ldlLastPageNumber": sStyleID = "lPage"; break;
                    case "ldlPageRange": sStyleID = ""; break;
                    case "ldlDOINumber": sStyleID = "doi"; break;
                    case "ldlPubMedIdNumber": sStyleID = "pmId"; break;
                    case "ldlISBNNumber": sStyleID = ""; break;
                    case "ldlISSNNumber": sStyleID = ""; break;
                    case "ldlURL": sStyleID = "website"; break;
                    case "ldlEmail": sStyleID = "email"; break;
                    case "ldlPublisherName": sStyleID = "publisherName"; break;
                    case "ldlInstitutionName": sStyleID = "institution"; break;
                    case "ldlELocationId": sStyleID = "eLocationId"; break;

                    case "ldlPublisherLocation": sStyleID = "publisherLoc"; break;
                    case "ldlCity": sStyleID = ""; break;
                    case "ldlState": sStyleID = ""; break;
                    case "ldlCountry": sStyleID = ""; break;

                    case "ldlReportNumber": sStyleID = "unknownComment"; break;
                    case "ldlThesisKeyword": sStyleID = "unknownComment"; break;
                    case "ldlReportKeyword": sStyleID = "std"; break;
                    case "ldlMisc": sStyleID = "unknownComment"; break;

                    case "ldlTestingLog": sStyleID = "testingLog"; break;
                    //case "ldlAuthorDelimiterChar": sStyleID = "auDelimiter"; break;
                    //case "ldlEditorDelimiterChar": sStyleID = "edDelimiter"; break;

                    case "ldlAuthorDelimiterChar": sStyleID = "connect"; break;
                    case "ldlEditorDelimiterChar": sStyleID = "connect"; break;

                    default:
                        sStyleID = "Normal";
                        break;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\GetStyleIDFromTagName", ex.Message, true, "");
            }
            return sStyleID;
        }

        private string GetStyleIDFromTagName_old(string sTagName)
        {
            string sStyleID = string.Empty;
            try
            {
                switch (sTagName)
                {
                    //reference label
                    case "Label": sStyleID = "label"; break;

                    //reference types
                    case "Book": sStyleID = "RefBook"; break;
                    case "Communication": sStyleID = "RefCommunication"; break;
                    case "Conference": sStyleID = "RefConference"; break;
                    case "Journal": sStyleID = "RefJournal"; break;
                    case "Other":
                    case "References":
                        sStyleID = "RefOther";
                        break;
                    case "Patent": sStyleID = "RefPatent"; break;
                    case "Report": sStyleID = "RefReport"; break;
                    case "Thesis": sStyleID = "RefThesis"; break;
                    case "Web": sStyleID = "RefWeb"; break;

                    //title information
                    case "Article_Title": sStyleID = "articleTitle"; break;
                    case "Journal_Title": sStyleID = "source"; break;
                    case "chapter-title": sStyleID = "chapterTitle"; break;
                    case "source": sStyleID = "source"; break;

                    //author/editor name elements
                    case "Surname": sStyleID = "surname"; break;
                    case "Forename": sStyleID = "givenName"; break;
                    case "Collab": sStyleID = "collab"; break;
                    case "Suffix": sStyleID = "suffix"; break;
                    case "Etal": sStyleID = "etal"; break;
                    case "ESurname": sStyleID = "eSurname"; break;
                    case "EForename": sStyleID = "eGivenName"; break;
                    case "EGivenname": sStyleID = "eGivenName"; break;

                    case "Year": sStyleID = "year"; break;
                    case "day": sStyleID = "day"; break;
                    case "month": sStyleID = "month"; break;

                    case "Vol_No": sStyleID = "volume"; break;
                    case "Issue_No": sStyleID = "issue"; break;
                    case "FirstPage": sStyleID = "fPage"; break;
                    case "LastPage": sStyleID = "lPage"; break;
                    case "Edition": sStyleID = "edition"; break;

                    case "doi-num": sStyleID = "doi"; break;
                    case "Doi": sStyleID = "doi"; break;
                    case "issn": sStyleID = "issn"; break;

                    case "conf-date": sStyleID = "confDate"; break;
                    case "conf-loc": sStyleID = "confLoc"; break;
                    case "conf-name": sStyleID = "confName"; break;
                    case "conf-sponsor": sStyleID = "confSponsor"; break;

                    case "PublisherName": sStyleID = "publisherName"; break;
                    case "PublisherLocation": sStyleID = "publisherLoc"; break;
                    case "pub-id": sStyleID = "pubId"; break;
                    case "pubid-other": sStyleID = "pubIdOther"; break;
                    case "publisher-loc": sStyleID = "publisherLoc"; break;
                    case "publisher-name": sStyleID = "publisherName"; break;

                    case "Website": sStyleID = "website"; break;
                    case "email": sStyleID = "email"; break;

                    default:
                        sStyleID = sTagName;
                        break;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\GetStyleIDFromTagName_old", ex.Message, true, "");
            }
            return sStyleID;
        }

        private string ConvertHexToString(string HexValue)
        {
            string StrValue = "";
            while (HexValue.Length > 0)
            {
                StrValue += System.Convert.ToChar(System.Convert.ToInt32(HexValue.Substring(0, 2), 16)).ToString();
                HexValue = HexValue.Substring(2, HexValue.Length - 2);
            }
            return StrValue;
        }

        private string DecimalEntityConversion(Match tmpMatch)
        {
            string tmpStr = tmpMatch.Value.ToString();
            try
            {
                tmpStr = "&#" + ((int)tmpStr[0]).ToString() + ";";
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\DecimalEntityConversion", ex.Message, true, "");
            }
            return tmpStr;
        }

        private string HexaEntityConversion(Match tmpMatch)
        {
            string tmpStr = tmpMatch.Value.ToString();
            try
            {
                tmpStr = "&#x" + ((int)tmpStr[0]).ToString("X4") + ";";
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\HexaEntityConversion", ex.Message, true, "");
            }
            return tmpStr;
        }

        private bool IsNodePresent(string sXmlContent, string sXpath)
        {
            bool bReturnVal = false;
            try
            {
                XmlDocument docXml = new XmlDocument();
                docXml.LoadXml(sXmlContent);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(docXml.NameTable);
                nsmgr.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
                if (docXml.SelectSingleNode(sXpath, nsmgr) != null)
                {
                    bReturnVal = true;
                }
                else
                {
                    bReturnVal = false;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\IsNodePresent", ex.Message, true, "");
                bReturnVal = false;
            }
            return bReturnVal;
        }
    }
}
