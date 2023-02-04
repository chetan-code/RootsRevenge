using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum NPCState
{
    Idle,
    Walk,
    Dead,
}


public class NPCController : MonoBehaviour
{

    [SerializeField]
    private Vector2 changeIntrestTime = new Vector2(20, 30);
    private NavMeshAgent agent;

    private NPCState currentState;
    private float currentTimeToSpend = 0;
    private float timeSpent = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(NPCState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSpent <= currentTimeToSpend)
        {
            timeSpent += Time.deltaTime;
            if (agent.velocity.magnitude <= 0)
            {
                ChangeState(NPCState.Idle);
            }
        }
        else
        {
            //Set a new time and destination
            timeSpent = 0;
            currentTimeToSpend = Random.Range(changeIntrestTime.x, changeIntrestTime.y);
            agent.SetDestination(FindTargetOfInterest());
            ChangeState(NPCState.Walk);
        }
    }


    private Vector3 FindTargetOfInterest()
    {
        float rand = Random.Range(-20, 20);
        Debug.Log("Found a new intrest point" + rand);
        return new Vector3(rand, 0, rand);
    }



    private void ChangeState(NPCState newState)
    {
        if (currentState == newState)
        {
            return;
        }
        Debug.Log("New State : " + newState.ToString());
        currentState = newState;
    }
}
