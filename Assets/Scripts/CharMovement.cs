using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour
{
    CharacterController controller;
    rotation rotInstance;

    [SerializeField] GameObject mainCamera;
    delayedMovement camMovmentInstance;

    private float baseSpeed = 5f;
    private float speedMulti = 0.5f;
    [SerializeField] float gravity = 5f;

    private void Awake()
    {
        //Get reference to character controller component
        controller = GetComponent<CharacterController>();
        rotInstance = GetComponent<rotation>();
        camMovmentInstance = mainCamera.GetComponent<delayedMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Create movement vector 3 from input axis
        //NNED TO MAKE DIAGONALS CONSISTENT SPEED
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movement = x * new Vector3(1, 0, 0) + y * new Vector3(0, 0, 1);
        movement *= baseSpeed * speedMulti;

        //Apply rotation facing movement
        if(movement != Vector3.zero)
        {
            //Debug.Log("sending rota");
            rotInstance.Rotate(movement);
        }

        //Add gravity to movement vector
        movement -= gravity * Vector3.up;
        //Apply movement vector and gravity to char controller
        controller.Move(Time.deltaTime * movement);

        camMovmentInstance.MoveWithDelay(transform.position);
    }
}
