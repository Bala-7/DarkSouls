using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEditor;
using UnityEngine;

public class ThirdPersonCameraMovement : MonoBehaviour
{
    private Camera _cam;

    public enum CAMERA_TYPE { FREE_LOOK, LOCKED }

    public CAMERA_TYPE type = CAMERA_TYPE.FREE_LOOK;

    [SerializeField][Range(0.1f, 5.0f)]
    private float sensitivity;
    [SerializeField]
    private bool invertXAxis;
    [SerializeField] 
    private bool invertYAxis;

    public Transform lookAt;
    private Transform trueLookAt;

    #region SphericalCoordinates
    private double theta = Math.PI / 2;
    private float tTheta = 0.5f;
    private double alpha = -Math.PI / 2;

    
    [Serializable]
    public struct CameraSettings
    {
        [SerializeField] [Range(-1.0f, 1.0f)]
        private float offsetX;
        [SerializeField] [Range(-1.0f, 2.0f)]
        private float offsetY;
        [SerializeField] [Range(0.0f, 90.0f)]
        private float maxVerticalAngle;
        [SerializeField] [Range(0.0f, 90.0f)]
        private float minVerticalAngle;
        [SerializeField]
        private float cameraDistance;

        private Vector3 rotationOrigin;

        public void SetRotationOrigin(Vector3 o)
        {
            rotationOrigin = o;
        }

        public float GetCameraDistance()
        {
            return cameraDistance;
        }

        public Vector2 GetOffset() {return new Vector2(offsetX, offsetY);}
        
        public Vector2 GetLimitVerticalAnglesRadians(){ return new Vector2(maxVerticalAngle * (float)Math.PI / 180.0f, (minVerticalAngle * (float)Math.PI / 180.0f));}
    }
    [SerializeField]
    private CameraSettings settings;
    #endregion

    public CameraSettings GetCameraSettings()
    {
        return settings;
    }
    
    private void Awake()
    {
        if (_cam) _cam = Camera.main;

        if (type == CAMERA_TYPE.LOCKED) {
            _cam.transform.parent = transform;
        }

        trueLookAt = transform.Find("LookAtTransform");
        settings.SetRotationOrigin(transform.position + Vector3.up);
    }


    private void FixedUpdate()
    {
        OrbitSphericalCoords();
    }

    private void OrbitSphericalCoords()
    {
        // Read input
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        // Settings
        h = (invertXAxis) ? h : (-h);
        v = (invertYAxis) ? (-v) : v;

        // Orbit the camera around the character
        if (h != 0)
        {   // Horizontal movement 
            alpha += h * sensitivity * Time.deltaTime; 
        }
        if (v != 0)
        {   // Vertical movement
            Vector2 limitAnglesRads = settings.GetLimitVerticalAnglesRadians();
            float maxAngle = limitAnglesRads.x;
            float minAngle = limitAnglesRads.y;

            maxAngle = ((float)Math.PI / 2) - maxAngle;
            minAngle = ((float)Math.PI / 2) + minAngle;
            
            
            tTheta += v * sensitivity * Time.deltaTime;
            tTheta = Mathf.Clamp(tTheta, 0, 1);
            
            theta = Mathf.Lerp(maxAngle, minAngle, tTheta);
            
        }
        
        float x = lookAt.transform.position.x + (float) (settings.GetCameraDistance() * Math.Sin(theta) * Math.Cos(alpha));
        float y = lookAt.transform.position.y + (float) (settings.GetCameraDistance() * Math.Cos(theta));
        float z = lookAt.transform.position.z + (float) (settings.GetCameraDistance() * Math.Sin(theta) * Math.Sin(alpha));
        
        Vector3 newCameraPosition = new Vector3(x, y , z);
        Vector3 offsetCameraPosition = newCameraPosition + settings.GetOffset().x * _cam.transform.right + settings.GetOffset().y * _cam.transform.up;
        _cam.transform.position = offsetCameraPosition;

        trueLookAt.transform.position = lookAt.transform.position + +settings.GetOffset().x * _cam.transform.right + settings.GetOffset().y * _cam.transform.up;

        _cam.transform.LookAt(trueLookAt);
    }

    public void SetCameraToOrigin()
    {
        double originTheta = Math.PI / 2;
        double originAlpha = -Math.PI / 2;

        if (!_cam) _cam = Camera.main;
        if(!trueLookAt) trueLookAt = transform.Find("LookAtTransform");

        float camDistance = settings.GetCameraDistance();

        if (lookAt)
        {
            Vector3 newCameraPosition = lookAt.transform.position +
                                    new Vector3(camDistance * (float)(Math.Sin(originTheta) * Math.Cos(originAlpha)),
                                        camDistance * (float)(Math.Cos(originTheta)),
                                        camDistance * (float)(Math.Sin(originTheta) * Math.Sin(originAlpha)));
            Vector3 offsetCameraPosition = newCameraPosition + settings.GetOffset().x * _cam.transform.right + settings.GetOffset().y * _cam.transform.up;

            _cam.transform.position = offsetCameraPosition;

            trueLookAt.transform.position = lookAt.transform.position + +settings.GetOffset().x * _cam.transform.right + settings.GetOffset().y * _cam.transform.up;
            _cam.transform.LookAt(trueLookAt);
        }
    }

    

    public Camera GetCamera() { return _cam ? _cam : Camera.main; }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (lookAt) {
            Vector2 limitAnglesRads = settings.GetLimitVerticalAnglesRadians();
            float maxAngle = limitAnglesRads.x;
            float minAngle = limitAnglesRads.y;

            maxAngle = ((float)Math.PI / 2) - maxAngle;
            minAngle = ((float)Math.PI / 2) + minAngle;

            float minCircleRadius = (float)Math.Cos(minAngle - Math.PI / 2) * settings.GetCameraDistance();
            float minCircleY = (float)(settings.GetCameraDistance() * Math.Cos(minAngle));
            Handles.color = Color.red;
            Handles.DrawWireDisc(lookAt.transform.position + new Vector3(0, minCircleY, 0), Vector3.up, minCircleRadius);

            float maxCircleRadius = (float)Math.Cos(maxAngle - Math.PI / 2) * settings.GetCameraDistance();
            float maxCircleY = (float)(settings.GetCameraDistance() * Math.Cos(maxAngle));
            Handles.color = Color.red;
            Handles.DrawWireDisc(lookAt.transform.position + new Vector3(0, maxCircleY, 0), Vector3.up, maxCircleRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(lookAt.transform.position, settings.GetCameraDistance());
        }
#endif
    }
}
