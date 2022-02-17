using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{

    /// <summary>
    /// Create bezier using 3 points
    /// </summary>
    /// <param name="pos1">Start position</param>
    /// <param name="pos2">Middle position</param>
    /// <param name="pos3">End position</param>
    /// <param name="dot">Prefab to spawn at the points </param>
    /// <param name="step">Distance between dots -> higher is smaller steps, default: 2.5f </param>
    public static void CreateBezier(Vector3 pos1, Vector3 pos2, Vector3 pos3, GameObject dot, float step = 2.5f)
    {
        //set initial values
        float time = 0;

        //get the distance
        float distance = Vector3.Distance(pos1, pos2) + Vector3.Distance(pos2, pos3);
        //multiple the distance by the step amount, the bigger the step amount the smaller distance between dots
        distance *= step;
        // divide 1 by the distance, so that you get small number
        //since the time needs to be inbetween 0 and 1
        step = 1f / distance;

        //do loop till time hits close to 1f
        while ((time <= 0.95f))
        {
            //formula for the bezier curve
            Vector3 result = Mathf.Pow((1 - time), 2) * pos1 + 2 * (1 - time) * time * pos2 + Mathf.Pow(time, 2) * pos3;
            //create dot with position from the bezier curve
            GameObject newDot = Instantiate(dot);
            newDot.transform.position = result;
            //increase the time step
            time += step;
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
