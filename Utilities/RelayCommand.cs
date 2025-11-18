using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnewheroVisitorManagement.Utilities
{
    class RelayCommand : ICommand //ICommand is an interface that defines a command
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged //event that is raised when the ability of the command to execute changes
        {
            add { CommandManager.RequerySuggested += value; } //when something changes, it will requery the command to see if it can execute
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) //RelayCommand takes an Action and a Func, canExecute is optional
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter); //if canExecute is null, return true, otherwise return the result of canExecute
        public void Execute(object parameter) => _execute(parameter); //execute the action
    }
}