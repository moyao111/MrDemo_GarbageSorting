using SC.XR.Unity;
using SC.XR.Unity.Module_Device;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_SDKSystem {

    public class Module_SDKSystem : SCModuleMono {

        public static Module_SDKSystem Instance { get; private set; }

        [SerializeField]
        private bool EnableInputSystem = true;

        public bool DebugLog;

        bool isStart = false;

        public bool IsRunning {
            get; private set;
        }
        public bool Initialized {
            get; private set;
        }

        private Module_InputSystem.Module_InputSystem mInputSystem;
        public Module_InputSystem.Module_InputSystem InputSystem {
            get {
                if(EnableInputSystem && mInputSystem == null) {
                    mInputSystem = GetComponentInChildren<Module_InputSystem.Module_InputSystem>(true);
                }
                return mInputSystem;
            }
        }

        //private GSXRManager mSvrManager;
        //public GSXRManager GSXRManager {
        //    get {
        //        if (mSvrManager == null) {
        //            mSvrManager = GetComponentInChildren<GSXRManager>(true);
        //        }
        //        return mSvrManager;
        //    }
        //}

        Coroutine waitSlam = null;
        Coroutine isRunningCoroutine = null;

        private Coroutine updateWaitForEndOfFrame = null;

        #region MonoBehavior Driver

        void Awake() {
            ModuleInit(false);
        }

        void OnEnable() {
            if(updateWaitForEndOfFrame == null) {
                updateWaitForEndOfFrame = StartCoroutine(UpdateWaitForEndOfFrame());
            }
            if(isStart == true) {
                ModuleStart();
            }
        }

        void Start() {
            isStart = true;
            ModuleStart();
        }

        void Update() {
            ModuleUpdate();
        }

        void LateUpdate() {
            ModuleLateUpdate();
        }

        void OnApplicationPause(bool pause) {
            if(isStart) {
                if(pause) {
                    ModuleStop();
                } else {
                    ModuleStart();
                }
            }
        }

        IEnumerator UpdateWaitForEndOfFrame() {
            while(true) {
                yield return new WaitForEndOfFrame();
                if(InputSystem && InputSystem.IsModuleStarted) {
                    InputSystem.ModuleEndOfFrame();
                }
            }
        }


        void OnDisable() {

            if(updateWaitForEndOfFrame != null) {
                StopCoroutine(updateWaitForEndOfFrame);
            }

            ModuleStop();
        }

        void OnDestroy() {
            ModuleDestroy();
            isStart = false;
        }

        #endregion


        #region Module Behavoir

        public override void OnSCAwake() {
            base.OnSCAwake();

            if(Instance != null) {
                DestroyImmediate(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;


            DebugMy.Log("Awake", this, true);
            DebugMy.Log("SDK Version:"+API_Module_SDKVersion.Version,this,true);
            DebugMy.Log("BatteryLevel: " + Module_BatteryStatus.getInstance.BatteryLevel + "% IsCharging: " + Module_BatteryStatus.getInstance.IsCharging, this, true);

            Module_Device.Module_Device.getInstance.Current.ShowInfo();

            if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "ShowDebugLog")) {
                DebugMy.isShowNormalLog = API_Module_SDKConfiguration.GetBool("Module_InputSystem", "ShowDebugLog", 0);
            } else {
                DebugMy.isShowNormalLog = DebugLog;
            }

            API_GSXR_Slam.SlamManager?.gameObject.SetActive(false);

            AddModule(InputSystem);

        }



        public override void OnSCStart() {
            base.OnSCStart();

            API_GSXR_Slam.SlamManager?.gameObject.SetActive(true);

            if (waitSlam == null) {
                waitSlam = StartCoroutine(WaitSlamAction());
            }

            if (isRunningCoroutine == null) {
                isRunningCoroutine = StartCoroutine(Running());
            }
        }

        IEnumerator WaitSlamAction() {
            yield return new WaitUntil(() => API_GSXR_Slam.SlamManager.IsRunning);
            InputSystem?.ModuleStart();
        }

        public override void OnSCDisable() {
            base.OnSCDisable();

            if (waitSlam != null) {
                StopCoroutine(waitSlam);
                waitSlam = null;
            }
            if (isRunningCoroutine != null) {
                StopCoroutine(isRunningCoroutine);
                isRunningCoroutine = null;
            }

            IsRunning = false;

            //不能操作 灭屏唤醒否则起不来
            //API_GSXR_Slam.SlamManager?.gameObject.SetActive(false);
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            if (Instance != this)
                return;

            if (waitSlam != null) {
                StopCoroutine(waitSlam);
                waitSlam = null;
            }
            if (isRunningCoroutine != null) {
                StopCoroutine(isRunningCoroutine);
                isRunningCoroutine = null;
            }

            API_GSXR_Slam.SlamManager?.gameObject.SetActive(false);
        }

        #endregion


        IEnumerator Running() {

            ///SLam model
            yield return new WaitUntil(() =>  API_GSXR_Slam.SlamManager.IsRunning);

            if (InputSystem) {
                yield return new WaitUntil(() =>InputSystem.IsRunning);
            }

            IsRunning = true;
            isRunningCoroutine = null;
            DebugMy.Log("SDKSystem Module IsRuning !", this,true);
        }


    }
}

