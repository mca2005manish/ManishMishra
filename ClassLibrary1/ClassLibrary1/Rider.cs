using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorControlSystem
{
    public class Rider
    {
        public Rider(int originatingFloor, int destinationFloor)
        {
            OriginatingFloor = originatingFloor;
            DestinationFloor = destinationFloor;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public int OriginatingFloor { get; private set; }
        public int DestinationFloor { get; private set; }

        public Direction Direction
        {
            get { return OriginatingFloor < DestinationFloor ? Direction.Up : Direction.Down; }
        }
    }
}
