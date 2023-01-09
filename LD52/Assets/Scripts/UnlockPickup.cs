using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPickup : MonoBehaviour
{
    [SerializeField]
    private SpellType spell;
    public SpellType Spell { get { return spell; } }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
