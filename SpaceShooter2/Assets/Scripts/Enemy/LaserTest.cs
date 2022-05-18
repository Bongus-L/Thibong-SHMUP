using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTest : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 5.0f;
    private Transform _player;

    private void Start()
    {
        _player = GameObject.Find("PlayerPosition").GetComponent<Transform>();
        transform.Rotate(0, 0, AngleBetweenPoints(transform.position, _player.transform.position));
        StartCoroutine(CountDownDestruction());
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.position, _laserSpeed * Time.deltaTime);
        transform.rotation = Quaternion.AngleAxis(AngleBetweenPoints(transform.position, _player.transform.position), Vector3.forward);

        if (transform.position.y < -5.35f || transform.position.x > 10.4 || transform.position.x < -10.4)
        {
            if (transform.parent)
            {
                // Code below also detroys any child objects associated with the parent object.
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    IEnumerator CountDownDestruction()
    {
        while (gameObject)
        {
            yield return new WaitForSeconds(5.5f);
            Destroy(this.gameObject);
        }
    }

    // Random behaviour missiles.
    //protected void LookAt(Vector2 point)
    //{
    //    float angle = AngleBetweenPoints(transform.position, point)-90f;
    //    var targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    //    // Add the lines below in update
    //    //LookAt(_player.position);
    //    //transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
    //}

    private float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return (Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg)+90f;
    }
}
