using System;
using Telemetry.Events;
using Telemetry.Persistance;
using Telemetry.Serialization;

namespace Telemetry
{
    public class Tracker
    {
        private static Tracker _instance; // The instance of the tracker system (singleton)
        private IPersistance _persistance; //persistance where the data will be sended
        private ISerialization _serialization; // how the data will be sended

        string mGameID_; //id of the game will be tracked
        Guid mSessionID_; //session id 

        //variables for the periodic flush (assuming delta time in seconds)
        float timeToFlush;
        float timer;


        Tracker(string gametittle, PersistanceType pt, SerializeType st,string fileName="")
        {
            _serialization = ChooseSerialization(st); 
            _persistance = ChoosePersistance(pt,fileName);

            this.mSessionID_ = Guid.NewGuid();
            this.mGameID_ = gametittle;

            timeToFlush = 5; //Once every 5 seconds
            timer = 0;
        }
        ~Tracker()
        {
            _persistance.Flush();
        }
        private IPersistance ChoosePersistance(PersistanceType pt,string fileName) {
            switch (pt)
            {
                case PersistanceType.File:
                    return new FilePersistance(fileName, _serialization);
                case PersistanceType.Server:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
        private ISerialization ChooseSerialization(SerializeType st) {
            switch (st)
            {
                case SerializeType.JSON:
                    return new JsonSerializer();
                case SerializeType.YAML:
                    return new YAMLSerializer();
                default:
                    throw new NotImplementedException();
            }

        }
        public static Tracker Instance(string game, PersistanceType pt,SerializeType st,string fileName="")
        {
            if (_instance == null)
                _instance = new Tracker(game,pt,st,fileName);
            return _instance;
        }
        public static Tracker getInstance()
        {
            return _instance;
        }
        public void init()
        {
            AddGameEvent(new StartTrackingEvent(mSessionID_, mGameID_));
        }

        public void startGame()
        {
            //AddGameEvent(new StartGameEvent());
        }
        public void initSession()
        {
          AddGameEvent(new InitSessionEvent());
        }
        public void endSession()
        {
            AddGameEvent(new EndSessionEvent());

        }
        public void endGame()
        {
            //AddGameEvent(new EndGameEvent());
        }
        public void AddGameEvent(Event e)
        {
            e.setSessionID(mSessionID_);
            _persistance.PersistEvent(e);
        }
        public void end()
        {
            AddGameEvent(new StopTrackingEvent());
            _persistance.Flush();
            _persistance.End();
        }

        public void update(float deltaTime) {
            timer += deltaTime;

            if (timer >= timeToFlush) {
                _persistance.Flush();
                timer = 0;
            }
        }
    }
}
