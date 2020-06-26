using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitForCredits : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("Wait");
	}
	
	IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
