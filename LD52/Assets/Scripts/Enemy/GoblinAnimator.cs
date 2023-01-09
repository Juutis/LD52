using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnimator : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private bool debugShoot;

    [SerializeField]
    private bool debugWalk;

    [SerializeField]
    private GameObject arrowInWeapon;

    [SerializeField]
    private GameObject arrowInHand;

    public bool ReadyToMove { get; private set; } = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (debugWalk) {
            SetWalking(!anim.GetBool("Walk"));
            debugWalk = false;
        }
        if (debugShoot) {
            Shoot();
            debugShoot = false;
        }
    }

    public void SetWalking(bool walking)
    {
        anim.SetBool("Walk", walking);
    }

    public void Shoot()
    {
        anim.SetBool("Shoot", true);
        ReadyToMove = false;
    }

    public void ShootCallback()
    {
        anim.SetBool("Shoot", false);
        ReadyToMove = true;
    }

    public void Fire()
    {
        arrowInWeapon.SetActive(false);
    }

    public void ShowArrowInHand()
    {
        arrowInHand.SetActive(true);
    }

    public void PutArrowToWeapon()
    {
        arrowInWeapon.SetActive(true);
        arrowInHand.SetActive(false);
    }

    public void Stunned(bool isStunned)
    {
        anim.SetBool("Stunned", isStunned);
    }
}
