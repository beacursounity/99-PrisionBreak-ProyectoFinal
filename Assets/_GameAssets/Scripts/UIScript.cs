using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    // PARA CARGAR TODAS LAS VIDAS
    public Image[] imagenesVida;
    public Image prefabImagenVida;
    // TENEMOS QUE RECOGER EL PANEL DE VIDAS PARA RECOGER LA REFERENCIA
    public GameObject panelVidas;

    public PlayerScript player;
    int numeroVidas;

    // Use this for initialization
    void Start() {
        // SI EL NIVEL ES 1 METEMOS TODOS LAS CARAS
        if (GameControler.GetNivel() == 1) { 
            numeroVidas = player.GetVidas();
        } else {
            // LO INICIALIZO
            numeroVidas = 5;
        }
        imagenesVida = new Image[numeroVidas];

		for (int i = 0; i < imagenesVida.Length; i++) {
            // INSTANCIAMOS EL PREFAB Y LO METEMOS DENTRO DEL PANELVIDAS
            imagenesVida[i] = Instantiate(prefabImagenVida, panelVidas.transform);
        }

        // LLAMAMOS A ESTE METODO POR SI ACASO YA ME HA QUITADO VIDAS       
        RestarVida();
       
    }

    public void RestarVida() {
        // LE CAMBIAMOS EL COLOR
        numeroVidas = player.GetVidas();
        if (numeroVidas > 0) {
            for (int i = numeroVidas; i < imagenesVida.Length; i++) {
                // SE DESTRUIRA CUANDO NO LO HALLA DESTRUIDO ANTES
                if (imagenesVida[i].gameObject != null) {
                    imagenesVida[i].color = new Color32(160, 160, 160, 128); //(,,,nIVEL ALPHA)
                }
            }
        }

    }

    public void ResetearPlayerRefs() {
        PlayerPrefs.DeleteAll();
    }
   
}
