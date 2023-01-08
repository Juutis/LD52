using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private void Awake()
    {
        main = this;
    }
    public static PlayerMovement main;

    [SerializeField]
    private GameConfigScriptableObject gameConfig;
    [SerializeField]
    private Rigidbody body;

    private float verticalAxis;
    private float horizontalAxis;

    [SerializeField]
    private Transform playerModel;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.main.Paused)
        {
            return;
        }
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        verticalAxis = Input.GetAxisRaw("Vertical");

        Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouse, out RaycastHit hitInfo, 1000, LayerMask.GetMask("Ground"));

        if (hitInfo.collider != null)
        {
            var mousePos = hitInfo.point;
            var mouseDir = mousePos - playerModel.position;
            mouseDir.y = 0.0f;
            playerModel.forward = mouseDir;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.main.Paused)
        {
            return;
        }
        body.velocity = new Vector3(horizontalAxis, 0, verticalAxis).normalized * gameConfig.PlayerMovementSpeed;
    }

    public void Teleport(Transform teleportLocation)
    {
        transform.position = Vector.SetY(teleportLocation.position, transform.position.y);
    }
}
