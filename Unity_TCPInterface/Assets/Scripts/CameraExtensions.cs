using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CameraExtensions
 {
     /// [summary]
     /// The resulting value of z' is normalized between the values of -1 and 1, 
     /// where the near plane is at -1 and the far plane is at 1. Values outside of 
     /// this range correspond to points which are not in the viewing frustum, and 
     /// shouldn't be rendered.
     /// 
     /// See: http://en.wikipedia.org/wiki/Z-buffering
     /// [/summary]
     /// [param name="camera"]
     /// The camera to use for conversion.
     /// [/param]
     /// [param name="point"]
     /// The point to convert.
     /// [/param]
     /// [returns]
     /// A world point converted to view space and normalized to values between -1 and 1.
     /// [/returns]
     public static Vector3 WorldToNormalizedViewportPoint(this Camera camera, Vector3 point)
     {
         // Use the default camera matrix to normalize XY, 
         // but Z will be distance from the camera in world units
         point = camera.WorldToViewportPoint(point);
 
          if(camera.orthographic)
         {
             // Convert world units into a normalized Z depth value
             // based on orthographic projection
             point.z = (2 * (point.z - camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane)) - 1f;
         }
         else
         {
             // Convert world units into a normalized Z depth value
             // based on perspective projection
             point.z = ((camera.farClipPlane + camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane))
                 + (1/point.z) * (-2 * camera.farClipPlane * camera.nearClipPlane / (camera.farClipPlane - camera.nearClipPlane));
         }
         
         return point;
     }
     
     /// [summary]
     /// Takes as input a normalized viewport point with values between -1 and 1,
     /// and outputs a point in world space according to the given camera.
     /// [/summary]
     /// [param name="camera"]
     /// The camera to use for conversion.
     /// [/param]
     /// [param name="point"]
     /// The point to convert.
     /// [/param]
     /// [returns]
     /// A normalized viewport point converted to world space according to the given camera.
     /// [/returns]
     public static Vector3 NormalizedViewportToWorldPoint(this Camera camera, Vector3 point)
     {
         if(camera.orthographic)
         {
             // Convert normalized Z depth value into world units
             // based on orthographic projection
             point.z = (point.z + 1f) * (camera.farClipPlane - camera.nearClipPlane) * 0.5f + camera.nearClipPlane;
         }
         else
         {
             // Convert normalized Z depth value into world units
             // based on perspective projection
             point.z = ((-2 * camera.farClipPlane * camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane)) /
                 (point.z - ((camera.farClipPlane + camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane)));
         }
 
         // Use the default camera matrix which expects normalized XY but world unit Z 
         return camera.ViewportToWorldPoint(point);
     }
 }