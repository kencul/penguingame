using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMovement : MonoBehaviour
{
    [SerializeField] float RotationSpeed = 50.0f;
    private float DistanceBelow = 0f;
    [SerializeField] int VertSpeed = 500;
    [SerializeField] float VertDistance = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        //Save the spawn location
        Vector3 StartingLocation = transform.position;

        //Check the distance to floor below, destroy this object if no floor within 10 units
        if (Physics.Raycast(StartingLocation, -Vector3.up, out RaycastHit hit, 10))
            DistanceBelow = hit.distance;
        else
            Destroy(this);

        //Shift down food to be 0.5 units above the floor
        transform.position = new Vector3(StartingLocation.x, StartingLocation.y - DistanceBelow + 0.5f, StartingLocation.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Vertical Sinusoidal Movement
        //https://answers.unity.com/questions/781748/using-mathfsin-to-move-an-object.html
        Vector3 NewPosition = transform.position;
        NewPosition.y += Mathf.Sin(Time.time * VertSpeed) * VertDistance;
        transform.position = NewPosition;

        //Constant rotation
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0, Space.World);
    }
}
