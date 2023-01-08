using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pullable : MonoBehaviour
{
    [SerializeField]
    private UnityEvent pullAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UnityEvent GetPullAction()
    {
        Debug.Log($"Get Force Pull Action");
        return pullAction;
    }
}
