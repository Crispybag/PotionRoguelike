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

        //CreateBezier();
        Temp();
    }


    public void Temp()
    {
        for (int i = 0; i < sortEnemies.Count; i += 2)
        {

            Vector3 fPosition = sortEnemies[i].transform.position;
            Vector3 sPosition = sortEnemies[i + 1].transform.position;


            Vector3 pointDirection = fPosition - sPosition;
            pointDirection.Normalize();
            Vector3 midPoint = fPosition + pointDirection * Vector3.Distance(fPosition, sPosition) / 2;

            //midpoint is the direction, because center is 0,0,0
            Vector3 midDirection = midPoint;
            midDirection.Normalize();

            Vector3 halfway = midDirection * (Vector3.Distance(new Vector3(0, 0, 0), midPoint) / 3);



            Vector3 encounterPosition = BezierCurve.getBezierPos(fPosition, halfway, sPosition, 20, 10, dotPrefab);

            encounterPositions.Add(encounterPosition);

            createPath(fPosition,encounterPosition);
            createPath(sPosition, encounterPosition);

        }
    }

    public void createPath(Vector3 fPosition, Vector3 encounterPosition)
    {
        //First line
        float fDistance = Vector3.Distance(fPosition, encounterPosition);
        //minus encounter position, because 0,0,0 is in the middle.
        Vector3 fDirection = encounterPosition - fPosition;
        fDirection.Normalize();

        float fOffset = fDistance / 6;
        float fRandomDistance = Random.Range((fDistance / 2) - fOffset, (fDistance / 2) + fOffset);


        Vector3 fLineMiddlePoint = fPosition + fDirection * fRandomDistance;


        //generate random number to decide if left or right first
        int randDirection = Random.Range(0, 2);

        Vector3 ffNormalDirection;
        Vector3 fsNormalDirection;

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



        //first section of the first line
        float f2Distance = Vector3.Distance(fPosition, fLineMiddlePoint);

        float f2Offset = f2Distance / 4;
        float f2RandomDistance = Random.Range((f2Distance / 2) - f2Offset, (f2Distance / 2) + f2Offset);

        Vector3 f2LineMiddlePoint = fPosition + fDirection * f2RandomDistance;


        Vector3 ffNormalPoint = f2LineMiddlePoint + ffNormalDirection * f2Distance / 2;

        BezierCurve.createBezier(fPosition, ffNormalPoint, fLineMiddlePoint, dotPrefab);

        //second section of the first line
        float f3Distance = Vector3.Distance(fLineMiddlePoint, encounterPosition);

        float f3Offset = f3Distance / 4;
        float f3RandomDistance = Random.Range((f3Distance / 2) - f3Offset, (f3Distance / 2) + f3Offset);

        Vector3 f3LineMiddlePoint = fLineMiddlePoint + fDirection * f3RandomDistance;


        Vector3 fsNormalPoint = f3LineMiddlePoint + fsNormalDirection * f3Distance / 2;


        BezierCurve.createBezier(fLineMiddlePoint, fsNormalPoint, encounterPosition, dotPrefab);

    }

    public void CreateBezier()
    {
        for (int i = 0; i < sortEnemies.Count; i += 2)
        {
            //middle point
            //distance
            //get middle of distance + offset
            //get normals between middle and start point, and normal between middle and end point
            //random middle for both sides,
            //random distance
            Vector3 middlePos = BezierCurve.getBezierPos(sortEnemies[i].transform.position, new Vector3(0, 0, 0), sortEnemies[i + 1].transform.position, 20, 10, dotPrefab);
            //BezierCurve.createBezier(sortEnemies[i].transform.position, sortEnemies[i + 1].transform.position, middlePos, 10, dotPrefab);

            float distanceSM = Vector3.Distance(sortEnemies[i].transform.position, middlePos);
            float distanceEM = Vector3.Distance(sortEnemies[i + 1].transform.position, middlePos);

            float offset = distanceSM / 6;
            float randomMiddle = Random.Range(distanceSM / 2 - offset, distanceSM / 2 + offset);
            Vector3 direction = sortEnemies[i + 1].transform.position - sortEnemies[i].transform.position;
            Vector3 midPointFL = sortEnemies[i].transform.position + direction * (randomMiddle / distanceSM);


            Vector3 midPointSM = new Vector3(0, 0, 0);
            Vector3 midPointEM = new Vector3(0, 0, 0);

            //decide which way it goes first
            int rand = Random.Range(0, 1);
            if (rand == 0)
            {

                float disFL = Vector3.Distance(sortEnemies[i].transform.position, midPointFL);
                float FLoffset = disFL / 6;
                float randomFLMiddle = Random.Range(disFL / 2 - FLoffset, disFL / 2 + FLoffset);
                midPointSM = sortEnemies[i].transform.position + direction * (randomFLMiddle / disFL) / 2;


                Vector3 normal = new Vector3(direction.y * -1, direction.x, direction.z);

                midPointSM += normal;
            }
            else
            {

            }

            BezierCurve.createBezier(sortEnemies[i].transform.position, midPointSM, midPointFL, 10, dotPrefab);

        }
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
        List<Vector3> newEncounterPositions = new List<Vector3>();

        for (int i = 0; i < encounterPositions.Count; i += 2)
        {

            Vector3 fPosition = encounterPositions[i];
            Vector3 sPosition = encounterPositions[i + 1];


            Vector3 pointDirection = fPosition - sPosition;
            pointDirection.Normalize();
            Vector3 midPoint = fPosition + pointDirection * Vector3.Distance(fPosition, sPosition) / 2;

            //midpoint is the direction, because center is 0,0,0
            Vector3 midDirection = midPoint;
            midDirection.Normalize();

            Vector3 halfway = midDirection * (Vector3.Distance(new Vector3(0, 0, 0), midPoint) / 3);

            Vector3 encounterPosition = BezierCurve.getBezierPos(fPosition, halfway, sPosition, 20, 10, dotPrefab);
            
            
            newEncounterPositions.Add(encounterPosition);

            createPath(fPosition, encounterPosition);
            createPath(sPosition, encounterPosition);

        }

        encounterPositions.Clear();
        encounterPositions = newEncounterPositions;

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
