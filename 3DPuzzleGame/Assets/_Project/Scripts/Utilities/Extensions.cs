// ============================================================================
// EXTENSION METHODS
// ============================================================================
using UnityEngine;
using System.Collections.Generic;

public static class Extensions
{
    // Transform Extensions
    public static void ResetTransform(this Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
    }

    public static void DestroyChildren(this Transform t)
    {
        for (int i = t.childCount - 1; i >= 0; i--)
            Object.Destroy(t.GetChild(i).gameObject);
    }

    // Vector3 Extensions
    public static Vector3 WithX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);
    public static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
    public static Vector3 WithZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);

    // List Extensions
    public static T GetRandom<T>(this List<T> list)
    {
        return list.Count > 0 ? list[UnityEngine.Random.Range(0, list.Count)] : default;
    }

    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
