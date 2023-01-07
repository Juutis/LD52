using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseIndicator : MonoBehaviour
{
    private Vector2 mousePos = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log(mouse.direction);
        Physics.Raycast(mouse, out RaycastHit hitInfo, 1000, LayerMask.GetMask("Ground"));

        if (hitInfo.collider != null)
        {
            Debug.Log(hitInfo.point);
            transform.position = hitInfo.point;
        }
    }
}
