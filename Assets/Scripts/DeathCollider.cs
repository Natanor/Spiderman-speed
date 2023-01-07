using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCollider : MonoBehaviour
{
    AudioSource deathNoise;
    public Canvas deathCanvas;
    public Canvas mainCanvas;

    private void Start()
    {
        deathNoise= GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                mainCanvas.gameObject.SetActive(false);
                deathCanvas.gameObject.SetActive(true);

                StartCoroutine(player.Die());
            }
        }
    }
}
