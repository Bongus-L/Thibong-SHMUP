using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;
    private float _yPositionLimit = -6f;

    [SerializeField]
    private int _itemTypeNumber;

    [SerializeField]
    private AudioClip _powerupSoundClip;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _yPositionLimit )
        {
            Destroy(this.gameObject);
        }
    }

    // When a player collects an item.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                AudioSource.PlayClipAtPoint(_powerupSoundClip, transform.position);
                switch (_itemTypeNumber)
                {
                    case 0:
                        player.PlayerPowerUp();
                        Destroy(this.gameObject);
                        break;
                    case 1:
                        player.PlayerSpeedUp();
                        Destroy(this.gameObject);
                        break;
                    case 2:
                        player.PlayerShieldUP();
                        Destroy(this.gameObject);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
