using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundNormal : MonoBehaviour
{
    private CharacterController CharCont;
    private float DistToGround;
    private Vector3 NormalVector = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        CharCont = GetComponent<CharacterController>();
        DistToGround = CharCont.height - CharCont.center.y + CharCont.skinWidth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {
    }

    public float FindDotProduct(Vector3 movement)
    {
        //https://answers.unity.com/questions/358376/whats-a-good-way-to-find-the-ground-normal.html
        //Raycast right under GO, and find and return normal vector
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, DistToGround))
            NormalVector = hit.normal;

        return Vector3.Dot(movement, NormalVector);
    }
}
