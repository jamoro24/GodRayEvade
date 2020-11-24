/*
 * Written by Johannes Schirm
 * Nara Institute of Science and Technology
 * Cybernetics and Reality Engineering Laboratory
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.PlayerLoop;
using ViveSR.anipal.Eye;
using SREye = ViveSR.anipal.Eye.SRanipal_Eye_API;
using SREyeData = ViveSR.anipal.Eye.EyeData_v2;

/// <summary>
/// Custom interface to access eye tracking data from SRanipal without callback.
/// The idea is to centralize as much specifics of the actual API as possible here.
/// </summary>
public class EyeTracker : MonoBehaviour
{
	/// <summary>
	/// Identifies different parts of the available eye tracking data.
	/// Left and right means the corresponding eye from the user's view.
	/// Combined is an API-provided combination of both eyes.
	/// </summary>
	public enum Source
	{
		Right, Left, Combined
	}

	/// <summary>
	/// 0.0 for almost no sensitivity and 1.0 for highest sensitivity.
	/// Too high sensitivity might result in too fluctuating data.
	/// Lower sensitivity leads to increasingly more delays.
	/// </summary>
	[Range(0.0f, 1.0f)]
	public double sensitivity = 0.75;

	/// <summary>
	/// The place for the API to write new eye tracking data to.
	/// </summary>
	private SREyeData eyeData;

	void Start()
	{
		// Set the sensitivity once at startup.
		EyeParameter ep;
		ep.gaze_ray_parameter.sensitive_factor = sensitivity;
		SREye.SetEyeParameter(ep);
	}

	void Update()
	{
		// Retrieve new eye tracking data each frame and store it in the member variable.
		// It is recommended to place the eye tracker at the top of the update order.
		ViveSR.Error e = SREye.GetEyeData_v2(ref eyeData);
		if (e != 0)
		{
			string eName = Enum.GetName(typeof(ViveSR.Error), e);
			Debug.LogError("[EyeTracker] Error " + eName + " occurred while retrieving data.");
		}
	}

	/// <summary>
	/// Builds a Unity ray in world space which represents the gaze ray of the given source.
	/// </summary>
	/// <param name="source">The eye data source to build the ray from.</param>
	/// <returns>A ray in Unity world space representing the gaze of the given source.</returns>
	public Ray GetRay(Source source = Source.Combined)
	{
		return GetWorldRay(ref GetEyeData(source));
	}

	/// <summary>
	/// Returns a value between 0.0f (closed) and 1.0f (opened) representing eye openness of the given source.
	/// The value returned for the combined source will be the average of the values for both eyes.
	/// </summary>
	/// <param name="source">The eye data source to determine eye openness from.</param>
	/// <returns>Eye openness of the given source on a smooth floating point scale.</returns>
	public float GetEyeOpenness(Source source = Source.Combined)
	{
		if (source == Source.Right)
		{
			return this.eyeData.verbose_data.right.eye_openness;
		}
		else if (source == Source.Left)
		{
			return this.eyeData.verbose_data.left.eye_openness;
		}
		else
		{
			// Just return the average for the combined source!
			return (this.eyeData.verbose_data.right.eye_openness + this.eyeData.verbose_data.left.eye_openness) / 2.0f;
		}
	}

	/// <summary>
	/// Calculates the user's current focus point in world space by intersecting the individual gaze rays.
	/// Please make sure to interpret the results of this function correctly, as its accuracy can fluctuate.
	/// Especially when focusing something more than 3 meters away, the focus point is difficult to determine.
	/// </summary>
	/// <returns>The current focus point of the user in Unity world space, based on the individual gaze rays.</returns>
	public Vector3 GetFocusPoint()
	{
		// Calculate the closest point to the other gaze ray for both of them.
		Ray rightEyeRay = GetRay(Source.Right);
		Ray leftEyeRay = GetRay(Source.Left);
		Vector3 closestPointRight, closestPointLeft;
		ClosestPointsOnTwoLines(out closestPointRight, out closestPointLeft, rightEyeRay, leftEyeRay);
		// They often do not actually intersect, so calculate the point inbetween.
		return Vector3.Lerp(closestPointLeft, closestPointRight, 0.5f);
	}

	/// <summary>
	/// Returns the corresponding (single) eye data from the current eye data.
	/// Eye data is updated once each frame through the Update method.
	/// </summary>
	/// <param name="source">The eye data source to return the data from.</param>
	/// <returns>A reference to the (single) eye data indicated by the given source.</returns>
	private ref SingleEyeData GetEyeData(Source source)
	{
		if (source == Source.Right)
		{
			return ref this.eyeData.verbose_data.right;
		}
		else if (source == Source.Left)
		{
			return ref this.eyeData.verbose_data.left;
		}
		else
		{
			return ref this.eyeData.verbose_data.combined.eye_data;
		}
	}

	/// <summary>
	/// Extracts a Unity ray from the eye data for a single eye, representing its gaze ray.
	/// </summary>
	/// <param name="singleEyeData">The (single) eye data to build the ray from.</param>
	/// <returns>A ray in Unity coordinates, starting at the gaze origin, pointing into the gaze direction.</returns>
	private static Ray GetWorldRay(ref SingleEyeData singleEyeData)
	{
		// We receive the origin and direction in a right-handed coordinate system, but Unity is left-handed.
		Vector3 origin = singleEyeData.gaze_origin_mm * 0.001f;
		origin.x *= -1;
		Vector3 direction = singleEyeData.gaze_direction_normalized;
		direction.x *= -1;
		return new Ray(Camera.main.transform.TransformPoint(origin), Camera.main.transform.TransformDirection(direction));
	}

	/// <summary>
	/// Two non-parallel rays which may or may not touch each other each have a point on them at which the other ray is closest.
	/// This function finds those two points. If the rays are not parallel, the function outputs true, otherwise false.
	/// Source: http://wiki.unity3d.com/index.php/3d_Math_functions
	/// </summary>
	/// <param name="closestPointRay1">An out parameter to store the resulting point on the first ray in.</param>
	/// <param name="closestPointRay2">An out parameter to store the resulting point on the second ray in.</param>
	/// <param name="ray1">The first input ray.</param>
	/// <param name="ray2">The second input ray.</param>
	/// <returns>Whether the two input rays are parallel.</returns>
	private static bool ClosestPointsOnTwoLines(out Vector3 closestPointRay1, out Vector3 closestPointRay2, Ray ray1, Ray ray2)
	{
		// TODO: Maybe move to a central place more related to general Unity vector math..?
		float a = Vector3.Dot(ray1.direction, ray1.direction);
		float b = Vector3.Dot(ray1.direction, ray2.direction);
		float e = Vector3.Dot(ray2.direction, ray2.direction);
		float d = a * e - b * b;

		if (d != 0.0f)
		{
			// The two lines are not parallel.
			Vector3 r = ray1.origin - ray2.origin;
			float c = Vector3.Dot(ray1.direction, r);
			float f = Vector3.Dot(ray2.direction, r);

			float s = (b * f - c * e) / d;
			float t = (a * f - c * b) / d;

			closestPointRay1 = ray1.origin + ray1.direction * s;
			closestPointRay2 = ray2.origin + ray2.direction * t;

			return true;
		}
		else
		{
			closestPointRay1 = Vector3.zero;
			closestPointRay2 = Vector3.zero;

			return false;
		}
	}
}
