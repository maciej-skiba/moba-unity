using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeCollider : MonoBehaviour
{
    //this method causes poor performance
    //TO DO: change it for something more efficient
    public List<GameObject> currentCollisions = new List<GameObject>();

    private void OnTriggerStay(Collider col)
    {
        // Add the GameObject collided with to the list.
        currentCollisions.Add(col.gameObject);

        // Print the entire list to the console.
        //foreach (GameObject gObject in currentCollisions)
        //{
        //    print(gObject.name);
        //}
    }
}

