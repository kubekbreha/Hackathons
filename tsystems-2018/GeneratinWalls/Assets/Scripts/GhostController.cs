using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    //this is player or reference of him
    public GameObject ghostTarget;
    //this is reference for ghost navemeshagent component
    UnityEngine.AI.NavMeshAgent agent;

    Vector3 zVector;
    public Rigidbody ghost;
    public bool isPlaying = true;

    // Use this for initialization
    void Start()
    {

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        ghostTarget = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            if (ghostTarget == null)
            {
                //Debug.Log("Fuck");
                isPlaying = false;
                return;
            }
            //this is for updating the target location
            agent.destination = ghostTarget.transform.position;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        zVector = new Vector3(0.1f, 0.0f, 0.0f);

        if (collision.gameObject.tag == "Wall")
        {
            ghost.transform.position = zVector;
        }
    }
}
