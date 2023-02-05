using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerState
{
    Idle,
    Walking,
}

public class ThirdPersonCharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterController character;

    [SerializeField]
    private float speed;
    [SerializeField]
    private RopeBuilder rope;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private LayerMask npcMask;
    [SerializeField]
    private GameObject rootsEffect;
    private Camera mainCamera;

    private GameObject other;


    private PlayerState playerState;

    public PlayerState GetPlayerState()
    {
        return playerState;
    }

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
        //var newPos = character.transform.position + new Vector3(moveDirection.x * speed, 0, moveDirection.z * speed) * Time.deltaTime;
        character.SimpleMove(new Vector3(moveDirection.x * speed, 0, moveDirection.z * speed));
        rope.GetFirstSegment().transform.position = character.transform.position + offset;
    }



    private void OnAttackHandler()
    {
        //sphere cast
        Collider[] colliders = Physics.OverlapSphere(character.transform.position, 1f, npcMask);
        NPCController npc = null;
        //look if we have any npc
        Debug.Log("Detected colliders : " + colliders.Length);
        if (colliders != null && colliders.Length > 0)
        {
            npc = colliders[0].GetComponent<NPCController>();
        }
        //attack the first NPC
        if (npc != null)
        {
            npc.Kill();
            Instantiate(rootsEffect, new Vector3(npc.transform.position.x, rootsEffect.transform.position.y, npc.transform.position.z), Quaternion.identity);
            StartCoroutine(RemoveNPC(npc));
            Debug.Log("Attacking : " + npc);
        }


    }

    private IEnumerator RemoveNPC(NPCController npc)
    {
        yield return new WaitForSeconds(1);
        Destroy(npc.transform.gameObject);
    }


}
