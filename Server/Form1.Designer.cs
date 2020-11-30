
namespace Server
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.radio_usb = new System.Windows.Forms.RadioButton();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.comboBox_ListReport = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textPort = new System.Windows.Forms.TextBox();
            this.textIP = new System.Windows.Forms.TextBox();
            this.radio_Web = new System.Windows.Forms.RadioButton();
            this.radio_Process = new System.Windows.Forms.RadioButton();
            this.radio_PrintDoc = new System.Windows.Forms.RadioButton();
            this.radio_Programs = new System.Windows.Forms.RadioButton();
            this.radio_Apparat = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textTimeEnd = new System.Windows.Forms.TextBox();
            this.textTimeStart = new System.Windows.Forms.TextBox();
            this.ShowInfo = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // radio_usb
            // 
            this.radio_usb.AutoSize = true;
            this.radio_usb.Location = new System.Drawing.Point(42, 188);
            this.radio_usb.Name = "radio_usb";
            this.radio_usb.Size = new System.Drawing.Size(192, 17);
            this.radio_usb.TabIndex = 33;
            this.radio_usb.Text = "О подключаемых USB носителях";
            this.radio_usb.UseVisualStyleBackColor = true;
            this.radio_usb.CheckedChanged += new System.EventHandler(this.radio_usb_CheckedChanged);
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.comboBox_ListReport);
            this.groupBox.Location = new System.Drawing.Point(22, 285);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(283, 52);
            this.groupBox.TabIndex = 32;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Отчёты за день:";
            // 
            // comboBox_ListReport
            // 
            this.comboBox_ListReport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ListReport.FormattingEnabled = true;
            this.comboBox_ListReport.Location = new System.Drawing.Point(11, 19);
            this.comboBox_ListReport.Name = "comboBox_ListReport";
            this.comboBox_ListReport.Size = new System.Drawing.Size(252, 21);
            this.comboBox_ListReport.TabIndex = 0;
            this.comboBox_ListReport.SelectedIndexChanged += new System.EventHandler(this.comboBox_ListReport_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(82, 427);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Порт:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 401);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "IP-адрес сервера:";
            // 
            // textPort
            // 
            this.textPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textPort.Location = new System.Drawing.Point(123, 424);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(182, 20);
            this.textPort.TabIndex = 29;
            // 
            // textIP
            // 
            this.textIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textIP.Location = new System.Drawing.Point(123, 398);
            this.textIP.Name = "textIP";
            this.textIP.Size = new System.Drawing.Size(182, 20);
            this.textIP.TabIndex = 28;
            // 
            // radio_Web
            // 
            this.radio_Web.AutoSize = true;
            this.radio_Web.Location = new System.Drawing.Point(42, 248);
            this.radio_Web.Name = "radio_Web";
            this.radio_Web.Size = new System.Drawing.Size(170, 17);
            this.radio_Web.TabIndex = 27;
            this.radio_Web.Text = "Об открываемых веб-сайтах";
            this.radio_Web.UseVisualStyleBackColor = true;
            this.radio_Web.CheckedChanged += new System.EventHandler(this.radio_Web_CheckedChanged);
            // 
            // radio_Process
            // 
            this.radio_Process.AutoSize = true;
            this.radio_Process.Location = new System.Drawing.Point(42, 228);
            this.radio_Process.Name = "radio_Process";
            this.radio_Process.Size = new System.Drawing.Size(149, 17);
            this.radio_Process.TabIndex = 26;
            this.radio_Process.Text = "О запущенных процесах";
            this.radio_Process.UseVisualStyleBackColor = true;
            this.radio_Process.CheckedChanged += new System.EventHandler(this.radio_Process_CheckedChanged);
            // 
            // radio_PrintDoc
            // 
            this.radio_PrintDoc.AutoSize = true;
            this.radio_PrintDoc.Location = new System.Drawing.Point(42, 207);
            this.radio_PrintDoc.Name = "radio_PrintDoc";
            this.radio_PrintDoc.Size = new System.Drawing.Size(215, 17);
            this.radio_PrintDoc.TabIndex = 25;
            this.radio_PrintDoc.Text = "О доументах, отпраляемых на печать";
            this.radio_PrintDoc.UseVisualStyleBackColor = true;
            this.radio_PrintDoc.CheckedChanged += new System.EventHandler(this.radio_PrintDoc_CheckedChanged);
            // 
            // radio_Programs
            // 
            this.radio_Programs.AutoSize = true;
            this.radio_Programs.Location = new System.Drawing.Point(42, 169);
            this.radio_Programs.Name = "radio_Programs";
            this.radio_Programs.Size = new System.Drawing.Size(133, 17);
            this.radio_Programs.TabIndex = 24;
            this.radio_Programs.Text = "Об установленом ПО";
            this.radio_Programs.UseVisualStyleBackColor = true;
            this.radio_Programs.CheckedChanged += new System.EventHandler(this.radio_Programs_CheckedChanged);
            // 
            // radio_Apparat
            // 
            this.radio_Apparat.AutoSize = true;
            this.radio_Apparat.Checked = true;
            this.radio_Apparat.Location = new System.Drawing.Point(42, 149);
            this.radio_Apparat.Name = "radio_Apparat";
            this.radio_Apparat.Size = new System.Drawing.Size(189, 17);
            this.radio_Apparat.TabIndex = 23;
            this.radio_Apparat.TabStop = true;
            this.radio_Apparat.Text = "Об аппаратном обеспечении ПК";
            this.radio_Apparat.UseVisualStyleBackColor = true;
            this.radio_Apparat.CheckedChanged += new System.EventHandler(this.radio_Apparat_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "до:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "с:";
            // 
            // textTimeEnd
            // 
            this.textTimeEnd.Enabled = false;
            this.textTimeEnd.Location = new System.Drawing.Point(52, 45);
            this.textTimeEnd.Name = "textTimeEnd";
            this.textTimeEnd.Size = new System.Drawing.Size(253, 20);
            this.textTimeEnd.TabIndex = 20;
            // 
            // textTimeStart
            // 
            this.textTimeStart.Enabled = false;
            this.textTimeStart.Location = new System.Drawing.Point(52, 19);
            this.textTimeStart.Name = "textTimeStart";
            this.textTimeStart.Size = new System.Drawing.Size(253, 20);
            this.textTimeStart.TabIndex = 19;
            // 
            // ShowInfo
            // 
            this.ShowInfo.Location = new System.Drawing.Point(22, 73);
            this.ShowInfo.Name = "ShowInfo";
            this.ShowInfo.Size = new System.Drawing.Size(283, 48);
            this.ShowInfo.TabIndex = 18;
            this.ShowInfo.Text = "Получить информацию";
            this.ShowInfo.UseVisualStyleBackColor = true;
            this.ShowInfo.Click += new System.EventHandler(this.ShowInfo_Click);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResult.Location = new System.Drawing.Point(311, 7);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResult.Size = new System.Drawing.Size(471, 437);
            this.textBoxResult.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.radio_usb);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textPort);
            this.Controls.Add(this.textIP);
            this.Controls.Add(this.radio_Web);
            this.Controls.Add(this.radio_Process);
            this.Controls.Add(this.radio_PrintDoc);
            this.Controls.Add(this.radio_Programs);
            this.Controls.Add(this.radio_Apparat);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textTimeEnd);
            this.Controls.Add(this.textTimeStart);
            this.Controls.Add(this.ShowInfo);
            this.Controls.Add(this.textBoxResult);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Центр удалённого мониторинга";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radio_usb;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.ComboBox comboBox_ListReport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textPort;
        private System.Windows.Forms.TextBox textIP;
        private System.Windows.Forms.RadioButton radio_Web;
        private System.Windows.Forms.RadioButton radio_Process;
        private System.Windows.Forms.RadioButton radio_PrintDoc;
        private System.Windows.Forms.RadioButton radio_Programs;
        private System.Windows.Forms.RadioButton radio_Apparat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textTimeEnd;
        private System.Windows.Forms.TextBox textTimeStart;
        private System.Windows.Forms.Button ShowInfo;
        private System.Windows.Forms.TextBox textBoxResult;
    }
}

