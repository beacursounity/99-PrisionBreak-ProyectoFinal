using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {


    public static int llaves = 0; 
    int numMaximoLlavesPrimerNivel = 4; 
    static int numMaximoLlavesAbrirCarcelEscape = 4;   

    int puntuacion = 5;
    int numNivel = 2;
    int numNivelNuevo = 3;

    [SerializeField] GameObject puertaCalabozo;
    [SerializeField] GameObject player;

    private void OnTriggerEnter(Collider collision) {

        if ( collision.gameObject.name == "Player") {

            // QUITAMOS EL BOXCOLLIDER DE LA LLAVE PARA QUE NO CHOUE MAS Y PUNTUE MAS VECES
            GetComponent<BoxCollider>().enabled = false;
            if (llaves == 0) llaves = GameControler.GetLlaves();
            llaves = llaves + 1;

            // CARGAMOS EN EL PLAYPREFS LAS LLAVES
            GameControler.StoreLlaves(llaves);

           // RECOGER LA PUNTUACION
           // INCREMENTAMOS PUNTOS POR COGER LA PISTA DEL BAUL !!!!!!!!!!! PROBARLO CUANDO FUNCIONE
           collision.gameObject.GetComponent<PlayerScript>().IncrementarPuntuacion(puntuacion);
           collision.gameObject.GetComponent<PlayerScript>().MostrarLlaves(llaves);
            // PARTICULAS
            gameObject.GetComponent<ParticleSystem>().Play();
            // AUDIO
            gameObject.GetComponent<AudioSource>().Play();

            // HAY QUE ABRIR LA PUERTA DEL CALABOZO SOLO PARA EL NIVEL 1
            if (GameControler.GetNivel() == 1 && llaves >= numMaximoLlavesPrimerNivel ) {
                puertaCalabozo.gameObject.GetComponent<PuertaCalabozoScript>().AbrirPuerta();
                // CARGAMOS LAS LLAVES QUE HEMOS RECOGIDO EN EL NIVEL 1
                GameControler.StoreLlavesNiveAnterior(llaves);
            }
            // PARA QUE HABRA LA PUERTA DEL NIVEL 2 SOLO RECOGERA 3 LLAVES
            else if (GameControler.GetNivel() == 2 && 
            llaves >= GameControler.GetLlavesNivelAnterior() + numMaximoLlavesAbrirCarcelEscape )
            {
                // ME VOY AL NIVEL 3 QUE ES LA PUERTA DE LA CARCEL PARA QUE SE VAYA
                // AL HELICOPTERO
                collision.gameObject.GetComponent<PlayerScript>().SetNivel(numNivelNuevo);
                // CARGAMOS EL NIVEL
                GameControler.StoreNivel(numNivelNuevo);
            }

            // DESTRUIMOS EL OBJETO
            Destroy(this.gameObject,1);
        }
            
    }

    public int GetLlaves(){
        return llaves;
    }
}
