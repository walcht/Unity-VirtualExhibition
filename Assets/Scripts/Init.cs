using UnityEngine;

/// <summary>
///     Initializes Random numbers generator and other systems.
/// </summary>
public class Init : MonoBehaviour
{
    private void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
    }
}
