using JCTool.InputHandler;
using TMPro;
using UnityEngine;


public class HintsSwitcher : MonoBehaviour
{
    [SerializeField] private SceneViewHub sceneViewHub;
    private TextMeshProUGUI hintsTextMesh;
    private void Awake()
    {
        sceneViewHub.InputModeNotifier += OnUpdateHints;
        hintsTextMesh = GetComponent<TextMeshProUGUI>();
    }
    private void OnDestroy()
    {
        sceneViewHub.InputModeNotifier -= OnUpdateHints;
    }
    private void OnUpdateHints(InputState _inputState)
    {
        if (_inputState is InputState.MouseClick)
            hintsTextMesh.text = "• Rotate by right-clicking.\n• Zoom in/out with the mouse scroll.\n• Hold the middle mouse button to pan the scene.";
        else if (_inputState is InputState.TouchPress)
            hintsTextMesh.text = "• Rotate using two fingers, swiping with one finger at a time.\n• Zoom in/out by pinching with two fingers.\n• Move two fingers together to pan the scene.";
    }
}
