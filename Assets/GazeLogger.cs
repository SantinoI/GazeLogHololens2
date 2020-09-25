using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Serialization;
using System.Collections;
using UnityEngine.UI;


namespace Microsoft.MixedReality.Toolkit.Input {
    public class GazeLogger : MonoBehaviour {
        // Start is called before the first frame update
      
        private int currentscore = 0;
        private GameObject s;
        public Text Origin;
        public Text Direction;
        public Text X_2D;
        public Text Y_2D;

        public Transform target;
        Camera cam;


        void Start()
        {
            s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            s.gameObject.transform.localScale = new Vector3(0.03F, 0.03F, 0.03F);
            s.GetComponent<MeshRenderer>().material.color = Color.red;


            Origin = GameObject.Find("Origin").GetComponent<Text>();
            Direction = GameObject.Find("Direction").GetComponent<Text>();
            X_2D = GameObject.Find("X_2D").GetComponent<Text>();
            Y_2D = GameObject.Find("Y_2D").GetComponent<Text>();

            cam = GameObject.Find("Main Camera").GetComponent<Camera>();



        }
       
        // Update is called once per frame
        void Update() {
            
            var eyeGazeProvider = CoreServices.InputSystem?.EyeGazeProvider;
            if (eyeGazeProvider != null && eyeGazeProvider.UseEyeTracking && eyeGazeProvider.IsEyeGazeValid) {

                //Debug.Log(eyeGazeProvider.GazeDirection.normalized);
                //Debug.Log("Gaze is looking in direction: " + eyeGazeProvider.GazeDirection);
                //Debug.Log("Gaze origin is: " + eyeGazeProvider.GazeOrigin);
            }
            s.transform.position =
                CoreServices.InputSystem.EyeGazeProvider.GazeOrigin +
                CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized;

            Origin.text = "Gaze Origin: " + CoreServices.InputSystem.EyeGazeProvider.GazeOrigin.ToString();
            Direction.text = "Gaze Direction: " + CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized.ToString();

            Vector3 viewPos = cam.WorldToViewportPoint(CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized);

            X_2D.text = "X (2D): " + viewPos.x.ToString();
            Y_2D.text = "Y (2D): " + viewPos.y.ToString();
            //Debug.Log(viewPos);
            Debug.Log(cam.pixelWidth);





            //s.transform.position = new Vector3(eyeGazeProvider.GazeDirection[0], eyeGazeProvider.GazeDirection[1], eyeGazeProvider.GazeDirection[2]);
        }
    }
}

