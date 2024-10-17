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
            GameObject objPrefab = Instantiate(obj);
            objDicList.Add(name, objPrefab);
            objPrefab.SetActive(false);
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
                    trackImageList.Add(trackedImage);
                    break;
            }
        }

        foreach (ARTrackedImage trackedImage in args.updated)
        {
            string name = trackedImage.referenceImage.name;
            GameObject obj = objDicList[name];
            StatusContoroll buttonControll = obj.GetComponent<StatusContoroll>();
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                obj.transform.position = trackedImage.transform.position;
                obj.transform.rotation = trackedImage.transform.rotation;
                obj.SetActive(true);
                timer = 0;
            }
            else
            {
                obj.SetActive(false);
                if (buttonControll != null)
                {
                    buttonControll.ChangeStatusText(StatusContoroll.State.Idle);
                }
            }
        }
    }

    private GameObject GetObjPrefab(string str)
    {
        return objDicList[str];
    }
}
