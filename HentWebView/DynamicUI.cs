using Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utility.Controls
{
    public class DynamicUI
    {
        public FormOption Sample()
        {
            FormOption formOption;
            List<OptionObject> options;
            options = new List<OptionObject>
            {
                new OptionObject("TEST", "TEST")
                {
                    ParameterObject = new List<ParamObject>
                    {
                        new ParamObject
                        {
                            Description="執行日",
                            ParamInput = new ParamInputObject{ ParamInputType = typeof(DateTimePicker) },
                            InputValue = DateTime.Today,
                            ParameterName="DATE"
                        },
                        new ParamObject
                        {
                            Description="版本",
                            ParamInput = new ParamInputObject
                            {
                                ParamInputType = typeof(ComboBox),
                                OptionSource = new List<ParamInputObject.SourceItem>
                                {
                                    new ParamInputObject.SourceItem("V1","V1"),
                                    new ParamInputObject.SourceItem("V2","V2"),
                                    new ParamInputObject.SourceItem("V3","V3"),
                                }
                            },
                            ParameterName="VERSION",
                            InputValue = $"{(DateTime.Today.Day == 1 ? "V1" : DateTime.Today.Day == 15 ? "V2" : "V3")}"
                        },
                    },
                    OnOptionObjAction = (frm,obj) =>
                    {
                        frm.ShowMessageNewLine(string.Join(",", obj.ParameterObject.Select(s => s.InputValue.ToString())));
                        MessageBox.Show(string.Join(",", obj.ParameterObject.Select(s => s.InputValue.ToString())));
                    }
                },
                new OptionObject("EXIT", "關閉", ()=>{ Application.Exit(); }),
            };

            formOption = FormMainOption(options);
            return formOption;
        }
        public FormOption FormMainOption(List<OptionObject> options)
        {
            var formOption = new FormOption(options);
            return formOption;
        }
        public class FormOption : Form
        {
            public ControlEventHandler OptionChecked;
            public Control.ControlCollection ContentControls { get { return pnlContent.Controls; } }

            public FormOption(List<OptionObject> options, int contentHeight = 100, int contentWidth = 250, int consoleWidth = 400)
            {
                var yPos = 25;
                foreach (var option in options)
                {
                    var btn = new Button
                    {
                        Location = new System.Drawing.Point(50, yPos),
                        Size = new System.Drawing.Size(150, 36),
                        Text = option.Description,
                        Tag = option
                    };
                    btn.Click += (s, e) =>
                    {
                        OptionChecked?.Invoke(option, new ControlEventArgs(btn));
                        if (OptionChecked == null)
                        {
                            if (option.ParameterObject.Any())
                            {
                                var frmParams = new DynamicUI().FormParameter(option);
                                var res = frmParams.ShowDialog();
                                if (res == DialogResult.OK)
                                {
                                    option.OnOptionObjAction(this, option);
                                }
                            }
                            else
                                if (option.OnOptionObjAction != null)
                                option.OnOptionObjAction(this, option);
                            else if (option.OnOptionAction != null)
                                option.OnOptionAction();
                            else
                                ShowMessageNewLine(option.Description);
                        }
                    };
                    ContentControls.Add(btn);
                    yPos += 50;
                    contentHeight += 50;
                }
                AutoSizeMode = AutoSizeMode.GrowAndShrink;
                var width = contentWidth + consoleWidth;
                Size = new System.Drawing.Size(width, contentHeight);
                txtOutpot.Size = new System.Drawing.Size(consoleWidth, contentHeight);
                Controls.Add(txtOutpot);
                Controls.Add(pnlContent);
            }

            #region Controls
            TextBox txtOutpot = new TextBox
            {
                BackColor = System.Drawing.Color.Black,
                ForeColor = System.Drawing.Color.White,
                Multiline = true,
                WordWrap = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Right,
                Location = new System.Drawing.Point(0, 0),
                Size = new System.Drawing.Size(150, 50),
            };
            Panel pnlContent = new Panel
            {
                Dock = DockStyle.Fill
            };
            #endregion

            public void ShowMessageNewLine(string message)
            {
                txtOutpot.AppendText($"{message}{Environment.NewLine}");
            }
        }
        public Form FormParameter(OptionObject option)
        {
            if (option.ParameterObject.Any())
            {
                Form formSubOption = new Form
                {
                    Size = new System.Drawing.Size(260, 60),
                    Text = $"{option.Description}執行參數",
                    StartPosition = FormStartPosition.CenterScreen
                };
                formSubOption.Load += (s, e) =>
                {
                    if (s is Form frm)
                    {
                        foreach (var ctrl in frm.Controls)
                        {
                            if (option.ParameterObject.Select(sel => sel.ParamInput.ParamInputType).Contains(ctrl.GetType()))
                            {
                                if (ctrl is TextBox txt && txt.Tag is ParamObject txtParam && txtParam.InputValue is string strValue)
                                {
                                    txt.Text = strValue;
                                }
                                else if (ctrl is DateTimePicker dtp && dtp.Tag is ParamObject dtpParam && dtpParam.InputValue is DateTime dtValue)
                                {
                                    dtp.Value = dtValue;
                                }
                                else if (ctrl is ComboBox cbx && cbx.Tag is ParamObject param && param.InputValue is object objValue)
                                {
                                    cbx.SelectedValue = objValue;
                                }
                            }
                        }
                    }
                };
                var posX = 30;
                var posY = 30;
                foreach (var item in option.ParameterObject)
                {
                    formSubOption.Controls.Add(new Label
                    {
                        Text = item.Description,
                        Size = new System.Drawing.Size(150, 30),
                        Location = new System.Drawing.Point(posX, posY),
                    });
                    posY += 40;
                    Control paramOption = GetParameterControl(item);
                    if (paramOption != null)
                    {
                        paramOption.Size = new System.Drawing.Size(150, 30);
                        paramOption.Location = new System.Drawing.Point(posX, posY);
                        posY += 50;
                        formSubOption.Controls.Add(paramOption);
                    }
                }
                var btnOK = new Button
                {
                    Text = "執行",
                    Size = new System.Drawing.Size(90, 30),
                    Location = new System.Drawing.Point(30, posY)
                };
                btnOK.Click += (s, e) => { formSubOption.DialogResult = DialogResult.OK; };
                formSubOption.Controls.Add(btnOK);

                var btnCancel = new Button
                {
                    Text = "取消執行",
                    Size = new System.Drawing.Size(90, 30),
                    Location = new System.Drawing.Point(130, posY)
                };
                btnCancel.Click += (s, e) => { formSubOption.DialogResult = DialogResult.Cancel; };
                posY += 90;
                formSubOption.Controls.Add(btnCancel);
                formSubOption.Size = new System.Drawing.Size(formSubOption.Size.Width, posY);
                return formSubOption;
            }
            else
            {
                throw new NullReferenceException("OptionObject is null or empty");
            }

            Control GetParameterControl(ParamObject param)
            {
                object result = Activator.CreateInstance(param.ParamInput.ParamInputType);

                //檢查是否為控制項
                if (!(result is Control))
                    return null;

                //處理文字輸入框
                if (result is TextBox ctxt && param.InputValue is string vstr)
                {
                    ctxt.Tag = param;
                    ctxt.Text = vstr;
                    ctxt.TextChanged += (s, e) =>
                    {
                        param.InputValue = (s as TextBox).Text;
                    };
                }
                //處理時間輸入框
                if (result is DateTimePicker cdtp && param.InputValue is DateTime vdt)
                {
                    cdtp.Tag = param;
                    cdtp.Value = vdt;
                    cdtp.ValueChanged += (s, e) =>
                    {
                        param.InputValue = (s as DateTimePicker).Value;
                    };
                }
                //處理下拉選單
                if (result is ComboBox ccbx && param.ParamInput.OptionSource.Any())
                {
                    ccbx.Tag = param;
                    ccbx.DataSource = param.ParamInput.OptionSource;
                    ccbx.DisplayMember = "Text";
                    ccbx.ValueMember = "Value";
                    ccbx.SelectedValueChanged += (s, e) =>
                    {
                        param.InputValue = (s as ComboBox).SelectedValue;
                    };
                }
                return result as Control;
            }
        }
    }
}
