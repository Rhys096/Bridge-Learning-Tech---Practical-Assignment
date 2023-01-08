using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid
{

    Vector3 center;
    Vector3 topLeft;
    float radius;
    int divisions;
    bool[] disabled;
    float squareLength;


    public Grid(Vector3 pCenter, float pRadius, int pDivisions)
    {
        center = pCenter;
        radius = pRadius;
        divisions = pDivisions;
        squareLength = (radius * 2) / divisions;
        disabled = new bool[divisions * divisions];
        topLeft = center - new Vector3(radius, 0, radius);
    }

    public void DisableSquare(int square)
    {
        disabled[square] = true;
    }

    int GetRandomEnabledSquare()
    {
        List<int> enabled = new List<int>();
        for (int i = 0; i < disabled.Length; i++)
        {
            if (disabled[i] == false)
            {
                enabled.Add(i);
            }
        }

        if (enabled.Count == 0)
        {
            return -1;
        }

        return enabled[Random.Range(0, enabled.Count)];
    }

    public Vector3 GetCenterRandomSquare(bool disable = true)
    {
        int randomSquare = GetRandomEnabledSquare();
        int x = randomSquare % divisions;
        int z = (int) (randomSquare / divisions);

        if (disable) disabled[randomSquare] = true;

        Vector3 position = topLeft + new Vector3(squareLength * x + squareLength / 2, 0, squareLength * z + squareLength / 2);

        return position;

    }

    public Vector3 GetRandomPointRandomSquare()
    {
        Vector3 position = GetCenterRandomSquare(false);
        position += Vector3.right * Random.Range(-squareLength / 2, squareLength / 2);
        position += Vector3.forward * Random.Range(-squareLength / 2, squareLength / 2);
        return position;

    }

    public int AvailableSquares()
    {
        int count = 0;
        for (int i =0; i < disabled.Length; i++)
        {
            if (!disabled[i]) count++;
        }
        //Debug.Log(count);
        return count;
    }


}
