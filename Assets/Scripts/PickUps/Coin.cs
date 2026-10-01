using UnityEngine;

public class Coin : PickUps
{
    protected override void PickUp()
    {
        Debug.Log("Coin picked up!");
    }
}
