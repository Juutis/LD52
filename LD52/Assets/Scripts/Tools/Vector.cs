using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector
{
    public static Vector2 V3to2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector3 V2to3(Vector2 v, float yOffset = 0)
    {
        return new Vector3(v.x, yOffset, v.y);
    }

    public static Vector2 Substract(Vector3 v1, Vector3 v2)
    {
        return new Vector2(v1.x - v2.x, v1.z - v2.z);
    }

    public static Vector3 SetY(Vector3 v, float y = 0)
    {
        return new Vector3(v.x, y, v.z);
    }
}
