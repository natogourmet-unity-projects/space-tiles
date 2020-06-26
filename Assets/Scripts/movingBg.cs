using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingBg : MonoBehaviour {

    Renderer bgRend;
    Vector2 offset;
    public 

	// Use this for initialization
	void Start () {
        bgRend = gameObject.GetComponent<Renderer>();
	}


    
    // Update is called once per frame
    void Update () {
        offset.x += Time.deltaTime * 0.005f;
        bgRend.material.mainTextureOffset = offset;
    }
}
