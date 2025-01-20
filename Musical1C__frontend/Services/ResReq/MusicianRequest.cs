using System;

namespace Musical1C__frontend.Services.ResReq;

public class MusicianRequest
{
    public Guid Id { get; set; } // Id музыканта
    public string Name { get; set; } // Имя музыканта
    public string Lastname { get; set; } // Фамилия музыканта
    public string Surname { get; set; } // Отчество музыканта

    public MusicianRequest(Guid id, string name, string lastname, string surname)
    {
        Id = id;
        Name = name;
        Lastname = lastname;
        Surname = surname;
    }
}

public class InstrumentRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class ConcertRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}