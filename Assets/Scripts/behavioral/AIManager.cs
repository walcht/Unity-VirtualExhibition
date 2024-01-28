using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Common;
using UnityEngine.AI;

public class AIManager : SafeSingleton<AIManager>
{
    public int              numberOfNPCs;                                               // number of NPC characters to instantiate from the list of NPC characters
    public List<GameObject> AICharacters;

    public AIRelatedData smallExhibitionSpawns;
    public AIRelatedData mediumExhibtionSpawns;
    public AIRelatedData largeExhibitionSpawns;

    public SceneManagementLayer sceneManagementLayer;

    private List<GameObject> instantiatedAgents = new List<GameObject>();

    private void OnEnable()
    {
        if (    smallExhibitionSpawns.spawnPositions.Count < numberOfNPCs ||
                mediumExhibtionSpawns.spawnPositions.Count < numberOfNPCs ||
                largeExhibitionSpawns.spawnPositions.Count < numberOfNPCs)
        {
            Debug.LogWarning("Number of AI characters provided is greater than the number of spawns in one of the exhibition scenes.");
        }

        sceneManagementLayer.ExhibitionSceneLoadedEvent += OnExhibitionSceneLoaded;
        sceneManagementLayer.EntranceSceneLoadedEvent += OnEntranceSceneLoaded;
    }

    private void OnDisable()
    {
        sceneManagementLayer.ExhibitionSceneLoadedEvent -= OnExhibitionSceneLoaded;
        sceneManagementLayer.EntranceSceneLoadedEvent -= OnEntranceSceneLoaded;
    }

    private void OnExhibitionSceneLoaded(Layout layout)
    {
        List<Vector3> remainingSpawnPositions;

        switch (layout)
        {
            case Layout.SMALL:

                // make a copy of SpawnPositions so that no two characters spawn in the same position
                remainingSpawnPositions = new List<Vector3>(smallExhibitionSpawns.spawnPositions);

                // Instantiate AI agents randomly in spawn positions
                for (int nbrNPCsToInstantiate = numberOfNPCs, index = 0; nbrNPCsToInstantiate > 0; --nbrNPCsToInstantiate, ++index)
                {
                    if (index == AICharacters.Count) index = 0;

                    GameObject instantiatedAgent = Instantiate<GameObject>(AICharacters[index]);
                    instantiatedAgent.GetComponent<ExhibitionFSM>().aiRelatedData = smallExhibitionSpawns;

                    instantiatedAgents.Add(instantiatedAgent);

                    int randIndex = Random.Range(0, remainingSpawnPositions.Count);

                    instantiatedAgent.transform.position = remainingSpawnPositions[randIndex];
                    remainingSpawnPositions.RemoveAt(randIndex);
                }

                break;

            case Layout.MEDIUM:

                // make a copy of SpawnPositions so that no two characters spawn in the same position
                remainingSpawnPositions = new List<Vector3>(mediumExhibtionSpawns.spawnPositions);

                // Instantiate AI agents randomly in spawn positions
                for (int nbrNPCsToInstantiate = numberOfNPCs, index = 0; nbrNPCsToInstantiate > 0; --nbrNPCsToInstantiate, ++index)
                {
                    if (index == AICharacters.Count) index = 0;

                    GameObject instantiatedAgent = Instantiate<GameObject>(AICharacters[index]);
                    instantiatedAgent.GetComponent<ExhibitionFSM>().aiRelatedData = mediumExhibtionSpawns;

                    instantiatedAgents.Add(instantiatedAgent);

                    int randIndex = Random.Range(0, remainingSpawnPositions.Count);

                    instantiatedAgent.transform.position = remainingSpawnPositions[randIndex];
                    remainingSpawnPositions.RemoveAt(randIndex);
                }
                break;

            case Layout.LARGE:

                // make a copy of SpawnPositions so that no two characters spawn in the same position
                remainingSpawnPositions = new List<Vector3>(largeExhibitionSpawns.spawnPositions);

                // Instantiate AI agents randomly in spawn positions
                for (int nbrNPCsToInstantiate = numberOfNPCs, index = 0; nbrNPCsToInstantiate > 0; --nbrNPCsToInstantiate, ++index)
                {
                    if (index == AICharacters.Count) index = 0;

                    GameObject instantiatedAgent = Instantiate<GameObject>(AICharacters[index]);
                    instantiatedAgent.GetComponent<ExhibitionFSM>().aiRelatedData = largeExhibitionSpawns;

                    instantiatedAgents.Add(instantiatedAgent);

                    int randIndex = Random.Range(0, remainingSpawnPositions.Count);

                    instantiatedAgent.transform.position = remainingSpawnPositions[randIndex];
                    remainingSpawnPositions.RemoveAt(randIndex);
                }

                break;

            default:
                break;
        }
    }

    private void OnEntranceSceneLoaded()
    {

    }

}
