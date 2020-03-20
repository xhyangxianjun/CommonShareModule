namespace CTDDJYDS.LeetCode
{
    partial class FormLeetCode
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonTwoSum = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTwoSum
            // 
            this.buttonTwoSum.Location = new System.Drawing.Point(12, 12);
            this.buttonTwoSum.Name = "buttonTwoSum";
            this.buttonTwoSum.Size = new System.Drawing.Size(92, 34);
            this.buttonTwoSum.TabIndex = 0;
            this.buttonTwoSum.Text = "两数之和";
            this.buttonTwoSum.UseVisualStyleBackColor = true;
            this.buttonTwoSum.Click += new System.EventHandler(this.buttonTwoSum_Click);
            // 
            // FormLeetCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 686);
            this.Controls.Add(this.buttonTwoSum);
            this.Name = "FormLeetCode";
            this.Text = "力扣";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTwoSum;
    }
}

