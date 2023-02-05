using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum NPCState
{
    Idle,
    Walk,
    Detected_Player,
    Dead,
}


public class NPCController : MonoBehaviour
{

    [SerializeField]
    private Vector2 changeIntrestTime = new Vector2(20, 30);
    [SerializeField]
    private LayerMask playerMask;
    [SerializeField]
    private GameObject warningIndicator;
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

        if (currentState == NPCState.Detected_Player)
        {
            return;
        }

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

        RaycastHit hit;
        //Raycast Detection
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10, playerMask))
        {

            Debug.Log("Raycast detected player");
            CharacterView view = hit.collider.gameObject.GetComponent<CharacterView>();
            var controller = view.GetCharacterController();
            warningIndicator.SetActive(true);
            StartCoroutine(ReportPlayer(controller));
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
        if (currentState == NPCState.Idle || currentState == NPCState.Walk)
        {
            warningIndicator.SetActive(false);
        }
    }


    private IEnumerator ReportPlayer(ThirdPersonCharacterController controller)
    {
        ChangeState(NPCState.Idle);
        yield return new WaitForSeconds(3);
        if (currentState == NPCState.Dead)
        {
            yield return null;
        }
        if (controller.GetPlayerState() == PlayerState.Walking)
        {
            //player is walking
            //Game over
            Debug.Log("Game Over : Player detected");
        }
        else
        {
            Debug.Log("NPC state : Back to idle");
            ChangeState(NPCState.Idle);
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward * 10);
    }

    public void Kill()
    {
        agent.isStopped = true;
        //Kill Effect
        Debug.Log("Killed NPC");

        ChangeState(NPCState.Dead);
        //Destroy(this.gameObject);
    }
}
