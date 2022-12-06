using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    private Vector3 localUp;

    /*[SerializeField] float rotInterpolation = 0.2f;
    Coroutine rotSlerp;
    private Vector3 currentVector;*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Rotate(Vector3 movement)
    {
        //Find local up vector
        //https://answers.unity.com/questions/46770/rotate-a-vector3-direction.html
        localUp = transform.rotation * Vector3.up;

        //No interpolated rotation
        transform.rotation = Quaternion.LookRotation(movement, localUp);

        /*//Run if new rotation
        if (currentVector != movement)
        {
            currentVector = movement;
            //Find local up vector
            //https://answers.unity.com/questions/46770/rotate-a-vector3-direction.html
            localUp = transform.rotation * Vector3.up;

            //Set rotation to movement direction taking into account local up
            //https://docs.unity3d.com/ScriptReference/Quaternion.LookRotation.html
            Quaternion rot = Quaternion.LookRotation(movement, localUp);
            if (rotSlerp != null)
                StopCoroutine(rotSlerp);
            rotSlerp = StartCoroutine(rotSlow(rot));
            Debug.Log("coroutine started");
        }*/
    }

    /*IEnumerator rotSlow(Quaternion rot)
    {
        float i = 0;
        Debug.Log("coroutine running");
        while (i < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, i);
            i += 0.05f;
            yield return null;
        }

    }*/
}
