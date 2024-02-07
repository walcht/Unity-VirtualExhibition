// #define DEBUG
// #define IN_EDITOR_DEBUG

using System.Collections;
using System.Collections.Generic;
using Common;
using DesignPatterns;
using JSONContainer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using Utils;

public class CustomizationManager : Singleton<CustomizationManager>
{
    public SceneManagementLayer SceneLayer;
    public CharactersContainer CharactersContainerSO;

    [Tooltip("make sure PreviewExhibition object is active in all exhibition scenes")]
    public bool isThisExhibitionPreview = false;

    [Tooltip("make sure PreviewEntrance object is active in the entrance scene")]
    public bool isThisEntrancePreview = false;
    public bool isThisStandPreview = false;
    public bool isThisStandMenuPreview = false;
    public GameObject pack00;
    public GameObject pack01;
    public GameObject pack02;
    public GameObject pack03;
    public GameObject[] XLBanners = new GameObject[4];
    public GameObject[] LLBanners = new GameObject[3];
    public GameObject[] LRBanners = new GameObject[3];
    public GameObject[] MBanners = new GameObject[2];
    public GameObject[] XLFurniture = new GameObject[4];
    public GameObject[] LLFurniture = new GameObject[3];
    public GameObject[] LRFurniture = new GameObject[3];
    public GameObject XLTV;
    public GameObject LRTV;
    public GameObject LLTV;
    public GameObject sponsorCylinder00;
    public GameObject sponsorCylinder01;
    public GameObject sponsorCylinder02;
    public GameObject sponsorCylinder03;
    public GameObject sponsorDisc;
    public GameObject sponsorDisplayScreen;
    public GameObject sponsorBanner00;
    public GameObject sponsorBanner01;
    public GameObject sponsorBanner02;
    public GameObject sponsorBanner03;
    public GameObject SponsorBanner00;
    public GameObject SponsorBanner01;
    public GameObject CubeScreen;
    public GameObject EnterAuditorium;
    public GameObject[] StandCharacters = new GameObject[18];
    public AIRelatedData smallExhibitonAIData;
    public AIRelatedData mediumExhibitionAIData;
    public AIRelatedData largeExhibitionAIData;

    [HideInInspector]
    public string id;

    [HideInInspector]
    public string entrance_id;

    [HideInInspector]
    public string preview_exhibition_id;

    [HideInInspector]
    public string preview_stand_id;

    [HideInInspector]
    public string stand_menu_id;

    [HideInInspector]
    public string jsonStaticURL;

    [HideInInspector]
    public string mediaStaticURL;

    [HideInInspector]
    public string exhibitionJSON;

    [HideInInspector]
    public string previewExhibitionJSON;

    [HideInInspector]
    public string previewStandJSON;

    [HideInInspector]
    public string entranceJSON;

    [HideInInspector]
    public string standMenuJSON;

    [HideInInspector]
    public ExhibitionDataContainer exhibitionCustomizationData;

    [HideInInspector]
    public EntranceDataContainer entranceCustomizationData;

    [HideInInspector]
    public StandDataContainer standCustomizationData;

    [HideInInspector]
    public MenuRelatedDataContainer standMenuCustomizationData;

    [HideInInspector]
    public int MainCharacterIndex;

    private GameObject exhibitionHall;
    private GameObject instantiatedExhibitionSponsorDisc;
    private readonly GameObject[] instantiatedExhibitionSponsorBanners = new GameObject[4];
    private readonly GameObject[] instantiatedExhibitionSponsorCylinders = new GameObject[4];
    private GameObject instantiatedEntranceSponsorBanner00;
    private GameObject instantiatedEntranceSponsorBanner01;
    private GameObject instantiatedEntranceCubeScreen;

    private void OnEnable()
    {
        SceneLayer.ExhibitionSceneLoadedEvent += OnExhibitionSceneLoad;
        SceneLayer.EntranceSceneLoadedEvent += OnEntranceSceneLoad;
        SceneLayer.ExhibitionSceneUnloadedEvent += OnExhibitionSceneUnload;
        SceneLayer.EntranceSceneUnloadedEvent += OnEntranceSceneUnload;
        SceneLayer.AuditoriumSceneUnloadedEvent += OnAuditoriumSceneUnload;

#if IN_EDITOR_DEBUG
        SetDefaultConfig();
#endif

        if (isThisEntrancePreview)
        {
            EventSystem.current.gameObject.SetActive(false);
            AIManager.Instance.gameObject.SetActive(false);
            StartCoroutine(InitPreviewEntrance());
        }
        else if (isThisExhibitionPreview)
        {
            EventSystem.current.gameObject.SetActive(false);
            AIManager.Instance.gameObject.SetActive(false);
            StartCoroutine(InitPreviewExhibition());
        }
        else if (isThisStandPreview)
        {
            EventSystem.current.gameObject.SetActive(false);
            AIManager.Instance.gameObject.SetActive(false);
            StartCoroutine(InitPreviewStand());
        }
        else if (isThisStandMenuPreview)
        {
            EventSystem.current.gameObject.SetActive(false);
            AIManager.Instance.gameObject.SetActive(false);
            StartCoroutine(InitPreviewMenuStand());
        }
        else
            StartCoroutine(Init());
    }

    private void OnDisable()
    {
        SceneLayer.ExhibitionSceneLoadedEvent -= OnExhibitionSceneLoad;
        SceneLayer.EntranceSceneLoadedEvent -= OnEntranceSceneLoad;
        SceneLayer.ExhibitionSceneUnloadedEvent -= OnExhibitionSceneUnload;
        SceneLayer.EntranceSceneUnloadedEvent -= OnEntranceSceneUnload;
        SceneLayer.AuditoriumSceneUnloadedEvent += OnAuditoriumSceneUnload;
    }

    /// <summary>
    ///     Call this funciton from frontEnd React app to set the exhibition ID.
    /// </summary>
    /// <param name="id">ID string that will be added to a static link to generate URL from where to GET JSON
    /// customization DATA</param>
    public void SetExhibitionID(string id) => this.id = id;

    /// <summary>
    ///     Call this funciton from frontEnd React app to set the preview exhibition ID.
    /// </summary>
    /// <param name="id">ID string that will be added to a static link to generate URL from where to GET JSON
    /// customization DATA</param>
    public void SetPreviewExhibitionID(string preview_exhibition_id) =>
        this.preview_exhibition_id = preview_exhibition_id;

    /// <summary>
    ///     Call this funciton from frontEnd React app to set entrance ID.
    /// </summary>
    /// <param name="id">ID string that will be added to a static link to generate URL from where to GET JSON
    /// customization DATA</param>
    public void SetEntranceID(string entrance_id) => this.entrance_id = entrance_id;

    /// <summary>
    ///     Call this funciton from frontEnd React app to set entrance ID.
    /// </summary>
    /// <param name="id">ID string that will be added to a static link to generate URL from where to GET JSON
    /// customization DATA</param>
    public void SetStandID(string stand_id) => this.preview_stand_id = stand_id;

    /// <summary>
    ///     Call this funciton from frontEnd React app to set stant menu ID.
    ///     This ID is the same as the stand's ID
    /// </summary>
    /// <param name="id">ID string that will be added to a static link to generate URL from where to GET JSON
    /// customization DATA</param>
    public void SetStandMenuID(string pstand_menu_id) => this.stand_menu_id = pstand_menu_id;

    /// <summary>
    ///     Call this function from frontEnd React app to set the static URL from which JSON data will be
    ///     fetched
    /// </summary>
    /// <param name="static_url">Static JSON URL added to stand ID will generate URL from where to fetch JSON
    /// data</param>
    public void SetJsonStaticURL(string json_static_url) => this.jsonStaticURL = json_static_url;

    /// <summary>
    ///     Call this function from frontEnd React app to set the static URL from which media files will be
    ///     downloaded
    /// </summary>
    /// <param name="media_static_url">Static media URL added to stand ID will generate URL from which to
    /// download media files</param>
    public void SetMediaStaticURL(string media_static_url) =>
        this.mediaStaticURL = media_static_url;

    /// <summary>
    ///     Gets exhibition JSON data from jsonStaticURL concatenated with exhibition_id as a string.
    /// </summary>
    IEnumerator SetExhibitionJSON()
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(jsonStaticURL + "exhibition/" + id))
        {
            yield return uwr.SendWebRequest();
            if (!(uwr.result == UnityWebRequest.Result.Success))
            {
                Debug.LogError("Failed to request exhibition JSON data");
                yield break;
            }
            else
            {
                exhibitionJSON = uwr.downloadHandler.text;
            }
        }
    }

    /// <summary>
    ///     Gets entrance JSON data from jsonStaticURL concatenated with entrance_id as a string
    /// </summary>
    IEnumerator SetEntranceJSON()
    {
#if DEBUG
        Debug.Log("Entrance JSON: \t" + jsonStaticURL + "exhibition/" + "entrance/" + entrance_id);
#endif
        using (
            UnityWebRequest uwr = UnityWebRequest.Get(
                jsonStaticURL + "exhibition/" + "entrance/" + entrance_id
            )
        )
        {
            yield return uwr.SendWebRequest();
            if (!(uwr.result == UnityWebRequest.Result.Success))
            {
                Debug.LogError("Failed to request entrance JSON data");
                yield break;
            }
            else
            {
                entranceJSON = uwr.downloadHandler.text;
            }
        }
    }

    /// <summary>
    ///     Gets preview stand JSON data from jsonStaticURL concatenated with preview_stand_id as a string
    /// </summary>
    IEnumerator SetPreviewStandJSON()
    {
        using (
            UnityWebRequest uwr = UnityWebRequest.Get(jsonStaticURL + "stand/" + preview_stand_id)
        )
        {
            yield return uwr.SendWebRequest();
            if (!(uwr.result == UnityWebRequest.Result.Success))
            {
                Debug.LogError("Failed to request preview exhibition JSON data");
            }
            else
            {
                previewStandJSON = uwr.downloadHandler.text;
            }
        }
    }

    /// <summary>
    ///     Gets preview stand menu JSON data from jsonStaticURL concatenated with preview_stand_id as a
    ///     string
    /// </summary>
    IEnumerator SetPreviewStandMenuJSON()
    {
        using (
            UnityWebRequest uwr = UnityWebRequest.Get(
                jsonStaticURL + "stand/" + "menu/" + stand_menu_id
            )
        )
        {
            yield return uwr.SendWebRequest();
            if (!(uwr.result == UnityWebRequest.Result.Success))
            {
                Debug.LogError("Failed to request stand menu JSON data");
                yield break;
            }
            else
            {
                standMenuJSON = uwr.downloadHandler.text;
            }
        }
    }

    /// <summary>
    ///     Gets preview exhibition JSON data from jsonStaticURL concatenated with preview_entrance_id
    ///     as a string
    /// </summary>
    IEnumerator SetPreviewExhibitionJSON()
    {
        using (
            UnityWebRequest uwr = UnityWebRequest.Get(
                jsonStaticURL + "exhibition/" + preview_exhibition_id
            )
        )
        {
            yield return uwr.SendWebRequest();
            if (!(uwr.result == UnityWebRequest.Result.Success))
            {
                Debug.LogError("Failed to request preview exhibition JSON data");
            }
            else
            {
                previewExhibitionJSON = uwr.downloadHandler.text;
            }
        }
    }

    /// <summary>
    ///     Populates initial necessary configurations
    /// </summary>
    public void SetDefaultConfig()
    {
        jsonStaticURL = "https://xpoland-api.rayenhamada.site/";
        mediaStaticURL = "https://xpoland-textures-bucket.s3.eu-west-3.amazonaws.com/";
        id = "648230403ea4e50d2066a498";
        entrance_id = id;
        preview_exhibition_id = "";
        preview_stand_id = "";
        stand_menu_id = preview_stand_id;
    }

    /// <summary>
    ///     Start this coroutine to initialize customization process.
    ///     This is done in a coroutine for synchronization reasons.
    /// </summary>
    /// <returns>Checks each frame whether JSON has been initialized before continuing the initialization
    /// process.</returns>
    IEnumerator Init()
    {
        UIManager.Instance.ShowLoadingUI();
        UIManager.Instance.SetLoadingUIMainText("Chargement des données");
        UIManager.Instance.ShowLoadingUILog();
        UIManager.Instance.SetLoadingLogText("réception des données...");

        while (
            string.IsNullOrWhiteSpace(id)
            || string.IsNullOrWhiteSpace(entrance_id)
            || string.IsNullOrWhiteSpace(mediaStaticURL)
            || string.IsNullOrWhiteSpace(jsonStaticURL)
        )
        {
            yield return null;
        }

        UIManager.Instance.SetLoadingUIProgress(0.3f);
        UIManager.Instance.SetLoadingLogText("désérialisation des données reçues...");

        StartCoroutine(SetExhibitionJSON());
        StartCoroutine(SetEntranceJSON());

        // wait untill exhibition JSON is set
        while (string.IsNullOrWhiteSpace(exhibitionJSON))
            yield return null;

        UIManager.Instance.SetLoadingUIProgress(0.8f);

        while (string.IsNullOrWhiteSpace(entranceJSON))
            yield return null;

        UIManager.Instance.SetLoadingUIProgress(0.9f);

        exhibitionCustomizationData = ExhibitionDataContainer.FromJson(exhibitionJSON);
        entranceCustomizationData = EntranceDataContainer.FromJson(entranceJSON);

        if (exhibitionCustomizationData == null)
        {
            Debug.LogError("Exhibition customization data isn\'t correctly set");
            yield break;
        }

        if (entranceCustomizationData == null)
        {
            Debug.LogError("Entrance customization data isn\'t correctly set");
            yield break;
        }

        UIManager.Instance.HideLoadingUI();

        SetGlobalVariables(); // informing SceneManagementLayer on which type of scene we're using
        SceneLayer.LoadChoseCharacterScene(false);
    }

    /// <summary>
    /// Initializes preview stand for stand customization or preview.
    /// </summary>
    IEnumerator InitPreviewStand()
    {
        UIManager.Instance.ShowLoadingUI();
        UIManager.Instance.SetLoadingUIMainText("Chargement des données");
        UIManager.Instance.ShowLoadingUILog();
        UIManager.Instance.SetLoadingLogText("réception des données...");

        while (
            string.IsNullOrWhiteSpace(preview_stand_id)
            || string.IsNullOrWhiteSpace(jsonStaticURL)
            || string.IsNullOrWhiteSpace(mediaStaticURL)
        )
            yield return null;

        UIManager.Instance.SetLoadingUIProgress(0.4f);

        StartCoroutine(SetPreviewStandJSON());

        yield return new WaitUntil(() => !string.IsNullOrWhiteSpace(previewStandJSON));

        UIManager.Instance.SetLoadingUIProgress(0.6f);
        UIManager.Instance.SetLoadingLogText("désérialisation des données reçues...");

        standCustomizationData = StandDataContainer.FromJson(previewStandJSON);

        UIManager.Instance.SetLoadingUIProgress(0.8f);

        if (standCustomizationData == null)
        {
            Debug.LogError(
                "Failure at serializing customization JSON. Make sure JSON is set properly."
            );
            yield break;
        }

        UIManager.Instance.HideLoadingUI();

        // PreviewStand Scene contains the main script that will initializes
        // the stand and corresponding UIs
        SceneLayer.LoadPreviewStandScene(false);
    }

    /// <summary>
    ///     Start this coroutine for Preview Exhibition customization process.
    /// </summary>
    /// <returns>Checks each frame whether JSON has been initialized before continuing the initialization
    /// process.</returns>
    IEnumerator InitPreviewExhibition()
    {
        UIManager.Instance.ShowLoadingUI();
        UIManager.Instance.SetLoadingUIMainText("Chargement des données");
        UIManager.Instance.ShowLoadingUILog();
        UIManager.Instance.SetLoadingLogText("réception des données...");

        // wait until essential variables are set
        while (
            string.IsNullOrWhiteSpace(jsonStaticURL)
            || string.IsNullOrWhiteSpace(mediaStaticURL)
            || string.IsNullOrWhiteSpace(preview_exhibition_id)
        )
            yield return null;

        UIManager.Instance.SetLoadingUIProgress(0.4f);

        StartCoroutine(SetPreviewExhibitionJSON());

        UIManager.Instance.SetLoadingLogText("désérialisation des données reçues...");

        // wait for serialization to complete
        while (string.IsNullOrWhiteSpace(previewExhibitionJSON))
            yield return null;

        exhibitionCustomizationData = ExhibitionDataContainer.FromJson(previewExhibitionJSON);

        UIManager.Instance.SetLoadingUIProgress(0.9f);

        if (exhibitionCustomizationData == null)
        {
            Debug.LogError("Preview Exhibition customization data isn\'t correctly set");
            yield break; // mb exit Unity?
        }

        UIManager.Instance.HideLoadingUI();
        // informing SceneManagementLayer on which type of scene we're using
        SceneLayer.ExhibitionScene = exhibitionCustomizationData.HallType;
        // load preview exhibition scene
        SceneLayer.LoadExhibitionScene(false);
    }

    IEnumerator InitPreviewEntrance()
    {
        UIManager.Instance.ShowLoadingUI();
        UIManager.Instance.SetLoadingUIMainText("Chargement des données");
        UIManager.Instance.ShowLoadingUILog();
        UIManager.Instance.SetLoadingLogText("réception des données...");

        while (
            string.IsNullOrWhiteSpace(entrance_id)
            || string.IsNullOrWhiteSpace(mediaStaticURL)
            || string.IsNullOrWhiteSpace(jsonStaticURL)
        )
            yield return null;

        UIManager.Instance.SetLoadingUIProgress(0.4f);

        StartCoroutine(SetEntranceJSON());
        UIManager.Instance.SetLoadingLogText("désérialisation des données reçues...");

        while (string.IsNullOrWhiteSpace(entranceJSON))
            yield return null;

        entranceCustomizationData = EntranceDataContainer.FromJson(entranceJSON);

        UIManager.Instance.SetLoadingUIProgress(0.9f);

        if (entranceCustomizationData == null)
        {
            Debug.LogError("Entrance customization data isn\'t correctly set");
            yield break;
        }

        UIManager.Instance.HideLoadingUI();

        SceneLayer.LoadEntranceScene(false);
    }

    IEnumerator InitPreviewMenuStand()
    {
        UIManager.Instance.ShowLoadingUI();
        UIManager.Instance.SetLoadingUIMainText("Chargement des données");
        UIManager.Instance.ShowLoadingUILog();
        UIManager.Instance.SetLoadingLogText("réception des données...");

        while (
            string.IsNullOrWhiteSpace(stand_menu_id)
            || string.IsNullOrWhiteSpace(mediaStaticURL)
            || string.IsNullOrWhiteSpace(jsonStaticURL)
        )
            yield return null;

        UIManager.Instance.SetLoadingUIProgress(0.4f);

        StartCoroutine(SetPreviewStandMenuJSON());
        UIManager.Instance.SetLoadingLogText("désérialisation des données reçues...");

        while (string.IsNullOrWhiteSpace(standMenuJSON))
            yield return null;

        standMenuCustomizationData = MenuRelatedDataContainer.FromJson(standMenuJSON);
        UIManager.Instance.SetLoadingUIProgress(0.9f);

        if (standMenuCustomizationData == null)
        {
            Debug.LogError("Stand menu customization data isn\'t correctly set");
            yield break;
        }

        UIManager.Instance.HideLoadingUI();

        SceneLayer.LoadPreviewStandMenuScene(false);
    }

    /// <summary>
    ///     Sets sharable, gloabal variables necessary for the game to function properly.
    ///     Make sure to call this after serializing customization data.
    /// </summary>
    private void SetGlobalVariables() =>
        SceneLayer.ExhibitionScene = exhibitionCustomizationData.HallType;

    /// <summary>
    ///     Customizes stands according to customization data provided by the stand's owner
    /// </summary>
    private void CustomizeStands()
    {
        foreach (var stand in exhibitionCustomizationData.Stands)
        {
            StandInfo standInfo = StandsObserver.Instance.GetStand(stand.Position);
            // essential for each purchased stand
            StandsObserver.Instance.SetStandAsInteractive(stand.Position);
            StandCustomizationData _standCustomizationData = new StandCustomizationData
            {
                standPosition = stand.Position,
                standID = stand.Id,

                standName = stand.StandName,
                logoDownloadURL = stand.LogoDownloadUrl
            };
            // setting stand menu data
            if (stand.Menu != null)
            {
                if (stand.Menu.MeetLink != null)
                    _standCustomizationData.meetLink = stand.Menu.MeetLink;
                if (stand.Menu.PdfDownloadUrl != null)
                    _standCustomizationData.pdfLink = Utils.Url.GenerateURL(
                        mediaStaticURL,
                        stand.Menu.PdfDownloadUrl
                    );
                if (stand.Menu.Website != null)
                    _standCustomizationData.website = stand.Menu.Website;
                if (stand.Menu.Address != null)
                    _standCustomizationData.address = stand.Menu.Address;
                if (stand.Menu.PhoneNumber != null)
                    _standCustomizationData.phoneNumber = stand.Menu.PhoneNumber;
            }

            StandsObserver.Instance.SetStandCustomizationData(
                stand.Position,
                _standCustomizationData
            );
            // stand texture request and assignement
            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    standInfo.ReferencedStand,
                    Utils.Url.GenerateURL(mediaStaticURL, stand.TextureDownloadUrl)
                )
            );
            // banner customization
            if (stand.Banner.Purchased)
            {
                GameObject banner = null;
                switch (standInfo.Type)
                {
                    case StandType.XL:
                        banner = Instantiate<GameObject>(
                            XLBanners[stand.Banner.BannerType],
                            standInfo.ReferencedStand.transform
                        );
                        break;
                    case StandType.LL:
                        banner = Instantiate<GameObject>(
                            LLBanners[stand.Banner.BannerType],
                            standInfo.ReferencedStand.transform
                        );
                        break;
                    case StandType.LR:
                        banner = Instantiate<GameObject>(
                            LRBanners[stand.Banner.BannerType],
                            standInfo.ReferencedStand.transform
                        );
                        break;
                    case StandType.M:
                        banner = Instantiate<GameObject>(
                            MBanners[stand.Banner.BannerType],
                            standInfo.ReferencedStand.transform
                        );
                        break;
                    default:
                        break;
                }
                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        banner,
                        Utils.Url.GenerateURL(mediaStaticURL, stand.Banner.TextureDownloadUrl)
                    )
                );
            }
            // furniture and TV customization
            switch (standInfo.Type)
            {
                case StandType.XL:
                    if (stand.Furniture.Purchased)
                        Instantiate<GameObject>(
                            XLFurniture[stand.Furniture.FurnitureType],
                            standInfo.ReferencedStand.transform
                        );
                    if (stand.Tv.Purchased)
                        Instantiate<GameObject>(XLTV, standInfo.ReferencedStand.transform);
                    break;
                case StandType.LL:
                    if (stand.Furniture.Purchased)
                        Instantiate<GameObject>(
                            LLFurniture[stand.Furniture.FurnitureType],
                            standInfo.ReferencedStand.transform
                        );
                    if (stand.Tv.Purchased)
                        Instantiate<GameObject>(LLTV, standInfo.ReferencedStand.transform);
                    break;
                case StandType.LR:
                    if (stand.Furniture.Purchased)
                        Instantiate<GameObject>(
                            LRFurniture[stand.Furniture.FurnitureType],
                            standInfo.ReferencedStand.transform
                        );
                    if (stand.Tv.Purchased)
                        Instantiate<GameObject>(LRTV, standInfo.ReferencedStand.transform);
                    break;
                case StandType.M:
                    break;
                case StandType.S:
                    break;
                default:
                    break;
            }
            // character customization
            StandsObserver.Instance.SetStandCharacter(
                stand.Position,
                StandCharacters[stand.CaracterType00],
                StandCharacters[stand.CaracterType01]
            );
        }
    }

    /// <summary>
    ///     Instantiates observable waypoints for purchased stands
    /// </summary>
    /// <param name="layout">Layout of the exhibition hall</param>
    private void AddStandsObservableWaypoints(Layout layout)
    {
        foreach (var stand in exhibitionCustomizationData.Stands)
        {
            StandInfo standInfo = StandsObserver.Instance.GetStand(stand.Position);
            float observing_distance = 0f;
            switch (standInfo.Type)
            {
                case StandType.XL:
                    observing_distance = 5.5f;
                    break;
                case StandType.LL:
                case StandType.LR:
                    observing_distance = 4.5f;
                    break;
                case StandType.M:
                    observing_distance = 3.5f;
                    break;
                case StandType.S:
                    observing_distance = 2.5f;
                    break;
                default:
                    break;
            }
            switch (layout)
            {
                case Layout.SMALL:
                    smallExhibitonAIData.observable_waypoints.Add(
                        (standInfo.ReferencedStand.transform.position, observing_distance)
                    );
                    break;
                case Layout.MEDIUM:
                    mediumExhibitionAIData.observable_waypoints.Add(
                        (standInfo.ReferencedStand.transform.position, observing_distance)
                    );
                    break;
                case Layout.LARGE:
                    largeExhibitionAIData.observable_waypoints.Add(
                        (standInfo.ReferencedStand.transform.position, observing_distance)
                    );
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    ///     Call this function after setting entrance customization data to customize entrance room.
    /// </summary>
    /// <param name="useCache">Whether to use cache for requesting textures
    /// in Entrance Scene</param>
    void CustomizeEntranceScene(bool useCache = true)
    {
        if (entranceCustomizationData.CubeScreen != null)
        {
            if (!string.IsNullOrWhiteSpace(entranceCustomizationData.CubeScreen.TextureDownloadUrl))
            {
                instantiatedEntranceCubeScreen = Instantiate<GameObject>(CubeScreen);
                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        HelperUtilities.GetChildGameObject(
                            instantiatedEntranceCubeScreen,
                            "screens"
                        ),
                        Utils.Url.GenerateURL(
                            mediaStaticURL,
                            entranceCustomizationData.CubeScreen.TextureDownloadUrl,
                            useCache: useCache
                        ),
                        true
                    )
                );
            }
        }
        if (entranceCustomizationData.SponsorBanners != null)
        {
            if (
                !string.IsNullOrWhiteSpace(
                    entranceCustomizationData.SponsorBanners.TextureDownloadUrl0
                )
            )
            {
                instantiatedEntranceSponsorBanner00 = Instantiate<GameObject>(SponsorBanner00);
                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        instantiatedEntranceSponsorBanner00,
                        Utils.Url.GenerateURL(
                            mediaStaticURL,
                            entranceCustomizationData.SponsorBanners.TextureDownloadUrl0,
                            useCache: useCache
                        ),
                        true
                    )
                );
            }
            if (
                !string.IsNullOrWhiteSpace(
                    entranceCustomizationData.SponsorBanners.TextureDownloadUrl1
                )
            )
            {
                instantiatedEntranceSponsorBanner01 = Instantiate<GameObject>(SponsorBanner01);
                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        instantiatedEntranceSponsorBanner01,
                        Utils.Url.GenerateURL(
                            mediaStaticURL,
                            entranceCustomizationData.SponsorBanners.TextureDownloadUrl1,
                            useCache: useCache
                        ),
                        true
                    )
                );
            }
        }

        TextureRequestManager.Instance.StartServingTextureRequests();
        if (entranceCustomizationData.Webinar)
            Instantiate(EnterAuditorium);
    }

    /// <summary>
    ///     Customizes exhibition hall according to customization data parsed from JSON.
    /// </summary>
    /// <param name="layout">Layout of exhibition hall.</param>
    /// <param name="useCache">Whether to disable cache for texture requests.</param>
    private void CustomizeExhibitionHall(Layout layout, bool useCache = true)
    {
        InstantiateDefaultExhibition(layout);
        if (exhibitionCustomizationData.SponsorDisc.Purchased)
        {
            InstantiateSponsorDisc();
            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    HelperUtilities.GetChildGameObject(
                        instantiatedExhibitionSponsorDisc,
                        "sponsor_disc_rotating"
                    ),
                    Url.GenerateURL(
                        mediaStaticURL,
                        exhibitionCustomizationData.SponsorDisc.TextureDownloadUrl,
                        useCache: useCache
                    ),
                    true
                )
            );
        }
        // if the set of sponsor cylinders is purchased then instantiate them and request their textures.
        if (exhibitionCustomizationData.SponsorCylinder.Purchased)
        {
            InstantiateExhibitionSponsorCylinders(layout);
            InstantiateExhibitionSponsorCylindersObservableWaypoints(layout);
            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    HelperUtilities.GetChildGameObject(
                        instantiatedExhibitionSponsorCylinders[0],
                        "sponsor_cylinder_rotating"
                    ),
                    Url.GenerateURL(
                        mediaStaticURL,
                        exhibitionCustomizationData.SponsorCylinder.TextureDownloadUrl0,
                        useCache: useCache
                    )
                )
            );
            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    HelperUtilities.GetChildGameObject(
                        instantiatedExhibitionSponsorCylinders[1],
                        "sponsor_cylinder_rotating"
                    ),
                    Url.GenerateURL(
                        mediaStaticURL,
                        exhibitionCustomizationData.SponsorCylinder.TextureDownloadUrl1,
                        useCache: useCache
                    )
                )
            );
            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    HelperUtilities.GetChildGameObject(
                        instantiatedExhibitionSponsorCylinders[2],
                        "sponsor_cylinder_rotating"
                    ),
                    Url.GenerateURL(
                        mediaStaticURL,
                        exhibitionCustomizationData.SponsorCylinder.TextureDownloadUrl2,
                        useCache: useCache
                    )
                )
            );
            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    HelperUtilities.GetChildGameObject(
                        instantiatedExhibitionSponsorCylinders[3],
                        "sponsor_cylinder_rotating"
                    ),
                    Url.GenerateURL(
                        mediaStaticURL,
                        exhibitionCustomizationData.SponsorCylinder.TextureDownloadUrl3,
                        useCache: useCache
                    )
                )
            );
        }
        // sponsor display screen customization
        if (exhibitionCustomizationData.DisplayScreen.Purchased)
        {
            switch (exhibitionCustomizationData.HallType)
            {
                case "small_exhibition":
                    Instantiate<GameObject>(
                        sponsorDisplayScreen,
                        exhibitionHall.transform
                    ).transform.localPosition = new Vector3(0, 7.5f, 24.64f);
                    break;
                case "medium_exhibition":
                    Instantiate<GameObject>(
                        sponsorDisplayScreen,
                        exhibitionHall.transform
                    ).transform.localPosition = new Vector3(0, 7.5f, 29.64f);
                    break;

                case "large_exhibition":
                    Instantiate<GameObject>(
                        sponsorDisplayScreen,
                        exhibitionHall.transform
                    ).transform.localPosition = new Vector3(0, 7.5f, 39.64f);
                    break;
                default:
                    break;
            }
        }

        // sponsor banners instantiation and customiztion
        if (exhibitionCustomizationData.SponsorBanners.Purchased)
        {
            InstantiateSponsorBanners(layout);

            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    instantiatedExhibitionSponsorBanners[0],
                    Url.GenerateURL(
                        mediaStaticURL,
                        exhibitionCustomizationData.SponsorBanners.TextureDownloadUrl0,
                        useCache: useCache
                    )
                )
            );

            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    instantiatedExhibitionSponsorBanners[1],
                    Url.GenerateURL(
                        mediaStaticURL,
                        exhibitionCustomizationData.SponsorBanners.TextureDownloadUrl1,
                        useCache: useCache
                    )
                )
            );

            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    instantiatedExhibitionSponsorBanners[2],
                    Url.GenerateURL(
                        mediaStaticURL,
                        exhibitionCustomizationData.SponsorBanners.TextureDownloadUrl2,
                        useCache: useCache
                    )
                )
            );

            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    instantiatedExhibitionSponsorBanners[3],
                    Url.GenerateURL(
                        mediaStaticURL,
                        exhibitionCustomizationData.SponsorBanners.TextureDownloadUrl3,
                        useCache: useCache
                    )
                )
            );
        }
    }

    private void InstantiateDefaultExhibition(Layout layout)
    {
        switch (layout)
        {
            case Layout.SMALL:
                exhibitionHall = GameObject.FindGameObjectWithTag("small_exhibition");
                break;

            case Layout.MEDIUM:
                exhibitionHall = GameObject.FindGameObjectWithTag("medium_exhibition");
                break;

            case Layout.LARGE:
                exhibitionHall = GameObject.FindGameObjectWithTag("large_exhibition");
                break;

            default:
                Debug.LogError("You have specified an unrecognizable layout!");
                break;
        }

        if (exhibitionHall == null)
        {
            Debug.LogError(
                "Make sure that you have TAGGED all exhibition scene object appropriatelly! "
                    + "Take a look at where this log originated from and make sure the tags"
                    + "and the names are the same!"
            );
        }
    }

    /// <summary>
    ///     Instantiates default sponsor disk in the middle of the exhibition hall.
    /// </summary>
    private void InstantiateSponsorDisc()
    {
        instantiatedExhibitionSponsorDisc = Instantiate<GameObject>(
            sponsorDisc,
            new Vector3(.0f, 6.00f, .0f),
            Quaternion.identity
        );
    }

    /// <summary>
    ///     Instantiates four sponsor cylinders according to specified exhibition Layout.
    /// </summary>
    /// <param name="layout">Layout of the exhibition hall</param>
    private void InstantiateExhibitionSponsorCylinders(Layout layout)
    {
        switch (layout)
        {
            case Layout.SMALL:

                instantiatedExhibitionSponsorCylinders[0] = Instantiate<GameObject>(
                    sponsorCylinder00,
                    new Vector3(19.25f, .00f, 21.25f),
                    Quaternion.identity
                );
                instantiatedExhibitionSponsorCylinders[1] = Instantiate<GameObject>(
                    sponsorCylinder01,
                    new Vector3(-19.25f, .00f, 21.25f),
                    Quaternion.identity
                );

                instantiatedExhibitionSponsorCylinders[2] = Instantiate<GameObject>(
                    sponsorCylinder02,
                    new Vector3(19.25f, .00f, -21.25f),
                    Quaternion.Euler(0.00f, 180.00f, 0.00f)
                );
                instantiatedExhibitionSponsorCylinders[3] = Instantiate<GameObject>(
                    sponsorCylinder03,
                    new Vector3(-19.25f, .00f, -21.25f),
                    Quaternion.Euler(0.00f, 180.00f, 0.00f)
                );
                break;

            case Layout.MEDIUM:

                instantiatedExhibitionSponsorCylinders[0] = Instantiate<GameObject>(
                    sponsorCylinder00,
                    new Vector3(20.25f, .00f, 24.00f),
                    Quaternion.identity
                );
                instantiatedExhibitionSponsorCylinders[1] = Instantiate<GameObject>(
                    sponsorCylinder01,
                    new Vector3(-20.25f, .00f, 24.00f),
                    Quaternion.identity
                );

                instantiatedExhibitionSponsorCylinders[2] = Instantiate<GameObject>(
                    sponsorCylinder02,
                    new Vector3(20.25f, .00f, -24.00f),
                    Quaternion.Euler(0.00f, 180.00f, 0.00f)
                );
                instantiatedExhibitionSponsorCylinders[3] = Instantiate<GameObject>(
                    sponsorCylinder03,
                    new Vector3(-20.25f, .00f, -24.00f),
                    Quaternion.Euler(0.00f, 180.00f, 0.00f)
                );
                break;

            case Layout.LARGE:

                instantiatedExhibitionSponsorCylinders[0] = Instantiate<GameObject>(
                    sponsorCylinder00,
                    new Vector3(24f, .00f, 35.75f),
                    Quaternion.identity
                );
                instantiatedExhibitionSponsorCylinders[1] = Instantiate<GameObject>(
                    sponsorCylinder01,
                    new Vector3(-24f, .00f, 35.75f),
                    Quaternion.identity
                );

                instantiatedExhibitionSponsorCylinders[2] = Instantiate<GameObject>(
                    sponsorCylinder02,
                    new Vector3(24f, .00f, -35.75f),
                    Quaternion.Euler(0.00f, 180.00f, 0.00f)
                );
                instantiatedExhibitionSponsorCylinders[3] = Instantiate<GameObject>(
                    sponsorCylinder03,
                    new Vector3(-24f, .00f, -35.75f),
                    Quaternion.Euler(0.00f, 180.00f, 0.00f)
                );
                break;
        }
    }

    /// <summary>
    ///     Instantiates observable waypoints for the previously instantiated sponsor cylinders
    /// </summary>
    /// <param name="layout">Layout of the exhibition hall</param>
    private void InstantiateExhibitionSponsorCylindersObservableWaypoints(Layout layout)
    {
        switch (layout)
        {
            case Layout.SMALL:
                for (int i = 0; i < instantiatedExhibitionSponsorCylinders.Length; ++i)
                    smallExhibitonAIData.observable_waypoints.Add(
                        (instantiatedExhibitionSponsorCylinders[i].transform.position, 3.0f)
                    );
                break;

            case Layout.MEDIUM:
                for (int i = 0; i < instantiatedExhibitionSponsorCylinders.Length; ++i)
                    mediumExhibitionAIData.observable_waypoints.Add(
                        (instantiatedExhibitionSponsorCylinders[i].transform.position, 3.0f)
                    );
                break;

            case Layout.LARGE:
                for (int i = 0; i < instantiatedExhibitionSponsorCylinders.Length; ++i)
                    largeExhibitionAIData.observable_waypoints.Add(
                        (instantiatedExhibitionSponsorCylinders[i].transform.position, 3.0f)
                    );
                break;

            default:
                break;
        }
    }

    /// <summary>
    ///     Instantiates four sponsor banners according to specified exhibition layout
    /// </summary>
    /// <param name="layout">Layout of the exhibition hall</param>
    private void InstantiateSponsorBanners(Layout layout)
    {
        Quaternion _180DegreesRotation = Quaternion.Euler(0.00f, 180.00f, 0.00f);
        switch (layout)
        {
            case Layout.SMALL:
                instantiatedExhibitionSponsorBanners[0] = Instantiate<GameObject>(
                    sponsorBanner00,
                    new Vector3(11.00f, 8.00f, 20.00f),
                    _180DegreesRotation
                );
                instantiatedExhibitionSponsorBanners[1] = Instantiate<GameObject>(
                    sponsorBanner01,
                    new Vector3(-11.00f, 8.00f, 20.00f),
                    _180DegreesRotation
                );

                instantiatedExhibitionSponsorBanners[2] = Instantiate<GameObject>(
                    sponsorBanner02,
                    new Vector3(11.00f, 8.00f, -20.00f),
                    Quaternion.identity
                );
                instantiatedExhibitionSponsorBanners[3] = Instantiate<GameObject>(
                    sponsorBanner03,
                    new Vector3(-11.00f, 8.00f, -20.00f),
                    Quaternion.identity
                );
                break;

            case Layout.MEDIUM:
                instantiatedExhibitionSponsorBanners[0] = Instantiate<GameObject>(
                    sponsorBanner00,
                    new Vector3(13.00f, 8.00f, 25.00f),
                    _180DegreesRotation
                );
                instantiatedExhibitionSponsorBanners[1] = Instantiate<GameObject>(
                    sponsorBanner01,
                    new Vector3(-13.00f, 8.00f, 25.00f),
                    _180DegreesRotation
                );

                instantiatedExhibitionSponsorBanners[2] = Instantiate<GameObject>(
                    sponsorBanner02,
                    new Vector3(13.00f, 8.00f, -25.00f),
                    Quaternion.identity
                );
                instantiatedExhibitionSponsorBanners[3] = Instantiate<GameObject>(
                    sponsorBanner03,
                    new Vector3(-13.00f, 8.00f, -25.00f),
                    Quaternion.identity
                );
                break;

            case Layout.LARGE:
                instantiatedExhibitionSponsorBanners[0] = Instantiate<GameObject>(
                    sponsorBanner00,
                    new Vector3(16.00f, 8.00f, 35.00f),
                    _180DegreesRotation
                );
                instantiatedExhibitionSponsorBanners[1] = Instantiate<GameObject>(
                    sponsorBanner01,
                    new Vector3(-16.00f, 8.00f, 35.00f),
                    _180DegreesRotation
                );

                instantiatedExhibitionSponsorBanners[2] = Instantiate<GameObject>(
                    sponsorBanner02,
                    new Vector3(16.00f, 8.00f, -35.00f),
                    Quaternion.identity
                );
                instantiatedExhibitionSponsorBanners[3] = Instantiate<GameObject>(
                    sponsorBanner03,
                    new Vector3(-16.00f, 8.00f, -35.00f),
                    Quaternion.identity
                );
                break;

            default:
                break;
        }
    }

    private void InstantiateMainCharacter(Vector3 position, Quaternion rotation)
    {
        Instantiate<GameObject>(
            CharactersContainerSO.MainCharacters[MainCharacterIndex],
            position,
            rotation
        );
    }

    /// <summary>
    ///     Returns a list of StandCustomizationData objects for possible use without having to instantiate
    ///     any actual stands.
    /// </summary>
    /// <returns></returns>
    public List<StandCustomizationData> GetStandCustomizationDatas()
    {
        List<StandCustomizationData> standCustomizationDatas = new List<StandCustomizationData>();
        foreach (var stand in exhibitionCustomizationData.Stands)
        {
            StandCustomizationData standCustomizationData = new StandCustomizationData
            {
                standPosition = stand.Position,
                standID = stand.Id,
                //standCustomizationData.videoURL;
                standName = stand.StandName,
                logoDownloadURL = stand.LogoDownloadUrl
            };
            if (stand.Menu != null)
            {
                standCustomizationData.meetLink = stand.Menu.MeetLink;
                standCustomizationData.pdfLink = stand.Menu.PdfDownloadUrl;
                standCustomizationData.website = stand.Menu.Website;
                standCustomizationData.shortDescription = stand.Menu.Description;
                standCustomizationData.phoneNumber = stand.Menu.PhoneNumber;
                standCustomizationData.address = stand.Menu.Address;
            }
            standCustomizationDatas.Add(standCustomizationData);
        }
        return standCustomizationDatas;
    }

    private void OnEntranceSceneLoad()
    {
        CustomizeEntranceScene(useCache: !isThisEntrancePreview);
        if (!isThisEntrancePreview)
            if (
                SceneLayer.PreviousActiveSceneName.Equals("small_exhibition")
                || SceneLayer.PreviousActiveSceneName.Equals("medium_exhibition")
                || SceneLayer.PreviousActiveSceneName.Equals("large_exhibition")
            )
            {
                InstantiateMainCharacter(
                    new Vector3(-11.50f, 0.00f, 0.00f),
                    Quaternion.Euler(0, 90, 0)
                );
            }
            else if (SceneLayer.PreviousActiveSceneName.Equals("auditorium"))
            {
                InstantiateMainCharacter(
                    new Vector3(-4.00f, 0.00f, 17.50f),
                    Quaternion.Euler(0, -180, 0)
                );
            }
            else
            {
                InstantiateMainCharacter(
                    new Vector3(10.00f, 0.00f, 0.00f),
                    Quaternion.Euler(0, -90, 0)
                );
            }
    }

    private void OnEntranceSceneUnload()
    {
        TextureRequestManager.Instance.Abort();
    }

    private void OnExhibitionSceneLoad(Layout layout)
    {
        // texture requests are scheluded here
        CustomizeExhibitionHall(layout, useCache: !isThisExhibitionPreview);
        if (!isThisExhibitionPreview)
        {
            CustomizeStands();
            AddStandsObservableWaypoints(layout);
        }
        TextureRequestManager.Instance.StartServingTextureRequests();
        // instantiating main character
        if (!isThisExhibitionPreview)
            switch (layout)
            {
                case Layout.SMALL:
                    InstantiateMainCharacter(
                        new Vector3(10.00f, 0.00f, 0.00f),
                        Quaternion.Euler(0, -90, 0)
                    );
                    break;
                case Layout.MEDIUM:
                    InstantiateMainCharacter(
                        new Vector3(20.00f, 0.00f, 0.00f),
                        Quaternion.Euler(0, -90, 0)
                    );
                    break;
                case Layout.LARGE:
                    InstantiateMainCharacter(
                        new Vector3(25.00f, 0.00f, 0.00f),
                        Quaternion.Euler(0, -90, 0)
                    );
                    break;
                default:
                    break;
            }
    }

    private void OnExhibitionSceneUnload(Layout layout)
    {
        TextureRequestManager.Instance.Abort();
    }

    private void OnAuditoriumSceneUnload()
    {
        TextureRequestManager.Instance.Abort();
    }

    /// <summary>
    ///     Call this from frontEnd React app to refresh Exhibition Sponsor Screen's texture.
    ///     Cache is disabled when calling this function.
    /// </summary>
    public void RefrechExhibitionSponsorScreen()
    {
        // TODO: to be implemented when SponsorScreen is implemented
    }

    /// <summary>
    ///     Call this from frontEnd React app to refresh Exhibition Sponsor Banner's texture identified by
    ///     its index.
    ///     Cache is disabled when calling this function
    /// </summary>
    /// <param name="index">ID of Exhibition Sponsor Banner. Should be between 0 and 3 inclusive.</param>
    public void RefrechExhibitionSponsorBanner(int index)
    {
        if (instantiatedExhibitionSponsorBanners[index] == null)
            return;

        switch (index)
        {
            case 0:

                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        instantiatedExhibitionSponsorBanners[0],
                        Url.GenerateURL(
                            mediaStaticURL,
                            exhibitionCustomizationData.SponsorBanners.TextureDownloadUrl0,
                            useCache: false
                        ),
                        true
                    )
                );
                break;

            case 1:

                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        instantiatedExhibitionSponsorBanners[1],
                        Url.GenerateURL(
                            mediaStaticURL,
                            exhibitionCustomizationData.SponsorBanners.TextureDownloadUrl1,
                            useCache: false
                        ),
                        true
                    )
                );
                break;

            case 2:

                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        instantiatedExhibitionSponsorBanners[2],
                        Url.GenerateURL(
                            mediaStaticURL,
                            exhibitionCustomizationData.SponsorBanners.TextureDownloadUrl2,
                            useCache: false
                        ),
                        true
                    )
                );
                break;

            case 3:
                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        instantiatedExhibitionSponsorBanners[3],
                        Url.GenerateURL(
                            mediaStaticURL,
                            exhibitionCustomizationData.SponsorBanners.TextureDownloadUrl3,
                            useCache: false
                        ),
                        true
                    )
                );
                break;

            default:
                Debug.LogError("SponsorBanner index should belong to [|0, 3|]!");
                break;
        }

        TextureRequestManager.Instance.StartServingTextureRequests();
    }

    /// <summary>
    ///     Call this function from frontEnd React app to refrech exhibition Sponsor Cylinder's texture
    ///     identified by its index. Cache is disabled when calling this function.
    /// </summary>
    /// <param name="index">ID of exhibition Sponsor Cylinder. Should be between 0 and 3 inclusive.</param>
    public void RefrechExhibitionSponsorCylinder(int index)
    {
        if (instantiatedExhibitionSponsorCylinders[index] == null)
            return;

        switch (index)
        {
            case 0:

                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        HelperUtilities.GetChildGameObject(
                            instantiatedExhibitionSponsorCylinders[0],
                            "sponsor_cylinder_rotating"
                        ),
                        Url.GenerateURL(
                            mediaStaticURL,
                            exhibitionCustomizationData.SponsorCylinder.TextureDownloadUrl0,
                            useCache: false
                        ),
                        true
                    )
                );
                break;

            case 1:

                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        HelperUtilities.GetChildGameObject(
                            instantiatedExhibitionSponsorCylinders[1],
                            "sponsor_cylinder_rotating"
                        ),
                        Url.GenerateURL(
                            mediaStaticURL,
                            exhibitionCustomizationData.SponsorCylinder.TextureDownloadUrl1,
                            useCache: false
                        ),
                        true
                    )
                );
                break;

            case 2:

                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        HelperUtilities.GetChildGameObject(
                            instantiatedExhibitionSponsorCylinders[2],
                            "sponsor_cylinder_rotating"
                        ),
                        Url.GenerateURL(
                            mediaStaticURL,
                            exhibitionCustomizationData.SponsorCylinder.TextureDownloadUrl2,
                            useCache: false
                        ),
                        true
                    )
                );
                break;

            case 3:

                TextureRequestManager.Instance.AddRequest(
                    new RequestData(
                        HelperUtilities.GetChildGameObject(
                            instantiatedExhibitionSponsorCylinders[3],
                            "sponsor_cylinder_rotating"
                        ),
                        Url.GenerateURL(
                            mediaStaticURL,
                            exhibitionCustomizationData.SponsorCylinder.TextureDownloadUrl3,
                            useCache: false
                        ),
                        true
                    )
                );
                break;

            default:
                Debug.LogWarning("SponsorCylinder index should belong to [|0, 3|]!");
                break;
        }

        TextureRequestManager.Instance.StartServingTextureRequests();
    }

    /// <summary>
    ///     Call this from frontEnd React to refresh exhibition sponsor disc texture
    ///     Cache is disabled when calling this function
    /// </summary>
    public void RefrechExhibitionSponsorDisc()
    {
        if (instantiatedExhibitionSponsorDisc == null)
            return;

        TextureRequestManager.Instance.AddRequest(
            new RequestData(
                HelperUtilities.GetChildGameObject(
                    instantiatedExhibitionSponsorDisc,
                    "sponsor_disc_rotating"
                ),
                Url.GenerateURL(
                    mediaStaticURL,
                    exhibitionCustomizationData.SponsorDisc.TextureDownloadUrl,
                    useCache: false
                ),
                true
            )
        );

        TextureRequestManager.Instance.StartServingTextureRequests();
    }

    /// <summary>
    ///     Call this from frontEnd React API to refrech all textures in currrent
    ///     Exhibition room.
    /// </summary>
    public void RefrechAllExhibitionSponsorTextures()
    {
        Debug.LogWarning(
            "Please avoid calling RefrechAllExhibitionSponsorTextures" + "for performance reasons!"
        );
        RefrechExhibitionSponsorScreen();
        for (int i = 0; i < 4; ++i)
        {
            RefrechExhibitionSponsorBanner(i);
        }
        for (int i = 0; i < 4; ++i)
        {
            RefrechExhibitionSponsorCylinder(i);
        }
        RefrechExhibitionSponsorDisc();
    }

    /// <summary>
    ///     Call this function from frontEnd React app to refrech SponsorBanner texture.
    ///     Cache is disabled when calling this function
    /// </summary>
    /// <param name="index">ID of entrance sponsor banner. Should be either 0 or 1.</param>
    public void RefrechEntranceSponsorBanner(int index)
    {
        if (index == 0)
        {
            if (instantiatedEntranceSponsorBanner00 == null)
                return;

            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    instantiatedEntranceSponsorBanner00,
                    Url.GenerateURL(
                        mediaStaticURL,
                        entranceCustomizationData.SponsorBanners.TextureDownloadUrl0,
                        useCache: false
                    ),
                    true
                )
            );
        }
        else if (index == 1)
        {
            if (instantiatedEntranceSponsorBanner01 == null)
                return;

            TextureRequestManager.Instance.AddRequest(
                new RequestData(
                    instantiatedEntranceSponsorBanner01,
                    Url.GenerateURL(
                        mediaStaticURL,
                        entranceCustomizationData.SponsorBanners.TextureDownloadUrl1,
                        useCache: false
                    ),
                    true
                )
            );
        }
        else
        {
            Debug.LogError("Entrance SponsorBanner should belong to [|0, 1|]");
            return;
        }

        TextureRequestManager.Instance.StartServingTextureRequests();
    }

    /// <summary>
    ///     Call this function from frontEnd React app to refrech CubeScreen texture.
    ///     Cache is disabled when using this function
    /// </summary>
    public void RefrechEntranceCubeScreen()
    {
        if (instantiatedEntranceCubeScreen == null)
            return;

        TextureRequestManager.Instance.AddRequest(
            new RequestData(
                HelperUtilities.GetChildGameObject(instantiatedEntranceCubeScreen, "screens"),
                Url.GenerateURL(
                    mediaStaticURL,
                    entranceCustomizationData.CubeScreen.TextureDownloadUrl,
                    useCache: false
                ),
                true
            )
        );

        TextureRequestManager.Instance.StartServingTextureRequests();
    }

    /// <summary>
    ///     Call this function from frontEnd React app to refresh all sponsor textures in the Entrance scene.
    ///     This function is useful in case all sponsor textures in the Entrance scene are reset.
    ///     Chaching is disabled when using this function.
    /// </summary>
    public void RefrechAllEntranceSponsorTextures()
    {
        Debug.LogWarning(
            "Please avoid calling RefrechAllEntranceSponsorTextures" + "for performance reasons!"
        );
        RefrechEntranceSponsorBanner(0);
        RefrechEntranceSponsorBanner(1);
        RefrechEntranceCubeScreen();
    }
}
