﻿using SeaCucumberShuffle.Interfaces;
using SeaCucumberShuffle.Models;
using SeaCucumberShuffle.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SeaCucumberShuffle.Services
{
    public class SeaCucumberService : ISeaCucumberService
    {
        private readonly SeaFloorRepository _seaFloorRepository = new();

        public List<SeaCucumber> GetAllSeaCucumbers()
        {
            var seaCucumbers = new List<SeaCucumber>();
            var seaFloor = _seaFloorRepository.GetSeaFloor();

            if (seaFloor.Layout == null)
            {
                return seaCucumbers;
            }

            for (int y = 0; y < seaFloor.Height; y++)
            {
                for (int x = 0; x < seaFloor.Width; x++)
                {
                    var seaCucumberType = DetermineSeaCucumberType(seaFloor.Layout.ElementAt(y)[x]);
                    seaCucumbers.Add(new SeaCucumber
                    {
                        XCoordinate = x + 1,
                        YCoordinate = y + 1,
                        Type = seaCucumberType,
                        HasMoved = false,
                    });
                }
            }

            return seaCucumbers;
        }

        private static Enums.SeaCucumberType DetermineSeaCucumberType(char item) => item switch
        {
            '>' => Enums.SeaCucumberType.EastFacing,
            'v' => Enums.SeaCucumberType.SouthFacing,
            '.' => Enums.SeaCucumberType.Empty,
            _ => Enums.SeaCucumberType.Empty,
        };
    }
}