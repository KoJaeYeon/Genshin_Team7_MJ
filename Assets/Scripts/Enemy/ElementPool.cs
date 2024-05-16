using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementPool : Singleton<ElementPool>
{
    public GameObject ElementPrefab;
    public Transform Player;
    private Queue<GameObject> ElementQueue;

    void Start()
    {
        ElementQueue = new Queue<GameObject>(); 

        for(int i = 0; i < 100; i++)
        {
            GameObject elementPrefab = Instantiate(ElementPrefab, transform);
            elementPrefab.SetActive(false);
            ElementObject element = elementPrefab.GetComponent<ElementObject>();
            element.Player = Player;
            ElementQueue.Enqueue(elementPrefab);
        }
    }

    public GameObject GetElementObject()
    {
        GameObject element = ElementQueue.Dequeue();
        ElementQueue.Enqueue(element);
        //element.SetActive(true);
        return element;
    }
}
