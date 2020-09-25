using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Windows.WebCam;
using UnityEngine.Events;
using System;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;



namespace Microsoft.MixedReality.Toolkit.Input
{
    public class videoCapture : MonoBehaviour
    {
        static readonly float MaxRecordingTime = 15.0f;
        private float time;
        float countSec = 0;
        static string timestamp;

        VideoCapture m_VideoCapture = null;
        float m_stopRecordingTimer = float.MaxValue;
        Camera cam;
        // Use this for initialization
        void Start()
        {
            cam = GameObject.Find("Main Camera").GetComponent<Camera>();
            Debug.Log("sto iniziando");
            timestamp = DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt").Replace(":", "-").Replace(" ", "_").Replace("/", "-");
            Debug.Log(timestamp);
            System.IO.File.WriteAllText(LogName(), String.Empty);
            StartVideoCaptureTest();

        }

        void Update()
        {
            var eyeGazeProvider = CoreServices.InputSystem?.EyeGazeProvider;

            Vector3 viewPos = cam.WorldToScreenPoint(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin +
                   CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized); // PASSO DALLE COORDINATE DEL GAZE(WORLD) AI PIXEL DELL'IMMAGINE

            time += Time.deltaTime;;
            if (time > 1)
            {
                System.IO.File.AppendAllText(LogName(), countSec.ToString() + "  " + "X: " + viewPos.x + "   " + "Y: " + viewPos.y + Environment.NewLine);
                countSec += 1;
                time = 0;
            }
            


            if (m_VideoCapture == null || !m_VideoCapture.IsRecording)
            {
                return;
            }

            if (Time.time > m_stopRecordingTimer)
            {
                m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
            }
        }


        void StartVideoCaptureTest()
        {
            Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            Debug.Log(cameraResolution);

            float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();
            Debug.Log(cameraFramerate);

            VideoCapture.CreateAsync(true, delegate (VideoCapture videoCapture)
            {
                if (videoCapture != null)
                {
                    m_VideoCapture = videoCapture;
                    Debug.Log("Created VideoCapture Instance!");

                    CameraParameters cameraParameters = new CameraParameters();
                    cameraParameters.hologramOpacity = 1f;
                    cameraParameters.frameRate = cameraFramerate;
                    cameraParameters.cameraResolutionWidth = cameraResolution.width;
                    cameraParameters.cameraResolutionHeight = cameraResolution.height;
                    cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

                    m_VideoCapture.StartVideoModeAsync(cameraParameters,
                        VideoCapture.AudioState.ApplicationAndMicAudio,
                        OnStartedVideoCaptureMode);
                }
                else
                {
                    Debug.LogError("Failed to create VideoCapture Instance!");
                }
            });
        }

        void OnStartedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
        {
            time = 0.0f;
            

            Debug.Log("Started Video Capture Mode!");
            //string timeStamp = Time.time.ToString().Replace(".", "").Replace(":", "");
            string filename = string.Format("Video_{0}.mp4", timestamp);
            string filepath = System.IO.Path.Combine(Application.persistentDataPath, filename);
            filepath = filepath.Replace("/", @"\");
            m_VideoCapture.StartRecordingAsync(filepath, OnStartedRecordingVideo);
        }

        void OnStoppedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
        {
            Debug.Log("Stopped Video Capture Mode!");
        }

        void OnStartedRecordingVideo(VideoCapture.VideoCaptureResult result)
        {
            Debug.Log("Started Recording Video!");
            m_stopRecordingTimer = Time.time + MaxRecordingTime;
        }

        void OnStoppedRecordingVideo(VideoCapture.VideoCaptureResult result)
        {
            Debug.Log("Stopped Recording Video!");
            m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
        }


        public static string LogName()
        {
            /*return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                                 Application.dataPath,
                                 width, height,
                                 System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));*/


            string filename = string.Format(@"Log_{0}.txt", timestamp);
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, filename);
            return filePath;

        }
    }

}