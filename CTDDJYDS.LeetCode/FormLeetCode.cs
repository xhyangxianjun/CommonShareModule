using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTDDJYDS.LeetCode
{
    public partial class FormLeetCode : Form
    {
        public FormLeetCode()
        {
            InitializeComponent();
        }
        //两数之和
        private void buttonTwoSum_Click(object sender, EventArgs e)
        {
            TwoSum(new int[5] { 1, 2, 3, 4, 5 }, 4);
        }
        public int[] TwoSum(int[] nums, int target)
        {
            int length = nums.Length;
            int[] sum = new int[2];
            Hashtable hashTable = new Hashtable();
            for (int i = 0; i < length; i++)
            {
                int a = nums[i];
                if(hashTable.ContainsKey(a))
                {
                    sum[0] = i;
                    sum[1] = (int)hashTable[a];
                    return sum;
                }
                int b = target - nums[i];
                if (hashTable.ContainsKey(b)==false)
                    hashTable.Add(b, i);
            }
                //for (int i = 0; i < length-1;i++)
                //{
                //    int a = nums[i];
                //    bool isFind = false;
                //    for(int j=i+1;j<length;j++)
                //    {
                //        int b = nums[j];
                //        if(target==a+b)
                //        {
                //            sum[0] = i;
                //            sum[1] = j;
                //            isFind = true;
                //            break;
                //        }
                //    }
                //    if (isFind)
                //        break;
                //}
                return sum;
        }
    }
}
