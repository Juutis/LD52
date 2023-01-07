using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseIndicator : MonoBehaviour
{
    [SerializeField]
    private Material NormalCursor;

    [SerializeField]
    private Material BlinkCursor;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Transform player;

    private bool blinkMode = false;
    private Vector3 mousePos = Vector3.zero;
    private LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        // Cursor.visible = false;
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouse, out RaycastHit hitInfo, 1000, LayerMask.GetMask("Ground"));
        var mouseDir = PlayerToMouseDir();

        if (hitInfo.collider != null)
        {
            mousePos = hitInfo.point;

            if (!blinkMode)
            {
                transform.position = new Vector3(mousePos.x, 0.1f, mousePos.z);
            }
            else
            {
                Physics.Raycast(player.position, mouseDir, out RaycastHit blinkHitInfo, PlayerToMouse2D().magnitude, LayerMask.GetMask("Obstacles"));

                if (blinkHitInfo.collider != null)
                {
                    Vector3 cursorPos = blinkHitInfo.point - mouseDir;
                    transform.position = new Vector3(cursorPos.x, 0.1f, cursorPos.z);
                }
                else
                {
                    transform.position = new Vector3(mousePos.x, 0.1f, mousePos.z);
                }
            }
        }

        Vector3 lineStart = player.position + mouseDir;
        Vector3 lineEnd = transform.position - mouseDir * 0.75f;
        line.SetPosition(0, new Vector3(lineStart.x, 0.1f, lineStart.z));
        line.SetPosition(1, new Vector3(lineEnd.x, 0.1f, lineEnd.z));
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            blinkMode = true;
            meshRenderer.sharedMaterial = BlinkCursor;
        }
        else
        {
            blinkMode = false;
            meshRenderer.sharedMaterial = NormalCursor;
        }
    }

    private Vector2 PlayerToMouse2D()
    {
        var playerPos2 = new Vector2(player.position.x, player.position.z);
        var ownPos2 = new Vector2(mousePos.x, mousePos.z);
        return ownPos2 - playerPos2;
    }

    private Vector3 PlayerToMouseDir()
    {
        var v2Dir = PlayerToMouse2D().normalized;
        return new(v2Dir.x, 0.1f, v2Dir.y);
    }
}
