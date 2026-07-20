# Royal run


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
![alt text](images/image.png)

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
![alt text](images/image-2.png)
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
![alt text](images/image-1.png)


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
then
`rigidBody.MovePosition(newPosition)`