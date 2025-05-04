using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Common;
using System.Xml.Serialization;
using Telemetry.Persistance;
using Telemetry.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using Telemetry.Events;

namespace Telemetry
{
    public class Tracker : MonoBehaviour
    {
        private static Tracker _instance; // The instance of the tracker system (singleton)
        private IPersistance _persistance; //persistance where the data will be sended
        private ISerialization _serialization; // how the data will be sended

        string mGameID_; //id of the game will be tracked
        Guid mSessionID_; //session id 

        float timeToFlush;
        float timer;

        Tracker(string gametittle, PersistanceType pt, SerializeType st, string fileName = "")
        {
            _serialization = ChooseSerialization(st);
            _persistance = ChoosePersistance(pt, fileName);

            this.mSessionID_ = Guid.NewGuid();
            this.mGameID_ = gametittle;

            timeToFlush = 5; //Once every 5 seconds
            timer = 0;
        }

        private IPersistance ChoosePersistance(PersistanceType pt, string fileName)
        {
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
        private ISerialization ChooseSerialization(SerializeType st)
        {
            switch (st)
            {
                case SerializeType.JSON:
                    return new Telemetry.Serialization.JsonSerializer();
                case SerializeType.YAML:
                    return new YAMLSerializer();
                default:
                    throw new NotImplementedException();
            }

        }
        public static Tracker Instance(string game, PersistanceType pt, SerializeType st, string fileName = "")
        {
            if (_instance == null)
                _instance = new Tracker(game, pt, st, fileName);
            return _instance;
        }
        public static Tracker getInstance()
        {
            return _instance;
        }


        // EVENTOS ------------------------------------------------------------------------

        /*
         * GENERAL: 
         * StartTrackingEvent
         * StopTrackingEvent
         * InitSessionEvent
         * EndSessionEvent
         * 
         * AUDIOMETRY:
         * LeftEqEvent
         * RightEqEvent
         * 
         * FPS: 
         * EnemyAppearedEvent
         * EnemyShotEvent
         * StartGameFPSEvent
         * EndGameFPSEvent
         * 
         * MEMORY:
         * ButtonAppearedEvent
         * ButtonPressedEvent
         * MaximumSequenceEvent
         * StartGameMEMORYEvent
         * EndGameMEMORYEvent
         */

        // GENERAL ----------------------------------------------------
        public void init()
        {
            AddGameEvent(new StartTrackingEvent(mSessionID_, mGameID_));
        }
        public void end()
        {
            AddGameEvent(new StopTrackingEvent());
            _persistance.Flush();
            _persistance.End();
        }
        public void initSession()
        {
            AddGameEvent(new InitSessionEvent());
        }
        public void endSession()
        {
            AddGameEvent(new EndSessionEvent());

        }

        // GENERAL ----------------------------------------------------
        // AUDIOMETRY -------------------------------------------------

        public void leftEq(int[] eq)
        {
            AddGameEvent(new Telemetry.Events.Audiometry.LeftEqEvent(eq));
        }

        public void rightE(int[] eq)
        {
            AddGameEvent(new Telemetry.Events.Audiometry.RightEqEvent(eq));
        }

        // FPS --------------------------------------------------------

        public void enemyAppearEvent(int id, string time)
        {
            AddGameEvent(new Telemetry.Events.FPS.EnemyAppearedEvent(id, time));
        }

        public void enemyShotEvent(int id, string time)
        {
            AddGameEvent(new Telemetry.Events.FPS.EnemyShotEvent(id, time));
        }

        public void startGameFPS()
        {
            AddGameEvent(new Telemetry.Events.FPS.StartGameFPSEvent());
        }

        public void endGameFPS()
        {
            AddGameEvent(new Telemetry.Events.FPS.EndGameFPSEvent());
        }

        // FPS --------------------------------------------------------
        // MEMORY -----------------------------------------------------

        public void buttonAppearEvent(int id, string time)
        {
            AddGameEvent(new Telemetry.Events.MEMORY.ButtonAppearedEvent(id, time));
        }

        public void buttonPressedeEvent(int id ,string time)
        {
            AddGameEvent(new Telemetry.Events.MEMORY.ButtonPressedEvent(id, time));
        }

        public void maxSequenceEvent(int id)
        {
            AddGameEvent(new Telemetry.Events.MEMORY.MaximumSequenceEvent(id));
        }

        public void startGameMemory()
        {
            AddGameEvent(new Telemetry.Events.MEMORY.StartGameMEMORYEvent());
        }

        public void endGameMemory()
        {
            AddGameEvent(new Telemetry.Events.MEMORY.EndGameMEMORYEvent());
        }

        // MEMORY -----------------------------------------------------

        // EVENTOS ------------------------------------------------------------------------  
        
        public void AddGameEvent(Telemetry.Events.Event e)
        {
            e.setSessionID(mSessionID_);
            _persistance.PersistEvent(e);
        }


        public void update(float deltaTime)
        {
            timer += deltaTime;

            if (timer >= timeToFlush)
            {
                _persistance.Flush();
                timer = 0;
            }
        }

    }
}

