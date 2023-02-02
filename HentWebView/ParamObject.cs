using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models
{
    /// <summary>
    /// 功能參數物件
    /// </summary>
    public class ParamObject
    {
        /// <summary>
        /// 選項名稱
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 設定選項物件(TextBox、DateTimePicker、ComboBox)
        /// </summary>
        public ParamInputObject ParamInput { get; set; }
        /// <summary>
        /// 預設值、輸入值
        /// </summary>
        public object InputValue { get; set; }
        /// <summary>
        /// 對應到OptionObject.ApiParam 的參數Key
        /// </summary>
        public string ParameterName { get; set; }
    }
}
