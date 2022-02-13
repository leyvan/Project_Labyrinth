using System.Collections;
using UnityEngine;


//Using Wander Code + Follow Player Code
public class AI_BehaviourV2 : MonoBehaviour
{
    public float speed = 1f;
    public float obstacleRange = 0.2f;
    public float turnSpeed = 0.2f;      
    public GameObject player;       
    public GameObject alert;        //Red Alert Text Display
    public int interval = 11;

    public bool _Found = false;
    void Update()
    {
        if(_Found == false)
        {
            transform.Translate(0, 0, speed * Time.deltaTime);

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray, 1.0f, out hit))
            {
                if (hit.distance < obstacleRange)
                {
                    float angle = Random.Range(-110, 110);
                    //float smoothRotation = angle * turnSpeed;
                    transform.Rotate(0, angle, 0);
                }       
            }
        }

        if(_Found == true)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            alert.SetActive(true);
            Debug.Log("Enemy detected!");
            _Found = true;
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            alert.SetActive(false);
            Debug.Log("Enemy out of range.");
            _Found = false;
        }
    }

    //Not Really working

    /*
    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player" && Time.frameCount % interval == 0)
        {
            alert.SetActive(false);
            _Found = false;
            Debug.Log("Got Bored!");
        }
    }
    */

}

