using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Musical1C__frontend.ViewModels;

public class SearchMusicianViewModel : ViewModelBase
{
    private readonly Action _switchToMainContentViewModel;
    public ICommand SwitchToMainContentViewModelCommand { get; }

    public SearchMusicianViewModel(Action switchToMainContentViewModel)
    {
        _switchToMainContentViewModel = switchToMainContentViewModel;
        SwitchToMainContentViewModelCommand = new RelayCommand(_switchToMainContentViewModel);
    }
}