using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Reloads the ARSession by first destroying the ARSession's GameObject
/// and then instantiating a new ARSession from a Prefab.
/// </summary>
public class SessionReloader : MonoBehaviour
{
    public ARSession session;
    public GameObject sessionPrefab;
    public ARSessionOrigin sessionOrigin;
    public Button pauseButton;
    public Button resumeButton;
    public Button resetButton;

    private GameObject placedPrefab;

    public void ReloadSession()
    {
        PlaceOnPlane place = sessionOrigin.GetComponent<PlaceOnPlane>();
        placedPrefab = place.spawnedObject;

        if (session != null)
        {
            StartCoroutine(DoReload());
        }
    }

    IEnumerator DoReload()
    {
        Destroy(session.gameObject);
        yield return null;

        if (sessionPrefab != null)
        {
            session = Instantiate(sessionPrefab).GetComponent<ARSession>();

            // Hook the buttons back up
            resetButton.onClick.AddListener(session.Reset);
            resetButton.onClick.AddListener(DestroyPrefab);
            pauseButton.onClick.AddListener(() => { session.enabled = false; });
            resumeButton.onClick.AddListener(() => { session.enabled = true; });
        }

    }

    public void DestroyPrefab()
    {
        Destroy(placedPrefab);
        Debug.Log("Destroyed");
    }
}
