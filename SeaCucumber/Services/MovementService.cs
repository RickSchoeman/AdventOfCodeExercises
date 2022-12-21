using Newtonsoft.Json;
using SeaCucumberShuffle.Extensions;
using SeaCucumberShuffle.Interfaces;
using SeaCucumberShuffle.Models;
using SeaCucumberShuffle.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeaCucumberShuffle.Services
{
    public class MovementService : IMovementService
    {
        private readonly ISeaCucumberService _seaCucumberService;
        private readonly SeaFloorRepository _seaFloorRepository = new();

        public MovementService(ISeaCucumberService seaCucumberService)
        {
            _seaCucumberService = seaCucumberService;
        }

        public int DetermineWhenMovementStops()
        {
            var seaFloor = _seaFloorRepository.GetSeaFloor();

            var seaCucumbers = _seaCucumberService.GetAllSeaCucumbers();

            var turn = 0;
            var height = seaFloor.Height;
            var width = seaFloor.Width;
            var stoppedMoving = false;

            do
            {
                seaCucumbers.SetValue(x => x.HasMoved = false);

                var seaCucumbersAfterEastMovement = CalculateMovement(seaCucumbers, height, width, Enums.SeaCucumberType.EastFacing);

                var seaCucumbersAfterSouthMovement = CalculateMovement(seaCucumbersAfterEastMovement, height, width, Enums.SeaCucumberType.SouthFacing);

                seaCucumbers = seaCucumbersAfterSouthMovement;

                stoppedMoving = !seaCucumbers.Any(x => x.HasMoved);

                if (!stoppedMoving)
                {
                    turn++;
                }
            }
            while (!stoppedMoving);

            return turn - 1;
        }

        private List<SeaCucumber> CalculateMovement(List<SeaCucumber> seaCucumbers, int height, int width, Enums.SeaCucumberType typeToCheck)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var selectedSeaCucumber = seaCucumbers.SingleOrDefault(sc => sc.XCoordinate == x + 1 && sc.YCoordinate == y + 1 && sc.Type == typeToCheck);

                    if (selectedSeaCucumber == null)
                    {
                        continue;
                    }

                    if (typeToCheck == Enums.SeaCucumberType.EastFacing && !selectedSeaCucumber.HasMoved)
                    {
                        var newXCoordinate = selectedSeaCucumber.XCoordinate + 1;

                        if (newXCoordinate > width)
                        {
                            newXCoordinate = 1;
                        }

                        if (seaCucumbers.Any(sc => sc.XCoordinate == newXCoordinate && sc.YCoordinate == selectedSeaCucumber.YCoordinate && sc.Type == Enums.SeaCucumberType.Empty))
                        {
                            seaCucumbers.Where(sc => sc.XCoordinate == selectedSeaCucumber.XCoordinate && sc.YCoordinate == selectedSeaCucumber.YCoordinate && sc.Type == typeToCheck).SetValue(sc => sc.Type = Enums.SeaCucumberType.Empty);
                            seaCucumbers.Where(sc => sc.XCoordinate == newXCoordinate && sc.YCoordinate == selectedSeaCucumber.YCoordinate).SetValue(sc => sc.Type = typeToCheck);
                            seaCucumbers.Where(sc => sc.XCoordinate == newXCoordinate && sc.YCoordinate == selectedSeaCucumber.YCoordinate).SetValue(sc => sc.HasMoved = true);
                        }
                    }
                    else if ((typeToCheck == Enums.SeaCucumberType.SouthFacing && !selectedSeaCucumber.HasMoved))
                    {
                        var newYCoordinate = selectedSeaCucumber.YCoordinate + 1;

                        if (newYCoordinate > height)
                        {
                            newYCoordinate = 1;
                        }

                        if (seaCucumbers.Any(sc => sc.XCoordinate == selectedSeaCucumber.XCoordinate && sc.YCoordinate == newYCoordinate && sc.Type == Enums.SeaCucumberType.Empty))
                        {
                            seaCucumbers.Where(sc => sc.XCoordinate == selectedSeaCucumber.XCoordinate && sc.YCoordinate == selectedSeaCucumber.YCoordinate && sc.Type == typeToCheck).SetValue(sc => sc.Type = Enums.SeaCucumberType.Empty);
                            seaCucumbers.Where(sc => sc.XCoordinate == selectedSeaCucumber.XCoordinate && sc.YCoordinate == newYCoordinate).SetValue(sc => sc.Type = typeToCheck);
                            seaCucumbers.Where(sc => sc.XCoordinate == selectedSeaCucumber.XCoordinate && sc.YCoordinate == newYCoordinate).SetValue(sc => sc.HasMoved = true);
                        }
                    }
                }
            }
            return seaCucumbers;
        }
    }
}
