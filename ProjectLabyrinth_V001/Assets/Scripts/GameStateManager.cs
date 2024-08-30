using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState {

    public enum GameStateType { Inactive, Starting, Falling, Combat, InMenu};

    public abstract class GameStateManager : MonoBehaviour
    {
    }
}
