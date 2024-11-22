using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public Transform target;
    public float trackingSpeed = 1f, yOffset;
    private Vector3 thisToTarget;
    void Start()
    {
        if(!target){
            target = GameObject.Find("Player").transform;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target){
            thisToTarget = target.position - transform.position;
            Vector3 correctedVector = new Vector3(thisToTarget.x, thisToTarget.y + yOffset, 0f);
            if(thisToTarget.magnitude > 0.1){
                transform.position += correctedVector * trackingSpeed * Time.unscaledDeltaTime;
            }
        }
    }
}
