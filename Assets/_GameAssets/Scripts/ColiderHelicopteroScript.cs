using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderHelicopteroScript : MonoBehaviour {

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.name == "Player"){
			other.gameObject.GetComponent<PlayerScript>().EntrarHelicoptero();
		}

	}
}
