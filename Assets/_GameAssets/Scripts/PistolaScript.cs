using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolaScript : MonoBehaviour {

    Animator miPistola;

    // Use this for initialization
    void Start () {
        miPistola = GetComponent<Animator>();
        miPistola.SetBool("Rotar", true);
	}

    public void QuitarAnimation() {
        // ESTA DENTRO DEL COFRE
        miPistola.SetBool("Rotar", false);
    }

    public void EsconderPistola() {
        gameObject.SetActive(false);
    }

}
