using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTrackControll : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imageManager;

    [SerializeField] List<GameObject> objList = new List<GameObject>();
    [SerializeField] Dictionary<string, GameObject> dicPrefab = new Dictionary<string, GameObject>();
    [SerializeField] List<ARTrackedImage> imageList = new List<ARTrackedImage>();
    public float maxTimer;
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
                    timer += Time.deltaTime;
                    if (maxTimer < timer)
                    {
                        Debug.Log("Limited인 상태에서 적정 시간이 지난 후");
                        string name = imageList[i].referenceImage.name;
                        Debug.Log(name + " name");
                        GameObject obj = dicPrefab[name];
                        Debug.Log(obj + " obj");
                        //Destroy(obj);
                        obj.SetActive(false);
                        Debug.Log(obj.activeSelf + " obj.activeself");
                        tImage.Add(imageList[i]);
                        timer = 0;
                    }
                }
            }
        
            if (tImage.Count > 0)
            {
                for (int i = 0; i < tImage.Count; i++)
                {
                    int num = imageList.IndexOf(tImage[i]);
                    imageList.Remove(imageList[num]);
                    Debug.Log(imageList.Count + " imageList Count");
                }
            }
        }
    }

    private void OnImageChange(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackImage in args.added)
        {
            if (!imageList.Contains(trackImage))
            {
                imageList.Add(trackImage);
            }

            string imageName = trackImage.referenceImage.name;
            
            switch (imageName)
            {
                case "Y Bot":
                    GameObject obj = Instantiate(GetObj(imageName), trackImage.transform.position, trackImage.transform.rotation);
                    obj.transform.parent = trackImage.transform;
                    break;
                default:
                    Debug.Log("해당된 trackImage의 이름이 없음");
                    break;
            }
        }

        foreach (ARTrackedImage trackImage in args.updated)
        {
            if (!imageList.Contains(trackImage))
            {
                imageList.Add(trackImage);
            }

            UpdateImage(trackImage);
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
