using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utility.Controls
{
    public class ExtListBox : ListBox
    {
        public Action<string> OnSelected;
        public ExtListBox()
        {
            init();
        }
        public ExtListBox(string[] source) : base()
        {
            init();
            RefreshList(source);
        }
        void init()
        {
            BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            ForeColor = System.Drawing.Color.FromArgb(255, 175, 45);
            Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            BorderStyle = System.Windows.Forms.BorderStyle.None;
            SelectedIndexChanged += (s, e) =>
            {
                if (s is ExtListBox ctrl && !string.IsNullOrEmpty(ctrl.Text))
                {
                    OnSelected(ctrl.Text);
                    ctrl.ClearSelected();
                }
            };
        }
        public void RefreshList(List<string> source)
        {
            RefreshList(source.ToArray());
        }
        public void RefreshList(string[] source)
        {
            Items.Clear();
            Items.AddRange(source);
        }
    }
}
