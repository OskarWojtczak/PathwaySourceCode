using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Serialization;

//extend the XRBaseInteractable class
public class TouchButton : XRBaseInteractable
    {
        //Make variables visible in Unity under "Visuals" heading
        [Header("Visuals")]
        public Material normalMaterial;
        public Material touchedMaterial;
        //Make variables visible in Unity under "Button Data" heading
        [Header("Button Data")]
        public int buttonNumber;
        public NumberPad linkedNumberpad;

        private int m_NumberOfInteractor = 0;
        private Renderer m_RendererToChange;

        //Render new Mesh at start of scene
        private void Start()
        {
            m_RendererToChange = GetComponent<MeshRenderer>();
        }

        //make interactable hoverable by XRDirectInteractor
        public override bool IsHoverableBy(IXRHoverInteractor interactor)
        {
            return base.IsHoverableBy(interactor) && (interactor is XRDirectInteractor);
        }

        //Change button colour and add number to entered code in Numberpad.cs when a number is hovered
        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);

            
            //statement makes sure only first hover entered (hand) can input number. Must be released before continuing
            if (m_NumberOfInteractor == 0)
            {
                m_RendererToChange.material = touchedMaterial;

                linkedNumberpad.ButtonPressed(buttonNumber);
            }

            m_NumberOfInteractor += 1;
        }

        //Reset m_NumberOfInteractor and button colour to normal 
        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);

            m_NumberOfInteractor -= 1;

            if (m_NumberOfInteractor == 0)
                m_RendererToChange.material = normalMaterial;
        }
    }
