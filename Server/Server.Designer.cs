namespace Client2
{
    partial class Server
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
            this.txtMess = new System.Windows.Forms.TextBox();
            this.rtbShowMessage = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lvwUserStatus = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // txtMess
            // 
            this.txtMess.Location = new System.Drawing.Point(12, 252);
            this.txtMess.Multiline = true;
            this.txtMess.Name = "txtMess";
            this.txtMess.Size = new System.Drawing.Size(299, 57);
            this.txtMess.TabIndex = 8;
            // 
            // rtbShowMessage
            // 
            this.rtbShowMessage.Location = new System.Drawing.Point(12, 12);
            this.rtbShowMessage.Name = "rtbShowMessage";
            this.rtbShowMessage.Size = new System.Drawing.Size(380, 213);
            this.rtbShowMessage.TabIndex = 7;
            this.rtbShowMessage.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(317, 252);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 57);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lvwUserStatus
            // 
            this.lvwUserStatus.FullRowSelect = true;
            this.lvwUserStatus.GridLines = true;
            this.lvwUserStatus.Location = new System.Drawing.Point(399, 12);
            this.lvwUserStatus.Name = "lvwUserStatus";
            this.lvwUserStatus.Size = new System.Drawing.Size(190, 297);
            this.lvwUserStatus.TabIndex = 9;
            this.lvwUserStatus.UseCompatibleStateImageBehavior = false;
            this.lvwUserStatus.View = System.Windows.Forms.View.Details;
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 330);
            this.Controls.Add(this.lvwUserStatus);
            this.Controls.Add(this.txtMess);
            this.Controls.Add(this.rtbShowMessage);
            this.Controls.Add(this.btnSend);
            this.Name = "Server";
            this.Text = "Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Server_FormClosing);
            this.Load += new System.EventHandler(this.Server_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMess;
        private System.Windows.Forms.RichTextBox rtbShowMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ListView lvwUserStatus;
    }
}

