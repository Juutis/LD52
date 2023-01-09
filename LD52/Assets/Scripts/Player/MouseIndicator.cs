using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MouseIndicator : MonoBehaviour
{
    [SerializeField]
    private Material NormalCursor;

    [SerializeField]
    private Material BlinkCursor;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Transform player;

    private bool blinkMode = false;
    private Vector3 mousePos = Vector3.zero;
    private LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        // Cursor.visible = false;
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var spell = CastableSpellManager.main.GetPreparedSpells().FirstOrDefault();
        if (spell != null)
        {
            transform.localScale = spell.GetMouseIndicatorSize() * Vector3.one;
        }
        else
        {
            transform.localScale = Vector3.one;
        }

        Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouse, out RaycastHit hitInfo, 1000, LayerMask.GetMask("Ground"));
        var mouseDir = PlayerToMouseDir();

        if (hitInfo.collider != null)
        {
            mousePos = hitInfo.point;

            if (spell == null)
            {
                transform.position = new Vector3(mousePos.x, 0.1f, mousePos.z);
                meshRenderer.sharedMaterial = NormalCursor;
            }
            else if (spell.GetSpellTargetType() == SpellTargeting.TargetGroundClip)
            {
                HandleClippingGroundTargetSpell(spell, mouseDir, mousePos);
            }
            else if (spell.GetSpellTargetType() == SpellTargeting.TargetGroundNoClip)
            {
                HandleNoClippingGroundTargetSpell(spell, mouseDir, mousePos);
            }
            else if (spell.GetSpellTargetType() == SpellTargeting.TargetRaycastNoClip)
            {
                Vector3 cursorPos = new Vector3(mousePos.x, 0.1f, mousePos.z);

                var cursorDist = Vector.Substract(cursorPos, player.position);
                if (cursorDist.magnitude > spell.GetCastRange())
                {
                    cursorPos = Vector.SetY(Vector.V2to3(cursorDist.normalized * spell.GetCastRange()) + player.position, 0.1f);
                }

                Physics.Raycast(cursorPos + Vector3.up * 20f, Vector3.down, out RaycastHit spellTargetHitInfo, 100, LayerMask.GetMask("DynamicObstacle"));
                if (spellTargetHitInfo.collider != null)
                {
                    spell.SetTargets(spellTargetHitInfo.collider.transform, player);
                }

                transform.position = new Vector3(cursorPos.x, 0.1f, cursorPos.z);
                meshRenderer.sharedMaterial = spell.GetMouseMaterial();
            }
        }

        Vector3 lineStart = player.position + mouseDir;
        Vector3 lineEnd = transform.position - mouseDir * 0.75f;
        line.SetPosition(0, new Vector3(lineStart.x, 0.1f, lineStart.z));
        line.SetPosition(1, new Vector3(lineEnd.x, 0.1f, lineEnd.z));
    }

    private void HandleClippingGroundTargetSpell(CastableSpell spell, Vector3 mouseDir, Vector3 mousePos)
    {
        float raycastDistance = Mathf.Min(spell.GetCastRange(), PlayerToMouse2D().magnitude);
        Physics.Raycast(player.position, mouseDir, out RaycastHit blinkHitInfo, raycastDistance, LayerMask.GetMask("Obstacles"));

        if (blinkHitInfo.collider != null)
        {
            Vector3 cursorPos = blinkHitInfo.point - mouseDir;
            cursorPos = new Vector3(cursorPos.x, 0.1f, cursorPos.z);
            Vector2 cursorDist = Vector.Substract(blinkHitInfo.point, player.position);

            if (cursorDist.magnitude > spell.GetCastRange())
            {
                cursorPos = Vector.SetY(Vector.V2to3(cursorDist.normalized * spell.GetCastRange()) + player.position, 0.1f);
            }

            transform.position = cursorPos;
            meshRenderer.sharedMaterial = spell.GetMouseMaterial();
        }
        else
        {

            Vector3 cursorPos = new Vector3(mousePos.x, 0.1f, mousePos.z);

            var cursorDist = Vector.Substract(cursorPos, player.position);
            if (cursorDist.magnitude > spell.GetCastRange())
            {
                cursorPos = Vector.SetY(Vector.V2to3(cursorDist.normalized * spell.GetCastRange()) + player.position, 0.1f);
            }

            bool hitBlinkPassable = Physics.Raycast(cursorPos + Vector3.up * 5f, Vector3.down, out RaycastHit blinkPassableHit, 5.5f, LayerMask.GetMask("BlinkPassable"));

            // if mouse is on top of something that's passable by blink, but can't blink into it
            if (hitBlinkPassable)
            {
                float blinkPassableRaycastDist = (cursorPos - player.position).magnitude;
                RaycastHit lastBlinkPassable =
                    Physics.RaycastAll(player.position, mouseDir, blinkPassableRaycastDist, LayerMask.GetMask("BlinkPassable"))
                    .ToList()
                    .LastOrDefault();

                if (lastBlinkPassable.collider != null)
                {
                    cursorPos = Vector.V2to3(Vector.Substract(lastBlinkPassable.point, mouseDir), 0.1f);
                }
            }

            transform.position = cursorPos;
            meshRenderer.sharedMaterial = spell.GetMouseMaterial();
        }

        spell.SetTargets(player, transform);
    }


    private void HandleNoClippingGroundTargetSpell(CastableSpell spell, Vector3 mouseDir, Vector3 mousePos)
    {
        float raycastDistance = Mathf.Min(spell.GetCastRange(), PlayerToMouse2D().magnitude);

        Vector3 cursorPos = new Vector3(mousePos.x, 0.1f, mousePos.z);

        var cursorDist = Vector.Substract(cursorPos, player.position);
        if (cursorDist.magnitude > spell.GetCastRange())
        {
            cursorPos = Vector.SetY(Vector.V2to3(cursorDist.normalized * spell.GetCastRange()) + player.position, 0.1f);
        }

        bool hitBlinkPassable = Physics.Raycast(cursorPos + Vector3.up * 5f, Vector3.down, out RaycastHit blinkPassableHit, 5.5f, LayerMask.GetMask("BlinkPassable"));

        // if mouse is on top of something that's passable by blink, but can't blink into it
        if (hitBlinkPassable)
        {
            float blinkPassableRaycastDist = (cursorPos - player.position).magnitude;
            RaycastHit lastBlinkPassable =
                Physics.RaycastAll(player.position, mouseDir, blinkPassableRaycastDist, LayerMask.GetMask("BlinkPassable"))
                .ToList()
                .LastOrDefault();

            if (lastBlinkPassable.collider != null)
            {
                float colliderRadius = Mathf.Max(lastBlinkPassable.collider.bounds.extents.x, lastBlinkPassable.collider.bounds.extents.z) + 0.5f;
                Vector3 colliderCenter = lastBlinkPassable.collider.bounds.center;
                cursorPos = Vector.V2to3(Vector.Substract(colliderCenter, mouseDir * colliderRadius), 0.1f);
            }
        }

        transform.position = cursorPos;
        meshRenderer.sharedMaterial = spell.GetMouseMaterial();

        spell.SetTargets(player, transform);
    }

    private void FixedUpdate()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    blinkMode = true;
        //    meshRenderer.sharedMaterial = BlinkCursor;
        //}
        //else
        //{
        //    blinkMode = false;
        //    meshRenderer.sharedMaterial = NormalCursor;
        //}
    }

    private Vector2 PlayerToMouse2D()
    {
        var playerPos2 = new Vector2(player.position.x, player.position.z);
        var ownPos2 = new Vector2(mousePos.x, mousePos.z);
        return ownPos2 - playerPos2;
    }

    private Vector3 PlayerToMouseDir()
    {
        var v2Dir = PlayerToMouse2D().normalized;
        return new(v2Dir.x, 0.1f, v2Dir.y);
    }
}
