using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using SC.XR.Unity;
using SC.XR.Unity.Module_Device;

public abstract class GSXRPlugin
{
	private static GSXRPlugin instance;

	public static GSXRPlugin Instance
	{
		get
		{
			if (instance == null)
			{
				if(!Application.isEditor && Application.platform == RuntimePlatform.Android)
				{
                    if (API_Module_Device.CurrentAndroid != null && API_Module_Device.CurrentAndroid.type == AndroidDeviceType.Other) {
                        instance = GSXRPluginAndroidOther.Create();
                    } else {
                        if (GSXRManager.Instance != null) {
                            if (GSXRManager.Instance.specificationType == GSXRManager.SpecificationType.GSXR) {
                                instance = GSXRPluginAndroid.Create();
                            } else {
                                instance = SvrPluginAndroid.Create();
                            }
                        }
                    }
				}
				else
				{
					instance = GSXRPluginWin.Create();
				}
			}
			return instance;
		}
	}

    public GSXRManager svrCamera = null;
    public GSXREye[] eyes = null;
    public GSXROverlay[] overlays = null;
    public DeviceInfo deviceInfo;

    public enum PerfLevel
	{
        kPerfSystem = 0,
        kPerfMaximum = 1,
		kPerfNormal = 2,
		kPerfMinimum = 3
	}

    public enum TrackingMode
    {
        kTrackingOrientation = (1 << 0),
        kTrackingPosition = (1 << 1),
        kTrackingEye = (1 << 2),
    }

    public enum EyePoseStatus
    {
        kGazePointValid = (1 << 0),
        kGazeVectorValid = (1 << 1),
        kEyeOpennessValid = (1 << 2),
        kEyePupilDilationValid = (1 << 3),
        kEyePositionGuideValid = (1 << 4)
    };

    public enum FrameOption
    {
        kDisableDistortionCorrection = (1 << 0),    //!< Disables the lens distortion correction (useful for debugging)
        kDisableReprojection = (1 << 1),            //!< Disables re-projection
        kEnableMotionToPhoton = (1 << 2),           //!< Enables motion to photon testing 
        kDisableChromaticCorrection = (1 << 3)      //!< Disables the lens chromatic aberration correction (performance optimization)
    };

    public struct HeadPose
    {
        public Vector3 position;
        public Quaternion orientation;
    }

    public struct EyePose
    {
        public int leftStatus;          //!< Bit field (svrEyePoseStatus) indicating left eye pose status
        public int rightStatus;         //!< Bit field (svrEyePoseStatus) indicating right eye pose status
        public int combinedStatus;      //!< Bit field (svrEyePoseStatus) indicating combined eye pose status

        public Vector3 leftPosition;        //!< Left Eye Gaze Point
        public Vector3 rightPosition;       //!< Right Eye Gaze Point
        public Vector3 combinedPosition;    //!< Combined Eye Gaze Point (HMD center-eye point)

        public Vector3 leftDirection;       //!< Left Eye Gaze Point
        public Vector3 rightDirection;      //!< Right Eye Gaze Point
        public Vector3 combinedDirection;   //!< Comnbined Eye Gaze Vector (HMD center-eye point)

        public float leftOpenness;          //!< Left eye value between 0.0 and 1.0 where 1.0 means fully open and 0.0 closed.
        public float rightOpenness;         //!< Right eye value between 0.0 and 1.0 where 1.0 means fully open and 0.0 closed.

        //public float leftDilation;       //!< Left eye value in millimeters indicating the pupil dilation
        //public float rightDilation;      //!< Right eye value in millimeters indicating the pupil dilation

        //public Vector3 leftGuide;    //!< Position of the inner corner of the left eye in meters from the HMD center-eye coordinate system's origin.
        //public Vector3 rightGuide;   //!< Position of the inner corner of the right eye in meters from the HMD center-eye coordinate system's origin.
    }

    public struct ViewFrustum
    {
        public float left;           //!< Left Plane of Frustum
        public float right;          //!< Right Plane of Frustum
        public float top;            //!< Top Plane of Frustum
        public float bottom;         //!< Bottom Plane of Frustum

        public float near;           //!< Near Plane of Frustum
        public float far;            //!< Far Plane of Frustum (Arbitrary)
    }

    public struct DeviceInfo
	{
		public int 		displayWidthPixels;
		public int    	displayHeightPixels;
		public float  	displayRefreshRateHz;
		public int    	targetEyeWidthPixels;
		public int    	targetEyeHeightPixels;
		public float  	targetFovXRad;
		public float  	targetFovYRad;
        public ViewFrustum targetFrustumLeft;
        public ViewFrustum targetFrustumRight;
        public float    targetFrustumConvergence;
        public float    targetFrustumPitch;
	}

	public virtual int  GetPredictedPoseModify(ref Quaternion orientation, ref Vector3 position, int frameIndex = -1)
	{
		return 0;
	}
    public virtual bool PollEvent(ref GSXRManager.SvrEvent frameEvent) { return false; }

    public virtual bool IsInitialized() { return false; }
    public virtual bool IsRunning() { return false; }
    public virtual IEnumerator Initialize ()
    {
        svrCamera = GSXRManager.Instance;
        if (svrCamera == null)
        {
            Debug.LogError("GSXRManager object not found!");
            yield break;
        }

        yield break;
    }
	public virtual IEnumerator BeginVr(int cpuPerfLevel =0, int gpuPerfLevel =0)
    {
        if (eyes == null)
        {
            eyes = GSXREye.Instances.ToArray();
            if (eyes == null)
            {
                Debug.Log("Components with GSXREye not found!");
            }

            Array.Sort(eyes);
        }

        if (overlays == null)
        {
            overlays = GSXROverlay.Instances.ToArray();
            if (overlays == null)
            {
                Debug.Log("Components with GSXROverlay not found!");
            }

            Array.Sort(overlays);
        }

        yield break;
    }
    public virtual void EndVr()
    {
        eyes = null;
        overlays = null;
    }
	public virtual void BeginEye(int sideMask, float[] frameDelta) { }
    public virtual void EndEye(int sideMask, int layerMask) { }
    public virtual void SetTrackingMode(int mode) { }
	public virtual void SetFoveationParameters(int textureId, int previousId, float focalPointX, float focalPointY, float foveationGainX, float foveationGainY, float foveationArea, float foveationMinimum) {}
    public virtual void ApplyFoveation() { }
    public virtual int  GetTrackingMode() { return 0; }
    public virtual void SetPerformanceLevels(int newCpuPerfLevel, int newGpuPerfLevel) { }
    public virtual void SetFrameOption(FrameOption frameOption) { }
    public virtual void UnsetFrameOption(FrameOption frameOption) { }
    public virtual void SetVSyncCount(int vSyncCount) { }
    public virtual bool RecenterTracking() { return true; }
    public virtual void SubmitFrame(int frameIndex, float fieldOfView, int frameType) { }
    public virtual int GetPredictedPose(ref Quaternion orientation, ref Vector3 position, int frameIndex = -1)
    {
        orientation = Quaternion.identity;
        position = Vector3.zero;
        return 0;
    }
    public virtual int GetHeadPose(ref HeadPose headPose, int frameIndex = -1)
    {
        headPose.orientation = Quaternion.identity;
        headPose.position = Vector3.zero;
        return 0;
    }
    public virtual int GetEyePose(ref EyePose eyePose, int frameIndex = -1)
    {
        eyePose.leftStatus = 0;
        eyePose.rightStatus = 0;
        eyePose.combinedStatus = 0;
        return 0;
    }
    public abstract DeviceInfo GetDeviceInfo ();

	public virtual void Shutdown()
    {
        GSXRPlugin.instance = null;
    }


	//---------------------------------------------------------------------------------------------
	public virtual int ControllerStartTracking(string desc) {
		return -1;
	}

	//---------------------------------------------------------------------------------------------
	public virtual void ControllerStopTracking(int handle) {
	}

	//---------------------------------------------------------------------------------------------
	public virtual GSXRControllerState ControllerGetState(int handle, int space) {
		return new GSXRControllerState();
	}

	//---------------------------------------------------------------------------------------------
	public virtual void ControllerSendMessage(int handle, GSXRController.svrControllerMessageType what, int arg1, int arg2) {
	}

	//---------------------------------------------------------------------------------------------
	public virtual object ControllerQuery(int handle, GSXRController.svrControllerQueryType what) {
		return null;
	}



    //-----------------------KS---------------------

    //-----------------------For A11G---------------------

    //-----------------------For HeadsetInput---------------------

    //-----------------------For GreyCamera---------------------



    #region HMD
    public virtual bool HeadSetEnterKeyDown() { return Input.GetMouseButtonDown(0); }
    public virtual bool HeadSetEnterKeyUp() { return Input.GetMouseButtonUp(0); }
    public virtual bool HeadSetBackKeyDown() { return Input.GetKeyDown(KeyCode.Escape); }
    public virtual bool HeadSetBackKeyUp() { return Input.GetKeyUp(KeyCode.Escape); }
    #endregion HMD

    #region Controller

    public delegate void OnKeyEvent(int keycode, int action, int lr);
    public delegate void OnKeyTouchEvent(bool key1, bool key2, bool key3, bool key4, int lr);
    public delegate void OnTouchEvent(int touch_x, int touch_y, int lr);
    public delegate void OnHallEvent(int hall_x, int hall_y, int lr);
    public delegate void OnChargingEvent(bool isCharging, int lr);
    public delegate void OnBatteryEvent(int battery, int lr);
    public delegate void OnConnectEvent(bool isConnected, int lr);

    public virtual bool GSXR_Is_SupportController() { return false; }
    public virtual int GSXR_Get_ControllerMode() { return 3; }
    public virtual int GSXR_Get_ControllerNum() { return 2; }
    public virtual void GSXR_Set_ControllerKeyEventCallback(OnKeyEvent _event) { }
    public virtual void GSXR_Set_ControllerKeyTouchEventCallback(OnKeyTouchEvent _event) { }
    public virtual void GSXR_Set_ControllerRockerCallback(OnTouchEvent _event) { }
    public virtual void GSXR_Set_ControllerHallCallback(OnHallEvent _event) { }
    public virtual void GSXR_Set_ControllerChargingEventCallback(OnChargingEvent _event) { }
    public virtual void GSXR_Set_ControllerBatteryEventCallback(OnBatteryEvent _event) { }
    public virtual void GSXR_Set_ControllerConnectEventCallback(OnConnectEvent _event) { }
    public virtual int GSXR_Get_ControllerBattery(int lr) { return -1; }
    public virtual int GSXR_Get_ControllerVersion(int lr) { return -1; }
    public virtual int GSXR_Get_ControllerList(int lr) { return -1; }
    public virtual bool GSXR_Is_ControllerConnect(int lr) { return false; }
    public virtual void GSXR_Set_ControllerLed(int lr) { return; }
    public virtual void GSXR_Set_ControllerVibrate(int lr) { return; }
    public virtual int GSXR_Get_ControllerPosture(float[] outOrientationArray, int lr) { return -1; }

    [Obsolete("Only For SVR")]
    public virtual int Fetch3dofHandShank(float[] outOrientationArray, int lr) { return -1; }

    [Obsolete("Only For SVR")]
    public virtual int Fetch6dofHandShank(float[] outOrientationArray, int lr) { return -1; }

    #endregion Controller

    #region HandTracking

    public virtual bool GSXR_Is_SupportHandTracking() { return false; }
    public virtual void GSXR_StartHandTracking(Action<int> func) { }
    public virtual void GSXR_StopHandTracking() { }
    public virtual void GSXR_Get_HandTrackingData(float[] mode, float[] pose) { return; }
    public virtual int GSXR_Set_HandTrackingData(float[] mode, float[] pose) { return 0; }
    public virtual int GSXR_Get_HandTrackingGestureIdx(ref UInt64 index, float[] model, float[] pose) { return 0; }
    public virtual void GSXR_Set_HandTrackingCallBack(Action<int> callback) { }
    public virtual int GSXR_Set_HandTrackingModelDataCallBack(Action callback) { return 0; }
    public virtual void GSXR_Set_HandTrackingLowPowerWarningCallback(Action<int> func) { }

    #endregion HandTracking

    #region  Deflection
    public virtual float GSXR_Get_Deflection() { return 0; }
    public virtual void GSXR_Set_RelativeDeflection(int deflection) { }
    #endregion  Deflection

    #region  PointCloud & Map
    public virtual int GSXR_Get_PointCloudData(ref int dataNum, ref ulong dataTimestamp, float[] dataArray) { return 0; }
    public virtual int GSXR_Get_OfflineMapRelocState() { return 0; }
    public virtual int GSXR_ResaveMap(string path) { return 0; }
    public virtual void GSXR_SaveMap() { }
    public virtual int GSXR_Get_Gnss(ref double dt, float[] gnss) { return 0; }
    public virtual int GSXR_Get_PanelNum() { return 0; }
    public virtual int GSXR_Get_PanelInfo(float[] info) { return 0; }

    #endregion  PointCloud & Map

    #region Grey Camera Data
    public virtual void GSXR_Get_LatestQVRCameraFrameData(ref bool outBUdate, ref uint outCurrFrameIndex, ref ulong outFrameExposureNano,
            byte[] outFrameData, float[] outTRDataArray) { outBUdate = false; }
    public virtual void GSXR_Get_LatestQVRCameraFrameDataNoTransform(ref bool outBUdate, ref uint outCurrFrameIndex, ref ulong outFrameExposureNano,
            byte[] outFrameData, float[] outTRDataArray) { outBUdate = false; }
    public virtual int GSXR_Get_LatestQVRCameraBinocularData(ref bool outBUdate, ref uint outCurrFrameIndex, ref ulong outFrameExposureNano, byte[] outLeftFrameData, byte[] outRightFrameData) { outBUdate = false; return 0; }

    #endregion Grey Camera Data

    #region Optics Calibration
    public Matrix4x4 leftViewMatrix, rightViewMatrix;
    public virtual bool GSXR_Is_SupportOpticsCalibration() { return false; }

    public virtual int GSXR_Get_TransformMatrix(ref bool outBLoaded, float[] outTransformArray) { outBLoaded = false; return 0; }
    public virtual int GSXR_Get_LatestEyeMatrices(float[] outLeftEyeMatrix,
                                           float[] outRightEyeMatrix,
                                           float[] outT,
                                           float[] outR,
                                           int frameIndex,
                                           bool isMultiThreadedRender) { return 0; }

    #endregion Optics Calibration

    #region USBDisconnect
    public virtual void SetGlassDisconnectedCallBack(Action callBack) { }
    #endregion USBDisconnect

    #region luncher
    public virtual int initLayer() { return -1; }
    public virtual int startLayerRendering() { return -1; }
    public virtual int getAllLayersData(ref GSXRManager.SCAllLayers outAllLayers) { return -1; }
    public virtual int endLayerRendering(ref GSXRManager.SCAllLayers allLayers) { return -1; }
    public virtual int destroyLayer() { return -1; }
    public virtual int updateModelMatrix(UInt32 layerId, float[] modelMatrixArray) { return -1; }
    public virtual int sendActionBarCMD(UInt32 layerId, int cmd) { return -1; }
    public virtual int injectMotionEvent(UInt32 layerId, int displayID, int action, float x, float y) { return -1; }

    #endregion luncher

    #region Device
    public virtual XRType GSXR_Get_XRType() { return API_Module_Device.Current.XRType; }

    public virtual void GSXR_Get_DeviceName(ref string deviceName) { deviceName = API_Module_Device.Current.modelName; }
    #endregion
}

