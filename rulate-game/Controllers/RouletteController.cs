using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Models;
using System;

namespace RouletteAPI.Controllers
{
    [Route("api/roulette")]
    public class RouletteController : ControllerBase
    {
        private readonly Random _random;

        public RouletteController()
        {
            _random = new Random();
        }

        [HttpGet("spin")]
        public ActionResult<SpinResult> Spin()
        {
            // Generar número aleatorio entre 0 y 36
            int number = _random.Next(0, 37);

            // Determinar color
            string color = DetermineColor(number);

            return new SpinResult
            {
                Number = number,
                Color = color
            };
        }

        [HttpPost("bet")]
        public ActionResult<BetResult> ProcessBet([FromBody] BetRequest request)
        {
            if (request == null || request.Bet == null || request.Result == null)
            {
                return BadRequest("Datos de apuesta inválidos");
            }

            var bet = request.Bet;
            var result = request.Result;

            // Validar los datos de la apuesta
            if (bet.Amount <= 0)
            {
                return BadRequest("El monto de la apuesta debe ser mayor que cero");
            }

            if (string.IsNullOrEmpty(bet.Type))
            {
                return BadRequest("El tipo de apuesta es requerido");
            }

            if (string.IsNullOrEmpty(bet.Color))
            {
                return BadRequest("El color de la apuesta es requerido");
            }

            bool won = false;
            decimal winAmount = 0;
            string message = "";

            // Procesar apuesta según su tipo
            switch (bet.Type)
            {
                case "color":
                    won = result.Color == bet.Color;
                    winAmount = won ? bet.Amount / 2 : 0;
                    message = won
                        ? $"¡Acertaste el color {bet.Color}!"
                        : $"El color fue {result.Color}, no {bet.Color}.";
                    break;

                case "parity":
                    if (string.IsNullOrEmpty(bet.Parity))
                    {
                        return BadRequest("La paridad es requerida para apuestas de tipo 'parity'");
                    }

                    bool isEven = result.Number % 2 == 0;
                    bool betOnEven = bet.Parity == "par";
                    bool correctParity = (isEven && betOnEven) || (!isEven && !betOnEven);

                    won = correctParity && result.Color == bet.Color;
                    winAmount = won ? bet.Amount : 0;

                    string resultParity = isEven ? "par" : "impar";
                    message = won
                        ? $"¡Acertaste! El número {result.Number} es {resultParity} y {result.Color}."
                        : $"El número {result.Number} es {resultParity} y {result.Color}.";
                    break;

                case "specific":
                    if (!bet.Number.HasValue)
                    {
                        return BadRequest("El número es requerido para apuestas de tipo 'specific'");
                    }

                    won = result.Number == bet.Number && result.Color == bet.Color;
                    winAmount = won ? bet.Amount * 3 : 0;
                    message = won
                        ? $"¡Acertaste el número {bet.Number} y color {bet.Color}!"
                        : $"El resultado fue {result.Number} {result.Color}.";
                    break;

                default:
                    return BadRequest($"Tipo de apuesta no válido: {bet.Type}");
            }

            return new BetResult
            {
                Won = won,
                WinAmount = winAmount,
                Message = message
            };
        }

        private string DetermineColor(int number)
        {
            // En una ruleta real, el 0 es verde, pero aquí simplificamos a rojo/negro
            if (number == 0)
            {
                // Para simplificar, asignamos el 0 a un color aleatorio
                return _random.Next(2) == 0 ? "rojo" : "negro";
            }

            // Números rojos en la ruleta: 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36
            int[] redNumbers = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };

            return Array.IndexOf(redNumbers, number) >= 0 ? "rojo" : "negro";
        }
    }
}