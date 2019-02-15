using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorControlSystem
{
    public class Elevator
    {
        public Elevator(int id)
        {
            Id = id;
            Riders = new List<Rider>();
        }

        public int Id { get; private set; }
        public int CurrentFloor { get; set; }
        public int DestinationFloor { get; set; }

        public Direction Direction
        {
            get
            {
                return CurrentFloor == 1
                    ? Direction.Up
                    : DestinationFloor > CurrentFloor ? Direction.Up : Direction.Down;
            }
        }

        public List<Rider> Riders { get; set; }
    }
    
}
