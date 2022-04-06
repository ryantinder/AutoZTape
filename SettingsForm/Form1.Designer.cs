
using System.Drawing;

namespace SettingsForm
{
    partial class SettingsForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StoreId_textBox = new System.Windows.Forms.TextBox();
            this.StoreId_panel = new System.Windows.Forms.Panel();
            this.StoreID_label = new System.Windows.Forms.Label();
            this.store_panel = new System.Windows.Forms.Panel();
            this.store_label = new System.Windows.Forms.Label();
            this.Store_textBox = new System.Windows.Forms.TextBox();
            this.targetPicker = new System.Windows.Forms.ComboBox();
            this.targetPicker_label = new System.Windows.Forms.Label();
            this.disableLivePush = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.disableMobileAPI = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.disableProgram = new System.Windows.Forms.CheckBox();
            this.disableAutoUpdate = new System.Windows.Forms.CheckBox();
            this.checkConnections = new System.Windows.Forms.CheckBox();
            this.StoreId_panel.SuspendLayout();
            this.store_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // StoreId_textBox
            // 
            this.StoreId_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.StoreId_textBox.Location = new System.Drawing.Point(109, 4);
            this.StoreId_textBox.MaxLength = 3;
            this.StoreId_textBox.Name = "StoreId_textBox";
            this.StoreId_textBox.Size = new System.Drawing.Size(114, 32);
            this.StoreId_textBox.TabIndex = 0;
            this.StoreId_textBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.StoreId_textBox.Leave += new System.EventHandler(this.StoreID_TextChanged);
            // 
            // StoreId_panel
            // 
            this.StoreId_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StoreId_panel.Controls.Add(this.StoreId_textBox);
            this.StoreId_panel.Location = new System.Drawing.Point(70, 76);
            this.StoreId_panel.Name = "StoreId_panel";
            this.StoreId_panel.Size = new System.Drawing.Size(239, 40);
            this.StoreId_panel.TabIndex = 2;
            // 
            // StoreID_label
            // 
            this.StoreID_label.AutoSize = true;
            this.StoreID_label.Location = new System.Drawing.Point(75, 81);
            this.StoreID_label.Name = "StoreID_label";
            this.StoreID_label.Size = new System.Drawing.Size(99, 32);
            this.StoreID_label.TabIndex = 1;
            this.StoreID_label.Text = "Store ID";
            // 
            // store_panel
            // 
            this.store_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.store_panel.Controls.Add(this.store_label);
            this.store_panel.Controls.Add(this.Store_textBox);
            this.store_panel.Location = new System.Drawing.Point(347, 76);
            this.store_panel.Name = "store_panel";
            this.store_panel.Size = new System.Drawing.Size(239, 40);
            this.store_panel.TabIndex = 5;
            // 
            // store_label
            // 
            this.store_label.AutoSize = true;
            this.store_label.Location = new System.Drawing.Point(4, 4);
            this.store_label.Name = "store_label";
            this.store_label.Size = new System.Drawing.Size(167, 32);
            this.store_label.TabIndex = 6;
            this.store_label.Text = "Store            #";
            // 
            // Store_textBox
            // 
            this.Store_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Store_textBox.Location = new System.Drawing.Point(177, 4);
            this.Store_textBox.MaxLength = 4;
            this.Store_textBox.Name = "Store_textBox";
            this.Store_textBox.Size = new System.Drawing.Size(47, 32);
            this.Store_textBox.TabIndex = 1;
            this.Store_textBox.Leave += new System.EventHandler(this.Store_TextChanged);
            // 
            // targetPicker
            // 
            this.targetPicker.FormattingEnabled = true;
            this.targetPicker.Items.AddRange(new object[] {
            "Test Database",
            "Dev Server",
            "Live Server"});
            this.targetPicker.Location = new System.Drawing.Point(347, 147);
            this.targetPicker.Name = "targetPicker";
            this.targetPicker.Size = new System.Drawing.Size(239, 40);
            this.targetPicker.TabIndex = 6;
            this.targetPicker.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // targetPicker_label
            // 
            this.targetPicker_label.AutoSize = true;
            this.targetPicker_label.Location = new System.Drawing.Point(70, 151);
            this.targetPicker_label.Name = "targetPicker_label";
            this.targetPicker_label.Size = new System.Drawing.Size(176, 32);
            this.targetPicker_label.TabIndex = 7;
            this.targetPicker_label.Text = "SQL Pointed To";
            // 
            // disableLivePush
            // 
            this.disableLivePush.AutoSize = true;
            this.disableLivePush.Location = new System.Drawing.Point(75, 288);
            this.disableLivePush.Name = "disableLivePush";
            this.disableLivePush.Size = new System.Drawing.Size(260, 36);
            this.disableLivePush.TabIndex = 8;
            this.disableLivePush.Text = "Disable Push to Live";
            this.disableLivePush.UseVisualStyleBackColor = true;
            this.disableLivePush.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(75, 358);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(361, 36);
            this.checkBox2.TabIndex = 9;
            this.checkBox2.Text = "Hold Terminal after Execution";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "  MM-dd-yyyy";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(347, 421);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(239, 39);
            this.dateTimePicker1.TabIndex = 10;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(75, 425);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(195, 36);
            this.checkBox3.TabIndex = 11;
            this.checkBox3.Text = "Override Date";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // disableMobileAPI
            // 
            this.disableMobileAPI.AutoSize = true;
            this.disableMobileAPI.Location = new System.Drawing.Point(347, 218);
            this.disableMobileAPI.Name = "disableMobileAPI";
            this.disableMobileAPI.Size = new System.Drawing.Size(247, 36);
            this.disableMobileAPI.TabIndex = 12;
            this.disableMobileAPI.Text = "Disable Mobile API";
            this.disableMobileAPI.UseVisualStyleBackColor = true;
            this.disableMobileAPI.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(70, 562);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(383, 32);
            this.label3.TabIndex = 13;
            this.label3.Text = "Generally, these are all unchecked.";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(75, 625);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 46);
            this.button1.TabIndex = 14;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.Leave += new System.EventHandler(this.button1_Release);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(435, 625);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(148, 46);
            this.button2.TabIndex = 15;
            this.button2.Text = "Advanced";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(437, 700);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(232, 32);
            this.label4.TabIndex = 16;
            this.label4.Text = "Configuration Saved";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // disableProgram
            // 
            this.disableProgram.AutoSize = true;
            this.disableProgram.Location = new System.Drawing.Point(75, 218);
            this.disableProgram.Margin = new System.Windows.Forms.Padding(4);
            this.disableProgram.Name = "disableProgram";
            this.disableProgram.Size = new System.Drawing.Size(222, 36);
            this.disableProgram.TabIndex = 17;
            this.disableProgram.Text = "Disable program";
            this.disableProgram.UseVisualStyleBackColor = true;
            this.disableProgram.CheckedChanged += new System.EventHandler(this.disableProgram_CheckedChanged);
            // 
            // disableAutoUpdate
            // 
            this.disableAutoUpdate.AutoSize = true;
            this.disableAutoUpdate.Location = new System.Drawing.Point(347, 288);
            this.disableAutoUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.disableAutoUpdate.Name = "disableAutoUpdate";
            this.disableAutoUpdate.Size = new System.Drawing.Size(266, 36);
            this.disableAutoUpdate.TabIndex = 18;
            this.disableAutoUpdate.Text = "Disable Auto Update";
            this.disableAutoUpdate.UseVisualStyleBackColor = true;
            this.disableAutoUpdate.CheckedChanged += new System.EventHandler(this.disableAutoUpdate_CheckedChanged);
            // 
            // checkConnections
            // 
            this.checkConnections.AutoSize = true;
            this.checkConnections.Location = new System.Drawing.Point(75, 488);
            this.checkConnections.Name = "checkConnections";
            this.checkConnections.Size = new System.Drawing.Size(326, 36);
            this.checkConnections.TabIndex = 19;
            this.checkConnections.Text = "Test SQL connections only";
            this.checkConnections.UseVisualStyleBackColor = true;
            this.checkConnections.CheckedChanged += new System.EventHandler(this.checkConnections_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(679, 744);
            this.Controls.Add(this.checkConnections);
            this.Controls.Add(this.disableAutoUpdate);
            this.Controls.Add(this.disableProgram);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.disableMobileAPI);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.disableLivePush);
            this.Controls.Add(this.targetPicker_label);
            this.Controls.Add(this.targetPicker);
            this.Controls.Add(this.store_panel);
            this.Controls.Add(this.StoreID_label);
            this.Controls.Add(this.StoreId_panel);
            this.Name = "SettingsForm";
            this.Text = "AutoZTape Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.StoreId_panel.ResumeLayout(false);
            this.StoreId_panel.PerformLayout();
            this.store_panel.ResumeLayout(false);
            this.store_panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox StoreId_textBox;
        private System.Windows.Forms.Panel StoreId_panel;
        private System.Windows.Forms.Label StoreID_label;
        private System.Windows.Forms.Panel store_panel;
        private System.Windows.Forms.Label store_label;
        private System.Windows.Forms.TextBox Store_textBox;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ComboBox targetPicker;
        private System.Windows.Forms.Label targetPicker_label;
        private System.Windows.Forms.CheckBox disableLivePush;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox disableMobileAPI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox disableProgram;
        private System.Windows.Forms.CheckBox disableAutoUpdate;
        private System.Windows.Forms.CheckBox checkConnections;
    }
}

