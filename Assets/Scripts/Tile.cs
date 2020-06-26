using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public bool state;
    public int x, y;
    Renderer rend;
    Animator animator;

    // Use this for initialization
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Hited()
    {
        if (state && Controller.controllerSingleton.EvaluateMovement(this))
        {
            state = false;
            Controller.controllerSingleton.EvaluateSituation(this);
        }
    }

    public void FallTile()
    {
        animator.SetBool("Fall", true);
        StartCoroutine("StepedTile");
    }

    IEnumerator StepedTile()
    {
        yield return new WaitForSeconds(1);
        Controller.controllerSingleton.SetPhantomTile(this);
        Destroy(gameObject);
    }
}
