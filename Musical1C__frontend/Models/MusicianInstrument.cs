using System;

namespace Musical1C__frontend.Models
{
    public class MusicianInstrument
    {
        public Guid MusicianId { get; set; }
        public Musician Musician { get; set; }
        public Guid InstrumentId { get; set; }
        public Musical1C__frontend.Models.Instrument Instrument { get; set; }

        // Конструктор для инициализации всех свойств
        public MusicianInstrument(Guid musicianId, Guid instrumentId, Musician musician, Musical1C__frontend.Models.Instrument instrument)
        {
            MusicianId = musicianId;
            InstrumentId = instrumentId;
            Musician = musician;
            Instrument = instrument;
        }

        // Пустой конструктор для возможности создания объекта без инициализации
        public MusicianInstrument(Guid musicianId, Guid instrumentId)
        {
            MusicianId = musicianId;
            InstrumentId = instrumentId;
        }

        public MusicianInstrument()
        {
        }
    }
}