using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CastableSpellManager : MonoBehaviour
{
    [SerializeField]
    private List<CastableSpell> unlockableSpells;
    [SerializeField]
    private List<CastableSpell> startingSpells;
    private List<CastableSpell> usableSpells = new List<CastableSpell>();

    private void Start()
    {
        foreach (CastableSpell spell in startingSpells)
        {
            MakeSpellUsable(spell);
        }
    }

    public void UnlockSpell(SpellType spellType)
    {
        CastableSpell spell = unlockableSpells.FirstOrDefault(spell => spell.Type == spellType);
        if (spell == null)
        {
            Debug.Log($"[CastableSpellManager]: Spell '{spellType}' not found in unlockableSpells.");
            return;
        }
        if (usableSpells.FirstOrDefault(spell => spell.Type == spellType) != null)
        {
            Debug.Log($"[CastableSpellManager]: Spell '{spellType}' was already unlocked!.");
            return;
        }
        MakeSpellUsable(spell);
    }

    private void MakeSpellUsable(CastableSpell spellPrefab)
    {
        CastableSpell spell = Instantiate(spellPrefab, Vector3.zero, Quaternion.identity, transform);
        usableSpells.Add(spell);
        spell.Initialize();
        DrawSpell(spell);
    }

    private void DrawSpell(CastableSpell spell)
    {
        UISpellBar.main.DrawSpell(spell);
    }

    private void CastSpells()
    {
        foreach (CastableSpell spell in usableSpells)
        {
            if (Input.GetKeyDown(spell.HotKey) || Input.GetKey(spell.HotKey))
            {
                spell.Cast(true);
            }
            else if (Input.GetKeyUp(spell.HotKey))
            {
                spell.Cast();
            }
        }
    }

    void Update()
    {
        CastSpells();
    }
}
