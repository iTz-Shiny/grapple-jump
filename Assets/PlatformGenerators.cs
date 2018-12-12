using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerators : MonoBehaviour {

    public GameObject platform;
    public GameObject redPlatform;
    public GameObject bluePlatform;
    public Transform GenerationPoint;
    public float distanceBetween;
    public GameObject[] platforms;
    private float random;

    // Use this for initialization
    void Start () {
        platforms = new GameObject[] { redPlatform, bluePlatform };

	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.x < GenerationPoint.position.x)
        {
            var randomPlatform = platforms[Random.Range(0, 2)];
            transform.position = new Vector3(transform.position.x + randomPlatform.GetComponent<BoxCollider2D>().size.x + distanceBetween, Random.value * 8, transform.position.z);
            //random = Random.Range(0, platforms.Length - 1);
            var current = Instantiate(randomPlatform, transform.position, transform.rotation);
            //current.transform.localScale *= (Random.value * 2) + 1;
        }
	}
}
