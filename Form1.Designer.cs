namespace Klimenko_mario1
{
    partial class Form1
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
            label1 = new Label();
            txtName = new TextBox();
            label2 = new Label();
            cmbDifficulty = new ComboBox();
            btnPlay = new Button();
            btnExit = new Button();
            btnRules = new Button();
            btnRecords = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(293, 33);
            label1.Name = "label1";
            label1.Size = new Size(78, 15);
            label1.TabIndex = 0;
            label1.Text = "Введите имя:";
            // 
            // txtName
            // 
            txtName.Location = new Point(293, 51);
            txtName.Name = "txtName";
            txtName.Size = new Size(124, 23);
            txtName.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(294, 97);
            label2.Name = "label2";
            label2.Size = new Size(72, 15);
            label2.TabIndex = 2;
            label2.Text = "Сложность:";
            // 
            // cmbDifficulty
            // 
            cmbDifficulty.FormattingEnabled = true;
            cmbDifficulty.Items.AddRange(new object[] { "Легко", "Нормально", "Сложно" });
            cmbDifficulty.Location = new Point(294, 115);
            cmbDifficulty.Name = "cmbDifficulty";
            cmbDifficulty.Size = new Size(167, 23);
            cmbDifficulty.TabIndex = 3;
            // 
            // btnPlay
            // 
            btnPlay.Location = new Point(342, 182);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(75, 23);
            btnPlay.TabIndex = 4;
            btnPlay.Text = "Играть";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(342, 275);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(75, 23);
            btnExit.TabIndex = 5;
            btnExit.Text = "Выход";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // btnRules
            // 
            btnRules.Location = new Point(342, 211);
            btnRules.Name = "btnRules";
            btnRules.Size = new Size(75, 24);
            btnRules.TabIndex = 6;
            btnRules.Text = "Правила";
            btnRules.UseVisualStyleBackColor = true;
            btnRules.Click += btnRules_Click;
            // 
            // btnRecords
            // 
            btnRecords.Location = new Point(342, 246);
            btnRecords.Name = "btnRecords";
            btnRecords.Size = new Size(75, 23);
            btnRecords.TabIndex = 7;
            btnRecords.Text = "Рекорды";
            btnRecords.UseVisualStyleBackColor = true;
            btnRecords.Click += btnRecords_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnRecords);
            Controls.Add(btnRules);
            Controls.Add(btnExit);
            Controls.Add(btnPlay);
            Controls.Add(cmbDifficulty);
            Controls.Add(label2);
            Controls.Add(txtName);
            Controls.Add(label1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Платформер";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtName;
        private Label label2;
        private ComboBox cmbDifficulty;
        private Button btnPlay;
        private Button btnExit;
        private Button btnRules;
        private Button btnRecords;
    }
}
