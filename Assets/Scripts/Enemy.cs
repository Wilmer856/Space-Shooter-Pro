using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player player;

    private Animator _anim;
    private AudioSource _audioSource;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if(player == null)
        {
            Debug.LogError("Player not found!");
        }

        _anim = GetComponent<Animator>();

        if(_anim == null)
        {
            Debug.LogError("Can't find Animation");
        }

    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y <= -5f)
        {
            float x = Random.Range(-11.3f, 11.3f);
            transform.position = new Vector3(x, 7f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) {

            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            GetComponent<Collider2D>().enabled = false;
            _audioSource.Play();
            Destroy(this.gameObject,2.8f);
        }

        if(other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            
            if(player != null)
            {
                player.updateScore(Random.Range(5,12));
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            GetComponent<Collider2D>().enabled = false;
            _audioSource.Play();
            Destroy(this.gameObject,2.8f);

        }

    }
}
