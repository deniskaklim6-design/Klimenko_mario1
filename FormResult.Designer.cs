namespace Klimenko_mario1
{
    partial class FormResult
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
            lblTitle = new Label();
            lblInfo = new Label();
            lstRecords = new ListBox();
            btnAgain = new Button();
            btnMenu = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(288, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(187, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Победа/Поражение";
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(59, 57);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(50, 15);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "Игрок...";
            // 
            // lstRecords
            // 
            lstRecords.FormattingEnabled = true;
            lstRecords.ItemHeight = 15;
            lstRecords.Location = new Point(60, 97);
            lstRecords.Name = "lstRecords";
            lstRecords.Size = new Size(120, 94);
            lstRecords.TabIndex = 2;
            // 
            // btnAgain
            // 
            btnAgain.DialogResult = DialogResult.Retry;
            btnAgain.Location = new Point(88, 234);
            btnAgain.Name = "btnAgain";
            btnAgain.Size = new Size(107, 23);
            btnAgain.TabIndex = 3;
            btnAgain.Text = "Играть заново";
            btnAgain.UseVisualStyleBackColor = true;
            // 
            // btnMenu
            // 
            btnMenu.DialogResult = DialogResult.OK;
            btnMenu.Location = new Point(88, 263);
            btnMenu.Name = "btnMenu";
            btnMenu.Size = new Size(107, 23);
            btnMenu.TabIndex = 4;
            btnMenu.Text = "В главное меню";
            btnMenu.UseVisualStyleBackColor = true;
            // 
            // FormResult
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnMenu);
            Controls.Add(btnAgain);
            Controls.Add(lstRecords);
            Controls.Add(lblInfo);
            Controls.Add(lblTitle);
            Name = "FormResult";
            Text = "FormResult";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnAgain;
        private Button btnMenu;
        public Label lblTitle;
        public Label lblInfo;
        public ListBox lstRecords;
    }
}