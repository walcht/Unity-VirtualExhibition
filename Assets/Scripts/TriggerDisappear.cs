using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerDisappear : MonoBehaviour
{
    [Range(0.0f, 3.0f)]
    public float fadingSpeed = 0.1f; // unit of Alpha transparency per Second
    Material material;
    int playerTagHash;

    Color materialDefaultColor;
    Color materialTargetColor; // Color that the material will have at the end of a FadeIn phase

    int colorShaderPropertyID;

    private void Start()
    {
        playerTagHash = "MainCharacter".GetHashCode();

        material = gameObject.GetComponent<Material>();
        materialDefaultColor = material.color;
        materialTargetColor = new Color(
            material.color.r,
            material.color.g,
            material.color.b,
            0.00f
        );
        colorShaderPropertyID = Shader.PropertyToID("_Color");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.GetHashCode() != playerTagHash)
            return;
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.GetHashCode() != playerTagHash)
            return;
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        while (material.color.a > Mathf.Epsilon)
        {
            material.SetColor(
                colorShaderPropertyID,
                Color.Lerp(materialDefaultColor, materialTargetColor, Time.deltaTime * fadingSpeed)
            );
            yield return null;
        }
    }

    IEnumerator FadeOutCoroutine()
    {
        while (material.color.a < materialDefaultColor.a - Mathf.Epsilon)
        {
            material.SetColor(
                colorShaderPropertyID,
                Color.Lerp(materialTargetColor, materialDefaultColor, Time.deltaTime * fadingSpeed)
            );
            yield return null;
        }
    }
}
