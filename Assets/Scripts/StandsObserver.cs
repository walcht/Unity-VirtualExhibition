using System.Collections.Generic;
using Common;
using DesignPatterns;
using UnityEngine;

[System.Serializable]
public class StandsObserver : Singleton<StandsObserver>
{
    // holds references to all instantiated stands
    [SerializeField]
    DictionaryStringStandInfo stands = new DictionaryStringStandInfo();

    // holds references to all instantiated stand packs
    [SerializeField]
    Dictionary<string, StandPackInfo> standPackReferences = new Dictionary<string, StandPackInfo>();

    /// <summary>
    ///     Get information about a stand identified by its position in the layout.
    /// </summary>
    /// <param name="id_name">Position of stand in the layout. This uniquely identifies each stand.</param>
    /// <returns>Information about the requested stand if it exists, throws an error otherwise.</returns>
    public StandInfo GetStand(string id_name)
    {
        if (stands.ContainsKey(id_name))
            return stands[id_name];

        Debug.LogError(
            "You are trying to get a stand that doesn't exist! Make sure you're only requesting"
                + " initialized stands!"
        );
        return new StandInfo { };
    }

    /// <summary>
    ///     Destroys all GameObject this observer references and clears its containers.
    /// </summary>
    public void Clear()
    {
        Debug.Log(stands.Count);
        foreach (var standpack in standPackReferences.Values)
            DestroyImmediate(standpack.ReferencedStandPack);

        stands.Clear();
        standPackReferences.Clear();
    }

    /// <summary>
    ///     Initializes stand and adds it to StandObserver's stands container.
    /// </summary>
    /// <param name="id_name">Name identifying the stand. This name is the same as the position of the stand
    /// in the layout.</param>
    /// <param name="stand_info">Information about the stand.</param>
    private void AddStand(string id_name, StandInfo stand_info)
    {
        if (!stands.ContainsKey(id_name))
        {
            // setting the stand's GameObject name
            stand_info.ReferencedStand.name = id_name;

            stands.Add(id_name, stand_info);
            return;
        }

        Debug.LogError("You are adding a stand that already exits!");
    }

    /// <summary>
    ///     Names the Stand Pack GameObject, initializes child stands of a stand pack and adds them to
    ///     StandObserver's stands container. Also adds this pack to a container.
    /// </summary>
    /// <param name="standPackReference">Information about the Stand pack to be initialized.</param>
    public void AddStandPack(string id_name, StandPackInfo standPackInfo)
    {
        standPackReferences.Add(id_name, standPackInfo);

        // setting Stand Pack GameObject name
        standPackInfo.ReferencedStandPack.name = id_name;

        IPackStands IPackComponent = standPackInfo.ReferencedStandPack.GetComponent<IPackStands>();
        if (IPackComponent == null)
            Debug.LogError(
                "Make sure each Stand Pack has an IPackStands componenent attached to it!"
            );

        for (int i = 0; i < 4; i++)
            if (IPackComponent.GetChildStand(i) != null)
                AddStand(id_name + string.Format("{0}", i), IPackComponent.GetChildStand(i));
    }

    /// <summary>
    ///     Initializes navigation mesh cubes for stand packs.
    ///     Call this function before baking a navmesh.
    /// </summary>
    /// <param name="pack00_navcube">PACK00 Navmesh cube.</param>
    /// <param name="pack01_navcube">PACK01 Navmesh cube.</param>
    /// <param name="pack02_navcube">PACK02 Navmesh cube.</param>
    /// <param name="pack03_navcube">PACK03 Navmesh cube.</param>
    public void AddNavmeshCubes(
        GameObject pack00_navcube,
        GameObject pack01_navcube,
        GameObject pack02_navcube,
        GameObject pack03_navcube
    )
    {
        foreach (var standPackReference in standPackReferences.Values)
        {
            switch (standPackReference.Type)
            {
                case Pack.PACK00:
                    Instantiate<GameObject>(
                        pack00_navcube,
                        standPackReference.ReferencedStandPack.transform
                    );
                    break;

                case Pack.PACK01:
                    Instantiate<GameObject>(
                        pack01_navcube,
                        standPackReference.ReferencedStandPack.transform
                    );
                    break;

                case Pack.PACK02:
                    Instantiate<GameObject>(
                        pack02_navcube,
                        standPackReference.ReferencedStandPack.transform
                    );
                    break;

                case Pack.PACK03:
                    Instantiate<GameObject>(
                        pack03_navcube,
                        standPackReference.ReferencedStandPack.transform
                    );
                    break;

                default:
                    break;
            }
        }
    }

    /// <summary>
    ///     Makes the purchased stand interactive. In other terms, after calling this functions,
    ///     the MainCharacter is able to interact with the stand. Make sure to call this
    ///     for each purchased stand!
    /// </summary>
    /// <param name="id_name">The position of the stand in the layout. This acts as an identifier for
    /// the stand.</param>
    public void SetStandAsInteractive(string id_name)
    {
        if (!stands.ContainsKey(id_name))
        {
            Debug.LogError(
                "You can only SetStandAsInteractive when the stand is already referenced!"
            );
            return;
        }

        GameObject interactiveStand = stands[id_name].ReferencedStand;
        if (interactiveStand == null)
        {
            Debug.LogError("Stand name doesn't exist in StandsObserver's database!");
            return;
        }

        StandHelper tmp_stand = interactiveStand.GetComponent<StandHelper>();
        tmp_stand.standCustomizationData.standPosition = id_name;

        // adding stand collider(s) to be able to start an interaction with the stand
        interactiveStand.GetComponent<StandHelper>().InstantiateStandColliders();
    }

    public void SetStandCharacter(
        string id_name,
        GameObject character_00,
        GameObject character_01 = null
    )
    {
        if (!stands.ContainsKey(id_name))
        {
            Debug.LogError("You can only SetStandCharacter when the stand is already referenced!");
            return;
        }
        stands[id_name]
            .ReferencedStand.GetComponent<StandHelper>()
            .SetStandCharacter(character_00, character_01);
    }

    /// <summary>
    ///     Set stand customization data for the stand identified by its position. Logs an error if the
    ///     stand doesn't exit in the database of StandObserver.
    /// </summary>
    /// <param name="id_name">The position of the stand in the layout. This acts as an identifier for
    /// the stand.</param>
    /// <param name="_standCustomizationData">Data class hlding stand customization data.</param>
    public void SetStandCustomizationData(
        string id_name,
        StandCustomizationData _standCustomizationData
    )
    {
        if (stands.ContainsKey(id_name))
        {
            stands[id_name].ReferencedStand.GetComponent<StandHelper>().standCustomizationData =
                _standCustomizationData;
            return;
        }
        Debug.LogError("Accessing an un-initialized stand!");
    }

    public Dictionary<string, StandInfo> Stands
    {
        get { return stands; }
    }
}
