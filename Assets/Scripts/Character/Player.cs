using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ggjj2020 {
	[RequireComponent(typeof(CapsuleCollider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(Animator))]
	public class Player : MonoBehaviour
	{
		#region PROPERTIES

        public CharacterStatsSO _characterStats;
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
		[Range(-1f, 0f)] public float _initialFallVelocity;
		
        protected bool _InPause = false;
		protected CharacterController2D _CharacterController2D;
        protected Vector2 _MoveVector;
        protected TileBase _CurrentSurface;
		protected bool _isJumping;

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
			// CharacterStats
			if (this._characterStats != null) {
				this._characterStats.OnInputUpdate += UpdateControl;
			} else { Debug.Log("No CharacterStats set in PlayerInput script"); }
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
			bool isGrounded = this.CheckForGrounded();
			if (isGrounded) { this._isJumping = false; } // We are not jumping if we are grounded
			if (this.CheckForJumpInput() && isGrounded ) {
				this.SetVerticalMovement(this._jumpSpeed);
				this._isJumping = true;
			}
			UpdateJump();
			AirborneHorizontalMovement();
			AirborneVerticalMovement();
		}
		void FixedUpdate () {
            this._CharacterController2D.Move(this._MoveVector * Time.deltaTime);
//m_Animator.SetFloat(m_HashHorizontalSpeedPara, this._MoveVector.x);
//m_Animator.SetFloat(m_HashVerticalSpeedPara, this._MoveVector.y);
		}
		#endregion


		#region METHODS
		
		// CharacterStats control to update
        private void UpdateControl(CharacterStatsSO characterStats)
        {
			this._jumpSpeed = this._characterStats.jumpSpeed;
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
		
        public bool CheckForGrounded()
        {
            //bool wasGrounded = this._Animator.GetBool(this._HashGroundedPara);
            bool grounded = this._CharacterController2D.IsGrounded;

            if (grounded){
                FindCurrentSurface();

				//only play the landing sound if falling "fast" enough (avoid small bump playing the landing sound)
                /*if (!wasGrounded && this._MoveVector.y < -1.0f){
                    landingAudioPlayer.PlayRandomSound(m_CurrentSurface);
                }*/
			} else { this._CurrentSurface = null; }

            //this._Animator.SetBool(this._HashGroundedPara, grounded);

            return grounded;
        }
		
        public void FindCurrentSurface()
        {
            Collider2D groundCollider = this._CharacterController2D.GroundColliders[0];

			if (groundCollider == null) { groundCollider = this._CharacterController2D.GroundColliders[1]; }

			if (groundCollider == null) { return; }

            TileBase b = PhysicsHelper.FindTileForOverride(groundCollider, transform.position, Vector2.down);
            if (b != null){
                this._CurrentSurface = b;
            }
        }

		// ----------- Horizontal Mouvement -----------------
        public void SetHorizontalMovement(float newHorizontalMovement)
        {
            this._MoveVector.x = newHorizontalMovement;
        }

		// Move the player on Horizontal axis
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
		
		// Update Vertical speed on Grounded wich move on vertical axis
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
			bool isGrounded = this.CheckForGrounded();
            if (Mathf.Approximately(this._MoveVector.y, 0f) || (this._CharacterController2D.IsCeilinged && this._MoveVector.y > 0f) || (isGrounded && !this._isJumping)){
                this._MoveVector.y = 0f;
            }
			// Jumping
			if (this._isJumping) {
				this._MoveVector.y -= this._gravity * Time.deltaTime;
			// Falling
			}else if (!isGrounded && !this._isJumping) {
				if(this._MoveVector.y == 0f) { this._MoveVector.y = this._initialFallVelocity * 100; }
				this._MoveVector.y -= this._gravity * Time.deltaTime;
			}

			// Cap the falling speed to gravity.
			if (this._MoveVector.y < -this._gravity) { this._MoveVector.y = -this._gravity; }
        }
		#endregion
		
		// ----------- Falling Mouvement -----------------
        public bool IsFalling()
        {
            return this._MoveVector.y < 0f && !this._Animator.GetBool(this._HashGroundedPara);
        }
	}
}
