using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class WikidataLoader : MonoBehaviour
{
    public GameObject starPrefab;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI regionText;
    public Image displayImage;
    public Sprite defaultSprite;
    public TextMeshProUGUI counterText;
    public int extraObjectsCount = 6000;
    public GameObject cylinderPrefab;
    public Material defaultMaterial;
    public TMP_InputField searchInputField; 
    public Slider deathFilterSlider;
    public TextMeshProUGUI deathFilterValueText;
    public Sprite activeSprite;  // Sprite for active state
    public Sprite inactiveSprite; // Sprite for inactive state
    public Button andinaRegionButton;
    public Button amazonicaRegionButton;
    public Button orinoquiaRegionButton;
    public Button pacificaRegionButton;
    public Button caribeInsularRegionButton;
    public Button resetSearchButton; // Button to clear search input and reset filters
    // Prefab del botón y el contenedor del scroll para los resultados
    public GameObject buttonPrefab;
    public Transform scrollContent;
    // Nueva función para mostrar los resultados en el scroll usando el prefab de botón
    // Objeto a desactivar al hacer click en un botón del dropdown
    public GameObject objectToDisableOnSelect;

    private bool showMasculine = true;
    private bool showFeminine = true;

    private Dictionary<string, bool> regionFilters = new Dictionary<string, bool>
    {
        { "Andina", true },
        { "Amazónica", true },
        { "Orinoquía", true },
        { "Pacífica", true },
        { "Caribe", true }
    };

    private Dictionary<string, Color> regionColors = new Dictionary<string, Color>
    {
        { "Andina", HexToColor("#DF8197") },       // Example HEX color for Andina
        { "Amazónica", HexToColor("#EA8472") },   // Example HEX color for Amazónica
        { "Orinoquía", HexToColor("#F1E8D9") },   // Example HEX color for Orinoquía
        { "Pacífica", HexToColor("#EDAF65") },    // Example HEX color for Pacífica
        { "Caribe", HexToColor("#F1E8D9") }       // Example HEX color for Caribe
    };

    // Normalized region colors map: keys are normalized (no accents, lowercase) versions
    private Dictionary<string, Color> normalizedRegionColors;
    // Normalized region filter map for lookup using normalized keys
    private Dictionary<string, bool> normalizedRegionFilters;
    private string wikidataQueryURL = "https://query.wikidata.org/sparql?query=";
    private List<WikidataBinding> wikidataObjects = new List<WikidataBinding>();

    void Start()
    {
        showMasculine = true;
        showFeminine = true;

        // Use a temporary list to avoid modifying the dictionary while iterating
        var regionKeys = new List<string>(regionFilters.Keys);
        foreach (var lugar_significativoLabel in regionKeys)
        {
            regionFilters[lugar_significativoLabel] = true; // Set all regions to active
        }

        // Build normalizedRegionColors for case-insensitive / accent-insensitive lookups
        normalizedRegionColors = new Dictionary<string, Color>();
        foreach (var kv in regionColors)
        {
            string normalizedKey = NormalizeText(kv.Key);
            if (!normalizedRegionColors.ContainsKey(normalizedKey))
                normalizedRegionColors[normalizedKey] = kv.Value;
        }
        // Build normalizedRegionFilters so we can lookup filter states using normalized region names
        normalizedRegionFilters = new Dictionary<string, bool>();
        foreach (var kv in regionFilters)
        {
            string normalizedKey = NormalizeText(kv.Key);
            if (!normalizedRegionFilters.ContainsKey(normalizedKey))
                normalizedRegionFilters[normalizedKey] = kv.Value;
        }

        if (deathFilterSlider != null)
        {
            deathFilterSlider.value = deathFilterSlider.maxValue; // Set slider to max value

            // Add a listener to update the filter when the slider value changes
            deathFilterSlider.onValueChanged.AddListener(delegate { FilterByYearOfDeath(); });
        }

        // Mostrar el dropdown de resultados cuando el input es clickeado o modificado
        if (searchInputField != null)
        {
            searchInputField.onSelect.AddListener(delegate { ShowDropdownResults(); });
            searchInputField.onValueChanged.AddListener(delegate { ShowDropdownResults(); });
        }

        // Hook the reset search button if assigned
        if (resetSearchButton != null)
        {
            resetSearchButton.onClick.AddListener(ResetSearchFilter);
        }

        StartCoroutine(FetchWikidataData());

        // Apply initial filters
        ApplyCombinedFilters();
    }

    [System.Serializable]
    public class WikidataResponse
    {
        public WikidataResults results;
    }

    [System.Serializable]
    public class WikidataResults
    {
        public WikidataBinding[] bindings;
    }

    [System.Serializable]
    public class WikidataBinding
    {
        public WikidataValue person;
        public WikidataValue personLabel;
        public WikidataValue fecha_de_nacimiento;
        public WikidataValue fecha_de_desaparicion;
        public WikidataValue fecha_de_muerte;
        public WikidataValue año_de_muerte;
        public WikidataValue responsable;
        public WikidataValue edad;
        public WikidataValue imagen;
        public WikidataValue genero;
        public WikidataValue lugar_significativoLabel;
    }

    [System.Serializable]
    public class WikidataValue
    {
        public string value;
    }

    IEnumerator FetchWikidataData()
    {
        string query = @"
            SELECT DISTINCT 
                ?person
                ?personLabel
                ?fecha_de_nacimiento
                ?fecha_de_muerte
                ?año_de_muerte
                ?lugar_significativoLabel
                ?imagen
                ?edad
                ?fuente
                ?fuenteLabel
                ?fuenteDescription

            WHERE {
            #Etiquetas y descripciones en es/en
              SERVICE wikibase:label { bd:serviceParam wikibase:language 'es,en'. }
              #personas asociadas al evento 'falsos porsitivos'
              ?person wdt:P793 wd:Q779179 .
                ?person wdt:P31 wd:Q5 .  # humanos

                # Lugar significativo (P7153)


            OPTIONAL { ?person wdt:P7153 ?lugar_significativo . }

            ############################
            # FECHA DE NACIMIENTO (P569)
            ############################
            OPTIONAL {
                ?person p:P569 ?birthStmt .
                ?birthStmt ps:P569 ?birthRaw .
                OPTIONAL {
                ?birthStmt psv:P569 ?birthValueNode .
                ?birthValueNode wikibase:timePrecision ?birthPrecision .
                }
                BIND(
                IF(BOUND(?birthPrecision),
                    IF(?birthPrecision = 9,
                    STR(YEAR(?birthRaw)),
                    IF(?birthPrecision = 10,
                        CONCAT(STR(MONTH(?birthRaw)),'/',STR(YEAR(?birthRaw))),
                        CONCAT(STR(DAY(?birthRaw)),'/',STR(MONTH(?birthRaw)),'/',STR(YEAR(?birthRaw)))
                    )
                    ),
                    CONCAT(STR(DAY(?birthRaw)),'/',STR(MONTH(?birthRaw)),'/',STR(YEAR(?birthRaw)))
                ) AS ?fecha_de_nacimiento
                )
            }

            ########################
            # FECHA DE MUERTE (P570)
            ########################
            OPTIONAL {
                ?person p:P570 ?deathStmt .
                ?deathStmt ps:P570 ?deathRaw .
                OPTIONAL {
                ?deathStmt psv:P570 ?deathValueNode .
                ?deathValueNode wikibase:timePrecision ?deathPrecision .
                }

                # Fecha de muerte formateada según precisión
                BIND(
                IF(BOUND(?deathPrecision),
                    IF(?deathPrecision = 9,
                    STR(YEAR(?deathRaw)),
                    IF(?deathPrecision = 10,
                        CONCAT(STR(MONTH(?deathRaw)),'/',STR(YEAR(?deathRaw))),
                        CONCAT(STR(DAY(?deathRaw)),'/',STR(MONTH(?deathRaw)),'/',STR(YEAR(?deathRaw)))
                    )
                    ),
                    CONCAT(STR(DAY(?deathRaw)),'/',STR(MONTH(?deathRaw)),'/',STR(YEAR(?deathRaw)))
                ) AS ?fecha_de_muerte
                )

                # Año/mes/día de muerte con el mismo criterio
                BIND(
                IF(BOUND(?deathPrecision),
                    IF(?deathPrecision = 9,
                    STR(YEAR(?deathRaw)),
                    IF(?deathPrecision = 10,
                        CONCAT(STR(MONTH(?deathRaw)),'/',STR(YEAR(?deathRaw))),
                        CONCAT(STR(DAY(?deathRaw)),'/',STR(MONTH(?deathRaw)),'/',STR(YEAR(?deathRaw)))
                    )
                    ),
                    CONCAT(STR(DAY(?deathRaw)),'/',STR(MONTH(?deathRaw)),'/',STR(YEAR(?deathRaw)))
                ) AS ?año_de_muerte
                )
            }

            # Imagen (P18)
            OPTIONAL { ?person wdt:P18 ?imagen . }

            # Edad ajustada por mes/día
            OPTIONAL {
                ?person wdt:P569 ?birthRaw2 .
                ?person wdt:P570 ?deathRaw2 .
                BIND(
                YEAR(?deathRaw2) - YEAR(?birthRaw2)
                - IF( (MONTH(?deathRaw2) < MONTH(?birthRaw2))
                    || (MONTH(?deathRaw2) = MONTH(?birthRaw2) && DAY(?deathRaw2) < DAY(?birthRaw2)),
                    1, 0)
                AS ?edad
                )
            }

            ###################################################
            # DESCRITO EN LA FUENTE (P1343) + descripción de la fuente
            ###################################################
            OPTIONAL { ?person wdt:P1343 ?fuente . }  # p.ej., Q134725043
            }
            ORDER BY ?personLabel



            ";

        string fullURL = wikidataQueryURL + UnityWebRequest.EscapeURL(query) + "&format=json";

        using (UnityWebRequest request = UnityWebRequest.Get(fullURL))
        {
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ProcessWikidataResponse(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error in SPARQL query: " + request.error);
            }
        }
    }

    void ProcessWikidataResponse(string jsonData)
    {
        WikidataResponse responseData = JsonUtility.FromJson<WikidataResponse>(jsonData);
        var results = responseData.results.bindings;

        if (results.Length > 0)
        {
            wikidataObjects.Clear();
            
            // Crear un HashSet para detectar duplicados usando el ID de la persona
            HashSet<string> uniquePersonIds = new HashSet<string>();
            List<WikidataBinding> uniqueResults = new List<WikidataBinding>();

            foreach (var result in results)
            {
                if (result.person != null && !string.IsNullOrEmpty(result.person.value))
                {
                    // Si el ID de la persona no está en el HashSet, agregarlo
                    if (uniquePersonIds.Add(result.person.value))
                    {
                        uniqueResults.Add(result);
                    }
                    else
                    {
                        Debug.Log($"Dato duplicado encontrado y omitido: {result.personLabel?.value}");
                    }
                }
            }

            Debug.Log($"Total de registros originales: {results.Length}");
            Debug.Log($"Total de registros únicos: {uniqueResults.Count}");

            wikidataObjects.AddRange(uniqueResults);

            // Delegate star creation to SpawnStarsByRegion
            SpawnStarsByRegion();
            //--------------------------------------------------------------------------------------------agregar semillas extra
            GameObject starContainer = GameObject.Find("StarContainer");
            CreateExtraObjects(starContainer);

            // Update the counter text
            if (counterText != null)
            {
                counterText.text = $"{results.Length + extraObjectsCount}";
            }
        }
        else
        {
            Debug.LogWarning("No data found in Wikidata.");
            if (counterText != null)
            {
                counterText.text = $"{extraObjectsCount}";
            }

            GameObject starContainer = GameObject.Find("StarContainer");
            if (starContainer != null)
            {
                CreateExtraObjects(starContainer);
            }
        }
        
    }

    void CreateStar(WikidataBinding data)
    {
        GameObject starContainer = GameObject.Find("StarContainer");
        if (starContainer == null)
        {
            Debug.LogError("StarContainer not found in the scene.");
            return;
        }

        float radius = 15f; 
        float theta = Random.Range(0f, Mathf.PI * 2); 
        float phi = Random.Range(0f, Mathf.PI); 

        float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = radius * Mathf.Cos(phi);

        Vector3 position = new Vector3(x, y, z) + starContainer.transform.position; 

        GameObject star = Instantiate(starPrefab, position, Quaternion.identity);
        star.name = data.person.value; // Use the URL as the object name

        star.transform.SetParent(starContainer.transform);

        ClickableStar clickable = star.AddComponent<ClickableStar>();
        clickable.SetData(data, nameText, dateText, regionText, displayImage, defaultSprite, null);

        // Assign the color based on the lugar_significativoLabel
        Renderer renderer = star.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            Color color = Color.white; // Default to white if no lugar_significativoLabel is found

            // Get the lugar_significativoLabel from the data and normalize it for lookup
            string lugar_significativoLabel = data.lugar_significativoLabel?.value?.Replace("Región ", "").Replace(" de Colombia", "").Trim() ?? "Unknown";

            // Normalize the key (remove diacritics and lowercase) and lookup in the normalized map
            string lookupKey = NormalizeText(lugar_significativoLabel);
            if (normalizedRegionColors != null && normalizedRegionColors.TryGetValue(lookupKey, out Color mappedColor))
            {
                color = mappedColor;
            }
            else
            {
                Debug.LogWarning($"Region '{lugar_significativoLabel}' (normalized: '{lookupKey}') not found in regionColors dictionary. Defaulting to white.");
            }

            material.color = color;
        }

        star.transform.localScale = Vector3.one * Random.Range(1f, 2f);

        Debug.Log($"Created star: {star.name}, Position: {position}, Region: {data.lugar_significativoLabel?.value}, Color: {renderer.material.color}");
    }

    void CreateStar(WikidataBinding data, Vector3 position, GameObject starContainer)
    {
        GameObject star = Instantiate(starPrefab, position, Quaternion.identity);
        star.name = data.person.value; // Use the URL as the object name

        star.transform.SetParent(starContainer.transform);

        ClickableStar clickable = star.AddComponent<ClickableStar>();
        clickable.SetData(data, nameText, dateText, regionText, displayImage, defaultSprite, null);

        // Assign the color based on the lugar_significativoLabel
        Renderer renderer = star.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            Color color = Color.white; // Default to white if no lugar_significativoLabel is found

            // Get the lugar_significativoLabel from the data and normalize it for lookup
            string lugar_significativoLabel = data.lugar_significativoLabel?.value?.Replace("Región ", "").Replace(" de Colombia", "").Trim() ?? "Unknown";

            // Normalize the key (remove diacritics and lowercase) and lookup in the normalized map
            string lookupKey = NormalizeText(lugar_significativoLabel);
            if (normalizedRegionColors != null && normalizedRegionColors.TryGetValue(lookupKey, out Color mappedColor))
            {
                color = mappedColor;
            }
            else
            {
                Debug.LogWarning($"Region '{lugar_significativoLabel}' (normalized: '{lookupKey}') not found in regionColors dictionary. Defaulting to white.");
            }

            material.color = color;
        }

        star.transform.localScale = Vector3.one * Random.Range(15f, 20f);

        Debug.Log($"Created star: {star.name}, Position: {position}, Region: {data.lugar_significativoLabel?.value}, Color: {renderer.material.color}");
    }

    void CreateExtraObjects(GameObject starContainer)
    {
        int objectsPerCluster = 50;
        float clusterRadius = 12f;
        float objectSpacing = 3f;

        int totalClusters = Mathf.CeilToInt((float)extraObjectsCount / objectsPerCluster);

        for (int clusterIndex = 0; clusterIndex < totalClusters; clusterIndex++)
        {
            float theta = Random.Range(0f, Mathf.PI * 2); 
            float phi = Random.Range(0f, Mathf.PI);

            float clusterX = clusterRadius * Mathf.Sin(phi) * Mathf.Cos(theta);
            float clusterY = clusterRadius * Mathf.Sin(phi) * Mathf.Sin(theta);
            float clusterZ = clusterRadius * Mathf.Cos(phi);

            Vector3 clusterCenter = new Vector3(clusterX, clusterY, clusterZ) + starContainer.transform.position;

            Material clusterMaterial = new Material(defaultMaterial);
            clusterMaterial.color = HexToColor("#F289A1");

            SpawnCylinder(clusterCenter, starContainer.transform.position, clusterMaterial);

            int objectsInThisCluster = Mathf.Min(objectsPerCluster, extraObjectsCount - (clusterIndex * objectsPerCluster));

            for (int i = 0; i < objectsInThisCluster; i++)
            {
                float offsetX = Random.Range(-objectSpacing, objectSpacing);
                float offsetY = Random.Range(-objectSpacing, objectSpacing);
                float offsetZ = Random.Range(-objectSpacing, objectSpacing);

                Vector3 position = clusterCenter + new Vector3(offsetX, offsetY, offsetZ);

                GameObject extraObject = Instantiate(starPrefab, position, Quaternion.identity);
                extraObject.name = $"ExtraObject_Cluster{clusterIndex + 1}_{i + 1}";

                Renderer renderer = extraObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = clusterMaterial;
                }

                extraObject.transform.SetParent(starContainer.transform);

                extraObject.transform.localScale = Vector3.one * Random.Range(10f, 15f);
            }
        }
    }

    void SpawnCylinder(Vector3 start, Vector3 end, Material material)
    {
        if (cylinderPrefab == null)
        {
            Debug.LogError("Cylinder prefab is not assigned.");
            return;
        }

        // Find the StarContainer
        GameObject starContainer = GameObject.Find("StarContainer");
        if (starContainer == null)
        {
            Debug.LogError("StarContainer not found in the scene.");
            return;
        }

        // Calculate the midpoint between the start and end points
        Vector3 midPoint = (start + end) / 2;

        // Calculate the direction and distance between the points
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        // Instantiate the cylinder
        /*GameObject cylinder = Instantiate(cylinderPrefab, midPoint, Quaternion.identity);

        // Set the cylinder's scale and rotation
        cylinder.transform.localScale = new Vector3(0.1f, distance / 2, 0.1f); // Scale the cylinder (adjust thickness as needed)
        cylinder.transform.up = direction.normalized; // Align the cylinder with the direction vector

        // Assign the material to the cylinder
        Renderer renderer = cylinder.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }

        // Make the cylinder a child of the StarContainer
        cylinder.transform.SetParent(starContainer.transform);*/
    }

    // Función auxiliar para normalizar texto (quitar acentos y convertir a minúsculas)
    private string NormalizeText(string text)
    {
        string normalized = text.Normalize(System.Text.NormalizationForm.FormD);
        var chars = normalized.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark);
        return new string(chars.ToArray()).ToLower();
    }

    public void FilterStars()
    {
        string searchQuery = NormalizeText(searchInputField.text); // Normalizar el texto de búsqueda

        var starDataDictionary = StarDataHandler.Instance.GetAllStarData();

        Transform matchingStar = null;

        foreach (var starEntry in starDataDictionary)
        {
            string objectName = starEntry.Key;
            StarData starData = starEntry.Value;

            GameObject starObject = GameObject.Find(objectName);
            if (starObject != null)
            {
                Renderer renderer = starObject.GetComponent<Renderer>();
                Collider collider = starObject.GetComponent<Collider>();

                if (renderer != null)
                {
                    Material material = renderer.material;
                    Color color = material.color;

                    // Normalizar el nombre de la estrella para la comparación
                    string normalizedStarName = NormalizeText(starData.Name);

                    if (normalizedStarName.Contains(searchQuery))
                    {
                        // Verificar que los caracteres aparecen en el mismo orden
                        int lastIndex = -1;
                        bool matchesSequence = true;

                        // Verificar cada caracter del texto de búsqueda
                        foreach (char c in searchQuery)
                        {
                            int currentIndex = normalizedStarName.IndexOf(c, lastIndex + 1);
                            if (currentIndex <= lastIndex)
                            {
                                matchesSequence = false;
                                break;
                            }
                            lastIndex = currentIndex;
                        }

                        if (matchesSequence)
                        {
                            // Make the star fully visible if it matches the search query
                            color.a = 1f; // Fully opaque
                            matchingStar = starObject.transform; // Save the matching star
                            starObject.transform.localScale = Vector3.one * 15f; // Scale up the matching star
                        }
                        else
                        {
                            // Make the star transparent if it doesn't match
                            color.a = 0f; // Fully transparent
                            starObject.transform.localScale = Vector3.one * 0.5f; // Scale down the non-matching star
                        }
                    }
                    else
                    {
                        // Make the star transparent if it doesn't match
                        color.a = 0f; // Fully transparent
                        starObject.transform.localScale = Vector3.one * 0.5f; // Scale down the non-matching star
                    }

                    material.color = color;

                    if (collider != null)
                    {
                        collider.enabled = color.a > 0.5f; // Enable collider only if the star is visible
                    }
                }
            }
        }

        // Rotate the sphere to center the matching star
        if (matchingStar != null)
        {
            GameObject starContainer = GameObject.Find("StarContainer");
            if (starContainer != null)
            {
                CenterObjectInView(matchingStar, starContainer);
            }
        }

        UpdateVisibleStarsCount();
    }
    
    public void ShowDropdownResults()
    {
        var starDataDictionary = StarDataHandler.Instance.GetAllStarData();
        if (buttonPrefab == null || scrollContent == null || searchInputField == null) return;

        // Ensure the ScrollRect (dropdown) is active so results are visible
        var ensureScrollRect = scrollContent.GetComponentInParent<ScrollRect>();
        if (ensureScrollRect != null)
            ensureScrollRect.gameObject.SetActive(true);
        else
            scrollContent.gameObject.SetActive(true);

        // Eliminar los botones previos
        foreach (Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }

        // Obtener el texto actual del input y normalizarlo
        string searchQuery = NormalizeText(searchInputField.text);

        // Obtener y ordenar los nombres alfabéticamente
        var sortedNames = starDataDictionary.Values.Select(s => s.Name).OrderBy(n => n).ToList();

        foreach (var name in sortedNames)
        {
            string normalizedName = NormalizeText(name);
            bool showButton = false;

            if (string.IsNullOrEmpty(searchQuery))
            {
                showButton = true; // Si no hay texto, mostrar todos
            }
            else if (normalizedName.Contains(searchQuery))
            {
                // Verificar que los caracteres aparecen en el mismo orden
                int lastIndex = -1;
                bool matchesSequence = true;
                foreach (char c in searchQuery)
                {
                    int currentIndex = normalizedName.IndexOf(c, lastIndex + 1);
                    if (currentIndex <= lastIndex)
                    {
                        matchesSequence = false;
                        break;
                    }
                    lastIndex = currentIndex;
                }
                if (matchesSequence)
                    showButton = true;
            }

            if (showButton)
            {
                GameObject buttonObj = Instantiate(buttonPrefab, scrollContent);
                var buttonText = buttonObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (buttonText != null)
                    buttonText.text = name;
                // Autocompletar al hacer click y desactivar objeto
                var buttonComponent = buttonObj.GetComponent<Button>();
                if (buttonComponent != null)
                {
                    string nameCopy = name; // Captura para el closure
                    buttonComponent.onClick.AddListener(() => {
                        searchInputField.text = nameCopy;
                        FilterStars();
                        if (objectToDisableOnSelect != null)
                            objectToDisableOnSelect.SetActive(false);
                    });
                }
            }
        }

        // Ajustar el scroll a la parte superior
        var scrollRect = scrollContent.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 1f;
    }

    private void CenterObjectInView(Transform target, GameObject starContainer)
    {
        // Get the direction from the StarContainer to the target object
        Vector3 directionToTarget = target.position - starContainer.transform.position;

        // Calculate the rotation needed to align the target with the camera's forward direction
        Quaternion targetRotation = Quaternion.LookRotation(-directionToTarget, Vector3.up);

        // Apply the rotation to the StarContainer
        starContainer.transform.rotation = targetRotation;
    }

    public void FilterByYearOfDeath()
    {
        float maxYearOfDeath = deathFilterSlider.value;
        deathFilterValueText.text = $"2002 - {Mathf.FloorToInt(maxYearOfDeath)}";

        var starDataDictionary = StarDataHandler.Instance.GetAllStarData();

        foreach (var starEntry in starDataDictionary)
        {
            string objectName = starEntry.Key;
            StarData starData = starEntry.Value;

            int starDeathYear = starData.AñoDeMuerte; // Use AñoDeMuerte directly
            bool isVisible = starDeathYear <= maxYearOfDeath && starDeathYear != -1;

            GameObject starObject = GameObject.Find(objectName);
            if (starObject != null)
            {
                Renderer renderer = starObject.GetComponent<Renderer>();
                Collider collider = starObject.GetComponent<Collider>();

                if (renderer != null)
                {
                    Material material = renderer.material;
                    Color color = material.color;
                    color.a = isVisible ? 1f : 0f; // Fully opaque or fully transparent
                    starObject.transform.localScale = isVisible ? Vector3.one * 15f : Vector3.one * 0.5f; // Scale up or down
                    material.color = color;

                    if (collider != null)
                    {
                        collider.enabled = isVisible;
                    }
                }
            }
        }

        UpdateVisibleStarsCount();
    }

    private void ApplyCombinedFilters()
    {
        float maxYearOfDeath = deathFilterSlider.value; // Get the current value of the year-of-death slider

        // Update the filter texts
        deathFilterValueText.text = $"2002 - {Mathf.FloorToInt(maxYearOfDeath)}";

        GameObject starContainer = GameObject.Find("StarContainer");
        if (starContainer == null)
        {
            Debug.LogError("StarContainer not found in the scene.");
            return;
        }

        foreach (Transform star in starContainer.transform)
        {
            ClickableStar clickableStar = star.GetComponent<ClickableStar>();
            Renderer renderer = star.GetComponent<Renderer>();
            Collider collider = star.GetComponent<Collider>();

            if (clickableStar != null && renderer != null)
            {
                int starDeath = clickableStar.GetDeath();
                string starGender = clickableStar.GetGender();
                string starRegion = clickableStar.GetRegion();

                // Check if the star meets all the filter criteria
                bool meetsDeathFilter = starDeath <= maxYearOfDeath;
                bool meetsGenderFilter = (starGender == "male" && showMasculine) || (starGender == "female" && showFeminine);
                bool meetsRegionFilter = regionFilters.ContainsKey(starRegion) && regionFilters[starRegion];

                bool isVisible = meetsDeathFilter && meetsGenderFilter && meetsRegionFilter;

                // Log the filter criteria for debugging
                Debug.Log($"Star: {star.name}, Death: {starDeath}, Gender: {starGender}, Region: {starRegion}, Visible: {isVisible}");
                Debug.Log($"Star: {star.name}, MeetsDeathFilter: {meetsDeathFilter}, MeetsGenderFilter: {meetsGenderFilter}, MeetsRegionFilter: {meetsRegionFilter}, Visible: {isVisible}");

                if (isVisible)
                {
                    // Show the star
                    Material material = renderer.material;
                    Color color = material.color;
                    color.a = 1f; // Fully opaque
                    material.color = color;

                    if (collider != null)
                    {
                        collider.enabled = true;
                    }
                }
                else
                {
                    // Hide the star
                    Material material = renderer.material;
                    Color color = material.color;
                    color.a = 0f; // Transparent
                    material.color = color;

                    if (collider != null)
                    {
                        collider.enabled = false;
                    }
                }
            }
        }

        UpdateVisibleStarsCount();
    }

    private void UpdateVisibleStarsCount()
    {
        // Actualizar el contador de estrellas visibles
        GameObject starContainer = GameObject.Find("StarContainer");
        if (starContainer == null || counterText == null)
        {
            return;
        }

        int visibleStars = 0;
        foreach (Transform star in starContainer.transform)
        {
            Renderer renderer = star.GetComponent<Renderer>();
            if (renderer != null && renderer.material.color.a > 0.5f) // Contar solo estrellas visibles
            {
                visibleStars++;
            }
        }

        counterText.text = $"{visibleStars}"; // Actualizar el texto del contador
    }

    public void ResetFilters()
    {
        // Reset gender filters
        showMasculine = true;
        showFeminine = true;

        // Use a temporary list to avoid modifying the dictionary while iterating
        var regionKeys = new List<string>(regionFilters.Keys);
        foreach (var lugar_significativoLabel in regionKeys)
        {
            regionFilters[lugar_significativoLabel] = true; // Set all regions to active
            Debug.Log($"Region {lugar_significativoLabel} reset to active.");
        }

        // Also reset normalizedRegionFilters
        if (normalizedRegionFilters != null)
        {
            var normalizedKeys = new List<string>(normalizedRegionFilters.Keys);
            foreach (var nk in normalizedKeys)
            {
                normalizedRegionFilters[nk] = true;
            }
        }

        // Reset sliders to their default values
        if (deathFilterSlider != null)
        {
            deathFilterSlider.value = deathFilterSlider.maxValue; // Reset to max value
        }

        // Apply the filters to reset the scene
        ApplyCombinedFilters();
    }

    public void ToggleRegionFilter(string lugar_significativoLabel)
    {
        // Normalize the provided label for lookup
        string normalizedLabel = NormalizeText(lugar_significativoLabel);

        // Try to toggle the normalized filter state
        if (normalizedRegionFilters != null && normalizedRegionFilters.ContainsKey(normalizedLabel))
        {
            normalizedRegionFilters[normalizedLabel] = !normalizedRegionFilters[normalizedLabel];

            // Also update the human-readable regionFilters dictionary (used for button colors)
            string originalKey = regionFilters.Keys.FirstOrDefault(k => NormalizeText(k) == normalizedLabel);
            if (!string.IsNullOrEmpty(originalKey))
            {
                regionFilters[originalKey] = normalizedRegionFilters[normalizedLabel];
            }

            ApplyRegionFilter(); // Reapply the filter

            // Update the button color (use original label passed in)
            UpdateRegionButtonColor(lugar_significativoLabel);
        }
        else
        {
            Debug.LogError($"Region {lugar_significativoLabel} (normalized: {normalizedLabel}) not found in normalizedRegionFilters dictionary.");
        }
    }

    private void ApplyRegionFilter()
    {
        var starDataDictionary = StarDataHandler.Instance.GetAllStarData();

        foreach (var starEntry in starDataDictionary)
        {
            string objectName = starEntry.Key;
            StarData starData = starEntry.Value;

            GameObject starObject = GameObject.Find(objectName);
            if (starObject != null)
            {
                Renderer renderer = starObject.GetComponent<Renderer>();
                Collider collider = starObject.GetComponent<Collider>();

                // Use the lugar_significativoLabel from StarDataHandler and normalize it
                string starRegion = starData.Region;
                Debug.Log($"Filtering star: {objectName}, Region: {starRegion}");

                // Normalize the star region for lookup
                string normalizedStarRegion = NormalizeText(starRegion);

                // Stars with "Desconocida" should always be visible
                bool isVisible = starRegion == "Desconocida" || (normalizedRegionFilters != null && normalizedRegionFilters.ContainsKey(normalizedStarRegion) && normalizedRegionFilters[normalizedStarRegion]);

                if (isVisible)
                {
                    // Show the star
                    if (renderer != null)
                    {
                        Material material = renderer.material;
                        Color color = material.color;
                        color.a = 1f; // Fully opaque
                        starObject.transform.localScale = Vector3.one * 15f; // Scale up the matching star
                        material.color = color;
                    }

                    if (collider != null)
                    {
                        collider.enabled = true;
                    }
                }
                else
                {
                    // Hide the star
                    if (renderer != null)
                    {
                        Material material = renderer.material;
                        Color color = material.color;
                        color.a = 0f; // Transparent
                        starObject.transform.localScale = Vector3.one * 0.5f; // Scale down the non-matching star
                        material.color = color;
                    }

                    if (collider != null)
                    {
                        collider.enabled = false;
                    }
                }
            }
        }

        UpdateVisibleStarsCount();
    }

    private void UpdateRegionButtonColor(string lugar_significativoLabel)
    {
        Button regionButton = null;

        // Find the corresponding button for the lugar_significativoLabel
        switch (lugar_significativoLabel)
        {
            case "Andina":
                regionButton = andinaRegionButton;
                break;
            case "Amazónica":
                regionButton = amazonicaRegionButton;
                break;
            case "Orinoquía":
                regionButton = orinoquiaRegionButton;
                break;
            case "Pacífica":
                regionButton = pacificaRegionButton;
                break;
            case "Caribe":
                regionButton = caribeInsularRegionButton;
                break;
            default:
                Debug.LogError($"No button assigned for lugar_significativoLabel: {lugar_significativoLabel}");
                return;
        }

        // Update the button's color based on the lugar_significativoLabel's state
        if (regionButton != null)
        {
            Image buttonImage = regionButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                if (regionFilters[lugar_significativoLabel])
                {
                    // Set the button's color to the specific color for the lugar_significativoLabel
                    buttonImage.color = regionColors[lugar_significativoLabel];
                }
                else
                {
                    // Set the button's color to grey when inactive
                    buttonImage.color = Color.grey;
                }
            }
            else
            {
                Debug.LogError($"Button for lugar_significativoLabel {lugar_significativoLabel} does not have an Image component.");
            }
        }
    }

    private static Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }
        else
        {
            Debug.LogError($"Invalid HEX color code: {hex}");
            return Color.white; // Default to white if the HEX code is invalid
        }
    }

    void Update()
    {
        // Check if the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // Trigger the FilterStars method
            FilterStars();

            // Hide the dropdown results (disable the ScrollRect parent if present)
            if (scrollContent != null)
            {
                var scrollRect = scrollContent.GetComponentInParent<ScrollRect>();
                if (scrollRect != null)
                {
                    scrollRect.gameObject.SetActive(false);
                }
                else
                {
                    // If no ScrollRect parent, hide the content itself
                    scrollContent.gameObject.SetActive(false);
                }
            }

            // Also disable the optional objectToDisableOnSelect if assigned
            if (objectToDisableOnSelect != null)
            {
                objectToDisableOnSelect.SetActive(false);
            }
        }
    }

    public void ResetSearchFilter()
    {
        // Clear the input text
        if (searchInputField != null)
        {
            searchInputField.text = string.Empty;
        }

        // Reapply the filter to reset the visibility of all stars
        FilterStars();

        Debug.Log("Search filter and input text have been reset.");
    }

    void SpawnStarsByRegion()
    {
        GameObject starContainer = GameObject.Find("StarContainer");
        if (starContainer == null)
        {
            Debug.LogError("StarContainer not found in the scene.");
            return;
        }

        // Group data by lugar_significativoLabel
        Dictionary<string, List<WikidataBinding>> regionGroups = new Dictionary<string, List<WikidataBinding>>();
        foreach (var data in wikidataObjects)
        {
            string lugar_significativoLabel = data.lugar_significativoLabel?.value?.Replace("Región ", "").Replace(" de Colombia", "").Trim() ?? "Unknown";

            if (!regionGroups.ContainsKey(lugar_significativoLabel))
            {
                regionGroups[lugar_significativoLabel] = new List<WikidataBinding>();
            }

            regionGroups[lugar_significativoLabel].Add(data);
        }

        // Sphere parameters
        float sphereRadius = 10f; // Radius of the main sphere
        float clusterSpread = 0.3f; // Spread of objects within the cluster
        float minDistance = 0.5f; // Minimum distance between objects
        int maxObjectsPerCluster = 20;

        foreach (var regionGroup in regionGroups)
        {
            string lugar_significativoLabel = regionGroup.Key;
            List<WikidataBinding> regionData = regionGroup.Value;

            Debug.Log($"Spawning stars for lugar_significativoLabel: {lugar_significativoLabel}, Count: {regionData.Count}");

            int totalClusters = Mathf.CeilToInt((float)regionData.Count / maxObjectsPerCluster);

            for (int clusterIndex = 0; clusterIndex < totalClusters; clusterIndex++)
            {
                // Calculate a random position on the sphere for the cluster
                float thetaCluster = Random.Range(0f, Mathf.PI * 2); // Longitude
                float phiCluster = Random.Range(0f, Mathf.PI);      // Latitude

                float clusterX = sphereRadius * Mathf.Sin(phiCluster) * Mathf.Cos(thetaCluster);
                float clusterY = sphereRadius * Mathf.Sin(phiCluster) * Mathf.Sin(thetaCluster);
                float clusterZ = sphereRadius * Mathf.Cos(phiCluster);

                Vector3 clusterPosition = new Vector3(clusterX, clusterY, clusterZ).normalized * sphereRadius + starContainer.transform.position;

                // Spawn a cylinder from the StarContainer to the cluster center
                SpawnCylinder(starContainer.transform.position, clusterPosition, defaultMaterial);

                // Spawn stars in the cluster
                int startIndex = clusterIndex * maxObjectsPerCluster;
                int endIndex = Mathf.Min(startIndex + maxObjectsPerCluster, regionData.Count);

                List<Vector3> placedPositions = new List<Vector3>(); // Track placed positions to enforce minimum distance

                for (int i = startIndex; i < endIndex; i++)
                {
                    Vector3 position;
                    int attempts = 0;
                    const int maxAttempts = 100; // Prevent infinite loops

                    do
                    {
                        // Generate a random position within the cluster
                        float offsetTheta = Random.Range(-clusterSpread, clusterSpread); // Small angular offset
                        float offsetPhi = Random.Range(-clusterSpread, clusterSpread);  // Small angular offset

                        float objectTheta = thetaCluster + offsetTheta;
                        float objectPhi = phiCluster + offsetPhi;

                        float objectX = sphereRadius * Mathf.Sin(objectPhi) * Mathf.Cos(objectTheta);
                        float objectY = sphereRadius * Mathf.Sin(objectPhi) * Mathf.Sin(objectTheta);
                        float objectZ = sphereRadius * Mathf.Cos(objectPhi);

                        position = new Vector3(objectX, objectY, objectZ).normalized * sphereRadius + starContainer.transform.position;

                        attempts++;
                    }
                    while (!IsPositionValid(position, placedPositions, minDistance) && attempts < maxAttempts);

                    if (attempts >= maxAttempts)
                    {
                        Debug.LogWarning("Could not find a valid position for an object in the cluster.");
                        continue;
                    }

                    placedPositions.Add(position); // Add the valid position to the list
                    CreateStar(regionData[i], position, starContainer);
                }
            }
        }
    }

    bool IsPositionValid(Vector3 position, List<Vector3> placedPositions, float minDistance)
    {
        foreach (var placedPosition in placedPositions)
        {
            if (Vector3.Distance(position, placedPosition) < minDistance)
            {
                return false; // Position is too close to an existing object
            }
        }
        return true; // Position is valid
    }
}
