
    using UnityEngine;
public class tempAngle : MonoBehaviour
{
    public Transform obj1, obj2;

    void Start()
    {
        Debug.Log(obj1.forward);
        Debug.Log((obj2.position - obj1.position).normalized);
        Debug.Log((obj2.position.magnitude - obj1.position.magnitude) + " " + Vector3.Distance(obj1.position,obj2.position));
        obj1.LookAt(obj2);
        Debug.Log(obj1.forward);
        //obj1.transform.Translate((obj2.position - obj1.position).normalized * Time.deltaTime);
    }
}
