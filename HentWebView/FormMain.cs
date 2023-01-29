using System.Diagnostics;
using System.Windows.Forms;

namespace HentWebView
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            InitializeList();
            txtPath.Click += TxtPath_Click;
            listBox.SelectedValueChanged += ListBox_SelectedValueChanged;
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

        private void TxtPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
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
                <title>Basic Web Page</title>
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