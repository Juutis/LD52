using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpellBar : MonoBehaviour
{
    [SerializeField]
    private UISpell spellPrefab;
    [SerializeField]
    private Transform spellContainer;
    public static UISpellBar main;
    private void Awake()
    {
        main = this;
    }

    public void DrawSpell(CastableSpell spell)
    {
        UISpell uiSpell = Instantiate(spellPrefab, Vector3.zero, Quaternion.identity, spellContainer);
        uiSpell.Initialize(spell);
    }
}
