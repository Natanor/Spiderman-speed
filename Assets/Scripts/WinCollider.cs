using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCollider : MonoBehaviour
{
    AudioSource winNoise;
    public Canvas winCanvas;
    public Canvas mainCanvas;
    public GameController gameController;


    private void Start()
    {
        winNoise = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                mainCanvas.gameObject.SetActive(false);
                winCanvas.gameObject.SetActive(true);

                //winNoise.Play();
                gameController.WinLevel();

            }
        }
    }
}
