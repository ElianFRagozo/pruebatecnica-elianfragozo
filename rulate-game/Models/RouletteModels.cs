using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RouletteAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Balance { get; set; }
    }

    public class SpinResult
    {
        public int Number { get; set; }
        public string Color { get; set; }
    }

    public class Bet
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Color { get; set; }

        // Quitar la validación Required de Parity
        public string Parity { get; set; }

        public int? Number { get; set; }
    }

    public class BetRequest
    {
        [Required]
        public Bet Bet { get; set; }

        [Required]
        public SpinResult Result { get; set; }
    }

    public class BetResult
    {
        public bool Won { get; set; }
        public decimal WinAmount { get; set; }
        public string Message { get; set; }
    }
}