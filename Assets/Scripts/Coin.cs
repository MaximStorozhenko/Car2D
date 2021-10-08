using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Vector3 rotator;

    void Start()
    {
        rotator = 1.1f * Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (Menu.is_paused) return;

        this.transform.Rotate(rotator);
    }
}
