using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour {

    private int arenaWidth, arenaHeight;
    public Wall wallPrefab;
    public Boundary boundaryPrefab;
    public GameObject spawnPointPrefab; 
    private LinkedList<Wall> walls;
    public int availableWidth, availableHeight; //the area available in which we can generate walls
    private int wallsPerQuadrant;
    private Boundary[] boundaries;
    private GameObject[] spawnPoints;
    private const int wallTries = 8;
    private const int spawnTries = 100;


    // Use this for initialization
    void Start () {
        StartCoroutine(MapGenerate());
    }
	
    IEnumerator MapGenerate()
    {
        walls = new LinkedList<Wall>();
        createBoundaries();
		//yield return new WaitForSeconds(1);

        createWalls(wallsPerQuadrant);
		//yield return new WaitForSeconds(1);

		checkForWalls();
        mirrorOver(true);
        mirrorOver(false);
        destroyBoundaries();

        yield return new WaitForEndOfFrame();

        addSpawnPoints();

		UnTriggerWalls();
    }

	// Update is called once per frame
	/**
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow)) //for testing
        {
            mirrorOver(true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) //for testing
        {
            mirrorOver(false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            createBoundaries();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            destroyBoundaries();
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            addSpawnPoints();
                }
    }
	**/

    public void init(int arenaWidth, int arenaHeight, int wallsPerQuadrant)
    {
        this.arenaWidth = arenaWidth;
        this.arenaHeight = arenaHeight;
        this.availableWidth = (arenaWidth / 2) - 1;
        this.availableHeight = (arenaHeight / 2) - 1;
        this.wallsPerQuadrant = wallsPerQuadrant;
    }

    private void createWall()
    {
        Wall wall = initWall();
        wall.transform.localScale = new Vector3(Random.Range(.5f, 3), 2, Random.Range(.5f, 3));
        wall.transform.localPosition = new Vector3(Random.Range(1, availableWidth), 1, Random.Range(1, availableHeight));
        wall.name = "Wall " + walls.Count;
        walls.AddLast(wall);
    }

    public void createWalls(int wallsPerQuadrant)
    {
        bool wallCreated;
        for (int x = 0; x < wallsPerQuadrant; x++)
        {
            wallCreated = false;
            if (Random.value > .25) //25% chance for square wall, 75% for rect wall
            {
                for (int tries = 0; tries < wallTries && !wallCreated; tries++) //try x times to create a wall
                {
                    wallCreated = createRectWall();
                }
            } else {
                for (int tries = 0; tries < wallTries && !wallCreated; tries++)
                {
                    wallCreated = createSquareWall();
                }
            }
        }
    }

    private bool createRectWall()
    {
        Wall newWall = initWall();
        Vector3 scale;
        Vector3 position;
        float height, width;
        if (Random.value > 0.5) // 50/50 chance to make tall or long rect
        {
            //make tall rect
            height = availableHeight / 1.5f; //long side can be 3/4 of the quadrant at most
            width = availableHeight / 6f; //short side can be 1/6 of the quadrant at most
            scale = new Vector3(Random.Range(.2f, width), 2, Random.Range(width, height));
        }
        else
        {
            //make long rect
            height = availableWidth / 6f;
            width = availableWidth / 1.5f;
            scale = new Vector3(Random.Range(height, width), 2, Random.Range(.2f, height));
        }
        position = new Vector3(Random.Range(1, availableWidth), 1, Random.Range(1, availableHeight));
        return finalizeWall(newWall, position, scale);
    }

    private bool createSquareWall()
    {
        Wall newWall = initWall();
        Vector3 scale;
        Vector3 position;
        int shorterDimension = availableHeight > availableWidth ? availableHeight : availableWidth; //shorter dimension of arena
        float side = Random.Range(2.5f, shorterDimension / 2); //shortest side cannot be shorter than 1 
        scale = new Vector3(side, 2, side + Random.Range(-1.5f, 1.5f)); //results in an imperfect square
        position = new Vector3(Random.Range(1, availableWidth), 1, Random.Range(1, availableHeight));
        newWall.transform.localScale = scale;
        newWall.transform.localPosition = position;
        return finalizeWall(newWall, position, scale);
    }

    private void createBoundaries()
    {
        Boundary xAxis = initBoundary();
        Boundary zAxis = initBoundary();
        Boundary leftBound = initBoundary();
        Boundary rightBound = initBoundary();
        Boundary upperBound = initBoundary();
        Boundary lowerBound = initBoundary();
        boundaries = new Boundary[6] { xAxis,zAxis,leftBound,rightBound,upperBound, lowerBound};

        //xAxis will separate walls across the middle of the arena on the x axis
        xAxis.transform.localPosition = new Vector3(0, 0, 0);
        xAxis.transform.localScale = new Vector3(1, 1, arenaHeight);

        zAxis.transform.localPosition = new Vector3(0, 0, 0);
        zAxis.transform.localScale = new Vector3(arenaWidth, 1, 1);

        leftBound.transform.localPosition = new Vector3(-arenaWidth/2, 0, 0);
        leftBound.transform.localScale = new Vector3(1, 1, arenaHeight);

        rightBound.transform.localPosition = new Vector3(arenaWidth/2, 0, 0);
        rightBound.transform.localScale = new Vector3(1, 1, arenaHeight);

        upperBound.transform.localPosition = new Vector3(0, 0, arenaHeight / 2);
        upperBound.transform.localScale = new Vector3(arenaWidth, 1, 1);

        lowerBound.transform.localPosition = new Vector3(0, 0, -arenaHeight / 2);
        lowerBound.transform.localScale = new Vector3(arenaWidth, 1, 1);
    }

    private Wall initWall()
    {
        Wall newWall = Instantiate(wallPrefab) as Wall;
        newWall.transform.parent = transform;
        return newWall;
    }

    private Boundary initBoundary()
    {
        Boundary boundary = Instantiate(boundaryPrefab) as Boundary;
        boundary.transform.parent = transform;
        return boundary;
    }

    private bool finalizeWall(Wall wall, Vector3 position, Vector3 scale)
    {
        if (wall.isAreaAvailable(position, scale))
        {
            wall.transform.localScale = scale;
            wall.transform.localPosition = position;
            wall.name = "Wall " + walls.Count;
            walls.AddLast(wall);
            return true;
        }
        else
        {
            Destroy(wall.gameObject);
            return false;
        }
    }

    //if a wall group is touching a boundary wall, delete it.
    private void checkBoundaries()
    {
        //createBoundaries();
        IEnumerator ienum = walls.GetEnumerator();
        while (ienum.MoveNext())
        {
            Wall wall = (Wall) ienum.Current;
            int collisions = 0;
            foreach (Boundary boundary in boundaries)
            {
                //if wall.collider
            }
            if (collisions > 2) {
                //Destroy
            }
        }
    }

    private void destroyBoundaries()
    {
        for (int i = 0; i < boundaries.Length; i++)
        {
            Destroy(boundaries[i].gameObject);
        }
    }

    private void checkForWalls()
    {
        if (walls.Count < wallsPerQuadrant)
        {
            int wallsToCreate = wallsPerQuadrant - walls.Count;
            for (int i = 0; i < wallsToCreate; i++)
            {
                createWall();
            }
        }
    }
    
    private Wall mirrorWall(Wall wall, bool overVertical) 
        //overVertical means the arena is mirrored over the vertical axis, otherwise it is mirrored over horizontal
    {
        Vector3 scale = wall.transform.localScale;
        Vector3 position = wall.transform.position;
        if (overVertical)
        {
            position.x = -position.x; //flip left
        } else {
            position.z = -position.z; //flip down
        }
        Wall newWall = Instantiate(wallPrefab) as Wall;
        newWall.transform.parent = transform;
        newWall.transform.localScale = scale;
        newWall.transform.localPosition = position;
        return newWall;
    }

    /* 
     * Take the array of walls and replicate them, flipping the x or z position to a negative value 
     * This mirrors the walls over the vertical or horizontal axis
     */ 
    private void mirrorOver(bool vertical)
    {
        LinkedList<Wall> mirrorWalls = new LinkedList<Wall>();
        foreach (Wall wall in walls)
        {
            Wall newWall = mirrorWall(wall, vertical);
            mirrorWalls.AddLast(newWall);
        }
        foreach (Wall wall in mirrorWalls)
        {
            wall.name = "Wall " + walls.Count;
            walls.AddLast(wall);
        }
    }

    private void addSpawnPoints()
    {
        Vector3 position = findSpawnPointOnVertical();
        GameObject spawnPoint = initSpawnPoint(position);
        position.z = -position.z;
        GameObject spawnPoint1 = initSpawnPoint(position);
        position = findSpawnPointOnHorizontal();
        GameObject spawnPoint2 = initSpawnPoint(position);
        position.x = -position.x;
        GameObject spawnPoint3 = initSpawnPoint(position);
        spawnPoints = new GameObject[4] { spawnPoint, spawnPoint1, spawnPoint2, spawnPoint3 };

		AddSpawnPointsToManager();
    }

	private void AddSpawnPointsToManager() {
		Transform[] points = new Transform[spawnPoints.Length];
		for(int i = 0; i < points.Length; i++) {
			points[i] = spawnPoints[i].transform;
		}
		MultiplayerManagement.main.spawnLocations = points;
	}

    private GameObject initSpawnPoint(Vector3 position)
    {
        GameObject spawnPoint = Instantiate(spawnPointPrefab) as GameObject;
        spawnPoint.transform.parent = transform;
        spawnPoint.transform.localPosition = position;
        spawnPoint.transform.localScale = new Vector3(1, 1, 1);
        return spawnPoint;
    }

    private Vector3 findSpawnPointOnVertical()
    {
        int start = availableHeight * 3 / 4;
        Vector3 position = new Vector3(0, 1, start);
        return searchForSpawn(position, true);
    }

    private Vector3 findSpawnPointOnHorizontal()
    {
        int start = availableWidth * 3 / 4;
        Vector3 position = new Vector3(start, 1, 0);
        return searchForSpawn(position, false); 
    }

    private Vector3 searchForSpawn(Vector3 start, bool vertical)
    {
        Vector3 halfExtents = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 position = start;
        bool spawnPointFound = false;
        for (int i = 0; i < spawnTries && !spawnPointFound; i++) // check a max of x different positions to find a spawn point.
        {
            //Debug.Log(position.x + ", " + position.y + ", " + position.z);
            if (checkArea(position, halfExtents))
            {
                spawnPointFound = true;
            }
            else
            {
                if (vertical) {
                    position.z = changeSlightly(position.z, availableHeight);
                    //Debug.Log(position.z);
                } else {
                    position.x = changeSlightly(position.x, availableWidth);
                    //Debug.Log(position.x);
                }
            }
        }
        return position;
    }

    private bool checkArea(Vector3 position, Vector3 halfExtents)
    {
        Collider[] colliders;
        colliders = Physics.OverlapBox(position, halfExtents);
        return (colliders.Length == 0); //is this area is colliding with anything?
    }

    private float changeSlightly(float point, int length)
    {
        //bool changedPoint;
        if (point <= (length * 0.75f) && point > 2f) // between 3/4 and close to center
        {
            point -= 1f;
           // changedPoint = true;
        }
        else if (point <= 2f)
        { //too close to center, start searching past 3/4
            point = ((length * 0.75f) + 1f);
            //Debug.Log("pass");
        }
        else if (point > (length * 0.75f) && point < (length - 1.5)) {  //between 3/4 and close to edge
            point += 1f;
           // changedPoint = true;
        }
        return point;        
    }

	private void UnTriggerWalls() {
		foreach(var wall in walls) {
			Collider col = wall.GetComponent<Collider>();
			if(col != null)
				col.isTrigger = false;
		}
	}
}
