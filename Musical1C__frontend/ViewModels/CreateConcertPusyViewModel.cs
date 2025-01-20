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
using Musical1C__frontend.Services.Requests;
using Musical1C__frontend.Services.Responses;

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

    public async Task OnCreateConcertSelected()
    {
        var token = new CancellationToken();
        var newConcert = new ConcertRequest();
        
        newConcert.Name = _concertName;
        newConcert.Date = _concertDate;
        newConcert.Type = (_concertType == 0) ? "Групповой" : "Общий";
        newConcert.Musicians = new List<MusicianIdRequest>();
        newConcert.Sounds = new List<SoundIdRequest>();
        
        
        Console.WriteLine($"Creating new concert: {newConcert}");

        foreach (var musician in MusiciansForAdd)
        {
            var newMusician = new MusicianIdRequest(musician.Id);
            newConcert.Musicians.Add(newMusician);
            Console.WriteLine($"Creating new musician: {newMusician}");
        };

        foreach (var sound in SoundsForAdd)
        {
            var newSound = new SoundIdRequest(sound.Id);
            newConcert.Sounds.Add(newSound);
            Console.WriteLine($"Creating new sound: {newSound}");
        }
        
        Console.WriteLine($"Creating new concert: {newConcert}");

        try
        {
            await _concertService.AddConcertAsync(newConcert, token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public void OnMusicianSelected(MusicianResponse? musicianResponse)
    {
        //TODO: Сделать проверу на то, что элемент существует, если да -- нахуй его в печь
        //TODO: Проверить, добавлен ли
        if (musicianResponse != null)
        {
            MusiciansForAdd.Add(musicianResponse);
        }
    }

    public void OnSoundSelected(SoundResponse? soundResponse)
    {
        //TODO: Сделать проверу на то, что элемент существует, если да -- нахуй его в печь
        //TODO: Проверить, добавлен ли
        if (soundResponse != null)
        {
            SoundsForAdd.Add(soundResponse);
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
            var sounds = await _soundService.GetSoundsAsync(token);

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