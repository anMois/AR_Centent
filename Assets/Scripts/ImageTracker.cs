using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTracker : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imageManager;

    [SerializeField] List<GameObject> objPrefabList = new List<GameObject>();
    [SerializeField] Dictionary<string, GameObject> objDicList = new Dictionary<string, GameObject>();
    [SerializeField] List<ARTrackedImage> trackImageList = new List<ARTrackedImage>();
    [SerializeField] float checkTime;
    [SerializeField] float timer;

    private void Awake()
    {
        foreach (GameObject obj in objPrefabList)
        {
            string name = obj.name;
            objDicList.Add(name, obj);
        }
    }

    private void OnEnable()
    {
        imageManager.trackedImagesChanged += OnImageChange;
    }

    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnImageChange;
    }

    private void Update()
    {
        if (trackImageList.Count > 0)
        {
            for (int i = 0; i < trackImageList.Count; i++)
            {
                if (trackImageList[i].trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
                {
                    if (checkTime < timer)
                    {
                        trackImageList[i].gameObject.SetActive(false);
                        timer = 0;
                    }
                    else
                    {
                        timer += Time.deltaTime;
                    }
                }
            }
        }
    }

    private void OnImageChange(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImage in args.added)
        {
            string imageName = trackedImage.referenceImage.name;

            switch (imageName)
            {
                case "Y Bot":
                    GameObject dragon = Instantiate(GetObjPrefab(imageName), trackedImage.transform.position, trackedImage.transform.rotation);
                    dragon.transform.parent = trackedImage.transform;
                    trackImageList.Add(trackedImage);
                    break;
            }
        }

        foreach (ARTrackedImage trackedImage in args.updated)
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                trackedImage.transform.GetChild(0).position = trackedImage.transform.position;
                trackedImage.transform.GetChild(0).rotation = trackedImage.transform.rotation;
                trackedImage.gameObject.SetActive(true);
                timer = 0;
            }
            else
            {
                trackedImage.gameObject.SetActive(false);
            }
        }
    }

    private GameObject GetObjPrefab(string str)
    {
        return objDicList[str];
    }
}
