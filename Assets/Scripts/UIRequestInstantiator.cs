//#define DEBUG_UI_REQUEST

using Common;
using UnityEngine;

public class UIRequestInstantiator : MonoBehaviour, IUIRequest
{
    [SerializeField]
    bool isShowUIRequestEnabled = true;

    [SerializeField]
    GameObject uiRequestPrefab;

    [SerializeField]
    Vector3 uiRequestPosition;

    GameObject instantiatedUIRequest = null;

    public bool IsShowUIRequestEnabled() => isShowUIRequestEnabled;

    public void InitializeUIRequest()
    {
        instantiatedUIRequest = Instantiate(uiRequestPrefab, transform);
        instantiatedUIRequest.transform.localPosition = uiRequestPosition;
    }

    public void DestroyUIRequest()
    {
#if !DEBUG_UI_REQUEST
        Destroy(instantiatedUIRequest);
#endif
    }
}
