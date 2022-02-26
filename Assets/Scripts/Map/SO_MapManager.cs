using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class bezierCurves
{
    public Vector3[] points;
}



[CreateAssetMenu(menuName = "Manager/MapManager")]
public class SO_MapManager : ScriptableObject
{
    public Dictionary<SO_Enemy, Vector3> enemies = new Dictionary<SO_Enemy, Vector3>();
    [SerializeField]public List<bezierCurves> beziers = new List<bezierCurves>();
    [SerializeField] public List<SO_Enemy> publicEnemies = new List<SO_Enemy>();
}
