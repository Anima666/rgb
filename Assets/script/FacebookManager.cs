using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System.IO;

public class FacebookManager : MonoBehaviour
{
    public GameObject share;
    public Text result_game;
    private void Awake()
    {
        print("kek");
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
        else
        {
            FB.ActivateApp();
        }


    }

    public void LogIn()
    {
        FB.LogInWithPublishPermissions(callback: OnLogIn);
    }
    public void Share()
    {
          FB.LogInWithPublishPermissions(callback: OnLogIn, permissions: new List<string> { "publish_actions" });
        
    }

    private void OnLogIn(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            StartCoroutine(ShareImageShot());
            AccessToken token = AccessToken.CurrentAccessToken;
            //userIdText.text = token.UserId;
        }
        else
            print("Canceled  Login");
    }
    private void OnShare(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("error: " + result.Error);
        }
        else

        if (!string.IsNullOrEmpty(result.PostId))
        {
            Debug.Log(result.PostId);
            share.SetActive(false);
            result_game.text = "Win";
            result_game.color = Color.yellow;
            Debug.Log("Share succeed");
        }

        else
        {
           
        }
    }

    IEnumerator ShareImageShot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenTexture = new Texture2D(Screen.width, 500, TextureFormat.RGB24, true);

        screenTexture.ReadPixels(new Rect(0f, Screen.height - 500, Screen.width, Screen.height), 0, 0);

        screenTexture.Apply();

        byte[] dataToSave = screenTexture.EncodeToPNG();

        string destination = Path.Combine(Application.persistentDataPath, "xz");
        print(destination);
        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", dataToSave, "InteractiveConsole.png");
        File.WriteAllBytes(destination, dataToSave);
        FB.API("me/photos", HttpMethod.POST, Callback, wwwForm);
    }
    
    void Callback(IGraphResult result)
    {
       
        if (result.Error != null)
        {
           
        }
        else
        {
            string resultraw = result.RawResult;
            string id= resultraw.Substring(7, resultraw.IndexOf(',') - 8);

            FB.ShareLink(
                contentTitle: "Ваш счет",
                contentURL: new System.Uri("https://www.facebook.com/photo.php?fbid=" + id),
                contentDescription: "очень классная игра",
                callback: OnShare);

        }
    }

}