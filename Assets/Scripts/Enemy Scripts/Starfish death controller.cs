using UnityEngine;
using System.Collections.Generic;

public class Starfishdeathcontroller : MonoBehaviour 
{
    public void DestroyEnemy(float delay)
    {
        Destroy(gameObject, delay);
    }
}
