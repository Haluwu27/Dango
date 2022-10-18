using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera MainCam;
        [SerializeField] Camera EatCam;
        [SerializeField] LayerMask LayerMask;
        [SerializeField] EatCamCubeScript EatCam_PlayerCrevice;
        private GameObject Pl;
        bool Eat = false;
        void Start()
        {
            MainCam.gameObject.SetActive(true);
            EatCam.gameObject.SetActive(false);
            Pl = EatCam.transform.parent.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if (Eat)
            {
                for (int i = 0; i < EatCam_PlayerCrevice.objects.Count; i++)
                {
                    if (EatCam_PlayerCrevice.objects[i].activeSelf)
                        EatCam_PlayerCrevice.objects[i].SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < EatCam_PlayerCrevice.objects.Count; i++)
                {
                    if (!EatCam_PlayerCrevice.objects[i].activeSelf)
                        EatCam_PlayerCrevice.objects[i].SetActive(true);
                }
                EatCam_PlayerCrevice.objects.Clear();
            }
        }

        public void OnEat()
        {

            EatCam.gameObject.SetActive(true);
            MainCam.gameObject.SetActive(false);

            Eat = true;
            PlayerData.Event = true;
        }

        public void OffEat()
        {
            MainCam.gameObject.SetActive(true);
            EatCam.gameObject.SetActive(false);

            Eat = false;

            PlayerData.Event = false;
        }
    } 
