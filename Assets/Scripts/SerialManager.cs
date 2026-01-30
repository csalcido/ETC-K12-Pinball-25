using System;
using System.Collections.Concurrent;
using System.IO.Ports;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

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
    public static float BallSpeed = 0;
    public static float prevBallSpeed = 0;
    public int baudRate_pi = 115200;

    //******************************************//
    private SerialPort serialPortL;
    private SerialPort serialPortR;
    private Thread readThreadL;
    private Thread writeThreadL;
    private Thread readThreadR;
    private Thread writeThreadR;
    private bool isRunningL = false;
    /* private bool isRunningR = false; */
    private float distanceCML = 0f;
    private float distanceCMR = 0f;

    public BallSwitchLeft leftBall;
    public LeftPlunger leftLaunch;
    public BallSwitchRight rightBall;
    public RightPlunger rightLaunch;

    public float DistanceCML => distanceCML;
    public float DistanceCMR => distanceCMR;

    /// <summary>
    /// Thread-safe queue for incoming serial data.
    /// </summary>
    private readonly ConcurrentQueue<string> incoming = new();

    void Start()
    {
        serialPortL = new SerialPort(portNameL, baudRate_pi);
        serialPortL.ReadTimeout = 20;
        // serialPortR = new SerialPort(portNameR, baudRate_pi);
        // serialPortR.ReadTimeout = 20;

        try
        {
            serialPortL.Open();
            isRunningL = true;
            Debug.Log($"{portNameL} opened");

            readThreadL = new Thread(ReadSerialDataL);
            readThreadL.Start();

            writeThreadL = new Thread(WriteSerialDataL);
            writeThreadL.Start();

            // serialPortR.Open();
            // isRunningR = true;
            // readThreadR = new Thread(ReadSerialDataR);
            // readThreadR.Start();
            // writeThreadR = new Thread(WriteSerialDataR);
            // writeThreadR.Start();
            Debug.Log("Both serial port opened successfully.");
        }
        catch (System.Exception e)
        {
            Debug.Log("Failed to open serial port: " + e.Message);
        }
        
    }

    private void LateUpdate()
    {
        /* Parse incoming data from the queue
         * 
         * Since you don't know the order in which scripts will run Update,
         * there is a chance that one script will check a button state BEFORE
         * this script has updated it leading to missed button presses.
         * 
         * LateUpdate runs AFTER all other Update() calls, so any change to
         * button states will be queued up and processed by other scripts
         * during their next Update() cycle and we know that the state will be
         * conssistet for all scripts because we won't change the state until
         * AFTER all Update() calls have been made.
         * 
         * Additionally, the thread reading the input data is running asynchronously
         * from the main Unity thread, so we need to place data into a thread-safe
         * queue to be processed here in LateUpdate.
         * 
         * We process just 1 line of input per LateUpdate() cycle, otherwise we
         * might both press and unpress a button in the same frame and the button
         * press would be missed by all the other scripts.
         * 
         * bm3n 
         */

        if (incoming.TryDequeue(out string line))
        {
            Debug.Log("Received: " + line);

            switch (line)
            {
                case "start_pressed":
                    StartPressed = true;
                    StartReleased = false;
                    break;
                case "start_released":
                    StartReleased = true;
                    StartPressed = false;
                    break;
                case "left_flipper_pressed":
                    LeftFlipperPressed = true;
                    LeftFlipperReleased = false;
                    break;
                case "left_flipper_released":
                    LeftFlipperReleased = true;
                    LeftFlipperPressed = false;
                    break;
                case "right_flipper_pressed":
                    RightFlipperPressed = true;
                    RightFlipperReleased = false;
                    break;
                case "right_flipper_released":
                    RightFlipperReleased = true;
                    RightFlipperPressed = false;
                    break;
                case "ball_sent":
                    BallSent = true;
                    break;
                case "ball_back":
                    BallSent = false;
                    break;
                default:
                    // If it isn't one of the known commands, try parsing as a float
                    if (float.TryParse(line, out float speed))
                    {
                        if (speed != prevBallSpeed)
                        {
                            BallSpeed = speed;
                            prevBallSpeed = speed;
                        }
                        else
                        {
                            BallSpeed = 0;
                            prevBallSpeed = 0;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to parse data: {line}");
                    }
                    break;
            }
        }

    }

    void OnDestroy()
    {
        isRunningL = false;

        if (readThreadL != null && readThreadL.IsAlive)
        {
            readThreadL.Join();
        }

        if (serialPortL != null && serialPortL.IsOpen)
        {
            serialPortL.Close();
            Debug.Log($"{portNameL} closed.");
        }

        /*
        isRunningR = false;

        if (readThreadR != null && readThreadR.IsAlive)
        {
            readThreadR.Join();
        }

        if (serialPortR != null && serialPortR.IsOpen)
        {
            serialPortR.Close();
            Debug.Log($"{portNameR}  closed.");
        }
        */
    }

    private void ReadSerialDataL()
    {
        while (isRunningL)
        {
            try
            {
                string data = serialPortL.ReadLine().Trim();
                incoming.Enqueue(data);
            }
            catch (TimeoutException) 
            { 
                /* ignore timeouts */ 
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Error reading data from {portNameL}: " + e.Message);
            }
        }
    }

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
                        Debug.LogWarning($"Error writing to {portNameL}: " + e.Message);
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
                    Debug.Log($"Launch data sent: {data}*{launchSent}");
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Error writing serial data: {e.Message}");
                }

            }
        }
    }

    /*
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
    */

}

