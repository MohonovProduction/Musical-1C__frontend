using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Musical1C__frontend.Services;
using Musical1C__frontend.Services.Responses;
using Storage;

namespace Musical1C__frontend.ViewModels;

public partial class SearchMusicianViewModel : ViewModelBase
{
    private readonly Action _switchToMainContentViewModel;
    public ICommand SwitchToMainContentViewModelCommand { get; }
    public ICommand SearchCommand { get; }
    
    public ObservableCollection<MusicianResponse> _musicians { get; set;  }
    
    private MusicianService _service;

    [ObservableProperty] 
    private string _searchText;
    
    public SearchMusicianViewModel(Action switchToMainContentViewModel)
    {
        _switchToMainContentViewModel = switchToMainContentViewModel;
        SwitchToMainContentViewModelCommand = new RelayCommand(_switchToMainContentViewModel);
        SearchCommand = new RelayCommand(GetMusiciansByLastName);
        
        _musicians = new ObservableCollection<MusicianResponse>();
        
        _service = new MusicianService();
    }

    public async void GetMusiciansByLastName()
    {
        _musicians.Clear();
        
        if (string.IsNullOrWhiteSpace(_searchText)) return;

        try
        {
            var musicians = await _service.SearchMusiciansByLastNameAsync(_searchText);
            if (musicians == null) return;
            foreach (var musician in musicians)
            {
                Console.WriteLine(musician);
                _musicians.Add(musician);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}