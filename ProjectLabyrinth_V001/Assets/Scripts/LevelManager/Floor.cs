using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Floor
{
    //[SerializeField]
    public GameObject _start;
    public GameObject _tp1;

    public void GenerateFloorTPs(GameObject start, GameObject tp1)
    {
        _start = start;
        _tp1 = tp1;
    }

}
