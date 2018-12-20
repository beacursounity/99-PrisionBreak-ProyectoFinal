using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SeleccionNivelesScript : MonoBehaviour {

	[SerializeField] GameObject player;
	Animator miAnimator;
	int nivel = 1;
    [SerializeField] GameObject nivel2;

	//
	int empieza = 1;
	int repetircada = 2;

    [SerializeField] Text txtHightScore;

    // Use this for initialization
    void Start () {

        miAnimator = player.GetComponent<Animator>();
		miAnimator.SetBool("Portada",false);
		InvokeRepeating("CambiarAnimation",empieza,repetircada);

        // REFERENCIA DEL SCRIPT DEL PLAYER PARA RECOGER EL NIVEL
        nivel = GameControler.GetNivel();
		// SI EL NIVEL 2 ESTA ACTIVADO LE PONEMOS ENABLED PARA QUE SE VEA EL TEXTO
		if (nivel == 2){
            nivel2.SetActive(true);
        }

        // RECOGEMOS EL HIGHT SCORE DEL PLAYPREFS
        txtHightScore.text = "Hight Score: "+GameControler.GetPuntosMaximos();
		
	}
	
	private void CambiarAnimation() {
		if (miAnimator.GetBool("Portada") == false){
			miAnimator.SetBool("Portada",true);
		}
		else
		{
			miAnimator.SetBool("Portada",false);
		}
	}

	public void NivelUno () {
		// SI EL NIVEL ES 1 TE DEJARA IR AL NIVEL 1
		// PERO SI NO LO NO ES QUE YA LO HA TERMINADO Y 
		// NO PUEDE VOLVER HACIA ATRAS 
		if (nivel == 1){
            GameControler.StoreLlaves(0);
            GameControler.StoreNivel(1);
			SceneManager.LoadScene(2);
		}
	}

	public void NivelDos () {
        int nivel = 2;
		GameControler.StoreNivel(nivel);
		SceneManager.LoadScene(3);
	}
}
