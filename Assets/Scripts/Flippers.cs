using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class Flippers : MonoBehaviour
{
    public HingeJoint leftFlipper;
    public HingeJoint rightFlipper;
    public HingeJoint upperLeftFlipper;
    public HingeJoint upperRightFlipper;

    public Restart restart;

    public SoundController flipperUpSound;
    public SoundController flipperDownSound;

    public float targetAngle = 45f;
    public float flipSpeed = 10000f;

    private JointSpring leftSpring;
    private JointSpring rightSpring;
    private JointSpring upperLeftSpring;
    private JointSpring upperRightSpring;

    private Xbox xboxControls;
    public GameStateManager gameStateManager;

    void Start()
    {
        leftSpring = leftFlipper.spring;
        rightSpring = rightFlipper.spring;
        upperLeftSpring = upperLeftFlipper.spring;
        upperRightSpring = upperRightFlipper.spring;

        xboxControls = new Xbox();
        xboxControls.Enable();
    }

    void Update()
    {

        leftSpring.spring = flipSpeed;
        rightSpring.spring = flipSpeed;
        upperLeftSpring.spring = flipSpeed;
        upperRightSpring.spring = flipSpeed;

        //check gameState
        if (gameStateManager.currentState == GameStateManager.ScreenState.GameBoard)
        {
            //play flippers sound
            if (Input.GetKeyDown(KeyCode.LeftArrow) || xboxControls.Player.LeftArrow.WasPressedThisFrame() || SerialManager.LeftFlipperPressed)
            {
                flipperUpSound.PlaySound();
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow) || xboxControls.Player.LeftArrow.WasReleasedThisFrame() || SerialManager.LeftFlipperReleased)
            {
                flipperDownSound.PlaySound();
            }

            //control flippers
            if (Input.GetKey(KeyCode.LeftArrow) || xboxControls.Player.LeftArrow.IsPressed() || SerialManager.LeftFlipperPressed)
            {
                Debug.Log("left arrow");
                leftSpring.targetPosition = -targetAngle;
                restart.RegisterLeftFlipper();
            }
            else
            {
                leftSpring.targetPosition = 0f;
                restart.CancelLeftFlipper();
            }

            // play sound for right flippers
            if (Input.GetKeyDown(KeyCode.RightArrow) || xboxControls.Player.RightArrow.WasPressedThisFrame() || SerialManager.RightFlipperPressed)
            {
                flipperUpSound.PlaySound();
            }
            if (Input.GetKeyUp(KeyCode.RightArrow) || xboxControls.Player.RightArrow.WasReleasedThisFrame() || SerialManager.RightFlipperReleased)
            {
                flipperDownSound.PlaySound();
            }

            // control right flippers
            if (Input.GetKey(KeyCode.RightArrow) || xboxControls.Player.RightArrow.IsPressed() || SerialManager.RightFlipperPressed)
            {
                rightSpring.targetPosition = targetAngle;
                restart.RegisterRightFlipper();
            }
            else
            {
                rightSpring.targetPosition = 0f;
                restart.CancelRightFlipper();
            }

            if (Input.GetKey(KeyCode.LeftArrow) || xboxControls.Player.LeftArrow.IsPressed() || SerialManager.LeftFlipperPressed)
            {
                upperLeftSpring.targetPosition = -targetAngle;
            }
            else
            {
                upperLeftSpring.targetPosition = 0f;
            }

            if (Input.GetKey(KeyCode.RightArrow) || xboxControls.Player.RightArrow.IsPressed() || SerialManager.RightFlipperPressed)
            {
                upperRightSpring.targetPosition = targetAngle;
            }
            else
            {
                upperRightSpring.targetPosition = 0f;
            }
        }

        leftFlipper.spring = leftSpring;
        rightFlipper.spring = rightSpring;
        upperLeftFlipper.spring = upperLeftSpring;
        upperRightFlipper.spring = upperRightSpring;
    }
}
