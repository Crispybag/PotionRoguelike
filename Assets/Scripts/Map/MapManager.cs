using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<TempEnemy> enemies;
    public GameObject prefab;
    public GameObject dotPrefab;


    public List<GameObject> availablePositions;

    private List<GameObject> positionsToRemove = new List<GameObject>();

    SortedDictionary<int, GameObject> sortedEnemies;

    public List<GameObject> sortEnemies = new List<GameObject>();

    public List<Vector3> encounterPositions = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        Dictionary<int, GameObject> unsortedEnemies = new Dictionary<int, GameObject>();
        //loop through all enemies to spawn them
        foreach (TempEnemy enemy in enemies)
        {
            int randomPos = Random.Range(0, availablePositions.Count);
            Vector3 newPos = availablePositions[randomPos].transform.position;
            GameObject newEnemy = Instantiate(prefab);
            newEnemy.transform.position = newPos;
            unsortedEnemies.Add(int.Parse(availablePositions[randomPos].name), newEnemy);
            RemovePositions(randomPos);
        }

        //sort enemies based on spawn position (makes sure an enemy beside them is chosen)
        sortedEnemies = new SortedDictionary<int, GameObject>(unsortedEnemies);



        //temporary code to colour code the enemies
        int index = 0;
        int bleh = 0;
        foreach(KeyValuePair<int, GameObject> dunno in sortedEnemies)
        {
            sortEnemies.Add(dunno.Value);
            switch (index)
            {
                case 0:
                    dunno.Value.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 1:
                    dunno.Value.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case 2:
                    dunno.Value.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                case 3:
                    dunno.Value.GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
            }

            if (bleh % 2 == 1)
            {
                index++;
            }
            bleh++;
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
            createBracket(sortEnemies[i].transform.position, sortEnemies[i + 1].transform.position);
        }
    }

    /// <summary>
    /// Creates a bracket
    /// </summary>
    /// <param name="fPosition">Start position</param>
    /// <param name="sPosition">End position</param>
    private void createBracket(Vector3 fPosition, Vector3 sPosition)
    {
        //get the direction between the 2 points
        Vector3 pointDirection = fPosition - sPosition;
        pointDirection.Normalize();
        //then get their middle point
        Vector3 midPoint = fPosition + pointDirection * Vector3.Distance(fPosition, sPosition) / 2;

        //midpoint is the direction, because center is 0,0,0
        Vector3 midDirection = midPoint;
        midDirection.Normalize();

        //then get it around 1/3 of the middle, this is where we would bezier curve to go
        Vector3 halfway = midDirection * (Vector3.Distance(new Vector3(0, 0, 0), midPoint) / 3);

        //with this half way position, we get the position exactly halfway from the 2 points, this way it is smoother between the 2 points
        Vector3 encounterPosition = BezierCurve.getBezierPos(fPosition, halfway, sPosition, 20, 10, dotPrefab);

        //add this position to the list for later use
        encounterPositions.Add(encounterPosition);

        //create the path based on the positions
        //from the first enemy to the encounter position
        createPath(fPosition, encounterPosition);
        //and from the second enemy to the encounter position
        createPath(sPosition, encounterPosition);
    }

    /// <summary>
    /// Creates a path between the middle points
    /// </summary>
    /// <param name="fPosition"> Start position</param>
    /// <param name="encounterPosition"> The encounter position, or end position</param>
    public void createPath(Vector3 fPosition, Vector3 encounterPosition)
    {
        //Get the distance between the 2 points
        float fDistance = Vector3.Distance(fPosition, encounterPosition);

        //get the direction between the 2 points
        Vector3 fDirection = encounterPosition - fPosition;
        fDirection.Normalize();

        //create an offset
        float fOffset = fDistance / 6;
        //get a random distance, the initial distance is divided by two so its around the middle, and then +- the offset so the offset is around the middle point
        float fRandomDistance = Random.Range((fDistance / 2) - fOffset, (fDistance / 2) + fOffset);

        //get the middle point based on the distance
        Vector3 fLineMiddlePoint = fPosition + fDirection * fRandomDistance;


        //generate random number to decide if left or right first
        int randDirection = Random.Range(0, 2);

        Vector3 ffNormalDirection;
        Vector3 fsNormalDirection;

        //using the random direction we set the normals of the line
        if (randDirection == 0)
        {
            ffNormalDirection = new Vector3(fDirection.y * -1, fDirection.x, fDirection.z);
            fsNormalDirection = new Vector3(fDirection.y, fDirection.x * -1, fDirection.z);
        }
        else
        {
            ffNormalDirection = new Vector3(fDirection.y, fDirection.x * -1, fDirection.z);
            fsNormalDirection = new Vector3(fDirection.y * -1, fDirection.x, fDirection.z);
        }

        //with the new position and the normals we generate a new line segment, because the bezier curve can only take 3 inputs
        createLineSegment(fPosition, fLineMiddlePoint, ffNormalDirection);
        createLineSegment(fLineMiddlePoint, encounterPosition, fsNormalDirection);

    }

    /// <summary>
    /// Creates a line segment
    /// </summary>
    /// <param name="pointA">Start position</param>
    /// <param name="pointB">End position</param>
    /// <param name="normalDirection">The normal direction of which way the curve goes</param>
    public void createLineSegment(Vector3 pointA, Vector3 pointB, Vector3 normalDirection)
    {
        //get the direction between the 2 points
        Vector3 fDirection = pointB - pointA;
        fDirection.Normalize();

        //get the distance between the 2 points
        float f3Distance = Vector3.Distance(pointA, pointB);

        //generate an offset based on the distance
        float f3Offset = f3Distance / 4;
        //get a random distance, the initial distance is divided by two so its around the middle, and then +- the offset so the offset is around the middle point
        float f3RandomDistance = Random.Range((f3Distance / 2) - f3Offset, (f3Distance / 2) + f3Offset);

        //get the middle point of the line segment point using the random distance
        Vector3 f3LineMiddlePoint = pointA + fDirection * f3RandomDistance;

        //then we generate a new point based on the calculate middle point, and add the normal line distance to it, this generates the third point we need to create a bezier
        Vector3 fsNormalPoint = f3LineMiddlePoint + normalDirection * f3Distance / 2;

        //generate the bezier using the 3 points
        BezierCurve.CreateBezier(pointA, fsNormalPoint, pointB, dotPrefab);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AdvanceBrackets();
        }
    }

    private void AdvanceBrackets()
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
        List<Vector3> encounterPos = new List<Vector3>(encounterPositions);
        //clear it
        encounterPositions = new List<Vector3>();

        //we needed to copy it, since we adjust the original list size
        for (int i = 0; i < encounterPos.Count; i += 2)
        {
            //fill the encounter positions with new values, based on the previous encounter positions (new brackets)
            createBracket(encounterPos[i], encounterPos[i + 1]);
        }
    }




    private void RemovePositions(int randomPos)
    {
        //removing the current position, but also around it, so they dont spawn too close to eachother

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
