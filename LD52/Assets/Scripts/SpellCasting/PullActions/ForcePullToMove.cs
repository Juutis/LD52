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

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector.V2to3(Vector2.Lerp(startPosition, targetPosition, lerpAmount));
        lerpAmount += pullSpeed * Time.deltaTime;
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
    }
}
