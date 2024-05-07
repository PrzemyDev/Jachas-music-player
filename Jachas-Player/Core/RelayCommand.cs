using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jachas_Lo_Fi_.Core
{
    internal class RelayCommand :ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        //To zdarzenie informuje system, że warunki wywołania metody CanExecute mogły ulec zmianie

        //ctor:
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
        //Określa czy dana komenda może być wykonana w danym momencie (jeśli true to aktywna i można)

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        //Wykonanie określonej akcji, która jest przekazana jako parametr do konstruktora RelayCommand
    }
}
