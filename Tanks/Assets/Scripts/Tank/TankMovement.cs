﻿/** 
 * @file TankMovement.cs
 * @brief Controls the movement and audio of a tank.
 */

using UnityEngine;

/** 
 * @namespace Complete
 * @brief Contains tank-related scripts for the complete tank game.
 */
namespace Complete
{
    /** 
     * @class TankMovement
     * @brief Controls the movement and audio of a tank.
     */
    public class TankMovement : MonoBehaviour
    {
        /** 
         * @brief Used to identify which tank belongs to which player. This is set by this tank's manager.
         */
        public int m_PlayerNumber = 1;

        public bool m_IsEnemy = false;

        /** 
         * @brief How fast the tank moves forward and back.
         */
        public float m_Speed = 12f;

        /** 
         * @brief How fast the tank turns in degrees per second.
         */
        public float m_TurnSpeed = 180f;

        /** 
         * @brief Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
         */
        public AudioSource m_MovementAudio;

        /** 
         * @brief Audio to play when the tank isn't moving.
         */
        public AudioClip m_EngineIdling;

        /** 
         * @brief Audio to play when the tank is moving.
         */
        public AudioClip m_EngineDriving;

        /** 
         * @brief The amount by which the pitch of the engine noises can vary.
         */
        public float m_PitchRange = 0.2f;

        /** 
         * @brief The right wheel of the tank.
         */
        public Transform m_rightWheel;
        public Transform m_rightBackWheel;

        /** 
         * @brief The left wheel of the tank.
         */
        public Transform m_leftWheel;
        public Transform m_leftBackWheel;
        



        private string m_MovementAxisName;
        private string m_TurnAxisName;
        private Rigidbody m_Rigidbody;
        private float m_MovementInputValue;
        private float m_TurnInputValue;
        private float m_OriginalPitch;
        private ParticleSystem[] m_particleSystems;
        private float m_maxWheelAngle = 45f;

        /** 
         * @brief Initializes the rigidbody component.
         */
        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        /** 
         * @brief Enables the tank and resets input values.
         */
        private void OnEnable()
        {
            // When the tank is turned on, make sure it's not kinematic.
            m_Rigidbody.isKinematic = false;

            // Also reset the input values.
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            // Start all particle systems.
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in m_particleSystems)
            {
                ps.Play();
            }
        }

        /** 
         * @brief Disables the tank and stops its movement.
         */
        private void OnDisable()
        {
            // When the tank is turned off, set it to kinematic so it stops moving.
            m_Rigidbody.isKinematic = true;

            // Stop all particle systems.
            foreach (ParticleSystem ps in m_particleSystems)
            {
                ps.Stop();
            }
        }

        /** 
         * @brief Initializes axis names and audio pitch.
         */
        private void Start()
        {
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;

            m_OriginalPitch = m_MovementAudio.pitch;
        }

        /** 
         * @brief Updates the tank's input and engine audio.
         */
        private void Update()
        {
            if (!m_IsEnemy)
            {
                m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
                m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

                EngineAudio();
            }
            else 
            {
                EnemyMovement();
            }
        }

        /** 
         * @brief Handles engine audio based on tank movement.
         */
        private void EngineAudio()
        {
            if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
            {
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
            else
            {
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }

        /** 
         * @brief Handles tank movement in FixedUpdate.
         */
        private void FixedUpdate()
        {
            Move();
            Turn();
        }

        /** 
         * @brief Moves the tank forward or backward and rotates the wheels accordingly (if moving).
         */
        private void Move()
        {
            // Calculate distance traveled by the tank in this frame
            float distance = m_MovementInputValue * m_Speed * Time.fixedDeltaTime;

            // Move the tank
            Vector3 movement = transform.forward * distance;
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

            // Calculate the rotational speed of each wheel based on its radius an current tank velocity
            float frontWheelRotateValue = (m_MovementInputValue * m_Speed / 0.085652f) * Time.fixedDeltaTime;
            float RearWheelRotateValue = (m_MovementInputValue * m_Speed / 0.122949f) * Time.fixedDeltaTime;

            Vector3 frontWheelRotation = new Vector3(frontWheelRotateValue, 0f, 0f);
            Vector3 RearWheelRotation = new Vector3(RearWheelRotateValue, 0f, 0f);

            m_leftWheel.Rotate(frontWheelRotation);
            m_rightWheel.Rotate(frontWheelRotation);
            m_leftBackWheel.Rotate(RearWheelRotation);
            m_rightBackWheel.Rotate(RearWheelRotation);
        }


        /** 
         * @brief Rotates the tank left or right.
         */
        private void Turn()
        {
            // Calculate turn angle based on input
            float targetWheelRotation = m_TurnInputValue * m_maxWheelAngle - 0.05f;

            // Smoothly rotate the wheels towards the target angle if there is input
            if (Mathf.Abs(m_TurnInputValue) > 0.0f)
            {
                Quaternion targetRotation = Quaternion.Euler(0f, targetWheelRotation, 0f);

                // Apply the smoothed rotation to both left and right wheels
                m_leftWheel.localRotation = Quaternion.RotateTowards(m_leftWheel.localRotation, targetRotation, m_TurnSpeed * Time.fixedDeltaTime);
                m_rightWheel.localRotation = Quaternion.RotateTowards(m_rightWheel.localRotation, targetRotation, m_TurnSpeed * Time.fixedDeltaTime);

                // Calculate turn angle for back wheels
                float targetBackWheelRotation = Mathf.Clamp(targetWheelRotation, -2.5f, 2.5f);
                Quaternion targetBackRotation = Quaternion.Euler(0f, targetBackWheelRotation, 0f);

                // Apply the smoothed rotation to both left and right back wheels
                m_leftBackWheel.localRotation = Quaternion.RotateTowards(m_leftBackWheel.localRotation, targetBackRotation, m_TurnSpeed * Time.fixedDeltaTime);
                m_rightBackWheel.localRotation = Quaternion.RotateTowards(m_rightBackWheel.localRotation, targetBackRotation, m_TurnSpeed * Time.fixedDeltaTime);
            }

            // Rotate the tank body
            if (Mathf.Abs(m_MovementInputValue) > 0.0f)
            {
                float turn = m_TurnInputValue * m_TurnSpeed * Time.fixedDeltaTime / 2.0f;
                Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
                m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
            }
        }

        private void EnemyMovement()
        {
        
        

        }

    }
}
