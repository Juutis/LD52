using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomYaw : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(Vector3.up, Random.Range(0, 360));
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
