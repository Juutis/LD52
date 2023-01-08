using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePullToMove : MonoBehaviour
{
    [SerializeField]
    private float pullAmount;
    [SerializeField]
    private float pullSpeed;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float lerpAmount = 0;
    private bool pulling = false;

    [SerializeField]
    private List<Transform> wheels;

    private float dir = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (pulling)
        {
            lerpAmount += pullSpeed * Time.deltaTime;
            transform.position = Vector.V2to3(Vector2.Lerp(startPosition, targetPosition, lerpAmount), transform.position.y);
            rotateWheels(pullSpeed * Time.deltaTime);

            if (lerpAmount >= 1)
            {
                pulling = false;
            }
        }
    }

    public void PullAction()
    {
        // Move object forward or backward depending on player's relative location
        Vector2 towardsPlayer = Vector.Substract(PlayerMovement.main.transform.position, transform.position).normalized;

        Vector2 forwards = Vector.V3to2(transform.forward);
        Vector2 moveDir = forwards.normalized * (Vector2.Dot(towardsPlayer, forwards) / forwards.magnitude);

        lerpAmount = 0;
        startPosition = Vector.V3to2(transform.position);
        targetPosition = Vector.V3to2(transform.position) + moveDir.normalized * pullAmount;
        pulling = true;
        dir = Vector2.Angle(forwards, moveDir) < 10.0f ? -1.0f : 1.0f;
    }

    private void rotateWheels(float distance)
    {
        var rotateAmount = Mathf.Rad2Deg * distance / (2.0f * Mathf.PI) * 10f * dir;
        foreach(var wheel in wheels)
        {
            wheel.Rotate(Vector3.up, rotateAmount, Space.Self);
        }
    }
}
