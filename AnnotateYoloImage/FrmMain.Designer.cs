namespace AnnotateYoloImage
{
    partial class FrmMain
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbImageList = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnContinueTrain = new System.Windows.Forms.Button();
            this.btnSelYOLOv3Detector = new System.Windows.Forms.Button();
            this.tbYOLOv3Detector = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnTrain = new System.Windows.Forms.Button();
            this.btnGenDarknetDataFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTestRate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSpliteSample = new System.Windows.Forms.Button();
            this.btnCfgMgr = new System.Windows.Forms.Button();
            this.btnAddRegion = new System.Windows.Forms.Button();
            this.btnSetClassNames = new System.Windows.Forms.Button();
            this.btnSaveAnnotate = new System.Windows.Forms.Button();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbImageList);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 97);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 489);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "图像列表";
            // 
            // lbImageList
            // 
            this.lbImageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbImageList.FormattingEnabled = true;
            this.lbImageList.ItemHeight = 12;
            this.lbImageList.Location = new System.Drawing.Point(3, 17);
            this.lbImageList.Name = "lbImageList";
            this.lbImageList.Size = new System.Drawing.Size(251, 469);
            this.lbImageList.TabIndex = 0;
            this.lbImageList.DoubleClick += new System.EventHandler(this.lbImageList_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnContinueTrain);
            this.panel1.Controls.Add(this.btnSelYOLOv3Detector);
            this.panel1.Controls.Add(this.tbYOLOv3Detector);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Controls.Add(this.btnTrain);
            this.panel1.Controls.Add(this.btnGenDarknetDataFile);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbTestRate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnSpliteSample);
            this.panel1.Controls.Add(this.btnCfgMgr);
            this.panel1.Controls.Add(this.btnAddRegion);
            this.panel1.Controls.Add(this.btnSetClassNames);
            this.panel1.Controls.Add(this.btnSaveAnnotate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 97);
            this.panel1.TabIndex = 1;
            // 
            // btnContinueTrain
            // 
            this.btnContinueTrain.Location = new System.Drawing.Point(660, 42);
            this.btnContinueTrain.Name = "btnContinueTrain";
            this.btnContinueTrain.Size = new System.Drawing.Size(75, 23);
            this.btnContinueTrain.TabIndex = 17;
            this.btnContinueTrain.Text = "继续训练";
            this.btnContinueTrain.UseVisualStyleBackColor = true;
            this.btnContinueTrain.Click += new System.EventHandler(this.btnContinueTrain_Click);
            // 
            // btnSelYOLOv3Detector
            // 
            this.btnSelYOLOv3Detector.Location = new System.Drawing.Point(194, 10);
            this.btnSelYOLOv3Detector.Name = "btnSelYOLOv3Detector";
            this.btnSelYOLOv3Detector.Size = new System.Drawing.Size(75, 23);
            this.btnSelYOLOv3Detector.TabIndex = 13;
            this.btnSelYOLOv3Detector.Text = "选择";
            this.btnSelYOLOv3Detector.UseVisualStyleBackColor = true;
            this.btnSelYOLOv3Detector.Click += new System.EventHandler(this.btnSelYOLOv3Detector_Click);
            // 
            // tbYOLOv3Detector
            // 
            this.tbYOLOv3Detector.Location = new System.Drawing.Point(88, 12);
            this.tbYOLOv3Detector.Name = "tbYOLOv3Detector";
            this.tbYOLOv3Detector.ReadOnly = true;
            this.tbYOLOv3Detector.Size = new System.Drawing.Size(100, 21);
            this.tbYOLOv3Detector.TabIndex = 12;
            this.tbYOLOv3Detector.Text = "snowman";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "yolo分类器";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(741, 42);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 10;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(579, 42);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(75, 23);
            this.btnTrain.TabIndex = 9;
            this.btnTrain.Text = "训练";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // btnGenDarknetDataFile
            // 
            this.btnGenDarknetDataFile.Location = new System.Drawing.Point(432, 42);
            this.btnGenDarknetDataFile.Name = "btnGenDarknetDataFile";
            this.btnGenDarknetDataFile.Size = new System.Drawing.Size(141, 23);
            this.btnGenDarknetDataFile.TabIndex = 8;
            this.btnGenDarknetDataFile.Text = "生成Darknetdata文件";
            this.btnGenDarknetDataFile.UseVisualStyleBackColor = true;
            this.btnGenDarknetDataFile.Click += new System.EventHandler(this.btnGenDarknetDataFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(415, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "%";
            // 
            // tbTestRate
            // 
            this.tbTestRate.Location = new System.Drawing.Point(374, 44);
            this.tbTestRate.Name = "tbTestRate";
            this.tbTestRate.Size = new System.Drawing.Size(35, 21);
            this.tbTestRate.TabIndex = 6;
            this.tbTestRate.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(303, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "测试集比例";
            // 
            // btnSpliteSample
            // 
            this.btnSpliteSample.Location = new System.Drawing.Point(194, 42);
            this.btnSpliteSample.Name = "btnSpliteSample";
            this.btnSpliteSample.Size = new System.Drawing.Size(100, 23);
            this.btnSpliteSample.TabIndex = 4;
            this.btnSpliteSample.Text = "切分样本";
            this.btnSpliteSample.UseVisualStyleBackColor = true;
            this.btnSpliteSample.Click += new System.EventHandler(this.btnSpliteSample_Click);
            // 
            // btnCfgMgr
            // 
            this.btnCfgMgr.Location = new System.Drawing.Point(88, 42);
            this.btnCfgMgr.Name = "btnCfgMgr";
            this.btnCfgMgr.Size = new System.Drawing.Size(100, 23);
            this.btnCfgMgr.TabIndex = 3;
            this.btnCfgMgr.Text = "生成配置文件";
            this.btnCfgMgr.UseVisualStyleBackColor = true;
            this.btnCfgMgr.Click += new System.EventHandler(this.btnCfgMgr_Click);
            // 
            // btnAddRegion
            // 
            this.btnAddRegion.Location = new System.Drawing.Point(351, 71);
            this.btnAddRegion.Name = "btnAddRegion";
            this.btnAddRegion.Size = new System.Drawing.Size(75, 23);
            this.btnAddRegion.TabIndex = 2;
            this.btnAddRegion.Text = "插入边界框";
            this.btnAddRegion.UseVisualStyleBackColor = true;
            this.btnAddRegion.Click += new System.EventHandler(this.btnAddRegion_Click);
            // 
            // btnSetClassNames
            // 
            this.btnSetClassNames.Location = new System.Drawing.Point(7, 42);
            this.btnSetClassNames.Name = "btnSetClassNames";
            this.btnSetClassNames.Size = new System.Drawing.Size(75, 23);
            this.btnSetClassNames.TabIndex = 1;
            this.btnSetClassNames.Text = "类别管理";
            this.btnSetClassNames.UseVisualStyleBackColor = true;
            this.btnSetClassNames.Click += new System.EventHandler(this.btnSetClassNames_Click);
            // 
            // btnSaveAnnotate
            // 
            this.btnSaveAnnotate.Location = new System.Drawing.Point(432, 71);
            this.btnSaveAnnotate.Name = "btnSaveAnnotate";
            this.btnSaveAnnotate.Size = new System.Drawing.Size(75, 23);
            this.btnSaveAnnotate.TabIndex = 0;
            this.btnSaveAnnotate.Text = "保存";
            this.btnSaveAnnotate.UseVisualStyleBackColor = true;
            this.btnSaveAnnotate.Click += new System.EventHandler(this.btnSaveAnnotate_Click);
            // 
            // pnlImage
            // 
            this.pnlImage.AutoScroll = true;
            this.pnlImage.Controls.Add(this.pbImage);
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImage.Location = new System.Drawing.Point(262, 97);
            this.pnlImage.Margin = new System.Windows.Forms.Padding(0);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(722, 489);
            this.pnlImage.TabIndex = 2;
            // 
            // pbImage
            // 
            this.pbImage.Location = new System.Drawing.Point(1, 2);
            this.pbImage.Margin = new System.Windows.Forms.Padding(0);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(300, 300);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            this.pbImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbImage_MouseDown);
            this.pbImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbImage_MouseUp);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(257, 97);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 489);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // tbLog
            // 
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbLog.Location = new System.Drawing.Point(0, 591);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(984, 170);
            this.tbLog.TabIndex = 4;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 586);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(984, 5);
            this.splitter2.TabIndex = 5;
            this.splitter2.TabStop = false;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.pnlImage);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.tbLog);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1000, 800);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "yolo图像标注工具";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMain_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlImage.ResumeLayout(false);
            this.pnlImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lbImageList;
        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.Button btnSaveAnnotate;
        private System.Windows.Forms.Button btnSetClassNames;
        private System.Windows.Forms.Button btnAddRegion;
        private System.Windows.Forms.Button btnCfgMgr;
        private System.Windows.Forms.Button btnSpliteSample;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTestRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGenDarknetDataFile;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox tbYOLOv3Detector;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelYOLOv3Detector;
        private System.Windows.Forms.Button btnContinueTrain;
    }
}

