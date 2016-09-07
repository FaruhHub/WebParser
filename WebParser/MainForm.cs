using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using WebParser.Code;

namespace WebParser
{
    public partial class MainForm : Form
    {
        private Checker _cheker;
        public MainForm()
        {
            InitializeComponent();
            _cheker = new Checker();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = _cheker.WebChecker(textBoxUrl.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBoxUrl.Text = String.Empty;
            richTextBox1.Text = String.Empty;
        }
    }
}
