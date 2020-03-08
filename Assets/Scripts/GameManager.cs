using System.Collections;
using System.Collections.Generic;
using BallMerge;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _ballPrefab;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] float delay = 2;
    [SerializeField] int _count = 1;
    public List<GameObject> ballsList = new List<GameObject>();
    private static GameManager _instance;
    public static GameManager Instance {
        get {            
            return _instance;
        }        
    }
    float _area=2.6f;

    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        StartCoroutine(_BallSpawn());
    }
    IEnumerator _BallSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);   
            for (int i = 0; i < _count; i++)
            {
                var _ball = Instantiate(_ballPrefab,Vector3.zero,Quaternion.identity,_spawnPoint);
                ballsList.Add(_ball);
                _ball.transform.localPosition = new Vector3(Random.Range(-_area,_area),0,0);
                 var __ball =_ball.GetComponent<Ball>();
                __ball.BallValue = Random.Range(2,7);
                __ball.Initialize();
            }                       
        }        
    }
}
