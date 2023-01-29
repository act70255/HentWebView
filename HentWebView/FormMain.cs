using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace HentWebView
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            InitializeList();
            KeyPreview = true;
            listBox.SelectedValueChanged += ListBox_SelectedValueChanged;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            OnHotKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OnHotKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F1:
                    SelectFolder();
                    break;
                case Keys.F2:
                    SelectFolder(true);
                    break;
                case Keys.F3:
                    ExecuteFile(Path.GetDirectoryName(Application.ExecutablePath));
                    break;
            }
        }

        public void ExecuteFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Not Found");
                return;
            }
            var process = new System.Diagnostics.ProcessStartInfo();
            process.UseShellExecute = true;
            process.FileName = path;
            System.Diagnostics.Process.Start(process);
        }

        private void ListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.ListBox ctrl && !string.IsNullOrEmpty(ctrl.Text))
            {
                var htmlPath = $"{Path.GetDirectoryName(Application.ExecutablePath)}\\{ctrl.Text}";
                ExecuteFile(ctrl.Text);
                ctrl.ClearSelected();
            }
        }

        private void InitializeList()
        {
            var execPath = Path.GetDirectoryName(Application.ExecutablePath);
            var files = Directory.GetFiles(execPath);
            var htmlList = files.Where(f => Path.GetExtension(f) == ".html").Select(s => Path.GetFileName(s));
            listBox.Items.Clear();
            listBox.Items.AddRange(htmlList.ToArray());
        }

        private void SelectFolder(bool expandSubFolder = false)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Application.ExecutablePath;
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                var path = fbd.SelectedPath;
                if (expandSubFolder)
                {
                    var folders = Directory.GetDirectories(path);
                    foreach (var folder in folders)
                    {
                        WriteToHtml(folder);
                    }
                }
                else
                {
                    var htmlfile = WriteToHtml(path);
                    ExecuteFile(htmlfile);
                }
            }
        }

        private string WriteToHtml(string path)
        {
            var html = GetContent(path);
            if (!string.IsNullOrEmpty(html))
            {
                var dicname = new DirectoryInfo(path).Name;
                var htmlPath = $"{Path.GetDirectoryName(Application.ExecutablePath)}\\{dicname}.html";
                File.WriteAllText(htmlPath, html);
                InitializeList();
                return htmlPath;
            }
            else
            {
                return "";
            }
        }

        private string GetContent(string path)
        {
            string[] extensions = { ".png", ".jpg", ".jpeg" };
            var imgs = Directory.GetFiles(path).Where(f => extensions.Contains(Path.GetExtension(f)));
            if (imgs.Count() > 0)
            {
                var imginfos = imgs.OrderBy(o => o).Select((s, i) => $"<div style=\"text-align:center;color:#999;padding-bottom:10px;font-size:13px;\"><img src=\"{s}\" width=\"99%\"><br><span>{i}/{imgs.Count()}</span></div>");
                string content = string.Join(Environment.NewLine, imginfos);
                string html = $@"<html>
            <head>
                <title>{new DirectoryInfo(path).Name}</title>
            </head>
            <body>
                {content}
            </body>
            </html>";
                return html;
            }
            else
                return "";
        }
    }
}