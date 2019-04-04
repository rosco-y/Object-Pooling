using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A pool manager that is ready to manage multiple pools!
/// HUGE Thanks to Sebastian Lague, who created this tutorial:
/// https://www.youtube.com/watch?v=LhqP3EghQ-Q
/// </summary>
public class PoolManager : MonoBehaviour                       
{
    // Very Clever use of the Generic Collections, a Dictionary of Queues!
    Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();

    //// Use the singleton pattern for easy access to the manager:
    /// And to support using a single PoolManager.

    static PoolManager _instance;

    // Instance accessor
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // find the PoolManager in the current scene.
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    /// <summary>
    /// CreatePool(GameObject prefab, int poolsize)
    /// Function to add a pool to the Dictionary
    /// (if it doesn't exist.)
    /// </summary>
    /// <param name="prefab"> The prefab to place in the new Pool. (Queue)</param>
    /// <param name="poolsize"> The number of prefabs to place in the Pool.</param>
    public void CreatePool(GameObject prefab, int poolsize)
    {
        // ensure Dictionary does not already contain the pool of prefabs
        // using the prefab's unique Instance ID
        // (if the Dictionary does contain a pool of the prefabs, then nothing happens.)
        int poolKey = prefab.GetInstanceID();
        if (!poolDictionary.ContainsKey(poolKey))
        {
            // if not, add the pool 
            poolDictionary.Add(poolKey, new Queue<GameObject>());

            // then instantiate the GameObjects for the pool
            for (int i = 0; i < poolsize; i++)
            {
                GameObject newObject = Instantiate(prefab) as GameObject; // as a GameObject!
                newObject.SetActive(false); // invisible for now.
            }
        }
    }

    /// <summary>
    /// Remove the first prefab from the Pool Queue, place it at the specified
    /// position and rotation, place it back in the end of the queue.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // unique identifier
        int poolKey = prefab.GetInstanceID();

        // verify that such a prefab pool exists.
        if (poolDictionary.ContainsKey(poolKey))
        {
            GameObject objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);  // add instiated objec to the pool Queue.

            // place retrieved object at specified location
            objectToReuse.SetActive(true);
            objectToReuse.transform.position = position;
            objectToReuse.transform.rotation = rotation;
        }
    }
}
