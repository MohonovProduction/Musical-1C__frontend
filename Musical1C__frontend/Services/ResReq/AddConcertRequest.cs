using System.Collections.Generic;
using WebStation.Trains;

namespace Musical1C__frontend.Services.ResReq;

public class AddConcertRequest
{
    public string Name { get; set; } // Название концерта
    public string Type { get; set; } // Тип концерта
    public string Date { get; set; } // Дата проведения концерта
    public List<MusicianRequest> Musicians { get; set; } // Список музыкантов
    public List<SoundRequest> Sounds { get; set; } // Список произведений
}