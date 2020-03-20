namespace DemoSharpMap
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.spCtrl = new System.Windows.Forms.SplitContainer();
            this.btnShapFile = new System.Windows.Forms.Button();
            this.btnMapInfo = new System.Windows.Forms.Button();
            this.btnDo2 = new System.Windows.Forms.Button();
            this.btnDo = new System.Windows.Forms.Button();
            this.mapDigitizeGeometriesToolStrip1 = new SharpMap.Forms.ToolBar.MapDigitizeGeometriesToolStrip(this.components);
            this.mapBox1 = new SharpMap.Forms.MapBox();
            this.mapZoomToolStrip1 = new SharpMap.Forms.ToolBar.MapZoomToolStrip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnSave = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.spCtrl)).BeginInit();
            this.spCtrl.Panel1.SuspendLayout();
            this.spCtrl.Panel2.SuspendLayout();
            this.spCtrl.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spCtrl
            // 
            this.spCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spCtrl.Location = new System.Drawing.Point(0, 0);
            this.spCtrl.Name = "spCtrl";
            // 
            // spCtrl.Panel1
            // 
            this.spCtrl.Panel1.Controls.Add(this.btnShapFile);
            this.spCtrl.Panel1.Controls.Add(this.btnMapInfo);
            this.spCtrl.Panel1.Controls.Add(this.btnDo2);
            this.spCtrl.Panel1.Controls.Add(this.btnDo);
            // 
            // spCtrl.Panel2
            // 
            this.spCtrl.Panel2.Controls.Add(this.mapDigitizeGeometriesToolStrip1);
            this.spCtrl.Panel2.Controls.Add(this.mapZoomToolStrip1);
            this.spCtrl.Panel2.Controls.Add(this.toolStrip1);
            this.spCtrl.Panel2.Controls.Add(this.mapBox1);
            this.spCtrl.Size = new System.Drawing.Size(833, 562);
            this.spCtrl.SplitterDistance = 212;
            this.spCtrl.TabIndex = 0;
            // 
            // btnShapFile
            // 
            this.btnShapFile.Location = new System.Drawing.Point(62, 108);
            this.btnShapFile.Name = "btnShapFile";
            this.btnShapFile.Size = new System.Drawing.Size(75, 23);
            this.btnShapFile.TabIndex = 5;
            this.btnShapFile.Text = "ShapFile";
            this.btnShapFile.UseVisualStyleBackColor = true;
            this.btnShapFile.Click += new System.EventHandler(this.btnShapFile_Click);
            // 
            // btnMapInfo
            // 
            this.btnMapInfo.Location = new System.Drawing.Point(62, 76);
            this.btnMapInfo.Name = "btnMapInfo";
            this.btnMapInfo.Size = new System.Drawing.Size(75, 23);
            this.btnMapInfo.TabIndex = 4;
            this.btnMapInfo.Text = "MapInfo";
            this.btnMapInfo.UseVisualStyleBackColor = true;
            this.btnMapInfo.Click += new System.EventHandler(this.btnMapInfo_Click);
            // 
            // btnDo2
            // 
            this.btnDo2.Location = new System.Drawing.Point(62, 44);
            this.btnDo2.Name = "btnDo2";
            this.btnDo2.Size = new System.Drawing.Size(75, 23);
            this.btnDo2.TabIndex = 3;
            this.btnDo2.Text = "定点数据";
            this.btnDo2.UseVisualStyleBackColor = true;
            this.btnDo2.Click += new System.EventHandler(this.btnDo2_Click);
            // 
            // btnDo
            // 
            this.btnDo.Location = new System.Drawing.Point(62, 12);
            this.btnDo.Name = "btnDo";
            this.btnDo.Size = new System.Drawing.Size(75, 23);
            this.btnDo.TabIndex = 0;
            this.btnDo.Text = "车辆轨迹";
            this.btnDo.UseVisualStyleBackColor = true;
            this.btnDo.Click += new System.EventHandler(this.btnDo_Click);
            // 
            // mapDigitizeGeometriesToolStrip1
            // 
            this.mapDigitizeGeometriesToolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.mapDigitizeGeometriesToolStrip1.Enabled = false;
            this.mapDigitizeGeometriesToolStrip1.Location = new System.Drawing.Point(384, 3);
            this.mapDigitizeGeometriesToolStrip1.MapControl = this.mapBox1;
            this.mapDigitizeGeometriesToolStrip1.Name = "mapDigitizeGeometriesToolStrip1";
            this.mapDigitizeGeometriesToolStrip1.Size = new System.Drawing.Size(110, 25);
            this.mapDigitizeGeometriesToolStrip1.TabIndex = 3;
            this.mapDigitizeGeometriesToolStrip1.Text = "mapDigitizeGeometriesToolStrip1";
            // 
            // mapBox1
            // 
            this.mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.None;
            this.mapBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.mapBox1.CustomTool = null;
            this.mapBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBox1.FineZoomFactor = 10D;
            this.mapBox1.Location = new System.Drawing.Point(0, 0);
            this.mapBox1.MapQueryMode = SharpMap.Forms.MapBox.MapQueryType.LayerByIndex;
            this.mapBox1.Name = "mapBox1";
            this.mapBox1.QueryGrowFactor = 5F;
            this.mapBox1.QueryLayerIndex = 0;
            this.mapBox1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.ShowProgressUpdate = true;
            this.mapBox1.Size = new System.Drawing.Size(617, 562);
            this.mapBox1.TabIndex = 0;
            this.mapBox1.Text = "mapBox1";
            this.mapBox1.WheelZoomMagnitude = -2D;
            // 
            // mapZoomToolStrip1
            // 
            this.mapZoomToolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.mapZoomToolStrip1.Enabled = false;
            this.mapZoomToolStrip1.Location = new System.Drawing.Point(64, 3);
            this.mapZoomToolStrip1.MapControl = this.mapBox1;
            this.mapZoomToolStrip1.Name = "mapZoomToolStrip1";
            this.mapZoomToolStrip1.Size = new System.Drawing.Size(412, 25);
            this.mapZoomToolStrip1.TabIndex = 2;
            this.mapZoomToolStrip1.Text = "mapZoomToolStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnSave});
            this.toolStrip1.Location = new System.Drawing.Point(494, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(64, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnSave
            // 
            this.tsbtnSave.Image = global::DemoSharpMap.Properties.Resources.save;
            this.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSave.Name = "tsbtnSave";
            this.tsbtnSave.Size = new System.Drawing.Size(52, 22);
            this.tsbtnSave.Text = "保存";
            this.tsbtnSave.Click += new System.EventHandler(this.tsbtnSave_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 562);
            this.Controls.Add(this.spCtrl);
            this.Name = "MainForm";
            this.Text = "Demo SharpMap";
            this.spCtrl.Panel1.ResumeLayout(false);
            this.spCtrl.Panel2.ResumeLayout(false);
            this.spCtrl.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spCtrl)).EndInit();
            this.spCtrl.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spCtrl;
        private SharpMap.Forms.MapBox mapBox1;
        private System.Windows.Forms.Button btnDo;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Button btnDo2;
        private SharpMap.Forms.ToolBar.MapZoomToolStrip mapZoomToolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnSave;
        private SharpMap.Forms.ToolBar.MapDigitizeGeometriesToolStrip mapDigitizeGeometriesToolStrip1;
        private System.Windows.Forms.Button btnMapInfo;
        private System.Windows.Forms.Button btnShapFile;
    }
}

