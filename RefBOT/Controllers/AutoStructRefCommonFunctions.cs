using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RefBOT.Controllers
{
    public static class AutoStructRefCommonFunctions
    {
        public static string sRefElementsLocalExePtn = "(?:Author|Doi|Editor|Etal|FirstPage|Issue_No|ldl|PageRange|PubDate|Vol_No|Website|Year)";

        public static string NormalizeTagSpaces(string sRefContent, bool bIsTrimRequired = true)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "<ldlAuthorDelimiterChar> </ldlAuthorDelimiterChar>", "<ldlAuthorDelimiterChar><skip_space/></ldlAuthorDelimiterChar>");
                sRefContent = Regex.Replace(sRefContent, "<ldlAuthorDelimiterChar><b> </b></ldlAuthorDelimiterChar>", "<ldlAuthorDelimiterChar><b><skip_space/></b></ldlAuthorDelimiterChar>");
                sRefContent = Regex.Replace(sRefContent, "<ldlAuthorDelimiterChar><i> </i></ldlAuthorDelimiterChar>", "<ldlAuthorDelimiterChar><i><skip_space/></i></ldlAuthorDelimiterChar>");
                sRefContent = Regex.Replace(sRefContent, "</ldlAuthorDelimiterChar><ldlAuthorDelimiterChar>", "");

                sRefContent = Regex.Replace(sRefContent, "<ldlEditorDelimiterChar> </ldlEditorDelimiterChar>", "<ldlEditorDelimiterChar><skip_space/></ldlEditorDelimiterChar>");
                sRefContent = Regex.Replace(sRefContent, "<ldlEditorDelimiterChar><b> </b></ldlEditorDelimiterChar>", "<ldlEditorDelimiterChar><b><skip_space/></b></ldlEditorDelimiterChar>");
                sRefContent = Regex.Replace(sRefContent, "<ldlEditorDelimiterChar><i> </i></ldlEditorDelimiterChar>", "<ldlEditorDelimiterChar><i><skip_space/></i></ldlEditorDelimiterChar>");
                sRefContent = Regex.Replace(sRefContent, "</ldlEditorDelimiterChar><ldlEditorDelimiterChar>", "");


                sRefContent = Regex.Replace(sRefContent, @"</([a-zA-Z_\-]+)><\1>", "");


                do
                {
                    sRefContent = Regex.Replace(sRefContent, @"(<[a-zA-Z_\-]+>)[ ]+", " $1");
                    sRefContent = Regex.Replace(sRefContent, @"[ ]+(</[a-zA-Z_\-]+>)", "$1 ");
                } while (Regex.IsMatch(sRefContent, @"(?:(<[a-zA-Z_\-]+>)[ ]+|[ ]+(</[a-zA-Z_\-]+>))"));

                sRefContent = Regex.Replace(sRefContent, "[ ]{2,}", " ");
                sRefContent = Regex.Replace(sRefContent, "<skip_space/>", " ");

                //updated by Dakshinamoorthy on 2019-Sep-28
                if (bIsTrimRequired == true)
                {
                    sRefContent = sRefContent.Trim();
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefCommonFunctions.cs\NormalizeTagSpaces", ex.Message, true, "");
            }
            return sRefContent;
        }


        public static string NormalizeFormattingTags(string sRefTaggedContent)
        {
            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(i|b)>((?:(?!</?\1>).)+)</\1>", HandleNormalizeFormattingTags);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleFormattingTags", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        public static string GroupItalicTag(string sRefTaggedContent)
        {
            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:(?:(?:(?:<i>(?:(?!</?i>).)+</i>)[ ]+)+)(?:(?:(?:<i>(?:(?!</?i>).)+</i>)\.?[ ]+)))", HandleGroupItalicTag);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleFormattingTags", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }


        public static string HandleGroupItalicTag(Match myItalicContentMatch)
        {
            string sOutputContent = myItalicContentMatch.Value;
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "(?:</?i>)", "");
                sOutputContent = string.Format("<i>{0}</i>", sOutputContent);
                sOutputContent = NormalizeTagSpaces(sOutputContent, false);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\GroupItalicTag", ex.Message, true, "");
            }
            return sOutputContent;
        }


        public static string HandleNormalizeFormattingTags(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].Value;
            string sInnerContent = myRefMatch.Groups[2].Value;

            try
            {
                //sRefElementsLocalExePtn

                sInnerContent = Regex.Replace(sInnerContent, @"<([bi])></\1>", "");
                if (Regex.IsMatch(sInnerContent, @"<(i|b)>((?:(?!</?\1>).)+)</\1>"))
                {
                    sInnerContent = Regex.Replace(sInnerContent, @"<(i|b)>((?:(?!</?\1>).)+)</\1>", HandleNormalizeFormattingTags);
                    sInnerContent = Regex.Replace(sInnerContent, string.Format("(</(?:(?:ldl[^<>]+)|(?:{0}))>)", sRefElementsLocalExePtn), string.Format("</{0}>$1<{0}>", sTagName));
                    sInnerContent = Regex.Replace(sInnerContent, string.Format("(<(?:(?:ldl[^<>]+)|(?:{0}))>)", sRefElementsLocalExePtn), string.Format("</{0}>$1<{0}>", sTagName));
                    sInnerContent = string.Format("<{0}>{1}</{0}>", sTagName, sInnerContent);
                }
                else
                {
                    sInnerContent = Regex.Replace(sInnerContent, string.Format("(</(?:(?:ldl[^<>]+)|(?:{0}))>)", sRefElementsLocalExePtn), string.Format("</{0}>$1<{0}>", sTagName));
                    sInnerContent = Regex.Replace(sInnerContent, string.Format("(<(?:(?:ldl[^<>]+)|(?:{0}))>)", sRefElementsLocalExePtn), string.Format("</{0}>$1<{0}>", sTagName));
                    sInnerContent = string.Format("<{0}>{1}</{0}>", sTagName, sInnerContent);
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\NormalizeFormattingTags", ex.Message, true, "");
            }
            sInnerContent = Regex.Replace(sInnerContent, @"<([bi])></\1>", "");
            return sInnerContent;
        }

        //AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\NormalizeFormattingTags", ex.Message, true, "");

        public static void ShowErrorMessage(string sFunctionPath, string sErrMsg, bool bLogToFile, string sStartTime)
        {
            try
            {
                if (General.bIsDebugModeRefStruc == true)
                {
                    //TODO: 2020-Nov-23
                    //System.Windows.Forms.MessageBox.Show(sErrMsg, string.Format("eBOT - AutoRefStruc - {0}", sFunctionPath), System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else
                {
                    //TODO: 2020-Nov-23
                    //eBOT.Database.GetInstance.HistoryLogIns(sFunctionPath, sErrMsg, bLogToFile, sStartTime);
                }
            }
            catch (Exception ex1)
            {
                if (General.bIsDebugModeRefStruc == true)
                {
                    //TODO: 2020-Nov-23
                    //System.Windows.Forms.MessageBox.Show(ex1.Message, string.Format("eBOT - AutoRefStruc - {0}", "AutoStructRefCommonFunctions.cs/ShowErrorMessage"), System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else
                {
                    //TODO: 2020-Nov-23
                    //eBOT.Database.GetInstance.HistoryLogIns(@"ERROR: AutoStructRefCommonFunctions.cs/ShowErrorMessage", ex1.Message, true, "");
                }
            }
        }
    }
}
