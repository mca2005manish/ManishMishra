using System;
using ElevatorControlSystem;

namespace TestElevatorSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Feel free to change these inputs
            const int numberOfFloors = 13;//-2,-1,0,1,2,3,4,5,6,7,8,9,10
            const int numberOfElevators = 3;
            const int numberOfRequests = 13 * 39; // You may run into a memory limit if this is too high

           
            var pickupCount = 0;
            var stepCount = 0;
            var random = new Random();
            IElevatorControlSystem system = new ControlSystem(numberOfElevators);


            while (pickupCount < numberOfRequests)
            {
                var originatingFloor = random.Next(1, numberOfFloors + 1);
                var destinationFloor = random.Next(1, numberOfFloors + 1);
                if (originatingFloor != destinationFloor)
                {
                    system.Pickup(originatingFloor, destinationFloor);
                    pickupCount++;
                }
            }

            while (system.AnyOutstandingPickups())
            {
                system.Step();
                stepCount++;
            }

            Console.WriteLine("Transported {0} elevator riders to their requested destinations in {1} steps.", pickupCount, stepCount);
            Console.ReadLine();
        }
    }
}
