using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class MouseLook
    {
        public Input inp;
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;

        public float bobAmplitude = 0.015f;
        public float bobFrequency = 10f;
        public float bobToggleSpeed = 3.0f;
        public Transform cameraTransform;
        public Transform holderTransform;
        public Vector3 bobStartPosition;
        public RigidbodyFirstPersonController player;
        public Rigidbody playerRb;


        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = true;

        public void Init(Transform character, Transform camera)
        {
            inp = new Input();
            inp.Player.Enable();
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
            bobStartPosition = camera.transform.localPosition;
        }

        public Vector3 FootStepMotion() {
            Vector3 pos = Vector3.zero;
            pos.y = MathF.Sin(Time.time * bobFrequency) * bobAmplitude;
            pos.x = MathF.Cos(Time.time * bobFrequency / 2) * bobAmplitude * 2;
            return pos;
        }

        public void CheckMotion() {
            float speed = new Vector3(playerRb.velocity.x, 0 , playerRb.velocity.z).magnitude;

            if (speed < bobToggleSpeed) return;
            if (!player.m_IsGrounded && !player.m_WallRunning) return;
            PlayMotion(FootStepMotion());
        }

        void PlayMotion(Vector3 motion){
            holderTransform.localPosition += motion; 
            float wallRunAngle = player.m_WallRunning ? (player.m_WallRunDirection * 12) : 0;
            Debug.Log(wallRunAngle);
            Quaternion newRotation = Quaternion.Euler(0, 0, wallRunAngle);
            holderTransform.localRotation = Quaternion.Slerp(holderTransform.localRotation, newRotation, Time.deltaTime * 10);
        }

        void ResetPosition() {
            if (cameraTransform.localPosition != bobStartPosition) {
                cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, bobStartPosition, Time.deltaTime);
            }
        }


        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = inp.Player.Look.ReadValue<Vector2>().x * XSensitivity;
            float xRot = inp.Player.Look.ReadValue<Vector2>().y * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

            if(clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

            if(smooth)
            {
                character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
            CheckMotion();
            ResetPosition();
            UpdateCursorLock();
        }

        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if(!lockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
                InternalLockUpdate();
        }

        private void InternalLockUpdate()
        {
            if(inp.Player.Escape.WasPerformedThisFrame())
            {
                m_cursorIsLocked = false;
            }
            else if(inp.Player.Fire.WasPerformedThisFrame())
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

    }
}
