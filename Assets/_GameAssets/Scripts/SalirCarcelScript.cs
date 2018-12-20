using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalirCarcelScript : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Player"){
			// CUANDO SALGA DE LA CARCEL PONDRA LA VARIABLE A TRUE PARA
			// QUE LE VIGILANTE YA LE PUEDA PERSEGUIR
			other.gameObject.GetComponent<PlayerScript>().SetSalirCarcel(true);
			Destroy(gameObject);
		}
	}
}
