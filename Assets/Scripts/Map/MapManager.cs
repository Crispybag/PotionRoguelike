using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public List<TempEnemy> enemies;
    public GameObject prefab;

    public List<GameObject> availablePositions;

    private List<GameObject> positionsToRemove = new List<GameObject>();

    public Dictionary<int, GameObject> unsortedEnemies;
    SortedDictionary<int, GameObject> sortedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        unsortedEnemies = new Dictionary<int, GameObject>();
        int i = 0;
        foreach (TempEnemy enemy in enemies)
        {
            int randomPos = Random.Range(0, availablePositions.Count);
            Vector3 newPos = availablePositions[randomPos].transform.position;
            GameObject newEnemy = Instantiate(prefab);
            newEnemy.transform.position = newPos;
            unsortedEnemies.Add(int.Parse(availablePositions[randomPos].name), newEnemy);
            RemovePositions(randomPos);
            i++;
        }

        sortedEnemies = new SortedDictionary<int, GameObject>(unsortedEnemies);

        int index = 0;
        int bleh = 0;
        foreach(KeyValuePair<int, GameObject> dunno in sortedEnemies)
        {
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
