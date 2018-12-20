using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
      
        // LOS BUSCAMOS POR SU TAG PARA SABER CONTRA QUIEN GOLPEO
        if (other.gameObject.tag == "Vigilante") {
            // LA BALA HA COLISIONADO CON EL VIGILANTE Y LE QUITAMOS VIDA AL VIGILANTE
            other.GetComponent<VigilanteScript>().QuitarVidas();
        }
        // SI TOCA AL PLAYER DE LA SEGUNDA ESCENA
        else if (GameControler.GetNivel() == 2 && other.gameObject.name == "Player") {
            // LA BALA HA COLISIONADO CON EL PLAYER Y LE QUITAMOS VIDA");
            other.GetComponent<PlayerScript>().QuitarVidas();
        }
        // SI TOCA AL VIGILANTE ESTATICO DE LA SEGUNDA ESCENA
        else if (other.gameObject.name == "VigilanteEstatico") {
            // LA BALA HA COLISIONADO CON EL PLAYER Y LE QUITAMOS VIDA");
            other.GetComponent<VigilanteEstaticoScript>().QuitarVidas();
        }



        Destroy(gameObject, 2);

    }    

}
