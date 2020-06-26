using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{

    public static Controller controllerSingleton;
    Tile[,] tiles;
    public GameObject tile;
    public GameObject phantomTile;
    public GameObject winMessage;
    public GameObject loseMessage;
    public int size = 5;
    Tile currentTile;
    Tile lastTile;
    Vector3 currentTilePos;
    public GameObject character;
    public Animator charAnimator;
    bool canMove;
    bool charCanMove;
    Queue<Tile> qdTiles;
    bool crRunning;

    public Vector2[] deleteTiles;
    public Vector2 initial;
    Tile iniTile;
    public Vector2 final;
    Tile finalTile;

    private void Awake()
    {
        CameraFollow.size = this.size;
    }


    // Use this for initialization
    void Start()
    {
        controllerSingleton = this;
        canMove = false;
        charCanMove = false;
        crRunning = false;
        qdTiles = new Queue<Tile>();

        tiles = new Tile[size, size];

        Tile actualTile;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                tiles[i, j] = Instantiate(tile, new Vector3(j, 0, i), Quaternion.identity).GetComponentInChildren<Tile>();
                actualTile = tiles[i, j];
                actualTile.state = true;
                actualTile.x = j;
                actualTile.y = i;

            }
        }

        iniTile = tiles[(int)initial.y, (int)initial.x];
        finalTile = tiles[(int)final.y, (int)final.x];

        currentTile = iniTile;
        lastTile = currentTile;

        iniTile.state = false;
        iniTile.gameObject.transform.parent.gameObject.SetActive(true);
        iniTile.gameObject.GetComponent<Renderer>().material.color = Color.red;
        currentTilePos = iniTile.transform.parent.gameObject.transform.position;
        finalTile.gameObject.transform.parent.gameObject.SetActive(true);
        finalTile.gameObject.GetComponent<Renderer>().material.color = Color.green;

        Vector3 charPos = iniTile.transform.parent.gameObject.transform.position;
        charPos.x += 0.1f;
        charPos.y += 0.3f;
        charPos.z += 0.1f;
        character.transform.position = charPos;


        StartCoroutine("Animations");
    }

    IEnumerator Animations()
    {
        foreach (Tile tile in tiles)
        {
            tile.gameObject.transform.parent.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine("DeletingAnimations");
    }

    IEnumerator DeletingAnimations()
    {
        foreach (Vector2 erase in deleteTiles)
        {
            tiles[(int)erase.y, (int)erase.x].FallTile();
            tiles[(int)erase.y, (int)erase.x].state = false;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        canMove = true;
    }

    public void SetPhantomTile(Tile tile)
    {
        Instantiate(phantomTile, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
    }


    public bool EvaluateMovement(Tile tile)
    {
        if (canMove)
        {
            int tileX = tile.x;
            int tileY = tile.y;
            if (tileX < size - 1 && tiles[tileY, tileX + 1] == currentTile)
            {
                SetTile(tile);
                return true;
            }
            if (tileX > 0 && tiles[tileY, tileX - 1] == currentTile)
            {
                SetTile(tile);
                return true;
            }
            if (tileY < size - 1 && tiles[tileY + 1, tileX] == currentTile)
            {
                SetTile(tile);
                return true;
            }
            if (tileY > 0 && tiles[tileY - 1, tileX] == currentTile)
            {
                SetTile(tile);
                return true;
            }
        }
        
        return false;
    }

    void SetTile(Tile tile)
    {
        currentTile = tile;
        currentTile.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        qdTiles.Enqueue(tile);
    }

    public void EvaluateSituation(Tile tile)
    {
        if (currentTile == finalTile)
        {
            EvaluateEndSituation();
            canMove = false;
            return;
        }
        int tileX = tile.x;
        int tileY = tile.y;
        if (tileX < size - 1 && tiles[tileY, tileX + 1].state) return;
        if (tileX > 0 && tiles[tileY, tileX - 1].state) return;
        if (tileY < size - 1 && tiles[tileY + 1, tileX].state) return;
        if (tileY > 0 && tiles[tileY - 1, tileX].state) return;
        EvaluateEndSituation();
    }

    


    void EvaluateEndSituation()
    {
        foreach (Tile tile in tiles)
        {
            if (tile.state)
            {
                loseMessage.SetActive(true);
                return;
            }
        }
        winMessage.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = currentTilePos;
        target.y = 0.3f;
        character.transform.LookAt(target);
        if (charCanMove)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, target, Time.deltaTime * 3f);
            Vector3 charPos = character.transform.position;
            charPos.y = 0.3f;
            float distance;
            distance = Vector3.Distance(charPos, target);
            if (distance == 0) charCanMove = false;
        }

        if (!crRunning && qdTiles.Count != 0)
        {
            StartCoroutine("DqTiles");
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Tile hitdTile = hit.collider.GetComponent<Tile>();
                if (hitdTile != null) hitdTile.Hited();
            }
        }
    }

    IEnumerator DqTiles()
    {
        Debug.Log("Working");
        crRunning = true;
        Tile dqdTile;
        while (qdTiles.Count > 0)
        {
            charCanMove = true;
            charAnimator.SetBool("Jump", true);
            lastTile.FallTile();
            dqdTile = qdTiles.Dequeue();
            currentTilePos = dqdTile.transform.parent.gameObject.transform.position;
            yield return new WaitForSeconds(0.3f);
            lastTile = dqdTile;
            charAnimator.SetBool("Jump", false);
            yield return new WaitForFixedUpdate();
        }
        crRunning = false;
    }

}
