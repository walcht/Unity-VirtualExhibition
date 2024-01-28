using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIRelatedData", menuName = "SO/AIRelatedData")]
public class AIRelatedData : ScriptableObject
{
    public List<Vector3> spawnPositions                                             = new List<Vector3>();
    public List<(Vector3 position, float observingDistance)> observable_waypoints   = new List<(Vector3 position, float observing)>();
}
