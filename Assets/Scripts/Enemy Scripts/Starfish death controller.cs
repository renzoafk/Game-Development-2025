using UnityEngine;
using System.Collections.Generic;
using UnityEngine;

public class Starfishdeathcontroller : MonoBehaviour 
{
    public void DestroyEnemy(float delay)
    {
        Destroy(gameObject, delay);
    }
}
