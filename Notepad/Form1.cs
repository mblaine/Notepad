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

            this.Text = Path.GetFileName(lastFile) + " - Notepad";
        }

        private void Save(String path, String text, String lineEnding = null)
        {
            if (lineEnding != null)
                text = text.Replace(Environment.NewLine, lineEnding);
            File.WriteAllText(path, text);
            this.Text = Path.GetFileName(path) + " - Notepad";
        }

        #region Menus

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeToolStripMenuItem_Click(sender, e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = lastPath;
            dialog.Filter = "Text Documents (*.txt)|*.txt|All Files (*.*)|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            Open(dialog.FileName);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastFile != null)
            {
                lastFile = null;
                lastLineEnding = null;
                txtEditor.Text = "";
                this.Text = "Untitled - Notepad";
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastFile != null)
            {
                Save(lastFile, txtEditor.Text, lastLineEnding);
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = lastPath;
            dialog.FileName = Path.GetFileName(lastFile); ;
            dialog.Filter = "Text Documents (*.txt)|*.txt|All Files (*.*)|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            lastPath = Path.GetDirectoryName(dialog.FileName);
            lastFile = dialog.FileName;

            Save(lastFile, txtEditor.Text, lastLineEnding);
        }

        private void pageToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtEditor.Undo();

        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtEditor.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtEditor.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtEditor.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtEditor.Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void goToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutNotepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog(this);
        }
        #endregion
    }
}
