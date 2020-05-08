using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace RunningBut_lab7
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Предыдущее положение мыши.
        /// </summary>
        Point lastMousePosition;
        /// <summary>
        /// Предыдущий размер окна.
        /// </summary>
        Size lastClientSize;
        /// <summary>
        /// Разница между новой и старой шириной окна.
        /// </summary>
        int diffX = 0;
        /// <summary>
        /// Разница между новой и старой высотой окна.
        /// </summary>
        int diffY = 0;
        /// <summary>
        /// Координаты мыши.
        /// </summary>
        Point point1;
        /// <summary>
        /// Смещение кнопки от края формы.
        /// </summary>
        int h = 20;
        /// <summary>
        /// Генератор случайных чисел
        /// </summary>
        Random rnd = new Random();
        /// <summary>
        /// Список "рабегающихся" кнопок
        /// </summary>
        List<ButtonWithSpeed> butList;
        /// <summary>
        /// Список задач
        /// </summary>
        List<Task> tasks = new List<Task>();
        /// <summary>
        /// Индекс для определения имени кнопки
        /// </summary>
        public int button_name_index = 0;
        bool flag = true;
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Класс-кнопка с возможностью задать скорость движения
        /// </summary>
        class ButtonWithSpeed : Form1
        {
            /// <summary>
            /// Элемент управления Button
            /// </summary>
            public Button button;
            /// <summary>
            /// Скорость кнопки - коэффициент задержки потока
            /// </summary>
            public int Speed;
            /// <summary>
            /// Конструктор класса
            /// </summary>
            public ButtonWithSpeed(int speed)
            {
                Speed = speed;
                button = new Button();
                button.Name = "button_" + (++button_name_index);
                button.Text = "Push me";
                button.Size = new Size(80, 39);
                button.Location = new Point(rnd.Next(0, ClientSize.Width - button.Width), rnd.Next(menuStrip1.Height, ClientSize.Height - button.Height));
                button.Font = new Font(FontFamily.GenericSansSerif, 12);
                button.MouseClick += button1_MouseClick;
                button.Anchor = AnchorStyles.None;
            }
            /// <summary>
            /// Конструктор класса
            /// </summary>
            public ButtonWithSpeed()
            {
                Speed = 1;
                button = new Button();
                button.Name = "button_" + button_name_index++;
                button.Text = "Push me";
                button.Size = new Size(80,39);
                button.Top = (ClientSize.Height - button.Height) / 2;
                button.Left = (ClientSize.Width - button.Width) / 2;
                button.Font = new Font(FontFamily.GenericSansSerif,12);
                button.MouseClick += button1_MouseClick;
                button.Anchor = AnchorStyles.None;
            }
        }
        /// <summary>
        /// Обработчик события нажатия на кнопку. Вызываем Message Box.
        /// </summary>
        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Поздравляем! Вы смогли нажать на кнопку!", "Убегающая кнопка");
        }
        /// <summary>
        /// Обработчик события загрузки формы.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            lastClientSize = ClientSize;
            butList = new List<ButtonWithSpeed>();
            ButtonWithSpeed firstbut = new ButtonWithSpeed();
            butList.Add(firstbut);
            Controls.Add(firstbut.button);
            flag = true;
            Task t = new Task(() =>
            {
                while (flag)
                {
                    if (point1 != lastMousePosition)
                    {
                        Point nmp = point1;
                        Point resPoint = ChangeLocation(firstbut, nmp);
                        Invoke(new Action(() =>
                        {
                            if (resPoint != new Point(-100, -100))
                                firstbut.button.Location = resPoint;
                        }));
                    }
                }
            });
            tasks.Add(t);
            t.Start();
        }
        /// <summary>
        /// Обработчик события движения мыши. Перемещаем кнопку согласно направлению мыши.
        /// </summary>
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            point1 = new Point(e.X, e.Y);
            if (lastMousePosition == point1)
                {
                    return;
                }
            lastMousePosition = point1;
        }
        /// <summary>
        /// Функция, возвращающая новое положение кнопки
        /// </summary>
        /// <param name="b">Объект класса ButtonWithSpeed.</param>
        /// <param name="newmp">Новое положение мыши в результате движения.</param>
        private Point ChangeLocation(ButtonWithSpeed b,Point newmp)
        {
            Point res =new Point (-100,-100);
            int x = newmp.X - b.button.Location.X;
            int y = newmp.Y - b.button.Location.Y;
            // Двигаться никуда не нужно
            if (x == 0 && y == 0) return res;
            // Длина вектора
            double k = Math.Sqrt(x * x + y * y);
            int dx = (int)(5 * x / k);
            int dy = (int)(5 * y / k);
            int newx = b.button.Location.X - dx;
            int newy = b.button.Location.Y - dy;
            int n;
            // Если движение мыши приводит к достижению кнопкой границ формы, отталкиваем кнопку на значение h от края
            if (b.button.Location.X - dx < 0 || b.button.Location.X - dx > ClientSize.Width - b.button.Width)
            {
                n = dx < 0 ? -h : +h;
                newx = b.button.Location.X + dx + n;
            }
            if (b.button.Location.Y - dy < menuStrip1.Height || b.button.Location.Y - dy > ClientSize.Height - b.button.Height)
            {
                n = dy < 0 ? -h : +h;
                newy = b.button.Location.Y + dy + n;
            }
            res = new Point(newx, newy);
            // Усыпляем поток для реализации скорости движения
            Thread.Sleep(10 * b.Speed);
            return res;
        }
        /// <summary>
        /// Обработчик события изменения размера формы. Корректируем положение кнопки.
        /// </summary>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            diffX = lastClientSize.Width - ClientSize.Width;
            diffY = lastClientSize.Height - ClientSize.Height;
            lastClientSize = ClientSize;
            
            if (diffX > 0 ||diffY> 0)
            {
                foreach (ButtonWithSpeed b in butList)
                {
                    if (b.button.Right > ClientSize.Width)
                    {
                        b.button.Left = ClientSize.Width - b.button.Width - h;
                    }

                    if (b.button.Left < 0)
                    {
                        b.button.Left = h;
                    }
                    if (b.button.Top < 0)
                    {
                        b.button.Top = h;
                    }

                    if (b.button.Bottom > ClientSize.Height)
                    {
                        b.button.Top = ClientSize.Height - b.button.Height - h;
                    }
                }
            }
        }
        /// <summary>
        /// Обработчик события нажатия на элемент Add Button меню. Добавление кнопки в форму с заданной скоростью.
        /// </summary>
        private void addButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddButton addform = new AddButton();
            addform.AddButtonEvent += new EventHandler<AddButtonEventArgs>(this.OnButtonAdded);
            addform.ShowDialog();
        }
        /// <summary>
        /// Метод для передачи между формами значения скорости новой кнопки
        /// </summary>
        private void OnButtonAdded(object sender, AddButtonEventArgs e)
        {
            ButtonWithSpeed newbut = new ButtonWithSpeed(Convert.ToInt32(e.Param));
            Controls.Add(newbut.button);
            butList.Add(newbut);
            flag = true;
            Task t = new Task(() =>
            {
                while (flag)
                {
                    if (point1 != lastMousePosition)
                    {
                        Point nmp = point1;
                        Point resPoint = ChangeLocation(newbut, nmp);
                        Invoke(new Action(() =>
                        {
                            if (resPoint != new Point(-100, -100))
                                newbut.button.Location = resPoint;
                        }));
                    }
                }
            });
            tasks.Add(t);
            t.Start();
        }
        /// <summary>
        /// Обработчик события нажатия на элемент Delete Button меню. Удаление всех "разбегающихся" кнопок.
        /// </summary>
        private void deleteButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = false;
            foreach (ButtonWithSpeed b in butList)
            {
                Controls.Remove(b.button);
            }
            butList.Clear();
        }
    }
}
