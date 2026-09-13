using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table("Weather")]
    public class Weather
    {
        [Key]
        public int WeatherID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public DateTime ForecastDate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Temperature { get; set; }

        [Required]
        [MaxLength(100)]
        public string Conditions { get; set; } = string.Empty;

        [Column(TypeName = "decimal(5,2)")]
        public decimal WindSpeed { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Humidity { get; set; }

        [ForeignKey("EventID")]
        public Event? Event { get; set; }
    }
}