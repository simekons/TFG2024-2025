using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0f;

    private bool peak = false;
    private bool loop = true;

    private void Start()
    {
        // EVENTO DE TELEMETRÌA
    }

    void Update()
    {
        if (loop)
        {
            if (this.gameObject.transform.position.y < 10)
            {
                this.gameObject.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
            }
            else
            {
                // EVENTO DE TELEMETRÕA
                peak = true;
                loop = false;
            }
        }
    }
}
