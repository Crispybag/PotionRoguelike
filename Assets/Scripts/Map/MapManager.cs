using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapManager : MonoBehaviour
{
    public List<SO_Enemy> enemies;
    public GameObject prefab;
    public GameObject dotPrefab;

    public SO_MapManager mapManager;

    public List<GameObject> availablePositions;



    SortedDictionary<int, GameObject> sortedEnemies;

    public List<GameObject> sortEnemies = new List<GameObject>();

    public List<Vector3> encounterPositions = new List<Vector3>();


    private void Start()
    {
        sortedEnemies = new SortedDictionary<int, GameObject>();
        BezierCurve.dotPrefab = dotPrefab;
        if (mapManager.enemies != null && mapManager.enemies.Count != 0)
        {
            //setup();
            ReadMap();
        }
    }

    public void ClearMap()
    {
        Debug.Log("Clearing map!");
        mapManager.enemies.Clear();
        mapManager.beziers.Clear();
        BezierCurve.ClearBezierCurves();
        mapManager.publicEnemies.Clear();
    }


    public void ReadMap()
    {
        //read values from map
        //Initialize them
        //create enemies
        //decide who wins
        //create new beziers
        //upload new data to map

        int i = 0;
        Dictionary<int, GameObject> newEnemies = new Dictionary<int, GameObject>();
        foreach (KeyValuePair<SO_Enemy, Vector3> enemy in mapManager.enemies)
        {
            GameObject newEnemy = Instantiate(prefab);
            newEnemy.GetComponent<MapEnemy>().enemy = enemy.Key;
            newEnemy.GetComponent<MapEnemy>().Setup();
            newEnemy.transform.position = enemy.Value;

            newEnemies.Add(i, newEnemy);
            i++;
        }

        foreach (KeyValuePair<int, GameObject> enemy in newEnemies)
        {
            sortEnemies.Add(enemy.Value);
        }
        Debug.Log("Found " + mapManager.beziers.Count + " bezier curves to spawn!");
        foreach (bezierCurves curve in mapManager.beziers)
        {
            Debug.Log("This curve has " + curve.points.Length + " points");
            BezierCurve.CreateDoubleBezier(curve.points[0], curve.points[1], curve.points[2], curve.points[3], curve.points[4], dotPrefab);
        }
        BezierCurve.createBezierDotsInstant();

    }











    public void setup()
    {
        if (mapManager.enemies != null && mapManager.enemies.Count != 0)
        {
            Dictionary<SO_Enemy, Vector3> newEnemies = new Dictionary<SO_Enemy, Vector3>();
            foreach (KeyValuePair<SO_Enemy, Vector3> enemy in mapManager.enemies)
            {
                GameObject newEnemy = Instantiate(prefab);
                newEnemy.GetComponent<MapEnemy>().enemy = enemy.Key;
                newEnemy.GetComponent<MapEnemy>().Setup();
                newEnemy.transform.position = enemy.Value;
                newEnemies.Add(enemy.Key, enemy.Value);
            }
            mapManager.enemies.Clear();
            mapManager.enemies = newEnemies;
        }
        else
        {
            test();
        }
    }


    public void test()
    {
        //spawn 8, later needs to be 7 so the player also spawns
        for(int i =0; i <= 7; i++)
        {
            int randomPos = Random.Range(0, availablePositions.Count);
            Vector3 newPos = availablePositions[randomPos].transform.position;
            GameObject newEnemy = Instantiate(prefab);

            int randomEnemy = Random.Range(0, enemies.Count);
            newEnemy.GetComponent<MapEnemy>().enemy = enemies[randomEnemy];
            enemies.Remove(enemies[randomEnemy]);

            newEnemy.GetComponent<MapEnemy>().Setup();
            newEnemy.transform.position = newPos;
            Debug.Log("New enemy at : " + newPos);
            sortedEnemies.Add(int.Parse(availablePositions[randomPos].name), newEnemy);
            RemovePositions(randomPos);
        }

        foreach (KeyValuePair<int, GameObject> enemy in sortedEnemies)
        {
            sortEnemies.Add(enemy.Value);
        }

        CreateStartingBrackets();
    }




    /// <summary>
    /// Creates the initial brackets between enemies
    /// </summary>
    public void CreateStartingBrackets()
    {
        for (int i = 0; i < sortEnemies.Count; i += 2)
        {
            CreateBrackets(sortEnemies[i].transform.position, sortEnemies[i + 1].transform.position, sortEnemies[i].GetComponent<MapEnemy>().enemy, sortEnemies[i + 1].GetComponent<MapEnemy>().enemy, i);
/*            Vector3[] positions = BezierCurve.createBracket(sortEnemies[i].transform.position, sortEnemies[i + 1].transform.position);

            Vector3 firstEndpoint = BezierCurve.getBezierPos(positions[2], positions[3], positions[4], 0.75f);
            Vector3 secondEndpoint = BezierCurve.getBezierPos(positions[7], positions[8], positions[9], 0.75f);



            mapManager.enemies.Add(sortEnemies[i].GetComponent<MapEnemy>().enemy, firstEndpoint);
            mapManager.enemies.Add(sortEnemies[i + 1].GetComponent<MapEnemy>().enemy, secondEndpoint);

            mapManager.publicEnemies.Add(sortEnemies[i].GetComponent<MapEnemy>().enemy);
            mapManager.publicEnemies.Add(sortEnemies[i + 1].GetComponent<MapEnemy>().enemy);

            bezierCurves firstBezier = new bezierCurves();
            firstBezier.points = new[] { positions[0], positions[1], positions[2], positions[3], firstEndpoint };
            bezierCurves secondBezier = new bezierCurves();
            secondBezier.points = new[] { positions[5], positions[6], positions[7], positions[8], secondEndpoint };
            mapManager.beziers.Add(firstBezier);
            mapManager.beziers.Add(secondBezier);

            sortEnemies[i].GetComponent<MapEnemy>().SetWalkPath(positions[0], positions[1], positions[2], positions[3], firstEndpoint);
            sortEnemies[i + 1].GetComponent<MapEnemy>().SetWalkPath(positions[5], positions[6], positions[7], positions[8], secondEndpoint);*/
        }
        BezierCurve.StartDotting();
    }



    public void AdvanceBrackets()
    {
        List<GameObject> removable = new List<GameObject>();

        //for now we randomize it, later on look at rivals, see who wins
        for (int i = 0; i < sortEnemies.Count; i += 2)
        {
            int random = Random.Range(0, 2);
            removable.Add(sortEnemies[i + random]);
        }

        //then remove them from list
        for (int j = 0; j < removable.Count; j++)
        {
            sortEnemies.Remove(removable[j]);
            Destroy(removable[j]);
        }
        AdvancePaths();
    }

    private void AdvancePaths()
    {
        //copy over current encounter positions
        List<Vector3> encounterPos = new List<Vector3>();
        List<SO_Enemy> soEnemies = new List<SO_Enemy>();

        foreach (GameObject enemy in sortEnemies)
        {
            encounterPos.Add(enemy.transform.position);
            soEnemies.Add(enemy.GetComponent<MapEnemy>().enemy);
        }

        //clear it
        mapManager.publicEnemies.Clear();
        mapManager.enemies.Clear();

        //we needed to copy it, since we adjust the original list size
        for (int i = 0; i < encounterPos.Count; i += 2)
        {
            CreateBrackets(encounterPos[i], encounterPos[i + 1], soEnemies[i], soEnemies[i+1], i);
            //fill the encounter positions with new values, based on the previous encounter positions (new brackets)
            //createBracket(encounterPos[i], encounterPos[i + 1]);
/*            Vector3[] positions = BezierCurve.createBracket(encounterPos[i], encounterPos[i + 1]);

            Vector3 firstEndpoint = BezierCurve.getBezierPos(positions[2], positions[3], positions[4], 0.75f);
            Vector3 secondEndpoint = BezierCurve.getBezierPos(positions[7], positions[8], positions[9], 0.75f);

            mapManager.enemies.Add(soEnemies[i], firstEndpoint);
            mapManager.enemies.Add(soEnemies[i + 1], secondEndpoint);

            mapManager.publicEnemies.Add(soEnemies[i]);
            mapManager.publicEnemies.Add(soEnemies[i+ 1]);

            bezierCurves firstBezier = new bezierCurves();
            firstBezier.points = new[] { positions[0], positions[1], positions[2], positions[3], firstEndpoint };
            bezierCurves secondBezier = new bezierCurves();
            secondBezier.points = new[] { positions[5], positions[6], positions[7], positions[8], secondEndpoint };
            mapManager.beziers.Add(firstBezier);
            mapManager.beziers.Add(secondBezier);

            sortEnemies[i].GetComponent<MapEnemy>().SetWalkPath(positions[0], positions[1], positions[2], positions[3], firstEndpoint);
            sortEnemies[i + 1].GetComponent<MapEnemy>().SetWalkPath(positions[5], positions[6], positions[7], positions[8], secondEndpoint);*/
        }

        BezierCurve.StartDotting();
    }


    public void CreateBrackets(Vector3 pos1, Vector3 pos2, SO_Enemy soEnemy1, SO_Enemy soEnemy2, int i)
    {
        Vector3[] positions = BezierCurve.createBracket(pos1, pos2);

        Vector3 firstEndpoint = BezierCurve.getBezierPos(positions[2], positions[3], positions[4], 0.75f);
        Vector3 secondEndpoint = BezierCurve.getBezierPos(positions[7], positions[8], positions[9], 0.75f);

        mapManager.enemies.Add(soEnemy1, firstEndpoint);
        mapManager.enemies.Add(soEnemy2, secondEndpoint);

        mapManager.publicEnemies.Add(soEnemy1);
        mapManager.publicEnemies.Add(soEnemy2);

        bezierCurves firstBezier = new bezierCurves();
        firstBezier.points = new[] { positions[0], positions[1], positions[2], positions[3], firstEndpoint };
        bezierCurves secondBezier = new bezierCurves();
        secondBezier.points = new[] { positions[5], positions[6], positions[7], positions[8], secondEndpoint };
        mapManager.beziers.Add(firstBezier);
        mapManager.beziers.Add(secondBezier);

        sortEnemies[i].GetComponent<MapEnemy>().SetWalkPath(positions[0], positions[1], positions[2], positions[3], firstEndpoint);
        sortEnemies[i + 1].GetComponent<MapEnemy>().SetWalkPath(positions[5], positions[6], positions[7], positions[8], secondEndpoint);
    }




    private void RemovePositions(int randomPos)
    {
        //removing the current position, but also around it, so they dont spawn too close to eachother
        List<GameObject> positionsToRemove = new List<GameObject>();
        //check if its the last one in the list
        if (randomPos == availablePositions.Count - 1)
        {
            positionsToRemove.Add(availablePositions[0]);
        }
        //if not just +1
        else
        {
            positionsToRemove.Add(availablePositions[randomPos + 1]);
        }
        //check if its the first on in the lest
        if (randomPos == 0)
        {
            positionsToRemove.Add(availablePositions[availablePositions.Count  - 1]);
        }
        //else just -1
        else
        {
            positionsToRemove.Add(availablePositions[randomPos - 1]);
        }
        //add the original position
        positionsToRemove.Add(availablePositions[randomPos]);

        //remove each of the positions
        foreach (GameObject position in positionsToRemove)
        {
            availablePositions.Remove(position);
        }
        positionsToRemove.Clear();
    }

}
