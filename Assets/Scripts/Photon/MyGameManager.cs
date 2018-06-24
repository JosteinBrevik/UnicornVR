﻿using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


namespace Com.DefaultCompany.UnicornVR
{
    public class MyGameManager : Photon.PunBehaviour
    {

        static public MyGameManager Instance;
        static public string ROOM_SCENCE_NAME = "straightPathsLevel";
        public float minX = -2f, maxX = 2f;
        private Vector3 position;


        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        void Start()
        {
            Instance = this;

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.Log("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
                    position = new Vector3(UnityEngine.Random.Range(minX, maxX), 2, 1);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, position, Quaternion.identity, 0);
                    //PlayerManager.LocalPlayerInstance.tag = PhotonNetwork.isMasterClient ? "Playe1" : "Player2";

                }
                else
                {
                    Debug.Log("Ignoring scene load for " + Application.loadedLevelName);
                }
            }
        }

        #region Photon Messages


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            if (PhotonNetwork.room != null && PhotonNetwork.room.PlayerCount == 2 && PhotonNetwork.isMasterClient)
                SceneManager.LoadScene(ROOM_SCENCE_NAME); // TODO ROOM_SCENCE_NAME?
            else
                Invoke("OnLeftRoom", 1);
        }


        #endregion


        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods


        void LoadArena()
        {
            if (!PhotonNetwork.isMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
            PhotonNetwork.LoadLevel(ROOM_SCENCE_NAME); //holds scence value
        }


        #endregion

        #region Photon Messages


        public override void OnPhotonPlayerConnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected

                LoadArena();
            }
        }


        public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects


            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerDisonnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


                //LoadArena();
            }
        }

        #endregion
    }
}