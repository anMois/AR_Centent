using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TestImageTracker : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imageManager;

    [SerializeField] GameObject objPrefab;
    [SerializeField] GameObject magicianprefab;
    [SerializeField] float timer;
    [SerializeField] float checkTime;
    [SerializeField] List<ARTrackedImage> trackImageList = new List<ARTrackedImage>();

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
        //새로운 이미지가 추적되었을 때
        foreach (ARTrackedImage trackedImage in args.added)
        {
            //이미지 라이브러리에서 이미지의 이름을 확인
            string imageName = trackedImage.referenceImage.name;

            //새로운 게임오브젝트를 트래킹한 이미지의 자식으로 생성
            switch (imageName)
            {
                case "Y Bot":
                    GameObject dragon = Instantiate(objPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    dragon.transform.parent = trackedImage.transform;
                    trackImageList.Add(trackedImage);
                    break;
                case "Magician":
                    GameObject magician = Instantiate(magicianprefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    magician.transform.parent = trackedImage.transform;
                    break;
            }
        }

        //기존의 이미지가 변경(이동, 회전) 되었을 때
        foreach (ARTrackedImage trackedImage in args.updated)
        {
            //이미지의 변셩사항이 있는 경우 자식으로 있건 게임오브젝트를 위치와 회전을 갱신
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                trackedImage.transform.GetChild(0).position = trackedImage.transform.position;
                trackedImage.transform.GetChild(0).rotation = trackedImage.transform.rotation;
                trackedImage.gameObject.SetActive(true);
            }
            else
            {
                trackedImage.gameObject.SetActive(false);
            }
        }
    }
}
