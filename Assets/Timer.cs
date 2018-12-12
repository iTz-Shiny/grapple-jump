using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour {
    
    public Text timerText;
    public Text distanceText;
    private float startTime;
    private float startingTime;
    public Vector3 startPoint;

	// Use this for initialization
	void Start () {
        //Give 1 minute
        startingTime = 60;
        startTime = Time.time;
        startPoint = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        float t = startingTime - (Time.time - startTime);
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        float distance = transform.position.x - startPoint.x;


        timerText.text = minutes + ":" + seconds;
        distanceText.text = distance.ToString("f2") + "m";
	}
}
