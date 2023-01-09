using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePullToMove : MonoBehaviour
{
    [SerializeField]
    private float pullAmount;
    [SerializeField]
    private float pullSpeed;

    private Rigidbody rbody;
    private Vector2 targetPosition;
    private bool pulling = false;

    [SerializeField]
    private List<Transform> wheels;

    private float dir = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pulling)
        {
            rbody.velocity = transform.forward * -1f * dir * pullSpeed;
            rotateWheels(rbody.velocity.magnitude * Time.deltaTime);

            if ((targetPosition - Vector.V3to2(transform.position)).magnitude < 0.1f)
            {
                pulling = false;
                rbody.velocity = Vector3.zero;
            }
        }
    }

    public void PullAction()
    {
        SoundManager.main.PlaySound(GameSoundType.PullCart);
        // Move object forward or backward depending on player's relative location
        Vector2 towardsPlayer = Vector.Substract(PlayerMovement.main.transform.position, transform.position).normalized;

        Vector2 forwards = Vector.V3to2(transform.forward);
        Vector2 moveDir = forwards.normalized * (Vector2.Dot(towardsPlayer, forwards) / forwards.magnitude);

        targetPosition = Vector.V3to2(transform.position) + moveDir.normalized * pullAmount;
        pulling = true;
        dir = Vector2.Angle(forwards, moveDir) < 10.0f ? -1.0f : 1.0f;
    }

    private void rotateWheels(float distance)
    {
        var rotateAmount = Mathf.Rad2Deg * distance / (2.0f * Mathf.PI) * 10f * dir;
        foreach (var wheel in wheels)
        {
            wheel.Rotate(Vector3.up, rotateAmount, Space.Self);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        pulling = false;
        rbody.velocity = Vector3.zero;
    }
}
