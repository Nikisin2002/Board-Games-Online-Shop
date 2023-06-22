
namespace Приложение_для_курсовой
{
    partial class Registration
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
            this.RegLogin = new System.Windows.Forms.TextBox();
            this.RegPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RegSurname = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.RegName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.RegThirdName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.RegBirthDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.RegClientComplete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RegLogin
            // 
            this.RegLogin.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RegLogin.Location = new System.Drawing.Point(239, 52);
            this.RegLogin.Name = "RegLogin";
            this.RegLogin.Size = new System.Drawing.Size(157, 26);
            this.RegLogin.TabIndex = 0;
            this.RegLogin.TextChanged += new System.EventHandler(this.RegLogin_TextChanged);
            // 
            // RegPassword
            // 
            this.RegPassword.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RegPassword.Location = new System.Drawing.Point(239, 106);
            this.RegPassword.Name = "RegPassword";
            this.RegPassword.Size = new System.Drawing.Size(157, 26);
            this.RegPassword.TabIndex = 0;
            this.RegPassword.TextChanged += new System.EventHandler(this.RegPassword_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(235, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Логин";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(235, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Пароль";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // RegSurname
            // 
            this.RegSurname.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RegSurname.Location = new System.Drawing.Point(55, 52);
            this.RegSurname.Name = "RegSurname";
            this.RegSurname.Size = new System.Drawing.Size(157, 26);
            this.RegSurname.TabIndex = 0;
            this.RegSurname.TextChanged += new System.EventHandler(this.RegSurname_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(51, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "Фамилия";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // RegName
            // 
            this.RegName.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RegName.Location = new System.Drawing.Point(55, 106);
            this.RegName.Name = "RegName";
            this.RegName.Size = new System.Drawing.Size(157, 26);
            this.RegName.TabIndex = 0;
            this.RegName.TextChanged += new System.EventHandler(this.RegName_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(51, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 19);
            this.label4.TabIndex = 1;
            this.label4.Text = "Имя";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // RegThirdName
            // 
            this.RegThirdName.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RegThirdName.Location = new System.Drawing.Point(55, 162);
            this.RegThirdName.Name = "RegThirdName";
            this.RegThirdName.Size = new System.Drawing.Size(157, 26);
            this.RegThirdName.TabIndex = 0;
            this.RegThirdName.TextChanged += new System.EventHandler(this.RegThirdName_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(51, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 19);
            this.label5.TabIndex = 1;
            this.label5.Text = "Отчество";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // RegBirthDate
            // 
            this.RegBirthDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RegBirthDate.CalendarFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RegBirthDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.RegBirthDate.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RegBirthDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.RegBirthDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RegBirthDate.Location = new System.Drawing.Point(235, 162);
            this.RegBirthDate.Name = "RegBirthDate";
            this.RegBirthDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RegBirthDate.Size = new System.Drawing.Size(161, 26);
            this.RegBirthDate.TabIndex = 2;
            this.RegBirthDate.ValueChanged += new System.EventHandler(this.BirthDate_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(235, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 19);
            this.label6.TabIndex = 1;
            this.label6.Text = "Дата рождения";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // RegClientComplete
            // 
            this.RegClientComplete.BackColor = System.Drawing.Color.Lime;
            this.RegClientComplete.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RegClientComplete.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.RegClientComplete.Location = new System.Drawing.Point(131, 205);
            this.RegClientComplete.Name = "RegClientComplete";
            this.RegClientComplete.Size = new System.Drawing.Size(198, 39);
            this.RegClientComplete.TabIndex = 3;
            this.RegClientComplete.Text = "Зарегистрироваться";
            this.RegClientComplete.UseVisualStyleBackColor = false;
            this.RegClientComplete.Click += new System.EventHandler(this.RegClientComplete_Click);
            // 
            // Registration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 267);
            this.Controls.Add(this.RegClientComplete);
            this.Controls.Add(this.RegBirthDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RegThirdName);
            this.Controls.Add(this.RegName);
            this.Controls.Add(this.RegSurname);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RegPassword);
            this.Controls.Add(this.RegLogin);
            this.Name = "Registration";
            this.Text = "Окно регистрации";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Registration_FormClosed);
            this.Load += new System.EventHandler(this.Registration_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox RegLogin;
        private System.Windows.Forms.TextBox RegPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox RegSurname;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox RegName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox RegThirdName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker RegBirthDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button RegClientComplete;
    }
}