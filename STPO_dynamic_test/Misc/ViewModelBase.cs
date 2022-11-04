using System;
using System.Windows;

using PropertyChanged;


namespace STPO_dynamic_test.Misc;

/// <summary>
///     Абстрактный класс для VM
/// </summary>
[AddINotifyPropertyChangedInterface]
public class ViewModelBase
{
    private RelayCommand _closeWindow;

    /// <summary>
    ///     Команда, закрывающая текущее окно
    /// </summary>
    public RelayCommand CloseWindow
    {
        get
        {
            return _closeWindow ??= new RelayCommand(o =>
            {
                OnClosingRequest();
            });
        }
    }

    /// <summary>
    ///     Команда, открывающая новое окно
    /// </summary>
    /// <param name="window"></param>
    public static void ShowChildWindow(Window window)
    {
        window.Show();
    }

    public event EventHandler ClosingRequest;

    /// <summary>
    ///     Функция, закрывающая текущее окно
    /// </summary>
    protected void OnClosingRequest()
    {
        ClosingRequest?.Invoke(this, EventArgs.Empty);
    }
}
