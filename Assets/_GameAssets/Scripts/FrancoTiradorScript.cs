using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrancoTiradorScript : MonoBehaviour {
    // DONDE ESTARIA LA MIRILLA
    [SerializeField] Transform puntoDisparo;
    [SerializeField] Camera miCamara;
    [SerializeField] GameObject mirilla;

    bool apuntando = false;
    float currentFOV;
    float minFOV = 25;
    float maxFOV;

    private void Start() {
        // FIJAMOS EL MAXIMO Y EL CURRENT EN LA POSICION ACTUAL
        maxFOV = miCamara.fieldOfView;
        currentFOV = miCamara.fieldOfView;

    }
    private void Update() {
        // PULSO EL BOTON DERECHO ACERCO LA VISTA AL OBJETIVO
        if (Input.GetMouseButtonDown(1)) {
            Debug.Log("Boton derecho pulsado");
            apuntando = true;
            mirilla.SetActive(true);
        }
        // CUANDO LO SOLTAMOS
        if (Input.GetMouseButtonUp(1)) {
            Debug.Log("Boton derecho soltado");
            apuntando = false;
            mirilla.SetActive(false);
        }

        // ESTO ES EL ZOOM DE LA CAMARA
        // IRA POR FRAMES HAY QUE USAR EL DELTATIME
        if (apuntando) {
            currentFOV = currentFOV - 1;
            // QUE COJA VALOR MAXIMO DE LOS DOS PARA QUE NO SE PASE
            // DE VALOR 
            currentFOV = Mathf.Max(currentFOV, minFOV);
        }
        // SI ES FALSO ES QUE LO HEMOS SOLTADO Y 
        // DEBERIA DE VOLVER HACIA ATRAS
        else {
            currentFOV = currentFOV + 1;
            // QUE COJA VALOR MINIMO DE LOS DOS PARA QUE NO SE PASE
            // DE VALOR 
            currentFOV = Mathf.Min(currentFOV, maxFOV);

        }


        miCamara.fieldOfView = currentFOV;
    }

    private void OnMouseDown() {
        //print("click");
        // DIRECCION DEL DISPARO
        Vector3 forward = puntoDisparo.forward;

        // CREAMOS UN RAYO
        Ray rayo = new Ray(puntoDisparo.position, forward);

        // DECLARAMOS EL OBJETO QUE RECOGE EL IMPACTO
        RaycastHit hitInfo;
        // DEPURAMOS EL RAYO
        Debug.DrawRay(puntoDisparo.position,puntoDisparo.forward,Color.red,5);

        // LANZAMOS EL RAYCAST
        // EN EL JUEGO EL PUNTO (RAYO) TENDRA QUE SE LA CAMARA.
        bool diana = Physics.Raycast(rayo, out hitInfo, 25);

        //if (diana) {
            //print(hitInfo.collider.gameObject.name);
            // DIBUJA UNA LINEA DE DISPARO 
            //Debug.DrawLine(puntoDisparo.position,
          //              hitInfo.collider.transform.position,
            //            Color.red, 5);
        //} else {
            //print("AGUA");
        //}


    }

}
