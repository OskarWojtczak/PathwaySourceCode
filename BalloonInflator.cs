using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//extend the XRGrabInteractable class
public class BalloonInflator : XRGrabInteractable
{

    //Make variables visible in Unity under "Balloon Data" heading
    [Header("Balloon Data")]
    public Transform attachPoint;
    public Balloon balloonPrefab;

    private Balloon m_BalloonInstance; //holds new instances of balloonPrefab
    private XRBaseController m_Controller;


    //create balloon prefab instance at inflator attach point and retrieve controller input data
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args); //base. used to call parent method to retain other functionality
        m_BalloonInstance = Instantiate(balloonPrefab, attachPoint);

        //retrieve and assign m_Controller with which controller is being used to pick up inflator
        var controllerInteractor = args.interactorObject as XRBaseControllerInteractor;
        m_Controller = controllerInteractor.xrController;
    }

    //Destroy balloon prefab when inflator is dropped
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        Destroy(m_BalloonInstance.gameObject); 
    }

    //called every frame. Inflates balloon in proportion to button press if object is selected and m_Controller has a value
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (isSelected && m_Controller != null) 
        {
            m_BalloonInstance.transform.localScale = Vector3.one * Mathf.Lerp(1.0f, 4.0f, m_Controller.activateInteractionState.value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
