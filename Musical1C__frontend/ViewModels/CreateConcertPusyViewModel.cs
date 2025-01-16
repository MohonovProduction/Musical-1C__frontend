using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Musical1C__frontend.Services;
using Storage;

namespace Musical1C__frontend.ViewModels; 

public partial class CreateConcertPusyViewModel : ViewModelBase
{
    private readonly Action _switchToMainContentViewModel;
    public ICommand SwitchToMainContentViewModelCommand { get; }

    public ObservableCollection<MusicianResponse> _musicians { get; set; }
    private ObservableCollection<Sound> _sounds { get; set; }

    private List<MusicianResponse> _musiciansForAdd;
    
    private MusicianService _musicianService;

    public CreateConcertPusyViewModel(Action switchToMainContentViewModel)
    {
        _switchToMainContentViewModel = switchToMainContentViewModel;
        SwitchToMainContentViewModelCommand = new RelayCommand(_switchToMainContentViewModel);
        _musicianService = new MusicianService();
        _musicians = new ObservableCollection<MusicianResponse>();
        
        LoadMusicianAsync();
    }
    
    [ObservableProperty]
    private string _concertName;

    [ObservableProperty] 
    private string _concertDate;

    [ObservableProperty] 
    private string _concertType;

    public void OnMusicianSelected(MusicianResponse musicianResponse)
    {
        _musiciansForAdd.Add(musicianResponse);
    }

    public async Task LoadMusicianAsync()
    {
        try
        {
            CancellationToken token = new CancellationToken();
            var musicians = await _musicianService.GetMusiciansAsync(token);
            Console.WriteLine(musicians.Count);

            foreach (var musician in musicians)
            {
                if (musician is null) 
                    continue;
                Console.WriteLine(musician);
                _musicians.Add(musician);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}