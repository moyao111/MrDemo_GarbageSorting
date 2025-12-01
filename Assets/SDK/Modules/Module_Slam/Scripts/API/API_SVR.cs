using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Obsolete("Please Use API_GSXR_Slam")]
public class API_SVR {

    public enum TrackMode {
        Mode_3Dof,
        Mode_6Dof,
    }

    ///API-No.1
    /// <summary>
    /// 设置眼镜进入模式,运行过程中可修改
    /// </summary>
    public static void SetTrackMode(TrackMode mode) {
        API_GSXR_Slam.GSXR_Set_TrackMode((API_GSXR_Slam.TrackMode)mode);
    }

    ///API-No.2
    /// <summary>
    /// Svr系统是否在运行
    /// </summary>
    /// <returns>true表示在运行，false表示未运行（Pause时为false）</returns>
    public static bool IsSvrRunning() {
        return API_GSXR_Slam.GSXR_Is_SlamRunning();
    }

    ///API-No.3
    /// <summary>
    /// Svr系统是否初始化完成
    /// </summary>
    /// <returns></returns>
    public static bool IsSvrInitialized() {
        return API_GSXR_Slam.GSXR_Is_SlamInitialized();
    }

    ///API-No.4
    /// <summary>
    /// 设置Svr初始化完成时的回调
    /// </summary>
    /// <param name="action"></param>
    public static void AddInitializedCallBack(Action action) {
        API_GSXR_Slam.GSXR_Add_InitializedCallBack(action);
    }

    ///API-No.5
    public static void RemoveInitializedCallBack(Action action) {
        API_GSXR_Slam.GSXR_Remove_InitializedCallBack(action);
    }

    ///API-No.6
    /// <summary>
    /// 设置渲染帧率,只能在Start中调用
    /// </summary>
    /// <param name="frameRate">默认-1表示系统默认帧率,设置范围0-200</param>
    public static void SetRenderFrame(int frameRate = -1) {
        API_GSXR_Slam.GSXR_Set_RenderFrame(frameRate);
    }

    ///API-No.7
    /// <summary>
    /// 获取左右眼摄像头
    /// </summary>
    /// <returns>List[0]左眼 List[1]右眼，空表示系统未启动完成</returns>
    public static List<Camera> GetEyeCameras() {
        return API_GSXR_Slam.GSXR_Get_EyeCameras();
    }

    ///API-No.8
    /// <summary>
    /// 获取左右眼渲染的画面，为获取当前帧的渲染结果，当前帧结束时调用
    /// </summary>
    /// <returns>List[0]左眼 List[1]右眼，空表示系统未启动完成</returns>
    public static List<RenderTexture> GetRenderTexure() {
        return API_GSXR_Slam.GSXR_Get_RenderTexure();
    }

    ///API-No.9
    /// <summary>
    /// 获取头部物体，如果想获取头部的旋转移动等数据，在LateUpdate方法里调用
    /// </summary>
    /// <returns>空表示系统未启动完成</returns>
    public static Transform GetHead() {
        return API_GSXR_Slam.GSXR_Get_Head();
    }

    ///API-No.10
    /// <summary>
    /// 设置瞳距，Awake时调用，Start后调用无效
    /// </summary>
    /// <param name="offset">瞳距的偏移量，单位米</param>
    public static void SetPD(float offset = 0) {
        API_GSXR_Slam.GSXR_Set_PD(offset);
    }

    ///API-No.11
    /// <summary>
    /// 重定位,若无效果，表示系统初始化未完成,且只有在眼镜上有效
    /// </summary>
    public static void RecenterTracking() {
        API_GSXR_Slam.GSXR_RecenterTracking();
    }

    ///API-No.12
    /// <summary>
    /// StartSlam
    /// </summary>
    public static void StartSlam() {
        API_GSXR_Slam.GSXR_Start_Slam();
    }

    ///API-No.13
    /// <summary>
    /// StopSlam
    /// When a StartSlam is running (not completed), calling StopSlam will not work
    /// </summary>
    public static void StopSlam() {
        API_GSXR_Slam.GSXR_Stop_Slam();
    }

    ///API-No.14
    /// <summary>
    /// ResetSlam
    /// </summary>
    public static void ResetSlam() {
        API_GSXR_Slam.GSXR_Reset_Slam();
    }

    ///API-No.15
    /// <summary>
    /// IS Slam 6Dof DataLost
    /// </summary>
    public static bool IsSlamDataLost {
        get {
            return API_GSXR_Slam.GSXR_Is_SlamDataLost;
        }
    }

    ///API-No.16
    /// <summary>
    /// Get QvrCamera Data
    /// </summary>
    public static int GetLatestQVRCameraBinocularData(ref bool outBUdate, ref uint outCurrFrameIndex, ref ulong outFrameExposureNano, byte[] outLeftFrameData, byte[] outRightFrameData) {
        return API_GSXR_Slam.GSXR_Get_LatestQVRCameraBinocularData(ref outBUdate, ref outCurrFrameIndex, ref outFrameExposureNano, outLeftFrameData, outRightFrameData);
    }

    public static GSXRManager SlamManager {
        get {
            return API_GSXR_Slam.SlamManager;
        }
    }
}
