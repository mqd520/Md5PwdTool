using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Md5PwdTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                button1_Click_1(button1, null);

                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string str1 = "";
            string str2 = "";
            string clearText = textBox1.Text.Trim();
            string key = textBox2.Text.Trim();

            bool b1 = radioButton1.Checked ? true : false;      // whether 32 bit 
            bool b2 = radioButton3.Checked ? true : false;      // whether lower case

            if (b1)
            {
                str1 = Common.Md5EncryptionTool.Encrypt(clearText, !b2);
                str2 = Common.Md5EncryptionTool.Encrypt(str1 + key, !b2);
            }
            else
            {
                str1 = Common.Md5EncryptionTool.Encrypt16(clearText, !b2);
                str2 = Common.Md5EncryptionTool.Encrypt(str1 + key, !b2);
            }

            textBox3.Text = str1;
            textBox4.Text = str2;
        }
    }
}
