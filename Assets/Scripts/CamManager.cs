using DesignPatterns;
using UnityEngine;

public class CamManager : SafeSingleton<CamManager>
{
    public Camera LoadingCamera;
    public SceneManagementLayer SceneManagementLayerSO;

    void OnEnable()
    {
        SceneManagementLayerSO.EntranceSceneLoadedEvent += OnSceneLoaded;
        SceneManagementLayerSO.AuditoriumSceneLoadedEvent += OnSceneLoaded;
        SceneManagementLayerSO.ExhibitionSceneLoadedEvent += OnExhibitionSceneLoaded;
        SceneManagementLayerSO.ChoseCharacterSceneLoadedEvent += OnSceneLoaded;

        SceneManagementLayerSO.BeforeLoadSceneEvent += OnBeforeSceneLoad;
    }

    void OnSceneLoaded() => LoadingCamera.gameObject.SetActive(false);

    void OnExhibitionSceneLoaded(Common.Layout _) => LoadingCamera.gameObject.SetActive(false);

    void OnBeforeSceneLoad(AsyncOperation _) => LoadingCamera.gameObject.SetActive(true);
}
