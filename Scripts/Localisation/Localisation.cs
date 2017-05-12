using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Localisation", menuName = "Game Framework/Localisation")]
[System.Serializable]
public class Localisation : ScriptableObject, ISerializationCallbackReceiver
{
    /// <summary>
    /// List of loaded languages.
    /// </summary>
    public Language[] Languages
    {
        get
        {
            return _languages;
        }
    }
    [SerializeField]
    Language[] _languages = new Language[0];

    /// <summary>
    /// List of loaded localisation entries.
    /// </summary>
    public List<LocalisationEntry> LocalisationEntries
    {
        get
        {
            return _localisationEntries;
        }
    }
    [SerializeField]
    List<LocalisationEntry> _localisationEntries = new List<LocalisationEntry>();



    /// <summary>
    /// Dictionary key is the Localisation key, values are array of languages from csv file.
    /// </summary>
    public static Dictionary<string, LocalisationEntry> Localisations
    {
        get
        {
            return _localisations;
        }
    }
    static readonly Dictionary<string, LocalisationEntry> _localisations = new Dictionary<string, LocalisationEntry>(System.StringComparer.Ordinal);

    void PopulateDictionary()
    {
        _localisations.Clear();
        foreach (var localisationEntry in LocalisationEntries)
        {
            _localisations.Add(localisationEntry.Key, localisationEntry);
        }
    }

    public void OnBeforeSerialize()
    {
        LocalisationEntries.Clear();
        foreach (var localisation in Localisations)
        {
            LocalisationEntries.Add(localisation.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        PopulateDictionary();
    }
}


[System.Serializable]
public class LocalisationEntry
{
    public string Key = string.Empty;
    public string Description = string.Empty;
    public string[] Languages = new string[0];
}

[System.Serializable]
public class Language
{
    public string Name = string.Empty;
}