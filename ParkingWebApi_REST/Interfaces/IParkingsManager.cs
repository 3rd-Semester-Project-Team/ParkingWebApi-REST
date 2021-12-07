using System.Collections.Generic;
using WebApi_REST.Models;

namespace WebApi_REST.Managers
{
    /// <summary>
    /// Interface that is inherited and implemented by the EFParkingsManager.
    /// </summary>
    public interface IParkingsManager
    {
        IEnumerable<ParkingSlot> GetAll();
        IEnumerable<ParkingSlot> GetById(int id);
        ParkingSlot AddParkSlot(ParkingSlot parkingSlot);
        IEnumerable<ParkingSlot> GetLatestParkingStatus();
    }
}