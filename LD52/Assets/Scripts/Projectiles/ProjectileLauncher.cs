using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public static ProjectileLauncher main;
    private void Awake()
    {
        main = this;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Launching");
            Launch(Vector3.zero, transform.position);
        }
    }

    public Projectile projectilePrefab;

    public void Launch(Vector3 targetPosition, Vector3 launchPosition)
    {
        Quaternion projectileRotation = Quaternion.LookRotation(targetPosition - launchPosition);
        Projectile newProjectile = Instantiate(projectilePrefab, launchPosition, projectileRotation, transform);
        newProjectile.Launch(targetPosition);
    }
}
