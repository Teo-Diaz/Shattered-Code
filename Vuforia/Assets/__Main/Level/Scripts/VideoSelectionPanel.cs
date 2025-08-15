using UnityEngine;
using UnityEngine.UI;

public class VideoSelectionPanel : MonoBehaviour
{
    [Header("Selection UI")]
    public ToggleGroup toggleGroup;          // ToggleGroup del panel
    public Toggle[] toggles;                 // 3 toggles (o más)
    public GameObject[] selectedMarkers;     // Marcador visual por toggle (opcional)

    [Header("Data")]
    [TextArea]
    public string[] videoUrls;               // URLs en el mismo orden que los toggles

    [Header("Target")]
    public VideoController videoController;  // Referencia al controlador del video

    [Header("Behavior")]
    public int defaultSelectedIndex = 0;     // El que inicia seleccionado
    public bool autoplayOnSelect = true;     // Política para cada cambio

    void Start()
    {
        // Validaciones básicas
        if (toggles == null || videoUrls == null || toggles.Length != videoUrls.Length)
        {
            Debug.LogWarning("VideoSelectionPanel: toggles y videoUrls deben tener el mismo tamaño.");
        }

        // Asegurar Group
        if (toggleGroup) toggleGroup.allowSwitchOff = false;

        // Suscribir listeners
        for (int i = 0; i < toggles.Length; i++)
        {
            int idx = i;
            if (toggles[i] == null) continue;

            toggles[i].group = toggleGroup;
            toggles[i].onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    ApplySelection(idx);
                }
                UpdateMarkers();
            });
        }

        // Selección por defecto
        if (defaultSelectedIndex >= 0 && defaultSelectedIndex < toggles.Length)
        {
            toggles[defaultSelectedIndex].isOn = true; // Dispara ApplySelection via listener
        }
        else
        {
            UpdateMarkers();
        }
    }

    void ApplySelection(int index)
    {
        if (!videoController) return;
        if (index < 0 || index >= videoUrls.Length) return;

        var url = videoUrls[index];
        videoController.SetUrl(url, autoplayOnSelect);
    }

    void UpdateMarkers()
    {
        if (selectedMarkers == null) return;
        for (int i = 0; i < selectedMarkers.Length; i++)
        {
            if (!selectedMarkers[i]) continue;

            bool isOn = (i < toggles.Length && toggles[i] && toggles[i].isOn);
            selectedMarkers[i].SetActive(isOn);
        }
    }
}
