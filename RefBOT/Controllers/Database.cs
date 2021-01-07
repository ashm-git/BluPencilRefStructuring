using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
//using Npgsql;
//using System.Windows.Forms;
using System.Net.Sockets;
using System.Globalization;
using System.Net;

namespace RefBOT.Controllers
{
    class Database
    {
        public int dbcount = 0;
        public General generalInstanceObj = General.GeneralInstance;
        private static Database dbObj = null;
        public string ConnectionString;
        public static string pcIpaddress;
        public string GetConnectionString
        {
            get
            {
                try
                {
                    if (ConnectionString != null)
                        return ConnectionString;
                    ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConString"].ToString();
                    /*string ConnFilePath = General.ProgramFiles + @"\eBOT\Database.ini";
                    if (File.Exists(ConnFilePath) == true)
                    {
                        ConnectionString = File.ReadAllText(ConnFilePath);
                        ConnectionString = General.DecryptString(ConnectionString, "nZr4u7x!A%D*G-Ka");
                    }
                    else
                    {
                        //MessageBox.Show("eBOT: Database configuration file expected in folder:\n\n" + ConnFilePath, "Database INI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //HistoryLogIns("error:e001", "Database connection file does not exists. File Path: " + ConnFilePath, true, "");
                    }*/
                    return ConnectionString;
                }
                catch (Exception e)
                {
                    HistoryLogIns("error:e002", e.Message, true, "");
                }
                return "";
            }
           
        }
        public void GetMappingStyles()
        {
            string dbVer = "";
            try
            {
                SqlCommand cmd = new SqlCommand();
                Database.GetInstance.ReadFromDatabase(cmd, "SELECT * FROM stylelog WHERE ID='" + General.customer + "\\" + General.GeneralInstance.projectName + "'");

            }
            catch (Exception e)
            { }

        }

        public void StyleLogIns(string InputStyle, string OutputStyle)
        {
            try
            {
                /* Insertion After Validations*/
                using (SqlConnection connection = new SqlConnection())
                {

                    connection.ConnectionString = GetConnectionString;// ConfigurationManager.ConnectionStrings["constr"].ToString();
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into stylelog values(@ID,@Input_Style,@Output_Style)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@ID", General.customer + "\\" + General.GeneralInstance.projectName));
                    cmd.Parameters.Add(new SqlParameter("@Input_Style", InputStyle));
                    cmd.Parameters.Add(new SqlParameter("@Output_Style", OutputStyle));

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();

                }
            }
            catch (Exception e)
            {
                //HistoryLogIns("error", e.Message, true, "");
            }
        }
        public void StyleLogDelete(string InputStyle, string OutputStyle)
        {
            try
            {
                /* Insertion After Validations*/
                using (SqlConnection connection = new SqlConnection())
                {

                    connection.ConnectionString = GetConnectionString;// ConfigurationManager.ConnectionStrings["constr"].ToString();
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Delete from stylelog where input_style=@Input_Style and id=@ID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@ID", General.customer + "\\" + General.GeneralInstance.projectName));
                    cmd.Parameters.Add(new SqlParameter("@Input_Style", InputStyle));


                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();

                }
            }
            catch (Exception e)
            {
                //eBOT.Database.GetInstance.HistoryLogIns("ErrorDatabase1", "eBOT: " + e.Message, true, "");
            }
        }

        /// <summary>
        /// This empty method to ensure no other instances are created
        /// </summary>
        private Database()
        {
        }

        /// <summary>
        /// A property to instantiate a Database to ensure NO multiple instances are open and if the object is null it will create and return the object instance
        /// Singleton class
        /// </summary>
        public static Database GetInstance
        {
            get
            {
                if (dbObj == null)
                {
                    dbObj = new Database();
                }
                return dbObj;
            }
        }
        
        public static Boolean isDBServerActive()
        {
            string ConnStr = "";
            string ServerIP = "";
            TcpClient tcpClient = new TcpClient();
            try
            {
                ConnStr = Database.GetInstance.GetConnectionString;
                ServerIP = Regex.Replace(ConnStr, "^.*Server *= *([0-9.]+) *;.*$", "$1", RegexOptions.IgnoreCase);
                int Port;
                int.TryParse(Regex.Replace(ConnStr, "^.*Server *= *([0-9.]+) *; *Port *= *([0-9]+) *;.*$", "$2", RegexOptions.IgnoreCase), out Port);
                try
                {
                    tcpClient.Connect(ServerIP, Port);// Port open
                }
                catch (Exception tcE)
                {
                    //eBOT.Database.GetInstance.HistoryLogIns("ErrorDatabase", tcE.Message.ToString(), true, "");
                }
                return true;
            }
            catch (Exception)
            {
                //eBOT.Database.GetInstance.HistoryLogIns("ErrorDatabase", "eBOT: Database Server not reachable. Could be due to Network issue (or) Server is inactive\n\nServer IP: " + ServerIP, true, "");
                return false; //"Port closed");
            }
        }


        public void eBotfilesCopy(string local_file, string server_file)
        {
            //getting updated SimpleReplace.ini from server to local
            if (File.Exists(server_file) == true)
            {
                if (File.Exists(local_file) == false)
                {
                    System.IO.File.Copy(server_file, local_file, true);
                }
                else
                {
                    DateTime serverExe = File.GetLastWriteTime(server_file);
                    DateTime localExe = File.GetLastWriteTime(local_file);
                    if (serverExe == localExe) { }
                    else
                    {
                        System.IO.File.Copy(server_file, local_file, true);
                    }
                }
            }
            else
            {
                //eBOT.Database.GetInstance.HistoryLogIns("Error:FileCopy", server_file + " not found or server not connected", true, "");
            }
        }


        public void HistoryLogInsTrace(string source, int lineNumber, string logType, string logDescription, bool logtoFile, string startTime)
        {
            string logRecord = "";
            try
            {
                string historyLogFilePath;
                if (General.historyLogPath == "")
                    General.GeneralInstance.SetGlobalVariables();
                historyLogFilePath = General.historyLogPath;// historyLogFilePath + "\\HistoryLog.txt";

                logRecord = generalInstanceObj.currentUser + "|";
                logRecord += logType + "|";
                if (startTime == "") // Just Dt1 and Dt2 is Null
                {
                    logRecord += "null|";
                    logRecord += DateTime.UtcNow.ToString() + "|";
                }
                else
                {                    
                    logRecord += startTime + "|";
                    logRecord += DateTime.UtcNow.ToString() + "|";
                }
                logRecord += generalInstanceObj.docId + "|";
                logRecord += General.LogMethod + "|";
                logRecord += "Source: " + source + "; Line Number: " + lineNumber + "; " + logDescription + Environment.NewLine;

                //check if log file is exists then write the log record to file
                if (File.Exists(historyLogFilePath))
                {
                    using (StreamWriter sw = File.AppendText(historyLogFilePath))
                    {
                        sw.Write(logRecord);
                    }
                }
                else
                {
                    //Otherwise create the logfile and write log record
                    using (StreamWriter sw = File.CreateText(historyLogFilePath))
                    {
                        sw.Write(logRecord);
                    }
                }
                //if (System.Diagnostics.Debugger.IsAttached == true)
                //{
                //    if (logType.ToLower().Contains("error") == true)
                //    {

                //        File.WriteAllText(General.GeneralInstance.GetDrive() + @"\eBOT\Logs\tempLog.txt", logType + "; " + "Method: " + General.LogMethod + " [Line: " + lineNumber + "]; " + "Source: " + source + "; " + logDescription + Environment.NewLine + Environment.NewLine);
                //        General.logIncrement = true;

                //    }
                //}

                if (General.logIncrement == true)
                {
                    General.HistoryLogForDebugging = File.ReadAllText(General.GeneralInstance.GetDrive() + @"\eBOT\Logs\tempLog.txt");
                    General.logIncrement = false;
                }
            }
            catch (Exception e)
            {
                //HistoryLogIns("error", e.Message, true, "");
            }

        }

        public void ReadFromDatabase1(SqlCommand cmd, string query, SqlConnection connection)
        {
            try
            {
                // using (SqlConnection connection = new SqlConnection())
                if (dbcount == 0)
                {
                    connection.ConnectionString = GetConnectionString; //ConfigurationManager.ConnectionStrings["constr"].ToString();
                    connection.Open();
                    cmd.Connection = connection;
                    dbcount++;
                }
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                GetInstance.generalInstanceObj.dataTable = new DataTable() { TableName = "projectTable" };
                da.Fill(GetInstance.generalInstanceObj.dataTable);
                // cmd.Dispose();
                // connection.Close();
            }
            catch (Exception e)
            {
            }
        }
        /// <summary>
        /// This function will insert log record in to historylog File
        /// ??? Better to have this inserted as XML as "|" will be a problem in case if one function is changed and the update function is missed
        /// also possible occurnce of this character within the error log
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logDescription"></param>
        /// <param name="logtoFile"></param>
        public void HistoryLogIns(string logType, string logDescription, bool logtoFile, string startTime)
        {
            string logRecord = "";
            try
            {
                string historyLogFilePath;
                if (General.historyLogPath == "")
                    General.GeneralInstance.SetGlobalVariables();
                historyLogFilePath = General.historyLogPath;// historyLogFilePath + "\\HistoryLog.txt";
                if(!Directory.Exists(Path.GetDirectoryName(historyLogFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(historyLogFilePath));
                }
                logRecord = generalInstanceObj.currentUser + "|";
                logRecord += logType + "|";
                if (startTime == "") // Just Dt1 and Dt2 is Null
                {
                    logRecord += "null|";
                    logRecord += DateTime.UtcNow.ToString() + "|";
                }
                else
                {
                    logRecord += startTime + "|";
                    logRecord += DateTime.UtcNow.ToString() + "|";
                }
                logRecord += generalInstanceObj.docId + "|";
                logRecord += General.LogMethod + "|";
                logRecord += logDescription;
                logRecord += Environment.NewLine;
                //check if log file is exists then write the log record to file
                if (File.Exists(historyLogFilePath))
                {
                    using (StreamWriter sw = File.AppendText(historyLogFilePath))
                    {
                        sw.Write(logRecord);
                    }
                }
                else
                {
                    //Otherwise create the logfile and write log record
                    using (StreamWriter sw = File.CreateText(historyLogFilePath))
                    {
                        sw.Write(logRecord);
                    }
                }
            }
            catch (Exception e)
            {
                //HistoryLogIns("error", e.Message, true, "");
            }
        }



        /// <summary>
        /// This function will add log record to database
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logDescription"></param>
        /// <param name="startTime"></param>
        public void HistoryLogIns(string logType, string logDescription, string startTime)
        {
            try
            {
                /* Insertion After Validations*/
                using (SqlConnection connection = new SqlConnection())
                {

                    connection.ConnectionString = GetConnectionString;// ConfigurationManager.ConnectionStrings["constr"].ToString();
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Insert into history_log values(@username,@logtype,@dt1,@dt2,@doc_id,@modulename,@description)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@username", General.GeneralInstance.currentUser));
                    cmd.Parameters.Add(new SqlParameter("@logtype", logType));
                    cmd.Parameters.Add(new SqlParameter("@dt1", startTime));
                    cmd.Parameters.Add(new SqlParameter("@dt2", DateTime.UtcNow.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@doc_id", General.GeneralInstance.docId));
                    cmd.Parameters.Add(new SqlParameter("@modulename", General.LogMethod));
                    cmd.Parameters.Add(new SqlParameter("@description", logDescription));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();

                }
            }
            catch (Exception e)
            {
                //HistoryLogIns("error", e.Message, true, "");
            }
        }
        /// <summary>
        /// Retrieves the latest release version based on the Database entries with Status = "Active";
        /// This also ensures whether the database has been successfully connected or not
        /// </summary>
        /// <param name="query"></param>
        public string RetrieveDBVersion()
        {
            string dbVer = "";
            try
            {
                SqlCommand cmd = new SqlCommand();
                Database.GetInstance.ReadFromDatabase(cmd, "SELECT version FROM release WHERE status='Active' ORDER BY id DESC LIMIT 1");
                dbVer = General.GeneralInstance.dataTable.Rows[0]["version"].ToString();
                //MessageBox.Show(dbVer);
            }
            catch (Exception e)
            {
               // MessageBox.Show("Error : \n " + e.Message.ToString());
            }
            return dbVer;
        }

        /// <summary>
        /// This function will execute query passed as a parameter
        /// ??? What is the ProjectTable down below?
        /// </summary>
        /// <param name="query"></param>
        public void ReadFromDatabase(SqlCommand cmd, string query)
        {
            try
            {
                //File.AppendAllText(@"D:\Projects\RefBOTDev\Log\Log.txt", "ReadFromDatabase: " + query);
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = GetConnectionString;
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(query, GetConnectionString);
                    GetInstance.generalInstanceObj.dataTable = new DataTable() { TableName = "projectTable" };
                    da.Fill(GetInstance.generalInstanceObj.dataTable);
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                File.AppendAllText(@"D:\Projects\RefBOTDev\Log\Log.txt", "ReadFromDatabase ex: " + e.Message);
                HistoryLogIns("ReadFromDatabase: " + query, e.Message, true, DateTime.Now.ToString());
               // MessageBox.Show("Error 2 : " + e.Message.ToString());
            }
        }

        /// <summary>
        /// This function will add log records from file to database
        /// ??? Where does the ConnectionString coming from?
        /// </summary>
        public void HistoryLogInsFromFile()
        {
            try
            {
                string historyLogFilePath = General.historyLogPath;
                string line = "";
                string lines = "";
                string[] sep = { "\r\n" };

                lines = General.FileRead(historyLogFilePath, Encoding.UTF8);

                //System.IO.StreamReader file = new System.IO.StreamReader(historyLogFilePath);
                //while ((line = file.ReadLine()) != null)
                //    lines = lines + "{{Break}}" + line;
                //file.Close();
                //using (SqlConnection connection = new SqlConnection())
                using (SqlConnection connection = new SqlConnection())
                {
                    string conn = GetConnectionString;// ConfigurationManager.ConnectionStrings["constr"].ConnectionString;//"Server=localhost;Port=5432;Database=eBOT_DB;User Id=postgres;Password=postgrace;";

                    connection.ConnectionString = conn;
                    connection.Open();

                    string[] LineSp = lines.Split(sep, StringSplitOptions.None);

                    //loop through each record in file and add to database
                    for (int i = 0; i < LineSp.Length; i++)
                    {
                        string[] records = LineSp[i].Split('|');
                        if (records.Length > 5)
                            InsertRecord(connection, records);
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                //HistoryLogIns("error", e.Message, true, "");
            }
        }

        /// <summary>
        /// This function will add single record to database
        /// ??? Why do we need InsertRecord if there another function "HistoryLogIns" doing the same
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="records"></param>
        private void InsertRecord(SqlConnection connection, string[] records)
        {
            try
            {
                //SqlCommand cmd = new SqlCommand();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                if (records[2] == "null")
                    cmd.CommandText = "insert into history_log(userName,type,dt2,doc_id,module,description)  values(@user,@type,@dt2,@doc_id,@module,@description);";
                else
                    cmd.CommandText = "insert into history_log(userName,type,dt1,dt2,doc_id,module,description)  values(@user,@type,@dt1,@dt2,@doc_id,@module,@description);";

                cmd.CommandType = CommandType.Text;

                //cmd.Parameters.Add(new SqlParameter("@user", records[0]));
                cmd.Parameters.Add(new SqlParameter("@user", records[0]));
                //cmd.Parameters.Add(new SqlParameter("@type", records[1]));
                cmd.Parameters.Add(new SqlParameter("@type", records[0]));
                //cmd.Parameters.Add(new SqlParameter("@dt1", DateTime.ParseExact(records[2], "dd-MMM-yy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture)));
                cmd.Parameters.Add(new SqlParameter("@dt2", DateTime.ParseExact(records[3], CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern, System.Globalization.CultureInfo.InvariantCulture)));

                if (records[2] != "null")
                    cmd.Parameters.Add(new SqlParameter("@dt1", DateTime.ParseExact(records[2], CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern, System.Globalization.CultureInfo.InvariantCulture)));

                cmd.Parameters.Add(new SqlParameter("@doc_id", records[4]));
                cmd.Parameters.Add(new SqlParameter("@module", records[5]));
                cmd.Parameters.Add(new SqlParameter("@description", records[6]));

                //CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern

                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                //HistoryLogInsTrace(e.Source, e.LineNumber(), "error:e003", e.Message, true, "");
            }
        }

        public bool ExecuteNonQueryFunction(string sQuery)
        {
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = GetConnectionString;
                connection.Open();
                SqlCommand command = new SqlCommand(sQuery, connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            catch (Exception e)
            {
                HistoryLogIns("error:e004", e.Message, true, "");
            }
            return true;
        }

        public static string GetIPaddress()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 1].ToString();

        }
    }

}


