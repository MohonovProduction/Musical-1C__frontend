using System;

namespace Musical1C__frontend.Services.ResReq;

public class SoundRequest
{
    public SoundRequest(Guid id, string name, string author)
    {
        Id = id;
        Name = name;
        Author = author;
    }

    public Guid Id { get; set; } // Id произведения (звука)
    public string Name { get; set; } // Название произведения
    public string Author { get; set; } // Автор произведения
}