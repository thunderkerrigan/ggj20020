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
		#region PROPERTIES
		// Horizontal mouvement parameters
		[Header("Horizontal mouvement")]
        public float _maxSpeed = 10f;
        public float _groundAcceleration = 100f;
        public float _groundDeceleration = 100f;

		// Vertical mouvement parameters
		[Header("Vertical mouvement")]
        [Range(0f, 1f)] public float _airborneAccelProportion;
        [Range(0f, 1f)] public float _airborneDecelProportion;
        public float _gravity = 50f;
        public float _jumpSpeed = 20f;
        public float _jumpAbortSpeedReduction = 100f;
		
        protected bool _InPause = false;
		protected CharacterController2D _CharacterController2D;
        protected Vector2 _MoveVector;

		// Animation
        protected Animator _Animator;
        protected readonly int _HashHorizontalSpeedPara = Animator.StringToHash("HorizontalSpeed");
        protected readonly int _HashVerticalSpeedPara = Animator.StringToHash("VerticalSpeed");
        protected readonly int _HashGroundedPara = Animator.StringToHash("Grounded");
		
        protected const float k_GroundedStickingVelocityMultiplier = 3f;    // This is to help the character stick to vertically moving platforms.
		#endregion


		#region TRIGGERS
		// Start is called before the first frame update
		void Awake (){			
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

			// Manage Jump
			UpdateJump();
		}
		void FixedUpdate () {
            this._CharacterController2D.Move(this._MoveVector * Time.deltaTime);
//m_Animator.SetFloat(m_HashHorizontalSpeedPara, this._MoveVector.x);
//m_Animator.SetFloat(m_HashVerticalSpeedPara, this._MoveVector.y);
		}
		#endregion


		#region METHODS
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
		// ----------- Mouvement -----------------
        public void SetMoveVector(Vector2 newMoveVector)
        {
            this._MoveVector = newMoveVector;
        }
        public void IncrementMovement(Vector2 additionalMovement)
        {
            this._MoveVector += additionalMovement;
        }

        public Vector2 GetMoveVector()
        {
            return this._MoveVector;
        }
		
		// ----------- Horizontal Mouvement -----------------
        public void SetHorizontalMovement(float newHorizontalMovement)
        {
            this._MoveVector.x = newHorizontalMovement;
        }

        public void GroundedHorizontalMovement(bool useInput, float speedScale = 1f)
        {
            float desiredSpeed = useInput ? PlayerInput.Instance.Horizontal.Value * this._maxSpeed * speedScale : 0f;
            float acceleration = useInput && PlayerInput.Instance.Horizontal.ReceivingInput ? this._groundAcceleration : this._groundDeceleration;
            this._MoveVector.x = Mathf.MoveTowards(this._MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
        }
		
        public void IncrementHorizontalMovement(float additionalHorizontalMovement)
        {
            this._MoveVector.x += additionalHorizontalMovement;
        }

		
		// ----------- Jump Mouvement -----------------
        public bool CheckForJumpInput()
        {
            return PlayerInput.Instance.Jump.Down;
        }

        public void SetVerticalMovement(float newVerticalMovement)
        {
            this._MoveVector.y = newVerticalMovement;
        }
		
        public void GroundedVerticalMovement()
        {
            this._MoveVector.y -= this._gravity * Time.deltaTime;

            if (this._MoveVector.y < -this._gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier){
                this._MoveVector.y = -this._gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier;
            }
        }

        public void IncrementVerticalMovement(float additionalVerticalMovement)
        {
            this._MoveVector.y += additionalVerticalMovement;
        }
		
        public bool IsFalling()
        {
            return this._MoveVector.y < 0f && !this._Animator.GetBool(this._HashGroundedPara);
        }

        public void UpdateJump()
        {
            if (!PlayerInput.Instance.Jump.Held && this._MoveVector.y > 0.0f){
                this._MoveVector.y -= this._jumpAbortSpeedReduction * Time.deltaTime;
            }
        }
		
        public void AirborneHorizontalMovement()
        {
            float desiredSpeed = PlayerInput.Instance.Horizontal.Value * this._maxSpeed;

            float acceleration;

			if (PlayerInput.Instance.Horizontal.ReceivingInput) {
				acceleration = this._groundAcceleration * this._airborneAccelProportion;
			} else {
				acceleration = this._groundDeceleration * this._airborneDecelProportion;
			}

            this._MoveVector.x = Mathf.MoveTowards(this._MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
        }

        public void AirborneVerticalMovement()
        {
            if (Mathf.Approximately(this._MoveVector.y, 0f) || this._CharacterController2D.IsCeilinged && this._MoveVector.y > 0f){
                this._MoveVector.y = 0f;
            }
            this._MoveVector.y -= this._gravity * Time.deltaTime;
        }
		#endregion

	}
}
