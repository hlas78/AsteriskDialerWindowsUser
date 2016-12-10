using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace AsteriskDialerWindowsUser
{
    public partial class frmMain : Form
    {
        String sLogin, sExtension, sContext;
        AsteriskClient ac;
        Logger log;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            String sIP, sUser, sPwd;
            int iPort;
            this.Visible = true;
            try
            {
                log = new Logger();
                sLogin = Environment.UserName;
                var sc = ConfigurationManager.ConnectionStrings["sqldb"].ConnectionString;
                SqlConnection conn = new SqlConnection(sc);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select extension from extensions where username = '" + sLogin + "'", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                sExtension = dr["extension"].ToString();
                this.Text = "Asterisk Dialer - User: " + sLogin + " Extension: " + sExtension;
                dr.Close();
                conn.Close();
            }
            catch (Exception err)
            {
                log.log("Error connecting DB. " + err.Message);
                lblInfo.Text = "Error connecting DB.\n" + err.Message;
            }
            try
            {
                sIP = ConfigurationManager.AppSettings["AsteriskIP"];
                sUser = ConfigurationManager.AppSettings["AsteriskUser"];
                sPwd = ConfigurationManager.AppSettings["AsteriskPwd"];
                iPort = int.Parse(ConfigurationManager.AppSettings["AsteriskPort"]);
                sContext = ConfigurationManager.AppSettings["Context"];
                lblInfo.Text = "Connecting to asterisk...";
                log.log("Connecting to " + sIP + "  User: " + sUser);
                ac = new AsteriskClient(sIP, iPort, sUser, sPwd);
                if (ac.StartClient() == 1)
                {
                    log.log("Connected to " + sIP);
                    ac.Login();
                    log.log("Credentials sent succesfuly");
                    lblInfo.Text = "Connected to asterisk...";
                    timer1.Enabled = true;
                }
                else
                {
                    log.log("Error connecting to Asterisk, please verify connectivity to " + sIP + " on port " + iPort.ToString());
                    lblInfo.Text = "Error connecting to Asterisk, restart application.";
                }
            }
            catch (Exception err)
            {
                log.log("Error connecting to Asterisk, please verify config file and connectivity");
                lblInfo.Text = "Error connecting to Asterisk, restart application.";
            }

        }

        private void btnDial_Click(object sender, EventArgs e)
        {
            if (txtPhoneToDial.Text.Trim().Length > 0)
            {
                log.log("Dialing " + txtPhoneToDial.Text + " for extension " + sExtension + " on context " + sContext);
                ac.Dial(txtPhoneToDial.Text, sExtension, sContext);
                lblInfo.Text = "Dialing " + txtPhoneToDial.Text + "...";
            }
        }
    }
}
