using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using System.Threading;

public class push_data : MonoBehaviour
{
    public string ReceiveMsg = "";
    public string ErrorMsg = "";
    public string SendMsg = "";
    byte[] dataSend = new byte[32];
    public string portName = "COM4";
    public int baudRate = 115200;
    private Parity parity = Parity.None;
    private int dataBits = 8;
    private StopBits stopBits = StopBits.One;
    private SerialPort sp = null;
    public string[] arrPorts;

    private Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        tf = this.GetComponent<Transform>();
        arrPorts = SerialPort.GetPortNames();
        StartPortOpen();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 position = tf.localPosition;
       
        Vector3 euler_angles = tf.localRotation.eulerAngles;
        set_protocal(position, euler_angles);
        
        display();
        WriteData(dataSend);
    }

    void display()
    {
        var hex = BitConverter.ToString(dataSend, 0).Replace("-", string.Empty).ToUpper();
        Debug.Log(hex);
    }

    void set_protocal(Vector3 position, Vector3 euler_angles)
    {
        Debug.Log(position + ":" + euler_angles);
        byte[] x = BitConverter.GetBytes(position.x);
        byte[] y = BitConverter.GetBytes(position.z);
        byte[] z = BitConverter.GetBytes(position.y);
        byte[] alpha = BitConverter.GetBytes(euler_angles.x);
        byte[] beta = BitConverter.GetBytes(euler_angles.z);
        byte[] gamma = BitConverter.GetBytes(euler_angles.y);
        dataSend[0] = 0xFB;
        dataSend[1] = 0xFD;
        for (int i = 0; i < 4; i++)
        {
            dataSend[2 + i] = x[i];
        }
        for (int i = 0; i < 4; i++)
        {
            dataSend[6 + i] = y[i];
        }
        for (int i = 0; i < 4; i++)
        {
            dataSend[10 + i] = z[i];
        }
        for (int i = 0; i < 4; i++)
        {
            dataSend[14 + i] = alpha[i];
        }
        for (int i = 0; i < 4; i++)
        {
            dataSend[18 + i] = beta[i];
        }
        for (int i = 0; i < 4; i++)
        {
            dataSend[22 + i] = gamma[i];
        }
        for (int i = 26; i < 31; i++)
        {
            dataSend[i] = 0x0;
        }
        int crc = 0;
        for (int i = 1; i < 31; i++)
        {
            crc += dataSend[i];
        }
        crc = crc % 256;
        dataSend[31] = (byte)crc;

    }


    void StartPortOpen()
    {
        if (sp != null && sp.IsOpen)
        {
            ClosePort();
        }
        OpenPort();
    }


    public void WriteData(byte[] databyte)
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Write(databyte,0,32);
            //sp.Write(byte[],offset,count);
        }
    }

    public void ClosePort()
    {
        try
        {
            if (sp != null)
                sp.Close();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void OpenPort()
    {
        sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        sp.ReadTimeout = 400;
        try
        {
            sp.Open();
        }
        catch (Exception ex)
        {
            ErrorMsg = ex.Message;
            Debug.Log(ex.Message);
        }
    }

    void OnApplicationQuit()
    {
        ClosePort();
    }
}
