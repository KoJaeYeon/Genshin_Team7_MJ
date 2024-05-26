using System.Collections;
using UnityEngine;

public class DamageText : Singleton<DamageText>
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
        transform.Rotate(0, 180, 0);
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
