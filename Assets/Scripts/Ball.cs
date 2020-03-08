using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace BallMerge
{
    public class Ball : MonoBehaviour
    {
        private int _value = 2 ;
        

        public int BallValue{
            get {return _value;}
            set {
                _value=value;
            }
        }

        private float ScaleValue => BallValue*GameConfig.sizeMultiplier;
        private Rigidbody _rigidBody;

        private TextMeshPro _text;

        private MeshRenderer _meshRenderer;

        void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _text=GetComponentInChildren<TextMeshPro>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        void Update()
        {
            
        }

        public void Initialize()
        {
             var scaleValue = ScaleValue;
            _rigidBody.mass = 1+scaleValue*5;
            _meshRenderer.material.color = Color.Lerp(Color.blue,Color.red,BallValue/10f);
            _text.text = Mathf.Pow(2,BallValue-1).ToString();

        }

        public void SizeUp()
        {
            StartCoroutine(_SizeUp());
        }
        private float updateSizeSpeed = 3;
        public IEnumerator _SizeUp()
        {
            BallValue++;
            var scaleValue = ScaleValue;
            _rigidBody.mass = 1+scaleValue*5;
            _meshRenderer.material.color = Color.Lerp(Color.blue,Color.red,BallValue/10f);
            _text.text = Mathf.Pow(2,BallValue-1).ToString();

            var alpha = 0f;
            var startScale = transform.localScale;
            var endScale = new Vector3(scaleValue,scaleValue,scaleValue);
            while (alpha<=1)
            {
                transform.localScale = Vector3.Lerp(startScale,endScale, Easings.BounceEaseInOut(alpha));
                transform.localEulerAngles = new Vector3(0,Mathf.Lerp(0,360,alpha),0);
                alpha+=Time.deltaTime*updateSizeSpeed;
                yield return null;
            }
            transform.localScale = Vector3.Lerp(startScale,endScale,1);
        }
        void OnCollisionEnter(Collision collision)
        {
            print("OnCollisionEnter");

            var _collidedBall= collision.collider.GetComponent<Ball>();
            if(_collidedBall!=null && BallValue == _collidedBall.BallValue)
            {
                collision.collider.enabled=false;
                collision.rigidbody.isKinematic=true;
                collision.collider.GetComponentInChildren<TextMeshPro>().enabled=false;
                _collidedBall.MoveToBall(transform);
                SizeUp();
            }
            // if (collision.relativeVelocity.magnitude > 2)
            //     audioSource.Play();
        }

        public void MoveToBall(Transform _otherBall)
        {
            StartCoroutine(_MoveToBall(_otherBall));
        }
        public IEnumerator _MoveToBall(Transform _otherBall)
        {
            float _attractionSpeed = 20;
            
            do
            {
                if(_otherBall ==null) break;
                transform.position = Vector3.Lerp(transform.position,_otherBall.position,Time.deltaTime*_attractionSpeed);            
                yield return null;
            }while (_otherBall!=null &&(transform.position-_otherBall.position).sqrMagnitude>0.01f);
            
            Destroy(gameObject);
        }
    }    
}
