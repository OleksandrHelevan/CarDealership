using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity;

public class GasolineCar : Car
{
    [Required]
    [ForeignKey(nameof(Engine))]
    [Column("engine_id")]
    public int EngineId { get; set; }

    [ForeignKey(nameof(EngineId))]
    public virtual GasolineEngine Engine { get; set; } = null!;

    // Computed property for engine display
    [NotMapped]
    public string EngineString => Engine?.EngineString ?? "Engine not specified";
}
