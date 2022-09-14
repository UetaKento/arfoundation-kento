using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
//using System.Collections;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Listens for touch events and performs an AR raycast from the screen touch point.
    /// AR raycasts will only hit detected trackables like feature points and planes.
    ///
    /// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
    /// and moved to the hit position.
    /// </summary>
    [RequireComponent(typeof(ARRaycastManager))]
    public class KentyPlaceOnPlane : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;
        List<GameObject> gameObjectsList = new List<GameObject>();

        private float paintScore = 0f;
        [SerializeField]
        Text scoreText;

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject placedPrefab
        {
            get { return m_PlacedPrefab; }
            set { m_PlacedPrefab = value; }
        }

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject spawnedObject { get; private set; }

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
        }

        bool TryGetTouchPosition(out Vector2 touchPosition)
        {
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }

            touchPosition = default;
            return false;
        }

        //void WhenHitScore()
        //{
        //    paintScore += 1;
        //}

        //void WhenNoHitScore()
        //{
        //    paintScore += 5;
        //}


        void Update()
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;
                Quaternion Fixminus = Quaternion.Euler(-90, 0, 0);

                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation * Fixminus);
                //spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

                //if (spawnedObject.GetComponent<ScorePrefabManager>().IsCollision)
                //{
                //    paintScore += 1f;
                //}
                //else
                //{
                //    paintScore += 0f;
                //}

                scoreText.text = paintScore.ToString();

                //if (spawnedObject == null)
                //{
                //    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                //}
                //else
                //{
                //    spawnedObject.transform.position = hitPose.position;
                //}
            }
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();       

        ARRaycastManager m_RaycastManager;
    }
}
