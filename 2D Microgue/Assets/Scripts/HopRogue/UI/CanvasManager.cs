using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HopRogue
{
public enum CanvasType
{
    GameUI,
    StartMenu,
    OptionsMenu,
    UpgradeMenu
}

public class CanvasManager : MonoBehaviour
{
    List<CanvasController> canvasControllerList;
    CanvasController lastActiveCanvas;

    [SerializeField] CanvasType defaultCanvasType;

    void Awake()
    {
        canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();
        canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
    }

    void Start() => SwitchCanvas(defaultCanvasType);

    public void SwitchCanvas(CanvasType _type)
    {
        if (lastActiveCanvas != null)
            lastActiveCanvas.gameObject.SetActive(false);

        CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == _type);

        if (desiredCanvas != null)
        {
            desiredCanvas.gameObject.SetActive(true);
            lastActiveCanvas = desiredCanvas;
        }

        else 
            Debug.LogWarning("The desired canvas was not found!");
    }
}
}