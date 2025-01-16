using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AvaloniaUI.Services;
using CommunityToolkit.Mvvm.Input;
using Musical1C__frontend.Services;

namespace Musical1C__frontend.ViewModels;

public class GetHistoryViewModel : ViewModelBase
{
    
    private readonly Action _switchToMainContentViewModel;
    public ICommand SwitchToMainContentViewModelCommand { get; }

    public ObservableCollection<ConcertResponse> _concerts { get; set; }
    
    private ConcertService _concertService;
    
    public GetHistoryViewModel(Action switchToMainContentViewModel)
    {
        _switchToMainContentViewModel = switchToMainContentViewModel;
        SwitchToMainContentViewModelCommand = new RelayCommand(_switchToMainContentViewModel);
        _concertService = new ConcertService();

        _concerts = new ObservableCollection<ConcertResponse>();
        LoadConcertsAsync();
    }
    
    public async Task LoadConcertsAsync()
    {
        try
        {
            CancellationToken token = new CancellationToken();
            var concerts = await _concertService.GetConcertsAsync(token);
            Console.WriteLine(concerts.Count);

            foreach (var concert in concerts)
            {
                _concerts.Add(concert);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}