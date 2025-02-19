﻿using System;
using UnityEngine;
using Vuforia;

public class DeployStageOnce : MonoBehaviour {

	public GameObject AnchorStage;
	private PositionalDeviceTracker _deviceTracker;
	private GameObject _previousAnchor;
	public static string theRoute;

	public void Start ()
	{
		if (AnchorStage == null)
		{
			Debug.Log("AnchorStage must be specified");
			return;
		}

		AnchorStage.SetActive(false);
	}

	public void Awake()
	{
		VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
	}

	public void OnDestroy()
	{
		VuforiaARController.Instance.UnregisterVuforiaStartedCallback(OnVuforiaStarted);
	}

	private void OnVuforiaStarted()
	{
		_deviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
	}

	public void OnInteractiveHitTest(HitTestResult result)
	{
		if (result == null || AnchorStage == null)
		{
			Debug.LogWarning("Hit test is invalid or AnchorStage not set");
			return;
		}
 
		var anchor = _deviceTracker.CreatePlaneAnchor(Guid.NewGuid().ToString(), result);

        GameObject anchorGO = new GameObject();

        anchorGO.transform.position = result.Position;

        anchorGO.transform.rotation = result.Rotation;

		if (anchor != null)
		{
            AnchorStage.transform.parent = anchorGO.transform;

            AnchorStage.transform.localPosition = Vector3.zero;

            AnchorStage.transform.localRotation = Quaternion.identity;

            AnchorStage.SetActive(true);
		}

		theRoute = "/" + anchorGO.name + "/" + AnchorStage.name;

		if (_previousAnchor != null)
		{
			Destroy(_previousAnchor);
		}

		_previousAnchor = anchorGO;
	}
}