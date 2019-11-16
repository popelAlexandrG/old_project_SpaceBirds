using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geme_Manger : MonoBehaviour {

    public GameObject player;
    public GameObject[] aster;
    public Camera cam;

    public float gup;
    public float complexityDelta;

    private readonly float _delta = 0.8f;

    private float _score = 0;
    private float _complexityFactor = 1;

    private bool _gOver = false;
    private float speedGame = 2f;

    private void Awake()
    {
        Messenger<bool>.AddListener(GameEvent.GAMEOVER, GameOver);              //Использую данный скрипт потому, что к нему привык. Использовал его с самого начала. 
                                                                                //Возможно есть какие-то альтернативы. Я даже не задумывался, он меня полностью устраивает
    }

    void Start () {
        GameObject instPlayer = Instantiate(player);
        instPlayer.GetComponent<Player_Controller>().cam = cam;
        InvokeRepeating("InstatceAster", 0f, speedGame);
    }
	
    public void InstatceAster()
    {
        if (Score > complexityDelta) ComplexityFactor += _score / complexityDelta / 20;
        speedGame /= ComplexityFactor * 1.5f;

        float rPos = cam.orthographicSize - (cam.orthographicSize - _delta - gup) * Random.value;
        GameObject asterUP = Instantiate(aster[Random.Range(0, aster.Length)]);
        asterUP.transform.position = new Vector2(cam.orthographicSize *3f, rPos);
        asterUP.GetComponent<Aster_Controller>().up = true;
        asterUP.GetComponent<Aster_Controller>().complexityFactor *= _complexityFactor;

        GameObject asterDown = Instantiate(aster[Random.Range(0, aster.Length)]);
        asterDown.transform.position = new Vector2(cam.orthographicSize * 3f, rPos - gup - cam.orthographicSize + _delta);
        asterDown.GetComponent<Aster_Controller>().up = false;

        asterDown.GetComponent<Aster_Controller>().complexityFactor *= _complexityFactor;

    }
    private void Update()
    {
        if (!_gOver)
        {
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * 10);            
        }
        else CancelInvoke();
    }

    private void FixedUpdate()
    {
        if (!_gOver) Score += 1;
    }

    public float Score
    {
        get { return _score; }
        set
        {
            //_score = Mathf.Clamp(value, 0, 10000); // Если нужен финал игры
            _score = value;
            Messenger<float>.Broadcast(GameEvent.SCORE_CHANGE, _score);
        }
    }
    public float ComplexityFactor
    {
        get { return _complexityFactor; }
        set
        {
            _complexityFactor = value;
            Messenger<float>.Broadcast(GameEvent.LEVEL, _complexityFactor);
        }
    }

    private void GameOver(bool gOver)
    {
        _gOver = gOver;
        Messenger<float>.Broadcast(GameEvent.LEVEL, 1);
    }

    private void OnDestroy()
    {
        Messenger<bool>.RemoveListener(GameEvent.GAMEOVER, GameOver);
    }
}
