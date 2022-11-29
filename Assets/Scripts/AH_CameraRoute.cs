using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_CameraRoute : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("AH_Player");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(0f, -1.65f + player.transform.position.y, -10f);
    }
}
