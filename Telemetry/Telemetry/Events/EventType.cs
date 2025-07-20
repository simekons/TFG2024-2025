
namespace Telemetry.Events
{
    /// <summary>
    /// All the events are numbered here
    /// </summary>
    public enum EventType
    {
        //basic events
        StartTracking,  //
        StopTracking,   //
        SessionStart,   //
        SessionEnd,     // 
        LeftEq,
        RightEq,
       
        // FPS
        StartGameFPS,    // 
        EnemyAppeared,  //
        ShootBullet,
        EnemyShot,     //
        EndGameFPS,     //
        
        // MEMORY
        StartGameMEMORY,        //
        ButtonAppeared,     //
        ButtonPressed,      //
        MaxSequence,      //      
        EndGameMEMORY,     //
        
        numOfEvents
    }
}
