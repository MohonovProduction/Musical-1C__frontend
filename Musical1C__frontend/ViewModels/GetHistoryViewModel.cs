using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Musical1C__frontend.Services;
using Musical1C__frontend.Services.ResReq;

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
                if (concert == null)
                {
                    Console.WriteLine("null");
                    continue;
                }
                _concerts.Add(concert);
                Console.WriteLine(concert.Name);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}