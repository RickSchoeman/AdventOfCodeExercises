using SeaCucumberShuffle.Extensions;
using SeaCucumberShuffle.Interfaces;
using SeaCucumberShuffle.Models;
using SeaCucumberShuffle.Repositories;

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
                seaCucumbers.SetValue(x => x.IsAllowedToMove = false);

                var seaCucumbersAfterEastMovement = CalculateMovement(seaCucumbers, height, width, Enums.SeaCucumberType.EastFacing);

                var seaCucumbersAfterSouthMovement = CalculateMovement(seaCucumbersAfterEastMovement, height, width, Enums.SeaCucumberType.SouthFacing);

                seaCucumbers = seaCucumbersAfterSouthMovement;

                stoppedMoving = !seaCucumbers.Any(x => x.IsAllowedToMove);

                if (!stoppedMoving)
                {
                    turn++;
                }
            }
            while (!stoppedMoving);

            return turn - 1;
        }


        public int DifferentCalculation()
        {
            var seaFloor = _seaFloorRepository.GetSeaFloor();
            int height = seaFloor.Height;
            int width = seaFloor.Width;
            var seaCucumbers = _seaCucumberService.GetAllSeaCucumbers();

            var isAllowedToMove = true;
            var turn = 0;
            do
            {
                var cucumbersAfterTurn = new List<SeaCucumber>();
                //Set all the EAST ones that may move to IsAllowedToMove = true
                foreach (var column in GetRow(seaCucumbers, height))
                {
                    foreach (var seaCucumber in GetCucumbersFromRow(column))
                    {
                        if (seaCucumber.Type != Enums.SeaCucumberType.EastFacing)
                        {
                            cucumbersAfterTurn.Add(seaCucumber);
                            continue;
                        }

                        int destinationCol = (seaCucumber.XCoordinate + 1) <= height ? (seaCucumber.XCoordinate + 1) : 1;
                        int destinationRow = seaCucumber.YCoordinate;
                        var destinationCucumber = GetCucumber(column, destinationRow, destinationCol);

                        if (destinationCucumber.Type == Enums.SeaCucumberType.Empty)
                        {
                            seaCucumber.IsAllowedToMove = true;
                        }
                        cucumbersAfterTurn.Add(seaCucumber);
                    }
                }

                //Set all the SOUTH ones that may move to IsAllowedToMove = true
                foreach (var column in GetColumn(seaCucumbers, width))
                {
                    foreach (var seaCucumber in GetCucumbersFromColumn(column))
                    {
                        if (seaCucumber.Type != Enums.SeaCucumberType.SouthFacing)
                        {
                            cucumbersAfterTurn.Add(seaCucumber);
                            continue;
                        }

                        int destinationCol = seaCucumber.XCoordinate;
                        int destinationRow = (seaCucumber.YCoordinate + 1) <= height ? (seaCucumber.YCoordinate + 1) : 1;
                        var destinationCucumber = GetCucumber(column, destinationRow, destinationCol);

                        if (destinationCucumber.Type == Enums.SeaCucumberType.Empty)
                        {
                            seaCucumber.IsAllowedToMove = true;
                        }
                        cucumbersAfterTurn.Add(seaCucumber);
                    }
                }

                isAllowedToMove = cucumbersAfterTurn.Any(x => x.IsAllowedToMove);

                var cucumbersAfterMove = new List<SeaCucumber>();

                //Move all the EAST ones that IsAllowedToMove = true
                foreach (var column in GetRow(cucumbersAfterTurn, height))
                {
                    foreach (var seaCucumber in GetCucumbersFromRow(column))
                    {
                        if (seaCucumber.Type != Enums.SeaCucumberType.EastFacing)
                        {
                            cucumbersAfterMove.Add(seaCucumber);
                            continue;
                        }

                        int destinationCol = (seaCucumber.XCoordinate + 1) <= height ? (seaCucumber.XCoordinate + 1) : 1;
                        int destinationRow = seaCucumber.YCoordinate;
                        var destinationCucumber = GetCucumber(column, destinationRow, destinationCol);

                        seaCucumber.Type = Enums.SeaCucumberType.Empty;
                        seaCucumber.IsAllowedToMove = false;
                        destinationCucumber.Type = Enums.SeaCucumberType.EastFacing;

                        cucumbersAfterMove.Add(destinationCucumber);
                        cucumbersAfterMove.Add(seaCucumber);
                    }
                }

                //Move all the SOUTH ones that IsAllowedToMove = true
                foreach (var column in GetColumn(cucumbersAfterTurn, width))
                {
                    foreach (var seaCucumber in GetCucumbersFromColumn(column))
                    {
                        if (seaCucumber.Type != Enums.SeaCucumberType.SouthFacing)
                        {
                            cucumbersAfterMove.Add(seaCucumber);
                            continue;
                        }

                        int destinationCol = seaCucumber.XCoordinate;
                        int destinationRow = (seaCucumber.YCoordinate + 1) <= height ? (seaCucumber.YCoordinate + 1) : 1;
                        var destinationCucumber = GetCucumber(column, destinationRow, destinationCol);

                        seaCucumber.Type = Enums.SeaCucumberType.Empty;
                        seaCucumber.IsAllowedToMove = false;
                        destinationCucumber.Type = Enums.SeaCucumberType.SouthFacing;

                        cucumbersAfterMove.Add(destinationCucumber);
                        cucumbersAfterMove.Add(seaCucumber);
                    }
                }

                seaCucumbers = cucumbersAfterMove;
                turn++;
            }
            while (isAllowedToMove);

            return turn;
        }

        private static SeaCucumber GetCucumber(List<SeaCucumber> seaCucumbers, int row, int column) => seaCucumbers.Single(x => x.YCoordinate == row && x.XCoordinate == column);

        private static IEnumerable<List<SeaCucumber>> GetRow(List<SeaCucumber> allCucumbers, int height)
        {
            for(var i = 1; i <= height; i++)
            {
                yield return allCucumbers.Where(x => x.YCoordinate == i).ToList();
            }
        }

        private static IEnumerable<SeaCucumber> GetCucumbersFromRow(List<SeaCucumber> seaCucumbers)
        {
            var length = seaCucumbers.Count;
            for (var i = 1; i <= length; i++)
            {
                yield return seaCucumbers.Single(x => x.XCoordinate == i);
            }
        }

        private static IEnumerable<List<SeaCucumber>> GetColumn(List<SeaCucumber> allCucumbers, int width)
        {
            for (var i = 1; i <= width; i++)
            {
                yield return allCucumbers.Where(x => x.XCoordinate == i).ToList();
            }
        }

        private static IEnumerable<SeaCucumber> GetCucumbersFromColumn(List<SeaCucumber> seaCucumbers)
        {
            var length = seaCucumbers.Count;
            for (var i = 1; i <= length; i++)
            {
                yield return seaCucumbers.Single(x => x.YCoordinate == i);
            }
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

                    if (typeToCheck == Enums.SeaCucumberType.EastFacing && !selectedSeaCucumber.IsAllowedToMove)
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
                            seaCucumbers.Where(sc => sc.XCoordinate == newXCoordinate && sc.YCoordinate == selectedSeaCucumber.YCoordinate).SetValue(sc => sc.IsAllowedToMove = true);
                        }
                    }
                    else if ((typeToCheck == Enums.SeaCucumberType.SouthFacing && !selectedSeaCucumber.IsAllowedToMove))
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
                            seaCucumbers.Where(sc => sc.XCoordinate == selectedSeaCucumber.XCoordinate && sc.YCoordinate == newYCoordinate).SetValue(sc => sc.IsAllowedToMove = true);
                        }
                    }
                }
            }
            return seaCucumbers;
        }
    }
}