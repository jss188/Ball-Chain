using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public int collisions;
    public bool isConnectedToBoundary;
    Vector3 scale;
    Vector3 position;

	// Use this for initialization
	void Start () {
        collisions = 0;
        isConnectedToBoundary = false;
        //testOverlapBox();
    }
	
	// Update is called once per frame
	void Update () {
	}

    /*
    void OnTriggerEnter(Collider collider)
    {  
        collisions++;
        this.transform.parent = collider.gameObject.transform;
        this.triggerOn(false);
    } */

    public bool initWall(Vector3 position, Vector3 scale)
    {
        if (isAreaAvailable(position, scale))
        {
            //initialize the wall
            //return the instantiated wall. Otherwise return null
        }
        return true;
    }

    public void testOverlapBox()
    {
        if (this.tag == "testWall")
        {
            Vector3 position = new Vector3(0, 1, 0);
            Vector3 scale = new Vector3(10, 2, 5);
            Vector3 halfExtents = new Vector3(scale.x / 2, scale.y / 2, scale.z / 2);
            Collider[] colliders = Physics.OverlapBox(position, halfExtents);
            foreach (Collider collider in colliders)
            {
                collisions++;
                Debug.Log("collision: " + collider.gameObject.name);
            }
        }
    }

    //check to see if the area which the wall will be placed is available
    public bool isAreaAvailable(Vector3 position, Vector3 scale)
    {
        bool areaAvailable = false;
        Vector3 halfExtents = new Vector3(scale.x / 2 + 1, scale.y / 2, scale.z / 2 + 1); //+1 to x and z to prevent tight spaces
        Collider[] colliders = Physics.OverlapBox(position, halfExtents);
        areaAvailable = checkColliders(colliders);
        return areaAvailable;
    }

    private bool checkColliders(Collider[] colliders)
    {
        isCollidingBoundary(colliders);
        bool shouldDestroy = false;
        for (int i = 0; i < colliders.Length && !shouldDestroy; i++)
        {
            Collider collider = colliders[i];
            if (collider.gameObject.tag == "Wall")
            {
                Wall wall = collider.gameObject.GetComponent<Wall>();
                shouldDestroy = checkAgainst(wall);
            }
        }
        if (isConnectedToBoundary && !shouldDestroy)
        {
            updateConnectingWalls(colliders);
        }
        return !shouldDestroy;
    }

    //update if this wall is connected to the boundary, and check if this wall should be destroyed
    private bool checkAgainst(Wall wall)
    {
        if (wall.isConnectedToBoundary)
        {
            if (this.isConnectedToBoundary)
            {
                return true;
            } else {
                this.isConnectedToBoundary = true;
            } 
        }
        return false;
    }

    public void isCollidingBoundary(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == "Boundary")
            {
                isConnectedToBoundary = true;
            }
        }
    }

    /*  this method should only be called if this wall is found to be connected to the boundary and is not to be destroyed
     *  the method will update all connecting walls to state that they are connected to the boundary.
     */
    private void updateConnectingWalls(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider collider = colliders[i];
            if (collider.gameObject.tag == "Wall")
            {
                Wall wall = collider.gameObject.GetComponent<Wall>();
                wall.isConnectedToBoundary = true;
            }
        }
    }
}
