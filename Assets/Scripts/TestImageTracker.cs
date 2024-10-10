using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TestImageTracker : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imageManager;

    [SerializeField] GameObject dragonprefab;
    [SerializeField] GameObject magicianprefab;

    private void OnEnable()
    {
        imageManager.trackedImagesChanged += OnImageChange;
    }

    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnImageChange;

    }
    private void OnImageChange(ARTrackedImagesChangedEventArgs args)
    {
        //���ο� �̹����� �����Ǿ��� ��
        foreach (ARTrackedImage trackedImage in args.added)
        {
            //�̹��� ���̺귯������ �̹����� �̸��� Ȯ��
            string imageName = trackedImage.referenceImage.name;

            //���ο� ���ӿ�����Ʈ�� Ʈ��ŷ�� �̹����� �ڽ����� ����
            switch (imageName)
            {
                case "Dragon":
                    GameObject dragon = Instantiate(dragonprefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    dragon.transform.parent = trackedImage.transform;
                    break;
                case "Magician":
                    GameObject magician = Instantiate(magicianprefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    magician.transform.parent = trackedImage.transform;
                    break;
            }
        }

        //������ �̹����� ����(�̵�, ȸ��) �Ǿ��� ��
        foreach (ARTrackedImage trackedImage in args.updated)
        {
            //�̹����� ���ͻ����� �ִ� ��� �ڽ����� �ְ� ���ӿ�����Ʈ�� ��ġ�� ȸ���� ����
            trackedImage.transform.GetChild(0).position = trackedImage.transform.position;
            trackedImage.transform.GetChild(0).rotation = trackedImage.transform.rotation;
        }

        //������ �̹����� ������� ��
        foreach (ARTrackedImage trackedImage in args.removed)
        {
            //�̹����� ����� ��� �ڽ����� �־��� ���ӿ�����Ʈ�� ����
            Destroy(trackedImage.transform.GetChild(0).gameObject);
        }
    }
}
