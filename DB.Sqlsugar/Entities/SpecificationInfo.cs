using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*-----------------------------------------------------------------------------------
 * 作者: AI Dept. 
 * 
 * 创建时间: 2017-06-21
 * 
 * 功能描述: 
 *      规格信息
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Data.SqlSugar.Entities
{

    public class SpecificationInfo
    {
        public int OID { get; set; }
        public string SpecificationCode { get; set; }

        public string SpecificationName { get; set; }

        public int Level1Count { get; set; }

        public string Level1Unit { get; set; }

        public int Level2Count { get; set; }

        public string Level2Unit { get; set; }

        public int Level3Count { get; set; }

        public string Level3Unit { get; set; }

        
        public bool IsEnable { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }




    }
}
