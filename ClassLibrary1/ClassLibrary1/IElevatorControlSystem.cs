using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorControlSystem
{
   public interface IElevatorControlSystem
    {
        Elevator GetStatus(int elevatorId);
        void Update(int elevatorId, int floorNumber, int goalFloorNumber);
        void Pickup(int pickupFloor, int destinationFloor);
        void Step();
        bool AnyOutstandingPickups();
    }
}
