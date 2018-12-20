using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointScript : MonoBehaviour {

    [SerializeField] GameObject prefabLlaves;

    [SerializeField] GameObject puertacarcel;

    private void Start() {
        print("ARRANCANDO CALAVERA");
    }

    private void OnTriggerEnter(Collider collision) {
        //print("LA CALAVERA INFERNAL");
        if ( collision.gameObject.name == "Player") {
            // PLAYERPREFS ES COMO UN DICIONARIO
            PlayerPrefs.SetFloat("X",this.transform.position.x);
            PlayerPrefs.SetFloat("Y",this.transform.position.y);
            PlayerPrefs.SetFloat("Z",this.transform.position.z);

            // RECOGER LA PUNTUACION
            int puntuacion = collision.gameObject.GetComponent<PlayerScript>().GetPuntos();

            // RECOGER LA NIVEL
            int nivel = collision.gameObject.GetComponent<PlayerScript>().GetNivel();

            // RECOGER LA VIDAS
            int vidas = collision.gameObject.GetComponent<PlayerScript>().GetVidas();

            // RECOGER LA LLAVES
            int llaves = prefabLlaves.GetComponent<KeyScript>().GetLlaves();


            // LO SALVAMOS
            PlayerPrefs.Save();

            // LLAMAMOS AL GAMECONTROLER
            //GameControler.StorePosicion(collision.gameObject.transform.position);
            //GameControler.StorePuntos(puntuacion);
            GameControler.StoreNivel(nivel);
            //GameControler.StoreNivel(llaves);
            //GameControler.StoreVidas(vidas);
            //GameControler.StorePosicionPuertaCarcel(puertacarcel.gameObject.transform.localPosition);
            
            
            //print("GUARDANDO:" + puertacarcel.gameObject.transform.localPosition);

            // SONIDO CUANDO LE DA EL PLAYER
            gameObject.GetComponent<AudioSource>().Play();
            // DESTRUIMOS EL OBJETO
            Destroy(this.gameObject,1);
        }
            
    }
}