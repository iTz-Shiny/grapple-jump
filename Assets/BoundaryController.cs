using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundaryController : MonoBehaviour {

    public GameObject player;
    private Rigidbody2D rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        //If player goes below map or pass the sides: Reset
        if (player.transform.position.y < 0 || player.transform.position.x < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //If player goes above, sent them back down at an angle.
        if (player.transform.position.y >= 14)
        {
            Vector3 currentVelocity = rBody.velocity;
            currentVelocity[1] = currentVelocity[1]*-1;
            rBody.velocity = currentVelocity;

        }

	}
}
