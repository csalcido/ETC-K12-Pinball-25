using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Drawing.Text;
using System;

public class SerialManager : MonoBehaviour
{

    public string portNameL = "COM3";
   // public string portNameR = "COM3";
    public int baudRate = 9600;

    //****************************************//
    //hannah section
    public static bool StartPressed = false;
    public static bool StartReleased = false;
    public static bool LeftFlipperPressed = false;
    public static bool LeftFlipperReleased = false;
    public static bool RightFlipperPressed = false;
    public static bool RightFlipperReleased = false;
    public static bool BallSent = false;
    public int baudRate_pi = 115200;

    //******************************************//
    private SerialPort serialPortL;
    private SerialPort serialPortR;
    private Thread readThreadL;
    private Thread writeThreadL;
    private Thread readThreadR;
    private Thread writeThreadR;
    private bool isRunningL = false;
    private bool isRunningR = false;
    private float distanceCML = 0f;
    private float distanceCMR = 0f;

    public BallSwitchLeft leftBall;
    public LeftPlunger leftLaunch;
    public BallSwitchRight rightBall;
    public RightPlunger rightLaunch;

    public float DistanceCML => distanceCML;
    public float DistanceCMR => distanceCMR;

    void Start()
    {
        // serialPortL = new SerialPort(portNameL, baudRate); guess whoooo
        serialPortL = new SerialPort(portNameL, baudRate_pi);
       // serialPortR = new SerialPort(portNameR, baudRate_pi);
        serialPortL.ReadTimeout = 20;
      //  serialPortR.ReadTimeout = 20;

        try
        {
            serialPortL.Open();
            Debug.Log("L opened");
          //  serialPortR.Open();
            isRunningL = true;
          //  isRunningR = true;

           // readThreadL = new Thread(ReadSerialDataL);
           // readThreadL.Start();
            writeThreadL = new Thread(WriteSerialDataL);
            writeThreadL.Start();
          //  readThreadR = new Thread(ReadSerialDataR);
           // readThreadR.Start();
           // writeThreadR = new Thread(WriteSerialDataR);
           // writeThreadR.Start();
            Debug.Log("Both serial port opened successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    void Update()
    {
        //Debug.Log("Left distance: " + distanceCML + " cm");
        //Debug.Log("Right distance: " + distanceCMR + " cm");

        //added later by hannah for pi controller purposes

        //*******************************************
        try
        {
            if (isRunningL && serialPortL.BytesToRead > 0)
            {
                string line = serialPortL.ReadLine().Trim();
                Debug.Log("Recieved: " + line);
                if (line == "start_pressed")
                {
                    StartPressed = true;
                    Debug.Log("Button Press Detected!");
                }
                else if (line == "start_released")
                {
                    StartPressed = false;
                }
                else if (line == "left_flipper_pressed")
                {
                    LeftFlipperPressed = true;
                    LeftFlipperReleased = false;
                }
                else if (line == "left_flipper_released")
                {
                    LeftFlipperReleased = true;
                    LeftFlipperPressed = false;
                }
                else if (line == "right_flipper_pressed")
                {
                    RightFlipperPressed = true;
                    RightFlipperReleased = false;
                }
                else if (line == "right_flipper_released")
                {
                    RightFlipperReleased = true;
                    RightFlipperPressed = false;
                }
                else if (line == "ball_sent")
                {
                    BallSent = true;
                }
                else if (line == "ball_back")
                {
                    BallSent = false;
                }
                else
                {
                    //put logic here for plunger data
                }
            }
        }
        catch (TimeoutException) { /* EAT IT */ }
        //*******************************************
    }

    void OnDestroy()
    {
        isRunningL = false;
        isRunningR = false;

        if (readThreadL != null && readThreadL.IsAlive)
        {
            readThreadL.Join();
        }

        if (serialPortL != null && serialPortL.IsOpen)
        {
            serialPortL.Close();
            Debug.Log("Serial port closed.");
        }

        if (readThreadR != null && readThreadR.IsAlive)
        {
            readThreadR.Join();
        }

        if (serialPortR != null && serialPortR.IsOpen)
        {
            serialPortR.Close();
            Debug.Log("Serial port closed.");
        }
    }
    /*
    private void ReadSerialDataL()
    {
        while (isRunningL)
        {
            try
            {
                string data = serialPortL.ReadLine();

                if (float.TryParse(data, out float parsedDistance))
                {
                    distanceCML = parsedDistance;
                }
                else
                {
                    Debug.LogWarning("Failed to parse data: " + data);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error reading serial data: " + e.Message);
            }
        }
    }
    private void ReadSerialDataR()
    {
        while (isRunningR)
        {
            try
            {
                string data = serialPortR.ReadLine();

                if (float.TryParse(data, out float parsedDistance))
                {
                    distanceCMR = parsedDistance;
                }
                else
                {
                    Debug.LogWarning("Failed to parse data: " + data);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error reading serial data: " + e.Message);
            }
        }
    }
    */
    private void WriteSerialDataL()
    {
        int colorCache = -1;
        bool accumulatedLaunch = false;
        int launchSent = 0;
        while (isRunningL)
        {
            while (leftLaunch.isLaunching)
            {
                accumulatedLaunch = true;
            }

            if (accumulatedLaunch == false)
            {
                if (colorCache != leftBall.currentMaterialIndex)
                {
                    colorCache = leftBall.currentMaterialIndex;
                    try
                    {
                        string data = (colorCache.ToString() + "\n");
                        serialPortL.Write(data);
                        Debug.Log("Color data sent: " + data);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning("Error writing serial data: " + e.Message);
                    }
                }
            }
            else
            {
                colorCache = leftBall.currentMaterialIndex + 3;
                accumulatedLaunch = false;
                try
                {
                    string data = (colorCache.ToString() + "\n");
                    serialPortL.Write(data);
                    launchSent++;
                    Debug.Log("Launch data sent: " + data + "*" + launchSent);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Error writing serial data: " + e.Message);
                }
                
            }
        }
    }
    private void WriteSerialDataR()
    {
        int colorCache = -1;
        bool accumulatedLaunch = false;
        int launchSent = 0;
        while (isRunningR)
        {
            while (rightLaunch.isLaunching)
            {
                accumulatedLaunch = true;
            }

            if (accumulatedLaunch == false)
            {
                if (colorCache != rightBall.currentMaterialIndex)
                {
                    colorCache = rightBall.currentMaterialIndex;
                    try
                    {
                        string data = (colorCache.ToString() + "\n");
                        serialPortR.Write(data);
                        Debug.Log("Color data sent: " + data);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning("Error writing serial data: " + e.Message);
                    }
                }
            }
            else
            {
                colorCache = rightBall.currentMaterialIndex + 3;
                accumulatedLaunch = false;
                try
                {
                    string data = (colorCache.ToString() + "\n");
                    serialPortR.Write(data);
                    launchSent++;
                    Debug.Log("Launch data sent: " + data + "*" + launchSent);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Error writing serial data: " + e.Message);
                }

            }
        }
    }
}

