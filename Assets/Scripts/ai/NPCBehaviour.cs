using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

/// <summary>
///     Monobehaviour that should be attached to each NPC to add a fade-in\fade-out effect when collision
///     with another NPC is detected. There are many reasons why having this feature is better than using
///     the builtin collision avoindance system in the NavMeshAgent component. It turns  out  after   all
///     that this is much-much more complicated to implement than I first thought it would be. So back to
///     the collision system provided by NavMeshAgent :(
/// </summary>
[RequireComponent(typeof(Collider))]
public class NPCBehaviour : MonoBehaviour
{
    [Range(0.1f, 3.0f)] public float fadeinTime;
    [Range(0.1f, 3.0f)] public float fadeoutTime;

    Color startColor;
    Color targetColor;

    MeshRenderer    attachedRenderer;
    int             nbrOfNPCsInCollider = 0;                                // number of NPCs whose colliders are still in touch with this NPC's collider
                                                                            // fade-out shoould only be played when this number is set to 0!
    private void Awake()
    {
        attachedRenderer = GetComponent<MeshRenderer>();

        startColor = attachedRenderer.material.color;                       
        targetColor = attachedRenderer.material.color;                      // target color is full transparency
        targetColor.a = 0;                                                  
    }

    // keep in mind that another NPC can enter trigger this collider whil
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("npc") || other.CompareTag("MainCharacter"))
        {
            if (nbrOfNPCsInCollider == 0) StartCoroutine(Lerp_MeshRenderer_Color(fadeinTime, startColor, targetColor));
            ++nbrOfNPCsInCollider;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("npc") || other.CompareTag("MainCharacter"))
        {
            --nbrOfNPCsInCollider;
            if (nbrOfNPCsInCollider == 0) StartCoroutine(Lerp_MeshRenderer_Color(fadeoutTime, targetColor, startColor));
        }
    }

    IEnumerator Lerp_MeshRenderer_Color(float lerpDuration, Color lerpStart, Color lerpTarget)
    {
        float startTime = Time.time;
        bool isLerpDone = false;

        while (!isLerpDone)
        {
            float progress = Time.time - startTime;
            attachedRenderer.material.color = Color.Lerp(lerpStart, lerpTarget, progress / lerpDuration);

            if (progress >= lerpDuration) isLerpDone = true;

            yield return null;
        }
    }
}
