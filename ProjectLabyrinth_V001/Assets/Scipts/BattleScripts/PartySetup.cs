using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySetup : MonoBehaviour
{
    public PartyListScriptableObject partyList;
    private List<Transform> enemySpawns = new List<Transform>();
    // Start is called before the first frame update
    void Awake()
    {
        var thisObjTag = this.gameObject.tag;

        if (thisObjTag == "Ally")
        {
            SetUpAllyParty();
        }
        else if (thisObjTag == "EnemyParty")
        {
            GetEnemySpawns();
        }
        else
        {
            Debug.Log("Error");
        }
    }

    private void SetUpAllyParty()
    {
        foreach (GameObject member in partyList.playerParty)
        {
            var partyMember = Instantiate(member, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            partyMember.transform.parent = gameObject.transform;
        }
    }

    void GetEnemySpawns()
    {
        foreach(Transform child in this.transform)
        {
            enemySpawns.Add(child);
        }

        if(partyList.enemyParty.Count > 0)
        {
            SetUpEnemyParty();
        }
        
    }

    void SetUpEnemyParty()
    {
        var rand = Random.Range(1, 4);
        var offSetZ = 0;
        //var healthBarOffset = 0.9f;
        Transform nextEnemySpawn;
        for (var i=0; i < rand; i++)
        {
            nextEnemySpawn = enemySpawns[i];
            nextEnemySpawn.position = new Vector3(enemySpawns[i].position.x, enemySpawns[i].position.y, enemySpawns[i].position.z + offSetZ);
            var partyMember = Instantiate(partyList.enemyParty[i],nextEnemySpawn.position, nextEnemySpawn.rotation);
            partyMember.transform.parent = gameObject.transform;


            offSetZ += 2;
        }
    }

}
