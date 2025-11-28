using labbproject2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace labproject2
{
    public partial class Form1 : Form
    {
        private RadioButton rbSax;
        private RadioButton rbDom;
        private RadioButton rbLinq;
        private TextBox txtAuthor;
        private Button btnSearch;
        private Button btnTransform;
        private RichTextBox rtbResults;
        private Label lblAuthor;
        private GroupBox gbStrategy;

        private const string XmlPath = "library.xml";
        private const string XslPath = "style.xsl";
        private const string HtmlPath = "output.html";

        public Form1()
        {
            this.Text = "Library Data Analyzer";
            this.Size = new Size(550, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            CreateInterface();
        }

        private void CreateInterface()
        {
            gbStrategy = new GroupBox();
            gbStrategy.Text = "Step 1: Choose Analysis Method (Strategy)";
            gbStrategy.Location = new Point(20, 20);
            gbStrategy.Size = new Size(500, 70);
            gbStrategy.Font = new Font(this.Font, FontStyle.Bold);

            rbSax = new RadioButton { Text = "SAX API (Memory Efficient)", Location = new Point(20, 30), AutoSize = true, Font = new Font(this.Font, FontStyle.Regular) };
            rbDom = new RadioButton { Text = "DOM API (Tree Structure)", Location = new Point(190, 30), AutoSize = true, Font = new Font(this.Font, FontStyle.Regular) };
            rbLinq = new RadioButton { Text = "LINQ to XML (Modern)", Location = new Point(350, 30), AutoSize = true, Checked = true, Font = new Font(this.Font, FontStyle.Regular) };

            gbStrategy.Controls.Add(rbSax);
            gbStrategy.Controls.Add(rbDom);
            gbStrategy.Controls.Add(rbLinq);
            this.Controls.Add(gbStrategy);

            lblAuthor = new Label { Text = "Step 2: Enter Author Name (or leave empty):", Location = new Point(20, 110), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            txtAuthor = new TextBox { Location = new Point(20, 135), Size = new Size(500, 25), Font = new Font(this.Font, FontStyle.Regular) };

            this.Controls.Add(lblAuthor);
            this.Controls.Add(txtAuthor);

            btnSearch = new Button { Text = "Find Books in XML", Location = new Point(20, 180), Size = new Size(240, 40), BackColor = Color.LightBlue };
            btnTransform = new Button { Text = "Generate HTML Report", Location = new Point(280, 180), Size = new Size(240, 40), BackColor = Color.LightGreen };

            btnSearch.Click += BtnSearch_Click;
            btnTransform.Click += BtnTransform_Click;

            this.Controls.Add(btnSearch);
            this.Controls.Add(btnTransform);

            rtbResults = new RichTextBox();
            rtbResults.Location = new Point(20, 240);
            rtbResults.Size = new Size(500, 350);
            rtbResults.ReadOnly = true;
            rtbResults.BorderStyle = BorderStyle.Fixed3D;
            rtbResults.BackColor = Color.WhiteSmoke;

            this.Controls.Add(rtbResults);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            rtbResults.Clear();
            string authorCriteria = txtAuthor.Text.Trim();

            ISearchStrategy strategy = null;

            if (rbSax.Checked) strategy = new SaxSearchStrategy();
            else if (rbDom.Checked) strategy = new DomSearchStrategy();
            else if (rbLinq.Checked) strategy = new LinqSearchStrategy();

            if (strategy == null) return;

            try
            {
                List<Book> results = strategy.Search(XmlPath, authorCriteria);
                DisplayResults(results);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading XML file:\n" + ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayResults(List<Book> books)
        {
            if (books.Count == 0)
            {
                rtbResults.Text = "No books found matching criteria.";
                return;
            }

            rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Bold);
            rtbResults.AppendText($"Total Books Found: {books.Count}\n\n");

            foreach (var book in books)
            {
                rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Bold);
                rtbResults.AppendText($"Title: ");
                rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Regular);
                rtbResults.AppendText($"\"{book.Title}\"\n");

                rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Bold);
                rtbResults.AppendText($"Author: ");
                rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Regular);
                rtbResults.AppendText($"{book.Author}\n");

                rtbResults.AppendText($"Category: {book.Category}\n");
                rtbResults.AppendText($"Current Reader: {book.ReaderName}\n");
                rtbResults.AppendText("--------------------------------------------------\n");
            }
        }

        private void BtnTransform_Click(object sender, EventArgs e)
        {
            try
            {
                var transformer = new HtmlTransformer();
                transformer.Transform(XmlPath, XslPath, HtmlPath);

                var result = MessageBox.Show(
                    "HTML Report generated successfully!\n\nWould you like to open it in your browser?",
                    "Report Ready",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(Path.GetFullPath(HtmlPath)) { UseShellExecute = true };
                    p.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Transformation failed:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}