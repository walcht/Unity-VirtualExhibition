using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersContainer", menuName = "SO/CharactersContainer")]
public class CharactersContainer : ScriptableObject
{
    public List<GameObject> DefaultCharacters = new List<GameObject>();
    public List<GameObject> MainCharacters = new List<GameObject>();
    public List<GameObject> AICharacters = new List<GameObject>();
}
