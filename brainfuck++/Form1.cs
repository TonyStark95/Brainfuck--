using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace Brainfuck___Compliler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "Welcome to Brainfuck++ IDE";
        }

        string name;
        string code;
        string syntax = "(@|#|$|!|^|*)";
        char[] tab = {};
        string output;
        int pointer = 0;
        int i = 0;
        bool error = false; //obsługa logu błędów - plik *.error

        /////////////////////////////////////////////////////////////////////////////////
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void syntaxListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 syntax = new Form2();
            syntax.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 about = new Form3();
            about.Show();
        }

        //zapisywanie kodu do pliku
        private void saveCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.richTextBox1.Text))
            {
                textBox1.Text = "Error[002]: Fatal error: Textbox is empty.";
            }

            else
            {
                saveFileDialog1.ShowDialog();
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            name = saveFileDialog1.FileName;
            File.WriteAllText(name, richTextBox1.Text); 
        }

        //otwieranie pliku i ładowanie jego zawartości
        private void loadCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            name = openFileDialog1.FileName;

            if (File.Exists(name) == false)
            {
                textBox1.Text = "Error[001]: File system error: File not exist or cannot open.";
            }

            else
            {
                richTextBox1.Text = File.ReadAllText(name);
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void compileFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkError();
        }

        //kompilacja (interpretacja) kodu
        private void compile()
        {
            code = richTextBox1.Text;
            output = textBox1.Text;
 
            foreach(char cmd in code)
            {
                if (cmd == '@') output = code[i+2].ToString();
                if (cmd == '>') pointer++;
                if (cmd == '<') pointer--;
                if (cmd == '#') output = tab[pointer].ToString();
                if (cmd == '^') output = "\n";
                if (cmd == '$') tab[pointer] = '0';
                if (cmd == '!') output = "666";
                //if (cmd == '*') tab[pointer] = code[2];

                i++;
            }
        }

        //błędy
        private void checkError()
        {
            errorProvider1.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;
            errorProvider1.SetIconAlignment(this, ErrorIconAlignment.MiddleLeft);
            errorProvider1.SetIconPadding(this, 2);

            if (richTextBox1.Text == "%" || richTextBox1.Text == "(" || richTextBox1.Text == ")" ||
                richTextBox1.Text == "-" || richTextBox1.Text == "_" || richTextBox1.Text == "=" ||
                richTextBox1.Text == "+" || richTextBox1.Text == "&")
            {
                error = true;
                errorProvider1.SetError(richTextBox1, "Error[003]: Syntax error: unknown commands.");
            }

            if (richTextBox1.Text == "ą" || richTextBox1.Text == "ż" || richTextBox1.Text == "ź" ||
                richTextBox1.Text == "ę" || richTextBox1.Text == "ó" || richTextBox1.Text == "ł" ||
                richTextBox1.Text == "ć")
            {
                error = true;
                errorProvider1.SetError(richTextBox1, "Error[003]: Syntax error: unknown chars.");
            }

            else
            {
                errorProvider1.Clear();
                error = false;
                compile();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //checkError(); //testing
        }
    }
}