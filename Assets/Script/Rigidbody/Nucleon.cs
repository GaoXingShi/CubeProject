using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Nucleon : MonoBehaviour
{
    public float attractionForce;
    public Vector3 finalV3;
    private Rigidbody body;

    private float initDistance;
    // Use this for initialization
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        finalV3 = new Vector3(0, 10, 0);
        //body.AddForce((finalV3 - transform.position).normalized, ForceMode.Impulse);
    }

    void Start()
    {
        initDistance = Vector3.Distance(finalV3, transform.position);
    }

    public void SetValue(Vector3 _finalV3)
    {
        finalV3 = _finalV3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //body.velocity = Vector3.zero;
        //body.AddForce((finalV3 - transform.position).normalized, ForceMode.Impulse);
        //body.AddExplosionForce(100, finalV3, 20);
        transform.RotateAround(Vector3.zero,Vector3.up,Time.deltaTime * 5);
    }
}
