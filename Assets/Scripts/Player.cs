using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private GameObject _shieldPrefab;
    [SerializeField]
    private float fireRate = 0.15f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private SpawnManager _spawnManager;
    private bool tripleShotOn = false;
    private bool speedBoostOn = false;
    private bool shieldOn = false;

    [SerializeField]
    private int _score;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioSource _audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource =GetComponent<AudioSource>();

        if(_spawnManager == null)
        {
            Debug.LogError("The sspawn manager is null!");
        }

        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is null!");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio source on player is null");
        } else
        {
            _audioSource.clip = _laserSound;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            fireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.80f, 0), 0);

        if (transform.position.x <= -11.30f)
        {
            transform.position = new Vector3(11.30f, transform.position.y, 0);
        }
        else if (transform.position.x >= 11.30f)
        {
            transform.position = new Vector3(-11.30f, transform.position.y, 0);
        }
    }

    void fireLaser()
    {
        _canFire = Time.time + fireRate;
        if(tripleShotOn)
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
        } else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();

    }

    public void Damage()
    {

        if (shieldOn)
        {
            _shieldPrefab.SetActive(false);
            shieldOn = false;
            return;
        }

        _lives--;

        if(_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if(_lives == 1) {
            _leftEngine.SetActive(true);
        }
        _uiManager.UpdateLives(_lives);
        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void ToggleTripleShot()
    {
        tripleShotOn = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void ToggleSpeedBoost()
    {
        speedBoostOn = true;
        _speed = 10f;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ToggleShield()
    {
        if(!shieldOn)
        {
            shieldOn = true;
            _shieldPrefab.SetActive(true);
        }
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        while(tripleShotOn)
        {
            yield return new WaitForSeconds(5.0f);
            tripleShotOn = false;
        }
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        while(speedBoostOn)
        {
            yield return new WaitForSeconds(5.0f);
            speedBoostOn = false;
            _speed = 5f;
        }
    }

    public void updateScore(int points)
    {
        _score+=points;
        _uiManager.UpdateScore(_score);
    }
}
