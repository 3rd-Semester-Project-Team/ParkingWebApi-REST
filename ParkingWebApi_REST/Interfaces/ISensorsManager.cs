using System.Collections.Generic;
using WebApi_REST.Models;

namespace WebApi_REST.Managers
{
    public interface ISensorsManager
    {
        List<Sensor> GetAll();
        Sensor GetById(int id);
        Sensor Add(Sensor sensor);
        void Delete(int id);
        Sensor Edit(int id, Sensor newSensor);
    }
}