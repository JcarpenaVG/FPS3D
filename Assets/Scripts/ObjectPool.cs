using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabObject;
    [SerializeField] private int objectsNumberOnStart;

    private List<GameObject> objectsPool = new List<GameObject>();

    private void Start()
    {
        CreateObjects(objectsNumberOnStart);
    }

    private void CreateObjects(int numberOfObjects)
    {
        for (int i = 0; i < objectsNumberOnStart; i++)
        {
            CreateNewObject();
        }
    }

    /// <summary>
    /// Instantiate new object and add to the list
    /// </summary>
    /// <returns>GameObject</returns>
    private GameObject CreateNewObject() 
    {
        //Instantiate anywhere
        GameObject newObject = Instantiate(prefabObject);
        //Deactive
        newObject.SetActive(false);
        //Add to the list (we add the bullets, but not specific number)
        objectsPool.Add(newObject);  

        return newObject;
    }

    /// <summary>
    /// Take from the List an available object
    /// if not exist create a new one
    /// and Active the object
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject() 
    {
        //Find in the objectsPool an object that is inactive in the game hierarchy
        GameObject theObject = objectsPool.Find(x => x.activeInHierarchy == false);

        //if not exist , create one
        if (theObject == null)
        {
            theObject = CreateNewObject();
        }

        //Active gameObject
        theObject.SetActive(true);

        return theObject;
            
    }

}
