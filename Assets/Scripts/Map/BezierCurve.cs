using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class dotBrackets
{
    public List<Vector3> dots = new List<Vector3>();
}

public class BezierCurve : MonoBehaviour
{


    private static List<dotBrackets> brackets = new List<dotBrackets>();
    private static GameObject dotPrefab;
    static BezierCurve bezier;


    void Awake()
    {
        bezier = this;

    }

    public static void CreateDoubleBezier(Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, Vector3 pos5, GameObject dot, float step = 2.5f)
    {
        dotBrackets newDots = new dotBrackets();
        CreateBezier(pos1, pos2, pos3, dot, ref newDots);
        CreateBezier(pos3, pos4, pos5, dot, ref newDots);
        brackets.Add(newDots);

    }


    public static void StartDotting()
    {
        //for each bracket start the couroutine to start placing the dots
        foreach (dotBrackets bracket in brackets)
        {
            bezier.StartCoroutine(createBezierDots(bracket));
        }
        brackets.Clear();
    }

    public static void CreateSingleBezier(Vector3 pos1, Vector3 pos2, Vector3 pos3, GameObject dot, float step = 2.5f)
    {
        dotBrackets newDots = new dotBrackets();
        CreateBezier(pos1, pos2, pos3,dot, ref newDots);
        brackets.Add(newDots);
        //bezier.StartCoroutine(createBezierDots());
    }


    /// <summary>
    /// Create bezier using 3 points
    /// </summary>
    /// <param name="pos1">Start position</param>
    /// <param name="pos2">Middle position</param>
    /// <param name="pos3">End position</param>
    /// <param name="dot">Prefab to spawn at the points </param>
    /// <param name="step">Distance between dots -> higher is smaller steps, default: 2.5f </param>
    public static void CreateBezier(Vector3 pos1, Vector3 pos2, Vector3 pos3, GameObject dot, ref dotBrackets newDots ,float step = 2.5f)
    {
        //set initial values
        float time = 0;
        dotPrefab = dot;


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
            
            //add the new position to the list of dots
            newDots.dots.Add(result);

            //increase the time step
            time += step;
        }


    }

    static IEnumerator createBezierDots(dotBrackets bracket)
    {
        //create a timer, all lines will take exactly 2f time to get to the end.
        float timer = 2f / bracket.dots.Count;
        //loop through all the dots inside the bracket
        for (int i = 0; i < bracket.dots.Count; i++)
        {
            GameObject dot = Instantiate(dotPrefab);
            dot.transform.position = bracket.dots[i];

            yield return new WaitForSeconds(timer);
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

    public static Vector3 getBezierPos(Vector3 pos1, Vector3 pos2, Vector3 pos3, int amount, int point)
    {
        float distance = 1f / amount;

        float time = point * distance;

        Vector3 result = Mathf.Pow((1 - time), 2) * pos1 + 2 * (1 - time) * time * pos2 + Mathf.Pow(time, 2) * pos3;
        return result;
    }

    public static Vector3 getBezierPos(Vector3 pos1, Vector3 pos2, Vector3 pos3, float time)
    {
        Vector3 result = Mathf.Pow((1 - time), 2) * pos1 + 2 * (1 - time) * time * pos2 + Mathf.Pow(time, 2) * pos3;
        return result;
    }



}
