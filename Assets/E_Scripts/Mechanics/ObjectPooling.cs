using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    [SerializeField] private GameObject objPrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private List<GameObject> objList;

    private static ObjectPooling instance;
    public static ObjectPooling Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        AddBullet2Pool(poolSize);
    }

    void AddBullet2Pool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(objPrefab, transform.position, Quaternion.identity);
            obj.SetActive(false);
            objList.Add(obj);
            obj.transform.parent = transform;
        }
    }

    public GameObject RequestBullet()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            if (!objList[i].activeSelf)
            {
                objList[i].SetActive(true);
                return objList[i];
            }
        }
        AddBullet2Pool(1);
        objList[objList.Count - 1].SetActive(true);
        return objList[objList.Count - 1];
    }

    public void TurnOffObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
