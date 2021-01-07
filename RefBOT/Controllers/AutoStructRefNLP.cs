using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenNLP;
using System.IO;

namespace RefBOT.Controllers
{
    public partial class AutoStructRefNLP
    {
        private OpenNLP.Tools.SentenceDetect.MaximumEntropySentenceDetector mSentenceDetector;
        private OpenNLP.Tools.Tokenize.EnglishMaximumEntropyTokenizer mTokenizer;
        private OpenNLP.Tools.PosTagger.EnglishMaximumEntropyPosTagger mPosTagger;
        private OpenNLP.Tools.Chunker.EnglishTreebankChunker mChunker;
        private OpenNLP.Tools.Parser.EnglishTreebankParser mParser;
        private OpenNLP.Tools.NameFind.EnglishNameFinder mNameFinder;
        private OpenNLP.Tools.Lang.English.TreebankLinker mCoreferenceFinder;
        public string gModelPath = string.Empty;

        public bool OpenNLP_SentenceSplit(string sRefRawContent, ref List<string> sRefOutput, string sModelPath, ref string sErrMsg)
        {
            try
            {
                gModelPath = sModelPath;

                //added by Dakshinamoorthy on 2019-Jan-04
                if (!File.Exists(Path.Combine(gModelPath, "EnglishSD.nbin")))
                {
                    AutoStructRefCommonFunctions.ShowErrorMessage(@"ERROR: AutoStructReferences.cs\AutoStructRefFunction", 
                        string.Format("The file '{0}' not found, Please check.", Path.Combine(gModelPath, "EnglishSD.nbin")), true, "");
                    return false;
                }

                
                sRefOutput = SplitSentences(sRefRawContent).ToList();
            }
            catch (Exception ex)
            {
                sErrMsg += ex.Message;
            }
            return true;
        }

        private string[] SplitSentences(string paragraph)
        {
            if (mSentenceDetector == null)
            {
                mSentenceDetector = new OpenNLP.Tools.SentenceDetect.EnglishMaximumEntropySentenceDetector(gModelPath + "EnglishSD.nbin");
            }
            return mSentenceDetector.SentenceDetect(paragraph);
        }

    }
}
