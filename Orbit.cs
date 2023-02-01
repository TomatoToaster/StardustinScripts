using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float rotateSpeed = 0;
    public GameObject offcenterPrefab;
    private int bulletMax;

    // Start is called before the first frame update
    void Start()
    {
        bulletMax = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().bulletMax;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0 , rotateSpeed * Time.deltaTime), Space.Self);
    }

    public void addBullet()
    {
        GameObject newBullet = Instantiate(offcenterPrefab, gameObject.transform);

        // And also rotate this offcenter bullet based the index of the newly created bullet (i.e. how many children there are -1)
        int lastBulletIndex = transform.childCount - 1;
        float rotationDiff = 360f / bulletMax;
        newBullet.transform.localRotation = Quaternion.Euler(0, 0, rotationDiff * lastBulletIndex);
    }

    public void removeBullet()
    {
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
    }
}
