using System;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Contacts_DB_WPF_UI.ViewModels
{
  public delegate bool ConfirmMessage(object sender, MessageEventArgs e);
  public delegate void WarningMessage(object sender, MessageEventArgs e);
  public delegate void ErrorMessage(object sender, MessageEventArgs e);

  public class MessageEventArgs : EventArgs
  {
    public string Message { get; private set; }

    public MessageEventArgs(string message)
      : base()
    {
      Message = message;
    }
  }

  public class RelayCommand : ICommand
  {
    readonly Action<object> execute;
    readonly Predicate<object> canExecute;

    public RelayCommand(Action<object> execute)
      : this(execute, null)
    {
    }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
      this.execute = execute;
      this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
      return canExecute == null ? true : canExecute(parameter);
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter)
    {
      execute(parameter);
    }
  }

  public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable, INotifyCollectionChanged
  {
    public event ErrorMessage ErrorHandler;
    public event WarningMessage WarningHandler;
    public event ConfirmMessage ConfirmHandler;
    public event PropertyChangedEventHandler PropertyChanged;
    public event NotifyCollectionChangedEventHandler CollectionChanged;
    public event EventHandler CloseHandler;
    public RelayCommand CloseCommand { get; private set; }

    public ViewModelBase()
    {
      CloseCommand = new RelayCommand(p => { if (CloseHandler != null) CloseHandler(this, EventArgs.Empty); });
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, object item)
    {
      if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item));
    }

    public void Dispose()
    {
      this.OnDispose();
    }

    protected virtual void OnDispose()
    {
    }

    public void OnWarning(string message)
    {
      if (WarningHandler != null) WarningHandler(this, new MessageEventArgs(message));
    }

    public void OnError(string error)
    {
      if (ErrorHandler != null) ErrorHandler(this, new MessageEventArgs(error));
    }

    public bool OnConfirm(string message)
    {
      if (ConfirmHandler != null) return ConfirmHandler(this, new MessageEventArgs(message));
      return false;
    }

    public void OnClose()
    {
      if (CloseHandler != null) CloseHandler(this, EventArgs.Empty);
    }
  }
}
