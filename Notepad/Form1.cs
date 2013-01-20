using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace Notepad
{
    public partial class Form1 : Form
    {

        private String lastFile = null;
        private String lastPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private String lastLineEnding = null;

        public Form1(String[] args)
        {
            InitializeComponent();

            if(args.Length > 0 && args[0].Length > 0)
                Open(args[0]);
        }

        private void Open(String path)
        {
            lastPath = Path.GetDirectoryName(path);
            lastFile = path;
            String text = File.ReadAllText(lastFile);

            int rn = new Regex("\r\n", RegexOptions.Multiline).Matches(text).Count;
            int n = new Regex("[^\r]\n", RegexOptions.Multiline).Matches(text).Count;
            if (rn > 0 && n == 0)
            {
                lastLineEnding = "\r\n";
                txtEditor.Text = text.Replace("\r\n", Environment.NewLine);
            }
            else if (n > 0 && rn == 0)
            {
                lastLineEnding = "\n";
                txtEditor.Text = text.Replace("\n", Environment.NewLine);
            }
            else
            {
                lastLineEnding = null;
                txtEditor.Text = text;
            }
        }

        #region Menus
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = lastPath;
            dialog.Filter = "Text Documents (*.txt)|*.txt|All Files (*.*)|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            Open(dialog.FileName);
        }

        private void aboutNotepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog(this);
        }
        #endregion
    }
}
