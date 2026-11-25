using System;                       //Подключение базовых классов (Console, Exception, Math и т.д.)
using System.Collections.Generic;   //Подключение коллекций (List, Dictionary)
using System.Drawing;               //Подключение графики (Rectangle, Color, Brushes, Graphics)
using System.Windows.Forms;         //Подключение элементов WinForms (Form, Timer, MessageBox)

namespace Klimenko_mario1           //Пространство имен проекта (должно совпадать с остальными файлами)
{
    //Класс игровой формы, наследуется от стандартной формы Windows (Form)
    public partial class FormGame : Form
    {
        //ПЕРЕМЕННЫЕ КЛАССА

        private string playerName;      //Имя игрока, полученное из меню
        private string difficulty;      //Уровень сложности, полученный из меню

        //Таймер
        //Используем полное имя System.Windows.Forms.Timer, чтобы компилятор не путал его с таймером из System.Threading
        private System.Windows.Forms.Timer gameTimer;

        //Физика квадрата
        private float playerX = 100;    //Координата X лево/право. Float для плавного движения
        private float playerY = 100;    //Координата Y верх/низ
        private float velX = 0;         //Скорость по горизонтали
        private float velY = 0;         //Скорость по вертикали (для гравитации и прыжков)

        //Константы игры не меняются во время игры
        private const float MoveSpeed = 5f;   //Скорость ходьбы (5 пикселей за кадр)
        private const float Gravity = 0.5f;   //Сила притяжения (каждый кадр добавляет скорость вниз)
        private const float JumpPower = -10f; //Сила прыжка (минус, так как ось Y направлена вниз)
        private int PlayerSize = 30;          //Размер квадратика Марио (30х30 пикселей)

        //Флаги нажатия клавиш
        //Нужны, чтобы игра понимала, какая клавиша зажата прямо сейчас
        private bool isLeftPressed = false;
        private bool isRightPressed = false;
        private bool isUpPressed = false;
        private bool isDownPressed = false;
        private bool canJump = false;         //Можем ли прыгнуть (стоим ли на земле)
        private bool isOnLadder = false;      //Находимся ли на лестнице

        //Игровые объекты
        //List<T> используется для хранения множества однотипных объектов
        private List<Rectangle> platforms = new List<Rectangle>(); //Список платформ
        private List<Rectangle> coins = new List<Rectangle>();     //Список монеток
        private List<Rectangle> enemies = new List<Rectangle>();   //Список врагов
        private List<float> enemySpeeds = new List<float>();       //Скорость каждого врага (для патрулирования)
        private List<Rectangle> ladders = new List<Rectangle>();   //Список лестниц
        private Rectangle finishDoor;                              //Финишная дверь (одна, поэтому не список)
        private int score = 0;                                     //Текущие очки

        //Жизни и неуязвимость
        private int lives = 3;                //Количество жизней
        private bool isInvulnerable = false;  //Флаг: неуязвим ли Марио сейчас?
        private int invulnerableTimer = 0;    //Таймер неуязвимости (в кадрах)

        //КОНСТРУКТОР ФОРМЫ
        //Принимает имя и сложность из главного меню
        public FormGame(string name, string diff)
        {
            InitializeComponent(); //Стандартный метод, создающий элементы формы (из дизайнера)

            //Сохраняем переданные данные в поля класса
            playerName = name;
            difficulty = diff;

            //Настраиваем окно
            this.Text = $"Игра - {playerName} ({difficulty})"; //Имя игрока в заголовке
            this.BackColor = Color.SkyBlue; //Голубое небо
            this.DoubleBuffered = true;     //Двойная буферизация (убирает мерцание при перерисовке)
            this.KeyPreview = true;         //Форма перехватывает нажатия клавиш

            //Подписываемся на события (привязываем методы к событиям)
            this.KeyDown += FormGame_KeyDown;       //Нажатие клавиши
            this.KeyUp += FormGame_KeyUp;           //Отпускание клавиши
            this.Paint += FormGame_Paint;           //Отрисовка формы
            this.FormClosing += FormGame_FormClosing; //Закрытие формы

            //Инициализация таймера
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 16; //16 миллисекунд = примерно 60 фпс 
            gameTimer.Tick += new EventHandler(GameLoop); //При каждом тике вызываем GameLoop

            BuildLevel();      //Создаем уровень (платформы, монеты, враги)
            gameTimer.Start(); //Запускаем игровой цикл
        }

        //ИГРОВОЙ ЦИКЛ
        //Вызывается 60 раз в секунду
        private void GameLoop(object? sender, EventArgs e)
        {
            UpdatePhysics(); //Обновляем физику Марио
            UpdateEnemies(); //Двигаем врагов
            CheckCoins();    //Проверяем сбор монет
            CheckEnemies();  //Проверяем столкновение с врагами
            CheckFinish();   //Проверяем, дошел ли до финиша

            //Обработка таймера неуязвимости
            if (isInvulnerable)
            {
                invulnerableTimer--; //Уменьшаем таймер
                if (invulnerableTimer <= 0) isInvulnerable = false; //Если время вышло - отключаем неуязвимость
            }

            Invalidate(); //Вызывает перерисовку экрана (событие Paint)
        }

        //СОЗДАНИЕ УРОВНЯ
        //Метод, который расставляет платформы, лестницы, монеты и врагов
        private void BuildLevel()
        {
            //Очищаем все списки, чтобы при перезапуске не оставались старые объекты
            platforms.Clear();
            coins.Clear();
            enemies.Clear();
            enemySpeeds.Clear();
            ladders.Clear();

            //Сбрасываем параметры игры
            score = 0;
            lives = 3;
            isInvulnerable = false;
            playerX = 50; //Спавн Марио слева
            playerY = this.ClientSize.Height - 50; //Спавн на земле

            int groundLevel = this.ClientSize.Height - 40; //Вычисляем уровень земли (для удобства)

            //В зависимости от выбранной сложности строим разную карту
            if (difficulty == "Легко")
            {
                //Платформа 1 (высота 250)
                platforms.Add(new Rectangle(150, 250, 150, 20));
                //Лестница от земли до платформы 1
                ladders.Add(new Rectangle(200, 250, 20, groundLevel - 250));

                //Платформа 2 (высота 150)
                platforms.Add(new Rectangle(450, 150, 150, 20));
                //Лестница от земли до платформы 2
                ladders.Add(new Rectangle(500, 150, 20, groundLevel - 150));

                //Монетки над платформами
                coins.Add(new Rectangle(180, 210, 20, 20));
                coins.Add(new Rectangle(480, 110, 20, 20));

                //Один враг на земле
                enemies.Add(new Rectangle(350, groundLevel - 30, 30, 30));
                enemySpeeds.Add(2f); //Двигается вправо со скоростью 2

                //Финишная дверь в правом нижнем углу
                finishDoor = new Rectangle(this.ClientSize.Width - 60, groundLevel - 70, 40, 70);
            }
            else if (difficulty == "Нормально")
            {
                //Три платформы разной высоты
                platforms.Add(new Rectangle(120, 300, 120, 20));
                ladders.Add(new Rectangle(150, 300, 20, groundLevel - 300));

                platforms.Add(new Rectangle(350, 220, 150, 20));
                ladders.Add(new Rectangle(400, 220, 20, groundLevel - 220));

                platforms.Add(new Rectangle(600, 140, 100, 20));
                ladders.Add(new Rectangle(630, 140, 20, groundLevel - 140));

                //Монетки
                coins.Add(new Rectangle(150, 260, 20, 20));
                coins.Add(new Rectangle(400, 180, 20, 20));
                coins.Add(new Rectangle(630, 100, 20, 20));

                //Два врага (один на земле, один на платформе)
                enemies.Add(new Rectangle(250, groundLevel - 30, 30, 30));
                enemySpeeds.Add(3f);
                enemies.Add(new Rectangle(500, 220 - 30, 30, 30));
                enemySpeeds.Add(-2f); // Идет влево

                finishDoor = new Rectangle(this.ClientSize.Width - 60, groundLevel - 70, 40, 70);
            }
            else //"Сложно"
            {
                //Много маленьких платформ (лесенкой)
                platforms.Add(new Rectangle(80, 320, 80, 20));
                ladders.Add(new Rectangle(100, 320, 20, groundLevel - 320));

                platforms.Add(new Rectangle(220, 260, 80, 20));
                ladders.Add(new Rectangle(240, 260, 20, groundLevel - 260));

                platforms.Add(new Rectangle(380, 200, 80, 20));
                ladders.Add(new Rectangle(400, 200, 20, groundLevel - 200));

                platforms.Add(new Rectangle(550, 140, 80, 20));
                ladders.Add(new Rectangle(570, 140, 20, groundLevel - 140));

                //Монетки
                coins.Add(new Rectangle(100, 280, 20, 20));
                coins.Add(new Rectangle(240, 220, 20, 20));
                coins.Add(new Rectangle(400, 160, 20, 20));
                coins.Add(new Rectangle(570, 100, 20, 20));

                //Три врага (два на земле, один на платформе)
                enemies.Add(new Rectangle(180, groundLevel - 30, 30, 30));
                enemySpeeds.Add(4f);
                enemies.Add(new Rectangle(300, groundLevel - 30, 30, 30));
                enemySpeeds.Add(-3f);
                enemies.Add(new Rectangle(450, 200 - 30, 30, 30));
                enemySpeeds.Add(3f);

                finishDoor = new Rectangle(this.ClientSize.Width - 60, groundLevel - 70, 40, 70);
            }
        }

        //ФИЗИКА ПЕРСОНАЖА
        //Самый важный метод: двигает Марио, проверяет гравитацию и столкновения
        private void UpdatePhysics()
        {
            //Проверка: находимся ли мы на лестнице?
            Rectangle playerRect = new Rectangle((int)playerX, (int)playerY, PlayerSize, PlayerSize);
            isOnLadder = false;
            foreach (var ladder in ladders)
            {
                if (playerRect.IntersectsWith(ladder))
                {
                    isOnLadder = true;
                    break; //Нашли лестницу - дальше не ищем
                }
            }

            //Горизонтальное движение (по флагам клавиш)
            if (isLeftPressed) velX = -MoveSpeed;
            else if (isRightPressed) velX = MoveSpeed;
            else velX = 0; //Если клавиши не нажаты - стоим на месте

            playerX += velX; //Применяем скорость к координате

            //Вертикальное движение (лестница или гравитация)
            if (isOnLadder)
            {
                velY = 0; //Отключаем гравитацию на лестнице
                if (isUpPressed) playerY -= MoveSpeed;   //Лезем вверх
                if (isDownPressed) playerY += MoveSpeed; //Лезем вниз
            }
            else
            {
                velY += Gravity; //Гравитация тянет вниз (скорость растет)
                playerY += velY; //Применяем скорость
            }

            bool onGround = false; //Флаг: стоим ли на твердой поверхности

            //Проверка столкновений с платформами
            foreach (var plat in platforms)
            {
                //Если мы лезем по лестнице, игнорируем верхние платформы, чтобы пролезть сквозь них
                if (isOnLadder && (isUpPressed || isDownPressed)) continue;

                if (playerRect.IntersectsWith(plat))
                {
                    //Если падаем вниз и коснулись платформы сверху
                    if (velY > 0 && playerRect.Bottom <= plat.Bottom + 10)
                    {
                        playerY = plat.Top - PlayerSize; //Ставим Марио НА платформу
                        velY = 0; //Останавливаем падение
                        onGround = true; //Мы на земле
                    }
                    //Если бьемся головой об платформу снизу
                    else if (velY < 0 && playerRect.Top >= plat.Top)
                    {
                        playerY = plat.Bottom; //Опускаем вниз
                        velY = 0; //Останавливаем полет
                    }
                }
            }

            //Проверка земли (пола)
            int floorY = this.ClientSize.Height - PlayerSize;
            if (playerY > floorY)
            {
                playerY = floorY; //Возвращаем на пол
                velY = 0;
                onGround = true;
            }

            //Проверка стен (не выходим за края экрана)
            if (playerX < 0) playerX = 0;
            if (playerX > this.ClientSize.Width - PlayerSize)
                playerX = this.ClientSize.Width - PlayerSize;

            //Разрешаем прыжок, только если мы на земле (и не на лестнице)
            if (!isOnLadder) canJump = onGround;
        }

        //ДВИЖЕНИЕ ВРАГОВ
        private void UpdateEnemies()
        {
            //Проходим по всем врагам
            for (int i = 0; i < enemies.Count; i++)
            {
                Rectangle enemy = enemies[i];

                //Двигаем врага по горизонтали (скорость из списка enemySpeeds)
                enemy.X += (int)enemySpeeds[i];
                enemies[i] = enemy; //Сохраняем обратно в список

                //Если враг дошел до края экрана - разворачиваем его
                if (enemy.X <= 0 || enemy.X + enemy.Width >= this.ClientSize.Width)
                {
                    enemySpeeds[i] *= -1; //Меняем знак скорости (например, 2 -> -2)
                }
            }
        }

        //СБОР МОНЕТ
        private void CheckCoins()
        {
            Rectangle playerRect = new Rectangle((int)playerX, (int)playerY, PlayerSize, PlayerSize);

            //Идем с конца списка, чтобы безопасно удалять элементы
            for (int i = coins.Count - 1; i >= 0; i--)
            {
                if (playerRect.IntersectsWith(coins[i]))
                {
                    coins.RemoveAt(i); //Удаляем монетку
                    score += 10;       //Добавляем 10 очков
                }
            }
        }

        //СТОЛКНОВЕНИЕ С ВРАГАМИ
        private void CheckEnemies()
        {
            if (isInvulnerable) return; //Если уже неуязвимы - пропускаем проверку

            Rectangle playerRect = new Rectangle((int)playerX, (int)playerY, PlayerSize, PlayerSize);

            foreach (var enemy in enemies)
            {
                if (playerRect.IntersectsWith(enemy))
                {
                    lives--; //Отнимаем жизнь
                    isInvulnerable = true; //Включаем неуязвимость
                    invulnerableTimer = 60; //60 кадров (примерно 1 секунда)

                    //Отбрасываем Марио
                    velY = -5;
                    playerX -= 30;

                    if (lives <= 0) //Если жизни кончились - проигрыш
                    {
                        GameOver(false);
                    }
                    break; //Выходим из цикла, чтобы не отнять сразу несколько жизней
                }
            }
        }

        //ПРОВЕРКА ФИНИША
        private void CheckFinish()
        {
            Rectangle playerRect = new Rectangle((int)playerX, (int)playerY, PlayerSize, PlayerSize);
            if (playerRect.IntersectsWith(finishDoor)) //Если коснулись двери
            {
                GameOver(true); //Победа!
            }
        }

        //КОНЕЦ ИГРЫ
        //Вызывается при победе (isWin = true) или поражении (isWin = false)
        private void GameOver(bool isWin)
        {
            gameTimer.Stop(); //Останавливаем игру

            //Открываем форму результатов (модально)
            using (FormResult resultForm = new FormResult(playerName, score, isWin))
            {
                DialogResult result = resultForm.ShowDialog(); //Показываем окно и ждем ответа

                if (result == DialogResult.Retry) //Если игрок нажал "Играть заново"
                {
                    BuildLevel(); //Пересобираем уровень
                    gameTimer.Start(); //Запускаем таймер
                }
                else
                {
                    this.Close(); //Закрываем игру (вернемся в меню)
                }
            }
        }

        //УПРАВЛЕНИЕ
        //Нажатие клавиш
        private void FormGame_KeyDown(object? sender, KeyEventArgs e)
        {
            //Ставим флаги в true, когда клавиша зажата
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) isLeftPressed = true;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) isRightPressed = true;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W) isUpPressed = true;
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S) isDownPressed = true;

            //Прыжок (только если стоим на земле)
            if (e.KeyCode == Keys.Space)
            {
                if (canJump && !isOnLadder)
                {
                    velY = JumpPower; //Задаем скорость вверх
                    canJump = false; //Запрещаем двойной прыжок
                }
            }
        }

        //Отпускание клавиш
        private void FormGame_KeyUp(object? sender, KeyEventArgs e)
        {
            //Ставим флаги в false, когда клавиша отпущена
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) isLeftPressed = false;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) isRightPressed = false;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W) isUpPressed = false;
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S) isDownPressed = false;
        }

        //ОТРИСОВКА
        private void FormGame_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; //Получаем объект для рисования

            //Рисуем землю (зеленая полоса внизу)
            g.FillRectangle(Brushes.Green, 0, this.ClientSize.Height - 20, this.ClientSize.Width, 20);

            //Рисуем лестницы
            foreach (var ladder in ladders)
            {
                g.FillRectangle(Brushes.SaddleBrown, ladder); // Заливаем коричневым
                //Рисуем поперечные перекладины каждые 15 пикселей
                for (int y = ladder.Y; y < ladder.Bottom; y += 15)
                {
                    g.DrawLine(Pens.Black, ladder.X, y, ladder.Right, y);
                }
            }

            //Рисуем платформы
            foreach (var plat in platforms)
            {
                g.FillRectangle(Brushes.SaddleBrown, plat); //Заливка
                g.DrawRectangle(Pens.Black, plat);          //Контур
            }

            //Рисуем монетки
            foreach (var coin in coins)
            {
                g.FillEllipse(Brushes.Gold, coin);            //Золотая заливка
                g.DrawEllipse(Pens.DarkGoldenrod, coin);      //Темный контур
            }

            //Рисуем врагов
            foreach (var enemy in enemies)
            {
                g.FillRectangle(Brushes.Black, enemy); //Черное тело
                //Красные глаза
                g.FillRectangle(Brushes.Red, enemy.X + 5, enemy.Y + 5, 5, 5);
                g.FillRectangle(Brushes.Red, enemy.X + 20, enemy.Y + 5, 5, 5);
            }

            //Рисуем финишную дверь
            g.FillRectangle(Brushes.Green, finishDoor);
            g.DrawRectangle(Pens.DarkGreen, finishDoor);
            g.DrawString("FINISH", this.Font, Brushes.White, finishDoor.X - 5, finishDoor.Y - 20);

            //Рисуем Марио (с миганием при неуязвимости)
            if (!isInvulnerable || invulnerableTimer % 10 < 5) //Каждый 10-й кадр Марио невидим
            {
                g.FillRectangle(Brushes.Red, playerX, playerY, PlayerSize, PlayerSize);
            }

            //Рисуем HUD (Интерфейс)
            //Очки (слева вверху)
            g.DrawString($"Очки: {score}", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, 10, 10);
            //Уровень (под очками)
            g.DrawString($"Уровень: {difficulty}", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 10, 35);

            //Жизни (справа вверху в виде красных кружков)
            for (int i = 0; i < lives; i++)
            {
                int heartX = this.ClientSize.Width - 30 - (i * 25); //Сдвигаем влево для каждой следующей жизни
                g.FillEllipse(Brushes.Red, heartX, 10, 20, 20);
                g.DrawEllipse(Pens.DarkRed, heartX, 10, 20, 20);
            }
        }

        //ЗАКРЫТИЕ ФОРМЫ
        private void FormGame_FormClosing(object? sender, FormClosingEventArgs e)
        {
            gameTimer.Stop(); //Останавливаем таймер, чтобы игра не грузила процессор
            Application.OpenForms["Form1"]?.Show(); //Показываем главное меню снова (оно было скрыто)
        }
    }
}