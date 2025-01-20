using System.Collections.Generic;
using WebStation.Trains;

namespace Musical1C__frontend.Services.ResReq;

public class AddMusicianRequest
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Surname { get; set; }
    public List<InstrumentRequest> Instruments { get; set; }
    public List<ConcertRequest> Concerts { get; set; }
}