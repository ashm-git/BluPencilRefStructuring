using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RefBOT.Controllers
{
    public static class AutoStructRefAuthor
    {
        #region References
        //H. M’saad,
        #endregion

        public static string sNormalSingleWordIniCaps = @"(?:(?!\b(?:Jr|Sr)\b)(?!(?:\b(?:I{1,3}|IV|V(?:I{1,3})?|I?X)\b))(?:(?:[A-Z][a-z]+[\u2019\u0027][a-z]+)|(?:[A-Z][a-z]+[\u2019\u0027])|(?:[A-Z][a-z]+)|(?:[A-Z][a-z]+)\-[A-Z]|(?:[A-Z][\u2019\u0027][a-z]+)))";

        public static string sSNM_LookBehindPtn = "(?<=(?:[ ]|^))";
        public static string sInitial_LookBehindPtn = "(?<=(?:[ ]|^))";

        public static string sNamePrefixLowerIniCaps = @"(?:(?:M[a]?c|M[\u2019\u0027]|[OD][\u2019\u0027]|[vV]an [Dd]e[rn]|[vV][ao]n|[Dd]el|[Dd]e|[Ll]a[\u0027]|[Dd]e[rs]|D[\u2019\u0027]|d[\u2019\u0027]|Le|Di|Mac|D[ea]l|San|[Dd]u|St|[Dd]a|A[Ii]\-|[Yy]a|[Yy]e|[Rr]e|[Ll]e|[Dd]ella|[Dd]e [Ll]a|[Aa]n [Dd]er|[Dd]os|[A]l|[Vv]an[ ]?[\u2019\u0027]t|[Dd]o|[Dd]ell[\u2019\u0027]|[Vv]an [Dd]e|[Dd]en|[Dd]iaz\-?|Mohd\.|[Ee]l|[Dd]i|[Ll][\u2019\u0027]|[Ll]a|[Dd]e\-|[Ee]l\-|[Tt]en|[Aa]f|[Dd]all[\u2019\u0027]|[Ll]o|[Ff]itz|[Pp]etit|[Vv]ande|[Ss]un|[Nn][\u2019\u0027])[ ]?)";
        public static string sSNM_SingleWord = string.Format(@"(?:(?:{0})?(?:{1}))", sNamePrefixLowerIniCaps, sNormalSingleWordIniCaps);
        //updated by Dakshinamoorthy on 2020-Dec-18
        //public static string sGNM_SingleWord = string.Format(@"(?:(?:{0})?(?:{1}))", sNamePrefixLowerIniCaps, sNormalSingleWordIniCaps);
        public static string sGNM_SingleWord = string.Format(@"(?:(?:{0})?(?:{1})(?:[\-\u2013\u2010](?:{1}))?)", sNamePrefixLowerIniCaps, sNormalSingleWordIniCaps);
        public static string sSuffixPattern = @"(?:(?:\b(?:Sr|Jr)\b)|(?:\b(?:I{1,3}|IV|V(?:I{1,3})?|I?X)\b)|(?:[0-9]+(?:st|nd|rd|th)))";

        public static int nSno = 0;
        public static string sAuthorGroupPattern = string.Empty;
        public static string sRefSNM_FullPattern = string.Empty;
        public static string sRefInitialFullPattern = string.Empty;
        public static string sRefAuthorDelimeterAnd = string.Empty;

        public static string sAuthorGroupPattern_Start = string.Empty;
        public static string sAuthorGroupPattern_And = string.Empty;
        public static string sAuthorGroupPattern_End = string.Empty;

        //Local Location & Publisher
        public static string sCityXmlContent = string.Empty;
        public static string sPublisherXmlContent = string.Empty;

        //added by Dakshinamoorthy on 2020-Jul-13
        public static bool LoadLocationDetails()
        {
            try
            {
                string sCountryXmlFilePath = General.GeneralInstance.drive + @"\eBOT\Data\countries.xml";
                string sStateXmlFilePath = General.GeneralInstance.drive + @"\eBOT\Data\states.xml";
                string sCityXmlFilePath = General.GeneralInstance.drive + @"\eBOT\Data\cities.xml";
                string sPublisherXmlFilePath = General.GeneralInstance.drive + @"\eBOT\Data\publishers.xml";

                if (File.Exists(sCityXmlFilePath) == false ||
                    File.Exists(sPublisherXmlFilePath) == false)
                {
                    return false;
                }

                sCityXmlContent = File.ReadAllText(sCityXmlFilePath);
                //commented by Daksinamoorthy on 2020-Jul-15 (Reason: Taking more time for processing) 
                //sPublisherXmlContent = File.ReadAllText(sPublisherXmlFilePath);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAuthor.cs\LoadLocationDetails", ex.Message, true, "");
            }
            return true;
        }

        public static bool LoadAuthorGroupPatterns()
        {
            try
            {
                GenerateSNM_Pattern();
                GenerateInitialPattern();
                GenerateSingleAuthorPattern();
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAuthor.cs\LoadAuthorGroupPatterns", ex.Message, true, "");
                return false;
            }
            return true;
        }


        private static bool GenerateSNM_Pattern()
        {
            try
            {
                List<string> lstSNM_Pattern = new List<string>();
                lstSNM_Pattern.Add(string.Format(@"(?:{0} {0} {0})", sSNM_SingleWord));
                lstSNM_Pattern.Add(string.Format(@"(?:{0}(?:[\-\u2013\u2010]|[ ]){0}[\-\u2013\u2010]{0})", sSNM_SingleWord));
                lstSNM_Pattern.Add(string.Format(@"(?:{0} {0})", sSNM_SingleWord));
                lstSNM_Pattern.Add(string.Format(@"(?:{0}[\-\u2013\u2010]{0})", sSNM_SingleWord));
                lstSNM_Pattern.Add(string.Format(@"(?:{0})", sSNM_SingleWord));

                sRefSNM_FullPattern = string.Format("(?:(?:{0})(?:{1}))", sSNM_LookBehindPtn, string.Join("|", lstSNM_Pattern.ToArray()));
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAuthor.cs\GenerateSNM_Pattern", ex.Message, true, "");
                return false;
            }
            return true;
        }

        private static bool GenerateInitialPattern()
        {
            try
            {
                List<string> lstInitialPattern = new List<string>();
                lstInitialPattern.Add(@"(?:[A-Z]\.(?:(?:[\-][A-Z]\b|\b[A-Z][\-])\.)(?:(?:[\-][A-Z]\b|\b[A-Z][\-])))");
                lstInitialPattern.Add(@"(?:[\-][A-Z](?:[\-][A-Z]\b|\b[A-Z][\-])\.(?: (?:[\-][A-Z]\b|\b[A-Z][\-])\.)(?: (?:[\-][A-Z]\b|\b[A-Z][\-])))");
                lstInitialPattern.Add(@"(?:(?:[\-][A-Z]\b|\b[A-Z][\-])\.(?:(?:[\-][A-Z]\b|\b[A-Z][\-])\.)(?:(?:[\-][A-Z]\b|\b[A-Z][\-])))");
                lstInitialPattern.Add(@"(?:(?:[\-][A-Z]\b|\b[A-Z][\-])(?: (?:[\-][A-Z]\b|\b[A-Z][\-]))(?: (?:[\-][A-Z]\b|\b[A-Z][\-])))");
                lstInitialPattern.Add(@"(?:[A-Z](?: (?:[\-][A-Z]\b|\b[A-Z][\-]))(?: (?:[\-][A-Z]\b|\b[A-Z][\-])))");
                lstInitialPattern.Add(@"(?:[A-Z]\.(?: [A-Z]\b\.)(?: [A-Z]\b))");
                lstInitialPattern.Add(@"(?:[A-Z]\.[ ]?(?:[A-Z]\b\.)[ ]?(?:[\-]?[A-Z]\b))");
                lstInitialPattern.Add(@"(?:[A-Z]\.(?: (?:[\-][A-Z]\b|\b[A-Z][\-])\.) [A-Z])");
                lstInitialPattern.Add(@"(?:[A-Z](?: (?:[\-][A-Z]\b|\b[A-Z][\-])) [A-Z]\b)");
                lstInitialPattern.Add(@"(?:[A-Z]\.(?:(?:[\-][A-Z]\b|\b[A-Z][\-])\.)[A-Z])");
                lstInitialPattern.Add(@"(?:[A-Z](?:(?:[\-][A-Z]\b|\b[A-Z][\-]))[A-Z]\b)");
                lstInitialPattern.Add(@"(?:[A-Z](?:(?:[\-][A-Z]\b|\b[A-Z][\-])){1,2}\b)");
                lstInitialPattern.Add(@"(?:(?:[\-][A-Z]\b|\b[A-Z][\-])\.(?: (?:[\-][A-Z]\b|\b[A-Z][\-])))");
                lstInitialPattern.Add(@"(?:(?:[\-][A-Z]\b|\b[A-Z][\-])(?:[\-][A-Z]\b|\b[A-Z][\-]){1,2})");
                lstInitialPattern.Add(@"(?:[A-Z](?: [A-Z]){1,2}\b)");
                lstInitialPattern.Add(@"(?:[A-Z]\.(?:(?:[\-][A-Z]\b|\b[A-Z][\-])))");
                lstInitialPattern.Add(@"(?:(?:[\-][A-Z]\b|\b[A-Z][\-])\.(?:(?:[\-][A-Z]\b|\b[A-Z][\-])))");
                lstInitialPattern.Add(@"(?:(?:[\-][A-Z]\b|\b[A-Z][\-])\.(?:[A-Z]\b|\b[A-Z][\-]))");
                lstInitialPattern.Add(@"(?:(?:[\-][A-Z]\b|\b[A-Z][\-])\.(?:[A-Z]\b\.)(?:[A-Z]\b))");
                lstInitialPattern.Add(@"(?:(?:[\-][A-Z]\b|\b[A-Z][\-])\.(?:[A-Z]\b\.))");
                lstInitialPattern.Add(@"(?:[A-Z]\.(?: (?:[\-][A-Z]\b|\b[A-Z][\-])))");
                lstInitialPattern.Add(@"(?:(?:[\-][A-Z]\b|\b[A-Z][\-])(?: (?:[\-][A-Z]\b|\b[A-Z][\-])))");
                lstInitialPattern.Add(@"(?:[A-Z](?: (?:[\-][A-Z]\b|\b[A-Z][\-])))");
                lstInitialPattern.Add(@"(?:[A-Z]\.(?: [A-Z]\b))");
                lstInitialPattern.Add(@"(?:[A-Z]\.(?:[A-Z]\b))");
                lstInitialPattern.Add(@"(?:[A-Z][A-Z]{1,2}\b)");
                lstInitialPattern.Add(@"(?:(?:[\-][A-Z]\b|\b[A-Z][\-]))");
                lstInitialPattern.Add(@"(?:[A-Z]\b)");

                //4 Initials
                lstInitialPattern.Add(@"(?:(?:[\-]?[A-Z]\b\.)[ ]?(?:[\-]?[A-Z]\b\.)[ ]?(?:[\-]?[A-Z]\b\.)[ ]?(?:[\-]?[A-Z]\b))");
                lstInitialPattern.Add(@"(?:(?:[A-Z]\b\.]?)[ ]?(?:[\-][A-Z]\b\.)[ ]?(?:[A-Z]\b\.))");
                lstInitialPattern.Add(@"(?:(?:[A-Z])(?:[\-][A-Z])(?:[A-Z]\b))");
                lstInitialPattern.Add(@"(?:(?:[A-Z]{4,5}\b))");
                lstInitialPattern.Add(@"(?:(?:[A-Z]{2,3}\-[A-Z]\b))");
                lstInitialPattern.Add(@"(?:(?:Ch\b))");
                lstInitialPattern.Add(@"(?:(?:Yu\. [A-Z]))");
                //added by Dakshinamoorthy on 2020-Dec-07
                lstInitialPattern.Add(@"(?:(?:El\-[A-Z]))");

                sRefInitialFullPattern = string.Format("(?:(?:{0})(?:{1}))", sInitial_LookBehindPtn, string.Join("|", lstInitialPattern.ToArray()));

            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAuthor.cs\GenerateInitialPattern", ex.Message, true, "");
                return false;
            }
            return true;
        }

        private static bool GenerateSingleAuthorPattern()
        {
            try
            {
                //updated by Dakshinamoorthy on 2020-May-26
                //sRefAuthorDelimeterAnd = "(?:(?:(?:[\u2026] )?(?:&|and))|[\u2026])";
                sRefAuthorDelimeterAnd = @"(?:(?:(?:[\u2026] |(?:[ ]?\.[ ]?\.[ ]?\.[ ]?))?(?:& (?:and)\b|&|(?:and)\b))|[\u2026]|(?:[ ]?\.[ ]?\.[ ]?\.[ ]?))";

                string sAntiRefAuthorDelimeterAnd = string.Format("(?! {0} )", sRefAuthorDelimeterAnd);
                //string sRefAuthorDelimeterEnd = @"(?=(?:[ ]?$| [\(\[]|[ ](?:(?:<date-info>(?:(?!</?date-info>).)+</date-info>)|(?:<etal>(?:(?!</?etal>).)+</etal>)|(?:<eds>(?:(?!</?eds>).)+</eds>))))";
                string sRefAuthorDelimeterEnd = @"(?=(?:[ ]?$|[ ](?:(?:<date-info>(?:(?!</?date-info>).)+</date-info>)|(?:<etal>(?:(?!</?etal>).)+</etal>)|(?:<eds>(?:(?!</?eds>).)+</eds>))))";
                string sRefAuthorDelimeterEnd_Full = string.Format("(?:(?:{0})|(?:{1}))", sAntiRefAuthorDelimeterAnd, sRefAuthorDelimeterEnd);

                //for sequence of authors
                List<string> lstAuthorPattern_Start = new List<string>();
                #region SETS
                //#region SET
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU1>(?<SNM1_1>{0} )(?<GNM1_1>{1} )(?<SUF1_1>{2}[,;] ))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern)); //Camargo CA Jr, 
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU2>(?<SNM2_1>{0}, )(?<GNM2_1>{1}\.[,]? )(?<SUF2_1>{2}\.[,;] ))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern)); //Feliu, S. Jr., -- Lavin, J. P., Jr., 
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU3>(?<SNM3_1>{0} )(?<GNM3_1>{1}, )(?<SUF3_1>{2}[\.]?[,;] ))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern)); //Marks WJ, Jr., 
                //lstAuthorPattern_Start.Add(string.Format(@"(?<AU3>(?<SNM3_1>{0}[,]? )(?<GNM3_1>{1}(?:\.|,|\.,)? )(?<SUF3_1>{2}(?:,|\.,|\.;) ))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern));
                //#endregion

                //#region SET
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU4>(?<SNM4_1>{0} )(?<SUF4_1>{1}[\.]?, )(?<GNM4_1>{2}(?:\.[,]?|,) ))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern)); //Ulibarri Jr., K.R. -- Pander III JE,
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU5>(?<SNM5_1>{0} )(?<SUF5_1>{1} )(?<GNM5_1>{2}(?:\.[,]?|,) ))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern)); //Ulibarri Jr., K.R. -- Pander III JE, 
                //lstAuthorPattern_Start.Add(string.Format(@"(?<AU5>(?<SNM5_1>{0}[,]? )(?<SUF5_1>{1}(?:\.,)? )(?<GNM5_1>{2}(?:\.|,|\.,)? ))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern));
                //#endregion

                //#region SET
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU6>(?<GNM6_1>{0} {1}\. )(?<SNM6_1>{2}\, ))", sGNM_SingleWord, sRefInitialFullPattern, sRefSNM_FullPattern)); //Maxwell E. McCombs, -- 
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU17>(?<GNM17_1>{0} {1} )(?<SNM17_1>{2}, ))", sGNM_SingleWord, sRefInitialFullPattern, sSNM_SingleWord)); //Oistein E Andersen, 
                //lstAuthorPattern_Start.Add(string.Format(@"(?<AU17>(?<GNM17_1>{0} {1}[\.]? )(?<SNM17_1>{2}, ))", sGNM_SingleWord, sRefInitialFullPattern, sSNM_SingleWord)); //Oistein E Andersen, 
                //#endregion

                //#region SET
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU9>(?<SNM9_1>{0}, )(?<GNM9_1>{1}\.[,;] ))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Loggins, R. A., 
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU10>(?<SNM10_1>{0} )(?<GNM10_1>{1}[,;] ))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Lance W,
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU11>(?<SNM11_1>{0}, )(?<GNM11_1>{1}[,;] ))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Husni, AS;
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU13>(?<SNM13_1>{0}, )(?<GNM13_1>{1}\., ))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Abro, R., 
                //lstAuthorPattern_Start.Add(string.Format(@"(?<AU13>(?<SNM13_1>{0}[,]? )(?<GNM13_1>{1}(?:\.[,;]|,|;) ))", sRefSNM_FullPattern, sRefInitialFullPattern));
                //#endregion

                //#region SET
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU14>(?<GNM14_1>{0}\. )(?<SNM14_1>{1},[;]? ))", sRefInitialFullPattern, sRefSNM_FullPattern)); //J.E. Mitchell, 
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU15>(?<GNM15_1>{0}\. )(?<SNM15_1>{1}\., ))", sRefInitialFullPattern, sRefSNM_FullPattern)); //W.A. Wallace.,
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU16>(?<GNM16_1>{0} )(?<SNM16_1>{1},) )", sRefInitialFullPattern, sRefSNM_FullPattern)); //L Lindsay, 
                //lstAuthorPattern_Start.Add(string.Format(@"(?<AU16>(?<GNM16_1>{0}[\.]? )(?<SNM16_1>{1}(?:,[;]?|\.,)) )", sRefInitialFullPattern, sRefSNM_FullPattern));
                //#endregion 
                #endregion
                #region OldCodes
                //lstAuthorPattern_Start.Add(string.Format(@"(?<AU7>(?<SNM7_1>{0}, )(?<GNM7_1>{1} {2}\.[,;] ))", sSNM_SingleWord, sGNM_SingleWord, sRefInitialFullPattern)); //Avis, Walter S. 1955.
                //lstAuthorPattern_Start.Add(string.Format(@"(?<AU8>(?<SNM8_1>{0} )(?<GNM8_1>{1}[\.]?, ))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Lance Bennett Lance W.,  
                ////updated by Dakshinamoorthy on 2020-May-25
                ////lstAuthorPattern_Start.Add(string.Format(@"(?<AU18>(?<SNM18_1>{0} )(?<GNM18_1>{1}, ))", sRefSNM_FullPattern, sGNM_SingleWord)); //Mariano Felice, 
                //lstAuthorPattern_Start.Add(string.Format(@"(?<AU18>(?<SNM18_1>{0} )(?<GNM18_1>{1}, )(?!{2}[\.,]*))", string.Format("(?:(?!{0})(?:{1}))", sNamePrefixLowerIniCaps, sRefSNM_FullPattern), sGNM_SingleWord, sRefInitialFullPattern)); 
                #endregion

                //Camargo CA Jr, //Feliu, S. Jr., --> Lavin, J. P., Jr., --> //Marks WJ, Jr., 
                lstAuthorPattern_Start.Add(string.Format(@"(?<AU3>(?<SNM3_1>{0}[,]? )(?<GNM3_1>{1}(?:\.|,|\.,)? )(?<SUF3_1>{2}(?:,|\.,|\.;) ))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern));

                //Ulibarri Jr., K.R. -- Pander III JE, --> //Ulibarri Jr., K.R. --> Pander III JE, 
                lstAuthorPattern_Start.Add(string.Format(@"(?<AU5>(?<SNM5_1>{0}[,]? )(?<SUF5_1>{1}(?:\.,|,)? )(?<GNM5_1>{2}(?:\.|,|\.,)? ))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern));

                //Oistein E Andersen, -> //Maxwell E. McCombs,
                lstAuthorPattern_Start.Add(string.Format(@"(?<AU17>(?<GNM17_1>{0} {1}[\.]? )(?<SNM17_1>{2}, ))", sGNM_SingleWord, sRefInitialFullPattern, sSNM_SingleWord));

                //Loggins, R. A., --> //Lance W, --> //Husni, AS; --> //Abro, R., 
                lstAuthorPattern_Start.Add(string.Format(@"(?<AU13>(?<SNM13_1>{0}[,]? )(?<GNM13_1>{1}(?:\.[,;]|,|;) ))", sRefSNM_FullPattern, sRefInitialFullPattern));

                //J.E. Mitchell, --> //W.A. Wallace., --> //L Lindsay, 
                lstAuthorPattern_Start.Add(string.Format(@"(?<AU16>(?<GNM16_1>{0}[\.]? )(?<SNM16_1>{1}(?:,[;]?|\.,)) )", sRefInitialFullPattern, sRefSNM_FullPattern));

                //Avis, Walter S. 1955.
                lstAuthorPattern_Start.Add(string.Format(@"(?<AU7>(?<SNM7_1>{0}, )(?<GNM7_1>{1} {2}\.[,;] ))", sSNM_SingleWord, sGNM_SingleWord, sRefInitialFullPattern));

                ////García Giraldo, J.M. et al. (2013). 
                //lstAuthorPattern_Start.Add(string.Format(@"(?<AU10>(?<SNM7_2>{0}, )(?<GNM7_2>{1}[\.] ))", sRefSNM_FullPattern, sRefInitialFullPattern));

                //Lance Bennett Lance W.,  
                lstAuthorPattern_Start.Add(string.Format(@"(?<AU8>(?<SNM8_1>{0} )(?<GNM8_1>{1}[\.]?, ))", sRefSNM_FullPattern, sRefInitialFullPattern));

                //Mariano Felice, 
                //lstAuthorPattern_Start.Add(string.Format(@"(?<AU18>(?<SNM18_1>{0} )(?<GNM18_1>{1}, ))", sRefSNM_FullPattern, sGNM_SingleWord));

                //updated by Dakshinamoorthy on 2020-Dec-07
                //lstAuthorPattern_Start.Add(string.Format(@"(?<AU18>(?<SNM18_1>{0} )(?<GNM18_1>{1}, )(?!{2}[\.,]*))", string.Format("(?:(?!{0})(?:{1}))", sNamePrefixLowerIniCaps, sRefSNM_FullPattern), sGNM_SingleWord, sRefInitialFullPattern));
                lstAuthorPattern_Start.Add(string.Format(@"(?<AU18>(?<SNM18_1>{0} )(?<GNM18_1>{1}, )(?!{2}[\.,]*))", string.Format(@"(?:(?!\b(?:{0})\b)(?:{1}))", sNamePrefixLowerIniCaps, sRefSNM_FullPattern), sGNM_SingleWord, sRefInitialFullPattern));

                sAuthorGroupPattern_Start = string.Join("|", lstAuthorPattern_Start.ToArray());

                //author with preceding (and, &)
                List<string> lstAuthorPattern_And = new List<string>();
                #region SETS
                //#region SET
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU19>(?<SNM19_1>{0} )(?<SUF19_1>{1}\., )(?<GNM19_1>{2}\. )(?=(?:{3} )))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern, sRefAuthorDelimeterAnd)); //Ulibarri Jr., K.R. and 
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU22>(?<SNM22_1>{0} )(?<SUF22_1>{1}[\.]?, )(?<GNM22_1>{2}\.[,]? ))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern)); //Pielke Sr, R., and 
                //lstAuthorPattern_And.Add(string.Format(@"(?<AU22>(?<SNM22_1>{0} )(?<SUF22_1>{1}[\.]?, )(?<GNM22_1>{2}\.[,]? ))(?=(?:{3} ))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern, sRefAuthorDelimeterAnd)); //Pielke Sr, R., and  
                //#endregion

                //#region SET
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU20>(?<SNM20_1>{0}, )(?<GNM20_1>{1}\., )(?<SUF20_1>{2}\.[,;] )(?=(?:{3} )))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern, sRefAuthorDelimeterAnd)); //Pond, R.S., Jr., and
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU21>(?<SNM21_1>{0} )(?<GNM21_1>{1} )(?<SUF21_1>{2}[,;]? )(?=(?:{3} )))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern, sRefAuthorDelimeterAnd)); //Azim HA Jr & 
                //lstAuthorPattern_And.Add(string.Format(@"(?<AU21>(?<SNM21_1>{0}[,]? )(?<GNM21_1>{1}(?:\.,)? )(?<SUF21_1>{2}(?:\.[,;]?)? )(?=(?:{3} )))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern, sRefAuthorDelimeterAnd)); //Azim HA Jr &  
                //#endregion

                //#region SET
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU23>(?<GNM23_1>{0}\. )(?<SNM23_1>{1}[,]? )(?<SUF23_1>{2}\.[,;]? )(?=(?:{3} )))", sRefInitialFullPattern, sRefSNM_FullPattern, sSuffixPattern, sRefAuthorDelimeterAnd)); //W. H. Hayt, Jr., and 
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU33>(?<GNM33_1>{0}\. )(?<SNM33_1>{1} )(?<SUF33_1>{2} )(?=(?:{3} )))", sRefInitialFullPattern, sRefSNM_FullPattern, sSuffixPattern, sRefAuthorDelimeterAnd)); //R. J. Malak Jr and 
                //lstAuthorPattern_And.Add(string.Format(@"(?<AU33>(?<GNM33_1>{0}\. )(?<SNM33_1>{1}[,]? )(?<SUF33_1>{2}(?:\.[,;]?)? )(?=(?:{3} )))", sRefInitialFullPattern, sRefSNM_FullPattern, sSuffixPattern, sRefAuthorDelimeterAnd));
                //#endregion

                //#region SET
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU26>(?<SNM26_1>{0}, )(?<GNM26_1>{1}\., )(?=(?:{2} )))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterAnd)); //Abro, R., … and 
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU27>(?<SNM27_1>{0}, )(?<GNM27_1>{1}\. )(?=(?:{2} )))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterAnd)); //Lynch, S.A. and 
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU28>(?<SNM28_1>{0} )(?<GNM28_1>{1}\.,? )(?=(?:{2} )))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterAnd)); //Bonfait G. and  -- Song J., and
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU29>(?<SNM29_1>{0} )(?<GNM29_1>{1}, )(?=(?:{2} )))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterAnd)); //Schwingshackl L, & 
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU30>(?<SNM30_1>{0}[,]? )(?<GNM30_1>{1} )(?=(?:{2} )))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterAnd)); //Vecoli M and 
                //lstAuthorPattern_And.Add(string.Format(@"(?<AU30>(?<SNM30_1>{0}[,]? )(?<GNM30_1>{1}(?:\.|,|\.[,;])? )(?=(?:{2} )))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterAnd));
                //#endregion

                //#region SET
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU31>(?<GNM31_1>{0}\. )(?<SNM31_1>{1}, )(?=(?:{2} )))", sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterAnd)); //J.E. Mitchell, and 
                ////lstAuthorPattern_And.Add(string.Format(@"(?<AU32>(?<GNM32_1>{0}[\.]? )(?<SNM32_1>{1} )(?=(?:{2} )))", sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterAnd)); //W. Desmet and 
                //lstAuthorPattern_And.Add(string.Format(@"(?<AU32>(?<GNM32_1>{0}[\.]? )(?<SNM32_1>{1}[,]? )(?=(?:{2} )))", sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterAnd)); //W. Desmet and  
                //#endregion 
                #endregion
                #region OldCodes
                //lstAuthorPattern_And.Add(string.Format(@"(?<AU24>(?<GNM24_1>{0}\. )(?<SUF24_1>{1}\.?[,;]? )(?<SNM24_1>{2}[,]? )(?=(?:{3} )))", sRefInitialFullPattern, sSuffixPattern, sRefSNM_FullPattern, sRefAuthorDelimeterAnd)); //H. B. Jr Brown and 
                //lstAuthorPattern_And.Add(string.Format(@"(?<AU25>(?<GNM25_1>{0} {1}\. )(?<SNM25_1>{2}[\.,]? )(?=(?:{3} )))", sGNM_SingleWord, sRefInitialFullPattern, sSNM_SingleWord, sRefAuthorDelimeterAnd)); //Maxwell E. McCombs. -- Donald J. Kessler and 
                //lstAuthorPattern_And.Add(string.Format(@"(?<AU34>(?<GNM34_1>{0} {1} )(?<SNM34_1>{2}, )(?=(?:{3} )))", sGNM_SingleWord, sRefInitialFullPattern, sSNM_SingleWord, sRefAuthorDelimeterAnd)); //Oistein E Andersen,
                //lstAuthorPattern_And.Add(string.Format(@"(?<AU35>(?<SNM35_1>{0} )(?<GNM35_1>{1}, )(?=(?:{2} )))", sRefSNM_FullPattern, sGNM_SingleWord, sRefAuthorDelimeterAnd)); //Mariano Felice, 
                ////added by Dakshinamoorthy on 2020-Apr-29
                //lstAuthorPattern_And.Add(string.Format(@"(?<AU62>(?<SNM62_1>{0}, )(?<GNM62_1>(?:{1} )*{1}[,]? )(?=(?:{2} )))", sRefSNM_FullPattern, sGNM_SingleWord, sRefAuthorDelimeterAnd)); //Khatri, Deepika and  
                #endregion

                //Pielke Sr, R., and --> //Ulibarri Jr., K.R. and -->
                lstAuthorPattern_And.Add(string.Format(@"(?<AU22>(?<SNM22_1>{0} )(?<SUF22_1>{1}[\.]?, )(?<GNM22_1>{2}\.[,]? ))(?=(?:{3} ))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern, sRefAuthorDelimeterAnd));

                //Pond, R.S., Jr., and --> //Azim HA Jr &  
                lstAuthorPattern_And.Add(string.Format(@"(?<AU21>(?<SNM21_1>{0}[,]? )(?<GNM21_1>{1}(?:\.,)? )(?<SUF21_1>{2}(?:\.[,;]?)? )(?=(?:{3} )))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern, sRefAuthorDelimeterAnd));

                //W. H. Hayt, Jr., and --> //R. J. Malak Jr and 
                lstAuthorPattern_And.Add(string.Format(@"(?<AU33>(?<GNM33_1>{0}\. )(?<SNM33_1>{1}[,]? )(?<SUF33_1>{2}(?:\.[,;]?)? )(?=(?:{3} )))", sRefInitialFullPattern, sRefSNM_FullPattern, sSuffixPattern, sRefAuthorDelimeterAnd));

                //H. B. Jr Brown and 
                lstAuthorPattern_And.Add(string.Format(@"(?<AU24>(?<GNM24_1>{0}\. )(?<SUF24_1>{1}\.?[,;]? )(?<SNM24_1>{2}[,]? )(?=(?:{3} )))", sRefInitialFullPattern, sSuffixPattern, sRefSNM_FullPattern, sRefAuthorDelimeterAnd));

                //Maxwell E. McCombs. -- Donald J. Kessler and 
                //updated by Dakshinamoorthy on 2020-Jul-10
                //lstAuthorPattern_And.Add(string.Format(@"(?<AU25>(?<GNM25_1>{0} {1}\. )(?<SNM25_1>{2}[\.,]? )(?=(?:{3} )))", sGNM_SingleWord, sRefInitialFullPattern, sSNM_SingleWord, sRefAuthorDelimeterAnd));
                lstAuthorPattern_And.Add(string.Format(@"(?<AU25>(?<GNM25_1>{0} {1}\.? )(?<SNM25_1>{2}[\.,]? )(?=(?:{3} )))", sGNM_SingleWord, sRefInitialFullPattern, sSNM_SingleWord, sRefAuthorDelimeterAnd));

                //Oistein E Andersen,
                lstAuthorPattern_And.Add(string.Format(@"(?<AU34>(?<GNM34_1>{0} {1} )(?<SNM34_1>{2}, )(?=(?:{3} )))", sGNM_SingleWord, sRefInitialFullPattern, sSNM_SingleWord, sRefAuthorDelimeterAnd));

                //added by Dakshinamoorthy on 2020-Jul-10
                //Kim, Julie M. and Rita Nangia (2008), 
                lstAuthorPattern_And.Add(string.Format(@"(?<AU35>(?<SNM35_1>{0}, )(?<GNM35_1>{1} (?:{2}\.? ))(?=(?:{3} )))", sRefSNM_FullPattern, sGNM_SingleWord, sRefInitialFullPattern, sRefAuthorDelimeterAnd));

                //Abro, R., … and --> //Lynch, S.A. and --> //Bonfait G. and  -- Song J., and --> //Schwingshackl L, & --> //Vecoli M and 
                lstAuthorPattern_And.Add(string.Format(@"(?<AU30>(?<SNM30_1>{0}[,]? )(?<GNM30_1>{1}(?:\.|,|\.[,;])? )(?=(?:{2} )))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterAnd));

                //W. Desmet and --> //J.E. Mitchell, and 
                lstAuthorPattern_And.Add(string.Format(@"(?<AU32>(?<GNM32_1>{0}[\.]? )(?<SNM32_1>{1}[,]? )(?=(?:{2} )))", sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterAnd));

                //added by Dakshinamoorthy on 2020-Apr-29
                //Mariano Felice, 
                lstAuthorPattern_And.Add(string.Format(@"(?<AU35>(?<SNM35_1>{0} )(?<GNM35_1>{1}[,]? )(?=(?:{2} )))", sRefSNM_FullPattern, sGNM_SingleWord, sRefAuthorDelimeterAnd));

                //added by Dakshinamoorthy on 2020-Jul-10
                //Boltho, Andrea and 
                lstAuthorPattern_And.Add(string.Format(@"(?<AU35>(?<SNM35_1>{0}, )(?<GNM35_1>{1} )(?=(?:{2} )))", sRefSNM_FullPattern, sGNM_SingleWord, sRefAuthorDelimeterAnd));

                //updated by Dakshinamoorthy on 2020-Jul-10
                //Khatri, Deepika and 
                //lstAuthorPattern_And.Add(string.Format(@"(?<AU62>(?<SNM62_1>{0}, )(?<GNM62_1>(?:{1} )*{1}[,]? )(?=(?:{2} )))", sRefSNM_FullPattern, sGNM_SingleWord, sRefAuthorDelimeterAnd));
                lstAuthorPattern_And.Add(string.Format(@"(?<AU62>(?<SNM62_1>{0}[,]? )(?<GNM62_1>(?:{1} )*{1}[,]? )(?=(?:{2} )))", sRefSNM_FullPattern, sGNM_SingleWord, sRefAuthorDelimeterAnd));





                //Caglar, Ayse, and 
                //Otsch, Walter and 
                sAuthorGroupPattern_And = string.Join("|", lstAuthorPattern_And.ToArray());

                //author end patterns
                List<string> lstAuthorPattern_End = new List<string>();
                #region SETS
                //#region SET
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU36>(?<SNM36_1>{0} )(?<GNM36_1>{1} )(?<SUF36_1>{2}[,;](?:{3})))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern, sRefAuthorDelimeterEnd)); //Fahey GC Jr, <i>et al.</i>
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU39>(?<SNM39_1>{0}[,]? )(?<GNM39_1>{1}[,\.]? )(?<SUF39_1>{2}\.[,]?))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern)); //Campbell DA, Jr. -- Ritchie, D. Sr. eds. 2011. 
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU39>(?<SNM39_1>{0}[,]? )(?<GNM39_1>{1}[,\.]? )(?<SUF39_1>{2}(?:[,;]|\.[,]?)))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern)); //Campbell DA, Jr. -- Ritchie, D. Sr. eds. 2011. 
                //#endregion

                //#region SET
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU37>(?<SNM37_1>{0} )(?<SUF37_1>{1} )(?<GNM37_1>{2}[,;]?(?:{3})))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern, sRefAuthorDelimeterEnd)); //Porte Jr D (1977) 
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU38>(?<SNM38_1>{0} )(?<SUF38_1>{1} )(?<GNM38_1>{2}[,;](?:{3})))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern, sRefAuthorDelimeterEnd)); //Izzo Jr JL, et al.
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU40>(?<SNM40_1>{0} )(?<SUF40_1>{1}\., )(?<GNM40_1>{2}\.[,]?))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern)); //Ulibarri Jr., K.R. 
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU40>(?<SNM40_1>{0} )(?<SUF40_1>{1}(?:\.,)? )(?<GNM40_1>{2}(?:[,;\.]|\.,)?))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern)); //Ulibarri Jr., K.R.  
                //#endregion

                //#region SET
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU44>(?<SNM44_1>{0}, )(?<GNM44_1>{1}\.,))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Abro, R.,
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU47>(?<SNM47_1>{0}, )(?<GNM47_1>{1}\.))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Schubert, E. F. 
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU52>(?<SNM52_1>{0}, )(?<GNM52_1>{1}\.;))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Seo, S.; <i>et al.</i> 
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU53>(?<SNM53_1>{0}, )(?<GNM53_1>{1}\.,))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Melika, G., <i>et al</i>.
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU53>(?<SNM53_1>{0}[,]? )(?<GNM53_1>{1}\.[,;]?))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Melika, G., <i>et al</i>. 

                //#endregion

                //#region SET
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU49>(?<SNM49_1>{0} )(?<GNM49_1>{1},)(?:{2}))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterEnd)); //Lin CC, et al. 
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU51>(?<SNM51_1>{0} )(?<GNM51_1>{1})(?:{2}))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterEnd)); //Feeley AB et al.
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU54>(?<SNM54_1>{0} )(?<GNM54_1>{1})(?:{2}))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterEnd)); //Iverson D (2010) 
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU54>(?<SNM54_1>{0} )(?<GNM54_1>{1}[,]?)(?:{2}))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterEnd)); //Iverson D (2010)  
                //#endregion

                //#region SET
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU45>(?<SNM45_1>{0} )(?<GNM45_1>{1}[\.,;]+))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Lance Bennett Lance W. and -- Radwan, MM; et al. 
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU46>(?<SNM46_1>{0}, )(?<GNM46_1>{1}[\.,;]+))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Lance Bennett Lance W. and -- Radwan, MM; et al. 
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU46>(?<SNM46_1>{0}[,]? )(?<GNM46_1>{1}[\.,;]+))", sRefSNM_FullPattern, sRefInitialFullPattern)); //Lance Bennett Lance W. and -- Radwan, MM; et al.  
                //#endregion

                //#region SET
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU56>(?<GNM56_1>{0}\. )(?<SNM56_1>{1}[\.]?)(?:{2}))", sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterEnd)); //M. A. Boles. 
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU57>(?<GNM57_1>{0}\. )(?<SNM57_1>{1})(?:{2}))", sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterEnd)); //I. Ciornei (2019)
                ////lstAuthorPattern_End.Add(string.Format(@"(?<AU61>(?<GNM61_1>{0}\. )(?<SNM61_1>{1}[\.,]+ )(?:{2}))", sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterEnd)); //C. Bastianenet, et al, 
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU61>(?<GNM61_1>{0}\. )(?<SNM61_1>{1}(?:\.|,|\.,)?)(?:{2}))", sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterEnd)); //C. Bastianenet, et al,  
                //#endregion

                #region SET
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU55>(?<GNM55_1>{0}\. )(?<SNM55_1>{1},[;]?))", sRefInitialFullPattern, sRefSNM_FullPattern)); //H. J. Snaith, -- K. Zhu,; 
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU58>(?<GNM58_1>{0} )(?<SNM58_1>{1}(?:\.|,|\.,)))", sRefInitialFullPattern, sRefSNM_FullPattern)); //WA Rutala. 2006.
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU62>(?<GNM62_1>{0}\. )(?<SNM62_1>{1}(?:\.|,) ))", sRefInitialFullPattern, sRefSNM_FullPattern)); //R. Langer.
                lstAuthorPattern_End.Add(string.Format(@"(?<AU62>(?<GNM62_1>{0}[\.]? )(?<SNM62_1>{1}(?:\.|,[;]?|\.[,;]) ))", sRefInitialFullPattern, sRefSNM_FullPattern));
                #endregion
                #endregion
                #region OldCodes
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU41>(?<GNM41_1>{0} {1}\. )(?<SNM41_1>{2}[\.])(?:{3}))", sGNM_SingleWord, sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterEnd)); //Maxwell E. McCombs.
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU42>(?<GNM42_1>{0} {1}\. )(?<SNM42_1>{2})(?:{3}))", sGNM_SingleWord, sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterEnd)); //Burton G. Cour-Palais (1978). 
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU48>(?<SNM48_1>{0}, )(?<GNM48_1>{1})(?:{2}))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterEnd)); //Baird, J (2014) 
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU50>(?<SNM50_1>{0}, )(?<GNM50_1>{1},)(?:{2}))", sRefSNM_FullPattern, sGNM_SingleWord, sRefAuthorDelimeterEnd)); //Dana, Rutledge, et al. 
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU59>(?<GNM59_1>{0} )(?<SNM59_1>{1}(?:\.|,|\.,))(?:{2}))", sGNM_SingleWord, sRefSNM_FullPattern, sRefAuthorDelimeterEnd)); //Corey Billington,
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU60>(?<GNM60_1>{0} {1} )(?<SNM60_1>{2},)(?:{3}))", sGNM_SingleWord, sRefInitialFullPattern, sSNM_SingleWord, sRefAuthorDelimeterEnd)); //Oistein E Andersen,
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU61>(?<SNM61_1>{0}, )(?<GNM61_1>(?:{1} )*{1})(?:{2}))", sRefSNM_FullPattern, sGNM_SingleWord, sRefAuthorDelimeterEnd)); //Kulkarni, Dhaval 
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU43>(?<SNM43_1>{0}, )(?<GNM43_1>{1} {2}\.))", sSNM_SingleWord, sGNM_SingleWord, sRefInitialFullPattern)); //Avis, Walter S. 1955.
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU61>(?<SNM61_1>{0}, )(?<GNM61_1>(?:{1} )*{1}\.))", sRefSNM_FullPattern, sGNM_SingleWord)); //Allott, Caetlin Benson. 
                #endregion
                //Maxwell E. McCombs.
                lstAuthorPattern_End.Add(string.Format(@"(?<AU41>(?<GNM41_1>{0} {1}\. )(?<SNM41_1>{2}[\.,])(?:{3})?)", sGNM_SingleWord, sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterEnd));

                //Burton G. Cour-Palais (1978). 
                lstAuthorPattern_End.Add(string.Format(@"(?<AU42>(?<GNM42_1>{0} {1}\. )(?<SNM42_1>{2})(?:{3}))", sGNM_SingleWord, sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterEnd));
                //Baird, J (2014) 
                lstAuthorPattern_End.Add(string.Format(@"(?<AU48>(?<SNM48_1>{0}, )(?<GNM48_1>{1}[\.]?)(?:{2}))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterEnd));
                //Iverson D (2010) --> //Lin CC, et al. --> //Feeley AB et al. --> //Iverson D (2010) 
                lstAuthorPattern_End.Add(string.Format(@"(?<AU54>(?<SNM54_1>{0} )(?<GNM54_1>{1}[,\.]?)(?:{2}))", sRefSNM_FullPattern, sRefInitialFullPattern, sRefAuthorDelimeterEnd));

                //updated by Dakshinamoorthy on 2020-Jul-10
                //Dana, Rutledge, et al. 
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU50>(?<SNM50_1>{0}, )(?<GNM50_1>{1},)(?:{2}))", sRefSNM_FullPattern, sGNM_SingleWord, sRefAuthorDelimeterEnd));
                lstAuthorPattern_End.Add(string.Format(@"(?<AU50>(?<SNM50_1>{0}, )(?<GNM50_1>(?:{1}[\.]?) {2}[\.,]*)(?:{3}))", sRefSNM_FullPattern, sRefInitialFullPattern, sGNM_SingleWord, sRefAuthorDelimeterEnd));

                //added by Dakshinamoorthy on 2020-Jul-10
                lstAuthorPattern_End.Add(string.Format(@"(?<AU50>(?<SNM50_1>{0}, )(?<GNM50_1>{1} (?:{2}[\.]?))(?:{3}))", sRefSNM_FullPattern, sGNM_SingleWord, sRefInitialFullPattern, sRefAuthorDelimeterEnd));

                //C. Bastianenet, et al,  --> //M. A. Boles.  --> //I. Ciornei (2019) --> //C. Bastianenet, et al, 
                lstAuthorPattern_End.Add(string.Format(@"(?<AU61>(?<GNM61_1>{0}\. )(?<SNM61_1>{1}(?:\.|,|\.,)?)(?:{2}))", sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterEnd));

                //updated by Dakshinamoorthy on 2020-Dec-2020
                //Corey Billington,
                //lstAuthorPattern_End.Add(string.Format(@"(?<AU59>(?<GNM59_1>{0} )(?<SNM59_1>{1}(?:\.|,|\.,))(?:{2}))", sGNM_SingleWord, sRefSNM_FullPattern, sRefAuthorDelimeterEnd));
                lstAuthorPattern_End.Add(string.Format(@"(?<AU59>(?<SNM59_1>{1} )(?<GNM59_1>{0}(?:,|\.,))(?:{2}))", sRefSNM_FullPattern, sGNM_SingleWord, sRefAuthorDelimeterEnd));
                lstAuthorPattern_End.Add(string.Format(@"(?<AU62>(?<SNM62_1>{1} )(?<GNM62_1>{0}(?:\.)))", sRefSNM_FullPattern, sGNM_SingleWord));

                //Oistein E Andersen,
                lstAuthorPattern_End.Add(string.Format(@"(?<AU60>(?<GNM60_1>{0} {1} )(?<SNM60_1>{2},)(?:{3}))", sGNM_SingleWord, sRefInitialFullPattern, sSNM_SingleWord, sRefAuthorDelimeterEnd));
                //added by Dakshinamoorthy on 2020-Apr-29
                lstAuthorPattern_End.Add(string.Format(@"(?<AU61>(?<SNM61_1>{0}, )(?<GNM61_1>(?:{1} )*{1})(?:{2}))", sRefSNM_FullPattern, sGNM_SingleWord, sRefAuthorDelimeterEnd)); //Kulkarni, Dhaval 

                //added by Dakshinamoorthy on 2020-Jul-10
                //Maria Weber (2009).
                lstAuthorPattern_End.Add(string.Format(@"(?<AU61>(?<SNM61_1>{0} )(?<GNM61_1>{1})(?:{2}))", sRefSNM_FullPattern, sGNM_SingleWord, sRefAuthorDelimeterEnd));
                //B Weingast (1999).
                lstAuthorPattern_End.Add(string.Format(@"(?<AU61>(?<GNM61_1>{0} )(?<SNM61_1>{1})(?:{2}))", sRefInitialFullPattern, sRefSNM_FullPattern, sRefAuthorDelimeterEnd));

                //Campbell DA, Jr. -- Ritchie, D. Sr. eds. 2011. --> //Fahey GC Jr, <i>et al.</i> --> //Campbell DA, Jr. -- Ritchie, D. Sr. eds. 2011. 
                lstAuthorPattern_End.Add(string.Format(@"(?<AU39>(?<SNM39_1>{0}[,]? )(?<GNM39_1>{1}[,\.]? )(?<SUF39_1>{2}(?:[,;]|\.[,]?)))", sRefSNM_FullPattern, sRefInitialFullPattern, sSuffixPattern));
                //Ulibarri Jr., K.R.  --> //Porte Jr D (1977)  --> //Izzo Jr JL, et al. --> //Ulibarri Jr., K.R. 
                lstAuthorPattern_End.Add(string.Format(@"(?<AU40>(?<SNM40_1>{0} )(?<SUF40_1>{1}(?:\.,|,)? )(?<GNM40_1>{2}(?:[,;\.]|\.,)?))", sRefSNM_FullPattern, sSuffixPattern, sRefInitialFullPattern));
                //Avis, Walter S. 1955.
                lstAuthorPattern_End.Add(string.Format(@"(?<AU43>(?<SNM43_1>{0}, )(?<GNM43_1>{1} {2}\.))", sSNM_SingleWord, sGNM_SingleWord, sRefInitialFullPattern));
                //Melika, G., <i>et al</i>. --> //Abro, R., --> //Schubert, E. F. --> //Seo, S.; <i>et al.</i>  --> //Melika, G., <i>et al</i>.
                lstAuthorPattern_End.Add(string.Format(@"(?<AU53>(?<SNM53_1>{0}[,]? )(?<GNM53_1>{1}\.[,;]?))", sRefSNM_FullPattern, sRefInitialFullPattern));
                //Lance Bennett Lance W. and -- Radwan, MM; et al. --> //Lance Bennett Lance W. and -- Radwan, MM; et al.  
                lstAuthorPattern_End.Add(string.Format(@"(?<AU46>(?<SNM46_1>{0}[,]? )(?<GNM46_1>{1}[\.,;]+))", sRefSNM_FullPattern, sRefInitialFullPattern));
                //H. J. Snaith, -- K. Zhu,; --> //WA Rutala. 2006. --> //R. Langer.
                lstAuthorPattern_End.Add(string.Format(@"(?<AU62>(?<GNM62_1>{0}[\.]? )(?<SNM62_1>{1}(?:\.|,[;]?|\.[,;]) ))", sRefInitialFullPattern, sRefSNM_FullPattern));
                lstAuthorPattern_End.Add(string.Format(@"(?<AU61>(?<SNM61_1>{0}, )(?<GNM61_1>(?:{1} )*{1}\.))", sRefSNM_FullPattern, sGNM_SingleWord)); //Allott, Caetlin Benson.



                //Hernandez-Castellano LE 2017. 
                //Paula Vainiomaki P (2004) 
                //Nina Glick Schiller. 2011. 
                //Mehmet Nargelecekenler. 
                //Horaczek, Nina (2017)
                //Orsnes, Bjarne. 2012. 

                sAuthorGroupPattern_End = string.Join("|", lstAuthorPattern_End.ToArray());

                //sAuthorGroupPattern = string.Format("^(?:(?<GRP_START>(?:{0}))(?<GRP_AND>(?:(?:{1})(?<DEL_AND>{2} ))?)(?<GRP_END>{3})?)", sAuthorGroupPattern_Start, sAuthorGroupPattern_And, sRefAuthorDelimeterAnd, sAuthorGroupPattern_End);
                sAuthorGroupPattern = string.Format("^(?:(?:(?<GRP_START>(?:{0})+)(?<GRP_AND>(?:(?:{1})(?<DEL_AND>{2} ))+)(?<GRP_END>{3})+)|(?:(?<GRP_START>(?:{0})+)(?<GRP_END>{3})+)|(?:(?<GRP_AND>(?:(?:{1})(?<DEL_AND>{2} ))+)(?<GRP_END>{3})+)|(?:(?<GRP_END>{3})+))", sAuthorGroupPattern_Start, sAuthorGroupPattern_And, sRefAuthorDelimeterAnd, sAuthorGroupPattern_End);

                string sAuthorGroupPattern_AndOnly = string.Format("(?:(?<GRP_AND>(?:{0})(?<DEL_AND>{1} ))(?<GRP_END>{2}))", sAuthorGroupPattern_And, sRefAuthorDelimeterAnd, sAuthorGroupPattern_End);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructRefAuthor.cs\GenerateSingleAuthorPattern", ex.Message, true, "");
                return false;
            }
            return true;
        }
    }
}
