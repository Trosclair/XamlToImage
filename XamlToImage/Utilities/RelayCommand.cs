using System.Windows.Input;

namespace XamlToPNG.Utilities
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> action;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object?> action)
        {
            this.action = action;
        }

        public RelayCommand(Action action)
        {
            this.action = (_) => action();
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            action(parameter);
        }
    }
}
