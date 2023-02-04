using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterController character;
    [SerializeField]
    private float speed;
    private Camera mainCamera;

    private GameObject other;



    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Move();

    }


    private void Move()
    {
        float x = InputManager.Instance.Horizontal;
        float y = InputManager.Instance.Vertical;

        Vector3 cameraForward = new(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z);
        Vector3 cameraRight = new(mainCamera.transform.right.x, 0, mainCamera.transform.right.z);

        Vector3 moveDirection = cameraForward.normalized * y + cameraRight.normalized * x;
        character.SimpleMove(new Vector3(moveDirection.x * speed, 0, moveDirection.z * speed));
    }


}
