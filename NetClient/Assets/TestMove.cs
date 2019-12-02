using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(-1, 0, 0);
        }else if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0, 0, 1);
        }else if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0, 0, -1);
        }
    }
}
