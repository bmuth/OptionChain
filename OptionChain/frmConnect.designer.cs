namespace OptionChain
{
    partial class frmConnect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tbClientId = new System.Windows.Forms.TextBox();
            this.rbIBGateway = new System.Windows.Forms.RadioButton();
            this.rbTWS = new System.Windows.Forms.RadioButton();
            this.btnConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Client Id";
            // 
            // tbClientId
            // 
            this.tbClientId.Location = new System.Drawing.Point(159, 63);
            this.tbClientId.Name = "tbClientId";
            this.tbClientId.Size = new System.Drawing.Size(34, 20);
            this.tbClientId.TabIndex = 11;
            this.tbClientId.Text = "5";
            // 
            // rbIBGateway
            // 
            this.rbIBGateway.Checked = true;
            this.rbIBGateway.Location = new System.Drawing.Point(77, 40);
            this.rbIBGateway.Name = "rbIBGateway";
            this.rbIBGateway.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbIBGateway.Size = new System.Drawing.Size(116, 17);
            this.rbIBGateway.TabIndex = 10;
            this.rbIBGateway.TabStop = true;
            this.rbIBGateway.Text = "IB Gateway";
            this.rbIBGateway.UseVisualStyleBackColor = true;
            // 
            // rbTWS
            // 
            this.rbTWS.AutoSize = true;
            this.rbTWS.Location = new System.Drawing.Point(77, 17);
            this.rbTWS.Name = "rbTWS";
            this.rbTWS.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbTWS.Size = new System.Drawing.Size(116, 17);
            this.rbTWS.TabIndex = 9;
            this.rbTWS.Text = "Trader Workstation";
            this.rbTWS.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConnect.Location = new System.Drawing.Point(111, 105);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 13;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // frmConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 148);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbClientId);
            this.Controls.Add(this.rbIBGateway);
            this.Controls.Add(this.rbTWS);
            this.Name = "frmConnect";
            this.Text = "Connect";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConnect_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbClientId;
        private System.Windows.Forms.RadioButton rbIBGateway;
        private System.Windows.Forms.RadioButton rbTWS;
        private System.Windows.Forms.Button btnConnect;
    }
}