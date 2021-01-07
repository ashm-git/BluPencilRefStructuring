using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RefBOT.Controllers
{
    public static class AutoStructReferencesPatterns
    {
        //public static string sAuthorEditorGroup = @"(?:(?:(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?(?:(?:(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)*(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*))|(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)|(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*)))(?:<Etal>(?:(?!</?Etal>).)+</Etal>[\:\.;, ]*)?))";

        //public static string sAuthorEditorGroup = @"(?:(?:(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?(?:(?:(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)*(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*))|(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>[ ]*))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)|(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*)))(?:<Etal>(?:(?!</?Etal>).)+</Etal>[\:\.;, ]*)?))";

        //public static string sAuthorEditorGroup = @"(?:(?:(?:(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?(?:(?:(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)*(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*))|(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>[ ]*))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)|(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*)))(?:<Etal>(?:(?!</?Etal>).)+</Etal>[\:\.;, ]*)?))|(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?<ldlFirstAuEdCollabGroup>(?:(?!</?ldlFirstAuEdCollabGroup>).)+</ldlFirstAuEdCollabGroup>)[\:\.;, ]*)";

        ////updated by Dakshinamoorthy.G on 2018-Dec-24
        ////To avoid infinity loop in Regex.IsMatch
        //public static string sAuthorEditorGroup = @"(?:(?:(?:(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?(?:(?:(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)?(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*))|(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>[ ]*))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)|(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*)))(?:<Etal>(?:(?!</?Etal>).)+</Etal>[\:\.;, ]*)?))|(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?<ldlFirstAuEdCollabGroup>(?:(?!</?ldlFirstAuEdCollabGroup>).)+</ldlFirstAuEdCollabGroup>)[\:\.;, ]*)";


        public static string sCapsNonEnglishChar = "\u00C0-\u00DF\u0100\u0102\u0104\u0106\u0108\u010A\u010C\u010E\u0110\u0112\u0114\u0116\u0118\u011A\u011C\u011E\u0120\u0122\u0124\u0126\u0128\u012A\u012C\u012E\u0130\u0132\u0134\u0136\u0139\u013B\u013D\u013F\u0141\u0143\u0145\u0147\u014A\u014C\u014E\u0150\u0152\u0154\u0156\u0158\u015A\u015C\u015E\u0160\u0162\u0164\u0166\u0168\u016A\u016C\u016E\u0170\u0172\u0174\u0176\u0178\u0179\u017B\u017D\u0181\u0184\u0186\u0187\u0189\u018A\u018B\u018E\u0191\u0193\u0197\u0198\u019A\u019D\u01A0\u01A2\u01A4\u01A6\u01A7\u01AC\u01AD\u01AF\u01B3\u01B5\u01C4\u01C7\u01C8\u01CA\u01CB\u01CD\u01CF\u01D3\u01D5\u01D7\u01D9\u01DB\u01DE\u01E0\u01E2\u01E4\u01E6\u01E8\u01EA\u01EC\u01EE\u01F1\u01F2\u01F4\u01F6\u01F8\u01FA\u01FC\u01FE\u0200\u0202\u0204\u0206\u0208\u020A\u020C\u020E\u0210\u0212\u0214\u0216\u0218\u021A\u021C\u021E\u0224\u0226\u0228\u022A\u022C\u022E\u0230\u0232\u023A\u023B\u023E\u0243\u0244\u0246\u0248\u024A\u024C\u024E\u0290\u0299\u029B\u029C\u029F\u0372\u0376\u0386\u0388\u0389\u038A\u038C\u038E\u03AA\u03AB\u03CF\u03DC\u03FA\u0400\u0401\u0402\u0405\u0406\u0407\u0408\u0409\u040C\u040D\u040E\u0410\u0411\u0412\u0415\u041A\u041C\u041D\u041E\u0420\u0421\u0422\u0423\u0425\u042A\u042B\u043B\u043C\u043D\u043E\u044F\u045C\u045D\u045E\u0460\u0474\u0475\u0476\u048A\u048C\u048E\u049A\u049C\u049E\u04A0\u04A2\u04A4\u04AA\u04AC\u04AE\u04AF\u04B0\u04B1\u04B2\u04B3\u04B6\u04B8\u04BA\u04BC\u04BE\u04C0\u04C3\u04C7\u04C9\u04CB\u04CD\u04CF\u04D0\u04D2\u04D4\u04D6\u04D8\u04DA\u04E2\u04E4\u04E6\u04E8\u04EA\u04EE\u04F0\u04F2\u04FC\u04FE\u051A\u051C\u051E\u1E00\u1E02\u1E04\u1E06\u1E08\u1E0A\u1E0C\u1E0E\u1E10\u1E12\u1E14\u1E16\u1E18\u1E1A\u1E1C\u1E1E\u1E20\u1E22\u1E24\u1E26\u1E28\u1E2A\u1E2C\u1E2E\u1E30\u1E32\u1E34\u1E36\u1E38\u1E3A\u1E3C\u1E3E\u1E40\u1E42\u1E44\u1E46\u1E48\u1E4A\u1E4C\u1E4E\u1E50\u1E52\u1E54\u1E56\u1E58\u1E5A\u1E5C\u1E5E\u1E60\u1E62\u1E64\u1E66\u1E68\u1E6A\u1E6C\u1E6E\u1E70\u1E72\u1E74\u1E76\u1E78\u1E7A\u1E7C\u1E7E\u1E80\u1E82\u1E84\u1E86\u1E88\u1E8A\u1E8C\u1E8E\u1E90\u1E92\u1E94\u1EA0\u1EA2\u1EA4\u1EA6\u1EA8\u1EAA\u1EAC\u1EAE\u1EB0\u1EB2\u1EB4\u1EB6\u1EB8\u1EBA\u1EBC\u1EBE\u1EC0\u1EC2\u1EC4\u1EC6\u1EC8\u1ECA\u1ECC\u1ECE\u1ED0\u1ED2\u1ED4\u1ED6\u1ED8\u1EDA\u1EDC\u1EDE\u1EE0\u1EE2\u1EE4\u1EE6\u1EE8\u1EEA\u1EEC\u1EEE\u1EF0\u1EF2\u1EF4\u1EF6\u1EF8\u1EFA\u1EFE\u1F08\u1F09\u1F0A\u1F0B\u1F0C\u1F0D\u1F0E\u1F0F\u1F1A\u1F1B\u1F1C\u1F1D\u1F28\u1F2A\u1F2B\u1F2C\u1F2D\u1F2E\u1F2F\u1F38\u1F39\u1F3A\u1F3B\u1F3C\u1F3D\u1F3E\u1F3F\u1F48\u1F49\u1F4A\u1F4B\u1F4C\u1F4D\u1F59\u1F5B\u1F5D\u1F5F\u1F89\u1F8A\u1F8B\u1F8C\u1F8D\u1F8E\u1F8F\u1F98\u1F99\u1F9A\u1F9B\u1F9C\u1F9D\u1F9E\u1F9F\u1FB8\u1FB9\u1FBA\u1FBB\u1FBC\u1FC8\u1FC9\u1FCA\u1FCB\u1FCC\u1FD6\u1FD7\u1FD8\u1FD9\u1FDA\u1FDB\u1FE6\u1FE7\u1FE8\u1FE9\u1FEA\u1FEB\u1FEC\u1FF8\u1FF9\u2C60\u2C62\u2C63\u2C64\u2C67\u2C69\u2C6B\u2C6E\u2C7E\u2C7F\uA740\uA742\uA744\uA748\uA74A\uA74C\uA74E\uA750\uA752\uA756\uA758\uA75E\uA764\uA766\uA7A0\uA7A2\uA7A4\uA7A6\uA7A8\uA7FE\uFFE5\uFFE6";

        public static string sSmallNonEnglishChar = "\u00E0-\u00FF\u0101\u0103\u0105\u0107\u0109\u010B\u010D\u010F\u0111\u0113\u0115\u0117\u0119\u011B\u011D\u011F\u0121\u0123\u0125\u0127\u0129\u012B\u012D\u012F\u0133\u0135\u0137\u0138\u013A\u013C\u013E\u0140\u0142\u0144\u0146\u0148\u0149\u014B\u014D\u014F\u0151\u0153\u0155\u0157\u0159\u015B\u015D\u015F\u0161\u0163\u0165\u0167\u0169\u016B\u016D\u016F\u0171\u0173\u0175\u0177\u017A\u017C\u017E\u017F\u0180\u0182\u0185\u0188\u018C\u018F\u0192\u0199\u01A1\u01A3\u01A5\u01A8\u01AB\u01AE\u01B0\u01B2\u01B4\u01B6\u01C5\u01C6\u01C9\u01CC\u01CE\u01D0\u01D2\u01D4\u01D6\u01D8\u01DA\u01DC\u01DD\u01DF\u01E1\u01E3\u01E5\u01E7\u01E9\u01EB\u01ED\u01EF\u01F0\u01F3\u01F5\u01F9\u01FB\u01FD\u01FF\u0201\u0203\u0205\u0207\u0209\u020B\u020D\u020F\u0211\u0213\u0215\u0217\u0219\u021B\u021D\u021F\u0225\u0227\u0229\u022B\u022D\u022F\u0231\u0233\u023C\u023F\u0240\u0247\u0249\u024B\u024D\u024F\u0250\u0251\u0252\u0259\u0258\u0260\u0261\u0265\u0266\u0267\u0268\u026B\u0287\u0288\u0289\u028B\u028D\u028E\u028F\u0291\u029E\u02A3\u02A4\u02A5\u02A6\u02A7\u02AA\u02AB\u02B0\u02B4\u02B6\u0390\u03BF\u03CA\u03CB\u03CC\u03CD\u03CE\u03DD\u03FB\u040A\u040B\u042C\u0435\u043A\u0450\u0451\u0457\u0461\u0477\u048B\u048D\u048F\u049B\u049D\u049F\u04A1\u04A3\u04A5\u04AB\u04AD\u04B7\u04B9\u04BB\u04BD\u04BF\u04C4\u04C8\u04CA\u04CC\u04CE\u04D1\u04D3\u04D5\u04D7\u04D9\u04DB\u04E3\u04E5\u04E7\u04E9\u04EB\u04EF\u04F1\u04F3\u04FD\u04FF\u051B\u051D\u051F\u0526\u0527\u1E01\u1E03\u1E05\u1E07\u1E09\u1E0B\u1E0D\u1E0F\u1E11\u1E13\u1E15\u1E17\u1E19\u1E1B\u1E1D\u1E1F\u1E21\u1E23\u1E25\u1E27\u1E29\u1E2B\u1E2D\u1E2F\u1E31\u1E35\u1E37\u1E39\u1E3B\u1E3D\u1E3F\u1E41\u1E43\u1E45\u1E47\u1E49\u1E4B\u1E4D\u1E4F\u1E51\u1E53\u1E55\u1E57\u1E59\u1E5B\u1E5D\u1E5F\u1E61\u1E63\u1E65\u1E67\u1E69\u1E6B\u1E6D\u1E6F\u1E71\u1E73\u1E75\u1E77\u1E79\u1E7B\u1E7D\u1E7F\u1E81\u1E83\u1E85\u1E87\u1E89\u1E8B\u1E8D\u1E8F\u1E91\u1E93\u1E95\u1E96\u1E97\u1E98\u1E99\u1E9A\u1EA1\u1EA3\u1EA5\u1EA7\u1EA9\u1EAB\u1EAD\u1EAF\u1EB1\u1EB3\u1EB5\u1EB7\u1EB9\u1EBB\u1EBD\u1EBF\u1EC1\u1EC3\u1EC5\u1EC7\u1EC9\u1ECB\u1ECD\u1ECF\u1ED1\u1ED3\u1ED5\u1ED7\u1ED9\u1EDB\u1EDD\u1EDF\u1EE1\u1EE3\u1EE5\u1EE7\u1EE9\u1EEB\u1EED\u1EEF\u1EF1\u1EF3\u1EF5\u1EF7\u1EF9\u1EFB\u1EFF\u1F18\u1F29\u1F78\u1F79\u1F7A\u1F7C\u1F7D\u2C65\u2C66\u2C68\u2C6A\u2C6C\u2C71\u2C73\uA741\uA743\uA745\uA749\uA74B\uA74D\uA74F\uA751\uA753\uA757\uA759\uA75F\uA765\uA767\uA7A1\uA7A3\uA7A5\uA7A7\uA7A9\u00DF\u00F8\u0131";

        //updated by Dakshinamoorthy.G on 2018-Oct-08
        //public static string sAuthorEditorGroup = @"(?:(?:(?:(?:(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?(?:[\[\(])?(?:(?:(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)?(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*))|(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>[ ]*))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)|(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*)))(?:<Etal>(?:(?!</?Etal>).)+</Etal>[\:\.;, ]*)?(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)?))|(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?<ldlFirstAuEdCollabGroup>(?:(?!</?ldlFirstAuEdCollabGroup>).)+</ldlFirstAuEdCollabGroup>))[\:\.;, \)]*)";

        //updated by Dakshinamoorthy.G on 2020-Apr-02
        //public static string sAuthorEditorGroup = @"(?:(?:(?:(?:(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?(?:[\[\(])?(?:(?:(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&|(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>))| )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&|(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>)) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)?(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*))|(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&|(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>)) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>[ ]*))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&|(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>)) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)|(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*)))(?:<Etal>(?:(?!</?Etal>).)+</Etal>[\:\.;, ]*)?(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)?))|(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?<ldlFirstAuEdCollabGroup>(?:(?!</?ldlFirstAuEdCollabGroup>).)+</ldlFirstAuEdCollabGroup>))[\:\.;, \)]*)";


        public static string sAuthorEditorGroup_Full = @"(?:(?:(?:(?:(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?(?:[\[\(])?(?:(?:(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&|(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>))| )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&|(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>)) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)?(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*))|(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&|(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>)) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>[ ]*))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&|(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>)) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)|(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*)))(?:<Etal>(?:(?!</?Etal>).)+</Etal>[\:\.;, ]*)?(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)?))|(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?<ldlFirstAuEdCollabGroup>(?:(?!</?ldlFirstAuEdCollabGroup>).)+</ldlFirstAuEdCollabGroup>))[\:\.;, \)]*)";

        public static string sAuthorEditorGroup = @"(?:(?:<AuEdGroup>(?:(?:(?!</?AuEdGroup>).)+)</AuEdGroup>)[\:\.;,\) ]*)";

        //reverted by Dakshinamoorthy on 2020-May-19 (adding "<RefPrefix>" tag)
        //public static string sAuthorEditorGroup = @"(?:(?:(?:(?:(?:(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?(?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>[\:\.;, ]*))?(?:[\[\(])?(?:(?:(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&|(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>))| )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&|(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>)) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)?(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*))|(?:(?:(?:<Author>(?:(?!</?Author>).)+</Author>[\:\.;, ]*(?:(?:\band|&|(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>)) )?)+)|(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>[ ]*))?(?:(?:<Editor>(?:(?!</?Editor>).)+</Editor>[\:\.;, ]*(?:(?:\band|&|(?:<ldlAuthorDelimiterAnd>(?:(?!</?ldlAuthorDelimiterAnd>).)+</ldlAuthorDelimiterAnd>)) )?)+))(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)|(?:<Collab>(?:(?!</?Collab>).)+</Collab>[\:\.;, ]*)))(?:<Etal>(?:(?!</?Etal>).)+</Etal>[\:\.;, ]*)?(?:(?:(?:<ldlEditorDelimiterEds_Back>(?:(?!</?ldlEditorDelimiterEds_Back>).)+</ldlEditorDelimiterEds_Back>)|(?:<ldlEditorDelimiterEds_Front>(?:(?!</?ldlEditorDelimiterEds_Front>).)+</ldlEditorDelimiterEds_Front>))?[\:\.;, ]*)?))|(?:(?:<RefLabel>(?:(?!</?RefLabel>).)+</RefLabel>[\:\.;, ]*)?(?:<RefPrefix>(?:(?!</?RefPrefix>).)+</RefPrefix>[\:\.;, ]*)?<ldlFirstAuEdCollabGroup>(?:(?!</?ldlFirstAuEdCollabGroup>).)+</ldlFirstAuEdCollabGroup>))[\:\.;, \)]*)";

        public static string sRefLabel = @"(?:(?:<RefLabel>(?:(?:(?!</?RefLabel>).)+)</RefLabel>)[ ]*)";
        public static string sPubDate = @"(?:<PubDate>(?:(?!</?PubDate>).)+</PubDate>[\:\.;, ]*)";
        public static string sAccessedDateWithLabel = @"(?:(?:<ldlAccessedDateLabel>(?:(?!</?ldlAccessedDateLabel).)+</ldlAccessedDateLabel>[\:\.;, ]*)?<ldlAccessedDate>(?:(?!</?ldlAccessedDate>).)+</ldlAccessedDate>[\:\.;, ]*)";
        public static string sPubMedIdWithLabel = @"(?:(?:<PubMedIdLabel>(?:(?!</?PubMedIdLabel>).)+</PubMedIdLabel>[\:.;, ]*)?(?:<PubMedIdNumber>(?:(?!</?PubMedIdNumber>).)+</PubMedIdNumber>)[\:\.;, ]*)";
        public static string sPublisherName = @"(?:(?:<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>[\:\.;, ]*)+)";
        public static string sLocationName = @"(?:(?:<ldlCity>(?:(?!</?ldlCity>).)+</ldlCity>[\:.;, ]*|<ldlState>(?:(?!</?ldlState>).)+</ldlState>[\:.;, ]*|<ldlCountry>(?:(?!</?ldlCountry>).)+</ldlCountry>[\:.;, ]*)+)";

        //updated by Dakshinamoorthy on 2019-Mar-20
        //public static string sURLWithLabel = @"(?:(?:<ldlURLLabel>(?:(?!</?ldlURLLabel>).)+</ldlURLLabel>[\:\.;, ]*)?<Website>(?:(?!</?Website>).)+</Website>[\:\.;, ]*)";
        public static string sURLWithLabel = @"(?:(?:<ldlURLLabel>(?:(?!</?ldlURLLabel>).)+</ldlURLLabel>[\:\.;, ]*)?<Website>(?:(?!</?Website>).)+</Website>[\:\.;, ]*(?:<Website>(?:(?!</?Website>).)+</Website>[\:\.;, ]*)*)";

        public static string sThesisKeyword = @"(?:(?:<ldlThesisKeyword>(?:(?!</?ldlThesisKeyword>).)+</ldlThesisKeyword>)[\:\.;, ]*)";
        public static string sReportKeyword = @"(?:(?:(?:<ldlReportKeyword>(?:(?!</?ldlReportKeyword>).)+</ldlReportKeyword>)|(?:<ldlReportNumber>(?:(?!</?ldlReportNumber>).)+</ldlReportNumber>))[\:\.;, ]*)";
        public static string sISBNNumber = @"(?:(?:<ldlISBNNumber>(?:(?!</?ldlISBNNumber>).)+</ldlISBNNumber>)[\:\.;, ]*)";
        public static string sPageRange = @"(?:(?:<PageRange>(?:(?!</?PageRange>).)+</PageRange>)[\:\.;, ]*)";
        public static string sVolumeNumber = @"(?:(?:<Vol_No>(?:(?!</?Vol_No>).)+</Vol_No>)[\:\.;, ]*)";
        public static string sIssueNumber = @"(?:(?:<Issue_No>(?:(?!</?Issue_No>).)+</Issue_No>)[\:\.;, ]*)";
        public static string sMisc = @"(?:(?:<ldlMisc>(?:(?!</?ldlMisc>).)+</ldlMisc>)[\:\.;, ]*)";
        public static string sArticleTitle = @"(?:(?:<Article_Title>(?:(?!</?Article_Title>).)+</Article_Title>)[\:\.;, ]*)";
        public static string sJournalTitle = @"(?:(?:<Journal_Title>(?:(?!</?Journal_Title>).)+</Journal_Title>)[\:\.;, ]*)";
        public static string sEditionNumber = @"(?:(?:<ldlEditionNumber>(?:(?!</?ldlEditionNumber>).)+</ldlEditionNumber>)[\:\.;, ]*)";
        public static string sDoiNumber = @"(?:(?:<Doi>(?:(?!</?Doi>).)+</Doi>)[\)\:\.;, ]*)";

        //public static string sUnknownNumber = @"(?:[ ]?\b(?:[0-9]+(?:[ ]?[\u2013\-][ ]?[0-9]+)?)(?:[\:\.;, ]+|$))";
        //public static string sUnknownNumber = @"(?:(?:[ ]?\b(?:[0-9]+(?:[ ]?[\u2013\-][ ]?[0-9]+)?)(?:[\:\.;, ]+|$))|(?:(?:\b(?=[MDCLXVI])M*(?:C[MD]|D?C{0,3})(?:X[CL]|L?X{0,3})(?:I[XV]|V?I{0,3})\b)(?:[ ]?[\u2013\-][ ]?(?:\b(?=[MDCLXVI])M*(?:C[MD]|D?C{0,3})(?:X[CL]|L?X{0,3})(?:I[XV]|V?I{0,3})\b))(?:[\:\.;, ]+|$))|(?:(?:\b(?=[mdclxvi])m*(?:c[md]|d?c{0,3})(?:x[cl]|l?x{0,3})(?:i[xv]|v?i{0,3})\b)(?:[ ]?[\u2013\-][ ]?(?:\b(?=[mdclxvi])m*(?:c[md]|d?c{0,3})(?:x[cl]|l?x{0,3})(?:i[xv]|v?i{0,3})\b))(?:[\:\.;, ]+|$)))";
        public static string sUnknownNumber = @"(?:(?:[ ]?\b(?:[0-9]+(?:[ ]?[\u2013\-\u2212][ ]?[0-9]+)?)(?:[\:\.;, ]+|$))|(?:\b(?:[IVX]+|[ivx]+){1,4}\+[0-9]+(?:[\:\.;, ]+|$))|(?:[ ]?(?:[IVX]{1,4})(?:[\:\.;, ]+|$))|(?:(?:\b(?=[MDCLXVI])M*(?:C[MD]|D?C{0,3})(?:X[CL]|L?X{0,3})(?:I[XV]|V?I{0,3})\b)(?:[ ]?[\u2013\-][ ]?(?:\b(?=[MDCLXVI])M*(?:C[MD]|D?C{0,3})(?:X[CL]|L?X{0,3})(?:I[XV]|V?I{0,3})\b))(?:[\:\.;, ]+|$))|(?:(?:\b(?=[mdclxvi])m*(?:c[md]|d?c{0,3})(?:x[cl]|l?x{0,3})(?:i[xv]|v?i{0,3})\b)(?:[ ]?[\u2013\-][ ]?(?:\b(?=[mdclxvi])m*(?:c[md]|d?c{0,3})(?:x[cl]|l?x{0,3})(?:i[xv]|v?i{0,3})\b))(?:[\:\.;, ]+|$)))";

        //updated by Dakshinamoorthy on 2019-Feb-06
        //public static string sTitleLabel = @"(?:(?:<i>)?[\[\{\(]?-?[Ii]n[\:.]?[\]\)\}]?[\:.]?(?:</i>)?[\:.]? )";
        public static string sTitleLabel = @"(?:(?:(?:<i>)?[\[\{\(]?-?[Ii]n[\:.]?[\]\)\}]?[\:.]?(?:</i>)?[\:.]? )|(?:<ldlTitleLabel>(?:(?!</?ldlTitleLabel>).)+</ldlTitleLabel>[\:.]* ))";

        public static string sConferenceName = @"(?:(?:<ldlConferenceName>(?:(?!</?ldlConferenceName>).)+</ldlConferenceName>)[\:\.;, ]*)";
        public static string sConferenceDate = @"(?:(?:<ldlConferenceDate>(?:(?!</?ldlConferenceDate>).)+</ldlConferenceDate>)[\:\.;, ]*)";

        //added by Dakshinamoorthy on 2019-Feb-06
        public static string sChapterTitle = @"(?:(?:<ldlChapterTitle>(?:(?!</?ldlChapterTitle>).)+</ldlChapterTitle>)[\:\.;, ]*)";
        public static string sBookTitle = @"(?:(?:<ldlBookTitle>(?:(?!</?ldlBookTitle>).)+</ldlBookTitle>)[\:\.;, ]*)";

        //title patterns
        //updated by Dakshinamoorthy on 2020-Aug-11
        //public static string sItalicTitle = @"(?:<i>(?:(?!</?i>).){5,}</i>[\:\.;, ]*)";
        public static string sItalicTitle = @"(?:(?:(?<=(?:<PubDate>(?:(?!</?PubDate>).)+</PubDate>[\:\.;, ]*))(?:<i>(?:(?!</?i>).){5,}</i>[\:\.;,]* (?:\([^\(\)\.,\:;]+(?:, [0-9]{4})?\)[\:\.;, ]*))(?=(?:<(?:ldlCity|ldlState|ldlCountry|ldlPublisherName|Vol_No|AuEdGroup)>)))|(?:(?<=(?:<PubDate>(?:(?!</?PubDate>).)+</PubDate>[\:\.;, ]*))(?:<i>(?:(?!</?i>).){5,}</i>[\:\.;,]* (?:\([0-9]{4}\)[\:\.;, ]*)))|(?:<i>(?:(?!</?i>).){5,}</i>[\:\.;, ]*))";

        public static string sUptoCommaTitle = @"(?:(?:[^<>\.,]{5,})\, )";
        public static string sUptoSemiColonTitle = @"(?:(?:[^<>\.,]{5,})\; )";

        //updated by Dakshinamoorthy on 2020-Aug-11
        //public static string sUptoDotTitle = @"(?:(?:[^<>\.]{5,})\.,? )";
        public static string sUptoDotTitle = @"(?:(?:[^\(<>\. ][^<>\.]{5,}[^\)<>\. ])\.,? )";
        
        public static string sUptoDotTitle_Small = @"(?:(?:[^<>\.]{4,})\.,?(?: |$))";
        public static string sTitleWithoutEndPeriod = @"(?:[A-Za-z\:,\- ]{5,}[\.,]*[ ])";
        public static string sUptoQuesMarkTitle = @"(?:(?:[^<>\.\?]{5,})\? )";
        public static string sDoubleQuoteTitle = @"(?:(?:[\u201c](?:[^\u201c\u201d]{5,})[\u201d])[\:\.;, ]*)";
        public static string sDownDoubleQuoteTitle = @"(?:(?:[\u201E](?:[^\u201E\u201C]{5,})[\u201C])[\:\.;, ]*)";
        public static string sSingleQuoteTitle = @"(?:(?:[\u2018](?:[^\u2018\u2019]{5,})[\u2019])[\:\.;, ]*)";

        //added by Dakshinamoorthy on 2019-Dec-09
        public static string sMonthPattern = @"(?:\b(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b)";
        public static string sMonthPatternWithRange = string.Format(@"(?:{0}(?:[ ]?(?:[\u2013\-][ ]?{0}))?)", @"(?:\b(?:January|February|March|April|May|June|July|August|September|October|November|December)|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b)");

        //added by Dakshinamoorthy on 2020-Jun-16
        public static string sRefContent_Temp = string.Empty;

        private static string HandleCollab(Match myCollabMatch)
        {
            string sOutputContent = myCollabMatch.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "<ldlPublisherName>", "<Collab>");
                sOutputContent = Regex.Replace(sOutputContent, "</ldlPublisherName>", "</Collab>");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesPatterns.cs\HandleCollab", ex.Message, true, "");
            }
            return sOutputContent;
        }

        private static string HandleEscapeSpace(Match mySpaceMatch)
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

        private static string EscapeClosingSingleQuote(Match myMatch)
        {
            string sOutputContent = myMatch.Value.ToString();
            try
            {
                sOutputContent = Regex.Replace(sOutputContent, "[\u2019]", "~~rsquo~~");
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesPatterns.cs\EscapeClosingSingleQuote", ex.Message, true, "");
                return sOutputContent;
            }
            return sOutputContent;
        }


        public static bool DoFullPatternMatching(ref string sRefContent)
        {
            //remove ref type
            sRefContent = Regex.Replace(sRefContent, "</?(?:Book|Communication|Conference|Journal|Other|Patent|References|Report|Thesis|Web)>", "");
            sRefContent = Regex.Replace(sRefContent, "<i>(<ldl[^<>]+>)", "$1<i>");
            sRefContent = Regex.Replace(sRefContent, "(</ldl[^<>]+>)</i>", "</i>$1");

            //added by Dakshinamoorthy on 2019-Jan-30
            sRefContent = Regex.Replace(sRefContent, "</?MayBeJouTitAbbr>", "");

            //added by Dakshinamoorthy on 2019-Feb-06
            sRefContent = Regex.Replace(sRefContent, @"(?:(?:(?<= )Vol(?:ume)?\. [0-9]+ of )|(?:(?<= [Vv]ersion )[0-9]+(?:\.[0-9]+)*))", HandleEscapeSpace);
            //added by Dakshinamoorthy on 2019-Sep-16
            sRefContent = Regex.Replace(sRefContent, @"(?:\b(?:Part [0-9]+\.[0-9]+))", HandleEscapeSpace);
            //added by Dakshinamoorthy on 2020-Feb-07
            sRefContent = Regex.Replace(sRefContent, @"(?:(?: (?:[0-9]+\.[0-9]+[A-Za-z]+)\b)|(?:(?<=[ ])[0-9]+\.[0-9]+(?=\.? )))", HandleEscapeSpace);
            //added by Dakshinamoorthy on 2020-Dec-18
            sRefContent = Regex.Replace(sRefContent, @"(?:(?<=[ ])(?:[0-9]+\.[0-9]+) (?:[a-z]{1,3}))", HandleEscapeSpace);

            string sJournalTtlAbbrPatn = @"(?:(?:(?:[A-Z][a-z]*\. ){1,}(?:[A-Z][a-z]*\.),?)|(?:(?:[A-Z][a-z]*\. )(?:(?:[A-Z][a-z]*\.? ){1,}(?:[A-Z][a-z]*\.),?))|(?:(?:[A-Z][a-z]* )(?:(?:[A-Z][a-z]*\. ){1,}(?:[A-Z][a-z]*\.)[, ]*)))";

            //added by Dakshinamoorthy on 2020-Jul-14
            sRefContent = Regex.Replace(sRefContent, @"<AuEdGroup>((?:(?!</?AuEdGroup>).)+)</AuEdGroup>([ ]*<ldlPublisherName>(?:(?!</?ldlPublisherName>).)+</ldlPublisherName>)(?=[ ]*<PubDate>(?:(?!</?PubDate>).)+</PubDate>)", "<AuEdGroup>$1$2</AuEdGroup>");

            //NormalizeFormattingTags
            sRefContent = AutoStructRefCommonFunctions.NormalizeFormattingTags(sRefContent);
            sRefContent = AutoStructRefCommonFunctions.NormalizeTagSpaces(sRefContent);
            sRefContent = AutoStructRefCommonFunctions.GroupItalicTag(sRefContent);

            //added by Dakshinamoorthy on 2019-Oct-25
            string sSingleQuoteEscapePtn = string.Format("(?:(?:[\u2019]s)\b|(?:(?:[A-Z{0}]|[a-z{1}])[\u2019](?:[A-Z{0}]|[a-z{1}])))", sCapsNonEnglishChar, sSmallNonEnglishChar);
            sRefContent = Regex.Replace(sRefContent, "([\u2018][^\u2018\u2019]+s)[\u2019]( [^\u2018\u2019]+[\u2019])", "$1~~rsquo~~$2");
            sRefContent = Regex.Replace(sRefContent, sSingleQuoteEscapePtn, EscapeClosingSingleQuote);

            CheckPublisherInsideFormattingTag(ref sRefContent);

            //handle collab
            try
            {
                sRefContent = Regex.Replace(sRefContent, string.Format(@"^({0})({1})({2})", sAuthorEditorGroup, sPublisherName, sPubDate), HandleCollab, RegexOptions.None, TimeSpan.FromSeconds(60));
                sRefContent = Regex.Replace(sRefContent, "(<ldlFirstAuEdCollabGroup>(?:(?!</?ldlFirstAuEdCollabGroup>).)+</ldlFirstAuEdCollabGroup>)", HandleCollab, RegexOptions.None, TimeSpan.FromSeconds(60));
            }
            catch
            { }




            string sFullPattern = string.Empty;
            int nPatternId = 0;
            //int nRegexTimeoutSeconds = 10;

            try
            {
                nPatternId = 1;
                //<Other><Author><Surname>Holling</Surname> <Forename>C.S</Forename></Author> <PubDate>(<Year>1986</Year>).</PubDate> Engineering resilience versus ecological resilience, in: Foundations of Ecological Resilience. <ldlPublisherName>Island Press.</ldlPublisherName></Other>
                sFullPattern = string.Format("^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sTitleLabel, sUptoDotTitle, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 1;
                sFullPattern = string.Format("^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 1;
                //<Author><Surname>Ferguson</Surname> <Forename>B. K</Forename></Author> <PubDate>(<Year>2005</Year>).</PubDate> “Porous pavements.” <ldlPublisherName>Taylor and Francis-CRS Press</ldlPublisherName><ldlCity>Boca Raton</ldlCity><ldlCountry>US.</ldlCountry>
                sFullPattern = string.Format("^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 1;
                //<Author><Surname>Lefevre</Surname> <Forename>G. H</Forename></Author> , <Author><Surname>Paus</Surname> <Forename>K. H</Forename></Author> , <Author><Surname>Natarajan</Surname> <Forename>P</Forename></Author> , and <Author><Surname>Hozalski</Surname> <Forename>R. M</Forename></Author> <PubDate>(<Year>2015</Year>).</PubDate> “Review of dissolved pollutants in urban storm water and their removal and fate in bioretention cells.” <i>Journal of Environmental Engineering</i>, 141.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})( [0-9]+\.)$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 1;
                //<Collab>Massachusetts Department of Environmental Protection.</Collab> <PubDate>(<Year>2008</Year>).</PubDate> <i>Volume 2 Chapter 2: Structural BMP Specifications for the Massachusetts Stormwater Handbook</i>. <ldlCity>Boston</ldlCity><ldlState>MA.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 1;
                //<Author><Surname>Bitton</Surname> <Forename>G</Forename></Author> , <PubDate><Year>1998</Year>.</PubDate> Formula handbook for environmental engineers and scientists. <ldlPublisherName>John Wiley & Sons.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 1;
                //<Author><Surname>Majersky</Surname> <Forename>G</Forename></Author> <PubDate>(<Year>2009</Year>),</PubDate> Metals recovery from acid mine drainage using pervious concrete, <ldlPublisherName>VDM Publishing House</ldlPublisherName>, <ldlISBNNumber>ISBN 978-3-639-02618-4,</ldlISBNNumber> <PageRange> 69p.</PageRange>
                sFullPattern = string.Format("^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sPublisherName, sISBNNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 1;
                //<Author><Surname>Majersky</Surname> <Forename>G</Forename></Author> <PubDate>(<Year>2009</Year>),</PubDate> Metals recovery from acid mine drainage using pervious concrete, <ldlPublisherName>VDM Publishing House</ldlPublisherName>, <ldlISBNNumber>ISBN 978-3-639-02618-4,</ldlISBNNumber> <PageRange> 69p.</PageRange>
                sFullPattern = string.Format("^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sPublisherName, sISBNNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 1;
                //<Collab>EPA.</Collab> <PubDate>(<Year>2014</Year>).</PubDate> <i>Focus on Urban Waste Water Treatment in 2012</i>. <ldlCity>Wexford</ldlCity><ldlState>Ireland</ldlState><ldlPublisherName>EPA.</ldlPublisherName>
                sFullPattern = string.Format("^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 1;
                //<Author><Surname>Holman</Surname> <Forename>J. B</Forename></Author> <PubDate>(<Year>2004</Year>).</PubDate> <i>THE APPLICATION OF pH and ORP PROCESS CONTROL PARAMETERS WITHIN THE AEROBIC DENITRIFICATION PROCESS</i>. <ldlPublisherName>The University of Canterbury.</ldlPublisherName>
                sFullPattern = string.Format("^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 1;
                //<Author><Surname>Crichton</Surname> <Forename>D</Forename></Author> <PubDate>(<Year>1999</Year>)</PubDate> The Risk Triangle. <Website><http://www.ilankelman.org/crichton.html></Website> <ldlAccessedDate>(Apr, 08, 2015).</ldlAccessedDate>
                sFullPattern = string.Format("^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 2;
                //<Author><Forename>AIA</Forename> <Surname>Louisiana</Surname></Author> <PubDate><Year>(n.d.)</Year></PubDate> Why Use AIA Documents. <ldlAccessedDateLabel>Retrieved</ldlAccessedDateLabel> <ldlAccessedDate>November 05, 2017,</ldlAccessedDate> <ldlURLLabel>from</ldlURLLabel> <Website>https://www.aiala.com/why-use-aia-documents-2/</Website>
                sFullPattern = string.Format("^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sAccessedDateWithLabel, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 3;
                //<Author><Surname>Totterdill</Surname> <Forename>B. W</Forename></Author> <PubDate>(<Year>2006</Year>).</PubDate> FIDIC users’ guide: A practical guide to the 1999 red and yellow books, <ldlPublisherName>Thomas Telford</ldlPublisherName><ldlCity>London</ldlCity>.
                sFullPattern = string.Format("^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 4;
                //<Author><Surname>Berry</Surname> <Forename>J.W</Forename></Author> <PubDate>(<Year>2012</Year>).</PubDate> Sediment based turbidity and bacteria reduction analysis in simulated construction site runoff. <ldlThesisKeyword>Doctoral Thesis</ldlThesisKeyword>, <ldlPublisherName>Clemson University.</ldlPublisherName>
                sFullPattern = string.Format("^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sThesisKeyword, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 5;
                //<Author><Surname>Holliday</Surname> <Forename>C.P</Forename></Author> , <Author><Surname>Rasmussen</Surname> <Forename>T.C</Forename></Author> , <PubDate>(<Year>2003</Year>).</PubDate> Establishing the relationship between turbidity and total suspended sediment concentration<i>.</i> Georgia Water Resources Conference. <ldlCity>Athens</ldlCity><ldlState>GA</ldlState><ldlPublisherName>Georgia Water Center Publications.</ldlPublisherName>
                sFullPattern = string.Format("^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoDotTitle, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 6;
                //<Author><Surname>Yeager</Surname> <Forename>R.P</Forename></Author> , <PubDate><Year>1998</Year>.</PubDate> Remediation of metal-contaminated soils and sediments using magnetic adsorbents. <ldlThesisKeyword>MS Thesis</ldlThesisKeyword>, <ldlPublisherName>University of Florida</ldlPublisherName><ldlCity>Gainesville</ldlCity><ldlState>FL</ldlState><ldlCountry>USA</ldlCountry>, 1998.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})( [0-9]+\.)$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sThesisKeyword, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 7;
                //<Author><Surname>Garcia</Surname> <Forename>O. J</Forename></Author> <PubDate>(<Year>1992</Year>).</PubDate> “Growth of <i>Thiobacillus ferrooxidans</i> on solid medium: effects os some surface-active agents on colony formation.” <Vol_No>282, </Vol_No><PageRange>279–282.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 8;
                //<Author><Surname>Holmes</Surname> <Forename>D. S</Forename></Author> , and <Author><Surname>Bonnefoy</Surname> <Forename>V</Forename></Author> <PubDate>(<Year>2007</Year>).</PubDate> “Biomining.” <Editor><EForename>D. E.</EForename> <ESurname>Rawlings</ESurname></Editor> and <Editor><EForename>D. B.</EForename> <ESurname>Johnson</ESurname></Editor> , <ldlEditorDelimiterEds_Back>eds.,</ldlEditorDelimiterEds_Back> <ldlPublisherName>Springer Berlin Heidelberg</ldlPublisherName><ldlCity>Berlin</ldlCity>, <ldlCity>Heidelberg</ldlCity>, 281–307.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sAuthorEditorGroup, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6<{1}>$7</{1}>", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 9;
                //<Author><Surname>Qasim</Surname> <Forename>S. R</Forename></Author> <PubDate>(<Year>1998</Year>).</PubDate> “Wastewater treatment plants: planning, design, and operation.” <ldlPublisherName>CRC Press.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 10;
                //<Author><Surname>Silva</Surname> <Forename>P</Forename></Author> <PubDate>(<Year>2001</Year>).</PubDate> “Concrete Reinforcement Study of Portland cement at the Experimental Circular Road of the Institute of Highway Research.” <ldlThesisKeyword>Doctorate Thesis</ldlThesisKeyword>, <ldlPublisherName>Federal University of Rio de Janeiro</ldlPublisherName><ldlState>Rio de Janeiro</ldlState>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sThesisKeyword, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 11;
                //<Author><Surname>Molina</Surname> <Forename>J. B</Forename></Author> <PubDate>(<Year>2015</Year>).</PubDate> “Evaluación de la eliminación de matériaorgânica y nitrógeno de las aguasresidualesem um reactor biopelícula de membrana tubular aireada.” <ldlThesisKeyword>Doctoral thesis</ldlThesisKeyword>, <ldlPublisherName>University of Coruña</ldlPublisherName><ldlCity>Spain</ldlCity> <ldlMisc>(in Spanish).</ldlMisc>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sThesisKeyword, sPublisherName, sLocationName, sMisc);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 12;
                //<Collab>APHA,</Collab> <PubDate>(<Year>2005</Year>).</PubDate> “Standard methods for the examination of water and wastewater.” <ldlEditionNumber>21<sup>th</sup> edn.,</ldlEditionNumber> <ldlPublisherName>American Public Health Association</ldlPublisherName><ldlCity>Washington</ldlCity><ldlState>DC.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sEditionNumber, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 13;
                //<Author><Surname>Potts</Surname> <Forename>M. E</Forename></Author> , and <Author><Surname>Churchwell</Surname> <Forename>D. R</Forename></Author> <PubDate>(<Year>1994</Year>).</PubDate> “Removal of radionuclides in wastewaters utilizing potassium ferrate ( VI ).” <Vol_No>66 </Vol_No><Issue_No>(April), </Issue_No><PageRange>107–109.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 14;
                //<Author><Surname>Ninane</Surname> <Forename>L</Forename></Author> , <Author><Surname>Kanari</Surname> <Forename>N</Forename></Author> , <Author><Surname>Criado</Surname> <Forename>C</Forename></Author> , <Author><Surname>Jeannot</Surname> <Forename>C</Forename></Author> , <Author><Surname>Evrard</Surname> <Forename>O</Forename></Author> , and <Author><Surname>Neveux</Surname> <Forename>N</Forename></Author> <PubDate>(<Year>2008</Year>).</PubDate> “New Processes for Alkali Ferrate Synthesis.” <i>American Chemical Society</i>, 102–111.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 15;
                //<Author><Surname>Laksono</Surname> <Forename>F. B</Forename></Author> , and <Author><Surname>Kim</Surname> <Forename>I</Forename></Author> <PubDate>(<Year>2016</Year>).</PubDate> “Removal of 2-Bromophenol by Advanced Oxidation Process with In- situ Liquid Ferrate ( VI ).” 128–135.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 15;
                //<Author><Surname>Streeter</Surname> <Forename>V. L</Forename></Author> and <Author><Surname>Wylie</Surname> <Forename>E.B</Forename></Author> , <PubDate>(<Year>1985</Year>),</PubDate> <i>Fluid Mechanics,</i> <ldlPublisherName>McGraw-Hill</ldlPublisherName>, <ldlEditionNumber>8<sup>th</sup> Edition</ldlEditionNumber>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sPublisherName, sEditionNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 16;
                //<Author><Surname>Paterson</Surname> <Forename>L</Forename></Author> , <Author><Surname>Kennedy</Surname> <Forename>T.S</Forename></Author> , <Author><Surname>Sweeney</Surname> <Forename>D</Forename></Author> <PubDate>(<Year>2008</Year>).</PubDate> Remediation of perfluorinated alkyl chemicals at a former fire-fighting training area., <Editor><EForename>E.S.</EForename> <ESurname>Association</ESurname></Editor> <ldlEditorDelimiterEds_Back>(Ed.)</ldlEditorDelimiterEds_Back> <ldlPublisherName>RemTech</ldlPublisherName><ldlCity>Alberta</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sAuthorEditorGroup, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 17;
                //<Collab>UNFPA (United Nations Population Fund),</Collab> <PubDate>(<Year>2007</Year>).</PubDate> <i>State of World Population: Unleashing the Potential of Urban Growth</i>. <Website><http://www.unfpa.org/swp/2007/english/introduction.html></Website> <ldlAccessedDateLabel>(Accessed</ldlAccessedDateLabel> <ldlAccessedDate>08 July 2015).</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 18;
                //<Author><Surname>Bosley</Surname> <Forename>E. K</Forename></Author> <PubDate>(<Year>2008</Year>).</PubDate> <i>Hydrologic evaluation of low impact development using a continuous, spatially-distributed model</i>. <PageRange> 348 p. </PageRange><ldlThesisKeyword>Dissertation</ldlThesisKeyword>, <ldlPublisherName>Virginia Polytechnic Institute and State University</ldlPublisherName><ldlCity>Blacksburg</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sPageRange, sThesisKeyword, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 19;
                //<Author><Surname>Akan</Surname> <Forename>A. O</Forename></Author> <PubDate>(<Year>1993</Year>).</PubDate> <i>Urban Stormwater Hydrology - A Guide to Engineering Calculations,</i> <ldlCity>Lancaster</ldlCity><ldlState>Pennsylvania</ldlState><ldlPublisherName>Technomic Publishing Co.</ldlPublisherName>, <PageRange> 268 p.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sLocationName, sPublisherName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 20;
                //<Author><Surname>Yang</Surname> <Forename>B</Forename></Author> , and <Author><Surname>Ying</Surname> <Forename>G.-G</Forename></Author> <PubDate>(<Year>2014</Year>).</PubDate> “Removal of Personal Care Products Through Ferrate(VI) Oxidation Treatment.” <i>Personal Care Products in the Aquatic Environment. Handbook of Environmental Chemistry</i>, <Editor><EForename>M. S.</EForename> <ESurname>Dìaz-Cruz</ESurname></Editor> and <Editor><EForename>D.</EForename> <ESurname>Barceló</ESurname></Editor> , <ldlEditorDelimiterEds_Back>eds.,</ldlEditorDelimiterEds_Back> <ldlPublisherName>Springer International Publishing Switzerland</ldlPublisherName><ldlCity>Switzerland</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sAuthorEditorGroup, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 21;
                //<Collab>APHA (American Public Health Association).</Collab> <PubDate>(<Year>1998</Year>).</PubDate> <i>Standard methods for the examination of water and wastewater</i>, <ldlEditionNumber>21st Ed.,</ldlEditionNumber> <ldlCity>Washington</ldlCity><ldlState>DC.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sEditionNumber, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 22;
                //<Author><Surname>Chernyak</Surname> <Forename>G.Y</Forename></Author> <PubDate>(<Year>1964</Year>).</PubDate> “Dielectric methods for investigating moist soils.” <ldlPublisherName>Israel Program for Scientific Translations</ldlPublisherName><ldlCity>Jerusalem</ldlCity>, <PageRange> p18.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sPublisherName, sLocationName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 23;
                //<Author><Surname>Morrison</Surname> <Forename>R.D</Forename></Author> , <Author><Surname>Murphy</Surname> <Forename>B.L</Forename></Author> <PubDate>(<Year>2010</Year>)</PubDate> “Environmental Forensics: Contaminant Specific Guide.” <ldlPublisherName>Academic Press</ldlPublisherName>, <PageRange> 576 pages.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sPublisherName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 24;
                //<Collab>WHO,</Collab> <PubDate>(<Year>2006</Year>).</PubDate> Guidelines for the safe use of wastewater, excreta and greywater. Wastewater Use in Agriculture, <Vol_No> vol. 2. </Vol_No><ldlPublisherName>WHO</ldlPublisherName><ldlCity>Geneva</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoCommaTitle, sVolumeNumber, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 25;
                //<Collab>United Nations Environment Programme,</Collab> <PubDate>(<Year>2002</Year>).</PubDate> Global Environment Outlook 3. <ldlPublisherName>Earthscan</ldlPublisherName><ldlCity>London</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 26;
                //<Author><Surname>Tung</Surname> <Forename>Y. K</Forename></Author> , <Author><Surname>Yen</Surname> <Forename>B. C</Forename></Author> , & <Author><Surname>Melching</Surname> <Forename>C. S</Forename></Author> <PubDate>(<Year>2006</Year>).</PubDate> “Hydrosystems engineering reliability assessment and risk analysis.” <ldlPublisherName>McGraw-Hill Education.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 27;
                //<Author><Surname>Ikumi</Surname> <Forename>D. S</Forename></Author> <PubDate>(<Year>2011</Year>).</PubDate> “The development of a three phase plant-wide mathematical model for sewage treatment” (<ldlThesisKeyword>Doctoral dissertation</ldlThesisKeyword>, <ldlPublisherName>University of Cape Town</ldlPublisherName>). <ldlURLLabel>Retrieved from</ldlURLLabel> <Website>https://open.uct.ac.za/handle/11427/11519.</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sThesisKeyword, sPublisherName, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 28;
                //<Author><Surname>Read</Surname> <Forename>J</Forename></Author> , and <Author><Surname>Whiteoak</Surname> <Forename>D</Forename></Author> <PubDate>(<Year>2003</Year>).</PubDate> <i>The Shell bitumen handbook.</i> <ldlEditionNumber>5th ed.,</ldlEditionNumber> <ldlPublisherName>Thomas Telford</ldlPublisherName><ldlCity>London</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sEditionNumber, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 29;
                //<Collab>United Nations (UN)</Collab> <PubDate>(<Year>1987</Year>).</PubDate> “Our common future.” <Website>http://www.un-documents.net/ocf-02.htm#III.5.</Website> <ldlAccessedDate>(Nov. 18, 2015)</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 30;
                //<Author><Surname>Saitoh</Surname> <Forename>S</Forename></Author> <PubDate>(<Year>1988</Year>).</PubDate> Experimental study of engineering properties of cement improved ground by the deep mixing method, <ldlThesisKeyword>Ph.D. Thesis</ldlThesisKeyword>, <ldlPublisherName>Nibon University.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sThesisKeyword, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 31;
                //<Author><Surname>Hendershot</Surname> <Forename>W. H</Forename></Author> , <Author><Surname>Lalande</Surname> <Forename>H</Forename></Author> , and <Author><Surname>Duquette</Surname> <Forename>M</Forename></Author> <PubDate>(<Year>2007</Year>).</PubDate> “Ion Exchange and Exchangeable Cations.” <i>Soil Sampling and Methods of Analysis</i>, <Editor><EForename>M. R.</EForename> <ESurname>Carter</ESurname></Editor> , and <Editor><EForename>E. G.</EForename> <ESurname>Gregorich</ESurname></Editor> , <ldlEditorDelimiterEds_Back>eds.,</ldlEditorDelimiterEds_Back> <ldlPublisherName>Canadian Society of Soil Science</ldlPublisherName><ldlState>FL</ldlState>, 197-206.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sAuthorEditorGroup, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7<{2}>$8</{2}>", "ldlChapterTitle", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 32;
                //<Author><Surname>Brandford</Surname> <Forename>PA</Forename></Author> , <Author><Surname>Baird</Surname> <Forename>J</Forename></Author> <PubDate>(<Year>1983</Year>)</PubDate> Industrial utilization of polysaccharide. In <Editor><ESurname>Aspinall</ESurname> <EForename>G.O</EForename></Editor> <ldlEditorDelimiterEds_Back>(eds)</ldlEditorDelimiterEds_Back> The Polysaccharides 2. 411-490. <ldlPublisherName>Academic Press</ldlPublisherName><ldlCity>New York</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sUptoDotTitle, sUnknownNumber, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}><{3}>$7</{3}>$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 33;
                //<Author><Surname>Karol</Surname> <Forename>RH</Forename></Author> <PubDate>(<Year>2003</Year>)</PubDate> Chemical grouting and soil stabilization, revised and expanded. <Vol_No> Vol:12. </Vol_No><ldlPublisherName>CRC Press.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sVolumeNumber, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 34;
                //<Author><Surname>Puppala</Surname> <Forename>AJ</Forename></Author> , <Author><Surname>Musenda</Surname> <Forename>C</Forename></Author> <PubDate>(<Year>2000</Year>)</PubDate> Effects of fibers reinforcement on strength and volume change behavior of expansive soils. <ldlPublisherName>Transportation Research Board</ldlPublisherName>, <ldlCity>Washington</ldlCity><ldlState>D.C.</ldlState>, <Vol_No>1736: </Vol_No><PageRange>134-140.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPublisherName, sLocationName, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 35;
                //<Author><Surname>Swain</Surname> <Forename>K</Forename></Author> <PubDate>(<Year>2015</Year>)</PubDate> Stabilization of soil using geopolymer and biopolymer. <ldlThesisKeyword>MSc thesis</ldlThesisKeyword>, <ldlPublisherName>National Institute of Technology</ldlPublisherName><ldlCity>Rourkela</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sThesisKeyword, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 36;
                //<Author><Surname>Fisher</Surname> <Forename>R.V</Forename></Author> , <Author><Surname>Schmincke</Surname> <Forename>H.-U</Forename></Author> , <PubDate><Year>1984</Year>.</PubDate> Alteration of Volcanic Glass, Pyroclastic Rocks. <ldlPublisherName>Springer Berlin Heidelberg</ldlPublisherName><ldlCity>Berlin</ldlCity>, <ldlCity>Heidelberg</ldlCity>, <PageRange> pp. 312-345.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPublisherName, sLocationName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 37;
                //<Author><Surname>Jackson</Surname> <Forename>M</Forename></Author> , <Author><Surname>Vola</Surname> <Forename>G</Forename></Author> , <Author><Surname>Všianský</Surname> <Forename>D</Forename></Author> , <Author><Surname>Oleson</Surname> <Forename>J</Forename></Author> , <Author><Surname>Scheetz</Surname> <Forename>B</Forename></Author> , <Author><Surname>Brandon</Surname> <Forename>C</Forename></Author> , <Author><Surname>Hohlfelder</Surname> <Forename>R</Forename></Author> , <PubDate><Year>2012</Year>.</PubDate> Cement Microstructures and Durability in Ancient Roman Seawater Concretes, in: <Editor><ESurname>Válek</ESurname> <EForename>J</EForename></Editor> , <Editor><ESurname>Hughes</ESurname> <EForename>J.J</EForename></Editor> , <Editor><ESurname>Groot</ESurname> <EForename>C.J.W.P</EForename></Editor> <ldlEditorDelimiterEds_Back>(Eds.),</ldlEditorDelimiterEds_Back> Historic Mortars. <ldlPublisherName>Springer Netherlands</ldlPublisherName>, <PageRange> pp. 49-76.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sTitleLabel, sAuthorEditorGroup, sUptoDotTitle, sPublisherName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5<{1}>$6</{1}>$7$8", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 38;
                //<Author><Surname>Taylor</Surname> <Forename>H.F.W</Forename></Author> , <PubDate><Year>1997</Year>.</PubDate> Cement Chemistry, <ldlEditionNumber>Second Edition.</ldlEditionNumber> <ldlPublisherName>Thomas Telford.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sEditionNumber, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 39;
                //<Author><Surname>ADAM</Surname> <Forename>A. A</Forename></Author> <PubDate><Year>2009</Year>.</PubDate> <i>Strength and Durability Properties of Alkali Activated Slag and Fly Ash-Based Geopolymer Concrete.</i> <ldlThesisKeyword>Doctor of Philosophy</ldlThesisKeyword>, <ldlPublisherName>RMIT University.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sThesisKeyword, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 40;
                //<Author><Surname>Anderson</Surname> <Forename>T. L</Forename></Author> <PubDate>(<Year>2017</Year>).</PubDate> Fracture mechanics: Fundamentals and applications, <ldlEditionNumber>fourth edition,</ldlEditionNumber> <ldlPublisherName>CRC Press</ldlPublisherName>, <ldlPublisherName>Taylor & Francis Group</ldlPublisherName>, <ldlCity>Boca Raton</ldlCity><ldlState>FL.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sEditionNumber, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 41;
                //<Collab>LaDOTD (Louisiana Department of Transportation and Development).</Collab> <PubDate>(<Year>2016</Year>).</PubDate> “Louisiana standard specifications for roads and bridges.” <ldlCity>Baton Rouge</ldlCity><ldlState>Louisiana</ldlState>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 42;
                //<Author><Surname>Bentur</Surname> <Forename>A</Forename></Author> , and <Author><Surname>Mindess</Surname> <Forename>S</Forename></Author> <PubDate>(<Year>2006</Year>).</PubDate> Fiber reinforced cementitious composites, <ldlPublisherName>CRC Press.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 43;
                //<Author><Surname>Soroushian</Surname> <Forename>P</Forename></Author> , and <Author><Surname>Lee</Surname> <Forename>C</Forename></Author> <PubDate>(<Year>1989</Year>).</PubDate> “Constitutive modeling of steel fiber reinforced concrete under direct tension and compression.” <ldlPublisherName>Elsevier Applied Science</ldlPublisherName>, 363-377.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4<{1}>$5</{1}>", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 44;
                //<Author><Surname>Doebling</Surname> <Forename>S.W</Forename></Author> , <Author><Surname>Farrar</Surname> <Forename>C.R</Forename></Author> , <Author><Surname>Prime</Surname> <Forename>M.B</Forename></Author> and <Author><Surname>Shevitz</Surname> <Forename>D.W</Forename></Author> <PubDate>(<Year>1996</Year>),</PubDate> “Damage identification and health monitoring of structural and mechanical systems from changes in their vibration characteristics: a literature review”, <ldlReportKeyword>Report No. LA-13070-MS</ldlReportKeyword>, <ldlPublisherName>Los Alamos National Laboratory</ldlPublisherName><ldlCity>Los Alamos</ldlCity><ldlState>NM.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sReportKeyword, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlSourceTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 45;
                //<Collab>FEMA</Collab> <PubDate>(<Year>2012</Year>),</PubDate> “Seismic performance assessment of buildings”, <ldlPublisherName>Federal Emergency Management Agency.</ldlPublisherName> <ldlReportKeyword>Report No. FEMA P-58</ldlReportKeyword>, <ldlCity>Washington</ldlCity><ldlState>D.C.</ldlState>, <ldlCountry>USA.</ldlCountry>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sPublisherName, sReportKeyword, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlSourceTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 46;
                //<Collab>Applied Technology Council (ATC).</Collab> <PubDate>(<Year>2005</Year>).</PubDate> “Improvement of inelastic seismic analysis procedures.” <ldlReportKeyword><i>Rep. FEMA-440</i></ldlReportKeyword>, <ldlCity>Washington</ldlCity><ldlState>DC</ldlState>, <ldlCountry>USA.</ldlCountry>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sReportKeyword, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlSourceTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 47;
                //<Author><Surname>Salsa</Surname> <Forename>D. E</Forename></Author> , and <Author><Surname>Salvadori</Surname> <Forename>J. E</Forename></Author> <PubDate>(<Year>2010</Year>).</PubDate> “Route 36 highlands bridge replacement.” <Website><http://aspirebridge.com/magazine/2010Summer>.</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 48;
                //<Author><Surname>Wanitkorkul</Surname> <Forename>A</Forename></Author> , and <Author><Surname>Filiatrault</Surname> <Forename>A</Forename></Author> <PubDate>(<Year>2005</Year>).</PubDate> “Simulation of strong ground motions for seismic fragility evaluation of nonstructural components in hospitals.” <ldlReportKeyword><i>Technical Rep. MCEER-05-0005</i></ldlReportKeyword>, Multidisciplinary Center for Earthquake Engineering Research, <ldlPublisherName>State Univ. of New York at Buffalo</ldlPublisherName>, <ldlState>N.Y.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sReportKeyword, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlSourceTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 49;
                //<Author><Surname>Fell</Surname> <Forename>B. V</Forename></Author> <PubDate><Year>2008</Year>.</PubDate> <i>Large-Scale Testing and Simulation of Earthquake-Induced Ultra Low Cycle Fatigue in Bracing Members Subjected to Cyclic Inelastic Buckling.</i> <ldlThesisKeyword>PhD Thesis</ldlThesisKeyword>, <ldlCity>Davis</ldlCity><ldlPublisherName>University of California Davis.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sThesisKeyword, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 50;
                //<Author><Surname>Chopra</Surname> <Forename>A. K</Forename></Author> <PubDate>(<Year>2007</Year>).</PubDate> <i>Dynamics of Structures</i>  <Vol_No>Vol. 4, </Vol_No><PageRange>p. 339)</PageRange>. <ldlCity>New Jersey</ldlCity><ldlPublisherName>Prentice Hall.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sVolumeNumber, sPageRange, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 51;
                //<Collab>International Federation for Structural Concrete (fib)</Collab> <PubDate>(<Year>2013</Year>).</PubDate> “Model Code for Concrete Structures 2010” <ldlPublisherName>Wiley-VCH Verlag GmbH & Co. KGaA.</ldlPublisherName> <ldlISBNNumber>Print ISBN: 9783433030615.</ldlISBNNumber>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sPublisherName, sISBNNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 52;
                //<Collab>AASHTO.</Collab> <PubDate>(<Year>2012</Year>).</PubDate> “LRFD bridge design specifications and commentary.” <ldlEditionNumber>6<sup>th</sup> Ed.,</ldlEditionNumber> <ldlCity>Washington</ldlCity><ldlState>DC.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sEditionNumber, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 53;
                //<Author><Surname>Wang</Surname> <Forename>S</Forename></Author> <PubDate>(<Year>2017</Year>).</PubDate> <i>Enhance seismic resilience of high-rise buildings with optimal design of supplemental energy dissipation devices</i>. <ldlThesisKeyword>(Unpublished doctoral thesis)</ldlThesisKeyword>. <ldlPublisherName>University of California</ldlPublisherName>, <ldlCity>Berkeley</ldlCity><ldlState>CA.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sThesisKeyword, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 53;
                //<Author><Surname>Dervilis</Surname> <Forename>N</Forename></Author> , <Author><Surname>Shi</Surname> <Forename>H</Forename></Author> , <Author><Surname>Worden</Surname> <Forename>K</Forename></Author> , and <Author><Surname>Cross</Surname> <Forename>E. J</Forename></Author> <PubDate>(<Year>2016</Year>).</PubDate> “Exploring environmental and operational variations in SHM data using heteroscedastic Gaussian processes.” <i>Dynamics of Civil Structures</i>, <Editor><EForename>S.</EForename> <ESurname>Pakzad</ESurname></Editor> , and <Editor><EForename>C.</EForename> <ESurname>Juan</ESurname></Editor> , <ldlEditorDelimiterEds_Back>eds.,</ldlEditorDelimiterEds_Back> <Vol_No> Vol. 2, </Vol_No><ldlPublisherName>Springer</ldlPublisherName><ldlCity>Cham</ldlCity>, 145-153.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sAuthorEditorGroup, sVolumeNumber, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7$8<{1}>$9</{1}>", "ldlChapterTitle", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 54;
                //<Author><Surname>Barsom</Surname> <Forename>J</Forename></Author> , and <Author><Surname>Rolfe</Surname> <Forename>S. T</Forename></Author> <PubDate>(<Year>1999</Year>).</PubDate> “Fracture and Fatigue Control in Structures – Applications of Fracture Mechanics.” <ldlPublisherName>American Society for Testing and Materials</ldlPublisherName>, <ldlEditionNumber>Third Edition.</ldlEditionNumber> <PageRange> 516 pp.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sPublisherName, sEditionNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 55;
                //<Collab>AOAC.</Collab> <PubDate><Year>1990</Year>.</PubDate> Official Method of Analysis. <ldlEditionNumber>15 th ed.</ldlEditionNumber> <ldlPublisherName>Association of Official Analytical Chemists</ldlPublisherName><ldlCity>Washington</ldlCity><ldlState>DC.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sEditionNumber, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 56;
                //<Collab>Canadian Dairy Information Centre.</Collab> <PubDate><Year>2014</Year>.</PubDate> Milk exchange quota 2014. <ldlURLLabel>Online</ldlURLLabel> <Website>http://www.dairyinfo.gc.ca/index_e.php?s1=dff-fcil&s2=quota&s3=qe-tq&s4=yr-an&page=2014</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 57;
                //<Author><Surname>Rathakrishnan</Surname> <Forename>E</Forename></Author> <i>Instrumentation, measurements, and experiments in fluids</i>, <ldlEditionNumber>2<sup>nd</sup> ed.</ldlEditionNumber> <ldlPublisherName>CRC Press</ldlPublisherName><ldlCity>Boca Raton</ldlCity><ldlState>FL</ldlState>, <PubDate><Year>2016</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sItalicTitle, sEditionNumber, sPublisherName, sLocationName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 58;
                //<Author><Surname>Abrate</Surname> <Forename>S</Forename></Author> <PubDate>(<Year>2011</Year>).</PubDate> <i>Impact engineering of composite structures</i>, <Vol_No> Vol. 526. </Vol_No><ldlPublisherName>Springer Science & Business Media.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sVolumeNumber, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 59;
                //<Author><Surname>Modarres</Surname> <Forename>M</Forename></Author> <PubDate>(<Year>2008</Year>).</PubDate> “Probabilistic risk assessment.” <i>Encyclopedia of Quantitative Risk Analysis and Assessment</i>, <Editor><EForename>E. L.</EForename> <ESurname>Melnick</ESurname></Editor> and <Editor><EForename>B. S.</EForename> <ESurname>Everitt</ESurname></Editor> , <ldlEditorDelimiterEds_Back>eds.,</ldlEditorDelimiterEds_Back> <ldlPublisherName>Wiley.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sAuthorEditorGroup, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "ldlBookTitle", "ldlChapterTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 60;
                //<Author><Surname>Balaguru</Surname> <Forename>P. N</Forename></Author> , and <Author><Surname>Shah</Surname> <Forename>S. P</Forename></Author> <PubDate>(<Year>1992</Year>).</PubDate> “<i>Fiber-reinforced cement composites.”</i> <ldlEditionNumber>First ed.</ldlEditionNumber> <ldlPublisherName>McGraw-Hill</ldlPublisherName><ldlCountry>US.</ldlCountry>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sEditionNumber, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 61;
                //<Author><Surname>Silverman</Surname> <Forename>B.W</Forename></Author> <PubDate>(<Year>1986</Year>).</PubDate> “Density estimation for statistics and data analysis.” <ldlState>New York</ldlState><ldlPublisherName>Chapman and Hall.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 62;
                //<Author><Surname>Hardjito</Surname> <Forename>D</Forename></Author> , and <Author><Surname>Rangan</Surname> <Forename>B. V</Forename></Author> <PubDate>(<Year>2005</Year>).</PubDate> “Development and Properties of Low-Calcium Fly Ash-based Geopolymer Concrete.” <ldlPublisherName>Curtin University of Technology</ldlPublisherName>, <ldlCity>Perth</ldlCity><ldlCountry>Australia</ldlCountry> 48.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5<{1}>$6</{1}>", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 63;
                //<Author><Surname>Yapp</Surname> <Forename>M. T</Forename></Author> , <Author><Surname>Durrani</Surname> <Forename>A. Z</Forename></Author> , and <Author><Surname>Finn</Surname> <Forename>F. N</Forename></Author> <PubDate>(<Year>1991</Year>).</PubDate> “HP-GPC and asphalt characterization literature review.” <i>Strategic Highway Research Program, Final Rep., No. SHRP-A/UIR-91-503</i>, <ldlPublisherName>National Research Council</ldlPublisherName><ldlCity>Washington</ldlCity><ldlState>D.C</ldlState>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "ldlBookTitle", "ldlChapterTitle"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 64;
                //<Author><Surname>West</Surname> <Forename>R</Forename></Author> , <Author><Surname>Willis</Surname> <Forename>J.R</Forename></Author> , and <Author><Surname>Marasteanu</Surname> <Forename>M</Forename></Author> <PubDate>(<Year>2013</Year>).</PubDate> “Improved mix design, evaluation, and materials management practices for hot mix asphalt with high reclaimed asphalt pavement content.” <i>NCHRP Report 752</i>, <ldlCity>Washington</ldlCity><ldlState>D.C</ldlState>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5", "ldlBookTitle", "ldlChapterTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 65;
                //<Author><Surname>Nicholson</Surname> <Forename>P</Forename></Author> <PubDate>(<Year>2014</Year>).</PubDate> Soil Improvement and Ground Modification Methods<Author><Surname>Nicholson</Surname> <Forename>P</Forename></Author> <PubDate>(<Year>2014</Year>).</PubDate> Soil Improvement and Ground Modification Methods, <ldlPublisherName>Butterworth and Heinemann</ldlPublisherName><ldlCity>Waltham</ldlCity><ldlState>MA</ldlState><ldlCountry>USA</ldlCountry>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5", "ldlBookTitle", "ldlChapterTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 65;
                //<Author><Surname>Movaghati</Surname> <Forename>S</Forename></Author> , and <Author><Surname>Rahai</Surname> <Forename>A</Forename></Author> <PubDate>(<Year>2012</Year>).</PubDate> “Eccentric compression loading test on steel HSS columns externally strengthened with CFRP composite materials” <ldlConferenceName>Proceedings, 6th International Conference on FRP Composite in Civil Engineering (CICE2012),</ldlConferenceName> <ldlCity>Rome</ldlCity><ldlCountry>Italy</ldlCountry>, <ldlConferenceDate>13-15 June.</ldlConferenceDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sLocationName, sConferenceDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 65;
                //<Author><Surname>Sayed-Ahmed</Surname> <Forename>E.Y</Forename></Author> <PubDate>(<Year>2004</Year>).</PubDate> “Strengthening of Thin-Walled Steel I-Section Beams Using CFRP Strips”. <ldlConferenceName>Proceedings, 4th International Conference on Advanced Composite Materials in Bridges and Structures (ACMBS IV),</ldlConferenceName> <ldlCity>Calgary</ldlCity><ldlState>Alberta</ldlState><ldlCountry>Canada</ldlCountry>, <ldlConferenceDate>20-23 July.</ldlConferenceDate> - CD Proceedings.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})(.*)$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sLocationName, sConferenceDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 66;
                //<Author><Surname>Gilber</Surname> <Forename>M</Forename></Author> <PubDate>(<Year>2007</Year>).</PubDate> “Limit analysis applied to masonry arch bridges: state-of-the-art and recent developments.” <ldlConferenceName>ARCH'07. 5TH Internationational Conference on Arch Bridges,</ldlConferenceName> <ldlCountry>Madeira</ldlCountry>, 13-28.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5<{1}>$6</{1}>", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 67;
                //<Author><Surname>Amhaz</Surname> <Forename>R</Forename></Author> , <Author><Surname>Chambon</Surname> <Forename>S</Forename></Author> , <Author><Surname>Idier</Surname> <Forename>J</Forename></Author> and <Author><Surname>Baltazart</Surname> <Forename>V</Forename></Author> <PubDate>(<Year>2014</Year>).</PubDate> “A new minimal path selection algorithm for automatic crack detection on pavement images.” <ldlConferenceName>Proc., International Conference on Image Processing,</ldlConferenceName> <ldlPublisherName>IEEE</ldlPublisherName><ldlCity>Piscataway</ldlCity><ldlState>NJ</ldlState>, 788-792.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6<{1}>$7</{1}>", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 68;
                //<Author><Surname>Maas</Surname> <Forename>A. L</Forename></Author> , <Author><Surname>Hannun</Surname> <Forename>A. Y</Forename></Author> and <Author><Surname>Ng</Surname> <Forename>A. Y</Forename></Author> <PubDate>(<Year>2013</Year>).</PubDate> “Rectifier nonlinearities improve neural network acoustic models.” <ldlConferenceName>Proc., the 30th International Conference on Machine Learning,</ldlConferenceName> <ldlPublisherName>International Machine Learning Society</ldlPublisherName><ldlCity>Atlanta</ldlCity><ldlCountry>Georgia</ldlCountry>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 69;
                //<Author><Surname>Freeman</Surname> <Forename>W. T</Forename></Author> , and <Author><Surname>Adelson</Surname> <Forename>E. H</Forename></Author> “Steerable filters for early vision, image analysis, and wavelet decomposition.” <ldlConferenceName>Proc., [1990] Proceedings Third International Conference on Computer Vision,</ldlConferenceName> 406-415.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sDoubleQuoteTitle, sConferenceName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3<{1}>$4</{1}>", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 69;
                //<Author><Surname>Zhou</Surname> <Forename>J</Forename></Author> , <Author><Surname>Huang</Surname> <Forename>P. S</Forename></Author> , and <Author><Surname>Chiang</Surname> <Forename>F.-P</Forename></Author> “Wavelet-aided pavement distress image processing.” <ldlConferenceName>Proc., Optical Science and Technology, SPIE's 48th Annual Meeting,</ldlConferenceName> <ldlPublisherName>SPIE</ldlPublisherName>, 12.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sDoubleQuoteTitle, sConferenceName, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4<{1}>$5</{1}>", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 70;
                //<Author><Surname>Kundu</Surname> <Forename>N</Forename></Author> , <Author><Surname>Pal</Surname> <Forename>M</Forename></Author> , and <Author><Surname>Saha</Surname> <Forename>S</Forename></Author> <PubDate>(<Year>2008</Year>).</PubDate> “East Kolkata Wetlands: a resource recovery system through productive activities.” in <ldlConferenceName>Proceedings of Taal 2007: The 12thWorld Lake Conference,</ldlConferenceName> <Editor><EForename>M.</EForename> <ESurname>Sengupta</ESurname></Editor> and <Editor><EForename>R.</EForename> <ESurname>Dalwani</ESurname></Editor> <ldlEditorDelimiterEds_Back>Eds.,</ldlEditorDelimiterEds_Back> <ldlCountry>India</ldlCountry>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sConferenceName, sAuthorEditorGroup, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7", "Article_Title", "ldlTitleLabel"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 71;
                //<Author><Surname>Royset</Surname> <Forename>J. O</Forename></Author> , and <Author><Surname>Rockafellar</Surname> <Forename>R. T</Forename></Author> <PubDate>(<Year>2015</Year>).</PubDate> “Risk measures in engineering design under uncertainty.” <ldlConferenceName>Proceedings of International Conference on Applications of Statistics and Probability in Civil Engineering (ICASP 2015).</ldlConferenceName></ldlConferenceDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 72;
                //<Author><Surname>Beecham</Surname> <Forename>S</Forename></Author> , <Author><Surname>Pezzaniti</Surname> <Forename>D</Forename></Author> , <Author><Surname>Myers</Surname> <Forename>B</Forename></Author> , <Author><Surname>Shackel</Surname> <Forename>B</Forename></Author> , and <Author><Surname>Pearson</Surname> <Forename>A</Forename></Author> <PubDate>(<Year>2009</Year>).</PubDate> “Experience in the application of permeable interlocking concrete paving in Australia.” <ldlConferenceName>9th International Conference on Concrete Block Paving,</ldlConferenceName> <ldlCity>Buenos Aires</ldlCity><ldlCountry>Argentina</ldlCountry>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 73;
                //<Author><Surname>Jovanovic</Surname> <Forename>S</Forename></Author> <PubDate>(<Year>2006</Year>).</PubDate> “Railway track quality assessment and related decision making”, <ldlConferenceName>Proc., AREMA,</ldlConferenceName> 202-230.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4<{1}>$5</{1}>", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 74;
                //<Author><Surname>Ahuja</Surname> <Forename>K.K</Forename></Author> and <Author><Surname>Brown</Surname> <Forename>W.H</Forename></Author> <PubDate>(<Year>1989</Year>).</PubDate> “Shear flow control by mechanical tabs.” <ldlConferenceName>Proc., AIAA, Shear Flow Conference,</ldlConferenceName> 2nd, <ldlCity>Tempe</ldlCity><ldlState>AZ</ldlState>, 89-0994.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})( [0-9]+(?:<sup>)?(?:st|nd|rd|th)(?:</sup>)?, )({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4<{1}>$5</{1}>$6<{2}>$7</{2}>", "Article_Title", "Vol_No", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 75;
                //<Author><Surname>Frye</Surname> <Forename>U</Forename></Author> , <Author><Surname>Ginger</Surname> <Forename>J</Forename></Author> , and <Author><Surname>Mullins</Surname> <Forename>P</Forename></Author> <PubDate>(<Year>2009</Year>).</PubDate> “Response of cladding to windborne debris impact.” <ldlConferenceName>The seventh Asia-Pacific conference on wind engineering.</ldlConferenceName> <ldlCity>Taipei</ldlCity><ldlCountry>Taiwan</ldlCountry><ldlPublisherName>Chinese Taiwan Association for Wind Engineering.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 76;
                //<Author><Surname>Yang</Surname> <Forename>D. Y</Forename></Author> , and <Author><Surname>Frangopol</Surname> <Forename>D. M</Forename></Author> <PubDate>(<Year>2017</Year>).</PubDate> “Risk-based bridge ranking considering transportation network performance.” <ldlConferenceName>Proceedings of the 12th International Conference on Structural Safety & Reliability (ICOSSAR 2017),</ldlConferenceName></ldlConferenceDate> <Editor><EForename>C.</EForename> <ESurname>Bucher</ESurname></Editor> , <Editor><EForename>B. R.</EForename> <ESurname>Ellingwood</ESurname></Editor> , and <Editor><EForename>D. M.</EForename> <ESurname>Frangopol</ESurname></Editor> , <ldlEditorDelimiterEds_Back>eds.,</ldlEditorDelimiterEds_Back> <ldlPublisherName>TU Verlag Vienna</ldlPublisherName><ldlCity>Vienna</ldlCity><ldlCountry>Austria</ldlCountry>, 358-366.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sAuthorEditorGroup, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7<{1}>$8</{1}>", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 77;
                //<Author><Surname>Modarres</Surname> <Forename>M</Forename></Author> <PubDate>(<Year>2008</Year>).</PubDate> “Probabilistic risk assessment.” Encyclopedia of Quantitative Risk Analysis and Assessment, <Editor><EForename>E. L.</EForename> <ESurname>Melnick</ESurname></Editor> and <Editor><EForename>B. S.</EForename> <ESurname>Everitt</ESurname></Editor> , <ldlEditorDelimiterEds_Back>eds.,</ldlEditorDelimiterEds_Back> <ldlPublisherName>Wiley.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sUptoCommaTitle, sAuthorEditorGroup, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 78;
                //<Author><Surname>Gardoni</Surname> <Forename>P</Forename></Author> , <Author><Surname>Guevara-Lopez</Surname> <Forename>F</Forename></Author> , and <Author><Surname>Contento</Surname> <Forename>A</Forename></Author> <PubDate>(<Year>2016</Year>).</PubDate> <Article_Title>“The life profitability method (LPM): A financial approach to engineering decisions.”</Article_Title> Structural Safety, <ldlPublisherName>Elsevier Ltd</ldlPublisherName>, 63, <PageRange>11–20.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sUptoCommaTitle, sPublisherName, sUnknownNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5<{1}>$6</{1}>$7", "Journal_Title", "Vol_No"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 79;
                //<Author><Surname>Nagatani</Surname> <Forename>K</Forename></Author> , <Author><Surname>Otake</Surname> <Forename>K</Forename></Author> , and <Author><Surname>Yoshida</Surname> <Forename>K</Forename></Author> <PubDate>(<Year>2014</Year>).</PubDate> Three-Dimensional Thermography Mapping for Mobile Rescue Robots. <ldlPublisherName>Springer Berlin Heidelberg</ldlPublisherName><ldlCity>Berlin</ldlCity>, <ldlCity>Heidelberg</ldlCity>, 49–63.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5<{1}>$6</{1}>", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 80;
                //<Author><Surname>Hiasa</Surname> <Forename>S</Forename></Author> <PubDate>(<Year>2016</Year>).</PubDate> “Investigation of infrared thermography for subsurface damage detection of concrete structures.” <ldlThesisKeyword>Ph.D. thesis</ldlThesisKeyword>, <ldlPublisherName>University of Central Florida</ldlPublisherName>, <ldlPublisherName>University of Central Florida</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sThesisKeyword, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 81;
                //<Author><Surname>Prakash</Surname> <Forename>T</Forename></Author> , <Author><Surname>Geo</Surname> <Forename>V. E</Forename></Author> , <Author><Surname>Martin</Surname> <Forename>L. J</Forename></Author> , and <Author><Surname>Nagalingam</Surname> <Forename>B</Forename></Author> <PubDate>(<Year>2018</Year>).</PubDate> “EFFECT OF TERNARY BLENDS OF BIO-ETHANOL, DIESEL AND CASTOR OIL ON PERFORMANCE, EMISSION AND COMBUSTION IN A CI ENGINE.” Renewable Energy, <Doi>doi: 10.1016/j.renene.2018.01.070.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sUptoCommaTitle, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 82;
                //<Author><Surname>Collins</Surname> <Forename>L</Forename></Author> , <Author><Surname>Alvarez</Surname> <Forename>D</Forename></Author> , and <Author><Surname>Chauhan</Surname> <Forename>A</Forename></Author> <PubDate>(<Year>2014</Year>).</PubDate> Microbial Biodegradation and Bioremediation, <Editor><EForename>S.</EForename> <ESurname>Das</ESurname></Editor> <ldlEditorDelimiterEds_Back>Ed.,</ldlEditorDelimiterEds_Back> <ldlPublisherName>Elsevier Inc.</ldlPublisherName><ldlCity>London</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sAuthorEditorGroup, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 83;
                //<Collab>ATSDR (Agency for Toxic Substances and Disease Registry).</Collab> <PubDate><Year>2017</Year>.</PubDate> ATSDR’s Substance Priority List <Website>https://www.atsdr.cdc.gov/spl/</Website> <ldlAccessedDateLabel>(accessed</ldlAccessedDateLabel> <ldlAccessedDate>28th April 2017).</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})([^0-9<>\(\)]{{5,}})({2})({3})$", sAuthorEditorGroup, sPubDate, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 84;
                //<Author><Surname>Yang</Surname> <Forename>D. Y</Forename></Author> , and <Author><Surname>Frangopol</Surname> <Forename>D. M</Forename></Author> <PubDate>(<Year>2017</Year>).</PubDate> “Risk-based bridge ranking considering transportation network performance.” <i><ldlConferenceName>Proceedings of the 12th International Conference on Structural Safety & Reliability (ICOSSAR 2017)</i>,</ldlConferenceName> <Editor><EForename>C.</EForename> <ESurname>Bucher</ESurname></Editor> , <Editor><EForename>B. R.</EForename> <ESurname>Ellingwood</ESurname></Editor> , and <Editor><EForename>D. M.</EForename> <ESurname>Frangopol</ESurname></Editor> , <ldlEditorDelimiterEds_Back>eds.,</ldlEditorDelimiterEds_Back> <ldlPublisherName>TU Verlag Vienna</ldlPublisherName><ldlCity>Vienna</ldlCity><ldlCountry>Austria</ldlCountry>, 358–366.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sAuthorEditorGroup, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7<{1}>$8</{1}>", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 84;
                //<Author><Surname>Frilot</Surname> <Forename>M</Forename></Author> , <Author><Surname>Klein</Surname> <Forename>L</Forename></Author> , & <Author><Surname>Hinton</Surname> <Forename>J</Forename></Author> <PubDate>(<Year>2008</Year>, <Month>March</Month> <Day>28</Day>).</PubDate> 2007 Revisions to the AIA A201. Construction News. <ldlAccessedDateLabel>Retrieved</ldlAccessedDateLabel> <ldlAccessedDate>October 31, 2017,</ldlAccessedDate> <ldlURLLabel>from</ldlURLLabel> <Website>https://www.bakerdonelson.com/2007-Revisions-to-the-AIA-A201-03-28-2008</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoDotTitle, sAccessedDateWithLabel, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 85;
                //<RefLabel>1.</RefLabel> <Collab>Professional Liability Agents Network (PLAN)</Collab> <PubDate><Year>(n.d.)</Year></PubDate> Delay Claims, RFIs and Change Orders. <ldlAccessedDateLabel>Retrieved</ldlAccessedDateLabel> <ldlAccessedDate>October 31, 2017,</ldlAccessedDate> <ldlURLLabel>from</ldlURLLabel> <Website>http://www.cavignac.com/delay-claims-rfis-and-change-orders/</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sAccessedDateWithLabel, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 86;
                //<Author><Surname>James</Surname> <Forename>W</Forename></Author> , and <Author><Surname>Gerrits</Surname> <Forename>C</Forename></Author> <PubDate>(<Year>2003</Year>).</PubDate> “Maintenance of infiltration in modular interlocking concrete pavers with external drainage cells.” <i>J. Water Manage. Model.</i>, <Doi>10.14796/JWMM.R215-22.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 87;
                //<Author><Surname>Melaragno</Surname> <Forename>M</Forename></Author> <PubDate>(<Year>2012</Year>).</PubDate> <i>An introduction to shell structures: The art and science of vaulting</i>. <ldlPublisherName>Springer Science & Business Media</ldlPublisherName><ldlCountry>U.S</ldlCountry>. <Doi>https://doi.org/10.1007/978-1-4757-0223-1.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sPublisherName, sLocationName, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 88;
                //<Author><Surname>Hafer</Surname> <Forename>R</Forename></Author> , <Collab>International Institute for Conflict Prevention & Resolution</Collab> <PubDate>(<Year>2010</Year>).</PubDate> Dispute Review Boards and Other Standing Neutrals. <ldlAccessedDateLabel>Retrieved</ldlAccessedDateLabel> <ldlAccessedDate>January 15, 2018,</ldlAccessedDate> <ldlURLLabel>from</ldlURLLabel> <Website>https://www.cpradr.org/resource-center/toolkits/construction-briefing-dispute-resolution-boards-other/_res/id=Attachments/index=0/CPR-Dispute-Review-Boards-and-Other-Standing-Neutrals-Constructiontitle.pdf</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sAccessedDateWithLabel, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 89;
                //<Author><Surname>Ami</Surname> <Forename>H. M</Forename></Author> <PubDate>(<Year>1928</Year>).</PubDate> Notes on the Adelaide Peninsula Skull. <ldlConferenceName><i>Proceedings and Transactions of the Royal Society of Canada,</i></ldlConferenceName> <i>22</i>, <PageRange>319–322.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})((?:<i>)?[0-9]+(?:</i>)?, )({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sConferenceName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4<{1}>$5</{1}>$6", "Article_Title", "Vol_No"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 90;
                //<Author><Surname>Pike</Surname> <Forename>I</Forename></Author> <PubDate>(<Year>2011</Year>).</PubDate> Injury prevention indicators for Inuit children and youth. <i>First Nations and Inuit Children and Youth Injury Indicators Working Group</i>. <ldlURLLabel>Retrieved from</ldlURLLabel> <Website>http://www.injuryresearch.bc.ca/docs/3_20101124_123305Inuit%20Injury%20Indicators%20-%20English.pdf</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sItalicTitle, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 91;
                //<Author><Surname>Huitema</Surname> <Forename>B. E</Forename></Author> <PubDate>(<Year>1980</Year>).</PubDate> <i>Analysis of Covariance</i>, Encyclopedia of Statistics in Behavioral Science, <ldlPublisherName>John Wiley & Sons, Ltd</ldlPublisherName><ldlCity>Hoboken</ldlCity><ldlState>New Jersey</ldlState>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sUptoCommaTitle, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 92;
                //<Author><Surname>Nelson</Surname> <Forename>L. J</Forename></Author> <PubDate>(<Year>2014</Year>).</PubDate> “Tall freeway spans will be relatively safe in quakes, Caltrans says.” <i>Los Angeles Times</i>, <Website><http://www.latimes.com/local/california/la-me-california-commute-20141125-story.html></Website> <ldlAccessedDate>(Jan. 10, 2018).</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 93;
                //<Author><Surname>Tornaghi</Surname> <Forename>R</Forename></Author> <PubDate>(<Year>1989</Year>).</PubDate> “Trattamento colonnare dei terreni mediante gettiniezione (jet-grouting)”. <ldlConferenceName><i>Proc., 14th Convegno Nazionale di Geotecnica</i>.</ldlConferenceName> <ldlCity>Taormina</ldlCity><ldlCountry>Italy</ldlCountry>, 193–203 <ldlMisc>(in Italian).</ldlMisc>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sLocationName, sUnknownNumber, sMisc);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5<{1}>$6</{1}>$7", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 94;
                //<Author><Surname>Bergschneider</Surname> <Forename>B</Forename></Author> <PubDate>(<Year>2002</Year>).</PubDate> <i>Zur Reichweite beim Düsenstrahlverfahren im Sand</i>, <ldlPublisherName>Shaker Verlag GmbH</ldlPublisherName><ldlCity>Herzogenrath</ldlCity><ldlCountry>Germany</ldlCountry> <ldlMisc>(in German).</ldlMisc>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sPublisherName, sLocationName, sMisc);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 95;
                //<Author><Surname>Barla</Surname> <Forename>G</Forename></Author> , <Author><Surname>Vai</Surname> <Forename>L</Forename></Author> <PubDate>(<Year>1999</Year>).</PubDate> Indagini geotecniche per la caratterizzazione del sottosuolo di Torino lungo il tracciato del Passante Ferroviario. <ldlConferenceName><i>Proc., 20th Congresso Nazionale di Geotecnica,</i></ldlConferenceName> <ldlCity>Parma</ldlCity><ldlCountry>Italy</ldlCountry>, 335-342 <ldlMisc>(In Italian).</ldlMisc>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sConferenceName, sLocationName, sUnknownNumber, sMisc);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5<{1}>$6</{1}>$7", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 96;
                //<Author><Surname>Panagoulia</Surname> <Forename>D</Forename></Author> , and <Author><Surname>Vlahogianni</Surname> <Forename>E</Forename></Author> <PubDate>(<Year>2014</Year>).</PubDate> “Nonlinear dynamics and recurrence analysis of extreme precipitation for observed and general circulation model generated climates” Hydrol. Process., <Doi>10.1002/hyp.9802.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sJournalTtlAbbrPatn, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 97;
                //<Author><Surname>Wekumbura</Surname> <Forename>C</Forename></Author> , <Author><Surname>Stastna</Surname> <Forename>J</Forename></Author> , and <Author><Surname>Zanzotto</Surname> <Forename>L</Forename></Author> <PubDate>(<Year>2007</Year>).</PubDate> “Destruction and recovery of internal structure in polymer-modified asphalts.” <i>Journal of Materials in Civil Engineering</i>, <Doi>10.1061/(ASCE)0899-1561(2007)19:3(227</Doi>), 227-232.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sDoiNumber, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>", "Article_Title", "Journal_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 98;
                //<Author><Surname>Sweet</Surname> <Forename>J</Forename></Author> , & <Author><Surname>Schneier</Surname> <Forename>M. M</Forename></Author> <PubDate>(<Year>2013</Year>).</PubDate> Legal aspects of architecture, engineering and the construction process. <ldlCity>Stamford</ldlCity><ldlState>CT</ldlState><ldlPublisherName>Cengage Learning.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 99;
                //<Author><Surname>Reddi</Surname> <Forename>L. N</Forename></Author> , <Author><Surname>Ming</Surname> <Forename>X</Forename></Author> , <Author><Surname>Hajra</Surname> <Forename>M. G</Forename></Author> , and <Author><Surname>Lee</Surname> <Forename>I. M</Forename></Author> <PubDate>(<Year>2000</Year>).</PubDate> <Article_Title>“Permeability reduction of soil filters due to physical clogging.”</Article_Title> J. Geotech. and Geoenviron. Eng., <Vol_No>126</Vol_No> <Issue_No>(3),</Issue_No> <PageRange>236-246.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})([A-Za-z \.,]{{5,}})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 100;
                //<Author><Surname>Reddi</Surname> <Forename>L. N</Forename></Author> , <Author><Surname>Ming</Surname> <Forename>X</Forename></Author> , <Author><Surname>Hajra</Surname> <Forename>M. G</Forename></Author> , and <Author><Surname>Lee</Surname> <Forename>I. M</Forename></Author> <PubDate>(<Year>2000</Year>).</PubDate> <Article_Title>“Permeability reduction of soil filters due to physical clogging.”</Article_Title> J. Geotech. and Geoenviron. Eng., <Vol_No>126</Vol_No> <Issue_No>(3),</Issue_No> <PageRange>236-246.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})([A-Za-z \.,]{{5,}})({4})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sJournalTitle, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3$4<{0}>$5</{0}>$6", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 101;
                //<Author><Surname>Reddi</Surname> <Forename>L. N</Forename></Author> , <Author><Surname>Ming</Surname> <Forename>X</Forename></Author> , <Author><Surname>Hajra</Surname> <Forename>M. G</Forename></Author> , and <Author><Surname>Lee</Surname> <Forename>I. M</Forename></Author> <PubDate>(<Year>2000</Year>).</PubDate> <Article_Title>“Permeability reduction of soil filters due to physical clogging.”</Article_Title> J. Geotech. and Geoenviron. Eng., <Vol_No>126</Vol_No> <Issue_No>(3),</Issue_No> <PageRange>236-246.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})([A-Za-z \.,]{{5,}})({4})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sJournalTitle, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3$4<{0}>$5</{0}>$6", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 102;
                //<Author><Surname>Liu</Surname> <Forename>X.M</Forename></Author> , <Author><Surname>Cao</Surname> <Forename>F.J</Forename></Author> , <Author><Surname>Wang</Surname> <Forename>L</Forename></Author> , <Etal>et al.</Etal> <PubDate>(<Year>2017</Year>).</PubDate> <Article_Title>“Investigation on road performances of mesoporous nano-silica modified asphalt binder.”</Article_Title> J. Wuhan Uni Tech-Mater. <Journal_Title>Sci.,</Journal_Title> <Vol_No>32</Vol_No> <Issue_No>(4),</Issue_No> <PageRange>691-700.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})([A-Z\-a-z \.,]{{5,}})({3})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sJournalTitle);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 103;
                //<Author><Surname>Lovett</Surname> <Forename>A</Forename></Author> <PubDate>(<Year>2017</Year>).</PubDate> Railroad decision support tools for track maintenance, <ldlThesisKeyword>PhD Thesis</ldlThesisKeyword>, <ldlPublisherName>Univ. Illinois</ldlPublisherName>, <ldlCountry>U.S.A.</ldlCountry>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sThesisKeyword, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 104;
                //<Author><Surname>Kimbell</Surname> <Forename>R</Forename></Author> , G., and <Author><Surname>Fies</Surname> <Forename>J</Forename></Author> <PubDate>(<Year>1953</Year>).</PubDate> “Two Typical Wood Frame Houses Exposed to Energy Released by Nuclear Fission.” <ldlPublisherName>National Lumber Manufacturers Association</ldlPublisherName><ldlCity>Washington</ldlCity><ldlState>D.C.</ldlState>, 16.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 105;
                //<Author><Surname>McKenna</Surname> <Forename>F</Forename></Author> , <Author><Surname>Scott</Surname> <Forename>M</Forename></Author> , and <Author><Surname>Fenves</Surname> <Forename>G</Forename></Author> , <PubDate>(<Year>2010</Year>).</PubDate> <Article_Title>“Nonlinear finite-element analysis software architecture using object composition.”</Article_Title> <Journal_Title>Journal of Computer Civil Engineering,</Journal_Title> <Doi>10.1061/(ASCE)CP.1943-5487.0000002,</Doi> 95–107.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sJournalTitle, sDoiNumber, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3$4$5<{0}>$6</{0}>", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 106;
                //<Author><Surname>Kwan</Surname> <Forename>M.-P</Forename></Author> , <Author><Surname>Liu</Surname> <Forename>D</Forename></Author> , and <Author><Surname>Vogliano</Surname> <Forename>J</Forename></Author> <PubDate>(<Year>2015</Year>).</PubDate> “Assessing dynamic exposure to air pollution.” Space-Time Integration in Geography and GIScience, <ldlPublisherName>Springer</ldlPublisherName>, 283-300.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sUptoCommaTitle, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>", "ldlChapterTitle", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 107;
                //<Author><Surname>Riisgaard</Surname> <Forename>B</Forename></Author> , <Author><Surname>Ngo</Surname> <Forename>T</Forename></Author> , <Author><Surname>Mendis</Surname> <Forename>P</Forename></Author> , <Author><Surname>Georgakis</Surname> <Forename>C. T</Forename></Author> , and <Author><Surname>Stang</Surname> <Forename>H</Forename></Author> <PubDate>(<Year>2007</Year>).</PubDate> “Dynamic increase factors for high performance concrete in compression using split Hopkinson pressure bar.” Fracture Mechanics of Concrete and Concrete Structures, <Editor><EForename>T.</EForename> <ESurname>Francis</ESurname></Editor> , <ldlEditorDelimiterEds_Back>ed.,</ldlEditorDelimiterEds_Back> <ldlPublisherName>Taylor & Francis</ldlPublisherName><ldlCity>Cataliana</ldlCity><ldlCountry>Italy</ldlCountry>, 4.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sUptoCommaTitle, sAuthorEditorGroup, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7<{2}>$8</{2}>", "ldlChapterTitle", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 108;
                //<Author><Surname>Randall</Surname> <Forename>P</Forename></Author> , <PubDate>(<Year>1955</Year>).</PubDate> “Damage to conventional and special types of residences exposed to nuclear effects.” <ldlCity>Battle Creek</ldlCity><ldlState>Michigan</ldlState>, 53.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4<{1}>$5</{1}>", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 109;
                //<Author><Surname>Makris</Surname> <Forename>N</Forename></Author> , and <Author><Surname>Zhang</Surname> <Forename>J</Forename></Author> <PubDate>(<Year>2004</Year>).</PubDate> <Article_Title>“Seismic response analysis of a highway overcrossing equipped with elastomeric bearings and fluid dampers.”</Article_Title> <Journal_Title>J. Struct. Eng.,</Journal_Title> <Doi>10.1061/(ASCE)0733-9445(2004)130:6(830</Doi>), 830–845.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sJournalTitle, sDoiNumber, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3$4<{0}>$5</{0}>", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 110;
                //<Collab>American Association of State Highway and Transportation Officials (AASHTO)</Collab> <PubDate>(<Year>2014</Year>).</PubDate> Guide Specifications for Seismic Isolation Design (4th Edition), <ldlCity>Washington</ldlCity><ldlState>DC.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 111;
                //<Author><Surname>Hagan</Surname> <Forename>M.T</Forename></Author> , <Author><Surname>Demuth</Surname> <Forename>H.B</Forename></Author> , <Author><Surname>Beale</Surname> <Forename>M.H</Forename></Author> and <Author><Surname>De Jesus</Surname> <Forename>O</Forename></Author> <PubDate>(<Year>2012</Year>).</PubDate> Neural network design, <ldlEditionNumber>2nd Ed.</ldlEditionNumber>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sEditionNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 112;
                //<Author><Surname>Lowe</Surname> <Forename>D. G</Forename></Author> <PubDate>(<Year>1999</Year>).</PubDate> “Object recognition from local scale-invariant features.” Computer vision, <ldlConferenceDate>1999.</ldlConferenceDate> <ldlConferenceName>The proceedings of the seventh IEEE international conference on,</ldlConferenceName> <Vol_No>Vol. 2,</Vol_No> <ldlPublisherName>Ieee</ldlPublisherName>, 1150–1157.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sUptoCommaTitle, sConferenceDate, sConferenceName, sVolumeNumber, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7$8<{2}>$9</{2}>", "Article_Title", "Journal_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 113;
                //<Author><Surname>Terzaghi</Surname> <Forename>K</Forename></Author> <PubDate>(<Year>1950</Year>).</PubDate> “Mechanism of Landslides.” <Editor><ESurname>In Paige</ESurname> <EForename>S</EForename></Editor> <ldlEditorDelimiterEds_Back>(ed.),</ldlEditorDelimiterEds_Back> Application of Geology to Engineering Practice (Berkey volume), <ldlPublisherName>Geological Society of America</ldlPublisherName><ldlCity>New York</ldlCity>, <Vol_No>1950,</Vol_No> <PageRange>83-123.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sAuthorEditorGroup, sUptoCommaTitle, sPublisherName, sLocationName, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4<{1}>$5</{1}>$6$7$8$9", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 114;
                //<Author><Surname>Abrate</Surname> <Forename>S</Forename></Author> <PubDate>(<Year>2011</Year>).</PubDate> Impact engineering of composite structures, <Vol_No>Vol. 526.</Vol_No> <ldlPublisherName>Springer Science & Business Media.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sVolumeNumber, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 115;
                //<Author><Surname>Song</Surname> <Forename>G</Forename></Author> , <Author><Surname>Gu</Surname> <Forename>H</Forename></Author> , <Author><Surname>Mo</Surname> <Forename>Y. L</Forename></Author> , <Author><Surname>Hsu</Surname> <Forename>T</Forename></Author> , <Author><Surname>Dhonde</Surname> <Forename>H</Forename></Author> , and <Author><Surname>Zhu</Surname> <Forename>R. R. H</Forename></Author> “Health monitoring of a concrete structure using piezoceramic materials.”, 108-119.
                sFullPattern = string.Format(@"^({0})({1})({2})$", sAuthorEditorGroup, sDoubleQuoteTitle, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 116;
                //<Author><Surname>Bocchini</Surname> <Forename>P</Forename></Author> , <Author><Surname>Saydam</Surname> <Forename>D</Forename></Author> , and <Author><Surname>Frangopol</Surname> <Forename>D. M</Forename></Author> <PubDate>(<Year>2013</Year>).</PubDate> <Article_Title>“Efficient, accurate, and simple Markov chain model for the life-cycle analysis of bridge groups.”</Article_Title> Structural Safety, <ldlPublisherName>Elsevier Ltd</ldlPublisherName>, <Vol_No>40,</Vol_No> <PageRange>51–64.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sUptoCommaTitle, sPublisherName, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 117;
                //<Author><Surname>Buades</Surname> <Forename>A</Forename></Author> , <Author><Surname>Coll</Surname> <Forename>B</Forename></Author> , and <Author><Surname>Morel</Surname> <Forename>J. M</Forename></Author> “A non-local algorithm for image denoising.” <ldlConferenceName>Proc., In Proc. of the 2005 IEEE Computer Society Conference on Computer Vision and Pattern Recognition (CVPR'05),</ldlConferenceName> 60-65 <Vol_No>vol. 62.</Vol_No>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sDoubleQuoteTitle, sConferenceName, sUnknownNumber, sVolumeNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3<{1}>$4</{1}>$5", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 118;
                //<Collab>Atlas Roofing Corporation</Collab> <PubDate>(<Year>2015</Year>).</PubDate> Nailable Insulation Guide. <ldlCity>Atlanta</ldlCity><ldlState>GA.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 119;
                //<Author><Surname>Ami</Surname> <Forename>H. M</Forename></Author> <PubDate>(<Year>1928</Year>).</PubDate> Notes on the Adelaide Peninsula Skull. <ldlConferenceName>Proceedings and Transactions of the Royal Society of Canada,</ldlConferenceName> <Vol_No>22,</Vol_No> <PageRange>319–322.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sConferenceName, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 120;
                //<Author><Surname>Stenton</Surname> <Forename>D. R</Forename></Author> <PubDate>(<Year>2015</Year>).</PubDate> 2014 Franklin Archaeology Project. <ldlReportNumber>Report on file,</ldlReportNumber> <ldlPublisherName>Department of Culture and Heritage</ldlPublisherName>, <ldlPublisherName>Heritage Division</ldlPublisherName><ldlCity>Igloolik</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sReportKeyword, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 121;
                //<Editor><ESurname>Herring,</ESurname> <EForename>Horace</EForename></Editor> and <Editor><EForename>Steve</EForename> <ESurname>Sorrell</ESurname></Editor> , <ldlEditorDelimiterEds_Front>eds.</ldlEditorDelimiterEds_Front> <i>Energy Efficiency and Sustainable Consumption: The Rebound Effect.</i> <ldlCity>Basinstoke</ldlCity><ldlCountry>UK</ldlCountry><ldlPublisherName>Palgrave Macmillan</ldlPublisherName>, <PubDate><Year>2009</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 122;
                //<Author><Surname>Levine</Surname> <Forename>R</Forename></Author> <Article_Title>“Finance and Growth: Theory and Evidence.”</Article_Title> <ldlTitleLabel>In</ldlTitleLabel> <Journal_Title><i>Handbook of Economic Growth</i></Journal_Title> <Vol_No>vol. 1,</Vol_No> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>P.</EForename> <ESurname>Aghion</ESurname></Editor> and <Editor><EForename>S.</EForename> <ESurname>Durlauf</ESurname></Editor> 865-934. <ldlCity>Amsterdam</ldlCity><ldlPublisherName>Elsevier North Holland.</ldlPublisherName> <PubDate><Year>2005</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sArticleTitle, sTitleLabel, sJournalTitle, sVolumeNumber, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6<{1}>$7</{1}>$8$9$10", "ldlTitleLabel", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 123;
                //<Author><Surname>McCraw</Surname> <Forename>T.K</Forename></Author> Prophet of Innovation: Joseph Schumpeter and Creative Destruction, <ldlCity>Cambridge</ldlCity><ldlPublisherName>Harvard University Press</ldlPublisherName>. <PubDate><Year>2007</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sUptoCommaTitle, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 124;
                //<Author><Surname>Rajan</Surname> <Forename>R</Forename></Author> and <Author><Forename>L.</Forename> <Surname>Zingales</Surname></Author> <i>Saving Capitalism from the Capitalists.</i> <ldlPublisherName>Random House</ldlPublisherName><ldlCity>New York</ldlCity>. <PubDate><Year>2003</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sItalicTitle, sPublisherName, sLocationName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 125;
                //<Author><Surname>Robinson</Surname> <Forename>E</Forename></Author> and <Author><Forename>M.</Forename> <Surname>Leising</Surname></Author> “Blythe Masters Tells Banks the Blockchain Changes Everything.” <PubDate><Year>2015</Year>.</PubDate> <Website>www.bloomberg.com/news/features/2015-09-01/blythe-masters-tells-banks-the-blockchain-changes-everything.</Website> <ldlAccessedDateLabel>Accessed</ldlAccessedDateLabel> <ldlAccessedDate>May 31, 2017.</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sDoubleQuoteTitle, sPubDate, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 126;
                //<Author><Surname>Arnason</Surname> <Forename>J</Forename></Author> “Creative Imagination.” In <i>Cornelis Castoriadis: Key Concepts,</i> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>S.</EForename> <ESurname>Adams</ESurname></Editor> , 43-52. <ldlCity>London</ldlCity><ldlPublisherName>Bloomsbury Academic</ldlPublisherName>, <PubDate><Year>2014</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sDoubleQuoteTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}><{2}>$4</{2}>$5<{3}>$6</{3}>$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 127;
                //<Author><Surname>Castoriadis,</Surname> <Forename>Cornelius.</Forename></Author> “Modern Capitalism and Revolution.” [1960] In <i>Cornelius Castoriadis: Political and Social Writings, Volume 2,</i> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>D.</EForename> <ESurname>Curtis</ESurname></Editor> , 226-315. <ldlCity>Minneapolis</ldlCity><ldlPublisherName>University of Minnesota Press</ldlPublisherName>, <PubDate><Year>1988</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sDoubleQuoteTitle, @"(?:\[[0-9]{4}[a-z]?\][\., ]+)", sTitleLabel, sItalicTitle, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3<{1}>$4</{1}><{2}>$5</{2}>$6<{3}>$7</{3}>$8$9$10", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 128;
                //<Author><Surname>Castoriadis,</Surname> <Forename>Cornelius.</Forename></Author> <i>The Imaginary Institution of Society</i> [1975]. <ldlCity>Cambridge</ldlCity><ldlPublisherName>MIT Press</ldlPublisherName>, <PubDate><Year>1998</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sItalicTitle, @"(?:\[[0-9]{4}[a-z]?\][\., ]+)", sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 129;
                //<Author><Surname>Escobar,</Surname> <Forename>Arturo</Forename></Author> . “Una Minga para el Posdesarrollo.” <i>América Latina en Movimiento</i> <Vol_No>445</Vol_No> <PubDate>(<Year>2009</Year>):</PubDate> <PageRange>24-30.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sDoubleQuoteTitle, sItalicTitle, sVolumeNumber, sPubDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5$6", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 130;
                //<Collab>Australian Bureau of Statistics (ABS)</Collab> <PubDate>(<Year>2013</Year>).</PubDate> <i>Personal safety survey 2012</i>. <ldlURLLabel>Retrieved from</ldlURLLabel> <Website>http://www.abs.gov.au/ausstats/abs@.nsf/mf/4906.0</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 131;
                //<Author><Surname>Chung</Surname> <Forename>D</Forename></Author> , & <Author><Surname>Wendt</Surname> <Forename>S</Forename></Author> <PubDate>(<Year>2015</Year>).</PubDate> Domestic violence against women: policy, practice and solutions in the Australian context. In <Editor><EForename>A.</EForename> <ESurname>Day</ESurname></Editor> & <Editor><EForename>E.</EForename> <ESurname>Fernandez</ESurname></Editor> <ldlEditorDelimiterEds_Back>(Eds.),</ldlEditorDelimiterEds_Back> <i>Preventing violence in Australia: Policy, practice and solutions.</i> <PageRange>(pp. 202-215)</PageRange>. <ldlCity>Annandale</ldlCity><ldlPublisherName>The Federation Press</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sPageRange, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 132;
                //<Author><Surname>Contreras</Surname> <Forename>M</Forename></Author> , <Author><Surname>Heilman</Surname> <Forename>B</Forename></Author> , <Author><Surname>Barker</Surname> <Forename>G</Forename></Author> , <Author><Surname>Singh</Surname> <Forename>A</Forename></Author> , <Author><Surname>Verma</Surname> <Forename>R</Forename></Author> , & <Author><Surname>Bloomfield</Surname> <Forename>J</Forename></Author> <PubDate>(<Year>2012</Year>).</PubDate> <i>Bridges to adulthood: Understanding the lifelong influence of men’s childhood experiences of violence.</i> <ldlPublisherName>International Centre for Research on Women</ldlPublisherName>. <ldlURLLabel>Retrieved from</ldlURLLabel> <Website>http://www.icrw.org/publications/bridges-adulthood</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sPublisherName, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 133;
                //<Author><Surname>Durrant</Surname> <Forename>J</Forename></Author> <PubDate>(<Year>2000</Year>).</PubDate> A generation without smacking: The impact of Sweden’s ban on physical punishment. <ldlPublisherName>Save the Children</ldlPublisherName>. <ldlURLLabel>Retrieved from:</ldlURLLabel> <Website>http://www.endcorporalpunishment.org/assets/pdfs/reference-documents/Durrant-GenerationwithoutSmacking-2000.pdf</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPublisherName, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 134;
                //<Author><Surname>Heise</Surname> <Forename>L</Forename></Author> <PubDate>(<Year>1998</Year>).</PubDate> Violence against women: An integrated, ecological framework. <i>Violence Against Women,</i> <Vol_No>4</Vol_No> <Issue_No>(3),</Issue_No> <PageRange>262-290.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sItalicTitle, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 135;
                //<Author><Surname>Mulroney</Surname> <Forename>J</Forename></Author> <PubDate>(<Year>2003</Year>).</PubDate> <i>Australian statistics on domestic violence</i>. Australian Domestic and Family Violence Clearinghouse Topic Paper 3. <ldlURLLabel>Retrieved from</ldlURLLabel> <Website>http://www.stvp.org.au/documents/Compendium/Statistical%20Data/Australian%20Domestic%20and%20Family%20Violence%20Clearinghouse%20Topic%20Paper.pdf</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sUptoDotTitle, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 136;
                //<Author><Surname>Straus</Surname> <Forename>M. A</Forename></Author> <PubDate>(<Year>1990</Year>).</PubDate> Ordinary violence, child abuse and wife beating: What do they have in common? In <Editor><EForename>M.A.</EForename> <ESurname>Straus</ESurname></Editor> & <Editor><EForename>R.J.</EForename> <ESurname>Gelles</ESurname></Editor> <ldlEditorDelimiterEds_Back>(Eds.),</ldlEditorDelimiterEds_Back> <i>Physical violence in American families: Risk factors and adaptations to violence in 8,145 families</i> <PageRange>(pp. 403-424)</PageRange> <ldlCity>New Brunswick</ldlCity><ldlState>NJ</ldlState><ldlPublisherName>Transaction.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoQuesMarkTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sPageRange, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 137;
                //<Author><Surname>Wilman</Surname> <Forename>H</Forename></Author> <PubDate><Year>1981</Year>.</PubDate> “Electron Diffraction by the Finch School.” In <i>Fifty Years of Electron Diffraction</i>, <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>Peter</EForename> <ESurname>Goodman</ESurname></Editor> , 164-175. <ldlCity>Dordrecht</ldlCity><ldlPublisherName>Springer.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6<{3}>$7</{3}>$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 138;
                //<Author><Surname>Veblen,</Surname> <Forename>Thorstein</Forename></Author> . The Instinct of Workmanship and the State of the Industrial Arts. <ldlCity>New Brunswick</ldlCity><ldlState>NJ</ldlState><ldlPublisherName>Transaction Books</ldlPublisherName>, <PubDate>[<Year>1914</Year>] <Year>1970</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sUptoDotTitle, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 139;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Lehmann,</Surname> <Forename>Martin</Forename></Author> , Bas de Leeuw and <Author><Forename>Eric</Forename> <Surname>Fehr</Surname></Author> .</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> <i>Circular Economy.</i> <ldlCity>Zurich</ldlCity><ldlCountry>Switzerland</ldlCountry><ldlPublisherName>Swiss Academy of Engineering Sciences</ldlPublisherName>, <PubDate><Year>2014</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 140;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Baker</Surname> <Forename>John H</Forename></Author></ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> <i>An Introduction to English Legal History,</i> <ldlEditionNumber><i>Fourth Edition</i></ldlEditionNumber>. <ldlPublisherName>Oxford</ldlPublisherName>: <ldlPublisherName>Oxford University Press</ldlPublisherName>, <PubDate><Year>2002</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sItalicTitle, sEditionNumber, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 141;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>McKinnon</Surname> <Forename>R.I</Forename></Author> “</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> Money and Capital in Economic Development. ” <ldlCity>Washington</ldlCity><ldlState>D.C.</ldlState>: <ldlPublisherName>Brookings Institution</ldlPublisherName>. <PubDate><Year>1973</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sDoubleQuoteTitle, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }



                nPatternId = 142;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Shiva,</Surname> <Forename>Vandana</Forename></Author> .</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> “Development, Ecology and Women.” In <i>Paradigms in Economic Development: Classic Perspectives, Critiques, and Reflections</i>, <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>Rajani</EForename> <ESurname>Kanth</ESurname></Editor> , <PageRange>pp. 243-253.</PageRange> <ldlCity>New York</ldlCity><ldlState>NY</ldlState><ldlPublisherName>M.E. Sharpe</ldlPublisherName>, <PubDate><Year>1994</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sDoubleQuoteTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sPageRange, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}><{2}>$4</{2}>$5$6$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 143;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Lugo,</Surname> <Forename>Yesenia</Forename></Author> .</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> “Facing an Economic Decline, Challenges Lie Ahead Bolivia.” <i>Global Risk Insights,</i> <PubDate><Month>April</Month> <Year>2015</Year>.</PubDate> <ldlURLLabel>Available at</ldlURLLabel> <Website>http://globalriskinsights.com/2015/04/facing-an-economic-decline-challenges-lie-ahead-forbolivia.</Website> <ldlAccessedDateLabel>Accessed</ldlAccessedDateLabel> <ldlAccessedDate>May 29, 2017.</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sDoubleQuoteTitle, sItalicTitle, sPubDate, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5$6", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 144;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Pratt,</Surname> <Forename>Mary Louise</Forename></Author> .</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> “Arts of Contact Zones.” <i>Profession</i> <PubDate>(<Year>1991</Year>):</PubDate> 33-40.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sDoubleQuoteTitle, sItalicTitle, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>", "ldlChapterTitle", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 145;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Kimmel</Surname> <Forename>P.D.</Forename></Author> , <Author><Forename>D.E.</Forename> <Surname>Kieso</Surname></Author> and <Author><Forename>J.J.</Forename> <Surname>Weygandt</Surname></Author></ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> <i>Financial Accounting, Tools for Business Decision Making</i>. <ldlCity>Hoboken</ldlCity><ldlPublisherName>John Wiley Publishers</ldlPublisherName>, <ldlEditionNumber>6<sup>th</sup> Edition.</ldlEditionNumber> <PubDate><Year>2010</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sEditionNumber, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 146;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Schumpeter</Surname> <Forename>J.A.</Forename></Author></ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> <i>Essays.</i> <Editor><EForename>R. V.</EForename> <ESurname>Clemence</ESurname></Editor> <ldlEditorDelimiterEds_Back>ed.,</ldlEditorDelimiterEds_Back> <ldlCity>New Brunswick</ldlCity><ldlState>NJ</ldlState><ldlPublisherName>Transactions Publishers.</ldlPublisherName> <PubDate>[<Year>1951</Year>] <Year>1991</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sItalicTitle, sAuthorEditorGroup, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 147;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Diamond,</Surname> <Forename>Jared</Forename></Author> .</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> “What Makes Countries Rich or Poor?” <i>The New York Review of Books</i>, <PubDate><Month>June</Month> <Day>8</Day>, <Year>2012</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sDoubleQuoteTitle, sItalicTitle, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 148;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Yeh,</Surname> <Forename>Kung Chia</Forename></Author> .</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> “China’s National Income, 1931–36.” In <i>Modern Chinese Economic History</i> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>C.M.</EForename> <ESurname>Hou</ESurname></Editor> and <Editor><EForename>T.S.</EForename> <ESurname>Yu</ESurname></Editor> <ldlEditorDelimiterEds_Back>eds.</ldlEditorDelimiterEds_Back> <ldlCity>Seattle</ldlCity>: <ldlPublisherName>University of Washington Press</ldlPublisherName>, <PubDate><Year>1979</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sDoubleQuoteTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}><{2}>$4</{2}>$5$6$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 149;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Juncker,</Surname> <Forename>Jean-Claude</Forename></Author> , <Author><Forename>Donald</Forename> <Surname>Tusk</Surname></Author> , <Author><Forename>Jeroen</Forename> <Surname>Dijsselbloem</Surname></Author> , <Author><Forename>Mario</Forename> <Surname>Draghi</Surname></Author> , and <Author><Forename>Martin</Forename> <Surname>Schulz</Surname></Author> .</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> <i>Completing Europe’s Economic and Monetary Union</i>, <PubDate><Month>June</Month> <Day>22</Day>, <Year>2015</Year>.</PubDate> <ldlPublisherName>European Commission</ldlPublisherName>. <ldlURLLabel>Available at</ldlURLLabel> <Website>https://ec.europa.eu/commission/sites/beta-political/files/5-presidents-report_en.pdf.</Website> <ldlAccessedDateLabel>Accessed</ldlAccessedDateLabel> <ldlAccessedDate>September 15, 2015.</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sItalicTitle, sPubDate, sPublisherName, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 150;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Castoriadis,</Surname> <Forename>Cornelius</Forename></Author> ,</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> <PubDate><Year>1988c</Year>,</PubDate> “Reflections on ~~in_ldquo~~Rationality~~in_rdquo~~ and ~~in_ldquo~~Development~~in_rdquo~~ ” [1988c]. In <i>Cornelius Castoriadis: Philosophy, Politics, Autonomy,</i> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>D.</EForename> <ESurname>Curtis</ESurname></Editor> , 175-198. <ldlCity>Oxford</ldlCity>: <ldlPublisherName>Oxford University Press</ldlPublisherName>, 1991.
                sFullPattern = string.Format(@"^({0})({1})({2}\[[0-9]{{4}}[a-z]?\]\. )({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8$9$10", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 151;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Curtis</Surname> <Forename>D.</Forename></Author></ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> “Foreword.” In <i>The Castoriadis Reader</i> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>D.</EForename> <ESurname>Curtis</ESurname></Editor> , vii-xv. <ldlCity>Blackwell</ldlCity><ldlPublisherName>Oxford</ldlPublisherName>, <PubDate><Year>1997</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2}\[[0-9]{{4}}[a-z]?\]\. )({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sDoubleQuoteTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }



                //added by Dakshinamoorthy on 2019-Jan-05
                nPatternId = 152;
                //<RefLabel>[3]</RefLabel> <Author><Forename>W. N.</Forename> <Surname>Sharaman</Surname></Author> , <Author><Forename>R. W.</Forename> <Surname>Birkmire</Surname></Author> , <Author><Forename>S.</Forename> <Surname>Marsillac</Surname></Author> , <Author><Forename>M.</Forename> <Surname>Marudachalam</Surname></Author> , <Author><Forename>N.</Forename> <Surname>Orbey</Surname></Author> , <Author><Forename>T. W. F.</Forename> <Surname>Russell</Surname></Author> , 1997 IEEE 26th PVSC, <ldlCity>Anaheim</ldlCity><ldlState>CA</ldlState><ldlCountry>USA</ldlCountry>, <PubDate><Month>January</Month> <Year>1997</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sUptoCommaTitle, sLocationName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Jan-05
                nPatternId = 153;
                //<RefLabel>[11].</RefLabel> <Author><Forename>G.I.</Forename> <Surname>Eskin</Surname></Author> , <Author><Forename>D.G.</Forename> <Surname>Eskin</Surname></Author> , Ultrasonic Treatment of Light Alloy Melts, <ldlEditionNumber>2<sup>nd</sup> Edition,</ldlEditionNumber> <ldlPublisherName>CRC Press</ldlPublisherName>, <PubDate><Year>2014</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sUptoCommaTitle, sEditionNumber, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                //added by Dakshinamoorthy on 2019-Jan-05
                nPatternId = 154;
                //<RefLabel>[49].</RefLabel> <Author><Forename>J.A.</Forename> <Surname>Dantzig</Surname></Author> and <Author><Forename>M.</Forename> <Surname>Rappaz</Surname></Author> Solidification <ldlEditionNumber>2<sup>nd</sup> edition.</ldlEditionNumber> <ldlCity>Lausanne</ldlCity><ldlPublisherName>EPFL Press</ldlPublisherName> <PubDate><Year>2016</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, "(?:[A-Za-z ]{5,})", sEditionNumber, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                //added by Dakshinamoorthy on 2019-Jan-12
                nPatternId = 155;
                //<RefLabel>[21]</RefLabel> <Author><Forename>D.</Forename> <Surname>Bai</Surname></Author> , <Author><Forename>J.</Forename> <Surname>Zhang</Surname></Author> , <Author><Forename>Z.</Forename> <Surname>Jin</Surname></Author> , <Author><Forename>H.</Forename> <Surname>Bian</Surname></Author> , <Author><Forename>K.</Forename> <Surname>Wang</Surname></Author> , <Author><Forename>H.</Forename> <Surname>Wang</Surname></Author> , <Author><Forename>L.</Forename> <Surname>Liang</Surname></Author> , <Author><Forename>Q.</Forename> <Surname>Wang</Surname></Author> , <Author><Forename>S. F.</Forename> <Surname>Liu</Surname></Author> , <i>ACS Energy Lett.</i> <PubDate><Year>2018</Year>,</PubDate> 970.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sItalicTitle, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3<{1}>$4</{1}>", "Journal_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Jan-12
                nPatternId = 156;
                //<RefLabel>[72]</RefLabel> <Author><Forename>A.</Forename> <Surname>Matuszewska</Surname></Author> , <Author><Forename>T.</Forename> <Surname>Strek</Surname></Author> , Vibr. Phys. Syst. <PubDate><Year>2018</Year>,</PubDate> 29.
                sFullPattern = string.Format(@"^({0})({1}[\.;, ]*)({2})({3})$", sAuthorEditorGroup, sJournalTtlAbbrPatn, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3<{1}>$4</{1}>", "Journal_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }


                //added by Dakshinamoorthy on 2019-Jan-21
                nPatternId = 157;
                //<Author><Surname>Muller</Surname> <Forename>M.</Forename></Author> <PubDate><Year>1999</Year>.</PubDate> “Interbasin water sharing: A South African perspective.” <ldlConferenceName>In International Hydrological Program V: Proceedings of the International Workshop,</ldlConferenceName> 61–70. <ldlCity>Paris</ldlCity><ldlPublisherName>UNESCO.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3$4<{0}>$5</{0}>$6$7", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Jan-30
                nPatternId = 158;
                //<Author><Surname>Seed</Surname> <Forename>H.B.</Forename></Author> , <Author><Surname>Woodward</Surname> <Forename>R.J.</Forename></Author> and <Author><Surname>Lundgren</Surname> <Forename>R.</Forename></Author> <PubDate>(<Year>1962b</Year>).</PubDate> Prediction of swelling potential for compacted clays. <ldlEditorDelimiterEds_Front>Discussion by</ldlEditorDelimiterEds_Front> <Editor><ESurname>Praszker</ESurname> <EForename>M.</EForename></Editor> , <Editor><ESurname>Zolkov</ESurname> <EForename>E.</EForename></Editor> , <Editor><ESurname>Li</ESurname> <EForename>M.C.</EForename></Editor> and <Editor><ESurname>Finn</ESurname> <EForename>W.D.</EForename></Editor> <i>ASCE J. Soil Mech. and Foundations Div.,</i> <Vol_No>88</Vol_No> <Issue_No>(6),</Issue_No> <PageRange>263-270.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sAuthorEditorGroup, sItalicTitle, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4<{1}>$5</{1}>$6$7$8", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }


                //added by Dakshinamoorthy on 2019-Jan-30
                nPatternId = 159;
                //<Author><Surname>Peirson</Surname> <Forename>W.L.</Forename></Author> <PubDate>(<Year>2018</Year>).</PubDate> <i>Flow induced shear stresses in cracks</i>. Water Research Laboratory Research Report 265. <ldlPublisherName>UNSW Australia</ldlPublisherName>. <ldlISBNNumber>ISBN 9780733438059.</ldlISBNNumber>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sUptoDotTitle, sPublisherName, sISBNNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Jan-31
                nPatternId = 160;
                //<Author><Surname>Hauser</Surname> <Forename>J. R.</Forename></Author> , and <Author><Forename>D.</Forename> <Surname>Clausing</Surname></Author> “The House of Quality.” <i>Harvard Business Review</i> <PubDate>(<Month>May–June</Month> <Year>1988</Year>),</PubDate> <PageRange>pp. 63–73.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sDoubleQuoteTitle, sItalicTitle, sPubDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                //added by Dakshinamoorthy on 2019-Feb-01
                nPatternId = 161;
                //<RefLabel>[35]</RefLabel> <Author><Forename>J. J.</Forename> <Surname>Slotine</Surname></Author> and <Author><Forename>W.</Forename> <Surname>Li</Surname></Author> , <i>Applied Nonlinear Control</i> (<ldlPublisherName>Prentice Hall</ldlPublisherName><ldlCity>New Jersey</ldlCity><ldlCountry>USA</ldlCountry>, <PubDate><Year>1991</Year>).</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sItalicTitle, sPublisherName, sLocationName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Feb-06
                nPatternId = 162;
                //<Author><Surname>Rossman</Surname> <Forename>L. A.</Forename></Author> , and <Author><Forename>J. E.</Forename> <Surname>Van Zyl</Surname></Author> <PubDate><Year>2010</Year>.</PubDate> “The open sourcing of EPANET.” In <ldlConferenceName>Proc., Water Distribution Systems Analysis 2010,</ldlConferenceName></ldlConferenceDate> 19–28.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sConferenceName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>", "Article_Title", "ldlTitleLabel", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Feb-06
                nPatternId = 163;
                //<Author><Surname>Giustolisi</Surname> <Forename>O.</Forename></Author> , <Author><Forename>D. A.</Forename> <Surname>Savic</Surname></Author> , <Author><Forename>L.</Forename> <Surname>Berardi</Surname></Author> , and <Author><Forename>D.</Forename> <Surname>Laucelli</Surname></Author> <PubDate><Year>2011b</Year>.</PubDate> “An Excel-based solution to bring water distribution network analysis closer to users.” In <ldlConferenceName>Vol. 3 of Proc., Computer and Control in Water Industry (CCWI),</ldlConferenceName> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>D.A.</EForename> <ESurname>Savić</ESurname></Editor> , <Editor><EForename>Z.</EForename> <ESurname>Kapelan</ESurname></Editor> , and <Editor><EForename>D.</EForename> <ESurname>Butler</ESurname></Editor> , 805–810. <ldlCity>Exeter</ldlCity><ldlCountry>UK.</ldlCountry>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sConferenceName, sAuthorEditorGroup, sUnknownNumber, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3$4$5$6<{0}>$7</{0}>$8", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Feb-06
                nPatternId = 164;
                //<Author><Surname>Oh</Surname> <Forename>E. H.</Forename></Author> , <Author><Surname>Deshmukh</Surname> <Forename>A.</Forename></Author> , and <Author><Surname>Hastak</Surname> <Forename>M.</Forename></Author> <PubDate>(<Year>2010</Year>).</PubDate> “Vulnerability Assessment of Critical Infrastructure, Associated Industries, and Communities during Extreme Events.” <i>Construction Research Congress 2010</i>, <ldlPublisherName>American Society of Civil Engineers</ldlPublisherName><ldlCity>Reston</ldlCity><ldlState>VA</ldlState>, 449–469.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6<{2}>$7</{2}>", "ldlChapterTitle", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Feb-08
                nPatternId = 165;
                //<Author><Surname>Yang</Surname> <Forename>D. Y.</Forename></Author> , and <Author><Surname>Frangopol</Surname> <Forename>D. M.</Forename></Author> <PubDate>(<Year>2017</Year>).</PubDate> “Risk-based bridge ranking considering transportation network performance.” <ldlConferenceName>Proceedings of the 12th International Conference on Structural Safety & Reliability (ICOSSAR 2017),</ldlConferenceName></ldlConferenceDate> <Editor><EForename>C.</EForename> <ESurname>Bucher</ESurname></Editor> , <Editor><EForename>B. R.</EForename> <ESurname>Ellingwood</ESurname></Editor> , and <Editor><EForename>D. M.</EForename> <ESurname>Frangopol</ESurname></Editor> , <ldlEditorDelimiterEds_Back>eds.,</ldlEditorDelimiterEds_Back> <ldlPublisherName>TU Verlag Vienna</ldlPublisherName><ldlCity>Vienna</ldlCity><ldlCountry>Austria</ldlCountry>, 358–366.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sConferenceName, sAuthorEditorGroup, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7<{1}>$8</{1}>", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Feb-08
                nPatternId = 166;
                //<RefLabel>19.</RefLabel> <Author><Forename>G.</Forename> <Surname>Agrawal</Surname></Author> , <i>Nonlinear fiber optics</i> (<ldlPublisherName>Academic press</ldlPublisherName>, <PubDate><Year>2007</Year>).</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sItalicTitle, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                //added by Dakshinamoorthy on 2019-Mar-06
                nPatternId = 167;
                //<Author><Surname>Amarakoon</Surname> <Forename>I. I.</Forename></Author> , <Author><Forename>C. L.</Forename> <Surname>Hamilton</Surname></Author> , <Author><Forename>S. A.</Forename> <Surname>Mitchell</Surname></Author> , <Author><Forename>P. F.</Forename> <Surname>Tennat</Surname></Author> , and <Author><Forename>M.E.</Forename> <Surname>Roye</Surname></Author> <PubDate><Year>2017</Year>.</PubDate> Biotechnology. In <i>Pharmacognosy</i>, <ldlEditorDelimiterEds_Front>ed.</ldlEditorDelimiterEds_Front> <Editor><EForename>S.</EForename> <ESurname>Badal</ESurname></Editor> and <Editor><EForename>R.</EForename> <ESurname>Delgoda</ESurname></Editor> , 549-63. <ldlCity>Tokyo</ldlCity><ldlPublisherName>Academic press.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6<{3}>$7</{3}>$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Mar-06
                nPatternId = 168;
                //<Author><Surname>Augustin</Surname> <Forename>M. A.</Forename></Author> , and <Author><Forename>C. M.</Forename> <Surname>Oliver</Surname></Author> <PubDate><Year>2012</Year>.</PubDate> An overview of the development and applications of nanoscale materials in the food industry. In. <i>Nanotechnology in the food, beverage and nutraceutical industries</i>, <ldlEditorDelimiterEds_Front>ed.</ldlEditorDelimiterEds_Front> <Editor><EForename>Q.</EForename> <ESurname>Huang</ESurname></Editor> <ldlCity>New Delhi</ldlCity><ldlPublisherName>Woodhead Publishing.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Mar-06
                nPatternId = 169;
                //<Author><Surname>Dashek</Surname> <Forename>W. V.</Forename></Author> <PubDate><Year>2017</Year>.</PubDate> An introduction to cells and their organelles. In Plant cells and their organelles, <ldlEditorDelimiterEds_Front>ed.</ldlEditorDelimiterEds_Front> <Editor><EForename>W. V.</EForename> <ESurname>Dashek</ESurname></Editor> and <Editor><EForename>G. S.</EForename> <ESurname>Miglani</ESurname></Editor> , 1-24. <ldlCity>United Kingdom</ldlCity><ldlPublisherName>John Wiley and Sons, Ltd.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoCommaTitle, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8", "ldlChapterTitle", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Mar-06
                nPatternId = 170;
                //<Author><Surname>Post</Surname> <Forename>M.</Forename></Author> , and <Author><Forename>C.</Forename> <Surname>Weele</Surname></Author> <PubDate><Year>2014</Year>.</PubDate> Principles of tissue engineering for food. <i>In Principles of tissue engineering</i>, <ldlEditorDelimiterEds_Front>ed.</ldlEditorDelimiterEds_Front> <Editor><EForename>R.</EForename> <ESurname>Lanza</ESurname></Editor> , <Editor><EForename>R.</EForename> <ESurname>Langer</ESurname></Editor> and <Editor><EForename>J.</EForename> <ESurname>Vacanti</ESurname></Editor> , 1647-662. <ldlCity>Tokyo</ldlCity><ldlPublisherName>Academic Press</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sItalicTitle, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8", "ldlChapterTitle", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Mar-20
                nPatternId = 171;
                //<Author><Surname>Raftery</Surname> <Forename>A.</Forename></Author> , <Author><Surname>Hoeting</Surname> <Forename>J.</Forename></Author> , <Author><Surname>Volinsky</Surname> <Forename>P.</Forename></Author> , <Author><Surname>Painter</Surname> <Forename>I.</Forename></Author> , & <Author><Surname>Yeung</Surname> <Forename>K. Y.</Forename></Author> <PubDate>(<Year>2006</Year>).</PubDate> BMA: Bayesian Model Averaging R package version 3.03. <ldlURLLabel>Retrieved from</ldlURLLabel> <Website>http://www.r-project.org,</Website> <Website>http://www.research.att.com/~volinsky/bma.html.</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Apr-24
                nPatternId = 171;
                //<Author><Surname>Snitz</Surname> <Forename>B. E.</Forename></Author> , <Author><Surname>Weissfeld</Surname> <Forename>L. A.</Forename></Author> , <Author><Surname>Cohen</Surname> <Forename>A. D.</Forename></Author> , <Author><Surname>Lopez</Surname> <Forename>O. L.</Forename></Author> , <Author><Surname>Nebes</Surname> <Forename>R. D.</Forename></Author> , <Author><Surname>Aizenstein</Surname> <Forename>H. J.</Forename></Author> , <Author><Surname>Mcdade</Surname> <Forename>E.</Forename></Author> , <Author><Surname>Price</Surname> <Forename>J. C.</Forename></Author> , <Author><Surname>Mathis</Surname> <Forename>C. A.</Forename></Author> & <Author><Surname>Klunk</Surname> <Forename>W. E.</Forename></Author> <PubDate><Year>2015</Year>.</PubDate> Subjective Cognitive Complaints, Personality and Brain Amyloid-beta in Cognitively Normal Older Adults. <i>Am J Geriatr Psychiatry</i>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sItalicTitle);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }


                //added by Dakshinamoorthy on 2019-May-11
                nPatternId = 172;
                //<Author><Surname>Frilot</Surname> <Forename>M.</Forename></Author> , <Author><Surname>Klein</Surname> <Forename>L.</Forename></Author> , & <Author><Surname>Hinton</Surname> <Forename>J.</Forename></Author> <PubDate>(<Year>2008</Year>, <Month>March</Month> <Day>28</Day>).</PubDate> 2007 Revisions to the AIA A201. <ldlPublisherName>Construction News</ldlPublisherName>. <ldlAccessedDateLabel>Retrieved</ldlAccessedDateLabel> <ldlAccessedDate>October 31, 2017,</ldlAccessedDate> <ldlURLLabel>from</ldlURLLabel> <Website>https://www.bakerdonelson.com/2007-Revisions-to-the-AIA-A201-03-28-2008.</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPublisherName, sAccessedDateWithLabel, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-10
                nPatternId = 173;
                //<Author><Surname>Atasu,</Surname> <Forename>Atalay</Forename></Author> , and <Author><Forename>Luk</Forename> <Surname>Wassenhove</Surname></Author> . “Getting to Grips with Take-back Laws.” IESE Insight <PubDate>(<Month>July</Month>/ <Month>August</Month> <Year>2011</Year>):</PubDate> 29–35.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sDoubleQuoteTitle, "(?:[A-Za-z ]{5,})", sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>", "ldlChapterTitle", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-10
                nPatternId = 174;
                //<Author><Surname>Hauser</Surname> <Forename>J. R.</Forename></Author> , and <Author><Forename>D.</Forename> <Surname>Clausing</Surname></Author> “The House of Quality.” <ldlPublisherName>Harvard Business Review</ldlPublisherName> <PubDate>(<Month>May–June</Month> <Year>1988</Year>):</PubDate> 63–73.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sDoubleQuoteTitle, sPublisherName, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4<{1}>$5</{1}>", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-10
                nPatternId = 175;
                //<Author><Surname>Jefferies</Surname> <Forename>M.</Forename></Author> , and <Author><Surname>Been</Surname> <Forename>K.</Forename></Author> <PubDate>(<Year>2016</Year>).</PubDate> Soil Liquefaction, A Critical State Approach <ldlEditionNumber>(2nd edition)</ldlEditionNumber>. <ldlPublisherName>CRC Press</ldlPublisherName>, <ldlCity>Boca Raton</ldlCity>, <PageRange>690 p.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, @"(?:[^0-9\.\(\)\[\]]{5,})", sEditionNumber, sPublisherName, sLocationName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-10
                nPatternId = 176;
                //<Author><Surname>McClintock</Surname> <Forename>F.</Forename></Author> <PubDate>(<Year>1860</Year>).</PubDate> The voyage of the ‘Fox’ in the Arctic seas. A narrative of the discovery of the fate of Sir John Franklin and his companions <ldlEditionNumber>(1st edn)</ldlEditionNumber>. <ldlCity>London</ldlCity><ldlPublisherName>John Murray.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, @"(?:[^0-9\.\(\)\[\]]{5,})", sEditionNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-11
                nPatternId = 177;
                //<Author><Surname>Futrelle</Surname> <Forename>D.</Forename></Author> <PubDate>(<Year>2006</Year>, <Month>August</Month>).</PubDate> Can money buy happiness? <i>Money</i>, <PageRange>pp. 127–131.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoQuesMarkTitle, sItalicTitle, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-11
                nPatternId = 178;
                //<Author><Surname>Groome</Surname> <Forename>D.</Forename></Author> <PubDate>(<Year>2016</Year>).</PubDate> Astrology. In <Editor><EForename>D.</EForename> <ESurname>Groome</ESurname></Editor> , & <Editor><EForename>R.</EForename> <ESurname>Roberts</ESurname></Editor> <ldlEditorDelimiterEds_Back>(Eds.),</ldlEditorDelimiterEds_Back> <i>Parapsychology</i> <PageRange>(pp. 129–143)</PageRange>. <ldlPublisherName>Psychology Press</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sPageRange, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-11
                nPatternId = 179;
                //<Author><Surname>Lewis</Surname> <Forename>D. M.</Forename></Author> , <Author><Surname>Al-Shawaf</Surname> <Forename>L.</Forename></Author> , <Author><Surname>Russell</Surname> <Forename>E. M.</Forename></Author> , & <Author><Surname>Buss</Surname> <Forename>D. M.</Forename></Author> <PubDate>(<Year>2015</Year>).</PubDate> Friends and happiness: An evolutionary perspective on friendship. In <Editor><EForename>M.</EForename> <ESurname>Demir</ESurname></Editor> <ldlEditorDelimiterEds_Back>(Ed.),</ldlEditorDelimiterEds_Back> <i>Friendship and happiness</i> <PageRange>(pp. 37–57)</PageRange>. <ldlPublisherName>Springer</ldlPublisherName><ldlCity>Dordrecht</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sPageRange, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-11
                nPatternId = 180;
                //<Author><Surname>Smith</Surname> <Forename>A.</Forename></Author> , & <Author><Surname>Anderson</Surname> <Forename>M.</Forename></Author> <PubDate>(<Year>2018</Year>).</PubDate> Social media use in 2018. <ldlPublisherName>Pew Research Center</ldlPublisherName>. <ldlURLLabel>Retrieved from</ldlURLLabel> <Website>http://www.pewinternet.org/2018/03/01/social-media-use-in-2018/</Website> <ldlAccessedDate>(July 30, 2018).</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPublisherName, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-11
                nPatternId = 181;
                //<Author><Surname>D’Zurilla</Surname> <Forename>T. J.</Forename></Author> , & <Author><Surname>Nezu</Surname> <Forename>A. M.</Forename></Author> <PubDate>(<Year>2010</Year>).</PubDate> Problem-solving therapy. In <Editor><EForename>K. S.</EForename> <ESurname>Dobson</ESurname></Editor> <ldlEditorDelimiterEds_Back>(Ed.),</ldlEditorDelimiterEds_Back> <i>Handbook of cognitive-behavioral therapies</i>, <ldlEditionNumber><i>3</i><sup>rd</sup> ed.,</ldlEditionNumber> <PageRange>(pp. 197–225)</PageRange>. <ldlCity>New York</ldlCity><ldlPublisherName>Guilford Press</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sEditionNumber, sPageRange, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8$9$10", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-11
                nPatternId = 182;
                //<RefLabel>27.</RefLabel> <Author><Surname>Kaas</Surname> <Forename>R.</Forename></Author> Compound Poisson distribution and GLM’s—Tweedie’s distribution. In: <Editor><ESurname>De Schepper</ESurname> <EForename>A</EForename></Editor> , <Editor><ESurname>Dhaene</ESurname> <EForename>J</EForename></Editor> , <Editor><ESurname>Reynaerts</ESurname> <EForename>H</EForename></Editor> , <Editor><ESurname>Schoutens</ESurname> <EForename>W</EForename></Editor> , <Editor><ESurname>Van Goethem</ESurname> <EForename>P</EForename></Editor> , <Editor><ESurname>Vanmaele</ESurname> <EForename>M</EForename></Editor> , <ldlEditorDelimiterEds_Back>eds.</ldlEditorDelimiterEds_Back> 3rd Actuarial and Financial Mathematics Day. <ldlCity>Wetteren</ldlCity><ldlCountry>Belgium</ldlCountry>: <ldlPublisherName>Universa Press</ldlPublisherName>; <PubDate><Year>2005</Year>:</PubDate> 3-12.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sUptoDotTitle, sLocationName, sPublisherName, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>$6$7$8<{3}>$9</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-11
                nPatternId = 183;
                //<RefLabel>28.</RefLabel> <Author><Surname>Klinker</Surname> <Forename>F.</Forename></Author> <Article_Title>Generalized linear mixed models for ratemaking: a means of introducing credibility into a generalized linear model setting.</Article_Title> Casualty Actuarial Society E-Forum. <PubDate><Year>2011</Year>;</PubDate> <Vol_No>2</Vol_No> <Issue_No>(Winter):</Issue_No> <PageRange>1-25.</PageRange> <ldlURLLabel>Available at:</ldlURLLabel> <Website>https://www.asact.org/pubs/forum/11wforumpt2/Klinker.pdf.</Website> <ldlAccessedDateLabel>Accessed</ldlAccessedDateLabel> <ldlAccessedDate>May 25, 2019.</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sArticleTitle, sUptoDotTitle, sPubDate, sVolumeNumber, sIssueNumber, sPageRange, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7$8$9", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-11
                nPatternId = 184;
                //<RefLabel>11.</RefLabel> <Author><Surname>Cameron</Surname> <Forename>LA</Forename></Author> , <Author><Surname>Footer</Surname> <Forename>MJ</Forename></Author> , <Author><Surname>van Oudenaarden</Surname> <Forename>A</Forename></Author> , <Author><Surname>Theriot</Surname> <Forename>JA.</Forename></Author> <PubDate>(<Year>1999</Year>).</PubDate> Motility of ActA protein-coated microspheres driven by actin polymerization. <ldlConferenceName><i>Proc. Natl. Acad. Sci. U.S.A</i></ldlConferenceName>. <Vol_No>96</Vol_No> <Issue_No>(9),</Issue_No> <PageRange>4908–13.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sConferenceName, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-13
                nPatternId = 185;
                //<Author><Surname>Zilverstand</Surname> <Forename>A.</Forename></Author> , <Author><Surname>Sorger</Surname> <Forename>B.</Forename></Author> , <Author><Surname>Sarkheil</Surname> <Forename>P.</Forename></Author> , & <Author><Surname>Goebel</Surname> <Forename>R.</Forename></Author> <PubDate>(<Year>2015</Year>).</PubDate> fMRI neurofeedback facilitates anxiety regulation in females with spider phobia. <i>Frontiers in Behavioral Neuroscience</i>, <Vol_No><i>9</i>,</Vol_No> <PageRange>148.</PageRange> <Doi>https://doi.org/10.3389/fnbeh.2015.00148.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sItalicTitle, sVolumeNumber, sPageRange, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-13
                nPatternId = 186;
                //<Author><Surname>Carone</Surname> <Forename>D. A.</Forename></Author> , & <Author><Surname>Bush</Surname> <Forename>S. S.</Forename></Author> <PubDate>(<Year>2013</Year>).</PubDate> Mild traumatic brain injury: Symptom validity assessment and malingering. In <i>Mild Traumatic Brain Injury: Symptom Validity Assessment and Malingering</i> <PageRange>(p. 15)</PageRange>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-13
                nPatternId = 187;
                //<Author><Surname>Mohagheghi</Surname> <Forename>S.</Forename></Author> , <Author><Surname>Yang</Surname> <Forename>F.</Forename></Author> , and <Author><Surname>Falahati</Surname> <Forename>B.</Forename></Author> <PubDate>(<Year>2011</Year>).</PubDate> “Impact of demand response on distribution system reliability.” <i>Power and Energy Society General Meeting, 2011 IEEE</i>, <ldlPublisherName>IEEE</ldlPublisherName>, 1–7.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>", "ldlChapterTitle", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-13
                nPatternId = 188;
                //<Author><Surname>Gingrich,</Surname> <Forename>Andre</Forename></Author> . <PubDate><Year>2015</Year>.</PubDate> <Article_Title>“Comparative Method in Anthropology.”</Article_Title> In <Journal_Title><i>International Encyclopedia of the Social & Behavioral Sciences</i>,</Journal_Title> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><ESurname>James</ESurname> <EForename>D. Wright</EForename></Editor> , <ldlEditionNumber>2nd edition,</ldlEditionNumber> <Vol_No>vol. 4,</Vol_No> <PageRange>411–14.</PageRange> <ldlCity>Oxford</ldlCity><ldlPublisherName>Elsevier.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})({10})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sTitleLabel, sJournalTitle, sAuthorEditorGroup, sEditionNumber, sVolumeNumber, sPageRange, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7$8$9$10$11", "ldlTitleLabel"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-13
                nPatternId = 189;
                //<Author><Surname>Trommsdorff,</Surname> <Forename>Gisela</Forename></Author> . <PubDate><Year>2003</Year>.</PubDate> „Kulturvergleichende Entwicklungspsychologie. “ In <i>Kulturvergleichende Psychologie: Eine Einführung</i>, <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>Alexander</EForename> <ESurname>Thomas</ESurname></Editor> , 139–79. <ldlCity>Göttingen</ldlCity><ldlPublisherName>Hogrefe.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sDownDoubleQuoteTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6<{3}>$7</{3}>$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-13
                nPatternId = 190;
                //<Author><Surname>Shweder</Surname> <Forename>Richard A</Forename></Author> , <Author><Forename>Jonathan</Forename> <Surname>Haidt</Surname></Author> , <Author><Forename>Randall</Forename> <Surname>Horton</Surname></Author> , and <Author><Forename>Joseph</Forename> <Surname>Craig</Surname></Author> . <PubDate><Year>2008</Year>.</PubDate> “The Cultural Psychology of the Emotions. Ancient and Renewed.” In <i>Handbook of Emotions.</i> <ldlEditionNumber>3rd Ed.,</ldlEditionNumber> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>M.</EForename> <ESurname>Lewis</ESurname></Editor> , <Editor><EForename>J. M.</EForename> <ESurname>Haviland-Jones</ESurname></Editor> and <Editor><EForename>L. F.</EForename> <ESurname>Barrett</ESurname></Editor> , 409–27. <ldlCity>New York</ldlCity><ldlPublisherName>Guilford Press</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sItalicTitle, sEditionNumber, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7<{3}>$8</{3}>$9$10", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-14
                nPatternId = 191;
                //<Author><Surname>Gingrich,</Surname> <Forename>Andre</Forename></Author> , and <Author><Forename>Peter</Forename> <Surname>Schweitzer</Surname></Author> . <PubDate><Year>2014</Year>.</PubDate> “Barter and Subsistence: Insights from Social Anthropology for Anatolia’s Prehistory.” In <ldlConferenceName><i>Prehistoric Economies of Anatolia: Subsistence Strategies and Exchange (Proceedings of a Workshop held at the Austrian Academy of Sciences in Vienna, November 13-14 2009),</i></ldlConferenceName> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>Celine</EForename> <ESurname>Wawruschka</ESurname></Editor> , 27-31. <ldlCity>Rahden</ldlCity><ldlPublisherName>Marie Leidorf Verlag.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sConferenceName, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6<{2}>$7</{2}>$8$9", "Article_Title", "ldlTitleLabel", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-14
                nPatternId = 192;
                //<Author><Surname>Bowles</Surname> <Forename>John R</Forename></Author> <PubDate><Year>1985</Year>.</PubDate> “Suicide and Attempted Suicide in Contemporary Western Samoa”. In <i>Culture,</i> <ldlConferenceName><i>Youth and Suicide in the Pacific: Chapters from an East-West Center Conference Center for Pacific Islands Studies</i>,</ldlConferenceName> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>F. X.</EForename> <ESurname>Hezel</ESurname></Editor> , <Editor><EForename>D. H.</EForename> <ESurname>Rubinstein</ESurname></Editor> , and <Editor><EForename>G. M.</EForename> <ESurname>White</ESurname></Editor> , 15-35. <ldlMisc>Working Paper series.</ldlMisc> <ldlCity>Honolulu</ldlCity><ldlState>Hawaii</ldlState>: <ldlPublisherName>Pacific Islands Studies Program</ldlPublisherName>, <ldlPublisherName>Center for Asian and Pacific Studies</ldlPublisherName>, <ldlPublisherName>University of Hawaii at Manoa</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})({10})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sItalicTitle, sConferenceName, sAuthorEditorGroup, sUnknownNumber, sMisc, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7<{3}>$8</{3}>$9$10$11", "Article_Title", "ldlTitleLabel", "Journal_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternId = 193;
                //<Author><Surname>Bell,</Surname> <Forename>Richard</Forename></Author> . <PubDate><Year>2011</Year>.</PubDate> <Article_Title>“Weeping for Werther: Suicide, Sympathy and the Reading Revolution in Early America”.</Article_Title> In <Journal_Title><i>The History of Reading</i>,</Journal_Title> <Vol_No>Volume 1,</Vol_No> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>S.</EForename> <ESurname>Towheed</ESurname></Editor> and <Editor><EForename>W. R.</EForename> <ESurname>Owens</ESurname></Editor> , 49-63. <ldlCity>Basingstoke</ldlCity><ldlPublisherName>Palgrave Macmillan.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sTitleLabel, sJournalTitle, sVolumeNumber, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7<{1}>$8</{1}>$9$10", "ldlTitleLabel", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternId = 194;
                //<Collab>EN1993-1-1</Collab> <PubDate><Year>2005</Year>.</PubDate> Design of steel structures–Part~~space~~1~~dot~~1: General rules and rules for buildings. <i>EN 1993-1-1:2005.</i> <ldlCity>Brussels</ldlCity><ldlCountry>Belgium</ldlCountry><ldlPublisherName>European Committee for Standardization.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sItalicTitle, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternId = 195;
                //<Author><Surname>O’Neill</Surname> <Forename>J.</Forename></Author> <PubDate>(<Year>2016</Year>).</PubDate> Final Report and Recommendations. The Review on Antimicrobial Resistance. <Website>https://amr-review.org/sites/default/files/160518_Final%20paper_with%20cover.pdf.</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoDotTitle, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternId = 196;
                //<Author><Surname>Snow</Surname> <Forename>R.</Forename></Author> , <Author><Surname>O’Connor</Surname> <Forename>B.</Forename></Author> , <Author><Surname>Jurafsky</Surname> <Forename>D.</Forename></Author> & <Author><Surname>Ng</Surname> <Forename>A. Y.</Forename></Author> <PubDate>(<Year>2008</Year>)</PubDate> Cheap and fast— but is it good? Evaluating non-expert annotations for natural language tasks. <ldlConferenceName><i>Proceedings of the Conference on Empirical Methods in Natural Language Processing</i>.</ldlConferenceName> <ldlCity>Stroudsburg</ldlCity><ldlPublisherName>Association for Computational Linguistics</ldlPublisherName>, 254–263. <Doi>https://doi.org/10.3115/1613715.1613751.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sConferenceName, sLocationName, sPublisherName, sUnknownNumber, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6<{1}>$7</{1}>$8", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternId = 197;
                //<Author><Surname>Simard</Surname> <Forename>D.</Forename></Author> <PubDate>(<Year>2018</Year>)</PubDate> Input enhancement. In <Editor><ESurname>Liontas</ESurname> <EForename>J. I.</EForename></Editor> & <Editor><ESurname>Delli Carpini</ESurname> <EForename>M.</EForename></Editor> <ldlEditorDelimiterEds_Back>(eds.),</ldlEditorDelimiterEds_Back> <i>The TESOL encyclopedia of English language teaching</i>. <ldlCity>Hoboken</ldlCity><ldlPublisherName>Wiley-Blackwell</ldlPublisherName>. <Doi>https://doi.org/10.1002/9781118784235.eelt0072.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternId = 198;
                //<Author><Surname>Robinson</Surname> <Forename>P.</Forename></Author> , <Author><Surname>Mackey</Surname> <Forename>A.</Forename></Author> , <Author><Surname>Gass</Surname> <Forename>S. M.</Forename></Author> & <Author><Surname>Schmidt</Surname> <Forename>R.</Forename></Author> <PubDate>(<Year>2012</Year>)</PubDate> Attention and awareness in second language acquisition. In <Editor><ESurname>Gass</ESurname> <EForename>S. M.</EForename></Editor> & <Editor><ESurname>Mackey</ESurname> <EForename>A.</EForename></Editor> <ldlEditorDelimiterEds_Back>(eds.),</ldlEditorDelimiterEds_Back> <i>The Routledge handbook of second language acquisition</i>. <ldlCity>New York</ldlCity><ldlPublisherName>Routledge</ldlPublisherName>, 247–267.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8<{3}>$9</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternId = 199;
                //<Author><Surname>Papineni</Surname> <Forename>K.</Forename></Author> , <Author><Surname>Roukos</Surname> <Forename>S.</Forename></Author> , <Author><Surname>Ward</Surname> <Forename>T.</Forename></Author> & <Author><Surname>Zhu</Surname> <Forename>W.-J.</Forename></Author> <PubDate>(<Year>2002</Year>)</PubDate> BLEU: A method for automatic evaluation of machine translation. In <ldlConferenceName><i>Proceedings of the 40th Annual Meeting on Association for Computational Linguistics</i>.</ldlConferenceName> <ldlCity>Stroudsburg</ldlCity><ldlPublisherName>Association for Computational Linguistics</ldlPublisherName>, 311–318. <Doi>https://doi.org/10.3115/1073083.1073135.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sConferenceName, sLocationName, sPublisherName, sUnknownNumber, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7<{2}>$8</{2}>$9", "Article_Title", "ldlTitleLabel", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternId = 200;
                //<Author><Surname>Meurers</Surname> <Forename>D.</Forename></Author> , <Author><Surname>Ziai</Surname> <Forename>R.</Forename></Author> , <Author><Surname>Amaral</Surname> <Forename>L.</Forename></Author> , <Author><Surname>Boyd</Surname> <Forename>A.</Forename></Author> , <Author><Surname>Dimitrov</Surname> <Forename>A.</Forename></Author> , <Author><Surname>Metcalf</Surname> <Forename>V.</Forename></Author> & <Author><Surname>Ott</Surname> <Forename>N.</Forename></Author> <PubDate>(<Year>2010</Year>)</PubDate> Enhancing authentic web pages for language learners. In <Editor><ESurname>Tetreault</ESurname> <EForename>J.</EForename></Editor> , <Editor><ESurname>Burstein</ESurname> <EForename>J.</EForename></Editor> & <Editor><ESurname>Leacock</ESurname> <EForename>C.</EForename></Editor> <ldlEditorDelimiterEds_Back>(eds.),</ldlEditorDelimiterEds_Back> <ldlConferenceName><i>Fifth Workshop on Innovative Use of NLP for Building Educational Applications: Proceedings of the Workshop</i></ldlConferenceName>. <ldlCity>Stroudsburg</ldlCity><ldlPublisherName>Association for Computational Linguistics</ldlPublisherName>, 10–18.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sConferenceName, sLocationName, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7$8<{2}>$9</{2}>", "Article_Title", "ldlTitleLabel", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternId = 201;
                //<Author><Surname>Meurers</Surname> <Forename>D.</Forename></Author> <PubDate>(<Year>2012</Year>)</PubDate> Natural language processing and language learning. In <Editor><ESurname>Chapelle</ESurname> <EForename>C. A.</EForename></Editor> <ldlEditorDelimiterEds_Back>(ed.),</ldlEditorDelimiterEds_Back> <i>Encyclopedia of applied linguistics</i>. <ldlCity>Hoboken</ldlCity><ldlPublisherName>John Wiley.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternId = 202;
                //<Author><Surname>Manning</Surname> <Forename>C. D.</Forename></Author> , <Author><Surname>Surdeanu</Surname> <Forename>M.</Forename></Author> , <Author><Surname>Bauer</Surname> <Forename>J.</Forename></Author> , <Author><Surname>Finkel</Surname> <Forename>J.</Forename></Author> , <Author><Surname>Bethard</Surname> <Forename>S. J.</Forename></Author> & <Author><Surname>McClosky</Surname> <Forename>D.</Forename></Author> <PubDate>(<Year>2014</Year>)</PubDate> The Stanford CoreNLP natural language processing toolkit. In <Editor><ESurname>Bontcheva</ESurname> <EForename>K.</EForename></Editor> & <Editor><ESurname>Zhu</ESurname> <EForename>J.</EForename></Editor> <ldlEditorDelimiterEds_Back>(eds.),</ldlEditorDelimiterEds_Back> <ldlConferenceName><i>Proceedings of 52nd Annual Meeting of the Association for Computational Linguistics: System Demonstrations</i>.</ldlConferenceName> <ldlCity>Stroudsburg</ldlCity><ldlPublisherName>Association for Computational Linguistics</ldlPublisherName>, 55–60. <Doi>https://doi.org/10.3115/v1/p14-5010.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sConferenceName, sItalicTitle, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                //added by Dakshinamoorthy on 2019-Sep-16
                nPatternId = 202;
                //<Author><Surname>Manning</Surname> <Forename>C. D.</Forename></Author> , <Author><Surname>Surdeanu</Surname> <Forename>M.</Forename></Author> , <Author><Surname>Bauer</Surname> <Forename>J.</Forename></Author> , <Author><Surname>Finkel</Surname> <Forename>J.</Forename></Author> , <Author><Surname>Bethard</Surname> <Forename>S. J.</Forename></Author> & <Author><Surname>McClosky</Surname> <Forename>D.</Forename></Author> <PubDate>(<Year>2014</Year>)</PubDate> The Stanford CoreNLP natural language processing toolkit. In <Editor><ESurname>Bontcheva</ESurname> <EForename>K.</EForename></Editor> & <Editor><ESurname>Zhu</ESurname> <EForename>J.</EForename></Editor> <ldlEditorDelimiterEds_Back>(eds.),</ldlEditorDelimiterEds_Back> <ldlConferenceName><i>Proceedings of 52nd Annual Meeting of the Association for Computational Linguistics: System Demonstrations</i>.</ldlConferenceName> <ldlCity>Stroudsburg</ldlCity><ldlPublisherName>Association for Computational Linguistics</ldlPublisherName>, 55–60. <Doi>https://doi.org/10.3115/v1/p14-5010.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sConferenceName, sLocationName, sPublisherName, sUnknownNumber, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7$8<{2}>$9</{2}>$10", "ldlChapterTitle", "ldlTitleLabel", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-23
                nPatternId = 203;
                //<Author><Surname>Chinkina</Surname> <Forename>M.</Forename></Author> , <Author><Surname>Ruiz</Surname> <Forename>S.</Forename></Author> & <Author><Surname>Meurers</Surname> <Forename>D.</Forename></Author> <PubDate>(<Year>2017</Year>)</PubDate> Automatically generating questions to support the acquisition of particle verbs: Evaluating via crowdsourcing. In <Editor><ESurname>Borthwick</ESurname> <EForename>K.</EForename></Editor> , <Editor><ESurname>Bradley</ESurname> <EForename>L.</EForename></Editor> & <Editor><ESurname>Thouësny</ESurname> <EForename>S.</EForename></Editor> <ldlEditorDelimiterEds_Back>(eds.),</ldlEditorDelimiterEds_Back> <i>CALL in a climate of change: Adapting to turbulent global conditions– short papers from EUROCALL 2017.</i> <ldlCity>Voillans</ldlCity><ldlPublisherName>Research-publishing.net</ldlPublisherName>, 73–78. <Doi>https://doi.org/10.14705/rpnet.2017.eurocall2017.692.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sUnknownNumber, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8<{3}>$9</{3}>$10", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-23
                nPatternId = 204;
                //<Author><Surname>Becker</Surname> <Forename>L.</Forename></Author> , <Author><Surname>Basu</Surname> <Forename>S.</Forename></Author> & <Author><Surname>Vanderwende</Surname> <Forename>L.</Forename></Author> <PubDate>(<Year>2012</Year>)</PubDate> Mind the gap: learning to choose gaps for question generation. In <ldlConferenceName><i>Proceedings of the 2012 Conference of the North American Chapter of the Association for Computational Linguistics: Human Language Technologies</i>.</ldlConferenceName> <ldlCity>Stroudsburg</ldlCity><ldlPublisherName>Association for Computational Linguistics</ldlPublisherName>, 742–751.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sConferenceName, sLocationName, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7<{2}>$8</{2}>", "Article_Title", "ldlTitleLabel", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-23
                nPatternId = 205;
                //<RefLabel>[25]</RefLabel> <Author><Surname>Papaspiliopoulos</Surname> <Forename>O.</Forename></Author> and <Author><Surname>Roberts</Surname> <Forename>G. O.</Forename></Author> <PubDate>(<Year>2003</Year>).</PubDate> Non-centered parameterisations for hierarchical models and data augmentation. In <i>Bayesian Statistics 7</i>, <ldlPublisherName>Oxford University Press</ldlPublisherName>, <ldlCity>New York</ldlCity>, <PageRange>pp. 307–326.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sPublisherName, sLocationName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-23
                nPatternId = 206;
                //<RefLabel>[8]</RefLabel> <Author><Surname>Geyer</Surname> <Forename>C. J.</Forename></Author> <PubDate>(<Year>1991</Year>).</PubDate> Markov chain Monte Carlo maximum likelihood. In <i>Computing Science and Statistics,</i> <ldlConferenceName><i>Proceedings of the 23rd Symposium on the Interface</i>,</ldlConferenceName> <PageRange>pp. 156–163.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sConferenceName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-23
                nPatternId = 207;
                //<Author><Surname>Reineccius</Surname> <Forename>G. A.</Forename></Author> <PubDate>(<Year>2000</Year>).</PubDate> Flavoring Systems for Functional Foods. In <i>Essentials of Functional Foods.</i> <Editor><EForename>M. K.</EForename> <ESurname>Schmidl</ESurname></Editor> and <Editor><EForename>T. P.</EForename> <ESurname>Labuza</ESurname></Editor> <ldlEditorDelimiterEds_Back>(Eds.),</ldlEditorDelimiterEds_Back> <PageRange>pp. 87–95,</PageRange> <ldlCity>Gaithersburg</ldlCity><ldlState>MD</ldlState><ldlPublisherName>Aspen Publishers.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sPageRange, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-23
                nPatternId = 208;
                //<Author><Surname>Erickson</Surname> <Forename>B. E.</Forename></Author> Agency responds to evidence that chemicals can cause cancer in animals FDA bans 7 synthetic food flavorings, <PubDate><Month>October</Month> <Day>9</Day>, <Year>2018</Year>,</PubDate> <Website>https://cen.acs.org/safety/consumer-safety/FDA-bans-7-synthetic-food/96/web/2018/10).</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sUptoCommaTitle, sPubDate, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-23
                nPatternId = 209;
                //<Author><Surname>Zhang</Surname> <Forename>H.Z.</Forename></Author> , <Author><Surname>Lee</Surname> <Forename>T.C.</Forename></Author> <PubDate><Year>1997</Year>.</PubDate> Gas chromatography-mass spectrometry analysis of volatile flavor compounds in Mackerel for assessment of fish quality. In <i>Flavor and Lipid Chemistry of Seafoods</i>, <ldlEditorDelimiterEds_Front>ed.</ldlEditorDelimiterEds_Front> <Editor><ESurname>Shahidi</ESurname> <EForename>F.</EForename></Editor> , <Editor><ESurname>Cadwallader</ESurname> <EForename>K.R.</EForename></Editor> , <PageRange>pp. 55–63,</PageRange> <ldlPublisherName>American Chemical Society</ldlPublisherName><ldlCity>Washington</ldlCity><ldlState>DC.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sPageRange, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-23
                nPatternId = 210;
                //<Author><Surname>Peter</Surname> <Forename>K.V.</Forename></Author> <PubDate><Year>2012</Year>.</PubDate> Handbook of herbs and spices. In <i>Food Science, Technology and Nutrition</i>, <Vol_No>Vol. 1,</Vol_No> <ldlPublisherName>Woodhead Publishing</ldlPublisherName><ldlCity>Philadelphia</ldlCity><ldlState>PA.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sVolumeNumber, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-23
                nPatternId = 211;
                //<Author><Surname>Wu</Surname> <Forename>SV</Forename></Author> , <Author><Surname>Rozengurt</Surname> <Forename>N</Forename></Author> , <Author><Surname>Yang</Surname> <Forename>M</Forename></Author> , <Author><Surname>Young</Surname> <Forename>SH</Forename></Author> , <Author><Surname>Sinnett-Smith</Surname> <Forename>J</Forename></Author> , <Author><Surname>Rozengurt</Surname> <Forename>E.</Forename></Author> Expression of bitter taste receptors of the T2R family in the gastrointestinal tract and enteroendocrine STC-1 cells. <ldlConferenceName><i>Proceedings of the National Academy of Sciences</i>.</ldlConferenceName> <PubDate><Year>2002</Year>;</PubDate> <Vol_No>99</Vol_No> <Issue_No>(4):</Issue_No> <PageRange>2392–2397.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sUptoDotTitle, sConferenceName, sPubDate, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5$6$7", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-24
                nPatternId = 212;
                //<Author><Surname>Nigel</Surname> <Forename>P. Melville</Forename></Author> , “Managing in the Fourth Industrial Age,” <i>BizEd Magazine</i> <PubDate>(<Month>March</Month> <Day>1</Day>, <Year>2018</Year>):</PubDate> <Website>bized.aacsb.edu</Website>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sDoubleQuoteTitle, sItalicTitle, sPubDate, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-25
                nPatternId = 213;
                //<Author><Forename>Dov</Forename> <Surname>Eden</Surname></Author> , <Author><Forename>Dvorah</Forename> <Surname>Geller</Surname></Author> , and <Author><Forename>Abigail</Forename> <Surname>Gerwirtz</Surname></Author> , <ldlConferenceName>“Implanting Pygmalion Leadership Style through Workshop Training: Seven Field Experiments,</ldlConferenceName>” <i>Leadership Quarterly,</i> <Vol_No>vol. 11</Vol_No> <Issue_No>(2)</Issue_No> <PubDate>(<Year>2000</Year>),</PubDate> <PageRange>pp. 171–210.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sConferenceName, sItalicTitle, sVolumeNumber, sIssueNumber, sPubDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-26
                nPatternId = 214;
                //<Author><Forename>Mandy</Forename> <Surname>Dorn</Surname></Author> and <Author><Forename>Michele</Forename> <Surname>Vana</Surname></Author> , “Younger Managers Rise in the Ranks,” <i>EY Building a Better Working World,</i> <Website>ey.com</Website> <ldlAccessedDateLabel>(accessed</ldlAccessedDateLabel> <ldlAccessedDate>December 9, 2013);.</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sDoubleQuoteTitle, sItalicTitle, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-28
                nPatternId = 215;
                //<Collab>——</Collab> <PubDate>(<Year>1990</Year>)</PubDate> ‘Justice as Reciprocity’. In <Editor><EForename>Samuel</EForename> <ESurname>Freeman</ESurname></Editor> <ldlEditorDelimiterEds_Back>(ed),</ldlEditorDelimiterEds_Back> <i>Collected Papers– John Rawls</i> <ldlCity>(Cambridge</ldlCity><ldlState>MA</ldlState>: <ldlPublisherName>Harvard University Press)</ldlPublisherName>, <PageRange>pp. 47–72.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sSingleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-28
                nPatternId = 216;
                //<Author><Surname>Bae</Surname> <Forename>S.</Forename></Author> <PubDate>(<Year>2005</Year>).</PubDate> “Seismic Performance of Full-Scale Reinforced Concrete Columns.” <ldlThesisKeyword>Ph.D. Thesis</ldlThesisKeyword>, <ldlPublisherName>University of Texas at Austin,</ldlPublisherName> <ldlCountry>USA</ldlCountry>, 312.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sThesisKeyword, sPublisherName, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3$4$5$6<{0}>$7</{0}>", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-30
                nPatternId = 217;
                //<Author><Surname>Kelsen,</Surname> <Forename>Hans</Forename></Author> <PubDate>(<Year>1967</Year>)</PubDate> The Pure Theory of Law. <ldlEditorDelimiterEds_Front>Trans.</ldlEditorDelimiterEds_Front> <Editor><EForename>Max</EForename> <ESurname>Knight</ESurname></Editor> . <ldlCity>Berkeley</ldlCity><ldlPublisherName>University of California Press</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sAuthorEditorGroup, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Sep-30
                nPatternId = 218;
                //<Author><Surname>Simma,</Surname> <Forename>Bruno</Forename></Author> . <PubDate>(<Year>1994</Year>)</PubDate> ‘From Bilateralism to Community Interest in International Law’ in <i>Collected Courses of the Hague Academy of International Law</i>, <Vol_No>vol. 250.</Vol_No> <ldlCity>Leiden</ldlCity><ldlPublisherName>Brill Nijhoff.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sSingleQuoteTitle, sTitleLabel, sItalicTitle, sVolumeNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-01
                nPatternId = 219;
                //<RefLabel>38.</RefLabel> <Collab>The Chartered Society of Physiotherapy.</Collab> <PubDate><Year>2014</Year>.</PubDate> The falls prevention economic model. <ldlMisc>[ONLINE]</ldlMisc> <ldlURLLabel>Available at:</ldlURLLabel> <Website>http://www.csp.org/documents/falls-prevention-economic-model.</Website> <ldlAccessedDateLabel>[Accessed</ldlAccessedDateLabel> <ldlAccessedDate>1 August 2019].</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sMisc, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-01
                nPatternId = 220;
                //<Author><Surname>Wilkie</Surname> <Forename>L.</Forename></Author> , <PubDate><Year>2009</Year>:</PubDate> Interpretive historical archaeologies, in <Editor><EForename>T.</EForename> <ESurname>Majewski</ESurname></Editor> and <Editor><EForename>D.</EForename> <ESurname>Gaimster</ESurname></Editor> <ldlEditorDelimiterEds_Back>(eds),</ldlEditorDelimiterEds_Back> <i>International handbook of historical archaeology</i>, New York, 333–45.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sUptoCommaTitle, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    string sRefContent_Temp = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}><{3}>$7</{3}><{4}>$8</{4}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "ldlCity", "PageRange"));
                    if (ValidatePublisherLocation(sRefContent_Temp) == true)
                    {
                        sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}><{3}>$7</{3}><{4}>$8</{4}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "ldlCity", "PageRange"));
                        goto LBL_SKIP_PTN;
                    }
                }

                //added by Dakshinamoorthy on 2019-Oct-01
                nPatternId = 221;
                //<Author><Surname>Ingold</Surname> <Forename>T.</Forename></Author> , <PubDate><Year>2011</Year>:</PubDate> <i>Being alive. Essays on movement, knowledge and description</i>, London.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sItalicTitle, @"(?:(?:[^<>\.0-9]{5,})\.)");
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    string sRefContent_Temp = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>", "ldlBookTitle", "ldlCity"));
                    if (ValidatePublisherLocation(sRefContent_Temp) == true)
                    {
                        sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>", "ldlBookTitle", "ldlCity"));
                        goto LBL_SKIP_PTN;
                    }
                }

                //added by Dakshinamoorthy on 2019-Oct-01
                nPatternId = 222;
                //<Author><Surname>Ingold</Surname> <Forename>T.</Forename></Author> , <PubDate><Year>2011</Year>:</PubDate> <i>Being alive. Essays on movement, knowledge and description</i>, London.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sItalicTitle, @"(?:(?:[^<>\.0-9]{5,})\.)");
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    string sRefContent_Temp = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>", "ldlBookTitle", "ldlCity"));
                    if (ValidatePublisherLocation(sRefContent_Temp) == true)
                    {
                        sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>", "ldlBookTitle", "ldlCity"));
                        goto LBL_SKIP_PTN;
                    }
                }

                //added by Dakshinamoorthy on 2019-Oct-01
                nPatternId = 223;
                //<Author><Surname>van Vliet</Surname> <Forename>K.</Forename></Author> , <PubDate><Year>2015</Year>:</PubDate> In line with things. A neomaterialist approach to archaeological assemblages, <ldlThesisKeyword>master’s thesis</ldlThesisKeyword>, <ldlPublisherName>Department of Archaeology and Classical Studies,</ldlPublisherName> <ldlPublisherName>Stockholm University</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoCommaTitle, sThesisKeyword, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-01
                nPatternId = 224;
                //<Author><Surname>Skousen</Surname> <Forename>B.J.</Forename></Author> , and <Author><Forename>M.E.</Forename> <Surname>Buchanan</Surname></Author> , <PubDate><Year>2015</Year>:</PubDate> Advancing an archaeology of movements and relationships, in <Editor><EForename>M.E.</EForename> <ESurname>Buchanan</ESurname></Editor> and <Editor><EForename>B.J.</EForename> <ESurname>Skousen</ESurname></Editor> <ldlEditorDelimiterEds_Back>(eds),</ldlEditorDelimiterEds_Back> <i>Tracing the relational. The archaeology of worlds, spirits, and temporalities</i>, <ldlCity>Salt Lake City</ldlCity><ldlState>UT</ldlState>, 1–19.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7<{3}>$8</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-03
                nPatternId = 225;
                //<Author><Surname>Robb</Surname> <Forename>J.</Forename></Author> , and <Author><Forename>T.R.</Forename> <Surname>Pauketat</Surname></Author> , <PubDate><Year>2013</Year>:</PubDate> From moments to millennia. Theorizing scale and change in human history, in <Editor><EForename>J.</EForename> <ESurname>Robb</ESurname></Editor> and <Editor><EForename>T.R.</EForename> <ESurname>Pauketat</ESurname></Editor> <ldlEditorDelimiterEds_Back>(eds),</ldlEditorDelimiterEds_Back> <i>Big histories, human lives. Tackling problems of scale in archaeology</i>, <ldlCity>Santa Fe</ldlCity><ldlState>NM</ldlState>, 3–34.
                sFullPattern = string.Format(@"^({0})({1})({2}{3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoCommaTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7<{3}>$8</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-03
                nPatternId = 226;
                //<Author><Surname>Overholtzer</Surname> <Forename>L.</Forename></Author> , and <Author><Forename>C.</Forename> <Surname>Robin</Surname></Author> , <PubDate><Year>2015</Year>:</PubDate> The materiality of everyday life. An introduction, <i>Archaeological papers of the American Anthropological Association</i> <Vol_No>26,</Vol_No> <PageRange>1–9.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2}{3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoCommaTitle, sItalicTitle, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-03
                nPatternId = 227;
                //<Author><Surname>Martin</Surname> <Forename>A.S.</Forename></Author> , <PubDate><Year>1994</Year>:</PubDate> ‘Fashionable sugar dishes, latest fashion ware.’ The creamware revolution in the 18th-century Chesapeake, in <Editor><EForename>P.</EForename> <ESurname>Shackel</ESurname></Editor> and <Editor><EForename>B.</EForename> <ESurname>Little</ESurname></Editor> <ldlEditorDelimiterEds_Back>(eds),</ldlEditorDelimiterEds_Back> <i>Historical archaeology of the Chesapeake</i>, <ldlCity>Washington</ldlCity><ldlState>DC</ldlState>, 169–87.
                sFullPattern = string.Format(@"^({0})({1})({2}{3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sSingleQuoteTitle, sUptoCommaTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7<{3}>$8</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-03
                nPatternId = 228;
                //<Author><Surname>Ibrahim</Surname> <Forename>R. A.</Forename></Author> <PubDate>(<Year>2005</Year>).</PubDate> “Liquid Sloshing Dynamics: Theory and Applications.” <ldlPublisherName>Cambridge University Press,</ldlPublisherName> <ldlCity>Cambridge</ldlCity><ldlState>New York</ldlState>. 130–137. <Doi>http://dx.doi.org/10.1017/CBO9780511536656.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sPublisherName, sLocationName, sUnknownNumber, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5<{1}>$6</{1}>$7", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-03
                nPatternId = 229;
                //<RefLabel>[1]</RefLabel> <Author><Surname>Aydin</Surname> <Forename>H</Forename></Author> , <Author><Surname>Turan</Surname> <Forename>O</Forename></Author> , <Author><Surname>Karakoc</Surname> <Forename>TH</Forename></Author> , <Author><Surname>Midilli</Surname> <Forename>A.</Forename></Author> <Article_Title>Component-based exergetic measures of the an experimental turboprop/turboshaft engine for propeller aircrafts and helicopters.</Article_Title> Int J Exergy <PubDate><Year>2012</Year>;</PubDate> <Vol_No>11</Vol_No> <Issue_No>(3):</Issue_No> <PageRange>322–48.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sArticleTitle, "(?:[A-Za-z ]{5,})", sPubDate, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-03
                nPatternId = 230;
                //<Author><Surname>Marshall</Surname> <Forename>H. E.</Forename></Author> <PubDate><Year>2002</Year>.</PubDate> “Economic approaches to homeland security for constructed facilities.” In <ldlConferenceName>Proc., <i>10th Joint W055-W056 Int. Symp. on Construction Innovation and Global Competitiveness</i>.</ldlConferenceName> <ldlCity>Cincinnati</ldlCity><ldlState>OH.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sConferenceName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "Article_Title", "ldlTitleLabel"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-03
                nPatternId = 231;
                //<Author><Surname>King</Surname> <Forename>S. A.</Forename></Author> , <Author><Forename>H. R.</Forename> <Surname>Adib</Surname></Author> , <Author><Forename>J.</Forename> <Surname>Drobny</Surname></Author> , and <Author><Forename>J.</Forename> <Surname>Buchanan</Surname></Author> <PubDate><Year>2003</Year>.</PubDate> “Earthquake and terrorism risk assessment: Similarities and differences.” In <ldlConferenceName>Proc., 6th <i>U.S. Conf. and Workshop on Lifeline Earthquake Engineering</i>,</ldlConferenceName> 789-798. <ldlCity>Long Beach</ldlCity><ldlState>CA.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sConferenceName, sUnknownNumber, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7", "Article_Title", "ldlTitleLabel", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-04
                nPatternId = 232;
                //<RefLabel>a)</RefLabel> <Author><Forename>H.</Forename> <Surname>Li</Surname></Author> , <Author><Forename>Y. V.</Forename> <Surname>Aulin</Surname></Author> , <Author><Forename>L.</Forename> <Surname>Frazer</Surname></Author> , <Author><Forename>E.</Forename> <Surname>Borguet</Surname></Author> , <Author><Forename>R.</Forename> <Surname>Kakodkar</Surname></Author> , <Author><Forename>J.</Forename> <Surname>Feser</Surname></Author> , <Author><Forename>Y.</Forename> <Surname>Chen</Surname></Author> , <Author><Forename>K.</Forename> <Surname>An</Surname></Author> , <Author><Forename>D. A.</Forename> <Surname>Dikin</Surname></Author> , <Author><Forename>F.</Forename> <Surname>Ren</Surname></Author> , ACS Appl. Mater. Interfaces <PubDate><Year>2017</Year>,</PubDate> 6655;.
                sFullPattern = string.Format(@"^({0})((?:[A-Z]{{3,}} )?{1}(?: [A-Z][a-z]+ )?)({2})({3})$", sAuthorEditorGroup, sJournalTtlAbbrPatn, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3<{1}>$4</{1}>", "Journal_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-05
                nPatternId = 233;
                //<Author><Surname>Terzaghi</Surname> <Forename>K.</Forename></Author> <PubDate>(<Year>1950</Year>).</PubDate> “Mechanism of Landslides.” In <Editor><ESurname>Paige</ESurname> <EForename>S.</EForename></Editor> <ldlEditorDelimiterEds_Back>(ed.),</ldlEditorDelimiterEds_Back> Application of Geology to Engineering Practice (Berkey volume), <ldlPublisherName>Geological Society of America</ldlPublisherName><ldlCity>New York</ldlCity>, <Vol_No>1950,</Vol_No> <PageRange>83-123.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sUptoCommaTitle, sPublisherName, sLocationName, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8$9$10", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-08
                nPatternId = 234;
                //<Author><Surname>Baden</Surname> <Forename>C.</Forename></Author> <PubDate><Year>2010</Year>.</PubDate> Communication, Contextualization & Cognition: Patterns & Processes of Frames’ Influence on People’s Interpretations of the Eu Constitution. Amsterdam School of Communication Research (ASCoR). <ldlURLLabel>Available at:</ldlURLLabel> <Website>http://dare.uva.nl/search?metis.record.id=331376</Website> <ldlAccessedDateLabel>[Accessed</ldlAccessedDateLabel> <ldlAccessedDate>May 24, 2017].</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoDotTitle, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-08
                nPatternId = 235;
                //<Author><Surname>Ancelovici</Surname> <Forename>M.</Forename></Author> <PubDate><Year>2016</Year>.</PubDate> Occupy Montreal and the Politics of Horizontalism. In: <Author><Surname>Dufour</Surname> <Forename>P.</Forename></Author> <Etal>et al.</Etal> <ldlEditorDelimiterEds_Back>eds.</ldlEditorDelimiterEds_Back> <i>Street Politics in the Age of Austerity: From The Indignados to Occupy</i>. <ldlCity>Amsterdam</ldlCity><ldlPublisherName>Amsterdam University Press</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-08
                nPatternId = 236;
                //<Author><Surname>Spanou</Surname> <Forename>C.</Forename></Author> <PubDate>(<Year>2017</Year>)</PubDate> ‘Modernisation: the end of the external constraint approach?’ In <Editor><ESurname>Economides</ESurname> <EForename>S.</EForename></Editor> <ldlEditorDelimiterEds_Back>(ed.),</ldlEditorDelimiterEds_Back> <i>Modernization and Europe 20 years on</i>. <ldlCity>London</ldlCity><ldlCountry>UK</ldlCountry><ldlPublisherName>Hellenic Observatory LSE.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sSingleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-08
                nPatternId = 237;
                //<Author><Surname>Choudry</Surname> <Forename>S.</Forename></Author> <PubDate>(<Year>2014</Year>),</PubDate> Pakistan: a journey of poverty-induced shame. <Editor><EForename>E.</EForename> <ESurname>Gubrium</ESurname></Editor> , <Editor><EForename>S.</EForename> <ESurname>Pellissery</ESurname></Editor> and <Editor><EForename>I.</EForename> <ESurname>Lodemel</ESurname></Editor> <ldlEditorDelimiterEds_Back>(Eds.)</ldlEditorDelimiterEds_Back> <i>The Shame of It. Global Perspectives on Anti-Poverty Policies</i>. <ldlCity>Bristol</ldlCity>, <ldlPublisherName>Policy Press</ldlPublisherName>: 111-133.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4<{1}>$5</{1}>$6$7<{2}>$8</{2}>", "ldlChapterTitle", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-09
                nPatternId = 238;
                //<RefLabel>36.</RefLabel> <Author><Surname>Hochgraf</Surname> <Forename>C</Forename></Author> , <Author><Surname>Nygate</Surname> <Forename>J</Forename></Author> , <Author><Surname>Bazdresch</Surname> <Forename>M</Forename></Author> , <Etal>et al.</Etal> Providing first responders with real-time status of cellular networks during a disaster. In: <ldlConferenceName>2018 IEEE International Symposium on Technologies for Homeland Security (HST).</ldlConferenceName> <ldlPublisherName>IEEE</ldlPublisherName>; <PubDate><Year>2018</Year>:</PubDate> 1-4.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sUptoDotTitle, sTitleLabel, sConferenceName, sPublisherName, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5$6<{2}>$7</{2}>", "Article_Title", "ldlTitleLabel", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-09
                nPatternId = 239;
                //<RefLabel>16.</RefLabel> <Collab>South Carolina Department of Mental Health.</Collab> The DMH telepsychiatry program. <Website>https://scdmh.net/dmhtelepsychiatry/telepsychiatry/.</Website> <ldlAccessedDateLabel>Accessed</ldlAccessedDateLabel> <ldlAccessedDate>June 30, 2019.</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sUptoDotTitle, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-09
                nPatternId = 240;
                //<RefLabel>13.</RefLabel> <Collab>Centers for Medicare & Medicaid Services.</Collab> Emergency Triage, Treat, and Transport (ET3) Model. <PubDate><Year>2019</Year>.</PubDate> <Website>https://www.cms.gov/newsroom/fact-sheets/emergency-triage-treat-and-transport-et3-model.</Website> <ldlAccessedDateLabel>Accessed</ldlAccessedDateLabel> <ldlAccessedDate>June 28, 2019.</ldlAccessedDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sUptoDotTitle, sPubDate, sURLWithLabel, sAccessedDateWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-09
                nPatternId = 241;
                //<RefLabel>9.</RefLabel> <Author><Surname>Ishikawa</Surname> <Forename>Y</Forename></Author> , <Author><Surname>Miura</Surname> <Forename>T</Forename></Author> , <Author><Surname>Ishikawa</Surname> <Forename>Y</Forename></Author> , <Etal>et al.</Etal> <Article_Title>Duchenne muscular dystrophy: Survival by cardio-respiratory interventions.</Article_Title> Neuromusc Disord <PubDate><Year>2011</Year>;</PubDate> <Vol_No>21:</Vol_No> <PageRange>47-51.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sArticleTitle, "(?:[A-Za-z ]{5,})", sPubDate, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-09
                nPatternId = 242;
                //<RefLabel>5.</RefLabel> <Author><Surname>Dubowitz</Surname> <Forename>V.</Forename></Author> Muscular dystrophies. In: Muscle Disorders in Childhood. <ldlEditionNumber>2nd edition.</ldlEditionNumber> <ldlCity>Philadelphia</ldlCity><ldlPublisherName>WB Saunders</ldlPublisherName>; <PubDate><Year>1995</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sUptoDotTitle, sTitleLabel, sUptoDotTitle, sEditionNumber, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}><{2}>$4</{2}>$5$6$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-09
                nPatternId = 243;
                //<Author><Surname>Resplendino</Surname> <Forename>J.</Forename></Author> “Ultra-high performance concretes—Recent realizations and research programs on UHPFRC bridges in France.” In <ldlConferenceName><i>Proc., 2nd Int. Symp. on Ultra High Performance Concrete,</i></ldlConferenceName> 31-43. <ldlCity>Kassel</ldlCity><ldlCountry>Germany</ldlCountry>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sDoubleQuoteTitle, sTitleLabel, sConferenceName, sUnknownNumber, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>$6", "Article_Title", "ldlTitleLabel", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-09
                nPatternId = 244;
                //<Author><Surname>Peng</Surname> <Forename>J.</Forename></Author> , <Author><Surname>Shan</Surname> <Forename>X.</Forename></Author> , <Author><Surname>Davidson</Surname> <Forename>R.</Forename></Author> , <Author><Surname>Kesete</Surname> <Forename>Y.</Forename></Author> , <Author><Surname>Gao</Surname> <Forename>Y.</Forename></Author> , <Author><Surname>Nozick</Surname> <Forename>L.</Forename></Author> <PubDate>(<Year>2013</Year>)</PubDate> Hurricane loss modeling to support retrofit policymaking: A North Carolina case study. <ldlConferenceName><i>Proceedings of the 11th international conference on structural safety and reliability</i>,</ldlConferenceName> <ldlConferenceDate>June 16–20,</ldlConferenceDate> <ldlCity>New York</ldlCity><ldlState>NY.</ldlState>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sConferenceName, sConferenceDate, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-09
                nPatternId = 245;
                //<RefLabel>43.</RefLabel> <Collab>Institute of Medicine.</Collab> Dietary Risk Assessment in the WIC Program. Committee on Dietary Risk Assessment in the WIC Program. <ldlCity>Washington</ldlCity><ldlState>DC</ldlState>: <ldlPublisherName>The National Academies Press,</ldlPublisherName> <PubDate><Year>2002</Year>.</PubDate> <PageRange>167p.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sUptoDotTitle, sUptoDotTitle, sLocationName, sPublisherName, sPubDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5$6$7", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-10
                nPatternId = 246;
                //<Author><Surname>Kjellberg</Surname> <Forename>F.</Forename></Author> , <Author><Forename>E.</Forename> <Surname>Jousselin</Surname></Author> , <Author><Forename>M.</Forename> <Surname>Hossaert-Mckey</Surname></Author> , and <Author><Forename>J.-Y.</Forename> <Surname>Rasplus</Surname></Author> <PubDate><Year>2005</Year>.</PubDate> Biology, ecology, and evolution of fig-pollinating wasps (Chalcidoidea, Agaonidae). In <i>Biology, Ecology and Evolution of Gall-inducing Arthropods</i> <Vol_No>Vol. 2</Vol_No> (<Editor><EForename>A.</EForename> <ESurname>Raman</ESurname></Editor> , <Editor><EForename>C. W.</EForename> <ESurname>Shaefer</ESurname></Editor> , <Editor><EForename>T. M.</EForename> <ESurname>Withers</ESurname></Editor> , <ldlEditorDelimiterEds_Back>eds.),</ldlEditorDelimiterEds_Back> <PageRange>pp. 539–572.</PageRange> <ldlPublisherName>Science Publishers Inc.</ldlPublisherName><ldlCity>Enfield</ldlCity><ldlState>New Hampshire</ldlState>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sVolumeNumber, sAuthorEditorGroup, sPageRange, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8$9$10", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-25
                nPatternId = 247;
                //<Author><Surname>McRae,</Surname> <Forename>Ben</Forename></Author> , ‘Entry into Force of the Convention on Supplementary Compensation for Nuclear Damage: Opening the Umbrella’ <PubDate>(<Year>2015</Year>)</PubDate> <i>Nuclear Law Bulletin</i> 7–25.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sSingleQuoteTitle, sPubDate, sItalicTitle, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3<{1}>$4</{1}><{2}>$5</{2}>", "Article_Title", "Journal_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-25
                nPatternId = 248;
                //<Author><Surname>Meron,</Surname> <Forename>Theodor</Forename></Author> , ‘State Responsibility for Violations of Human Rights’ <PubDate>(<Year>1989</Year>)</PubDate> 83 <i>American Society of International Law Proceedings</i> 372–385.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sSingleQuoteTitle, sPubDate, sUnknownNumber, sItalicTitle, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3<{1}>$4</{1}><{2}>$5</{2}><{3}>$6</{3}>", "Article_Title", "Vol_No", "Journal_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-25
                nPatternId = 249;
                //<Author><Surname>Nieminen,</Surname> <Forename>Katja</Forename></Author> , ‘The Rules of Attribution and the Private Military Contractors at Abu Ghraib: Private Acts or Public Wrongs?’ <PubDate>(<Year>2006</Year>)</PubDate> XV <i>Finnish Yearbook of International Law</i> 289–319.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sSingleQuoteTitle, sPubDate, sUnknownNumber, sItalicTitle, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3<{1}>$4</{1}><{2}>$5</{2}><{3}>$6</{3}>", "Article_Title", "Vol_No", "Journal_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }


                //added by Dakshinamoorthy on 2019-Oct-25
                nPatternId = 250;
                //<Author><Surname>Mohr,</Surname> <Forename>Manfred</Forename></Author> , ‘The ILC’s Distinction between “International Crimes” and “International Delicts” and Its Implications’, in <Editor><EForename>Marina</EForename> <ESurname>Spinedi</ESurname></Editor> and <Editor><EForename>Bruno</EForename> <ESurname>Simma</ESurname></Editor> <ldlEditorDelimiterEds_Back>(eds.),</ldlEditorDelimiterEds_Back> <i>United Nations Codification of State Responsibility</i> <ldlCity>(New York</ldlCity><ldlPublisherName>Oceana Publications,</ldlPublisherName> <PubDate><Year>1987</Year>),</PubDate> <PageRange>pp. 115–141.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sSingleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sPubDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>$6$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-25
                nPatternId = 251;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Collab>Mégret,</Collab> Frédéric,</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> ‘Globalization’, in <i>Max Planck Encyclopedia of International Law</i> <ldlCity>(Oxford</ldlCity><ldlPublisherName>Oxford University Press,</ldlPublisherName> <PubDate><Year>2013</Year>).</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sSingleQuoteTitle, sTitleLabel, sItalicTitle, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}><{2}>$4</{2}>$5$6$7", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-25
                nPatternId = 252;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Paulus,</Surname> <Forename>Andreas</Forename></Author> ,</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> ‘Whether Universal Values can Prevail over Bilateralism and Reciprocity’, in <Editor><EForename>Antonio</EForename> <ESurname>Cassese</ESurname></Editor> <ldlEditorDelimiterEds_Back>(ed.),</ldlEditorDelimiterEds_Back> <i>Realizing Utopia</i>. <i>The Future of International Law</i> <ldlCity>(Oxford</ldlCity><ldlPublisherName>Oxford University Press,</ldlPublisherName> <PubDate><Year>2012</Year>),</PubDate> <PageRange>pp. 89–104.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sSingleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sPubDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>$6$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-25
                nPatternId = 253;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Proulx,</Surname> <Forename>Vincent-Joël</Forename></Author> ,</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> ‘An Uneasy Transition? Linkages between the Law of State Responsibility and the Law Governing the Responsibility of International Organizations’, in <Editor><EForename>Maurizio</EForename> <ESurname>Ragazzi</ESurname></Editor> <ldlEditorDelimiterEds_Back>(ed.),</ldlEditorDelimiterEds_Back> <i>Responsibility of International Organizations. Essays in Memory of Sir Ian Brownlie</i> <ldlCity>(Leiden</ldlCity><ldlPublisherName>Martinus Nijhoff Publishers,</ldlPublisherName> <PubDate><Year>2013</Year>)</PubDate> 109–120.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sSingleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>$6$7$8<{3}>$9</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-25
                nPatternId = 254;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Forename>Romero</Forename> <Surname>Philips</Surname></Author> , Christen,</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> ‘The International Criminal Court & Deterrence. A Report to the Office of Global Criminal Justice, U.S. Department of State’, <i>Stanford Law School: Law & Policy Lab</i>, <PubDate><Month>June</Month> <Year>2016</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sSingleQuoteTitle, sItalicTitle, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-25
                nPatternId = 255;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Sadat,</Surname> <Forename>Leila Nadya</Forename></Author> ,</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> ‘The French Experience’, in <Editor><EForename>M.</EForename> <ESurname>Cherif Bassiouni</ESurname></Editor> <ldlEditorDelimiterEds_Back>(ed.),</ldlEditorDelimiterEds_Back> <i>International Criminal Law. Volume III. International Enforcement</i> <ldlEditionNumber>(3rd edn,</ldlEditionNumber> <ldlCity>Leiden</ldlCity><ldlPublisherName>Martinus Nijhoff Publishers,</ldlPublisherName> <PubDate><Year>2008</Year>),</PubDate> <PageRange>pp. 329–358.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sSingleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sEditionNumber, sLocationName, sPublisherName, sPubDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>$6$7$8$9$10", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-25
                nPatternId = 256;
                //<ldlFirstAuEdCollabGroup><ldlAuthorEditorGroup><Author><Surname>Stockburger</Surname> <Forename>Peter Z</Forename></Author> ,</ldlAuthorEditorGroup></ldlFirstAuEdCollabGroup> ‘Control and Capabilities Test: Toward a New Lex Specialis Governing State Responsibility for Third Party Cyber Incidents’, in <Editor><EForename>H.</EForename> <ESurname>Roigas</ESurname></Editor> , <Editor><EForename>R.</EForename> <ESurname>Jakschis</ESurname></Editor> , <Editor><EForename>L.</EForename> <ESurname>Lindström</ESurname></Editor> and <Editor><EForename>T.</EForename> <ESurname>Minárik</ESurname></Editor> <ldlEditorDelimiterEds_Back>(eds.),</ldlEditorDelimiterEds_Back> <ldlConferenceName><i>Defending the Core, 9th International Conference on Cyber Conflict</i></ldlConferenceName> <ldlCity>(Tallinn</ldlCity><ldlPublisherName>NATO CCD COE Publications,</ldlPublisherName> <PubDate><Year>2017</Year>),</PubDate> <PageRange>pp. 149–162.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sSingleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sConferenceName, sLocationName, sPublisherName, sPubDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5$6$7$8$9", "ldlChapterTitle", "ldlTitleLabel"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-25
                nPatternId = 257;
                //<Author><Surname>Zadeh</Surname> <Forename>L. A.</Forename></Author> <PubDate><Year>1981</Year>.</PubDate> “Possibility theory and soft data analysis.” In Mathematical frontiers of the social and policy sciences, <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>Lauren</EForename> <ESurname>Cobb</ESurname></Editor> and <Editor><EForename>R. M.</EForename> <ESurname>Thrall</ESurname></Editor> , 69–129. <ldlCity>Boulder</ldlCity><ldlState>CO</ldlState><ldlPublisherName>Westview.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sUptoCommaTitle, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6<{3}>$7</{3}>$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-25
                nPatternId = 258;
                //<Author><Surname>Nozière</Surname> <Forename>P</Forename></Author> and <Author><Surname>Hoch</Surname> <Forename>T</Forename></Author> <PubDate><Year>2006</Year>.</PubDate> Modelling fluxes of volatile fatty acids from rumen to portal blood. In Nutrient digestion and utilization in farm animals <ldlEditorDelimiterEds_Front>(ed.</ldlEditorDelimiterEds_Front> <Editor><EForename>E</EForename> <ESurname>Kebreab</ESurname></Editor> , <Editor><EForename>J</EForename> <ESurname>Dijkstra</ESurname></Editor> , <Editor><EForename>A</EForename> <ESurname>Bannink</ESurname></Editor> , <Editor><EForename>WJJ</EForename> <ESurname>Gerrits</ESurname></Editor> and <Editor><EForename>J</EForename> <ESurname>France</ESurname></Editor> ), <PageRange>pp. 40–47.</PageRange> <ldlPublisherName>CABI Publishing</ldlPublisherName><ldlCity>Wallingford</ldlCity><ldlCountry>UK.</ldlCountry>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, "(?:[A-Za-z ]{5,})", sAuthorEditorGroup, sPageRange, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-31
                nPatternId = 259;
                //<Author><Surname>Taylor</Surname> <Forename>C. A.</Forename></Author> , <Author><Surname>Lee</Surname> <Forename>S. J.</Forename></Author> , <Author><Surname>Guterman</Surname> <Forename>N. B.</Forename></Author> , & <Author><Surname>Rice</Surname> <Forename>J. C.</Forename></Author> <PubDate>(<Year>2010</Year>).</PubDate> On cellular automaton approaches to modeling biological cells. In <ldlConferenceName><i>Proceedings of the Advances in Transfusion Safety Symposium</i></ldlConferenceName> <PageRange>(pp. 1–39).</PageRange> <ldlCity>New York</ldlCity><ldlPublisherName>Springer.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sConferenceName, sPageRange, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7$8", "Article_Title", "ldlTitleLabel"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Oct-31
                nPatternId = 260;
                //<Author><Surname>Marino</Surname> <Forename>B.</Forename></Author> Patterns of congenital heart disease and associated cardiac anomalies in children with Down syndrome. In: <Editor><ESurname>Marino</ESurname> <EForename>B</EForename></Editor> , <Editor><ESurname>Pueschel</ESurname> <EForename>SM</EForename></Editor> <ldlEditorDelimiterEds_Back>(eds).</ldlEditorDelimiterEds_Back> Heart Disease in Persons with Down Syndrome, <ldlEditionNumber>4<sup>th</sup> edn.</ldlEditionNumber> <ldlPublisherName>Paul Brookes Publishing</ldlPublisherName><ldlCity>Baltimore</ldlCity>, <PubDate><Year>1996</Year>:</PubDate> 133–140.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sUptoCommaTitle, sEditionNumber, sPublisherName, sLocationName, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>$6$7$8$9<{3}>$10</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-08
                nPatternId = 261;
                //<Author><Forename>M.</Forename> <Surname>Kashiwara</Surname></Author> and <Author><Forename>P. S.</Forename> <Surname>Schapira</Surname></Author> , <i>Sheaves on manifolds</i>, A Series of Comprehensive Studies in Mathematics, <Vol_No>vol. 292,</Vol_No> <ldlEditionNumber>2nd edition,</ldlEditionNumber> <ldlPublisherName>(Springer-Verlag</ldlPublisherName><ldlCity>Berlin</ldlCity>, <PubDate><Year>1994</Year>),</PubDate> x+512.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sItalicTitle, sUptoCommaTitle, sVolumeNumber, sEditionNumber, sPublisherName, sLocationName, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5$6$7$8<{2}>$9</{2}>", "ldlChapterTitle", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-08
                nPatternId = 262;
                //<Author><Surname>Schafer</Surname> <Forename>JL</Forename></Author> <PubDate>(<Year>1997</Year>)</PubDate> Inference by data augmentation. In <i>Analysis of Incomplete Multivariate Data. Monographs on Statistics and Applied Probability 72</i>, <Vol_No>vol. 70,</Vol_No> <PageRange>pp. 89–146</PageRange> [<Editor><EForename>J</EForename> <ESurname>Schafer</ESurname></Editor> , <ldlEditorDelimiterEds_Back>editor].</ldlEditorDelimiterEds_Back> <ldlCity>Boca Raton</ldlCity><ldlState>FL</ldlState><ldlPublisherName>Chapman and Hall/CRC.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sVolumeNumber, sPageRange, sAuthorEditorGroup, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8$9$10", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-08
                nPatternId = 263;
                //<Author><Surname>Cosgrove</Surname> <Forename>DO</Forename></Author> , <Author><Surname>Meire</Surname> <Forename>HB</Forename></Author> , <Author><Surname>Lim</Surname> <Forename>A.</Forename></Author> Chapter 3: Ultrasound: general principles. In: <Editor><ESurname>Adam</ESurname> <EForename>A</EForename></Editor> , <Editor><ESurname>Dixon</ESurname> <EForename>AK</EForename></Editor> , <Editor><ESurname>Grainger</ESurname> <EForename>RC</EForename></Editor> , <Editor><ESurname>Allison</ESurname> <EForename>D</EForename></Editor> , <ldlEditorDelimiterEds_Back>editors.</ldlEditorDelimiterEds_Back> Grainger and Allison~~rsquo~~s diagnostic radiology: a textbook of medical imaging, <ldlEditionNumber>5th ed.</ldlEditionNumber> <ldlCity>Orlando</ldlCity><ldlState>FL</ldlState><ldlPublisherName>Churchill Livingstone</ldlPublisherName>; <PubDate><Year>2008</Year>,</PubDate> <PageRange>pp. 55–77.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sUptoCommaTitle, sEditionNumber, sLocationName, sPublisherName, sPubDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>$6$7$8$9$10", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-08
                nPatternId = 263;
                //<Author><Surname>Gluckman</Surname> <Forename>PD</Forename></Author> , <Author><Surname>Hanson</Surname> <Forename>MA.</Forename></Author> The developmental origins of health and disease: an overview. In <i>Developmental Origins of Health and Disease</i> <ldlEditorDelimiterEds_Front>(eds.</ldlEditorDelimiterEds_Front> <Editor><ESurname>Gluckman</ESurname> <EForename>P</EForename></Editor> , <Editor><ESurname>Hanson</ESurname> <EForename>M</EForename></Editor> ), <PubDate><Year>2006</Year>;</PubDate> <PageRange>pp. 1-5.</PageRange> <ldlPublisherName>Cambridge University Press,</ldlPublisherName> <ldlCity>Cambridge</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sUptoDotTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sPubDate, sPageRange, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}><{2}>$4</{2}>$5$6$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-09
                nPatternId = 264;
                //<Author><Surname>Kuret</Surname> <Forename>J. A.</Forename></Author> & <Author><Surname>Murad</Surname> <Forename>F.</Forename></Author> <PubDate>(<Year>1990</Year>)</PubDate> Adenohypophyseal hormones and related substances. In <i>The Pharmacological Basis of Therapeutics</i>, <Editor><ESurname>Gilman</ESurname> <EForename>A. G.</EForename></Editor> & <Editor><ESurname>Taylor</ESurname> <EForename>P.</EForename></Editor> <ldlEditorDelimiterEds_Back>(eds),</ldlEditorDelimiterEds_Back> <ldlEditionNumber>8th ed.,</ldlEditionNumber> <Vol_No>vol. 2.</Vol_No> <ldlCity>New York</ldlCity><ldlPublisherName>Pergamon,</ldlPublisherName> <PageRange>pp. 20-28.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})({10})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sEditionNumber, sVolumeNumber, sLocationName, sPublisherName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8$9$10$11", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-09
                nPatternId = 265;
                //<Author><Surname>Abdelrasoul</Surname> <Forename>A.</Forename></Author> , <Author><Forename>H.</Forename> <Surname>Doan</Surname></Author> , and <Author><Forename>A.</Forename> <Surname>Lohi</Surname></Author> <PubDate><Year>2013</Year>.</PubDate> “Fouling in membrane filtration and remediation methods.” In <Editor><EForename>H.</EForename> <ESurname>Nakajima</ESurname></Editor> <ldlEditorDelimiterEds_Back>(Ed.),</ldlEditorDelimiterEds_Back> <i>Mass transfer-advances in sustainable energy and environment oriented numerical modeling</i> <ldlEditionNumber>(1st ed.)</ldlEditionNumber>. <ldlPublisherName>IntechOpen</ldlPublisherName>. <Doi>https://doi.org/10.5772/52370.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sEditionNumber, sPublisherName, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-09
                nPatternId = 266;
                //<Author><Surname>Evans</Surname> <Forename>B.</Forename></Author> , <Author><Forename>J. T.</Forename> <Surname>Fredrich</Surname></Author> , and <Author><Forename>T.-F.</Forename> <Surname>Wong</Surname></Author> <PubDate><Year>1990</Year>.</PubDate> <i>The brittle-ductile transition in rocks: Recent experimental and theoretical progress</i>. Geophysical Monograph 56. <ldlCity>Washington</ldlCity><ldlState>DC</ldlState><ldlPublisherName>American Geophysical Union.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sUptoDotTitle, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-09
                nPatternId = 267;
                //<Editor><ESurname>Clini</ESurname> <EForename>C.</EForename></Editor> , <Editor><EForename>I.</EForename> <ESurname>Musu</ESurname></Editor> , and <Editor><EForename>M. L.</EForename> <ESurname>Gullino</ESurname></Editor> , <ldlEditorDelimiterEds_Front>eds.</ldlEditorDelimiterEds_Front> <PubDate><Year>2008</Year>.</PubDate> <i>Sustainable development and environmental management: Experiences and case studies.</i> <ldlMisc>[In Chinese.]</ldlMisc> <ldlCity>Dordrecht</ldlCity><ldlCountry>Netherlands</ldlCountry><ldlPublisherName>Springer.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sMisc, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-09
                nPatternId = 268;
                //<Author><Surname>Eshenaur</Surname> <Forename>S. R.</Forename></Author> , <Author><Forename>J. M.</Forename> <Surname>Kulicki</Surname></Author> , and <Author><Forename>D. R.</Forename> <Surname>Mertz</Surname></Author> <PubDate><Year>1991</Year>.</PubDate> “Retrofitting distortion-induced fatigue cracking of noncomposite steel girder-floorbeam-stringer bridges.” In <ldlConferenceName><i>Proc., 8th Annual Int. Bridge Conf.</i>,</ldlConferenceName> 380–388. <ldlCity>Pittsburgh</ldlCity><ldlPublisherName>Engineers’ Society of Western Pennsylvania.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sConferenceName, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8", "Article_Title", "ldlTitleLabel", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-09
                nPatternId = 269;
                //<Author><Surname>Karam</Surname> <Forename>G. N.</Forename></Author> <PubDate><Year>1991</Year>.</PubDate> “Effect of fiber volume on the strength properties of short fiber reinforced cements with application to bending strength of WFRC.” <ldlTitleLabel>In <ldlTitleLabel><ldlConferenceName>Vol. 1 of <i>Proc., 6th Technical. Conf. of the American Society for Composites,</i></ldlConferenceName> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>A.</EForename> <ESurname>Smith</ESurname></Editor> , 548–557. <ldlCity>Lancaster</ldlCity><ldlState>PA</ldlState><ldlPublisherName>Technomics.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sTitleLabel, sConferenceName, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6<{1}>$7</{1}>$8$9", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-09
                nPatternId = 270;
                //<Author><Surname>Bispo</Surname> <Forename>E</Forename></Author> , <Author><Surname>Franco</Surname> <Forename>D</Forename></Author> , <Author><Surname>Monserrat</Surname> <Forename>L</Forename></Author> , <Author><Surname>González</Surname> <Forename>L</Forename></Author> , <Author><Surname>Pérez</Surname> <Forename>N</Forename></Author> and <Author><Surname>Moreno</Surname> <Forename>T</Forename></Author> <PubDate><Year>2007</Year>.</PubDate> Economic considerations of cull dairy cows fattened for a special market. In <ldlConferenceName>Proceedings of the 53rd International Congress of Meat Science and Technology,</ldlConferenceName> <ldlConferenceDate>5–10 August 2007,</ldlConferenceDate> <ldlCity>Beijing</ldlCity><ldlCountry>China</ldlCountry>, <PageRange>pp. 581–582.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sConferenceName, sConferenceDate, sLocationName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7$8", "Article_Title", "ldlTitleLabel"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-09
                nPatternId = 271;
                //<Author><Surname>YOUN</Surname> <Forename>H</Forename></Author> and <Author><Surname>SHEMYAKIN</Surname> <Forename>A</Forename></Author> <PubDate>(<Year>1999</Year>)</PubDate> Statistical aspects of joint life insurance pricing. <ldlConferenceName><i>Proceedings of the Business and Statistics Section of the American Statistical Association</i>,</ldlConferenceName> <PageRange>pp. 34–38.</PageRange> <ldlPublisherName>American Statistical Association.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sConferenceName, sPageRange, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-09
                nPatternId = 272;
                //<Author><Surname>Béland</Surname> <Forename>Y</Forename></Author> , <Author><Surname>Dale</Surname> <Forename>V</Forename></Author> , <Author><Surname>Dufour</Surname> <Forename>J</Forename></Author> , <Etal><i>et al</i>.</Etal> <PubDate>(<Year>2005</Year>)</PubDate> The Canadian 716 Community Health Survey: building on the success from 717 the past. <ldlConferenceName><i>Proceedings of the American Statistical Association 718 Joint Statistical Meeting, Section on Survey Research Methods</i>,</ldlConferenceName> <ldlConferenceDate>August 2005,</ldlConferenceDate> <PageRange>pp. 2738–2746.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sConferenceName, sConferenceDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-09
                nPatternId = 273;
                //<Author><Surname>Alber</Surname> <Forename>MS</Forename></Author> , <Author><Surname>Kiskowski</Surname> <Forename>MA</Forename></Author> , <Author><Surname>Glazier</Surname> <Forename>JA</Forename></Author> , <Author><Surname>Jiang</Surname> <Forename>Y.</Forename></Author> On cellular automaton approaches to modeling biological cells. In: <ldlConferenceName>Proceedings of the Advances in Transfusion Safety Symposium.</ldlConferenceName> <ldlCity>New York</ldlCity><ldlPublisherName>Springer</ldlPublisherName>; <PubDate><Year>2010</Year>,</PubDate> <PageRange>pp. 1–39.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sUptoDotTitle, sTitleLabel, sConferenceName, sLocationName, sPublisherName, sPubDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5$6$7$8", "Article_Title", "ldlTitleLabel"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-09
                nPatternId = 274;
                //<Author><Surname>Straus</Surname> <Forename>MA</Forename></Author> , <Etal><i>et al</i>.</Etal> Ordinary violence, child abuse and wife beating: what do they have in common? In: <Editor><ESurname>Gelles</ESurname> <EForename>DW</EForename></Editor> , <ldlEditorDelimiterEds_Back>eds.</ldlEditorDelimiterEds_Back> <i>Physical Violence in American Families: Risk Factors and Adaptations to Violence in 8,145 Families</i>. <ldlEditionNumber>3rd ed.</ldlEditionNumber> <ldlCity>New Brunswick</ldlCity><ldlState>NJ</ldlState><ldlPublisherName>Transaction</ldlPublisherName>; <PubDate><Year>1990</Year>.</PubDate> 403–424.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sUptoQuesMarkTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sEditionNumber, sLocationName, sPublisherName, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>$6$7$8$9<{3}>$10</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-09
                nPatternId = 275;
                //<Author><Surname>Alber</Surname> <Forename>M. S.</Forename></Author> , <Author><Surname>Kiskowski</Surname> <Forename>M. A.</Forename></Author> , <Author><Surname>Glazier</Surname> <Forename>J. A.</Forename></Author> & <Author><Surname>Jiang</Surname> <Forename>Y.</Forename></Author> <PubDate>(<Year>2003</Year>)</PubDate> On cellular automaton approaches to modeling biological cells. In: <Editor><EForename>J.</EForename> <ESurname>Rosenthal</ESurname></Editor> and <Editor><EForename>D. S.</EForename> <ESurname>Giliam</ESurname></Editor> <ldlEditorDelimiterEds_Back>(editors),</ldlEditorDelimiterEds_Back> <i>Mathematical Systems Theory in Biology, Communications, Computation, and Finance</i>, <Vol_No>Vol. 134,</Vol_No> <ldlPublisherName>Springer</ldlPublisherName><ldlCity>New York</ldlCity>, <PageRange>pp. 1–39.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sVolumeNumber, sPublisherName, sLocationName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8$9$10", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-11
                nPatternId = 276;
                //<Author><Forename>A.</Forename> <Surname>Beilinson</Surname></Author> and <Author><Forename>A.</Forename> <Surname>Levin</Surname></Author> , The elliptic polylogarithm, in <i>Motives: Proceedings of Symposia in Pure Mathematics</i> <Editor>(<EForename>U.</EForename> <ESurname>Jannsen</ESurname></Editor> , <ldlEditorDelimiterEds_Back>Editor),</ldlEditorDelimiterEds_Back> <Vol_No>vol. 55,</Vol_No> <ldlMisc>Part 2</ldlMisc> <ldlPublisherName>(Springer</ldlPublisherName><ldlCity>Berlin</ldlCity>, <PubDate><Year>1994</Year>),</PubDate> 123–190.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})({10})$", sAuthorEditorGroup, sUptoCommaTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sVolumeNumber, sMisc, sPublisherName, sLocationName, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}><{2}>$4</{2}>$5$6$7$8$9$10<{3}>$11</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-11
                nPatternId = 277;
                //<Author><Surname>Yang</Surname> <Forename>D.Y.</Forename></Author> & <Author><Surname>Frangopol</Surname> <Forename>D.M.</Forename></Author> <PubDate>(<Year>2018a</Year>).</PubDate> Hazard analysis for bridge scour evaluation at watershed level considering climate change impacts. In <ldlConferenceName>Proceedings of the Sixth International Symposium on Life-Cycle Civil Engineering (IALCCE 2018).</ldlConferenceName> <ldlPublisherName>CRC Press,</ldlPublisherName> <ldlCity>Ghent</ldlCity><ldlCountry>Belgium</ldlCountry>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sConferenceName, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7", "Article_Title", "ldlTitleLabel"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-11
                nPatternId = 278;
                //<Author><Surname>Dickson</Surname> <Forename>D.C.M.</Forename></Author> <PubDate>(<Year>2001</Year>).</PubDate> Modern landmarks in actuarial science, <ldlReportNumber>technical report</ldlReportNumber>, <ldlPublisherName>The University of Melbourne,</ldlPublisherName> <ldlCity>Melbourne</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sReportKeyword, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-11
                nPatternId = 279;
                //<Author><Surname>Wang</Surname> <Forename>H.</Forename></Author> Ultrasound: general principles. <ldlThesisKeyword>PhD Thesis</ldlThesisKeyword>, <ldlPublisherName>School of Computing Science</ldlPublisherName>; <PubDate><Year>2011</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sUptoDotTitle, sThesisKeyword, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-11
                nPatternId = 280;
                //<Author><Surname>Moore</Surname> <Forename>C.</Forename></Author> <PubDate>(<Year>2004</Year>),</PubDate> Between diplomacy and interest representation: regional offices in the EU. A German–British comparison. <ldlThesisKeyword>PhD thesis</ldlThesisKeyword>. <ldlCity>Birmingham</ldlCity><ldlPublisherName>University of Birmingham</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoDotTitle, sThesisKeyword, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-11
                nPatternId = 281;
                //<Author><Forename>Y.</Forename> <Surname>Boote</Surname></Author> , On the symmetric square of quaternionic projective space, <ldlThesisKeyword>PhD Thesis</ldlThesisKeyword> <ldlPublisherName>(University of Manchester,</ldlPublisherName> <ldlCountry>UK</ldlCountry>, <PubDate><Year>2016</Year>).</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sUptoCommaTitle, sThesisKeyword, sPublisherName, sLocationName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5$6", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-19
                nPatternId = 282;
                //<Author><Surname>Van Bavel</Surname> <Forename>J. J.</Forename></Author> , <Author><Surname>Mende-Siedlecki</Surname> <Forename>P.</Forename></Author> , <Author><Surname>Brady</Surname> <Forename>W. J.</Forename></Author> , & <Author><Surname>Reinero</Surname> <Forename>D. A.</Forename></Author> <PubDate>(<Year>2016</Year>).</PubDate> Contextual sensitivity in scientific reproducibility. <ldlConferenceName><i>Proceedings of the National Academy of Sciences of the United States of America</i>,</ldlConferenceName> <Vol_No><i>113</i></Vol_No> <Issue_No>(23),</Issue_No> <PageRange>6454–6459.</PageRange> <Doi>doi:10.1073/pnas.1521897113.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sConferenceName, sVolumeNumber, sIssueNumber, sPageRange, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7$8", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-19
                nPatternId = 283;
                //<Author><Surname>Loftus</Surname> <Forename>E. F.</Forename></Author> <PubDate>(<Year>2010</Year>).</PubDate> Afterword: Why parapsychology is not yet ready for prime time. Debating psychic experience: Human potential or human illusion? In <Editor><EForename>S.</EForename> <ESurname>Krippner</ESurname></Editor> & <Editor><EForename>H. L.</EForename> <ESurname>Friedman</ESurname></Editor> <ldlEditorDelimiterEds_Back>(Eds.),</ldlEditorDelimiterEds_Back> <i>Debating psychic experience: Human potential or human illusion?</i> <PageRange>(pp. 211–214).</PageRange> <ldlCity>Santa Barbara</ldlCity><ldlState>CA</ldlState><ldlPublisherName>Praeger/ABC-CLIO.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoQuesMarkTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sPageRange, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6$7$8", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-19
                nPatternId = 284;
                //<Author><Surname>Elias</Surname> <Forename>S</Forename></Author> , <Author><Surname>Garay</Surname> <Forename>A</Forename></Author> <PubDate>(<Year>2004</Year>)</PubDate> Tetrazolium Test (TZ): A Fast Reliable Test to Determine Seed Viability. <ldlPublisherName>Oregon State University Seed Laboratory</ldlPublisherName>. <PageRange>4 p.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPublisherName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-19
                nPatternId = 285;
                //<Author><Surname>Broster</Surname> <Forename>JC</Forename></Author> , <Author><Surname>Walsh</Surname> <Forename>MJ</Forename></Author> , <Author><Surname>Chambers</Surname> <Forename>AJ</Forename></Author> <PubDate>(<Year>2016</Year>)</PubDate> Harvest weed seed control: the influence of harvester set up and speed on efficacy in south-eastern Australia wheat crops. <PageRange>Pages 38–41</PageRange> <i>in</i> <ldlConferenceName>Proceedings of 20th Australasian Weeds Conference.</ldlConferenceName> <ldlCity>Perth</ldlCity><ldlState>WA</ldlState><ldlCountry>Australia</ldlCountry>: <ldlPublisherName>Weeds Society of Western Australia.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPageRange, sTitleLabel, sConferenceName, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4<{1}>$5</{1}>$6$7$8", "ldlBookTitle", "ldlTitleLabel"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-19
                nPatternId = 286;
                //<Author><Surname>Marek</Surname> <Forename>V. W.</Forename></Author> and <Author><Surname>Truszczyński</Surname> <Forename>M.</Forename></Author> <PubDate><Year>1999</Year>.</PubDate> Stable Models and an Alternative Logic Programming Paradigm. In <i>The Logic Programming Paradigm – A 25-Year Perspective</i>, <Editor><EForename>K. R.</EForename> <ESurname>Apt</ESurname></Editor> , <Editor><EForename>V. W.</EForename> <ESurname>Marek</ESurname></Editor> , <Editor><EForename>M.</EForename> <ESurname>Truszczyński</ESurname></Editor> and <Editor><EForename>D. S.</EForename> <ESurname>Warren</ESurname></Editor> , <ldlEditorDelimiterEds_Back>Eds.</ldlEditorDelimiterEds_Back> <ldlPublisherName>Springer Verlag,</ldlPublisherName> 375–398.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7<{3}>$8</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Nov-19
                nPatternId = 287;
                //<RefLabel>9.</RefLabel> <Author><Surname>Newton</Surname> <Forename>CW.</Forename></Author> Dynamics of severe convective storms. In: <Editor><ESurname>David</ESurname> <EForename>A</EForename></Editor> , <Editor><ESurname>Booker</ESurname> <EForename>RD</EForename></Editor> , <Editor><ESurname>Byers</ESurname> <EForename>H</EForename></Editor> , <Etal>et al.,</Etal> <ldlEditorDelimiterEds_Back>eds.</ldlEditorDelimiterEds_Back> <i>Severe Local Storms.</i> <ldlCity>Boston</ldlCity><ldlState>MA</ldlState><ldlPublisherName>American Meteorological Society</ldlPublisherName>; <PubDate><Year>1963</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>$6$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Dec-05
                nPatternId = 288;
                //<RefLabel>6.</RefLabel> <Author><Forename>G.</Forename> <Surname>Stevens</Surname></Author> and <Author><Forename>T.</Forename> <Surname>Woodbridge</Surname></Author> , “Mid-ir fused fiber couplers,” in <i>Components and Packaging for Laser Systems II,</i> <Vol_No>vol. 9730</Vol_No> <ldlPublisherName>(International Society for Optics and Photonics,</ldlPublisherName> <PubDate><Year>2016</Year>),</PubDate> <PageRange>p. 973007.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sDoubleQuoteTitle, sTitleLabel, sItalicTitle, sVolumeNumber, sPublisherName, sPubDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}><{2}>$4</{2}>$5$6$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Dec-09
                nPatternId = 289;
                //<Collab>ASTM International.</Collab> <PubDate><Year>2014</Year>.</PubDate> <i>Standard Test Method for Sieve Analysis of Fine and Coarse Aggregates</i>. ASTM C136/C136M-14. <ldlCity>West Conshohocken</ldlCity><ldlState>PA</ldlState><ldlPublisherName>ASTM International,</ldlPublisherName> 2014. <Doi>https://doi.org/10.1520/C0136_C0136M-14.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sItalicTitle, @"(?:ASTM [^<> ]+\. )", sLocationName, sPublisherName, sUnknownNumber, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6<{2}>$7</{2}>$8", "ldlBookTitle", "ldlMisc", "ldlMisc"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Dec-09
                nPatternId = 290;
                //<Collab>ASTM International.</Collab> <PubDate><Year>2007</Year>.</PubDate> <i>Standard Test Method for Particle-Size Analysis of Soils</i> (Withdrawn 2016). ASTM D422-63(2007)e2. <ldlCity>West Conshohocken</ldlCity><ldlState>PA</ldlState><ldlPublisherName>ASTM International,</ldlPublisherName> 2007. <Doi>https://doi.org/10.1520/D0422-63R07E02.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2}(?:(?:\([^\(\)]{{5,15}}\))[\:\.;, ]*))({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sItalicTitle, @"(?:ASTM [^<> ]+\. )", sLocationName, sPublisherName, sUnknownNumber, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6<{2}>$7</{2}>$8", "ldlBookTitle", "ldlMisc", "ldlMisc"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2019-Dec-10
                nPatternId = 291;
                //<RefLabel>[36]</RefLabel> <Author><Forename>S.</Forename> <Surname>Nakamura</Surname></Author> and <Author><Forename>S. F.</Forename> <Surname>Chichibu</Surname></Author> , <i>Introduction to Nitride Semiconductor Blue Laser and Light-emitting diodes</i>, Chapter 5, <ldlPublisherName><ldlPublisherName>CRC Press</ldlPublisherName></ldlPublisherName><ldlState>NY</ldlState><ldlCountry>USA</ldlCountry> <PubDate><Year>2000</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sItalicTitle, sUptoCommaTitle, sPublisherName, sLocationName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5$6", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jan-11
                nPatternId = 292;
                //<Author><Surname>Jayaprakasha</Surname> <Forename>GK</Forename></Author> , <Author><Surname>Bae</Surname> <Forename>H</Forename></Author> , <Author><Surname>Crosby</Surname> <Forename>K</Forename></Author> , <Author><Surname>Jifon</Surname> <Forename>JL</Forename></Author> and <Author><Surname>Patil</Surname> <Forename>BS</Forename></Author> <PubDate><Year>2012</Year>.</PubDate> Bioactive compounds in peppers and their antioxidant potential. Hispanic Foods: chemistry and bioactive compounds, <ldlEditorDelimiterEds_Front>(ed.</ldlEditorDelimiterEds_Front> <Editor><EForename>MH</EForename> <ESurname>Tunick</ESurname></Editor> and <Editor><EForename>E</EForename> <ESurname>González de Mejía</ESurname></Editor> ), <PageRange>pp. 43-56.</PageRange> <ldlPublisherName>American Chemical Society</ldlPublisherName><ldlCity>Washington</ldlCity><ldlState>DC</ldlState><ldlCountry>USA</ldlCountry>. <Doi>doi: 10.1021/bk-2012-1109.ch004.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoCommaTitle, sAuthorEditorGroup, sPageRange, sPublisherName, sLocationName, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7$8$9", "ldlChapterTitle", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Feb-07
                nPatternId = 293;
                //<RefLabel>1.</RefLabel> <Author><Surname>Gray</Surname> <Forename>P. R.</Forename></Author> , <Author><Surname>Hurst</Surname> <Forename>P. J.</Forename></Author> , <Author><Surname>Lewis</Surname> <Forename>S. H.</Forename></Author> , and <Author><Surname>Meyer</Surname> <Forename>R. G.</Forename></Author> <PubDate>(<Year>2009</Year>).</PubDate> Analysis and Design of Analog Integrated Circuits, <ldlEditionNumber>5th ed.</ldlEditionNumber> <ldlPublisherName>(John Wiley & Sons, Inc.</ldlPublisherName><ldlCountry>USA</ldlCountry>).
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sEditionNumber, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Feb-07
                nPatternId = 294;
                //<RefLabel>2.</RefLabel> <Author><Surname>Borejko</Surname> <Forename>T.</Forename></Author> , and <Author><Surname>Pleskacz</Surname> <Forename>W. A.</Forename></Author> <PubDate>(<Year>2008</Year>).</PubDate> A Resistorless Voltage Reference Source for 90 nm CMOS Technology with Low Sensitivity to Process and Temperature Variations. <ldlConferenceName>11th IEEE Workshop on Design and Diagnostics of Electronic Circuits and Systems,</ldlConferenceName> 1–6.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sConferenceName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4<{1}>$5</{1}>", "Article_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Feb-07
                nPatternId = 295;
                //<RefLabel>10.</RefLabel> <Author><Surname>Takayanagi</Surname> <Forename>I.</Forename></Author> , <Author><Surname>Yoshimura</Surname> <Forename>N.</Forename></Author> , <Author><Surname>Sato</Surname> <Forename>T.</Forename></Author> , <Author><Surname>Matsuo</Surname> <Forename>S.</Forename></Author> , <Author><Surname>Kawaguchi</Surname> <Forename>T.</Forename></Author> , <Author><Surname>Mori</Surname> <Forename>K.</Forename></Author> , <Author><Surname>Nakamura</Surname> <Forename>J.</Forename></Author> <PubDate>(<Year>2013</Year>).</PubDate> A 1-inch Optical Format, 80fps,~~space~~10~~dot~~8Mpixel CMOS Image Sensor Operating in a Pixel-to-ADC Pipelined Sequence Mode. In <ldlConferenceName><i>Proc. Int~~rsquo~~l Image Sensor Workshop</i>,</ldlConferenceName> <PageRange>(pp. 325–328).</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sConferenceName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "Article_Title", "ldlTitleLabel"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Feb-07
                nPatternId = 296;
                //<RefLabel>15.</RefLabel> <Author><Surname>Hamami</Surname> <Forename>S.</Forename></Author> , <Author><Surname>Fleshel</Surname> <Forename>L.</Forename></Author> , <Author><Surname>Yadid-Pecht</Surname> <Forename>O.</Forename></Author> , & <Author><Surname>Driver</Surname> <Forename>R.</Forename></Author> <PubDate>(<Year>2004</Year>).</PubDate> CMOS Aps ImagerEmploying~~space~~3~~dot~~3V 12 bit 6~~dot~~3 ms/s pipelined ADC. <ldlConferenceName>Proceedings of the 2004 International Symposium on Circuits and Systems,</ldlConferenceName> ISCAS’04.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sConferenceName, @"(?:[A-Z][A-Z][A-Z]+[\u2019\u0027][0-9]+\.)$");
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4<{1}>$5</{1}>", "Article_Title", "ldlConferenceName"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Feb-07
                nPatternId = 297;
                //<RefLabel>20.</RefLabel> <Author><Surname>Baker</Surname> <Forename>R. J.</Forename></Author> <PubDate>(<Year>2010</Year>).</PubDate> CMOS: Circuit Design, Layout, and Simulation <ldlEditionNumber>(3rd ed.)</ldlEditionNumber>. <ldlPublisherName>Wiley-IEEE Press</ldlPublisherName>. <Doi>doi:10.1002/9780470891179.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sTitleWithoutEndPeriod, sEditionNumber, sPublisherName, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Feb-07
                nPatternId = 298;
                //<RefLabel>21.</RefLabel> <Editor><ESurname>Norsworthy</ESurname> <EForename>S. R.</EForename></Editor> , <Editor><ESurname>Schreier</ESurname> <EForename>R.</EForename></Editor> , and <Editor><ESurname>Temes</ESurname> <EForename>G. C.</EForename></Editor> <ldlEditorDelimiterEds_Front>(Eds.).</ldlEditorDelimiterEds_Front> <PubDate>(<Year>1997</Year>).</PubDate> Delta-sigma data converters: theory, design, and simulation <Vol_No>(Vol. 97</Vol_No>). <ldlCity>New York</ldlCity><ldlPublisherName>IEEE press.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sTitleWithoutEndPeriod, sVolumeNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Feb-07
                nPatternId = 299;
                //<RefLabel>22.</RefLabel> <Author><Surname>Mahmoodi</Surname> <Forename>A.</Forename></Author> , and <Author><Surname>Joseph</Surname> <Forename>D.</Forename></Author> <PubDate>(<Year>2008</Year>).</PubDate> Pixel-Level Delta-Sigma ADC with Optimized Area and Power for Vertically-Integrated Image Sensors. <ldlConferenceName>Proceedings of the 51st Midwest Symposium on Circuits and Systems MWSCAS’08</ldlConferenceName> <PageRange>(pp.41–44).</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sConferenceName, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Feb-07
                nPatternId = 300;
                //<RefLabel>13.</RefLabel> <Author><Surname>Song</Surname> <Forename>B.</Forename></Author> <PubDate>(<Year>2000</Year>).</PubDate> Nyquist-Rate ADC and DAC. In <i>VLSI Handbook</i>. <Editor><EForename>E. W.</EForename> <ESurname>Chen</ESurname></Editor> <ldlEditorDelimiterEds_Back>(Ed.),</ldlEditorDelimiterEds_Back> <ldlPublisherName>Academic Press</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Apr-13
                nPatternId = 301;
                //<RefLabel>19.</RefLabel> <i>Standard Test Method for Random Vibration Testing of Shipping Containers</i>, ASTM D4728-17 <ldlCity>(West Conshohocken</ldlCity><ldlState>PA</ldlState><ldlPublisherName>ASTM International,</ldlPublisherName> <PubDate><Year>2017</Year>).</PubDate> <Doi>https://doi.org/10.1520/D4728-17.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sRefLabel, sItalicTitle, @"(?:(?:ASTM [A-Z][0-9]+\-[0-9]+)[ ])", sLocationName, sPublisherName, sPubDate, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5$6$7", "ldlBookTitle", "ldlMisc"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-May-20
                nPatternId = 302;
                //<RefLabel>1</RefLabel> <RefPrefix>See</RefPrefix> <Author><Forename>J. P.</Forename> <Surname>Kotter</Surname></Author> , Power and Influence: Beyond Formal Authority <ldlCity>(New York</ldlCity><ldlPublisherName>Free Press,</ldlPublisherName> <PubDate><Year>1985</Year>).</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sTitleWithoutEndPeriod, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-May-21
                nPatternId = 303;
                //<RefLabel>16</RefLabel> <RefPrefix>See</RefPrefix> <Collab>Mary Parker Follett,</Collab> “The Basis of Authority,” in <Editor><EForename>L.</EForename> <ESurname>Urwick</ESurname></Editor> <ldlEditorDelimiterEds_Back>(ed.),</ldlEditorDelimiterEds_Back> Freedom and Coordination: Lectures in Business Organisation by Mary Parker Follett <ldlCity>(London</ldlCity><ldlPublisherName>Management Publications Trust, Ltd.,</ldlPublisherName> <PubDate><Year>1949</Year>),</PubDate> 34–46.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sDoubleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sTitleWithoutEndPeriod, sLocationName, sPublisherName, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8<{3}>$9</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-May-26
                nPatternId = 304;
                //<RefLabel>a)</RefLabel> <Author><Forename>L.</Forename> <Surname>Fu,</Surname></Author> <Author><Forename>K.</Forename> <Surname>Tang,</Surname></Author> <Author><Forename>K.</Forename> <Surname>Song,</Surname></Author> <Author><Forename>P. A.</Forename> <Surname>van Aken,</Surname></Author> <Author><Forename>Y.</Forename> <Surname>Yu,</Surname></Author> <Author><Forename>J.</Forename> <Surname>Maier,</Surname></Author> Nanoscale<PubDate><Year>2014</Year>,</PubDate> <Vol_No>6,</Vol_No> <PageRange>1384;.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sTitleWithoutEndPeriod, sPubDate, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-May-26
                nPatternId = 305;
                //<RefLabel>a)</RefLabel> <Author><Forename>J.</Forename> <Surname>Ding,</Surname></Author> <Author><Forename>H.</Forename> <Surname>Wang,</Surname></Author> <Author><Forename>Z.</Forename> <Surname>Li,</Surname></Author> <Author><Forename>A.</Forename> <Surname>Kohandehghan,</Surname></Author> <Author><Forename>K.</Forename> <Surname>Cui,</Surname></Author> <Author><Forename>Z.</Forename> <Surname>Xu,</Surname></Author> <Author><Forename>B.</Forename> <Surname>Zahiri,</Surname></Author> <Author><Forename>X.</Forename> <Surname>Tan,</Surname></Author> <Author><Forename>E. M.</Forename> <Surname>Lotfabad,</Surname></Author> <Author><Forename>B. C.</Forename> <Surname>Olsen,</Surname></Author> <Author><Forename>D.</Forename> <Surname>Mitlin,</Surname></Author> ACS Nano.<PubDate><Year>2013</Year>,</PubDate> <Vol_No>7,</Vol_No> <PageRange>11004;.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sUptoDotTitle, sPubDate, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-May-26
                nPatternId = 306;
                //<RefLabel>2</RefLabel> <Author><Forename>P.</Forename> <Surname>Murphy</Surname></Author> <Etal><i>et al.</i></Etal>, “Stitching interferometry: a flexible solution for surface metrology,” <i>Opt. Photon. News</i> <Vol_No>14,</Vol_No> <PageRange>38–43</PageRange> <PubDate>(<Year>2003</Year>).</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sDoubleQuoteTitle, sItalicTitle, sVolumeNumber, sPageRange, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5$6", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-May-26
                nPatternId = 307;
                //<RefLabel>5</RefLabel> <Author><Forename>M.</Forename> <Surname>Guizar-Sicairos</Surname></Author> <Etal><i>et al.</i></Etal>, “Measurement of hard x-ray lens wavefront aberrations using phase retrieval,” <i>Appl. Phys. Lett.</i> <Vol_No>98</Vol_No> <Issue_No>(11)</Issue_No> <PubDate>(<Year>2011</Year>).</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sDoubleQuoteTitle, sItalicTitle, sVolumeNumber, sIssueNumber, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4$5$6", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jun-09
                nPatternId = 308;
                //<AuEdGroup><RefLabel>8.</RefLabel> <Author><Forename>A. P.</Forename> <Surname>Hatzes,</Surname></Author></AuEdGroup> “The radial velocity method for the detection of exoplanets,” in <i>Methods of Detecting Exoplanets: 1st Advanced School on Exoplanetary Science,</i> <AuEdGroup><Editor><EForename>V.</EForename> <ESurname>Bozza</ESurname></Editor> , <Editor><EForename>L.</EForename> <ESurname>Mancini</ESurname></Editor> , and <Editor><EForename>A.</EForename> <ESurname>Sozzetti</ESurname></Editor> , <ldlEditorDelimiterEds_Back>eds.</ldlEditorDelimiterEds_Back></AuEdGroup> <ldlPublisherName>(Springer International publishing,</ldlPublisherName> <PubDate><Year>2016</Year>).</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sDoubleQuoteTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}><{2}>$4</{2}>$5$6$7", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jun-09
                nPatternId = 309;
                //<AuEdGroup><Author><Surname>Todd</Surname> <Forename>Johnson</Forename></Author> <ldlAuthorDelimiterAnd>and</ldlAuthorDelimiterAnd> <Author><Forename>Kim</Forename> <Surname>Petrone,</Surname></Author></AuEdGroup> The FASB Cases on Recognition and Measurement, <ldlEditionNumber>Second Edition</ldlEditionNumber> <ldlCity>(New York</ldlCity><ldlPublisherName>John Wiley and Sons, Inc.,</ldlPublisherName> <PubDate><Year>1996</Year>);.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sUptoCommaTitle, sEditionNumber, sLocationName, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jun-10
                nPatternId = 310;
                //<AuEdGroup><Author><Forename>K.</Forename> <Surname>Knop,</Surname></Author></AuEdGroup> Diversity and Self-Determination in International Law <PubDate>(<Year>2002</Year>),</PubDate> 117.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sTitleWithoutEndPeriod, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3<{1}>$4</{1}>", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jun-10
                nPatternId = 311;
                //<AuEdGroup><Author><Forename>K.</Forename> <Surname>Keith,</Surname></Author></AuEdGroup> 'Challenges to the Independence of the International Judiciary: Reflections on the International Court of Justice', <PubDate>(<Year>2017</Year>)</PubDate> 30 LJIL 137.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sSingleQuoteTitle, sPubDate, sUnknownNumber, "(?:[A-Z][A-Z][A-Z][A-Z]+)[ ]", sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3<{1}>$4</{1}><{2}>$5</{2}><{3}>$6</{3}>", "Article_Title", "Vol_No", "Journal_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jun-10
                nPatternId = 312;
                //<AuEdGroup><Author><Forename>E.</Forename> <Surname>McWhinney,</Surname></Author></AuEdGroup> ‘Western and Non-Western Legal Cultures and the International Court of Justice’, <PubDate>(<Year>1987</Year>).</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})$", sAuthorEditorGroup, sSingleQuoteTitle, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jun-10
                nPatternId = 313;
                //<AuEdGroup><Author><Forename>C.</Forename> <Surname>Ostberg</Surname></Author> <ldlAuthorDelimiterAnd>and</ldlAuthorDelimiterAnd> <Author><Forename>M.</Forename> <Surname>Wetstein,</Surname></Author></AuEdGroup> Attitudinal Decision Making in the Supreme Court of Canada <PubDate>(<Year>2011</Year>).</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})$", sAuthorEditorGroup, sTitleWithoutEndPeriod, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }


                //added by Dakshinamoorthy on 2020-Jun-10
                nPatternId = 314;
                //<AuEdGroup><Author><Forename>M.</Forename> <Surname>Foucault,</Surname></Author></AuEdGroup> ‘The Order of Discourse’, in <AuEdGroup><Editor><EForename>R.</EForename> <ESurname>Young</ESurname></Editor> <ldlEditorDelimiterEds_Back>(ed.),</ldlEditorDelimiterEds_Back></AuEdGroup> Untying the Text: A Poststructuralist Reader <PubDate>(<Year>1981</Year>),</PubDate> 48.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sSingleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sTitleWithoutEndPeriod, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>$6<{3}>$7</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jun-16
                nPatternId = 315;
                //<AuEdGroup><Author><Surname>Baglione</Surname> <Forename>M.P.</Forename></Author></AuEdGroup> <PubDate><Year>1976</Year>.</PubDate> Il territorio di Bomarzo. Rome.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoDotTitle_Small);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent_Temp = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>", "ldlBookTitle", "ldlCity"));
                    if (ValidatePublisherLocation(sRefContent_Temp))
                    {
                        sRefContent = sRefContent_Temp;
                        goto LBL_SKIP_PTN;
                    }
                }

                //added by Dakshinamoorthy on 2020-Jun-16
                nPatternId = 316;
                //<AuEdGroup><Author><Surname>Papi</Surname> <Forename>E.</Forename></Author></AuEdGroup> <PubDate><Year>2004</Year>.</PubDate> A New Golden Age? The northern Praefectura Urbi from Severans to Diocletian. in <AuEdGroup><Editor><ESurname>Swain</ESurname> <EForename>S.</EForename></Editor> , <Editor><ESurname>Edwards</ESurname> <EForename>M.</EForename></Editor> <ldlEditorDelimiterEds_Back>(eds).</ldlEditorDelimiterEds_Back></AuEdGroup> Approaching Late Antiquity. The Transformation from Early to Late Empire. Oxford: 53-81.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})({9})$", sAuthorEditorGroup, sPubDate, sUptoQuesMarkTitle, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sUptoDotTitle, sUptoDotTitle, @"(?:[A-Z][a-z][a-z][a-z]+\: )", sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent_Temp = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3$4</{0}><{1}>$5</{1}>$6<{2}>$7$8</{2}><{3}>$9</{3}><{4}>$10</{4}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "ldlCity", "PageRange"));
                    if (ValidatePublisherLocation(sRefContent_Temp))
                    {
                        sRefContent = sRefContent_Temp;
                        goto LBL_SKIP_PTN;
                    }
                }

                //added by Dakshinamoorthy on 2020-Jun-17
                nPatternId = 317;
                //<AuEdGroup><Author><Surname>Jolivet</Surname> <Forename>V.,</Forename></Author> <Author><Surname>Joncheray</Surname> <Forename>C.</Forename></Author></AuEdGroup> <PubDate><Year>2013</Year>.</PubDate> Piammiano (Statonia?). Prospections gaophysiques sur le site de Piammiano (Statonia ?). <i>Chronique des activitas archaologiques de l~~rsquo~~Acole franaaise de Rome, MEFRA</i>: <Website>https://journals.openedition.org/cefr/959?lang=it.</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sUptoDotTitle, sItalicTitle, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent_Temp = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3$4</{0}><{1}>$5</{1}>$6", "Article_Title", "Journal_Title"));
                    sRefContent = sRefContent_Temp;
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jun-17
                nPatternId = 318;
                //<AuEdGroup><Author><Surname>Bertini</Surname> <Forename>M.,</Forename></Author> <Author><Surname>D~~rsquo~~Amico</Surname> <Forename>C.,</Forename></Author> <Author><Surname>Deriu</Surname> <Forename>M.,</Forename></Author> <Author><Surname>Tagliavini</Surname> <Forename>S.,</Forename></Author> <Author><Surname>Vernia</Surname> <Forename>L.</Forename></Author></AuEdGroup> <PubDate><Year>1971</Year>.</PubDate> Note illustrative della Carta Geologica d~~rsquo~~Italia alla scala 1: 100~~dot~~000. <ldlPublisherName>Foglio</ldlPublisherName> 143 “Bracciano”. Roma.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPublisherName, sUnknownNumber, sDoubleQuoteTitle, sUptoDotTitle_Small);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent_Temp = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4<{1}>$5</{1}><{2}>$6</{2}><{3}>$7</{3}>", "ldlBookTitle", "PageRange", "ldlCity", "ldlCity"));
                    if (ValidatePublisherLocation(sRefContent_Temp))
                    {
                        sRefContent = sRefContent_Temp;
                        goto LBL_SKIP_PTN;
                    }
                }

                //added by Dakshinamoorthy on 2020-Jun-17
                nPatternId = 319;
                //<AuEdGroup><Author><Surname>Leucci</Surname> <Forename>G.</Forename></Author></AuEdGroup> <PubDate><Year>2019</Year>.</PubDate> Nondestructive Testing for Archaeology and Cultural Heritage: A practical guide and new perspective. Springer editore <PageRange>pp 217,</PageRange> <ldlISBNNumber>ISBN 978-3-030-01898-6.</ldlISBNNumber>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleWithoutEndPeriod, sPageRange, sISBNNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent_Temp = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6", "ldlBookTitle", "ldlPublisherName"));
                    if (ValidatePublisherLocation(sRefContent_Temp))
                    {
                        sRefContent = sRefContent_Temp;
                        goto LBL_SKIP_PTN;
                    }
                }

                //added by Dakshinamoorthy on 2020-Jun-17
                nPatternId = 320;
                //<AuEdGroup><Author><Surname>Leucci</Surname> <Forename>G.</Forename></Author></AuEdGroup> <PubDate><Year>2020</Year>.</PubDate> Advances in Geophysical Methods Applied to Forensic Investigations: New Developments in Acquisition and Data Analysis Methodologies. <ldlPublisherName>Springer editore,</ldlPublisherName> <PageRange>pp 200,</PageRange> <ldlISBNNumber>ISBN 978-3-030-46241-3.</ldlISBNNumber>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPublisherName, sPageRange, sISBNNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jul-07
                nPatternId = 321;
                //<AuEdGroup><Author><Surname>De Soto,</Surname> <Forename>H.</Forename></Author></AuEdGroup> <PubDate>(<Year>2000</Year>).</PubDate> The Mystery of Capital: Why Capitalism Triumphs in the West and Fails Everywhere Else, <ldlCity>New York</ldlCity><ldlPublisherName>Basic Books.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jul-07
                nPatternId = 322;
                //<AuEdGroup><Author><Surname>Jie,</Surname> <Forename>Gan</Forename></Author></AuEdGroup> <PubDate>(<Year>2008</Year>)</PubDate> Privatization in China: Experiences and Lessons; <ldlPublisherName>Hong Kong University of Science andTechnology</ldlPublisherName>; <Website>http://english.ckgsb.edu.cn/sites/default/files/privatization_in_china.pdf;.</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoSemiColonTitle, sPublisherName, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jul-07
                nPatternId = 323;
                //<AuEdGroup><Author><Surname>Schumpeter</Surname> <Forename>J.A.</Forename></Author></AuEdGroup> <PubDate>(<Year>1961</Year>).</PubDate> The theory of economic development: An inquiry into profits, capital, interest and the business cycle <Vol_No>(Vol.55).</Vol_No> <ldlPublisherName>Transaction Books.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sTitleWithoutEndPeriod, sVolumeNumber, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jul-07
                nPatternId = 324;
                //<AuEdGroup>Taylor Jerry</AuEdGroup> <PubDate>(<Year>Undated</Year>),</PubDate> Sustainable Development: A Model for China? <Website>http://www.cato.org/pubs/chapters/chinamil24.html.</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sUptoQuesMarkTitle, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jul-07
                nPatternId = 325;
                //<AuEdGroup><Author><Surname>Wei,</Surname> <Forename>Pan</Forename></Author></AuEdGroup> <PubDate>(<Year>2007</Year>).</PubDate> The Chinese Model of Development; <Website>http://fpc.org.uk/fsblob/888.pdf.</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sPubDate, sUptoSemiColonTitle, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jul-07
                nPatternId = 326;
                //<AuEdGroup><Author><Surname>Zhang,</Surname> <Forename>Ming</Forename></Author></AuEdGroup> <PubDate>(<Year>2014</Year>).</PubDate> The Transition of China~~rsquo~~s Development Model; Chapter 6 of the book “China~~rsquo~~s Economic Development: Institutions, Geowth and Imbalance” <AuEdGroup><ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>Ming</EForename> <ESurname>Lu</ESurname></Editor> , <Editor><EForename>Zhao</EForename> <ESurname>Chen</ESurname></Editor> , <Editor><EForename>Yongqin</EForename> <ESurname>Wang</ESurname></Editor> , <Editor><EForename>Yang</EForename> <ESurname>Zhang</ESurname></Editor> , <Editor><EForename>Yuan</EForename> <ESurname>Zhang</ESurname></Editor> , and <Editor><EForename>Changyuan</EForename> <ESurname>Luo</ESurname></Editor> , <Editor><EForename>Edward</EForename> <ESurname>Elgar</ESurname></Editor> , <Editor><ESurname>Cheltenham</ESurname> <EForename>UK.</EForename></Editor></AuEdGroup> <Website>http://www.kas.de/upload/dokumente/2011/10/G20_E-Book/chapter_6.pdf.</Website>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoSemiColonTitle, "(?:Chapter 6 of the book )", sDoubleQuoteTitle, sAuthorEditorGroup, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7", "ldlChapterTitle", "ldlMisc", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jul-11
                nPatternId = 327;
                //<AuEdGroup><Author><Surname>Banik,</Surname> <Forename>A.</Forename></Author> <ldlAuthorDelimiterAnd>&</ldlAuthorDelimiterAnd> <Author><Surname>Barai,</Surname> <Forename>M. K.</Forename></Author></AuEdGroup> <PubDate>(<Year>2017</Year>).</PubDate> Introduction, In<AuEdGroup><Editor><ESurname>Arindam</ESurname> <EForename>Banik,</EForename></Editor> <Editor><ESurname>Munim Kumar</ESurname> <EForename>Barai</EForename></Editor> <ldlAuthorDelimiterAnd>&</ldlAuthorDelimiterAnd> <Editor><ESurname>Yasushi</ESurname> <EForename>Suzuki</EForename></Editor> <ldlEditorDelimiterEds_Back>(Eds.)</ldlEditorDelimiterEds_Back></AuEdGroup> Towards A Common Future: Understanding Growth, Sustainability in the Asia-Pacific Region, <ldlPublisherName>Palgrave Macmillan.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5}(?:[^,]+, )?)({6})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sTitleLabel, sAuthorEditorGroup, sUptoCommaTitle, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jul-11
                nPatternId = 328;
                //<AuEdGroup><Author><Surname>Chaudhuri,</Surname> <Forename>K.N.</Forename></Author></AuEdGroup> <PubDate>(<Year>2008</Year>).</PubDate> Foreign Trade and Balance of Payments (1757-1947). In<AuEdGroup><Editor><ESurname>Dharma</ESurname> <EForename>Kumar</EForename></Editor> <ldlAuthorDelimiterAnd>and</ldlAuthorDelimiterAnd> <Editor><ESurname>Meghnad</ESurname> <EForename>Desai</EForename></Editor> <ldlEditorDelimiterEds_Back>(Eds.)</ldlEditorDelimiterEds_Back></AuEdGroup> The Cambridge Economic History of India, <ldlPublisherName>Cambridge University Press</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, sUptoCommaTitle, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jul-23
                nPatternId = 329;
                //<AuEdGroup><RefLabel>12.</RefLabel> <Author><Forename>W.</Forename> <Surname>Kowalski,</Surname></Author></AuEdGroup> Ultraviolet Germicidal Irradiation Handbook: UVGI for Air and Surface Disinfection <ldlPublisherName>(Springer,</ldlPublisherName> <PubDate><Year>2009</Year>).</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sTitleWithoutEndPeriod, sPublisherName, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jul-23
                nPatternId = 330;
                //<AuEdGroup><Author><Surname>Ehmke,</Surname> <Forename>E.</Forename></Author></AuEdGroup> <PubDate>(<Year>2019</Year>)</PubDate> ‘Social security expansion in the South: from welfare regimes to implementation. A study of India and its national rural employment’, in <AuEdGroup><Editor><EForename>C.</EForename> <ESurname>Scherrer</ESurname></Editor> <ldlEditorDelimiterEds_Back>(ed.),</ldlEditorDelimiterEds_Back></AuEdGroup> <Journal_Title><i>Labor and Globalization</i>:</Journal_Title> <Vol_No>Volume 18,</Vol_No> <ldlPublisherName>Rainer Hampp Verlag.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sSingleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sJournalTitle, sVolumeNumber, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7$8", "ldlChapterTitle", "ldlTitleLabel"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Jul-23
                nPatternId = 331;
                //<AuEdGroup><Author><Surname>Choi,</Surname> <Forename>Y. J.</Forename></Author></AuEdGroup> <PubDate>(<Year>2013</Year>)</PubDate> ‘Developmentalism and productivism in East Asian welfare regime’, in <AuEdGroup><Editor><EForename>M.</EForename> <ESurname>Izuhara</ESurname></Editor> <ldlEditorDelimiterEds_Back>(ed.),</ldlEditorDelimiterEds_Back></AuEdGroup> <i>The Handbook on East Asian Social Policy</i>, <ldlCity>Cheltenham</ldlCity><ldlPublisherName>Edward Elgar,</ldlPublisherName> 207–25.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sSingleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8<{3}>$9</{3}>", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Aug-08
                nPatternId = 332;
                //<AuEdGroup><Author><Surname>Anjaria,</Surname> <Forename>Ulka.</Forename></Author></AuEdGroup> <PubDate><Year>2011</Year>.</PubDate> ‘“Why Don~~rsquo~~t You Speak?”: The Narrative Politics of Silence in Senapati, Premchand, and Monica Ali.’ In <i>Colonialism, Modernity, and Literature: A View from India</i>, <AuEdGroup><ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><ESurname>Satya</ESurname> <EForename>P. Mohanty</EForename></Editor> ,</AuEdGroup> 153–70. <ldlCity>New York</ldlCity><ldlPublisherName>Palgrave Macmillan.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sSingleQuoteTitle, sTitleLabel, sItalicTitle,  sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6<{3}>$7</{3}>$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Aug-09
                nPatternId = 333;
                //<AuEdGroup><Collab>_____</Collab></AuEdGroup>. <PubDate><Year>2006</Year>.</PubDate> ‘Visions of a New Banaras and the New Novel in Hindi.’ In <i>Visualizing Space in Banaras: Images, Maps, and the Practice of Representation</i>, <AuEdGroup><ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>Martin</EForename> <ESurname>Gaenszle</ESurname></Editor> and <Editor><EForename>Jarg</EForename> <ESurname>Gengnagel</ESurname></Editor> ,</AuEdGroup> 325–48. <ldlPublisherName>Harrassowitz Verlag</ldlPublisherName><ldlCity>Wiesbaden</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sSingleQuoteTitle, sTitleLabel, sItalicTitle, sAuthorEditorGroup, sUnknownNumber, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6<{3}>$7</{3}>$8$9", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Aug-10
                nPatternId = 334;
                //<AuEdGroup><Author><Surname>Levinas,</Surname> <Forename>E.</Forename></Author></AuEdGroup> <PubDate>(<Year>1969</Year>).</PubDate> <i>Totality and infinity: An essay on exteriority</i> <AuEdGroup><ldlEditorDelimiterEds_Front>(trans,</ldlEditorDelimiterEds_Front> <Editor><EForename>A.</EForename> <ESurname>Lingis</ESurname></Editor></AuEdGroup> ). <ldlCity>Pennsylvania</ldlCity><ldlPublisherName>Duquesne University Press</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sAuthorEditorGroup, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Aug-11
                nPatternId = 335;
                //<AuEdGroup><RefLabel>19.</RefLabel> <Author><Surname>Ghosh</Surname> <Forename>S.</Forename></Author></AuEdGroup> <PubDate>(<Year>2017</Year>).</PubDate> Studies on Entrepreneurship, Regulation and Economic Freedom, <ldlThesisKeyword>Doctoral Dissertation</ldlThesisKeyword>, <ldlPublisherName>West Virginia University</ldlPublisherName><ldlCountry>USA.</ldlCountry>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sUptoCommaTitle, sUptoCommaTitle, sThesisKeyword, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3$4</{0}>$5$6$7", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Aug-11
                nPatternId = 335;
                //<AuEdGroup><Collab>_____</Collab></AuEdGroup>. <PubDate><Year>2004a</Year>.</PubDate> <i>Mansarovar</i> (Selected Stories of Premchand). <Vol_No>Vol. 7.</Vol_No> <ldlCity>New Delhi</ldlCity><ldlPublisherName>Prakashan Sansthan.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sVolumeNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Aug-11
                nPatternId = 336;
                //<AuEdGroup><Collab>_____</Collab></AuEdGroup>. <PubDate><Year>2006</Year>.</PubDate> <i>Karmabhumi</i> (The Field of Action, 1932). <AuEdGroup><ldlEditorDelimiterEds_Front>Translated by</ldlEditorDelimiterEds_Front> <Editor><EForename>Lalit</EForename> <ESurname>Srivastava</ESurname></Editor> .</AuEdGroup> <ldlCity>New Delhi</ldlCity><ldlPublisherName>Oxford University Press</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sAuthorEditorGroup, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5$6", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Aug-11
                nPatternId = 337;
                //<AuEdGroup><Collab>_____</Collab></AuEdGroup>. <PubDate><Year>2016</Year>.</PubDate> <i>Prema</i> (1907). New Delhi: Prabhat Paperbacks.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sItalicTitle, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Aug-11
                nPatternId = 338;
                //<AuEdGroup><Author><Surname>Sinha,</Surname> <Forename>D.,</Forename></Author> <ldlAuthorDelimiterAnd>&</ldlAuthorDelimiterAnd> <Author><Surname>Dey,</Surname> <Forename>D.</Forename></Author></AuEdGroup> <PubDate>(<Year>2018</Year>).</PubDate> Supply Chain Strategies to Sustain Economic and Customer Uncertainties. In <i>Flexible Strategies in VUCA Markets</i> <PageRange>(pp. 139-153).</PageRange> <ldlPublisherName>Springer</ldlPublisherName><ldlCity>Singapore</ldlCity>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sItalicTitle, sPageRange, sPublisherName, sLocationName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}><{2}>$5</{2}>$6$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Aug-11
                nPatternId = 339;
                //<AuEdGroup><Author><Surname>Disalkar,</Surname> <Forename>D.B.</Forename></Author></AuEdGroup> ‘Maratha Vakils with the British at Bombay, Calcutta and Madras’, in <AuEdGroup><Editor><EForename>V.</EForename> <ESurname>Rangacharya</ESurname></Editor> <Etal>et al,</Etal> <ldlEditorDelimiterEds_Back>eds,</ldlEditorDelimiterEds_Back></AuEdGroup> <i>Dr S. Krishnaswami Aiyangar Commemoration Volume</i>, <ldlCity>Madras</ldlCity>, <PubDate><Year>1936</Year>,</PubDate> <PageRange>pp. 26-29.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sSingleQuoteTitle, sTitleLabel, sAuthorEditorGroup, sItalicTitle, sLocationName, sPubDate, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}><{1}>$3</{1}>$4<{2}>$5</{2}>$6$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Dec-01
                nPatternId = 340;
                //<AuEdGroup><Collab>ASTM D6648 - 08,</Collab></AuEdGroup> <PubDate><Year>2016</Year>.</PubDate> Standard Test Method for Determining the Flexural Creep Stiffness of Asphalt Binder Using the Bending Beam Rheometer (BBR). <ldlCity>West Conshohocken</ldlCity><ldlState>PA</ldlState><ldlPublisherName>ASTM International.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle_Small, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Dec-18
                nPatternId = 341;
                //<AuEdGroup><RefLabel>49.</RefLabel> <AuEdGroup><Collab>RS Components Ltd.,</Collab></AuEdGroup> <AuEdGroup>Block Single Core 0. 1 mm diameter Copper Wire. <Website>https://uk.rs-online.com/web/p/copper-wire/3377088/,</Website> <ldlAccessedDateLabel>Accessed</ldlAccessedDateLabel> <ldlAccessedDate>July 18, 2020;</ldlAccessedDate> <PubDate><Year>2020</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sUptoDotTitle, sURLWithLabel, sAccessedDateWithLabel, sPubDate);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                //added by Dakshinamoorthy on 2020-Dec-29
                nPatternId = 342;
                //<AuEdGroup><Author><Surname>Smith,</Surname> <Forename>J. D.,</Forename></Author> <ldlAuthorDelimiterAnd>&</ldlAuthorDelimiterAnd> <Author><Surname>Dishion,</Surname> <Forename>T. J.</Forename></Author></AuEdGroup> <PubDate>(<Year>2013</Year>).</PubDate> Mindful parenting in the development and maintenance of youth psychopathology. In <AuEdGroup><Editor><EForename>J. T.</EForename> <ESurname>Ehrenreich-May</ESurname></Editor> <ldlAuthorDelimiterAnd>&</ldlAuthorDelimiterAnd> <Editor><EForename>B. C.</EForename> <ESurname>Chu</ESurname></Editor> <ldlEditorDelimiterEds_Back>(Eds.),</ldlEditorDelimiterEds_Back></AuEdGroup> Transdiagnostic mechanisms and treatment for youth psychopathology <PageRange>(pp. 138-160).</PageRange> <ldlPublisherName>Guilford Press</ldlPublisherName>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sTitleLabel, sAuthorEditorGroup, @"(?:(?:[^\(<>\. ][^<>\.]{5,}[^\)<>\. ])[\.]? )", sPageRange, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>$7$8", "ldlChapterTitle", "ldlTitleLabel", "ldlBookTitle"));
                    goto LBL_SKIP_PTN;
                }


                //LAST_PTN
                LBL_SKIP_PTN:
                string sTemp = nPatternId.ToString();
                sRefContent = string.Format("<{0}>{1}</{0}>", "Other", sRefContent);

                //post cleanup
                sRefContent = Regex.Replace(sRefContent, "</?ldlFirstAuEdCollabGroup>", "");
                sRefContent = Regex.Replace(sRefContent, "</?ldlAuthorEditorGroup>", "");
                sRefContent = Regex.Replace(sRefContent, "~~space~~", " ");
                sRefContent = Regex.Replace(sRefContent, "~~space~~", " ");
                sRefContent = Regex.Replace(sRefContent, "~~dot~~", ".");
                sRefContent = Regex.Replace(sRefContent, "~~comma~~", ",");
                sRefContent = Regex.Replace(sRefContent, "~~rsquo~~", "’");
                //commented by Dakshinamoorthy on 2020-Jun-29
                //sRefContent = Regex.Replace(sRefContent, "</?AuEdGroup>", "");

            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("DoFullPatternMatching: <ref>" + sRefContent + "</ref> " + ex.Message.ToString());
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesPatterns.cs\DoFullPatternMatching", ex.Message, true, "");
            }
            return true;
        }


        private static bool ValidatePublisherLocation(string sRefTaggedContent)
        {
            string sPubLocPattern = @"<(ldlCity|ldlState|ldlCountry|ldlPublisherName)>((?:(?!</?\1>).)+)</\1>";
            bool bReturnValue = false;

            try
            {
                if (Regex.IsMatch(sRefTaggedContent, sPubLocPattern))
                {
                    MatchCollection mcPubLoc = Regex.Matches(sRefTaggedContent, sPubLocPattern);
                    AutoStructReferencesHelper objRefHelper = new AutoStructReferencesHelper();

                    foreach (Match mPubLoc in mcPubLoc)
                    {
                        string sType = mPubLoc.Groups[1].Value;
                        string sValue = mPubLoc.Groups[2].Value;

                        switch (sType)
                        {
                            case "ldlCity":
                                if (objRefHelper.IsValidCityName(sValue))
                                { bReturnValue = true; }
                                else { return false; }
                                break;
                            case "ldlState":
                                if (objRefHelper.IsValidStateName(sValue))
                                { bReturnValue = true; }
                                else { return false; }
                                break;
                            case "ldlCountry":
                                if (objRefHelper.IsValidCountryName(sValue))
                                { bReturnValue = true; }
                                else { return false; }
                                break;
                            case "ldlPublisherName":
                                if (objRefHelper.IsValidPublisherName(sValue))
                                { bReturnValue = true; }
                                else { return false; }
                                break;
                            default:
                                break;
                        }
                    }
                    return bReturnValue;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesPatterns.cs\ValidatePublisherLocation", ex.Message, true, "");
            }
            return false;
        }



        private static bool CheckPublisherInsideFormattingTag(ref string sRefContent)
        {
            try
            {
                sRefContent = Regex.Replace(sRefContent, "(?:<i>(?:(?!</?i>).)+</i>)", HandlePublisherInsideFormattingTag);
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesPatterns.cs\CheckPublisherInsideFormattingTag", ex.Message, true, "");
            }
            return true;
        }

        private static string HandlePublisherInsideFormattingTag(Match myMatch)
        {
            string sOutputContent = myMatch.Value.ToString();

            try
            {
                if (!Regex.IsMatch(sOutputContent, "<i><ldlPublisherName>(?:(?!</?(?:ldlPublisherName|i)>).)+</ldlPublisherName></i>"))
                {
                    sOutputContent = Regex.Replace(sOutputContent, "</?ldlPublisherName>", "");
                }
            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesPatterns.cs\HandlePublisherInsideFormattingTag", ex.Message, true, "");
            }
            return sOutputContent;
        }


        public static bool DoFullPatternMatching_Post(ref string sRefContent)
        {
            sRefContent = Regex.Replace(sRefContent, "</?(?:Book|Communication|Conference|Journal|Other|Patent|References|Report|Thesis|Web)>", "");
            string sFullPattern = string.Empty;
            int nPatternId = 0;

            sRefContent = Regex.Replace(sRefContent, "</Author> , ", "</Author>, ");
            sRefContent = AutoStructRefCommonFunctions.NormalizeTagSpaces(sRefContent);
            ValidatePublisherLocationInJournal(ref sRefContent);
            sRefContent = AutoStructRefCommonFunctions.NormalizeTagSpaces(sRefContent);

            //int nRegexTimeoutSeconds = 10;

            try
            {
                nPatternId = 1;
                //<Author><Surname>Ke</Surname> <Forename>H</Forename>,</Author> <Author><Surname>Hu</Surname> <Forename>J</Forename>,</Author> <Author><Surname>Xu</Surname> <Forename>X.B</Forename>,</Author> <Author><Surname>Wang</Surname> <Forename>W.F</Forename>,</Author> <Author><Surname>Chen</Surname> <Forename>Y.M</Forename></Author> and <Author><Surname>Zhan</Surname> <Forename>L.T</Forename>,</Author> <PubDate><Year>2017</Year>.</PubDate> <Article_Title>Evolution of saturated hydraulic conductivity with compression and degradation for municipal solid waste. </Article_Title>Waste Manage., <Vol_No>65:</Vol_No> <PageRange>63-74.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})((?:[A-Z][a-z]{{3,}}[\.\,]* )+)({3})({4})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 2;
                //<Author><Surname>Durmusoglu</Surname> <Forename>E</Forename>,</Author> <Author><Surname>Sanchez</Surname> <Forename>I.M</Forename>,</Author> <Author><Surname>Corapcioglu</Surname> <Forename>M.Y</Forename>,</Author> <PubDate><Year>2006</Year>.</PubDate> <Article_Title>Permeability and compression characteristics of municipal solid waste samples. </Article_Title>Environ. <Journal_Title>Geo., </Journal_Title><Vol_No>50:</Vol_No> <PageRange>773–786.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})((?:[A-Z][a-z]+[\.\,]* )+)({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sJournalTitle, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 2;
                //<Author><Surname>Durmusoglu</Surname> <Forename>E</Forename>,</Author> <Author><Surname>Sanchez</Surname> <Forename>I.M</Forename>,</Author> <Author><Surname>Corapcioglu</Surname> <Forename>M.Y</Forename>,</Author> <PubDate><Year>2006</Year>.</PubDate> <Article_Title>Permeability and compression characteristics of municipal solid waste samples. </Article_Title>Environ. <Journal_Title>Geo., </Journal_Title><Vol_No>50:</Vol_No> <PageRange>773–786.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})((?:[A-Z][a-z]+\.? )*(?:[A-Z][a-z]*)[\.,]* )({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sJournalTitle, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3$4<{0}>$5</{0}>$6$7$8", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 3;
                //<Author><Surname>Jiang</Surname> <Forename>NJ</Forename>,</Author> <Author><Surname>Du</Surname> <Forename>YJ</Forename>,</Author> <Author><Surname>Liu</Surname> <Forename>S</Forename>,</Author> <Author><Surname>Wei</Surname> <Forename>ML</Forename>,</Author> <Author><Surname>Horpibulsuk</Surname> <Forename>S</Forename>,</Author> <Author><Surname>Arulrajah</Surname> <Forename>A</Forename></Author> <PubDate>(<Year>2015</Year>)</PubDate> <Article_Title>Multi-scale laboratory evaluation of the physical, mechanical and microstructural properties of soft highway subgrade soil stabilized with calcium carbide residue. </Article_Title>Can. Geotech. J. <Vol_No>53</Vol_No> <Issue_No>(3):</Issue_No> <PageRange>373-383.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})((?:[A-Z][a-z]+\.? )*(?:[A-Z][a-z]*)[\.,]* )({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 3;
                //<Author><Surname>Lee</Surname> <Forename>K.-Y</Forename>,</Author> and <Author><Surname>Heo</Surname> <Forename>T.-R</Forename></Author> <PubDate><Year>2000</Year>.</PubDate> <Article_Title>Survival of Bifidobacterium longum immobilized in calcium alginate beads in simulated gastric juices and bile salt solution.</Article_Title> Appl. Environ. Microbl. <Vol_No>66:</Vol_No> <PageRange>869–873.</PageRange> <Doi>doi:10.1128/AEM.66.2.869-873.2000.</Doi> <PubMedIdLabel>PMID:</PubMedIdLabel> <PubMedIdNumber>10653768</PubMedIdNumber>.
                sFullPattern = string.Format(@"^({0})({1})({2})((?:[A-Z][a-z]+\.? )*(?:[A-Z][a-z]*)[\.,]* )({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sVolumeNumber, sPageRange, sDoiNumber, sPubMedIdWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7$8", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 3;
                //<Author><Surname>Abadulla</Surname> <Forename>E</Forename>,</Author> <Author><Surname>Tzanov</Surname> <Forename>T</Forename>,</Author> <Author><Surname>Costa</Surname> <Forename>S</Forename>,</Author> <Author><Surname>Robra</Surname> <Forename>K.H</Forename>,</Author> <Author><Surname>Cavaco-Paulo</Surname> <Forename>A</Forename>,</Author> and <Author><Surname>Gübitz</Surname> <Forename>G.M</Forename></Author> <PubDate><Year>2000</Year>.</PubDate> <Article_Title>Decolorization and detoxification of textile dyes with a laccase from Trametes hirsuta.</Article_Title> Appl Environ Microb. <Vol_No>66</Vol_No> <Issue_No>(8):</Issue_No> <PageRange>3357-3362.</PageRange> <Doi>Doi: 10.1128/AEM.66.8.3357-3362.2000.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})((?:[A-Z][a-z]+\.? )*(?:[A-Z][a-z]*)[\.,]* )({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sVolumeNumber, sIssueNumber, sPageRange, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7$8", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 3;
                //<Author><Surname>Pedersen</Surname> <Forename>K</Forename>,</Author> and <Author><Surname>Tannock</Surname> <Forename>G</Forename></Author> <PubDate><Year>1989</Year>.</PubDate> <Article_Title>Colonization of the porcine gastrointestinal tract by lactobacilli.</Article_Title> Appl. Environ. Microb. <Vol_No>55:</Vol_No> <PageRange>279–283.</PageRange> <PubMedIdLabel>PMID:</PubMedIdLabel> <PubMedIdNumber>2719474</PubMedIdNumber>.
                sFullPattern = string.Format(@"^({0})({1})({2})((?:[A-Z][a-z]+\.? )*(?:[A-Z][a-z]*)[\.,]* )({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sVolumeNumber, sPageRange, sPubMedIdWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 3;
                //<Author><Surname>Kuhn</Surname> <Forename>K.S</Forename>,</Author> <Author><Surname>Stehle</Surname> <Forename>P</Forename>,</Author> and <Author><Surname>Furst</Surname> <Forename>P</Forename></Author> <PubDate><Year>1996</Year>.</PubDate> <Article_Title>Glutamine content of protein and peptide-based enteral products.</Article_Title> J. Parenter. Enteral. Nutr. <Vol_No>20:</Vol_No> <PageRange>292-295.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})((?:[A-Z][a-z]*\.? )*(?:[A-Z][a-z]*)[\.,]* )({3})({4})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sVolumeNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }



                nPatternId = 3;
                //<Author><Surname>Bo</Surname> <Forename>MW</Forename>,</Author> <Author><Surname>Arulrajah</Surname> <Forename>A</Forename>,</Author> <Author><Surname>Horpibulsuk</Surname> <Forename>S</Forename>,</Author> <Author><Surname>Leong</Surname> <Forename>M</Forename>,</Author> <Author><Surname>Disfani</Surname> <Forename>MM</Forename></Author> <PubDate>(<Year>2014</Year>)</PubDate> <Article_Title>Densification of land reclamation sands by deep vibratory compaction. </Article_Title>J Mater Civ Eng <Vol_No>06014016</Vol_No> <Issue_No>(1-6).</Issue_No>
                sFullPattern = string.Format(@"^({0})({1})({2})([A-Za-z ]{{5,}})({3})({4})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sVolumeNumber, sIssueNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 3;
                //<Author><Surname>Khatami</Surname> <Forename>HR</Forename>,</Author> <Author><Surname>O’Kelly</Surname> <Forename>BC</Forename></Author> <PubDate>(<Year>2012</Year>)</PubDate> <Article_Title>Improving mechanical properties of sand using biopolymers. </Article_Title>J Geotech Geoenviron Eng <Vol_No>139</Vol_No> <Issue_No>(8):</Issue_No> <PageRange>1402-1406</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})([A-Za-z ]{{5,}})({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }


                nPatternId = 3;
                //<Author><Surname>Shanmuganathan</Surname> <Forename>T</Forename>,</Author> <Author><Surname>Samarasinghe</Surname> <Forename>K</Forename>,</Author> and <Author><Surname>Wenk</Surname> <Forename>C</Forename></Author> <PubDate><Year>2004</Year>.</PubDate> <Article_Title>Supplemental enzymes, yeast culture and effective micro-organism culture to enhance the performance of rabbits fed diets containing high levels of rice bran.</Article_Title> Asian Austral <Journal_Title>J Anim.</Journal_Title> <Vol_No>17</Vol_No> <Issue_No>(5):</Issue_No> <PageRange>678-683.</PageRange> <Doi>Doi:10.5713/ajas.2004.678.</Doi>
                sFullPattern = string.Format(@"^({0})({1})({2})([A-Za-z ]{{5,}})({3})({4})({5})({6})({7})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sJournalTitle, sVolumeNumber, sIssueNumber, sPageRange, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7$8$9", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }



                nPatternId = 4;
                //<Author><Surname>Etemadi</Surname> <Forename>O</Forename>,</Author> <Author><Surname>Petrisor</Surname> <Forename>IG</Forename>,</Author> <Author><Surname>Kim</Surname> <Forename>D</Forename>,</Author> <Author><Surname>Wan</Surname> <Forename>MW</Forename>,</Author> <Author><Surname>Yen</Surname> <Forename>TF</Forename></Author> <PubDate>(<Year>2003</Year>)</PubDate> <Article_Title>Stabilization of metals in subsurface by biopolymers: Laboratory drainage flow studies. </Article_Title>Soil Sediment Contamin., <Vol_No>12</Vol_No> <Issue_No>(5):</Issue_No> <PageRange>647-661.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})([^0-9<>\.,]{{5,}}[\.,]+ )({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 5;
                //<Author><Surname>Khan</Surname> <Forename>TA</Forename>,</Author> <Author><Surname>Taha</Surname> <Forename>MR</Forename>,</Author> <Author><Surname>Firoozi</Surname> <Forename>AA</Forename>,</Author> <Author><Surname>Firoozi</Surname> <Forename>AA</Forename></Author> <PubDate>(<Year>2016</Year>)</PubDate> <Article_Title>Strength tests of enzyme-treated illite and black soil mixtures. </Article_Title>Proc of the ICE Engineering <Journal_Title>Sustainability  </Journal_Title><Vol_No>169</Vol_No> <Issue_No>(5):</Issue_No> <PageRange>214-222.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})([^0-9<>\.,]{{5,}} )({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sJournalTitle, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7$8", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 6;
                //<Author><Surname>Latifi</Surname> <Forename>N</Forename>,</Author> <Author><Surname>Meehan</Surname> <Forename>CL</Forename>,</Author> <Author><Surname>Majid</Surname> <Forename>MZA</Forename>,</Author> <Author><Surname>Horpibulsuk</Surname> <Forename>S</Forename></Author> <PubDate>(<Year>2016a</Year>)</PubDate> <Article_Title>Strengthening montmorillonitic and kaolinitic clays using a calcium-based non-traditional additive: A micro-level study. </Article_Title>Applied Clay <Journal_Title>Sci,  </Journal_Title><Website>http://dx.<Doi>doi.org/10.1016/j.clay.2016.06.004</Doi></Website>
                sFullPattern = string.Format(@"^({0})({1})({2})([^0-9<>\.,]{{5,}} )({3})({4})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sJournalTitle, sURLWithLabel);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 7;
                //<Author><Surname>Nugent</Surname> <Forename>RA</Forename></Author> <PubDate>(<Year>2007</Year>)</PubDate> The effect of exopolymers on the compressibility and shear strength of kaolinite. <ldlPublisherName>Louisiana State University</ldlPublisherName>, <ldlThesisKeyword>PhD thesis</ldlThesisKeyword>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sPublisherName, sThesisKeyword);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>$4$5", "Article_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 8;
                //<Author><Surname>Sharma</Surname> <Forename>B</Forename>,</Author> <Author><Surname>Bora</Surname> <Forename>PK</Forename></Author> <PubDate>(<Year>2003</Year>)</PubDate> Plastic Limit, Liquid Limit and Undrained Shear Strength of Soil – Reappraisal. J Geotech Geoenviron Eng <Vol_No>129</Vol_No> <Issue_No>(8):</Issue_No> <PageRange>774-777.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})([^0-9<>\.,]{{5,}} )({3})({4})({5})$", sAuthorEditorGroup, sPubDate, sUptoDotTitle, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5$6$7", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 9;
                //<Author><Surname>Castel</Surname> <Forename>A</Forename>,</Author> and <Author><Surname>Nasser</Surname> <Forename>A</Forename></Author> <PubDate>(<Year>2015</Year>).</PubDate> <Article_Title>“Influence of pre‐existing oxides layer and interface condition with carbonated concrete on active reinforcing steel corrosion.”</Article_Title> <i>Materi. Corros.</i>, <Vol_No>66</Vol_No> <Issue_No>(3),</Issue_No> <PageRange>206-214.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sItalicTitle, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 10;
                //<Author><Surname>Radovic</Surname> <Forename>M</Forename>,</Author> <Author><Surname>Ghonima</Surname> <Forename>O</Forename>,</Author> and <Author><Surname>Schumacher</Surname> <Forename>T</Forename></Author> <PubDate>(<Year>2017</Year>).</PubDate> <Article_Title>“Data mining of bridge concrete deck parameters in the national bridge inventory by two-step cluster analysis.”</Article_Title> ASCE-ASME Journal of Risk and Uncertainty in Engineering Systems, Part B: <Journal_Title>Mechanical Engineering,</Journal_Title> <Vol_No>3</Vol_No> <Issue_No>(2),</Issue_No> <PageRange>F4016004.</PageRange>
                sFullPattern = string.Format(@"^({0})({1})({2})([^<>]+)({3})({4})({5})({6})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sJournalTitle, sVolumeNumber, sIssueNumber, sPageRange);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>$5$6$7$8", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 11;
                //<Author><Surname>Youssef</Surname> <Forename>A. S</Forename></Author> and <Author><Surname>Kahil</Surname> <Forename>M. A</Forename></Author> <PubDate>(<Year>2016</Year>)</PubDate> “Solar sludge drying for Medina Al-Munawarah sewage treatment plant in the Kingdom of Saudi Arabia” J. Environ. Eng., <Doi>10.1061/(<ldlPublisherName>ASCE</ldlPublisherName>)EE.1943-7870.0001152,</Doi> 5016006.
                sFullPattern = string.Format(@"^({0})({1})({2})([^<>0-9]+)({3})({4})$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sDoiNumber, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>", "Article_Title", "Journal_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 12;
                //<Author><Surname>Zhang</Surname> <Forename>A</Forename></Author> , <Author><Surname>Wang</Surname> <Forename>K. C. P</Forename></Author> , <Author><Surname>Ji</Surname> <Forename>R</Forename></Author> and <Author><Surname>Li</Surname> <Forename>Q</Forename></Author> <PubDate>(<Year>2016a</Year>).</PubDate> “Efficient system of cracking-detection algorithms with 1-mm 3D-surface models and performance measures.” Journal of Computing in Civil Engineering, <Doi>10.1061/(ASCE)CP.1943-5487.0000581,</Doi> 04016020.1-04016020.16.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})( [0-9]+\.[0-9]+[\-\u2013][0-9]+\.[0-9]+\.?)$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sUptoCommaTitle, sDoiNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5<{2}>$6</{2}>", "Article_Title", "Journal_Title", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 13;
                //<Author><Surname>Mangalathu</Surname> <Forename>S</Forename></Author> , and <Author><Surname>Jeon</Surname> <Forename>J. -S</Forename></Author> <PubDate>(<Year>2018</Year>).</PubDate> “Evaluation of adjustment factors to account for the effect of horizontal curvature on the seismic response of California concrete bridges.” <i>Earthquake Spectra</i>, in press.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})( [Ii]n [Pp]ress\.)$", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle, sItalicTitle);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}><{1}>$4</{1}>$5", "Article_Title", "Journal_Title"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 13;
                //<Other><Author><Surname>Sadrmomtazi</Surname> <Forename>A</Forename></Author> , <Author><Surname>Tahmouresi</Surname> <Forename>B</Forename></Author> and <Author><Surname>Kohani Khoshkbijari</Surname> <Forename>R</Forename></Author> <PubDate>(<Year>2017</Year>).</PubDate> <Article_Title>“Effect of fly ash and silica fume on transition zone, pore structure and permeability of concrete”. </Article_Title><Journal_Title>Mag. Concr. Res.,</Journal_Title>1-14.</Other>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sPubDate, sArticleTitle, sJournalTitle, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3$4<{0}>$5</{0}>", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 14;
                //<RefLabel>[21]</RefLabel> <Author><Surname>Bai</Surname> <Forename>D</Forename>.;</Author> <Author><Surname>Zhang</Surname> <Forename>J</Forename>.;</Author> <Author><Surname>Jin</Surname> <Forename>Z</Forename>.;</Author> <Author><Surname>Bian</Surname> <Forename>H</Forename>.;</Author> <Author><Surname>Wang</Surname> <Forename>K</Forename>.;</Author> <Author><Surname>Wang</Surname> <Forename>H</Forename>.;</Author> <Author><Surname>Liang</Surname> <Forename>L</Forename>.;</Author> <Author><Surname>Wang</Surname> <Forename>Q</Forename>.;</Author> <Author><Surname>Liu</Surname> <Forename>S. F.</Forename>,</Author> <Article_Title>Interstitial</Article_Title> <Journal_Title><i>ACS Energy Letters</i></Journal_Title> <PubDate><Year>2018</Year>,</PubDate> 970.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})$", sAuthorEditorGroup, sArticleTitle, sJournalTitle, sPubDate, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3$4<{0}>$5</{0}>", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 15;
                //<RefLabel>[22]</RefLabel> <Author><Surname>Zeng</Surname> <Forename>Q</Forename>.;</Author> <Author><Surname>Zhang</Surname> <Forename>X</Forename>.;</Author> <Author><Surname>Feng</Surname> <Forename>X</Forename>.;</Author> <Author><Surname>Lu</Surname> <Forename>S</Forename>.;</Author> <Author><Surname>Chen</Surname> <Forename>Z</Forename>.;</Author> <Author><Surname>Yong</Surname> <Forename>X</Forename>.;</Author> <Author><Surname>Redfern</Surname> <Forename>S. A. T</Forename>.;</Author> <Author><Surname>Wei</Surname> <Forename>H</Forename>.;</Author> <Author><Surname>Wang</Surname> <Forename>H</Forename>.;</Author> <Author><Surname>Shen</Surname> <Forename>H</Forename>.;</Author> <Author><Surname>Zhang</Surname> <Forename>W</Forename>.;</Author> <Author><Surname>Zheng</Surname> <Forename>W</Forename>.;</Author> <Author><Surname>Zhang</Surname> <Forename>H</Forename>.;</Author> <Author><Surname>Tse</Surname> <Forename>J. S</Forename>.;</Author> <Author><Surname>Yang</Surname> <Forename>B.</Forename>,</Author> <Journal_Title><i>Adv Mater</i></Journal_Title> <PubDate><Year>2018</Year>,</PubDate> <i>30</i>.
                sFullPattern = string.Format(@"^({0})({1})({2})({3})$", sAuthorEditorGroup, sJournalTitle, sPubDate, @"(?:<i>[0-9]+</i>\.)");
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3<{0}>$4</{0}>", "PageRange"));
                    goto LBL_SKIP_PTN;
                }

                nPatternId = 16;
                //<Author><Surname>Todini</Surname> <Forename>E.</Forename>,</Author> and <Author><Forename>S.</Forename> <Surname>Pilati</Surname></Author> <PubDate><Year>1988</Year>.</PubDate> <ldlChapterTitle>“A gradient algorithm for the analysis of pipe networks.”</ldlChapterTitle> <ldlTitleLabel>In</ldlTitleLabel> <ldlBookTitle>Vol. 1 of Computer applications in water supply: System analysis and simulation,</ldlBookTitle> <ldlEditorDelimiterEds_Front>edited by</ldlEditorDelimiterEds_Front> <Editor><EForename>B.</EForename> <ESurname>Coulbeck</ESurname></Editor> and <Editor><EForename>C.H.</EForename> <ESurname>Orr</ESurname>,</Editor> 1–20. <ldlCity>London</ldlCity><ldlPublisherName>Wiley.</ldlPublisherName>
                sFullPattern = string.Format(@"^({0})({1})({2})({3})({4})({5})({6})({7})({8})$", sAuthorEditorGroup, sPubDate, sChapterTitle, sTitleLabel, sBookTitle, sAuthorEditorGroup, sUnknownNumber, sLocationName, sPublisherName);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2$3$4$5$6<{0}>$7</{0}>$8$9", "PageRange"));
                    goto LBL_SKIP_PTN;
                }



            LBL_SKIP_PTN:


                nPatternId = 11;
                //<Author><Surname>Rublee</Surname> <Forename>E</Forename></Author> , <Author><Surname>Rabaud</Surname> <Forename>V</Forename></Author> , <Author><Surname>Konolige</Surname> <Forename>K</Forename></Author> , and <Author><Surname>Bradski</Surname> <Forename>G</Forename></Author> <PubDate>(<Year>2011</Year>).</PubDate> “ORB: An efficient alternative to SIFT or SURF.” Computer Vision (<ldlPublisherName>ICCV</ldlPublisherName>), <ldlConferenceName>2011 IEEE International Conference on,</ldlConferenceName> <ldlPublisherName>IEEE</ldlPublisherName>, 2564–2571.
                sFullPattern = string.Format(@"^({0})({1})({2})", sAuthorEditorGroup, sPubDate, sDoubleQuoteTitle);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>", "Article_Title"));
                }

                nPatternId = 11;
                //<Author><Surname>Hagerstrand</Surname> <Forename>T</Forename></Author> “Impact of Transport on the Quality of Life.” <ldlConferenceName>Proc., This paper was presented at the 5th International Symposium on Theory and Practice in Transport Economics,</ldlConferenceName> <ldlPublisherName>Transport in the</ldlPublisherName> 1980-1990 Decade, Athens, <PubDate><Month>October</Month>, <Year>1973</Year>.</PubDate>
                sFullPattern = string.Format(@"^({0})({1})", sAuthorEditorGroup, sDoubleQuoteTitle);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>", "Article_Title"));
                }

                nPatternId = 12;
                //<Author><Surname>Rublee</Surname> <Forename>E</Forename></Author> , <Author><Surname>Rabaud</Surname> <Forename>V</Forename></Author> , <Author><Surname>Konolige</Surname> <Forename>K</Forename></Author> , and <Author><Surname>Bradski</Surname> <Forename>G</Forename></Author> <PubDate>(<Year>2011</Year>).</PubDate> “ORB: An efficient alternative to SIFT or SURF.” Computer Vision (<ldlPublisherName>ICCV</ldlPublisherName>), <ldlConferenceName>2011 IEEE International Conference on,</ldlConferenceName> <ldlPublisherName>IEEE</ldlPublisherName>, 2564–2571.
                sFullPattern = string.Format(@"({0})({1})({2})$", sConferenceName, sPublisherName, sUnknownNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1$2<{0}>$3</{0}>", "PageRange"));
                }

                nPatternId = 13;
                //<Author><Surname>Olivito</Surname> <Forename>R.S</Forename></Author> and <Author><Surname>Zuccarello</Surname> <Forename>F.A</Forename></Author> <PubDate>(<Year>2010</Year>).</PubDate> <Article_Title>“An experimental study on the tensile strength of steel fiber reinforced concrete”.</Article_Title> <Journal_Title>Compos.</Journal_Title> Part B- Eng., <Vol_No>41</Vol_No> <Issue_No>(3),</Issue_No> <PageRange>246-255.</PageRange>
                sFullPattern = string.Format(@"({0})({1})({2})", sJournalTitle, @"(?: (?:[A-Za-z\- ]+[\.,]+) )", sVolumeNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3", "Journal_Title"));
                }

                nPatternId = 14;
                //<Author><Surname>Xie</Surname> <Forename>Y</Forename>,</Author> and <Author><Surname>Zhang</Surname> <Forename>J</Forename></Author> <PubDate>(<Year>2017</Year>).</PubDate> <Article_Title>“Optimal Design of Seismic Protective Devices for Highway Bridges Using Performance Based Methodology and Multi-objective Genetic Optimization.” </Article_Title>J. Bridge Eng., <Vol_No>22</Vol_No> <Issue_No>(3):</Issue_No> <eLocation_Id>04016129</eLocation_Id> <PageRange>1-13.</PageRange>
                sFullPattern = string.Format(@"({0})({1})({2})", sArticleTitle, @"(?:[A-Z&a-z\., ]{5,})", sVolumeNumber);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3", "Journal_Title"));
                }

                nPatternId = 15;
                //<Author><Surname>DeFronzo</Surname> <Forename>R. A.</Forename></Author> <PubDate>(<Year>1988</Year>).</PubDate> <Article_Title>Lilly Lecture 1987. The triumvirate: Beta-cell, muscle, liver.</Article_Title> A collusion responsible for NIDDM. <Journal_Title><i>Diabetes,</i></Journal_Title> <Vol_No>37,</Vol_No> <PageRange>667–687.</PageRange> <Doi>https://doi.org/10.2337/diab.37.6.667.</Doi>
                sFullPattern = string.Format(@"({0})({1})({2})", sArticleTitle, sUptoDotTitle, sJournalTitle);
                if (Regex.IsMatch(sRefContent, sFullPattern))
                {
                    sRefContent = Regex.Replace(sRefContent, sFullPattern, string.Format("$1<{0}>$2</{0}>$3", "Article_Title"));
                }


                sRefContent = string.Format("<{0}>{1}</{0}>", "Other", sRefContent);
                //added by Dakshinamoorthy on 2020-Jun-29
                sRefContent = Regex.Replace(sRefContent, "</?AuEdGroup>", "");
            }
            catch (Exception ex)
            {

                //System.Windows.Forms.MessageBox.Show("DoFullPatternMatching_Post: <ref>" + sRefContent + "</ref> " + ex.Message.ToString());
                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\DoFullPatternMatching_Post", ex.Message, true, "");
            }
            return true;
        }

        private static bool ValidatePublisherLocationInJournal(ref string sRefContent)
        {
            try
            {
                //<Author><Surname>Castel</Surname> <Forename>A</Forename>,</Author> and <Author><Surname>Nasser</Surname> <Forename>A</Forename></Author> <PubDate>(<Year>2015</Year>).</PubDate> <Article_Title>“Influence of pre‐existing oxides layer and interface condition with carbonated concrete on active reinforcing steel corrosion.” </Article_Title><Journal_Title><i>Materi.</i> </Journal_Title><ldlCity><i>Corros</i></ldlCity><i>.</i>, <Vol_No>66</Vol_No> <Issue_No>(3),</Issue_No> <PageRange>206-214.</PageRange>
                sRefContent = Regex.Replace(sRefContent, @"<Journal_Title><i>((?:(?!</?i>).)+)</i></Journal_Title> <ldlCity><i>((?:(?!</?i>).)+)</i></ldlCity>", "<i>$1 $2</i>");


            }
            catch (Exception ex)
            {

                AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferencesHelper.cs\ValidatePublisherLocationInJournal", ex.Message, true, "");
            }
            return true;
        }

    }
}
