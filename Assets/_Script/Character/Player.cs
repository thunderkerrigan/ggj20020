using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ggjj2020 {
	[RequireComponent(typeof(CapsuleCollider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(Animator))]
	public class Player : MonoBehaviour
	{
		
        protected bool _InPause = false;
		protected CharacterController2D _CharacterController2D;
        protected Vector2 _MoveVector;
        public float maxSpeed = 10f;
        public float groundAcceleration = 100f;
        public float groundDeceleration = 100f;

		// Start is called before the first frame update
		void Awake(){			
            this._CharacterController2D = GetComponent<CharacterController2D>();
		}

		// Update is called once per frame
		void Update(){
			// Get Pause input
			if (PlayerInput.Instance.Pause.Down){
                if (!this._InPause){
//if (ScreenFader.IsFading) { return; }                        

                    PlayerInput.Instance.ReleaseControl(false);
                    PlayerInput.Instance.Pause.GainControl();
                    this._InPause = true;
                    Time.timeScale = 0;
//UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("UIMenus", UnityEngine.SceneManagement.LoadSceneMode.Additive);
                } else { Unpause(); }
            }

			// Get Horizontal movement input
			this.GroundedHorizontalMovement(PlayerInput.Instance.Horizontal.ReceivingInput);
		}
		void FixedUpdate () {
            this._CharacterController2D.Move(this._MoveVector * Time.deltaTime);
//m_Animator.SetFloat(m_HashHorizontalSpeedPara, this._MoveVector.x);
//m_Animator.SetFloat(m_HashVerticalSpeedPara, this._MoveVector.y);
		}

		public void Unpause(){
				//if the timescale is already > 0, we 
				if (Time.timeScale > 0){return;}

				StartCoroutine(UnpauseCoroutine());
		}

		protected IEnumerator UnpauseCoroutine(){
			Time.timeScale = 1;
//UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("UIMenus");
			PlayerInput.Instance.GainControl();

			//we have to wait for a fixed update so the pause button state change, otherwise we can get in case were the update
			//of this script happen BEFORE the input is updated, leading to setting the game in pause once again
			yield return new WaitForFixedUpdate();
			yield return new WaitForEndOfFrame();
			this._InPause = false;
		}

		// Public functions - called mostly by StateMachineBehaviours in the character's Animator Controller but also by Events.
        public void SetMoveVector(Vector2 newMoveVector)
        {
            this._MoveVector = newMoveVector;
        }

        public void SetHorizontalMovement(float newHorizontalMovement)
        {
            this._MoveVector.x = newHorizontalMovement;
        }

        public void SetVerticalMovement(float newVerticalMovement)
        {
            this._MoveVector.y = newVerticalMovement;
        }

        public void IncrementMovement(Vector2 additionalMovement)
        {
            this._MoveVector += additionalMovement;
        }

        public void IncrementHorizontalMovement(float additionalHorizontalMovement)
        {
            this._MoveVector.x += additionalHorizontalMovement;
        }

        public void IncrementVerticalMovement(float additionalVerticalMovement)
        {
            this._MoveVector.y += additionalVerticalMovement;
        }
		
        public Vector2 GetMoveVector()
        {
            return this._MoveVector;
        }
		
        public void GroundedHorizontalMovement(bool useInput, float speedScale = 1f)
        {
            float desiredSpeed = useInput ? PlayerInput.Instance.Horizontal.Value * maxSpeed * speedScale : 0f;
            float acceleration = useInput && PlayerInput.Instance.Horizontal.ReceivingInput ? groundAcceleration : groundDeceleration;
            this._MoveVector.x = Mathf.MoveTowards(this._MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
        }
	}
}
