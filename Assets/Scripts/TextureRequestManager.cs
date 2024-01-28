using System.Collections;
using System.Collections.Generic;
using Common;
using DesignPatterns;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
///     Schedules texture requests in a simple FIFO structure and serves them.
/// </summary>
public class TextureRequestManager : SafeSingleton<TextureRequestManager>
{
    public delegate void RequestHandler(GameObject customizable);
    public delegate void ErrorHandler(string error);
    public event RequestHandler TextureRequestStartEvent;
    public event RequestHandler TextureRequestCompletionEvent;
    public event ErrorHandler ErrorEvent;

    // a queue data structure to hold texture requests
    Queue<RequestData> requestQueue = new Queue<RequestData>();
    UnityWebRequest currentlyRunningWebRequest;
    bool isCurrentlyServingTextureRequests = false;

    /// <summary>
    ///     Adds a texture request to the queue of texture requests waiting to be served
    /// </summary>
    /// <param name="customizable">customizable GameObject that the requested texture will be applied to</param>
    /// <param name="url">https link from where to fetch the requested texture</param>
    public void AddRequest(RequestData requestData)
    {
        if (!requestQueue.Contains(requestData))
        {
            requestQueue.Enqueue(requestData);
            return;
        }
        Debug.Log("Texture request already exists in queue!");
    }

    /// <summary>
    ///     Suspends texture request serving routine. Use Continue() to continue serving texture requests from
    ///     where it left off.
    ///     This function does nothing in case it is called when texture request serving routine isn't running.
    /// </summary>
    public void Suspend()
    {
        if (isCurrentlyServingTextureRequests)
        {
            isCurrentlyServingTextureRequests = false;
            TextureRequestCompletionEvent -= OnTextureRequestCompletion;
            StopAllCoroutines();
            if (currentlyRunningWebRequest != null)
            {
                currentlyRunningWebRequest.Dispose();
                currentlyRunningWebRequest = null;
            }
            return;
        }
    }

    /// <summary>
    ///     Aborts serving texture requests. Call this to stop serving texture requests and clear all requests.
    /// </summary>
    public void Abort()
    {
        ClearRequests();
        Suspend();
    }

    /// <summary>
    ///     Continue previously suspended request serving routine.
    ///     This function does nothing in case it is called when texture request serving routine is running
    /// </summary>
    public void Continue()
    {
        if (!isCurrentlyServingTextureRequests)
        {
            StartServingTextureRequests();
            isCurrentlyServingTextureRequests = true;
            return;
        }
    }

    /// <summary>
    ///     Returns the download progress of the currently running texture request.
    ///     Make sure to call this function in a method subscribed to TextureRequestStartEvent.
    /// </summary>
    /// <returns>download progress between 0.00 and 1.00</returns>
    public float GetDownloadProgress()
    {
        if (currentlyRunningWebRequest != null)
            return currentlyRunningWebRequest.downloadProgress;
        return 1.0f;
    }

    /// <summary>
    ///     Removes all texture requests from the queue
    /// </summary>
    public void ClearRequests()
    {
        requestQueue.Clear();
    }

    /// <summary>
    ///     Use this to know the number of requests still waiting to be served
    /// </summary>
    /// <returns>The number of texture requests waiting to be served</returns>
    public int GetRemainingRequests()
    {
        return requestQueue.Count;
    }

    /// <summary>
    ///     Starts the set of coroutines requesting textures synchronously; only one coroutine at a time.
    ///     Make sure to call this after adding all texture requests to the queue.
    /// </summary>
    public void StartServingTextureRequests()
    {
        // checks if there are any texture requests in the queue waiting to be served
        if (requestQueue.Count == 0)
        {
            //Debug.Log("Make sure to queue some coroutines before calling StartCoroutinesSychronously.");
            return;
        }
        // checks if serving texture requests is already running
        if (isCurrentlyServingTextureRequests)
            return;
        isCurrentlyServingTextureRequests = true;
        TextureRequestCompletionEvent += OnTextureRequestCompletion;
        OnTextureRequestCompletion();
    }

    /// <summary>
    ///     Downloads the texture from <paramref name="url"/> and assigns it as albedo texture to <paramref name="customizableObject"/>
    ///     's material
    /// </summary>
    /// <param name="customizableObject">GameObject that its albedo texture will be changed</param>
    /// <param name="url">Download link for a .png texture</param>
    /// <returns></returns>
    IEnumerator ServeTextureRequest(RequestData requestData)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(requestData.url, true))
        {
            currentlyRunningWebRequest = uwr;
            if (TextureRequestStartEvent != null)
                TextureRequestStartEvent.Invoke(requestData.Customizable);
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                ErrorEvent.Invoke(uwr.error);
            }
            else
            {
                // notify user through UI about success of downloading
                // get and assign downloaded textur
                if (requestData.isComponentImgaeUI)
                {
                    Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                    requestData.Customizable.GetComponent<Image>().sprite = Sprite.Create(
                        tex,
                        new Rect(0.0f, 0.0f, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f)
                    );
                }
                else if (!requestData.shareableMaterial)
                    requestData.Customizable.GetComponent<Renderer>().material.mainTexture =
                        DownloadHandlerTexture.GetContent(uwr);
                else
                    requestData.Customizable.GetComponent<Renderer>().sharedMaterial.mainTexture =
                        DownloadHandlerTexture.GetContent(uwr);
            }
            currentlyRunningWebRequest = null;
        }
        requestQueue.Dequeue();
        TextureRequestCompletionEvent.Invoke(requestData.Customizable);
    }

    /// <summary>
    ///     This will be called when the coroutine ServeTextureRequest has completed excecution.
    /// </summary>
    void OnTextureRequestCompletion(GameObject customizable = null)
    {
        StopAllCoroutines();
        if (requestQueue.Count != 0)
            StartCoroutine(ServeTextureRequest(requestQueue.Peek()));
        else
        {
            isCurrentlyServingTextureRequests = false;
            TextureRequestCompletionEvent -= OnTextureRequestCompletion;
        }
    }
}
