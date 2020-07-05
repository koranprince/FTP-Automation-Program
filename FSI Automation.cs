using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace FSI_Automated_Support
{

    
    class Automation
    {
        public string Jobname { get; set; }
        public string Passthru { get; set; }
        public string SOURCE_DIRECTORY { get; set; }
        public string NSOURCE_DIRECTORY { get; set; }
        public string NSOURCE_NAME { get; set; }
        public string NDESTINATION_CONNECTION { get; set; }
        public string NDESTINATION_DIRECTORY { get; set; }
        public string NDESTINATION_NAME { get; set; }
        public string Response { get; set; }
        public bool found { get; set; }
        public string batchs { get; set; }
        
        //Array Custom Config information 

        List<String> OldFile = new List<String>();
        
        //File Types in Passthrus
        string[] FileType = { "*.PDF", "*.Doc", "*.DAT", "*.csv", "*.TXT", "*.ZIP", "*.rpt" };

        

        //Connection String
        static string myconnstring = ConfigurationManager.ConnectionStrings["koranprince"].ConnectionString;
        static string myconnstrings = ConfigurationManager.ConnectionStrings["koranprince2"].ConnectionString;
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["koranprince"].ConnectionString);

        public void batch()
        {
            
            SqlConnection conn = new SqlConnection(myconnstrings);
            DataTable dte = new DataTable();
            List<string> batches = new List<string>();
            Automation ab = new Automation();
            
            try
            {
                //Query
                String GetJobname = "select script from [dbo].[M_Log] where Status='25741' and Server_Date like'05/18/20'and Script like'ASM_FAC%'group by script";
                //Command 
                SqlCommand cmds = new SqlCommand(GetJobname, conn);
                //Data adapter
                SqlDataAdapter adapter = new SqlDataAdapter(cmds);
                
                conn.Open();

                adapter.Fill(dte);

                foreach (DataRow jnames in dte.Rows)
                {
                    string name = (jnames["script"].ToString());
                    Passthru = name;
                    DataTable ds = ab.Name(ab);

                    foreach(DataRow results in ds.Rows)
                    {
                        string result = results["PASS_THRU_DIRECTORY"].ToString();
                        ab.filetype(result);


                    }


                }

            }

            catch
            {


            }

            finally
            {


            }
            //return dte; 
            
        }

        //Getting passthru from text box
        public DataTable Name(Automation A)
        {
            //Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);
            DataTable dt = new DataTable();




            try
            {
                //Query
                String GetJobname = "Select PASS_THRU_DIRECTORY,SOURCE_NAME, DESTINATION_CONNECTION,DESTINATION_DIRECTORY,DESTINATION_NAME from Cleanup1 where FTP_Config_Name = @Passthru";
                //Command 
                SqlCommand cmd = new SqlCommand(GetJobname, conn);
                //Parameters
                cmd.Parameters.AddWithValue("@Passthru", A.Passthru);
                
                //Data adapter
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
              
                adapter.Fill(dt);
                




            }
            catch (Exception e)
            {
                e.GetBaseException();

            }
            finally
            {
                conn.Close();

            }
            return dt;
        }

        //int i = 0;
        public  void  filetype(string Filename)
        {
            //found = false;
               
            //MainWindow main = new MainWindow();
            try
            {



                foreach (string Type in FileType)
                {
                    string Pass_Directory = (@"C:\Users\prink\Desktop\kpconfig\k");
                    //(@" " + Pass_Dir + "");

                    //JobName.Text = filep;
                    //AS4_ZOT_RADPHYSBILLPAGEPRM_W2
                    //ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E
                    //Check if any files are in the passthru
                    string[] kp6 = Directory.GetFiles(Pass_Directory, Type);
               
                    if (kp6.Length == 0)
                    {
                        foreach(string names in kp6)
                        {
                            Response = names;


                        }
                        //batchs = Filename;
                      
                    }
                    else
                    {
                        //found = true;
                        //foreach (string passthruName in kp6)
                        //{
                        int i = -1;
                        string[] CustomScript = File.ReadAllLines(@"C:\Users\prink\Desktop\kpconfig\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini");

                        
                        foreach (var line in CustomScript)
                            {
                            
                            String[] Sourceinfor = { "SOURCE_DIRECTORY", "SOURCE_NAME", "DESTINATION_CONNECTION", "DESTINATION_DIRECTORY", "DESTINATION_NAME" };
                                foreach (string SI in Sourceinfor)
                                {

                                int index = Array.IndexOf(Sourceinfor,SI);
                                if (line.Contains(SI))
                                    {
                                    int a = i++;
                                    
                                    if (OldFile.Count < 5)
                                        {
                                           OldFile.Add(line);
                                            if (OldFile.Count ==5)
                                        {
                                            Response = "Working...";
                                            ChangeCustPass();

                                        }
                                        }
                                        else
                                        {
                                        Response = "Working...";
                                        SourceInfo(line,index);
                                        }
                                        
                                    }
                                                                   
                            }
                        }

                    }


                }

            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            finally
            {

                

            }
            //return found;

        }

        private void SourceInfo(string newname, int OldName)
         {

            int index = OldName;
            if (index != -1)
            {

                //SOURCE_DIRECTORY = index.ToString();
                OldFile[index] = newname;
                if (index == 4){
                    ChangeCustPass();
                }
            }
            else
            {
                SOURCE_DIRECTORY = index.ToString();
                //OldFile.Add(newname);
            }

        }





        public void ChangeCustPass()
        {
            string SOURCE_DIRECTORY = OldFile[0].ToString();
            string SOURCE_NAME = OldFile[1].ToString();
            string DESTINATION_CONNECTION = OldFile[2].ToString();
            string DESTINATION_DIRECTORY = OldFile[3].ToString();
            string DESTINATION_NAME = OldFile[4].ToString();
            FileProcess1(NSOURCE_DIRECTORY, SOURCE_DIRECTORY, NSOURCE_NAME, SOURCE_NAME);
            FileProcess2(NDESTINATION_CONNECTION, DESTINATION_CONNECTION, NDESTINATION_DIRECTORY, DESTINATION_DIRECTORY);
            FileProcess3(NDESTINATION_NAME, DESTINATION_NAME);


        }
        //Process of replacing current information with new information in Custom Config
        public void FileProcess1(string NEWSOURCE_DIRECTORY, string SOURCE_DIRECTORY, string NEWSOURCE_NAME, string SOURCE_NAME)
        {


            string str = File.ReadAllText(@"C:\Users\prink\Desktop\kpconfig\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini");
            //Passthru Directory to Source Directory
            str = str.Replace(@"" + SOURCE_DIRECTORY + "", @"SOURCE_DIRECTORY=" + NEWSOURCE_DIRECTORY + "");


            //Source Name to Source name
            str = str.Replace(@"" + SOURCE_NAME + "", @"SOURCE_NAME=" + NEWSOURCE_NAME + "");

            File.WriteAllText(@"C:\Users\prink\Desktop\kpconfig\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini", str);
           

        }

        public void FileProcess2(string NEWDESTINATION_CONNECTION, string DESTINATION_CONNECTION, string NEWDESTINATION_DIRECTORY, string DESTINATION_DIRECTORY)
        {


            string str = File.ReadAllText(@"C:\Users\prink\Desktop\kpconfig\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini");

            ////Destination Connection to Destination Connection
            str = str.Replace(@"" + DESTINATION_CONNECTION + "", @"DESTINATION_CONNECTION={" + NEWDESTINATION_CONNECTION + "}");

            //Destination Name to Destination Name
            str = str.Replace(@"" + DESTINATION_DIRECTORY + "", @"DESTINATION_DIRECTORY=" + NEWDESTINATION_DIRECTORY + "");

            File.WriteAllText(@"C:\Users\prink\Desktop\kpconfig\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini", str);
            //ReRunFtpJob(Job);

        }

        public void FileProcess3(string NEWDESTINATION_NAME, string DESTINATION_NAME)
        {

            string str = File.ReadAllText(@"C:\Users\prink\Desktop\kpconfig\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini");


            //Destination Directory to Destination Directory
            str = str.Replace(@"" + DESTINATION_NAME + "", @"DESTINATION_NAME=" + NEWDESTINATION_NAME + "");

            File.WriteAllText(@"C:\Users\prink\Desktop\kpconfig\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini", str);


            //Response = "Complete";
            //ReRunFtpJob();
            Filemovement();



        }

        public  void Filemovement()
        {
            string[] f = Directory.GetFiles(@"C:\Users\prink\Desktop\kpconfig\");
            foreach (string name in f)
            {
                if (name.Contains("ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E"))
                {
                    Thread.Sleep(5000);
                    File.Copy(name, @"\\CORP-BFTP07\Corp-BFTP07_Passthru\Koran\RSO\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini",true);
                    Thread.Sleep(5000);
                    File.Move(@"\\CORP-BFTP07\Corp-BFTP07_Passthru\Koran\RSO\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini", @"\\CORP-BFTP07\Corp-BFTP07_Passthru\Koran\RSO\test\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini");
                    //await Task.Delay(5000);
                    Thread.Sleep(2000);
                    File.Delete(@"\\CORP-BFTP07\Corp-BFTP07_Passthru\Koran\RSO\test\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini");
                    Thread.Sleep(5000);
                    //await Task.Delay(5000);
                }
            }
            


            

        }


        private void ReRunFtpJob()
        {
            string passsss = Passthru;
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\kp\DoScript.Exe");
            //ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\Beyond FTP\DoScript.Exe");
            startInfo.Arguments = "" + Passthru + "/O";
            Process.Start(startInfo);
            //Task.Delay(6000);
            Validate();

        }

        private string Validate()
        {
            string kp = "hi";
            return kp;

            //string Validation = "";
            //if (List.count > 0)
            //{
            //    string Validation = "Please contact Vendor";
            //    return Validation;
            //}
            //else
            //{
            //    string Validation = "Job Succesful , files have been transferred  to ";
            //    +Destination name
            //    return Validation;

            //}
        }


    }
}
