using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cobweb : MonoBehaviour
{
    public bool isDestroyable;
    PlayerController player;
	public float velocityIncreaseValue;
	AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
	    player = FindObjectOfType<PlayerController>();
	    audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 newVel = Vector2.Reflect(rb.velocity, gameObject.transform.up);
            newVel = newVel * velocityIncreaseValue;
            rb.velocity = newVel;

            //play anims and sfx and stuff

            if (isDestroyable) 
            {
                Destroy(gameObject);
            }
        }
    }
}
