using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ClickableStar : MonoBehaviour
{
    private WikidataLoader.WikidataBinding data;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI regionText;
    [SerializeField] private Image displayImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Button redirectButton;
    [SerializeField] private Image panelImage;
    [SerializeField] private Image manuallyAssignedImage;

    public string region = "Región Andina de Colombia";
    public int año_de_muerte = 0;
    public string genero = "male";
    

    private static ClickableStar currentlySelectedStar; // Static reference to the currently selected star
    private Material originalMaterial; // Store the original material of this star
    [SerializeField] private Material selectedMaterial; // Material to apply when selected

    private void Start()
    {
        // Make the manually assigned image invisible at the start
        if (manuallyAssignedImage != null)
        {
            SetImageAlpha(manuallyAssignedImage, 0f);
        }
    }

    public void SetData(WikidataLoader.WikidataBinding newData, TextMeshProUGUI nameUI, TextMeshProUGUI dateUI, TextMeshProUGUI regionUI, Image imageUI, Sprite defaultImg, Button button)
    {
        data = newData;
        nameText = nameUI;
        dateText = dateUI;
        regionText = regionUI;
        displayImage = imageUI;
        defaultSprite = defaultImg;
        redirectButton = button;

        if (data != null)
        {
            // Call UpdateFilterData to initialize the filter-related values
            UpdateFilterData();
            Debug.Log($"Data assigned to {gameObject.name} with element value: {data.person?.value}");
        }
        else
        {
            Debug.LogError($"Data is null for {gameObject.name} in SetData.");
        }
    }

    public void UpdateFilterData()
    {
        if (data != null)
        {
            string fechaMuerte = data.fecha_de_muerte?.value ?? "Desconocida";
            string genero = data.genero?.value?.ToLower() ?? "unknown";
            string region = data.lugar_significativoLabel?.value?.Replace("Región ", "").Replace(" de Colombia", "").Trim() ?? "Desconocida";

            if (StarDataHandler.Instance != null)
            {
                StarDataHandler.Instance.AddOrUpdateStarData(gameObject.name, data.personLabel?.value ?? "Unknown", fechaMuerte, genero, region);
            }
            else
            {
                Debug.LogError("StarDataHandler instance is null. Cannot update star data.");
            }
        }
        else
        {
            Debug.LogWarning($"Data is null for {gameObject.name}. No updates made.");
        }
    }

    void Update()
    {
        // Verificar si el mouse está sobre un elemento de UI
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    ShowInfo();
                }
                if (panelImage != null)
                {
                    if (panelImage.gameObject.activeSelf == false)
                    {
                        panelImage.gameObject.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogWarning("Panel Image no está asignado en el inspector de Unity");
                }
            }
        }
    }

    private bool IsPointerOverUI()
    {
        // Verifica si el puntero está sobre un elemento de UI
        if (EventSystem.current == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    public string GetElementValueOnClick()
    {
        if (data != null && !string.IsNullOrEmpty(data.person?.value))
        {
            Debug.Log($"Element value: {data.person.value}");
            return data.person.value; // Return the element value
        }

        Debug.LogWarning($"Element value is missing for {gameObject.name}.");
        return "No value available"; // Default value if the element is missing
    }

    void ShowInfo()
    {
        if (data != null)
        {
            // Find the ElementValueHandler script in the scene
            ElementValueHandler elementValueHandler = FindFirstObjectByType<ElementValueHandler>();
            if (elementValueHandler != null)
            {
                elementValueHandler.SetElementValue(data.person?.value);
            }
            else
            {
                Debug.LogWarning("ElementValueHandler not found in the scene.");
            }

            // Handle deselection of the previously selected star
            if (currentlySelectedStar != null && currentlySelectedStar != this)
            {
                currentlySelectedStar.ViewedMaterial();
            }

            // Set this star as the currently selected star
            currentlySelectedStar = this;

            // Change the material to the selected material
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                if (originalMaterial == null)
                {
                    originalMaterial = renderer.material; // Store the original material
                }
                renderer.material = selectedMaterial; // Apply the selected material
            }

            // Update UI elements
            if (nameText != null)
            {
                nameText.text = $"{data.personLabel.value}";
            }

            if (dateText != null)
            {
                string fechaNacimiento = !string.IsNullOrEmpty(data.fecha_de_nacimiento?.value) ? data.fecha_de_nacimiento.value : "Desconocida";
                string fechaMuerte = !string.IsNullOrEmpty(data.fecha_de_muerte?.value) ? data.fecha_de_muerte.value : "Desconocida";

                dateText.text = $"{fechaNacimiento} - {fechaMuerte}";

                // Nueva lógica para extraer el año de muerte
                if (fechaMuerte != "Desconocida" && fechaMuerte.Length >= 4) // Asegúrate de que la cadena es lo suficientemente larga
                {
                    Debug.Log($"Fecha de muerte original: {fechaMuerte}"); // Log para confirmar
                    // Asumimos que la fecha viene en formato "dd/mm/yyyy" o similar donde el año son los últimos 4 caracteres.
                    // Si el formato es diferente, esta lógica necesitará ajustes.
                    string yearString = "";
                    if (fechaMuerte.Contains("-")) // Ejemplo: "dd/mm/yyyy"
                    {
                        string[] dateParts = fechaMuerte.Split('-');
                        if (dateParts.Length == 3)
                        {
                            yearString = dateParts[2];
                        }
                    }
                    else // Si no hay "/", intenta tomar los últimos 4 caracteres como antes, pero con más cuidado.
                    {
                        // Esta es la lógica original, asegúrate que sea adecuada para todos tus formatos de fecha.
                        if (fechaMuerte.Length >=4) {
                            yearString = fechaMuerte.Substring(fechaMuerte.Length - 4);
                        }
                    }

                    if (!string.IsNullOrEmpty(yearString) && int.TryParse(yearString, out int year))
                    {
                        año_de_muerte = year;
                        Debug.Log($"Año de muerte asignado: {año_de_muerte}"); // Log para confirmar
                    }
                    else
                    {
                        Debug.LogWarning($"No se pudo convertir el año desde yearString: '{yearString}' (original: '{fechaMuerte}')");
                        año_de_muerte = 2002; // O un valor centinela como -1 para indicar "desconocido"
                    }
                }
                else
                {
                    Debug.LogWarning($"Fecha de muerte es '{fechaMuerte}', no se intentará extraer el año.");
                    año_de_muerte = 0; // O -1 si prefieres
                }
            }

            if (regionText != null)
            {
                string regionItem = !string.IsNullOrEmpty(data.lugar_significativoLabel?.value) ? data.lugar_significativoLabel.value : "Desconocida";

                regionText.text = $"Región: {regionItem}";
            }

            // Extract and display gender
            if (data.genero != null && !string.IsNullOrEmpty(data.genero.value))
            {
                genero = data.genero.value.ToLower(); // Assign to the genero field
            }
            else
            {
                Debug.LogWarning($"Gender is missing for {gameObject.name}");
                genero = "unknown"; // Assign a default value
            }

            if (displayImage != null)
            {
                if (data.imagen != null && !string.IsNullOrEmpty(data.imagen.value))
                {
                    StartCoroutine(LoadImage(data.imagen.value));
                    SetImageAlpha(displayImage, 1f);
                }
                else
                {
                    displayImage.sprite = defaultSprite;
                    SetImageAlpha(displayImage, 1f);
                }
            }

            if (manuallyAssignedImage != null)
            {
                SetImageAlpha(manuallyAssignedImage, 1f);
            }

            // Call the new function to get the element value
            string elementValue = GetElementValueOnClick();
            Debug.Log($"Clicked object element value: {elementValue}");
        }
        else
        {
            Debug.LogWarning($"Data is null for {gameObject.name}");

            if (displayImage != null)
            {
                SetImageAlpha(displayImage, 0f);
            }

            if (manuallyAssignedImage != null)
            {
                SetImageAlpha(manuallyAssignedImage, 0f);
            }
        }
    }

    // Helper method to set the alpha of an Image
    private void SetImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    private IEnumerator LoadImage(string imageUrl)
    {
        if (imageUrl.StartsWith("http://"))
        {
            imageUrl = imageUrl.Replace("http://", "https://");
        }

        using (UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((UnityEngine.Networking.DownloadHandlerTexture)request.downloadHandler).texture;
                displayImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                Debug.LogWarning($"Failed to load image from URL: {imageUrl}. Using default sprite.");
                displayImage.sprite = defaultSprite;
            }
        }
    }

    public string GetRegion()
    {
        if (!string.IsNullOrEmpty(region))
        {
            // Remove "Región " and " de Colombia" from the region string
            string cleanedRegion = region.Replace("Región ", "").Replace(" de Colombia", "").Trim();
            region = cleanedRegion; // Update the region field
            Debug.Log($"Region extracted: {cleanedRegion}");
            return cleanedRegion;
        }

        Debug.LogWarning($"Region is not set or invalid for {gameObject.name}. Returning default value.");
        return "Desconocida"; // Default value if region is not set
    }

    public int GetDeath()
    {   
        if(año_de_muerte != 0)
        {
            return año_de_muerte;
        }
        Debug.LogWarning($"Year of death is not set for {gameObject.name}. Returning default value.");
        return 0; // Default value if year of death is not set

    }

    public string GetGender()
    {
        if (!string.IsNullOrEmpty(genero))
        {
            return genero;
        }

        Debug.LogWarning($"Gender is not set for {gameObject.name}. Returning default value.");
        return "unknown";
    }

    private void ViewedMaterial()
    {
        // Restore the original material
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(0.5f, 1f, 0.5f, 1f);
        }
    }


}
