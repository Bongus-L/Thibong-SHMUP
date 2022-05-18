using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeedMax = 50.0f;
    // Below so you can drag and drop the explosion prefab.
    [SerializeField]
    private GameObject _explosion;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, (Time.deltaTime * _rotationSpeedMax));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject, 0.25f);
            // Below so you can avoid "Destroying assets is not permitted to avoid data loss".
            GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
        }
    }
}
