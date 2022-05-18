using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private float _xPositionLimit = 12f;
    private float _yOriginalPosition = 8f;
    private Player _player;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    private AudioSource _explosionSoundSource;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _laserOffset = -0.8f;
    private bool _isAlive = true;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager.isCoopMode)
        {
            _player = GameObject.Find("Player 1").GetComponent<Player>();
        }
        else
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
        }

        
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _explosionSoundSource = GetComponent<AudioSource>();
        _explosionSoundSource.clip = _explosionSoundClip;

        if (_player == null)
        {
            Debug.Log("Player NULL");
        }

        if (_animator == null)
        {
            Debug.Log("Animator NULL");
        }

        if (_boxCollider == null)
        {
            Debug.Log("Collider NULL");
        }

        StartCoroutine(CountLaserFire());
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-_xPositionLimit, _xPositionLimit);
            transform.position = new Vector3(randomX, _yOriginalPosition, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If an enemy collides with a laser.
        if (other.tag == "Laser")
        {
            EnemyDeathSequence();
            Destroy(other.gameObject);
            _player.PlayerScore(10);
        }

        // If an enemy collides with the player.
        if (other.tag == "Player")
        {
            EnemyDeathSequence();
            var player = other.GetComponent<Player>();
            player.PlayerDamage();
            // Check if player actually exists before calling PlayerDamage().
            //if (_player != null)
            //{
            //    _player.PlayerDamage();
                
            //}
        }
    }

    private void EnemyDeathSequence()
    {
        Destroy(_boxCollider);
        _explosionSoundSource.Play();
        _animator.SetTrigger("OnEnemyDeath");
        _isAlive = false;
        Destroy(gameObject, 2.633f);
    }

    private void Fire()
    {
        _canFire = Time.time + _fireRate;

        // Laser to spawn at the enemy position.
        Vector3 enemyCurrentPosition = transform.position;

        if (!_gameManager.isGameOver)
        {
            // Instantiate with X position = player position, Y position = player position + offsest, and Quarrternio (rotation) = 0.
            Instantiate(_laserPrefab, enemyCurrentPosition + new Vector3(0, _laserOffset, 0), Quaternion.identity);
        }
    }

    IEnumerator CountLaserFire()
    {
        while (_isAlive)
        {
            Fire();
            yield return new WaitForSeconds(2.0f);
        }
    }
}