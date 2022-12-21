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

            var steps = 1;
            var allHaveStopped = false;

/*            var eastFacingSeaCucumbers = seaCucumbers.Select(x => x.Type == Enums.SeaCucumberType.EastFacing).ToList();
            var southFacingSeaCucumbers = seaCucumbers.Select(x => x.Type == Enums.SeaCucumberType.SouthFacing).ToList();
            var emptySpaces = seaCucumbers.Select(x => x.Type == Enums.SeaCucumberType.Empty).ToList();*/

            do
            {
                var eastFacingSeaCucumbers = seaCucumbers.Where(x => x.Type == Enums.SeaCucumberType.EastFacing).ToList();
                var movementEast = Movement(seaCucumbers, eastFacingSeaCucumbers, seaFloor.Width, seaFloor.Height).ToList();



                var southFacingSeaCucumbers = seaCucumbers.Where(x => x.Type == Enums.SeaCucumberType.SouthFacing).ToList();
                var emptySpaces = seaCucumbers.Where(x => x.Type == Enums.SeaCucumberType.Empty).ToList();

                var movedEast = movementEast.Except(seaCucumbers).ToList();

                var amountMovedEast = movedEast.Count();
                foreach(var moved in movedEast)
                {
                    var changedSeaCucumber = seaCucumbers.Where(x => x.XCoordinate == moved.XCoordinate - 1 && x.YCoordinate == moved.YCoordinate);
                    seaCucumbers.Add(moved);
                }

                var notMovedEast = seaCucumbers.Intersect(movementEast);

                if (!seaCucumbers.Any(x => x.HasMoved))
                {
                    allHaveStopped = true;
                }
            }
            while(!allHaveStopped);

            return steps;
        }

        public int NewMethod()
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


        public List<SeaCucumber> Movement(List<SeaCucumber> allSeaCucumbers, List<SeaCucumber> movingSeaCucumbers, int width, int height)
        {
            var newList = new List<SeaCucumber>();

            foreach(var movingSeaCucumber in movingSeaCucumbers)
            {
                var newSeaCucumberLocation = DetermineNewSeaCucumberLocation(allSeaCucumbers, movingSeaCucumber, width, height);
                if(JsonConvert.SerializeObject(newSeaCucumberLocation.oldPosition) == JsonConvert.SerializeObject(newSeaCucumberLocation.newPosition))
                {
                    newList.Add(newSeaCucumberLocation.oldPosition);
                }
                else
                {
                    newList.Add(newSeaCucumberLocation.oldPosition);
                    newList.Add(newSeaCucumberLocation.newPosition);
                }
            }

            return newList;
        }

        private static (SeaCucumber oldPosition, SeaCucumber newPosition)  DetermineNewSeaCucumberLocation(List<SeaCucumber> allSeaCucumbers, SeaCucumber seaCucumber, int width, int height)
        {
            switch (seaCucumber.Type)
            {
                case Enums.SeaCucumberType.EastFacing:
                    var originalEastSeaCucumber = seaCucumber;
                    var eastFacingCoordinates = (x: seaCucumber.XCoordinate, y: seaCucumber.YCoordinate);

                    if (eastFacingCoordinates.x == width)
                    {
                        eastFacingCoordinates.x = 1;
                    }
                    else
                    {
                        eastFacingCoordinates.x++;
                    }

                    if (allSeaCucumbers.Any(x => 
                    (x.XCoordinate == eastFacingCoordinates.x && x.YCoordinate == eastFacingCoordinates.y) && 
                    (x.Type == Enums.SeaCucumberType.EastFacing || x.Type == Enums.SeaCucumberType.SouthFacing)))
                    {
                        seaCucumber.HasMoved = false;
                        return (originalEastSeaCucumber, seaCucumber);
                    }

                    originalEastSeaCucumber.Type = Enums.SeaCucumberType.Empty;
                    seaCucumber.XCoordinate = eastFacingCoordinates.x;
                    seaCucumber.HasMoved = true;
                    return (originalEastSeaCucumber, seaCucumber);
                case Enums.SeaCucumberType.SouthFacing:
                    var originalSouthSeaCucumber = seaCucumber;
                    var southFacingCoordinates = (x: seaCucumber.XCoordinate, y: seaCucumber.YCoordinate);

                    if(southFacingCoordinates.y == height)
                    {
                        southFacingCoordinates.y = 1;
                    }
                    else
                    {
                        southFacingCoordinates.y++;
                    }

                    if (allSeaCucumbers.Any(x => (x.YCoordinate == southFacingCoordinates.y && x.XCoordinate == southFacingCoordinates.x) &&
                    (x.Type == Enums.SeaCucumberType.EastFacing || x.Type == Enums.SeaCucumberType.SouthFacing)))
                    {
                        seaCucumber.HasMoved = false;
                        return (originalSouthSeaCucumber, seaCucumber);
                    }

                    originalSouthSeaCucumber.Type = Enums.SeaCucumberType.Empty;
                    seaCucumber.YCoordinate = southFacingCoordinates.y;
                    seaCucumber.HasMoved = true;
                    return (originalSouthSeaCucumber, seaCucumber);
                case Enums.SeaCucumberType.Empty:
                    return (seaCucumber, seaCucumber);
                default:
                    return (seaCucumber, seaCucumber);
            }
        }
    }
}
