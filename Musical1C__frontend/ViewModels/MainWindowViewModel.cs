using CommunityToolkit.Mvvm.ComponentModel;
using Musical1C__frontend.Views;

namespace Musical1C__frontend.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase? _currentViewModel;
    
    public MainWindowViewModel()
    {
        CurrentViewModel = new MainContentViewModel(SwitchToCreateViewModel, SwitchToGetHistoryViewModel, SwitchToSearchMusicianViewModel);
    }

    private void SwitchToSearchMusicianViewModel()
    {
        CurrentViewModel = new SearchMusicianViewModel(SwitchToMainContentViewModel);
    }

    private void SwitchToGetHistoryViewModel()
    {
        CurrentViewModel = new GetHistoryViewModel(SwitchToMainContentViewModel);
    }

    private void SwitchToCreateViewModel()
    {
        CurrentViewModel = new CreateConcertPusyViewModel(SwitchToMainContentViewModel);
    }

    private void SwitchToMainContentViewModel()
    {
        CurrentViewModel = new MainContentViewModel(SwitchToCreateViewModel, SwitchToGetHistoryViewModel, SwitchToSearchMusicianViewModel);
    }
}