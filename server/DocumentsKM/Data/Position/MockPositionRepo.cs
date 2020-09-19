using System;
using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    // Repo implementation for testing purposes
    public class MockPositionRepo : IPositionRepo
    {
        // Initial Positions list
        private readonly List<Position> _positions = new List<Position>
        {
            new Position
            {
                Code=1100,
                Name="Test position1",
            },
            new Position
            {
                Code=1185,
                Name="Test position3",
            },
            new Position
            {
                Code=1285,
                Name="Test position2",
            },
        };

        public Position GetPositionByCode(uint code)
        {
            try
            {
                var position = _positions[Convert.ToInt32(code)];
                return position;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }
}
