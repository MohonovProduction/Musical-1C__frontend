using System;

namespace Musical1C__frontend.Services.ResReq;

public class SoundResponse
{
    public Guid Id { get; set; } // Id произведения
    public string Name { get; set; } // Название произведения
    public string Author { get; set; } // Автор произведения
}