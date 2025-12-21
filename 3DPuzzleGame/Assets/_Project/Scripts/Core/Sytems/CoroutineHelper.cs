// ============================================================================
// COROUTINE HELPER
// ============================================================================
using UnityEngine;
using System.Collections;

public class CoroutineHelper : Singleton<CoroutineHelper>
{
    public static Coroutine StartCoroutineStatic(IEnumerator routine)
    {
        return Instance.StartCoroutine(routine);
    }

    public static void StopCoroutineStatic(Coroutine routine)
    {
        if (routine != null)
            Instance.StopCoroutine(routine);
    }
}