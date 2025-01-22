using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Musical1C__frontend.Services;
using Musical1C__frontend.Services.ResReq;

namespace Musical1C__frontend.ViewModels; 

public partial class CreateConcertPusyViewModel : ViewModelBase
{
    private readonly Action _switchToMainContentViewModel;
    
    public ICommand SwitchToMainContentViewModelCommand { get; }
    public ICommand OnMusicianSelectedCommand { get; set; }
    public ICommand OnSoundSelectedCommand { get; set; }
    public ICommand OnCreateConcertCommand { get; set;  }

    public ObservableCollection<MusicianResponse> _musicians { get; set; }
    public ObservableCollection<SoundResponse> _sounds { get; set; }

    public ObservableCollection<MusicianResponse> MusiciansForAdd { get; set; }
    public ObservableCollection<SoundResponse> SoundsForAdd { get; set; }
    
    [ObservableProperty]
    private string _stateMessage;
    [ObservableProperty]
    private bool _showStateMessage;

    private readonly MusicianService _musicianService;
    private readonly SoundService _soundService;
    private readonly ConcertService _concertService;

    public CreateConcertPusyViewModel(Action switchToMainContentViewModel)
    {
        _switchToMainContentViewModel = switchToMainContentViewModel;
        SwitchToMainContentViewModelCommand = new RelayCommand(_switchToMainContentViewModel);
        
        OnMusicianSelectedCommand = new RelayCommand<MusicianResponse>(OnMusicianSelected);
        OnSoundSelectedCommand = new RelayCommand<SoundResponse>(OnSoundSelected);
        OnCreateConcertCommand = new AsyncRelayCommand(OnCreateConcertSelected);
        
        
        _musicianService = new MusicianService();
        _soundService = new SoundService();
        _concertService = new ConcertService();
        
        _musicians = new ObservableCollection<MusicianResponse>();
        _sounds = new ObservableCollection<SoundResponse>();
        
        MusiciansForAdd = new ObservableCollection<MusicianResponse>();
        SoundsForAdd = new ObservableCollection<SoundResponse>();
        
        _ = LoadMusicianAsync();
        _ = LoadSoundAsync();
    }
    
    [ObservableProperty]
    private string _concertName;

    [ObservableProperty] 
    private string _concertDate;

    [ObservableProperty] 
    private int _concertType;

    private async Task ShowMessageAsync(string message, int duration = 5000)
    {
        StateMessage = message;
        //_showStateMessage = true;
        ShowStateMessage = true;
        await Task.Delay(duration);
        ShowStateMessage = false;
        //_showStateMessage = false;
    }
    
    public async Task OnCreateConcertSelected()
    {
        var token = new CancellationToken();

        if (string.IsNullOrWhiteSpace(_concertName) || _concertDate == default || _concertType < 0 || MusiciansForAdd == null || SoundsForAdd == null)
        {
            await ShowMessageAsync("Invalid input data. Please check concert details.");
            return;
        }

        var newConcert = new AddConcertRequest
        {
            Name = _concertName,
            Date = _concertDate,
            Type = (_concertType == 0) ? "Групповой" : "Общий",
            Musicians = new List<MusicianRequest>(),
            Sounds = new List<SoundRequest>()
        };

        Console.WriteLine($"Creating new concert: {newConcert}");

        foreach (var musician in MusiciansForAdd)
        {
            var newMusician = new MusicianRequest(
                musician.Id,
                musician.Name,
                musician.LastName,
                musician.Surname);
            newConcert.Musicians.Add(newMusician);
            Console.WriteLine($"Creating new musician: {newMusician}");
        }

        foreach (var sound in SoundsForAdd)
        {
            var newSound = new SoundRequest(sound.Id, sound.Name, sound.Author);
            newConcert.Sounds.Add(newSound);
            Console.WriteLine($"Creating new sound: {newSound}");
        }

        try
        {
            await _concertService.AddConcertAsync(newConcert, token);
            await ShowMessageAsync("Concert created successfully", 3000);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await ShowMessageAsync("Error creating new concert, call to the support\n+88005553535 - Zinaida Pavlovna", 10000);
            throw;
        }
    }
    
    public void OnMusicianSelected(MusicianResponse? musicianResponse)
    {
        if (musicianResponse != null)
        {
            // Проверяем, существует ли элемент в коллекции
            if (MusiciansForAdd.Contains(musicianResponse))
            {
                // Убираем его из массива
                MusiciansForAdd.Remove(musicianResponse);
            }
            else
            {
                // Добавляем элемент, если его нет в коллекции
                MusiciansForAdd.Add(musicianResponse);
            }
        }
    }

    public void OnSoundSelected(SoundResponse? soundResponse)
    {
        if (soundResponse != null)
        {
            if (SoundsForAdd.Contains(soundResponse))
            {
                SoundsForAdd.Remove(soundResponse);
            }
            else
            {
                SoundsForAdd.Add(soundResponse);
            }
        }
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
                Console.WriteLine(musician);
                _musicians.Add(musician);
            }
            
            Console.WriteLine(_musicians.Count);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    public async Task LoadSoundAsync()
    {
        try
        {
            CancellationToken token = new CancellationToken();
            var sounds = await _soundService.GetAllMusicAsync(token);

            foreach (var sound in sounds)
            {
                if (sound is null) 
                    continue;
                Console.WriteLine(sound);
                _sounds.Add(sound);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}