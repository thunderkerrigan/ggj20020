using UnityEngine;

namespace ggjj2020
{
    public class PlayerInput : InputComponent, IDataPersister
    {
        public static PlayerInput Instance
        {
            get { return s_Instance; }
        }

        protected static PlayerInput s_Instance;
		
        public CharacterStatsSO _characterStats;
        private Coroutine watchFileCoroutine;
    
    
        public bool HaveControl { get { return m_HaveControl; } }

        public InputButton Pause = new InputButton(KeyCode.Escape, XboxControllerButtons.Menu);
        public InputButton Interact = new InputButton(KeyCode.E, XboxControllerButtons.Y);
        public InputButton MeleeAttack = new InputButton(KeyCode.K, XboxControllerButtons.X);
        public InputButton RangedAttack = new InputButton(KeyCode.O, XboxControllerButtons.B);
        public InputButton Jump = new InputButton(KeyCode.Space, XboxControllerButtons.A);
        public InputAxis Horizontal = new InputAxis(KeyCode.D, KeyCode.A, XboxControllerAxes.LeftstickHorizontal);
        public InputAxis Vertical = new InputAxis(KeyCode.W, KeyCode.S, XboxControllerAxes.LeftstickVertical);
        [HideInInspector]
        public DataSettings dataSettings;

        protected bool m_HaveControl = true;

        protected bool m_DebugMenuIsOpen = false;

        void Awake ()
        {
            
            if (s_Instance == null)
            {
                s_Instance = this;
				// CharacterStats
				if (this._characterStats != null) {
					this._characterStats.OnInputUpdate += UpdateControl;
					this._characterStats.ReadFromConfigFile();
					watchFileCoroutine = StartCoroutine(this._characterStats.WatchFile());
				} else { Debug.Log("No CharacterStats set in PlayerInput script"); }
            }
            else
                throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnEnable()
        {
            
            if (s_Instance == null)
            {
                s_Instance = this;
				// CharacterStats
				if (this._characterStats != null) {
					this._characterStats.OnInputUpdate += UpdateControl;
					this._characterStats.ReadFromConfigFile();
					//watchFileCoroutine = StartCoroutine(this._characterStats.WatchFile());
				} else { Debug.Log("No CharacterStats set in PlayerInput script"); }
            }
            else if(s_Instance != this)
                throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
        
            PersistentDataManager.RegisterPersister(this);
        }

        void OnDisable()
        {
            PersistentDataManager.UnregisterPersister(this);
				// CharacterStats
				if (this._characterStats != null) {
					StopCoroutine(watchFileCoroutine);
					this._characterStats.OnInputUpdate -= UpdateControl;
				} else { Debug.Log("No CharacterStats set in PlayerInput script"); }
            s_Instance = null;
        }
		
		// CharacterStats control to update
        private void UpdateControl(CharacterStatsSO characterStats)
        {
			Pause = new InputButton(characterStats.Pause, XboxControllerButtons.Menu);
			Interact = new InputButton(characterStats.Interact, XboxControllerButtons.Y);
			MeleeAttack = new InputButton(characterStats.MeleeAttack, XboxControllerButtons.X);
			RangedAttack = new InputButton(characterStats.RangedAttack, XboxControllerButtons.B);
			Jump = new InputButton(characterStats.Jump, XboxControllerButtons.A);
			Horizontal = new InputAxis(characterStats.HorizontalPositive, characterStats.HorizontalNegative, XboxControllerAxes.LeftstickHorizontal);
			Vertical = new InputAxis(characterStats.VerticalPositive, characterStats.VerticalNegative, XboxControllerAxes.LeftstickVertical);
        }

        protected override void GetInputs(bool fixedUpdateHappened)
        {
            Pause.Get(fixedUpdateHappened, inputType);
            Interact.Get(fixedUpdateHappened, inputType);
            MeleeAttack.Get(fixedUpdateHappened, inputType);
            RangedAttack.Get(fixedUpdateHappened, inputType);
            Jump.Get(fixedUpdateHappened, inputType);
            Horizontal.Get(inputType);
            Vertical.Get(inputType);

            if (Input.GetKeyDown(KeyCode.F12))
            {
                m_DebugMenuIsOpen = !m_DebugMenuIsOpen;
            }
        }

        public override void GainControl()
        {
            m_HaveControl = true;

            GainControl(Pause);
            GainControl(Interact);
            GainControl(MeleeAttack);
            GainControl(RangedAttack);
            GainControl(Jump);
            GainControl(Horizontal);
            GainControl(Vertical);
        }

        public override void ReleaseControl(bool resetValues = true)
        {
            m_HaveControl = false;

            ReleaseControl(Pause, resetValues);
            ReleaseControl(Interact, resetValues);
            ReleaseControl(MeleeAttack, resetValues);
            ReleaseControl(RangedAttack, resetValues);
            ReleaseControl(Jump, resetValues);
            ReleaseControl(Horizontal, resetValues);
            ReleaseControl(Vertical, resetValues);
        }

        public void DisableMeleeAttacking()
        {
            MeleeAttack.Disable();
        }

        public void EnableMeleeAttacking()
        {
            MeleeAttack.Enable();
        }

        public void DisableRangedAttacking()
        {
            RangedAttack.Disable();
        }

        public void EnableRangedAttacking()
        {
            RangedAttack.Enable();
        }

        public DataSettings GetDataSettings()
        {
            return dataSettings;
        }

        public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
        {
            dataSettings.dataTag = dataTag;
            dataSettings.persistenceType = persistenceType;
        }

        public Data SaveData()
        {
            return new Data<bool, bool>(MeleeAttack.Enabled, RangedAttack.Enabled);
        }

        public void LoadData(Data data)
        {
            Data<bool, bool> playerInputData = (Data<bool, bool>)data;

            if (playerInputData.value0)
                MeleeAttack.Enable();
            else
                MeleeAttack.Disable();

            if (playerInputData.value1)
                RangedAttack.Enable();
            else
                RangedAttack.Disable();
        }

        void OnGUI()
        {
            if (m_DebugMenuIsOpen)
            {
                const float height = 100;

                GUILayout.BeginArea(new Rect(30, Screen.height - height, 200, height));

                GUILayout.BeginVertical("box");
                GUILayout.Label("Press F12 to close");

                bool meleeAttackEnabled = GUILayout.Toggle(MeleeAttack.Enabled, "Enable Melee Attack");
                bool rangeAttackEnabled = GUILayout.Toggle(RangedAttack.Enabled, "Enable Range Attack");

                if (meleeAttackEnabled != MeleeAttack.Enabled)
                {
                    if (meleeAttackEnabled)
                        MeleeAttack.Enable();
                    else
                        MeleeAttack.Disable();
                }

                if (rangeAttackEnabled != RangedAttack.Enabled)
                {
                    if (rangeAttackEnabled)
                        RangedAttack.Enable();
                    else
                        RangedAttack.Disable();
                }
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }
    }
}
