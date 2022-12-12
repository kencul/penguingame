using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour
{
    //References to componenets
    CharacterController controller;
    rotation rotInstance;
    groundNormal GroundNormalInstance;

    //GameObject References
    [SerializeField] GameObject mainCamera;
    delayedMovement camMovmentInstance;

    //Movement Numbers
    private float baseSpeed = 5f;
    private float speedMulti = 0.5f;
    [SerializeField] float gravity = 5f;
    private Vector3 momentum = Vector3.zero;

    private void Awake()
    {
        //Get reference to character controller component
        controller = GetComponent<CharacterController>();
        rotInstance = GetComponent<rotation>();
        camMovmentInstance = mainCamera.GetComponent<delayedMovement>();
        GroundNormalInstance = GetComponent<groundNormal>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0;
        float y = 0;

        //Create movement vector 3 from input axis
        if (StateManager.Instance.GameState == StateManager.State.Play)
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
        }
        Vector3 movement = x * new Vector3(1, 0, 0) + y * new Vector3(0, 0, 1);

        //Make diagonals consistent speed by clamping the magnitude of the movement vector to 1
        //https://forum.unity.com/threads/diagonal-movement-speed-to-fast.271703/
        movement = Vector3.ClampMagnitude(movement, 1f);

        //Find and process Dot Product of movement and ground normal
        float DotProduct = GroundNormalInstance.FindDotProduct(movement);

        DotProduct += 1;

        Vector3 movementVector = baseSpeed * movement * speedMulti * DotProduct;

        movement = movementVector;

        //momentum code that I'll try to do again if I have time
        /*if (Vector3.Magnitude(movementVector) >= Vector3.Magnitude(momentum))
        {
            movement = movementVector;
        }
        else
            movement = momentum;

        //movement = Vector3.ClampMagnitude(movement, baseSpeed * 2);

        //Reduce momentum by the DotProduct, lose more when going uphill then down
        momentum = movement*Mathf.Clamp(DotProduct, 0f, 1f);*/

        //Apply rotation facing movement
        if(movement != Vector3.zero)
        {
            rotInstance.Rotate(movement);
        }

        //Add gravity to movement vector
        movement -= gravity * Vector3.up;
        //Apply movement vector and gravity to char controller
        controller.Move(Time.deltaTime * movement);

        camMovmentInstance.MoveWithDelay(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Finish":
                StateManager.Instance.GameState = StateManager.State.Win;
                break;
            case "Food":
                GameManager.Instance.FoodPickup();
                Destroy(other.transform.parent.gameObject);
                break;
        }
        /*if (other.CompareTag("Finish"))
        {
            Debug.Log("FINISH");
        }*/
    }
}
