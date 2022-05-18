using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 16.0f;
    private Transform _player;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(!_gameManager.isGameOver)
        {
            _player = GameObject.Find("PlayerPosition").GetComponent<Transform>();
            transform.Rotate(0, 0, AngleBetweenPoints(transform.position, _player.transform.position) + 90);
        }
        StartCoroutine(CountDownDestruction());
    }

    private void Update()
    {
        if (_player)
        {
            LookAt(_player.transform.position);
        }
        else
        {
            return;
        }
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        if (transform.position.y < -5.35f || transform.position.x > 10.4 || transform.position.x < -10.4)
        {
            if (transform.parent)
            {
                // Code below also detroys any child objects associated with the parent object.
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    IEnumerator CountDownDestruction()
    {
        while (gameObject)
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(this.gameObject);
        }
    }

    // Below for homing missiles.
    protected void LookAt(Vector2 point)
    {
        float angle = AngleBetweenPoints(transform.position, point);
        var targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle+60));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }

    private float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}