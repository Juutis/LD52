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

    public Projectile projectilePrefab;

    public void Launch(Vector3 targetPosition, Vector3 launchPosition)
    {
        Quaternion projectileRotation = Quaternion.LookRotation(targetPosition - launchPosition);
        Projectile newProjectile = Instantiate(projectilePrefab, launchPosition, projectileRotation, transform);
        newProjectile.Launch(targetPosition);
    }
}
