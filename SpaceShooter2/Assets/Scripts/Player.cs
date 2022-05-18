using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player position and other properties vars.
    Vector3 defaultPosition = new Vector3(0, 0, 0);
    // SerializedField name field allows you to change a private var value within Unity.
    // Private variables usually start with an underscore to easily distinguish.
    [SerializeField]
    private float _playerSpeed;
    private float _speedMultiplier = 1.5f;
    private float _xPosition;
    private float _yPosition;
    private float _yPositionLimit = 5f; 
    private float _yPositionMinusLimit = -3.72f;
    private float _xPositionLimit = 11.24f;
    private float horizontalInput;
    private float verticalInput;

    // Other player vars.
    // Best practice to leave player health as private and only allow class function to access / modify.
    [SerializeField]
    private int _playerHealth = 3;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _playerScore = 0;
    public bool isPlayer1;
    public bool isPlayer2;

    // Player powerup vars.
    [SerializeField]
    private bool _hasTriplePowerUp;
    [SerializeField]
    private bool _hasSpeedUp;
    [SerializeField]
    private bool _hasShield;

    // Laser vars.
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    private float _laserOffset = 1.02f;

    // Other object vars.
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _shieldObject;
    [SerializeField]
    private GameObject[] _engineFires;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _laserSoundSource;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    private UIManager _UImanager;
    private GameManager _gameManager;
    private Animator _animator;

    // By default, all functionsn are set to private.
    void Start()
    {
        _hasTriplePowerUp = false;
        _hasSpeedUp = false;
        _hasShield = false;
        _playerSpeed = 5f;

        // Grab Animator.
        _animator = GetComponent<Animator>();

        // Grab SpawnManager.
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        // Grab UIManager.
        _UImanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Check if the spawn manager exists.
        if (_spawnManager == false)
        {
            Debug.LogError("Spawn manager is NULL");
        }

        _laserSoundSource = GetComponent<AudioSource>();
        _laserSoundSource.clip = _laserSoundClip;

        if (!_gameManager.isCoopMode)
        {
            // Set player initial position.
            transform.position = defaultPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.isGameOver)
        {
            Destroy(this.gameObject);
        }

        if (isPlayer1)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            var keyCodeRight = KeyCode.RightArrow;
            var keyCodeLeft = KeyCode.LeftArrow;
            MovementAnimation(keyCodeRight, keyCodeLeft);
            CalculateMovement(horizontalInput, verticalInput);

            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                Shoot();
            }
        }
        else if (isPlayer2)
        {
            horizontalInput = Input.GetAxis("Horizontal2");
            verticalInput = Input.GetAxis("Vertical2");
            verticalInput = -Input.GetAxis("Vertical2");
            var keyCodeRight = KeyCode.D;
            var keyCodeLeft = KeyCode.A;
            MovementAnimation(keyCodeRight, keyCodeLeft);
            CalculateMovement(horizontalInput, verticalInput);

            if (Input.GetKeyDown(KeyCode.Return) && Time.time > _canFire)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        _canFire = Time.time + _fireRate;

        // Laser to spawn at the player position.
        Vector3 playerCurrentPosition = transform.position;

        if (_hasTriplePowerUp)
        {
            Instantiate(_tripleLaserPrefab, playerCurrentPosition, Quaternion.identity);
        }
        else
        {
            // Instantiate with X position = player position, Y position = player position + offsest, and Quarrternio (rotation) = 0.
            Instantiate(_laserPrefab, playerCurrentPosition + new Vector3(0, _laserOffset, 0), Quaternion.identity);
        }
        _laserSoundSource.Play();
    }

    void CalculateMovement(float horizontalInput, float verticalInput)
    {
        // Take user input and translate (move) the player object.
        Vector3 playerDirection = new Vector3(horizontalInput, verticalInput, 0);

        // Time.deltaTime allows you to move it real time (i.e. 1 = 1 m/s).
        if (_hasSpeedUp)
        {
            transform.Translate(playerDirection * _playerSpeed * Time.deltaTime * _speedMultiplier);
        }
        else
        {
            transform.Translate(playerDirection * _playerSpeed * Time.deltaTime);
        }
        
        // Get player current x and y positions.
        _xPosition = transform.position.x;
        _yPosition = transform.position.y;

        // Y-axis limit. Player won't be able to go over the y limits.
        transform.position = new Vector3(_xPosition, Mathf.Clamp(_yPosition, _yPositionMinusLimit, _yPositionLimit), 0);

        // X-axis limit. When the player hits either end of the x limit, move it to the other side of the screen.
        if (_xPosition >= _xPositionLimit)
        {
            transform.position = new Vector3(-_xPositionLimit, _yPosition, 0);
        }

        else if (_xPosition <= -_xPositionLimit)
        {
            transform.position = new Vector3(_xPositionLimit, _yPosition, 0);
        }
    }

    private void MovementAnimation(KeyCode rightKey, KeyCode leftKey)
    {
        // Moving Right.
        if (Input.GetKeyDown(rightKey))
        {
            _animator.SetBool("IsMovingRight", true);
            _animator.SetBool("IsMovingLeft", false);
        }
        else if (Input.GetKeyUp(rightKey))
        {
            _animator.SetBool("IsMovingRight", false);
            _animator.SetBool("IsMovingLeft", false);
        }

        // Moving Left.
        if (Input.GetKeyDown(leftKey))
        {
            _animator.SetBool("IsMovingRight", false);
            _animator.SetBool("IsMovingLeft", true);
        }
        else if (Input.GetKeyUp(leftKey))
        {
            _animator.SetBool("IsMovingRight", false);
            _animator.SetBool("IsMovingLeft", false);
        }       
    }

    // Takes one life away from the player and if player has 0 health left, destroys.
    public void PlayerDamage()
    {
        if (_hasShield)
        {
            _hasShield = false;
            _shieldObject.SetActive(false);
            return;
        }
        _playerHealth--;
        switch (_playerHealth)
        {
            case 0:
                _UImanager.CheckBestScore(_playerScore);
                _gameManager.GameOver();
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
                GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(explosion, 2.6f);
                break;
            case 1:
                _engineFires[0].SetActive(true);
                _engineFires[1].SetActive(true);
                break;
            case 2:
                _engineFires[Random.Range(0, 2)].SetActive(true);
                break;
            default:
                break;
        }
        _UImanager.UpdatePlayerLivesImage(_playerHealth);
    }

    public void PlayerScore(int score)
    {
        _playerScore += score;
        _UImanager.UpdatePlayerScore(_playerScore);
    }

    public void PlayerPowerUp()
    {
        _hasTriplePowerUp = true;
        StartCoroutine(PowerupCountDown());
    }

    public void PlayerSpeedUp()
    {
        _hasSpeedUp = true;
        StartCoroutine(SpeedupCountDown());
    }

    public void PlayerShieldUP()
    {
        _hasShield = true;
        _shieldObject.SetActive(true);
    }

    IEnumerator PowerupCountDown()
    {
        while (_hasTriplePowerUp == true)
        {
            yield return new WaitForSeconds(5.0f);
            _hasTriplePowerUp = false;
        }
    }

    IEnumerator SpeedupCountDown()
    {
        while (_hasSpeedUp == true)
        {
            yield return new WaitForSeconds(5.0f);
            _hasSpeedUp = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If an enemy collides with a laser.
        if (collision.tag == "EnemyLaser")
        {
            PlayerDamage();
            Destroy(collision.gameObject);
        }
    }
}