using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask obstacleLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroy if bullet hits obstacle
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Walls")))
            Destroy(gameObject);
             
    }


}
