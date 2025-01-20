using System;
using System.Collections.Generic;

namespace Musical1C__frontend.Models;

public class Instrument
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    // Связь с музыкантами через таблицу musician_instrument
    public ICollection<MusicianInstrument> MusicianInstruments { get; set; }

    public Instrument(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Instrument()
    {
    }
}