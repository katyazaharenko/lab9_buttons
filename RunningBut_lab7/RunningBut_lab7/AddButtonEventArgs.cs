using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningBut_lab7
{
    /// <summary>
    /// Класс для передачи аргументов между формами при исполнении события добавления кнопки.
    /// </summary>
    public class AddButtonEventArgs : EventArgs
    {
        public string Param { get; internal set; }

        public AddButtonEventArgs(string paramValue)
        {
            Param = paramValue;
        }
    }
}
