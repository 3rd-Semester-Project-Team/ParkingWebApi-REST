using System.Collections.Generic;
using WebApi_REST.Models;

namespace WebApi_REST.Managers
{
    public interface IParkingsManager
    {
        IEnumerable<ParkingSlot> GetAll();
        ParkingSlot GetById(int id);
        ParkingSlot AddParkSlot(ParkingSlot parkingSlot);
    }
}