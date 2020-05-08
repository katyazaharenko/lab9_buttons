using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunningBut_lab7
{
    public partial class AddButton : Form
    {
        public event EventHandler<AddButtonEventArgs> AddButtonEvent;

        public AddButton()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки OK.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" )
            {
                MessageBox.Show("Скорость не задана!", "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                AddButtonEvent?.Invoke(this, new AddButtonEventArgs(comboBox1.Text));
                Close();
            }
        }
        private void AddButton_Load(object sender, EventArgs e)
        {

        }
    }
}
