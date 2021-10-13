using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSolver : MonoBehaviour
{
   public float Speed;
    void Update()
    {
        transform.Translate(Vector3.up * Speed * Time.deltaTime);
    }
    public void Yposition()
    {
        transform.position = new Vector3(transform.position.x, .6f, transform.position.z);
    }
}
