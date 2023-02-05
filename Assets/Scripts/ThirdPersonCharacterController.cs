using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;


public enum PlayerState
{
    Idle,
    Walking,
}

public class ThirdPersonCharacterController : MonoBehaviour
{
    [SerializeField]
    private Transform character;
    [SerializeField]
    private float speed;
    [SerializeField]
    private RopeBuilder rope;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private LayerMask npcMask;
    private Camera mainCamera;

    private GameObject other;


    private PlayerState playerState;


    private void Start()
    {
        mainCamera = Camera.main;
        InputManager.Instance.OnAttack.AddListener(OnAttackHandler);
        playerState = PlayerState.Idle;
    }

    private void Update()
    {
        float x = InputManager.Instance.Horizontal;
        float y = InputManager.Instance.Vertical;
        Move(x, y);
        if (Mathf.Abs(x) > 0.1 || Mathf.Abs(y) > 0.1)
        {
            playerState = PlayerState.Walking;
        }
        else
        {
            playerState = PlayerState.Idle;
        }
    }


    private void Move(float x, float y)
    {

        Vector3 cameraForward = new(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z);
        Vector3 cameraRight = new(mainCamera.transform.right.x, 0, mainCamera.transform.right.z);

        Vector3 moveDirection = cameraForward.normalized * y + cameraRight.normalized * x;
        character.transform.position += new Vector3(moveDirection.x * speed, 0, moveDirection.z * speed) * Time.deltaTime;
        //character.SimpleMove(new Vector3(moveDirection.x * speed, 0, moveDirection.z * speed));
        rope.GetFirstSegment().transform.position = character.transform.position + offset;
    }



    private void OnAttackHandler()
    {
        //sphere cast
        Collider[] colliders = Physics.OverlapSphere(character.position, 1, npcMask);
        NPCController npc = null;
        //look if we have any npc
        Debug.Log("Detected colliders : " + colliders.Length);
        if (colliders != null && colliders.Length > 0)
        {
            npc = colliders[0].GetComponent<NPCController>();
        }
        //attack the first NPC
        if (npc != null) { npc.Kill(); }

        Debug.Log("Attacking : " + npc);
    }

}
