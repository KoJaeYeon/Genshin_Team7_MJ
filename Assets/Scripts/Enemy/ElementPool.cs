using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementPool : Singleton<ElementPool>
{
    public GameObject ElementPrefab;
    public Transform Player;
    private Queue<GameObject> ElementQueue;

    public GameObject TestElement;
    public Transform TestPool;
    private Queue<GameObject> TestElementQueue;

    void Start()
    {
        ElementQueue = new Queue<GameObject>(); 
        TestElementQueue = new Queue<GameObject>();

        for(int i = 0; i < 100; i++)
        {
            GameObject elementPrefab = Instantiate(ElementPrefab, transform);
            ElementObject element = elementPrefab.GetComponent<ElementObject>();
            element.SetPlayerTransform(Player);
            elementPrefab.SetActive(false);
            ElementQueue.Enqueue(elementPrefab);
        }

        for(int i = 0; i < 100; i++)
        {
            GameObject testElement = Instantiate(TestElement, TestPool);
            testElement.SetActive(false);
            TestElementQueue.Enqueue(testElement);
        }
    }

    public GameObject GetElementObject()
    {
        GameObject element = ElementQueue.Dequeue();
        ElementQueue.Enqueue(element);
        return element;
    }

    public GameObject GetTestElement()
    {
        GameObject element = TestElementQueue.Dequeue();
        TestElementQueue.Enqueue(element);
        element.SetActive(true);
        return element;
    }
}
