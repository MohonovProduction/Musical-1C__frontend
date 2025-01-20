using System;
using System.Collections.Generic;

namespace Musical1C__frontend.Services.ResReq;

public class MusicianResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Surname { get; set; }
    public List<InstrumentResponse> Instruments { get; set; }
    public List<ConcertResponse> Concerts { get; set; }
}