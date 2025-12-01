using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//namespace SC.XR.Unity
//{
public class TrackableApi
{
    private const int PER_PLANE_DATA_COUNT = 68;

    private static Dictionary<int, PlaneTrackable> trackableDic = new Dictionary<int, PlaneTrackable>();

//#if !UNITY_EDITOR

//        [DllImport("svrplugin")]
//        public static extern int ScGetPanelInfo(float[] info);

//        [DllImport("svrplugin")]
//        public static extern int ScGetPanel();
//#else
//    public static int ScGetPanelInfo(float[] info)
//    {
//        info[0] = 1f;
//        info[1] = 3f;

//        info[2] = 1f;
//        info[3] = 0f;
//        info[4] = 0f;

//        info[5] = 0f;
//        info[6] = 0f;
//        info[7] = -1f;

//        info[8] = -1f;
//        info[9] = 0f;
//        info[10] = 0f;

//        return 0;
//    }

//    public static int ScGetPanel()
//    {
//        return 1;
//    }
//#endif

    /// <summary>
    /// index:
    /// 0: panelId
    /// 1: verticeCount
    /// 2~67 x y z
    /// </summary>
    /// <param name="trackables"></param>
    public static void GetPlaneInfo<T>(List<T> trackables) where T : Trackable
    {
        if (trackables == null)
        {
            Debug.LogError("please init trackables first!!!");
            return;
        }

        trackables.Clear();
        int planeCount = API_GSXR_Slam.GSXR_Get_PanelNum();
        float[] rawData = new float[planeCount * PER_PLANE_DATA_COUNT];
        API_GSXR_Slam.GSXR_Get_PanelInfo(rawData);

        for (int i = 0; i < planeCount; i++)//plane loop
        {
            int planeId = (int)rawData[i * PER_PLANE_DATA_COUNT];
            int planeVerticesCount = (int)rawData[i * PER_PLANE_DATA_COUNT + 1];
            Vector3[] vertices = new Vector3[planeVerticesCount];
            for (int j = 0; j < vertices.Length; j++) // plane vertices loop
            {
                float x = rawData[(i * PER_PLANE_DATA_COUNT + 2) + (vertices.Length - j - 1) * 3];
                float y = rawData[(i * PER_PLANE_DATA_COUNT + 2) + (vertices.Length - j - 1) * 3 + 1];
                float z = -rawData[(i * PER_PLANE_DATA_COUNT + 2) +(vertices.Length - j - 1) * 3 + 2];
                vertices[j] = new Vector3(x, y, z);
            }
            PlaneTrackable trackable = CreateTrackable(planeId, vertices);//new PlaneTrackable(planeId, vertices);
            trackables.SafeAdd(trackable);
        }
    }

    private static PlaneTrackable CreateTrackable(int planeId, Vector3[] vertices)
    {
        if (trackableDic.ContainsKey(planeId))
        {
            PlaneTrackable planeTrackableCache = trackableDic[planeId];
            planeTrackableCache.UpdateVertices(vertices);
            return planeTrackableCache;
        }

        PlaneTrackable newTrackablePlane = new PlaneTrackable(planeId, vertices);
        trackableDic.Add(planeId, newTrackablePlane);
        return newTrackablePlane;

    }
}
//}
