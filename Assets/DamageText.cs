using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    Transform cameraTrans;

    public void SetCameraTrans(Transform cameraTrans)
    {
        this.cameraTrans = cameraTrans;
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTrans);
    }

    private void OnEnable()
    {
        StartCoroutine(Die());
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
