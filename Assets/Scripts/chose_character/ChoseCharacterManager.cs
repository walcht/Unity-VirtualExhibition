using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using DesignPatterns;

public class ChoseCharacterManager : Singleton<ChoseCharacterManager>
{
    public SceneManagementLayer SceneManagementLayerSO;
    public int defaultMainCharacterIndex;
    public CharactersContainer CharactersContainerSO;


    int _mainCharacterIndex;
    GameObject instantiatedMainCharacter;

    private void Start()
    {
        _mainCharacterIndex = defaultMainCharacterIndex;
        InstantiateMainCharacter(defaultMainCharacterIndex);
    }

    /// <summary>
    ///     Instantiates Main Character whose index is in the mainCharacters list.
    ///     The previously instantiated character is destroyed.
    /// </summary>
    /// <param name="index">ID of the main character to be instantiated. See list of mainCharacters of characters IDs.</param>
    public void InstantiateMainCharacter(int index)
    {
        if ((index < 0) || (index >= CharactersContainerSO.MainCharacters.Count)) Debug.LogError("Provided Main Character Index is out of range!");
        if (instantiatedMainCharacter != null) Destroy(instantiatedMainCharacter);
        instantiatedMainCharacter = Instantiate<GameObject>(CharactersContainerSO.DefaultCharacters[index], Vector3.zero, Quaternion.identity);
    }

    public int MainCharacterIndex
    {
        get { return _mainCharacterIndex; }
        set
        {
            if ( (value >= CharactersContainerSO.MainCharacters.Count) || (value < 0))
            {
                Debug.LogError("Provided character index is out of range!");
            }
            else
            {
                _mainCharacterIndex = value;
                CustomizationManager.Instance.MainCharacterIndex = _mainCharacterIndex;
                SceneManagementLayerSO.LoadEntranceScene();
            }
        }
    }
}
