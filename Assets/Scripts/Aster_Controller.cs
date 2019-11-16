using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aster_Controller : MonoBehaviour {

    public float speed;

    [HideInInspector]
    public bool up;
    [HideInInspector]
    public float complexityFactor;

    private void Awake()
    {
        Messenger<bool>.AddListener(GameEvent.GAMEOVER, GameOver);
    }

    // Use this for initialization
    void Start () {
        float randPosY = speed / 2 * Random.value;
        if (up) randPosY *= -1;
        float complexity = 1;
        if (complexityFactor > 1) complexity = complexityFactor;

        GetComponent<Rigidbody2D>().velocity = new Vector2(-speed * (complexity), randPosY);
    }
	
	// Update is called once per frame
	void Update () {
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void GameOver(bool gOver)
    {
        if (gOver)
        {
            float randPosY = speed / 2 * Random.value;
            if (up) randPosY *= -1;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, randPosY);
        }
    }

    private void OnDestroy()
    {
        Messenger<bool>.RemoveListener(GameEvent.GAMEOVER, GameOver);
    }
}
