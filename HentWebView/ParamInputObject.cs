using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models
{
    /// <summary>
    /// 功能參數輸入項物件
    /// </summary>
    public class ParamInputObject
    {
        /// <summary>
        /// 輸入元件(TextBox、DateTimePicker、ComboBox)
        /// </summary>
        public Type ParamInputType { get; set; }
        /// <summary>
        /// ComboBox的選項
        /// </summary>
        public List<SourceItem> OptionSource { get; set; } = new List<SourceItem>();
        public class SourceItem
        {
            public SourceItem(string text, object value)
            {
                Text = text;
                Value = value;
            }

            public string Text { get; set; }
            public object Value { get; set; }
        }
    }
}
