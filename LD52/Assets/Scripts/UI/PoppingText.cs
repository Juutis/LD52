using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PoppingText : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private TextMeshPro txtMessage;

    public void Show(Vector3 position, string message, Color color, int fontSize)
    {
        transform.position = new Vector3(position.x, position.y + 2f, position.z);
        txtMessage.color = color;
        txtMessage.text = message;
        txtMessage.fontSize = fontSize;
        animator.Play("poppingTextShow");
    }

    public void ShowFinished()
    {
        Destroy(gameObject);
    }
}
