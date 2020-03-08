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

        private float ScaleValue => 0.8f+ BallValue*GameConfig.sizeMultiplier;
        private Rigidbody _rigidBody;

        private TextMeshPro _text;

        private MeshRenderer _meshRenderer;

        void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _text=GetComponentInChildren<TextMeshPro>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        public void Initialize()
        {
             var scaleValue = ScaleValue;
             transform.localScale = Vector3.one * scaleValue;
            _rigidBody.mass = 1+scaleValue*5;
            _meshRenderer.material.color = Color.Lerp(Color.blue,Color.red,BallValue/10f);
            _text.text = BallText;

        }
        private string BallText  {
            get
            {
                var _val = Mathf.Pow(2,BallValue-1);
                var _str=_val.ToString();
                return _val<=4096?_str:_str.Substring(0,_str.Length-3)+"K";
            }
        }
        public Coroutine _sizeUpRoutine;
        public void SizeUp()
        {
            _sizeUpRoutine= StartCoroutine(_SizeUp());
        }
        private float updateSizeSpeed = 3;
        public IEnumerator _SizeUp()
        {
            BallValue++;
            var scaleValue = ScaleValue;
            _rigidBody.mass = 1+scaleValue*15;
            _meshRenderer.material.color = Color.Lerp(Color.blue,Color.red,BallValue/10f);
            _text.text = BallText;

            var alpha = 0f;
            var startScale = transform.localScale;
            var endScale = new Vector3(scaleValue,scaleValue,scaleValue);
            print($"startScale: {startScale} ;; endScale: {endScale}");
            while (alpha<=1)
            {
                var _val=Easings.BackEaseInOut(alpha);
               /// print("_val"+_val);
                transform.localScale = new Vector3(Mathf.Lerp(startScale.x,endScale.x,_value),Mathf.Lerp(startScale.y,endScale.y,_value), Mathf.Lerp(startScale.z,endScale.z,_value));
                ///print("x: "+transform.localScale.x);
                transform.GetChild(0).localEulerAngles = new Vector3(0,Mathf.Lerp(0,360,alpha),0);
                alpha+=Time.deltaTime*updateSizeSpeed;
                yield return null;
            }

            transform.localScale = new Vector3(Mathf.Lerp(startScale.x,endScale.x,1),Mathf.Lerp(startScale.y,endScale.y,1), Mathf.Lerp(startScale.z,endScale.z,1));
            print("Final x: "+transform.localScale.x);
            //transform.localScale = Vector3.Lerp(startScale,endScale,1);
            transform.GetChild(0).localEulerAngles = new Vector3(0,Mathf.Lerp(0,360,alpha),0);
            _sizeUpRoutine=null;
        }
        void OnCollisionEnter(Collision collision)
        {
            OnCollision(collision);
        }
        void OnCollision(Collision collision)
        {
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
        void OnCollisionStay(Collision collision)
        {
            OnCollision(collision);        
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
            GameManager.Instance.ballsList.Remove(gameObject);
            Destroy(gameObject);
        }
    }    
}
