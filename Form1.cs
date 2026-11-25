using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Klimenko_mario1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите имя игрока!", "Ошибка");
                return;
            }
            if (cmbDifficulty.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите сложность!", "Ошибка");
                return;
            }

            // Создаем игру и передаем данные
            FormGame gameForm = new FormGame(txtName.Text, cmbDifficulty.Text);
            this.Hide(); // Прячем меню
            gameForm.Show(); // Показываем игру
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRules_Click(object sender, EventArgs e)
        {
            FormRules rules = new FormRules();
            rules.ShowDialog();
        }
            private void btnRecords_Click(object sender, EventArgs e)
            {
                // Пока оставим заглушку, чтобы проверить работу кнопок
                MessageBox.Show("Таблица рекордов скоро появится!");
            }
        }
    }
