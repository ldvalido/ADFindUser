using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Text.RegularExpressions;

namespace ADFindUser
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        string findUser(string userAccount) 
        {
            string returnValue = String.Empty;
            DirectoryEntry entry = new DirectoryEntry("LDAP://");
            // Filter the domain out of the useraccount
            
            string account = userAccount.Replace(@"LA\", "");
            try
            {
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + account + ")";
                search.PropertiesToLoad.Add("displayName");
                Regex regex = new Regex(@"(?<txtComillas>"".*\"")");
                StringBuilder sb = new StringBuilder();
                foreach (Group g in regex.Matches(account)) 
                {
                    sb.Append(g.Value);
                }
                sb.ToString();
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    returnValue =  result.Properties["displayname"][0].ToString();
                }
                else
                {
                    returnValue =  "Unknown User";
                }
            }
            catch (Exception ex)
            {
                string debug = ex.Message;
            }
            return returnValue;
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(txtName.Text)) 
            {
                lblResult.Text = String.Concat("User Name: ", txtName.Text , ": ", findUser(txtName.Text));
            }
        }
    }
}