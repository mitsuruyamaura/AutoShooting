using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : MonoBehaviour {

    EnemyController enemyController;
    public bool isSearching;
    public GameObject player;

    void Start() {
        enemyController = GetComponentInParent<EnemyController>();
    }

	private void OnTriggerStay(Collider col) {
        if (col.gameObject.tag == "Player") {
            isSearching = true;
            player = col.gameObject;
            Debug.Log("発見した");
        }
    }

    private void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "Player") {
            isSearching = false;
            player = null;
            Debug.Log("見失った");
        }
    }
}
