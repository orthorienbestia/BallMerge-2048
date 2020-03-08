using System.Collections;
using System.Collections.Generic;
using BallMerge;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _ballPrefab;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] float delay = 2;
    float _area=2.6f;
    void Start()
    {
        StartCoroutine(_BallSpawn());
    }
    IEnumerator _BallSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);    
            var _ball = Instantiate(_ballPrefab,Vector3.zero,Quaternion.identity,_spawnPoint);
            _ball.transform.localPosition = new Vector3(Random.Range(-_area,_area),0,0);
            var __ball =_ball.GetComponent<Ball>();
            __ball.BallValue = Random.Range(2,4);
            __ball.Initialize();
            Debug.Log(__ball.BallValue);            
        }        
    }
}
