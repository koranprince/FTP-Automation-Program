using System.Windows;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FSI_Automated_Support
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            

        }
        
        Automation A = new Automation();
        Automation BE = new Automation();
        public string no { get; set; }

        private void Button(object sender, RoutedEventArgs e)
        {

            kp.Text = "";
            JobName.Text = "";
            //Submit.Visibility = System.Windows.Visibility.Visible;
            //Submit2.Visibility = System.Windows.Visibility.Hidden;
        }

        private  void Button_Click(object sender, RoutedEventArgs e)
        {

            if (JobName.Text== "Please enter Job name")
            {
                kp.Text = "Please enter a FTP job";
                kp.Visibility = System.Windows.Visibility.Visible;

            }

            else {
               
                Start();
               
            }
        }
        







        private async void Start()
        {
            if (JobName.Text=="")
            {
                kp.Text = "Please enter FTP Jobname";
                kp.Visibility = System.Windows.Visibility.Visible;


            }
            else
            {
               
                try
                {
                    A.Passthru = JobName.Text;
                    DataTable ds = A.Name(A);

                    if (ds.Rows.Count == 0)
                    {
                        kp.Visibility = System.Windows.Visibility.Visible;
                        kp.Text = JobName.Text + " does not exist in database";
                        

                        Submit.Visibility = System.Windows.Visibility.Visible;
                        JobName.Text = " Please enter FTP Job name";

                        no = "kp";

                    }
                    else
                    {
                         
                        await running();
                        kp.Text = "Completed";
                        JobName.Text = " Please enter FTP Jobname";

                        Submit.Visibility = System.Windows.Visibility.Visible;

                    }
                }

                catch
                (Exception)
                {

                }
            }

        }




          
        private async Task running()
        {



            try
            {
                kp.Text = "Working...";
                kp.Visibility = System.Windows.Visibility.Visible;
                Submit.Visibility = System.Windows.Visibility.Hidden;
                await Task.Run(() => Submitting());
                
                
                
            }
            catch(Exception e)
            {
                //kp.Text = e.GetBaseException();

            }
            
           // await Task.Run(()=>Submitting());
           //Buttonvis();
            //await Task.Delay(5000);
           
           
            


        }

        private void Submitting()
        {
            //bool uiAccess = JobName.Dispatcher.CheckAccess();

            JobName.Dispatcher.Invoke(() => { A.Passthru = JobName.Text; });
             JobName.Dispatcher.Invoke(() => { BE.Passthru = JobName.Text; });


            DataTable dt = A.Name(A);

            List<String> Pass_Thru_Dir = new List<String>();


            // int a = -1;


            foreach (DataRow dr in dt.Rows)
            {


                int A = 0;
                // String[] PassthuA = dr["PASS_THRU_DIRECTORY"].ToString();
                //Pass_Thru_Dir.Add(dr["PASS_THRU_DIRECTORY"].ToString());
                if (Pass_Thru_Dir.Count == 0)
                {
                    Pass_Thru_Dir.Add(dr["PASS_THRU_DIRECTORY"].ToString());
                    Pass_Thru_Dir.Add(dr["SOURCE_NAME"].ToString());
                    Pass_Thru_Dir.Add(dr["DESTINATION_CONNECTION"].ToString());
                    Pass_Thru_Dir.Add(dr["DESTINATION_DIRECTORY"].ToString());
                    Pass_Thru_Dir.Add(dr["DESTINATION_NAME"].ToString());

                    BE.NSOURCE_DIRECTORY = Pass_Thru_Dir[0];
                    BE.NSOURCE_NAME = Pass_Thru_Dir[1];
                    BE.NDESTINATION_CONNECTION = Pass_Thru_Dir[2];
                    BE.NDESTINATION_DIRECTORY = Pass_Thru_Dir[3];
                    BE.NDESTINATION_NAME = Pass_Thru_Dir[4];

                    string first = Pass_Thru_Dir[0];

                    BE.filetype(first);


                }
                else
                {

                    //BE.NSOURCE_NAME = Pass_Thru_Dir[1];
                    //BE.NDESTINATION_CONNECTION = Pass_Thru_Dir[2];
                    //BE.NDESTINATION_NAME = Pass_Thru_Dir[4];
                    //BE.NDESTINATION_DIRECTORY = Pass_Thru_Dir[3];

                    int index = A;
                    if (index != -1)
                    {

                        //SOURCE_DIRECTORY = index.ToString();
                        Pass_Thru_Dir[0] = dr["PASS_THRU_DIRECTORY"].ToString();
                        Pass_Thru_Dir[1] = dr["SOURCE_NAME"].ToString();
                        Pass_Thru_Dir[2] = dr["DESTINATION_CONNECTION"].ToString();
                        Pass_Thru_Dir[3] = dr["DESTINATION_DIRECTORY"].ToString();
                        Pass_Thru_Dir[4] = dr["DESTINATION_NAME"].ToString();

                        BE.NSOURCE_DIRECTORY = Pass_Thru_Dir[0];
                        BE.NSOURCE_NAME = Pass_Thru_Dir[1];
                        BE.NDESTINATION_CONNECTION = Pass_Thru_Dir[2];
                        BE.NDESTINATION_DIRECTORY = Pass_Thru_Dir[3];
                        BE.NDESTINATION_NAME = Pass_Thru_Dir[4];

                        string first = Pass_Thru_Dir[0];

                        BE.filetype(first);
                    }
                    


                }
            }
        

        }

        private void kp_Loaded(object sender, RoutedEventArgs e)
        {
            string[] aary = { "Koran", "Prince" };
            

            //string str = File.ReadAllText(@"C:\Users\prink\Desktop\kpconfig\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini");
            //str = str.Replace(@"\Interface\159_Aiken_A0PZ\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E", @"\Interface\159_Aiken_A0PZ\ASM_IMX_MONTHLYMATRIXFEDLWR_E");
            //File.WriteAllText(@"C:\Users\prink\Desktop\kpconfig\ASM_ZOT_EDBILLINGLOGSMONTHLYAIK_E.ini", str);
            kp.Visibility = System.Windows.Visibility.Hidden;
            Submit2.Visibility = System.Windows.Visibility.Hidden;
        }

        private void batchs(object sender,RoutedEventArgs e)
        {
            //A.batch();
            batch.Visibility = System.Windows.Visibility.Hidden;
            kp.Text = BE.batchs;
            

        }

        public void kpe(object sender, RoutedEventArgs e)
        {
            JobName.Text = "";
        }

        public void jobnameloaded(object sender, RoutedEventArgs e)
        {
            //JobName.Text = "Please enter FTP Jobname";

        }
        public void doubled(object sender, RoutedEventArgs e)
        {
            // JobName.Text = JobName.Text;

        }

        public void textbox(object sender, RoutedEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //    button.PerformClick();
        }
    }
}
//