using UnityEngine;

public class Apple : PickUps
{
    protected override void PickUp()
    {
        Debug.Log("Apple picked up!");
    }

    
}
