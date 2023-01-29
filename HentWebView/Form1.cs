using System.Diagnostics;
using System.Windows.Forms;

namespace HentWebView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeList();
            txtPath.Click += TxtPath_Click;
            listBox.SelectedValueChanged += ListBox_SelectedValueChanged;
        }
        public void ExecuteFile(string path)
        {
            var process = new System.Diagnostics.ProcessStartInfo();
            process.UseShellExecute = true;
            process.FileName= path;
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

        private void TxtPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.InitialDirectory= Application.ExecutablePath;
            fbd.SelectedPath = Application.ExecutablePath;
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                ProcessPath(fbd.SelectedPath);
            }
        }

        private void ProcessPath(string path)
        {
            var htmlfile = WriteToHtml(path);
            ExecuteFile(htmlfile);
            //ShowOnBrowser(path);
        }

        private string WriteToHtml(string path)
        {
            var html = GetContent(path);
            var dicname = new DirectoryInfo(path).Name;
            var htmlPath = $"{Path.GetDirectoryName(Application.ExecutablePath)}\\{dicname}.html";
            File.WriteAllText(htmlPath, html);
            InitializeList();
            return htmlPath;
        }

        private string GetContent(string path)
        {
            var imgs = Directory.GetFiles(path);
            var imginfos = imgs.OrderBy(o => o).Select((s, i) => $"<div style=\"text-align:center;color:#999;padding-bottom:10px;font-size:13px;\"><img src=\"{s}\" width=\"99%\"><br><span>{i}/{imgs.Length}</span></div>");
            string content = string.Join(Environment.NewLine, imginfos);
            string html = $@"<html>
            <head>
                <title>Basic Web Page</title>
            </head>
            <body>
                {content}
            </body>
            </html>";
            return html;
        }
    }
}