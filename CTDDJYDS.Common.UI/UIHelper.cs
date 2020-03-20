using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTDDJYDS.Common.UI
{
    public class UIHelper
    {
        /// <summary>
        /// 获取窗体上的控件
        /// </summary>
        /// <param name="form"></param>
        public static void GetFormControl(Form form)
        {
            System.Reflection.FieldInfo[] fieldInfo = form.GetType().GetFields(System.Reflection.BindingFlags.NonPublic |
System.Reflection.BindingFlags.Instance);
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                switch (fieldInfo[i].FieldType.Name)
                {
                    case "Button":
                        {
                            Button btn = (Button)fieldInfo[i].GetValue(form);
                            break;
                        }
                }
            }
        }
    }
}
