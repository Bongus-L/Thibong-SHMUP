using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerupContainer;
    [SerializeField]
    private GameObject[] _gameItem;
    private bool _isPlayerDead = false;
    private float _xPositionLimit = 10f;
    private float _yOriginalPosition = 7f;

    public void StartSpawning()
    {
        StartCoroutine(EnemySpawn());
        StartCoroutine(PowerupSpawn());
    }

    // This function keeps the game going continously. Every 3 seconds, it will spawn an enemy.
    IEnumerator EnemySpawn()
    {
        yield return new WaitForSeconds(3.0f);
        while (_isPlayerDead == false)
        {
            Vector3 enemyPosition = new Vector3(Random.Range(-_xPositionLimit, _xPositionLimit), _yOriginalPosition, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, enemyPosition, Quaternion.identity);
            // Line below ensures that you spawn enemy wihtin the enemy container.
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    // This functions spawns random powerup item.
    IEnumerator PowerupSpawn()
    {
        yield return new WaitForSeconds(3.0f);
        while (_isPlayerDead == false)
        {
            yield return new WaitForSeconds(5.0f);
            Vector3 powerupPosition = new Vector3(Random.Range(-_xPositionLimit, _xPositionLimit), _yOriginalPosition, 0);
           
            //Randomely select item type to spawn.
            int itemTypeNumber = Random.Range(0, 3);
            GameObject newPowerup = Instantiate(_gameItem[itemTypeNumber], powerupPosition, Quaternion.identity);
            
            // Instantiate within 'ItemContainer' to keep it clean.
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(10.0f);
        }
    }

    public void OnPlayerDeath()
    {
        _isPlayerDead = true;
    }
}