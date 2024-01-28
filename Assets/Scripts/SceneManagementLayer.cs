using Common;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneManagementLayer", menuName = "SO/SceneManagementLayer")]
public class SceneManagementLayer : ScriptableObject
{
    // ScenelLoad delegates
    public delegate void VoidSceneLoadHandler();
    public delegate void ExhibitionSceneLoadHandler(Layout layout);
    public delegate void SceneLoadHandler(AsyncOperation operation);

    // SceneUnload delegates
    public delegate void VoidSceneUnloadHandler();
    public delegate void ExhibitionSceneUnloadHandler(Layout layout);
    public delegate void SceneUnloadHandler(AsyncOperation operation);
    public event SceneLoadHandler BeforeLoadSceneEvent;

    // SceneLoad events
    public event VoidSceneLoadHandler EntranceSceneLoadedEvent;
    public event ExhibitionSceneLoadHandler ExhibitionSceneLoadedEvent;
    public event VoidSceneLoadHandler AuditoriumSceneLoadedEvent;
    public event VoidSceneLoadHandler ChoseCharacterSceneLoadedEvent;
    public event VoidSceneLoadHandler PreviewStandSceneLoadedEvent;

    // SceneUnload events
    public event VoidSceneUnloadHandler EntranceSceneUnloadedEvent;
    public event ExhibitionSceneUnloadHandler ExhibitionSceneUnloadedEvent;
    public event VoidSceneUnloadHandler AuditoriumSceneUnloadedEvent;
    public event VoidSceneUnloadHandler ChoseCharacterSceneUnloadedEvent;
    public event VoidSceneUnloadHandler PreviewStandSceneUnloadedEvent;
    string _exhibitionScene;
    Layout _exhibitionLayout;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    /// <summary>
    ///     Loads the chose-character scene on top of the current active scene(s) (default behavior)
    /// </summary>
    /// <param name="unloadCurrentActiveScene">Default: true. If set to false, current active scene is NOT
    /// unloaded</param>
    public void LoadChoseCharacterScene(bool unloadCurrentActiveScene = true)
    {
        if (unloadCurrentActiveScene)
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadSceneAsync("chose_character", LoadSceneMode.Additive);
    }

    /// <summary>
    ///     Loads the entrance scene on top of current scene(s) and unloads current active scene
    /// </summary>
    /// <param name="unloadCurrentActiveScene">Default: true. If set to false, current active scene is NOT
    /// unloaded</param>
    public void LoadEntranceScene(bool unloadCurrentActiveScene = true)
    {
        if (unloadCurrentActiveScene)
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        AsyncOperation operation = SceneManager.LoadSceneAsync("entrance", LoadSceneMode.Additive);
        BeforeLoadSceneEvent?.Invoke(operation);
    }

    /// <summary>
    ///      Loads the exhibition scene specified by the CustomizationManager script on top of current
    ///      scene(s) and unloads current active scene. Make sure NOT to call this function in OnEnable()
    ///      or OnAwake() callbacks!
    /// </summary>
    /// <param name="unloadCurrentActiveScene">Default: true. If set to false, current active scene is NOT
    /// unloaded</param>
    public void LoadExhibitionScene(bool unloadCurrentActiveScene = true)
    {
        if (ExhibitionScene == null)
        {
            // raise exception here
            Debug.LogError(
                "Are you calling LoadExhibitionScene from OnEnable() or OnAwake() callbacks?"
            );
            return;
        }
        if (unloadCurrentActiveScene)
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        AsyncOperation operation = SceneManager.LoadSceneAsync(
            ExhibitionScene,
            LoadSceneMode.Additive
        );
        BeforeLoadSceneEvent?.Invoke(operation);
    }

    /// <summary>
    ///     Loads the auditorium scene on top of current scene(s) and unloads current active scene
    /// </summary>
    /// <param name="unloadCurrentActiveScene">Default: true. If set to false, current active scene is NOT
    /// unloaded</param>
    public void LoadAuditoriumScene(bool unloadCurrentActiveScene = true)
    {
        if (unloadCurrentActiveScene)
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        AsyncOperation operation = SceneManager.LoadSceneAsync(
            "auditorium",
            LoadSceneMode.Additive
        );
        BeforeLoadSceneEvent?.Invoke(operation);
    }

    /// <summary>
    ///     Loads the preview stand scene on top of current scene(s) and unloads current active scene
    /// </summary>
    /// <param name="unloadCurrentActiveScene">Default: true. If set to false, current active scene is NOT
    /// unloaded</param>
    public void LoadPreviewStandScene(bool unloadCurrentActiveScene = true)
    {
        if (unloadCurrentActiveScene)
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadSceneAsync("preview_stand", LoadSceneMode.Additive);
    }

    /// <summary>
    ///     Loads the preview stand menu on top of current scene(s) and unloads current active scene
    /// </summary>
    /// <param name="unloadCurrentActiveScene">Default: true. If set to false, current active scene is NOT
    /// unloaded</param>
    public void LoadPreviewStandMenuScene(bool unloadCurrentActiveScene = true)
    {
        if (unloadCurrentActiveScene)
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadSceneAsync("preview_stand_menu", LoadSceneMode.Additive);
    }

    public string ExhibitionScene
    {
        get { return _exhibitionScene; }
        set
        {
            switch (value.ToLower())
            {
                case "small_exhibition":
                    _exhibitionScene = value;
                    _exhibitionLayout = Layout.SMALL;
                    break;
                case "medium_exhibition":
                    _exhibitionScene = value;
                    _exhibitionLayout = Layout.MEDIUM;
                    break;
                case "large_exhibition":
                    _exhibitionScene = value;
                    _exhibitionLayout = Layout.LARGE;
                    break;

                default:
                    Debug.LogError(
                        "Assigned exhibition scene name doesn't reference any exhibition scene"
                    );
                    break;
            }
        }
    }

    public Layout ExhibitionLayout
    {
        get { return _exhibitionLayout; }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the newly loaded scene is loaded additively, then make sure to re-compute LightProbes data
        if (mode == LoadSceneMode.Additive)
        {
            SceneManager.SetActiveScene(scene);
            LightProbes.Tetrahedralize();
        }
        switch (scene.name.ToLower())
        {
            case "chose_character":
                ChoseCharacterSceneLoadedEvent?.Invoke();
                break;
            case "entrance":
                EntranceSceneLoadedEvent?.Invoke();
                break;
            case "small_exhibition":
                ExhibitionSceneLoadedEvent?.Invoke(Layout.SMALL);
                break;
            case "medium_exhibition":
                ExhibitionSceneLoadedEvent?.Invoke(Layout.MEDIUM);
                break;
            case "large_exhibition":
                ExhibitionSceneLoadedEvent?.Invoke(Layout.LARGE);
                break;
            case "auditorium":
                AuditoriumSceneLoadedEvent?.Invoke();
                break;
            case "preview_stand":
                PreviewStandSceneLoadedEvent?.Invoke();
                break;
            default:
                break;
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        PreviousActiveSceneName = scene.name.ToLower();
        switch (scene.name.ToLower())
        {
            case "chose_character":
                ChoseCharacterSceneUnloadedEvent?.Invoke();
                break;
            case "entrance":
                EntranceSceneUnloadedEvent?.Invoke();
                break;
            case "small_exhibition":
                ExhibitionSceneUnloadedEvent?.Invoke(Layout.SMALL);
                break;
            case "medium_exhibition":
                ExhibitionSceneUnloadedEvent?.Invoke(Layout.MEDIUM);
                break;
            case "large_exhibition":
                ExhibitionSceneUnloadedEvent?.Invoke(Layout.LARGE);
                break;
            case "auditorium":
                AuditoriumSceneUnloadedEvent?.Invoke();
                break;
            case "preview_stand":
                PreviewStandSceneUnloadedEvent?.Invoke();
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public string PreviousActiveSceneName { get; set; }
}
