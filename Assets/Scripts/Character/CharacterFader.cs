using System.Collections;
using UnityEngine;

public class CharacterFader : MonoBehaviour
{
    private Renderer[] renderers;
    private Color[] originalColors;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for(int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    public void SetAlpha(float alpha)
    {
        for(int i = 0; i < renderers.Length; i++)
        {
            Color color = originalColors[i];
            color.a = alpha;
            renderers[i].material.color = color;
        }
    }

    public IEnumerator FadeOut(float duration)
    {
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            SetAlpha(1 - t / duration);
            yield return null;
        }
        SetAlpha(0);    
    }

    public IEnumerator FadeIn(float duration)
    {
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            SetAlpha(t / duration);
            yield return null;
        }
        SetAlpha(1);
    }
}
