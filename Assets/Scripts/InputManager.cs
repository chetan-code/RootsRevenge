using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public float Horizontal;
    public float Vertical;

    public UnityEvent OnAttack;


    public void Awake()
    {
        Instance = this;
    }

    public void Update()
    {

        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            OnAttack.Invoke();
        }
    }



}


