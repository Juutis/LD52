using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pullable : MonoBehaviour
{
    [SerializeField]
    private UnityEvent pullAction;

    [SerializeField]
    private Transform hilight;

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

    public void Highlight()
    {
        hilight.gameObject.SetActive(true);
    }

    public void Unhighlight()
    {
        hilight.gameObject.SetActive(false);
    }
}
