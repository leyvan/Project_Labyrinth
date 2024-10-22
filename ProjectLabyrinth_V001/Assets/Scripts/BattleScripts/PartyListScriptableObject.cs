using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Party List", menuName = "Party", order = 4)]
public class PartyListScriptableObject : ScriptableObject
{
    public List<GameObject> playerParty = new List<GameObject>();
    public List<GameObject> enemyParty = new List<GameObject>();
}
