using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {
    public class InputDataGetGGT26DofPosture : InputDataGetHandPosture {
        public InputDataGetGGT26Dof inputDataGetGGT26Dof;
        public InputDataGetGGT26DofPosture(InputDataGetGGT26Dof _inputDataGetGGT26Dof) : base(_inputDataGetGGT26Dof) {
            inputDataGetGGT26Dof = _inputDataGetGGT26Dof;
        }

        protected override void OnUpdateHandTransform() {

            if(API_GSXR_Slam.SlamManager != null && API_GSXR_Slam.SlamManager.gameObject.activeSelf) {
                inputDataGetGGT26Dof.inputDeviceGGT26DofPart.inputDataGGT26Dof.position = API_GSXR_Slam.SlamManager.head.transform.TransformPoint(inputDataGetGGT26Dof.inputDeviceGGT26DofPart.inputDataGGT26Dof.handInfo.localPosition + inputDataGetGGT26Dof.inputDeviceGGT26DofPart.inputDataGGT26Dof.handInfo.positionOffest);
                inputDataGetGGT26Dof.inputDeviceGGT26DofPart.inputDataGGT26Dof.rotation =Quaternion.Euler( API_GSXR_Slam.SlamManager.head.transform.eulerAngles + inputDataGetGGT26Dof.inputDeviceGGT26DofPart.inputDataGGT26Dof.handInfo.eulerAnglesOffset);
                //DebugMy.Log("handInfo:" + handInfo.position + "::" + API_GSXR_Slam.SlamManager.leftCamera.transform.position, this);
            }
        }



    }
}
