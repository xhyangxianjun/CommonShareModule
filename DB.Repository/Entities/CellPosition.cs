using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Chaint.Data.SqlSugar.Entities
{
    public class CellPosition
    {
        /// <summary>
        /// 排
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public int Bay { get; set; }

        /// <summary>
        /// 层
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 深位数
        /// </summary>
        public int LocationID { get; set; }

        public CellPosition(int row,int bay,int level,int locationID)
        {
            this.Row = row;
            this.Bay = bay;
            this.Level = level;
            this.LocationID = locationID;
        }

        /// <summary>
        /// AABBCCDD
        /// </summary>
        /// <param name="strCell"></param>
        public CellPosition(string strCell)
        {
            int Length = strCell.Length / 4;
            this.Row = Convert.ToInt32(strCell.Substring(0, Length));
            this.Bay = Convert.ToInt32(strCell.Substring(Length, Length));
            this.Level = Convert.ToInt32(strCell.Substring(2 * Length, Length));
            this.LocationID= Convert.ToInt32(strCell.Substring(3 * Length, Length));
        }

        public string ToString(int formatLength)
        {
            string FormatString = String.Format("D{0}", formatLength);

            return
                    String.Concat(
                    Row.ToString(FormatString),
                    Bay.ToString(FormatString),
                    Level.ToString(FormatString),
                    LocationID.ToString(FormatString));
        }

        public override string ToString()
        {
            return ToString(2);
        }

    }
}
