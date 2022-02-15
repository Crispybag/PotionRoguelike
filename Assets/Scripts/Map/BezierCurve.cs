using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void createBezier(Vector3 pos1, Vector3 pos2, Vector3 pos3, int amount, GameObject dot)
    {
        float distance = 1f / amount;
        Debug.Log("Distance: " + distance);
        for (int i = 0; i < amount; i++)
        {
            float time = i * distance;

            Vector3 result = Mathf.Pow((1 - time), 2) * pos1 + 2 * (1 - time) * time * pos2 + Mathf.Pow(time, 2) * pos3;
            Debug.Log("Time : " + time + " Result: " + result);
            GameObject newDot = Instantiate(dot);
            newDot.transform.position = result;
        }
    }

    public static void createLine(Vector3 pos1, Vector3 pos2, int amount, GameObject dot)
    {
        float distance = Vector3.Distance(pos1, pos2);
        distance /= amount;
        Vector3 direction = pos2 - pos1;
        direction.Normalize();

        for (int i = 1; i < amount; i++)
        {
            float time = i * distance;
            Vector3 result = pos1;
            result += direction * time;
            GameObject newDot = Instantiate(dot);
            newDot.transform.position = result;

        }


    }

    public static Vector3 getBezierPos(Vector3 pos1, Vector3 pos2, Vector3 pos3, int amount, int point, GameObject dot)
    {
        float distance = 1f / amount;

        float time = point * distance;

        Vector3 result = Mathf.Pow((1 - time), 2) * pos1 + 2 * (1 - time) * time * pos2 + Mathf.Pow(time, 2) * pos3;
        return result;
    }



}
