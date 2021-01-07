using System;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using System.Linq;
//using DomainModules;
//using Npgsql;
using System.Xml;
using System.Collections;
using System.Net.NetworkInformation;
//using Microsoft.VisualBasic.FileIO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
//using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Data.SqlClient;
using System.Data;
//using System.Windows.Forms;

namespace RefBOT.Controllers
{
    public class General
    {
        private static General generalInstanceObj = null;
        public const string releaseVersion = "1.0.1.02";
        public static bool SilentExecution = false;
        public bool TrackRevisionStatus;
        public bool HiddentxtStatus;
        public int noneditableeqn = 0;
        public int editableeqn = 0;
        public int wrdcount = 0;
        public static string HistoryLogForDebugging = "";
        public string docId;
        public string currentDocName;
        public string currentUser;
        public string currentDocPath;
        public string currentFolderPath;
        public string currentDocOpenTime;
        public static string systemIP;
        public static string customer;
        public string project;
        public string projectName;
        public string document;
        public string currentRunningFunction;
        public string currentRunningFunctionLabel;
        public string currentRunningGroupLabel;

        public string stage = "default";
        public System.Data.DataTable dataTable;
        public string macID;
        public string ribbonXml = "";
        //public Microsoft.Office.Tools.Ribbon.RibbonBase Rib;
        public Hashtable _tabs = new Hashtable();
        public Hashtable _group = new Hashtable();
        public Hashtable _control = new Hashtable();
        public Hashtable startTime = new Hashtable();
        public static string historyLogPath = "";
        public string _project = "";
        public string userDetails = "";
        public string drive;
        public string serverpath;
        public string citationXml;
        public string submissionreportXml;
        public bool isShowStyler = true;
        public bool isShowMapper = true;
        public bool comment = false;
        public static Boolean Trace = false;
        public static string ExecutionMode = "User"; // User or Server - By default: User; This enable message boxes; Server mode no messages and run sequentially
        public StringBuilder FileString = new StringBuilder();
        public static bool logIncrement = false;
        public static bool isExecuting = false;
        //public StylesUC uc;
        public List<Tuple<string, List<Tuple<string, List<string>>>>> _TabIDandGroupID = new List<Tuple<string, List<Tuple<string, List<string>>>>>();
        public Hashtable styleDetails = new Hashtable();
        public Dictionary<string, int> bookMarks = new Dictionary<string, int>();
        public static string startBookmark = "", endBookmark = "";
        public XDocument _xdoc;
        public Dictionary<string, string> StyleType = new Dictionary<string, string>();
        public List<string> styles = new List<string>();
        public List<string> groups = new List<string>();
        public List<string> rightChar = new List<string>();
        public bool indexLoop = false;
        public static bool bIsDebugModeRefStruc = false;
        public static bool functionExecutedSuccessfully = true;
        public static string AuthorWithEllipsisInPara = "";
        internal bool isStyleMapper;
        public string wrdcntDetail = "";
        public string gmrchkDetail = "";
        //added by Dakshinamoorthy on 2020-Nov-23
        private string _CustomerName;

        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; }
        }


        public static string ProgramFiles
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).TrimEnd('\\'); }
        }
        /// <summary>
        /// This function will get the Drive details
        /// </summary>
        /// <returns></returns>
        public string GetDrive()
        {
            try
            {
                if (drive != null)
                    return drive;
                string ConfigFilePath = ProgramFiles + @"\eBOT\Config.ini";
                //check the configuration file is present
                //then get the details of the drive
                if (File.Exists(ConfigFilePath) == true)
                {
                    XDocument xdoc = XDocument.Load(GetTextReader(File.ReadAllText(ConfigFilePath)));
                    drive = xdoc.Descendants("basepath").First().Value.Replace("\\", "").ToString();
                    return drive;
                }
            }
            catch (Exception e)
            {
                //Database.GetInstance.HistoryLogInsTrace(e.Source, e.LineNumber(), "Error: e055", e.Message, true, "");
            }
            drive = "";
            return drive;
        }

        //added by Dakshinamoorthy on 2020-Nov-23
        public string EnryptString(string input, string key)
        {
            string sOutputContent = string.Empty;
            try
            {
                byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                sOutputContent = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                //Database.GetInstance.HistoryLogInsTrace(ex.Source, ex.LineNumber(), "Error: e0555", ex.Message, true, "");
            }
            return sOutputContent;
        }

        public static General GeneralInstance
        {
            get
            {
                if (generalInstanceObj == null)
                {
                    generalInstanceObj = new General();
                }
                return generalInstanceObj;
            }
        }

        public static string DatabaseIndexCleanup(string Content)
        {
            string indexData = Regex.Replace(Content, @"&[^ ;]+;", "");
            indexData = Regex.Replace(Content, @"<[^<>]+>", "");
            indexData = Regex.Replace(indexData, @"[--.,;:/]", " "); //\u201C\u201D
            indexData = Regex.Replace(indexData, @" *\b(of|the|an|a|the|in|and|to|for|on|at|with|by|from|is|do|as)\b *", " ", RegexOptions.IgnoreCase);
            indexData = Regex.Replace(indexData, @"[^a-zA-Z \r\n]", "");
            indexData = Regex.Replace(indexData, @" [ ]+", " ");
            indexData = indexData.Trim();
            indexData = indexData.ToLower();
            return indexData;
        }

        /// <summary>
        /// This property will the calling method name
        /// </summary>
        public static string LogMethod
        {
            get
            {
                StackTrace stackTrace = new StackTrace();
                StackFrame stackFrame = stackTrace.GetFrame(2);
                MethodBase methodBase = stackFrame.GetMethod();
                return methodBase.Name;

            }
        }
        /// <summary>
        /// This function will return the string parameter
        /// as Text reader
        /// </summary>
        /// <param name="_string"></param>
        /// <returns></returns>
        public TextReader GetTextReader(string _string)
        {

            /*
             This function will read the string and returns textreader
             */
            TextWriter _stringWriter;
            _stringWriter = new StringWriter();
            _stringWriter.Write(_string);
            TextReader _stringReader;
            _stringReader = new StringReader(_stringWriter.ToString());
            return _stringReader;
        }
        /// <summary>
        /// This function will fetch the MacId
        /// </summary>
        /// <returns></returns>
        public string FetchMacId()
        {
            string macAddresses = "";
            //loop therough each network interface
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                //if the status is Up then get the MacId
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }
        /// <summary>
        /// This funciton will get the configuration of the ribbon
        /// </summary>
        public void GetRibbionConfiguration()
        {
            try
            {
                string metadata = "";
                string MetadatFilePath = currentDocPath.Trim('\\') + "\\Metadata.xml";

                stage = "default";
                //check for the metadatfile
                if (File.Exists(MetadatFilePath) == true)
                {
                    try
                    {
                        // read metadata
                        using (StreamReader sr = File.OpenText(MetadatFilePath))
                        {
                            metadata = sr.ReadToEnd();
                        }
                        //get the stage from metadata
                        XDocument metaXml = XDocument.Load(GetTextReader(metadata));
                        stage = metaXml.Descendants("stage").First().Value;
                    }
                    catch (Exception e)
                    {
                        //Database.GetInstance.HistoryLogInsTrace(e.Source, e.LineNumber(), "Error:e010", e.Message, true, "");
                    }
                }

                int counter = 0;
                while (++counter <= 2)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.Add(new SqlParameter("@project", customer + @"\" + project + @"\" + stage));

                    cmd.Parameters[0].Value = cmd.Parameters[0].Value.ToString().Replace("\\\\", @"\");
                    _project = cmd.Parameters[0].Value.ToString().Replace("\\\\", @"\");
                    //Database.GetInstance.ReadFromDatabase(cmd, "select * from project_config where LOWER(project)=LOWER(@project)");
                    Database.GetInstance.ReadFromDatabase(cmd, "select * from project_config where LOWER(project)=LOWER(@project) and LOWER(module)='menus'");
                    TextWriter tw = new StringWriter();
                    dataTable.WriteXml(tw);
                    if (tw.ToString().Length < 20) { project = "default"; continue; } // Checking if Default project exists and loading the default project
                    tw.Write(tw.ToString());
                    TextReader _stringReader;
                    _stringReader = new StringReader("<data>" + tw.ToString() + "</data>");
                    XDocument xdoc = XDocument.Load(_stringReader);
                    ribbonXml = xdoc.Descendants("DocumentElement").First().Descendants("projectTable")
                                   .Where(x => x.Descendants("module").Where(y => y.Value == "Menus").Count() > 0).First().Descendants("config").First().Value;
                    if (ribbonXml.ToString().Length > 20)
                        break;
                }
            }
            catch (Exception e)
            {
                //Database.GetInstance.HistoryLogInsTrace(e.Source, e.LineNumber(), "Error:e011", e.Message, true, "");
            }
        }

        /// <summary>
        /// This function will delete temperory files
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteTemporaryFiles(string path)
        {
            try
            {
                //check the file exists the delete
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception e)
            {
                //Database.GetInstance.HistoryLogInsTrace(e.Source, e.LineNumber(), "Error:e012", e.Message, true, "");
            }
        }

        /// <summary>
        /// This function will set the global variable
        /// </summary>
        public void SetGlobalVariables()
        {
            try
            {

                historyLogPath = GetDrive() + @"\eBOT\Logs\HistoryLog.txt";
                //currentDocName = Globals.ThisAddIn.Application.ActiveDocument.Name;
                //currentDocPath = Globals.ThisAddIn.Application.ActiveDocument.FullName;
                //currentFolderPath = Globals.ThisAddIn.Application.ActiveDocument.Path;
                currentDocOpenTime = DateTime.UtcNow.ToString();

                currentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                if (startTime.ContainsKey(currentDocPath) == false)
                    startTime.Add(currentDocPath, currentDocOpenTime);
                else
                    startTime[currentDocPath] = currentDocOpenTime;

                string[] folders = System.Text.RegularExpressions.Regex.Split(currentDocPath, @"\\");
                // string lstFldName = DocPath.Substring(lst+1, DocPath.Length - lst - 1);
                int fldCnt = folders[folders.Length - 2].Count(f => f == '_');

                //based on the file path set the project details here
                // Check whether folder name is short starting with "_" (underscore) and total 3 undercores to define customer, project, document
                if (File.Exists(currentFolderPath.Trim('\\') + "\\ebot.cnf"))
                {
                    string PubDetail = File.ReadAllText(currentFolderPath + "\\ebot.cnf");
                    document = folders[folders.Length - 2];
                    //XmlPubname = Regex.Match(XmlconfigTxt, "<publisher name=\"([^\"]+)\"/>").Groups[1].Value;
                    //project = Regex.Match(PubDetail, "^.*project *= *([A-Za-z]+)*").Groups[1].Value;
                    project = Regex.Match(PubDetail, "project=([^\n]+)").Groups[1].Value;
                    projectName = Regex.Match(PubDetail, "project=([^\n]+)").Groups[1].Value;
                    customer = Regex.Match(PubDetail, "customer=([^\n^\r]+)").Groups[1].Value;
                }
                else if (folders[folders.Length - 2].Substring(0, 1) == "_" && fldCnt == 3)
                {
                    string[] flds = folders[folders.Length - 2].Split('_');

                    document = flds[flds.Length - 1];
                    project = flds[flds.Length - 2];
                    projectName = flds[flds.Length - 2];
                    customer = flds[flds.Length - 3];
                }
                else if (folders.Length >= 4)
                {
                    document = folders[folders.Length - 2];
                    project = folders[folders.Length - 3];
                    projectName = folders[folders.Length - 3];
                    customer = folders[folders.Length - 4];
                }


                docId = customer + "/" + project + "/" + document;
                GetRibbionConfiguration();
                macID = FetchMacId();
                string hostName = Dns.GetHostName();
                for (int i = 0; i < Dns.GetHostEntry(hostName).AddressList.Length; i++)
                {
                    if (Regex.Match(Dns.GetHostEntry(hostName).AddressList[i].ToString(), @"^ *([0-9]+\.){3}[0-9]+ *$").Success == true)
                        systemIP = Dns.GetHostEntry(hostName).AddressList[i].ToString();
                }

                userDetails = "User: " + currentUser + ", Mode: " + ExecutionMode + ", DocID: " + docId + ", IP: " + systemIP + ", MacID: " + macID + ", ReleaseVersion: " + releaseVersion;
                //set mode for RefAutoStruc
                string sRefAutoStructXml = General.GeneralInstance.drive + @"\eBOT\Data\RefAutoStructuring.xml";

                if (!File.Exists(sRefAutoStructXml))
                {
                    //eBOT.Database.GetInstance.HistoryLogIns(string.Format("The file '{0}' does not exits.", sRefAutoStructXml), "", true, "");
                }

                XmlDocument docXml = new XmlDocument();
                docXml.Load(sRefAutoStructXml);

                //debug mode
                string sDebugModeOption = string.Empty;
                XmlNode nReferenceTitle = docXml.SelectSingleNode(@"//ReferenceAutoStructuring/IsDebugMode");
                if (nReferenceTitle != null)
                {
                    sDebugModeOption = nReferenceTitle.InnerText.ToString();
                    if (sDebugModeOption.ToLower().Equals("yes"))
                    {
                        bIsDebugModeRefStruc = true;
                    }
                    else
                    {
                        bIsDebugModeRefStruc = false;
                    }
                }
                else
                {
                    bIsDebugModeRefStruc = false;
                }

            }
            catch (Exception e)
            {
                //Database.GetInstance.HistoryLogInsTrace(e.Source, e.LineNumber(), "Error:e013", e.Message, true, "");
            }
        }

        public static string FileRead(string filePath, Encoding encoding)
        {
            string str = "";
            StreamReader sr;
            if (File.Exists(filePath))
            {
                sr = new StreamReader(filePath, encoding);
                try
                {
                    str = sr.ReadToEnd();
                    sr.Close();
                }

                catch (Exception e)
                {
                    //Database.GetInstance.HistoryLogInsTrace(e.Source, e.LineNumber(), "Error in Fileread Method", e.Message, true, "");
                    sr.Close();
                }
            }
            else
            {
                Database.GetInstance.HistoryLogIns("File does not Exist: " + filePath, "File not Found", true, "");
            }
            return str;
        }

        public static void FileReadNew(string filePath)
        {
            General.GeneralInstance.FileString.Clear();
            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            General.GeneralInstance.FileString.Append(line);
                        }
                    }
                }
            }
            //return "";
        }

        public static void FileWrite(string filePath, string str, Encoding encoding, Boolean append)
        {
            try
            {
                StreamWriter sw1 = new StreamWriter(filePath, append, encoding);
                sw1.Write(str);
                sw1.Close();
            }
            catch (Exception e)
            {
                //Database.GetInstance.HistoryLogInsTrace(e.Source, e.LineNumber(), "Error in FileWrite Method", e.Message, true, "");
            }

        }

        public static string DecryptString(string input, string key)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static string CorrectDateFormat(string num)
        {
            num = num.ToUpper();
            switch (num)
            {
                case "1": case "01": case "JAN": case "JANUARY": num = "January"; break;
                case "2": case "02": case "FEB": case "FEBRUARY": num = "February"; break;
                case "3": case "03": case "MAR": case "MARCH": num = "March"; break;
                case "4": case "04": case "APR": case "APRIL": num = "April"; break;
                case "5": case "05": case "MAY": num = "May"; break;
                case "6": case "06": case "JUN": case "JUNE": num = "June"; break;
                case "7": case "07": case "JUL": case "JULY": num = "July"; break;
                case "8": case "08": case "AUG": case "AUGUST": num = "August"; break;
                case "9": case "09": case "SEP": case "SEPTEMBER": num = "September"; break;
                case "10": case "OCT": case "OCTOBER": num = "October"; break;
                case "11": case "NOV": case "NOVEMBER": num = "November"; break;
                case "12": case "DEC": case "DECEMBER": num = "December"; break;
            }
            return num.ToString();
        }
    }
}

