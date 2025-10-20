using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;
using CarDealership.enums;

namespace CarDealership.entity
{
    public class ElectroCar : Car
    {
        [Required]
        [ForeignKey(nameof(Engine))]
        [Column("engine_id")]
        public int EngineId { get; set; }

        [ForeignKey(nameof(EngineId))]
        public virtual ElectroEngine Engine { get; set; } = null!;

        // Computed property for engine display
        [NotMapped]
        public string EngineString => Engine?.EngineString ?? "Engine not specified";
    }
}
