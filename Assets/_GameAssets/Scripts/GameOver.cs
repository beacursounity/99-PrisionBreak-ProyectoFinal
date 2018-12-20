using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

[SerializeField] GameObject player;
	Animator miAnimator;
	AudioSource audio;

	//
	int empieza = 1;
	int repetircada = 3;

	// Use this for initialization
	void Start () {
		miAnimator = player.GetComponent<Animator>();
		miAnimator.SetBool("GameOver",false);
		// RECOJO EL AUDIO
		audio = player.GetComponent<AudioSource>();
		// REPETIMOS LA ANIMACION CADA 3 SEGUNDOS
		InvokeRepeating("CambiarAnimation",empieza,repetircada);
		
	}
	
	private void CambiarAnimation() {
		if (miAnimator.GetBool("GameOver") == false){
			miAnimator.SetBool("GameOver",true);
			audio.Play();
		}
		else
		{
			miAnimator.SetBool("GameOver",false);
		}
	}

	// SI DAMOS CUALQUIER TECLA VOLVEMOS AL MENU DEL NIVEL 
	public void CambiarEscenaNiveles() {
        GameControler.StoreLlaves(0);
        SceneManager.LoadScene(1);
	}
}
