using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class FarPointer : PointerBase {
        public HandDetector handDetector {
            get {
                return detectorBase as HandDetector;
            }
        }

        public override PointerType PointerType { get => PointerType.Far; }


        public Action<bool> TargetDetectModelChange;


        protected override void UpdateTransform() {
            //transform.position = handDetector.inputDeviceHandPart.inputDeviceHandPartUI.modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.Four).position;
            //transform.rotation = API_GSXR_Slam.SlamManager.leftCamera.transform.rotation;

            transform.position = Vector3.Lerp(transform.position, handDetector.inputDeviceHandPart.inputDeviceHandPartUI.modelHand.ActiveHandModel.GetJointTransform(FINGER.forefinger, JOINT.Four).position, 0.5f);

            Quaternion temprotation = Quaternion.identity;
            if (handDetector.inputDeviceHandPart.PartType == InputDevicePartType.HandLeft) {
                temprotation = Quaternion.LookRotation(transform.position - API_GSXR_Slam.SlamManager.shoulder.Left.position, API_GSXR_Slam.SlamManager.shoulder.Left.up);
            } else if (handDetector.inputDeviceHandPart.PartType == InputDevicePartType.HandRight) {
                temprotation = Quaternion.LookRotation(transform.position - API_GSXR_Slam.SlamManager.shoulder.Right.position, API_GSXR_Slam.SlamManager.shoulder.Right.up);
            }
            transform.rotation = temprotation;
        }

        protected override void DoTargetDetect() {
                SCInputModule.Instance.ProcessCS(handDetector.inputDevicePartBase.inputDataBase.SCPointEventData, transform, LayerMask, MaxDetectDistance);
                IsFocusLocked = handDetector.inputDevicePartBase.inputDataBase.SCPointEventData.DownPressGameObject != null;
        }

        public override void OnSCDisable() {
            base.OnSCDisable();
            IsFocusLocked = false;
        }
    }
}
