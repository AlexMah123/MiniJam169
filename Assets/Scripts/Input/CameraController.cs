using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerInput
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Configs")]
        public Transform cameraTransform;
        public float normalSpeed = 0.025f;
        public float fastSpeed = 0.05f;
        public float rotationAmount = 0.05f;
        public Vector3 zoomAmount = new Vector3(0, -10, 10);
        public float minZoomInAmount = 100f;
        public float maxZoomInAmount = 30f;
        
        [Header("Movement Configs")]
        public float movementSpeed = 1f;
        public float movementTime = 2.5f;

        [Header("Current Values")]
        public Vector3 newPosition;
        public Quaternion newRotation;
        public Vector3 newZoom;
        
        [Header("Mouse Input Values")]
        public Vector3 dragStartPosition;
        public Vector3 dragCurrentPosition;
        public Vector3 rotateStartPosition;
        public Vector3 rotateCurrentPosition;
        
        private void Start()
        {
            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = cameraTransform.localPosition;
        }

        private void Update()
        {
            HandleKeyboardInput();
            HandleMouseInput();
        }

        private void LateUpdate()
        {
            HandleMovement();
        }

        private void HandleMouseInput()
        {
            //Movement Panning
            if (Input.GetMouseButtonDown(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }
            if (Input.GetMouseButton(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);
                    newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                }
            }
            
            //Zoom
            if (Input.mouseScrollDelta.y != 0f)
            {
                //negate, scroll up is zoomIn, scroll down is zoomOut.
                newZoom -= zoomAmount * Input.mouseScrollDelta.y;
                
                //z needs to be negative, because the camera is facing downwards.
                newZoom.y = Mathf.Clamp(newZoom.y, maxZoomInAmount, minZoomInAmount);
                newZoom.z = Mathf.Clamp(newZoom.z, -minZoomInAmount, -maxZoomInAmount);
            }
            
            //Rotation Orbit
            if (Input.GetMouseButtonDown(2))
            {
                rotateStartPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(2))
            {
                rotateCurrentPosition = Input.mousePosition;
                
                Vector3 delta = rotateStartPosition - rotateCurrentPosition;
                
                newRotation *= Quaternion.Euler(Vector3.up * (-delta.x * rotationAmount));
                
                rotateStartPosition = rotateCurrentPosition;
            }
        }

        private void HandleKeyboardInput()
        {
            movementSpeed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : normalSpeed;

            if (Input.GetKey(KeyCode.W))
            {
                newPosition += transform.forward * movementSpeed;
            }

            if (Input.GetKey(KeyCode.S))
            {
                newPosition += transform.forward * -movementSpeed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                newPosition += transform.right * -movementSpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                newPosition += transform.right * movementSpeed;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                newRotation *= Quaternion.Euler(Vector3.up * (rotationAmount * 7f));
            }

            if (Input.GetKey(KeyCode.E))
            {
                newRotation *= Quaternion.Euler(Vector3.up * (-rotationAmount * 7f));
            }
        }
        
        private void HandleMovement()
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        }
    }
}