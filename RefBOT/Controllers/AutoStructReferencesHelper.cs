//using Npgsql;
//using eBOT.Stanford_English_NLP_Service;
using OpenNLP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Data.SqlClient;

namespace RefBOT.Controllers
{
    class AutoStructReferencesHelper
    {
        public DataTable dtRefData = new DataTable();
        public string sRefTypeLocalExePtn = "(?:Book|Communication|Conference|Journal|Other|Patent|References|Report|Thesis|Web)";
        public string sRefTypeTagPattern = @"(?:ldlRefTypeJournal|ldlRefTypeBook|ldlRefTypeWeb|ldlRefTypePaper|ldlRefTypeConference|ldlRefTypePatent|ldlRefTypeReport|ldlRefTypeThesis|ldlRefTypeOther)";
        public string sRefTypeGroupTagPattern = @"(?:ldlGroupRefTypeJournal|ldlGroupRefTypeBook|ldlGroupRefTypeWeb|ldlGroupRefTypePaper|ldlGroupRefTypeConference|ldlGroupRefTypePatent|ldlGroupRefTypeReport|ldlGroupRefTypeThesis|ldlGroupRefTypeOther)";
        public string sRefElementsLocalExePtn = "(?:Author|Doi|Editor|Etal|FirstPage|Issue_No|ldl|PageRange|PubDate|Vol_No|Website|Year)";
        public string sMonthPattern = @"(?:\b(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b)";
        public string sMonthPatternWithRange = string.Format(@"(?:{0}(?:[ ]?(?:[\u2013\-][ ]?{0}))?)", @"(?:\b(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b)");
        public string sSkippedWordsPattern4Title = "(?:of|the|an|a|the|in|and|to|for|on|&)";
        public string sNonKeyboardCharPattern = "[^a-z0-9~\\!\\@\\#\\$\\%\\^\\*\\(\\)\\\\\\-_\\+\\{\\}><\\[\\]:;,\\.\\/\\?\"\\' \\n\\r\\s]";
        public int nPubDateCount = 0;
        public int nAccessedDateCount = 0;
        public int nConfNameCount = 0;
        public int nReportNoCount = 0;

        //added by Dakshinamoorthy on 2019-Jan-29
        public int nSubRefCount = 0;
        public static int nRegExTimeOut_MS = 60000;

        //added by Dakshinamoorthy on 2020-Jun-02
        public string sRefPrefixContent = string.Empty;

        public string[] arrTestingUsers = { "dakshina-moorthy.g" };
        public bool bIsEditorBackSearch = false;
        public bool bIsEditorFrontSearch = false;
        public bool bIsConfProcTypeRef = false;
        public string sAuthorPatternWithFullCaps = string.Empty;

        //added by Dakshinamoorthy on 2020-Apr-01
        public string sAuthorGroupPattern_Start = string.Empty;
        public string sAuthorGroupPattern_And = string.Empty;
        public string sAuthorGroupPattern_End = string.Empty;
        public string sRefAuthorDelimeterAnd = string.Empty;

        //added by Dakshinamoorthy  on 2020-Jul-13
        public XmlDocument docCityXml = new XmlDocument();
        public XmlDocument docPublisherXml = new XmlDocument();
        public bool bOption4UseLocalLocation = false;

        //added by Dakshinamoorthy on 2020-Jul-24
        public List<string> lstInlinePublisherNames = new List<string>();


        //added by Dakshinamoorthy on 2020-May-27
        //updated by Dakshinamoorthy on 2020-Jul-14
        //public string sRefPrefixPattern = @"(?:(?<=(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)|^))(?:[ ]*(?:(?:[Ss]ee(?: also)?|also cited[\:]?|and|(?:(?:(?:<b>)?[Ss]ources[\:](?:</b>)? )?(?:Compiled|Adapted) from) |(?:Useful reviews include a chapter in )|(?:(?:<b>)?[Ss]ources[\:](?:</b>)?) ))))";
        public string sRefPrefixPattern = @"(?:(?<=(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)|^))(?:[ ]*(?:(?:(?:[\*][ ]?)+|[Ss]ee(?: also)?\b|also cited\b[\:]?|\band\b|(?:(?:(?:<b>)?[Ss]ources[\:](?:</b>)? )?(?:Compiled|Adapted) from\b) |(?:Useful reviews include a chapter in )|(?:(?:<b>)?[Ss]ources[\:](?:</b>)?) ))))";

        //updated by Dakshinamoorthy on 2019-Jan-18
        //string sEdsFrontPattern = @"(?<edsfront>(?<=(?<!st|nd|rd|th),? )(?:[Ee]d\.? by |[Ee]dited\.? by |[Oo]rganized by |[Tt]ranslated by |[\(]?Ed\: ))";
        //updated by Dakshinamoorthy on 2019-Sep-28
        //string sEdsFrontPattern = @"(?<edsfront>(?<=(?<!(?:(?:[0-9]+(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?))|Int),? )(?:[\(]?[Ee]d\.? by\.? |[\(][Tt]rans[,\.]+ |tr\. |(?:[Tt]ranslated (?:and|&) )?[Ee]dited\.? by\.? |[Oo]rganized by |[Dd]iscussion by |[\(]?[Tt]ranslated by |(?:[\[\(][Ee]ds\. )|[\(\[]?[Ee]d[\:\.] ))";
        //updated by Dakshinamoorthy on 2020-Aug-24
        string sEdsFrontPattern = @"(?<edsfront>(?<=(?<!(?:(?:[0-9]+(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?))|Int),? )(?:[\(]?[Ee]d\.? by\.? |tr\. |(?:[Tt]ranslated (?:and|&) )?[Ee]dited\.? by\.? |[Oo]rganized by |[Dd]iscussion by |[\(]?[Tt]ranslated by |[\[\(](?:Ed\. & Trans[\.,]+|Trans\. & Ed[\.,]+|Writer & Director[\.,]+|Writer & Dir[\.,]+|[Nn]arr[\.,]+|[Tt]rans[\.,]+|Director[\.,]+|Dir[\.,]+|Writer[\.,]+|Host[\.,]+|(?:Executive Producer\(s\)[\.,]+)) |(?:[\[\(][Ee]ds\. )|[\(\[]?[Ee]d[\:\.] ))";

        string sJouTitAbbrPattern = @"(?:(?:(?:(?:(?:[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?|[J]|[A-Z][a-z]{1,})\. ){1,})(?:(?:(?:[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?|[J]|[A-Z][a-z]{1,}) ){1,})(?:(?:(?:[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?|[J]|[A-Z][a-z]{1,})\. ){1,}))|(?:(?:(?:[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?|[J]|[A-Z][a-z]{1,})\. ){2,}(?:\([A-Z][a-z]+\)))|(?:(?:(?:[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?|[J]|[A-Z][a-z]{1,})\. ){1,}(?:[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?) (?:(?:[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?|[J]|[A-Z][a-z]{1,})\. ){1,})|(?:(?:(?:[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?|[J]|[A-Z][a-z]{1,})\. ){2,}(?:[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?\b))|(?:(?:(?:[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?|[J]|[A-Z][a-z]{1,})\. ){3,})|(?:(?:(?:[A-Z]{3,}|[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?) )(?:(?:[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?|[J]|[A-Z][a-z]{1,})\. ){2,})|(?:(?:(?:[A-Z][a-z]{2,}(?:\-[A-Z][a-z]{2,})?)\. ){2,}))";

        //string sEdsBackPattern = @"(?<edsback>(?<=(?:(?<!st|nd|rd|th),? |</Etal>[ ]?))(?:\([Ee]ds?\.?\)[\.,]*|[Ee]ds?[\.,]+ |\([Ee]ditors\)[\.,]* |[Ee]ds? |[Ee]ditors?(?: in Chief)?[,\.]* ))";
        //string sEdsBackPattern = @"(?<edsback>(?<=(?:(?<!st|nd|rd|th),? |</Etal>[ ]?))(?:(?:\([Ee]ds?\.?\)[\.,]*|[Ee]ds?[\.,]+(?:[ ]|(?=</ldlAuthorEditorGroup>))|\([Ee]ditors\)[\.,]*(?:[ ]|(?=</ldlAuthorEditorGroup>))|[Ee]ds?(?:[ ]|(?=</ldlAuthorEditorGroup>))|[Ee]ditors?(?: in Chief)?[,\.]*(?:[ ]|(?=</ldlAuthorEditorGroup>)))))";
        //string sEdsBackPattern = @"(?<edsback>(?<=(?:(?<!st|nd|rd|th),? |</Etal>[ ]?))(?:(?:\([Ee]ds?\.?\)(?:[\.,;]*|[ ]*$)|[Ee]ds?[\.,;]+(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|\([Ee]ditors\)[\.,]*(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|[Ee]ds?(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|[Ee]ditors?(?: in Chief)?[,\.]*(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>)))))";

        //updated by Dakshinamoorthy on 2019-Jan-18
        //string sEdsBackPattern = @"(?<edsback>(?<=(?:(?<!st|nd|rd|th),? |</Etal>[ ]?))(?:(?:\([Ee]ds?\.?\)(?:[\.,;]*|[ ]*$)|[Ee]ds?[\.,;]+(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|\([Ee]ditors\)[\.,]*(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|[Ee]ds?(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|[Ee]ditors?(?: in Chief)?[,\.]*(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>)))))";
        //updated by Dakshinamoorthy on 2019-Sep-25
        //string sEdsBackPattern = @"(?<edsback>(?<=(?:(?<!st|nd|rd|th|Int),? |</Etal>[ ]?))(?:(?:\([Ee]ds?\.?\)(?:[\.,;]*|[ ]*$)|[Ee]ds?[\.,;]+(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|\([Ee]ditors\)[\.,]*(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|[Ee]ds?(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|[Ee]ditors?(?: in Chief)?[,\.]*(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>)))))";
        //updated by Dakshinamoorthy on 2019-Sep-25
        //string sEdsBackPattern = @"(?<edsback>(?<=(?:(?<!st|nd|rd|th|Int),? |</Etal>[ ]?))(?:(?:\([Ee]ds?\.?(?: (?:and|&) [Tt]rans)?\.?\)(?:[\.,;]*|[ ]*$)|[Ee]ds?[\.,;]+(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|\((?:Guest )?[Ee]ditors?\)[\.,]*(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|[Ee]ds?(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|[Ee]ditors?(?: in Chief)?[,\.]*(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>)))))";
        //updated by Dakshinamoorthy on 2020-Aug-24
        //string sEdsBackPattern = @"(?<edsback>(?<=(?:(?<!(?:(?:[0-9]+(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?))|Int),? |</Etal>[ ]?))(?:(?:[\(\[][Ee]ds?\.?(?: (?:and|&) [Tt]rans)?\.?[\)\]](?:[\.,;]*|[ ]*$)|(?:[Ee]ds?|[Nn]arr|[Tt]rans)[\.,;\)\]]+(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|(?:[Ee]ditors?[\]\)][\.,]+ )|[\(\[](?:Guest )?[Ee]ditors?[\)\]][\.,]*(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|(?:[Ee]ds?|Trans\.)(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|[Ee]ditors?(?: in Chief)?[,\.]*(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>)))))";
        string sEdsBackPattern = @"(?<edsback>(?<=(?:(?<!(?:(?:[0-9]+(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?))|Int),? |</Etal>[ ]?))(?:(?:[\(\[][Ee]ds?\.?(?: (?:and|&) [Tt]rans)?\.?[\)\]](?:[\.,;]*|[ ]*$)|(?:[Ee]ds?|[Nn]arr|[Tt]rans|Ed\. & Trans|Trans\. & Ed|Director|Dir|Writer|Writer & Director|Writer & Dir|Host|(?:Executive Producer\(s\)))[\.,;\)\]]+(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|(?:[Ee]ditors?[\]\)][\.,]+ )|[\(\[](?:Guest )?[Ee]ditors?[\)\]][\.,]*(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|(?:[Ee]ds?|Trans\.)(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>))|[Ee]ditors?(?: in Chief)?[,\.]*(?:[ ]|[ ]*$|(?=</ldlAuthorEditorGroup>)))))";

        public string sCapsNonEnglishChar = "\u00C0-\u00DF\u0100\u0102\u0104\u0106\u0108\u010A\u010C\u010E\u0110\u0112\u0114\u0116\u0118\u011A\u011C\u011E\u0120\u0122\u0124\u0126\u0128\u012A\u012C\u012E\u0130\u0132\u0134\u0136\u0139\u013B\u013D\u013F\u0141\u0143\u0145\u0147\u014A\u014C\u014E\u0150\u0152\u0154\u0156\u0158\u015A\u015C\u015E\u0160\u0162\u0164\u0166\u0168\u016A\u016C\u016E\u0170\u0172\u0174\u0176\u0178\u0179\u017B\u017D\u0181\u0184\u0186\u0187\u0189\u018A\u018B\u018E\u0191\u0193\u0197\u0198\u019A\u019D\u01A0\u01A2\u01A4\u01A6\u01A7\u01AC\u01AD\u01AF\u01B3\u01B5\u01C4\u01C7\u01C8\u01CA\u01CB\u01CD\u01CF\u01D3\u01D5\u01D7\u01D9\u01DB\u01DE\u01E0\u01E2\u01E4\u01E6\u01E8\u01EA\u01EC\u01EE\u01F1\u01F2\u01F4\u01F6\u01F8\u01FA\u01FC\u01FE\u0200\u0202\u0204\u0206\u0208\u020A\u020C\u020E\u0210\u0212\u0214\u0216\u0218\u021A\u021C\u021E\u0224\u0226\u0228\u022A\u022C\u022E\u0230\u0232\u023A\u023B\u023E\u0243\u0244\u0246\u0248\u024A\u024C\u024E\u0290\u0299\u029B\u029C\u029F\u0372\u0376\u0386\u0388\u0389\u038A\u038C\u038E\u03AA\u03AB\u03CF\u03DC\u03FA\u0400\u0401\u0402\u0405\u0406\u0407\u0408\u0409\u040C\u040D\u040E\u0410\u0411\u0412\u0415\u041A\u041C\u041D\u041E\u0420\u0421\u0422\u0423\u0425\u042A\u042B\u043B\u043C\u043D\u043E\u044F\u045C\u045D\u045E\u0460\u0474\u0475\u0476\u048A\u048C\u048E\u049A\u049C\u049E\u04A0\u04A2\u04A4\u04AA\u04AC\u04AE\u04AF\u04B0\u04B1\u04B2\u04B3\u04B6\u04B8\u04BA\u04BC\u04BE\u04C0\u04C3\u04C7\u04C9\u04CB\u04CD\u04CF\u04D0\u04D2\u04D4\u04D6\u04D8\u04DA\u04E2\u04E4\u04E6\u04E8\u04EA\u04EE\u04F0\u04F2\u04FC\u04FE\u051A\u051C\u051E\u1E00\u1E02\u1E04\u1E06\u1E08\u1E0A\u1E0C\u1E0E\u1E10\u1E12\u1E14\u1E16\u1E18\u1E1A\u1E1C\u1E1E\u1E20\u1E22\u1E24\u1E26\u1E28\u1E2A\u1E2C\u1E2E\u1E30\u1E32\u1E34\u1E36\u1E38\u1E3A\u1E3C\u1E3E\u1E40\u1E42\u1E44\u1E46\u1E48\u1E4A\u1E4C\u1E4E\u1E50\u1E52\u1E54\u1E56\u1E58\u1E5A\u1E5C\u1E5E\u1E60\u1E62\u1E64\u1E66\u1E68\u1E6A\u1E6C\u1E6E\u1E70\u1E72\u1E74\u1E76\u1E78\u1E7A\u1E7C\u1E7E\u1E80\u1E82\u1E84\u1E86\u1E88\u1E8A\u1E8C\u1E8E\u1E90\u1E92\u1E94\u1EA0\u1EA2\u1EA4\u1EA6\u1EA8\u1EAA\u1EAC\u1EAE\u1EB0\u1EB2\u1EB4\u1EB6\u1EB8\u1EBA\u1EBC\u1EBE\u1EC0\u1EC2\u1EC4\u1EC6\u1EC8\u1ECA\u1ECC\u1ECE\u1ED0\u1ED2\u1ED4\u1ED6\u1ED8\u1EDA\u1EDC\u1EDE\u1EE0\u1EE2\u1EE4\u1EE6\u1EE8\u1EEA\u1EEC\u1EEE\u1EF0\u1EF2\u1EF4\u1EF6\u1EF8\u1EFA\u1EFE\u1F08\u1F09\u1F0A\u1F0B\u1F0C\u1F0D\u1F0E\u1F0F\u1F1A\u1F1B\u1F1C\u1F1D\u1F28\u1F2A\u1F2B\u1F2C\u1F2D\u1F2E\u1F2F\u1F38\u1F39\u1F3A\u1F3B\u1F3C\u1F3D\u1F3E\u1F3F\u1F48\u1F49\u1F4A\u1F4B\u1F4C\u1F4D\u1F59\u1F5B\u1F5D\u1F5F\u1F89\u1F8A\u1F8B\u1F8C\u1F8D\u1F8E\u1F8F\u1F98\u1F99\u1F9A\u1F9B\u1F9C\u1F9D\u1F9E\u1F9F\u1FB8\u1FB9\u1FBA\u1FBB\u1FBC\u1FC8\u1FC9\u1FCA\u1FCB\u1FCC\u1FD6\u1FD7\u1FD8\u1FD9\u1FDA\u1FDB\u1FE6\u1FE7\u1FE8\u1FE9\u1FEA\u1FEB\u1FEC\u1FF8\u1FF9\u2C60\u2C62\u2C63\u2C64\u2C67\u2C69\u2C6B\u2C6E\u2C7E\u2C7F\uA740\uA742\uA744\uA748\uA74A\uA74C\uA74E\uA750\uA752\uA756\uA758\uA75E\uA764\uA766\uA7A0\uA7A2\uA7A4\uA7A6\uA7A8\uA7FE\uFFE5\uFFE6";

        public string sSmallNonEnglishChar = "\u00E0-\u00FF\u0101\u0103\u0105\u0107\u0109\u010B\u010D\u010F\u0111\u0113\u0115\u0117\u0119\u011B\u011D\u011F\u0121\u0123\u0125\u0127\u0129\u012B\u012D\u012F\u0133\u0135\u0137\u0138\u013A\u013C\u013E\u0140\u0142\u0144\u0146\u0148\u0149\u014B\u014D\u014F\u0151\u0153\u0155\u0157\u0159\u015B\u015D\u015F\u0161\u0163\u0165\u0167\u0169\u016B\u016D\u016F\u0171\u0173\u0175\u0177\u017A\u017C\u017E\u017F\u0180\u0182\u0185\u0188\u018C\u018F\u0192\u0199\u01A1\u01A3\u01A5\u01A8\u01AB\u01AE\u01B0\u01B2\u01B4\u01B6\u01C5\u01C6\u01C9\u01CC\u01CE\u01D0\u01D2\u01D4\u01D6\u01D8\u01DA\u01DC\u01DD\u01DF\u01E1\u01E3\u01E5\u01E7\u01E9\u01EB\u01ED\u01EF\u01F0\u01F3\u01F5\u01F9\u01FB\u01FD\u01FF\u0201\u0203\u0205\u0207\u0209\u020B\u020D\u020F\u0211\u0213\u0215\u0217\u0219\u021B\u021D\u021F\u0225\u0227\u0229\u022B\u022D\u022F\u0231\u0233\u023C\u023F\u0240\u0247\u0249\u024B\u024D\u024F\u0250\u0251\u0252\u0259\u0258\u0260\u0261\u0265\u0266\u0267\u0268\u026B\u0287\u0288\u0289\u028B\u028D\u028E\u028F\u0291\u029E\u02A3\u02A4\u02A5\u02A6\u02A7\u02AA\u02AB\u02B0\u02B4\u02B6\u0390\u03BF\u03CA\u03CB\u03CC\u03CD\u03CE\u03DD\u03FB\u040A\u040B\u042C\u0435\u043A\u0450\u0451\u0457\u0461\u0477\u048B\u048D\u048F\u049B\u049D\u049F\u04A1\u04A3\u04A5\u04AB\u04AD\u04B7\u04B9\u04BB\u04BD\u04BF\u04C4\u04C8\u04CA\u04CC\u04CE\u04D1\u04D3\u04D5\u04D7\u04D9\u04DB\u04E3\u04E5\u04E7\u04E9\u04EB\u04EF\u04F1\u04F3\u04FD\u04FF\u051B\u051D\u051F\u0526\u0527\u1E01\u1E03\u1E05\u1E07\u1E09\u1E0B\u1E0D\u1E0F\u1E11\u1E13\u1E15\u1E17\u1E19\u1E1B\u1E1D\u1E1F\u1E21\u1E23\u1E25\u1E27\u1E29\u1E2B\u1E2D\u1E2F\u1E31\u1E35\u1E37\u1E39\u1E3B\u1E3D\u1E3F\u1E41\u1E43\u1E45\u1E47\u1E49\u1E4B\u1E4D\u1E4F\u1E51\u1E53\u1E55\u1E57\u1E59\u1E5B\u1E5D\u1E5F\u1E61\u1E63\u1E65\u1E67\u1E69\u1E6B\u1E6D\u1E6F\u1E71\u1E73\u1E75\u1E77\u1E79\u1E7B\u1E7D\u1E7F\u1E81\u1E83\u1E85\u1E87\u1E89\u1E8B\u1E8D\u1E8F\u1E91\u1E93\u1E95\u1E96\u1E97\u1E98\u1E99\u1E9A\u1EA1\u1EA3\u1EA5\u1EA7\u1EA9\u1EAB\u1EAD\u1EAF\u1EB1\u1EB3\u1EB5\u1EB7\u1EB9\u1EBB\u1EBD\u1EBF\u1EC1\u1EC3\u1EC5\u1EC7\u1EC9\u1ECB\u1ECD\u1ECF\u1ED1\u1ED3\u1ED5\u1ED7\u1ED9\u1EDB\u1EDD\u1EDF\u1EE1\u1EE3\u1EE5\u1EE7\u1EE9\u1EEB\u1EED\u1EEF\u1EF1\u1EF3\u1EF5\u1EF7\u1EF9\u1EFB\u1EFF\u1F18\u1F29\u1F78\u1F79\u1F7A\u1F7C\u1F7D\u2C65\u2C66\u2C68\u2C6A\u2C6C\u2C71\u2C73\uA741\uA743\uA745\uA749\uA74B\uA74D\uA74F\uA751\uA753\uA757\uA759\uA75F\uA765\uA767\uA7A1\uA7A3\uA7A5\uA7A7\uA7A9\u00DF\u00F8\u0131";


        string sNationalityPattern = @"(?:\b(?:(?:Afghan)|(?:Albanian)|(?:Algerian)|(?:Andorran)|(?:Angolan)|(?:Argentinian)|(?:Armenian)|(?:Australian)|(?:Austrian)|(?:Azerbaijani)|(?:Bahamian)|(?:Bahraini)|(?:Bangladeshi)|(?:Barbadian)|(?:Belarusian|Belarusan)|(?:Belgian)|(?:Belizean)|(?:Beninese)|(?:Bhutanese)|(?:Bolivian)|(?:Bosnian)|(?:Botswanan)|(?:Brazilian)|(?:British)|(?:Bruneian)|(?:Bulgarian)|(?:Burkinese)|(?:Burmese)|(?:Burundian)|(?:Cambodian)|(?:Cameroonian)|(?:Canadian)|(?:Cape Verdean)|(?:Chadian)|(?:Chilean)|(?:Chinese)|(?:Colombian)|(?:Congolese)|(?:Costa Rican)|(?:Croat|Croatian)|(?:Cuban)|(?:Cypriot)|(?:Czech)|(?:Danish)|(?:Djiboutian)|(?:Dominican)|(?:Dominican)|(?:Ecuadorean)|(?:Egyptian)|(?:Salvadorean)|(?:English)|(?:Eritrean)|(?:Estonian)|(?:Ethiopian)|(?:Fijian)|(?:Finnish)|(?:French)|(?:Gabonese)|(?:Gambian)|(?:Georgian)|(?:German)|(?:Ghanaian)|(?:Greek)|(?:Grenadian)|(?:Guatemalan)|(?:Guinean)|(?:Guyanese)|(?:Haitian)|(?:Dutch)|(?:Honduran)|(?:Hungarian)|(?:Icelandic)|(?:Indian)|(?:Indonesian)|(?:Iranian)|(?:Iraqi)|(?:Irish)|(?:Italian)|(?:Jamaican)|(?:Japanese)|(?:Jordanian)|(?:Kazakh)|(?:Kenyan)|(?:Kuwaiti)|(?:Laotian)|(?:Latvian)|(?:Lebanese)|(?:Liberian)|(?:Libyan)|(?:Lithuanian)|(?:Macedonian)|(?:Malagasy|Madagascan)|(?:Malawian)|(?:Malaysian)|(?:Maldivian)|(?:Malian)|(?:Maltese)|(?:Mauritanian)|(?:Mauritian)|(?:Mexican)|(?:Moldovan)|(?:Monégasque|Monacan)|(?:Mongolian)|(?:Montenegrin)|(?:Moroccan)|(?:Mozambican)|(?:Namibian)|(?:Nepalese)|(?:Dutch)|(?:New Zealand)|(?:Nicaraguan)|(?:Nigerien)|(?:Nigerian)|(?:North Korean)|(?:Norwegian)|(?:Omani)|(?:Pakistani)|(?:Panamanian)|(?:Papua New Guinean|Guinean)|(?:Paraguayan)|(?:Peruvian)|(?:Philippine)|(?:Polish)|(?:Portuguese)|(?:Qatari)|(?:Romanian)|(?:Russian)|(?:Rwandan)|(?:Saudi Arabian|Saudi)|(?:Scottish)|(?:Senegalese)|(?:Serb|Serbian)|(?:Seychellois)|(?:Sierra Leonian)|(?:Singaporean)|(?:Slovak)|(?:Slovene|Slovenian)|(?:Somali)|(?:South African)|(?:South Korean)|(?:Spanish)|(?:Sri Lankan)|(?:Sudanese)|(?:Surinamese)|(?:Swazi)|(?:Swedish)|(?:Swiss)|(?:Syrian)|(?:Taiwanese)|(?:Tajik|Tadjik)|(?:Tanzanian)|(?:Thai)|(?:Togolese)|(?:Trinidadian)|(?:Tobagan)|(?:Tobagonian)|(?:Tunisian)|(?:Turkish)|(?:Turkmen|Turkoman)|(?:Tuvaluan)|(?:Ugandan)|(?:Ukrainian)|(?:UAE|Emirates|Emirati)|(?:UK|British)|(?:US)|(?:Uruguayan)|(?:Uzbek)|(?:Vanuatuan)|(?:Venezuelan)|(?:Vietnamese)|(?:Welsh)|(?:Western Samoan)|(?:Yemeni)|(?:Yugoslav)|(?:Zaïrean)|(?:Zambian)|(?:Zimbabwean)|(?:Asian)|(?:Australasian))\b)";



        public enum RefType
        {
            ldlRefTypeJournal,
            ldlRefTypeBook,
            ldlRefTypeWeb,
            ldlRefTypePaper,
            ldlRefTypeConference,
            ldlRefTypePatent,
            ldlRefTypeReport,
            ldlRefTypeThesis,
            ldlRefTypeOther
        }

        #region Old Code
        //public enum RefSkipElements_GROBID
        //{
        //    ldlRefLabel,
        //    ldlAuthorInPrevoiusRef,
        //    ldlCollab,
        //    ldlAuthorSurName,
        //    ldlAuthorGivenName,
        //    ldlAuthorSuffix,
        //    ldlAuthorDelimiterEtal,
        //    ldlAuthorDelimiterAnd,
        //    ldlAuthorDelimiterChar,
        //    ldlEditorSurName,
        //    ldlEditorGivenName,
        //    ldlEditorSuffix,
        //    ldlEditorDelimiterEtal,
        //    ldlEditorDelimiterAnd,
        //    ldlEditorDelimiterChar,
        //    ldlEditorDelimiterEds,
        //    ldlEditorDelimiterEds_Front,
        //    ldlEditorDelimiterEds_Back,
        //    ldlPublicationYear,
        //    ldlPublicationMonth,
        //    ldlPublicationDay,
        //    ldlAccessedDate,
        //    ldlAccessedYear,
        //    ldlAccessedMonth,
        //    ldlAccessedDay,
        //    ldlUpdatedYear,
        //    ldlUpdatedMonth,
        //    ldlUpdatedDay,
        //    ldlConferenceYear,
        //    ldlConferenceMonth,
        //    ldlConferenceDay,
        //    ldlSeason,
        //    ldlTitleLabel,
        //    ldlAccessedDateLabel,
        //    ldlURLLabel,
        //    ldlEmailLabel,
        //    ldlPublisherName,
        //    ldlInstitutionName
        //} 
        #endregion
        public enum RefSkipElements_GROBID
        {
            ldlRefLabel,
            ldlAuthorInPrevoiusRef,
            ldlCollab,
            ldlAuthorSurName,
            ldlAuthorGivenName,
            ldlAuthorSuffix,
            ldlAuthorDelimiterEtal,
            ldlAuthorDelimiterAnd,
            ldlAuthorDelimiterChar,
            ldlEditorSurName,
            ldlEditorGivenName,
            ldlEditorSuffix,
            ldlEditorDelimiterEtal,
            ldlEditorDelimiterAnd,
            ldlEditorDelimiterChar,
            ldlEditorDelimiterEds,
            ldlEditorDelimiterEds_Front,
            ldlEditorDelimiterEds_Back,
            ldlConferenceDate,
            ldlConferenceSponser,
            ldlPublicationYear,
            ldlPublicationMonth,
            ldlPublicationDay,
            ldlAccessedDate,
            ldlAccessedYear,
            ldlAccessedMonth,
            ldlAccessedDay,
            ldlUpdatedYear,
            ldlUpdatedMonth,
            ldlUpdatedDay,
            ldlConferenceYear,
            ldlConferenceMonth,
            ldlConferenceDay,
            ldlSeason,
            ldlTitleLabel,
            ldlAccessedDateLabel,
            ldlURLLabel,
            ldlEmailLabel,
            ldlVolumeLabel,
            ldlIssueLabel,
            ldlEditionLabel,
            ldlPageLabel,
            ldlDOILabel,
            ldlPubMedIdLabel,
            ldlISBNLabel,
            ldlISSNLabel,
            ldlEditionNumber,
            ldlDOINumber,
            ldlPubMedIdNumber,
            ldlISBNNumber,
            ldlISSNNumber,
            ldlELocationId,
            ldlURL,
            ldlEmail,
            ldlPublisherName,
            ldlInstitutionName,
            ldlPublisherLocation,
            ldlConferenceLocation,
            ldlCity,
            ldlState,
            ldlCountry,
            ldlThesisKeyword,
            ldlReportKeyword,
            ldlReportNumber,
            ldlMisc,
            ldlRefPrefix,
            ldlPaperKeyword
        }

        public enum RefElements
        {
            ldlUnknownElement,
            ldlRefLabel,
            ldlAuthorInPrevoiusRef,
            ldlCollab,
            ldlAuthorSurName,
            ldlAuthorGivenName,
            ldlAuthorSuffix,
            ldlAuthorDelimiterEtal,
            ldlAuthorDelimiterAnd,
            ldlAuthorDelimiterChar,
            ldlEditorSurName,
            ldlEditorGivenName,
            ldlEditorSuffix,
            ldlEditorDelimiterEtal,
            ldlEditorDelimiterAnd,
            ldlEditorDelimiterChar,
            ldlEditorDelimiterEds,
            ldlEditorDelimiterEds_Front,
            ldlEditorDelimiterEds_Back,
            ldlArticleTitle,
            ldlJournalTitle,
            ldlChapterTitle,
            ldlBookTitle,
            ldlSourceTitle,
            ldlConferenceName,
            ldlConferenceDate,
            ldlConferenceSponser,
            ldlPublicationYear,
            ldlPublicationMonth,
            ldlPublicationDay,
            ldlAccessedDate,
            ldlAccessedYear,
            ldlAccessedMonth,
            ldlAccessedDay,
            ldlUpdatedYear,
            ldlUpdatedMonth,
            ldlUpdatedDay,
            ldlConferenceYear,
            ldlConferenceMonth,
            ldlConferenceDay,
            ldlSeason,
            ldlTitleLabel,
            ldlAccessedDateLabel,
            ldlURLLabel,
            ldlEmailLabel,
            ldlVolumeLabel,
            ldlIssueLabel,
            ldlEditionLabel,
            ldlPageLabel,
            ldlDOILabel,
            ldlPubMedIdLabel,
            ldlISBNLabel,
            ldlISSNLabel,
            ldlVolumeNumber,
            ldlIssueNumber,
            ldlEditionNumber,
            ldlFirstPageNumber,
            ldlLastPageNumber,
            ldlPageRange,
            ldlDOINumber,
            ldlPubMedIdNumber,
            ldlISBNNumber,
            ldlISSNNumber,
            ldlELocationId,
            ldlURL,
            ldlEmail,
            ldlPublisherName,
            ldlInstitutionName,
            ldlPublisherLocation,
            ldlConferenceLocation,
            ldlCity,
            ldlState,
            ldlCountry,
            ldlThesisKeyword,
            ldlReportKeyword,
            ldlReportNumber,
            ldlMisc,
            ldlRefPrefix,
            ldlPaperKeyword
        }


        private string RemoveUnwantedItalicWithTitle(string sRefTaggedContent)
        {
            try
            {
                //italic merged with title
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"((?<=<i>(?:(?!</?i>).)+, )[0-9]{1,3})</i>(, [0-9]+\.)", "</i>$1$2");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"((?<=<i>(?:(?!</?i>).)+, )[0-9]{1,3})</i>(<split/>\()", "</i>$1$2");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"((?<=<i>(?:(?!</?i>).)+, )[0-9]{1,3},?)</i>(,? [0-9]+(?:[ ]?[\u2013\-][ ]?[0-9]+)?\.)", "</i>$1$2");
                //added by Dakshinamoorthy on 2019-Oct-03
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @" \(</i>(?=(?:(?![\(\)]|</?i>).)+\))", "</i> (");
                //added by Dakshinamoorthy on 2020-Jul-25
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"((?<=[ ])(?:[A-Z][a-z]+))</i>([a-z])(?=,[ ])", "$1$2</i>");
                //added by Dakshinamoorthy on 2020-Aug-11
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:(?:(?<=(?:, ))([A-Z])[\.]{2})(?=<split/>|[ ]))", "$1.");

                sRefTaggedContent = AutoStructRefCommonFunctions.NormalizeTagSpaces(sRefTaggedContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveUnwantedItalicWithTitle", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        private string ChangeSubRefLabel(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "<ldlRefLabel>", "<ldlSubRefLabel>");
                sRefContent = Regex.Replace(sRefContent, "</ldlRefLabel>", "</ldlSubRefLabel>");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ChangeSubRefLabel", ex.Message, true, "");
            }

            return sRefContent;
        }


        //added by Dakshinamoorthy on 2019-Jan-29
        public string ImproveLocalConversionWithSubRef(string sRefOriginalContent, string sRefTaggedContent, ref Dictionary<string, string> dicLogInfo, bool bIsStructureOnlyAutor = false, bool bUseLocalLocationData = false)
        {
            string sTaggedContentWithSubRefs = string.Empty;
            nSubRefCount = 0;
            bOption4UseLocalLocation = bUseLocalLocationData;

            try
            {
                sTaggedContentWithSubRefs = IdentifySubRefs(sRefTaggedContent);

                if (Regex.IsMatch(sTaggedContentWithSubRefs, "(?:<SubRefLabel>(?:(?:(?!</?SubRefLabel>).)+)</SubRefLabel>)"))
                {
                    //string sSubRefContentPtn = @"((?<=(?:<SubRefLabel>(?:(?:(?!</?SubRefLabel>).)+)</SubRefLabel> ))(?:(?:(?!</?SubRefLabel>).)+))(?=(?:(?: <SubRefLabel>(?:(?:(?!</?SubRefLabel>).)+)</SubRefLabel> )|$))";
                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs, "<SubRefLabel>", "<split/><SubRefLabel>");

                    List<string> lstSpittedSubRef = new List<string>();
                    lstSpittedSubRef = Regex.Split(sTaggedContentWithSubRefs, "<split/>").ToList();

                    List<string> lstSpittedSubRef_Output = new List<string>();

                    foreach (string sSplittedContent in lstSpittedSubRef)
                    {
                        if (Regex.IsMatch(sSplittedContent, "^(?:<SubRefLabel>(?:(?:(?!</?SubRefLabel>).)+)</SubRefLabel>)"))
                        {
                            string sCleanedSplittedContent = sSplittedContent;
                            nSubRefCount += 1;
                            sCleanedSplittedContent = Regex.Replace(sCleanedSplittedContent, "</?SubRefLabel>", "");
                            lstSpittedSubRef_Output.Add(ChangeSubRefLabel(ImproveLocalConversion(sCleanedSplittedContent, sCleanedSplittedContent, ref dicLogInfo)));
                        }
                        else
                        {
                            lstSpittedSubRef_Output.Add(sSplittedContent);
                        }
                    }

                    sTaggedContentWithSubRefs = string.Join(" ", lstSpittedSubRef_Output.ToArray());
                    NormalizeSpaces(ref sTaggedContentWithSubRefs);

                    //Change SubRef Label as SubRef Type
                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs,
                        "<ldlRefTypeJournal><ldlSubRefLabel>((?:(?!</?ldlSubRefLabel>).)+)</ldlSubRefLabel>",
                        string.Format("<{0}>$1</{0}>", "ldlSubRefTypeJournal"));

                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs,
                        "<ldlRefTypeBook><ldlSubRefLabel>((?:(?!</?ldlSubRefLabel>).)+)</ldlSubRefLabel>",
                        string.Format("<{0}>$1</{0}>", "ldlSubRefTypeBook"));

                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs,
                        "<ldlRefTypeWeb><ldlSubRefLabel>((?:(?!</?ldlSubRefLabel>).)+)</ldlSubRefLabel>",
                        string.Format("<{0}>$1</{0}>", "ldlSubRefTypeWeb"));

                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs,
                        "<ldlRefTypePaper><ldlSubRefLabel>((?:(?!</?ldlSubRefLabel>).)+)</ldlSubRefLabel>",
                        string.Format("<{0}>$1</{0}>", "ldlSubRefTypePaper"));

                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs,
                        "<ldlRefTypeConference><ldlSubRefLabel>((?:(?!</?ldlSubRefLabel>).)+)</ldlSubRefLabel>",
                        string.Format("<{0}>$1</{0}>", "ldlSubRefTypeConference"));

                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs,
                        "<ldlRefTypePatent><ldlSubRefLabel>((?:(?!</?ldlSubRefLabel>).)+)</ldlSubRefLabel>",
                        string.Format("<{0}>$1</{0}>", "ldlSubRefTypePatent"));

                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs,
                        "<ldlRefTypeReport><ldlSubRefLabel>((?:(?!</?ldlSubRefLabel>).)+)</ldlSubRefLabel>",
                        string.Format("<{0}>$1</{0}>", "ldlSubRefTypeReport"));

                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs,
                        "<ldlRefTypeThesis><ldlSubRefLabel>((?:(?!</?ldlSubRefLabel>).)+)</ldlSubRefLabel>",
                        string.Format("<{0}>$1</{0}>", "ldlSubRefTypeThesis"));

                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs,
                        "<ldlRefTypeOther><ldlSubRefLabel>((?:(?!</?ldlSubRefLabel>).)+)</ldlSubRefLabel>",
                        string.Format("<{0}>$1</{0}>", "ldlSubRefTypeOther"));


                    //string sFirstSubRefType = GetFirstSubRefType(sTaggedContentWithSubRefs);

                    ////Removing Inner Reference Types
                    //sTaggedContentWithSubRefs = RemoveSubRefTypeElements(sTaggedContentWithSubRefs);

                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs, "</?(?:ldlRefTypeJournal|ldlRefTypeBook|ldlRefTypeWeb|ldlRefTypePaper|ldlRefTypeConference|ldlRefTypePatent|ldlRefTypeReport|ldlRefTypeThesis|ldlRefTypeOther)>", "");

                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs, "<RefLabel>", "<ldlRefLabel>");
                    sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs, "</RefLabel>", "</ldlRefLabel>");

                    sTaggedContentWithSubRefs = string.Format("<{0}>{1}</{0}>", "ldlRefTypeSubRef", sTaggedContentWithSubRefs);

                    //calculate matching percentage
                    if (dicLogInfo.ContainsKey("MatchedPercentage"))
                    {
                        string sTotalWords = string.Empty;
                        string sMatchedWords = string.Empty;
                        string sMatchedPercentage = string.Empty;

                        if (dicLogInfo.ContainsKey("TotalWords") && dicLogInfo.ContainsKey("MatchedWords"))
                        {
                            sTotalWords = dicLogInfo["TotalWords"].ToString();
                            sMatchedWords = dicLogInfo["MatchedWords"].ToString();

                            double per = (double)(Convert.ToInt32(sMatchedWords)) / (double)(Convert.ToInt32(sTotalWords));
                            sMatchedPercentage = Convert.ToInt32(per * 100).ToString();
                            dicLogInfo["MatchedPercentage"] = sMatchedPercentage;

                            if (dicLogInfo.ContainsKey("RefType"))
                            {
                                dicLogInfo["RefType"] = "ldlRefTypeSubRef";
                            }
                        }
                    }
                }
                else
                {
                    //added by Dakshinamoorthy on 2020-May-20
                    if (General.GeneralInstance.CustomerName.ToLower().Equals("wileybk"))
                    {
                        sRefOriginalContent = Regex.Replace(sRefOriginalContent, "; ", ";<split/>");
                        sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"(?:(\.) ((?:also cited|see(?: also)?[\: ]+)))", "$1<split/>$2", RegexOptions.IgnoreCase);

                        if (Regex.IsMatch(sRefOriginalContent, "<split/>"))
                        {
                            List<string> lstSplitedRef = new List<string>();
                            List<string> lstSplitedREfOutput = new List<string>();
                            lstSplitedRef = Regex.Split(sRefOriginalContent, "<split/>").ToList();

                            foreach (string sEachRefContent in lstSplitedRef)
                            {
                                //updated by Dakshinamoorthy on 2020-Aug-11
                                //lstSplitedREfOutput.Add(RemoveRefRootTags(ImproveLocalConversion(sEachRefContent.Trim(), sEachRefContent.Trim(), ref dicLogInfo)));
                                lstSplitedREfOutput.Add(ChangeRefRootTagsToGroup(ImproveLocalConversion(sEachRefContent.Trim(), sEachRefContent.Trim(), ref dicLogInfo)));
                            }
                            sTaggedContentWithSubRefs = string.Join(" ", lstSplitedREfOutput);
                            sTaggedContentWithSubRefs = sTaggedContentWithSubRefs.Trim();
                            //added by Dakshinamoorthy on 2020-Aug-2020
                            sTaggedContentWithSubRefs = Regex.Replace(sTaggedContentWithSubRefs, string.Format("(?:(<{0}>)(<ldlRefPrefix>(?:(?!</?ldlRefPrefix>).)+</ldlRefPrefix>[ ]*))", sRefTypeGroupTagPattern), "$2$1");
                            sTaggedContentWithSubRefs = string.Format("<{0}>{1}</{0}>", "ldlRefTypeOther", sTaggedContentWithSubRefs);
                        }
                        else
                        {
                            //updated by Dakshinamoorthy on 2020-Aug-11
                            //sTaggedContentWithSubRefs = ImproveLocalConversion(sRefOriginalContent, sRefTaggedContent, ref dicLogInfo, bIsStructureOnlyAutor: bIsStructureOnlyAutor);
                            sTaggedContentWithSubRefs = ChangeRefRootTagsToGroup(ImproveLocalConversion(sRefOriginalContent, sRefTaggedContent, ref dicLogInfo, bIsStructureOnlyAutor: bIsStructureOnlyAutor), true);
                        }
                    }
                    else
                    {
                        sTaggedContentWithSubRefs = ImproveLocalConversion(sRefOriginalContent, sRefTaggedContent, ref dicLogInfo, bIsStructureOnlyAutor: bIsStructureOnlyAutor);
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ImproveLocalConversionWithSubRef", ex.Message, true, "");
            }

            return sTaggedContentWithSubRefs;
        }

        //added by Dakshinamoorthy on 2020-May-20
        private string RemoveRefRootTags(string sRefOutput)
        {
            try
            {
                sRefOutput = Regex.Replace(sRefOutput, string.Format("(?:</?{0}>)", sRefTypeTagPattern), "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveRefRootTags", ex.Message, true, "");
            }
            return sRefOutput;
        }

        //added by Dakshinamoorthy on 2020-Aug-11
        private string ChangeRefRootTagsToGroup(string sRefOutput, bool IsThisInlineRef = false)
        {
            try
            {
                //updated by Dakshinamoorthy on 2020-Aug-13
                if (IsThisInlineRef == true)
                {
                    sRefOutput = Regex.Replace(sRefOutput, @"<(?:ldl(RefTypeJournal|RefTypeBook|RefTypeWeb|RefTypePaper|RefTypeConference|RefTypePatent|RefTypeReport|RefTypeThesis|RefTypeOther))>", string.Format("<ldl$1><ldlGroup$1>"));
                    sRefOutput = Regex.Replace(sRefOutput, @"</(?:ldl(RefTypeJournal|RefTypeBook|RefTypeWeb|RefTypePaper|RefTypeConference|RefTypePatent|RefTypeReport|RefTypeThesis|RefTypeOther))>", string.Format("</ldlGroup$1></ldl$1>"));
                }
                else
                {
                    sRefOutput = Regex.Replace(sRefOutput, @"<(?:ldl(RefTypeJournal|RefTypeBook|RefTypeWeb|RefTypePaper|RefTypeConference|RefTypePatent|RefTypeReport|RefTypeThesis|RefTypeOther))>", string.Format("<ldlGroup$1>"));
                    sRefOutput = Regex.Replace(sRefOutput, @"</(?:ldl(RefTypeJournal|RefTypeBook|RefTypeWeb|RefTypePaper|RefTypeConference|RefTypePatent|RefTypeReport|RefTypeThesis|RefTypeOther))>", string.Format("</ldlGroup$1>"));
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ChangeRefRootTagsToGroup", ex.Message, true, "");
            }
            return sRefOutput;
        }

        private string IdentifySubRefs(string sRefContent)
        {
            string sOutputContent = sRefContent;

            try
            {
                NormalizeSpaces(ref sOutputContent);
                IdentifyRefLabel(ref sOutputContent);

                if (!Regex.IsMatch(sOutputContent, "^(?:<RefLabel>(?:(?:(?!</?RefLabel>).)+)</RefLabel>)"))
                {
                    return sRefContent;
                }

                IdentifySubRefLabel(ref sOutputContent);
                NormalizeSpaces(ref sOutputContent);

                if (!Regex.IsMatch(sOutputContent, "(?:<SubRefLabel>(?:(?:(?!</?SubRefLabel>).)+)</SubRefLabel>)"))
                {
                    return sRefContent;
                }

                //identifying 1st SubRefLabel
                string sFirstSubRefLabel = string.Empty;
                string sFisrtSubRefLabelPtn = @"^(?:<RefLabel>(?:(?:(?!</?RefLabel>).)+)</RefLabel> (?:<SubRefLabel>((?:(?!</?SubRefLabel>).)+)</SubRefLabel>))";

                if (Regex.IsMatch(sOutputContent, sFisrtSubRefLabelPtn))
                {
                    sFirstSubRefLabel = Regex.Match(sOutputContent, sFisrtSubRefLabelPtn).Groups[1].Value.ToString();
                }
                else
                {
                    return sRefContent;
                }

                string sRegExEscapeCharPattern = @"(?:[\.\$\^\{\[\(\|\)\*\+\?\\])";
                string sCurrentSubRefLabelPtn = Regex.Replace(sFirstSubRefLabel, string.Format("({0})", sRegExEscapeCharPattern), "\\$&");
                sCurrentSubRefLabelPtn = Regex.Replace(sCurrentSubRefLabelPtn, "([a-z])", "[a-z]");
                sCurrentSubRefLabelPtn = Regex.Replace(sCurrentSubRefLabelPtn, "([A-Z])", "[A-Z]");
                sCurrentSubRefLabelPtn = Regex.Replace(sCurrentSubRefLabelPtn, "([0-9])", "[0-9]");
                sCurrentSubRefLabelPtn = string.Format("(?:{0})", sCurrentSubRefLabelPtn);

                sOutputContent = Regex.Replace(sOutputContent, "<SubRefLabel>", "<Split/><SubRefLabel>");
                sOutputContent = Regex.Replace(sOutputContent, "</SubRefLabel>", "</SubRefLabel><Split/>");

                DataTable dtSubRefLabel = new DataTable();
                dtSubRefLabel.Columns.Add("Sno", typeof(int));
                dtSubRefLabel.Columns.Add("RefContent", typeof(string));

                List<string> lstSubRefLabel = new List<string>();
                lstSubRefLabel = Regex.Split(sOutputContent, "<Split/>").ToList();

                List<string> lstSubRefLabel_Output = new List<string>();

                int nRowCount = 0;
                int nValidSubRefLabelCount = 0;
                string sContent = string.Empty;

                List<char> lstLowerAlphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray().ToList();
                List<char> lstUpperAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();
                string sListType = string.Empty;

                if (lstSubRefLabel.Count > 0)
                {
                    foreach (string sEachContent in lstSubRefLabel)
                    {
                        nRowCount += 1;

                        if (Regex.IsMatch(sEachContent, string.Format("^<SubRefLabel>(?:{0})</SubRefLabel>$", sCurrentSubRefLabelPtn)))
                        {
                            string sLabelCharOnly = Regex.Replace(sEachContent, "</?SubRefLabel>", "");
                            sLabelCharOnly = Regex.Replace(sLabelCharOnly, @"[\.\,\:;\[\]\{\}\(\)]", "");

                            if (nValidSubRefLabelCount == 0)
                            {
                                if (sLabelCharOnly.Equals("a"))
                                {
                                    sListType = "lstLowerAlphabet";
                                    sContent = sEachContent;
                                    nValidSubRefLabelCount += 1;
                                }
                                else if (sLabelCharOnly.Equals("A"))
                                {
                                    sListType = "lstUpperAlphabet";
                                    sContent = sEachContent;
                                    nValidSubRefLabelCount += 1;
                                }
                            }
                            else if (nValidSubRefLabelCount >= 1)
                            {
                                if (sListType == "lstLowerAlphabet")
                                {
                                    if (nValidSubRefLabelCount <= 25)
                                    {
                                        if (sLabelCharOnly.Equals(lstLowerAlphabet[nValidSubRefLabelCount].ToString()))
                                        {
                                            sContent = sEachContent;
                                            nValidSubRefLabelCount += 1;
                                        }
                                    }
                                }
                                else if (sListType == "lstUpperAlphabet")
                                {
                                    if (nValidSubRefLabelCount <= 25)
                                    {
                                        if (sLabelCharOnly.Equals(lstLowerAlphabet[nValidSubRefLabelCount + 1].ToString()))
                                        {
                                            sContent = sEachContent;
                                            nValidSubRefLabelCount += 1;
                                        }
                                    }
                                }
                                else
                                {
                                    sContent = Regex.Replace(sEachContent, "</?SubRefLabel>", "");
                                }
                            }
                        }
                        else
                        {
                            sContent = Regex.Replace(sEachContent, "</?SubRefLabel>", "");
                        }

                        lstSubRefLabel_Output.Add(sContent);
                    }

                    if (nValidSubRefLabelCount > 0)
                    {
                        Console.WriteLine(string.Join("", lstSubRefLabel_Output.ToArray()));
                        Console.ReadLine();
                        return string.Join("", lstSubRefLabel_Output.ToArray());
                    }
                    else
                    {
                        return sRefContent;
                    }
                }
                else
                {
                    return sRefContent;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifySubRefs", ex.Message, true, "");
            }

            return sOutputContent;
        }

        private static bool IdentifySubRefLabel(ref string sRefContent)
        {
            string sRefLabelPattern = @"(?:(?<=(?:</RefLabel>|[\;\.]))(?:(?<SubStyle1>(?: [a-z]\)\.? )|(?<SubStyle2>(?: \([a-z]\)\.? )))))";

            try
            {
                sRefContent = Regex.Replace(sRefContent, sRefLabelPattern, "<SubRefLabel>$&</SubRefLabel>");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifySubRefLabel", ex.Message, true, "");
            }

            return true;
        }

        //added by Dakshinamoorthy on 2020-Apr-25
        private string EscapeNonEnglishChar(string sInputContent)
        {
            try
            {
                sInputContent = Regex.Replace(sInputContent, string.Format("[{0}]", sCapsNonEnglishChar), "A");
                sInputContent = Regex.Replace(sInputContent, string.Format("[{0}]", sSmallNonEnglishChar), "a");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\EscapeNonEnglishChar", ex.Message, true, "");
            }
            return sInputContent;
        }


        public string ImproveLocalConversion(string sRefOriginalContent, string sRefTaggedContent, ref Dictionary<string, string> dicLogInfo, bool bIsStructureOnlyAutor = false)
        {
            try
            {
                sRefOriginalContent = HexUnicodeToCharConvertor(sRefOriginalContent);
                sRefOriginalContent = RefOriginalContentPreCleanup(sRefOriginalContent);
                sRefOriginalContent = IntroSpaceAtValidPosition(sRefOriginalContent);
                //added by Dakshinamoorthy on 2020-Nov-30
                sRefOriginalContent = ChangeCaseAtValidPosition(sRefOriginalContent);
                //added by Dakshinamoorthy on 2020-Dec-08
                sRefOriginalContent = RemoveSpaceAtValidPosition(sRefOriginalContent);

                sRefTaggedContent = sRefOriginalContent;

                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:~~space~~)", " ");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:~~dot~~)", ".");

                //added by Dakshinamoorthy on 2020-Apr-13
                //remove query tag
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:</?LQ_[0-9A-Z]{6}>)", "");
                //added by Dakshinamoorthy on 2020-Apr-23
                //remove InlineShapes tag
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:<LIS_[0-9A-Z]{6}/>)", "");
                //remove OMaths tag
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:<LOM_[0-9A-Z]{6}/>)", "");

                //added by Dakshinamoorthy on 2020-Dec-29
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:<(authorQueryOpen|authorQuerySelf|authorQueryClose|procIntruc)>((?:(?!</?\1).)+)</\1>)", "");

                //added by Dakshinamoorthy on 2020-Apr-25
                //Escape Non-English Character
                sRefTaggedContent = EscapeNonEnglishChar(sRefTaggedContent);

                //added by Daksinamoorthy on 2020-Jul-24
                lstInlinePublisherNames = new List<string>();

                //added by Dakshinamoorthy on 2018-OCT-25
                sRefTaggedContent = RemoveUnwantedItalicWithTitle(sRefTaggedContent);
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<split/>", " ");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<r_space/>", "");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:</?b>)", "");
                //added by Dakshinamoorthy on 2019-Sep-11
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:</?u>)", "");

                //updated by Dakshinamoorthy on 2019-Oct-03
                //sRefTaggedContent = Regex.Replace(sRefTaggedContent, "[\u2026]", "and");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "((?<=[ ])[\u2026](?=[ ]))", "and");
                //added by Dakshinamoorthy on 2020-Jul-28
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:[\u002D\u00AD\u2010]{3})", "–");


                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</?sc>", "");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<i>\.</i>", ".");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"([^\.>]$)", "$1.");

                #region OpenNLP
                //----- StanfordNLP Start Here
                //sRefTaggedContent = SetFirstAuEdCollabBoundary(sRefTaggedContent);
                //System.Windows.Forms.MessageBox.Show(sRefTaggedContent);
                //----- StanfordNLP End End

                //----- OpenNLP Start Here
                //AutoStructRefNLP objNLP = new AutoStructRefNLP();
                //List<string> lstSplittedSentence = new List<string>();
                //string sErrMsg = string.Empty;
                //string sModelPath = General.GeneralInstance.drive + @"\eBOT\Modules\OpenNLP\Models\";
                //string sRefContent4NLP = string.Empty;


                //IdentifyRefLabel(ref sRefTaggedContent);
                //sRefTaggedContent = EscapeDot4SentenceSplit(sRefTaggedContent);

                ////comment start - 2019-Oct-25

                //if (objNLP.OpenNLP_SentenceSplit(sRefTaggedContent, ref lstSplittedSentence, sModelPath, ref sErrMsg))
                //{
                //    sRefContent4NLP = String.Join("<sbr/>", lstSplittedSentence.ToArray());
                //    sRefContent4NLP = Regex.Replace(sRefContent4NLP, "[ ]*<sbr/>[ ]*", "<sbr/>");
                //    sRefContent4NLP = Regex.Replace(sRefContent4NLP, "(?:[\u201C](?:(?![\u201C\u201D]).)+[\u201D])", ReplaceSbrWithSpace);

                //    //added by Dakshinamoorthy on 2019-Oct-25
                //    sRefContent4NLP = Regex.Replace(sRefContent4NLP, "(,) ([\u2018])", "$1<sbr/>$2");

                //    //sRefContent4NLP = Regex.Replace(sRefContent4NLP, @"\. ", ".<sbr/>");
                //    sRefContent4NLP = Regex.Replace(sRefContent4NLP, @"(?<=[ ])<i>(?:(?!</?i>).)+</i>[\:.;,]*(?=[ ])", SplitSentenceByItalic);
                //    sRefContent4NLP = Regex.Replace(sRefContent4NLP, @"((?:<sbr/>){2,})", "<sbr/>");
                //    sRefContent4NLP = Regex.Replace(sRefContent4NLP, @"(?:(?<=(?:~~dot~~|\.),?)(?:[ ]<i>(?:(?!</i>).){5,}(?:\.</i>|</i>,)(?: |<sbr/>)))", "<sbr/>$&<sbr/>");


                //    sRefContent4NLP = ValidateSentenceEnd(sRefContent4NLP);

                //    sRefTaggedContent = Regex.Replace(sRefTaggedContent, "~~dot~~", ".");
                //    sRefContent4NLP = Regex.Replace(sRefContent4NLP, "~~dot~~", ".");

                //    sRefContent4NLP = NormalizePunctuation4SbrTag(sRefContent4NLP);
                //    sRefContent4NLP = Regex.Replace(sRefContent4NLP, "(?:</?(?:b|i|u|sc|sub|sup)>)", "");

                //    sRefTaggedContent = MatchNLP_SentenceResult(sRefTaggedContent, sRefContent4NLP);


                //    if (Regex.IsMatch(sRefTaggedContent, "^(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)"))
                //    {
                //        //sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?<=(?:^(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)))(?:(?:(?!(?:<sbr/>)).)+)", IdentifyFirstAuEdCollabBoundary);
                //        //sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?<=(?:^(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)))(?:(?:(?!(?:<sbr/>)).)+)", IdentifyFirstAuEdCollabGroup);
                //        //updated by Dakshinamoorthy on 2019-Jan-05

                //        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?<=(?:^(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)))(?:(?:(?!(?:<sbr/>|<i>)).)+)", IdentifyFirstAuEdCollabBoundary);
                //        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?<=(?:^(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)))(?:(?:(?!(?:<sbr/>|<i>)).)+)", IdentifyFirstAuEdCollabGroup);
                //    }
                //    else
                //    {
                //        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "^(?:(?:(?!<sbr/>).)+)", IdentifyFirstAuEdCollabBoundary);
                //        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "^(?:(?:(?!<sbr/>).)+)", IdentifyFirstAuEdCollabGroup);
                //    }

                //    sRefTaggedContent = SetFirstAuEdCollabBoundary_PostCleanup(sRefTaggedContent);
                //    sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<sbr/>", " ");
                //}

                ////comment end - 2019-Oct-25

                //sRefTaggedContent = Regex.Replace(sRefTaggedContent, "~~dot~~", ".");

                //----- OpenNLPNLP End End 
                #endregion

                //RefTaggedContentPreCleanup(ref sRefTaggedContent);
                sRefTaggedContent = DoReferencePatternMatch(sRefTaggedContent);

                //updated by Dakshinamoorthy on 2020-May-23
                #region MyRegion
                if (bIsStructureOnlyAutor == false)
                {
                    sRefTaggedContent = IdentifyVolIssuePageRange(sRefTaggedContent);

                    //<Other><ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Minsky</Surname> <Forename>H.P.</Forename></Author></ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> <i>“Can “IT” Happen Again? Essays on Instability and Finance,”</i> Armonk, NY: M.E. Sharpe & Co. <PubDate><Year>1982</Year>.</PubDate></Other>
                    sRefTaggedContent = HandleQuotesWithInQuotes(sRefTaggedContent);

                    sRefTaggedContent = IdentifyConferenceName(sRefTaggedContent);

                    sRefTaggedContent = IdentifyReportNumber(sRefTaggedContent);

                    if (IsConfProcTypeRef(sRefTaggedContent) == false && IsVolOrIssueDoiFound(sRefTaggedContent) == true)
                    {
                        sRefTaggedContent = IdentifyJournalArticleTitle(sRefTaggedContent);
                    }

                    NormalizeSpaces(ref sRefTaggedContent);

                    //added by Dakshinamoorthy on 2020-Dec-07
                    #region Exclude Title Information from AUG
                    if (IsTitlesFound(sRefTaggedContent) == false)
                    {
                        string sMayBeTitleContnetTagged = GetLastAuthorInFirstAUG(sRefTaggedContent);
                        string sMayBeTitleContnet = Regex.Replace(sMayBeTitleContnetTagged, "<[^<>]+>", "").Trim();

                        if (string.IsNullOrEmpty(sMayBeTitleContnet) == false)
                        {
                            string sTitleTagged = Regex.Replace(sMayBeTitleContnet, "^(.+)$", HandleJournalArticleTitle);
                            if (Regex.IsMatch(sTitleTagged.Trim(), "^(?:<Journal_Title>(?:(?!</?Journal_Title>).)+</Journal_Title>)$"))
                            {
                                sRefTaggedContent = sRefTaggedContent.Replace(sMayBeTitleContnetTagged, string.Format(" <{0}>{1}</{0}> ", "MayBeJourTitle", sMayBeTitleContnetTagged));
                                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:<MayBeJourTitle>(?:(?!</?MayBeJourTitle>).)+</MayBeJourTitle>)", RemoveAuInfoTagAsJourTitle);
                                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:(<MayBeJourTitle>(?:(?!</?MayBeJourTitle>).)+</MayBeJourTitle>)[ ]*</AuEdGroup>)", "</AuEdGroup> $1 ");

                                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<MayBeJourTitle>", "<Journal_Title>");
                                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</MayBeJourTitle>", "</Journal_Title>");

                                NormalizeSpaces(ref sRefTaggedContent);
                            }
                        }
                    }
                    #endregion

                    IdentifyPublisherLocation(ref sRefTaggedContent);
                    AutoStructReferencesPatterns.DoFullPatternMatching(ref sRefTaggedContent);

                    if (IsConfProcTypeRef(sRefTaggedContent) == false && IsVolOrIssueDoiFound(sRefTaggedContent) == false)
                    {
                        sRefTaggedContent = IdentifyBookChapterTitle(sRefTaggedContent);
                    }
                    if (!Regex.IsMatch(sRefTaggedContent, @"</?(?:Article_Title|Journal_Title|ldlBookTitle|ldlChapterTitle|ldlArticleTitle|ldlJournalTitle|ldlConferenceName)>"))
                    {
                        sRefTaggedContent = IdentifyJournalArticleTitle(sRefTaggedContent);
                    }
                    AutoStructReferencesPatterns.DoFullPatternMatching_Post(ref sRefTaggedContent);
                }
                #endregion

                //Misc
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:~~(?:out|in)_ldquo~~)", "“");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:~~(?:out|in)_rdquo~~)", "”");

                //sRefTaggedContent = IdentifyEtAl(sRefTaggedContent);
                sRefTaggedContent = ConvertGenericRef(sRefOriginalContent, sRefTaggedContent, ref dicLogInfo);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ImproveLocalConversion", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        private string RemoveAuInfoTagAsJourTitle(Match myJourTitle)
        {
            string sOutputContent = myJourTitle.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, @"<MayBeJourTitle>", "~~MayBeJourTitle~~");
                sOutputContent = Regex.Replace(sOutputContent, @"</MayBeJourTitle>", "~~/MayBeJourTitle~~");
                sOutputContent = Regex.Replace(sOutputContent, "<[^<>]+>", "");
                sOutputContent = Regex.Replace(sOutputContent, @"~~MayBeJourTitle~~", @"<MayBeJourTitle>");
                sOutputContent = Regex.Replace(sOutputContent, @"~~/MayBeJourTitle~~", @"</MayBeJourTitle>");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveAuInfoTagAsJourTitle", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private string GetLastAuthorInFirstAUG(string sRefTaggedContent)
        {
            string sOutputContent = string.Empty;
            try
            {
                string sPatternFirstAUG = string.Format(@"(?:(?<=(?:<(?:{0})>))(?:<AuEdGroup>((?:(?!</?AuEdGroup>).)+)</AuEdGroup>))", sRefTypeLocalExePtn);

                if (Regex.IsMatch(sRefTaggedContent, sPatternFirstAUG))
                {
                    string sFirstAuGroup = Regex.Match(sRefTaggedContent, sPatternFirstAUG).Value;
                    string sLastAuthorPattern = @"(?:<Author>((?:(?!</?Author>).)+)</Author>)(?=(?:</AuEdGroup>))";

                    if (Regex.IsMatch(sFirstAuGroup, sLastAuthorPattern))
                    {
                        string sLastAuthorTagged = Regex.Match(sFirstAuGroup, sLastAuthorPattern).Value;
                        string sLastAuthorContent = Regex.Replace(sLastAuthorTagged, "<[^<>]+>", "").Trim();

                        if (Regex.IsMatch(sLastAuthorContent, @"(?:(?:[A-Z][a-z]+ )+(?:[A-Z][a-z]+\.))"))
                        {
                            return sLastAuthorTagged;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\GetLastAuthorInFirstAUG", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private bool IsTitlesFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"(?:<((?:Article_Title|Journal_Title))>(?:(?!</?\1>).)+</\1>)"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsTitlesFound", ex.Message, true, "");
            }
            return false;
        }


        private string GetFirstSubRefType(string sRefTaggedContent)
        {
            string sOutputType = "ldlRefTypeOther";

            try
            {
                string sSubRefTypePattern = @"<((?:ldlRefTypeJournal|ldlRefTypeBook|ldlRefTypeWeb|ldlRefTypePaper|ldlRefTypeConference|ldlRefTypePatent|ldlRefTypeReport|ldlRefTypeThesis|ldlRefTypeOther))>(?:(?!</?\1>).)+</\1>";

                if (Regex.IsMatch(sRefTaggedContent, sSubRefTypePattern))
                {
                    Match myMatch = Regex.Matches(sRefTaggedContent, sSubRefTypePattern)[0];
                    sOutputType = myMatch.Groups[1].Value.ToString();
                }
                else
                {
                    return sOutputType;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\GetFirstSubRefType", ex.Message, true, "");
            }

            return sOutputType;
        }

        private string RemoveSubRefTypeElements(string sRefTaggedContent)
        {
            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"</?((?:ldlRefTypeJournal|ldlRefTypeBook|ldlRefTypeWeb|ldlRefTypePaper|ldlRefTypeConference|ldlRefTypePatent|ldlRefTypeReport|ldlRefTypeThesis|ldlRefTypeOther))>", "");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</?ldlRefLabel>", "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveSubRefTypeElements", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        private string HandleQuotesWithInQuotes(string sRefTaggedContent)
        {
            try
            {
                do
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, "([\u201C][^\u201C\u201D]+[\u201D])", Deli_HandleQuotesWithInQuotes);
                } while (Regex.IsMatch(sRefTaggedContent, "([\u201C][^\u201C\u201D]+[\u201D])"));

                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "~~out_ldquo~~", "“");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "~~out_rdquo~~", "”");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleQuotesWithInQuotes", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        private string Deli_HandleQuotesWithInQuotes(Match myMatch)
        {
            string sOutputContent = myMatch.Value.ToString();
            try
            {
                if (!Regex.IsMatch(sOutputContent, "(?:~~(?:in|out)_ldquo~~|~~(?:in|out)_rdquo~~)"))
                {
                    sOutputContent = Regex.Replace(sOutputContent, "[\u201C]", "~~out_ldquo~~");
                    sOutputContent = Regex.Replace(sOutputContent, "[\u201D]", "~~out_rdquo~~");
                }
                else
                {
                    sOutputContent = Regex.Replace(sOutputContent, "~~out_ldquo~~", "~~in_ldquo~~");
                    sOutputContent = Regex.Replace(sOutputContent, "~~out_rdquo~~", "~~in_rdquo~~");

                    sOutputContent = Regex.Replace(sOutputContent, "[\u201C]", "~~out_ldquo~~");
                    sOutputContent = Regex.Replace(sOutputContent, "[\u201D]", "~~out_rdquo~~");
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\Deli_HandleQuotesWithInQuotes", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private string SplitSentenceByItalic(Match myMatch)
        {
            string sOutputContent = myMatch.Value.ToString();
            try
            {
                if (Regex.IsMatch(sOutputContent, "^(?:<i>(?:(?!(?:</i>|\b[a-z]+\b)).)+</i>)$"))
                {
                    sOutputContent = string.Format("<sbr/>{0}<sbr/>", sOutputContent);
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\SplitSentenceByItalic", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private string ReplaceSbrWithSpace(Match myMatch)
        {
            string sOutputContent = myMatch.Value.ToString();

            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "<sbr/>", " ");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ReplaceSbrWithSpace", ex.Message, true, "");

            }
            return sOutputContent;
        }

        private string EscapeDot4SentenceSplit(string sRefTaggedContent)
        {
            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"((?:\b(?:[A-Z]\.[ ]?){1,})|(?:(?:\-[A-Z]\.[ ]?){1,}))", HandleEscapeDot4SentenceSplit);
                //sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"([A-Z][a-z]+) ([a-z]+\.) ", HandleEscapeDot4SentenceSplit);

                //(?<=[.!?]|[.!?][\\'\"])(?<!e\.g\.|i\.e\.|vs\.|p\.m\.|a\.m\.|Mr\.|Mrs\.|Ms\.|St\.|Fig\.|fig\.|Jr\.|Dr\.|Prof\.|Sr\.|[A-Z]\.)\s+
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(e\.g\.|i\.e\.|vs\.|p\.m\.|a\.m\.|Mr\.|Mrs\.|Ms\.|St\.|Fig\.|fig\.|Jr\.|Dr\.|Prof\.|Sr\.)\s+", HandleEscapeDot4SentenceSplit);

                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)", HandleEscapeDot4SentenceSplit);

                //escape hellips
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"([\.]{3,})", "~~dot~~~~dot~~~~dot~~");

            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\EscapeDot4SentenceSplit", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }


        private string HandleEscapeDot4SentenceSplit(Match myMatch)
        {
            string sOutputContent = myMatch.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, @"[\.]", "~~dot~~");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesPatterns.cs\EscapeDot4SentenceSplit", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private bool IsConfProcTypeRef(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, "(?:<ldlConferenceName>(?:(?!</?ldlConferenceName>).)+</ldlConferenceName>)"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesPatterns.cs\IsConfProcTypeRef", ex.Message, true, "");
            }
            return false;
        }

        private bool IsVolOrIssueDoiFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, "(?:(?:<Vol_No>(?:(?!</?Vol_No>).)+</Vol_No>)|(?:<Doi>(?:(?!</?Doi>).)+</Doi>)|(?:<Issue_No>(?:(?!</?Issue_No>).)+</Issue_No>))"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsVolOrIssueDoiFound", ex.Message, true, "");
            }
            return false;
        }



        private string RefOriginalContentPreCleanup(string sRefOriginalContent)
        {
            try
            {
                sRefOriginalContent = AutoStructRefCommonFunctions.NormalizeTagSpaces(sRefOriginalContent);

                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"[\uFF0C]", ", ");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"<(i|b|u|su[bp])></\1>", "");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"</([A-Za-z]+)><\1>", "");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, "[\u00A0]", " ");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, ", ([\u201D]) ", ",$1 ");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, "</i><i>.</i> <i>", ". ");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, ", <i> </i>", ", ");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, "</i> <i>", " ");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, "</Doi>.PMID", "</Doi>. PMID");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, "(?:<i>([\u201c])</i>(?! ?<i>))", "$1");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"<i>(\.?[\u201d]) ", "$1 <i>");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @" ,", ", ");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"((?<=[a-z])<i>\.</i>(?=[ ]))", ".");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"(?<!</i>)<i>,</i> ", ", ");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"<b>\(</b>([^\(\)]+)\)", "($1)");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"[ ]([0-9]+)<b>,</b>[ ]", " $1, ");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"[\uFE60]", " & ");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"(?<!</b>)(?:<b>\.</b>)", ".");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"<i>\. ", ". <i>");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"(\.</i>)(<b>(?:(?!</?b>).)+</b>)", "$1 $2");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"([\u2018]{2,})", "“");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"([\u2019]{2,})", "”");

                //added by Dakshinamoorthy on 2020-May-29 (to handle inline referernces in footnote area)
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"^<R>", "");
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"</R>$", "");

                //added by Dakshinamoorthy on 2020-Dec-17 (for handling BluPencil author query)
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"(?:<comment(?: [^<>]+)?[^\/]>)", string.Format("<{0}>{1}</{0}>", "authorQueryOpen", "$&"));
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"(?:<comment(?: [^<>]+)?[\/]>)", string.Format("<{0}>{1}</{0}>", "authorQuerySelf", "$&"));
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"(?:</comment>)", string.Format("<{0}>{1}</{0}>", "authorQueryClose", "$&"));
                //added by Dakshinamoorthy on 2020-Dec-29 (for handling BluPencil "3B2 processing instruction")
                //sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"(?:(?:<\!\-\-\{cke_protected\}\{[^\{\}]+\})(?:\<\!\-\-\?)(?:(?:(?!(?:(?:\<\!\-\-\?)|(?:\?\-\-\>))).)+)(?:\?\-\->)(?:\-\->))", string.Format("<{0}>{1}</{0}>", "procIntruc", "$&"));
                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"(?:(?:\<\!\-\-\{cke_protected\})(?:(?:(?!(?:\<\!\-\-\{cke_protected\})).)+)(?:\-\-\>))", string.Format("<{0}>{1}</{0}>", "procIntruc", "$&"));

                sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"(?:<(authorQueryOpen|authorQuerySelf|authorQueryClose|procIntruc)>((?:(?!</?\1).)+)</\1>)", HandleBP_Comment);

                //italic merged with title
                //sRefOriginalContent = Regex.Replace(sRefOriginalContent, @"((?<=<i>(?:(?!</?i>).)+, )[0-9]{1,3})</i>(, [0-9]+\.)", "</i>$1$2");

                sRefOriginalContent = AutoStructRefCommonFunctions.NormalizeTagSpaces(sRefOriginalContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RefOriginalContentPreCleanup", ex.Message, true, "");
            }
            return sRefOriginalContent;
        }

        private string HandleBP_Comment(Match matchComment)
        {
            string sReturnValue = string.Empty;
            string sTagName = matchComment.Groups[1].Value.ToString();
            string sContent = matchComment.Groups[2].Value.ToString();

            try
            {
                sContent = EncodeTo64(sContent);
                sReturnValue = string.Format("<{0}>{1}</{0}>", sTagName, sContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleBP_Comment", ex.Message, true, "");
            }
            return sReturnValue;
        }

        public string EncodeTo64(string toEncode)
        {
            string returnValue = string.Empty;
            try
            {
                byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
                returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\EncodeTo64", ex.Message, true, "");
            }
            return returnValue;
        }


        public string DecodeFrom64(string encodedData)
        {
            string returnValue = string.Empty;
            try
            {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
                returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\DecodeFrom64", ex.Message, true, "");
            }
            return returnValue;
        }


        private bool IdentifyDOI(ref string sRefTaggedContent)
        {
            try
            {
                //updated by Dakshinamoorthy on 2019-Sep-11
                //string sDOIPattern = @"(?:\b(?:([Dd][Oo][Ii][\: ]*)|(?:(?:https?:\/+)?(?:www\.)?doi\.org\/)?)(?:10.[0-9]{4,9}\/[-._;\(\)\/:A-Za-z0-9&;]+\b)(?:[\.,]*))";
                //string sDOIPattern = @"(?:\b(?:([Dd][Oo][Ii][\: ]*)|(?:(?:https?:\/+)?(?:www\.)?doi\.org\/)?)(?:10.[0-9]{4,9}[\/\.][-._;\(\)\/:A-Za-z0-9&;]+\b)(?:[\.,]*))";
                //updated by Dakshinamoorthy on 2020-Jan-13
                //string sDOIPattern = @"(?:(?:\b(?:([Dd][Oo][Ii][\: ]*)|(?:(?:https?:\/+)?(?:www\.)?(?:dx\.)?doi\.org\/)?)(?:10.[0-9]{4,9}[\/\.][-._;\(\)\/:A-Za-z0-9&;]+\b)(?:[\.,]*))|(?:(?:(?:https?:\/+)?(?:www\.)?doi\.org\/)(?:[-._;\(\)\/:A-Za-z0-9&;]+)))";
                string sDOIPattern = @"(?:(?:(?:[Dd][Oo][Ii][\: ]*))?(?:\b(?:(?:[Dd][Oo][Ii][\: ]*)|(?:(?:https?:\/+)?(?:www\.)?(?:dx\.)?doi\.org\/)?)(?:10.[0-9]{4,9}[\/\.][-._;\(\)\/:A-Za-z0-9&;]+\b)(?:[\.,]*))|(?:(?:(?:https?:\/+)?(?:www\.)?doi\.org\/)(?:[-._;\(\)\/:A-Za-z0-9&;]+)))";


                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</?doi>", "", RegexOptions.IgnoreCase);
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<Website>((?:https?:\/\/(?:www\.)?(?:dx\.)?doi.org/(?:(?!</Website>).)+))</Website>", "$1", RegexOptions.IgnoreCase);
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, sDOIPattern, string.Format("<Doi>{0}</Doi>", "$&"));
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyDOI", ex.Message, true, "");
            }
            return true;
        }

        private bool IdentifyURL(ref string sRefTaggedContent)
        {
            try
            {
                //string sURLPattern = @"(?:(?:[<\(])?(?:\b(?:https?\://[^ ]+)|(?:www[0-9]*\.[^ ]+))(?:[>\)])?)";
                //updated by Dakshinamoorthy on 2019-Sep-24
                string sURLPattern = @"(?:(?<=(?:[ ]|^))(?:[<\(\u2329])?(?:(?:\b(?:https?\://[^ ]+)|(?:www[0-9]*\.[^ ]+)|(?:\b(?:[0-9A-Za-z_-]{5,}\.)+(?:aero|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cu|cv|cx|cy|cz|cz|de|dj|dk|dm|do|dz|ec|ee|eg|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mn|mn|mo|mp|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|nom|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ra|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sj|sk|sl|sm|sn|so|sr|st|su|sv|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw|arpa)\b(?:\:[0-9]+)?(?:(?:\/(?:[~0-9a-zA-Z\#\+\%@\.\/_-]+))?(?:\?[0-9a-zA-Z\+\%@\/&amp;\[\];=_-]+)?)?[\.]?))(?:[>\)\u232A])?)(?=(?:[ ]|$)))";

                string sURLLabelPattern = @"(?<=(?:(?:[\:.,;\>\)\]\u201d\u2019] )|(?:[\(])|[0-9]+ ))(?:(?:Retrieved from|Retieved from|from|Online|Retrieved|Retrieved at|Available|Available at)[\:]? )(?=<Website>)";

                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</?Website>", "", RegexOptions.IgnoreCase);
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, sURLPattern, string.Format("<Website>{0}</Website>", "$&"));

                //Identify URL Label
                if (Regex.IsMatch(sRefTaggedContent, "</?Website>"))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sURLLabelPattern, string.Format("<ldlURLLabel>{0}</ldlURLLabel>", "$&"));
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyURL", ex.Message, true, "");
            }
            return true;
        }


        private string IdentifyVolIssuePageRange(string sRefTaggedContent)
        {
            string sPartialRefPattern = string.Empty;
            string sRefTaggedContent_Backup = string.Empty;
            int nPatternId = 0;

            EscapeContentFromPublisherLocation(ref sRefTaggedContent);


            sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"\(<PubDate>", "<PubDate>(");


            string sYearTagPattern = @"(?:(?:<Year>(?:(?!</?Year>).)+</Year>[ ]?)|(?:<PubDate>(?:(?!</?PubDate>).)+</PubDate>[ ]?))";

            //string sVolNumPattern = @"\b(?:(?:<i>)?(?:[0-9]+)(?:[ ]?(?:[\-\u2013]|&#8211;|&#x2013;)[ ]?[0-9]+)?[,]?(?:<\/i>)?(?:[,\:]? |[\:]|(?=\()))";
            string sVolNumPattern = @"(?:(?:<[bi]>)?(?:\b(?:[V]ol(?:ume)?|[Vv](?:ol)?)[\:\. ]+)?(?:<[bi]>)?\b(?:[A-Z]?[0-9]+|[0-9]+[A-Z])(?:[ ]?(?:[\-\u2013\u002D]|&#8211;|&#x2013;)[ ]?[0-9]+)?[,\:]?(?:<\/[bi]>)?(?:[,\:]? |[\:]|(?=\()))";

            //updated by Dakshinamoorthy on 2019-Sep-28
            //string sVolNumWithLabelPattern = @"(?:\b(?:(?:[Vv]ol(?:ume)?|[Vv](?:ol)?)[\:\. ]+)([0-9]+)\b[\:\.;, ]*)";
            string sVolNumWithLabelPattern = @"(?:[\(]?(?:\b(?:(?:[Vv]ol(?:ume)?|[Vv](?:ol)?)[\:\. ]+)(?:(?:[0-9]+)|(?:[A-Z0-9\-]+))\b[\:\.;, ]*)|(?<=, )(?:[0-9]+ [Vv]ols?[\.,]+ )|(?:\([0-9]+ (?:[Vv]ol(?:ume)?|[Vv](?:ol)?\.?)\)[\:\.;, ]*))";


            //string sIssNumPattern = string.Format(@"(?:(?:(?:<i>)?(?:[\(][^\(\)]+\))[,\.]?(?:<\/i>)?[,]?(?: |\: ?|(?=</{0}>|$)))|(?:(?:[0-9]+) (?:[Ss]uppl(?:ement)?)[\.,\:; ]+))", sRefTypeLocalExePtn);
            string sIssNumPattern = string.Format(@"(?:(?:(?:<i>)?(?:[\(][^\(\),]+\))[,\.]?(?:<\/i>)?[,]?(?: |[\:,] ?|(?=</{0}>|$)))|(?:(?:[0-9]+) (?:[Ss]uppl(?:ement)?)[\.,\:; ]+)|\b(?:(?:[Ii]ss(?:ue)?)|\b(?:[Ss]pecial [Ii]ssue[\.,]+ )|[Nn]um(?:ber)?|[Nn]o[s]?)[\:\. ]+(?:[A-Z]?[0-9]+(?:[ ]?[\u2013\-][ ]?[A-Z]?[0-9]+)?)[\:\.;, ]+|[0-9]+\:[ ]?|(?:Spring|Winter|Summer|Autumn|Fall)[\:.;, ]+|(?:[0-9]+(?:[ ]?[\u2013\-][ ]?[0-9]+)?)[\.,]* (?=<PubDate>))", sRefTypeLocalExePtn);
            string sIssNumWithLabelPatternPattern = @"(?:(?:(?:[Ii]ss(?:ue)?)|[Nn]um(?:ber)?|[Nn]r|[Nn]o[s]?)[\:\. ]+[0-9]+[\:\.;, ]+)";

            //string sPageRangePattern = string.Format(@"(?:(?:[A-Za-z]?[0-9]+[A-Za-z]?)(?:[ ]?(?:[\-\u2013]|&#8211;|&#x2013;)[ ]?[A-Za-z]?[0-9]+[A-Za-z]?)?\.?(?:(?:$|(?=</{0}>))| (?=(?:(?:[Dd][Oo][Ii][\: ]*)|(?:P[Mm][Ii][Dd])|(?:<(?:PubMedIdNumber|PubMedIdLabel)>)|(?:<Doi>)>))))", sRefTypeLocalExePtn);
            //string sPageRangePattern = string.Format(@"(?:(?:[e]?[A-Za-z]?[0-9]+[A-Za-z]?)(?:[ ]?(?:[\-\u2013]|&#8211;|&#x2013;)[ ]?[e]?[A-Za-z]?[0-9]+[A-Za-z]?)?\.?)(?:[ ]|$|(?=</{0}>)|(?= (?:(?:[Dd][Oo][Ii][\: ]*)|<Doi>|(?:P[Mm][Ii][Dd])|(?:<(?:PubMedIdNumber|PubMedIdLabel)>))))", sRefTypeLocalExePtn);
            //updated by Dakshinamoorthy on 2019-Sep-11
            //string sPageRangePattern = string.Format(@"(?:\b(?:[Pp]p?|[Pp]ages?)[\:\. ]+)?(?:(?:[e]?[A-Za-z]?[0-9]+[A-Za-z]?)(?:[ ]?(?:[\-\u2013\u002D]|&#8211;|&#x2013;)[ ]?[e]?[A-Za-z]?[0-9]+[A-Za-z]?)?[\.,;]*)(?: (?:[Pp]p?|[Pp]ages?)[\.,]*)?(?:\)|[ ]|$|(?=</{0}>)|(?= (?:(?:[Dd][Oo][Ii][\: ]*)|<Doi>|(?:P[Mm][Ii][Dd])|(?:<(?:PubMedIdNumber|PubMedIdLabel)>))))", sRefTypeLocalExePtn);
            //updated by Dakshinamoorthy on 2019-Dec-10
            //string sPageRangePattern = string.Format(@"(?:\b(?:[Pp]p?|[Pp]ages?)[\:\. ]+)?(?:(?:(?:[e]?[A-Za-z]?[0-9]+[A-Za-z]?)|[0-9]+ra[0-9]+)(?:[ ]?(?:[\-\u2013\u002D]|&#8211;|&#x2013;)[ ]?(?:[e]?[A-Za-z]?[0-9]+[A-Za-z]?|[0-9]+ra[0-9]+))?[\.,;]*)(?: (?:[Pp]p?|[Pp]ages?)[\.,]*)?(?:\)|[ ]|$|(?=</{0}>)|(?= (?:(?:[Dd][Oo][Ii][\: ]*)|<Doi>|(?:P[Mm][Ii][Dd])|(?:<(?:PubMedIdNumber|PubMedIdLabel)>))))", sRefTypeLocalExePtn);
            string sPageRangePattern = string.Format(@"(?:\b(?:[Pp]p?|(?:[Pp]\.[ ]?[Pp]\.[ ]?)|[Pp]ages?)[\:\. ]+)?(?:(?:(?:[e]?[A-Za-z]?[0-9]+[A-Za-z]?)|[0-9]+ra[0-9]+|[0-9]+\:[0-9]+)(?:[ ]?(?:[\-\u2013\u002D\u2012]|&#8211;|&#x2013;)[ ]?(?:[e]?[A-Za-z]?[0-9]+[A-Za-z]?|[0-9]+ra[0-9]+|[0-9]+\:[0-9]+))?[\.,;]*)(?: (?:[Pp]p?|[Pp]ages?)[\.,]*)?(?:\)|[ ]|$|(?=</{0}>)|(?= (?:(?:[Dd][Oo][Ii][\: ]*)|<Doi>|(?:P[Mm][Ii][Dd])|(?:<(?:PubMedIdNumber|PubMedIdLabel)>))))", sRefTypeLocalExePtn);


            //updated by Dakshinamoorthy on 2019-Jan-12
            //string sPageRangeWithLabelPattern = @"(?:(?:(?:\b(?:[Pp]p?|[Pp]ages?)[\:\. ]*)(?:(?:[e]?[A-Za-z]?[0-9]+[A-Za-z]?)(?:[ ]?(?:[\-\u2013\u002D]|&#8211;|&#x2013;)[ ]?[e]?[A-Za-z]?[0-9]+[A-Za-z]?)?\b[\.,; ]*))|(?:\b[0-9]+ ?(?:[Pp][Pp]?|[Pp]ages)[\.,;]+ ?))";
            //updated by Dakshinamoorthy on 2019-Sep-11
            //string sPageRangeWithLabelPattern = @"(?:(?:(?:\b(?:[Pp]p?|[Pp]ages?)[\:\. ]*)(?:(?:(?:[e]?[A-Za-z]?[0-9]+[A-Za-z]?)|(?: [A-Z]{1,3}))(?:[ ]?(?:[\-\u2013\u002D]|&#8211;|&#x2013;)[ ]?[e]?[A-Za-z]?[0-9]+[A-Za-z]?)?\b[\.,; ]*))|(?:\b[0-9]+ ?(?:[Pp][Pp]?|[Pp]ages)[\.,;]+ ?))";
            //updated by Dakshinamoorthy on 2019-Oct-25
            string sPageRangeWithLabelPattern = @"(?:(?:[\[\(])?(?:(?:(?:\b(?:[Pp]p?|(?:[Pp]\.[ ]?[Pp]\.[ ]?)|[Pp]ages?)[\:\. ]*)(?:(?:(?:[e]?[A-Za-z]?[0-9]+[A-Za-z]?)|(?: [A-Z]{1,3})|(?:(?:[IVX]|[ivx]){1,4})|(?:[A-Z]{1,2}[0-9]+))(?:[ ]?(?:[\-\u2013\u002D\u2012]|&#8211;|&#x2013;)[ ]?(?:(?:[e]?[A-Za-z]?[0-9]+[A-Za-z]?)|(?:(?:[IVX]|[ivx]){1,4})|(?:[A-Z]{1,2}[0-9]+)))?\b[\.,; ]*))|(?:\b[0-9]+ ?(?:[Pp][Pp]?|[Pp]ages)[\.,;]+ ?))(?:[\]\)][\.,; ]*)?)";


            string sAbstractPattern = string.Format(@"(?:(?:[ ]Abstr(?:act)?)[\.\:]? [0-9]+[\.,;]*(?: |$|(?=</{0}>)))", sRefTypeLocalExePtn);
            string sELocationIdPattern = string.Format(@"(?<=[\: ,]+)(?:(?:[0-9]{{5,}}(?:\.[0-9]+)?))(?:[ ]?[\u2013\-][ ]?(?:[0-9]{{5,}}(?:\.[0-9]+)?))?\b(?:[\.\,]*(?:[ ]|$|(?=</{0}>)))", sRefTypeLocalExePtn);
            string sDoiTaggedPattern = @"(?:(?:<Doi>(?:(?!</?Doi>).)+</Doi>)(?:[\:\.;, ]+|(?=[<])))";

            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<Review>(?:(?!</?Review>).)+</Review>", "");
                sRefTaggedContent_Backup = sRefTaggedContent;

                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</?(?:Vol_No|Issue_No|FirstPage|LastPage)>", "");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<Review>(?:(?!</?Review>).)+</Review>", "");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:(?:(?<=[,])(?:~~space~~|[ ])([0-9]+))</i>(?= (?:\([0-9]+\))))", "</i> <i>$1</i>");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:(?:(?<=[,])(?:~~space~~|[ ])([0-9]+))</i>(?=[,]? (?:[a-z]?[0-9]+)))", "</i> <i>$1</i>");

                string sMonthPtnWithDelim = string.Format(@"(?:\({0}\)[\:.;, ]+)", sMonthPatternWithRange);

                //added by Dakshinamoorthy on 2019-Dec-06
                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})({3})", sVolNumPattern, sIssNumPattern, sMonthPtnWithDelim, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No><MonthMisc>$3</MonthMisc><PageRange>$4</PageRange>");
                    goto LBL_PATTERN_END;
                }


                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})({3})", sVolNumPattern, sIssNumPattern, @"(?:\([0-9]{4}\)[\:.;, ]+)", sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No><PubDate>$3</PubDate><PageRange>$4</PageRange>");
                    goto LBL_PATTERN_END;
                }

                //added by Dakshinamoorthy on 2020-Jun-17
                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})", sVolNumPattern, sIssNumPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No><PageRange>$3</PageRange>");
                    goto LBL_PATTERN_END;
                }

                //with e-location id

                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})({3})", sVolNumPattern, sIssNumPattern, @"(?:<PubDate>(?:(?!</?PubDate>).)+</PubDate>[\:\.;, ]+)", sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No>$3<PageRange>$4</PageRange>");
                    goto LBL_PATTERN_END;
                }

                //added by Dakshinamoorthy on 2020-Jul-11
                //with temp date
                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})({3})", sVolNumPattern, sIssNumPattern, @"(?:<ldlTempDate>(?:(?!</?ldlTempDate>).)+</ldlTempDate>[\:\.;, ]+)", sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No>$3<PageRange>$4</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})", sVolNumWithLabelPattern, sIssNumWithLabelPatternPattern, sPageRangeWithLabelPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No><PageRange>$3</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})", @"(?:[A-Z]{2,}\-[0-9]+ )", sIssNumPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No><PageRange>$3</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})({3})", sVolNumPattern, sIssNumPattern, sELocationIdPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No><eLocation_Id>$3</eLocation_Id><PageRange>$4</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})", sVolNumPattern, sIssNumPattern, sELocationIdPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No><eLocation_Id>$3</eLocation_Id>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})", sDoiTaggedPattern, sELocationIdPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " $1<eLocation_Id>$2</eLocation_Id>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})({3})", sVolNumPattern, sIssNumPattern, sDoiTaggedPattern, sELocationIdPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No>$3<eLocation_Id>$4</eLocation_Id>");
                    goto LBL_PATTERN_END;
                }

                //added by Dakshinamoorthy on 2019-Sep-13
                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})", sVolNumPattern, "(?:[0-9]+, )", sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No><PageRange>$3</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})", sVolNumPattern, sELocationIdPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><eLocation_Id>$2</eLocation_Id>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})({3})", sYearTagPattern, sVolNumPattern, sIssNumPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " $1<Vol_No>$2</Vol_No><Issue_No>$3</Issue_No><PageRange>$4</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})({3})", sYearTagPattern, sIssNumPattern, sVolNumPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " $1<Issue_No>$2</Issue_No><Vol_No>$3</Vol_No><PageRange>$4</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})", sVolNumPattern, sYearTagPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No>$2<PageRange>$3</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 0;
                sPartialRefPattern = string.Format(" ({0})({1})({2})", sYearTagPattern, sVolNumPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " $1<Vol_No>$2</Vol_No><PageRange>$3</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 0;
                sPartialRefPattern = string.Format(@" ({0})({1})({2})", sYearTagPattern, sVolNumPattern, sIssNumPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " $1<Vol_No>$2</Vol_No><Issue_No>$3</Issue_No>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 1;
                sPartialRefPattern = string.Format(" ({0})({1})({2})", sVolNumPattern, sIssNumPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No><PageRange>$3</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 2;
                sPartialRefPattern = string.Format(@" ({0})(\b(?:[Pp]art[\- ][0-9]+, ))({1})", sVolNumPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No><PageRange>$3</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 2;
                sPartialRefPattern = string.Format(" ({0})({1})", sIssNumWithLabelPatternPattern, sPageRangeWithLabelPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Issue_No>$1</Issue_No><PageRange>$2</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 2;
                sPartialRefPattern = string.Format(@"([\( ]{0})({1})", sVolNumPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><PageRange>$2</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 3;
                sPartialRefPattern = string.Format(" ({0})({1})({2})", sVolNumPattern, sIssNumPattern, sAbstractPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No>$3");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 4;
                sPartialRefPattern = string.Format(" ({0})({1})({2})", sVolNumPattern, sIssNumPattern, sYearTagPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No>$3");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 5;
                sPartialRefPattern = string.Format(@"( [0-9]+\.)([0-9]+\: ?)({2})", sVolNumPattern, sIssNumPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No><PageRange>$3</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 6;
                sPartialRefPattern = string.Format(@"( {0})({1})", sVolNumPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><PageRange>$2</PageRange>");
                    goto LBL_PATTERN_END;
                }

                nPatternId = 6;
                sPartialRefPattern = string.Format(@"( {0})({1})", sVolNumPattern, sIssNumPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No><Issue_No>$2</Issue_No>");
                    goto LBL_PATTERN_END;
                }

                //added by Dakshinamoorthy on 2019-Sep-11
                nPatternId = 7;
                sPartialRefPattern = @"(?:(?<=(?:</i>,? ))(<i>[0-9]+</i>)(?=(?:, (?:<Doi>(?:(?!</?Doi>).)+</Doi>))))";
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, "<PageRange>$&</PageRange>");
                    goto LBL_PATTERN_END;
                }

                //added by Dakshinamoorthy on 2019-Sep-11
                nPatternId = 8;
                sPartialRefPattern = string.Format(@"({0})({1})({2})", sYearTagPattern, sIssNumPattern, sPageRangePattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, "$1<Issue_No>$2</Issue_No><PageRange>$3</PageRange>");
                    goto LBL_PATTERN_END;
                }

                //added by Dakshinamoorthy on 2019-Sep-13
                nPatternId = 9;
                sPartialRefPattern = string.Format(@"((?<=(?:</i>), ){0})({1})", sPageRangePattern, sDoiTaggedPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, "<PageRange>$1</PageRange>$2");
                    goto LBL_PATTERN_END;
                }

                //With Label
                nPatternId = 101;
                sPartialRefPattern = string.Format(@"( {0})", sVolNumWithLabelPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Vol_No>$1</Vol_No>");
                }

                nPatternId = 102;
                sPartialRefPattern = string.Format(@"( {0})", sIssNumWithLabelPatternPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <Issue_No>$1</Issue_No>");
                }

                nPatternId = 103;
                sPartialRefPattern = string.Format(@"( \(?{0}\)?)", sPageRangeWithLabelPattern);
                if (Regex.IsMatch(sRefTaggedContent, sPartialRefPattern))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, sPartialRefPattern, " <PageRange>$1</PageRange>");
                }



            LBL_PATTERN_END:

                if (!Regex.IsMatch(sRefTaggedContent, "(?:<(?:Vol_No|Issue_No|PageRange)>|(?:<Doi>(?:(?!</?Doi>).)+</Doi>))"))
                {
                    sRefTaggedContent = sRefTaggedContent_Backup;
                }

                //added by Dakshinamoorthy on 2020-Jul-11
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:<ldlTempDate>(?:(?!</?ldlTempDate>).)+</ldlTempDate>)", HandleTempDateInfo);

                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "~~dot~~", ".");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "~~space~~", " ");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "~~o_parenthesis~~", "(");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "~~c_parenthesis~~", ")");

                //added by Dakshinamoorthy on 2019-Jan-11
                //element name 'eLocation_Id' changed to 'PageRange' in-order to make Reference Structuring as generic
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<eLocation_Id>", "<PageRange>");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</eLocation_Id>", "</PageRange>");

                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"</Vol_No>(\)\. )", "$1</Vol_No>");

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ImproveLocalConversion", ex.Message, true, "");

            }
            return sRefTaggedContent;
        }

        private string IdentifyEtAl(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, @"\b([Ee]t[ \-]al)\b", "<Etal>$1</Etal>");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyEtAl", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string IdentifyBookChapterTitle(string sRefContent)
        {
            try
            {
                if (Regex.IsMatch(sRefContent, "<ldlChapterTitle>"))
                {
                    return sRefContent;
                }

                //sRefContent = Regex.Replace(sRefContent, "</?(?:Article_Title|Journal_Title|chapter-title|book-title)>", "");
                sRefContent = Regex.Replace(sRefContent, "</Etal></i>", "</i></Etal>");
                sRefContent = Regex.Replace(sRefContent, @"</Author> ([\:\.;,]+) ", "</Author>$1 ");
                sRefContent = Regex.Replace(sRefContent, @"</Author>([\:\.;,]+) ", "$1</Author> ");
                sRefContent = Regex.Replace(sRefContent, @"</Editor> ([\:\.;,]+) ", "</Editor>$1 ");
                sRefContent = Regex.Replace(sRefContent, @"</Editor>([\:\.;,]+) ", "$1</Editor> ");

                //added by Dakshinamoorthy on 2019-Feb-06
                sRefContent = Regex.Replace(sRefContent, @"(?:(?<= )Vol(?:ume)?\. [0-9]+ of )", HandleEscapeSpace);

                //sRefContent = HexUnicodeToCharConvertor(ConvertDecimalToHexDecimal(sRefContent));

                NormalizeSpaces(ref sRefContent);

                string sBookChapTitlePtn = string.Empty;
                string sTitleLabelPattern = @"(?:(?:<i>)?[\[\{\(]?-?[Ii]n[\:.]?[\]\)\}]?[\:.]?(?:</i>)?[\:.]? )";

                //Full Pattern Matching
                sBookChapTitlePtn = @"(?:(?<=</PubDate>[\:\.;,]* )(?:[^0-9\()<>\u201c\u201d\.]{5,}\. )(?=(?:(?:(?:<(?:ldlCity|ldlState|ldlCountry)>(?:(?!</?(?:ldlCity|ldlState|ldlCountry)>).)+</(?:ldlCity|ldlState|ldlCountry)>[\:\.;, ]*)+)(?:<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>))))";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<BT>{0}</BT>", "$&"));
                    goto LBL_SKIP_PTN;
                }

                sBookChapTitlePtn = @"(?:(?<=</PubDate>[\:\.;,]* )(?:[^0-9\()<>\u201c\u201d\.]{5,}\. )(?=<ldlEditionNumber>(?:(?!</?ldlEditionNumber>).)+</ldlEditionNumber>[\:\.,;]* (?:(?:(?:<(?:ldlCity|ldlState|ldlCountry)>(?:(?!</?(?:ldlCity|ldlState|ldlCountry)>).)+</(?:ldlCity|ldlState|ldlCountry)>[\:\.;, ]*)+)(?:<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>))))";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<BT>{0}</BT>", "$&"));
                    goto LBL_SKIP_PTN;
                }

                sBookChapTitlePtn = @"(?:(?<=</PubDate>[\:\.;,]* )(?:[^0-9\()<>\u201c\u201d\.]{5,}\. )(?=\[[a-zA-Z ]+[\:\.;,]*\][\:\.;,]* (?:(?:(?:<(?:ldlCity|ldlState|ldlCountry)>(?:(?!</?(?:ldlCity|ldlState|ldlCountry)>).)+</(?:ldlCity|ldlState|ldlCountry)>[\:\.;, ]*)+)(?:<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>))))";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<BT>{0}</BT>", "$&"));
                    goto LBL_SKIP_PTN;
                }

                //Partial pattern matching
                sBookChapTitlePtn = @"((?:[\u201c][^\u201c\u201d]+[\u201d])[.;,]* )(" + sTitleLabelPattern + @")([^\.<>]{5,})";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<CT>{0}</CT><TL>{1}</TL><BT>{2}</BT>", "$1", "$2", "$3"));
                    goto LBL_SKIP_PTN;
                }

                sBookChapTitlePtn = @"((?:(?<=</(?:Author|Editor|Etal|Collab|PubDate|ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>[\:\.;, ]* ))[^\.,<>]{5,}, )(" + sTitleLabelPattern + @")([^<>\.]{5,})";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<CT>{0}</CT><TL>{1}</TL><BT>{2}</BT>", "$1", "$2", "$3"));
                    goto LBL_SKIP_PTN;
                }

                sBookChapTitlePtn = @"((?<=(?:(?:</(?:Author|PubDate|Etal|Collab)>[\.,; ]*))) (?:(?:[\u201c](?:(?![\u201c\u201d]).)+[\u201d])[\.;,]* ))(" + sTitleLabelPattern + @")([^<>0-9\.,]{5,}, )";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<CT>{0}</CT><TL>{1}</TL><BT>{2}</BT>", "$1", "$2", "$3"));
                    goto LBL_SKIP_PTN;
                }

                sBookChapTitlePtn = @"((?<=</Author>.{0,10})(?:[\u201c][^\u201c\u201d]+[\u201d]) )(" + sTitleLabelPattern + @")([^<>0-9\.,;]{5,})";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<CT>{0}</CT><TL>{1}</TL><BT>{2}</BT>", "$1", "$2", "$3"));
                    goto LBL_SKIP_PTN;
                }

                sBookChapTitlePtn = @"(?<=</Author>[\:\.;, ]+)([^0-9<>\.;,]{5,}, )((?:<i>)?(?:Chapt(?:er)?[\. ]+[0-9]+ )?-?in[\:.]?(?:</i>)?[\:.]? )([^0-9<>\.;,]{5,}, )";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<CT>{0}</CT><TL>{1}</TL><BT>{2}</BT>", "$1", "$2", "$3"));
                    goto LBL_SKIP_PTN;
                }

                sBookChapTitlePtn = @"((?:[\u201c][^\u201c\u201d]+[\u201d])[.;,]* )([^a-z0-9<][^\.<>]{5,})";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<CT>{0}</CT><BT>{1}</BT>", "$1", "$2"));
                    goto LBL_SKIP_PTN;
                }

                sBookChapTitlePtn = @"(?<=</PubDate>)([^\.0-9<>\(\)]{5,}\. )(" + sTitleLabelPattern + @")([^\.0-9<>\(\)]{5,}\.)";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<CT>{0}</CT><TL>{1}</TL><BT>{2}</BT>", "$1", "$2", "$3"));
                    goto LBL_SKIP_PTN;
                }

                sBookChapTitlePtn = @"((?<=</PubDate> )<i>(?:(?!</?i>).)+</i>[\.,;]* )((?:(?<= )Vol(?:ume)?(?:\. |~~dot~~~~space~~)[0-9]+(?:[ ]|~~space~~)of(?:[ ]|~~space~~))<i>(?:(?!</?i>).)+</i>[\.,;]* )";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<CT>{0}</CT><BT>{1}</BT>", "$1", "$2"));
                    goto LBL_SKIP_PTN;
                }

                //Thesis
                sBookChapTitlePtn = @"(?:(?<=</PubDate>[\:\.;, ]+)([\u201c][^\u201c\u201d]+[\u201d][\:\.;,]* )(?=<ldlThesisKeyword>))";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<{0}>{1}</{0}>", "ldlArticleTitle", "$&"));
                    goto LBL_SKIP_PTN;
                }


                //Book Title only
                sBookChapTitlePtn = @"((?<=</(?:Author|Editor|Etal|Collab|ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>)[\:\.;, ]+(?:[^<>\.,;\(\)]+[\.,; ]*)(?=\(?<(?:ldlPublisherName|Vol_No)>))";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<BT>{0}</BT>", "$1"));
                }

                sBookChapTitlePtn = @"((?<=</(?:Author|Editor|Etal|Collab|ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>[\:\.;, ]{5,})(?:[^<>\.,;\(\)]+[\.,; ]*))";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<BT>{0}</BT>", "$1"));
                }

                sBookChapTitlePtn = @"((?<=</(?:Author|Editor|Etal|Collab|ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>[\:\.;, ]+)(?:[\u201c][^\u201c\u201d]+[\u201d])(?= (?:-?in[\:.]* )|(?: [<])))";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<BT>{0}</BT>", "$1"));
                }

                sBookChapTitlePtn = @"(?<=(?:</(?:Author|Editor|Etal|Collab|ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>[\:\.;, ]+))([\u201c][^\u201c\u201d]+[\u201d][\.;, ]*)";
                if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
                {
                    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<BT>{0}</BT>", "$1"));
                }

            //sBookChapTitlePtn = @"((?<=</(?:Author|Editor|Etal|Collab|ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>[\:\.;, ]+)[^\.<>]{5,})";
            //if (Regex.IsMatch(sRefContent, sBookChapTitlePtn))
            //{
            //    sRefContent = Regex.Replace(sRefContent, sBookChapTitlePtn, string.Format("<BT>{0}</BT>", "$1"));
            //}


            LBL_SKIP_PTN:

                sRefContent = Regex.Replace(sRefContent, "<CT>", "<ldlChapterTitle>");
                sRefContent = Regex.Replace(sRefContent, "</CT>", "</ldlChapterTitle>");

                sRefContent = Regex.Replace(sRefContent, "<BT>", "<ldlBookTitle>");
                sRefContent = Regex.Replace(sRefContent, "</BT>", "</ldlBookTitle>");

                sRefContent = Regex.Replace(sRefContent, "<TL>", "<ldlTitleLabel>");
                sRefContent = Regex.Replace(sRefContent, "</TL>", "</ldlTitleLabel>");

                sRefContent = Regex.Replace(sRefContent, "<T>", "<ldlChapterTitle>");
                sRefContent = Regex.Replace(sRefContent, "</T>", "</ldlChapterTitle>");

                sRefContent = Regex.Replace(sRefContent, "~~space~~", " ");
                sRefContent = Regex.Replace(sRefContent, "~~dot~~", ".");

                //sRefContent = DoHexaDecimalUnicodeConversion(sRefContent);

                //check duplication in book title
                if (Regex.Matches(sRefContent, "<ldlBookTitle>((?:(?!</ldlBookTitle>).)+)</ldlBookTitle>").Count == 2)
                {
                    sRefContent = Regex.Replace(sRefContent, "^((?:(?!</?ldlBookTitle>).)+)<ldlBookTitle>((?:(?!</ldlBookTitle>).)+)</ldlBookTitle>",
                        "$1<ldlChapterTitle>$2</ldlChapterTitle>");
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyBookChapterTitle", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string IdentifyConferenceName(string sRefContent)
        {
            string sOutputContent = string.Empty;
            nConfNameCount = 0;
            try
            {
                string sMonthName = @"\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b";

                //updated by Dakshinamoorthy on 2019-Jan-12
                //string sConfNamePattern = @"(?:(?:(?: [^\.,<>]+(?:Conference|Annual Meeting|Symposium)[^\.,]*[\.,]+)|((?: |<i>)(?:[0-9]+(?:<sup>)?(?:th|st|rd|nd)(?:</sup>)?|[A-Z][a-z]+(?:th|st|rd|nd) )[^\.,]*[^\.,]+(?:Conference|Symposium|Conf(?:\.|~~dot~~))[^\.,]*[\.,]+)|(?:International )?(?:Proc(?:\.,?|(?:~~dot~~(?:~~comma~~)?))[\.,]*[^\.,]*[\.,]+|(?:Symposium )?Proceedings?(?:[\.,]*|(?:~~dot~~(?:~~comma~~)?))[^\.,]*[\.,]+))+)";
                //string sConfNamePattern = @"(?:(?:(?: [^\.,<>]+(?:Conference|Annual Meeting|Symposium|Workshop)[^\.,]*[\.,]+)|(?:(?:(?:20|19)[0-9]{2}) (?:\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b)(?: \b(?<!<monthNum>)(?:(?:[0]?[1-9])|(?:[1-2][0-9])|(?:[3][0-1]))(?:(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?)?\b)? ([A-Za-z]+ )+(International|Int~~dot~~),)|((?: |<i>)(?:Proc(?:\.,?|(?:~~dot~~(?:~~comma~~)?)) )?(?:[0-9]+(?:<sup>)?(?:th|st|rd|nd)(?:</sup>)?|[A-Z][a-z]+(?:th|st|rd|nd) )[^\.,]*[^\.,]+(?:Conference|Symposium|Conf(?:\.|~~dot~~))[^\.,]*[\.,]+)|(?:<i>(?:Proc\. Natl\.|Proceedings?) (?:(?!</?i>).)+</i>)|(?:(?<=(?:[Ii]n ))(?:Proceeding|Proc(?:~~dot~~|~~comma~~)+ <i>(?:(?!</?i>).)+</i>)[\.\;, ]+)|(?:Vol(?:ume)?(?:~~dot~~|[\.])? [0-9]+ of )?(?:International )?(?:Proc(?:\.,?|(?:~~dot~~(?:~~comma~~)?))[\.,]*[^\.,]*[\.,]+|(?:Symposium )?Proceedings?(?:[\.,]*|(?:~~dot~~(?:~~comma~~)?))[^\.,<>]*[\.,]+))+)";

                //string sConfNamePattern = @"(?:(?:(?: [^\.,<>]+(?:Conference|Annual Meeting|Symposium|Workshop)[^\.,\:\(\)]*[\.,]+)|<i>(?:(?:(?!</?i>).)*)[0-9]+(?:(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?) International Conference (?:(?:(?!</?i>).)+)</i>|(?:(?:[0-9]+(?:(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?)) [^\.,;\:\(\)]+ Meeting[\.,;]+ )|(?:(?:(?:20|19)[0-9]{2}) (?:\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b)(?: \b(?<!<monthNum>)(?:(?:[0]?[1-9])|(?:[1-2][0-9])|(?:[3][0-1]))(?:(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?)?\b)? ([A-Za-z]+ )+(International|Int~~dot~~),)|(?:(?: |<i>)(?:Proc(?:\.,?|(?:~~dot~~(?:~~comma~~)?)) )?(?:[0-9]+(?:<sup>)?(?:th|st|rd|nd)(?:</sup>)?|[A-Z][a-z]+(?:th|st|rd|nd) )[^\.,\:\(\)]*[^\.,\:\(\)]+(?:Conference|Symposium|Conf(?:\.|~~dot~~))[^\.,\:\(\)]*[\.,]+)|(?:<i>(?:Proc\. Natl\.|Proceedings?) (?:(?!</?i>).)+</i>)|(?:(?<=(?:[Ii]n ))(?:Proceeding|Proc(?:~~dot~~|~~comma~~)+ <i>(?:(?!</?i>).)+</i>)[\.\;, ]+)|(?:Vol(?:ume)?(?:~~dot~~|[\.])? [0-9]+ of )?(?:International )?(?:Proc(?:\.,?|(?:~~dot~~(?:~~comma~~)?))[\.,]*[^\.,]*[\.,]+|(?:Symposium )?Proceedings?(?:[\.,]*|(?:~~dot~~(?:~~comma~~)?))[^\.,<>]*[\.,]+))+)";
                string sConfNamePattern = @"(?:(?:(?: [^\.,<>]+(?:Conference|Annual Meeting|Symposium|Workshop)[^\.,\:\(\)]*[\.,]+)|<i>(?:(?:(?!</?i>).)*)[0-9]+(?:(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?) International Conference (?:(?:(?!</?i>).)+)</i>|(?:(?:[0-9]+(?:(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?)) [^\.,;\:\(\)]+ Meeting[\.,;]+ )|(?:(?:(?:20|19)[0-9]{2}) (?:\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b)(?: \b(?<!<monthNum>)(?:(?:[0]?[1-9])|(?:[1-2][0-9])|(?:[3][0-1]))(?:(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?)?\b)? ([A-Za-z]+ )+(International|Int~~dot~~),)|(?:(?: |<i>)(?:Pr oc(?:\.,?|(?:~~dot~~(?:~~comma~~)?)) )?(?:[0-9]+(?:<sup>)?(?:th|st|rd|nd)(?:</sup>)?|[A-Z][a-z]+(?:th|st|rd|nd) )[^\.,\:\(\)]*[^\.,\:\(\)]+(?:Conference|Symposium|Conf(?:\.|~~dot~~))[^\.,\:\(\)]*[\.,]+)|(?:<i>(?:Pr oc\. Natl\.|Proceedings?) (?:(?!</?i>).)+</i>)|(?:(?<=(?:[Ii]n ))(?:Proceeding|Pr oc(?:~~dot~~|~~comma~~)+ <i>(?:(?!</?i>).)+</i>)[\.\;, ]+)|(?:Vol(?:ume)?(?:~~dot~~|[\.])? [0-9]+ of )?(?:International )?(?:(?:Proc(?:\.,?|(?:~~dot~~(?:~~comma~~)?)) [Ii]n )?Proc(?:\.,?|(?:~~dot~~(?:~~comma~~)?))[\.,]*[^\.,]*[\.,]+|(?:(?:20|19)[0-9]{2}) ([0-9]+(?:(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)? ))?International Conference [^\.,;<>]+[\.,;]+|International Conference [^<>\.,;\:]+[\:\.,;]+|(?:Proceedings of the [^<>\.,\:;]+ [A-Z]{3,}[\u2019\u0027][0-9]+[\.]? )|(?:Symposium )?Proceedings?(?:[\.,]*|(?:~~dot~~(?:~~comma~~)?))[^\.,<>]*[\.,]+))+)";


                //dot escape
                sRefContent = Regex.Replace(sRefContent, @"\b(?:Proc\.,)", "Proc~~dot~~~~comma~~");
                sRefContent = Regex.Replace(sRefContent, @"\b(?:Proceeding,)", "Proceeding~~comma~~");
                sRefContent = Regex.Replace(sRefContent, @"\b(?:Conf\.)", "Conf~~dot~~");
                sRefContent = Regex.Replace(sRefContent, @"\b(?:Int\.)", "Int~~dot~~");
                sRefContent = Regex.Replace(sRefContent, @"(?:\b(?:[A-Z]\.))+", EscapeDot4ConfName);
                sRefContent = Regex.Replace(sRefContent, @"(?:(?<= )Vol(?:ume)?\. [0-9]+ of )", EscapeDot4ConfName);

                //cycle 1
                sRefContent = Regex.Replace(sRefContent, sConfNamePattern, HandleIdentifyConferenceName, RegexOptions.IgnoreCase);

                //added by Dakshinamoorthy on 2019-Jan-12
                //cycle 2
                nConfNameCount = 0;
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=</ldlConferenceName>[ ]*)(?:{0}))", sConfNamePattern), HandleIdentifyConferenceName, RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"</ldlConferenceName>([\:\.;, ]*)<ldlConferenceName>", "$1");
                sRefContent = Regex.Replace(sRefContent, @"(<i>(?:(?!</?(?:ldlConferenceName|i)>).)+<ldlConferenceName>(?:(?!</?(?:ldlConferenceName|i)>).)+)(</i>\.</ldlConferenceName>)", "$1</ldlConferenceName></i>.");

                sRefContent = HandleMissingConferenceNamePart(sRefContent);
                sRefContent = Regex.Replace(sRefContent, @"(<ldlConferenceName>(?:(?!</?ldlConferenceName>).)+</ldlConferenceName>)", DateInsideConferenceName);

                //added by Dakshinamoorthy on 2019-Sep-14
                sRefContent = Regex.Replace(sRefContent, @"(?:<i>(?:(?!</?(?:ldlConferenceName|i)>).)*<ldlConferenceName>(?:(?!</?(?:ldlConferenceName)>).)+</ldlConferenceName>(?:(?!</?(?:ldlConferenceName|i)>).)*</i>)", MakeFullContentAsConfName);
                sRefContent = Regex.Replace(sRefContent, @"</ldlConferenceName>([\u201D\.,;]+)", "$1</ldlConferenceName>");
                sRefContent = Regex.Replace(sRefContent, @"((?<=(?:[\.\u201D\u2019;,]+ ))[Ii]n[\:]?) (Vol(?:~~dot~~|\.) [0-9]+ of )<ldlConferenceName>",
                    "<ldlTitleLabel>$1</ldlTitleLabel> <ldlConferenceName>$2");

                //reverting "Dot" and "Comma"
                sRefContent = Regex.Replace(sRefContent, "~~dot~~", ".");
                sRefContent = Regex.Replace(sRefContent, "~~comma~~", ",");
                sRefContent = Regex.Replace(sRefContent, @"<ldlConferenceName>([ ]*[Ii]n\: )", "$1<ldlConferenceName>");

            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyConferenceName", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string MakeFullContentAsConfName(Match myConfNameMatch)
        {
            string sConfNameContent = myConfNameMatch.Value.ToString();

            try
            {
                sConfNameContent = Regex.Replace(sConfNameContent, "</?ldlConferenceName>", "");
                sConfNameContent = string.Format("<{0}>{1}</{0}>", "ldlConferenceName", sConfNameContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\MakeFullContentAsConfName", ex.Message, true, "");
            }

            return sConfNameContent;
        }


        private string EscapeDot4ConfName(Match myMatch)
        {
            string sOutputContent = myMatch.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, @"\.", "~~dot~~");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\EscapeDot4ConfName", ex.Message, true, "");

            }
            return sOutputContent;
        }

        private string IdentifyReportNumber(string sRefContent)
        {
            string sOutputContent = string.Empty;
            nReportNoCount = 0;
            try
            {
                string sReportNumberPattern = @"(?:(?<=(?:[\u201d\u2019\.,]+ |</i>[\.,]* ))(?:(?:Consultant[\u2019]s report [^\.,]+)|(?:Technical Report (?:(?:(?:[0-9]+)(?:\.[0-9]+)?)|(?:[A-Z]{2,}[0-9]+[\u2013\-][0-9]+)))|Report on (?:ﬁle|file)|(?:(?:Rep\.|Rpt\. No\.) [A-Z0-9\-\/]+))[\.,]+)";
                sRefContent = Regex.Replace(sRefContent, sReportNumberPattern, HandleIdentifyReportNumber, RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyConferenceName", ex.Message, true, "");

            }
            return sRefContent;
        }


        private string DateInsideConferenceName(Match myMatch)
        {
            string sOutputContent = myMatch.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "</?ldlConferenceDate>", "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\DateInsideConferenceName", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private string HandleMissingConferenceNamePart(string sRefContent)
        {
            int nPatternId = 0;
            string sMissingConferenceName = string.Empty;

            try
            {
                nPatternId = 1;
                sMissingConferenceName = @"(?<=(?:<PubDate>(?:(?!</?PubDate>).)+</PubDate>[\.;,\: ]*)(?:[\u201c][^\u201c\u201d]+[\u201d]))(?: [A-Z]{4,}['\u2019][0-9]{2,4}[\:\.,; ]+)(?=(?:[ ]?<ldlConferenceName>))";
                if (Regex.IsMatch(sRefContent, sMissingConferenceName))
                {
                    sRefContent = Regex.Replace(sRefContent, sMissingConferenceName, string.Format("<{0}>{1}</{0}>", "ldlConferenceName", "$&"));
                    goto LBL_SKIP_MATCH;
                }

            LBL_SKIP_MATCH:
                sRefContent = Regex.Replace(sRefContent, "</ldlConferenceName><ldlConferenceName>", "");

            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleMissingConferenceNamePart", ex.Message, true, "");
            }
            return sRefContent;
        }


        private string HandleIdentifyConferenceName(Match myConfMatch)
        {
            string sOutputContent = myConfMatch.Value.ToString();
            nConfNameCount += 1;

            try
            {
                if (nConfNameCount == 1)
                {
                    sOutputContent = string.Format("<{0}>{1}</{0}>", "ldlConferenceName", sOutputContent);
                }
                else
                {
                    return sOutputContent;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleIdentifyConferenceName", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private string HandleIdentifyReportNumber(Match myConfMatch)
        {
            string sOutputContent = myConfMatch.Value.ToString();
            nReportNoCount += 1;

            try
            {
                if (nReportNoCount == 1)
                {
                    sOutputContent = string.Format("<{0}>{1}</{0}>", "ldlReportNumber", sOutputContent);
                }
                else
                {
                    return sOutputContent;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleIdentifyReportNumber", ex.Message, true, "");
            }
            return sOutputContent;
        }




        private string IdentifyJournalArticleTitle(string sRefContent)
        {
            string sOuputContent = string.Empty;
            string sBackup4Title = sRefContent;

            try
            {
                //sRefContent = Regex.Replace(sRefContent, "</?(?:Article_Title|Journal_Title|chapter-title)>", "");
                sRefContent = Regex.Replace(sRefContent, "</Etal></i>", "</i></Etal>");

                //sRefContent = HexUnicodeToCharConvertor(sRefContent);
                sRefContent = HandleFormattingTags(sRefContent);
                NormalizeSpaces(ref sRefContent);

                //updated by Dakshinamoorthy on 2020-July-03 ("AuEdGroup" added)
                if (Regex.IsMatch(sRefContent, @"((?<=<\/(?:Year|PubDate|Etal|Author|ldlFirstAuEdCollabGroup|AuEdGroup)>[\)\.\:;]* )(?:[^a-z])(?:(?!<\/?(?:Vol_No|ldl[^<> ]+|Doi|Author|ldlFirstAuEdCollabGroup|AuEdGroup|Editor|Etal|Issue_No|FirstPage|Year|PubDate|Website|PageRange|Article_Title|Journal_Title)>).)+(?= <(?:Vol_No|ldl[^<> ]+|Doi|Issue_No|FirstPage|Year|PubDate|Website|PageRange)>))"))
                {
                    sOuputContent = Regex.Replace(sRefContent, @"((?<=<\/(?:Year|PubDate|Etal|Author|ldlFirstAuEdCollabGroup|AuEdGroup)>[\)\.\:;]* )(?:[^a-z])(?:(?!<\/?(?:Vol_No|ldl[^<> ]+|Doi|Author|ldlFirstAuEdCollabGroup|AuEdGroup|Editor|Etal|Issue_No|FirstPage|Year|PubDate|Website|PageRange|Article_Title|Journal_Title)>).)+(?= <(?:Vol_No|ldl[^<> ]+|Doi|Issue_No|FirstPage|Year|PubDate|Website|PageRange)>))", HandleJournalArticleTitle);
                    sOuputContent = ValidateArticleTitle(sOuputContent);
                }
                else if (Regex.IsMatch(sRefContent, @"((?<=<\/?(?:Year|PubDate|Etal|Author|ldlFirstAuEdCollabGroup|AuEdGroup)>[\)\.\:;]* )(?:(?:(?!(?:<(?:Etal|Author|ldlFirstAuEdCollabGroup|AuEdGroup|Editor|Vol_No|ldl[^<> ]+|Doi|Issue_No|FirstPage|Year|PubDate|Website|Article_Title|Journal_Title|PageRange)>)).)+(?:(?= \b(?:[0-9]+)\b)).)+(?=[ ]?\b(?:[0-9]+)\b))"))
                {
                    sOuputContent = Regex.Replace(sRefContent, @"((?<=<\/?(?:Year|PubDate|Etal|Author|ldlFirstAuEdCollabGroup|AuEdGroup)>[\)\.\:;]* )(?:(?:(?!(?:<(?:Etal|Author|ldlFirstAuEdCollabGroup|AuEdGroup|Editor|Vol_No|ldl[^<> ]+|Doi|Issue_No|FirstPage|Year|PubDate|Website|Article_Title|Journal_Title|PageRange)>)).)+(?:(?= \b(?:[0-9]+)\b)).)+(?=[ ]?\b(?:[0-9]+)\b))", HandleJournalArticleTitle);
                    sOuputContent = ValidateArticleTitle(sOuputContent);
                }

                if (!Regex.IsMatch(sOuputContent, "</?(?:Article_Title|Journal_Title)>"))
                {
                    sOuputContent = Regex.Replace(sRefContent, @"((?:(?<=<\/(?:Year|PubDate|Etal|Author|ldlFirstAuEdCollabGroup|AuEdGroup)>[\)\.\:;]* )(?:[^a-z])(?:(?!<\/?(?:Vol_No|Doi|Author|ldlFirstAuEdCollabGroup|AuEdGroup|Editor|Etal|Issue_No|ldl[^<> ]+|FirstPage|Year|PubDate|Website|Article_Title|Journal_Title)>)[^<>])+(?= <(?:Vol_No|Doi|Issue_No|FirstPage|Year|PubDate)>))|(?:(?<=<\/?(?:Year|PubDate|Etal|Author)>[\)\.\:;]* )(?:(?:(?!(?:<(?:Etal|Author|Vol_No|Doi|Issue_No|FirstPage|Year|PubDate|Website)>)).)+(?:(?= \b(?:[0-9]+)\b)).)+(?= \b(?:[0-9]+)\b)))", SplitArtJouTitleByLowerCaseWord);
                }

                if (!Regex.IsMatch(sOuputContent, "</?(?:Article_Title|Journal_Title)>"))
                {
                    sOuputContent = sBackup4Title;
                }


                sOuputContent = IdentifyMissingElements4JournalTitle(sOuputContent);
                sOuputContent = Regex.Replace(sOuputContent, @"<Article_Title>([\:\.;, ]+)", "$1<Article_Title>");
                sOuputContent = Regex.Replace(sOuputContent, @"<Journal_Title>([\:\.;, ]+)", "$1<Journal_Title>");

                sOuputContent = Regex.Replace(sOuputContent, @"</Article_Title>([\:\.;,\u201d\u2019 ]+)", "$1</Article_Title>");
                sOuputContent = Regex.Replace(sOuputContent, @"</Journal_Title>([\:\.;,\u201d\u2019 ]+)", "$1</Journal_Title>");
                sOuputContent = Regex.Replace(sOuputContent, @"<Journal_Title>([Ii]n\:[ ]?)", "<ldlTitleLabel>$1</ldlTitleLabel><Journal_Title>");

                //sOuputContent = DoHexaDecimalUnicodeConversion(sOuputContent);

            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyJournalArticleTitle", ex.Message, true, "");
            }
            return sOuputContent;
        }

        private string PatternMacth4JournalArticleTitle(string sTitleContent)
        {
            try
            {
                //Identify Article and Journal Title Using Pattern
                int nPatternId = 0;
                bool bIsSpaceIntroAtEnd = false;

                string sArticleJournalTitlePtn = string.Empty;
                string sDoubleQuotesTitle = @"(?:(?:[\.,]+ )?(?:(?:&#x201[Cc];(?:(?!(?:&#x201[Cc];|&#x201[Dd];)).)+&#x201[Dd];)|(?:[\u201C][^\u201C\u201D]+[\u201D]))(?:[\:\.;, ]+|$))";
                string sSinlgeQuotesTitle = @"(?:(?:[\.,]+ )?(?:(?:&#x2018;(?:(?!(?:&#x2018;|&#x2019;)).)+&#x2019;)|(?:[\u2018][^\u2018\u2019]+[\u2019]))(?:[\:\.;, ]+|$))";
                string sItalicTitle = @"(?:(?:[\.,]+ )?(?:<i>(?:(?!</?i>).)+</i>)(?:[\:\.;, ]+|$|(?= [0-9]+)))";
                string sUptoCommaTitle = @"(?:(?:[^0-9<>\(\)\[\]\.,]{5,}),(?: |$))";
                string sUptoDotTitle = @"(?:(?:[^0-9<>\(\)\[\]\.,]{5,})\.(?: |$))";
                //added by Dakshinamoorthy on 2020-Jul-08
                string sTitleWithoutEndPeriod = @"(?:(?:[^0-9<>\(\)\[\]\.,]{5,})(?: |$))";

                string sUptoDotTitle_lite = @"(?:(?:[^\.,]{5,})\.(?: |$))";
                string sStartsWithNationalityTitle = string.Format(@"(?:(?:{0}) (?:J\. [^0-9\(\)\[\]]+))", sNationalityPattern);
                string sJournalTtlAbbrPatn = @"(?:(?:(?:[A-Z][a-z]*\. ){2,}(?:[A-Z][a-z]*\.),?)|(?:(?:[A-Z][a-z]*\. )(?:(?:[A-Z][a-z]*\.? ){2,}(?:[A-Z][a-z]*\.),?))|(?:(?:[A-Z][a-z]* )(?:(?:[A-Z][a-z]*\. ){1,}(?:[A-Z][a-z]*\.),?)))";

                nPatternId = 1;
                sArticleJournalTitlePtn = string.Format("^({0})({1})$", sDoubleQuotesTitle, sItalicTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 1;
                sArticleJournalTitlePtn = string.Format("^({0})({1})$", sSinlgeQuotesTitle, sItalicTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 2;
                sArticleJournalTitlePtn = string.Format(@"^({0})((?:(?:<i>(?:(?!</?i>).)+</i>)(?:[\:\.;, ]*|$|(?= [0-9].*))))$", sDoubleQuotesTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 3;
                sArticleJournalTitlePtn = string.Format("^({0})({1})$", sDoubleQuotesTitle, sUptoCommaTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 3;
                sArticleJournalTitlePtn = string.Format("^({0})({1})$", sUptoDotTitle_lite, sItalicTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 4;
                sArticleJournalTitlePtn = string.Format("^({0})({1})$", sDoubleQuotesTitle, sStartsWithNationalityTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 5;
                sArticleJournalTitlePtn = string.Format(@"^({0})((?:(?:Journal|J\.) of |Int\. J\. )[^0-9\(\)]+)$", sDoubleQuotesTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 6;
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})$", sUptoDotTitle, sItalicTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 7;
                sArticleJournalTitlePtn = string.Format(@"^({0})((?:Journal of |Int. J. )[^0-9\(\)]+)$", sUptoDotTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 8;
                sArticleJournalTitlePtn = string.Format(@"^({0})((?:Journal of |Int. J. |Int. )[^0-9\(\)]+)$", sDoubleQuotesTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 9;
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})$", sDoubleQuotesTitle, sJournalTtlAbbrPatn);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 10;
                sArticleJournalTitlePtn = string.Format(@"^({0})([^\.]{{5,}}\.,?)$", sDoubleQuotesTitle, sUptoDotTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 11;
                //“Finance and Growth: Theory and Evidence.” In <i>Handbook of Economic Growth</i>
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})({2})$", sDoubleQuotesTitle, "In ", sItalicTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}>{2}<{3}>{4}</{3}>", "Article_Title", "$1", "$2", "Journal_Title", "$3"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 12;
                //“Social Protection as Development Policy: A New International Agenda for Action.” International Development Policy
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})$", sDoubleQuotesTitle, "(?:[A-Za-z ]{5,})");
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 13;
                //A First Look at Finance Patents, 1971 to 2000.” <i>Journal of Finance</i>.
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})$", sDoubleQuotesTitle, sItalicTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }


                //added by Dakshinamoorthy on 2019-Jan-21
                nPatternId = 14;
                //Auto regressive and ensemble empirical mode decomposition hybrid model for annual runoff forecasting, <i>Water Resour. Manage</i>.,
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})$", sUptoCommaTitle, sItalicTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                //added by Dakshinamoorthy on 2019-Jan-21
                nPatternId = 15;
                //Auto regressive and ensemble empirical mode decomposition hybrid model for annual runoff forecasting, <i>Water Resour. Manage</i>.,
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})$", @"(?:[A-Za-z, \-\:]{5,}, )", sItalicTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }


                //added by Dakshinamoorthy on 2019-Sep-22
                nPatternId = 16;
                //Minimising MCMC variance via diffusion limits, with an application to simulated tempering. <i>Ann. Appl. Prob.</i>
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})$", @"(?:(?:[^0-9<>\(\)\[\]\.]{5,})\.(?: |$))", sItalicTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                //added by Dakshinamoorthy on 2019-Sep-30
                nPatternId = 17;
                //“Seismic Performance of Square Concrete Columns Confined with Glass Fiber-Reinforced Polymer Ties.” J. Compos. Constr., 2018,
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})((?: [0-9]{{4}}[, ]*))$", sDoubleQuotesTitle, sJournalTtlAbbrPatn);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>$3", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                //added by Dakshinamoorthy on 2020-Jul-08
                nPatternId = 18;
                //‘Economic Effect Analysis of Trade Liberalization and Facilitation of the 21st Century Maritime Silk Road’. Asia-pacific Economic Review Asia-pacific Economic Review
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})$", sSinlgeQuotesTitle, sTitleWithoutEndPeriod);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                //added by Dakshinamoorthy on 2020-Jul-08
                nPatternId = 19;
                //‘The overall framework of institutionalization in China - India relations’. South Asian Studies Quarterly,
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})$", sSinlgeQuotesTitle, sUptoCommaTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                //added by Dakshinamoorthy on 2020-Jul-08
                nPatternId = 20;
                //Connotation characteristics of precision poverty alleviation thought and Its Enlightenment to the practice of poverty alleviation [J]. Jianghan Academic.
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})$", sUptoDotTitle_lite, sUptoDotTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                //added by Dakshinamoorthy on 2020-Jul-09
                nPatternId = 21;
                //<i>Precise identification of tourism Poverty Alleviation under the background of precision poverty alleviation [J]</i>. Ecological economy,
                sArticleJournalTitlePtn = string.Format(@"^({0})({1})$", sItalicTitle, sUptoCommaTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    goto LBL_SKIP_MATCH;
                }

                //Journal Title Only

                if (!Regex.IsMatch(sTitleContent, "[ ]$"))
                {
                    sTitleContent = sTitleContent + " ";
                    bIsSpaceIntroAtEnd = true;
                }


                nPatternId = 1;
                //, <i>Science</i>,
                sArticleJournalTitlePtn = string.Format(@"^((?:[,\. ]*){0})$", sItalicTitle);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}>", "Journal_Title", "$1"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 2;
                //, Phys. Rev. B,
                sArticleJournalTitlePtn = string.Format(@"^((?:[,\. ]*){0}(?:[,\. ]*))$", sJouTitAbbrPattern);
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}>", "Journal_Title", "$&"));
                    goto LBL_SKIP_MATCH;
                }

                nPatternId = 3;
                //, Phys. Rev. B,
                sArticleJournalTitlePtn = string.Format(@"^((?:[,\. ]*){0}(?:[,\. ]*))$", "(?:[A-Z][a-z]+)");
                if (Regex.IsMatch(sTitleContent, sArticleJournalTitlePtn))
                {
                    sTitleContent = Regex.Replace(sTitleContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}>", "Journal_Title", "$&"));
                    goto LBL_SKIP_MATCH;
                }

                //-------------------------------------
                // Database match
                //-------------------------------------
                //added by Dakshinamoorthy on 2019-Jan-12


                string sQuery = string.Empty;
                SqlCommand cmd = new SqlCommand();
                DataTable dtResult = new DataTable();

                string sTitleContentCleaned = General.DatabaseIndexCleanup(sTitleContent);
                sTitleContentCleaned = Regex.Replace(sTitleContentCleaned, @"^(?:[\.;, ]+)", "");
                sTitleContentCleaned = Regex.Replace(sTitleContentCleaned, @"(?:[\.;, ]+)$", "");

                sQuery = string.Format("select distinct(data1) from data_dict_journaltitle where data1 = '{0}'", sTitleContentCleaned.ToLower());
                Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                dtResult = General.GeneralInstance.dataTable;

                if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                {
                    sTitleContent = string.Format("<{0}>{1}</{0}>", "Journal_Title", sTitleContent);
                    goto LBL_SKIP_MATCH;
                }

            //-------------------------------------

            LBL_SKIP_MATCH:

                if (bIsSpaceIntroAtEnd == true)
                {
                    NormalizeSpaces(ref sTitleContent);
                    sTitleContent.TrimEnd();
                }

                return sTitleContent;
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\PatternMacth4JournalArticleTitle", ex.Message, true, "");
            }
            return sTitleContent;
        }


        private string HandleJournalArticleTitle(Match myTitleMatch)
        {
            string sTitleTaggedContent = myTitleMatch.Value.ToString();

            try
            {
                NormalizeSpaces(ref sTitleTaggedContent);

                sTitleTaggedContent = PatternMacth4JournalArticleTitle(sTitleTaggedContent);
                if (Regex.IsMatch(sTitleTaggedContent, "<Article_Title>") || Regex.IsMatch(sTitleTaggedContent, "<Journal_Title>"))
                {
                    return sTitleTaggedContent;
                }
                else
                {
                    sTitleTaggedContent = Regex.Replace(sTitleTaggedContent, @"</?(?:Article_Title|Journal_Title)>", "");
                }

                List<string> lstTitleContentSplited = Regex.Split(sTitleTaggedContent, "[ ]").ToList();
                List<string> lstCleanedWords = new List<string>();
                List<string> lstCleanedWordsExact = new List<string>();
                List<int> lstMatchedIndex = new List<int>();
                string sTemp = string.Empty;
                string sCleanedTitle = string.Empty;
                string sCleanedTitle_ASCII = string.Empty;
                //string sSkippedWordsPattern = "(?:of|the|an|a|the|in|and|to|for|on)";
                string sQuery = string.Empty;
                DataTable dtResult = new DataTable();
                bool bIsExactMatchFound = false;

                string sTemp1 = string.Empty;
                string sTemp2 = string.Empty;

                //updated by Dakshinamoorthy on 2019-Jan-18
                //Description: Bug fixed
                //for (int i = lstTitleContentSplited.Count - 1; i >= 1; i--)
                for (int i = lstTitleContentSplited.Count - 1; i >= 0; i--)
                {
                    sTemp = lstTitleContentSplited[i];
                    sTemp = HexUnicodeToCharConvertor(sTemp);
                    sTemp = TitleContentCleanup(sTemp);

                    if (Regex.IsMatch(sTemp.ToLower(), string.Format("^{0}$", sSkippedWordsPattern4Title)))
                    {
                        lstMatchedIndex.Add(i);
                        continue;

                    }

                    lstCleanedWords.Add(sTemp);
                    sCleanedTitle = string.Join(" ", lstCleanedWords.AsEnumerable().Reverse().ToList()).ToLower();
                    sCleanedTitle = Regex.Replace(sCleanedTitle, string.Format("[{0}{1}]", sCapsNonEnglishChar, sSmallNonEnglishChar), "_");

                    sTemp1 = Regex.Replace(sCleanedTitle, @"[\-]", "");
                    sTemp2 = Regex.Replace(sCleanedTitle, @"[\-]", " ");

                    sQuery = string.Format("select distinct(data1) from data_dict_journaltitle where data1 like '%{0}' or data1 like '%{1}'", sTemp1, sTemp2);
                    SqlCommand cmd = new SqlCommand();
                    Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                    dtResult = General.GeneralInstance.dataTable;

                    if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                    {
                        if (General.GeneralInstance.dataTable.Rows.Count == 1)
                        {
                            bIsExactMatchFound = true;
                        }
                        lstMatchedIndex.Add(i);
                        lstCleanedWordsExact.Add(sTemp);
                    }
                    else
                    {
                        if (lstMatchedIndex.Count > 0)
                        {
                        LBL_MATCH_START:
                            sCleanedTitle_ASCII = ConvertNonEnglishChar2ASCII(string.Join(" ", lstCleanedWordsExact.AsEnumerable().Reverse().ToList()).ToLower());
                            sCleanedTitle = RemoveNonEnglishChar(string.Join(" ", lstCleanedWordsExact.AsEnumerable().Reverse().ToList()).ToLower());

                            sQuery = string.Format("select distinct(data1) from data_dict_journaltitle where data1 = '{0}' or data1 = '{1}'", Regex.Replace(sCleanedTitle_ASCII, @"[\-]", ""), Regex.Replace(sCleanedTitle, @"[\-]", " "));
                            cmd = new SqlCommand();
                            Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                            dtResult = General.GeneralInstance.dataTable;

                            if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                            {
                                if (IsTitleSymptomsFound(lstTitleContentSplited, lstMatchedIndex))
                                {
                                    sTitleTaggedContent = DoTagging4IdentifiedTitle(lstTitleContentSplited, lstMatchedIndex);
                                    break;
                                }
                                else
                                {
                                    if (lstMatchedIndex.Count > 1)
                                    {
                                        lstCleanedWordsExact.RemoveAt(lstCleanedWordsExact.Count - 1);
                                        lstMatchedIndex.RemoveAt(lstMatchedIndex.Count - 1);
                                        goto LBL_MATCH_START;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (lstCleanedWordsExact.Count > 1 && lstMatchedIndex.Count > 1)
                                {
                                    lstCleanedWordsExact.RemoveAt(lstCleanedWordsExact.Count - 1);
                                    lstMatchedIndex.RemoveAt(lstMatchedIndex.Count - 1);
                                    goto LBL_MATCH_START;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleJournalArticleTitle", ex.Message, true, "");
            }
            return sTitleTaggedContent;
        }

        private string ValidateArticleTitle(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, @"((?<= (?:vs|wt))(\.)(?=[ %]))", "~~dot~~");


                sRefContent = Regex.Replace(sRefContent, @"((?<= )<i>(?:(?!(?:</?i>|</?Article_Title>|</?Journal_Title>)).)+)</Article_Title> <Journal_Title>((?:(?!</?i>).)+</i>)</Journal_Title>", "</Article_Title><Journal_Title>$1 $2</Journal_Title>");


                sRefContent = Regex.Replace(sRefContent, @"</Article_Title>([\:.;,]+)", "$1</Article_Title>");
                sRefContent = Regex.Replace(sRefContent, "<Article_Title>((?:(?!</?Article_Title>).)+)</Article_Title>", HandleValidateArticleTitle);

                sRefContent = Regex.Replace(sRefContent, "~~dot~~", ".");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ValidateArticleTitle", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string HandleValidateArticleTitle(Match myArticleTitMatch)
        {
            string sArticleTitle = myArticleTitMatch.Groups[1].Value.ToString();
            try
            {
                if (Regex.IsMatch(sArticleTitle, @"^(&#x201[Cc];)(.+)(&#x201[Dd];)[\:\.;, ]*$"))
                {
                    return myArticleTitMatch.Value.ToString();
                }

                if (Regex.IsMatch(sArticleTitle, @"^([\u201c])(.+)([\u201d])[\:\.;, ]*$"))
                {
                    return myArticleTitMatch.Value.ToString();
                }

                //added by Dakshinamoorthy on 2019-Oct-09
                if (Regex.IsMatch(sArticleTitle, @"^([\u2018])(.+)([\u2019])[\:\.;, ]*$"))
                {
                    return myArticleTitMatch.Value.ToString();
                }


                MatchCollection mcArt = Regex.Matches(sArticleTitle, @"\b(?:(?:[0-9]+)?[a-z]+(?:[0-9]+)?)(?:(?:[\.\?]?(?:&#x201[dD];|&#x2019;)[\.\?]?)|(?:[\.\?]))");
                if (mcArt != null && mcArt.Count > 0)
                {
                    Match myLastWordMatch = mcArt[mcArt.Count - 1];
                    if (myLastWordMatch != null)
                    {
                        int lastIndexArt = (myLastWordMatch.Index + myLastWordMatch.Length);
                        string sArtFirstPart = sArticleTitle.Substring(0, lastIndexArt);
                        string sArtSecondPart = sArticleTitle.Substring(lastIndexArt, (sArticleTitle.Length - lastIndexArt));
                        sArticleTitle = string.Format("<{0}>{1}</{0}>{2}", "Article_Title", sArtFirstPart, sArtSecondPart);
                    }
                    else
                    {
                        sArticleTitle = string.Format("<{0}>{1}</{0}>", "Article_Title", sArticleTitle);
                    }
                }
                else
                {
                    sArticleTitle = string.Format("<{0}>{1}</{0}>", "Article_Title", sArticleTitle);
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleValidateArticleTitle", ex.Message, true, "");
            }
            return sArticleTitle;
        }

        private string IdentifyMissingElements4JournalTitle(string sRefContentTagged)
        {
            try
            {
                if (Regex.IsMatch(sRefContentTagged, @"</Article_Title>[\:\.;, ]*(?:(?:<i>)?[A-Za-z\-0-9]+)[\:\.;, ]*<Journal_Title>"))
                {
                    sRefContentTagged = CheckNationality4JouTitle(sRefContentTagged);
                }
                else if (Regex.IsMatch(sRefContentTagged, @"<i>(?:(?!</i>).)+</i></Article_Title> <Journal_Title><i>"))
                {
                    sRefContentTagged = CheckNationality4JouTitle(sRefContentTagged);
                }
                else if (Regex.IsMatch(sRefContentTagged, @"(</Article_Title>(?:[\:\.;,]*)) (J\. (?:(?:[A-Z][a-z]{2,})\.? )+)<Journal_Title>"))
                {
                    sRefContentTagged = Regex.Replace(sRefContentTagged, @"(</Article_Title>(?:[\:\.;,]*)) (J\. (?:(?:[A-Z][a-z]{2,})\.? )+)<Journal_Title>", "$1 <Journal_Title>$2");
                }
                else if (Regex.IsMatch(sRefContentTagged, @"(?:</Article_Title> (International Journal of [A-Za-z ]+)<Journal_Title>)"))
                {
                    sRefContentTagged = Regex.Replace(sRefContentTagged, @"(?:</Article_Title> (International Journal of [A-Za-z ]+)<Journal_Title>)", "</Article_Title> <Journal_Title>$1");
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyMissingElements4JournalTitle", ex.Message, true, "");
            }
            return sRefContentTagged;
        }

        private string CheckNationality4JouTitle(string sRefTaggedContent)
        {
            try
            {
                sNationalityPattern = @"(?:\b(?:(?:Afghan)|(?:Albanian)|(?:Algerian)|(?:Andorran)|(?:Angolan)|(?:Argentinian)|(?:Armenian)|(?:Australian)|(?:Austrian)|(?:Azerbaijani)|(?:Bahamian)|(?:Bahraini)|(?:Bangladeshi)|(?:Barbadian)|(?:Belarusian|Belarusan)|(?:Belgian)|(?:Belizean)|(?:Beninese)|(?:Bhutanese)|(?:Bolivian)|(?:Bosnian)|(?:Botswanan)|(?:Brazilian)|(?:British)|(?:Bruneian)|(?:Bulgarian)|(?:Burkinese)|(?:Burmese)|(?:Burundian)|(?:Cambodian)|(?:Cameroonian)|(?:Canadian)|(?:Cape Verdean)|(?:Chadian)|(?:Chilean)|(?:Chinese)|(?:Colombian)|(?:Congolese)|(?:Costa Rican)|(?:Croat|Croatian)|(?:Cuban)|(?:Cypriot)|(?:Czech)|(?:Danish)|(?:Djiboutian)|(?:Dominican)|(?:Dominican)|(?:Ecuadorean)|(?:Egyptian)|(?:Salvadorean)|(?:English)|(?:Eritrean)|(?:Estonian)|(?:Ethiopian)|(?:Fijian)|(?:Finnish)|(?:French)|(?:Gabonese)|(?:Gambian)|(?:Georgian)|(?:German)|(?:Ghanaian)|(?:Greek)|(?:Grenadian)|(?:Guatemalan)|(?:Guinean)|(?:Guyanese)|(?:Haitian)|(?:Dutch)|(?:Honduran)|(?:Hungarian)|(?:Icelandic)|(?:Indian)|(?:Indonesian)|(?:Iranian)|(?:Iraqi)|(?:Irish)|(?:Italian)|(?:Jamaican)|(?:Japanese)|(?:Jordanian)|(?:Kazakh)|(?:Kenyan)|(?:Kuwaiti)|(?:Laotian)|(?:Latvian)|(?:Lebanese)|(?:Liberian)|(?:Libyan)|(?:Lithuanian)|(?:Macedonian)|(?:Malagasy|Madagascan)|(?:Malawian)|(?:Malaysian)|(?:Maldivian)|(?:Malian)|(?:Maltese)|(?:Mauritanian)|(?:Mauritian)|(?:Mexican)|(?:Moldovan)|(?:Monégasque|Monacan)|(?:Mongolian)|(?:Montenegrin)|(?:Moroccan)|(?:Mozambican)|(?:Namibian)|(?:Nepalese)|(?:Dutch)|(?:New Zealand)|(?:Nicaraguan)|(?:Nigerien)|(?:Nigerian)|(?:North Korean)|(?:Norwegian)|(?:Omani)|(?:Pakistani)|(?:Panamanian)|(?:Papua New Guinean|Guinean)|(?:Paraguayan)|(?:Peruvian)|(?:Philippine)|(?:Polish)|(?:Portuguese)|(?:Qatari)|(?:Romanian)|(?:Russian)|(?:Rwandan)|(?:Saudi Arabian|Saudi)|(?:Scottish)|(?:Senegalese)|(?:Serb|Serbian)|(?:Seychellois)|(?:Sierra Leonian)|(?:Singaporean)|(?:Slovak)|(?:Slovene|Slovenian)|(?:Somali)|(?:South African)|(?:South Korean)|(?:Spanish)|(?:Sri Lankan)|(?:Sudanese)|(?:Surinamese)|(?:Swazi)|(?:Swedish)|(?:Swiss)|(?:Syrian)|(?:Taiwanese)|(?:Tajik|Tadjik)|(?:Tanzanian)|(?:Thai)|(?:Togolese)|(?:Trinidadian)|(?:Tobagan)|(?:Tobagonian)|(?:Tunisian)|(?:Turkish)|(?:Turkmen|Turkoman)|(?:Tuvaluan)|(?:Ugandan)|(?:Ukrainian)|(?:UAE|Emirates|Emirati)|(?:UK|British)|(?:US)|(?:Uruguayan)|(?:Uzbek)|(?:Vanuatuan)|(?:Venezuelan)|(?:Vietnamese)|(?:Welsh)|(?:Western Samoan)|(?:Yemeni)|(?:Yugoslav)|(?:Zaïrean)|(?:Zambian)|(?:Zimbabwean)|(?:Asian)|(?:Australasian))\b)";

                if (Regex.IsMatch(sRefTaggedContent, string.Format(@"</Article_Title> ((?:<i>)?{0}[\:\.;, ]*)<Journal_Title>", sNationalityPattern)))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, string.Format(@"</Article_Title> ((?:<i>)?{0}[\:\.;, ]*)<Journal_Title>", sNationalityPattern), "</Article_Title> <Journal_Title>$1");
                }
                else if (Regex.IsMatch(sRefTaggedContent, string.Format(@"</Article_Title> ((?:<i>)?{0}(?:&#x2013|&#8211;|\-){0}[\:\.;, ]*)<Journal_Title>", sNationalityPattern)))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, string.Format(@"</Article_Title> ((?:<i>)?{0}(?:&#x2013|&#8211;|\-){0}[\:\.;, ]*)<Journal_Title>", sNationalityPattern), "</Article_Title> <Journal_Title>$1");
                }
                else if (Regex.IsMatch(sRefTaggedContent, string.Format(@"<i>({0}[\:\.;, ]*)</i></Article_Title> <Journal_Title><i>", sNationalityPattern)))
                {
                    sRefTaggedContent = Regex.Replace(sRefTaggedContent, string.Format(@"<i>({0}[\:\.;, ]*)</i></Article_Title> <Journal_Title><i>", sNationalityPattern), "</Article_Title> <Journal_Title><i>$1 ");
                }

                NormalizeSpaces(ref sRefTaggedContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckNationality4JouTitle", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }


        private string SplitArtJouTitleByLowerCaseWord(Match myTitleMatch)
        {
            string sOutputContent = myTitleMatch.Value.ToString();
            try
            {
                if (Regex.IsMatch(sOutputContent, "</?(?:Article_Title|Journal_Title)>"))
                {
                    return sOutputContent;
                }

                //Identify Article and Journal Title Using Pattern
                int nPatternId = 0;
                string sArticleJournalTitlePtn = string.Empty;
                string sDoubleQuotesTitle = @"(?:(?:&#x201[Cc];(?:(?!(?:&#x201[Cc];|&#x201[Dd];)).)+&#x201[Dd];)(?:[\:\.;, ]+|$))";
                string sItalicTitle = @"(?:(?:<i>(?:(?!</?i>).)+</i>)(?:[\:\.;, ]+|$))";

                nPatternId = 1;
                sArticleJournalTitlePtn = string.Format("^({0})({1})$", sDoubleQuotesTitle, sItalicTitle);
                if (Regex.IsMatch(sOutputContent, sArticleJournalTitlePtn))
                {
                    sOutputContent = Regex.Replace(sOutputContent, sArticleJournalTitlePtn, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", "$1", "Journal_Title", "$2"));
                    return sOutputContent;
                }





                //Split titles by  lower case with space 
                MatchCollection mcArt = Regex.Matches(sOutputContent, @"\b(?:(?:[0-9]+)?[a-z]+(?:[0-9]+)?)(?:(?:[\.\?]?(?:&#x201[dD];|&#x2019;)[\.\?]?)|(?:[\.\?]))");
                if (mcArt != null && mcArt.Count > 0)
                {
                    Match myLastWordMatch = mcArt[0];
                    if (myLastWordMatch != null)
                    {
                        int lastIndexArt = (myLastWordMatch.Index + myLastWordMatch.Length);
                        string sArtFirstPart = sOutputContent.Substring(0, lastIndexArt);
                        string sArtSecondPart = sOutputContent.Substring(lastIndexArt, (sOutputContent.Length - lastIndexArt));

                        if (IsThisJournalTitle(sArtSecondPart))
                        {
                            sOutputContent = string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "Article_Title", sArtFirstPart, "Journal_Title", sArtSecondPart);
                        }
                        else
                        {
                            sOutputContent = string.Format("<{0}>{1}</{0}>{2}", "Article_Title", sArtFirstPart, sArtSecondPart);
                        }
                    }
                    else
                    {
                        return sOutputContent;
                    }
                }
                else
                {
                    return sOutputContent;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\SplitArtJouTitleByLowerCaseWord", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private bool IsThisJournalTitle(string sJouTitle)
        {
            bool bReturnValue = false;

            try
            {
                sJouTitle = TitleContentCleanup(sJouTitle).Trim();
                string sCleanedTitle_ASCII = ConvertNonEnglishChar2ASCII(sJouTitle);
                string sCleanedTitle = RemoveNonEnglishChar(sJouTitle);
                string sQuery = string.Empty;
                DataTable dtResult = new DataTable();

                sQuery = string.Format("select distinct(data1) from data_dict_journaltitle where data1 = '{0}' or data1 = '{1}'", Regex.Replace(sCleanedTitle_ASCII, @"[\-]", ""), Regex.Replace(sCleanedTitle, @"[\-]", " "));
                SqlCommand cmd = new SqlCommand();
                Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                dtResult = General.GeneralInstance.dataTable;

                if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                {
                    bReturnValue = true;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsThisJournalTitle", ex.Message, true, "");

            }
            return bReturnValue;
        }

        private string ConvertNonEnglishChar2ASCII(string sContent)
        {
            try
            {
                //return String.Join("",
                //     sContent.Normalize(NormalizationForm.FormD)
                //    .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
                sContent = String.Join("",
                     sContent.Normalize(NormalizationForm.FormD)
                    .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ConvertNonEnglishChar2ASCII", ex.Message, true, "");

            }
            return sContent;
        }

        private string RemoveNonEnglishChar(string sContent)
        {
            try
            {
                sContent = Regex.Replace(sContent, string.Format("[{0}{1}]", sCapsNonEnglishChar, sSmallNonEnglishChar), "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveNonEnglishChar", ex.Message, true, "");

            }
            return sContent;
        }

        private string DoTagging4IdentifiedTitle(List<string> lstTitleContentSplited, List<int> lstMatchedIndex)
        {
            string sTitleContent = string.Empty;
            StringBuilder sbTitle = new StringBuilder();
            try
            {
                for (int i = 0; i <= lstTitleContentSplited.Count - 1; i++)
                {
                    if (lstMatchedIndex.Contains(i))
                    {
                        sbTitle.Append(string.Format("<jt>{0} </jt>", lstTitleContentSplited[i]));
                    }
                    else
                    {
                        sbTitle.Append(string.Format("{0} ", lstTitleContentSplited[i]));
                    }
                }

                sTitleContent = sbTitle.ToString();
                sTitleContent = Regex.Replace(sTitleContent, "</jt><jt>", "");
                sTitleContent = Regex.Replace(sTitleContent, "^((?:(?!</?jt>).){5,})(<jt>(?:(?!</?jt>).)+</jt>)$", "<at>$1</at>$2");
                sTitleContent = Regex.Replace(sTitleContent, "<at>", "<Article_Title>");
                sTitleContent = Regex.Replace(sTitleContent, "</at>", "</Article_Title>");
                sTitleContent = Regex.Replace(sTitleContent, "<jt>", "<Journal_Title>");
                sTitleContent = Regex.Replace(sTitleContent, "</jt>", "</Journal_Title>");

                sTitleContent = Regex.Replace(sTitleContent, @"([ \:\.;,]+)</Article_Title>", "</Article_Title>$1");
                sTitleContent = Regex.Replace(sTitleContent, @"([ \:\.;,]+)</Journal_Title>", "</Journal_Title>$1");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\DoTagging4IdentifiedTitle", ex.Message, true, "");

            }
            return sTitleContent;
        }

        private bool IsTitleSymptomsFound(List<string> lstTitleContentSplited, List<int> lstMatchedIndex)
        {
            bool bReturnValue = false;
            StringBuilder sbTitle = new StringBuilder();
            string sTitleContent = string.Empty;
            try
            {
                for (int i = lstMatchedIndex.Count - 1; i >= 0; i--)
                {
                    sbTitle.Append(string.Format("{0} ", lstTitleContentSplited[lstMatchedIndex[i]]));
                }
                sTitleContent = sbTitle.ToString().ToString().Trim();

                if (Regex.IsMatch(sTitleContent, @"^[ ]*(<i>(?:(?!</?i>).){5,}</i>[\:\.;, ]*)$"))
                {
                    return true;
                }
                else if (!Regex.IsMatch(Regex.Replace(sTitleContent, "</?[^<>]+>", ""), @"^[a-z]"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsTitleSymptomsFound", ex.Message, true, "");
            }
            return bReturnValue;
        }

        private string TitleContentCleanup(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "</?[^<>]+>", "");
                //sRefContent = Regex.Replace(sRefContent, @"[:,;\(\)\[\]\$=""\'\?\!\/\-\u201C\u201D\u2019\.]", "");
                sRefContent = Regex.Replace(sRefContent, @"[:,;\(\)\[\]\$=""\'\?\!\/\u201C\u201D\u2019\.]", "");
                //RefPreCleanup(ref sRefContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\TitleContentCleanup", ex.Message, true, "");

            }
            return sRefContent;
        }

        private string RefTaggedContentPreCleanup(ref string sRefTaggedContent)
        {
            try
            {
                //updated by Dakshinamoorthy on 2019-Sep-11
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:(?<=[ ](?:<[bi]>)?)\. \. \.(?=(?:</[bi]>)?[ ]))", "...");

                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"</([A-Za-z]+)><\1>", "");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "[\u00A0]", " ");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, ", &#x201D; ", ",&#x201D; ");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</i><i>.</i> <i>", ". ");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, ", <i> </i>", ", ");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</i> <i>", " ");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</Doi>.PMID", "</Doi>. PMID");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:<i>([\u201c])</i>(?! ?<i>))", "$1");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<i>(\.?[\u201d]) ", "$1 <i>");

                //added by Dakshinamoorthy on 2019-Jan-05
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"\b([a-z]?[0-9]+[a-z]?)[ ]?([\u2013]|\-|&#x2013;)[ ]?([a-z]?[0-9]+[a-z]?)\b", "$1$2$3");

                NormalizeSpaces(ref sRefTaggedContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RefTaggedContentPreCleanup", ex.Message, true, "");

            }
            return sRefTaggedContent;
        }

        //Generic tag conversion
        public string ConvertGenericRef(string sRefOriginalContent, string sRefTaggedContent, ref Dictionary<string, string> dicLogInfo, bool bProcessFromAPI = false)
        {
            try
            {
                //updated by Dakshinamoorthy on 2020-Apr-30
                if (bProcessFromAPI == false)
                {
                    RefPreCleanup(ref sRefOriginalContent);
                }
                RefTaggedContentPreCleanup(ref sRefTaggedContent);

                //sRefOriginalContent = IntroSpaceAtValidPosition(sRefOriginalContent);
                List<Tuple<string, string>> lstRefSplited = new List<Tuple<string, string>>();
                lstRefSplited = SmartSplitRefContent(sRefOriginalContent);
                string sRefGenericType = string.Empty;

                dtRefData = new DataTable();
                dtRefData.Columns.Add("Sno", typeof(int));
                dtRefData.Columns.Add("RefContent", typeof(string));
                dtRefData.Columns.Add("ElementType", typeof(string));
                dtRefData.Columns.Add("GroupId", typeof(string));
                dtRefData.Columns.Add("PuncGroupId", typeof(int));
                dtRefData.Columns.Add("SpaceType", typeof(string));

                for (int i = 0; i <= lstRefSplited.Count - 1; i++)
                {
                    dtRefData.Rows.Add(i, lstRefSplited[i].Item1.ToString(), "", "", 0, lstRefSplited[i].Item2.ToString());
                }

                //
                DataSet dsRefMain = new DataSet();

                DataTable dtRefImpElements = new DataTable("RefImpElements");
                dtRefImpElements.Columns.Add("RefSource", typeof(string));
                dtRefImpElements.Columns.Add("RefType", typeof(string));

                DataTable dtRefIdentifiedElements = new DataTable("RefIdentifiedElements");
                dtRefIdentifiedElements.Columns.Add("Sno", typeof(int));
                dtRefIdentifiedElements.Columns.Add("RefContent", typeof(string));
                dtRefIdentifiedElements.Columns.Add("RefContentCleaned", typeof(string));
                dtRefIdentifiedElements.Columns.Add("RefElementType", typeof(RefElements));
                dtRefIdentifiedElements.Columns.Add("RefElementGroupId", typeof(string));
                dtRefIdentifiedElements.Columns.Add("RefElementPriority", typeof(int));

                int macthPercentage = 0;
                StringBuilder sbReviewContent = new StringBuilder();
                bool isPubMedIndexAvailable = false;
                string sPubMedIndexAvailable = "No";
                string sPMIndex1Val = string.Empty;
                string sRefType_Identified = "Other";

                dsRefMain.Tables.Add(dtRefImpElements);
                dsRefMain.Tables.Add(dtRefIdentifiedElements);

                sRefTaggedContent = HandleMisingRefElements(sRefTaggedContent);
                sRefTaggedContent = GroupAuthorEditorDelimiterAnd(sRefTaggedContent);

                ClassifyRefElements4Match_LocalExe(ref dsRefMain, sRefTaggedContent);
                MatchRefElements(ref dtRefData, dsRefMain.Tables["RefIdentifiedElements"]);

                sRefTaggedContent = GetTaggedContent(ref dtRefData);
                //added by Dakshinamoorthy on 2020-Apr-13 to Handle Query
                //sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:</?LQ_[0-9A-Z]{6}>)", HandleQueryTag);
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<r_space/>", " ");

                NormalizeSpaces(ref sRefTaggedContent);

                //added by Dakshinamoorthy on 2019-May-11 (to avoid unicode semicolon consider as element delimiter)
                sRefTaggedContent = HexUnicodeToCharConvertor(sRefTaggedContent);

                sRefTaggedContent = HanldeAuthorDelimiter(sRefTaggedContent);
                sRefTaggedContent = HanldeEditorDelimiter(sRefTaggedContent);
                //added by Dakshinamoorthy on 2019-Feb-19
                sRefTaggedContent = HanldeGivenNameSurnameDelimiter(sRefTaggedContent);

                //added by Dakshinamoorthy on 2019-May-11
                sRefTaggedContent = DoHexaDecimalUnicodeConversion(sRefTaggedContent);

                sRefTaggedContent = RemoveGroupElements(sRefTaggedContent);
                sRefTaggedContent = CollabToAuthorInPrevoiusRef(sRefTaggedContent);

                //added by Dakshinamoorthy on 2020-Dec-18
                //TODO: Dakshina Check 
                //sRefTaggedContent = CombineSameRefElements(sRefTaggedContent);

                //reference type identification
                sRefType_Identified = IdentifyRefType(sRefTaggedContent);
                sRefTaggedContent = NormalizeElementsBasedOnRefType(sRefType_Identified, sRefTaggedContent);
                sRefGenericType = dsRefMain.Tables["RefImpElements"].Rows[0]["RefType"].ToString();
                sRefGenericType = (!sRefType_Identified.Equals("Other")) ? sRefType_Identified : sRefGenericType;
                sRefGenericType = GetGenericRefType_LocalExe(sRefGenericType);
                sRefTaggedContent = ProcessPageRange(sRefTaggedContent);
                sRefTaggedContent = HandleFormattingTags(sRefTaggedContent);

                //review content only for testing users
                macthPercentage = GetElementCapturedPercentage(dtRefData, ref dicLogInfo);

                //added by Dakshinamoorthy on 2020-Apr-30
                #region Ref API Integration Starts Here
                //string sUseAPI = File.ReadAllText(@"d:\Dakshina\Temp\API.txt").Trim();
                //if (sUseAPI.ToLower().Equals("y"))
                //{
                if (macthPercentage <= 79 && bProcessFromAPI == false)
                {
                    string sAPI_Name = "GROBID";
                    bool bIsAPI_WorkingFine = false;
                    string sAPI_ResultContent = string.Empty;
                    sAPI_ResultContent = DoAutoStructRefAPI(dtRefData, sRefOriginalContent, sAPI_Name, ref bIsAPI_WorkingFine);
                    if (bIsAPI_WorkingFine == true)
                    {
                        int actualMacthPercentage = macthPercentage;
                        sRefTaggedContent = sAPI_ResultContent;
                        macthPercentage = GetElementCapturedPercentage(dtRefData, ref dicLogInfo);

                        sRefType_Identified = IdentifyRefType(sRefTaggedContent);
                        sRefGenericType = GetGenericRefType_LocalExe(sRefType_Identified);

                        sRefType_Identified = string.Format("{0}-{1}-{2}-{3}", sRefType_Identified, sAPI_Name, actualMacthPercentage, macthPercentage);

                        if (!dicLogInfo.ContainsKey("RefTypeForAPI"))
                        {
                            dicLogInfo.Add("RefTypeForAPI", sRefType_Identified);
                        }
                    }
                }
                //}
                #endregion

                if (arrTestingUsers.Contains(System.Environment.UserName.ToLower()))
                {
                    //add review tag only for developers and tester
                    sbReviewContent.Append(string.Format("{0}%; ", macthPercentage.ToString()));
                    sPubMedIndexAvailable = IsPubMedIndexAvailable(sRefTaggedContent, ref sPMIndex1Val) ? "Yes" : "No";
                    sbReviewContent.Append(string.Format("PM Index: {0}, {1};", sPubMedIndexAvailable, sPMIndex1Val));
                    sRefTaggedContent = string.Format("{0}<ldlTestingLog>[{1}]</ldlTestingLog>", sRefTaggedContent, sbReviewContent.ToString());
                }

                //identify reference type
                sRefTaggedContent = string.Format("<{0}>{1}</{0}>", sRefGenericType, sRefTaggedContent);
                sRefTaggedContent = HandlePunctuationsInTaggedRef(sRefTaggedContent);
                NormalizeSpaces(ref sRefTaggedContent);
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"</([a-z]+)><\1>", "", RegexOptions.IgnoreCase);

                //insert log information
                //updated by Dakshinamoorthy on 2019-Jan-29
                if (!dicLogInfo.ContainsKey("RefType"))
                {
                    dicLogInfo.Add("RefType", sRefGenericType);
                }

                if (dicLogInfo.ContainsKey("MatchedPercentage"))
                {
                    string sMatchedPercentage_Already = dicLogInfo["MatchedPercentage"].ToString();
                    dicLogInfo["MatchedPercentage"] = ((Convert.ToInt32(sMatchedPercentage_Already) + Convert.ToInt32(macthPercentage)) / nSubRefCount).ToString();
                }
                else
                {
                    dicLogInfo.Add("MatchedPercentage", macthPercentage.ToString());
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ConvertGenericRef", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        private string CombineSameRefElements(string sRefTaggedContent)
        {
            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:</(ldl(?:[A-Za-z]+))> <\1>)", " ");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CombineSameRefElements", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }


        //added by Dakshinamoorthy ono 2020-May-04
        private string DoAutoStructRefAPI(DataTable dtRefData_API, string sRefOriginalContent, string sAPI_Name, ref bool bIsAPI_WorkingFine)
        {
            string sTaggedContent_API = string.Empty;

            try
            {
                if (sAPI_Name.ToLower().Equals("anystyle"))
                {


                    if (AutoStructRefAPI.IdentifyRefElements(sRefOriginalContent, ref sTaggedContent_API) == true)
                    {
                        bIsAPI_WorkingFine = true;
                    }
                    else
                    {
                        bIsAPI_WorkingFine = false;
                        return string.Empty;
                    }
                }
                else if (sAPI_Name.ToLower().Equals("grobid"))
                {
                    sRefOriginalContent = Regex.Replace(sRefOriginalContent, "<split/>", " ");
                    IdentifyRefLabel(ref sRefOriginalContent);
                    IdentifyRefPrefix(ref sRefOriginalContent);
                    sRefOriginalContent = Regex.Replace(sRefOriginalContent, "(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)[ ]*)", "");
                    sRefOriginalContent = Regex.Replace(sRefOriginalContent, "(?:(?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>)[ ]*)", "");
                    sRefOriginalContent = sRefOriginalContent.Trim();

                    if (AutoStructRefAPI.IdentifyRefElements_GROBID(sRefOriginalContent, ref sTaggedContent_API) == true)
                    {
                        bIsAPI_WorkingFine = true;
                    }
                    else
                    {
                        bIsAPI_WorkingFine = false;
                        return string.Empty;
                    }
                }

                //API Tagged Content Processing Starts Here
                sTaggedContent_API = HexUnicodeToCharConvertor(sTaggedContent_API);
                NormalizeSpaces(ref sTaggedContent_API);
                for (int i = 0; i <= dtRefData_API.Rows.Count - 1; i++)
                {
                    if (Enum.IsDefined(typeof(RefSkipElements_GROBID), dtRefData_API.Rows[i]["ElementType"].ToString()) == false)
                    {
                        dtRefData_API.Rows[i]["ElementType"] = "";
                        dtRefData_API.Rows[i]["GroupId"] = "";
                    }
                }

                //---------

                DataSet dsRefMain_API = new DataSet();

                DataTable dtRefImpElements_API = new DataTable("RefImpElements");
                dtRefImpElements_API.Columns.Add("RefSource", typeof(string));
                dtRefImpElements_API.Columns.Add("RefType", typeof(string));

                DataTable dtRefIdentifiedElements_API = new DataTable("RefIdentifiedElements");
                dtRefIdentifiedElements_API.Columns.Add("Sno", typeof(int));
                dtRefIdentifiedElements_API.Columns.Add("RefContent", typeof(string));
                dtRefIdentifiedElements_API.Columns.Add("RefContentCleaned", typeof(string));
                dtRefIdentifiedElements_API.Columns.Add("RefElementType", typeof(RefElements));
                dtRefIdentifiedElements_API.Columns.Add("RefElementGroupId", typeof(string));
                dtRefIdentifiedElements_API.Columns.Add("RefElementPriority", typeof(int));

                dsRefMain_API.Tables.Add(dtRefImpElements_API);
                dsRefMain_API.Tables.Add(dtRefIdentifiedElements_API);

                sTaggedContent_API = HandleMisingRefElements(sTaggedContent_API);
                sTaggedContent_API = GroupAuthorEditorDelimiterAnd(sTaggedContent_API);

                ClassifyRefElements4Match_LocalExe(ref dsRefMain_API, sTaggedContent_API);
                MatchRefElements(ref dtRefData, dsRefMain_API.Tables["RefIdentifiedElements"]);

                sTaggedContent_API = GetTaggedContent(ref dtRefData);

                NormalizeSpaces(ref sTaggedContent_API);

                //added by Dakshinamoorthy on 2019-May-11 (to avoid unicode semicolon consider as element delimiter)
                sTaggedContent_API = HexUnicodeToCharConvertor(sTaggedContent_API);

                sTaggedContent_API = IdentifyMissingLabelInfo(sTaggedContent_API);

                sTaggedContent_API = HanldeAuthorDelimiter(sTaggedContent_API);
                sTaggedContent_API = HanldeEditorDelimiter(sTaggedContent_API);
                //added by Dakshinamoorthy on 2019-Feb-19
                sTaggedContent_API = HanldeGivenNameSurnameDelimiter(sTaggedContent_API);

                //added by Dakshinamoorthy on 2019-May-11
                sTaggedContent_API = DoHexaDecimalUnicodeConversion(sTaggedContent_API);

                sTaggedContent_API = RemoveGroupElements(sTaggedContent_API);
                sTaggedContent_API = CollabToAuthorInPrevoiusRef(sTaggedContent_API);

                //reference type identification
                string sRefType_Identified = IdentifyRefType(sTaggedContent_API);

                string sRefTaggedContent = NormalizeElementsBasedOnRefType(sRefType_Identified, sTaggedContent_API);
                string sRefGenericType = dsRefMain_API.Tables["RefImpElements"].Rows[0]["RefType"].ToString();
                sRefGenericType = (!sRefType_Identified.Equals("Other")) ? sRefType_Identified : sRefGenericType;
                sRefGenericType = GetGenericRefType_LocalExe(sRefGenericType);
                sTaggedContent_API = ProcessPageRange(sTaggedContent_API);
                sTaggedContent_API = HandleFormattingTags(sTaggedContent_API);

                //review content only for testing users
                //int macthPercentage = GetElementCapturedPercentage(dtRefData, ref dicLogInfo);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\DoAutoStructRefAPI", ex.Message, true, "");
            }
            return sTaggedContent_API;
        }

        //added by Dakshinamoorthy on 2020-Dec-16
        private string IdentifyMissingLabelInfo(string sRefTaggedContent)
        {
            try
            {
                //Page number label
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:<ldlUnknownElement>((?:\b(?:[Pp]p?|(?:[Pp]\.[ ]?[Pp]\.[ ]?)|[Pp]ages?)[\:\. ]+))</ldlUnknownElement>( <ldlPageRange>(?:(?!</?ldlPageRange>).)+</ldlPageRange>))", string.Format("<{0}>{1}</{0}>{2}", "ldlPageLabel", "$1", "$2"));
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyMissingLabelInfo", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        private string GroupAuthorEditorDelimiterAnd(string sRefTaggedContent)
        {
            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"</Editor>( <ldlEditorDelimiterAnd>(?:(?!</?ldlEditorDelimiterAnd>).)+</ldlEditorDelimiterAnd>)", "$1</Editor>");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"</Author>( <ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>)", "$1</Author>");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\GroupAuthorEditorDelimiterAnd", ex.Message, true, "");

            }
            return sRefTaggedContent;
        }

        private string HanldeAuthorDelimiter(string sRefTaggedContent)
        {
            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(aug_[0-9]+)>((?:(?!</?\1>).)+)</\1>", InsertAuthorDelimiterTag);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HanldeAuthorDelimiter", ex.Message, true, "");

            }
            return sRefTaggedContent;
        }

        private string InsertAuthorDelimiterTag(Match myAuDelimiter)
        {
            string sAugTag = myAuDelimiter.Groups[1].Value.ToString();
            string sAuContent = myAuDelimiter.Groups[2].Value.ToString();
            string sOutputContent = myAuDelimiter.Value.ToString();

            try
            {
                if (Regex.IsMatch(sAuContent, "([,;])</b>(</(?:ldlAuthorGivenName|ldlAuthorSurName|ldlAuthorSuffix)>)$"))
                {
                    sAuContent = Regex.Replace(sAuContent, "([,;])</b>(</(?:ldlAuthorGivenName|ldlAuthorSurName|ldlAuthorSuffix)>)$", "</b>$2<b>$1</b>");
                }
                else if (Regex.IsMatch(sAuContent, "([,;])(</(?:ldlAuthorGivenName|ldlAuthorSurName|ldlAuthorSuffix)>)$"))
                {
                    sAuContent = Regex.Replace(sAuContent, "([,;])(</(?:ldlAuthorGivenName|ldlAuthorSurName|ldlAuthorSuffix)>)$", "$2$1");
                }
                else if (Regex.IsMatch(sAuContent, "([,;])(</(?:ldlAuthorGivenName|ldlAuthorSurName|ldlAuthorSuffix)>)( <ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>)$"))
                {
                    sAuContent = Regex.Replace(sAuContent, "([,;])(</(?:ldlAuthorGivenName|ldlAuthorSurName|ldlAuthorSuffix)>)( <ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>)$", "$2$1$3");
                }
                //added by Dakshinamoorthy on 2020-Dec-15
                else if (Regex.IsMatch(sAuContent, @"(?:([\.])([\.]+)(</(?:ldlAuthorGivenName|ldlAuthorSurName|ldlAuthorSuffix)>)$)"))
                {
                    sAuContent = Regex.Replace(sAuContent, @"(?:([\.])([\.]+)(</(?:ldlAuthorGivenName|ldlAuthorSurName|ldlAuthorSuffix)>)$)", "$1$3$2");
                }


                sAuContent = Regex.Replace(sAuContent, "(</(?:ldlAuthorSurName|ldlAuthorGivenName|ldlAuthorSuffix)>) (<(?:ldlAuthorSurName|ldlAuthorGivenName|ldlAuthorSuffix)>)",
                    string.Format("{0}<{2}> </{2}>{1}", "$1", "$2", "ldlAuthorDelimiterChar"));
                sOutputContent = string.Format("<{0}>{1}</{0}>", sAugTag, sAuContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\InsertAuthorDelimiterTag", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private string HanldeEditorDelimiter(string sRefTaggedContent)
        {
            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(edg_[0-9]+)>((?:(?!</?\1>).)+)</\1>", InsertEditorDelimiterTag);
                //sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(</edg_[0-9]+>) <ldlUnknownElement>((?:and|&))</ldlUnknownElement>", string.Format(" <{0}>{1}</{0}>{2}", "ldlEditorDelimiterAnd", "$2", "$1"));
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HanldeEditorDelimiter", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }


        private string HanldeGivenNameSurnameDelimiter(string sRefTaggedContent)
        {
            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<ldlAuthorSurName>((?:(?!</?ldlAuthorSurName>).)+)([\.\:;,]+)</ldlAuthorSurName></aug_([0-9]+)>",
                    "<ldlAuthorSurName>$1</ldlAuthorSurName>$2</aug_$3>");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<ldlEditorSurName>((?:(?!</?ldlEditorSurName>).)+)([\.\:;,]+)</ldlEditorSurName></edg_([0-9]+)>",
                    "<ldlEditorSurName>$1</ldlEditorSurName>$2</edg_$3>");

                sRefTaggedContent = Regex.Replace(sRefTaggedContent,
                    string.Format(@"<ldlAuthorGivenName>(?:((?:(?:[A-Z{0}]\.? ?)+ )?(?:(?:[A-Z{0}][a-z{1}]+)))([\.\:;,]+))</ldlAuthorGivenName></aug_([0-9]+)>", sCapsNonEnglishChar, sSmallNonEnglishChar),
                    "<ldlAuthorGivenName>$1</ldlAuthorGivenName>$2</aug_$3>");

                sRefTaggedContent = Regex.Replace(sRefTaggedContent,
                    string.Format(@"<ldlEditorGivenName>(?:((?:(?:[A-Z{0}]\.? ?)+ )?(?:(?:[A-Z{0}][a-z{1}]+)))([\.\:;,]+))</ldlEditorGivenName></aug_([0-9]+)>", sCapsNonEnglishChar, sSmallNonEnglishChar),
                    "<ldlEditorGivenName>$1</ldlEditorGivenName>$2</aug_$3>");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HanldeEditorDelimiter", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }


        private string InsertEditorDelimiterTag(Match myEdDelimiter)
        {
            string sEdgTag = myEdDelimiter.Groups[1].Value.ToString();
            string sEdContent = myEdDelimiter.Groups[2].Value.ToString();
            string sOutputContent = myEdDelimiter.Value.ToString();

            try
            {
                if (Regex.IsMatch(sEdContent, "([,;])</b>(</(?:ldlEditorGivenName|ldlEditorSurName|ldlEditorSuffix)>)$"))
                {
                    sEdContent = Regex.Replace(sEdContent, "([,;])</b>(</(?:ldlEditorGivenName|ldlEditorSurName|ldlEditorSuffix)>)$", "</b>$2<b>$1</b>");
                }
                else if (Regex.IsMatch(sEdContent, "([,;])(</(?:ldlEditorGivenName|ldlEditorSurName|ldlEditorSuffix)>)$"))
                {
                    sEdContent = Regex.Replace(sEdContent, "([,;])(</(?:ldlEditorGivenName|ldlEditorSurName|ldlEditorSuffix)>)$", "$2$1");
                }

                sEdContent = Regex.Replace(sEdContent, "(</(?:ldlEditorSurName|ldlEditorGivenName|ldlEditorSuffix)>) (<(?:ldlEditorSurName|ldlEditorGivenName|ldlEditorSuffix)>)",
                    string.Format("{0}<{2}> </{2}>{1}", "$1", "$2", "ldlEditorDelimiterChar"));
                sOutputContent = string.Format("<{0}>{1}</{0}>", sEdgTag, sEdContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\InsertEditorDelimiterTag", ex.Message, true, "");
            }
            return sOutputContent;
        }



        private string HandleMisingRefElements(string sRefTaggedContent)
        {
            try
            {
                //added by Dakshinamoorthy on 2019-Sep-11
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?<=(?:</(?:Forename|Surname|Author|Suffix|Etal)>[\.,;]*))((?: (?:and |& |[\u2016] |\.\.\. )+))(?=<(?:Forename|Surname|Author|Suffix|Etal)>)", string.Format("<{0}>{1}</{0}>", "ldlAuthorDelimiterAnd", "$1"));
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?<=(?:</(?:EForename|ESurname|Editor|ESuffix|Etal)>[\.,;]*))((?: (?:and |& |[\u2016] |\.\.\. )+))(?=<(?:EForename|ESurname|Editor|ESuffix|Etal)>)", string.Format("<{0}>{1}</{0}>", "ldlEditorDelimiterAnd", "$1"));
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?<=(?:</(?:Forename|Surname|Author|Suffix|Etal)>[\.,; ]+))(and|&|[\u2026])(?=[ ]+<(?:Forename|Surname|Author|Suffix|Etal)>)", string.Format("<{0}>{1}</{0}>", "ldlAuthorDelimiterAnd", "$1"));
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?<=(?:</(?:EForename|ESurname|Editor|ESuffix|Etal)>[\.,; ]+))(and|&|[\u2026])(?=[ ]+<(?:EForename|ESurname|Editor|ESuffix|Etal)>)", string.Format("<{0}>{1}</{0}>", "ldlEditorDelimiterAnd", "$1"));
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(\b(?:[Dd][Oo][Ii])[\: ]*(?=<Doi>))", string.Format("<{0}>{1}</{0}>", "ldlDOILabel", "$1"));
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleMisingRefElements", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        private int GetElementCapturedPercentage(DataTable dtRefData, ref Dictionary<string, string> dicLogInfo)
        {
            double percentage = 0;
            try
            {
                int emptyRowCount = dtRefData.AsEnumerable().Where(c => c.Field<string>("ElementType") == "").Count();
                int matchedRowCount = dtRefData.Rows.Count - emptyRowCount;
                percentage = ((double)matchedRowCount / (double)dtRefData.Rows.Count) * 100;

                //updated by Dakshinamoorthy on 2019-Jan-29
                if (dicLogInfo.ContainsKey("TotalWords"))
                {
                    string sTotalWords_Already = dicLogInfo["TotalWords"].ToString();
                    dicLogInfo["TotalWords"] = (Convert.ToInt32(sTotalWords_Already) + dtRefData.Rows.Count).ToString();
                }
                else
                {
                    dicLogInfo.Add("TotalWords", dtRefData.Rows.Count.ToString());
                }

                if (dicLogInfo.ContainsKey("MatchedWords"))
                {
                    string sMatchedWords_Already = dicLogInfo["MatchedWords"].ToString();
                    dicLogInfo["MatchedWords"] = (Convert.ToInt32(sMatchedWords_Already) + matchedRowCount).ToString();
                }
                else
                {
                    dicLogInfo.Add("MatchedWords", matchedRowCount.ToString());
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\GetElementCapturedPercentage", ex.Message, true, "");
            }
            return Convert.ToInt32(percentage);
        }

        private bool IsPubMedIndexAvailable(string sRefTaggedContent, ref string sIndex1Vaue)
        {
            bool bReturnValue = false;

            string key1FirstAuSurname = string.Empty;
            string key2PubYear = string.Empty;
            string key3FirstPage = string.Empty;
            string key4ArticleTitle = string.Empty;

            string index1Value = string.Empty;
            string sQuery = string.Empty;

            try
            {
                //first author surname
                MatchCollection mcAuSurname = Regex.Matches(sRefTaggedContent, "<ldlAuthorSurName>((?:(?!</?ldlAuthorSurName>).)+)</ldlAuthorSurName>");
                if (mcAuSurname != null && mcAuSurname.Count > 0)
                {
                    key1FirstAuSurname = mcAuSurname[0].Groups[1].Value.ToString();
                    key1FirstAuSurname = TitleContentCleanup(key1FirstAuSurname);
                    key1FirstAuSurname = Regex.Replace(key1FirstAuSurname, @"[\-]", "");
                    key1FirstAuSurname = ConvertNonEnglishChar2ASCII(key1FirstAuSurname);
                    key1FirstAuSurname = Regex.Match(key1FirstAuSurname, "^[a-z]+", RegexOptions.IgnoreCase).Value.ToString();
                }

                //publication year
                MatchCollection mcYear = Regex.Matches(sRefTaggedContent, "<ldlPublicationYear>((?:(?!</?ldlPublicationYear>).)+)</ldlPublicationYear>");
                if (mcYear != null && mcYear.Count > 0)
                {
                    key2PubYear = mcYear[0].Groups[1].Value.ToString();
                    key2PubYear = TitleContentCleanup(key2PubYear);
                    key2PubYear = Regex.Replace(key2PubYear, @"[\-]", "");
                    key2PubYear = Regex.Match(key2PubYear, "^[0-9]{4}", RegexOptions.IgnoreCase).Value.ToString();
                }

                //first page number
                MatchCollection mcFirstPage = Regex.Matches(sRefTaggedContent, "<ldlFirstPageNumber>((?:(?!</?ldlFirstPageNumber>).)+)</ldlFirstPageNumber>");
                if (mcFirstPage != null && mcFirstPage.Count > 0)
                {
                    key3FirstPage = mcFirstPage[0].Groups[1].Value.ToString();
                    key3FirstPage = TitleContentCleanup(key3FirstPage);
                    key3FirstPage = Regex.Replace(key3FirstPage, @"[\-]", "");
                    key3FirstPage = Regex.Match(key3FirstPage, "[0-9]+", RegexOptions.IgnoreCase).Value.ToString();
                }

                //article title
                MatchCollection mcArticleTitle = Regex.Matches(sRefTaggedContent, "<ldlArticleTitle>((?:(?!</?ldlArticleTitle>).)+)</ldlArticleTitle>");
                if (mcArticleTitle != null && mcArticleTitle.Count > 0)
                {
                    key4ArticleTitle = mcArticleTitle[0].Groups[1].Value.ToString();
                    key4ArticleTitle = Regex.Replace(key4ArticleTitle, string.Format(@"\b(?:{0})\b", sSkippedWordsPattern4Title), "", RegexOptions.IgnoreCase);
                    NormalizeSpaces(ref key4ArticleTitle);
                }

                index1Value = string.Format("{0}_{1}_{2}", key1FirstAuSurname, key2PubYear, key3FirstPage);
                index1Value = index1Value.ToLower();
                sIndex1Vaue = index1Value;

                if (!string.IsNullOrEmpty(Regex.Replace(index1Value, @"[_]", "")))
                {
                    sQuery = string.Format("select * from data_dict_reference where index1 = '{0}'", index1Value);
                    SqlCommand cmd = new SqlCommand();
                    Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                    DataTable dtResult = new DataTable();
                    dtResult = General.GeneralInstance.dataTable;

                    if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                    {
                        bReturnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsPubMedIndexAvailable", ex.Message, true, "");
            }
            return bReturnValue;
        }

        private string GetGenericRefType_LocalExe(string sRefType)
        {
            string sRefGenericType = string.Empty;
            try
            {
                switch (sRefType)
                {
                    case "Journal":
                        sRefGenericType = "ldlRefTypeJournal";
                        break;
                    case "Book":
                        sRefGenericType = "ldlRefTypeBook";
                        break;
                    case "Conference":
                        sRefGenericType = "ldlRefTypeConference";
                        break;
                    case "Web":
                        sRefGenericType = "ldlRefTypeWeb";
                        break;
                    case "Report":
                        sRefGenericType = "ldlRefTypeReport";
                        break;
                    case "Thesis":
                        sRefGenericType = "ldlRefTypeThesis";
                        break;
                    case "Paper":
                        sRefGenericType = "ldlRefTypePaper"; //added by Dakshinamoorthy on 2020-Jul-08
                        break;
                    case "References":
                    default:
                        sRefGenericType = "ldlRefTypeOther";
                        break;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\GetGenericRefType_LocalExe", ex.Message, true, "");
            }
            return sRefGenericType;
        }

        private string ProcessPageRange(string sRefContent)
        {
            try
            {
                if (!Regex.IsMatch(sRefContent, "<ldlPageRange>((?:(?!</?ldlPageRange>).)+)</ldlPageRange>"))
                {
                    return sRefContent;
                }
                sRefContent = Regex.Replace(sRefContent, "<ldlPageRange>((?:(?!</?ldlPageRange>).)+)</ldlPageRange>", SplitPage);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ProcessPageRange", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string SplitPage(Match myPageMatch)
        {
            string sSplitedPage = myPageMatch.Groups[1].Value.ToString();
            try
            {
                //updated by Dakshinamoorthy on 2019-Jan-11
                //"\." in page range 
                if (Regex.IsMatch(sSplitedPage, @"([0-9a-zA-Z\.]+)([ ]?(?:&#8211;|&#x2013;|&#x00AD;|&#x2010;|&#x2012;|\-|\u2013|\u2012)+[ ]?)([0-9a-zA-Z\.]+)"))
                {
                    sSplitedPage = Regex.Replace(sSplitedPage, @"([0-9a-zA-Z\.]+)([ ]?(?:&#8211;|&#x2013;|&#x00AD;|&#x2010;|&#x2012;|\-|\u2013|\u2012)+[ ]?)([0-9a-zA-Z\.]+)", "<ldlFirstPageNumber>$1</ldlFirstPageNumber>$2<ldlLastPageNumber>$3</ldlLastPageNumber>");
                }
                else
                {
                    sSplitedPage = string.Format("<{0}>{1}</{0}>", "ldlFirstPageNumber", sSplitedPage);
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\SplitPage", ex.Message, true, "");
            }
            return sSplitedPage;
        }

        //added by Dakshinamoorthy on 2019-Sep-28
        private string CollabToAuthorInPrevoiusRef(string sRefTaggedContent)
        {
            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"(?:<ldlCollab>(?:(?!</?ldlCollab>)(?:(?:&#x2014;)[ ]?))+</ldlCollab>)", HandleCollabToAuthorInPrevoiusRef);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CollabToAuthorInPrevoiusRef", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }


        private string HandleCollabToAuthorInPrevoiusRef(Match myCollabMatch)
        {
            string sOutputContent = myCollabMatch.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "</?ldlCollab>", "");
                sOutputContent = string.Format("<{0}>{1}</{0}>", "ldlAuthorInPrevoiusRef", sOutputContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleCollabToAuthorInPrevoiusRef", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private string RemoveGroupElements(string sRefTaggedContent)
        {
            try
            {
                //for RefBOT API
                #region MyRegion
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(<aug_[0-9]+>)", "<ldlAuthor>");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(</aug_[0-9]+>)", "</ldlAuthor>");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(<edg_[0-9]+>)", "<ldlEditor>");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(</edg_[0-9]+>)", "</ldlEditor>");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:<ldlEditorDelimiterEds_(?:Back|Front)>)", "<ldlEditorDelimiterEds>");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?:</ldlEditorDelimiterEds_(?:Back|Front)>)", "</ldlEditorDelimiterEds>");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(</?dateg_[0-9]+>)", "");
                #endregion

                //for eBOT
                #region MyRegion
                //sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(</?aug_[0-9]+>)", "");
                //sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(</?dateg_[0-9]+>)", "");
                #endregion
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveGroupElements", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        private string GetTaggedContent(ref DataTable dtRefData)
        {
            string sTaggedContent = string.Empty;
            StringBuilder sbTaggedContent = new StringBuilder();
            try
            {
                string sRefWord = string.Empty;
                string sRefType = string.Empty;
                string sSpaceType = string.Empty;
                string sGroupId = string.Empty;

                for (int i = 0; i <= dtRefData.Rows.Count - 1; i++)
                {
                    sRefWord = dtRefData.Rows[i]["RefContent"].ToString();
                    sRefType = dtRefData.Rows[i]["ElementType"].ToString();
                    sSpaceType = dtRefData.Rows[i]["SpaceType"].ToString();
                    sGroupId = dtRefData.Rows[i]["GroupId"].ToString();

                    if (string.IsNullOrEmpty(sRefType))
                    {
                        RefElements eleRef = (RefElements)int.Parse("0");
                        sRefType = Enum.GetName(typeof(RefElements), eleRef);
                    }

                    if (sSpaceType.Equals("wos"))
                    {
                        if (string.IsNullOrEmpty(sGroupId))
                        {
                            sbTaggedContent.Append(string.Format("<{0}>{1}</{0}>", sRefType, sRefWord));
                        }
                        else
                        {
                            sbTaggedContent.Append(string.Format("<{2}><{0}>{1}</{0}></{2}>", sRefType, sRefWord, sGroupId));
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(sGroupId))
                        {
                            sbTaggedContent.Append(string.Format("<{0}>{1} </{0}>", sRefType, sRefWord));
                        }
                        else
                        {
                            sbTaggedContent.Append(string.Format("<{2}><{0}>{1} </{0}></{2}>", sRefType, sRefWord, sGroupId));
                        }
                    }
                }

                sTaggedContent = sbTaggedContent.ToString();

                do
                {
                    sTaggedContent = Regex.Replace(sTaggedContent, @"</([^<> ]+)><\1>", "");
                } while (Regex.IsMatch(sTaggedContent, @"</([^<> ]+)><\1>"));



                sTaggedContent = Regex.Replace(sTaggedContent, "[ ]+</([^<> ]+)>", "</$1> ").Trim();

            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\GetTaggedContent", ex.Message, true, "");
            }
            return sTaggedContent;
        }

        private bool ClassifyRefElements4Match_LocalExe(ref DataSet dsRefClassfied, string sRefTaggedContent)
        {
            string sRefTypeTemp = string.Empty;
            string sRefLabel = string.Empty;
            string sRefInnerXml = string.Empty;

            try
            {
                if (dsRefClassfied.Tables.Count < 2)
                {
                    return false;
                }

                //remove formatting tags while matching
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</?(?:b|i|u|sup|sub|sc|ac)>", "");
                sRefTaggedContent = DoHexaDecimalUnicodeConversion(sRefTaggedContent);

                //sRefTaggedContent = ConvertDecimalToHexDecimal(sRefTaggedContent);

                //get reference type
                if (Regex.IsMatch(sRefTaggedContent, string.Format("^(?:<{0}>(?:(?!</?{0}>).)+</{0}>)$", sRefTypeLocalExePtn)))
                {
                    sRefTypeTemp = Regex.Match(sRefTaggedContent, string.Format("^(?:<({0})>(?:(?!</?{0}>).)+</{0}>)$", sRefTypeLocalExePtn)).Groups[1].Value.ToString();
                    sRefInnerXml = Regex.Match(sRefTaggedContent, string.Format("^(?:<(?:{0})>((?:(?!</?{0}>).)+)</{0}>)$", sRefTypeLocalExePtn)).Groups[1].Value.ToString();
                    dsRefClassfied.Tables["RefImpElements"].Rows.Add("LocalExe", sRefTypeTemp);
                }

                //get reference label
                if (Regex.IsMatch(sRefInnerXml, string.Format("^<Label>((?:(?!</?Label>).)+)</Label>", sRefTypeLocalExePtn)))
                {
                    sRefLabel = Regex.Match(sRefInnerXml, string.Format("^<Label>((?:(?!</?Label>).)+)</?Label>", sRefTypeLocalExePtn)).Groups[1].Value.ToString();
                    sRefInnerXml = Regex.Replace(sRefInnerXml, "^<Label>((?:(?!</?Label>).)+)</Label>", "");
                    dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefLabel, Cleanup4MatchingContent(sRefLabel), (int)RefElements.ldlRefLabel, "", RefElements.ldlRefLabel);
                }

                //get reference ref label
                if (Regex.IsMatch(sRefInnerXml, string.Format("^<RefLabel>((?:(?!</?RefLabel>).)+)</RefLabel>", sRefTypeLocalExePtn)))
                {
                    sRefLabel = Regex.Match(sRefInnerXml, string.Format("^<RefLabel>((?:(?!</?RefLabel>).)+)</?RefLabel>", sRefTypeLocalExePtn)).Groups[1].Value.ToString();
                    //TODO: RefPrefix Check (2020-May-19)
                    sRefInnerXml = Regex.Replace(sRefInnerXml, "^<RefLabel>((?:(?!</?RefLabel>).)+)</RefLabel>", "");
                    dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefLabel, Cleanup4MatchingContent(sRefLabel), (int)RefElements.ldlRefLabel, "", RefElements.ldlRefLabel);
                }

                //added by Dakshinamoorthy on 2020-Jun-02
                if (string.IsNullOrEmpty(sRefPrefixContent) == false)
                {
                    sRefPrefixContent = Regex.Replace(sRefPrefixContent, "</?(?:b|i|u|sup|sub|sc|ac)>", "");
                    sRefPrefixContent = DoHexaDecimalUnicodeConversion(sRefPrefixContent);
                    dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefPrefixContent, Cleanup4MatchingContent(sRefPrefixContent), (int)RefElements.ldlRefPrefix, "", RefElements.ldlRefPrefix);
                }


                sRefInnerXml = Regex.Replace(sRefInnerXml, "<FirstPage>((?:(?!</?FirstPage>).)+)</FirstPage>([ ]?(?:&#8211;|&#x2013;)[ ]?)<LastPage>((?:(?!</?LastPage>).)+)</LastPage>", "<PageRange>$1$2$3</PageRange>");

                //reference clasification started
                string sRefEleTypeTemp = string.Empty;
                int auCount = 0;
                int edCount = 0;
                int dateCount = 0;
                string auGroupName = string.Empty;
                string edGroupName = string.Empty;
                string dateGroupName = string.Empty;

                if (Regex.IsMatch(sRefInnerXml, @"<([^<>]+)>((?:(?!</?\1>).)+)</\1>"))
                {
                    MatchCollection mcRefEle = Regex.Matches(sRefInnerXml, @"<([^<>]+)>((?:(?!</?\1>).)+)</\1>");

                    foreach (Match eachElementMatch in mcRefEle)
                    {
                        switch (eachElementMatch.Groups[1].Value.ToString())
                        {
                            case "Author":
                                string sAuthorInnerXml = eachElementMatch.Groups[2].Value.ToString();
                                MatchCollection mcAuEle = Regex.Matches(sAuthorInnerXml, @"<([^<>]+)>((?:(?!</?\1>).)+)</\1>");
                                auCount += 1;
                                auGroupName = string.Format("aug_{0}", auCount.ToString());

                                foreach (Match eachAuInEle in mcAuEle)
                                {
                                    switch (eachAuInEle.Groups[1].Value.ToString())
                                    {
                                        //author part
                                        case "Surname":
                                            sRefEleTypeTemp = eachAuInEle.Groups[2].Value.ToString();
                                            dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlAuthorSurName, auGroupName, RefElements.ldlAuthorSurName);
                                            break;
                                        case "Forename":
                                            sRefEleTypeTemp = eachAuInEle.Groups[2].Value.ToString();
                                            dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlAuthorGivenName, auGroupName, RefElements.ldlAuthorGivenName);
                                            break;
                                        case "Suffix":
                                            sRefEleTypeTemp = eachAuInEle.Groups[2].Value.ToString();
                                            dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlAuthorSuffix, auGroupName, RefElements.ldlAuthorSuffix);
                                            break;
                                        case "ldlAuthorDelimiterAnd":
                                            sRefEleTypeTemp = eachAuInEle.Groups[2].Value.ToString();
                                            dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlAuthorDelimiterAnd, auGroupName, RefElements.ldlAuthorDelimiterAnd);
                                            break;
                                    }
                                }
                                break;


                            case "Editor":
                                string sEditorInnerXml = eachElementMatch.Groups[2].Value.ToString();
                                MatchCollection mcEdEle = Regex.Matches(sEditorInnerXml, @"<([^<>]+)>((?:(?!</?\1>).)+)</\1>");
                                edCount += 1;
                                edGroupName = string.Format("edg_{0}", edCount.ToString());

                                foreach (Match eachEdInEle in mcEdEle)
                                {
                                    switch (eachEdInEle.Groups[1].Value.ToString())
                                    {
                                        //author part
                                        case "ESurname":
                                            sRefEleTypeTemp = eachEdInEle.Groups[2].Value.ToString();
                                            dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlEditorSurName, edGroupName, RefElements.ldlEditorSurName);
                                            break;
                                        case "EForename":
                                            sRefEleTypeTemp = eachEdInEle.Groups[2].Value.ToString();
                                            dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlEditorGivenName, edGroupName, RefElements.ldlEditorGivenName);
                                            break;
                                        case "ESuffix":
                                            sRefEleTypeTemp = eachEdInEle.Groups[2].Value.ToString();
                                            dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlEditorSuffix, edGroupName, RefElements.ldlEditorSuffix);
                                            break;
                                        case "ldlEditorDelimiterAnd":
                                            sRefEleTypeTemp = eachEdInEle.Groups[2].Value.ToString();
                                            dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlEditorDelimiterAnd, edGroupName, RefElements.ldlEditorDelimiterAnd);
                                            break;
                                    }
                                }
                                break;

                            case "ldlAccessedDate":
                                //added by Dakshinamoorthy on 2020-Dec-18
                                dateCount += 1;
                                dateGroupName = string.Format("dateg_{0}", dateCount.ToString());

                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                //updated by Dakshinamoorthy on 2020-Dec-18
                                //dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlAccessedDate, "", RefElements.ldlAccessedDate);
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlAccessedDate, dateGroupName, RefElements.ldlAccessedDate);
                                break;

                            case "ldlConferenceDate":
                                //added by Dakshinamoorthy on 2020-Dec-18
                                dateCount += 1;
                                dateGroupName = string.Format("dateg_{0}", dateCount.ToString());

                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                //updated by Dakshinamoorthy on 2020-Dec-18
                                //dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlConferenceDate, "", RefElements.ldlConferenceDate);
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlConferenceDate, dateGroupName, RefElements.ldlConferenceDate);
                                break;

                            case "ldlConferenceName":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlConferenceName, "", RefElements.ldlConferenceName);
                                break;

                            case "ldlAccessedDateLabel":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlAccessedDate, "", RefElements.ldlAccessedDate);
                                break;

                            case "PubDate":
                                string sDateInnerXml = eachElementMatch.Groups[2].Value.ToString();
                                MatchCollection mcDateEle = Regex.Matches(sDateInnerXml, @"<([^<>]+)>((?:(?!</?\1>).)+)</\1>");
                                dateCount += 1;
                                dateGroupName = string.Format("dateg_{0}", dateCount.ToString());

                                foreach (Match eachDateInEle in mcDateEle)
                                {
                                    switch (eachDateInEle.Groups[1].Value.ToString())
                                    {
                                        case "Year":
                                            sRefEleTypeTemp = eachDateInEle.Groups[2].Value.ToString();
                                            dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPublicationYear, dateGroupName, RefElements.ldlPublicationYear);
                                            break;
                                        case "Month":
                                            sRefEleTypeTemp = eachDateInEle.Groups[2].Value.ToString();
                                            dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPublicationMonth, dateGroupName, RefElements.ldlPublicationMonth);
                                            break;
                                        case "Day":
                                            sRefEleTypeTemp = eachDateInEle.Groups[2].Value.ToString();
                                            dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPublicationDay, dateGroupName, RefElements.ldlPublicationDay);
                                            break;
                                        case "Season":
                                            sRefEleTypeTemp = eachDateInEle.Groups[2].Value.ToString();
                                            dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlSeason, dateGroupName, RefElements.ldlSeason);
                                            break;
                                    }
                                }
                                break;

                            case "Collab":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlCollab, "", RefElements.ldlCollab);
                                break;
                            case "Suffix":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlAuthorSuffix, "", RefElements.ldlAuthorSuffix);
                                break;
                            case "Etal":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlAuthorDelimiterEtal, "", RefElements.ldlAuthorSuffix);
                                break;

                            //editor part
                            case "ESurname":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlEditorSurName, "", RefElements.ldlEditorSurName);
                                break;
                            case "EForename":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlEditorGivenName, "", RefElements.ldlEditorGivenName);
                                break;

                            case "ldlEditorDelimiterEds_Front":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlEditorDelimiterEds_Front, "", RefElements.ldlEditorDelimiterEds_Front);
                                break;

                            case "ldlEditorDelimiterEds_Back":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlEditorDelimiterEds_Back, "", RefElements.ldlEditorDelimiterEds_Back);
                                break;


                            //titles
                            case "ldlArticleTitle":
                            case "Article_Title":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlArticleTitle, "", RefElements.ldlArticleTitle);
                                break;
                            case "Journal_Title":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlJournalTitle, "", RefElements.ldlJournalTitle);
                                break;

                            case "ldlChapterTitle":
                            case "chapter-title":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlChapterTitle, "", RefElements.ldlChapterTitle);
                                break;

                            case "ldlBookTitle":
                            case "book-title":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlBookTitle, "", RefElements.ldlBookTitle);
                                break;

                            case "ldlSourceTitle":
                            case "source":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlSourceTitle, "", RefElements.ldlSourceTitle);
                                break;

                            //dates
                            case "Year":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPublicationYear, "", RefElements.ldlPublicationYear);
                                break;
                            case "month":
                            case "MonthMisc": //added by Dakshinamoorthy on 2019-Dec-09
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPublicationMonth, "", RefElements.ldlPublicationMonth);
                                break;
                            case "day":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPublicationDay, "", RefElements.ldlPublicationDay);
                                break;

                            case "Vol_No":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlVolumeNumber, "", RefElements.ldlVolumeNumber);
                                break;
                            case "Issue_No":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlIssueNumber, "", RefElements.ldlIssueNumber);
                                break;

                            case "FirstPage":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlFirstPageNumber, "", RefElements.ldlFirstPageNumber);
                                break;
                            case "LastPage":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlLastPageNumber, "", RefElements.ldlLastPageNumber);
                                break;

                            case "PageRange":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPageRange, "", RefElements.ldlPageRange);
                                break;


                            case "Edition":
                            case "ldlEditionNumber":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlEditionNumber, "", RefElements.ldlEditionNumber);
                                break;
                            case "Doi":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlDOINumber, "", RefElements.ldlDOINumber);
                                break;

                            case "issn":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlISSNNumber, "", RefElements.ldlISSNNumber);
                                break;

                            case "conf-date":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlConferenceDate, "", RefElements.ldlConferenceDate);
                                break;
                            case "conf-loc":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlConferenceLocation, "", RefElements.ldlConferenceLocation);
                                break;
                            case "conf-name":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlConferenceName, "", RefElements.ldlConferenceName);
                                break;
                            case "conf-sponsor":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlConferenceSponser, "", RefElements.ldlConferenceSponser);
                                break;

                            case "PublisherName":
                            case "ldlPublisherName":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPublisherName, "", RefElements.ldlPublisherName);
                                break;

                            case "ldlInstitutionName":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlInstitutionName, "", RefElements.ldlInstitutionName);
                                break;

                            case "ldlPublisherLocation": //added by Dakshinamoorthy 2020-Apr-30
                            case "PublisherLocation":
                            case "ldlCity":
                            case "ldlState":
                            case "ldlCountry":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPublisherLocation, "", RefElements.ldlPublisherLocation);
                                break;

                            case "pub-id":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPubMedIdNumber, "", RefElements.ldlPubMedIdNumber);
                                break;
                            case "Website":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlURL, "", RefElements.ldlURL);
                                break;

                            case "ldlURLLabel":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlURLLabel, "", RefElements.ldlURLLabel);
                                break;


                            case "email":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlEmail, "", RefElements.ldlEmail);
                                break;

                            case "PubMedIdLabel":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPubMedIdLabel, "", RefElements.ldlPubMedIdLabel);
                                break;
                            case "PubMedIdNumber":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPubMedIdNumber, "", RefElements.ldlPubMedIdNumber);
                                break;

                            case "ldlAuthorDelimiterAnd":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlAuthorDelimiterAnd, "", RefElements.ldlAuthorDelimiterAnd);
                                break;

                            case "ldlEditorDelimiterAnd":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlEditorDelimiterAnd, "", RefElements.ldlEditorDelimiterAnd);
                                break;

                            case "ldlDOILabel":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlDOILabel, "", RefElements.ldlDOILabel);
                                break;

                            case "ldlTitleLabel":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlTitleLabel, "", RefElements.ldlTitleLabel);
                                break;

                            case "ldlISBNNumber":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlISBNNumber, "", RefElements.ldlISBNNumber);
                                break;

                            case "ldlThesisKeyword":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlThesisKeyword, "", RefElements.ldlThesisKeyword);
                                break;

                            case "ldlPaperKeyword":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlPaperKeyword, "", RefElements.ldlPaperKeyword);
                                break;

                            case "ldlReportKeyword":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlReportKeyword, "", RefElements.ldlReportKeyword);
                                break;

                            case "ldlReportNumber":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlReportNumber, "", RefElements.ldlReportNumber);
                                break;

                            case "ldlMisc":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlMisc, "", RefElements.ldlMisc);
                                break;

                            case "ldlELocationId":
                            case "eLocation_Id":
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlELocationId, "", RefElements.ldlELocationId);
                                break;

                            case "RefPrefix": //added by Dakshinamoorthy on 2020-May-20
                                sRefEleTypeTemp = eachElementMatch.Groups[2].Value.ToString();
                                dsRefClassfied.Tables["RefIdentifiedElements"].Rows.Add(1, sRefEleTypeTemp.Trim(), Cleanup4MatchingContent(sRefEleTypeTemp), (int)RefElements.ldlRefPrefix, "", RefElements.ldlRefPrefix);
                                break;

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ClassifyRefElements4Match_LocalExe", ex.Message, true, "");
            }
            return true;
        }

        private string Cleanup4MatchingContent(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, @"[\(\[\]\)]", "");
                sRefContent = Regex.Replace(sRefContent, @"[\:\.,; ]+$", "");
                sRefContent = Regex.Replace(sRefContent, @"^((?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))[\/]$", "$1");
                //added by Dakshinamoorthy on 2020-Apr-13
                //to handle query
                sRefContent = Regex.Replace(sRefContent, @"(?:</?LQ_[0-9A-Z]{6}>)", "");

                //added by Dakshinamoorthy on 2020-Dec-29 (for escapting "Author Query" content for BluPencil)
                sRefContent = Regex.Replace(sRefContent, @"(?:<(authorQueryOpen|authorQuerySelf|authorQueryClose|procIntruc)>((?:(?!</?\1).)+)</\1>)", "");

                //added by Dakshinamoorthy on 2020-Apr-25
                sRefContent = Regex.Replace(sRefContent, @"(?:<LIS_[0-9A-Z]{6}/>)", "");
                sRefContent = Regex.Replace(sRefContent, @"(?:<LOM_[0-9A-Z]{6}/>)", "");
                //added by Dakshinamoorthy on 2020-Jul-28
                sRefContent = Regex.Replace(sRefContent, @"(?:[\u002D\u00AD\u2010]{3})", "–");
                //added by Dakshinamoorthy on 2020-Dec-08
                sRefContent = Regex.Replace(sRefContent, @"<r_space/>", "");
                //added by Dakshinamoorthy on 2020-Dec-12
                sRefContent = Regex.Replace(sRefContent, @"^([\u201C])(.+)$", "$2");
                sRefContent = Regex.Replace(sRefContent, @"^(.+)([\.,]+[\u201D])$", "$1");

                //updated by Dakshinamoorthy on 2020-Dec-18
                sRefContent = Regex.Replace(sRefContent, "</?[^<>]+>", "");

                sRefContent = EscapeNonEnglishChar(sRefContent);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\Cleanup4MatchingContent", ex.Message, true, "");
            }
            return sRefContent;
        }

        private bool GenarateGroupId(ref DataTable dtRefData)
        {
            try
            {
                string sOriginalContent = string.Empty;
                int nGroupId = 1;

                for (int i = 0; i <= dtRefData.Rows.Count - 1; i++)
                {
                    sOriginalContent = dtRefData.Rows[i]["RefContent"].ToString();

                    if (Regex.IsMatch(sOriginalContent, "^(.*),$"))
                    {
                        dtRefData.Rows[i]["PuncGroupId"] = nGroupId;
                        nGroupId += 1;
                    }
                    else if (Regex.IsMatch(sOriginalContent, @"^(.*)\.$"))
                    {
                        dtRefData.Rows[i]["PuncGroupId"] = nGroupId;
                        nGroupId += 1;
                    }
                    else if (Regex.IsMatch(sOriginalContent, @"^(.*)\.(?:&#x201D;)$", RegexOptions.IgnoreCase))
                    {
                        dtRefData.Rows[i]["PuncGroupId"] = nGroupId;
                        nGroupId += 1;
                    }
                    else
                    {
                        dtRefData.Rows[i]["PuncGroupId"] = nGroupId;
                    }
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\GenarateGroupId", ex.Message, true, "");
            }
            return true;
        }



        private bool MatchRefElements(ref DataTable dtRefData, DataTable dtIdentifedData)
        {
            try
            {
                string sRefElementContent = string.Empty;
                string sRefElementType = string.Empty;
                string sRefElementGroupId = string.Empty;

                string sOriginalContent = string.Empty;
                string sOriginalType = string.Empty;
                string sIdentifiedContent = string.Empty;

                int rowNumberDt = 0;

                bool bMatchingStarted = false;

                //genarate group ids
                GenarateGroupId(ref dtRefData);



                //IEnumerable<DataRow> query = from row in dtIdentifedData.AsEnumerable()
                //                             orderby row.Field<string>("RefContentCleaned").Length descending
                //                             orderby row.Field<int>("RefElementPriority") descending
                //                             select row;

                var groups = dtIdentifedData.AsEnumerable();
                var groupList = from g in groups
                                group g by g.Field<string>("RefElementGroupId") into Group1
                                select new { Column1 = Group1.Key, Properties = Group1 };
                List<DataClass> newList = new List<DataClass>();

                foreach (var item in groupList)
                {
                    if (!string.IsNullOrEmpty(item.Column1.ToString()))
                    {
                        newList.Add(new DataClass() { Column1 = item.Column1, Properties = item.Properties.ToList<DataRow>() });
                    }
                }

                List<Tuple<string, string, string>> lstEachGroup = new List<Tuple<string, string, string>>();

                foreach (var eachItem in newList)
                {
                    lstEachGroup = new List<Tuple<string, string, string>>();
                    for (int i = 0; i <= eachItem.Properties.Count - 1; i++)
                    {
                        DataRow eachRow = eachItem.Properties[i];
                        sRefElementContent = eachRow.Field<string>("RefContent");
                        sRefElementType = eachRow.Field<int>("RefElementType").ToString();
                        sRefElementGroupId = eachRow.Field<string>("RefElementGroupId");
                        lstEachGroup.Add(new Tuple<string, string, string>(sRefElementContent, sRefElementType, sRefElementGroupId));
                    }
                    MatchRefElementsWithGroup(ref dtRefData, lstEachGroup);
                }



                IEnumerable<DataRow> query = dtIdentifedData.AsEnumerable().OrderByDescending(s => s["RefContentCleaned"].ToString().Length)
                    .ThenBy(s => s["RefElementPriority"]);

                foreach (DataRow eachSortedRow in query)
                {
                    bMatchingStarted = false;
                    sRefElementContent = eachSortedRow.Field<string>("RefContent");
                    sRefElementType = eachSortedRow.Field<int>("RefElementType").ToString();
                    sRefElementGroupId = eachSortedRow.Field<string>("RefElementGroupId");

                    if (!string.IsNullOrEmpty(sRefElementGroupId))
                    {
                        continue;
                    }

                    rowNumberDt = 0;

                    List<string> lstIdentifiedSplitedContent = Regex.Split(sRefElementContent, "[ ]").ToList();
                    List<int> lstIdentifiedIndex = new List<int>();
                    List<Tuple<int, string>> lstIdentifiedIndexGroupId = new List<Tuple<int, string>>();

                    for (int i = 0; i <= lstIdentifiedSplitedContent.Count - 1; i++)
                    {
                        //lstIdentifiedIndex = new List<int>();
                        lstIdentifiedIndexGroupId = new List<Tuple<int, string>>();
                        for (int j = rowNumberDt; j <= dtRefData.Rows.Count - 1; j++)
                        {

                            //added by Dakshinamoorthy on 2018-Dec-26
                            if (j > dtRefData.Rows.Count - 1)
                            {
                                break;
                            }

                            sOriginalContent = dtRefData.Rows[j]["RefContent"].ToString();
                            sOriginalType = dtRefData.Rows[j]["ElementType"].ToString();

                            sIdentifiedContent = lstIdentifiedSplitedContent[i].ToString();

                            sOriginalContent = DoHexaDecimalUnicodeConversion(Cleanup4MatchingContent(HexUnicodeToCharConvertor(ConvertDecimalToHexDecimal(sOriginalContent))));
                            sIdentifiedContent = DoHexaDecimalUnicodeConversion(Cleanup4MatchingContent(HexUnicodeToCharConvertor(ConvertDecimalToHexDecimal(sIdentifiedContent))));

                            //sOriginalContent = ConvertDecimalToHexDecimal(Cleanup4MatchingContent(sOriginalContent));
                            //sIdentifiedContent = ConvertDecimalToHexDecimal(Cleanup4MatchingContent(sIdentifiedContent));

                            sOriginalContent = sOriginalContent.ToLower();
                            sIdentifiedContent = sIdentifiedContent.ToLower();

                            //if (((sOriginalContent.Equals(sIdentifiedContent)) ||
                            //    ((sOriginalContent.Equals("&")  || sOriginalContent.ToLower().Equals("&#x0026;")) && sOriginalContent.Equals("and")))) && 
                            //    (string.IsNullOrEmpty(sOriginalType))))

                            if (((sOriginalContent.Equals(sIdentifiedContent)) ||
                                (Regex.IsMatch(sOriginalContent, "^(?:&|&#x0026;|[\u2026]|&#x2026;)$") && sIdentifiedContent.Equals("and")) ||
                                (sOriginalContent.Equals(Regex.Replace(sIdentifiedContent, "&#x2013;", "-")))) &&
                                (string.IsNullOrEmpty(sOriginalType)))

                            {
                                bMatchingStarted = true;
                                //lstIdentifiedIndex.Add(j);
                                lstIdentifiedIndexGroupId.Add(new Tuple<int, string>(j, sRefElementGroupId));

                                for (i = i + 1; i <= lstIdentifiedSplitedContent.Count - 1; i++)
                                {
                                    j += 1;
                                    rowNumberDt = j;
                                    sOriginalContent = dtRefData.Rows[j]["RefContent"].ToString();
                                    sIdentifiedContent = lstIdentifiedSplitedContent[i].ToString();

                                    sOriginalContent = DoHexaDecimalUnicodeConversion(Cleanup4MatchingContent(HexUnicodeToCharConvertor(ConvertDecimalToHexDecimal(sOriginalContent))));
                                    sIdentifiedContent = DoHexaDecimalUnicodeConversion(Cleanup4MatchingContent(HexUnicodeToCharConvertor(ConvertDecimalToHexDecimal(sIdentifiedContent))));

                                    //sOriginalContent = ConvertDecimalToHexDecimal(Cleanup4MatchingContent(sOriginalContent));
                                    //sIdentifiedContent = ConvertDecimalToHexDecimal(Cleanup4MatchingContent(sIdentifiedContent));

                                    sOriginalContent = sOriginalContent.ToLower();
                                    sIdentifiedContent = sIdentifiedContent.ToLower();

                                    if (((sOriginalContent.Equals(sIdentifiedContent)) ||
                                (Regex.IsMatch(sOriginalContent, "^(?:&|&#x0026;|[\u2026]|&#x2026;)$") && sIdentifiedContent.Equals("and")) ||
                                (sOriginalContent.Equals(Regex.Replace(sIdentifiedContent, "&#x2013;", "-")))) &&
                                (string.IsNullOrEmpty(sOriginalType)))
                                    {
                                        //lstIdentifiedIndex.Add(j);
                                        lstIdentifiedIndexGroupId.Add(new Tuple<int, string>(j, sRefElementGroupId));
                                    }
                                    else
                                    {
                                        if (bMatchingStarted)
                                        {
                                            bMatchingStarted = false;
                                            goto LBL_SKIP_MATCH;
                                        }
                                    }
                                }
                                goto LBL_SKIP_MATCH;

                            }
                            else
                            {
                                rowNumberDt = j;
                                if (bMatchingStarted)
                                {
                                    bMatchingStarted = false;
                                    goto LBL_SKIP_MATCH;
                                }
                            }
                        }

                    LBL_SKIP_MATCH:
                        //lstIdentifiedIndexGroupId.Add(new Tuple<int, string>(j, sRefElementGroupId));

                        //if (lstIdentifiedIndex != null && (lstIdentifiedIndex.Count == lstIdentifiedSplitedContent.Count))
                        if (lstIdentifiedIndexGroupId != null && (lstIdentifiedIndexGroupId.Count == lstIdentifiedSplitedContent.Count))
                        {
                            //int nStartIndex = lstIdentifiedIndexGroupId[0].Item1;
                            //int nPuncGroupId = Convert.ToInt32(dtRefData.Rows[nStartIndex]["PuncGroupId"].ToString());
                            //int emptyRowCount = dtRefData.AsEnumerable().Where(c => c.Field<int>("PuncGroupId") == nPuncGroupId).Count();

                            foreach (var eachIndex in lstIdentifiedIndexGroupId)
                            {
                                RefElements eleRef = (RefElements)int.Parse(sRefElementType);
                                //dtRefData.Rows[eachIndex]["ElementType"] = Enum.GetName(typeof(RefElements), sRefElementType);
                                dtRefData.Rows[eachIndex.Item1]["ElementType"] = Enum.GetName(typeof(RefElements), eleRef);
                                dtRefData.Rows[eachIndex.Item1]["GroupId"] = eachIndex.Item2;
                            }
                            rowNumberDt = 0;
                        }
                        else
                        {
                            if ((rowNumberDt > 0) && (rowNumberDt < dtRefData.Rows.Count - 1))
                            {
                                i = -1;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\MatchRefElements", ex.Message, true, "");
            }
            return true;
        }


        private bool MatchRefElementsWithGroup(ref DataTable dtRefData, List<Tuple<string, string, string>> lstEachGroupData)
        {
            try
            {
                string sRefElementContent = string.Empty;
                string sRefElementType = string.Empty;
                string sRefElementGroupId = string.Empty;

                string sOriginalContent = string.Empty;
                string sOriginalType = string.Empty;
                string sIdentifiedContent = string.Empty;

                int rowNumberDt = 0;

                bool bMatchingStarted = false;

                foreach (var eachSortedRow in lstEachGroupData)
                {
                    bMatchingStarted = false;
                    sRefElementContent = eachSortedRow.Item1;
                    sRefElementType = eachSortedRow.Item2;
                    sRefElementGroupId = eachSortedRow.Item3;

                    //rowNumberDt = 0;

                    List<string> lstIdentifiedSplitedContent = Regex.Split(sRefElementContent, "[ ]").ToList();
                    List<int> lstIdentifiedIndex = new List<int>();
                    List<Tuple<int, string>> lstIdentifiedIndexGroupId = new List<Tuple<int, string>>();

                    for (int i = 0; i <= lstIdentifiedSplitedContent.Count - 1; i++)
                    {
                        //lstIdentifiedIndex = new List<int>();
                        lstIdentifiedIndexGroupId = new List<Tuple<int, string>>();
                        for (int j = rowNumberDt; j <= dtRefData.Rows.Count - 1; j++)
                        {
                            sOriginalContent = dtRefData.Rows[j]["RefContent"].ToString();
                            sOriginalType = dtRefData.Rows[j]["ElementType"].ToString();

                            sIdentifiedContent = lstIdentifiedSplitedContent[i].ToString();

                            sOriginalContent = DoHexaDecimalUnicodeConversion(Cleanup4MatchingContent(HexUnicodeToCharConvertor(ConvertDecimalToHexDecimal(sOriginalContent))));
                            sIdentifiedContent = DoHexaDecimalUnicodeConversion(Cleanup4MatchingContent(HexUnicodeToCharConvertor(ConvertDecimalToHexDecimal(sIdentifiedContent))));

                            //sOriginalContent = ConvertDecimalToHexDecimal(Cleanup4MatchingContent(sOriginalContent));
                            //sIdentifiedContent = ConvertDecimalToHexDecimal(Cleanup4MatchingContent(sIdentifiedContent));

                            //commented by Dakshinamoorthy on 2019-Jan-29
                            //sOriginalContent = sOriginalContent.ToLower();
                            //sIdentifiedContent = sIdentifiedContent.ToLower();


                            //if (((sOriginalContent.Equals(sIdentifiedContent)) ||
                            //    ((sOriginalContent.Equals("&")  || sOriginalContent.ToLower().Equals("&#x0026;")) && sOriginalContent.Equals("and")))) && 
                            //    (string.IsNullOrEmpty(sOriginalType))))

                            if (((sOriginalContent.Equals(sIdentifiedContent)) ||
                                (Regex.IsMatch(sOriginalContent, "^(?:&|&#x0026;|[\u2026]|&#x2026;)$") && sIdentifiedContent.Equals("and")) ||
                                (sOriginalContent.Equals(Regex.Replace(sIdentifiedContent, "&#x2013;", "-")))) &&
                                (string.IsNullOrEmpty(sOriginalType)))

                            {
                                bMatchingStarted = true;
                                //lstIdentifiedIndex.Add(j);
                                lstIdentifiedIndexGroupId.Add(new Tuple<int, string>(j, sRefElementGroupId));

                                for (i = i + 1; i <= lstIdentifiedSplitedContent.Count - 1; i++)
                                {
                                    j += 1;
                                    rowNumberDt = j;
                                    sOriginalContent = dtRefData.Rows[j]["RefContent"].ToString();
                                    sIdentifiedContent = lstIdentifiedSplitedContent[i].ToString();

                                    sOriginalContent = DoHexaDecimalUnicodeConversion(Cleanup4MatchingContent(HexUnicodeToCharConvertor(ConvertDecimalToHexDecimal(sOriginalContent))));
                                    sIdentifiedContent = DoHexaDecimalUnicodeConversion(Cleanup4MatchingContent(HexUnicodeToCharConvertor(ConvertDecimalToHexDecimal(sIdentifiedContent))));

                                    //sOriginalContent = ConvertDecimalToHexDecimal(Cleanup4MatchingContent(sOriginalContent));
                                    //sIdentifiedContent = ConvertDecimalToHexDecimal(Cleanup4MatchingContent(sIdentifiedContent));

                                    sOriginalContent = sOriginalContent.ToLower();
                                    sIdentifiedContent = sIdentifiedContent.ToLower();

                                    if (((sOriginalContent.Equals(sIdentifiedContent)) ||
                                (Regex.IsMatch(sOriginalContent, "^(?:&|&#x0026;|[\u2026]|&#x2026;)$") && sIdentifiedContent.Equals("and")) ||
                                (sOriginalContent.Equals(Regex.Replace(sIdentifiedContent, "&#x2013;", "-")))) &&
                                (string.IsNullOrEmpty(sOriginalType)))
                                    {
                                        //lstIdentifiedIndex.Add(j);
                                        lstIdentifiedIndexGroupId.Add(new Tuple<int, string>(j, sRefElementGroupId));
                                    }
                                    else
                                    {
                                        if (bMatchingStarted)
                                        {
                                            bMatchingStarted = false;
                                            goto LBL_SKIP_MATCH;
                                        }
                                    }
                                }
                                goto LBL_SKIP_MATCH;

                            }
                            else
                            {
                                rowNumberDt = j;
                                if (bMatchingStarted)
                                {
                                    bMatchingStarted = false;
                                    goto LBL_SKIP_MATCH;
                                }
                            }
                        }

                    LBL_SKIP_MATCH:
                        //lstIdentifiedIndexGroupId.Add(new Tuple<int, string>(j, sRefElementGroupId));

                        //if (lstIdentifiedIndex != null && (lstIdentifiedIndex.Count == lstIdentifiedSplitedContent.Count))
                        if (lstIdentifiedIndexGroupId != null && (lstIdentifiedIndexGroupId.Count == lstIdentifiedSplitedContent.Count))
                        {
                            foreach (var eachIndex in lstIdentifiedIndexGroupId)
                            {
                                RefElements eleRef = (RefElements)int.Parse(sRefElementType);
                                //dtRefData.Rows[eachIndex]["ElementType"] = Enum.GetName(typeof(RefElements), sRefElementType);
                                dtRefData.Rows[eachIndex.Item1]["ElementType"] = Enum.GetName(typeof(RefElements), eleRef);
                                dtRefData.Rows[eachIndex.Item1]["GroupId"] = eachIndex.Item2;
                            }
                            rowNumberDt = 0;
                        }
                        else
                        {
                            if ((rowNumberDt > 0) && (rowNumberDt < dtRefData.Rows.Count - 1))
                            {
                                i = -1;
                            }
                        }

                    }

                    //updated by Dakshinamoorthy.G on 2018-Dec-25
                    if (lstIdentifiedIndexGroupId.Count > 0)
                    {
                        rowNumberDt = lstIdentifiedIndexGroupId[lstIdentifiedIndexGroupId.Count - 1].Item1 + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\MatchRefElementsWithGroup", ex.Message, true, "");
            }
            return true;
        }

        private string HandleWebsiteContent(Match myWebsiteMatch)
        {
            string sWebsiteContent = myWebsiteMatch.Value;
            sWebsiteContent = Regex.Replace(sWebsiteContent, @"</?Website>", "");

            try
            {
                sWebsiteContent = Regex.Replace(sWebsiteContent, @"[\.]", "~~dot~~");
                sWebsiteContent = Regex.Replace(sWebsiteContent, @"[\,]", "~~comma~~");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleWebsiteContent", ex.Message, true, "");
            }
            return sWebsiteContent;
        }

        private string UnescapteChar(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "~~comma~~", ",");
                sRefContent = Regex.Replace(sRefContent, "~~dot~~", ".");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\UnescapteChar", ex.Message, true, "");
            }
            return sRefContent;
        }

        //added by Dakshinamoorthy on 2020-Nov-30
        private string ChangeCaseAtValidPosition(string sRefContent)
        {
            try
            {
                //Ru-rong, J., Nackley, A., Huh, Y., et al. (2018) Neuroinflammation and central sensitization in chronic and widespread pain. Anesthesiology. 129: (343-366).
                sRefContent = Regex.Replace(sRefContent, @"(?:(?<=\b(?:[A-Z][a-z]+\-))(?:[a-z])(?=[a-z]+[\.,;\: ]+))", ChangeUpperCaseFunction);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ChangeCaseAtValidPosition", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string ChangeUpperCaseFunction(Match myCaseMatch)
        {
            string sOutputContent = myCaseMatch.Value.ToString();
            try
            {
                sOutputContent = sOutputContent.ToUpper();
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ChangeCaseAtValidPosition", ex.Message, true, "");
                return myCaseMatch.Value;
            }
            return sOutputContent;
        }

        private string RemoveSpaceAtValidPosition(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, @"(?:( [0-9]+) ;)", "$1<r_space/>;");
                sRefContent = Regex.Replace(sRefContent, @"(?:( [0-9]+) \:)", "$1<r_space/>:");
                //updated by Dakshinamoorthy on 2020-Dec-29
                sRefContent = Regex.Replace(sRefContent, @"(?:<(authorQueryOpen|authorQuerySelf|authorQueryClose|procIntruc)>((?:(?!</?\1).)+)</\1>)", IgnoreRemoveSpaceTag);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveSpaceAtValidPosition", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string IgnoreRemoveSpaceTag(Match myRemoveSpaceMatch)
        {
            string sOutputContent = myRemoveSpaceMatch.Value;
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "(?:<r_space/>)", " ");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IgnoreRemoveSpaceTag", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private string IntroSpaceAtValidPosition_Old(string sRefContent)
        {
            try
            {
                //added by Dakshinamoorthy on 2020-Jul-23
                IdentifyURL(ref sRefContent);
                if (Regex.IsMatch(sRefContent, @"(?:<Website>((?:(?!</?Website>).)+)</Website>)"))
                {
                    sRefContent = Regex.Replace(sRefContent, @"(?:<Website>((?:(?!</?Website>).)+)</Website>)", HandleWebsiteContent);
                    sRefContent = Regex.Replace(sRefContent, @"(?:</?ldlURLLabel>)", "");
                }

                //5. Courneya, K. S., and McAuley, E. (1995). Cognitive mediators of the social influence-exercise adherence relationship: a test of the theory of planned behavior. Journal of Behavioral Medicine, 18(5), 499–515.
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+)(\([0-9]+\)[,\:])", "$1<split/>$2");

                //3. Blair, S. N., and Brodney, S. (1999). Effects of physical inactivity and obesity on morbidity and mortality: current evidence and research issues. Medicine and Science in Sports and Exercise, 31(11 Suppl), S646–S662.
                sRefContent = Regex.Replace(sRefContent, @"((?:[ ]|<split/>)[0-9]+;)((?:[0-9]+))(\([^\(\)]+\)[\:]?)([a-z]?[0-9]+[a-z]?)", "$1<split/>$2<split/>$3<split/>$4", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+;)([0-9]+\:)([a-z]?[0-9]+[a-z]?)", "$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);

                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+)(\([0-9]+[ _])", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( <i>[0-9]+</i>)(\([^\(\)]+\))", "$1<split/>$2");
                //sRefContent = Regex.Replace(sRefContent, @"( (?:[Dd][Oo][Ii][\:]?))([^ \:])", "$1<split/>$2");
                //updated by Dakshinamoorthy on 2019-Dec-10
                //sRefContent = Regex.Replace(sRefContent, @"( [0-9]+\:)([0-9]+(?:[\u2013\-]|&#x2013;))", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+\:)([0-9]+(?:[\u2013\-]|&#x2013;))(?!(?:[0-9]+\:)(?:[0-9]+))", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"(\b(?:P[Mm][Ii][Dd][:]))((?:[0-9]+)\b)", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+\:)([0-9]+\.)", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+)(\([0-9]+(?:&#x2013;|\-)[0-9]+)", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"((?<=(?:<split/>|[ ]))\([0-9]+\)\:)([0-9]+)", "$1<split/>$2");

                //updated by Dakshinamoorthy on 2019-Dec-10
                //sRefContent = Regex.Replace(sRefContent, @"( [0-9]+\:)([A-Za-z]?[0-9]+[A-Za-z]?[ ]?(?:[\u2013\-]|&#x2013;|&#8211;)?)", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+\:)([A-Za-z]?[0-9]+[A-Za-z]?[ ]?(?:[\u2013\-]|&#x2013;|&#8211;)?)", "$1<split/>$2");

                sRefContent = Regex.Replace(sRefContent, @"( <b>(?:[0-9]+)</b>)(\()", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"((?:<split/>|[ ])(?:\([0-9]+\))\:?)([a-z]?[0-9]+[a-z]?)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?:<split/>|[ ])[0-9]+;)([0-9]+(?:<split/>|[ ]))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?:<split/>|[ ])(?:\([^\(\)]+\),?))([a-z]?[0-9]+[a-z]?)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+;)([0-9]+)(\([^\(\)]+\))", "$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);

                sRefContent = Regex.Replace(sRefContent, @"((?:<split/>|[ ])\([^\(\)]+\)[\:\.;,]?)(e?[a-z]?[0-9]+e?[a-z]?)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+)(\([^\(\)]+\)[\:.,]?)([a-z]?[0-9]+[a-z]?)", "$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( (?:<b>[0-9]+</b>[\:,\.]|<b>[0-9]+[\:,\.]</b>))(\b(?:e?[a-z]?[0-9]+e?[a-z]?)\b)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( (?:<[bi]>)?[0-9]+)(\([^\(\)]+\)(?:</b>)?[\:., ])", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( (?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))[\:\.;,])([a-z]?[0-9]+[a-z]?\b)", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"((?:[ ]|<split/>)[0-9]+)(\([^\(\)]+\)[\:\.,;])([a-z]?[0-9]+[a-z]?\b)", "$1<split/>$2<split/>$3");
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+[;])([0-9]+\.)", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( (?:<i>)?[0-9]+(?:</i>)?)(\([^\(\)]+\))", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"(\.</i>)([0-9]+\()", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"((?<=(?:<split/>|[ ]))(?:[0-9]+))(\()", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( [a-z]\.)(\([0-9]+)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"([\.,]+)((?:and|&) )", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(?:(?<=[^ ])\(\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b)", "<split/>$&", RegexOptions.IgnoreCase);
                //added by Dakshinamoorthy on 2019-Sep-10
                sRefContent = Regex.Replace(sRefContent, @"(?:(\(\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b[\/])(\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b))", "$1<split/>$2", RegexOptions.IgnoreCase);

                sRefContent = Regex.Replace(sRefContent, @"(?:(?<=[^ ])\(\b(?:(?:Accessed))\b)", "<split/>$&", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"([\u201d])<i>", "$1<split/><i>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?<=, )(?:[A-Z]{2}\-[0-9]+))(\([^\(\)]+\))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(<b>(?:(?!</?b>).)+</b>,)(\([0-9]+)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @", ([0-9]+,)</i>(?= [0-9]+(?:[\u2013][0-9]+)?)", ",</i> <i>$1</i>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @", ([0-9]+,)</i>(?= [e]?[0-9]{5,})", ",</i> <i>$1</i>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]{4};)(\([0-9]+\)\:)([0-9]+)", "$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( (?:19|20)[0-9]{2}\/)(\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+[A-Z])(\([0-9]+\))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?<=(?:[A-Za-z]+))[,])([0-9]+(?=[ ]?\([^\(\)]+\)))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(\b(?:IEEE J\.(?=[^ ])))", "$1<split/>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+,)([0-9]+[\-\u2013][0-9]+\.)", "$1<split/>$2", RegexOptions.IgnoreCase);

                //added by Dakshinamoorthy on 2019-Dec-10 (revert split)
                sRefContent = Regex.Replace(sRefContent, @"(?: [0-9]+\:<split/>[0-9]+[\u2013\-][0-9]+:[0-9]+)", RemoveSplitTag);

                //added by Dakshinamoorthy on 2020-May-05
                sRefContent = Regex.Replace(sRefContent, @"(\.,)(\.\.\.)", "$1<split/>$2", RegexOptions.IgnoreCase);
                //added by Dakshinamoorthy on 2020-Jul-07
                sRefContent = Regex.Replace(sRefContent, @"( [A-Za-z]+)(\([0-9]{4}\))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"([;,\.]+)(https?\:\/\/)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?<=[^ ])\(Undated\))", "<split/>$&", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [A-Za-z]+\.)([0-9]{4}\. )", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(\.)(Journal of )", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?<=[,])[0-9]{4},)(\([0-9]+\))([0-9]+)", "<split/>$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?<=[,])[0-9]{4},)([0-9]+\:)([0-9]+)", "<split/>$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(<(?:https?\:\/\/)?www\.[^<> ]+>)\(", "$1<split/>(", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=[.,]+ )(&)([A-Z{0}][a-z{1}]+,)(?=[ ]))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(,)(pp\.[ ]?[0-9]+)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=[.,]+ )(&)([A-Z{0}][a-z{1}]+,)(?=[ ]))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(\([0-9]+\)\.)([A-Z{0}][a-z{1}]+))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:([A-Z{0}][a-z{1}]+[\.]+)(\([ ]*[0-9]+\)\.)([A-Z{0}][a-z{1}]+))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=(?:<split/>|[ ]))([A-Z{0}]\.\,)([A-Z{0}][a-z{1}]+))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(?:(?<=[ ])(\([0-9]{4}\)\.)(<i>(?:(?!</?i>).)+</i>))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=[ ])([A-Z{0}][a-z{1}]+)(&)(?=[ ]))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=(?:, ))([A-Z][\.]+)(\([0-9]+\)))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=[ ])(\([0-9]{{4}}\)\:)(?=[A-Za-z]+))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=(?:<split/>|[ ]))(\([0-9]{{4}}\)[\.]+)(?=<i>))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=(?:<split/>|[ ]))(\([0-9]{{4}}\)[\.]+)(?=<i>))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(?:(?:^|[ ])(?:[\*][ ]?)+(?=[^ ]))", "$&<split/>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(?:(\.)(The ))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [A-Z][a-z]+\:)([A-Z][a-z]+ )", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [A-Z][a-z]+\.)([0-9]{4};)", "$1<split/>$2", RegexOptions.IgnoreCase);

                //added by Dakshinamoorthy on 2019-Jan-05
                do
                {
                    sRefContent = Regex.Replace(sRefContent, @"([A-Za-z]+,)([A-Za-z]+)", "$1<split/>$2");
                } while (Regex.IsMatch(sRefContent, @"([A-Za-z]+,)([A-Za-z]+)"));

                sRefContent = Regex.Replace(sRefContent, @"(, <i>[0-9]+</i>,)([0-9]+\.)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(<split/>){2,}", "<split/>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"^((?:<sup>[0-9]+</sup>)(?=[^ ]))", "$&<split/>", RegexOptions.IgnoreCase);


                sRefContent = UnescapteChar(sRefContent);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IntroSpaceAtValidPosition", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string IntroSpaceAtValidPosition(string sRefContent)
        {
            try
            {
                //added by Dakshinamoorthy on 2020-Jul-23
                IdentifyURL(ref sRefContent);
                if (Regex.IsMatch(sRefContent, @"(?:<Website>((?:(?!</?Website>).)+)</Website>)"))
                {
                    sRefContent = Regex.Replace(sRefContent, @"(?:<Website>((?:(?!</?Website>).)+)</Website>)", HandleWebsiteContent);
                    sRefContent = Regex.Replace(sRefContent, @"(?:</?ldlURLLabel>)", "");
                }

                sRefContent = Regex.Replace(sRefContent, @"(?:(\.)(The ))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [A-Z][a-z]+\:)([A-Z][a-z]+ )", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [A-Z][a-z]+\.)([0-9]{4};)", "$1<split/>$2", RegexOptions.IgnoreCase);

                //5. Courneya, K. S., and McAuley, E. (1995). Cognitive mediators of the social influence-exercise adherence relationship: a test of the theory of planned behavior. Journal of Behavioral Medicine, 18(5), 499–515.
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+)(\([0-9]+\)[,\:])", "$1<split/>$2");

                //3. Blair, S. N., and Brodney, S. (1999). Effects of physical inactivity and obesity on morbidity and mortality: current evidence and research issues. Medicine and Science in Sports and Exercise, 31(11 Suppl), S646–S662.
                sRefContent = Regex.Replace(sRefContent, @"((?:[ ]|<split/>)[0-9]+;)((?:[0-9]+))(\([^\(\)]+\)[\:]?)([a-z]?[0-9]+[a-z]?)", "$1<split/>$2<split/>$3<split/>$4", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+;)([0-9]+\:)([a-z]?[0-9]+[a-z]?)", "$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);

                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+)(\([0-9]+[ _])", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( <i>[0-9]+</i>)(\([^\(\)]+\))", "$1<split/>$2");
                //sRefContent = Regex.Replace(sRefContent, @"( (?:[Dd][Oo][Ii][\:]?))([^ \:])", "$1<split/>$2");
                //updated by Dakshinamoorthy on 2019-Dec-10
                //sRefContent = Regex.Replace(sRefContent, @"( [0-9]+\:)([0-9]+(?:[\u2013\-]|&#x2013;))", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+\:)([0-9]+(?:[\u2013\-]|&#x2013;))(?!(?:[0-9]+\:)(?:[0-9]+))", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"(\b(?:P[Mm][Ii][Dd][:]))((?:[0-9]+)\b)", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+\:)([0-9]+\.)", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+)(\([0-9]+(?:&#x2013;|\-)[0-9]+)", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"((?<=(?:<split/>|[ ]))\([0-9]+\)\:)([0-9]+)", "$1<split/>$2");

                //updated by Dakshinamoorthy on 2019-Dec-10
                //sRefContent = Regex.Replace(sRefContent, @"( [0-9]+\:)([A-Za-z]?[0-9]+[A-Za-z]?[ ]?(?:[\u2013\-]|&#x2013;|&#8211;)?)", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+\:)([A-Za-z]?[0-9]+[A-Za-z]?[ ]?(?:[\u2013\-]|&#x2013;|&#8211;)?)", "$1<split/>$2");

                sRefContent = Regex.Replace(sRefContent, @"( <b>(?:[0-9]+)</b>)(\()", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"((?:<split/>|[ ])(?:\([0-9]+\))\:?)([a-z]?[0-9]+[a-z]?)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?:<split/>|[ ])[0-9]+;)([0-9]+(?:<split/>|[ ]))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?:<split/>|[ ])(?:\([^\(\)]+\),?))([a-z]?[0-9]+[a-z]?)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+;)([0-9]+)(\([^\(\)]+\))", "$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);

                sRefContent = Regex.Replace(sRefContent, @"((?:<split/>|[ ])\([^\(\)]+\)[\:\.;,]?)(e?[a-z]?[0-9]+e?[a-z]?)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+)(\([^\(\)]+\)[\:.,]?)([a-z]?[0-9]+[a-z]?)", "$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( (?:<b>[0-9]+</b>[\:,\.]|<b>[0-9]+[\:,\.]</b>))(\b(?:e?[a-z]?[0-9]+e?[a-z]?)\b)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( (?:<[bi]>)?[0-9]+)(\([^\(\)]+\)(?:</b>)?[\:., ])", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( (?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))[\:\.;,])([a-z]?[0-9]+[a-z]?\b)", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"((?:[ ]|<split/>)[0-9]+)(\([^\(\)]+\)[\:\.,;])([a-z]?[0-9]+[a-z]?\b)", "$1<split/>$2<split/>$3");
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+[;])([0-9]+\.)", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( (?:<i>)?[0-9]+(?:</i>)?)(\([^\(\)]+\))", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"(\.</i>)([0-9]+\()", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"((?<=(?:<split/>|[ ]))(?:[0-9]+))(\()", "$1<split/>$2");
                sRefContent = Regex.Replace(sRefContent, @"( [a-z]\.)(\([0-9]+)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"([\.,]+)((?:and|&) )", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(?:(?<=[^ ])\(\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b)", "<split/>$&", RegexOptions.IgnoreCase);
                //added by Dakshinamoorthy on 2019-Sep-10
                sRefContent = Regex.Replace(sRefContent, @"(?:(\(\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b[\/])(\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b))", "$1<split/>$2", RegexOptions.IgnoreCase);

                sRefContent = Regex.Replace(sRefContent, @"(?:(?<=[^ ])\(\b(?:(?:Accessed))\b)", "<split/>$&", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"([\u201d])<i>", "$1<split/><i>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?<=, )(?:[A-Z]{2}\-[0-9]+))(\([^\(\)]+\))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(<b>(?:(?!</?b>).)+</b>,)(\([0-9]+)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @", ([0-9]+,)</i>(?= [0-9]+(?:[\u2013][0-9]+)?)", ",</i> <i>$1</i>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @", ([0-9]+,)</i>(?= [e]?[0-9]{5,})", ",</i> <i>$1</i>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]{4};)(\([0-9]+\)\:)([0-9]+)", "$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( (?:19|20)[0-9]{2}\/)(\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+[A-Z])(\([0-9]+\))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?<=(?:[A-Za-z]+))[,])([0-9]+(?=[ ]?\([^\(\)]+\)))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(\b(?:IEEE J\.(?=[^ ])))", "$1<split/>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [0-9]+,)([0-9]+[\-\u2013][0-9]+\.)", "$1<split/>$2", RegexOptions.IgnoreCase);

                //added by Dakshinamoorthy on 2019-Dec-10 (revert split)
                sRefContent = Regex.Replace(sRefContent, @"(?: [0-9]+\:<split/>[0-9]+[\u2013\-][0-9]+:[0-9]+)", RemoveSplitTag);

                //added by Dakshinamoorthy on 2020-May-05
                sRefContent = Regex.Replace(sRefContent, @"(\.,)(\.\.\.)", "$1<split/>$2", RegexOptions.IgnoreCase);
                //added by Dakshinamoorthy on 2020-Jul-07
                sRefContent = Regex.Replace(sRefContent, @"( [A-Za-z]+)(\([0-9]{4}\))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"([;,\.]+)(https?\:\/\/)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?<=[^ ])\(Undated\))", "<split/>$&", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"( [A-Za-z]+\.)([0-9]{4}\. )", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(\.)(Journal of )", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?<=[,])[0-9]{4},)(\([0-9]+\))([0-9]+)", "<split/>$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"((?<=[,])[0-9]{4},)([0-9]+\:)([0-9]+)", "<split/>$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(<(?:https?\:\/\/)?www\.[^<> ]+>)\(", "$1<split/>(", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=[.,]+ )(&)([A-Z{0}][a-z{1}]+,)(?=[ ]))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(,)(pp\.[ ]?[0-9]+)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=[.,]+ )(&)([A-Z{0}][a-z{1}]+,)(?=[ ]))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(\([0-9]+\)\.)([A-Z{0}][a-z{1}]+))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:([A-Z{0}][a-z{1}]+[\.]+)(\([ ]*[0-9]+\)\.)([A-Z{0}][a-z{1}]+))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2<split/>$3", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=(?:<split/>|[ ]))([A-Z{0}]\.\,)([A-Z{0}][a-z{1}]+))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(?:(?<=[ ])(\([0-9]{4}\)\.)(<i>(?:(?!</?i>).)+</i>))", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=[ ])([A-Z{0}][a-z{1}]+)(&)(?=[ ]))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=(?:, ))([A-Z][\.]+)(\([0-9]+\)))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=[ ])(\([0-9]{{4}}\)\:)(?=[A-Za-z]+))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=(?:<split/>|[ ]))(\([0-9]{{4}}\)[\.]+)(?=<i>))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, string.Format(@"(?:(?<=(?:<split/>|[ ]))(\([0-9]{{4}}\)[\.]+)(?=<i>))", sCapsNonEnglishChar, sSmallNonEnglishChar), "$1<split/>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(?:(?:^|[ ])(?:[\*][ ]?)+(?=[^ ]))", "$&<split/>", RegexOptions.IgnoreCase);

                //updated by Dakshinamoorthy on 2020-Dec-29
                sRefContent = Regex.Replace(sRefContent, @"(?:<(authorQueryOpen|authorQuerySelf|authorQueryClose|procIntruc)>((?:(?!</?\1).)+)</\1>)", RemoveSplitTag);

                //added by Dakshinamoorthy on 2019-Jan-05
                do
                {
                    sRefContent = Regex.Replace(sRefContent, @"([A-Za-z]+,)([A-Za-z]+)", "$1<split/>$2");
                } while (Regex.IsMatch(sRefContent, @"([A-Za-z]+,)([A-Za-z]+)"));

                sRefContent = Regex.Replace(sRefContent, @"(, <i>[0-9]+</i>,)([0-9]+\.)", "$1<split/>$2", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"(<split/>){2,}", "<split/>", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, @"^((?:<sup>[0-9]+</sup>)(?=[^ ]))", "$&<split/>", RegexOptions.IgnoreCase);


                sRefContent = UnescapteChar(sRefContent);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IntroSpaceAtValidPosition", ex.Message, true, "");
            }
            return sRefContent;
        }

        //added by Dakshinamoorthy on 2019-Dec-10
        private string RemoveSplitTag(Match mySplitMatch)
        {
            string sOutputContent = mySplitMatch.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "(<split/>)", "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveSplitTag", ex.Message, true, "");
                return mySplitMatch.Value.ToString();
            }
            return sOutputContent;
        }

        private List<Tuple<string, string>> SmartSplitRefContent(string sRefContent)
        {
            List<Tuple<string, string>> lstSmartSplitedRefContent = new List<Tuple<string, string>>();
            List<string> lstSpaceSplitedRefContent = new List<string>();
            List<string> lstIntroValidSpaceSplitedRefContent = new List<string>();

            string sTemp = string.Empty;

            try
            {
                lstSpaceSplitedRefContent = sRefContent.Split(' ').ToList();
                if (lstSpaceSplitedRefContent.Count > 0)
                {
                    foreach (string eachSpaceSplited in lstSpaceSplitedRefContent)
                    {
                        sTemp = eachSpaceSplited;
                        lstIntroValidSpaceSplitedRefContent = Regex.Split(sTemp, "<split/>").ToList();
                        if (lstIntroValidSpaceSplitedRefContent.Count > 0)
                        {
                            for (int i = 0; i <= lstIntroValidSpaceSplitedRefContent.Count - 1; i++)
                            {
                                if (i == lstIntroValidSpaceSplitedRefContent.Count - 1)
                                {
                                    lstSmartSplitedRefContent.Add(new Tuple<string, string>(lstIntroValidSpaceSplitedRefContent[i], "ws"));
                                }
                                else
                                {
                                    lstSmartSplitedRefContent.Add(new Tuple<string, string>(lstIntroValidSpaceSplitedRefContent[i], "wos"));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\SmartSplitRefContent", ex.Message, true, "");
            }
            return lstSmartSplitedRefContent;
        }

        private string ConvertDecimalToHexDecimal(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "&#([0-9]+);", Decimal2Hexa, RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ConvertDecimalToHexDecimal", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string DoHexaDecimalUnicodeConversion(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, sNonKeyboardCharPattern, HexaEntityConversion, RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\DoHexaDecimalUnicodeConversion", ex.Message, true, "");
            }
            return sRefContent;
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

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HexaEntityConversion", ex.Message, true, "");
            }
            return tmpStr;
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

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\Decimal2Hexa", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private bool RefPreCleanup(ref string sRefContent)
        {
            try
            {
                sRefContent = ConvertDecimalToHexDecimal(DoHexaDecimalUnicodeConversion(sRefContent));
                sRefContent = Regex.Replace(sRefContent, "[\u00A0]", " ");
                //updated by Dakshinamoorthy on 2019-Sep-11
                //sRefContent = Regex.Replace(sRefContent, @"[ ]+\.", ". ");
                sRefContent = Regex.Replace(sRefContent, @"(?:(?<=[ ](?:<[bi]>)?)\. \. \.(?=(?:</[bi]>)?[ ]))", "...");
                sRefContent = Regex.Replace(sRefContent, @"[ ]+\.(?![ ]?\.[ ]?\.)", ". ");
                sRefContent = Regex.Replace(sRefContent, @"\b([a-z]?[0-9]+[a-z]?)[ ]?([\u2013]|\-|&#x2013;)[ ]?([a-z]?[0-9]+[a-z]?)\b", "$1$2$3");
                sRefContent = Regex.Replace(sRefContent, @"</i><i>.</i> <i>", ". ");
                sRefContent = Regex.Replace(sRefContent, @",<i> </i>", ", ");


                do
                {
                    sRefContent = Regex.Replace(sRefContent, "(<[^/][^<>]*>)[ ]+", " $1");
                    sRefContent = Regex.Replace(sRefContent, "[ ]+(</[^<>]+>)", "$1 ");
                } while (Regex.IsMatch(sRefContent, "(?:(?:(<[^/][^<>]*>)[ ]+)|(?:[ ]+(</[^<>]+>)))"));

                do
                {
                    sRefContent = Regex.Replace(sRefContent, @"(<[a-zA-Z_\-]+>)[ ]+", " $1");
                    sRefContent = Regex.Replace(sRefContent, @"[ ]+(</[a-zA-Z_\-]+>)", "$1 ");
                } while (Regex.IsMatch(sRefContent, "(?:(?:(<[^/][^<>]+>)[ ]+)|(?:[ ]+(</[^<>]+>)))"));

                do
                {
                    sRefContent = Regex.Replace(sRefContent, "[ ]{2,}", " ");
                } while (Regex.IsMatch(sRefContent, "[ ]{2,}"));

                sRefContent = Regex.Replace(sRefContent, "<i></i>", "");


            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RefPreCleanup", ex.Message, true, "");
            }
            return true;
        }

        private bool RefPreCleanup4TaggedContent(ref string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "[\u00A0]", " ");
                sRefContent = Regex.Replace(sRefContent, @"[ ]+\.", ". ");

                do
                {
                    sRefContent = Regex.Replace(sRefContent, "(<[^/][^<>]+>)[ ]+", " $1");
                    sRefContent = Regex.Replace(sRefContent, "[ ]+(</[^<>]+>)", "$1 ");
                } while (Regex.IsMatch(sRefContent, "(?:(?:(<[^/][^<>]+>)[ ]+)|(?:[ ]+(</[^<>]+>)))"));



                do
                {
                    sRefContent = Regex.Replace(sRefContent, "[ ]{2,}", " ");
                } while (Regex.IsMatch(sRefContent, "[ ]{2,}"));

                sRefContent = Regex.Replace(sRefContent, @"<i>\.</i> ", ". ");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RefPreCleanup", ex.Message, true, "");
            }
            return true;
        }

        private string HandleLtGtInWebsite(Match myWebsiteMatch)
        {
            string sOutputContent = myWebsiteMatch.Value;
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "<", "~~lt~~");
                sOutputContent = Regex.Replace(sOutputContent, ">", "~~gt~~");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleLtGtInWebsite", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private bool NormalizeSpaces(ref string sRefContent)
        {
            try
            {
                //added by Dakshinamoorthy on 2020-Jul-14
                sRefContent = Regex.Replace(sRefContent, @"(<(?:https?\:\/\/)?www\.[^<> ]+>)", HandleLtGtInWebsite);

                sRefContent = Regex.Replace(sRefContent, "<ldlAuthorDelimiterChar> </ldlAuthorDelimiterChar>", "<ldlAuthorDelimiterChar><skip_space/></ldlAuthorDelimiterChar>");
                sRefContent = Regex.Replace(sRefContent, "<ldlAuthorDelimiterChar><b> </b></ldlAuthorDelimiterChar>", "<ldlAuthorDelimiterChar><b><skip_space/></b></ldlAuthorDelimiterChar>");
                sRefContent = Regex.Replace(sRefContent, "<ldlAuthorDelimiterChar><i> </i></ldlAuthorDelimiterChar>", "<ldlAuthorDelimiterChar><i><skip_space/></i></ldlAuthorDelimiterChar>");
                sRefContent = Regex.Replace(sRefContent, "</ldlAuthorDelimiterChar><ldlAuthorDelimiterChar>", "");

                sRefContent = Regex.Replace(sRefContent, "<ldlEditorDelimiterChar> </ldlEditorDelimiterChar>", "<ldlEditorDelimiterChar><skip_space/></ldlEditorDelimiterChar>");
                sRefContent = Regex.Replace(sRefContent, "<ldlEditorDelimiterChar><b> </b></ldlEditorDelimiterChar>", "<ldlEditorDelimiterChar><b><skip_space/></b></ldlEditorDelimiterChar>");
                sRefContent = Regex.Replace(sRefContent, "<ldlEditorDelimiterChar><i> </i></ldlEditorDelimiterChar>", "<ldlEditorDelimiterChar><i><skip_space/></i></ldlEditorDelimiterChar>");
                sRefContent = Regex.Replace(sRefContent, "</ldlEditorDelimiterChar><ldlEditorDelimiterChar>", "");


                do
                {
                    sRefContent = Regex.Replace(sRefContent, "(<[^/][^<>]+>)[ ]+", " $1");
                    sRefContent = Regex.Replace(sRefContent, "[ ]+(</[^<>]+>)", "$1 ");
                } while (Regex.IsMatch(sRefContent, "(?:(?:(<[^/][^<>]+>)[ ]+)|(?:[ ]+(</[^<>]+>)))"));

                do
                {
                    sRefContent = Regex.Replace(sRefContent, @"(<[a-zA-Z_\-]+>)[ ]+", " $1");
                    sRefContent = Regex.Replace(sRefContent, @"[ ]+(</[a-zA-Z_\-]+>)", "$1 ");
                } while (Regex.IsMatch(sRefContent, "(?:(?:(<[^/][^<>]+>)[ ]+)|(?:[ ]+(</[^<>]+>)))"));


                do
                {
                    sRefContent = Regex.Replace(sRefContent, "[ ]{2,}", " ");
                } while (Regex.IsMatch(sRefContent, "[ ]{2,}"));

                sRefContent = Regex.Replace(sRefContent, "<skip_space/>", " ");
                sRefContent = Regex.Replace(sRefContent, "~~lt~~", "<");
                sRefContent = Regex.Replace(sRefContent, "~~gt~~", ">");
                sRefContent = Regex.Replace(sRefContent, "~~space~~", " ");
                sRefContent = Regex.Replace(sRefContent, "~~dot~~", ".");


                sRefContent = sRefContent.Trim();
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\NormalizeSpaces", ex.Message, true, "");
            }
            return true;
        }

        private string HandlePunctuationsInTaggedRef(string sRefTaggedContent)
        {
            try
            {
                //Unicode conversion
                sRefTaggedContent = HexUnicodeToCharConvertor(sRefTaggedContent);

                //Author/Editor Surname
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlAuthorSurName|ldlEditorSurName|ldlAuthorInPrevoiusRef|ldlAuthorSuffix|ldlEditorSuffix|ldlEditorDelimiterEtal)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4AuEdSnm);

                //Author/Editor Surname
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlAuthorDelimiterEtal)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4Etal);

                //Author/Editor Given Name
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlAuthorGivenName|ldlEditorGivenName)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4AuEdGnm);

                //Collab
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlCollab)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4Titles);

                //Eds TODO: Check
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlEditorDelimiterEds)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4Titles);

                //Year
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlPublicationYear|ldlPublicationMonth|ldlPublicationDay|ldlSeason)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4PubYear);

                //Conference Date
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlConferenceDate|ldlConferenceYear|ldlConferenceMonth|ldlConferenceDay)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4PubYear);

                //Access and Updated Date
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlAccessedYear|ldlAccessedMonth|ldlAccessedDay|ldlUpdatedYear|ldlUpdatedMonth|ldlUpdatedDay)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4PubYear);

                //Publication Date
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlConferenceDate|ldlConferenceYear|ldlConferenceMonth|ldlConferenceDay)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4PubYear);

                //Publisher Information
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlConferenceSponser|ldlPublisherName|ldlPublisherLocation|ldlConferenceLocation)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4Titles);

                //Titles
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlArticleTitle|ldlChapterTitle|ldlBookTitle|ldlSourceTitle|ldlConferenceName)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4Titles);

                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlJournalTitle)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4JouTit);

                //Volume
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlVolumeNumber)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4VolumeNum);

                //Issue
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlIssueNumber)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4IssueNum);

                //Edition
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlEditionNumber)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4Titles);

                //Page
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlFirstPageNumber)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4FirstPage);
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlIssueNumber|ldlEditionNumber|ldlFirstPageNumber|ldlLastPageNumber)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4Titles);

                //DOI
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlDOINumber)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4DOINum);

                //DOI, PubMedID, ISBN, ISSN
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlDOINumber|ldlPubMedIdNumber|ldlISBNNumber|ldlISSNNumber)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4Titles);

                //URL
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlURL)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4URL);

                //E-Mail
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlEmail)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4Titles);

                //eLocationID
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlELocationId)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4Titles);

                //ldlEditorDelimiterEds_Back, ldlEditorDelimiterEds_Front
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4Role);

                //Ref Label
                //commented by Dakshinamoorthy based on Manaimaran.G requirement on 2019-Jan-05
                //sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(ldlRefLabel)>((?:(?!</?\1>).)+)</\1>", HandlePunctuations4Titles);

                //revert unicode
                sRefTaggedContent = DoHexaDecimalUnicodeConversion(sRefTaggedContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuationsInTaggedRef", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        //added by Dakshinamoorthy on 2020-Aug-24
        private string HandlePunctuations4Role(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();
            try
            {
                if (Regex.IsMatch(sHandledContent, @"^([\(])") && Regex.IsMatch(sHandledContent, @"([\.]?\)[\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\(])(.*)?([\.]?\)[\.,:;]*)$", string.Format("$1<{0}>$2</{0}>$3", sTagName));
                }
                else if (Regex.IsMatch(sHandledContent, @"([\.]?\)[\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.*)?([\.]?\)[\.,:;]*)$", string.Format("<{0}>$1</{0}>$2", sTagName));
                }
                else if (Regex.IsMatch(sHandledContent, @"([\.][,\:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^((?:.+)(?:[\.]))([,\:;]*)$", string.Format("<{0}>$1</{0}>$2", sTagName));
                }
                else if (Regex.IsMatch(sHandledContent, @"^([\(])"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\(])(.+)$", string.Format("$1<{0}>$2</{0}>", sTagName));
                }
                else
                {
                    return myRefMatch.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4FirstPage", ex.Message, true, "");
            }
            return sHandledContent;
        }

        private string HandlePunctuations4FirstPage(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();
            try
            {
                if (Regex.IsMatch(sHandledContent, @"^(pp?\.)(.+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(pp?\.)(.+)$", string.Format("$1<{0}>$2</{0}>", sTagName));
                }
                else
                {
                    return myRefMatch.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4FirstPage", ex.Message, true, "");
            }
            return sHandledContent;
        }

        private string HandlePunctuations4AuEdSnm(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();
            string sDelimiterCharName = (sTagName.Equals("ldlAuthorSurName")) ? "ldlAuthorDelimiterChar" : "ldlEditorDelimiterChar";


            try
            {
                if (Regex.IsMatch(sHandledContent, "^<b>") && Regex.IsMatch(sHandledContent, @"([\.,:;]+</b>)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>(.+)([\.,:;]+)</b>$", string.Format("<{0}><b>{1}</b></{0}><{3}><b>{2}</b></{3}>", sTagName, "$1", "$2", sDelimiterCharName));
                }
                else if (Regex.IsMatch(sHandledContent, @"([\.,:;\)]+(?:</[bi]>)?)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([\.,:;\)]+(?:</[bi]>)?)$", string.Format("<{0}>{1}</{0}><{3}>{2}</{3}>", sTagName, "$1", "$2", sDelimiterCharName));
                }
                else //deafult
                {
                    return myRefMatch.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4AuEdSnm", ex.Message, true, "");
            }
            return sHandledContent;
        }

        private string HandlePunctuations4Etal(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();
            try
            {
                //updated by Dakshinamoorthy on 2019-Nov-18
                //if (Regex.IsMatch(sHandledContent, "^<i>") && Regex.IsMatch(sHandledContent, @"(?:[\.,:;]+)</i>$"))
                if (Regex.IsMatch(sHandledContent, "^<i>") && Regex.IsMatch(sHandledContent, @"(?:[,:;]+)</i>$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<i>(.+)([,:;]+)</i>$", string.Format("<{0}><i>{1}</i></{0}><i>{2}</i>", sTagName, "$1", "$2"));
                }
                //updated by Dakshinamoorthy on 2019-Nov-18
                //else if (Regex.IsMatch(sHandledContent, @"([\.,:;]+)$"))
                else if (Regex.IsMatch(sHandledContent, @"([,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([,:;]+)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else //deafult
                {
                    return myRefMatch.Value.ToString();
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4AuEdSnm", ex.Message, true, "");
            }
            return sHandledContent;
        }

        private string HandlePunctuations4AuEdGnm(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();
            string sDelimiterCharName = (sTagName.Equals("ldlAuthorSurName")) ? "ldlAuthorDelimiterChar" : "ldlEditorDelimiterChar";

            try
            {
                if (Regex.IsMatch(sHandledContent, @"^((?:[A-Z]\.? ?){1,3}\.)([,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^((?:[A-Z]\.? ?){1,3}\.)([,:;]+)$", string.Format("<{0}>{1}</{0}><{3}>{2}</{3}>", sTagName, "$1", "$2", sDelimiterCharName));
                }
                else if (Regex.IsMatch(sHandledContent, @"^[\(\[]((?:[A-Z]\.? ?){1,3}\.)([,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^[\(\[]((?:[A-Z]\.? ?){1,3}\.)([,:;]+)$", string.Format("(<{0}>{1}</{0}><{3}>{2}</{3}>", sTagName, "$1", "$2", sDelimiterCharName));
                }
                else if (Regex.IsMatch(sHandledContent, @"^[\(\[]((?:[A-Z]\.? ?){1,3}\.)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^[\[\(]((?:[A-Z]\.? ?){1,3}\.)$", string.Format("(<{0}>{1}</{0}>", sTagName, "$1"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^([\(\[])([A-Z]{1,4})$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\(\[])([A-Z]{1,4})$", string.Format("{0}<{1}>{2}</{1}>", "$1", sTagName, "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^([A-Z]{1,4})([\)\]])$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([A-Z]{1,4})([\)\]])$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                //added by Dakshinamoorthy on 2019-Feb-19
                else if (Regex.IsMatch(sHandledContent, string.Format(@"^(?:([A-Z{0}][a-z{1}]+)([\.\:;,]+))$", sCapsNonEnglishChar, sSmallNonEnglishChar)))
                {
                    sHandledContent = Regex.Replace(sHandledContent,
                        string.Format(@"^(?:([A-Z{0}][a-z{1}]+)([\.\:;,]+))$", sCapsNonEnglishChar, sSmallNonEnglishChar),
                        string.Format("<{0}>{1}</{0}><{3}>{2}</{3}>", sTagName, "$1", "$2", sDelimiterCharName));
                }
                else //deafult
                {
                    return myRefMatch.Value.ToString();
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4AuEdGnm", ex.Message, true, "");
            }
            return sHandledContent;
        }

        private string HandlePunctuations4PubYear(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();
            try
            {
                if (sHandledContent.StartsWith("(") && Regex.IsMatch(sHandledContent, @"(\.\)[\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\()(.+)(\.\)[\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (sHandledContent.StartsWith("(") && Regex.IsMatch(sHandledContent, @"(\.?\)[\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\()(.+)(\.?\)[\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (sHandledContent.StartsWith("(") && Regex.IsMatch(sHandledContent, @"([\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\()(.+)([\.,:;]+)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (sHandledContent.StartsWith("(") && !Regex.IsMatch(sHandledContent, @"([\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\()(.+)$", string.Format("{0}<{1}>{2}</{1}>", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>(.+)([\.,:;]+)</b>$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>(.+)([\.,:;]+)</b>$", string.Format("<{0}><b>{1}</b></{0}><b>{2}</b>", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^(.+)([\)\]][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([\)\]][\.,:;]*)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^(.+)([\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([\.,:;]+)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else //deafult
                {
                    return myRefMatch.Value.ToString();
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4PubYear", ex.Message, true, "");
            }
            return sHandledContent;
        }

        private string HandlePunctuations4URL(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();
            try
            {
                if (sHandledContent.StartsWith("(") && Regex.IsMatch(sHandledContent, @"(\)[\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\()(.+)(\)[\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (sHandledContent.StartsWith("[") && Regex.IsMatch(sHandledContent, @"(\][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\[)(.+)(\][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (sHandledContent.StartsWith("<") && Regex.IsMatch(sHandledContent, @"(>[\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\<)(.+)(\>[\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^(?:[\u201C])") && Regex.IsMatch(sHandledContent, @"(?:[\u201D][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u201C])(.+)([\u201D][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^[\u2018]") && Regex.IsMatch(sHandledContent, @"([\u2019][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u2018])(.+)([\u2019][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>") && Regex.IsMatch(sHandledContent, @"(?:[\.,:;]+)</b>$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>(.+)([\.,:;]+)</b>$", string.Format("<{0}><b>{1}</b></{0}><b>{2}</b>", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>\(") && Regex.IsMatch(sHandledContent, @"\)</b>(?:[\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>\((.+)\)</b>([\.,:;]+)$", string.Format("<b>(</b><{0}><b>{1}</b></{0}><b>)</b>{2}", sTagName, "$1", "$2"));
                }

                else if (Regex.IsMatch(sHandledContent, @"([\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([\.,:;]+)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else //deafult
                {
                    return myRefMatch.Value.ToString();
                }

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4Titles", ex.Message, true, "");
            }
            return sHandledContent;
        }

        private string HandlePunctuations4DOINum(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();
            try
            {
                if (Regex.IsMatch(sHandledContent, @"^((?:(?:<[bi]>)?(?:[\(\[])?(?:(?:<[bi]>)?(?:\b(?:[Dd][Oo][Ii])[\:\. ]+))))") && Regex.IsMatch(sHandledContent, @"((?:(?:</[bi]>)?(?:[\)\]])?(?:(?:</[bi]>)?[\:\.;, ]+)))$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^((?:(?:<[bi]>)?(?:[\(\[])?(?:(?:<[bi]>)?(?:\b(?:[Dd][Oo][Ii])[\:\. ]+))))(.+)((?:(?:</[bi]>)?(?:[\)\]])?(?:(?:</[bi]>)?[\:\.;, ]+)))$", string.Format("<{0}>{1}</{0}><{2}>{3}</{2}><{4}>{5}</{4}>", "ldlDOILabel", "$1", sTagName, "$2", "ldlDOILabel", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^((?:(?:<[bi]>)?(?:[\(\[])?(?:(?:<[bi]>)?(?:\b(?:[V]ol(?:ume)?)[\:\.]+ ?))))") && Regex.IsMatch(sHandledContent, @"((?:(?:</[bi]>)?(?:[\)\]])?(?:(?:</[bi]>)?[\:\.;, ]*)))$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^((?:(?:<[bi]>)?(?:[\(\[])?(?:(?:<[bi]>)?(?:\b(?:[Dd][Oo][Ii])[\:\.]+ ?))))(.+)((?:(?:</[bi]>)?(?:[\)\]])?(?:(?:</[bi]>)?[\:\.;, ]*)))$", string.Format("<{0}>{1}</{0}><{2}>{3}</{2}><{4}>{5}</{4}>", "ldlDOILabel", "$1", sTagName, "$2", "ldlDOILabel", "$3"));
                }
                else if (sHandledContent.StartsWith("(") && Regex.IsMatch(sHandledContent, @"(\)[\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\()(.+)(\)[\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (sHandledContent.StartsWith("[") && Regex.IsMatch(sHandledContent, @"(\][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\[)(.+)(\][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^(?:[\u201C])") && Regex.IsMatch(sHandledContent, @"(?:[\u201D][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u201C])(.+)([\u201D][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^[\u2018]") && Regex.IsMatch(sHandledContent, @"([\u2019][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u2018])(.+)([\u2019][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>") && Regex.IsMatch(sHandledContent, @"(?:[\.,:;]+)</b>$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>(.+)([\.,:;]+)</b>$", string.Format("<{0}><b>{1}</b></{0}><b>{2}</b>", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>\(") && Regex.IsMatch(sHandledContent, @"\)</b>(?:[\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>\((.+)\)</b>([\.,:;]+)$", string.Format("<b>(</b><{0}><b>{1}</b></{0}><b>)</b>{2}", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"([\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([\.,:;]+)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else //deafult
                {
                    return myRefMatch.Value.ToString();
                }

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4Titles", ex.Message, true, "");
            }
            return sHandledContent;
        }

        private string HandlePunctuations4VolumeNum(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();
            try
            {
                if (Regex.IsMatch(sHandledContent, @"^((?:(?:<[bi]>)?(?:[\(\[])?(?:(?:<[bi]>)?(?:\b(?:[Vv]ol(?:ume)?)[\:\. ]+))))") && Regex.IsMatch(sHandledContent, @"((?:(?:</[bi]>)?(?:[\)\]])?(?:(?:</[bi]>)?[\:\.;, ]+)))$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^((?:(?:<[bi]>)?(?:[\(\[])?(?:(?:<[bi]>)?(?:\b(?:[Vv]ol(?:ume)?)[\:\. ]+))))([^\(\)]+)((?:(?:</[bi]>)?(?:[\)\]])?(?:(?:</[bi]>)?[\:\.;, ]+)))$", string.Format("<{0}>{1}</{0}><{2}>{3}</{2}><{4}>{5}</{4}>", "ldlVolumeLabel", "$1", sTagName, "$2", "ldlVolumeLabel", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^((?:(?:<[bi]>)?(?:[\(\[])?(?:(?:<[bi]>)?(?:\b(?:[Vv]ol(?:ume)?)[\:\.]+ ?))))") && Regex.IsMatch(sHandledContent, @"((?:(?:</[bi]>)?(?:[\)\]])?(?:(?:</[bi]>)?[\:\.;, ]*)))$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^((?:(?:<[bi]>)?(?:[\(\[])?(?:(?:<[bi]>)?(?:\b(?:[Vv]ol(?:ume)?)[\:\.]+ ?))))(.+)((?:(?:</[bi]>)?(?:[\)\]])?(?:(?:</[bi]>)?[\:\.;, ]*)))$", string.Format("<{0}>{1}</{0}><{2}>{3}</{2}><{4}>{5}</{4}>", "ldlVolumeLabel", "$1", sTagName, "$2", "ldlVolumeLabel", "$3"));
                }
                else if (sHandledContent.StartsWith("(") && Regex.IsMatch(sHandledContent, @"(\)[\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\()(.+)(\)[\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (sHandledContent.StartsWith("[") && Regex.IsMatch(sHandledContent, @"(\][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\[)(.+)(\][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^(?:[\u201C])") && Regex.IsMatch(sHandledContent, @"(?:[\u201D][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u201C])(.+)([\u201D][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^[\u2018]") && Regex.IsMatch(sHandledContent, @"([\u2019][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u2018])(.+)([\u2019][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>") && Regex.IsMatch(sHandledContent, @"(?:[\.,:;]+)</b>$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>(.+)([\.,:;]+)</b>$", string.Format("<{0}><b>{1}</b></{0}><b>{2}</b>", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>\(") && Regex.IsMatch(sHandledContent, @"\)</b>(?:[\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>\((.+)\)</b>([\.,:;]+)$", string.Format("<b>(</b><{0}><b>{1}</b></{0}><b>)</b>{2}", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^((?:[Vv]ol(?:ume)?|[Vv])[\:\. ]+)") && Regex.IsMatch(sHandledContent, @"(?:[\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^((?:[Vv]ol(?:ume)?|[Vv])[\:\. ]+)(.+)((?:[\.,:;]+))$", string.Format("<{0}>{1}</{0}><{2}>{3}</{2}><{4}>{5}</{4}>", "ldlVolumeLabel", "$1", sTagName, "$2", "ldlVolumeLabel", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^(<i>(?:(?!</?i>).)+</i>)(<b>[\:\.;,]+</b>)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(<i>(?:(?!</?i>).)+</i>)(<b>[\:\.;,]+</b>)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"([\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([\.,:;]+)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else //deafult
                {
                    return myRefMatch.Value.ToString();
                }

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4Titles", ex.Message, true, "");
            }
            return sHandledContent;
        }

        private string HandlePunctuations4IssueNum(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();
            try
            {
                if (Regex.IsMatch(sHandledContent, @"^((?:(?:[Ii]ss(?:ue)?)|[Nn]um(?:ber)?|[Nn]o[s]?|[Nn]r[s]?)[\:\. ]+)") && Regex.IsMatch(sHandledContent, @"([\:\.;, ]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^((?:(?:[Ii]ss(?:ue)?)|[Nn]um(?:ber)?|[Nn]o[s]?|[Nn]r[s]?)[\:\. ]+)(.+)([\:\.;, ]*)$", string.Format("<{0}>{1}</{0}><{2}>{3}</{2}><{4}>{5}</{4}>", "ldlIssueLabel", "$1", sTagName, "$2", "ldlIssueLabel", "$3"));
                }
                else if (sHandledContent.StartsWith("(") && Regex.IsMatch(sHandledContent, @"(\)[\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\()(.+)(\)[\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (sHandledContent.StartsWith("[") && Regex.IsMatch(sHandledContent, @"(\][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\[)(.+)(\][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^(?:[\u201C])") && Regex.IsMatch(sHandledContent, @"(?:[\u201D][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u201C])(.+)([\u201D][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^[\u2018]") && Regex.IsMatch(sHandledContent, @"([\u2019][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u2018])(.+)([\u2019][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>") && Regex.IsMatch(sHandledContent, @"(?:[\.,:;]+)</b>$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>(.+)([\.,:;]+)</b>$", string.Format("<{0}><b>{1}</b></{0}><b>{2}</b>", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>\(") && Regex.IsMatch(sHandledContent, @"\)</b>(?:[\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>\((.+)\)</b>([\.,:;]+)$", string.Format("<b>(</b><{0}><b>{1}</b></{0}><b>)</b>{2}", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"([\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([\.,:;]+)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else //deafult
                {
                    return myRefMatch.Value.ToString();
                }

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4Titles", ex.Message, true, "");
            }
            return sHandledContent;
        }
        private string HandlePunctuations4EditionNum(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();
            try
            {
                if (sHandledContent.StartsWith("(") && Regex.IsMatch(sHandledContent, @"(\)[\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\()(.+)(\)[\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (sHandledContent.StartsWith("[") && Regex.IsMatch(sHandledContent, @"(\][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\[)(.+)(\][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^(?:[\u201C])") && Regex.IsMatch(sHandledContent, @"(?:[\u201D][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u201C])(.+)([\u201D][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^[\u2018]") && Regex.IsMatch(sHandledContent, @"([\u2019][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u2018])(.+)([\u2019][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>") && Regex.IsMatch(sHandledContent, @"(?:[\.,:;]+)</b>$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>(.+)([\.,:;]+)</b>$", string.Format("<{0}><b>{1}</b></{0}><b>{2}</b>", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>\(") && Regex.IsMatch(sHandledContent, @"\)</b>(?:[\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>\((.+)\)</b>([\.,:;]+)$", string.Format("<b>(</b><{0}><b>{1}</b></{0}><b>)</b>{2}", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"([\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([\.,:;]+)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else //deafult
                {
                    return myRefMatch.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4Titles", ex.Message, true, "");
            }
            return sHandledContent;
        }


        private string HandlePunctuations4Titles(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();
            try
            {
                if (sHandledContent.StartsWith("(") && Regex.IsMatch(sHandledContent, @"(\)[\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\()(.+)(\)[\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (sHandledContent.StartsWith("[") && Regex.IsMatch(sHandledContent, @"(\][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\[)(.+)(\][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^(?:[\u201C])") && Regex.IsMatch(sHandledContent, @"(?:[\u201D][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u201C])(.+)([\u201D][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^(?:[\u201E])") && Regex.IsMatch(sHandledContent, @"(?:[\u201C][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u201E])(.+)([\u201C][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^[\u2018]") && Regex.IsMatch(sHandledContent, @"([\u2019][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u2018])(.+)([\u2019][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>") && Regex.IsMatch(sHandledContent, @"(?:[\.,:;]+)</b>$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>(.+)([\.,:;]+)</b>$", string.Format("<{0}><b>{1}</b></{0}><b>{2}</b>", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<b>\(") && Regex.IsMatch(sHandledContent, @"\)</b>(?:[\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<b>\((.+)\)</b>([\.,:;]+)$", string.Format("<b>(</b><{0}><b>{1}</b></{0}><b>)</b>{2}", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^\(") && Regex.IsMatch(sHandledContent, @"\)(?:[\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^\((.+)\)([\.,:;]+)$", string.Format("(<{0}>{1}</{0}>){2}", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^\(") && Regex.IsMatch(sHandledContent, @"(?:[\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^\((.+)([\.,:;]+)$", string.Format("(<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^<i>\(</i>") && Regex.IsMatch(sHandledContent, @"(?:[\.,:;\)]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<i>\(</i>(.+)([\.,:;\)]+)$", string.Format("<i>(</i><{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^(pp?\. )") && Regex.IsMatch(sHandledContent, @"(?:[\.,:;\)]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(pp?\. )([^\.,:;\)]+)([\.,:;\)]+)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^\("))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^\((.+)$", string.Format("(<{0}>{1}</{0}>", sTagName, "$1"));
                }
                else if (Regex.IsMatch(sHandledContent, @"\)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)\)$", string.Format("<{0}>{1}</{0}>)", sTagName, "$1"));
                }
                else if (Regex.IsMatch(sHandledContent, @"[\(]") == false && Regex.IsMatch(sHandledContent, @"[\)][\.,;]*$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([\)][\.,;]*)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else if (Regex.IsMatch(sHandledContent, @"([\.,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([\.,:;]+)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                else //deafult
                {
                    return myRefMatch.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4Titles", ex.Message, true, "");
            }
            return sHandledContent;
        }

        private string HandlePunctuations4JouTit(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].ToString();
            string sHandledContent = myRefMatch.Groups[2].ToString();

            //sCapsNonEnglishChar = string.Empty;
            //sSmallNonEnglishChar = string.Empty;


            //J. Agric. Res. 
            //J.Geophys.Res.- Earth Surf.
            //Sci.Total Environ.
            //Small Rumin. Res.
            string sJouTitleAbbPtn = string.Format(@"(?:(?:^(?:[A-Z{0}][a-z{1}]*\. )(?:(?!(?:\b[a-z{1}]+)).)*(?:[A-Z{0}][a-z{1}]*\.)$)|(?:^(?:[A-Z{0}][a-z{1}]* )(?:(?!(?:\b[a-z{1}]+)).)*([A-Z{0}][a-z{1}]*\.) (?:[A-Z{0}][a-z{1}]*\.)$))", sCapsNonEnglishChar, sSmallNonEnglishChar);

            string sJouTitleAbbPtn_Italic = string.Format(@"(?:^((?:<i>)(?:(?:(?:[A-Z{0}][a-z{1}]*\. )(?:(?!(?:\b[a-z{1}]+)).)*(?:[A-Z{0}][a-z{1}]*))|(?:^(?:[A-Z{0}][a-z{1}]* )(?:(?!(?:\b[a-z{1}]+)).)*(?:[A-Z{0}][a-z{1}]*\.) (?:[A-Z{0}][a-z{1}]*)))(?:\.</i>|</i>\.))([,; ]*)$)", sCapsNonEnglishChar, sSmallNonEnglishChar);

            try
            {
                if (sHandledContent.StartsWith("(") && Regex.IsMatch(sHandledContent, @"(\)[\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\()(.+)(\)[\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (sHandledContent.StartsWith("[") && Regex.IsMatch(sHandledContent, @"(\][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(\[)(.+)(\][\.,:;]+)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^[\u201C]") && Regex.IsMatch(sHandledContent, @"([\u201D][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u201C])(.+)([\u201D][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(sHandledContent, @"^[\u2018]") && Regex.IsMatch(sHandledContent, @"([\u2019][\.,:;]*)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^([\u2018])(.+)([\u2019][\.,:;]*)$", string.Format("{0}<{1}>{2}</{1}>{3}", "$1", sTagName, "$2", "$3"));
                }
                else if (Regex.IsMatch(RemoveUnwantedWordFromAbbrJouTitle(sHandledContent), sJouTitleAbbPtn))
                {
                    sHandledContent = string.Format("<{0}>{1}</{0}>", sTagName, sHandledContent);
                }
                else if ((Regex.IsMatch(RemoveUnwantedWordFromAbbrJouTitle(sHandledContent), sJouTitleAbbPtn_Italic)) || (Regex.Matches(sHandledContent, string.Format(@"(?:[A-Z{0}][a-z{1}]+\.,?(?=(?: |$|<)))", sCapsNonEnglishChar, sSmallNonEnglishChar)).Count >= 3))
                {
                    string sInnerItalicPattern = @"(?:^((?:<i>)(?:(?!</?i>).)+(?:\.?</i>|</i>\.?))([,; ]*)$)";
                    //sHandledContent = string.Format("<{0}>{1}</{0}>", sTagName, sHandledContent);

                    if (Regex.IsMatch(sHandledContent, sInnerItalicPattern))
                    {
                        sHandledContent = Regex.Replace(sHandledContent, sInnerItalicPattern, string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                    }
                    else if (Regex.IsMatch(sHandledContent, @"^(.+)([\.,:;]+)$"))
                    {
                        sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([\.,:;]+)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                    }
                    else
                    {
                        return myRefMatch.Value.ToString();
                    }
                }
                //updated by Dakshinamoorthy on 2019-Nov-11 (for CUP Thangadurai's update)
                //else if (Regex.IsMatch(sHandledContent, @"([\.,:;]+)$"))
                else if (Regex.IsMatch(sHandledContent, @"([,:;]+)$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^(.+)([,:;]+)$", string.Format("<{0}>{1}</{0}>{2}", sTagName, "$1", "$2"));
                }
                //updated by Dakshinamoorthy on 2019-Nov-11 (for CUP Thangadurai's update)
                //else if (Regex.IsMatch(sHandledContent, @"^(<i>)") && Regex.IsMatch(sHandledContent, @"([\.,:;]+)</i>$"))
                else if (Regex.IsMatch(sHandledContent, @"^(<i>)") && Regex.IsMatch(sHandledContent, @"([,:;]+)</i>$"))
                {
                    sHandledContent = Regex.Replace(sHandledContent, @"^<i>(.+)([,:;]+)</i>$", string.Format("<{0}><i>{1}</i></{0}><i>{2}</i>", sTagName, "$1", "$2"));
                }
                else //deafult
                {
                    return myRefMatch.Value.ToString();
                }

            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePunctuations4Titles", ex.Message, true, "");
            }
            return sHandledContent;
        }

        private string RemoveUnwantedWordFromAbbrJouTitle(string sRefContent)
        {
            string sOutputContent = sRefContent;

            try
            {
                sOutputContent = Regex.Replace(sOutputContent, @"\b(?:of)\b", "");

                do
                {
                    sOutputContent = Regex.Replace(sOutputContent, "[ ]{2,}", " ");
                } while (Regex.IsMatch(sOutputContent, "[ ]{2,}"));
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveUnwantedWordFromAbbrJouTitle", ex.Message, true, "");
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

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HexUnicodeToCharConvertor", ex.Message, true, "");
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

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\Hex2Char", ex.Message, true, "");
            }
            return sHexValue;
        }

        private string HandleFormattingTags(string sRefTaggedContent)
        {
            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"<(i|b)>((?:(?!</?\1>).)+)</\1>", NormalizeFormattingTags);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleFormattingTags", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        private string NormalizeFormattingTags(Match myRefMatch)
        {
            string sTagName = myRefMatch.Groups[1].Value;
            string sInnerContent = myRefMatch.Groups[2].Value;

            try
            {
                //sRefElementsLocalExePtn

                sInnerContent = Regex.Replace(sInnerContent, @"<([bi])></\1>", "");
                if (Regex.IsMatch(sInnerContent, @"<(i|b)>((?:(?!</?\1>).)+)</\1>"))
                {
                    sInnerContent = Regex.Replace(sInnerContent, @"<(i|b)>((?:(?!</?\1>).)+)</\1>", NormalizeFormattingTags);
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

        //Pattern Matching Starts Here
        public string DoReferencePatternMatch(string sRefContent)
        {
            string sRefType = string.Empty;

            try
            {
                sRefContent = Regex.Replace(sRefContent, "</?chapter-title>", "");

                if (Regex.IsMatch(sRefContent, string.Format("^<({0})>", sRefTypeLocalExePtn)))
                {
                    sRefType = Regex.Match(sRefContent, string.Format("^<({0})>", sRefTypeLocalExePtn)).Groups[1].Value;
                }
                else
                {
                    sRefType = "Other";
                }

                //0.Pre Cleanup
                DoPreCleanup4PatternMatch(ref sRefContent);

                //1. Ref Label
                IdentifyRefLabel(ref sRefContent);

                //2. Ref Prefix
                IdentifyRefPrefix(ref sRefContent);

                //3. Et-al
                IdentifyEtAl(ref sRefContent);

                IdentifyThesisKeyword(ref sRefContent);

                IdentifyReportKeyword(ref sRefContent);

                IdentifyPaperKeyword(ref sRefContent);

                IdentifyURL(ref sRefContent);

                IdentifyDOI(ref sRefContent);

                //3. Date information
                IdentifyDateInfo(ref sRefContent);

                IdentifyAccessedDate(ref sRefContent);

                IdentifyEditionNumber(ref sRefContent);

                IdentifyMiscInformation(ref sRefContent);

                if (Regex.IsMatch(sRefContent, "^(?:[\u201C][^\u201C\u201D]+\u201D)"))
                {
                    //Insert Dummy Collab Tag
                    sRefContent = string.Format("<Collab>~~dummy~~</Collab> {0}", sRefContent);
                }
                else
                {
                    //Identify Collab
                    IdentifyCollab(ref sRefContent);
                }

                //4. First Author Group
                IdentifyFirstAuthorGroup(ref sRefContent);

                if (!Regex.IsMatch(sRefContent, "(?:</?(?:Surname|Forename|Suffix|Collab)>)"))
                {
                    IdentifyCollab1(ref sRefContent);
                }

                //added by Dakshinamoorthy on 2020-Dec-18
                ValidateSingleAuthorAsCollab(ref sRefContent);

                //added by Dakshinamoorthy on 2020-Jun-09
                sRefContent = Regex.Replace(sRefContent, @"(?:(?:<RefLabel>(?:(?:(?!</?RefLabel>).)+)</RefLabel>[ ]*)?<Collab>(?:(?:(?!</?Collab>).)+)</Collab>)", "<AuEdGroup>$&</AuEdGroup>");

                IdentifyISBN(ref sRefContent);

                IdentifyEditorGroup(ref sRefContent);

                //added by Dakshinamoorthy on 2020-Jun-04
                ApplyAuEdGroup(ref sRefContent);

                ValidateDateInformation(ref sRefContent);

                ApplyEditorGroup(ref sRefContent);

                //5. Change PubDate to In-house tool comaptible tag "<Year>"
                ChangeYearTag(ref sRefContent);

                //6. Identify PubMedId
                IdentifyPubMedId(ref sRefContent);

                //IdentifyPublisherLocation
                //IdentifyPublisherLocation(ref sRefContent);

                //some misc operations
                sRefContent = Regex.Replace(sRefContent, @"^(?:(?:<Collab>(?:(?!</?Collab>).)+</Collab>)[\:\.;, ]*(?:\[?WWW Document\]?[\.,]* ))", HandleWWWDocument);
                sRefContent = Regex.Replace(sRefContent, @"</Collab>( [0-9]+[\.,]+[ ]*)((?:</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> )?<PubDate>)", "$1</Collab>$2");
                sRefContent = Regex.Replace(sRefContent, @"</Collab>( [0-9A-Z\/\- ]+[\.,]+[ ]*)((?:</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> )?<PubDate>)", "$1</Collab>$2");
                sRefContent = Regex.Replace(sRefContent, @"</Collab>( [0-9a-z\/\- ]+[\.,]+[ ]*)((?:</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> )?<PubDate>)", "$1</Collab>$2");
                sRefContent = Regex.Replace(sRefContent, @"</Collab>( \([A-Z0-9\- ]+\)[ ]*)((?:</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> )?<PubDate>)", "$1</Collab>$2");
                sRefContent = Regex.Replace(sRefContent, @"</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> <(ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>((?:(?!\1).)+)</\1>", " <$1>$2</$1></ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup>");

                NormalizeSpaces(ref sRefContent);

                sRefContent = string.Format("<{0}>{1}</{0}>", sRefType, sRefContent);
            }
            catch (Exception ex)
            {

                //System.Windows.Forms.MessageBox.Show(ex.Message);
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\DoReferencePatternMatch", ex.Message, true, "");
            }
            return sRefContent;
        }


        private bool ApplyAuEdGroup(ref string sRefTaggedContent)
        {
            try
            {
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(<RefLabel>(?:(?:(?!</?RefLabel>).)+)</RefLabel>[ ]*)<AuEdGroup>", "<AuEdGroup>$1");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</AuEdGroup>((?:[ ]*<Etal>(?:(?:(?!</?Etal>).)+)</Etal>))", "$1</AuEdGroup>");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"</AuEdGroup>((?:[ ]*<(ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>(?:(?:(?!</?\2).)+)</\2>))", "$1</AuEdGroup>");
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, @"((?:[ ]*<(ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>(?:(?:(?!</?\2).)+)</\2>)[ ]*)<AuEdGroup>", "<AuEdGroup>$1");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ApplyAuEdGroup", ex.Message, true, "");
                return false;
            }
            return true;
        }

        private string HandleWWWDocument(Match myMatch)
        {
            string sOutputContent = myMatch.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "</?Collab>", "");
                sOutputContent = string.Format("<{0}>{1}</{0}>", "Collab", sOutputContent);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleWWWDocument", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private bool IdentifyMiscInformation(ref string sRefContent)
        {
            try
            {
                string sMiscPattern = string.Empty;

                //Molina, J. B. (<PubDate><ldlyear>2015</ldlyear>).</PubDate> “Evaluación de la eliminación de matériaorgânica y nitrógeno de las aguasresidualesem um reactor biopelícula de membrana tubular aireada.” <ldlThesisKeyword>Doctoral thesis</ldlThesisKeyword>, University of Coruña, Spain (in Spanish).
                sMiscPattern = string.Format(@"((?:(?<=[ ])\((?:[Ii]n )?{0}\)[\.;]*)$)", sNationalityPattern);
                sRefContent = Regex.Replace(sRefContent, sMiscPattern, string.Format("<{0}>{1}</{0}>", "ldlMisc", "$&"));

                //Clini, C., I. Musu, and M. L. Gullino, eds. <PubDate><ldlyear>2008</ldlyear>.</PubDate> <i>Sustainable development and environmental management: Experiences and case studies.</i> [In Chinese.] Dordrecht, Netherlands: Springer.
                sMiscPattern = string.Format(@"((?:(?<=[ ])\[(?:[Ii]n )?{0}\.?\][\.;]*))", sNationalityPattern);
                sRefContent = Regex.Replace(sRefContent, sMiscPattern, string.Format("<{0}>{1}</{0}>", "ldlMisc", "$&"));

                //Bowles, John R. <PubDate><ldlyear>1985</ldlyear>.</PubDate> “Suicide and Attempted Suicide in Contemporary Western Samoa”. In <i>Culture, Youth and Suicide in the Pacific: Chapters from an East-West Center Conference Center for Pacific Islands Studies</i>, edited by F. X. Hezel, D. H. Rubinstein, and G. M. White, 15-35. Working Paper series. Honolulu, Hawaii: Pacific Islands Studies Program, Center for Asian and Pacific Studies, University of Hawaii at Manoa.
                sMiscPattern = @"(?:(?<=[\.>,;] )(?:Working Paper series[.;,]+))";
                sRefContent = Regex.Replace(sRefContent, sMiscPattern, string.Format("<{0}>{1}</{0}>", "ldlMisc", "$&"));

                //<RefLabel>38.</RefLabel> The Chartered Society of Physiotherapy. <PubDate><ldlyear>2014</ldlyear>.</PubDate> The falls prevention economic model. [ONLINE] <ldlURLLabel>Available at:</ldlURLLabel> <Website>http://www.csp.org/documents/falls-prevention-economic-model.</Website> [<ldlAccessedDateLabel>Accessed</ldlAccessedDateLabel> <AccessedDate><ldldayNum>1</ldldayNum> <ldlmonthName>August</ldlmonthName> <ldlyear>2019</ldlyear>].</AccessedDate>
                sMiscPattern = @"(?:(?<=[\.>,;] )(?:\[ONLINE\][.;, ]*))";
                sRefContent = Regex.Replace(sRefContent, sMiscPattern, string.Format("<{0}>{1}</{0}>", "ldlMisc", "$&"), RegexOptions.IgnoreCase);

                //Durnova, A. and Zittoun, P. <PubDate>(<ldlyear>2013</ldlyear>),</PubDate> Les approches discursives des politiques publiques. <i>Revue francx̧aise de science politique, 63</i>, 569-577 [English translation]. <Doi>DOI 10.3917/rfsp.633.0569.</Doi>
                sMiscPattern = @"(?:(?<=[\.>,;0-9] )(?:\[English translation\][\.;, ]*))";
                sRefContent = Regex.Replace(sRefContent, sMiscPattern, string.Format("<{0}>{1}</{0}>", "ldlMisc", "$&"), RegexOptions.IgnoreCase);

                //A. Beilinson and A. Levin, The elliptic polylogarithm, in <i>Motives: Proceedings of Symposia in Pure Mathematics</i> (U. Jannsen, Editor), vol. 55, Part 2 (Springer, Berlin, <PubDate><ldlyear>1994</ldlyear>),</PubDate> 123–190.
                sMiscPattern = @"(?:(?<=, )[Pp]art [0-9]+(?= [\(\[]))";
                sRefContent = Regex.Replace(sRefContent, sMiscPattern, string.Format("<{0}>{1}</{0}>", "ldlMisc", "$&"), RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyMiscInformation", ex.Message, true, "");
            }
            return true;
        }

        private bool IdentifyEditionNumber(ref string sRefContent)
        {
            try
            {
                NormalizeSpaces(ref sRefContent);
                string sAccessLabelPattern = string.Empty;

                string sOne_to_9_Pattern = @"(?<one_to_9>(?:first|second|third|fourth|fifth|sixth|seventh|eighth|ninth))";
                string sTen_to_19_Pattern = @"(?<ten_to_19>(?:tenth|eleventh|twelfth|thirteenth|fourteenth|fifteenth|sixteenth|seventeenth|eighteenth|nineteenth))";
                string sTwoDigitPrefix_Pattern = @"(?<two_digit_prefix>(?:s(?:even|ix)|t(?:hir|wen)|f(?:if|or)|eigh|nine)ty)";
                string sTens_Pattern = @"(?<tens>(?:twentieth|thirtieth|fortieth|fiftieth|sixtieth|seventieth|eightieth|ninetieth))";
                string sOne_to_99_Pattern = string.Format(@"(?<one_to_99>(?:{0})(?:[- ](?:{1}))?|(?:{2})|(?:{1})|{3})", sTwoDigitPrefix_Pattern, sOne_to_9_Pattern, sTen_to_19_Pattern, sTens_Pattern);

                string sEditionOrdinalNumber = string.Format("(?:(?:{0})(?:{1})(?:{2}))", @"(?<=(?:[\.,;\u201D\u2019]|\)|\>) )", sOne_to_99_Pattern, @"(?:[ ]*[Rr]ev(?:ised)?\.?)?[ ]+(?:[Ee]d(?:itio)?n?)\b\.?(?:[,;\:\) ]*|(?=[<]))");

                //updated by Dakshinamoorthy on 2020-Jul-23
                //sRefContent = Regex.Replace(sRefContent, @"(?:(?<=(?:[ ]|\>|\()(?<!(?:\: )))(?:(?:(?:[0-9]+|<i>[0-9]+</i>)[ ]?(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?)?(?:[ ]*[Rr]ev(?:ised)?\.?)?[ ]+(?:[Ee]d(?:itio)?n?)\b\.?(?:[,;\:\) ]*|(?=[<]))))",
                //string.Format("<{0}>{1}</{0}>", "ldlEditionNumber", "$&"));

                sRefContent = Regex.Replace(sRefContent, @"(?:(?<=(?:[ ]|\>[\.,;]*|\()(?<!(?:\: )))(?:[0-9]+[ ]?(?:<sup>)?(?:st|nd|rd|th|ST|ND|RD|TH)\.?(?:</sup>)?(?:[ ]+[Rr]ev(?:ised)?\.?)?[ ]+(?:(?:[Ee]d(?:itio)?n?)\b|(?:[Ee]dit)\b)\.?(?:[,;\:\) ]*|(?=[<]))))",
                string.Format("<{0}>{1}</{0}>", "ldlEditionNumber", "$&"));

                //added by Dakshinamoorthy on 2019-Feb-26
                sRefContent = Regex.Replace(sRefContent, "\\(<ldlEditionNumber>", "<ldlEditionNumber>(", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, sEditionOrdinalNumber, string.Format("<{0}>{1}</{0}>", "ldlEditionNumber", "$&"), RegexOptions.IgnoreCase);

                sRefContent = HandleFormattingTags(sRefContent);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyEditionNumber", ex.Message, true, "");
            }
            return true;
        }


        private bool IdentifyAccessedDate(ref string sRefContent)
        {
            try
            {
                NormalizeSpaces(ref sRefContent);
                string sAccessLabelPattern = string.Empty;

                sRefContent = Regex.Replace(sRefContent, @"(\(?(?:[Aa]ccessed|Retrieved|[Dd]ownloaded|[Pp]osted)[\:]?(?: on| in| during)? )<PubDate>((?:(?!</?PubDate>).)+)</PubDate>",
                    string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "ldlAccessedDateLabel", "$1", "AccessedDate", "$2"));


                if (!Regex.IsMatch(sRefContent, "</?(?:ldlAccessedDateLabel|AccessedDate)>"))
                {
                    sRefContent = Regex.Replace(sRefContent, @"(?<=(?:</Website>[\:\.;, ]*))<PubDate>((?:(?!</?PubDate>).)+)</PubDate>",
                    string.Format("<{0}>{1}</{0}>", "AccessedDate", "$1"));

                }

                sRefContent = Regex.Replace(sRefContent, @"([\[\(])<ldlAccessedDateLabel>", "<ldlAccessedDateLabel>$1");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyAccessedDate", ex.Message, true, "");
            }
            return true;
        }



        private bool IdentifyISBN(ref string sRefContent)
        {
            try
            {
                string sISBNPattern = @"(?:(?<=[ >])(?:(?:Print )?ISBN(?:10|13)?[\: ]+[0-9][^ ]+))";
                sRefContent = Regex.Replace(sRefContent, sISBNPattern, string.Format("<{0}>{1}</{0}>", "ldlISBNNumber", "$&"));
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyISBN", ex.Message, true, "");
            }
            return true;
        }

        private bool EscapeContentFromPublisherLocation(ref string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "(?:(?:[\u201c][^\u201c\u201d]+[\u201d])|(?:[\u2018][^\u2018\u2019]+[\u2019]))", HandleEscapeSpace);
                sRefContent = Regex.Replace(sRefContent, "(?:(?:[\u201c][^\u201c\u201d]+[\u201d])|(?:[\u2018][^\u2018\u2019]+[\u2019]))", HandleEscapeParenthesis);

                sRefContent = Regex.Replace(sRefContent, @"(?<=[a-z] )\([^\(\)]+\)(?= [a-z])", HandleEscapeParenthesis);
                sRefContent = Regex.Replace(sRefContent, "(<Collab>(?:(?!</?Collab>).)+</Collab>)", HandleEscapeParenthesis);
                sRefContent = Regex.Replace(sRefContent, @"((?:(?:[A-Z][a-z]*\. ){2,}(?:[A-Z][a-z]*\.)))", HandleEscapeSpace);
                sRefContent = Regex.Replace(sRefContent, @"<(ldl[^<>]+)>(?:(?!</\1>).)+</\1>", HandleEscapeSpace);

                sRefContent = Regex.Replace(sRefContent, "(<Doi>(?:(?!</?Doi>).)+</Doi>)", HandleEscapeSpace);
                sRefContent = Regex.Replace(sRefContent, "(<Doi>(?:(?!</?Doi>).)+</Doi>)", HandleEscapeParenthesis);

                sRefContent = Regex.Replace(sRefContent, @"(?!<=(?:<PubDate>(?:(?!</?PubDate>).)+</PubDate>)[\:\.;, ]*)(?:<i>(?:(?!</?i>).){5,}</i>)", HandleEscapeSpace);
                sRefContent = Regex.Replace(sRefContent, @"(?!<=(?:<PubDate>(?:(?!</?PubDate>).)+</PubDate>)[\:\.;, ]*)(?:<i>(?:(?!</?i>).){5,}</i>)", HandleEscapeParenthesis);

                sRefContent = Regex.Replace(sRefContent, @"(?:(?<= )Vol(?:ume)?\. [0-9]+ of )", HandleEscapeSpace);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\EscapeContentFromPublisherLocation", ex.Message, true, "");
            }
            return true;
        }

        private string HandleEscapeParenthesis(Match mySpaceMatch)
        {
            string sOutputContent = mySpaceMatch.Value.ToString();

            try
            {
                sOutputContent = Regex.Replace(sOutputContent, @"\(", "~~o_parenthesis~~");
                sOutputContent = Regex.Replace(sOutputContent, @"\)", "~~c_parenthesis~~");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleEscapeSpace", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private string HandleEscapeSpace(Match mySpaceMatch)
        {
            string sOutputContent = mySpaceMatch.Value.ToString();

            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "[ ]", "~~space~~");
                sOutputContent = Regex.Replace(sOutputContent, @"\.", "~~dot~~");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleEscapeSpace", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private bool EscapeDot(ref string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, @"Univ\.", "Univ~~dot~~");
                sRefContent = Regex.Replace(sRefContent, @"Inst\.", "Inst~~dot~~");
                sRefContent = Regex.Replace(sRefContent, @"Acad\.", "Acad~~dot~~");
                sRefContent = Regex.Replace(sRefContent, @"St\.", "St~~dot~~");
                sRefContent = Regex.Replace(sRefContent, @"\.net", "~~dot~~net");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\EscapeDot", ex.Message, true, "");
            }
            return true;
        }

        private bool IdentifyThesisKeyword(ref string sRefContent)
        {
            try
            {

                string sLookAheadPattern = @"(?=[,;\:\.\)]*(?:[,;\:\.\)]+|(?:[ ]\()| (?:\(?<(?:PubDate|ldl[a-zA-Z]+)>)|\)|~~dot~~[\:;, ]*<[a-zA-Z]+>| [0-9]+| (?:(?:[Pp]p?|[Pp]ages?)[\:. ]+[0-9]+)|(?:[\.; ]*$)))";
                string sLookBehindPattern = @"(?<=(?:(?:(?:[>\.,\u201d\u2019]+ )|(?:[\)\]][\:\.;,]* ))))";
                string sThesisKeywordPattern = @"(?:(?:<i>)?(?:(?:[\(\[])?(?:(?:(?:Doctoral|Doctorate|M\.[ ]?S\.|M\.Sc\.|M[Ss]c|Master|MS|Ph\.D\.|PhD\.?) [Tt]hesis)|(?:Dissertation)|M[Ss] [Tt]hesis|(?:Ph\.D\. Dissertation)|(?:Doctoral [Dd]issertation)|Thesis \(PhD\)|(?:PhD diss)|(?:Ph\.D\. [Dd]issertation)|(?:PhD\/MSc thesis)|(?:PhD Dissertation)|(?:Ph\.D\.)|(?:MSc)|(?:M\.A\.Sc\.)|(?:M\.Sc [Tt]hesis)|(?:[Mm]aster[\u2019']s thesis)|(?:Doctor of Philosophy)|(?:Ph\.[ ]?[Dd][\.,]+ [Tt]hesis)|M\.[ ]?Sc\. \([Ee]ngg\) thesis|(?:Unpublished [Dd]octoral (?:[Tt]hesis|[Dd]issertation))))(?:[\)\]])?(?:</i>)?)";
                string sThesisKeywordFullPattern = string.Format(@"(?:{0})(?:{1})(?:{2})", sLookBehindPattern, sThesisKeywordPattern, sLookAheadPattern);

                if (Regex.IsMatch(sRefContent, sThesisKeywordFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sThesisKeywordFullPattern, string.Format("<{0}>{1}</{0}>", "ldlThesisKeyword", "$&"));
                }

                sRefContent = Regex.Replace(sRefContent, @"<ldlThesisKeyword></ldlThesisKeyword>", "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyThesisKeyword", ex.Message, true, "");
            }
            return true;
        }

        //added by Dakshinamoorthy on 2020-Jul-08
        private bool IdentifyPaperKeyword(ref string sRefContent)
        {
            try
            {
                //updated by Dakshinamoorthy on 2020-Dec-16
                //string sPaperKeywordPtn = @"(?:(?:(?<=[, ])(?:[A-Z]{4} Paper No\.[ ]?[0-9]+)[\.\;])(?=[ <]))";
                string sPaperKeywordPtn = @"(?:(?:(?<=[,\.] )(?:(?:[A-Z]{4} Paper No\.[ ]?[0-9]+)|(?:Working Paper))[\.\;,])(?=[ <]))";

                sRefContent = Regex.Replace(sRefContent, sPaperKeywordPtn, string.Format("<{0}>{1}</{0}>", "ldlPaperKeyword", "$&"));
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyPaperKeyword", ex.Message, true, "");
                return false;
            }
            return true;
        }

        private bool IdentifyReportKeyword(ref string sRefContent)
        {
            try
            {

                string sLookAheadPattern = @"(?=[,;\:\.]*(?:[,;\:\.]+| (?:\(?<(?:PubDate|ldl[a-zA-Z]+)>)|\)|~~dot~~[\:;, ]*<[a-zA-Z]+>| [0-9]+| (?:(?:[Pp]p?|[Pp]ages?)[\:. ]+[0-9]+)|(?:[\.; ]*$)))";
                string sLookBehindPattern = @"(?<=(?:(?:(?:[>\.,\u201d\u2019]+ )|(?:[\)\]][\:\.;,]* ))))";
                string sReportKeywordPattern = @"(?:(?:(?:<i>)?(?:[^\.,\:,<>\(\)\[\]\u201c\u201d\u2018\u2019]*(?: (?:Report No[\:.]|(?:Technical )?Rep\.|) [^\.,]+))(?:</i>)?)|(?:[Tt]echnical [Rr]eport))";
                string sReportKeywordFullPattern = string.Format(@"(?:{0})(?:{1})(?:{2})", sLookBehindPattern, sReportKeywordPattern, sLookAheadPattern);

                //sRefContent = Regex.Replace(sRefContent, sReportKeywordFullPattern, string.Format("<{0}>{1}</{0}>", "ldlReportKeyword", "$&"));
                sRefContent = Regex.Replace(sRefContent, sReportKeywordFullPattern, string.Format("<{0}>{1}</{0}>", "ldlReportNumber", "$&"));
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyReportKeyword", ex.Message, true, "");
            }
            return true;
        }

        private bool IsJournalSymptomsFound(string sRefContent)
        {
            try
            {
                bool bIsArticleTitleFound = false;
                bool bIsJournalTitleFound = false;
                bool bIsVolumeNumberFound = false;
                bool bIsIssueNumberFound = false;
                bool bIsPageRangeFound = false;
                bool bIsEditionNumberFound = false;

                bIsArticleTitleFound = CheckArticleTitleFound(sRefContent);
                bIsJournalTitleFound = CheckJournalTitleFound(sRefContent);
                bIsVolumeNumberFound = CheckVolumeNumberFound(sRefContent);
                bIsIssueNumberFound = CheckIssueNumberFound(sRefContent);
                bIsPageRangeFound = CheckPageNumberFound(sRefContent);
                bIsEditionNumberFound = CheckEditionNumberFound(sRefContent);

                if (bIsArticleTitleFound && bIsJournalTitleFound && bIsVolumeNumberFound && bIsIssueNumberFound && bIsPageRangeFound)
                {
                    return true;
                }
                else if ((bIsArticleTitleFound || bIsJournalTitleFound) && bIsVolumeNumberFound && bIsIssueNumberFound)
                {
                    return true;
                }
                else if (bIsVolumeNumberFound && bIsIssueNumberFound && bIsPageRangeFound)
                {
                    return true;
                }
                else if (bIsArticleTitleFound && bIsJournalTitleFound && bIsVolumeNumberFound && bIsPageRangeFound && bIsEditionNumberFound == false)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsJournalSymptomsFound", ex.Message, true, "");
            }
            return false;
        }

        private bool RevertEscapedSpace4Collab(ref string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "<ldlFirstAuEdCollabGroup>(?:(?!</?ldlFirstAuEdCollabGroup>).)+</ldlFirstAuEdCollabGroup>", HandleRevertEscapedSpace4Collab);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RevertEscapedSpace4Collab", ex.Message, true, "");
            }
            return true;
        }

        private string HandleRevertEscapedSpace4Collab(Match myMatch)
        {
            string sOutputContent = myMatch.Value.ToString();

            try
            {
                sOutputContent = Regex.Replace(sOutputContent, @"<(Author|Editor|Collab)>(?:(?!</?\1>).)+</\1>", SaveSpaceInAuEdCollab);
                sOutputContent = Regex.Replace(sOutputContent, "~~space~~", " ");
                sOutputContent = Regex.Replace(sOutputContent, "~~a_space~~", "~~space~~");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleRevertEscapedSpace4Collab", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private string SaveSpaceInAuEdCollab(Match myMatch)
        {
            string sOutputContent = myMatch.Value.ToString();

            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "~~space~~", "~~a_space~~");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\SaveSpaceInAuEdCollab", ex.Message, true, "");
            }
            return sOutputContent;
        }



        private bool IdentifyPublisherLocation(ref string sRefContent)
        {
            try
            {
                if (IsJournalSymptomsFound(sRefContent) == true)
                {
                    return true;
                }

                //added by Dakshinamoorthy on 2020-Jul-13
                //Get Local Location Details
                if (string.IsNullOrEmpty(AutoStructRefAuthor.sCityXmlContent) == false)
                {
                    docCityXml = new XmlDocument();
                    docCityXml.LoadXml(AutoStructRefAuthor.sCityXmlContent);
                }

                //if (string.IsNullOrEmpty(AutoStructRefAuthor.sPublisherXmlContent) == false)
                //{
                //    docPublisherXml = new XmlDocument();
                //    docPublisherXml.LoadXml(AutoStructRefAuthor.sPublisherXmlContent);
                //}

                //pre-cleanup
                sRefContent = Regex.Replace(sRefContent, "</?PublisherName>", "");
                sRefContent = Regex.Replace(sRefContent, "</?PublisherLocation>", "");
                //sRefContent = Regex.Replace(sRefContent, "</?Vol_No>", "");

                NormalizeSpaces(ref sRefContent);

                EscapeDot(ref sRefContent);
                EscapeContentFromPublisherLocation(ref sRefContent);
                //added by Dakshinamoorthy on 2020-Jul-13
                //sRefContent = Regex.Replace(sRefContent, @"(?:(?<=(?:</PubDate>))(?:[^\.<>]+(?=\.(?:[ ]|<))))", HandleEscapeSpace);
                sRefContent = Regex.Replace(sRefContent, @"(?:(?<=(?:</PubDate>))(?:[^\.,<>\u201C\u201D\u2018\u2019]+(?=,(?:[ ]|<))))", HandleEscapeSpace);
                sRefContent = Regex.Replace(sRefContent, @"(?:<(ldlEditorDelimiterEds_Back|dlEditorDelimiterEds_Front)>(?:(?:(?!</?\1>).)+)</\1>)", HandleEscapeParenthesis);

                RevertEscapedSpace4Collab(ref sRefContent);

                //updated by Dakshinamoorthy on 2019-Jan-11
                //string sPublisherPattern = @"(?<publisher>(?:(?:(?:(?:[a-z]+ ?)?(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,}(?: (?:[Ii]nc|[Ll]td)[\.,]*)?)|(?:[A-Z]{3,})|(?:(?:[A-Z]\.? ?){1,} (?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,}))(?:, (?:[Ii]nc|[Ll]td)[\.,]*|\. [A-Za-z]+)?)\.?)";
                //updated by Dakshinamoorthy on 2019-Dec-10
                //string sPublisherPattern = @"(?<publisher>(?:(?:(?:[A-Z]{2,} \([A-Za-z ]{5,}\))|(?:(?:[a-z]+ ?)?(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,}(?: (?:[Ii]nc|[Ll]td)[\.,]*)?)|(?:[A-Z]{3,})|(?:(?:[A-Z]\.? ?){1,} (?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,}))(?:, (?:[Ii]nc|[Ll]td)[\.,]*|\. [A-Za-z]+)?)\.?)";
                //updated by Dakshinamoorthy on 2020-Dec-01
                //string sPublisherPattern = @"(?<publisher>(?:(?:(?:(?:(?:[A-Z]{2,} \([A-Za-z ]{5,}\))|(?:(?:[a-z]+ ?)?(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,}(?: (?:[Ii]nc|[Ll]td)[\.,]*)?)|(?:[A-Z]{3,})|(?:(?:[A-Z]\.? ?){1,} (?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,}))(?:, (?:[Ii]nc|[Ll]td)[\.,]*|\. [A-Za-z]+)?)\.?)|(?:<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>)))";

                string sPublisherPattern = @"(?<publisher>(?:(?:(?:(?:(?:[A-Z]{2,} \([A-Za-z ]{5,}\))|(?:(?:[a-z]+ ?)?(?:[A-Z])[^0-9<>\(\)\?\.,\:;\u201c\u201d]{3,}(?: (?:[Ii]nc|[Ll]td)[\.,]*)?)|(?:[A-Z]{3,})|(?:(?:[A-Z]\.? ?){1,} (?:[A-Z])[^0-9<>\(\)\?\.,\:;\u201c\u201d]{3,}))(?:, (?:[Ii]nc|[Ll]td)[\.,]*)?)\.?)|(?:<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>)))";

                string sPublisherPattern_Italic = @"(?<publisher>(?:<i>)(?:(?:(?:(?:[a-z]+ ?)?(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,}(?: (?:[Ii]nc|[Ll]td)[\.,]*)?)|(?:[A-Z]{3,})|(?:(?:[A-Z]\. )(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,}))(?:, (?:[Ii]nc|[Ll]td)[\.,]*|\. [A-Za-z]+)?)\.?(?:</i>))";

                string sUniversityNamePattern = @"(?<university>(?:[A-Za-z&\- ]*\b(?:University|Univ(?:\.|~~dot~~)|Institute|Institution|Ministry|Council|College)[A-Za-z&\-\(\) ]*))";
                string sCenterNamePattern = @"(?<center>(?:[A-Za-z&\- ]*\b(?:Research (?:Center|Centre)|(?:Center|Centre) for |Administration)[A-Za-z&\- ]*)|(?:(?:[A-Za-z ]+ Centre)))";
                string sFacultyNamePattern = @"(?<faculty>(?:Faculty of [A-Za-z\- ]+))";

                //updated by Dakshinamoorthy on 2019-Jan-11
                //string sDepartmentNamePattern = @"(?<department>(?:[A-Za-z&\- ]*\b(?:Department|Institution|Office|Dept(?:\.|~~dot~~)) of [A-Za-z&\-\(\) ]+))";
                //string sDepartmentNamePattern = @"(?<department>(?:(?:[A-Za-z&\- ]*\b(?:Department|Institution|Office|Dept(?:\.|~~dot~~)) of [A-Za-z&\-\(\) ]+)|(?:[A-Za-z]+ ){2,}(?:Department|Institution|Office|Dept(?:\.|~~dot~~))))";
                //updated by Dakshinamoorthy on 2020-Dec-12
                string sDepartmentNamePattern = @"(?<department>(?:(?:(?:(?:[A-Za-z&\- ]*)|(?:(?:[A-Z]\.?[ ]?){2,4}))\b(?:Department|Institution|Office|Dept(?:\.|~~dot~~)) of [A-Za-z&\-\(\) ]+)|(?:[A-Za-z]+ ){2,}(?:Department|Institution|Office|Dept(?:\.|~~dot~~))))";

                string sSchoolNamePattern = @"(?<school>(?:[A-Za-z&\- ]*\b(?:School )[A-Za-z&\- ]*))";
                string sPressNamePattern = @"(?<Press>(?:(?:[A-Za-z]+) (?:[A-Za-z]+ )*(?:Press)))";

                //updated by Dakshinamoorthy on 2020-Dec-01
                //string sCityPattern = @"(?:(?<city>(?:(?:(?:(?:(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,})|(?:(?:[A-Z]\. )(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,}))))))";
                string sCityPattern = @"(?:(?<city>(?:(?:(?:(?:(?:[A-Z])[^0-9<>\?\(\)\.,\:;\u201c\u201d]{3,})|(?:(?:[A-Z]\. )(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,}))))))";

                //added by Dakshinamoorthy on 2020-Aug-11
                string sCityPattern_Multiple = string.Format("(?<city_m>(?:{0}; {0}))", Regex.Replace(sCityPattern, @"(?:\(\?<city>)", "(?:"));

                //added by Dakshinamoorthy on 2020-Dec-01
                //string sStatePattern = @"(?<state>(?:(?:(?:(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,})|(?:(?:[A-Z]\. )(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,})|(?:[A-Z]\.? ?){2,3})))";
                string sStatePattern = @"(?<state>(?:(?:(?:(?:[A-Z])[^0-9<>\(\)\?\.,\:;\u201c\u201d]{3,})|(?:(?:[A-Z]\. )(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,})|(?:[A-Z]\.? ?){2,3})))";

                string sCountryPattern = @"(?<country>(?:(?:(?:(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,})|(?:[A-Z]\.? ?){2,}|(?:(?:[A-Z]\. )(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,}))))";
                string sCityStateCombined = @"(?:(?<city>(?:(?:(?:(?:(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,})|(?:(?:[A-Z]\. )(?:[A-Z])[^0-9<>\(\)\.,\:;\u201c\u201d]{3,})))))) (?<state>(?:(?:[A-Z]\.? ?){2,3})|(?:\((?:[A-Z]\.? ?){2,3}\)))";

                //updated by Dakshinamoorthy on 2019-Sep-13
                //string sLookAheadPattern = @"(?=[,;\:\.]*(?:(?:</[bi]>)?[,;\:\.]+| (?:\(?<PubDate|ldl[a-zA-Z]+)>)|\)|~~dot~~[\:;, ]*<[a-zA-Z]+>| [0-9]+| (?:(?:[Pp]p?|[Pp]ages?)[\:. ]+[0-9]+)|(?:[\.; ]*</(?:ldlAuthorEditorGroup)>)|(?:[\.; ]*(?:</(?:(?:Book|Communication|Conference|Journal|Other|Patent|References|Report|Thesis|Web)>)?$)))";
                string sLookAheadPattern = @"(?=[,;\:\.]*(?:(?:</[bi]>)?[,;\:\.]+| (?:\(?<(?:PubDate|ldl[a-zA-Z]+)>)|\)|~~dot~~[\:;, ]*<[a-zA-Z]+>| [0-9]+| (?:(?:[Pp]p?|[Pp]ages?)[\:. ]+[0-9]+)|(?:[\.; ]*</(?:ldlAuthorEditorGroup)>)|(?:[\.; ]*(?:</(?:(?:Book|Communication|Conference|Journal|Other|Patent|References|Report|Thesis|Web)>)?$))))";

                //updated by Dakshinamoorthy on 2020-Dec-01
                //string sLookBehindPattern = @"(?<=(?:(?:[\(\[])|(?:[>\.,\u201D\u2019\:;]+ )|(?:[\)\]][\:\.;,]* )))";
                string sLookBehindPattern = @"(?<=(?:(?:[\(\[])|(?:[>\.,\u201D\u2019\:;\?]+ )|(?:[\)\]][\:\.;,]* )))";

                string sRefTypeClosePattern = @"(?:</(?:Book|Communication|Conference|Journal|Other|Patent|References|Report|Thesis|Web)>)";



                string sFullPattern = string.Empty;
                int nPatternId = 0;

                List<Tuple<string, int, int>> lstPublisherLocationPattern = new List<Tuple<string, int, int>>();

                bool isPubLocTest = false;

                if (isPubLocTest)
                {
                    //City, State Combined
                    nPatternId = 5;
                    //sFullPattern = string.Format(@"(?:(?<=(?:\(|[>\.,]+ )){0})\: (?:{1}(?=(?:[,;\.]? (?:\(?<PubDate>|[0-9]+))))", sCityPattern, sPublisherPattern);
                    sFullPattern = string.Format(@"(?:{0}{1}[\:,] {2}, {3}{4})", sLookBehindPattern, sPublisherPattern, sCityPattern, sCountryPattern, sLookAheadPattern);
                    lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 1));

                    goto LBL_END_PTN;
                }

                //===================
                //Element Count = 1
                //===================

                //publisher
                nPatternId = 1;
                sFullPattern = string.Format(@"(?:{0}{1}{2})", sLookBehindPattern, sPublisherPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 1, 1));

                ////publisher
                //nPatternId = 1;
                //sFullPattern = string.Format(@"(?<=\)?[\:.,;]* )({0})(?=[\.,]*(?:[ ]|$))", sPublisherPattern);
                //lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 1, 1));

                //===================
                //Element Count = 2
                //===================

                //publisher, city
                nPatternId = 1;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}{3})", sLookBehindPattern, sPublisherPattern, sCityPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 1));

                //publisher, country
                nPatternId = 2;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}{3})", sLookBehindPattern, sPublisherPattern, sCountryPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 1));

                //publisher, state
                nPatternId = 2;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}{3})", sLookBehindPattern, sPublisherPattern, sStatePattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 1));

                //publisher, city
                nPatternId = 3;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}{3})", sLookBehindPattern, sPublisherPattern, sCityPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 1));

                //city, publisher
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}[\:,] {2}{3})", sLookBehindPattern, sCityPattern, sPublisherPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 1));

                //city_multiple, publisher
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}[\:,] {2}{3})", sLookBehindPattern, sCityPattern_Multiple, sPublisherPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));

                //City, State Combined
                nPatternId = 5;
                sFullPattern = string.Format(@"(?:{0}{1}{2})", sLookBehindPattern, sCityStateCombined, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 1));

                //city, state
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}[,] {2}{3})", sLookBehindPattern, sCityPattern, sStatePattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 1));

                //publisher, city
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}[\:,] {2}{3})", sLookBehindPattern, sPublisherPattern, sCityPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 1));

                //state, publisher
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}[\:,] {2}{3})", sLookBehindPattern, sStatePattern, sPublisherPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 1));


                //===================
                //Element Count = 3
                //===================

                //publisher, state, country
                nPatternId = 5;
                sFullPattern = string.Format(@"(?:{0}{1}[,] {2}[,] {3}{4})", sLookBehindPattern, sPublisherPattern, sStatePattern, sCountryPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));

                //city, state, country
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}, {3}{4})", sLookBehindPattern, sCityPattern, sStatePattern, sCountryPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));

                //city, state, publisher
                nPatternId = 1;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}[\:,] {3}{4})", sLookBehindPattern, sCityPattern, sStatePattern, sPublisherPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));


                //city, state, publisher
                nPatternId = 1;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}[\:,] {3}{4})", sLookBehindPattern, sCityPattern, sStatePattern, sPublisherPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));

                //city, state, publisher
                nPatternId = 1;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}[\.]?[\:] {3}{4})", sLookBehindPattern, sCityPattern, sStatePattern, sPublisherPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));

                //city, country, publisher
                nPatternId = 1;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}[\:,] {3}{4})", sLookBehindPattern, sCityPattern, sCountryPattern, sPublisherPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));

                //city, country, publisher_italic
                nPatternId = 1;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}[\:,] {3}{4})", sLookBehindPattern, sCityPattern, sCountryPattern, sPublisherPattern_Italic, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));

                //publisher, city, country
                nPatternId = 2;
                sFullPattern = string.Format(@"(?:{0}{1}[\:,] {2}, {3}{4})", sLookBehindPattern, sPublisherPattern, sCityPattern, sCountryPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));

                // publisher_italic, city, country
                nPatternId = 2;
                sFullPattern = string.Format(@"(?:{0}{1}[\:,] {2}, {3}{4})", sLookBehindPattern, sPublisherPattern_Italic, sCityPattern, sCountryPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));

                //publisher, city, country
                nPatternId = 3;
                sFullPattern = string.Format(@"(?:{0}{1}[\:,] {2}, {3}{4})", sLookBehindPattern, sPublisherPattern, sCityPattern, sCountryPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));

                //publisher, city, country
                nPatternId = 3;
                sFullPattern = string.Format(@"(?:{0}{1}[\:,] {2}, {3}{4})", sLookBehindPattern, sPublisherPattern, sCityPattern, sCountryPattern, "(?=(?:[ ]?<ldlMisc>))");
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));

                //publisher, city, state
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}[,\:] {2}, {3}{4})", sLookBehindPattern, sPublisherPattern, sCityPattern, sStatePattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));


                //publisher, city, state
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}[,\:] {2}, {3}{4})$", sLookBehindPattern, sPublisherPattern, sCityPattern, sStatePattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));


                //publisher, city, state
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}[,\:] {2}[,\.] {3}{4})", sLookBehindPattern, sPublisherPattern_Italic, sCityPattern, sStatePattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));



                ////publisher, city, city
                //nPatternId = 5;
                //sFullPattern = string.Format(@"(?:(?<=(?:\(|[>\.,]+ )){0}), ({1}), ({2}(?=(?:[,]? \(?<PubDate>)))", sPublisherPattern, sCityPattern, sCityPattern);
                //lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 3, 1));

                //===================
                //Element count = 4
                //===================

                sFullPattern = string.Format(@"(?:{0}{1}[,\:] {2}, {3}, {4}{5})", sLookBehindPattern, sPublisherPattern, sCityPattern, sStatePattern, sCountryPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 4, 1));

                //***********************************************
                //Location Only
                //***********************************************


                //city, country
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}{3})", sLookBehindPattern, sCityPattern, sCountryPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 3));

                //city, state
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}{3})", sLookBehindPattern, sCityPattern, sStatePattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 3));

                //city, country
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}, {2}{3})", sLookBehindPattern, sCityPattern, sCountryPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 3));

                //city, publisher
                nPatternId = 4;
                sFullPattern = string.Format(@"(?<=(?:<i>(?:(?!</?i>).)+</i>[\:\.;,]* )){0}\: {1}\.?(?={2})", sCityPattern, sPublisherPattern, sRefTypeClosePattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 3));

                //city, publisher
                nPatternId = 4;
                //updated by Dakshinamoorthy on 2020-Dec-01
                //sFullPattern = string.Format(@"(?<=(?:[^\.<>]{{5,}}\.] )){0}\: {1}\.?(?={2})", sCityPattern, sPublisherPattern, sRefTypeClosePattern);
                sFullPattern = string.Format(@"(?<=(?:[^\.<>]{{5,}}[\.\?]+ )){0}\: {1}\.?(?={2})", sCityPattern, sPublisherPattern, sRefTypeClosePattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 2, 3));


                //***********************************************
                //Single Location Only
                //***********************************************

                //country
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}{2})", @"(?<=</(?:ldlConferenceName|ldlConferenceDate|ldlEditorDelimiterEds_Back|ldlCity|ldlPublisherName|i)>[\:\.;, ]*)", sCountryPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 1, 1));

                //city
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}{1}{2})", @"(?<=</(?:ldlConferenceName|ldlEditorDelimiterEds_Back|ldlCity|ldlPublisherName|i)>[\:\.;, ]*)", sCityPattern, sLookAheadPattern);
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 1, 1));

                //city
                nPatternId = 4;
                sFullPattern = string.Format(@"(?:{0}: {1})", sCityPattern, @"(?=(?:<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>))");
                lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 1, 1));





            ////city
            //nPatternId = 4;
            //sFullPattern = string.Format(@"(?:{0}{1}{2})", sLookBehindPattern, sCityPattern, sLookAheadPattern);
            //lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 1, 4));

            ////state
            //nPatternId = 4;
            //sFullPattern = string.Format(@"(?:{0}{1}{2})", sLookBehindPattern, sStatePattern, sLookAheadPattern);
            //lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 1, 4));

            ////country
            //nPatternId = 4;
            //sFullPattern = string.Format(@"(?:{0}{1}{2})", sLookBehindPattern, sCountryPattern, sLookAheadPattern);
            //lstPublisherLocationPattern.Add(new Tuple<string, int, int>(sFullPattern, 1, 4));

            LBL_END_PTN:

                //default patterns

                sFullPattern = string.Format(@"(?:{0}{1}{2})", sLookBehindPattern, sUniversityNamePattern, sLookAheadPattern);
                sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("<{0}>{1}</{0}>", "ldlPublisherName", "$&"));

                //added by Dakshinamoorthy on 2019-Jan-21
                //<ldlPublisherName>University Park</ldlPublisherName>
                sRefContent = Regex.Replace(sRefContent, "<ldlPublisherName>((?:University (?:Center|City|Colony|Estates|Forest|Gardens|Heights|Highlands|Mobile Home Park|Park|Place|Shadows|Town|View|Village)))</ldlPublisherName>", "$1");
                sRefContent = Regex.Replace(sRefContent, "<ldlPublisherName>((?:College (?:Acres|City|Corner|Court|Crest|Estates|Green|Grove|Heights|Heights Estates|Hill|Hills|Manor|Meadows|Mound|of Medical Technologies Colony|Park|Park Estates|Park Woods|Place|Point|Savannah|Springs|Station|View|View Park|Ward)))</ldlPublisherName>", "$1");


                sFullPattern = string.Format(@"(?:{0}{1}{2})", sLookBehindPattern, sFacultyNamePattern, sLookAheadPattern);
                sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("<{0}>{1}</{0}>", "ldlPublisherName", "$&"));

                sFullPattern = string.Format(@"(?:{0}{1}{2})", sLookBehindPattern, sDepartmentNamePattern, sLookAheadPattern);
                sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("<{0}>{1}</{0}>", "ldlPublisherName", "$&"));

                sFullPattern = string.Format(@"(?:{0}{1}{2})", sLookBehindPattern, sSchoolNamePattern, sLookAheadPattern);
                sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("<{0}>{1}</{0}>", "ldlPublisherName", "$&"));
                sRefContent = Regex.Replace(sRefContent, "<ldlPublisherName>((?:School (?:Village|Gardens|Side Manor|Hill|Station)))</ldlPublisherName>", "$1");

                sFullPattern = string.Format(@"(?:{0}{1}{2})", sLookBehindPattern, sCenterNamePattern, sLookAheadPattern);
                sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("<{0}>{1}</{0}>", "ldlPublisherName", "$&"));

                sFullPattern = string.Format(@"(?:{0}{1}{2})", sLookBehindPattern, sPressNamePattern, sLookAheadPattern);
                sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("<{0}>{1}</{0}>", "ldlPublisherName", "$&"));


                //missing elements
                sRefContent = Regex.Replace(sRefContent, @"(?<=\:[ ]*)((?:U\.S\. ))(<ldlPublisherName>)", "<ldlPublisherName>$1");

                //OrderByDescending = Element Count
                //Then OrderByAscending = Weightage
                lstPublisherLocationPattern = lstPublisherLocationPattern.AsEnumerable().OrderByDescending(x => x.Item2).ThenBy(y => y.Item3).ToList();

                string sRefContent_Backup = string.Empty;
                string sRefContent_Temp = string.Empty;
                StringBuilder sbRefContent = new StringBuilder();
                bool bFlag = false;

                foreach (var eachFullPattern in lstPublisherLocationPattern)
                {

                    string sEachPubLocPattern = eachFullPattern.Item1.ToString();
                    if (Regex.IsMatch(sRefContent, sEachPubLocPattern))
                    {
                        sRefContent_Backup = sRefContent;
                        sbRefContent = new StringBuilder();
                        do
                        {
                            bFlag = false;

                            sRefContent = Regex.Replace(sRefContent, sEachPubLocPattern, HandlePublisherLocation);
                            if (sRefContent_Backup.Equals(sRefContent))
                            {
                                Match myMatch = Regex.Match(sRefContent, sEachPubLocPattern);
                                if (myMatch.Groups.Count > 0)
                                {
                                    int index = myMatch.Groups[1].Index;
                                    int length = myMatch.Groups[1].Length;
                                    int nextStartIndex = (index + length);

                                    if (sRefContent.Length > nextStartIndex)
                                    {
                                        sbRefContent.Append(sRefContent.Substring(0, nextStartIndex));
                                        sRefContent = sRefContent.Substring(nextStartIndex, (sRefContent.Length - nextStartIndex));
                                    }
                                    else
                                    {
                                        sRefContent = sbRefContent.ToString() + sRefContent;
                                        break;
                                    }
                                }
                                else
                                {
                                    sRefContent = sbRefContent.ToString() + sRefContent;
                                    break;
                                }
                            }
                            else
                            {
                                sRefContent = sbRefContent.ToString() + sRefContent;
                                break;
                            }

                            if (Regex.IsMatch(sRefContent, sEachPubLocPattern))
                            {
                                bFlag = true;
                            }
                            else
                            {
                                sRefContent = sbRefContent.ToString() + sRefContent;
                            }

                        } while (bFlag == true);


                    }
                }

                //sRefContent = DoFullPatternMatch4PubNameLocation(sRefContent);


                sRefContent = Regex.Replace(sRefContent, "~~dot~~", ".");
                sRefContent = Regex.Replace(sRefContent, "~~space~~", " ");
                sRefContent = Regex.Replace(sRefContent, "~~o_parenthesis~~", "(");
                sRefContent = Regex.Replace(sRefContent, "~~c_parenthesis~~", ")");

                //Full pattern Match
                sRefContent = Regex.Replace(sRefContent, "<i>(<ldl[^<>]+>)", "$1<i>");
                sRefContent = Regex.Replace(sRefContent, "(</ldl[^<>]+>)</i>", "</i>$1");

                //nPatternId = 1;
                //if (IsPublisherNameLocationFound(sRefContent) == false)
                //{
                //    if (Regex.IsMatch(sRefContent, ""))
                //    {

                //    }
                //}


                sRefContent = PostCleanup4PublisherLocation(sRefContent);
                sRefContent = DoPatternMatch4PubNameLocation(sRefContent);
                sRefContent = DoPatternMatch4MissingPubName(sRefContent);
                sRefContent = HandleLocationPublisherClash(sRefContent);
                sRefContent = RemoveUnwantedPublisher(sRefContent);

                //added by Dakshinamoorthy on 2020-Feb-08
                sRefContent = Regex.Replace(sRefContent, @"(?:(</(?:ldlCountry|ldlState|ldlCity)>)\)\.)", ").$1");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyPublisherLocation", ex.Message, true, "");
            }
            return true;
        }

        //added by Dakshinamoorthy on 2019-Sep-10
        private string RemoveUnwantedPublisher(string sPubLocContent)
        {
            try
            {
                sPubLocContent = Regex.Replace(sPubLocContent, @"</ldlPublisherName>, <ldlPublisherName>", ", ");
                sPubLocContent = Regex.Replace(sPubLocContent, @",</ldlPublisherName> <ldlPublisherName>", ", ");


                if (GetPublisherNameCount(sPubLocContent) <= 1)
                {
                    return sPubLocContent;
                }

                string sPattern = string.Empty;
                string sUpdatedContent = string.Empty;
                int nPatternID = 0;
                int nCurPubNameCount = GetPublisherNameCount(sPubLocContent);

                nPatternID = 1;
                sPattern = @"(?:<(ldlCity|ldlState|ldlCountry)>(?:(?!</?\1>).)+</\1>[ ]*)(?:<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>)";
                if (Regex.IsMatch(sPubLocContent, sPattern) && nCurPubNameCount > 1)
                {
                    sUpdatedContent = NormalizePublisherTag(sPubLocContent, sPattern);
                    return sUpdatedContent;
                }

            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveUnwantedPublisher", ex.Message, true, "");
            }
            return sPubLocContent;
        }


        private string NormalizePublisherTag(string sPubLocContent, string sPattern)
        {
            string sUpdatedContent = sPubLocContent;

            try
            {
                Match myPubMatch = Regex.Match(sPubLocContent, sPattern);
                int nStartIndex = myPubMatch.Index;
                int nLength = myPubMatch.Length;

                string sPart1 = sPubLocContent.Substring(0, nStartIndex);
                string sPart2 = sPubLocContent.Substring(nStartIndex, nLength);
                string sPart3 = sPubLocContent.Substring(nStartIndex + nLength, sPubLocContent.Length - (nStartIndex + nLength));
                sUpdatedContent = RemovePublisherTag(sPart1) + sPart2 + RemovePublisherTag(sPart3);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\NormalizePublisherTag", ex.Message, true, "");
            }
            return sUpdatedContent;
        }

        private string RemovePublisherTag(string sPubLocContent)
        {
            try
            {
                sPubLocContent = Regex.Replace(sPubLocContent, @"</?ldlPublisherName>", "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemovePublisherTag", ex.Message, true, "");
            }
            return sPubLocContent;
        }

        private int GetPublisherNameCount(string sPubLocContent)
        {
            int nPubCount = 0;
            try
            {
                MatchCollection mcPubLoc = Regex.Matches(sPubLocContent, @"(?:<ldlPublisherName>(?:(?:(?!</?ldlPublisherName>).)+)</ldlPublisherName>)");
                nPubCount = mcPubLoc.Count;
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\GetPublisherNameCount", ex.Message, true, "");
            }
            return nPubCount;
        }


        private string HandleLocationPublisherClash(string sPubLocContent)
        {
            try
            {
                //type1
                if (Regex.IsMatch(sPubLocContent, @"<ldlPublisherName>((?:(?!</?ldlPublisherName>).)+)</ldlPublisherName>\: <ldlPublisherName>(\1 (?:(?!</?ldlPublisherName>).)+)</ldlPublisherName>"))
                {
                    sPubLocContent = Regex.Replace(sPubLocContent, @"<ldlPublisherName>((?:(?!</?ldlPublisherName>).)+)</ldlPublisherName>\: <ldlPublisherName>(\1 (?:(?!</?ldlPublisherName>).)+)</ldlPublisherName>", string.Format("<{0}>{1}</{0}>: <{2}>{3}</{2}>", "ldlCity", "$1", "ldlPublisherName", "$2"));
                    return sPubLocContent;
                }

                //type2
                if (Regex.IsMatch(sPubLocContent, @"<ldlPublisherName>((?:Oxford|Cambridge))</ldlPublisherName>\: <ldlPublisherName>((?:(?!<ldlPublisherName>).)+)</ldlPublisherName>"))
                {
                    sPubLocContent = Regex.Replace(sPubLocContent, @"<ldlPublisherName>((?:Oxford))</ldlPublisherName>\: <ldlPublisherName>((?:(?!<ldlPublisherName>).)+)</ldlPublisherName>", string.Format("<{0}>{1}</{0}>: <{2}>{3}</{2}>", "ldlCity", "$1", "ldlPublisherName", "$2"));
                    return sPubLocContent;
                }

            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleLocationPublisherClash", ex.Message, true, "");
            }
            return sPubLocContent;
        }


        //added by Dakshinamoorthy on 2019-Jan-21
        private string DoPatternMatch4PubNameLocation(string sRefContent)
        {
            try
            {
                if (Regex.IsMatch(sRefContent, "</?ldlPublisherName>"))
                {
                    return sRefContent;
                }

                int nPatternID = 0;
                string sPublisherPattern = string.Empty;

                nPatternID = 1;
                sPublisherPattern = @"(<(ldlCity|ldlState|ldlCountry)>(?:(?!</?\2>).)+</\2>\: )([A-Z]{3}\.)";
                if (Regex.IsMatch(sRefContent, sPublisherPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sPublisherPattern, "$1<ldlPublisherName>$3</ldlPublisherName>");
                    goto LBL_END;
                }

                nPatternID = 2;
                sPublisherPattern = @"((?<=[\.,;>] )[A-Z]{3,}, )(?=(?:(?:<(?:ldlCity|ldlState|ldlCountry)>(?:(?!</?(?:ldlCity|ldlState|ldlCountry)>).)+</(?:ldlCity|ldlState|ldlCountry)>)+)\.)";
                if (Regex.IsMatch(sRefContent, sPublisherPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sPublisherPattern, "<ldlPublisherName>$&</ldlPublisherName>");
                    goto LBL_END;
                }

                //added by Dakshinamoorthy on 2019-Sep-14
                nPatternID = 3;
                sPublisherPattern = @"(?:(?<=(?:</(?:ldlCity|ldlState|ldlCountry)>\:[ ]?))([^,]+)(?=, <ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>)+)";
                if (Regex.IsMatch(sRefContent, sPublisherPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sPublisherPattern, "<ldlPublisherName>$&</ldlPublisherName>");
                    goto LBL_END;
                }


            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\DoPatternMatch4PubNameLocation", ex.Message, true, "");
            }

        LBL_END:

            return sRefContent;
        }

        //added by Dakshinamoorthy on 2019-Sep-14
        private string DoPatternMatch4MissingPubName(string sRefContent)
        {
            try
            {
                int nPatternID = 0;
                string sPublisherPattern = string.Empty;

                //added by Dakshinamoorthy on 2019-Sep-14
                nPatternID = 1;
                sPublisherPattern = @"(?:(?<=(?:</(?:ldlCity|ldlState|ldlCountry)>\:[ ]?))([^,]+)(?=, <ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>)+)";
                if (Regex.IsMatch(sRefContent, sPublisherPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sPublisherPattern, "<ldlPublisherName>$&</ldlPublisherName>");
                    goto LBL_END;
                }

                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternID = 2;
                sPublisherPattern = @"(?:(?<=(?:(?:<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>, )+))(?:[^0-9\(\)\[\]<>,\.\;\:]{5,}), (?=(?:<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>[ ,]*<(?:ldlCity|ldlState|ldlCountry)>)))";
                if (Regex.IsMatch(sRefContent, sPublisherPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sPublisherPattern, "<ldlPublisherName>$&</ldlPublisherName>");
                    goto LBL_END;
                }

                //added by Dakshinamoorthy on 2019-Nov-08
                nPatternID = 3;
                sPublisherPattern = @"((?<=, )U\.S\. )<ldlPublisherName>";
                if (Regex.IsMatch(sRefContent, sPublisherPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sPublisherPattern, "<ldlPublisherName>$1");
                    goto LBL_END;
                }

                //added by Dakshinamoorthy on 2020-Dec-12
                nPatternID = 4;
                sPublisherPattern = @"(?:(?<=(?:</ldlPublisherName>[ ]))((?:[A-Z][a-z]+ )+(?:Division)[\;\.] ))";
                if (Regex.IsMatch(sRefContent, sPublisherPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sPublisherPattern, "<ldlPublisherName>$&</ldlPublisherName>");
                    goto LBL_END;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\DoPatternMatch4MissingPubName", ex.Message, true, "");
            }

        LBL_END:

            return sRefContent;
        }



        private string DoFullPatternMatch4PubNameLocation(string sRefContent)
        {
            int nPatternId = 0;
            try
            {
                //remove ref type
                sRefContent = Regex.Replace(sRefContent, "</?(?:Book|Communication|Conference|Journal|Other|Patent|References|Report|Thesis|Web)>", "");
                sRefContent = Regex.Replace(sRefContent, "<i>(<ldl[^<>]+>)", "$1<i>");
                sRefContent = Regex.Replace(sRefContent, "(</ldl[^<>]+>)</i>", "</i>$1");

                nPatternId = 1;
                if (IsPublisherNameLocationFound(sRefContent) == false)
                {
                    if (Regex.IsMatch(sRefContent, ""))
                    {

                    }
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\DoFullPatternMatch4PubNameLocation", ex.Message, true, "");
            }
            return sRefContent;
        }

        private bool IsPublisherNameLocationFound(string sRefContent)
        {
            try
            {

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsVolOrIssueDoiFound", ex.Message, true, "");
            }
            return false;
        }




        private string PostCleanup4PublisherLocation(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, @"</ldlPublisherName>(, Inc\.|, Ltd\.)", "$1</ldlPublisherName>");
                sRefContent = Regex.Replace(sRefContent, @"\(<ldlPublisherName>", "<ldlPublisherName>(");
                sRefContent = Regex.Replace(sRefContent, @"\((<(?:ldlCity|ldlState|ldlCountry)>)", "$1(");
                sRefContent = Regex.Replace(sRefContent, @"</ldlPublisherName>([\),]+)", "$1</ldlPublisherName>");
                //added by Dakshinamoorthy on 2020-Jul-07
                sRefContent = Regex.Replace(sRefContent, @"(?<=(?:<PubDate>(?:(?!</?PubDate>).)+</PubDate>[\:\.; ]+))(?:<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>[\:][ ]?[A-Za-z]+)", RemovePublisherTag);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\PostCleanup4PublisherLocation", ex.Message, true, "");
            }
            return sRefContent;
        }

        //added by Dakshinamoorthy on 2020-Jul-07
        private string RemovePublisherTag(Match myPubMatch)
        {
            string sPublisherContent = myPubMatch.Value.ToString();
            try
            {
                sPublisherContent = Regex.Replace(sPublisherContent, "(?:</?ldlPublisherName>)", "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\PostCleanup4PublisherLocation", ex.Message, true, "");
            }
            return sPublisherContent;
        }


        private string HandlePublisherLocation(Match myPubLocMatch)
        {
            string sPubLocOriginal = myPubLocMatch.Value.ToString();
            string sPubLocTagged = myPubLocMatch.Value.ToString();

            sPubLocOriginal = Regex.Replace(sPubLocOriginal, "~~dot~~", ".");
            sPubLocTagged = Regex.Replace(sPubLocTagged, "~~dot~~", ".");

            try
            {
                bool bIsPublisherValid = false;
                bool bIsCityValid = false;
                bool bIsStateValid = false;
                bool bIsCountryValid = false;

                List<Tuple<string, string, int>> lstPublisherLocationInfo = new List<Tuple<string, string, int>>();

                string sPublisherName = myPubLocMatch.Groups["publisher"].Value;
                sPublisherName = Regex.Replace(sPublisherName, "~~dot~~", ".");
                int nPublisherNameIndex = myPubLocMatch.Groups["publisher"].Index;

                string sCityName = myPubLocMatch.Groups["city"].Value;
                sCityName = Regex.Replace(sCityName, "~~dot~~", ".");
                int nCityNameIndex = myPubLocMatch.Groups["city"].Index;

                if (string.IsNullOrEmpty(sCityName))
                {
                    sCityName = myPubLocMatch.Groups["city_m"].Value;
                    sCityName = Regex.Replace(sCityName, "~~dot~~", ".");
                    nCityNameIndex = myPubLocMatch.Groups["city_m"].Index;
                }


                string sStateName = myPubLocMatch.Groups["state"].Value;
                sStateName = Regex.Replace(sStateName, "~~dot~~", ".");
                int nStateNameIndex = myPubLocMatch.Groups["state"].Index;

                string sCountryName = myPubLocMatch.Groups["country"].Value;
                sCountryName = Regex.Replace(sCountryName, "~~dot~~", ".");
                int nCountryNameIndex = myPubLocMatch.Groups["country"].Index;

                string sMessageContent = string.Format(
                    "Publisher: {0},{4}City: {1},{4}State: {2},{4}Country: {3}, ",
                    sPublisherName, sCityName, sStateName, sCountryName, System.Environment.NewLine);

                if (string.IsNullOrEmpty(sPublisherName) && string.IsNullOrEmpty(sCityName) && string.IsNullOrEmpty(sStateName) && string.IsNullOrEmpty(sCountryName))
                {
                    return myPubLocMatch.Value.ToString();
                }

                //Validate City Name
                if (string.IsNullOrEmpty(sCityName))
                {
                    bIsCityValid = true;
                }
                else
                {
                    if (IsValidCityName(sCityName))
                    {
                        bIsCityValid = true;
                        lstPublisherLocationInfo.Add(new Tuple<string, string, int>(sCityName, "ldlCity", nCityNameIndex));
                    }
                    else
                    {
                        return sPubLocOriginal;
                    }
                }

                //Validte State Name
                if (string.IsNullOrEmpty(sStateName))
                {
                    bIsStateValid = true;
                }
                else
                {
                    if (IsValidStateName(sStateName))
                    {
                        bIsStateValid = true;
                        lstPublisherLocationInfo.Add(new Tuple<string, string, int>(sStateName, "ldlState", nStateNameIndex));
                    }
                    else
                    {
                        return sPubLocOriginal;
                    }
                }

                //Validate Country Name
                if (string.IsNullOrEmpty(sCountryName))
                {
                    bIsCountryValid = true;
                }
                else
                {
                    if (IsValidCountryName(sCountryName))
                    {
                        bIsCountryValid = true;
                        lstPublisherLocationInfo.Add(new Tuple<string, string, int>(sCountryName, "ldlCountry", nCountryNameIndex));
                    }
                    else
                    {
                        return sPubLocOriginal;
                    }
                }

                //Validate Publisher Name
                if (string.IsNullOrEmpty(sPublisherName))
                {
                    bIsPublisherValid = true;
                }
                else
                {
                    //added by Dakshinamoorthy on 2019-Dec-10
                    if (Regex.IsMatch(sPublisherName, "(?:^(?:<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>)$)"))
                    {
                        bIsPublisherValid = true;
                        sPublisherName = Regex.Replace(sPublisherName, "</?ldlPublisherName>", "");
                    }

                    if (Regex.IsMatch(sPublisherName.ToLower(), @"^(?:chapt?(?:er)?|volume)[\:\.;, ]*$"))
                    {
                        return sPubLocOriginal;
                    }

                    if (IsValidPublisherName(sPublisherName))
                    {
                        bIsPublisherValid = true;
                        lstPublisherLocationInfo.Add(new Tuple<string, string, int>(sPublisherName, "ldlPublisherName", nPublisherNameIndex));
                    }
                    else
                    {
                        return sPubLocOriginal;
                    }
                }

                if (bIsPublisherValid && bIsCityValid && bIsStateValid && bIsCountryValid)
                {
                    lstPublisherLocationInfo = lstPublisherLocationInfo.OrderBy(i => i.Item3).ToList();

                    StringBuilder sbTaggedContent = new StringBuilder();

                    foreach (var eachIdentifiedElement in lstPublisherLocationInfo)
                    {
                        string sContent = eachIdentifiedElement.Item1.ToString();
                        string sTagName = eachIdentifiedElement.Item2.ToString();
                        int nStartIndex = eachIdentifiedElement.Item3;

                        sbTaggedContent.Append(string.Format("<{0}>{1}</{0}>", sTagName, sContent));
                    }

                    sPubLocTagged = sbTaggedContent.ToString();
                    sPubLocTagged = Regex.Replace(sPubLocTagged, @"</([a-zA-Z]+)><\1>", "");

                    //added by Dakshinamoorthy on 2019-Jan-11
                    //to escape the space inside identified element
                    sPubLocTagged = Regex.Replace(sPubLocTagged, " ", "~~space~~");
                    sPubLocTagged = Regex.Replace(sPubLocTagged, @"\.", "~~dot~~");

                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandlePublisherLocation", ex.Message, true, "");
            }
            return sPubLocTagged;
        }

        private string PublisherNameCleanup4DBSearch(string sPublisherName)
        {
            try
            {
                sPublisherName = Regex.Replace(sPublisherName, "</?[^<>]+>", "");
                sPublisherName = Regex.Replace(sPublisherName, @"[:,;\(\)\[\]\$=""\'\?\!\/\u201C\u201D\u2019\.]", "");
                sPublisherName = Regex.Replace(sPublisherName, @"\b(?:of|the|an|a|the|in|and|to|for|on)\b", "", RegexOptions.IgnoreCase);
                sPublisherName = Regex.Replace(sPublisherName, @" & ", " ", RegexOptions.IgnoreCase);
                sPublisherName = Regex.Replace(sPublisherName, @"[\-]", " ");
                NormalizeSpaces(ref sPublisherName);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\PublisherNameCleanup4DBSearch", ex.Message, true, "");
            }
            return sPublisherName;
        }

        public bool IsValidPublisherName(string sPublisherName)
        {
            bool bReturnValue = false;
            try
            {
                string sQuery = string.Empty;
                SqlCommand cmd = new SqlCommand();
                DataTable dtResult = new DataTable();

                //added by Dakshinamoorthy on 2019-Sep-14
                sPublisherName = Regex.Replace(sPublisherName, "(?:~~space~~)", " ");

                //added by Dakshinamoorthy on 2020-Dec-15
                if (Regex.IsMatch(sPublisherName, @"(?:\b(?:Inc|Ltd)\b)"))
                {
                    return true;
                }

                //List Publisher as Eds
                List<string> lstCheckPublisherActualName = new List<string>();
                lstCheckPublisherActualName.Add("editor");

                //added by Dakshinamoorthy on 2020-Jul-24
                if (lstInlinePublisherNames.Count > 0)
                {
                    string sPublisherName_Temp = Regex.Replace(sPublisherName, @"(?:^\(|\)$)", "");
                    sPublisherName_Temp = Regex.Replace(sPublisherName_Temp, @"[\.,:; ]*$", "");
                    if (lstInlinePublisherNames.Contains(sPublisherName_Temp))
                    {
                        return true;
                    }
                }

                //added by Dakshinamoorthy on 2019-Jan-21
                //0. Publisher name matching with keywords with RegEx pattern
                if (Regex.IsMatch(sPublisherName, @"^(?:[A-Za-z]+ (?:[Pp]ublication[s]?)[\.,; ]*)$"))
                {
                    return true;
                }

                //1. Local Database Search
                //string sPublisherNameCleaned = PublisherNameCleanup4DBSearch(sPublisherName);
                string sPublisherNameCleaned = General.DatabaseIndexCleanup(sPublisherName);

                ////0. Local Xml File Search
                //added by Dakshinamoorthy on 2020-Jul-13
                //if (string.IsNullOrEmpty(AutoStructRefAuthor.sPublisherXmlContent) == false)
                //{
                //    if (IsValidPublisherName_Local(sPublisherNameCleaned))
                //    {
                //        return true;
                //    }
                //    else if (bOption4UseLocalLocation == false)
                //    {
                //        return false;
                //    }
                //}

                sQuery = string.Format("select distinct(index1) from data_dict_publisher where index1 = '{0}'", sPublisherNameCleaned.ToLower());
                Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                dtResult = General.GeneralInstance.dataTable;

                if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                {
                    //updated by Dakshinamoorthy on 2019-Nov-11
                    if (lstCheckPublisherActualName.Contains(sPublisherNameCleaned))
                    {
                        sQuery = string.Format("select distinct(index1) from data_dict_publisher where data1 = '{0}'", CleanPublisherActualName(sPublisherName));
                        Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                        dtResult = General.GeneralInstance.dataTable;

                        if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }

                //2. Institute Database
                cmd = new SqlCommand();
                dtResult = new DataTable();
                if (sPublisherNameCleaned.Length > 5)
                {
                    sQuery = string.Format("select * from data_dict_institute where index1 = '{0}'", sPublisherNameCleaned.ToLower());
                    Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                    dtResult = General.GeneralInstance.dataTable;

                    if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                    {
                        return true;
                    }
                }


                ////3. WorldCat Database Search
                //string sPublisherName4WC = string.Empty;
                //sPublisherName4WC = Regex.Replace(sPublisherName, @"[\:.,; ]+$", "");
                //if (AutoStructReferencesWCSearch.IsPublisherNameInWC(sPublisherName4WC))
                //{
                //    return true;
                //}

                //string sPublisherContent = File.ReadAllText(@"d:\Dakshina\Temp\PublisherLocationData\publisher.txt");
                //sPublisherName = string.Format("<publisher>{0}</publisher>", sPublisherName);
                //if (Regex.IsMatch(sPublisherContent, sPublisherName, RegexOptions.IgnoreCase))
                //{
                //    bReturnValue = true;
                //}

                //4. Multiple Publisher Name
                List<string> lstPublisherNames = Regex.Split(sPublisherName, @"[\/]").ToList();
                bool bIsValidPublisher = false;
                if (lstPublisherNames.Count > 1)
                {
                    foreach (string sEachPublisherName in lstPublisherNames)
                    {
                        if (!string.IsNullOrEmpty(sEachPublisherName.Trim()))
                        {
                            sPublisherNameCleaned = General.DatabaseIndexCleanup(sEachPublisherName);
                            cmd = new SqlCommand();
                            sQuery = string.Format("select distinct(index1) from data_dict_publisher where index1 = '{0}'", sPublisherNameCleaned.ToLower());
                            Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                            dtResult = General.GeneralInstance.dataTable;

                            if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                            {
                                bIsValidPublisher = true;
                                continue;
                            }
                            else
                            {
                                //2. Institute Database
                                cmd = new SqlCommand();
                                dtResult = new DataTable();
                                if (sPublisherNameCleaned.Length > 5)
                                {
                                    sQuery = string.Format("select * from data_dict_institute where index1 = '{0}'", sPublisherNameCleaned.ToLower());
                                    Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                                    dtResult = General.GeneralInstance.dataTable;

                                    if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                                    {
                                        bIsValidPublisher = true;
                                        continue;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    return bIsValidPublisher;
                }


            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsValidPublisherName", ex.Message, true, "");
            }
            return bReturnValue;
        }

        private string CleanPublisherActualName(string sPubActualName)
        {
            try
            {
                sPubActualName = Regex.Replace(sPubActualName, @"([\:\.;, ]+$)", "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CleanPublisherActualName", ex.Message, true, "");
            }
            return sPubActualName;
        }

        //added by Dakshinamoorthy on 2020-Jul-13
        #region Local Location Validation
        public bool IsValidCityName_Local(string sCityName)
        {
            bool bReturnValue = false;
            try
            {
                XmlNode nCityName = docCityXml.SelectSingleNode(string.Format("//cities/city/name[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '{0}']", sCityName.ToLower()));
                if (nCityName != null)
                {
                    bReturnValue = true;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsValidCityName_Local", ex.Message, true, "");
            }
            return bReturnValue;
        }

        public bool IsValidPublisherName_Local(string sPublisherName_ID)
        {
            bool bReturnValue = false;
            try
            {
                sPublisherName_ID = General.GeneralInstance.EnryptString(sPublisherName_ID.ToLower(), "nZr4u7x!A%D*G-Ka");
                XmlNode nPubName = docPublisherXml.SelectSingleNode(string.Format("//publisherName/pub='{0}'", sPublisherName_ID.ToLower()));
                if (nPubName != null)
                {
                    bReturnValue = true;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsValidPublisherName_Local", ex.Message, true, "");
            }
            return bReturnValue;
        }
        #endregion

        public bool IsValidCityName(string sCityName)
        {
            bool bReturnValue = false;

            try
            {
                #region Check
                string sMultipleCityPattern = @"^(?:([A-Za-z ]+)(?:[\/]|; )([A-Za-z ]+))$";
                if (Regex.IsMatch(sCityName, sMultipleCityPattern))
                {
                    string[] arrCityNames = Regex.Split(sCityName, @"(?:[\/]|; )");
                    foreach (string sEachCity in arrCityNames)
                    {
                        if (IsValidCityName(sEachCity) == false)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                #endregion

                //added by Dakshinamoorthy on 2020-Jul-13
                if (string.IsNullOrEmpty(AutoStructRefAuthor.sCityXmlContent) == false)
                {
                    if (IsValidCityName_Local(sCityName))
                    {
                        return true;
                    }
                    else if (bOption4UseLocalLocation == false)
                    {
                        return false;
                    }
                }

                string sQuery = string.Empty;
                SqlCommand cmd = new SqlCommand();
                DataTable dtResult = new DataTable();

                //0. Multiple


                //1. Local Database Search
                //string sCityNameCleaned = PublisherNameCleanup4DBSearch(sCityName);
                string sCityNameCleaned = General.DatabaseIndexCleanup(sCityName);

                sQuery = string.Format("select distinct(index1) from data_dict_location where index1 = '{0}' and data4 = 'City'", sCityNameCleaned.ToLower());
                Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                dtResult = General.GeneralInstance.dataTable;

                if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                {
                    return true;
                }


                //2. Multiple City Name

                List<string> lstCityNames = Regex.Split(sCityName, "(?:and|&)").ToList();
                bool bIsValidCity = false;
                if (lstCityNames.Count > 1)
                {
                    foreach (string sEachCityName in lstCityNames)
                    {
                        if (!string.IsNullOrEmpty(sEachCityName.Trim()))
                        {
                            sCityNameCleaned = General.DatabaseIndexCleanup(sEachCityName);
                            cmd = new SqlCommand();
                            sQuery = string.Format("select distinct(index1) from data_dict_location where index1 = '{0}' and data4 = 'City'", sCityNameCleaned.ToLower());
                            Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                            dtResult = General.GeneralInstance.dataTable;

                            if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                            {
                                bIsValidCity = true;
                                continue;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return bIsValidCity;
                }

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsValidCityName", ex.Message, true, "");
            }
            return bReturnValue;
        }

        public bool IsValidStateName(string sStateName)
        {
            bool bReturnValue = false;
            try
            {
                string sQuery = string.Empty;
                SqlCommand cmd = new SqlCommand();
                DataTable dtResult = new DataTable();

                ////1. Local Database Search
                ////string sCityNameCleaned = PublisherNameCleanup4DBSearch(sCityName);
                //string sStateNameCleaned = General.DatabaseIndexCleanup(sStateName);

                //sQuery = string.Format("select distinct(index1) from data_dict_location where index1 = '{0}' and data4 = 'State'", sStateNameCleaned.ToLower());
                //Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                //dtResult = General.GeneralInstance.dataTable;

                //if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                //{
                //    return true;
                //}


                //updated by Dakshinamoorthy on 2018-OCT-26
                //string sStateContent = File.ReadAllText(@"D:\eBOT\Data\Ref_State.txt");
                string sStateContent = File.ReadAllText(General.GeneralInstance.drive + @"\eBOT\Data\Ref_State.txt");

                if (Regex.IsMatch(sStateName, @"^(?:[A-Z]{2})[\:\.;, ]*$"))
                {
                    sStateName = Regex.Replace(sStateName, @"[\:\.;, ]+", "");
                }
                else
                {
                    sStateName = PublisherNameCleanup4DBSearch(sStateName);
                }
                sStateName = string.Format("<state>{0}</state>", sStateName, RegexOptions.IgnoreCase);

                if (Regex.IsMatch(sStateContent, sStateName))
                {
                    bReturnValue = true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsValidStateName", ex.Message, true, "");
            }
            return bReturnValue;
        }

        public bool IsValidCountryName(string sCountryName)
        {
            bool bReturnValue = false;
            try
            {

                string sQuery = string.Empty;
                SqlCommand cmd = new SqlCommand();
                DataTable dtResult = new DataTable();

                ////1. Local Database Search
                ////string sCountryNameCleaned = PublisherNameCleanup4DBSearch(sCountryName);
                //string sCountryNameCleaned = General.DatabaseIndexCleanup(sCountryName);

                //sQuery = string.Format("select distinct(index1) from data_dict_location where index1 = '{0}' and data4 = 'Country'", sCountryNameCleaned.ToLower());
                //Database.GetInstance.ReadFromDatabase(cmd, sQuery);
                //dtResult = General.GeneralInstance.dataTable;

                //if (General.GeneralInstance.dataTable.Rows.Count >= 1)
                //{
                //    return true;
                //}

                //updated by Dakshinamoorthy on 2018-OCT-26
                //string sCountryContent = File.ReadAllText(@"D:\eBOT\Data\Ref_Country.txt");
                string sCountryContent = File.ReadAllText(General.GeneralInstance.drive + @"\eBOT\Data\Ref_Country.txt");

                sCountryName = PublisherNameCleanup4DBSearch(sCountryName);
                sCountryName = string.Format("<country>{0}</country>", sCountryName, RegexOptions.IgnoreCase);

                if (Regex.IsMatch(sCountryContent, sCountryName))
                {
                    bReturnValue = true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsValidCountryName", ex.Message, true, "");
            }
            return bReturnValue;
        }





        private bool ApplyEditorGroup(ref string sRefContent)
        {
            try
            {
                //sRefContent = Regex.Replace(sRefContent, @"(?:<Author>(?:(?!</?Author>).)+</Author>(?:[\:\.;, ]*)(?: (?:and|&|&#x0026;) )?)+<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>", HandleApplyEditorGroup);

                //updated by Dakshinamoorthy on 2020-Jul-11
                #region Old Code
                //sRefContent = Regex.Replace(sRefContent, @"(?:(?:<Author>(?:(?!</?Author>).)+</Author>(?:[\:\.;, ]*)(?: (?:and|&|&#x0026;) )?)+(?:<Etal>(?:(?!</?Etal>).)+</Etal>[ ]?)?<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)", HandleApplyEditorGroup);
                //sRefContent = Regex.Replace(sRefContent, @"(?:(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>)[\:.,; ]*(?:<Author>(?:(?!</?Author>).)+</Author>[,;\:\. ]*(?:\b(?:and|&) )?)+)", HandleApplyEditorGroup);
                //sRefContent = Regex.Replace(sRefContent, @"(?:<Author>(?:(?!</?Author>).)+</Author>(?:[\:\.;, ]*)(?: (?:and|&|&#x0026;) )?)+<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>", HandleApplyEditorGroup);
                //sRefContent = Regex.Replace(sRefContent, @"(?:<ldlAuthorEditorGroup>(?:(?!</?ldlAuthorEditorGroup>).)+</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup>(?:[\:\.;, ]*)<(ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>(?:(?!</?\1>).)+</\1>)", HandleApplyEditorGroup); 
                #endregion

                sRefContent = Regex.Replace(sRefContent, @"(?:(?:<Author>(?:(?!</?Author>).)+</Author>(?:[\:\.;, ]*)(?:(?:\b(?:and)|&|&#x0026;|<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>) )?)+(?:<Etal>(?:(?!</?Etal>).)+</Etal>[ ]?)?<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)", HandleApplyEditorGroup);
                sRefContent = Regex.Replace(sRefContent, @"(?:(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>)[\:.,; ]*(?:<Author>(?:(?!</?Author>).)+</Author>[,;\:\. ]*(?:(?:\b(?:and)|&|<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>) )?)+)", HandleApplyEditorGroup);
                sRefContent = Regex.Replace(sRefContent, @"(?:<Author>(?:(?!</?Author>).)+</Author>(?:[\:\.;, ]*)(?:(?:\b(?:and)|&|&#x0026;|<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>) )?)+<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>", HandleApplyEditorGroup);
                sRefContent = Regex.Replace(sRefContent, @"(?:<ldlAuthorEditorGroup>(?:(?!</?ldlAuthorEditorGroup>).)+</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup>(?:[\:\.;, ]*)<(ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>(?:(?!</?\1>).)+</\1>)", HandleApplyEditorGroup);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ApplyEditorGroup", ex.Message, true, "");
            }
            return true;
        }

        private string HandleApplyEditorGroup(Match myEdgMatch)
        {
            string sEdgContent = myEdgMatch.Value.ToString();
            try
            {
                sEdgContent = Regex.Replace(sEdgContent, "<Author>", "<Editor>");
                sEdgContent = Regex.Replace(sEdgContent, "</Author>", "</Editor>");

                sEdgContent = Regex.Replace(sEdgContent, "<Forename>", "<EForename>");
                sEdgContent = Regex.Replace(sEdgContent, "</Forename>", "</EForename>");

                sEdgContent = Regex.Replace(sEdgContent, "<Surname>", "<ESurname>");
                sEdgContent = Regex.Replace(sEdgContent, "</Surname>", "</ESurname>");

                sEdgContent = Regex.Replace(sEdgContent, "<Suffix>", "<ESuffix>");
                sEdgContent = Regex.Replace(sEdgContent, "</Suffix>", "</ESuffix>");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleApplyEditorGroup", ex.Message, true, "");
            }
            return sEdgContent;
        }

        //added by Dakshinamoorthy on 2020-Dec-18
        private bool ValidateSingleAuthorAsCollab(ref string sRefContent)
        {
            try
            {
                string sSingleAuthorPattern = @"(?<=(?:^|(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[ ]*)))(?:<((?:AuEdGroup))><Author>((?:(?!</?Author>).)+)</Author></\1>)";
                bool isCollabSymptomsFound = false;
                if (Regex.IsMatch(sRefContent, sSingleAuthorPattern))
                {
                    string sSingleAuContent = Regex.Match(sRefContent, sSingleAuthorPattern).Groups[2].Value;
                    sSingleAuContent = Regex.Replace(sSingleAuContent, "<[^<>]+>", "").Trim();

                    if (IsCollabSymptomsFound(sSingleAuContent))
                    {
                        isCollabSymptomsFound = true;
                    }
                    else if (IsValidPublisherName(sSingleAuContent))
                    {
                        isCollabSymptomsFound = true;
                    }

                    if (isCollabSymptomsFound == true)
                    {
                        sRefContent = Regex.Replace(sRefContent, sSingleAuthorPattern, TagAsCollab);
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ValidateSingleAuthorAsCollab", ex.Message, true, "");
            }
            return true;
        }

        private string TagAsCollab(Match myCollabMatch)
        {
            string sRootTagName = myCollabMatch.Groups[1].Value;
            string sCollabContent = myCollabMatch.Groups[2].Value;

            try
            {
                sCollabContent = Regex.Replace(sCollabContent, "<[^<>]+>", "");
                sCollabContent = string.Format("<{0}>{1}</{0}> ", "Collab", sCollabContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\TagAsCollab", ex.Message, true, "");
            }
            return sCollabContent;
        }

        private bool IdentifyCollab1(ref string sRefContent)
        {
            try
            {
                string sCollabPattern = string.Empty;

                //updated by Dakshinamoorthy on 2020-May-19 (to add "<RefPrefix>")
                sCollabPattern = @"(?<=(?:^|(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>) |(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)?[ ]*(?:<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup>)))(?:(?:(?:[^0-9<>\(\)\.,]+(?: \([^\(\)0-9]+\))[\., ]*|(?:[A-Z\-]+(?:[0-9]+)? )+|[A-Z]{3,}\.? (?=<PubDate>)|(?:[A-Z\-]+[0-9]+)|(?:(?:[^0-9<>^\(\)\.,]{5,})))[\., ]*))";

                //reverted by Dakshinamoorthy on 2020-May-19 (to remove "<RefPrefix>")
                //sCollabPattern = @"(?<=(?:^|(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>) (?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>)?|(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)?[ ]*(?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>)?[ ]*(?:<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup>)))(?:(?:(?:[^0-9<>\(\)\.,]+(?: \([^\(\)0-9]+\))[\., ]*|(?:[A-Z\-]+(?:[0-9]+)? )+|[A-Z]{3,}\.? (?=<PubDate>)|(?:[A-Z\-]+[0-9]+)|(?:(?:[^0-9<>^\(\)\.,]{5,})))[\., ]*))";

                sRefContent = Regex.Replace(sRefContent, sCollabPattern, string.Format("<{0}>{1}</{0}>", "Collab", "$&"));

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyCollab1", ex.Message, true, "");
            }
            return true;
        }


        private string HandleIdentifyCollab1(Match myCollabMatch)
        {
            string sOutputContent = myCollabMatch.Value.ToString();

            try
            {
                string sCollabPattern = string.Empty;

                //updated by Dakshinamoorthy on 2020-May-19 (to add "<RefPrefix>")
                sCollabPattern = @"(?<=(?:^|(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>) ))(?:(?:(?:[^0-9<>\(\)\.,]+(?: \([^\(\)0-9]+\))[\., ]*|(?:[A-Z\-]+(?:[0-9]+)? )+|(?:[A-Z\-]+[0-9]+)|(?:(?:[^0-9<>^\(\)\.,]+)))[\., ]*))";

                //reverted by Dakshinamoorthy on 2020-May-19 (to remove "<RefPrefix>")
                //sCollabPattern = @"(?<=(?:^|(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>) (?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>[ ]*)?))(?:(?:(?:[^0-9<>\(\)\.,]+(?: \([^\(\)0-9]+\))[\., ]*|(?:[A-Z\-]+(?:[0-9]+)? )+|(?:[A-Z\-]+[0-9]+)|(?:(?:[^0-9<>^\(\)\.,]+)))[\., ]*))";

                sOutputContent = Regex.Replace(sOutputContent, sCollabPattern, string.Format("<{0}>{1}</{0}>", "Collab", "$&"));
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleIdentifyCollab1", ex.Message, true, "");
            }
            return sOutputContent;
        }




        private bool ValidateDateInformation(ref string sRefTaggedContent)
        {
            try
            {
                NormalizeSpaces(ref sRefTaggedContent);
                //merge date information
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</PubDate>( (?:to) )<PubDate>", "$1");

                //validation starts
                sRefTaggedContent = Regex.Replace(sRefTaggedContent, "(?<= (?:to|the) )(?:<PubDate>(?:(?!</?PubDate>).)+</PubDate>)", RemoveDateInformaion);

                //Perreault S, Brennan S. Criminal victimization in Canada, 2009. Juristat. 2010;30(2). Available at: http://www.statcan.gc.ca/pub/85-002-x/85-002-x2010002-eng.htm (Accessed October 22, 2015).
                if (Regex.IsMatch(sRefTaggedContent, string.Format(@"(?:(?:(?<=(?:in Canada, ))<PubDate><ldlyear>(?:(?:(?!</?ldlyear>).)+)</ldlyear>.</PubDate>)|(?:(?<=(?:[A-Za-z]+ ))(?:<PubDate><ldldayNum>(?:(?!</?ldldayNum>).)+</ldldayNum> <ldlmonthName>(?:(?!</?ldlmonthName>).)+</ldlmonthName> <ldlyear>(?:(?!</?ldlyear>).)+</ldlyear></PubDate>)(?=(?: [A-Za-z]+)))|(?:[,\:] <PubDate><ldlyear>[0-9]+[ ]?[\-\u2013][ ]?[0-9]+</ldlyear>.</PubDate>))")))
                {
                    if (Regex.Matches(sRefTaggedContent, "<PubDate>(?:(?!</?PubDate>).)+</PubDate>").Count > 1)
                    {
                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, string.Format("(?<=(?:in Canada, ))<PubDate><ldlyear>(?:(?:(?!</?ldlyear>).)+)</ldlyear>.</PubDate>"), RemoveDateInformaion);
                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, string.Format(@"(?:[,\:] <PubDate><ldlyear>[0-9]+[ ]?[\-\u2013][ ]?[0-9]+</ldlyear>.</PubDate>)"), RemoveDateInformaion);
                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, string.Format(@"(?:(?<=(?:[A-Za-z]+ ))(?:<PubDate><ldldayNum>(?:(?!</?ldldayNum>).)+</ldldayNum> <ldlmonthName>(?:(?!</?ldlmonthName>).)+</ldlmonthName> <ldlyear>(?:(?!</?ldlyear>).)+</ldlyear></PubDate>)(?=(?: [A-Za-z]+)))"), RemoveDateInformaion);
                    }
                }

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ValidateDateInformation", ex.Message, true, "");
            }
            return true;
        }


        private string RemoveDateInformaion(Match myDateMatch)
        {
            string sOutputContent = myDateMatch.Value.ToString();

            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "(?:</?(?:PubDate|ldl(?:year|monthName|monthNum|dayNum|dayName|yearMisc))>)", "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveDateInformaion", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private string HandleAcronymCollab(Match myMatchAcronymCollab)
        {
            string sOutputContent = myMatchAcronymCollab.Value;
            string sTextPart = myMatchAcronymCollab.Groups[1].Value.Trim();
            string sAcronymPart = myMatchAcronymCollab.Groups[2].Value.Trim();
            try
            {
                string sAcronymFromText = string.Empty;
                string[] strSplit = sTextPart.Split();
                foreach (string res in strSplit)
                {
                    if (Regex.IsMatch(res, "^(?:[A-Z][a-z]+)"))
                    {
                        sAcronymFromText = sAcronymFromText + res.Substring(0, 1);
                    }
                }

                if (sAcronymFromText.Equals(sAcronymPart))
                {
                    sOutputContent = string.Format("<Collab>{0}</Collab>", sOutputContent);
                    lstInlinePublisherNames.Add(Regex.Replace(sTextPart, @"[\(\)]", "").Trim());
                    lstInlinePublisherNames.Add(Regex.Replace(sAcronymPart, @"[\(\)]", "").Trim());
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleAcronymCollab", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private string HandleAcronymCollab1(Match myMatchAcronymCollab)
        {
            string sOutputContent = myMatchAcronymCollab.Value;
            string sTextPart = myMatchAcronymCollab.Groups[2].Value.Trim();
            string sAcronymPart = myMatchAcronymCollab.Groups[1].Value.Trim();
            try
            {
                string sAcronymFromText = string.Empty;
                string sAcronymFromText1 = string.Empty;

                string[] strSplit = sTextPart.Split();
                foreach (string res in strSplit)
                {
                    string sResTemp = res;
                    sResTemp = Regex.Replace(sResTemp, @"^\(", "");
                    sResTemp = Regex.Replace(sResTemp, @"\)$", "");
                    if (Regex.IsMatch(sResTemp, "^(?:[A-Z][a-z]+)"))
                    {
                        sAcronymFromText = sAcronymFromText + sResTemp.Substring(0, 1);
                        sAcronymFromText1 = sAcronymFromText1 + sResTemp.Substring(0, 1);
                    }
                    else if (Regex.IsMatch(sResTemp, "^(?:of)$"))
                    {
                        sAcronymFromText1 = sAcronymFromText1 + sResTemp.Substring(0, 1).ToUpper();
                    }
                }

                if (sAcronymFromText.Equals(sAcronymPart) || sAcronymFromText1.Equals(sAcronymPart))
                {
                    sOutputContent = string.Format("<Collab>{0}</Collab>", sOutputContent);
                    lstInlinePublisherNames.Add(Regex.Replace(sTextPart, @"[\(\)]", "").Trim());
                    lstInlinePublisherNames.Add(Regex.Replace(sAcronymPart, @"[\(\)]", "").Trim());
                    lstInlinePublisherNames.Add(Regex.Replace(sAcronymFromText1, @"[\(\)]", "").Trim());
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleAcronymCollab", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private bool IdentifyCollab(ref string sRefContent)
        {
            try
            {
                string sCollabPattern = string.Empty;
                int nPatternId = 0;

                //added by Dakshinamoorthy on 2020-Jul-14 (for Acronym Collab)
                //Federal Emergency Management Agency (FEMA 445) (2006). Next-Generation Performance-Based Seismic Design Guidelines: Program Plan for New and Existing Buildings, Washington, DC.
                #region Acronym Collab
                sCollabPattern = @"(?:(?<=(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel> |^))((?:[A-Z][a-z]+ )(?:[A-Z][a-z]+(?: (?:of|&|and|on|the))? )+(?:[A-Z][a-z]+ ))\(([A-Z]+)(?: [A-Z\-0-9]+)?\)[\.,]*(?=[ ]?<PubDate>))";
                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sCollabPattern, HandleAcronymCollab);
                    if (Regex.IsMatch(sRefContent, "^<Collab>"))
                    {
                        goto LBL_END_COLLAB;
                    }
                }
                #endregion

                //added by Dakshinamoorthy on 2020-Aug-10 (for Acronym Collab)
                //OCMAL (Observatory of Mineral Conflicts in Latin America). Found at: <Website>https://mapa.conflictosmineros.net/ocmal_db-v2/,</Website> <ldlAccessedDateLabel>Accessed</ldlAccessedDateLabel> <AccessedDate><ldlmonthName>July</ldlmonthName> <ldldayNum>7</ldldayNum>, <ldlyear>2020</ldlyear>.</AccessedDate>
                #region Acronym Collab
                sCollabPattern = @"(?:(?<=(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel> |^))(?:[A-Z]+ )(\([A-Za-z ]+\)\.) )";
                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                    goto LBL_END_COLLAB;
                }
                #endregion




                //added by Dakshinamoorthy on 2020-Jul-24 (for Acronym Collab)
                //2. MORTH (Ministry of Road Transport and Highways). 2018. Road accidents in India 2018. New Delhi, India: MORTH.
                #region Acronym Collab
                sCollabPattern = @"(?:(?<=(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel> |^))([A-Z]+) (\((?:[A-Z][a-z]+(?: (?:of|&|and|on|the))?(?:[ ]|\)))+))";
                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sCollabPattern, HandleAcronymCollab1);
                    if (Regex.IsMatch(sRefContent, "^<Collab>"))
                    {
                        goto LBL_END_COLLAB;
                    }
                }
                #endregion



                nPatternId = 0;
                sCollabPattern = @"(?:^(?:[\u2014][ ]?)+)";
                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                    goto LBL_END_COLLAB;
                }

                nPatternId = 0;
                sCollabPattern = @"(?:^(?:[\u005F][ ]?)+)";
                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                    goto LBL_END_COLLAB;
                }

                ////added by Dakshinamoorthy on 2020-Jul-14
                //nPatternId = 0;
                //sCollabPattern = @"(?:([A-Z])(?:[a-z]+ )([A-Z])(?:[a-z]+ )([A-Z])(?:[a-z]+ )([A-Z])(?:[a-z]+ )(\(\1\2\3\4\))[\., ]+)";
                //if (Regex.IsMatch(sRefContent, sCollabPattern))
                //{
                //    sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                //    goto LBL_END_COLLAB;
                //}

                //nPatternId = 1;
                //sCollabPattern = @"^(?:(?:[A-Z]{3,} )(?:\(?(?:[A-Z][a-z]+(?: (?:of|on))?[\ \)])+)[\:\.,;]* )";
                //if (Regex.IsMatch(sRefContent, sCollabPattern))
                //{
                //    sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                //    goto LBL_END_COLLAB;
                //}

                nPatternId = 2;
                sCollabPattern = @"^([^\.,><0-9\u201c\u2019\u2018\u2019]{5,}\.?)(?=[ ]?<PubDate>)";
                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    string sCollabContent = Regex.Match(sRefContent, sCollabPattern).Value.ToString();
                    if (IsValidPublisherName(sCollabContent))
                    {
                        sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                        goto LBL_END_COLLAB;
                    }
                }

                //added by Dakshinamoorthy on 2020-Jul-10
                nPatternId = 2;
                sCollabPattern = string.Format(@"^(?:(?:[A-Z\/{0}]+\.?)(?=[ ]?<PubDate>))", sCapsNonEnglishChar);
                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                    goto LBL_END_COLLAB;
                }

                //added by Dakshinamoorthy on 2020-Jul-14
                nPatternId = 2;
                sCollabPattern = string.Format(@"^(?:(?:(?:[A-Z{0}][a-z{1}]+){{2,}})\.?(?=[ ]?<PubDate>))", sCapsNonEnglishChar, sSmallNonEnglishChar);
                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                    goto LBL_END_COLLAB;
                }

                //added by Dakshinamoorthy on 2020-Jul-14
                nPatternId = 2;
                sCollabPattern = string.Format(@"^(?:(?:[A-Z{0}]{{3,}}) [A-Z{0}0-9\-\/ ]+[\.,])(?=[ ]?<PubDate>)", sCapsNonEnglishChar);
                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                    goto LBL_END_COLLAB;
                }

                //added by Dakshinamoorthy on 2019-Sep-13
                nPatternId = 3;
                //updated by Dakshinamoorthy on 2020-May-19 (to add "<RefPrefix>")
                sCollabPattern = @"(?:(?<=(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel> |^))(?:(?:(?:\b(?:[A-Z][a-z]{2,})\b)+ ){2,}(?:(?:and|&) )?(?:(?:\b(?:[A-Z][a-z]{2,})\b)+ ){2,}\([A-Z]+\)[\.]? ))";
                //reverted by Dakshinamoorthy on 2020-May-19 (to remove "<RefPrefix>")
                //sCollabPattern = @"(?:(?<=(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel> (?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>[ ]*)?|^(?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>[ ]*)?))(?:(?:(?:\b(?:[A-Z][a-z]{2,})\b)+ ){2,}(?:(?:and|&) )?(?:(?:\b(?:[A-Z][a-z]{2,})\b)+ ){2,}\([A-Z]+\)[\.]? ))";

                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                    goto LBL_END_COLLAB;
                }

                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternId = 4;

                //updated by Dakshinamoorthy on 2020-May-19 (to add "<RefPrefix>")
                sCollabPattern = @"(?:(?<=(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel> |^))(?:(?:[A-Z]+[0-9]+\-[0-9]+\-[0-9]+\.?)|(?:[A-Z]{2,5}\/[A-Z]{2,5}[0-9]+))(?=[ ]?<PubDate>))";

                //reverted by Dakshinamoorthy on 2020-May-19 (to from "<RefPrefix>")
                //sCollabPattern = @"(?:(?<=(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel> (?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>[ ]*)?|^(?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>[ ]*)?))(?:(?:[A-Z]+[0-9]+\-[0-9]+\-[0-9]+\.?)|(?:[A-Z]{2,5}\/[A-Z]{2,5}[0-9]+))(?=[ ]?<PubDate>))";

                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                    goto LBL_END_COLLAB;
                }

                //added by Dakshinamoorthy on 2019-Oct-01
                nPatternId = 5;
                //updated by Dakshinamoorthy on 2020-May-19 (to add "<RefPrefix>")
                sCollabPattern = @"(?:(?<=(?:(?:^<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)[ ]*)|^)(?:<Website>((?:(?!</?Website>).)+)</Website>))";

                //reverted by Dakshinamoorthy on 2020-May-19 (to from "<RefPrefix>")
                //sCollabPattern = @"(?:(?<=(?:(?:^<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)[ ]*(?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>[ ]*)?)|^(?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>[ ]*)?)(?:<Website>((?:(?!</?Website>).)+)</Website>))";

                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$1</Collab>");
                    goto LBL_END_COLLAB;
                }

                //added by Dakshinamoorthy on 2019-Oct-03
                nPatternId = 6;

                //updated by Dakshinamoorthy on 2020-May-19 (to add "<RefPrefix>")
                sCollabPattern = @"(?:(?<=(?:(?:^<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)[ ]*)|^)(?:[A-Z]{2,}\-[A-Z0-9]{4,}\/[A-Z0-9]{4,})[\.\:]* )";

                //reverted by Dakshinamoorthy on 2020-May-19 (to remove "<RefPrefix>")
                //sCollabPattern = @"(?:(?<=(?:(?:^<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)[ ]*(?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>[ ]*)?)|^(?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>[ ]*)?)(?:[A-Z]{2,}\-[A-Z0-9]{4,}\/[A-Z0-9]{4,})[\.\:]* )";

                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                    goto LBL_END_COLLAB;
                }

                //added by Dakshinamoorthy on 2020-Jul-23
                nPatternId = 7;
                sCollabPattern = @"(?:(?<=(?:(?:^<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)[ ]*)|^)(?:[A-Za-z\- ]+[.,]+)(?=[ ]?(?:(?:<PubDate>)|[\u201C\u2018])))";
                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    string sCollabContent = Regex.Match(sRefContent, sCollabPattern).Value.ToString();
                    if (IsValidPublisherName(sCollabContent))
                    {
                        sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                        goto LBL_END_COLLAB;
                    }
                }

                //added by Dakshinamoorthy on 2020-Dec-15
                nPatternId = 8;
                sCollabPattern = @"(?:(?<=(?:(?:^<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)[ ]*)|^)(?:(?:(?:[^\.,;:<>\(\)\[\]]+), (?:Inc|Ltd))\. ))";
                if (Regex.IsMatch(sRefContent, sCollabPattern))
                {
                    string sCollabContent = Regex.Match(sRefContent, sCollabPattern).Value.ToString();
                    if (IsValidPublisherName(sCollabContent))
                    {
                        sRefContent = Regex.Replace(sRefContent, sCollabPattern, "<Collab>$&</Collab>");
                        goto LBL_END_COLLAB;
                    }
                }

            LBL_END_COLLAB:

                string s = string.Empty;
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyCollab", ex.Message, true, "");
            }
            return true;
        }

        private bool DoPreCleanup4PatternMatch(ref string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "</?e?(?:surname|forename)>", "", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, "</?e?givenname>", "", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, "</?e?suffix>", "", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, "</?etal>", "", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, "</?year>", "", RegexOptions.IgnoreCase);
                sRefContent = Regex.Replace(sRefContent, "</?collab>", "", RegexOptions.IgnoreCase);

                sRefContent = Regex.Replace(sRefContent, string.Format("</?{0}>", sRefTypeLocalExePtn), "", RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\DoPreCleanup4PatternMatch", ex.Message, true, "");
            }
            return true;
        }

        //added by Dakshinamoorthy on 2020-May-19
        private bool IdentifyRefPrefix(ref string sRefContent)
        {
            try
            {
                sRefPrefixContent = string.Empty;
                sRefContent = Regex.Replace(sRefContent, sRefPrefixPattern, string.Format("<{0}>{1}</{0}>", "RefPrefix", "$&"), RegexOptions.IgnoreCase);
                NormalizeSpaces(ref sRefContent);

                if (Regex.IsMatch(sRefContent, @"(<RefPrefix>((?:(?!</?RefPrefix>).)+)</RefPrefix>)"))
                {
                    sRefPrefixContent = Regex.Match(sRefContent, @"(<RefPrefix>((?:(?!</?RefPrefix>).)+)</RefPrefix>)").Groups[1].Value;
                    sRefContent = Regex.Replace(sRefContent, @"(<RefPrefix>((?:(?!</?RefPrefix>).)+)</RefPrefix>)", "");
                    NormalizeSpaces(ref sRefContent);

                    //added by Dakshinamoorthy on 2020-Jun-09
                    if (Regex.IsMatch(sRefPrefixContent, "^(?:<RefPrefix>and</RefPrefix>)$", RegexOptions.IgnoreCase))
                    {
                        //Insert Dummy Collab Tag
                        sRefContent = string.Format("<Collab>~~dummy~~</Collab> {0}", sRefContent);
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyRefPrefix", ex.Message, true, "");
                return false;
            }
            return true;
        }

        public bool IdentifyRefLabel(ref string sRefContent)
        {
            //string sRefLabelPattern = @"^(?:(?:<[bi]>[\(\[]?)?(?:[0-9]+\.)(?:[\]\)]?</[bi]>)?[ ])";
            //string sRefLabelPattern = @"(?<=^(?:<ldlFirstAuEdCollabGroup><(?:ldlCollab|ldlAuthorEditorGroup)>)?)(?:(?:(?:<[bi]>)?[\(\[]?)?(?:(?:[0-9]+|[a-z])[\.\-]?)(?:[\]\)]?[\.\-]?(?:</[bi]>)?)?[ ])";
            //updated by Dakshinamoorthy on 2019-Sep-24
            string sRefLabelPattern = @"(?<=^(?:<ldlFirstAuEdCollabGroup><(?:ldlCollab|ldlAuthorEditorGroup)>)?)(?:(?:(?:(?:<(?:[bi]|sup)>)?[\(\[]?)?(?:(?:[0-9]+|[a-z])[\.\-]?)(?:[\]\)]?[\.\-]?(?:</(?:[bi]|sup)>)?)?[ ])|(?:<sup>[0-9]+</sup>)[ ]?)";

            try
            {
                sRefContent = Regex.Replace(sRefContent, sRefLabelPattern, "<RefLabel>$&</RefLabel>");
                sRefContent = Regex.Replace(sRefContent, "^(<ldlFirstAuEdCollabGroup><(?:ldlCollab|ldlAuthorEditorGroup)>)(<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>)", "$2$1");
                sRefContent = Regex.Replace(sRefContent, "[ ]+</RefLabel>", "</RefLabel> ");
            }
            catch (Exception ex)
            {

                //System.Windows.Forms.MessageBox.Show(ex.Message);
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyRefLabel", ex.Message, true, "");
            }
            return true;
        }

        private bool IdentifyEds4AUG(ref string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, string.Format("({0})", sEdsBackPattern), "<ldlEditorDelimiterEds_Back>$1</ldlEditorDelimiterEds_Back>");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyEds4AUG", ex.Message, true, "");
            }
            return true;
        }


        private bool IdentifyEtAl(ref string sRefContent)
        {
            string sRefLabelPattern = @"(?:(?:(?:<i>)?[Ee]t[\- ]al(?:</i>)?)[\.;,\: ]+)";
            try
            {
                sRefContent = Regex.Replace(sRefContent, sRefLabelPattern, "<Etal>$&</Etal>");
            }
            catch (Exception ex)
            {

                //System.Windows.Forms.MessageBox.Show(ex.Message);
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyEtAl", ex.Message, true, "");
            }
            return true;
        }

        private bool IdentifyDateInfo(ref string sRefContent)
        {

            sRefContent = Regex.Replace(sRefContent, "</?FirstPage>", "");
            sRefContent = Regex.Replace(sRefContent, "</?LastPage>", "");

            //updated by Dakshinamoorthy on 2019-Oct-08
            //string sFourDigitYearPtn = @"\b(?<!<yearRange_4_2>)(?:(?:(?:(?:(?:1[8-9])(?:[0-9]{2}))|(?:(?:20)(?:[0-1])(?:[0-9])))[\-]?[a-z]?))\b";
            //string sFourDigitYearPtn = @"\b(?<!<yearRange_4_2>)(?:(?:(?:(?:(?:1[8-9])(?:[0-9]{2}))|(?:(?:20)(?:[0-1])(?:[0-9])))(?:(?:<i>[a-z]</i>)|(?:[\-]?[a-z]?\b))))";
            //updated by Dakshinamoorthy on 2019-Jun-17
            string sFourDigitYearPtn = @"\b(?<!<yearRange_4_2>)(?:(?:(?:(?:(?:1[8-9])(?:[0-9]{2}))|(?:(?:20)(?:[0-1])(?:[0-9])|(?:20)[0-9][0-9]))(?:(?:<i>[a-z]</i>)|(?:[\-]?[a-z]?\b))))";

            string sFourTwoDigitYearRngPtn = @"\b(?:(?:(?:(?:(?:1[8-9])(?:[0-9]{2}))|(?:(?:20)(?:[0-1])(?:[0-9])))[\-]?[a-z]?))(?:[ ]?[\u2013\-][ ]?[0-9]{2})\b";
            string sMonthNamePtn = @"\b(?:(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\b";
            string sMonthNumPtn = @"\b(?:(?:[0]?[1-9])|(?:[1][0-2]))\b";
            string sDaysInMonthNumPtn = @"\b(?<!<monthNum>)(?:(?:[0]?[1-9])|(?:[1-2][0-9])|(?:[3][0-1]))(?:(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?)?\b";
            string sDayNamePtn = @"\b(?:Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday)\b";
            string sMiscYearPtn = @"(?:(?:\((?:n\.d\.)(?:\-[a-z])?\)\.?)|n\.d\.|\b(?:[Uu]ndated)|\b(?:forthcoming)\.|(?:\((?:in press(?:\-[a-z])?)\)\.?))";
            string sSeasonPtn = "(?:Spring|Winter|Summer|Autumn|Fall)";
            string sFullDate_MMDDYYYY_Ptn = @"(?:\b(?:(?:[0]?[1-9])|(?:[1][0-2]))\b[\/]\b(?<!<monthNum>)(?:(?:[0]?[1-9])|(?:[1-2][0-9])|(?:[3][0-1]))\b[\/]\b(?:(?:(?:(?:(?:19)(?:[0-9]{2}))|(?:(?:20)(?:[0-1])(?:[0-9])))))\b)";
            string sFullDate_DDMMYYYY_Ptn = @"(?:\b(?<!<monthNum>)(?:(?:[0]?[1-9])|(?:[1-2][0-9])|(?:[3][0-1]))\b[\/]\b(?:(?:[0]?[1-9])|(?:[1][0-2]))\b[\/]\b(?:(?:(?:(?:(?:19)(?:[0-9]{2}))|(?:(?:20)(?:[0-1])(?:[0-9])))))\b)";

            try
            {
                sRefContent = Regex.Replace(sRefContent, string.Format("({0})", sFourTwoDigitYearRngPtn), string.Format("<yearRange_4_2>{0}</yearRange_4_2>", "$1"));
                //validate year range 4_2
                sRefContent = Regex.Replace(sRefContent, @"<(yearRange_4_2)>((?:(?!</?yearRange_4_2>).)+)</\1>", CleanUpYearRange_4_2_Digit);
                sRefContent = Regex.Replace(sRefContent, @"<(yearRange_4_2)>((?:(?!</?yearRange_4_2>).)+)</\1>", HanldeYearRange_4_2_Digit);

                sRefContent = Regex.Replace(sRefContent, string.Format("({0})", sFourDigitYearPtn), string.Format("<year>{0}</year>", "$1"));
                sRefContent = Regex.Replace(sRefContent, string.Format("({0})", sMonthNamePtn), string.Format("<monthName>{0}</monthName>", "$1"));
                sRefContent = Regex.Replace(sRefContent, string.Format("({0})", sMonthNumPtn), string.Format("<monthNum>{0}</monthNum>", "$1"));
                sRefContent = Regex.Replace(sRefContent, string.Format("({0})", sDaysInMonthNumPtn), string.Format("<dayNum>{0}</dayNum>", "$1"));
                sRefContent = Regex.Replace(sRefContent, string.Format("({0})", sDayNamePtn), string.Format("<dayName>{0}</dayName>", "$1"));
                sRefContent = Regex.Replace(sRefContent, string.Format("({0})", sMiscYearPtn), string.Format("<yearMisc>{0}</yearMisc>", "$1"));
                sRefContent = Regex.Replace(sRefContent, string.Format("({0})", sSeasonPtn), string.Format("<season>{0}</season>", "$1"));
                //sRefContent = Regex.Replace(sRefContent, string.Format("({0})", sFullDate_MMDDYYYY_Ptn), string.Format("<fullDateMMDDYYYY>{0}</fullDateMMDDYYYY>", "$1"));
                //sRefContent = Regex.Replace(sRefContent, string.Format("({0})", sFullDate_DDMMYYYY_Ptn), string.Format("<fullDateDDMMYYYY>{0}</fullDateDDMMYYYY>", "$1"));

                sRefContent = Regex.Replace(sRefContent, @"</year>([ ]?(?:[\u2013\-]|&#8211;)[ ]?)<year>", "$1");
                sRefContent = Regex.Replace(sRefContent, @"</monthName>([ ]?(?:[\u2013\-]|&#8211;)[ ]?)<monthName>", "$1");
                sRefContent = Regex.Replace(sRefContent, @"</monthNum>([ ]?(?:[\u2013\-]|&#8211;)[ ]?)<monthNum>", "$1");
                sRefContent = Regex.Replace(sRefContent, @"</dayNum>([ ]?(?:[\u2013\-]|&#8211;)[ ]?)<dayNum>", "$1");
                sRefContent = Regex.Replace(sRefContent, @"</dayName>([ ]?(?:[\u2013\-]|&#8211;)[ ]?)<dayName>", "$1");
                sRefContent = Regex.Replace(sRefContent, @"</yearMisc>([ ]?(?:[\u2013\-]|&#8211;)[ ]?)<yearMisc>", "$1");
                sRefContent = Regex.Replace(sRefContent, @"</season>([ ]?(?:[\u2013\-]|&#8211;)[ ]?)<season>", "$1");

                //normalize
                sRefContent = Regex.Replace(sRefContent, @"</monthName>([ \.,]+)<monthNum>((?:(?!</?monthNum>).)+)</monthNum>", "</monthName>$1<dayNum>$2</dayNum>");
                sRefContent = Regex.Replace(sRefContent, @"<monthNum>((?:(?!</?monthNum>).)+)</monthNum>([ \.,]+)<monthName>", "<dayNum>$1</dayNum>$2<monthName>");
                sRefContent = Regex.Replace(sRefContent, @"<year>((?:(?!</?year>).)+)([\-\u2013])</year>", "<year>$1</year>$2");
                sRefContent = Regex.Replace(sRefContent, @"<monthNum>((?:(?!</?monthNum>).)+)</monthNum>/<monthNum>((?:(?!</?monthNum>).)+)</monthNum>/<year>((?:(?!</?year>).)+)</year>",
                    "<monthNum>$1</monthNum>/<dayNum>$2</dayNum>/<year>$3</year>");
                //added by Dakshinamoorthy on 2020-Jul-07
                sRefContent = Regex.Replace(sRefContent, @"<sup>((?:(?!</?(?:dayNum|sup)>).)+)</dayNum></sup>", "<sup>$1</sup></dayNum>");


                sRefContent = Regex.Replace(sRefContent, @"<monthNum>((?:(?!</?monthNum>).)+)</monthNum>, <monthNum>((?:(?!</?monthNum>).)+)</monthNum>, <year>((?:(?!</?year>).)+)</year>",
    "<monthNum>$1</monthNum>, <dayNum>$2</dayNum>, <year>$3</year>");

                sRefContent = Regex.Replace(sRefContent, @"<monthNum>((?:(?!</?monthNum>).)+)</monthNum>/<dayNum>((?:(?!</?dayNum>).)+)</dayNum>/<year>((?:(?!</?year>).)+)</year>",
    "<fullDateMMDDYYYY>$&</fullDateMMDDYYYY>");
                sRefContent = Regex.Replace(sRefContent, @"<dayNum>((?:(?!</?dayNum>).)+)</dayNum>/<monthNum>((?:(?!</?monthNum>).)+)</monthNum>/<year>((?:(?!</?year>).)+)</year>",
    "<fullDateDDMMYYYY>$&</fullDateDDMMYYYY>");

                sRefContent = Regex.Replace(sRefContent, @"</fullDateMMDDYYYY>([ ]?(?:[\u2013\-]|&#8211;)[ ]?)<fullDateMMDDYYYY>", "$1");
                sRefContent = Regex.Replace(sRefContent, @"</fullDateDDMMYYYY>([ ]?(?:[\u2013\-]|&#8211;)[ ]?)<fullDateDDMMYYYY>", "$1");



                List<string> lstDateHighLevelPtn = new List<string>();
                lstDateHighLevelPtn.Add(@"(?:\((?:<year>(?:(?!</?year>).)+</year>, )+(?:<year>(?:(?!</?year>).)+</year>)\)(?:[\:\.;,]+(?: |$)))");
                lstDateHighLevelPtn.Add(@"\(<year/>[\u2013\-\/]<year/>\)[\:\.;,]+(?:(?: |$))");
                lstDateHighLevelPtn.Add(@"<year/>[\u2013\-]<year/>[\.](?:(?: |$))");
                lstDateHighLevelPtn.Add(@"\(<year/>,? <monthName/> <dayNum/>\)(?:[\:\.;,]+(?: |$)|$)");
                lstDateHighLevelPtn.Add(@"\(<monthName/>[\.,]? <dayNum/>, <year/>\)(?:[\:\.;,]+(?: |$)|$)");
                //added by Dakshinamoorthy on 2019-Sep-14
                lstDateHighLevelPtn.Add(@"[\(\[]<monthNum/>, <dayNum/>, <year/>[\)\]](?:[\:\.;,]+(?: |$)|$)");
                //added by Dakshinamoorthy on 2019-Sep-14
                lstDateHighLevelPtn.Add(@"\(<monthName/> <year/>\/[ ]?<monthName/> <year/>\)(?:[\:\.;,]+(?: |$)|$)");

                lstDateHighLevelPtn.Add(@"\(<monthName/>\/[ ]?<monthName/>[\.,]? <year/>\)(?:[\:\.;,]+(?: |$)|$)");
                lstDateHighLevelPtn.Add(@"<monthName/>[\.,]? <dayNum/>, <year/>\)(?:[\:\.;,]+(?: |$)|$)");
                lstDateHighLevelPtn.Add(@" <year/>,? <monthName/> <dayNum/>(?:[\:\.;,]+(?: |$))");
                lstDateHighLevelPtn.Add(@" <monthName/> <dayNum/>, <year/>[\]]?(?:[\:\.;,]*(?: |$))");
                lstDateHighLevelPtn.Add(@" <monthName/>\. <dayNum/>, <year/>(?:[\:\.;,]*(?: |$))");
                lstDateHighLevelPtn.Add(@" <dayNum/> <monthName/>[\.,]? <year/>[\)\]]?(?:[\:\.;,]*(?: |$))");
                lstDateHighLevelPtn.Add(@" <year/>,? <monthName/>(?:[\:\.;,]+(?: |$))");
                lstDateHighLevelPtn.Add(@" <dayNum/> <monthName/>(?:[\:\.;,]*(?: |$))");
                lstDateHighLevelPtn.Add(@" <monthName/>,? <year/>(?:[\:\.;,]+(?: |$))");
                lstDateHighLevelPtn.Add(@" <monthName/> <year/>(?:\)[\:\.;,]+(?: |$))");
                lstDateHighLevelPtn.Add(@"\(<monthName/>(?:\.?,)? <year/>\)(?:[\:\.;,]+(?: |$)|$)");
                lstDateHighLevelPtn.Add(@"\(<monthNum/>[,;] <year/>\)(?:[\:\.;,]+(?: |$)|$)");
                lstDateHighLevelPtn.Add(@" <monthName/> <dayNum/>,(?:[\:\.;,]*(?: |$))");
                lstDateHighLevelPtn.Add(@"\[<year/>\] <year/>(?:[\:\.;,]+(?: |$)|$)");
                lstDateHighLevelPtn.Add(@" <year/> [\[\(]<year/>[\]\)](?:[\:\.;,]+(?: |$)|$)");

                lstDateHighLevelPtn.Add(@"\(<season/>[,]? <year/>\)(?:[\:\.;,]*(?: |$)|$)");
                lstDateHighLevelPtn.Add(@"(?<=,) <season/> <year/>(?:[\:\.;,]+(?: |$)|$)");

                lstDateHighLevelPtn.Add(@"\(<year/>, <monthName/>\)(?:[\:\.;,]*(?: |$)|$)");
                lstDateHighLevelPtn.Add(@"\(<year/>, <season/>\)(?:[\:\.;,]+(?: |$)|$)");
                lstDateHighLevelPtn.Add(@"<year/>\)(?:[\:\.;,]*(?: |$))");
                lstDateHighLevelPtn.Add(@"\(<year/>\)(?:[\:\.;,]+(?: |$))");
                lstDateHighLevelPtn.Add(@"\(<yearMisc/>\)(?:[\:\.;,]+(?: |$))");
                lstDateHighLevelPtn.Add(@"\(<year/>[\:\.;,]+\)(?:(?: |$))");

                lstDateHighLevelPtn.Add(@"\(<yearRange_4_2/>\)(?:[\:\.;,]+(?: |$))");
                lstDateHighLevelPtn.Add(@"(?:(?<=[\.] )(?:<year/>)(?= [\u201C]))");
                lstDateHighLevelPtn.Add(@"<year/>(?:[\:\.;,\)]+(?: |$))");
                lstDateHighLevelPtn.Add(@"<yearMisc/>(?:[\:\.;,]*(?: |$))");

                //added by Dakshinamoorthy on 2020-Dec-11
                lstDateHighLevelPtn.Add(@"(?:(?<=(?:</[A-Za-z]+> ))<year/>(?=(?: <[A-Za-z]+>)))");

                string sEachDatePtn = string.Empty;

                foreach (string sEachString in lstDateHighLevelPtn)
                {
                    sEachDatePtn = sEachString;
                    sEachDatePtn = Regex.Replace(sEachDatePtn, "<(year|yearRange_4_2|monthName|monthNum|dayNum|dayName|yearMisc|season)/>", string.Format("(?:<{0}>(?:(?:(?!</?{0}>).)+)</{0}>)", "$1"));
                    sRefContent = Regex.Replace(sRefContent, sEachDatePtn, ProcessDateInfo);
                }

                sRefContent = Regex.Replace(sRefContent, "</?(year|yearRange_4_2|monthName|monthNum|dayNum|dayName|yearMisc|season)>", "");

                //post clean-up
                sRefContent = Regex.Replace(sRefContent, @"([\(\[])<PubDate>", "<PubDate>$1");
                sRefContent = Regex.Replace(sRefContent, @"[ ]+</PubDate>", "</PubDate> ");

                //added by Dakshinamoorthy on 2020-Aug-10
                sRefContent = RemoveUnwantedDateInfoInTitle(sRefContent);

                //full date
                sRefContent = Regex.Replace(sRefContent, @"<fullDateMMDDYYYY>", "<PubDate>");
                sRefContent = Regex.Replace(sRefContent, @"</fullDateMMDDYYYY>", "</PubDate>");

                sRefContent = Regex.Replace(sRefContent, @"<fullDateDDMMYYYY>", "<PubDate>");
                sRefContent = Regex.Replace(sRefContent, @"</fullDateDDMMYYYY>", "</PubDate>");

                sRefContent = Regex.Replace(sRefContent, @"</ldlyear>([\u2013\-])<ldlyear>", "$1");
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyDateInfo", ex.Message, true, "");
            }
            return true;
        }

        private string RemoveUnwantedDateInfoInTitle(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, @"(?:<i>(?:(?!</?i>).)+</i>)", RemoveUnwantedDateInfo);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveUnwantedDateInfoInTitle", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string RemoveUnwantedDateInfo(Match myDateMacth)
        {
            string sOutputContent = myDateMacth.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "(?:</?(?:(?:ldl(?:year|monthName|monthNum|dayNum|dayName|yearMisc|season))|PubDate)>)", "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RemoveUnwantedDateInfo", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private string CleanUpYearRange_4_2_Digit(Match myMatch)
        {
            string sTagName = myMatch.Groups[1].Value.ToString();
            string sValue = myMatch.Groups[2].Value.ToString();

            try
            {
                sValue = Regex.Replace(sValue, "</?[^<>]+>", "");
                sValue = string.Format("<{0}>{1}</{0}>", sTagName, sValue);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CleanUpYearRange_4_2_Digit", ex.Message, true, "");
            }
            return sValue;
        }

        private string HanldeYearRange_4_2_Digit(Match myMatch)
        {
            string sTagName = myMatch.Groups[1].Value.ToString();
            string sValue = myMatch.Groups[2].Value.ToString();
            string sActualContent = myMatch.Value.ToString();

            try
            {
                sValue = Regex.Replace(sValue, "<[a-zA-Z]+>", "");
                sValue = Regex.Replace(sValue, "</[a-zA-Z]+>", "");

                sValue = Regex.Replace(sValue, @"[\[\]\(\)\.,\:;]", "");
                sValue = sValue.Trim();

                if (Regex.IsMatch(sValue, @"^(?:([0-9]{4})(?:[ ]?[\u2013\-][ ]?)([0-9]{2}))$"))
                {
                    string sYear_4_Digit = Regex.Match(sValue, @"^(?:([0-9]{4})(?:[ ]?[\u2013\-][ ]?)([0-9]{2}))$").Groups[1].Value.ToString();
                    string sYear_2_Digit = Regex.Match(sValue, @"^(?:([0-9]{4})(?:[ ]?[\u2013\-][ ]?)([0-9]{2}))$").Groups[2].Value.ToString();
                    string sYear_4_Digit_Last_Two = sYear_4_Digit.Substring(2, 2);

                    if (Convert.ToInt32(sYear_4_Digit_Last_Two) < Convert.ToInt32(sYear_2_Digit))
                    {
                        return sActualContent;
                    }
                    else
                    {
                        return sValue;
                    }
                }
                else
                {
                    return sValue;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HanldeYearRange_4_2_Digit", ex.Message, true, "");
            }

            return sValue;
        }


        private bool IdentifyFirstAuthorGroup(ref string sRefContent)
        {
            try
            {
                if (Regex.IsMatch(sRefContent, @"(?:<ldlFirstAuEdCollabGroup>(?:(?!</?ldlFirstAuEdCollabGroup>).)+</ldlFirstAuEdCollabGroup>)"))
                {
                    sRefContent = Regex.Replace(sRefContent, @"(?:<ldlFirstAuEdCollabGroup>(?:(?!</?ldlFirstAuEdCollabGroup>).)+</ldlFirstAuEdCollabGroup>)", HandleFirstAuEdCollabGroup);
                    sRefContent = Regex.Replace(sRefContent, "<ldlCollab>", "<Collab>");
                    sRefContent = Regex.Replace(sRefContent, "</ldlCollab>", "</Collab>");

                    ////post cleanup
                    //sRefContent = Regex.Replace(sRefContent, "</?ldlFirstAuEdCollabGroup>", "");
                    //sRefContent = Regex.Replace(sRefContent, "</?ldlAuthorEditorGroup>", "");
                }
                //else
                //{
                //    //updated by Dakshinamoorthy on 2020-May-19 (to add "<RefPrefix>")
                //    //string sFirstAuthorGrpArea = @"(?:(?:(?<=^|</(?:RefLabel|Label)>)(?:(?!(?:</?auStop>|</?lCase>|</?Etal>|</?PubDate>|</?MayBeJouTitAbbr>|</?Collab>|</?RefLabel>|</?Label>|(?:&#x201[CcDd];)|[\u201c\u201d]|\b(?:[0-9]{2,})|[\(\)\[\]])).){3,}))";
                //    string sFirstAuthorGrpArea = @"(?:(?:(?<=^|</(?:RefLabel|RefPrefix|Label)>)(?:(?!(?:</?auStop>|</?lCase>|</?Etal>|</?PubDate>|</?MayBeJouTitAbbr>|</?Collab>|</?RefPrefix>|</?RefLabel>|</?Label>|(?:&#x201[CcDd];)|[\u201c\u201d]|\b(?:[0-9]{2,})|[\(\)\[\]])).){3,}))";
                //    sRefContent = IdentifyAuthorGroupStopWord(sRefContent);
                //    sRefContent = IdentifyAUG_BoundaryByPunctuation(sRefContent);
                //    //sRefContent = Regex.Replace(sRefContent, sFirstAuthorGrpArea, GetValidAuthorGroupEnd);
                //    sRefContent = Regex.Replace(sRefContent, sFirstAuthorGrpArea, ProcessAuthorGroup);
                //    sRefContent = Regex.Replace(sRefContent, "(</?auStop>|</?lCase>|</?MayBeJouTitAbbr>)", "", RegexOptions.IgnoreCase);
                //}
                //added by Dakshinamoorthy on 2020-Apr-01
                else
                {
                    //updated by Dakshinamoorthy on 2020-May-19 (to add "<RefPrefix>")
                    string sFirstAuthorGrpArea = @"(?:(?:(?<=^|</(?:RefLabel|Label)>[ ])(?:(?!(?:</?auStop>|</?lCase>|</?Etal>|</?PubDate>|</?MayBeJouTitAbbr>|</?Collab>|</?RefLabel>|</?Label>|(?:&#x201[CcDd];)|[\u201c\u201d]|\b(?:[0-9]{2,})|[\(\)\[\]])).){3,}))";

                    //reverted by Dakshinamoorthy on 2020-May-19 (to remove "<RefPrefix>")
                    //string sFirstAuthorGrpArea = @"(?:(?:(?<=(?:^|</(?:RefLabel|RefPrefix|Label)>[ ]))(?:(?!(?:</?auStop>|</?lCase>|</?Etal>|</?PubDate>|</?MayBeJouTitAbbr>|</?Collab>|</?RefPrefix>|</?RefLabel>|</?Label>|(?:&#x201[CcDd];)|[\u201c\u201d]|\b(?:[0-9]{2,})|[\(\)\[\]])).){3,}))";

                    sRefContent = IdentifyAuthorGroupStopWord(sRefContent);
                    sRefContent = IdentifyAUG_BoundaryByPunctuation(sRefContent);

                    //added by Dakshinamoorthy on 2020-Jun-10
                    sRefContent = Regex.Replace(sRefContent, "<auStop>[ ]+", " <auStop>");

                    sRefContent = Regex.Replace(sRefContent, sFirstAuthorGrpArea, ProcessAuthorGroupNew);
                    //post cleanup
                    sRefContent = PostCleanupForAuthorGroup(sRefContent);
                    sRefContent = Regex.Replace(sRefContent, "(</?auStop>|</?lCase>|</?MayBeJouTitAbbr>)", "", RegexOptions.IgnoreCase);

                    //added by Dakshinamoorthy on 2020-Jul-24 (for handling unwanted suffix)
                    sRefContent = HandleUnwantedSuffix(sRefContent);

                    //added by Dakshinamoorthy on 2020-Jul-07
                    sRefContent = Regex.Replace(sRefContent, "(</(?:Author|Editor)>)((?:(?!</?(?:Author|Editor|AuEdGroup)>).)+)</AuEdGroup>", "$1</AuEdGroup>$2", RegexOptions.IgnoreCase);
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyFirstAuthorGroup", ex.Message, true, "");
            }
            return true;
        }

        private string HandleUnwantedSuffix(string sRefTaggedContnet)
        {
            try
            {
                sRefTaggedContnet = Regex.Replace(sRefTaggedContnet, @"(?:<Author><Surname>((?:(?!</?Surname>).)+)</Surname> <Forename>((?:[A-Z]\.?[ ]?)+)</Forename> <Suffix>((?:[A-Z][\., ]*))</Suffix></Author>)", "<Author><Surname>$1</Surname> <Forename>$2 $3</Forename></Author>");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleUnwantedSuffix", ex.Message, true, "");
            }
            return sRefTaggedContnet;
        }


        private string PostCleanupForAuthorGroup(string sAuthorGroupContent)
        {
            string sOutputContent = sAuthorGroupContent;
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, @"(?:</?(?:GRP_START|GRP_AND|GRP_END)>)", "");

                sOutputContent = Regex.Replace(sOutputContent, "<AU>", "<Author>");
                sOutputContent = Regex.Replace(sOutputContent, "</AU>", "</Author>");

                sOutputContent = Regex.Replace(sOutputContent, "<SNM>", "<Surname>");
                sOutputContent = Regex.Replace(sOutputContent, "</SNM>", "</Surname>");

                sOutputContent = Regex.Replace(sOutputContent, "<GNM>", "<Forename>");
                sOutputContent = Regex.Replace(sOutputContent, "</GNM>", "</Forename>");

                sOutputContent = Regex.Replace(sOutputContent, "<SUF>", "<Suffix>");
                sOutputContent = Regex.Replace(sOutputContent, "</SUF>", "</Suffix>");

                sOutputContent = Regex.Replace(sOutputContent, "<DEL_AND>", "<ldlAuthorDelimiterAnd>");
                sOutputContent = Regex.Replace(sOutputContent, "</DEL_AND>", "</ldlAuthorDelimiterAnd>");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyFirstAuthorGroup", ex.Message, true, "");
                return sAuthorGroupContent;
            }
            return sOutputContent;
        }




        //added by Dakshinamoorthy on 2019-Jan-05
        //for identifying author boundary
        //Example:
        //<RefLabel>[31].</RefLabel> M.F. Zhu, C.P. Hong, D.M. Stefanescu, Y.A. Chang, Computational Modeling <lCase>of</lCase> Microstructure Evolution <lCase>in</lCase> Solidification <lCase>of</lCase> Aluminum Alloys, <i>Metallurgical and Materials Transactions B</i>, 38, (517–524), <PubDate>(<ldlyear>2007</ldlyear>).</PubDate>

        private string IdentifyAUG_BoundaryByPunctuation(string sRefContent)
        {
            try
            {
                //updated by Dakshinamoorthy on 2020-Dec-18
                //sRefContent = Regex.Replace(sRefContent, @"(?<=[\.,;>]+)(?:(?!(?:[\.,;<>]+)).)+(?=<lCase>)", "<auStop>$&</auStop>");
                sRefContent = Regex.Replace(sRefContent, @"(?<=[\.,;>]+ )(?:(?!(?:[\.,;<>]+)).)+(?=<lCase>)", "<auStop>$&</auStop>");
                sRefContent = Regex.Replace(sRefContent, @"( (?:and|&|&#x0026;) )<MayBeJouTitAbbr>((?:(?!</?MayBeJouTitAbbr>).)+)</MayBeJouTitAbbr>", "$1$2");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyAUG_BoundaryByPunctuation", ex.Message, true, "");
            }
            return sRefContent;
        }


        private string HandleFirstAuEdCollabGroup(Match myAuEdCollabGrp)
        {
            string sAuEdCollabTaggedContent = myAuEdCollabGrp.Value.ToString();

            try
            {
                sAuEdCollabTaggedContent = Regex.Replace(sAuEdCollabTaggedContent, @"(?<=(?:<ldlAuthorEditorGroup>))(?:(?:(?!</?ldlAuthorEditorGroup>).)+)(?=</ldlAuthorEditorGroup>)", ProcessAuthorGroup);

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleFirstAuEdCollabGroup", ex.Message, true, "");
            }

            return sAuEdCollabTaggedContent;
        }




        private bool IdentifyEditorGroup(ref string sRefContent)
        {
            try
            {
                NormalizeSpaces(ref sRefContent);

                //updated by Dakshinamoorthy on 2019-Oct-08
                sRefContent = Regex.Replace(sRefContent, @"(?:\b(?:[Tt]he) [^ ]+\.)", HandleEscapeSpace);

                //rename already identified Author/Editor
                sRefContent = Regex.Replace(sRefContent, "<Author>", "<Author1>");
                sRefContent = Regex.Replace(sRefContent, "</Author>", "</Author1>");

                sRefContent = Regex.Replace(sRefContent, "<Editor>", "<Editor1>");
                sRefContent = Regex.Replace(sRefContent, "</Editor>", "</Editor1>");

                sRefContent = Regex.Replace(sRefContent, @"(?:(?<=(?:\.(?:(?:</[^<>]+>)|[\u201D])?|(?:\?[\u201C\u2019]?)) )(?:[Ii]n)(?= ))", "<auStop>$&</auStop>");
                sRefContent = Regex.Replace(sRefContent, @"(?:(?:[^\.<>\u201c\u201D\u2019\u2018 ]+\. )+([Tt]rans\. )(?:[^\.<>\u201c\u201D\u2019\u2018 ]+\. )+)", EscapeDot4ConfName);

                string sEdsFullPattern = string.Format("(?:{0}|{1})", sEdsFrontPattern, sEdsBackPattern);

                sRefContent = Regex.Replace(sRefContent, sEdsFullPattern, sHandleEds);

                //added by Dakshinamoorthy on 2020-Aug-24
                string sEdsBackFront = @"(?:(?:<ldlEditorDelimiterEds_Front>(?:(?!<ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>)(?:[ ]?(?:\b(?:and)\b|&)[ ]?)(?:<ldlEditorDelimiterEds_Back>(?:(?!<ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>))";
                sRefContent = Regex.Replace(sRefContent, sEdsBackFront, MakeEdsBack);

                //back
                string sEdsBackBoundaryPtn = @"(?<=(?:(?:</?(?:Author[1]?|Collab|Etal|PubDate|ldl[^<> ]+)>)|(&#x201[CcDd];)|(?:\b(?:In[\:]?)\b[ ])|[\u201c\u201d]|\b[0-9]{2,}|[\(\[\]\)]))(?:(?!(?:(?:</?(?:Author[1]?|Collab|Etal|PubDate|ldl[^<> ]+)>)|[\u201c\u201d]|\b[0-9]{2,}|[\(\[\]\)])).)+(?=(?:[ ]?<Etal>(?:(?!</?Etal>).)+</Etal>[\.\:,; ]*)?<ldlEditorDelimiterEds_Back>)";
                bIsEditorBackSearch = true;
                sRefContent = Regex.Replace(sRefContent, sEdsBackBoundaryPtn, HandleEdsBackBoundary);
                bIsEditorBackSearch = false;

                if (Regex.IsMatch(sRefContent, "<ldlEditorDelimiterEds_Back>") && !Regex.IsMatch(sRefContent, "<Author>"))
                {
                    sRefContent = Regex.Replace(sRefContent, "<ldlEditorDelimiterEds_Back>", "<ldlEditorDelimiterEds_Front>");
                    sRefContent = Regex.Replace(sRefContent, "</ldlEditorDelimiterEds_Back>", "</ldlEditorDelimiterEds_Front>");
                }

                //front
                string sEdsFrontBoundaryPtn = @"(?:(?<=(?:</ldlEditorDelimiterEds_Front>))(?:(?:(?![\(\)]|</?ldl[^<>]+>).)+))";
                bIsEditorFrontSearch = true;
                sRefContent = Regex.Replace(sRefContent, sEdsFrontBoundaryPtn, HandleEdsFrontBoundary);
                bIsEditorFrontSearch = false;

                sRefContent = Regex.Replace(sRefContent, "(</?auStop>|</?lCase>)", "", RegexOptions.IgnoreCase);

                //rename already identified Author/Editor
                sRefContent = Regex.Replace(sRefContent, "<Author1>", "<Author>");
                sRefContent = Regex.Replace(sRefContent, "</Author1>", "</Author>");

                sRefContent = Regex.Replace(sRefContent, "<Editor1>", "<Editor>");
                sRefContent = Regex.Replace(sRefContent, "</Editor1>", "</Editor>");

                if (Regex.IsMatch(sRefContent, "</?Editor>", RegexOptions.IgnoreCase) == false)
                {
                    sRefContent = Regex.Replace(sRefContent, @"<ldlEditorDelimiterEds_Front>(Trans\.[ ]*)</ldlEditorDelimiterEds_Front>", "$1");
                }

                sRefContent = Regex.Replace(sRefContent, "(~~dot~~)", ".");
                sRefContent = Regex.Replace(sRefContent, @"\(<Author>", "<Author>(");
                //added by Dakshinamoorthy on 2020-Jui-11
                sRefContent = Regex.Replace(sRefContent, @"<AuEdGroup></AuEdGroup>", "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyEditorGroup", ex.Message, true, "");
            }
            return true;
        }

        private string MakeEdsBack(Match myEdsMatch)
        {
            string sOutputContent = myEdsMatch.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "(?:</?(?:ldlEditorDelimiterEds_Front|ldlEditorDelimiterEds_Back)>)", "");
                sOutputContent = string.Format("<{0}>{1}</{0}>", "ldlEditorDelimiterEds_Back", sOutputContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\MakeEdsBack", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private string HandleEdsFrontBoundary(Match myEdsFrontMatch)
        {
            string sEdsFrontTagged = myEdsFrontMatch.Value.ToString();
            try
            {

                sEdsFrontTagged = IdentifyAuthorGroupStopWord(sEdsFrontTagged);
                sEdsFrontTagged = HexUnicodeToCharConvertor(ConvertDecimalToHexDecimal(sEdsFrontTagged));
                sEdsFrontTagged = Regex.Replace(sEdsFrontTagged, @"(?<=</lCase> )(?:[^,\.,;]{3,})[\.,;]+", string.Format("<{0}>{1}</{0}>", "auStop", "$&"));
                sEdsFrontTagged = Regex.Replace(sEdsFrontTagged, @"(?<=,</auStop> )(?:[A-Z][^A-Z,\:\.;]{3,} and [A-Z][^A-Z,\:\.;]{3,}[\:\.;,]+)", string.Format("<{0}>{1}</{0}>", "auStop", "$&"));
                sEdsFrontTagged = Regex.Replace(sEdsFrontTagged, @"(?<=</auStop>)(?:[0-9][^\.,;]+[\.;,]+)", string.Format("<{0}>{1}</{0}>", "auStop", "$&"));
                sEdsFrontTagged = Regex.Replace(sEdsFrontTagged, @"</auStop>([ ]?[Ii]n[\.\:]? )", "$1</auStop>");

                string sExactFrontEditorGroupPtn = @"^(?:(?:(?!(?:</?(?:Author|Collab|Etal|PubDate|lCase|auStop|ldl[^<> ]+)>|[\u201c\u201d]|\b[0-9]{2,})).)+)";
                if (Regex.IsMatch(sEdsFrontTagged, sExactFrontEditorGroupPtn))
                {
                    sEdsFrontTagged = Regex.Replace(sEdsFrontTagged, sExactFrontEditorGroupPtn, ProcessAuthorGroup);
                }
                else
                {
                    //updated by Dakshinamoorthy on 2020-Jul-10
                    //sEdsFrontTagged = Regex.Replace(sEdsFrontTagged, "^(.+)$", ProcessAuthorGroup);
                    sEdsFrontTagged = Regex.Replace(sEdsFrontTagged, "^(.+)$", ProcessEditorGroupNew);
                    sEdsFrontTagged = PostCleanupForAuthorGroup(sEdsFrontTagged);
                }

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleEdsFrontBoundary", ex.Message, true, "");
            }
            return sEdsFrontTagged;
        }

        private string HandleEdsBackBoundary(Match myEdsBackMatch)
        {
            string sEdsBackTagged = myEdsBackMatch.Value.ToString();
            try
            {

                sEdsBackTagged = IdentifyAuthorGroupStopWord(sEdsBackTagged);
                sEdsBackTagged = HexUnicodeToCharConvertor(ConvertDecimalToHexDecimal(sEdsBackTagged));
                //updated by Dakshinamoorthy on 2019-Oct-31
                //sEdsBackTagged = Regex.Replace(sEdsBackTagged, @"(?<=</lCase> )(?:[^,\.,;\u2019\u201C]{3,})[\.,;\u2019\u201C ]+(?=[ $<])", string.Format("<{0}>{1}</{0}>", "auStop", "$&"));
                sEdsBackTagged = Regex.Replace(sEdsBackTagged, @"(?<=</lCase> )(?:(?:(?!(?:</?(?:lCase|auStop)>|[Ii]n\:? |[,\.,;\u2019\u201C]))).){3,}[\.,;\u2019\u201C\? ]+(?=[ $<])", string.Format("<{0}>{1}</{0}>", "auStop", "$&"));

                sEdsBackTagged = Regex.Replace(sEdsBackTagged, @"(?<=,</auStop> )(?:[A-Z][^A-Z,\:\.;]{3,} and [A-Z][^A-Z,\:\.;]{3,}[\:\.;,]+)", string.Format("<{0}>{1}</{0}>", "auStop", "$&"));
                sEdsBackTagged = Regex.Replace(sEdsBackTagged, @"(?<=</auStop>)(?:[0-9][^\.,;]+[\.;,]+)", string.Format("<{0}>{1}</{0}>", "auStop", "$&"));
                sEdsBackTagged = Regex.Replace(sEdsBackTagged, @"</auStop>([ ]?[Ii]n[\.\:]? )", "$1</auStop>");
                sEdsBackTagged = Regex.Replace(sEdsBackTagged, @"</lCase>([ ]?[Ii]n[\.\:]? )", "$1</lCase>");
                //added by Dakshinamoorthy on 2020-Jul-11
                sEdsBackTagged = Regex.Replace(sEdsBackTagged, @"(?:(?<=[\.,] )[Ii]n[\:\.]?(?=[ ]))", "<auStop>$&</auStop>");

                string sExactBackEditorGroupPtn = @"(?<=(?:</(?:Author|Collab|Etal|PubDate|lCase|auStop)>))(?:(?!(?:</?(?:Author|Collab|Etal|PubDate|lCase|auStop|ldl[^<> ]+)>|[\u201c\u201d]|\b[0-9]{2,})).)+(?=$)";
                if (Regex.IsMatch(sEdsBackTagged, sExactBackEditorGroupPtn))
                {
                    //updated by Dakshinamoorthy on 2020-Jul-10
                    //sEdsBackTagged = Regex.Replace(sEdsBackTagged, sExactBackEditorGroupPtn, ProcessAuthorGroup);
                    sEdsBackTagged = Regex.Replace(sEdsBackTagged, sExactBackEditorGroupPtn, ProcessEditorGroupNew);
                    sEdsBackTagged = PostCleanupForAuthorGroup(sEdsBackTagged);
                }
                else
                {
                    //updated by Dakshinamoorthy on 2020-Jul-10
                    //sEdsBackTagged = Regex.Replace(sEdsBackTagged, "^(.+)$", ProcessAuthorGroup);
                    sEdsBackTagged = Regex.Replace(sEdsBackTagged, "^(.+)$", ProcessEditorGroupNew);
                    sEdsBackTagged = PostCleanupForAuthorGroup(sEdsBackTagged);
                }

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleEdsBackBoundary", ex.Message, true, "");
            }
            return sEdsBackTagged;
        }



        private string sHandleEds(Match myEdsMatch)
        {
            string sEdsContent = myEdsMatch.Value.ToString();
            try
            {
                if (myEdsMatch.Groups["edsfront"] != null && !string.IsNullOrEmpty(myEdsMatch.Groups["edsfront"].Value))
                {
                    sEdsContent = string.Format("<{0}>{1}</{0}>", "ldlEditorDelimiterEds_Front", myEdsMatch.Value.ToString());
                    return sEdsContent;
                }
                else if (myEdsMatch.Groups["edsback"] != null && !string.IsNullOrEmpty(myEdsMatch.Groups["edsback"].Value.ToString()))
                {
                    sEdsContent = string.Format("<{0}>{1}</{0}>", "ldlEditorDelimiterEds_Back", myEdsMatch.Value.ToString());
                    return sEdsContent;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\sHandleEds", ex.Message, true, "");
            }
            return sEdsContent;
        }

        private bool ChageShortToLongTag4Format(ref string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "<i>", "<Italic>");
                sRefContent = Regex.Replace(sRefContent, "</i>", "</Italic>");

                sRefContent = Regex.Replace(sRefContent, "<b>", "<Bold>");
                sRefContent = Regex.Replace(sRefContent, "</b>", "</Bold>");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\Decimal2Hexa", ex.Message, true, "");
            }
            return true;
        }

        private bool ChageLongToShortTag4Format(ref string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "<Italic>", "<i>");
                sRefContent = Regex.Replace(sRefContent, "</Italic>", "</i>");

                sRefContent = Regex.Replace(sRefContent, "<Bold>", "<b>");
                sRefContent = Regex.Replace(sRefContent, "</Bold>", "</b>");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ChageLongToShortTag4Format", ex.Message, true, "");
            }
            return true;
        }

        private string IdentifyAuthorGroupStopWord(string sRefContent)
        {
            try
            {
                ChageShortToLongTag4Format(ref sRefContent);

                //added by Dakshinamoorthy on 2020-Jul-07
                sRefContent = Regex.Replace(sRefContent, @"(?:(?: (?:(?:[A-Z][a-z]{2,}\.?)|(?:[A-Z])))+(?: [0-9]+, [0-9]+))", string.Format("<auStop>$&</auStop>"));

                //added by Dakshinamoorthy on 2019-Jan-12
                sRefContent = Regex.Replace(sRefContent, @"(?:(?:\b(?:[J])\. ){2,})", EscapeDot4ConfName);
                sRefContent = Regex.Replace(sRefContent, @"(?:~~dot~~)", "~~DOT~~");

                sRefContent = Regex.Replace(sRefContent, string.Format(@"\b(?<!(?:[<])|(?:</)|(?:[\-\u2019]))(?:[a-z{0}]+)(?!=[>])\b", sSmallNonEnglishChar), ProcessLowerCaseWord);
                sRefContent = Regex.Replace(sRefContent, "</lCase>( (?:and|&) )<lCase>", "$1");

                sRefContent = Regex.Replace(sRefContent, "</lCase>( (?:and|&) )<lCase>", "$1");
                sRefContent = Regex.Replace(sRefContent, "<lCase>([\u2019](?:(?!</?lCase>).)*)</lCase>", "$1");

                //added by Dakshinamoorthy for Single Sentence Reference With Journal title abbriviation
                //<RefLabel>[1]</RefLabel> Y. Fujimoto, M. Nakatsuka, Jpn. J. Appl. Phys. <PubDate><ldlyear>2001</ldlyear>,</PubDate> 40, L279.

                //commented by Dakshinamoorthy on 2020-Dec-07
                //sRefContent = Regex.Replace(sRefContent, sJouTitAbbrPattern, string.Format("<{0}>{1}</{0}>", "MayBeJouTitAbbr", "$&"));
                //sRefContent = Regex.Replace(sRefContent, @"<Italic>(?:(?!</?Italic>).)+</Italic>", string.Format("<{0}>{1}</{0}>", "MayBeJouTitAbbr", "$&"));

                //added by Dakshinamoorthy on 2020-Jul-07
                sRefContent = Regex.Replace(sRefContent, @"( Int\. J\.[ ]*)((?:<auStop>[ ]*)?<MayBeJouTitAbbr>)", "$2$1 ");

                //sRefContent = Regex.Replace(sRefContent, @"\b((?:[A-Z][^<> ]+)(?= (?:<lCase>)))", "<auStop>$1</auStop>");
                sRefContent = Regex.Replace(sRefContent, @"</lCase>\?", "?</lCase>");
                sRefContent = Regex.Replace(sRefContent, @"~~dot~~", ".", RegexOptions.IgnoreCase);

                ChageLongToShortTag4Format(ref sRefContent);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyAuthorGroupStopWord", ex.Message, true, "");
            }
            return sRefContent;
        }


        private string HandleGrammaticalTermsInAuArea(Match MyAuArea)
        {
            string sAuAreaContent = MyAuArea.Value.ToString();
            try
            {
                sAuAreaContent = Regex.Replace(sAuAreaContent, @"(?:\b(?:The)\b)", "<auStop>$&</auStop>");
                sAuAreaContent = Regex.Replace(sAuAreaContent, @"(?:(?:<Italic>(?:(?!</?Italic>).)+</Italic>)[\:\.;, ]*)", "<auStop>$&</auStop>");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleGrammaticalTermsInAuArea", ex.Message, true, "");
            }
            return sAuAreaContent;
        }



        private string ProcessLowerCaseWord(Match myLCaseMatch)
        {
            string sContent = myLCaseMatch.Value.ToString();
            try
            {
                string sSkipLowerCase4Author = "(?:and|da|d|dal|de|del|den|der|des|di|du|la|le|mac|mc|san|ten|van|von|ya)";
                if (!Regex.IsMatch(sContent, string.Format("^{0}$", sSkipLowerCase4Author)))
                {
                    sContent = string.Format("<lCase>{0}</lCase>", sContent);
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ProcessLowerCaseWord", ex.Message, true, "");
            }
            return sContent;
        }

        private bool ChangeYearTag(ref string sRefContent)
        {
            nPubDateCount = 0;
            try
            {
                nPubDateCount = 0;
                nAccessedDateCount = 0;
                bIsConfProcTypeRef = false;

                if (Regex.IsMatch(sRefContent, @"(?:Proceedings|Conference )|\b(Proc\.|Conf\.)", RegexOptions.IgnoreCase))
                {
                    bIsConfProcTypeRef = true;
                }

                //commented by Dakshinamoorthy on 2019-Feb-01
                //remove unwanted date info
                //sRefContent = Regex.Replace(sRefContent, @"(?:\([^\(\)]+<PubDate>(?:(?!</?PubDate>).)+\)[\:\.;, ]*</PubDate>)", HandleUnwantedPubDate);

                //nPubDateCount
                sRefContent = Regex.Replace(sRefContent, "<PubDate>((?:(?!</?PubDate>).)+)</PubDate>", sProcessPubDate);
                sRefContent = Regex.Replace(sRefContent, "<AccessedDate>((?:(?!</?AccessedDate>).)+)</AccessedDate>", sProcessAccessedDate);

                //added by Dakshinamoorthy on 2019-Feb-08
                sRefContent = Regex.Replace(sRefContent, @"(?:\([^\(\)]+<ldlConferenceDate>[^\(\)]+\)[\:.,; ]*</ldlConferenceDate>)", HandleUnwantedPubDate);

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ChangeYearTag", ex.Message, true, "");
            }
            return true;
        }


        private string HandleUnwantedPubDate(Match myDateMatch)
        {
            string sOutputContent = myDateMatch.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "</?ldl(?:year|monthName|monthNum|dayNum|dayName|yearMisc)>", "");
                sOutputContent = Regex.Replace(sOutputContent, "</?PubDate>", "");
                sOutputContent = Regex.Replace(sOutputContent, "</?ldlConferenceDate>", "");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleUnwantedPubDate", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private bool IdentifyPubMedId(ref string sRefContent)
        {
            try
            {
                string sPubMedIdPattern = @"(\b(?:P[Mm][Ii][Dd][: ]+))((?:[0-9]+)\b)";
                sRefContent = Regex.Replace(sRefContent, sPubMedIdPattern, string.Format("<{0}>{1}</{0}><{2}>{3}</{2}>", "PubMedIdLabel", "$1", "PubMedIdNumber", "$2"));
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyPubMedId", ex.Message, true, "");
            }
            return true;
        }

        private string sProcessPubDate(Match myPubDateMatch)
        {
            string sOutput = myPubDateMatch.Groups[1].Value;
            try
            {
                if (nPubDateCount == 0)
                {
                    sOutput = Regex.Replace(sOutput, "<ldl(?:year|yearMisc)>", "<Year>");
                    sOutput = Regex.Replace(sOutput, "</ldl(?:year|yearMisc)>", "</Year>");

                    sOutput = Regex.Replace(sOutput, "<ldl(?:monthName|monthNum)>", "<Month>");
                    sOutput = Regex.Replace(sOutput, "</ldl(?:monthName|monthNum)>", "</Month>");

                    sOutput = Regex.Replace(sOutput, "<ldl(?:dayNum|dayName)>", "<Day>");
                    sOutput = Regex.Replace(sOutput, "</ldl(?:dayNum|dayName)>", "</Day>");

                    sOutput = Regex.Replace(sOutput, "<ldlseason>", "<Season>");
                    sOutput = Regex.Replace(sOutput, "</ldlseason>", "</Season>");


                    sOutput = string.Format("<PubDate>{0}</PubDate>", sOutput);
                }
                else
                {
                    if (bIsConfProcTypeRef == true && nPubDateCount == 1)
                    {
                        //sOutput = Regex.Replace(sOutput, "<ldl(?:year|yearMisc)>", "<Year>");
                        //sOutput = Regex.Replace(sOutput, "</ldl(?:year|yearMisc)>", "</Year>");

                        //sOutput = Regex.Replace(sOutput, "<ldl(?:monthName|monthNum)>", "<Month>");
                        //sOutput = Regex.Replace(sOutput, "</ldl(?:monthName|monthNum)>", "</Month>");

                        //sOutput = Regex.Replace(sOutput, "<ldl(?:dayNum|dayName)>", "<Day>");
                        //sOutput = Regex.Replace(sOutput, "</ldl(?:dayNum|dayName)>", "</Day>");

                        //sOutput = Regex.Replace(sOutput, "<ldlseason>", "<Season>");
                        //sOutput = Regex.Replace(sOutput, "</ldlseason>", "</Season>");

                        if (Regex.IsMatch(sOutput, @"(?:^(?:<ldlyear>(?:(?!</?ldlyear>).)+</ldlyear>\:)$)") == false)
                        {
                            sOutput = Regex.Replace(sOutput, "</?ldl(?:year|monthName|monthNum|dayNum|dayName|yearMisc)>", "");
                            sOutput = string.Format("<ldlConferenceDate>{0}</ldlConferenceDate>", sOutput);
                        }
                        else
                        {
                            sOutput = Regex.Replace(sOutput, "</?ldl(?:year|monthName|monthNum|dayNum|dayName|yearMisc)>", "");
                        }

                    }
                    else
                    {
                        if (Regex.IsMatch(sOutput, "(?:</?ldlmonthName>)") && Regex.IsMatch(sOutput, "(?:</?ldlyear>)"))
                        {
                            sOutput = string.Format("<ldlTempDate>{0}</ldlTempDate>", sOutput);
                        }
                        else
                        {
                            sOutput = Regex.Replace(sOutput, "</?ldl(?:year|monthName|monthNum|dayNum|dayName|yearMisc)>", "");
                        }
                    }
                }
                nPubDateCount += 1;
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleJournalArticleTitle", ex.Message, true, "");
            }
            return sOutput;
        }

        //added by Dakshinamoorthy on 2020-Jul-11
        private string HandleTempDateInfo(Match myTempDateMatch)
        {
            string sOutputContent = myTempDateMatch.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "</?ldl(?:TempDate|year|monthName|monthNum|dayNum|dayName|yearMisc)>", "");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleTempDateInfo", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private string sProcessAccessedDate(Match myAccessedDateMatch)
        {
            string sOutput = myAccessedDateMatch.Groups[1].Value;
            try
            {
                if (nAccessedDateCount == 0)
                {
                    //sOutput = Regex.Replace(sOutput, "<ldl(?:year|yearMisc)>", "<Year>");
                    //sOutput = Regex.Replace(sOutput, "</ldl(?:year|yearMisc)>", "</Year>");

                    //sOutput = Regex.Replace(sOutput, "<ldl(?:monthName|monthNum)>", "<Month>");
                    //sOutput = Regex.Replace(sOutput, "</ldl(?:monthName|monthNum)>", "</Month>");

                    //sOutput = Regex.Replace(sOutput, "<ldl(?:dayNum|dayName)>", "<Day>");
                    //sOutput = Regex.Replace(sOutput, "</ldl(?:dayNum|dayName)>", "</Day>");

                    //sOutput = Regex.Replace(sOutput, "<ldlseason>", "<Season>");
                    //sOutput = Regex.Replace(sOutput, "</ldlseason>", "</Season>");

                    sOutput = Regex.Replace(sOutput, "</?(?:ldl(?:year|yearMisc)|ldl(?:monthName|monthNum)|ldl(?:dayNum|dayName)|ldlseason)>", "");
                    sOutput = string.Format("<ldlAccessedDate>{0}</ldlAccessedDate>", sOutput);
                }
                else
                {
                    sOutput = Regex.Replace(sOutput, "</?ldl(?:year|monthName|monthNum|dayNum|dayName|yearMisc)>", "");
                }
                nAccessedDateCount += 1;
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleJournalArticleTitle", ex.Message, true, "");
            }
            return sOutput;
        }


        private string ProcessDateInfo(Match myDateMatch)
        {
            string sDateContent = myDateMatch.Value.ToString();
            try
            {
                sDateContent = Regex.Replace(sDateContent, "<(year|monthName|monthNum|dayNum|dayName|yearMisc|season)>", string.Format("<ldl{0}>", "$1"));
                sDateContent = Regex.Replace(sDateContent, "</(year|monthName|monthNum|dayNum|dayName|yearMisc|season)>", string.Format("</ldl{0}>", "$1"));

                //yearRange_4_2
                sDateContent = Regex.Replace(sDateContent, "<yearRange_4_2>", "<ldlyear>");
                sDateContent = Regex.Replace(sDateContent, "</yearRange_4_2>", "</ldlyear>");

                sDateContent = string.Format("<PubDate>{0}</PubDate>", sDateContent);
            }
            catch (Exception ex)
            {

                //System.Windows.Forms.MessageBox.Show(ex.Message);
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ProcessDateInfo", ex.Message, true, "");
            }
            return sDateContent;
        }


        private string GetValidAuthorGroupEnd(Match myAuthorContent)
        {
            string sValidAuthorEnd = myAuthorContent.Value.ToString();
            try
            {
                MatchCollection mcArt = Regex.Matches(sValidAuthorEnd, @"(?:[\.](?=[ ]))");
                if (mcArt != null && mcArt.Count > 0)
                {
                    Match myLastWordMatch = mcArt[mcArt.Count - 1];
                    if (myLastWordMatch != null)
                    {
                        int lastIndexArt = (myLastWordMatch.Index + myLastWordMatch.Length);
                        string sArtFirstPart = sValidAuthorEnd.Substring(0, lastIndexArt);
                        string sArtSecondPart = sValidAuthorEnd.Substring(lastIndexArt, (sValidAuthorEnd.Length - lastIndexArt));
                        sValidAuthorEnd = string.Format("{0}<{1}>{2}<{1}>", sArtFirstPart, "auStop", sArtSecondPart);
                    }
                    else
                    {
                        return sValidAuthorEnd;
                    }
                }
                else
                {
                    return sValidAuthorEnd;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleValidateArticleTitle", ex.Message, true, "");
            }
            return sValidAuthorEnd;
        }

        //added by Dakshinamoorthy on 2020-April-01
        private string ProcessEditorGroupNew(Match myAUGMatch)
        {
            string sRefInputContent = myAUGMatch.Value.ToString();
            sAuthorGroupPattern_Start = AutoStructRefAuthor.sAuthorGroupPattern_Start;
            sAuthorGroupPattern_And = AutoStructRefAuthor.sAuthorGroupPattern_And;
            sAuthorGroupPattern_End = AutoStructRefAuthor.sAuthorGroupPattern_End;
            sRefAuthorDelimeterAnd = AutoStructRefAuthor.sRefAuthorDelimeterAnd;

            try
            {
                string sPattern = string.Empty;

                //Banik, A. & Barai, M. K. (2017). Introduction, In Arindam Banik, Munim Kumar Barai & Yasushi Suzuki (Eds.) Towards A Common Future: Understanding Growth, Sustainability in the Asia-Pacific Region, Palgrave Macmillan.
                if (bIsEditorBackSearch == true)
                {
                    sRefInputContent = sRefInputContent.Trim();
                    sPattern = string.Format("(?<GRP_END>(?<= (?:{0}) )(?:{1})+)$", sRefAuthorDelimeterAnd, sAuthorGroupPattern_End);
                    if (Regex.IsMatch(sRefInputContent, sPattern, RegexOptions.None, TimeSpan.FromMilliseconds(nRegExTimeOut_MS)))
                    {
                        sRefInputContent = Regex.Replace(sRefInputContent, sPattern, string.Format("<{0}>{1}</{0}>", "GRP_END", "$&"));
                        sRefInputContent = Regex.Replace(sRefInputContent, @"(?:<(GRP_END)>((?:(?!</?\1>).)+)</\1>)", ProcessAuthorInnerElements);
                        NormalizeSpaces(ref sRefInputContent);
                        sRefInputContent = Regex.Replace(sRefInputContent, "</GRP_END>", "</GRP_END> ");
                    }

                    //sPattern = string.Format("(?:(?<GRP_START>(?:{0})+)(?<GRP_AND>(?:(?:{1})(?<DEL_AND>{2} )))(?=(?:<GRP_END>(?:(?!</?GRP_END>).)+</GRP_END>[ ]*)$))", sAuthorGroupPattern_Start, sAuthorGroupPattern_And, sRefAuthorDelimeterAnd);
                    sPattern = string.Format("(?:(?<GRP_START>(?:{0})+)(?=(?<GRP_AND>(?:(?:{1})(?<DEL_AND>{2} )))(?:<GRP_END>(?:(?!</?GRP_END>).)+</GRP_END>[ ]*)$))", sAuthorGroupPattern_Start, sAuthorGroupPattern_And, sRefAuthorDelimeterAnd);
                    if (Regex.IsMatch(sRefInputContent, sPattern, RegexOptions.None, TimeSpan.FromMilliseconds(nRegExTimeOut_MS)))
                    {
                        //sPattern = string.Format("(?:(?<=(?:(?:^(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START> ))|^))(?:(?<GRP_AND>(?:(?:{0})(?<DEL_AND>{1} )))))", sAuthorGroupPattern_And, sRefAuthorDelimeterAnd);
                        sRefInputContent = Regex.Replace(sRefInputContent, sPattern, string.Format("<{0}>{1}</{0}>", "GRP_START", "$&"));
                        sRefInputContent = Regex.Replace(sRefInputContent, @"(?:<(GRP_START)>((?:(?!</?\1>).)+)</\1>)", ProcessAuthorInnerElements);
                        NormalizeSpaces(ref sRefInputContent);
                        sRefInputContent = Regex.Replace(sRefInputContent, "</GRP_START>", "</GRP_START> ");
                    }

                    //added by Dakshinamoorthy on 2020-Jul-11
                    sPattern = string.Format("(?:(?<!(?:{0}))(?<GRP_AND>(?:(?:{1})(?<DEL_AND>{2} )))(?=(?:<GRP_END>(?:(?!</?GRP_END>).)+</GRP_END>[ ]*)$))", sAuthorGroupPattern_Start, sAuthorGroupPattern_And, sRefAuthorDelimeterAnd);
                    if (Regex.IsMatch(sRefInputContent, sPattern, RegexOptions.None, TimeSpan.FromMilliseconds(nRegExTimeOut_MS)))
                    {
                        sRefInputContent = Regex.Replace(sRefInputContent, sPattern, string.Format("<{0}>{1}</{0}>", "GRP_AND", "$&"));
                        sRefInputContent = Regex.Replace(sRefInputContent, @"(?:<(GRP_AND)>((?:(?!</?\1>).)+)</\1>)", ProcessAuthorInnerElements);
                        NormalizeSpaces(ref sRefInputContent);
                        sRefInputContent = Regex.Replace(sRefInputContent, "</GRP_AND>", "</GRP_AND> ");
                    }


                    sPattern = string.Format("(?:(?:(?<=^(?:(?:(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START>) (?:<GRP_AND>(?:(?!</?GRP_AND>).)+</GRP_AND>))|(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START>)|(?:<GRP_AND>(?:(?!</?GRP_AND>).)+</GRP_AND>)) )|^)(?<GRP_END>{0}))", sAuthorGroupPattern_End);
                    if (Regex.IsMatch(sRefInputContent, sPattern, RegexOptions.None, TimeSpan.FromMilliseconds(nRegExTimeOut_MS)))
                    {
                        sRefInputContent = Regex.Replace(sRefInputContent, sPattern, string.Format("<{0}>{1}</{0}>", "GRP_END", "$&"));
                        sRefInputContent = Regex.Replace(sRefInputContent, @"(?:<(GRP_END)>((?:(?!</?\1>).)+)</\1>)", ProcessAuthorInnerElements);
                        NormalizeSpaces(ref sRefInputContent);
                        sRefInputContent = Regex.Replace(sRefInputContent, "</GRP_END>", "</GRP_END> ");
                    }
                }
                else if (bIsEditorFrontSearch == true)
                {
                    sPattern = string.Format("^(?<GRP_START>(?:{0})+(?!(?:{1})))", sAuthorGroupPattern_Start, sRefAuthorDelimeterAnd);
                    if (Regex.IsMatch(sRefInputContent, sPattern, RegexOptions.None, TimeSpan.FromMilliseconds(nRegExTimeOut_MS)))
                    {
                        sRefInputContent = Regex.Replace(sRefInputContent, sPattern, string.Format("<{0}>{1}</{0}>", "GRP_START", "$&"));
                        sRefInputContent = Regex.Replace(sRefInputContent, @"(?:<(GRP_START)>((?:(?!</?\1>).)+)</\1>)", ProcessAuthorInnerElements);
                        NormalizeSpaces(ref sRefInputContent);
                        sRefInputContent = Regex.Replace(sRefInputContent, "</GRP_START>", "</GRP_START> ");
                    }

                    sPattern = string.Format("(?:(?<=(?:(?:^(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START> ))|^))(?:(?<GRP_AND>(?:(?:{0})(?<DEL_AND>{1} )))(?<GRP_END>{2})))", sAuthorGroupPattern_And, sRefAuthorDelimeterAnd, sAuthorGroupPattern_End);
                    if (Regex.IsMatch(sRefInputContent, sPattern, RegexOptions.None, TimeSpan.FromMilliseconds(nRegExTimeOut_MS)))
                    {
                        sPattern = string.Format("(?:(?<=(?:(?:^(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START> ))|^))(?:(?<GRP_AND>(?:(?:{0})(?<DEL_AND>{1} )))))", sAuthorGroupPattern_And, sRefAuthorDelimeterAnd);
                        sRefInputContent = Regex.Replace(sRefInputContent, sPattern, string.Format("<{0}>{1}</{0}>", "GRP_AND", "$&"));
                        sRefInputContent = Regex.Replace(sRefInputContent, @"(?:<(GRP_AND)>((?:(?!</?\1>).)+)</\1>)", ProcessAuthorInnerElements);
                        NormalizeSpaces(ref sRefInputContent);
                        sRefInputContent = Regex.Replace(sRefInputContent, "</GRP_AND>", "</GRP_AND> ");
                    }

                    sPattern = string.Format("(?:(?:(?<=^(?:(?:(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START>) (?:<GRP_AND>(?:(?!</?GRP_AND>).)+</GRP_AND>))|(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START>)|(?:<GRP_AND>(?:(?!</?GRP_AND>).)+</GRP_AND>)) )|^)(?<GRP_END>{0}))", sAuthorGroupPattern_End);
                    if (Regex.IsMatch(sRefInputContent, sPattern, RegexOptions.None, TimeSpan.FromMilliseconds(nRegExTimeOut_MS)))
                    {
                        sRefInputContent = Regex.Replace(sRefInputContent, sPattern, string.Format("<{0}>{1}</{0}>", "GRP_END", "$&"));
                        sRefInputContent = Regex.Replace(sRefInputContent, @"(?:<(GRP_END)>((?:(?!</?\1>).)+)</\1>)", ProcessAuthorInnerElements);
                        NormalizeSpaces(ref sRefInputContent);
                        sRefInputContent = Regex.Replace(sRefInputContent, "</GRP_END>", "</GRP_END> ");
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ProcessAuthorGroupNew", ex.Message, true, "");
                return myAUGMatch.Value.ToString();
            }
            //added by Dakshinamoorthy on 2020-Jun-04;
            sRefInputContent = string.Format(" <{0}>{1}</{0}> ", "AuEdGroup", sRefInputContent);
            return sRefInputContent;
        }

        //added by Dakshinamoorthy on 2020-April-01
        private string ProcessAuthorGroupNew(Match myAUGMatch)
        {
            string sRefInputContent = myAUGMatch.Value.ToString();
            sAuthorGroupPattern_Start = AutoStructRefAuthor.sAuthorGroupPattern_Start;
            sAuthorGroupPattern_And = AutoStructRefAuthor.sAuthorGroupPattern_And;
            sAuthorGroupPattern_End = AutoStructRefAuthor.sAuthorGroupPattern_End;
            sRefAuthorDelimeterAnd = AutoStructRefAuthor.sRefAuthorDelimeterAnd;

            try
            {
                string sPattern = string.Empty;
                //updated by Dakshinamoorthy on 2020-Jul-28
                sPattern = string.Format("^(?<GRP_START>(?:{0})+(?!(?:{1})))", sAuthorGroupPattern_Start, sRefAuthorDelimeterAnd);
                //sPattern = string.Format("^(?<GRP_START>(?:{0})+(?:(?:{1})))", sAuthorGroupPattern_Start, sRefAuthorDelimeterAnd);
                //if (Regex.IsMatch(sRefInputContent, sPattern, RegexOptions.None, TimeSpan.FromMilliseconds(nRegExTimeOut_MS)) == false && Regex.Match(sRefInputContent, sPattern, RegexOptions.None, TimeSpan.FromMilliseconds(nRegExTimeOut_MS)).Length > 0)
                if (Regex.IsMatch(sRefInputContent, sPattern, RegexOptions.None, TimeSpan.FromMilliseconds(nRegExTimeOut_MS)))
                {
                    sRefInputContent = Regex.Replace(sRefInputContent, sPattern, string.Format("<{0}>{1}</{0}>", "GRP_START", "$&"));
                    sRefInputContent = Regex.Replace(sRefInputContent, @"(?:<(GRP_START)>((?:(?!</?\1>).)+)</\1>)", ProcessAuthorInnerElements);
                    NormalizeSpaces(ref sRefInputContent);
                    sRefInputContent = Regex.Replace(sRefInputContent, "</GRP_START>", "</GRP_START> ");
                }

                sPattern = string.Format("(?:(?<=(?:(?:^(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START> ))|^))(?:(?<GRP_AND>(?:(?:{0})(?<DEL_AND>{1} )))(?<GRP_END>{2})))", sAuthorGroupPattern_And, sRefAuthorDelimeterAnd, sAuthorGroupPattern_End);
                if (Regex.IsMatch(sRefInputContent, sPattern, RegexOptions.None, TimeSpan.FromMilliseconds(nRegExTimeOut_MS)))
                {
                    sPattern = string.Format("(?:(?<=(?:(?:^(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START> ))|^))(?:(?<GRP_AND>(?:(?:{0})(?<DEL_AND>{1} )))))", sAuthorGroupPattern_And, sRefAuthorDelimeterAnd);
                    sRefInputContent = Regex.Replace(sRefInputContent, sPattern, string.Format("<{0}>{1}</{0}>", "GRP_AND", "$&"));
                    sRefInputContent = Regex.Replace(sRefInputContent, @"(?:<(GRP_AND)>((?:(?!</?\1>).)+)</\1>)", ProcessAuthorInnerElements);
                    NormalizeSpaces(ref sRefInputContent);
                    sRefInputContent = Regex.Replace(sRefInputContent, "</GRP_AND>", "</GRP_AND> ");
                }

                sPattern = string.Format("(?:(?:(?<=^(?:(?:(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START>) (?:<GRP_AND>(?:(?!</?GRP_AND>).)+</GRP_AND>))|(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START>)|(?:<GRP_AND>(?:(?!</?GRP_AND>).)+</GRP_AND>)) )|^)(?<GRP_END>{0}))", sAuthorGroupPattern_End);
                if (Regex.IsMatch(sRefInputContent, sPattern, RegexOptions.None, TimeSpan.FromMilliseconds(nRegExTimeOut_MS)))
                {
                    sRefInputContent = Regex.Replace(sRefInputContent, sPattern, string.Format("<{0}>{1}</{0}>", "GRP_END", "$&"));
                    sRefInputContent = Regex.Replace(sRefInputContent, @"(?:<(GRP_END)>((?:(?!</?\1>).)+)</\1>)", ProcessAuthorInnerElements);
                    NormalizeSpaces(ref sRefInputContent);
                    sRefInputContent = Regex.Replace(sRefInputContent, "</GRP_END>", "</GRP_END> ");
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ProcessAuthorGroupNew", ex.Message, true, "");
                return myAUGMatch.Value.ToString();
            }
            //added by Dakshinamoorthy on 2020-Jun-04;
            sRefInputContent = string.Format("<{0}>{1}</{0}>", "AuEdGroup", sRefInputContent);

            //added by Dakshinamoorthy on 2020-May-26;
            sRefInputContent = sRefInputContent + " ";
            return sRefInputContent;
        }

        private string ProcessAuthorInnerElements(Match matchStart)
        {
            string sTagName = matchStart.Groups[1].Value.ToString();
            string sAuthorContent = matchStart.Groups[2].Value.ToString();
            string sAuthorGroupPattern = string.Empty;
            string sAuthorGroupContent = string.Empty;

            try
            {
                switch (sTagName)
                {
                    case "GRP_START":
                        sAuthorGroupPattern = string.Format("^(?<GRP_START>(?:{0})+(?!(?:{1})))", sAuthorGroupPattern_Start, sRefAuthorDelimeterAnd);
                        break;
                    case "GRP_AND":
                        sAuthorGroupPattern = string.Format("(?:(?<=(?:(?:^(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START> ))|^))(?:(?<GRP_AND>(?:(?:{0})(?<DEL_AND>{1} )))))", sAuthorGroupPattern_And, sRefAuthorDelimeterAnd);
                        break;
                    case "GRP_END":
                        sAuthorGroupPattern = string.Format("(?:(?:(?<=^(?:(?:(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START>) (?:<GRP_AND>(?:(?!</?GRP_AND>).)+</GRP_AND>))|(?:<GRP_START>(?:(?!</?GRP_START>).)+</GRP_START>)|(?:<GRP_AND>(?:(?!</?GRP_AND>).)+</GRP_AND>)) )|^)(?<GRP_END>{0}))", sAuthorGroupPattern_End);
                        break;
                    default:
                        return sAuthorContent;
                }

                Match mcAuthorNames = Regex.Match(sAuthorContent, sAuthorGroupPattern);
                Regex regex = new Regex(sAuthorGroupPattern);
                GroupCollection groups = regex.Match(sAuthorContent).Groups;
                var grpNames = regex.GetGroupNames();

                List<Tuple<string, string, int, int>> lstAuthorGroupInfo = new List<Tuple<string, string, int, int>>();
                foreach (var grpName in grpNames)
                {
                    if ((Regex.IsMatch(grpName, "^(?:AU)(?:[0-9]+)$") || Regex.IsMatch(grpName, "^(?:DEL_AND)$")) && string.IsNullOrEmpty(groups[grpName].Value) == false)
                    {
                        for (int i = 0; i <= mcAuthorNames.Groups[grpName].Captures.Count - 1; i++)
                        {
                            lstAuthorGroupInfo.Add(new Tuple<string, string, int, int>(grpName, mcAuthorNames.Groups[grpName].Captures[i].Value, mcAuthorNames.Groups[grpName].Captures[i].Index, mcAuthorNames.Groups[grpName].Captures[i].Length));
                        }
                    }
                }

                lstAuthorGroupInfo = lstAuthorGroupInfo.OrderBy(x => x.Item3).ThenByDescending(y => y.Item4).ToList();

                StringBuilder sbAuthorGroupTagged = new StringBuilder();
                foreach (var eachAuthorInfo in lstAuthorGroupInfo)
                {
                    if (Regex.IsMatch(eachAuthorInfo.Item1, "^(?:AU[0-9]+)$"))
                    {
                        sbAuthorGroupTagged.Append(string.Format("<{0}>{1}</{0}>", "AU", IdentifyAuthorInnerElements(eachAuthorInfo.Item2, sAuthorGroupPattern, sTagName) + " "));
                    }
                    else if (Regex.IsMatch(eachAuthorInfo.Item1, "^(?:DEL_AND)$"))
                    {
                        sbAuthorGroupTagged.Append(string.Format("<{0}>{1}</{0}>", "DEL_AND", eachAuthorInfo.Item2));
                    }
                }

                sAuthorGroupContent = sbAuthorGroupTagged.ToString();
                sAuthorGroupContent = sAuthorGroupContent + " ";
                NormalizeSpaces(ref sAuthorGroupContent);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ProcessAuthorInnerElements", ex.Message, true, "");
                return matchStart.Value.ToString();
            }

            sAuthorGroupContent = string.Format("<{0}>{1}</{0}>", sTagName, sAuthorGroupContent);
            return sAuthorGroupContent;
        }

        private static string IdentifyAuthorInnerElements(string sAuthorContent, string sPattern, string sTagName)
        {
            string sOutputContent = sAuthorContent;
            try
            {
                if (sTagName.Equals("GRP_AND"))
                {
                    sAuthorContent = string.Format("{0} and ", sAuthorContent.TrimEnd());
                }

                Regex regex = new Regex(sPattern);
                GroupCollection groups = regex.Match(sAuthorContent).Groups;
                var grpNames = regex.GetGroupNames();

                List<Tuple<string, string, int, int>> lstAuthorInnerElementsInfo = new List<Tuple<string, string, int, int>>();
                foreach (var grpName in grpNames)
                {
                    if (Regex.IsMatch(grpName, "^(?:SNM|GNM|SUF)(?:[0-9]+_[0-9]+)$") && string.IsNullOrEmpty(groups[grpName].Value) == false)
                    {
                        lstAuthorInnerElementsInfo.Add(new Tuple<string, string, int, int>(grpName, groups[grpName].Value, groups[grpName].Index, groups[grpName].Length));
                    }
                }

                lstAuthorInnerElementsInfo = lstAuthorInnerElementsInfo.OrderBy(x => x.Item3).ThenByDescending(y => y.Item4).ToList();

                if (lstAuthorInnerElementsInfo.Count > 0)
                {
                    StringBuilder sbAuthorInnerElementTagged = new StringBuilder();
                    foreach (var eachAuthorInfo in lstAuthorInnerElementsInfo)
                    {
                        sbAuthorInnerElementTagged.Append(string.Format("<{0}>{1}</{0}>", GetTagName(eachAuthorInfo.Item1), eachAuthorInfo.Item2));
                    }
                    sOutputContent = sbAuthorInnerElementTagged.ToString();
                }
                else
                {
                    sOutputContent = sAuthorContent;
                }

            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyAuthorInnerElements", ex.Message, true, "");
                return sAuthorContent;
            }
            return sOutputContent;
        }

        private static string GetTagName(string sTagNameWithNumber)
        {
            string sOutputContent = string.Empty;
            try
            {
                string sAuthorPartPattern = @"(?:(?:SNM|GNM|SUF|AU)(?:[0-9]+|[0-9]+_[0-9]+))";
                if (Regex.IsMatch(sTagNameWithNumber, sAuthorPartPattern))
                {
                    sOutputContent = Regex.Replace(sTagNameWithNumber, "(?:[0-9]+|[0-9]+_[0-9]+)$", "");
                }
                else
                {
                    sOutputContent = sTagNameWithNumber;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\GetTagName", ex.Message, true, "");
                return sTagNameWithNumber;
            }
            return sOutputContent;
        }


        private string ProcessAuthorGroup(Match myAUGMatch)
        {
            string sAUGContent = myAUGMatch.Value.ToString();

            try
            {
                //added by Dakshinamoorthy on 2019-Jan-12
                string sSmallNonEnglishChar_Modified = sSmallNonEnglishChar + "\u2019\u0308\u0327";

                // (?<= |\t|^)(?:(?:(?<fnm>(?:[A-Z](?:[-. ]*[A-Z])*\.? )?(?:Mc|O’|[vV]an der |[vV]an |de ?|la |der |D'|d'|De ?|Le |Di|Mac|Dal |San |du )*[A-ZÁŠ][a-z\x{C0}-\x{17E}]+(?:[-‐][A-Z][a-z\x{C0}-\x{17E}]+)?)[, ]+(?<snm>[A-Z\x{C0}-\x{17E}](?:[-. ]*[A-Z\x{C0}-\x{17E}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?)
                //updated by Dakshinamoorthy on 2018-May-30
                //string sAuthorPattern = string.Format(@"(?<= |\t|^)(?:(?:(?<snm>(?:[A-Z{0}](?:[-. ]*[A-Z{0}])*\.? )?(?:Mc|O’|[vV]an der |[vV]an |de ?|la |der |D'|d'|De ?|Le |Di|Mac|Dal |San |du )*[A-Z{0}][a-z{1}]+(?:[-‐][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?)", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);
                //string sAuthorPattern = string.Format(@"(?<= |\t|^)(?:(?:(?<snm>(?:[A-Z{0}](?:[-. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] |[vV]an |[Dd]e ?|la |der |D[’']|d[’']|De ?|Le |Di|Mac|Dal |San |du )*[A-Z{0}][a-z{1}]+(?:[-‐][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?)", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);

                //sCapsNonEnglishChar = string.Empty;
                //sSmallNonEnglishChar_Modified = string.Empty;

                //string sAuthorPattern = string.Format(@"(?:(?:(?<= |\t|^)(?:(?:(?<snm>(?:[A-Z{0}](?:[-. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di|Mac|Dal ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[-‐][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<= |\t)(?:(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di|Mac|Dal ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[-‐][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)(?=[,.] ))))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);

                //commented by Dakshinamoorthy on 2018-Jun-05
                //string sAuthorPattern = string.Format(@"(?:(?:(?<= |\t|^)(?:(?:(?<snm>(?:[A-Z{0}](?:[-. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[-‐][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?)(?=(?:[,;] |[,;\. ]*(?:$|and|&|[\u2026]|<i>))))|(?:(?<= |\t)(?:(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[-‐][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)(?=[,.] ))))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);
                //string sAuthorPattern = string.Format(@"(?:(?:(?<= |\t|^)(?:(?:(?<snm>(?:[A-Z{0}](?:[-. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[-‐][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<= |\t|^)(?:(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[-‐][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)(?=[,.] ))))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);

                //string sAuthorPattern1 = string.Format(@"(?:(?:(?<= |\t|^)(?:(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[-‐][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)(?=[,.] )))|(?:(?<= |\t|^)(?:(?:(?<snm>(?:[A-Z{0}](?:[-. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[-‐][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?)))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);

                //string sAuthorPatternWithFullCaps = string.Format(@"(?:(?:(?<= |\t|^)(?:(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[-‐][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<= |\t|^)(?:(?:(?<snm>(?:[A-Z{0}](?:[-. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[-‐][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<= |\t|^)(?:(?:(?<snm>(?:MC|O[’']|VAN DE[RN] ?|V[AO]N ?|DE ?|LA ?|DE[RS] ?|D[’']|DE ?|LE ?|DI ?|MAC ?|D[EA]l ?|SAN ?|DU ?)*[A-Z{0}][A-Z{0}]+(?:[-‐][A-Z{0}][A-Z{0}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[-. ]*[A-Z{0}])*\b\.?))(?<deg>(?: JR| DR| I+)?\b)\.?)))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);

                //sAuthorPatternWithFullCaps = string.Format(@"(?:(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[--. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[---][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[--. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}](?:[--. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[---][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:MC|O[’']|VAN DE[RN] ?|V[AO]N ?|DE ?|LA ?|DE[RS] ?|D[’']|DE ?|LE ?|DI ?|MAC ?|D[EA]l ?|SAN ?|DU ?)*[A-Z{0}][A-Z{0}]+(?:[---][A-Z{0}][A-Z{0}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[--. ]*[A-Z{0}])*\b\.?))(?<deg>(?: JR| DR| I+)?\b)\.?)))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);

                //sAuthorPatternWithFullCaps = string.Format(@"(?:(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.,?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)(?:\.,?|[^\.],)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b),)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:MC|O[’']|VAN DE[RN] ?|V[AO]N ?|DE ?|LA ?|DE[RS] ?|D[’']|DE ?|LE ?|DI ?|MAC ?|D[EA]l ?|SAN ?|DU ?)*[A-Z{0}][A-Z{0}]+(?:[\-‐][A-Z{0}][A-Z{0}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: JR| DR| I+)?\b)\.?)))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);

                //sAuthorPatternWithFullCaps = string.Format(@"(?:(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?,))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:MC|O[’']|VAN DE[RN] ?|V[AO]N ?|DE ?|LA ?|DE[RS] ?|D[’']|DE ?|LE ?|DI ?|MAC ?|D[EA]l ?|SAN ?|DU ?)*[A-Z{0}][A-Z{0}]+(?:[\-‐][A-Z{0}][A-Z{0}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: JR| DR| I+)?\b)\.?)))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);

                //sAuthorPatternWithFullCaps = string.Format(@"(?:(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?,))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:MC|O[’']|VAN DE[RN] ?|V[AO]N ?|DE ?|LA ?|DE[RS] ?|D[’']|DE ?|LE ?|DI ?|MAC ?|D[EA]l ?|SAN ?|DU ?)*[A-Z{0}][A-Z{0}]+(?:[\-‐][A-Z{0}][A-Z{0}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: JR| DR| I+)?\b)\.?)))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);


                //updated by Dakshinamoorthy on 2018-Sep-25
                //new author pattern included (Adkisson, Richard V.)
                //Sample ref (Adkisson, Richard V. “The Economy as an Open System: An Institutionalist Framework for Economic Development.” In Institutional Analysis and Praxis, edited by Tara Natarajan, Wolfram Elsner and Scott Fullwiler, pp. 25-38. New York, NY: Springer, 2009a.)

                //sAuthorPatternWithFullCaps = string.Format(@"(?:(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?,))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}][a-z{1}]+ [A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?), (?<gnm>[A-Z{0}][a-z{1}]+ [A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}][a-z{1}]+,)) (?<gnm>(?:[A-Z{0}][a-z{1}]+)(?: [A-Z{0}][a-z{1}]+)?\b))(?=(?:[\.,]+| and | & ))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<gnm>(?:[A-Z{0}][a-z{1}]+)) (?<snm>(?:[A-Z{0}][a-z{1}]+)\b))(?=(?:[\.,]+| and | & ))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:MC|O[’']|VAN DE[RN] ?|V[AO]N ?|DE ?|LA ?|DE[RS] ?|D[’']|DE ?|LE ?|DI ?|MAC ?|D[EA]l ?|SAN ?|DU ?)*[A-Z{0}][A-Z{0}]+(?:[\-‐][A-Z{0}][A-Z{0}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: JR| DR| I+)?\b)\.?)))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);

                //updated by Dakshinamoorthy on 2018-Nov-24
                //new author pattern included (Stahler-Scholk, Richard, )
                //new author pattern included (Genevieve Reday-Mulvey. )
                //Sample ref (Stahler-Scholk, Richard, Harry E. Vanden and Glen D. Kuecker. “Globalizing Resistance: The New Politics of Social Movements in Latin America.” Latin American Perspectives 34, 2 (2007): 5-16.)
                //Sample ref (Stahel, Walter R. and Genevieve Reday-Mulvey. Jobs for Tomorrow: The Potential for Substituting Manpower for Energy. New York, NY: Vantage Press, 1981.)

                string sNamePrefixLowerIniCaps = @"(?:M[a]?c|M[\u2019\u0027]|[OD][\u2019\u0027]|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|[Ll]a[\u0027] ?|[Dd]e[rs] ?|D[\u2019\u0027]|d[\u2019\u0027]|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|[Dd]u ?|St ?|[Dd]a ?|A[Ii][\u002D\u2010]|[Ee]l[\u002D\u2010]|[Yy]a ?|[Yy]e ?|[Rr]e ?|Al ?|[Ll]e ?|[Nn][\u2019\u0027] ?)";
                string sNamePrefixAllCaps = @"(?:M[A]?C|[OD][\u2019\u0027]|VAN DE[RN] ?|V[AO]N ?|DE ?|LA ?|DE[RS] ?|D[\u2019\u0027]|DE ?|LE ?|DI ?|MAC ?|D[EA]l ?|SAN ?|DU ?)";

                //sAuthorPatternWithFullCaps = string.Format(@"(?:(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z][a-z]+(?:[\-‐ ][A-Z][a-z]+)?)[, ]+(?<gnm>[A-Z](?:[\-‐. ]*[A-Z])*\b\.?(?: [A-Z][a-z]+)?))(?=(?:[\.,]+| and | & ))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?,))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}][a-z{1}]+ [A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV]an ?|[Dd]e ?|la ?|der ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\.? )?(?:Mc|O[’']|[vV]an [Dd]e[rn] ?|[vV][ao]n ?|[Dd]e ?|la ?|[Dd]e[rs] ?|D[’']|d[’']|De ?|Le ?|Di ?|Mac ?|D[ea]l ?|San ?|du ?)*[A-Z{0}][a-z{1}]+(?:[\-‐ ][A-Z{0}][a-z{1}]+)?), (?<gnm>[A-Z{0}][a-z{1}]+ [A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}][a-z{1}]+\-)?(?:[A-Z{0}][a-z{1}]+,)) (?<gnm>(?:[A-Z{0}][a-z{1}]+)(?: [A-Z{0}][a-z{1}]+)?\b))(?=(?:[\.,]+| and | & ))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<gnm>(?:[A-Z{0}][a-z{1}]+)) (?<snm>(?:[A-Z{0}][a-z{1}]+\-)?(?:[A-Z{0}][a-z{1}]+)\b))(?=(?:[\.,]+| and | & ))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:MC|O[’']|VAN DE[RN] ?|V[AO]N ?|DE ?|LA ?|DE[RS] ?|D[’']|DE ?|LE ?|DI ?|MAC ?|D[EA]l ?|SAN ?|DU ?)*[A-Z{0}][A-Z{0}]+(?:[\-‐][A-Z{0}][A-Z{0}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-‐. ]*[A-Z{0}])*\b\.?))(?<deg>(?: JR| DR| I+)?\b)\.?)))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified);


                //sAuthorPatternWithFullCaps = string.Format(@"(?:(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?(?: (?:[A-Z{0}][a-z{1}]+\-)?[A-Z{0}][a-z{1}]+)?))(?=(?:[\.,]+| and | & |[ ]*$|[ ]*<Etal>(?:(?!</?Etal>).)+</Etal>))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?,))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}][a-z{1}]+ [A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )?(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?(?![\u0027\u2019])))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )?(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?), (?<gnm>[A-Z{0}][a-z{1}]+ [A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}][a-z{1}]+\-)?(?:(?:{2})*[A-Z{0}][a-z{1}]+,)) (?<gnm>(?:[A-Z{0}][a-z{1}]+)(?:[ \-](?:{2})*[A-Z{0}][a-z{1}]+)?\b))(?=(?:[\.,]+| and | & |[ ]*$|[ ]*<Etal>(?:(?!</?Etal>).)+</Etal>))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<gnm>(?:[A-Z{0}][a-z{1}]+(?:\-[A-Z{0}][a-z{1}]+)?)) (?<snm>(?:[A-Z{0}][a-z{1}]+\-)?(?:(?:{2})*[A-Z{0}][a-z{1}]+)\b))(?=(?:[\.,]+| and | & |[ ]*$|[ ]*<Etal>(?:(?!</?Etal>).)+</Etal>))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:{3})*[A-Z{0}][A-Z{0}]+(?:[\-?][A-Z{0}][A-Z{0}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?))(?<deg>(?: JR| DR| I+)?\b)\.?)))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified, sNamePrefixLowerIniCaps, sNamePrefixAllCaps);


                //updated by Dakshinamoorthy on 2018-Jan-05
                //"Ph", "Ch" added in GNM

                //sAuthorPatternWithFullCaps = string.Format(@"(?:(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?(?: (?:[A-Z{0}][a-z{1}]+\-)?[A-Z{0}][a-z{1}]+)?))(?=(?:[\.,]+| and | & |[ ]*$|[ ]*<Etal>(?:(?!</?Etal>).)+</Etal>))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?,))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}][a-z{1}]+ [A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>(?:(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*)|(?:Ph|Ch)(?:\.?\-[A-Z{0}])?)\.? )(?:(?<snm>(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )?(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?(?![\u0027\u2019])))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )?(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?), (?<gnm>[A-Z{0}][a-z{1}]+ [A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}][a-z{1}]+\-)?(?:(?:{2})*[A-Z{0}][a-z{1}]+,)) (?<gnm>(?:[A-Z{0}][a-z{1}]+)(?:[ \-](?:{2})*[A-Z{0}][a-z{1}]+)?\b))(?=(?:[\.,]+| and | & |[ ]*$|[ ]*<Etal>(?:(?!</?Etal>).)+</Etal>))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<gnm>(?:[A-Z{0}][a-z{1}]+(?:\-[A-Z{0}][a-z{1}]+)?)) (?<snm>(?:[A-Z{0}][a-z{1}]+\-)?(?:(?:{2})*[A-Z{0}][a-z{1}]+)\b))(?=(?:[\.,]+| and | & |[ ]*$|[ ]*<Etal>(?:(?!</?Etal>).)+</Etal>))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:{3})*[A-Z{0}][A-Z{0}]+(?:[\-?][A-Z{0}][A-Z{0}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?))(?<deg>(?: JR| DR| I+)?\b)\.?)))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified, sNamePrefixLowerIniCaps, sNamePrefixAllCaps);

                //updated by Dakshinamoorthy on 2019-Sep-14
                //"given: "Le Minh" -> "Le Minh Giang"

                //sAuthorPatternWithFullCaps = string.Format(@"(?:(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?(?: (?:[A-Z{0}][a-z{1}]+\-)?[A-Z{0}][a-z{1}]+)?))(?=(?:[\.,]+| and | & |[ ]*$|[ ]*<Etal>(?:(?!</?Etal>).)+</Etal>))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?,))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}][a-z{1}]+ [A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>(?:(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*)|(?:Ph|Ch)(?:\.?\-[A-Z{0}])?)\.? )(?:(?<snm>(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ](?:{2})*[A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )?(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?(?![\u0027\u2019])))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )?(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?), (?<gnm>[A-Z{0}][a-z{1}]+ [A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}][a-z{1}]+\-)?(?:(?:{2})*[A-Z{0}][a-z{1}]+,)) (?<gnm>(?:[A-Z{0}][a-z{1}]+)(?:[ \-](?:{2})*[A-Z{0}][a-z{1}]+)?\b))(?=(?:[\.,]+| and | & |[ ]*$|[ ]*<Etal>(?:(?!</?Etal>).)+</Etal>))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<gnm>(?:{2})*(?:[A-Z{0}][a-z{1}]+(?:\-[A-Z{0}][a-z{1}]+)?)) (?<snm>(?:[A-Z{0}][a-z{1}]+\-)?(?:(?:{2})*[A-Z{0}][a-z{1}]+)\b))(?=(?:[\.,]+| and | & |[ ]*$|[ ]*<Etal>(?:(?!</?Etal>).)+</Etal>))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:{3})*[A-Z{0}][A-Z{0}]+(?:[\-?][A-Z{0}][A-Z{0}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?))(?<deg>(?: JR| DR| I+)?\b)\.?)))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified, sNamePrefixLowerIniCaps, sNamePrefixAllCaps);

                // updated by Dakshinamoorthy on 2020-Jan-11
                //"snm: "Abd El-Hack ME"
                sAuthorPatternWithFullCaps = string.Format(@"(?:(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:{2})*[A-Z{0}][a-z{1}]+(?:[\- ](?:{2})*[A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?(?: (?:[A-Z{0}][a-z{1}]+\-)?[A-Z{0}][a-z{1}]+)?))(?=(?:[\.,]+| and | & |[ ]*$|[ ]*<Etal>(?:(?!</?Etal>).)+</Etal>))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?,))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>[A-Z{0}][a-z{1}]+ [A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )(?:(?<snm>(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?<gnm>(?:(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*)|(?:Ph|Ch)(?:\.?\-[A-Z{0}])?)\.? )(?:(?<snm>(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ](?:{2})*[A-Z{0}][a-z{1}]+)?)(?<deg>(?: Jr| Dr)?\b)\.?)))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )?(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?(?![\u0027\u2019])))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\.? )?(?:{2})*[A-Z{0}][a-z{1}]+(?:[\-? ][A-Z{0}][a-z{1}]+)?), (?<gnm>[A-Z{0}][a-z{1}]+ [A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?))(?<deg>(?: Jr| Dr)?\b)\.?))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:[A-Z{0}][a-z{1}]+\-)?(?:(?:{2})*[A-Z{0}][a-z{1}]+,)) (?<gnm>(?:[A-Z{0}][a-z{1}]+)(?:[ \-](?:{2})*[A-Z{0}][a-z{1}]+)?\b))(?=(?:[\.,]+| and | & |[ ]*$|[ ]*<Etal>(?:(?!</?Etal>).)+</Etal>))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<gnm>(?:{2})*(?:[A-Z{0}][a-z{1}]+(?:\-[A-Z{0}][a-z{1}]+)?)) (?<snm>(?:[A-Z{0}][a-z{1}]+\-)?(?:(?:{2})*[A-Z{0}][a-z{1}]+)\b))(?=(?:[\.,]+| and | & |[ ]*$|[ ]*<Etal>(?:(?!</?Etal>).)+</Etal>))))|(?:(?<=(?:[>\:\.;,0-9]+ | (?:and|&|&#x0026;) )|\t|^[ ]?)(?:(?:(?<snm>(?:{3})*[A-Z{0}][A-Z{0}]+(?:[\-?][A-Z{0}][A-Z{0}]+)?)[, ]+(?<gnm>[A-Z{0}](?:[\-?. ]*[A-Z{0}])*\b\.?))(?<deg>(?: JR| DR| I+)?\b)\.?)))", sCapsNonEnglishChar, sSmallNonEnglishChar_Modified, sNamePrefixLowerIniCaps, sNamePrefixAllCaps);

                string sEditorBackProgressive = string.Format(@"(?:(?:(?:{0})[\:\.;, ]*(?:(?:[ ]?\b(?:and)|[ ]?&) )?)+)$", sAuthorPatternWithFullCaps);
                string sEditorFrontProgressive = string.Format(@"^(?:(?:(?:{0})[\:\.;, ]*(?:(?:[ ]?\b(?:and)|[ ]?&) )?)+)", sAuthorPatternWithFullCaps);

                if (bIsEditorBackSearch == true)
                {
                    sAUGContent = Regex.Replace(sAUGContent, sEditorBackProgressive, string.Format("<{0}>{1}</{0}>", "AuthorGroup", "$&"));
                    sAUGContent = Regex.Replace(sAUGContent, "(?:(?<=<AuthorGroup>)(?:(?!</?AuthorGroup>).)+(?=</AuthorGroup>))", ProcessAuthorProgressiveGroup);
                }
                else if (bIsEditorFrontSearch == true)
                {
                    sAUGContent = Regex.Replace(sAUGContent, sEditorFrontProgressive, string.Format("<{0}>{1}</{0}>", "AuthorGroup", "$&"));
                    sAUGContent = Regex.Replace(sAUGContent, "(?:(?<=<AuthorGroup>)(?:(?!</?AuthorGroup>).)+(?=</AuthorGroup>))", ProcessAuthorProgressiveGroup);
                }
                else
                {
                    sAUGContent = Regex.Replace(sAUGContent, sAuthorPatternWithFullCaps, ProcessAuthor);
                }

                sAUGContent = Regex.Replace(sAUGContent, "</?AuthorGroup>", "");


                //if (!Regex.IsMatch(sAUGContent, "<(?:Surname|Forename|Suffix|Author)>") && IsCollabSymptomsFound(sAUGContent))
                //{
                //    sAUGContent = string.Format("<{0}>{1}</{0}>", "Collab", sAUGContent);
                //}
            }
            catch (Exception ex)
            {

                //System.Windows.Forms.MessageBox.Show(ex.Message);
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ProcessAuthorGroup", ex.Message, true, "");
            }
            return sAUGContent;
        }

        private string ProcessAuthorProgressiveGroup(Match myAuMatch)
        {
            string sAUGContent = myAuMatch.Value.ToString();
            try
            {
                sAUGContent = Regex.Replace(sAUGContent, sAuthorPatternWithFullCaps, ProcessAuthor);

                //added Dakshiamoorthy on 2020-Jun-04
                sAUGContent = string.Format("<AuEdGroup>{0}</AuEdGroup>", sAUGContent);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ProcessAuthorProgressiveGroup", ex.Message, true, "");
            }
            return sAUGContent;
        }

        private string ProcessAuthor(Match myAuMatch)
        {
            string sAUContent = myAuMatch.Value.ToString();
            StringBuilder sbAuthorGroup = new StringBuilder();
            List<Tuple<string, int>> lstAuthorInfo = new List<Tuple<string, int>>();

            try
            {
                if (myAuMatch.Groups.Count == 4)
                {
                    string sAuSurname = myAuMatch.Groups["snm"].Value;
                    int nAuSurnameIndex = myAuMatch.Groups["snm"].Index;

                    string sAuGivenName = myAuMatch.Groups["gnm"].Value;
                    int nAuGivenNameIndex = myAuMatch.Groups["gnm"].Index;

                    string sAuSuffix = myAuMatch.Groups["deg"].Value;
                    int nAuSuffixIndex = myAuMatch.Groups["deg"].Index;

                    if (!string.IsNullOrEmpty(sAuSurname))
                    {
                        lstAuthorInfo.Add(new Tuple<string, int>(string.Format("<{0}>{1}</{0}> ", "Surname", sAuSurname), nAuSurnameIndex));
                    }

                    if (!string.IsNullOrEmpty(sAuGivenName))
                    {
                        lstAuthorInfo.Add(new Tuple<string, int>(string.Format("<{0}>{1}</{0}> ", "Forename", sAuGivenName), nAuGivenNameIndex));
                    }

                    if (!string.IsNullOrEmpty(sAuSuffix))
                    {
                        lstAuthorInfo.Add(new Tuple<string, int>(string.Format("<{0}>{1}</{0}> ", "Suffix", sAuSuffix), nAuSuffixIndex));
                    }


                    //if (!string.IsNullOrEmpty(sAuSurname))
                    //{
                    //    sbAuthorGroup.Append(string.Format("<{0}>{1}</{0}> ", "Surname", sAuSurname));
                    //}

                    //if (!string.IsNullOrEmpty(sAuGivenName))
                    //{
                    //    sbAuthorGroup.Append(string.Format("<{0}>{1}</{0}> ", "Forename", sAuGivenName));
                    //}

                    //if (!string.IsNullOrEmpty(sAuSuffix))
                    //{
                    //    sbAuthorGroup.Append(string.Format("<{0}>{1}</{0}> ", "Suffix", sAuSuffix));
                    //}

                    //if (string.IsNullOrEmpty(sbAuthorGroup.ToString()))
                    //{
                    //    return sAUContent;
                    //}

                    if (lstAuthorInfo.Count <= 0)
                    {
                        return sAUContent;
                    }

                    lstAuthorInfo = lstAuthorInfo.OrderBy(i => i.Item2).ToList();

                    foreach (var eachAuEle in lstAuthorInfo)
                    {
                        sbAuthorGroup.Append(eachAuEle.Item1);
                    }

                    //include author group
                    sAUContent = sbAuthorGroup.ToString();
                    sAUContent = string.Format("<{0}>{1}</{0}>", "Author", sAUContent);
                }
                else
                {
                    return sAUContent;
                }
            }
            catch (Exception ex)
            {

                //System.Windows.Forms.MessageBox.Show(ex.Message);
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ProcessAuthor", ex.Message, true, "");
            }

            return sAUContent;
        }

        private bool IsCollabSymptomsFound(string sRefContent)
        {
            bool bReturnValue = false;
            string sOrgPattern = @"\b(?:Centre|Agency|Communities|Institute|National|Research|Council|Standard)\b";
            string sGrammerCheck = @"\b(?:of|the|an|the|in|to|for|on)\b";

            try
            {
                if (Regex.IsMatch(sRefContent, @"^(?:(?:[A-Z][a-z]+ ){2,}(?:[A-Z][a-z]+)[\:\.;, ]*)$")) //National Research Council. 
                {
                    bReturnValue = true;
                }
                else if (Regex.IsMatch(sRefContent, @"^(?:(?:(?:[A-Z]{3,}))(?: \([^\(\)]+\))?[\:\.,; ]*)$")) //NIH., IOM (Institute of Medicine), 
                {
                    bReturnValue = true;
                }
                else if (Regex.IsMatch(sRefContent, sOrgPattern)) //European Communities, 
                {
                    bReturnValue = true;
                }
                else if (Regex.IsMatch(sRefContent, sGrammerCheck)) //Association of Official Analytical Chemists.  
                {
                    bReturnValue = true;
                }
                else if (Regex.IsMatch(sRefContent, @"^(?:[ ]?[A-Z]{3,}(?:-[A-Z]{3,})?[\.]?)$"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IsCollabSymptomsFound", ex.Message, true, "");
            }
            return bReturnValue;
        }

        private string IdentifyRefType(string sRefTaggedContent)
        {
            //ldlRefTypeJournal,
            //ldlRefTypeBook,
            //ldlRefTypeWeb,
            //ldlRefTypePaper,
            //ldlRefTypeConference,
            //ldlRefTypePatent,
            //ldlRefTypeReport,
            //ldlRefTypeThesis,
            //ldlRefTypeOther

            string sRefType = "ldlRefTypeOther";
            try
            {
                bool isAuthorGroupFound = false;
                bool isEditorGroupFound = false;
                bool isCollabFound = false;

                bool isPubYearFound = false;
                bool isArticleTitleFound = false;
                bool isJournalTitleFound = false;
                bool isSourceTitleFound = false;
                bool isChapterTitleFound = false;
                bool isBookTitleFound = false;
                bool isConferenceNameFound = false;

                bool isVolumeNumFound = false;
                bool isIssueNumFound = false;
                bool isPageNumFound = false;
                bool isEditionNumFound = false;

                bool isDOINumFound = false;
                bool isPubMedIDFound = false;

                bool isPublisherNameFound = false;
                bool isPublisherLocationFound = false;

                bool isURLFound = false;
                //added by Dakshinamoorthy on 03-Jan-2020
                bool isURL_LabelFound = false;
                bool isAccessedDateFound = false;
                bool isThesisKeywordFound = false;
                bool isReportKeywordFound = false;
                bool isPaperKeywordFound = false;


                isAuthorGroupFound = CheckAuthorGroupFound(sRefTaggedContent);
                isEditorGroupFound = CheckEditorGroupFound(sRefTaggedContent);
                isCollabFound = CheckCollabFound(sRefTaggedContent);

                isPubYearFound = CheckPubYearFound(sRefTaggedContent);
                isArticleTitleFound = CheckArticleTitleFound(sRefTaggedContent);
                isJournalTitleFound = CheckJournalTitleFound(sRefTaggedContent);
                isSourceTitleFound = CheckSourceTitleFound(sRefTaggedContent);
                isChapterTitleFound = CheckChapterTitleFound(sRefTaggedContent);
                isBookTitleFound = CheckBookTitleFound(sRefTaggedContent);

                isConferenceNameFound = CheckConferenceNameFound(sRefTaggedContent);

                isVolumeNumFound = CheckVolumeNumberFound(sRefTaggedContent);
                isIssueNumFound = CheckIssueNumberFound(sRefTaggedContent);
                isPageNumFound = CheckPageNumberFound(sRefTaggedContent);
                isEditionNumFound = CheckEditionNumberFound(sRefTaggedContent);

                isDOINumFound = CheckDOINumberFound(sRefTaggedContent);
                isPubMedIDFound = CheckPubMedIdNumberFound(sRefTaggedContent);

                isPublisherNameFound = CheckPublisherNameFound(sRefTaggedContent);
                isPublisherLocationFound = CheckPublisherLocationFound(sRefTaggedContent);
                isURLFound = CheckURLFound(sRefTaggedContent);
                isURL_LabelFound = CheckURL_LabelFound(sRefTaggedContent);
                isAccessedDateFound = CheckAccessedDateFound(sRefTaggedContent);

                isThesisKeywordFound = CheckThesisKeywordFound(sRefTaggedContent);
                isReportKeywordFound = CheckReportKeywordFound(sRefTaggedContent);
                isPaperKeywordFound = CheckPaperKeywordFound(sRefTaggedContent);

                if (isThesisKeywordFound)
                {
                    sRefType = "Thesis";
                }
                else if (isReportKeywordFound)
                {
                    sRefType = "Report";
                }
                else if (isPaperKeywordFound)
                {
                    sRefType = "Paper";
                }
                else if (isConferenceNameFound)
                {
                    sRefType = "Conference";
                }
                else if (((isChapterTitleFound || isBookTitleFound) || (isArticleTitleFound || isJournalTitleFound)) && (isPublisherLocationFound || isPublisherNameFound) && !isThesisKeywordFound && !isIssueNumFound)
                {
                    sRefType = "Book";
                }
                else if (isPublisherLocationFound && isPublisherNameFound && isEditorGroupFound && (isAuthorGroupFound || isCollabFound))
                {
                    sRefType = "Book";
                }
                else if (((isChapterTitleFound || isArticleTitleFound) || (isBookTitleFound || isJournalTitleFound)) && ((isPageNumFound && isIssueNumFound && isVolumeNumFound) || (isArticleTitleFound && isJournalTitleFound && isDOINumFound) || (isIssueNumFound && isVolumeNumFound) || (isVolumeNumFound && isPageNumFound)))
                {
                    sRefType = "Journal";
                }
                else if (((isChapterTitleFound || isBookTitleFound) || (isArticleTitleFound || isJournalTitleFound)) && isURLFound && (isAccessedDateFound || isURL_LabelFound))
                {
                    sRefType = "Web";
                }

                else if (isArticleTitleFound && isJournalTitleFound && (isPageNumFound || isIssueNumFound || isVolumeNumFound))
                {
                    sRefType = "Journal";
                }
                else
                {
                    sRefType = "Other";
                }

            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleJournalArticleTitle", ex.Message, true, "");
            }
            return sRefType;
        }


        private string NormalizeElementsBasedOnRefType(string sRefType, string sRefTaggedContent)
        {
            try
            {
                switch (sRefType)
                {
                    case "Journal":
                        if (Regex.IsMatch(sRefTaggedContent, "</?(?:ldlChapterTitle|ldlBookTitle)>"))
                        {
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlChapterTitle>", "<ldlArticleTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlChapterTitle>", "</ldlArticleTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlBookTitle>", "<ldlJournalTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlBookTitle>", "</ldlJournalTitle>");
                        }
                        break;

                    case "Thesis":
                        if (Regex.IsMatch(sRefTaggedContent, "</?(?:ldlChapterTitle|ldlBookTitle)>"))
                        {
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlChapterTitle>", "<ldlArticleTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlChapterTitle>", "</ldlArticleTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlBookTitle>", "<ldlJournalTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlBookTitle>", "</ldlJournalTitle>");
                        }

                        if (Regex.IsMatch(sRefTaggedContent, "</?(?:ldlPublisherName)>"))
                        {
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlPublisherName>", "<ldlInstitutionName>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlPublisherName>", "</ldlInstitutionName>");
                        }

                        break;

                    case "Report":
                        if (Regex.IsMatch(sRefTaggedContent, "</?(?:ldlChapterTitle|ldlBookTitle)>"))
                        {
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlChapterTitle>", "<ldlArticleTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlChapterTitle>", "</ldlArticleTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlBookTitle>", "<ldlJournalTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlBookTitle>", "</ldlJournalTitle>");
                        }

                        if (Regex.IsMatch(sRefTaggedContent, "</?(?:ldlPublisherName)>"))
                        {
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlPublisherName>", "<ldlInstitutionName>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlPublisherName>", "</ldlInstitutionName>");
                        }

                        break;


                    case "Book":
                        if (Regex.IsMatch(sRefTaggedContent, "</?(?:ldlArticleTitle|ldlJournalTitle)>"))
                        {
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlArticleTitle>", "<ldlChapterTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlArticleTitle>", "</ldlChapterTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlJournalTitle>", "<ldlBookTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlJournalTitle>", "</ldlBookTitle>");
                        }
                        break;

                    case "Web":
                        //added by Dakshinamoorthy on 03-Jan-2020

                        if (Regex.IsMatch(sRefTaggedContent, "</?ldlChapterTitle>") && Regex.IsMatch(sRefTaggedContent, "</?ldlBookTitle>"))
                        {
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlChapterTitle>", "<ldlArticleTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlChapterTitle>", "</ldlArticleTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlBookTitle>", "<ldlSourceTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlBookTitle>", "</ldlSourceTitle>");
                        }
                        else if (Regex.IsMatch(sRefTaggedContent, "</?ldlBookTitle>") && Regex.IsMatch(sRefTaggedContent, "</?ldlChapterTitle>") == false)
                        {
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlBookTitle>", "<ldlArticleTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlBookTitle>", "</ldlArticleTitle>");
                        }
                        else if (Regex.IsMatch(sRefTaggedContent, "</?ldlBookTitle>") == false && Regex.IsMatch(sRefTaggedContent, "</?ldlChapterTitle>"))
                        {
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlChapterTitle>", "<ldlArticleTitle>");
                            sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlChapterTitle>", "</ldlArticleTitle>");
                        }

                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlJournalTitle>", "<ldlSourceTitle>");
                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlJournalTitle>", "</ldlSourceTitle>");
                        break;

                    case "Conference":
                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlChapterTitle>", "<ldlArticleTitle>");
                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlChapterTitle>", "</ldlArticleTitle>");

                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlBookTitle>", "<ldlSourceTitle>");
                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlBookTitle>", "</ldlSourceTitle>");

                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "<ldlPublisherLocation>", "<ldlConferenceLocation>");
                        sRefTaggedContent = Regex.Replace(sRefTaggedContent, "</ldlPublisherLocation>", "</ldlConferenceLocation>");
                        break;

                    default:
                        sRefTaggedContent = sRefTaggedContent;
                        break;
                }

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\NormalizeTitlesBasedOnRefType", ex.Message, true, "");
            }
            return sRefTaggedContent;
        }

        private bool CheckThesisKeywordFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlThesisKeyword)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckThesisKeywordFound", ex.Message, true, "");
            }
            return false;
        }

        //added by Dakshinamoorthy on 2020-Jul-08
        private bool CheckPaperKeywordFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlPaperKeyword)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckPaperKeywordFound", ex.Message, true, "");
            }
            return false;
        }

        private bool CheckReportKeywordFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlReportKeyword|ldlReportNumber)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckThesisKeywordFound", ex.Message, true, "");
            }
            return false;
        }

        private bool CheckAccessedDateFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlAccessedDate)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckAccessedDateFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckURLFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlURL)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckURLFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckURL_LabelFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlURLLabel)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckURL_LabelFound", ex.Message, true, "");
            }
            return false;
        }

        private bool CheckPublisherNameFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlPublisherName)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckAuthorGroupFound", ex.Message, true, "");
            }
            return false;
        }

        private bool CheckPublisherLocationFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlPublisherLocation)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckAuthorGroupFound", ex.Message, true, "");
            }
            return false;
        }


        private bool CheckAuthorGroupFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlAuthorSurName|ldlAuthorGivenName|ldlAuthorSuffix)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckAuthorGroupFound", ex.Message, true, "");
            }
            return false;
        }

        private bool CheckConferenceNameFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlConferenceName)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckAuthorGroupFound", ex.Message, true, "");
            }
            return false;
        }

        private bool CheckEditorGroupFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlEditorSurName|ldlEditorGivenName|ldlEditorSuffix)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckEditorGroupFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckCollabFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlCollab)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckCollabFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckPubYearFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlPublicationYear)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckPubYearFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckArticleTitleFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlArticleTitle)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
                else if (Regex.IsMatch(sRefTaggedContent, @"<(Article_Title)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckArticleTitleFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckJournalTitleFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlJournalTitle)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
                else if (Regex.IsMatch(sRefTaggedContent, @"<(Journal_Title)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckJournalTitleFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckChapterTitleFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlChapterTitle)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckChapterTitleFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckBookTitleFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlBookTitle)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckBookTitleFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckSourceTitleFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlSourceTitle)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckSourceTitleFound", ex.Message, true, "");
            }
            return false;
        }

        private bool CheckVolumeNumberFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlVolumeNumber)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
                else if (Regex.IsMatch(sRefTaggedContent, @"<(Vol_No)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckVolumeNumberFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckIssueNumberFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlIssueNumber)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
                else if (Regex.IsMatch(sRefTaggedContent, @"<(Issue_No)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckIssueNumberFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckEditionNumberFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlEditionNumber)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckEditionNumberFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckPageNumberFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlFirstPageNumber|ldlLastPageNumber|ldlPageRange)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
                else if (Regex.IsMatch(sRefTaggedContent, @"<(PageRange)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckPageNumberFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckDOINumberFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlDOINumber)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckDOINumberFound", ex.Message, true, "");
            }
            return false;
        }
        private bool CheckPubMedIdNumberFound(string sRefTaggedContent)
        {
            try
            {
                if (Regex.IsMatch(sRefTaggedContent, @"<(ldlPubMedIdNumber)>(?:(?!</?\1>).)+</\1>"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\CheckPubMedIdNumberFound", ex.Message, true, "");
            }
            return false;
        }

        ///// <summary>
        ///// This function identify the boundary for Author, Editor and Collab group.
        ///// </summary>
        ///// <param name="sRefContent">Sentence splitted reference content</param>
        ///// <returns>Boundary identified content</returns>
        //private string SetFirstAuEdCollabBoundary(string sRefContent)
        //{
        //    try
        //    {
        //        string sErrMsg = string.Empty;
        //        string sRefContent4NLP = sRefContent;

        //        StanfordEnglishNLP objNLP = new StanfordEnglishNLP();

        //        sRefContent4NLP = objNLP.DoSentenceSplitting(sRefContent4NLP, ref sErrMsg);
        //        sRefContent4NLP = NormalizePunctuation4SbrTag(sRefContent4NLP);

        //        sRefContent4NLP = Regex.Replace(sRefContent4NLP, "(?:</?(?:b|i|u|sc|sub|sup)>)", "");

        //        sRefContent = MatchNLP_SentenceResult(sRefContent, sRefContent4NLP);



        //        sRefContent = Regex.Replace(sRefContent, "^(?:(?:(?!<sbr/>).)+)", IdentifyFirstAuEdCollabBoundary);
        //        sRefContent = Regex.Replace(sRefContent, "^(?:(?:(?!<sbr/>).)+)", IdentifyFirstAuEdCollabGroup);

        //        sRefContent = SetFirstAuEdCollabBoundary_PostCleanup(sRefContent);
        //        sRefContent = Regex.Replace(sRefContent, "<sbr/>", " ");

        //        //System.Windows.Forms.MessageBox.Show(sRefContent);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    return sRefContent;
        //}


        private string MatchNLP_SentenceResult(string sOriginalRefContent, string sNLP_RefContent)
        {
            string sMatchedRefContent = sNLP_RefContent;
            try
            {

                sNLP_RefContent = SetFirstAuEdCollabBoundary_PostCleanup(sNLP_RefContent);

                sNLP_RefContent = Regex.Replace(sNLP_RefContent, " ,", ",");
                sOriginalRefContent = Regex.Replace(sOriginalRefContent, " ,", ",");

                sNLP_RefContent = Regex.Replace(sNLP_RefContent, "[\u2002]", " ");
                sOriginalRefContent = Regex.Replace(sOriginalRefContent, "[\u2002]", " ");

                List<string> lstSentences = new List<string>();
                List<string> lstWords = new List<string>();
                int nSentenceCount = 0;
                int nSerNo = 0;
                string sOriginalContent_Temp = string.Empty;
                string sNLPContent_Temp = string.Empty;

                DataTable dtRefOriginal = new DataTable();
                dtRefOriginal.Columns.Add("SerNo", typeof(int));
                dtRefOriginal.Columns.Add("RefContent", typeof(string));
                dtRefOriginal.Columns.Add("SentenceNo", typeof(int));

                lstSentences = Regex.Split(sNLP_RefContent, "(?:<sbr/>)").ToList();

                lstWords = new List<string>();
                lstWords = sOriginalRefContent.Split(' ').ToList();

                foreach (string eachOriWord in lstWords)
                {
                    nSerNo += 1;
                    dtRefOriginal.Rows.Add(nSerNo, eachOriWord, 0);
                }

                lstWords = new List<string>();
                int nWordCount = 0;

                foreach (string sEachSentence in lstSentences)
                {
                    nSentenceCount += 1;
                    lstWords = sEachSentence.Split(' ').ToList();

                    foreach (string eachWord in lstWords)
                    {
                        sOriginalContent_Temp = dtRefOriginal.Rows[nWordCount]["RefContent"].ToString();
                        sOriginalContent_Temp = NLPOutputCleanup(sOriginalContent_Temp);
                        sOriginalContent_Temp = Regex.Replace(sOriginalContent_Temp, "(?:</?(?:b|i|u|sc|sub|sup)>)", "");

                        sNLPContent_Temp = eachWord;
                        sNLPContent_Temp = NLPOutputCleanup(eachWord);
                        sNLPContent_Temp = Regex.Replace(sNLPContent_Temp, "(?:</?(?:b|i|u|sc|sub|sup)>)", "");

                        if (sOriginalContent_Temp.ToLower().Trim().Equals(sNLPContent_Temp.ToLower().Trim()))
                        {
                            dtRefOriginal.Rows[nWordCount]["SentenceNo"] = nSentenceCount;
                        }
                        else
                        {
                            return sNLP_RefContent;
                        }

                        nWordCount += 1;
                    }
                }

                StringBuilder sbTemp = new StringBuilder();
                string sSenNo = string.Empty;
                string sTempContent = string.Empty;

                for (int j = 0; j <= dtRefOriginal.Rows.Count - 1; j++)
                {
                    sSenNo = dtRefOriginal.Rows[j]["SentenceNo"].ToString();
                    sTempContent = dtRefOriginal.Rows[j]["RefContent"].ToString();
                    sbTemp.Append(string.Format("<SEN{0}>{1} </SEN{0}>", sSenNo, sTempContent));
                }

                sMatchedRefContent = sbTemp.ToString();
                sMatchedRefContent = Regex.Replace(sMatchedRefContent, @"</(SEN[0-9]+)><\1>", "");
                sMatchedRefContent = Regex.Replace(sMatchedRefContent, @"</(SEN[0-9]+)><(SEN[0-9]+)>", "<sbr/>");
                sMatchedRefContent = Regex.Replace(sMatchedRefContent, @"</?(SEN[0-9]+)>", "");
                sMatchedRefContent = Regex.Replace(sMatchedRefContent, @"[ ]+<sbr/>", "<sbr/>");

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\MatchNLP_SentenceResult", ex.Message, true, "");
            }

            return sMatchedRefContent;
        }

        private string NLPOutputCleanup(string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "[\u201c\u201d\u2018\u2019]", "");
                sRefContent = Regex.Replace(sRefContent, "[\"\']", "");
                sRefContent = Regex.Replace(sRefContent, "[`]", "");
                sRefContent = Regex.Replace(sRefContent, @"\.", "");
                sRefContent = Regex.Replace(sRefContent, @"\-", "");
                sRefContent = Regex.Replace(sRefContent, @"[\u2014\u2013]", "");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\NLPOutputCleanup", ex.Message, true, "");
            }
            return sRefContent;
        }


        private string IdentifyFirstAuEdCollabGroup(Match sRefFirstSentence)
        {
            string sFirstAuEdCollabGroup = sRefFirstSentence.Value.ToString();

            sFirstAuEdCollabGroup = Regex.Replace(sFirstAuEdCollabGroup, "</?(?:b|i|u|sc|sub|sup)>", "");

            try
            {

                int nPatternId = 0;
                string sCollabPattern = string.Empty;

                //Check et-al and eds
                if (Regex.IsMatch(sFirstAuEdCollabGroup, "(?:<Etal>(?:(?!</?Etal>).)+</Etal>)"))
                {
                    sFirstAuEdCollabGroup = string.Format("<{0}>{1}</{0}>", "ldlAuthorEditorGroup", sFirstAuEdCollabGroup);
                    goto LBL_SKIP_PTN;
                }


                //collab identification
                nPatternId = 1;
                sCollabPattern = @"^(?:(?:[A-Z]+)[\:.;,]*)$";
                if (Regex.IsMatch(sFirstAuEdCollabGroup, sCollabPattern))
                {
                    sFirstAuEdCollabGroup = Regex.Replace(sFirstAuEdCollabGroup, sCollabPattern, string.Format("<{0}>{1}</{0}>", "ldlCollab", "$&"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 2;
                //Uretek (2014). “Deep Injection.” Methods, <http://www.uretekworldwide.com/solutions/methods/geopolymer-injection>.
                sCollabPattern = @"^(?:(?:[A-Z][a-z]+)[\:.;,]*)$";
                if (Regex.IsMatch(sFirstAuEdCollabGroup, sCollabPattern))
                {
                    sFirstAuEdCollabGroup = Regex.Replace(sFirstAuEdCollabGroup, sCollabPattern, string.Format("<{0}>{1}</{0}>", "ldlCollab", "$&"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 2;
                //ASTM-C109/C109M
                sCollabPattern = @"^(?:(?:[^ ]+)[\:.;,]*)$";
                if (Regex.IsMatch(sFirstAuEdCollabGroup, sCollabPattern))
                {
                    sFirstAuEdCollabGroup = Regex.Replace(sFirstAuEdCollabGroup, sCollabPattern, string.Format("<{0}>{1}</{0}>", "ldlCollab", "$&"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 3;
                //ASTM C1679. (2014). Standard Practice for Measuring Hydration Kinetics of Hydraulic Cementitious Mixtures Using Isothermal Calorimetry. ASTM International, West Conshohocken, PA, 15 pp.
                sCollabPattern = @"^(?:(?:[A-Z]+ [A-Z][0-9]+)[\:.;,]*)$";
                if (Regex.IsMatch(sFirstAuEdCollabGroup, sCollabPattern))
                {
                    sFirstAuEdCollabGroup = Regex.Replace(sFirstAuEdCollabGroup, sCollabPattern, string.Format("<{0}>{1}</{0}>", "ldlCollab", "$&"));
                    goto LBL_SKIP_PTN;
                }

                //Check with Local Database
                if (IsValidPublisherName(sFirstAuEdCollabGroup))
                {
                    sFirstAuEdCollabGroup = string.Format("<{0}>{1}</{0}>", "ldlCollab", sFirstAuEdCollabGroup);
                    goto LBL_SKIP_PTN;
                }

            ////Check with NLP 
            //if (IsValidCollabNameFromNLP(sFirstAuEdCollabGroup))
            //{
            //    sFirstAuEdCollabGroup = string.Format("<{0}>{1}</{0}>", "ldlCollab", sFirstAuEdCollabGroup);
            //    goto LBL_SKIP_PTN;
            //}

            LBL_SKIP_PTN:
                if (!Regex.IsMatch(sFirstAuEdCollabGroup, "(?:<ldlCollab>(?:(?!</?ldlCollab>).)+</ldlCollab>)") && !Regex.IsMatch(sFirstAuEdCollabGroup, "(?:<ldlAuthorEditorGroup>(?:(?!</?ldlAuthorEditorGroup>).)+</ldlAuthorEditorGroup>)"))
                {
                    sFirstAuEdCollabGroup = string.Format("<{0}>{1}</{0}>", "ldlAuthorEditorGroup", sFirstAuEdCollabGroup);
                }

                sFirstAuEdCollabGroup = string.Format("<{0}>{1}</{0}>", "ldlFirstAuEdCollabGroup", sFirstAuEdCollabGroup);

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyFirstAuEdCollabGroup", ex.Message, true, "");
            }
            return sFirstAuEdCollabGroup;
        }

        //private bool IsAuthorGroupSymptoms(string sRefContent)
        //{
        //    try
        //    {
        //        StanfordEnglishNLP objNLP = new StanfordEnglishNLP();
        //        string sErrMsg = string.Empty;
        //        string sNERTaggedContent = objNLP.DoNER_Tagging(sRefContent, ref sErrMsg);

        //        if (Regex.IsMatch(sNERTaggedContent, "<PERSON>"))
        //        {
        //            return true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return false;
        //}

        //private bool IsValidCollabNameFromNLP(string sRefContent)
        //{
        //    try
        //    {
        //        StanfordEnglishNLP objNLP = new StanfordEnglishNLP();
        //        string sErrMsg = string.Empty;

        //        sRefContent = SetFirstAuEdCollabBoundary_PostCleanup(sRefContent);
        //        sRefContent = Regex.Replace(sRefContent, "<sbr/>", " ");

        //        string sNERTaggedContent = objNLP.DoNER_Tagging(sRefContent, ref sErrMsg);

        //        //Cleanup
        //        sNERTaggedContent = Regex.Replace(sNERTaggedContent, "</ORGANIZATION>, <ORGANIZATION>", ", ");
        //        sNERTaggedContent = Regex.Replace(sNERTaggedContent, "</ORGANIZATION>, <ORGANIZATION>", ", ");

        //        sNERTaggedContent = Regex.Replace(sNERTaggedContent, @"^(?:<ORGANIZATION>(?:(?!</?ORGANIZATION>).)+</ORGANIZATION> [0-9]+[\:\.;,]*)$", NormalizeOrganizationTag);
        //        sNERTaggedContent = Regex.Replace(sNERTaggedContent, @"^(?:<ORGANIZATION>(?:(?!</?ORGANIZATION>).)+</ORGANIZATION> \(<ORGANIZATION>(?:(?!</?ORGANIZATION>|[\)]).)+</ORGANIZATION>[^\(\)]+\)[\:\.;,]*)$", NormalizeOrganizationTag);
        //        sNERTaggedContent = Regex.Replace(sNERTaggedContent, @"^(?:<ORGANIZATION>(?:(?!</?ORGANIZATION>).)+</ORGANIZATION> \([A-Z]+\))$", NormalizeOrganizationTag);
        //        sNERTaggedContent = Regex.Replace(sNERTaggedContent, @"^(?:<ORGANIZATION>(?:(?!</?ORGANIZATION>).)+</ORGANIZATION> \(<ORGANIZATION>(?:(?!</?ORGANIZATION>).)+</ORGANIZATION>\))$", NormalizeOrganizationTag);

        //        if (Regex.IsMatch(sNERTaggedContent, @"^(?:<ORGANIZATION>(?:(?!</?ORGANIZATION>).)+</ORGANIZATION>[\:\.;,]*)$"))
        //        {
        //            NpgsqlCommand cmd = new NpgsqlCommand();
        //            string sQuery = string.Format("select count(*) from data_dict_publisher_learn");
        //            Database.GetInstance.ReadFromDatabase(cmd, sQuery);
        //            DataTable dtResult = General.GeneralInstance.dataTable;
        //            int RowCount = Convert.ToInt32(dtResult.Rows[0][0].ToString());

        //            string sIndexContent = Regex.Replace(sNERTaggedContent, "</?ORGANIZATION>", "");
        //            string sActualPublisherCollab = sIndexContent;

        //            sIndexContent = General.DatabaseIndexCleanup(sIndexContent).ToLower();
        //            sActualPublisherCollab = Regex.Replace(sActualPublisherCollab, @"[\:.;,]+$", "");

        //            string sDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //            sQuery = string.Format("insert into data_dict_publisher_learn(id, index1, data1, source, created_by, created_date)  values({0}, '{1}', '{2}', 'Standford NLP (Ver. 2018-02-27)', '{3}', '{4}')", RowCount + 1,
        //                            sIndexContent, sActualPublisherCollab, "eBot-Automatic", sDateTime);
        //            Database.GetInstance.ExecuteNonQueryFunction(sQuery);

        //            return true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return false;
        //}

        //private string Cleanup4QueryContent(string sContent)
        //{
        //    try
        //    {
        //        sContent = Regex.Replace(sContent, "[']", "''");
        //    }
        //    catch (Exception ex)
        //    {
        //        AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\Cleanup4QueryContent", ex.Message, true, "");
        //    }
        //    return sContent;
        //}

        //private string NormalizeOrganizationTag(Match myOrgMatch)
        //{
        //    string sOutputContent = myOrgMatch.Value.ToString();
        //    try
        //    {
        //        sOutputContent = Regex.Replace(sOutputContent, "</?ORGANIZATION>", "");
        //        sOutputContent = string.Format("<{0}>{1}</{0}>", "ORGANIZATION", sOutputContent);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return sOutputContent;
        //}



        private string IdentifyFirstAuEdCollabBoundary(Match sRefFirstSentence)
        {
            string sBoundaryIndentifiedContent = sRefFirstSentence.Value.ToString();
            //updated by Dakshinamoorthy on 2019-Jan-05
            //sBoundaryIndentifiedContent = Regex.Replace(sBoundaryIndentifiedContent, "</?(?:b|i|u|sc|sup|sub)>", "");
            sBoundaryIndentifiedContent = Regex.Replace(sBoundaryIndentifiedContent, "</?(?:b|u|sc|sup|sub)>", "");


            try
            {
                //identify unique elements
                IdentifyEtAl(ref sBoundaryIndentifiedContent);
                IdentifyDateInfo(ref sBoundaryIndentifiedContent);
                IdentifyURL(ref sBoundaryIndentifiedContent);
                IdentifyEds4AUG(ref sBoundaryIndentifiedContent);

                //identify title like content
                IdentifyTitleLikeContent(ref sBoundaryIndentifiedContent);
                RegenerateSentences(ref sBoundaryIndentifiedContent);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyFirstAuEdCollabBoundary", ex.Message, true, "");
            }
            return sBoundaryIndentifiedContent;
        }




        private bool IdentifyTitleLikeContent(ref string sRefContent)
        {
            try
            {
                string sTitleLikePattern = @"(?:(?:[\u201c][^\u201c\u201d]+[\u201d])|(?:[\u2018][^\u2018\u2019]+[\u2019])|(?:<i>(?:(?!</?i>).)+</i>))";
                sRefContent = Regex.Replace(sRefContent, sTitleLikePattern, string.Format("<{0}>{1}</{0}>", "ldlTempTitle", "$&"));
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\IdentifyTitleLikeContent", ex.Message, true, "");
            }
            return true;
        }

        private string ValidateSentenceEnd(string sRefContent)
        {
            try
            {

                sRefContent = Regex.Replace(sRefContent, "^((?:(?!<sbr/>).)+)", HandleValidateSentenceEnd);
                //sRefContent = Regex.Replace(sRefContent, "^((?:(?!<sbr/>).)+)", HandleValidateSentenceEnd_DotEscaped);
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ValidateSentenceEnd", ex.Message, true, "");
            }
            return sRefContent;
        }

        private string HandleValidateSentenceEnd(Match MyFirstSentence)
        {
            string sOutputContent = MyFirstSentence.Value.ToString();
            try
            {
                MatchCollection mcDotSpace = Regex.Matches(sOutputContent, @"(?:(?:\.) )");
                if (mcDotSpace != null && mcDotSpace.Count > 0)
                {
                    Match myDotMatch = mcDotSpace[mcDotSpace.Count - 1];
                    string sFirstPart = sOutputContent.Substring(0, myDotMatch.Index + myDotMatch.Length);
                    string sSecondPart = sOutputContent.Substring(myDotMatch.Index + myDotMatch.Length, ((sOutputContent.Length) - (myDotMatch.Index + myDotMatch.Length)));

                    //U.S. Census Bureau.
                    if (!Regex.IsMatch(sFirstPart, "[a-z]"))
                    {
                        return sOutputContent;
                    }

                    if (!Regex.IsMatch(sSecondPart, "^(?:\b(?:[a-z]+)|(?:and|&))"))
                    {
                        sOutputContent = string.Format("{0}<sbr/>{1}", sFirstPart.Trim(), sSecondPart);
                    }
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleValidateSentenceEnd", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private string HandleValidateSentenceEnd_DotEscaped(Match MyFirstSentence)
        {
            string sOutputContent = MyFirstSentence.Value.ToString();
            try
            {
                MatchCollection mcDotSpace = Regex.Matches(sOutputContent, @"(?:(?:~~dot~~))");
                if (mcDotSpace != null && mcDotSpace.Count > 0)
                {
                    Match myDotMatch = mcDotSpace[mcDotSpace.Count - 1];

                    if ((myDotMatch.Index + myDotMatch.Length + 1) <= sOutputContent.Length)
                    {
                        string sSpaceChar = sOutputContent.Substring(myDotMatch.Index + myDotMatch.Length, 1);
                        if (sSpaceChar.Equals(" "))
                        {
                            string sFirstPart = sOutputContent.Substring(0, myDotMatch.Index + myDotMatch.Length);
                            string sSecondPart = sOutputContent.Substring(myDotMatch.Index + myDotMatch.Length, ((sOutputContent.Length) - (myDotMatch.Index + myDotMatch.Length)));

                            if (Regex.IsMatch(sSecondPart, @"(?:(?:ed[s]?)\.)[ ]*$"))
                            {
                                return sOutputContent;
                            }

                            if (Regex.IsMatch(Regex.Replace(sFirstPart, @"[\-\.]", ""), @"(?:(?:(?:,|(?:\b(?:&|and))) (?:[A-Z][a-z]+ )?(?:(?:[A-Z]+)~~dot~~)+)[ ]*)$"))
                            {
                                return sOutputContent;
                            }

                            if (Regex.IsMatch(sFirstPart, @"^(?:(?:[A-Z](?:~~dot~~|\.) ?)+)$"))
                            {
                                return sOutputContent;
                            }

                            if (Regex.IsMatch(sSecondPart, @"^(?:[ ]*(?:and|&))"))
                            {
                                return sOutputContent;
                            }


                            if (Regex.IsMatch(sSecondPart, @"\b([a-z]+)\b"))
                            {
                                sOutputContent = string.Format("{0}<sbr/>{1}", sFirstPart.Trim(), sSecondPart);
                            }
                            else if (!Regex.IsMatch(sSecondPart, @"^(?:\b(?:[a-z]+)|(?:and|&))"))
                            {
                                sOutputContent = string.Format("{0}<sbr/>{1}", sFirstPart.Trim(), sSecondPart);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\HandleValidateSentenceEnd_DotEscaped", ex.Message, true, "");
            }
            return sOutputContent;
        }


        private string SetFirstAuEdCollabBoundary_PostCleanup(string sRefContent)
        {
            try
            {
                string sEscapedSymbols = "(?:DOT|ESCLA|QUES|LRB|RRB|LSB|RSB)";

                sRefContent = Regex.Replace(sRefContent, string.Format(@"\-({0})[\u2013]({0})\-", sEscapedSymbols), "-$1--$2-");

                //sRefContent = Regex.Replace(sRefContent, "</?PubDate>", "");
                sRefContent = Regex.Replace(sRefContent, "</?Etal>", "");
                sRefContent = Regex.Replace(sRefContent, "</?Website>", "");
                sRefContent = Regex.Replace(sRefContent, "</?ldlURLLabel>", "");
                sRefContent = Regex.Replace(sRefContent, "</?ldlTempTitle>", "");

                //sRefContent = Regex.Replace(sRefContent, "<sbr/>", " ");

                sRefContent = Regex.Replace(sRefContent, "-DOT-", ".");
                sRefContent = Regex.Replace(sRefContent, "-ESCLA-", "!");
                sRefContent = Regex.Replace(sRefContent, "-QUES-", "?");

                sRefContent = Regex.Replace(sRefContent, "-LRB-", "(");
                sRefContent = Regex.Replace(sRefContent, "-RRB-", ")");
                sRefContent = Regex.Replace(sRefContent, "-LSB-", "[");
                sRefContent = Regex.Replace(sRefContent, "-RSB-", "]");

                sRefContent = Regex.Replace(sRefContent, "-RRB–RRB-", "))");

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\SetFirstAuEdCollabBoundary_PostCleanup", ex.Message, true, "");
            }
            return sRefContent;
        }


        private bool RegenerateSentences(ref string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "(?:<PubDate>(?:(?!</?PubDate>).)+</PubDate>)", "<sbr/>$&<sbr/>");
                sRefContent = Regex.Replace(sRefContent, "(?:<Etal>(?:(?!</?Etal>).)+</Etal>)", "$&<sbr/>");
                sRefContent = Regex.Replace(sRefContent, "(?:<Website>(?:(?!</?Website>).)+</Website>)", "<sbr/>$&<sbr/>");
                sRefContent = Regex.Replace(sRefContent, "(?:<ldlURLLabel>(?:(?!</?ldlURLLabel>).)+</ldlURLLabel>)", "<sbr/>$&<sbr/>");
                sRefContent = Regex.Replace(sRefContent, "(?:<ldlTempTitle>(?:(?!</?ldlTempTitle>).)+</ldlTempTitle>)", "<sbr/>$&<sbr/>");
                sRefContent = Regex.Replace(sRefContent, "(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)", "<sbr/>$&<sbr/>");

                sRefContent = NormalizePunctuation4SbrTag(sRefContent);

            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\RegenerateSentences", ex.Message, true, "");
            }
            return true;
        }

        private string NormalizePunctuation4SbrTag(string sRefContent)
        {
            try
            {
                //post cleanup
                sRefContent = Regex.Replace(sRefContent, "<sbr/></(b|i|u|sc|sub|sup)>", "</$1><sbr/>");
                sRefContent = Regex.Replace(sRefContent, "<(b|i|u|sc|sub|sup)><sbr/>", "<sbr/><$1>");

                sRefContent = Regex.Replace(sRefContent, "([ ]*<sbr/>[ ]*)", "<sbr/>");
                sRefContent = Regex.Replace(sRefContent, @"<sbr/>([\:.,;,]+)", "$1<sbr/>");
                sRefContent = Regex.Replace(sRefContent, "([ ]*<sbr/>[ ]*)", "<sbr/>");
                sRefContent = Regex.Replace(sRefContent, "(?:(?:<sbr/>){2,})", "<sbr/>");
                sRefContent = Regex.Replace(sRefContent, "(?:<sbr/>)$", "");
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\NormalizePunctuation4SbrTag", ex.Message, true, "");
            }
            return sRefContent;
        }

    }





    public class DataClass
    {
        public string Column1 { get; set; }
        public List<DataRow> Properties { get; set; }
    }


    //public class StanfordEnglishNLP
    //{
    //    Stanford_NLP_ServiceClient objNLP = new Stanford_NLP_ServiceClient();

    //    public string DoSentenceSplitting(string sRawContent, ref string sErrMsg)
    //    {
    //        try
    //        {
    //            sRawContent = EscapeWordWithDot(sRawContent);
    //            sRawContent = objNLP.SentenceSplitter(sRawContent);
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //        return sRawContent;
    //    }

    //    public string DoNER_Tagging(string sRawContent, ref string sErrMsg)
    //    {
    //        try
    //        {
    //            sRawContent = objNLP.NER_Tagger(sRawContent);
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //        return sRawContent;
    //    }


    //    private string EscapeWordWithDot(string sRefContent)
    //    {
    //        try
    //        {
    //            sRefContent = Regex.Replace(sRefContent, @"(?:<(b|i|u|sc|sup|sub)>)(?:(?!</?\1>).)+(?:</\1>)", EscapeDot4NLP);
    //            sRefContent = Regex.Replace(sRefContent, @"(?:[\u201c](?:[^\u201c\u201d]+)[\u201d])", EscapeDot4NLP);
    //            sRefContent = Regex.Replace(sRefContent, @"(?:[\u2018](?:[^\u2018\u2019]+)[\u2019])", EscapeDot4NLP);
    //            sRefContent = Regex.Replace(sRefContent, @"(?:[\(](?:[^\(\)]+)[\)])", EscapeDot4NLP);
    //            sRefContent = Regex.Replace(sRefContent, @"(?:[\[](?:[^\[\]]+)[\]])", EscapeDot4NLP);
    //            sRefContent = Regex.Replace(sRefContent, @"(?:[\{](?:[^\{\}]+)[\}])", EscapeDot4NLP);

    //            sRefContent = Regex.Replace(sRefContent, @"(?<=[ ])(?:Mc\.)(?= [A-Z]\.)", EscapeDot4NLP);
    //            sRefContent = Regex.Replace(sRefContent, @"(?:(?<=[ ])(?:(?:[Nn]um(?:ber)?|[Nn]o)\.)(?:[^\.,<>]+))", EscapeDot4NLP);
    //            sRefContent = Regex.Replace(sRefContent, @"(?:(?<=[ ])(?:(?:[Vv]ol(?:ume)?|[Vv])\.)(?:[^\.,<>]+))", EscapeDot4NLP);
    //            sRefContent = Regex.Replace(sRefContent, @"(?:(?<=[ ])(?:(?:[Ii]ss(?:ue)?)\.)(?:[^\.,<>]+))", EscapeDot4NLP);
    //            sRefContent = Regex.Replace(sRefContent, @"(?:(?<=[ ])(?:(?:Univ|Inst|Acad)\.)(?=[ ]))", EscapeDot4NLP);


    //            //Un-Escape DOT
    //            sRefContent = Regex.Replace(sRefContent, @"([\)\]])-DOT-</b>", "$1.</b>");

    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //        return sRefContent;
    //    }

    //    private string EscapeDot4NLP(Match myDotEscapeMatch)
    //    {
    //        string sOutputContent = myDotEscapeMatch.Value.ToString();
    //        try
    //        {
    //            sOutputContent = Regex.Replace(sOutputContent, @"[\.] ", "-DOT- ");
    //            sOutputContent = Regex.Replace(sOutputContent, @"[\!] ", "-ESCLA- ");
    //            sOutputContent = Regex.Replace(sOutputContent, @"[\?] ", "-QUES- ");

    //            sOutputContent = Regex.Replace(sOutputContent, @"[\.]</i>", "-DOT-</i>");
    //            sOutputContent = Regex.Replace(sOutputContent, @"[\!]</i>", "-ESCLA-</i>");
    //            sOutputContent = Regex.Replace(sOutputContent, @"[\?]</i>", "-QUES-</i>");

    //            sOutputContent = Regex.Replace(sOutputContent, @"[\.]$", "-DOT-");
    //            sOutputContent = Regex.Replace(sOutputContent, @"[\!]$", "-ESCLA-");
    //            sOutputContent = Regex.Replace(sOutputContent, @"[\?]$", "-QUES-");

    //            sOutputContent = Regex.Replace(sOutputContent, @"[\.](?=(?:</(?:b|i|u|sc|sup|sub)>))", "-DOT-");
    //            sOutputContent = Regex.Replace(sOutputContent, @"[\!](?=(?:</(?:b|i|u|sc|sup|sub)>))", "-ESCLA-");
    //            sOutputContent = Regex.Replace(sOutputContent, @"[\?](?=(?:</(?:b|i|u|sc|sup|sub)>))", "-QUES-");



    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //        return sOutputContent;
    //    }


    //}
}
