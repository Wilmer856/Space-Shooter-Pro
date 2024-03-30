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
    private GameObject[] powerUps;
    [SerializeField]
    private GameObject _triplePowerUpPrefab;
    [SerializeField]
    private GameObject _speedPowerUpPrefab;
    [SerializeField]
    private GameObject _shieldPowerUpPrefab;
    private bool _stopSpawning = false;
    // Start is called before the first frame update
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            float x = Random.Range(-9.0f, 9.0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(x, 7f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine() {

        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            float seconds = Random.Range(3f, 7f);
            int randomPowerup = Random.Range(0, 3);
            Instantiate(powerUps[randomPowerup], new Vector3(Random.Range(-8f,8f),7f,0), Quaternion.identity);
            yield return new WaitForSeconds(seconds);
        }
        
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
