using System.Collections.Generic;
using WebApi_REST.Models;

namespace WebApi_REST.Managers
{
    /// <summary>
    /// Interface that is inherited and implemented by the EFSensorsManager
    /// </summary>
    public interface ISensorsManager
    {
        List<Sensor> GetAll();
        Sensor GetById(int id);
        Sensor Add(Sensor sensor);
        void Delete(int id);
        Sensor Edit(int id, Sensor newSensor);
    }
}