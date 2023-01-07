using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameConfigScriptableObject gameConfig;
    [SerializeField]
    private Rigidbody body;

    private float verticalAxis;
    private float horizontalAxis;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        verticalAxis = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector3(horizontalAxis, 0, verticalAxis).normalized * gameConfig.PlayerMovementSpeed;
    }
}
