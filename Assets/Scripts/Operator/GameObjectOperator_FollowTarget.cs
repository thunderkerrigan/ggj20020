/* *******************************************************************************
 *	Classe			: GameObjectOperator_FollowTarget
 *	
 *	------------------------------
 *	Desription		: used to force a GameObject to follow an target gameobject
 *
 *  ------------------------------
 *	Autheur			: Florian BOULET
 *	
 *  ------------------------------
 *  History :
 *      - date : V1
 *
 *  ------------------------------
 *  Notes :
 *			
 * *******************************************************************************/

using UnityEngine;

namespace ggjj2020 {

	[RequireComponent(typeof(Transform))]
	public class GameObjectOperator_FollowTarget : MonoBehaviour {
		//_____ PUBLIC PROPERTIES ___________________________________________________
		public GameObject   _target;
		public Vector3      _gap = Vector3.zero;
		public bool         _followOnX = true;
		public bool         _followOnY = false;
		public bool         _followOnZ = true;


		//_____ PRIVATE PROPERTIES __________________________________________________
		private Transform   _targetTransform;
		private Vector3     _targetLastPosition;


		[Range(0.01f,1f)]
		public float    _lerpSpeed = 0.2f;



		//_____ TRIGGER _____________________________________________________________
		/* -------------------------------------------------------------------------------------------------------------------------
		 * Initialisation
		 * ------------------------------------------------------------------------------------------------------------------------- */
		void Start () {
			// Check if the _target have a transform component
			if (this._target != null) {
				this._targetTransform = this._target.GetComponent<Transform>();
				if (this._targetTransform == null) {
					this._target = null;
					throw new System.Exception("The GameObject '" + this._target.name + "' need a transform to be assigned to a GameObjectOperator_FollowTarget");
				// Initialise the current position as the last one if the target have a Transform component
				} else { this._targetLastPosition = this._targetTransform.position; }
			}
		}

		/* -------------------------------------------------------------------------------------------------------------------------
		 * Update
		 * ------------------------------------------------------------------------------------------------------------------------- */
		void LateUpdate () {
			if (this._target != null) { this.moveToTarget(); }
		}




		//_____ METHODES _____________________________________________________________
		/* -------------------------------------------------------------------------------------------------------------------------
		 * Move the GameObject to the Target position
		 * ------------------------------------------------------------------------------------------------------------------------- */
		private void moveToTarget () {
			if (this._target != null && this._targetTransform != null && this._targetLastPosition != this._targetTransform.position) {

				// Set the new position of the actual GameObject
				Vector3 positionToTarget = new Vector3();
				if (this._followOnX) { positionToTarget.x = this._targetTransform.position.x + this._gap.x; } else { positionToTarget.x = this.transform.position.x; }
				if (this._followOnY) { positionToTarget.y = this._targetTransform.position.y + this._gap.y; } else { positionToTarget.y = this.transform.position.y; }
				if (this._followOnZ) { positionToTarget.z = this._targetTransform.position.z + this._gap.z; } else { positionToTarget.z = this.transform.position.z; }
				this.transform.position = Vector3.Lerp(this.transform.position, positionToTarget, this._lerpSpeed);

				// Set the current target position as the last one
				this._targetLastPosition = this._targetTransform.position;
			}
		}

	}
}
