using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class KeycardSpawner : MonoBehaviour
    {
        public GameObject keyCardPrefab;

        
        public void SpawnKeyCard()
        {
            var keycard = Instantiate(keyCardPrefab);
            keycard.transform.position = transform.position;

            Destroy(gameObject);
        }
    }