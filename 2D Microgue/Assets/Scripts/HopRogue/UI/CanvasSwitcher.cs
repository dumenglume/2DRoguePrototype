using UnityEngine;
using UnityEngine.UI;

namespace HopRogue
{
[RequireComponent(typeof(Button))]
public class CanvasSwitcher : MonoBehaviour
{
    public CanvasType desiredCanvasType;

    CanvasManager canvasManager;
    Button menuButton;

    private void Start()
    {
        menuButton = GetComponent<Button>();
        menuButton.onClick.AddListener(OnButtonClicked);
        // canvasManager = CanvasManager.GetInstance();
    }

    void OnButtonClicked()
    {
        canvasManager.SwitchCanvas(desiredCanvasType); 
    }
}
}