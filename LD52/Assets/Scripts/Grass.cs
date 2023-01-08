using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField]
    private Material baseMaterial;

    [SerializeField]
    private GameObject grassLayer;

    [SerializeField]
    private int layers = 3;

    [SerializeField]
    private float height = 0.1f;

    [SerializeField]
    private Color topColor;

    [SerializeField]
    private Color bottomColor;

    // Start is called before the first frame update
    void Start()
    {
        for(var i = 0; i < layers; i++)
        {
            var t = (float)i / (float)layers;
            var layer = Instantiate(grassLayer);
            var material = Instantiate(baseMaterial);

            var color = Color.Lerp(bottomColor, topColor, t);
            material.SetFloat("_Cutoff", t);
            material.SetColor("_BaseColor", color);

            var meshRenderer = layer.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = material;
            layer.transform.position = transform.position + Vector3.up * t * height;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
