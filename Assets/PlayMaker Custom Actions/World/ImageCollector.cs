using System;
using System.Collections;
using System.IO;
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace neapolis.tarzerind.eu.actions
{
    /// <summary>
    ///     <author> Tar Zerind Kft. - Nea Polisz Game Development Division.</author>
    ///     <see cref="https://github.com/neapolisz/PlayMakerCustomActions_U5"/>
    /// </summary>
    [ActionCategory(ActionCategory.World)]
    [HutongGames.PlayMaker.Tooltip("This Action collects Image for any specified image CDN.")]
    public class ImageCollector : FsmStateAction
    {
        private Image InGameMenuBgImage;
        public FsmGameObject fsmImageHolder;
        public FsmString ImageShouldBeCollectedFromStreamingAssets;
        private UnityWebRequest _unityWebRequest;

        public override void OnEnter()
        {
            //DownloadImage(imageName.Value);
            ChangeImage();
            Finish();
        }
        
        public void DownloadImage(string url)
        {
            StartCoroutine(ImageRequest(url, (UnityWebRequest req) =>
            {
                if (req.isNetworkError || req.isHttpError)
                {
                    Debug.Log($"{req.error}: {req.downloadHandler.text}");
                } 
                else
                {
                    
                    //\\nimrod_nas\home\unity\Tankman Pilot Assets\Images
                    // Get the texture out using a helper downloadhandler
                    Texture2D texture = DownloadHandlerTexture.GetContent(req);
                    Debug.Log("Where Should be saved: " + Application.streamingAssetsPath + "/ingame-menu-bg-image.jpg");
                    ES3.SaveImage(texture, Application.streamingAssetsPath + "/ingame-menu-bg-image.jpg");
                    
                    
                    // Save it into the Image UI's sprite
                    InGameMenuBgImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    fsmImageHolder.Value.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    
                }
            }));
        }

        IEnumerator ImageRequest(string url, Action<UnityWebRequest> callback)
        {
            using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
            {
                yield return req.SendWebRequest();
                callback(req);
            }
        }
        private string GetFileLocation(string relativePath)
        {
            return "file://" + Path.Combine(Application.streamingAssetsPath, relativePath);
        }

        public void ChangeImage()
        {
            StartCoroutine(ChangeImageCoRoutine());
        }

        private IEnumerator ChangeImageCoRoutine()
        {
            using (_unityWebRequest = UnityWebRequestTexture.GetTexture(GetFileLocation(ImageShouldBeCollectedFromStreamingAssets.Value)))
            {
                yield return _unityWebRequest.SendWebRequest();
                if (_unityWebRequest.isNetworkError || _unityWebRequest.isHttpError)
                {
                }
                else
                {
                    fsmImageHolder.Value.GetComponent<RawImage>().texture =
                        DownloadHandlerTexture.GetContent(_unityWebRequest);
                }
            }
        }
    }

}