using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utility.Models
{
    /// <summary>
    /// 功能物件
    /// </summary>
    public class OptionObject
    {
        public OptionObject()
        {
        }
        public OptionObject(string name, string description, Action action)
        {
            Name = name;
            Description = description;
            OnOptionAction = action;
        }
        public OptionObject(string name, string description)
        {
            Name = name;
            Description = description;
        }
        /// <summary>
        /// 畫面按鈕名稱
        /// </summary>
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 設定參數選項
        /// </summary>
        public List<ParamObject> ParameterObject { get; set; } = new List<ParamObject>();
        /// <summary>
        /// Click事件執行動作
        /// </summary>
        public Action OnOptionAction { get; set; }
        public Action<Controls.DynamicUI.FormOption, OptionObject> OnOptionObjAction { get; set; }
    }
}
