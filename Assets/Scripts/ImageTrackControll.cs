using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTrackControll : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imageManager;

    [SerializeField] List<GameObject> objList = new List<GameObject>();
    [SerializeField] Dictionary<string, GameObject> dicPrefab = new Dictionary<string, GameObject>();
    [SerializeField] List<ARTrackedImage> imageList = new List<ARTrackedImage>();
    [SerializeField] List<float> timerList = new List<float>();
    public float timer;

    private void Awake()
    {
        foreach (GameObject obj in objList)
        {
            string name = obj.name;
            dicPrefab.Add(name, obj);
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
        if (imageList.Count > 0)
        {
            List<ARTrackedImage> tImage = new List<ARTrackedImage>();

            for (int i = 0; i < imageList.Count; i++)
            {
                if (imageList[i].trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
                {
                    if (timerList[i] > timer)
                    {
                        string name = imageList[i].referenceImage.name;
                        GameObject obj = dicPrefab[name];
                        obj.SetActive(false);
                        tImage.Add(imageList[i]);
                    }
                    else
                    {
                        timerList[i] += Time.deltaTime;
                    }
                }
            }

            if (tImage.Count > 0)
            {
                for (int i = 0; i < tImage.Count; i++)
                {
                    int num = imageList.IndexOf(tImage[i]);
                    imageList.Remove(imageList[num]);
                    timerList.Remove(timerList[num]);
                }
            }
        }
    }

    private void OnImageChange(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage image in args.added)
        {
            Debug.Log("add if진입전");
            if (!imageList.Contains(image))
            {
                Debug.Log("if진입후");
                imageList.Add(image);
                timerList.Add(0);
            }

            string imageName = image.referenceImage.name;

            switch (imageName)
            {
                case "Y Bot":
                    GameObject obj = Instantiate(GetObj(imageName), image.transform.position, image.transform.rotation);
                    obj.transform.parent = image.transform;
                    break;
            }

        }

        foreach (ARTrackedImage image in args.updated)
        {
            if (!imageList.Contains(image))
            {
                imageList.Add(image);
                timerList.Add(0);
            }
            else
            {
                int num = imageList.IndexOf(image);
                timerList[num] = 0;
            }

            UpdateImage(image);
        }
    }

    private void UpdateImage(ARTrackedImage image)
    {
        string str = image.referenceImage.name;
        GameObject obj = dicPrefab[str];
        obj.transform.position = image.transform.position;
        obj.transform.rotation = image.transform.rotation;
    }

    private GameObject GetObj(string str)
    {
        return dicPrefab[str];
    }
}
