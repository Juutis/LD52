using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTeleport : CastableSpell
{
    [SerializeField]
    private GameObject afterEffect;

    [SerializeField]
    private GameObject entryEffect;

    [SerializeField]
    private GameObject followingEffect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void PerformSpellEffect()
    {
        ZarguufAnimator.main.Cast();
        base.PerformSpellEffect();
        Debug.Log($"Perform Teleport");
        var fx = Instantiate(afterEffect);
        fx.transform.position = ZarguufAnimator.main.transform.position;

        if (spellTargetEntity.TryGetComponent<PlayerMovement>(out PlayerMovement movement))
        {
            movement.Teleport(spellTargetLocation);
            SoundManager.main.PlaySound(GameSoundType.Teleport);

            var fx2 = Instantiate(followingEffect);
            fx2.transform.position = ZarguufAnimator.main.transform.position;
            fx2.transform.parent = ZarguufAnimator.main.transform;

            var fx3 = Instantiate(entryEffect);
            fx3.transform.position = ZarguufAnimator.main.transform.position;
        }
        else
        {
            SoundManager.main.PlaySound(GameSoundType.SpellFizzle);
        }
    }
}
