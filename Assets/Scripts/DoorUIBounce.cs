using UnityEngine;

public class DoorUIBounce : MonoBehaviour
{
    public AnimationCurve movementCurve;

    float speed = 1.2f;

    float initialYPos;

    float time = 0.00f;

    private void Start()
    {
        initialYPos = transform.position.y;
    }

    void Update()
    {
        if ((time += speed * Time.deltaTime) > 1.00f)
            time = 0.00f;
        transform.position = new Vector3(
            transform.position.x,
            initialYPos + movementCurve.Evaluate(time),
            transform.position.z
        );
    }
}
