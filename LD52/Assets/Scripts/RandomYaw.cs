using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomYaw : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var scale = Random.Range(0.9f, 1.1f);
        transform.Rotate(Vector3.up, Random.Range(0, 360));
        transform.localScale = new Vector3(scale, scale, scale);
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
