using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public abstract class GameStateBase //Base class for all gamestates
    {
        public GameStateType ID = GameStateType.Inactive;
        
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
    }
}

