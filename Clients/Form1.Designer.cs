namespace Clients
{
    partial class Frm
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
            this.txtText = new System.Windows.Forms.TextBox();
            this.btnSendNoise = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.txtHashNoise = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.lblEncrypt = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.btnSendKey = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(72, 312);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(716, 20);
            this.txtText.TabIndex = 7;
            // 
            // btnSendNoise
            // 
            this.btnSendNoise.Location = new System.Drawing.Point(412, 382);
            this.btnSendNoise.Name = "btnSendNoise";
            this.btnSendNoise.Size = new System.Drawing.Size(75, 23);
            this.btnSendNoise.TabIndex = 6;
            this.btnSendNoise.Text = "Send Noise";
            this.btnSendNoise.UseVisualStyleBackColor = true;
            this.btnSendNoise.Click += new System.EventHandler(this.btnSendNoir_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(314, 382);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.ReadOnly = true;
            this.richTextBoxMessage.Size = new System.Drawing.Size(776, 218);
            this.richTextBoxMessage.TabIndex = 4;
            this.richTextBoxMessage.Text = "";
            // 
            // txtHashNoise
            // 
            this.txtHashNoise.Location = new System.Drawing.Point(72, 243);
            this.txtHashNoise.Name = "txtHashNoise";
            this.txtHashNoise.ReadOnly = true;
            this.txtHashNoise.Size = new System.Drawing.Size(716, 20);
            this.txtHashNoise.TabIndex = 8;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(12, 315);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(53, 13);
            this.lblText.TabIndex = 10;
            this.lblText.Text = "Message:";
            // 
            // lblEncrypt
            // 
            this.lblEncrypt.AutoSize = true;
            this.lblEncrypt.Location = new System.Drawing.Point(12, 246);
            this.lblEncrypt.Name = "lblEncrypt";
            this.lblEncrypt.Size = new System.Drawing.Size(46, 13);
            this.lblEncrypt.TabIndex = 9;
            this.lblEncrypt.Text = "Encrypt:";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(607, 384);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(100, 20);
            this.txtKey.TabIndex = 12;
            // 
            // btnSendKey
            // 
            this.btnSendKey.Location = new System.Drawing.Point(713, 382);
            this.btnSendKey.Name = "btnSendKey";
            this.btnSendKey.Size = new System.Drawing.Size(75, 23);
            this.btnSendKey.TabIndex = 11;
            this.btnSendKey.Text = "Send Key";
            this.btnSendKey.UseVisualStyleBackColor = true;
            this.btnSendKey.Click += new System.EventHandler(this.btnSendKey_Click);
            // 
            // Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 419);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.btnSendKey);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.lblEncrypt);
            this.Controls.Add(this.txtHashNoise);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.btnSendNoise);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.richTextBoxMessage);
            this.Name = "Frm";
            this.Text = "Clients";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Frm_FormClosed);
            this.Load += new System.EventHandler(this.Frm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.Button btnSendNoise;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox richTextBoxMessage;
        private System.Windows.Forms.TextBox txtHashNoise;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Label lblEncrypt;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Button btnSendKey;
    }
}

