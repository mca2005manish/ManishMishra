using System;
using System.Collections.Generic;
using System.Linq;

namespace ElevatorControlSystem
{
    public class ControlSystem : IElevatorControlSystem
    {
        public List<Elevator> Elevators { get; set; }
        public List<Rider> WaitingRiders { get; set; }

        /// <summary>
        /// Constructor to set elevator as per the passing elevator count (As per the question : we will pass 3 elevator – E1, E2 & E3)
        /// </summary>
        /// <param name="numberOfElevators"></param>
        public ControlSystem(int numberOfElevators)
        {
            Elevators = Enumerable.Range(0, numberOfElevators).Select(eid => new Elevator(eid)).ToList();
            WaitingRiders = new List<Rider>();
        }

        /// <summary>
        /// Get Elevator Status
        /// </summary>
        /// <param name="elevatorId"></param>
        /// <returns></returns>
        public Elevator GetStatus(int elevatorId)
        {
            return Elevators.First(e => e.Id == elevatorId);
        }

        /// <summary>
        /// This method will update the elevator & destination floor status
        /// </summary>
        /// <param name="elevatorId"></param>
        /// <param name="floorNumber"></param>
        /// <param name="goalFloorNumber"></param>
        public void Update(int elevatorId, int floorNumber, int goalFloorNumber)
        {
            UpdateElevator(elevatorId, e =>
            {
                e.CurrentFloor = floorNumber;
                e.DestinationFloor = goalFloorNumber;
            });
        }

        /// <summary>
        /// This method will help to get pickup status
        /// </summary>
        /// <param name="pickupFloor"></param>
        /// <param name="destinationFloor"></param>
        public void Pickup(int pickupFloor, int destinationFloor)
        {
            WaitingRiders.Add(new Rider(pickupFloor, destinationFloor));
        }
        /// <summary>
        /// This method will update the elevator status
        /// </summary>
        /// <param name="elevatorId"></param>
        /// <param name="update"></param>
        private void UpdateElevator(int elevatorId, Action<Elevator> update)
        {
            Elevators = Elevators.Select(e =>
            {
                if (e.Id == elevatorId) update(e);
                return e;
            }).ToList();
        }

        /// <summary>
        /// This method includes the implemented algorithm to handle the elevators actual problem.
        /// </summary>
        public void Step()
        {
            var busyElevatorIds = new List<int>();
            // unload elevators
            Elevators = Elevators.Select(e =>
            {
                var disembarkingRiders = e.Riders.Where(r => r.DestinationFloor == e.CurrentFloor).ToList();
                if (disembarkingRiders.Any())
                {
                    busyElevatorIds.Add(e.Id);
                    e.Riders = e.Riders.Where(r => r.DestinationFloor != e.CurrentFloor).ToList();
                }

                return e;
            }).ToList();

            // Embark passengers to available elevators
            WaitingRiders.GroupBy(r => new { r.OriginatingFloor, r.Direction }).ToList().ForEach(waitingFloor =>
            {
                var availableElevator =
                    Elevators.FirstOrDefault(
                        e =>
                            e.CurrentFloor == waitingFloor.Key.OriginatingFloor &&
                            (e.Direction == waitingFloor.Key.Direction || !e.Riders.Any()));
                if (availableElevator != null)
                {
                    busyElevatorIds.Add(availableElevator.Id);
                    var embarkingPassengers = waitingFloor.ToList();
                    UpdateElevator(availableElevator.Id, e => e.Riders.AddRange(embarkingPassengers));
                    WaitingRiders = WaitingRiders.Where(r => embarkingPassengers.All(er => er.Id != r.Id)).ToList();
                }
            });


            Elevators.ForEach(e =>
            {
                var isBusy = busyElevatorIds.Contains(e.Id);
                int destinationFloor;
                if (e.Riders.Any())
                {
                    var closestDestinationFloor =
                        e.Riders.OrderBy(r => Math.Abs(r.DestinationFloor - e.CurrentFloor))
                            .First()
                            .DestinationFloor;
                    destinationFloor = closestDestinationFloor;
                }
                else if (e.DestinationFloor == e.CurrentFloor && WaitingRiders.Any())
                {
                    // Lots of optimization could be done here, perhaps?
                    destinationFloor = WaitingRiders.GroupBy(r => new { r.OriginatingFloor }).OrderBy(g => g.Count()).First().Key.OriginatingFloor;
                }
                else
                {
                    destinationFloor = e.DestinationFloor;
                }

                var floorNumber = isBusy
                    ? e.CurrentFloor
                    : e.CurrentFloor + (destinationFloor > e.CurrentFloor ? 1 : -1);

                Update(e.Id, floorNumber, destinationFloor);
            });
        }

        public bool AnyOutstandingPickups()
        {
            return WaitingRiders.Any();
        }
    }

}
