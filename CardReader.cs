using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Serialization;

//extend the XRSocketInteractor class
public class CardReader : XRSocketInteractor
    {
        //Make variables visible in Unity under "CardReader ReaderOptions Data" heading
        [Header("CardReader ReaderOptions Data")]
        public float allowedUprightErrorRange = 0.2f;

        //Make variables visible in Unity under "Success References" heading
        [Header("Success References")]
        public GameObject visualLockToHide;
        public MonoBehaviour handleToEnable;

        private Vector3 m_HoverEntry;
        private bool m_SwipIsValid;

        private Transform m_KeycardTransform;

        //Disable selection
        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            return false;
        }

        //Only Keycard objects can hover
        public override bool CanHover(IXRHoverInteractable interactable)
        {
            return interactable is Keycard;
        }

        //when hover is entered, store key card position information 
        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);

            m_KeycardTransform = args.interactableObject.transform;
            m_HoverEntry = m_KeycardTransform.position;
            m_SwipIsValid = true;
        }

        //When hover is exited
        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);

            //create vector from card entry to exit position
            Vector3 entryToExit = m_KeycardTransform.position - m_HoverEntry;

            //if swipe is valid and vector was at least 0.15 unit long in the y direction, unlock door
            if (m_SwipIsValid && entryToExit.y < -0.15f)
            {
                visualLockToHide.gameObject.SetActive(false);
                handleToEnable.enabled = true;
            }

            //resset m_KeycardTransform to stop if statement in Update()
            m_KeycardTransform = null;
        }

        private void Update()
        {
            //runs only when card is hovering
            if (m_KeycardTransform != null)
            {
                //Store current Keycard 'up' vector
                Vector3 keycardUp = m_KeycardTransform.forward;

                //Take dot product of Keycard up vector and true 'up'
                float dot = Vector3.Dot(keycardUp, Vector3.up);

                //If Keycard up and true up are perfectly parallel as intended. Dot product = 1. 
                //If calculated dot is bellow 1 -  error range invalidate the swipe 
                if (dot < 1 - allowedUprightErrorRange)
                {
                    m_SwipIsValid = false;
                }
            }
        }
    }