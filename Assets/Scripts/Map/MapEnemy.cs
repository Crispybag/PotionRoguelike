using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyPath
{
    public Vector3 startPos;
    public Vector3 midPos;
    public Vector3 endPos;
}

public class MapEnemy : MonoBehaviour
{

    public SO_Enemy enemy;

    private EnemyPath[] paths = new EnemyPath[2];

    private Vector3 endPosition;
    private int currentPath = 0;
    private float currentTime = 0;


    private bool hasSetWalkPath = false;

    public void Setup()
    {
        this.GetComponent<SpriteRenderer>().sprite = enemy.sprite;
    }

    public void SetWalkPath(Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, Vector3 pos5)
    {
        currentPath = 0;
        currentTime = 0;


        EnemyPath newPath = new EnemyPath();
        endPosition = pos3;
        newPath.startPos = pos1;
        newPath.midPos = pos2;
        newPath.endPos = pos3;

        paths[0] = newPath;

        EnemyPath newPath2 = new EnemyPath();
        newPath2.startPos = pos3;
        newPath2.midPos = pos4;
        newPath2.endPos = pos5;

        paths[1] = newPath2;

        hasSetWalkPath = true;
    }

    public void Update()
    {
        if (hasSetWalkPath)
        {
            updatePosition();
        }

    }

    public void updatePosition()
    {
        if (endPosition != this.transform.position)
        {

            this.gameObject.transform.position = BezierCurve.getBezierPos(paths[currentPath].startPos, paths[currentPath].midPos, paths[currentPath].endPos, currentTime);
            currentTime+= Time.deltaTime;

            if (currentTime > 1f)
            {
                currentTime = 1;
            }

        }
        else
        {
            if (currentPath <= paths.Length - 2)
            {
                currentPath++;
                endPosition = paths[currentPath].endPos;
                currentTime = 0;
            }
        }
    }

}
