using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARKit;
using UnityEngine.UI;

/// <summary>
/// Populates the action unit coefficients for an <see cref="ARFace"/>.
/// </summary>
/// <remarks>
/// If this <c>GameObject</c> has a <c>SkinnedMeshRenderer</c>,
/// this component will generate the blend shape coefficients from the underlying <c>ARFace</c>.
///
/// </remarks>
/// 
[RequireComponent(typeof(ARFace))]
public class BlendShapeVisualizer : MonoBehaviour
{
    [SerializeField]
    float m_CoefficientScale = 100.0f;
    [SerializeField]
    GameObject placementPrefab;

    float goalWeight;
    Text jawWeightText;

    public float coefficientScale
    {
        get { return m_CoefficientScale; }
        set { m_CoefficientScale = value; }
    }

    [SerializeField]
    SkinnedMeshRenderer m_SkinnedMeshRenderer;

    public SkinnedMeshRenderer skinnedMeshRenderer
    {
        get
        {
            return m_SkinnedMeshRenderer;
        }
        set
        {
            m_SkinnedMeshRenderer = value;
            CreateFeatureBlendMapping();
        }
    }

    ARKitFaceSubsystem m_ARKitFaceSubsystem;

    Dictionary<ARKitBlendShapeLocation, int> m_FaceArkitBlendShapeIndexMap;

    ARFace m_Face;

    void Awake()
    {
        m_Face = GetComponent<ARFace>();
        CreateFeatureBlendMapping();
        jawWeightText = GameObject.Find("Text (Legacy)").GetComponent<Text>();
        goalWeight = Random.Range(50, 100);
    }

    void CreateFeatureBlendMapping()
    {
        if (skinnedMeshRenderer == null || skinnedMeshRenderer.sharedMesh == null)
        {
            return;
        }

        const string strPrefix = "blendShape2.";
        m_FaceArkitBlendShapeIndexMap = new Dictionary<ARKitBlendShapeLocation, int>();

        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowDownLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "browDown_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowDownRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "browDown_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowInnerUp] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "browInnerUp");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowOuterUpLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "browOuterUp_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowOuterUpRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "browOuterUp_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.CheekPuff] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "cheekPuff");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.CheekSquintLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "cheekSquint_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.CheekSquintRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "cheekSquint_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeBlinkLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeBlink_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeBlinkRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeBlink_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeLookDownLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeLookDown_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeLookDownRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeLookDown_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeLookInLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeLookIn_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeLookInRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeLookIn_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeLookOutLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeLookOut_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeLookOutRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeLookOut_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeLookUpLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeLookUp_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeLookUpRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeLookUp_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeSquintLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeSquint_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeSquintRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeSquint_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeWideLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeWide_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeWideRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "eyeWide_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.JawForward] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "jawForward");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.JawLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "jawLeft");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.JawOpen] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "jawOpen");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.JawRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "jawRight");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthClose] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthClose");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthDimpleLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthDimple_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthDimpleRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthDimple_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthFrownLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthFrown_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthFrownRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthFrown_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthFunnel] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthFunnel");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthLeft");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthLowerDownLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthLowerDown_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthLowerDownRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthLowerDown_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthPressLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthPress_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthPressRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthPress_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthPucker] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthPucker");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthRight");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthRollLower] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthRollLower");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthRollUpper] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthRollUpper");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthShrugLower] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthShrugLower");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthShrugUpper] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthShrugUpper");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthSmileLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthSmile_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthSmileRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthSmile_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthStretchLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthStretch_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthStretchRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthStretch_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthUpperUpLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthUpperUp_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthUpperUpRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "mouthUpperUp_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.NoseSneerLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "noseSneer_L");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.NoseSneerRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "noseSneer_R");
        m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.TongueOut] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "tongueOut");
    }

    void SetVisible(bool visible)
    {
        if (skinnedMeshRenderer == null) return;

        skinnedMeshRenderer.enabled = visible;
    }

    void UpdateVisibility()
    {
        var visible =
            enabled &&
            (m_Face.trackingState == TrackingState.Tracking) &&
            (ARSession.state > ARSessionState.Ready);

        SetVisible(visible);
    }

    void OnEnable()
    {
        var faceManager = FindObjectOfType<ARFaceManager>();
        if (faceManager != null)
        {
            m_ARKitFaceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;
        }
        UpdateVisibility();
        m_Face.updated += OnUpdated;
        ARSession.stateChanged += OnSystemStateChanged;
    }

    void OnDisable()
    {
        m_Face.updated -= OnUpdated;
        ARSession.stateChanged -= OnSystemStateChanged;
    }

    void OnSystemStateChanged(ARSessionStateChangedEventArgs eventArgs)
    {
        UpdateVisibility();
    }

    void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
    {
        UpdateVisibility();
        UpdateFaceFeatures();
    }
    GameObject instantiatedObject = null;
    void UpdateFaceFeatures()
    {
        int jawOpenIndex;
        float jawOpenWeight;
        float goalDistance;
        float goalPercentage;

        // https://futabazemi.net/notes/script_function/
        // ChangeText.instance.ShowMessage("顎検出");のように外部関数を使うとなぜか動かなくなる。
        if (skinnedMeshRenderer == null || !skinnedMeshRenderer.enabled || skinnedMeshRenderer.sharedMesh == null)
        {
            return;
        }
        // https://programming.pc-note.net/csharp/dictionary_method.html
        // TryGetValueの第1引数で顔のパーツ(今回は顎の開き具合)を与えて、outを使ってその顔のパーツがある配列の場所(インデックス)を第2引数にもらう。
        m_FaceArkitBlendShapeIndexMap.TryGetValue(ARKitBlendShapeLocation.JawOpen, out jawOpenIndex);

        // https://docs.unity3d.com/ScriptReference/SkinnedMeshRenderer.GetBlendShapeWeight.html
        // TryGetValueで示されたその顔のパーツのインデックスをもとに、その顔のパーツの動いている度合い(BlendShapeWeight)を取得。
        jawOpenWeight = skinnedMeshRenderer.GetBlendShapeWeight(jawOpenIndex);
        //jawWeightText.text = jawOpenWeight.ToString();

        goalDistance = Mathf.Abs(jawOpenWeight - goalWeight);
        goalPercentage = Mathf.Clamp01(goalDistance) * 100;
        jawWeightText.text = goalPercentage.ToString();

        if (goalDistance < 1)
        {
            instantiatedObject = Instantiate(placementPrefab, new Vector3(0.5f, 0.5f, 0.0f), Quaternion.identity);
        }
        // https://docs.unity3d.com/Packages/com.unity.xr.arkit-face-tracking@1.0/api/UnityEngine.XR.ARKit.ARKitFaceSubsystem.html
        // https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@4.0/api/UnityEngine.XR.ARSubsystems.TrackableId.html
        // https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.0/api/UnityEngine.XR.ARFoundation.ARTrackable-2.html#UnityEngine_XR_ARFoundation_ARTrackable_2_trackableId
        // https://docs.unity3d.com/ja/2018.4/Manual/JobSystemNativeContainer.html
        // 処理後にリソース解放するためusingを使用。
        using (var blendShapes = m_ARKitFaceSubsystem.GetBlendShapeCoefficients(m_Face.trackableId, Allocator.Temp))
        {
            foreach (var featureCoefficient in blendShapes)
            {
                int mappedBlendShapeIndex;
                if (m_FaceArkitBlendShapeIndexMap.TryGetValue(featureCoefficient.blendShapeLocation, out mappedBlendShapeIndex))
                {
                    if (mappedBlendShapeIndex >= 0)
                    {
                        skinnedMeshRenderer.SetBlendShapeWeight(mappedBlendShapeIndex, featureCoefficient.coefficient * coefficientScale);
                    }
                }
            }
        }
    }

}

