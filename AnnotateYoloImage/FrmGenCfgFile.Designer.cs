namespace AnnotateYoloImage
{
    partial class FrmGenCfgFile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGenCfgFile = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnGenCfgFile
            // 
            this.btnGenCfgFile.Location = new System.Drawing.Point(12, 12);
            this.btnGenCfgFile.Name = "btnGenCfgFile";
            this.btnGenCfgFile.Size = new System.Drawing.Size(99, 23);
            this.btnGenCfgFile.TabIndex = 0;
            this.btnGenCfgFile.Text = "生成配置文件";
            this.btnGenCfgFile.UseVisualStyleBackColor = true;
            this.btnGenCfgFile.Click += new System.EventHandler(this.btnGenCfgFile_Click);
            // 
            // tbLog
            // 
            this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLog.Location = new System.Drawing.Point(12, 52);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(776, 386);
            this.tbLog.TabIndex = 1;
            // 
            // FrmGenCfgFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.btnGenCfgFile);
            this.Name = "FrmGenCfgFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成cfg配置文件";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenCfgFile;
        private System.Windows.Forms.TextBox tbLog;
    }
}