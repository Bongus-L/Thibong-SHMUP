using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8.0f;

    void Update()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            if (transform.parent)
            {
                // Code below also detroys any child objects associated with the parent object.
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}