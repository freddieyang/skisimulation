using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class push_data : MonoBehaviour
{
    private Transform tf;
    // Start is called before the first frame update
    void Start()
    {
        tf = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 euler_angles = tf.localRotation.eulerAngles;
        Vector3 position = tf.localPosition;
        string tex = conv(euler_angles.x);
        string tey = conv(euler_angles.y);
        string tez = conv(euler_angles.z);
        string tpx = conv(position.x);
        string tpy = conv(position.y);
        string tpz = conv(position.z);
        print(tpx + tpy + tpz + tez + tex + tey);
    }

    string conv(float x)
    {
        byte[] bytes = BitConverter.GetBytes(x);
        Array.Reverse(bytes);
        return (BitConverter.ToInt32(bytes, 0).ToString("X8"));
    }
}
