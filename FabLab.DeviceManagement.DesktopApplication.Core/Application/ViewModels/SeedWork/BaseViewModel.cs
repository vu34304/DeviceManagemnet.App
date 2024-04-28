using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork
{
    public class BaseViewModel : IViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected object _propertyValueCheckLock = new object();
        public string ErrorMessage { get; set; } = "";
        public bool IsErrorMessageShowed { get; set; } = false;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public virtual void Dispose() { }

        protected async Task RunCommandAsync(bool updatingFlag, Func<Task> action)
        {
            lock (_propertyValueCheckLock)
            {
                if (updatingFlag)
                {
                    return;
                }
                updatingFlag = true;
            }

            try
            {
                await action();
            }
            finally
            {
                updatingFlag = false;
            }
        }

        public void ShowErrorMessage(string message)
        {
            ErrorMessage = message;
            IsErrorMessageShowed = true;
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
