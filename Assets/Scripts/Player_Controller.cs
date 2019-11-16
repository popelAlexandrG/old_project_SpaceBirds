using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour {

    public Vector2 UpForce = new Vector2();
    public GameObject explosion;
    public Camera cam;

    private bool _noGaming = false;

    // Use this for initialization
    void Start () {
        transform.position = new Vector2(-2f, 0);
        _noGaming = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!_noGaming)
        {
            if (Input.GetMouseButtonDown(0) && !_noGaming)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().AddForce(UpForce);
            }

            Vector2 position = cam.WorldToScreenPoint(transform.position);
            if (position.y > Screen.height || position.y < 0)
            {
                GameOver();
            }
        }
        else StartCoroutine(DestroyPlayer());
    }

    private IEnumerator DestroyPlayer()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D()
    {
        if (!_noGaming) GameOver();
    }

    private void GameOver()
    {
        _noGaming = true;
        GameObject InstExplosion = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(InstExplosion, 2);
        Messenger<bool>.Broadcast(GameEvent.GAMEOVER, true);
    }
}
