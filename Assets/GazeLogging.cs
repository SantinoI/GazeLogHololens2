using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Serialization;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace Microsoft.MixedReality.Toolkit.Input
{
    public class GazeLogging : MonoBehaviour
    {
        // Start is called before the first frame update

        private int currentscore = 0;
        private GameObject s;
        private GameObject c1;
        public TextMeshPro Origin;
        public TextMeshPro Direction;
        public TextMeshPro X_2D;
        public TextMeshPro Y_2D;

        private float time = 0.0f;
        private float global_time = 0.0f;
        public float interpolationPeriod = 2;

        public Transform target;
        Camera cam;
        void Start()
        {
            s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            s.gameObject.transform.localScale = new Vector3(0.03F, 0.03F, 0.03F);
            s.GetComponent<MeshRenderer>().material.color = Color.red;


            Origin = GameObject.Find("Origin").GetComponent<TextMeshPro>();
            Direction = GameObject.Find("Direction").GetComponent<TextMeshPro>();
            X_2D = GameObject.Find("X_2D").GetComponent<TextMeshPro>();
            Y_2D = GameObject.Find("Y_2D").GetComponent<TextMeshPro>();

            cam = GameObject.Find("Main Camera").GetComponent<Camera>();



            //pallino giallo
            c1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            c1.gameObject.transform.localScale = new Vector3(0.03F, 0.03F, 0.03F);
            c1.GetComponent<MeshRenderer>().material.color = Color.yellow;


            //Al momento i file di log sono sempre resettati ad ogni avvio

            //System.IO.File.WriteAllText(@"./Assets/Log/log.txt", "");  //SVUOTO IL FILE DEI LOG

        }

        public static string ScreenShotName(int width, int height)
        {
            return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                                 Application.dataPath,
                                 width, height,
                                 System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
                                 

            string filename = string.Format(@"CapturedImage{0}_n.jpg", Time.time);
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, filename);
            return filePath;

        }
        public static string LogName()
        {
            /*return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                                 Application.dataPath,
                                 width, height,
                                 System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));*/
                                 

            string filename = string.Format(@"Log.txt");
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, filename);
            return filePath;

        }

        // Update is called once per frame
        void Update()
        {
            var eyeGazeProvider = CoreServices.InputSystem?.EyeGazeProvider;
            if (eyeGazeProvider != null && eyeGazeProvider.UseEyeTracking && eyeGazeProvider.IsEyeGazeValid)
            {

                //Debug.Log(eyeGazeProvider.GazeDirection.normalized);
                //Debug.Log("Gaze is looking in direction: " + eyeGazeProvider.GazeDirection);
                //Debug.Log("Gaze origin is: " + eyeGazeProvider.GazeOrigin);
            }
            s.transform.position =
                CoreServices.InputSystem.EyeGazeProvider.GazeOrigin +
                CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized;

            c1.transform.position = cam.ViewportToWorldPoint(new Vector3(1, 1, 1));// PASSO DALLE COORDINATE VIEW PORT(DOVE 1,1 è IN ALTO A DX A QUELLE DELLA CAMERA

            Origin.text = "Gaze Origin: " + CoreServices.InputSystem.EyeGazeProvider.GazeOrigin;
            Direction.text = "Gaze Direction: " + CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized;

            Vector3 viewPos = cam.WorldToScreenPoint(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin +
                CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized); // PASSO DALLE COORDINATE DEL GAZE(WORLD) AI PIXEL DELL'IMMAGINE

            X_2D.text = "X (2D): " + (viewPos.x * 1);
            Y_2D.text = "Y (2D): " + (viewPos.y * 1);

            Vector3 posPallinoGiallo = cam.WorldToScreenPoint(c1.transform.position);

            //Debug.Log(posPallinoGiallo);
            //Debug.Log(cam.pixelWidth,cam.pixelHeight)
            //Debug.Log("Risoluzione camera da Unity"+cam.pixelWidth+" "+ cam.pixelHeight);

            //time += Time.deltaTime;
            //global_time += Time.deltaTime;

            // QUESTA PARTE ABILITA GLI SCREEN E I LOG 

            /*if (time >= 6){
                time = 0.0f;

                Debug.Log("Faccio uno screen");
                int resWidth = (int)posPallinoGiallo.x;
                int resHeight = (int)posPallinoGiallo.y;
                RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
                cam.targetTexture = rt;
                Texture2D screenShot = new Texture2D((int) posPallinoGiallo.x, (int) posPallinoGiallo.y, TextureFormat.RGB24, false);
                cam.Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
                cam.targetTexture = null;
                RenderTexture.active = null; // JC: added to avoid errors
                Destroy(rt);
                byte[] bytes = screenShot.EncodeToPNG();
                string filename = ScreenShotName(resWidth, resHeight);

                System.IO.File.WriteAllBytes(filename, bytes);
                Debug.Log(string.Format("Took screenshot to: {0}", filename));
            } */

            
            //System.IO.File.AppendAllText(LogName(), global_time.ToString()+"  "+"X: "+ viewPos.x+"   "+"Y: "+ viewPos.y + Environment.NewLine);
            

            /*
            
             World Space Point
                A world space point is defined in global coordinates (for example, Transform.position). It is the position of the object in world or global space. 

                Unity in-built functions
                Unity offers in-built functions to inter-convert world point, viewport point and space points into each other.

                Camera.ScreenToWorldPoint()
                Transforms position from screen space into world space.

                Camera.ScreenToViewportPoint()
                Transforms position from screen space into viewport space.

                Camera.ViewportToScreenPoint()
                Transforms position from viewport space into screen space.

                Camera.ViewportToWorldPoint()
                Transforms position from viewport space into world space

                Camera.WorldToScreenPoint()
                Transforms position from world space into screen space.

                Camera.WorldToViewportPoint()
                Transforms position from world space into viewport space.*/
        }
    }
}