using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Common;
using UnityEngine.UI;

public class ChoseCharacterUIManager : Singleton<ChoseCharacterUIManager>
{
    public CharactersContainer CharactersContainerSO;

    public Button OKButton;
    public Button NextCharacterButton;
    public Button PreviousCharacterButton;

    int _currentMainCharacterIndex;

    private void OnEnable()
    {
        OKButton.onClick.AddListener(OnOKButtonClick);
        NextCharacterButton.onClick.AddListener(OnNextCharacterButtonClick);
        PreviousCharacterButton.onClick.AddListener(OnPreviousCharacterButtonClick);
    }

    private void Start()
    {
        _currentMainCharacterIndex = ChoseCharacterManager.Instance.defaultMainCharacterIndex;
    }

    void OnNextCharacterButtonClick()
    {
        _currentMainCharacterIndex = (_currentMainCharacterIndex + 1) % CharactersContainerSO.MainCharacters.Count;
        ChoseCharacterManager.Instance.InstantiateMainCharacter(_currentMainCharacterIndex);
    }

    void OnPreviousCharacterButtonClick()
    {
        if (--_currentMainCharacterIndex < 0) _currentMainCharacterIndex = CharactersContainerSO.MainCharacters.Count - 1;
        ChoseCharacterManager.Instance.InstantiateMainCharacter(_currentMainCharacterIndex);
    }

    void OnOKButtonClick()
    {
        // this will load the Entrance scene and unloads the current scene.
        ChoseCharacterManager.Instance.MainCharacterIndex = _currentMainCharacterIndex;
    }
}
