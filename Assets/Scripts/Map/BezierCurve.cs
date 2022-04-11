using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class dotBrackets
{
    public List<Vector3> dots = new List<Vector3>();
}

public class BezierCurve : MonoBehaviour
{

    private static List<dotBrackets> brackets = new List<dotBrackets>();
    public static GameObject dotPrefab;
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

    public static void ClearBezierCurves()
    {
        brackets.Clear();
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

    public static void createBezierDotsInstant()
    {
        //loop through all the dots inside the bracket
        foreach (dotBrackets bracket in brackets)
        {
            for (int i = 0; i < bracket.dots.Count; i++)
            {
                GameObject dot = Instantiate(dotPrefab);
                dot.transform.position = bracket.dots[i];
            }
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


    /// <summary>
    /// Creates a bracket
    /// </summary>
    /// <param name="startPosition">Start position</param>
    /// <param name="endPosition">End position</param>
    public static Vector3[] createBracket(Vector3 startPosition, Vector3 endPosition)
    {
        //get the direction between the 2 points
        Vector3 pointDirection = startPosition - endPosition;
        pointDirection.Normalize();
        //then get their middle point
        Vector3 midPoint = startPosition + pointDirection * Vector3.Distance(startPosition, endPosition) / 2;

        //midpoint is the direction, because center is 0,0,0
        Vector3 midDirection = midPoint;
        midDirection.Normalize();

        //then get it around 1/3 of the middle, this is where we would bezier curve to go
        Vector3 halfway = midDirection * (Vector3.Distance(new Vector3(0, 0, 0), midPoint) / 3);

        //with this half way position, we get the position exactly halfway from the 2 points, this way it is smoother between the 2 points
        Vector3 encounterPosition = BezierCurve.getBezierPos(startPosition, halfway, endPosition, 20, 10);

        //add this position to the list for later use
        //encounterPositions.Add(encounterPosition);

        //create the path based on the positions
        //from the first enemy to the encounter position
        Vector3[] firstPos = createPath(startPosition, encounterPosition);
        //and from the second enemy to the encounter position
        Vector3[] secondPos = createPath(endPosition, encounterPosition);

        Vector3[] allPos = firstPos.Concat(secondPos).ToArray();

        return allPos;
    }

    /// <summary>
    /// Creates a path between the middle points
    /// </summary>
    /// <param name="startPosition"> Start position</param>
    /// <param name="endPosition"> The encounter position, or end position</param>
    private static Vector3[] createPath(Vector3 startPosition, Vector3 endPosition)
    {
        //Get the distance between the 2 points
        float distance = Vector3.Distance(startPosition, endPosition);

        //get the direction between the 2 points
        Vector3 direction = endPosition - startPosition;
        direction.Normalize();

        //create an offset
        float offset = distance / 6;
        //get a random distance, the initial distance is divided by two so its around the middle, and then +- the offset so the offset is around the middle point
        float randomDistance = Random.Range((distance / 2) - offset, (distance / 2) + offset);

        //get the middle point based on the distance
        Vector3 lineMiddlePoint = startPosition + direction * randomDistance;


        //generate random number to decide if left or right first
        int randDirection = Random.Range(0, 2);

        Vector3 firstLineSegmentNormalDirection;
        Vector3 secondLineSegmentNormalDirection;

        //using the random direction we set the normals of the line
        if (randDirection == 0)
        {
            firstLineSegmentNormalDirection = new Vector3(direction.y * -1, direction.x, direction.z);
            secondLineSegmentNormalDirection = new Vector3(direction.y, direction.x * -1, direction.z);
        }
        else
        {
            firstLineSegmentNormalDirection = new Vector3(direction.y, direction.x * -1, direction.z);
            secondLineSegmentNormalDirection = new Vector3(direction.y * -1, direction.x, direction.z);
        }

        //with the new middle pos and the new normal direction, we get the normal points
        Vector3 firstNormalPoint = createLineSegment(startPosition, lineMiddlePoint, firstLineSegmentNormalDirection);
        Vector3 secondNormalPoint = createLineSegment(lineMiddlePoint, endPosition, secondLineSegmentNormalDirection);
        //with these values we generate a double bezier curve
        //start pos -> first normal point -> line middle point
        //line middle point -> second normal point -> end position
        Vector3[] positions = new[] { startPosition, firstNormalPoint, lineMiddlePoint, secondNormalPoint, endPosition };

        BezierCurve.CreateDoubleBezier(startPosition, firstNormalPoint, lineMiddlePoint, secondNormalPoint, endPosition, dotPrefab);

        return positions;
    }

    /// <summary>
    /// Creates a line segment
    /// </summary>
    /// <param name="pointA">Start position</param>
    /// <param name="pointB">End position</param>
    /// <param name="normalDirection">The normal direction of which way the curve goes</param>
    private static Vector3 createLineSegment(Vector3 pointA, Vector3 pointB, Vector3 normalDirection)
    {
        //get the direction between the 2 points
        Vector3 fDirection = pointB - pointA;
        fDirection.Normalize();

        //get the distance between the 2 points
        float distance = Vector3.Distance(pointA, pointB);

        //generate an offset based on the distance
        float offset = distance / 4;
        //get a random distance, the initial distance is divided by two so its around the middle, and then +- the offset so the offset is around the middle point
        float randomDistance = Random.Range((distance / 2) - offset, (distance / 2) + offset);

        //get the middle point of the line segment point using the random distance
        Vector3 lineSegmentMiddlePoint = pointA + fDirection * randomDistance;

        //then we generate a new point based on the calculate middle point, and add the normal line distance to it, this generates the third point we need to create a bezier
        Vector3 lineSegmentNormalPoint = lineSegmentMiddlePoint + normalDirection * distance / 2;

        //return the new segment
        return lineSegmentNormalPoint;
    }



}
