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

    public static CastableSpellManager main;
    private List<CastableSpell> spellsThatNeedPreparation = new List<CastableSpell>();
    private List<CastableSpell> spellsThatDont = new List<CastableSpell>();

    private void Awake()
    {
        main = this;
    }

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
        UIManager.main.ShowSpellUnlock(spell);
        spellsThatNeedPreparation = usableSpells.Where(spell => spell.NeedsPreparation).ToList();
        spellsThatDont = usableSpells.Where(spell => !spell.NeedsPreparation).ToList();
    }

    public IEnumerable<CastableSpell> GetPreparedSpells()
    {
        return usableSpells.Where(x => x.IsPrepared);
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

    private CastableSpell DetermineCurrentSpell()
    {
        CastableSpell currentSpell = null;
        foreach (CastableSpell spell in spellsThatNeedPreparation)
        {
            if (spell.IsPrepared)
            {
                currentSpell = spell;
            }
        }
        foreach (CastableSpell spell in spellsThatNeedPreparation)
        {
            if (Input.GetKeyDown(spell.HotKey))
            {
                if (spell.IsPrepared)
                {
                    if (spell == currentSpell)
                    {
                        currentSpell = null;
                    }
                    spell.Unprepare();
                }
                else
                {
                    currentSpell = spell;
                }
            }
        }
        if (currentSpell != null)
        {
            spellsThatNeedPreparation.ForEach(spell =>
            {
                if (spell != currentSpell)
                {
                    spell.Unprepare();
                }
            });
            if (!currentSpell.IsPrepared)
            {
                currentSpell.Prepare();
            }
        }
        return currentSpell;
    }

    private void CastSpells()
    {
        CastableSpell preparedSpell = DetermineCurrentSpell();
        if (preparedSpell != null && Input.GetMouseButtonDown(0))
        {
            preparedSpell.Cast();
        }
        if (Input.GetMouseButtonDown(1))
        {
            foreach (CastableSpell spell in usableSpells)
            {
                if (spell.IsPrepared)
                {
                    spell.Unprepare();
                }
            }
        }
        foreach (CastableSpell spell in spellsThatDont)
        {
            if (Input.GetKeyDown(spell.HotKey))
            {
                spell.Cast();
            }
        }
    }

    void Update()
    {
        if (GameManager.main.Paused)
        {
            return;
        }
        CastSpells();
    }
}
