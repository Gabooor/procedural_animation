using UnityEngine;

public class CameraCircleAround : MonoBehaviour
{
    public Transform spider; 
    float distance = 10f; // Camera distance from spider
    float height = 5f; // Height of camera
    float speed = 0.5f; // Rotation speed of camera
    float tiltAngle = 5f; // Angle at which the camera is tilted

    private float angle = 0f; 
    private float spiderStartingY; // The y position of the spider at the start, using this to prevent camera bobbing


    void Start()
    {
        spiderStartingY = spider.position.y;
    }

    void Update()
    {
        // Circular path around the spider
        float x = Mathf.Sin(angle) * distance;
        float z = Mathf.Cos(angle) * distance;

        // Camera positioning
        Vector3 newPosition = new Vector3(spider.position.x + x, spiderStartingY + height, spider.position.z + z);
        transform.position = newPosition;

        // Setting a Vector3 to look at. Instead of using the spider's y position we use its initial y position to prevent camera bobbing
        Vector3 spiderLookAtPosition = new Vector3(spider.position.x, spiderStartingY, spider.position.z);
        transform.LookAt(spiderLookAtPosition);

        transform.Rotate(Vector3.right, tiltAngle);

        angle += speed * Time.deltaTime;
    }
}
