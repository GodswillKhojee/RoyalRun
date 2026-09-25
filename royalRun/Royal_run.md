the things will be in the project ->

**player experience:**
dodge physics based obstacles on a shaky bridge

**core mechanics:**
procedurally generated level with hazards

**game loop: **
last as long as possible with powerups and checkpoints

**the things we are going to focus on ->**
- procedural generation
- rigid body physics
- physics materials
- handling collision
- animator transitions
- inspector attributes
- C# -> lists, inheritance, dependency injection, new keywords


Used this assets for this game
![[GameDevTV+Royal+Run+Assets.unitypackage]]

now from prefab folder look for King prefab and drag drop in the scene and play then you will see the animation of the king running


then create empty game object naming player and reset it positing and again create another game object naming model and also reseting its position and move the king prefab to the model folder

also i changed the scene name to `MainScene` as it will only have one scene which will be the endless running


## level generation overview 
we are not making the **king** move from point a to be 
but we are making the ground move from y to -x direction
meaning we will generate the world from the y direction then when the player crosses that part we will destroy that part 
![[Pasted image 20260711214512.png]]

now we are going to instantiate the ground from C# 
we made the ground and moved the cube to the feet of the king without touching the chunk prefab to make the prefab instantiate at the center of the king 
![[Pasted image 20260711220510.png]]
then we prefabbed it 
now the to instantiate using C# 
```c#
[SerializeField] GameObject chunckPrefab;

void Start()
{
    Instantiate(chunckPrefab, transform.position, Quaternion.identity);
}
```

Instantiate() -> used to create dynamically create clone of a prefab or a game object
ex-: `instantiate(prefab, transform.position, Quaternion.identity)`

# using for loop to instantiate many game object
so now i want to generate platform one by one to make it look like a endless running we are going to for loop as it will run the block of code for the given amount of time till the time is out
this is the serializeField which take the info about the tile to be spawn
```cs
[SerializeField] int startingChunksAmount = 12;
// this determines how many chunk will be instantiate
[SerializeField] Transform chunkParent;
// this will take the parent position of the chunk

[SerializeField] float chunkLength = 10f;
// this will make sure what will be the length of the new chunk
```

```cs
void Start()
{
	// using loop to Instantiate the chunkAmount
    for (int i = 0; i < startingChunksAmount; i++)
    {
        float spawnPositionZ; // spawning in the z direction
        if (i == 0) spawnPositionZ = transform.position.z ; // default position
        else spawnPositionZ = transform.position.y  + (i*chunkLength); // this is to ensure that the platform does not overlap and spawn sequencailly

        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
        // now making a new vector3 variable to store the position of the new chunk then instantiating it
        Instantiate(chunckPrefab,chunkSpawnPos, Quaternion.identity, chunkParent);
    }
}
```

# chunk movement

now i am storing the instantiated chunks in an array to make it move using `transform.translate`

i made array of gameobject right the side 12
and moveSpeed for the movement of the chunk towards the -z direction
```cs
[SerializeField] float moveSpeed = 9f;
GameObject[] chunks = new GameObject[12];
```

`MoveChunk() will be called every second to move the chunk toward -z direction`
```cs
private void Update()
{
    MoveChunk();
}

 void MoveChunk()
 {
 // making gameobject in chunks to move using transform.translate
 // using time.deltatime to make it look same in diff framerate pc
     foreach (var chunk in chunks)
     {
         chunk.transform.Translate(-transform.forward * (Time.deltaTime * moveSpeed));
     }
 }
```

# adding and removing tiles

now we for also optimizing the game 
we will remove the tile object when it reaches the camera  and can't be seen then we will remove that tile
but with removing it we will add another tile at front of the ground to make it look like endless running
```cs
else
	{
	    spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;
	}
```

```cs
void MoveChunks()
{
    for (int i = 0; i < chunks.Count; i++) // we can also use foreach loop here
    {
        GameObject chunk = chunks[i];
        chunk.transform.Translate(-transform.forward * (moveSpeed * Time.deltaTime));
        // when the tiles passes the camera it should get destroyed and a new chunk should be spawn
        if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
        {
            chunks.Remove(chunk);
            Destroy(chunk);
            SpawnChunk();
        }
    }
}
```

```cs
private void SpawnChunk()
{
    float spawnPositionZ = CalculateSpawnPositionZ();

    Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
    GameObject newChunk = Instantiate(chunkPrefab, chunkSpawnPos, Quaternion.identity, chunkParent);

    chunks.Add(newChunk);
}
```

# player movement

Now for the player movement
we are only giving our playing left right and jump crouch option as this is a run endless game

for to achieve this we created a input action map 
![[Pasted image 20260720145944.png]]
we made these changes
and make a script which we connect it to the `player` game object
```cs
[SerializeField] Rigidbody playerRb;
Vector2 movement;
public void Move(InputAction.CallbackContext context)
{
    movement = context.ReadValue<Vector2>();
    Debug.Log(movement);
}
```

and in player added the component `input action`
from which we selection our input action map in the `action` field and selected `invoke unity event` on behavior 
then in `event` drop down added player map then this
![[Pasted image 20260720150354.png]]


now to make the player move in the world we are going to use `MovePosition`

```cs
 private void FixedUpdate()
 {
     HandleMovement();
 }
 void HandleMovement()
 {
     Vector3 currentPosition = rigidBody.position;
     Vector3 movePosition = new Vector3(movement.x, 0f, movement.y);
     Vector3 newPosition = currentPosition + movePosition * moveSpeed * Time.fixedDeltaTime;
     rigidBody.MovePosition(newPosition);
 }
```

we are talking tree vector3 
one for the rigid body position
one for the where to move the player
and last for totaling the vectors and fixed delta time
then `rigidBody.MovePosition(newPosition)`

also at first the player movement looks jittery 
for this we turned on the `kinetic motion` in the player inspector
then in `interpolation` -> interpolation
![[Pasted image 20260720153338.png]]


# clamping the player to x and z 

for clamp reference
https://docs.unity3d.com/ScriptReference/Mathf.Clamp.html

`clamp` takes three float parameter 
```cs // inside HandleMovement

 newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp);
 newPosition.z = Mathf.Clamp(newPosition.z, -zClamp, zClamp);

```


# obstacle spawning using loops
```cs
public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;
    int obstacleSpawnes= 4;

    private void Start()
    {
        while (obstacleSpawnes > 0)
        {
            Instantiate(objectPrefab, transform.position, Quaternion.identity);
            obstacleSpawnes--;
        }
    }
}
```

we made a prefab of a object which is a cube 
then we manipulated the local gravity something which made the object to move towards player
then we instantiate the object on the location where we want to


so the problem is now that the object is spawning at the same time
to make object spawn at the time interval we are going to use coroutine

==A coroutine is a method that can suspend execution and resume at a later time.==

==**Important**: Don’t confuse coroutines with threads. Synchronous operations that run within a coroutine still execute on the main thread. If you want to reduce the amount of CPU time spent on the main thread, it’s just as important to avoid blocking operations in coroutines as in any other script code.==

```cs
IEnumerator spawnObstacleRoutine()
{
    while (obstacleSpawnes > 0)
    {
        yield return new WaitForSeconds(obstacleSpawnTIme);
        Instantiate(objectPrefab, transform.position, Quaternion.identity);
        obstacleSpawnes--;
    }
}
```

and this is called by this 
```cs
 private void Start()
 {
     StartCoroutine(spawnObstacleRoutine());
 }
```

# physics material
in this one we made a prefab variant in which we use a cart to behave like object moving toward player and added random rotation instead of quatornion.identity

use physics material because carts was getting stuck in the road so make it move normally we used it

# obstacle prefab variants
now in this we made arrays of obstacle for different type of obstacle
```cs
using UnityEngine;
using System.Collections;

public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] objectPrefab;
    [SerializeField] float obstacleSpawnTime = 1f;
    [SerializeField] `Transform obstacleParent;`
    [SerializeField] float spawnWidth = 4f;

    private void Start()
    {
        StartCoroutine(spawnObstacleRoutine());
    }

    IEnumerator spawnObstacleRoutine()
    {
        while (true)
        {
            GameObject noOfObject = objectPrefab[Random.Range(0, objectPrefab.Length)];
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y, transform.position.z);
            yield return new WaitForSeconds(obstacleSpawnTime);
            Instantiate(noOfObject, spawnPosition, Random.rotation,obstacleParent);
        }
    }
}

```

now here we used `Transform obstaclePrefab;` this is used to make hierarchy
now to make the object to spawn in random position we use 
this
`Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y, transform.position.z);`


# fence chunk hazard

now in this we are going to spawn fence

```cs
[SerializeField] GameObject fencePrefab;

[SerializeField] float[] lanes = { -2.5f, 0f, 2.5f };

private void Start()
{
    spawnFence();
}

void spawnFence()
{
    int randomlaneIdx = Random.Range(0, lanes.Length);
    Vector3 spawnPosition = new Vector3(lanes[randomlaneIdx], transform.position.y, transform.position.z);
    Instantiate(fencePrefab, spawnPosition, Quaternion.identity,this.transform);
}
```
this is a straightforward code
we made a lane array with three float values
we are going to spawn fence on three places on the chunk map
then instantiated on the map


# remove fence

```cs
 List<int> availableLanes = new List<int> { 0, 1, 2 };
 int fenceToSpawn = Random.Range(0, 3);
 for (int i = 0; i < fenceToSpawn; i++)
 {
     if (availableLanes.Count <= 0) break;

     int randomlaneIdx = Random.Range(0, availableLanes.Count);
     int selectedLane = availableLanes[randomlaneIdx];
     availableLanes.RemoveAt(randomlaneIdx);

     Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
     Instantiate(fencePrefab, spawnPosition, Quaternion.identity,this.transform);
 }
```
now we are deciding how many fences should be spawn on the lane
also after selecting the lane we are deleting the lane because we are making sure if the lanes is selected we do not accidently spawn fence on same lane

