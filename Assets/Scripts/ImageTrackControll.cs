using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTrackControll : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imageManager;

    [SerializeField] List<GameObject> objList = new List<GameObject>();
    [SerializeField] Dictionary<string, GameObject> dicPrefab = new Dictionary<string, GameObject>();

    private void Start()
    {
        foreach (GameObject obj in objList)
        {
            string str = obj.name;
            dicPrefab.Add(str, obj);
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

    private void OnImageChange(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage image in obj.added)
        {

        }
        
        foreach (ARTrackedImage image in obj.updated)
        {

        }
    }
}
