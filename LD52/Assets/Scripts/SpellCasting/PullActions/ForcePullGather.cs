using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePullGather : MonoBehaviour
{

    [SerializeField]
    private List<Transform> berries = new List<Transform>();
    [SerializeField]
    private int pullBerryCount;
    [SerializeField]
    private float pullSpeed;

    private List<Transform> allBerries;
    private List<Transform> berriesBeingPulled = new List<Transform>();
    private List<Vector3> startPositions = new List<Vector3>();
    private Vector3 targetPosition;
    private float lerpAmount = 0;
    private bool pulling = false;

    // Start is called before the first frame update
    void Start()
    {
        allBerries = new List<Transform>(berries);
    }

    // Update is called once per frame
    void Update()
    {
        if (pulling)
        {
            targetPosition = PlayerMovement.main.transform.position;
            lerpAmount += pullSpeed * Time.deltaTime;

            for (int i = 0; i < berriesBeingPulled.Count; i++)
            {
                var berry = berriesBeingPulled[i];
                var startPos = startPositions[i];
                berry.position = Vector3.Lerp(startPos, targetPosition, lerpAmount);
            }

            if (lerpAmount >= 1)
            {
                pulling = false;
                int berriesCount = berriesBeingPulled.Count;
                for (int i = 0; i < berriesCount; i++)
                {
                    var berry = berriesBeingPulled[i];
                    Destroy(berry.gameObject);
                }
                GameManager.main.AddBerry(berriesCount);

                berriesBeingPulled = new List<Transform>();
                startPositions = new List<Vector3>();
            }
        }
    }

    public void PullAction()
    {
        for (int i = 0; i < pullBerryCount; i++)
        {
            if (allBerries.Count >= 1)
            {
                Transform berry = allBerries[0];
                allBerries.Remove(berry);
                berriesBeingPulled.Add(berry);
                startPositions.Add(berry.position);
                pulling = true;
                lerpAmount = 0;
            }
        }
    }
}
