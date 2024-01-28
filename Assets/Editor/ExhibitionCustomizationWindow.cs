using UnityEditor;
using UnityEngine;
using Common;
using UnityEditor.SceneManagement;

public class ExhibitionCustomizationWindow : EditorWindow
{
    public Layout ExhibitionLayout;

    public GameObject pack00;
    public GameObject pack01;
    public GameObject pack02;
    public GameObject pack03;

    public GameObject pack00_navcube;
    public GameObject pack01_navcube;
    public GameObject pack02_navcube;
    public GameObject pack03_navcube;

    // references initialized stands

    [MenuItem("Window/ExhibitionCustomization")]
    public static void ShowExhibitionWindow()
    {
        EditorWindow.GetWindow<ExhibitionCustomizationWindow>();
    }

    /// <summary>
    ///     Initializes default stand packs according to a layout specified by the the current exhibition scene
    /// </summary>
    void InstantiateDefaultStands()
    {
        Clear();

        Quaternion Rotation = Quaternion.identity;
        Quaternion Rotation180 = Quaternion.identity;
        Rotation180.eulerAngles = new Vector3(0, 180f, 0);

        switch (ExhibitionLayout)
        {
            case Layout.SMALL:
                for (int i = 0; i < 3; i++)
                {
                    // right side
                    GameObject RB = Instantiate<GameObject>(pack02, new Vector3(13.00f - (13 * i), 0f, 16.00f), Rotation);
                    StandsObserver.Instance.AddStandPack(
                        "RB" + i,
                        new StandPackInfo
                        {
                            ReferencedStandPack = RB,
                            Type = Pack.PACK02
                        }
                    );

                    // left side
                    GameObject LB = Instantiate<GameObject>(pack02, new Vector3(13.00f - (13 * i), 0f, -16.00f), Rotation180);
                    StandsObserver.Instance.AddStandPack(
                        "LB" + i,
                        new StandPackInfo
                        {
                            ReferencedStandPack = LB,
                            Type = Pack.PACK02
                        }
                    );
                }

                for (int i = 0; i < 2; i++)
                {
                    // right side
                    GameObject RA = Instantiate<GameObject>(pack00, new Vector3(13.00f - (26 * i), 0f, 6.50f), Rotation);
                    StandsObserver.Instance.AddStandPack(
                        "RA" + (i * 2),
                        new StandPackInfo
                        {
                            ReferencedStandPack = RA,
                            Type = Pack.PACK00
                        }
                    );

                    // left side
                    GameObject LA = Instantiate<GameObject>(pack00, new Vector3(13.00f - (26 * i), 0f, -6.50f), Rotation180);
                    StandsObserver.Instance.AddStandPack(
                        "LA" + (i * 2),
                        new StandPackInfo
                        {
                            ReferencedStandPack = LA,
                            Type = Pack.PACK00
                        }
                    );
                }

                // right side
                GameObject RA1 = Instantiate<GameObject>(pack01, new Vector3(0.00f, 0f, 5.75f), Rotation);
                StandsObserver.Instance.AddStandPack(
                    "RA1",
                     new StandPackInfo
                     {
                         ReferencedStandPack = RA1,
                         Type = Pack.PACK01
                     }
                );

                // left side
                GameObject LA1 = Instantiate<GameObject>(pack01, new Vector3(0.00f, 0f, -5.75f), Rotation180);
                StandsObserver.Instance.AddStandPack(
                    "LA1",
                        new StandPackInfo
                        {
                            ReferencedStandPack = LA1,
                            Type = Pack.PACK01
                        }
                    );
                break;

            case Layout.MEDIUM:
                for (int i = 0; i < 3; i++)
                {
                    // right side
                    GameObject RA = Instantiate<GameObject>(pack00, new Vector3(13.00f - (13 * i), 0f, 7.00f), Rotation);
                    StandsObserver.Instance.AddStandPack(
                        "RA" + i,
                        new StandPackInfo
                        {
                            ReferencedStandPack = RA,
                            Type = Pack.PACK00
                        }
                    );
                    
                    GameObject RC = Instantiate<GameObject>(pack03, new Vector3(13.00f - (13 * i), 0f, 26.50f), Rotation);
                    StandsObserver.Instance.AddStandPack(
                        "RC" + i,
                        new StandPackInfo
                        {
                            ReferencedStandPack = RC,
                            Type = Pack.PACK03
                        }
                    );
                    
                    // left side
                    GameObject LA = Instantiate<GameObject>(pack00, new Vector3(13.00f - (13 * i), 0f, -7.00f), Rotation180);
                    StandsObserver.Instance.AddStandPack(
                        "LA" + i,
                        new StandPackInfo
                        {
                            ReferencedStandPack = LA,
                            Type = Pack.PACK00
                        }
                    );

                    GameObject LC = Instantiate<GameObject>(pack03, new Vector3(13.00f - (13 * i), 0f, -26.50f), Rotation180);
                    StandsObserver.Instance.AddStandPack(
                        "LC" + i,
                        new StandPackInfo
                        {
                            ReferencedStandPack = LC,
                            Type = Pack.PACK03
                        }
                    );
                }

                for (int i = 0; i < 2; i++)
                {
                    // right side
                    GameObject RB = Instantiate<GameObject>(pack02, new Vector3(13.00f - (26 * i), 0f, 18.00f), Rotation);
                    StandsObserver.Instance.AddStandPack(
                        "RB" + (i * 2),
                        new StandPackInfo
                        {
                            ReferencedStandPack = RB,
                            Type = Pack.PACK02
                        }
                    );
                    
                    // left side
                    GameObject LB = Instantiate<GameObject>(pack02, new Vector3(13.00f - (26 * i), 0f, -18.00f), Rotation180);
                    StandsObserver.Instance.AddStandPack(
                        "LB" + (i * 2),
                        new StandPackInfo
                        {
                            ReferencedStandPack = LB,
                            Type = Pack.PACK02
                        }
                    );
                }

                // right side
                GameObject RB1 = Instantiate<GameObject>(pack01, new Vector3(0.00f, 0f, 18.00f), Rotation);
                StandsObserver.Instance.AddStandPack(
                    "RB1",
                        new StandPackInfo
                        {
                            ReferencedStandPack = RB1,
                            Type = Pack.PACK01
                        }
                    );

                // left side
                GameObject LB1 = Instantiate<GameObject>(pack01, new Vector3(0.00f, 0f, -18.00f), Rotation180);
                StandsObserver.Instance.AddStandPack(
                    "LB1",
                        new StandPackInfo
                        {
                            ReferencedStandPack = LB1,
                            Type = Pack.PACK01
                        }
                    );
                break;

            case Layout.LARGE:
                for (int i = 0; i < 4; i++)
                {
                    // right side
                    GameObject RA = Instantiate<GameObject>(pack00, new Vector3(19.50f - (i * 13), 0f, 7.00f), Rotation);
                    StandsObserver.Instance.AddStandPack(
                        "RA" + i,
                        new StandPackInfo
                        {
                            ReferencedStandPack = RA,
                            Type = Pack.PACK00
                        }
                    );

                    GameObject RC = Instantiate<GameObject>(pack02, new Vector3(19.50f - (i * 13), 0f, 27.50f), Rotation);
                    StandsObserver.Instance.AddStandPack(
                        "RC" + i,
                        new StandPackInfo
                        {
                            ReferencedStandPack = RC,
                            Type = Pack.PACK02
                        }
                    );

                    // left side
                    GameObject LA = Instantiate<GameObject>(pack00, new Vector3(19.50f - (i * 13), 0f, -7.00f), Rotation180);
                    StandsObserver.Instance.AddStandPack(
                        "LA" + i,
                        new StandPackInfo
                        {
                            ReferencedStandPack = LA,
                            Type = Pack.PACK00
                        }
                    );

                    GameObject LC = Instantiate<GameObject>(pack02, new Vector3(19.50f - (i * 13), 0f, -27.50f), Rotation180);
                    StandsObserver.Instance.AddStandPack(
                        "LC" + i,
                        new StandPackInfo
                        {
                            ReferencedStandPack = LC,
                            Type = Pack.PACK02
                        }
                    );

                }

                for (int i = 0; i < 2; i++)
                {
                    // right side
                    GameObject RB = Instantiate<GameObject>(pack01, new Vector3(6.50f - (13 * i), 0f, 18.00f), Rotation);
                    StandsObserver.Instance.AddStandPack(
                        "RB" + (i + 1),
                        new StandPackInfo
                        {
                            ReferencedStandPack = RB,
                            Type = Pack.PACK01
                        }
                    );

                    RB = Instantiate<GameObject>(pack02, new Vector3(19.50f - (39 * i), 0f, 18.00f), Rotation);
                    StandsObserver.Instance.AddStandPack(
                        "RB" + (i * 3),
                        new StandPackInfo
                        {
                            ReferencedStandPack = RB,
                            Type = Pack.PACK02
                        }
                    );

                    GameObject RD = Instantiate<GameObject>(pack03, new Vector3(15.25f - (i * 30.50f), 0f, 36.00f), Rotation);
                    StandsObserver.Instance.AddStandPack(
                        "RD" + i,
                        new StandPackInfo
                        {
                            ReferencedStandPack = RD,
                            Type = Pack.PACK03
                        }
                    );

                    // left side
                    GameObject LB = Instantiate<GameObject>(pack01, new Vector3(6.50f - (13 * i), 0f, -18.00f), Rotation180);
                    StandsObserver.Instance.AddStandPack(
                        "LB" + (i + 1),
                        new StandPackInfo
                        {
                            ReferencedStandPack = LB,
                            Type = Pack.PACK01
                        }
                    );

                    LB = Instantiate<GameObject>(pack02, new Vector3(19.50f - (39 * i), 0f, -18.00f), Rotation180);
                    StandsObserver.Instance.AddStandPack(
                        "LB" + (i * 3),
                        new StandPackInfo
                        {
                            ReferencedStandPack = LB,
                            Type = Pack.PACK02
                        }
                    );

                    GameObject LD = Instantiate<GameObject>(pack03, new Vector3(15.25f - (i * 30.50f), 0f, -36.00f), Rotation180);
                    StandsObserver.Instance.AddStandPack(
                        "LD" + i,
                        new StandPackInfo
                        {
                            ReferencedStandPack = LD,
                            Type = Pack.PACK03
                        }
                    );
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    ///     Call this to clear scene from instantiated stands.
    /// </summary>
    void Clear()
    {
        StandsObserver.Instance.Clear();
    }

    private void OnGUI()
    {
        ExhibitionLayout = (Layout)EditorGUILayout.EnumPopup(ExhibitionLayout);

        pack00 = (GameObject)EditorGUILayout.ObjectField("Stand Pack 00", pack00, typeof(GameObject), false);
        pack01 = (GameObject)EditorGUILayout.ObjectField("Stand Pack 01", pack01, typeof(GameObject), false);
        pack02 = (GameObject)EditorGUILayout.ObjectField("Stand Pack 02", pack02, typeof(GameObject), false);
        pack03 = (GameObject)EditorGUILayout.ObjectField("Stand Pack 03", pack03, typeof(GameObject), false);

        if (GUILayout.Button("Instantiate Default Stands"))
        {
            InstantiateDefaultStands();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }


        if (GUILayout.Button("Show StandsObserver Stands"))
        {
            foreach (var item in StandsObserver.Instance.Stands.Keys)
            {
                Debug.Log(item);
            }
        }

        pack00_navcube = (GameObject)EditorGUILayout.ObjectField("Stand Pack 00 NavCube", pack00_navcube, typeof(GameObject), false);
        pack01_navcube = (GameObject)EditorGUILayout.ObjectField("Stand Pack 01 NavCube", pack01_navcube, typeof(GameObject), false);
        pack02_navcube = (GameObject)EditorGUILayout.ObjectField("Stand Pack 02 NavCube", pack02_navcube, typeof(GameObject), false);
        pack03_navcube = (GameObject)EditorGUILayout.ObjectField("Stand Pack 03 NavCube", pack03_navcube, typeof(GameObject), false);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Instantiate Navigation Cubes"))
        {
            StandsObserver.Instance.AddNavmeshCubes(pack00_navcube, pack01_navcube, pack02_navcube, pack03_navcube);
        }

        if (GUILayout.Button("Clear Instantiated Stands"))
        {
            Clear();
        }
        EditorGUILayout.EndHorizontal();
    }
}
