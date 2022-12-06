using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delayedMovement : MonoBehaviour
{
    //private float posLerp = 0.2f;
    [SerializeField] float movementDelay = 0.1f;
    WaitForSeconds enumDelay;

    // Start is called before the first frame update
    void Start()
    {
        enumDelay = new WaitForSeconds(movementDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveWithDelay(Vector3 pos)
    {
        StartCoroutine(delayedMove(pos + new Vector3(0, 3.5f, -4.75f)));
    }

    IEnumerator delayedMove (Vector3 pos)
    {
        yield return enumDelay;
        transform.position = pos;
    }
}
