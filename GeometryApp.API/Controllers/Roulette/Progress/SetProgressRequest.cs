using System;
using System.ComponentModel.DataAnnotations;

namespace GeometryApp.API.Controllers.Roulette.Progress;

public class SetProgressRequest
{
    [Required]
    public string SessionId { get; set; }
    [Required]
    public Guid RouletteId { get; set; }
    [Required]
    public int SequenceNumber { get; set; }
    [Required]
    public int Progress { get; set; }
}
