using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Klimenko_mario1
{
    public partial class FormResult : Form
    {
        // Статический список для хранения рекордов
        public static List<string> Records = new List<string>();

        public FormResult(string name, int score, bool isWin)
        {
            InitializeComponent();

            // Настраиваем заголовок и текст
            if (isWin)
            {
                lblTitle.Text = "ПОБЕДА!";
                lblTitle.ForeColor = Color.Green;
            }
            else
            {
                lblTitle.Text = "ИГРА ОКОНЧЕНА";
                lblTitle.ForeColor = Color.Red;
            }

            lblInfo.Text = $"Игрок: {name}\nНабрано очков: {score}";

            // Если это не пустой вызов, сохраняем результат
            if (name != "")
            {
                string recordString = $"{name} - {score} очков";
                Records.Add(recordString);
            }

            // Выводим все рекорды в список
            lstRecords.Items.Clear();
            foreach (var record in Records)
            {
                lstRecords.Items.Add(record);
            }
        }
    }
}