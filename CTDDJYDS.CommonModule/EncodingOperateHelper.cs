using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTDDJYDS.CommonModule
{
    public class EncodingOperateHelper
    {
        public static void ToHexString(byte[] bytes, TextWriter sw)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return;
            }

            try
            {
                byte b;

                for (int bx = 0; bx < bytes.Length; ++bx)
                {
                    b = (byte)(bytes[bx] >> 4);
                    sw.Write((char)(b > 9 ? b - 10 + 'A' : b + '0'));

                    b = ((byte)(bytes[bx] & 0x0F));
                    sw.Write((char)(b > 9 ? b - 10 + 'A' : b + '0'));
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Log(ex);
            }
        }
    }
}
