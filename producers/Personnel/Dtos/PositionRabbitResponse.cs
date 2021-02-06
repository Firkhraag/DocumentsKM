
using Personnel.Models;

namespace Personnel.Dtos
{
    public class PositionRabbitResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public PositionRabbitResponse(Position position)
        {
            Id = position.Id;
            Name = position.ShortName;
        }
    }
}
