using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SoulCollecter
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (!txtUserAgent.Equals(""))
            {
                webView1.CustomUserAgent = txtUserAgent.Text;
            }

            if (!txtSignInURL.Equals(""))
            {
                webView1.LoadUrl(txtSignInURL.Text);
            }

        }
        private void Main_Load(object sender, EventArgs e)
        {
            string path = "./setting.ini";
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader("./setting.ini"))
                {
                    string line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        switch (i)
                        {
                            case 0: txtSignInURL.Text = line; break;
                            case 1: txtUserAgent.Text = line; break;
                            case 2: txtDomain.Text = line; goto done;
                        }
                        i++;
                    }
                    done: Console.WriteLine("Read Complete");
                }
            }
        }

        private void BtnPick_Click(object sender, EventArgs e)
        {
            if (txtDomain.Text.Length > 0)
            {
                EO.WebEngine.CookieCollection cc = webView1.Engine.CookieManager.GetCookies(txtDomain.Text);
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < cc.Count; i++)
                {
                    stringBuilder.Append(cc[i].Name + "=" + cc[i].Value + ";");
                }
                string cookie = stringBuilder.ToString();
                string userAgent = txtUserAgent.Text;
                if (cookie.Length > 0 && userAgent.Length > 0)
                {
                    Encoding encode = Encoding.ASCII;
                    byte[] byteData = encode.GetBytes("ck=" + stringBuilder.ToString() + "\r\nua=" + userAgent);
                    string result = Convert.ToBase64String(byteData, 0, byteData.Length);
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".soul";
                    saveFileDialog.FileName = fileName;
                    saveFileDialog.Filter = "soul file(*.soul)|*.soul";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fullName = saveFileDialog.FileName;
                        using (StreamWriter file = new StreamWriter(fullName, false))
                        {
                            file.Write(result);
                        }
                        MessageBox.Show("Saved");
                    }
                }
            }
        }
    }
}
