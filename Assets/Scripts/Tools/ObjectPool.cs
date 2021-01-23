using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    [SerializeField] Stack<GameObject> InactiveObjects = new Stack<GameObject>();

    GameObject objectPrefab;
    Transform objectParentTransform;
    int optimalPoolSize;

    public ObjectPool(GameObject objectPrefab,int optimalPoolSize, Transform objectParentTransform)
    {
        this.objectPrefab = objectPrefab;
        this.optimalPoolSize = optimalPoolSize;
        this.objectParentTransform = objectParentTransform;
        while(InactiveObjects.Count < optimalPoolSize)
        {
            GameObject currentObject = GameObject.Instantiate(objectPrefab, objectParentTransform);
            InactiveObjects.Push(currentObject);
            currentObject.SetActive(false);
        }
    }

    public GameObject TakeObjectFromPool()
    {
        GameObject currentObject = null;

        if (InactiveObjects.Count < 1)
        {
            currentObject = GameObject.Instantiate(objectPrefab, objectParentTransform);
        }
        else
        {
            currentObject = InactiveObjects.Pop();
        }
        currentObject.SetActive(true);
        return currentObject;
    }
    public void ReturnObjectToPool(GameObject returnedItem)
    {
        if (InactiveObjects.Count >= optimalPoolSize)
        {
            GameObject.Destroy(returnedItem);
        }
        else
        {
            InactiveObjects.Push(returnedItem);
            returnedItem.SetActive(false);
        }
    }
}
