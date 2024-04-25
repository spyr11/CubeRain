using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public event Action<Cube> Hit;

    private void OnCollisionEnter(Collision collision)
    {
        Hit?.Invoke(this);
    }
}
