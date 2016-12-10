namespace AsteriskDialerWindowsUser
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.lblInfo = new System.Windows.Forms.Label();
            this.txtPhoneToDial = new System.Windows.Forms.TextBox();
            this.btnDial = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(12, 42);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(35, 13);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "label1";
            // 
            // txtPhoneToDial
            // 
            this.txtPhoneToDial.Location = new System.Drawing.Point(12, 9);
            this.txtPhoneToDial.Name = "txtPhoneToDial";
            this.txtPhoneToDial.Size = new System.Drawing.Size(100, 20);
            this.txtPhoneToDial.TabIndex = 1;
            // 
            // btnDial
            // 
            this.btnDial.Location = new System.Drawing.Point(118, 9);
            this.btnDial.Name = "btnDial";
            this.btnDial.Size = new System.Drawing.Size(75, 23);
            this.btnDial.TabIndex = 2;
            this.btnDial.Text = "Dial";
            this.btnDial.UseVisualStyleBackColor = true;
            this.btnDial.Click += new System.EventHandler(this.btnDial_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 72);
            this.Controls.Add(this.btnDial);
            this.Controls.Add(this.txtPhoneToDial);
            this.Controls.Add(this.lblInfo);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txtPhoneToDial;
        private System.Windows.Forms.Button btnDial;
        private System.Windows.Forms.Timer timer1;
    }
}

