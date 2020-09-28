using System;
using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    // Мок репозитория для тестирования
    public class MockPositionRepo : IPositionRepo
    {
        // Начальные значения
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

        public Position GetByCode(int code)
        {
            foreach (Position pos in _positions)
            {
                if (pos.Code == code)
                    return pos;
            }
            return null;
        }

        public bool SaveChanges()
        {
            return true;
        }
    }
}
