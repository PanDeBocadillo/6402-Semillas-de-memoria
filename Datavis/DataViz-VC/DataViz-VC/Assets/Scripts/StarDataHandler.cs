using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class StarDataHandler : MonoBehaviour
{
    private string fechaMuerte;
    private int añoDeMuerte;
    private string genero;
    private string region;

    private WikidataLoader.WikidataBinding data;
    private StarDataHandler dataHandler;

    // Dictionary to store star data, keyed by the object's name
    private Dictionary<string, StarData> starDataDictionary = new Dictionary<string, StarData>();

    // Singleton instance for global access
    public static StarDataHandler Instance { get; private set; }

    private void Awake()
    {
        // Ensure only one instance of StarDataHandler exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the raw data for processing.
    /// </summary>
    public void SetData(string fechaMuerteRaw, string generoRaw, string regionRaw)
    {
        fechaMuerte = fechaMuerteRaw;
        genero = generoRaw;
        region = regionRaw;

        ProcessFechaMuerte();
        ProcessRegion();
    }

    public void SetData(WikidataLoader.WikidataBinding newData, TextMeshProUGUI nameUI, TextMeshProUGUI dateUI, TextMeshProUGUI regionUI, Image imageUI, Sprite defaultImg, Button button)
    {
        if (newData == null)
        {
            Debug.LogError("New data is null in SetData.");
            return;
        }

        data = newData;

        if (dataHandler == null)
        {
            Debug.LogError("DataHandler is null in SetData.");
            return;
        }

        // Pass raw data to the data handler
        string fechaDeMuerte = data.fecha_de_muerte?.value;
        string genero = data.genero?.value;
        string region = data.lugar_significativoLabel?.value;

        if (fechaDeMuerte == null || genero == null || region == null)
        {
            Debug.LogWarning($"One or more properties are null: fechaDeMuerte={fechaDeMuerte}, genero={genero}, region={region}");
        }

        dataHandler.SetData(fechaDeMuerte, genero, region);

    }

    /// <summary>
    /// Processes the raw fechaMuerte string to extract the year of death.
    /// </summary>
    private void ProcessFechaMuerte()
    {
        if (!string.IsNullOrEmpty(fechaMuerte) && fechaMuerte.Length >= 4)
        {
            string yearString = "";

            if (fechaMuerte.Contains("/")) // Example: "dd/mm/yyyy"
            {
                string[] dateParts = fechaMuerte.Split('/');
                if (dateParts.Length == 3)
                {
                    yearString = dateParts[2];
                }
            }
            else if (fechaMuerte.Length >= 4) // Fallback: take the last 4 characters
            {
                yearString = fechaMuerte.Substring(fechaMuerte.Length - 4);
            }

            if (!string.IsNullOrEmpty(yearString) && int.TryParse(yearString, out int year))
            {
                añoDeMuerte = year;
                Debug.Log($"Year of death processed: {añoDeMuerte}");
            }
            else
            {
                Debug.LogWarning($"Failed to parse year from fechaMuerte: '{fechaMuerte}'");
                añoDeMuerte = -1; // Default value for unknown year
            }
        }
        else
        {
            Debug.LogWarning($"Invalid fechaMuerte: '{fechaMuerte}'");
            añoDeMuerte = -1; // Default value for unknown year
        }
    }

    /// <summary>
    /// Processes the raw region string to clean it up.
    /// </summary>
    private void ProcessRegion()
    {
        if (!string.IsNullOrEmpty(region))
        {
            region = region.Replace("Region ", "").Replace(" de Colombia", "").Trim();
            Debug.Log($"Region processed: {region}");
        }
        else
        {
            Debug.LogWarning("Region is not set or invalid.");
            region = "Desconocida"; // Default value
        }
    }

    /// <summary>
    /// Gets the processed year of death.
    /// </summary>
    public int GetYearOfDeath()
    {
        return añoDeMuerte;
    }

    /// <summary>
    /// Gets the processed region.
    /// </summary>
    public string GetRegion()
    {
        return region;
    }

    /// <summary>
    /// Gets the gender.
    /// </summary>
    public string GetGender()
    {
        return !string.IsNullOrEmpty(genero) ? genero.ToLower() : "unknown";
    }

    /// <summary>
    /// Adds or updates the data for a star object.
    /// </summary>
    public void AddOrUpdateStarData(string objectName, string name, string fechaMuerte, string genero, string region)
    {
        int añoDeMuerte = -1; // Default value for unknown year

        if (!string.IsNullOrEmpty(fechaMuerte) && fechaMuerte.Length >= 4)
        {
            string yearString = "";

            if (fechaMuerte.Contains("-"))
            {
                string[] dateParts = fechaMuerte.Split('-');
                if (dateParts.Length == 3)
                {
                    yearString = dateParts[2];
                }
            }
            else if (fechaMuerte.Length >= 4)
            {
                yearString = fechaMuerte.Substring(fechaMuerte.Length - 4);
            }

            if (!string.IsNullOrEmpty(yearString) && int.TryParse(yearString, out int year))
            {
                añoDeMuerte = year;
            }
        }

        if (starDataDictionary.ContainsKey(objectName))
        {
            // Update existing data
            starDataDictionary[objectName] = new StarData(name, fechaMuerte, añoDeMuerte, genero, region);
        }
        else
        {
            // Add new data
            starDataDictionary.Add(objectName, new StarData(name, fechaMuerte, añoDeMuerte, genero, region));
        }

        Debug.Log($"Star data updated: {objectName} -> {name}, AñoDeMuerte: {añoDeMuerte}, {genero}, {region}");
    }

    /// <summary>
    /// Gets the data for a star object by name.
    /// </summary>
    public StarData GetStarData(string objectName)
    {
        if (starDataDictionary.TryGetValue(objectName, out StarData data))
        {
            return data;
        }

        Debug.LogWarning($"Star data not found for: {objectName}");
        return null;
    }

    /// <summary>
    /// Gets all star data.
    /// </summary>
    public Dictionary<string, StarData> GetAllStarData()
    {
        return starDataDictionary;
    }
}

/// <summary>
/// Class to store star data.
/// </summary>
public class StarData
{
    public string Name { get; private set; }
    public string FechaMuerte { get; private set; }
    public int AñoDeMuerte { get; private set; }
    public string Genero { get; private set; }
    public string Region { get; private set; }

    public StarData(string name, string fechaMuerte, int añoDeMuerte, string genero, string region)
    {
        Name = name;
        FechaMuerte = fechaMuerte;
        AñoDeMuerte = añoDeMuerte;
        Genero = genero;
        Region = region;
    }
}