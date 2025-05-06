using Telemetry.Events;
using Telemetry.Serialization;

namespace Telemetry.Persistance
{
   
    public interface IPersistance
    {
        /// <returns>where the data is sended <see cref="PersistanceType"/> </returns>
        PersistanceType GetPersistanceType();

        /// <returns>the instance of <see cref="ISerialization"/> </returns>
        ISerialization GetSerialization();
 
        /// <summary>
        /// Puts an Event on the queue to be sended
        /// </summary>
        /// <param name="ev"></param>
        void PersistEvent(Event ev);
        
        /// <summary>
        /// Sends all the events in the pending queue to their media 
        /// </summary>
        void Flush();

        /// <summary>
        /// Closes persistence system 
        /// </summary>
        void End();
    }
}
