using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : MonoBehaviour
{
    public Material mat;
    public Vector2 pos;
    public float scale, angle;

    private Vector2 smoothPos;
    private float smoothScale, smoothAngle;

    private void UpdateShader() {
        smoothPos = Vector2.Lerp(smoothPos, pos, .03f);
        smoothScale = Mathf.Lerp(smoothScale, scale, .03f);
        smoothAngle = Mathf.Lerp(smoothAngle, angle, .03f);

        float aspect = (float)Screen.width / (float)Screen.height;

        float scaleX = smoothScale;
        float scaleY = smoothScale;

        if(aspect>1f)
            scaleY /= aspect;
        else
            scaleX *= aspect;

        mat.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY));
        mat.SetFloat("_Angle", smoothAngle);
    }

    private void HandleKeys() {
        if(Input.GetKey(KeyCode.UpArrow))
            scale *= .99f;
        if(Input.GetKey(KeyCode.DownArrow))
            scale *= 1.01f;

        if(Input.GetKey(KeyCode.E))
            angle -= .01f;
        if(Input.GetKey(KeyCode.Q))
            angle += .01f;

        Vector2 dir = new Vector2(.01f * scale, 0);
        float s = Mathf.Sin(angle);
        float c = Mathf.Cos(angle);
        dir = new Vector2(dir.x*c, dir.x*s);

        if(Input.GetKey(KeyCode.A))
            pos -= dir;
        if(Input.GetKey(KeyCode.D))
            pos += dir;

        dir = new Vector2(-dir.y, dir.x);

        if(Input.GetKey(KeyCode.S))
            pos -= dir;
        if(Input.GetKey(KeyCode.W))
            pos += dir;
    }

    private void HandleTouches() {
        if (Input.touchCount == 1) {
            pos -= .0002f * scale * Input.GetTouch(0).deltaPosition;
        }

        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            scale *= 1f + 0.0005f * deltaMagnitudeDiff;


            Vector2 touchMiddle = (touchZero.position + touchOne.position) / 2;
            Vector2 screenMiddle = new Vector2(Screen.width/2, Screen.height/2);
            Vector2 posDelta = touchMiddle - screenMiddle;
            pos += 0.00001f * scale * posDelta;
        }
    }

    private void HandleInputs() {
        HandleKeys();
        HandleTouches();
    }

    void FixedUpdate()
    {
        HandleInputs();
        UpdateShader();
    }
}
