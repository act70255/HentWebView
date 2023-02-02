using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utility.Controls
{
    public class ExtPopupText : TextBox
    {
        public bool AllowType { get; set; }
        public IEnumerable<string> DataSource { get; set; }
        public ExtPopupText()
        {
            Click += (s, e) => ShowOptionsDialog();
        }
        public void ShowOptionsDialog()
        {
            if (DataSource == null || !DataSource.Any())
                return;

            var list = new ExtListBox(DataSource.ToArray())
            {
                Dock = DockStyle.Fill
            };
            var frmOptionsDialog = new Form()
            {
                BackColor = list.BackColor,
                Size = new System.Drawing.Size(150, 100),
                StartPosition = FormStartPosition.CenterScreen,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
        };
            frmOptionsDialog.Controls.Add(list);
            list.OnSelected = s =>
            {
                Text = s;
                frmOptionsDialog.Close();
            };
            frmOptionsDialog.ShowDialog();
        }
    }
}
