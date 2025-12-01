using AOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class InputDeviceKS : InputDeviceGC {

        public override InputDeviceType inputDeviceType {
            get {
                return InputDeviceType.KS;
            }
        }

        bool isInvokeOnce = false;


        [Header("Enable GameController")]
        public bool LeftActive = true;
        public bool RightActive = true;
        protected override void InputDeviceStart() {
            SetActiveInputDevicePart(InputDevicePartType.KSLeft, LeftActive);
            SetActiveInputDevicePart(InputDevicePartType.KSRight, RightActive);
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();

            if(API_GSXR_Slam.SlamManager.plugin != null && API_GSXR_Slam.SlamManager.IsRunning == false)
                return;

            if(isInvokeOnce == false) {
                isInvokeOnce = true;
                if(Application.platform == RuntimePlatform.Android) {
                    try {
                        API_GSXR_Slam.SlamManager.plugin.GSXR_Set_ControllerKeyEventCallback(KeyEvent);
                        //API_GSXR_Slam.SlamManager.plugin.GSXR_Set_ControllerKeyTouchEventCallback(KeyTouchEvent);
                        API_GSXR_Slam.SlamManager.plugin.GSXR_Set_ControllerRockerCallback(JoystickEvent);
                        API_GSXR_Slam.SlamManager.plugin.GSXR_Set_ControllerHallCallback(HallEvent);
                        API_GSXR_Slam.SlamManager.plugin.GSXR_Set_ControllerChargingEventCallback(ChargingEvent);
                        API_GSXR_Slam.SlamManager.plugin.GSXR_Set_ControllerBatteryEventCallback(BatteryEvent);
                        API_GSXR_Slam.SlamManager.plugin.GSXR_Set_ControllerConnectEventCallback(ConnectEvent);
                    } catch(Exception e) {
                        Debug.Log(e);
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(Action<int,int, int>))]
        public static void KeyEvent(int keycode, int action, int lr) {
            Debug.Log("KS -- key event: " + keycode + " " + action + " " + lr);
            InputDataGC.GCData.GCKeyList.Add(new GCKeyData() { keycode = keycode, keyevent = action, deivceID = lr });
        }


        [MonoPInvokeCallback(typeof(Action<bool, bool,bool,bool,int>))]
        static void KeyTouchEvent(bool key1, bool key2, bool key3, bool key4, int lr) {
            Debug.Log("KS -- KeyTouchEvent:" + key1 + " " + key2 + " " + key3 + " " + key4 + " " + lr);
        }

        [MonoPInvokeCallback(typeof(Action<int,int, int>))]
        static void JoystickEvent(int touch_x, int touch_y, int lr) {
            Debug.Log("KS -- JoystickEvent:" + touch_x +" "+ touch_y + " " + lr);
            InputDataKS.TempJoystickDataList.Add(new InputDataKS.JoystickData() { JoystickX = touch_x, JoystickY = touch_y, deviceID = lr });
        }

        [MonoPInvokeCallback(typeof(Action<int,int, int>))]
        static void HallEvent(int hall_x, int hall_y, int lr) {
            Debug.Log("KS -- HallEvent:" + hall_x + " " + hall_y + " " + lr);
            InputDataKS.TempHallDataList.Add(new InputDataKS.HallData() { HallInside = hall_x, HallFoward = hall_y, deviceID = lr });
        }

        [MonoPInvokeCallback(typeof(Action<bool, int>))]
        static void ChargingEvent(bool isCharging, int lr) {
            Debug.Log("KS -- ChargingEvent:" + isCharging + " " + lr);
        }

        [MonoPInvokeCallback(typeof(Action<int, int>))]
        static void BatteryEvent(int battery, int lr) {
            Debug.Log("KS -- BatteryEvent:" + battery + " " + lr);
        }

        [MonoPInvokeCallback(typeof(Action<bool,int>))]
        static void ConnectEvent(bool isConnected, int lr) {
            Debug.Log("KS -- ConnectEvent:" + isConnected + " " + lr);
            InputDataKS.StatusDataList.Add(new InputDataKS.StatusData() { isConnected = isConnected, deviceID = lr });
        }

    }
}
