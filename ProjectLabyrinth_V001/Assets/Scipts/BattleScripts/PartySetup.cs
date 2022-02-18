using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySetup : MonoBehaviour
{
    public PartyListScriptableObject partyList;
    // Start is called before the first frame update
    void Awake()
    {
        if(this.gameObject.tag == "Ally")
        {
            SetUpAllyParty();
        }
        else if(this.gameObject.tag == "Enemy")
        {
            SetUpEnemyParty();
        }
        else
        {
            Debug.Log("Error");
        }
    }

    private void SetUpAllyParty()
    {
        foreach(GameObject member in partyList.playerParty)
        {
            var partyMember = Instantiate(member, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            partyMember.transform.parent = gameObject.transform;
        }
    }

    private void SetUpEnemyParty()
    {

    }
}
