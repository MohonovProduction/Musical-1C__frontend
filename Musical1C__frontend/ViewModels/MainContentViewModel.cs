using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Musical1C__frontend.ViewModels;

public class MainContentViewModel : ViewModelBase
{
    public ICommand NavigateToCreateViewModelCommand { get; }
    public ICommand NavigateToGetHistoryViewModelCommand { get; }
    public ICommand NavigateToSearchMusicianViewModelViewModelCommand { get; }
    private readonly Action _NavigateToCreateViewModelCommand;
    private readonly Action _NavigateToGetHistoryViewModelCommand;
    private readonly Action _NavigateToSearchMusicianViewModelCommand;
    public MainContentViewModel(Action switchToCreateViewModel, Action switchToGetHistoryViewModel, Action switchToSearchMusicianViewModel)
    {
        _NavigateToCreateViewModelCommand = switchToCreateViewModel;
        _NavigateToGetHistoryViewModelCommand = switchToGetHistoryViewModel;
        _NavigateToSearchMusicianViewModelCommand = switchToSearchMusicianViewModel;
        
        NavigateToCreateViewModelCommand = new RelayCommand(_NavigateToCreateViewModelCommand);
        NavigateToGetHistoryViewModelCommand = new RelayCommand(_NavigateToGetHistoryViewModelCommand);
        NavigateToSearchMusicianViewModelViewModelCommand = new RelayCommand(_NavigateToSearchMusicianViewModelCommand);
    }
    
    
}