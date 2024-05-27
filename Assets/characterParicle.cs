using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterParicle : MonoBehaviour
{
    Coroutine coroutine;
    private void OnEnable()
    {
        if(coroutine != null) { StopCoroutine(coroutine); }
        coroutine = StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
        yield break;
    }
}
