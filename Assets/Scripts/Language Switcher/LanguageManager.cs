using UnityEngine;
using I2.Loc;

public class LanguageManager : MonoBehaviour
{
    /// <summary>
    /// Changes the language and refreshes the UI.
    /// </summary>
    /// <param name="languageName">The name of the language to switch to.</param>
    public void ChangeLanguage(string languageName)
    {
        // Attempt to change the language
        if (LocalizationManager.HasLanguage(languageName))
        {
            // Set the current language in I2 Localization
            LocalizationManager.CurrentLanguage = languageName;
            Debug.Log($"Language successfully changed to: {languageName}");

            // Refresh all localized components to reflect the new language
            LocalizationManager.LocalizeAll(true);

            // Debug: Confirm all localized components have updated
            ValidateLocalizedComponents();
        }
        else
        {
            Debug.LogError($"Language '{languageName}' is not available. Please add it to the I2 Localization system.");
        }
    }

    /// <summary>
    /// Validates and logs all localized components in the scene to ensure they are updated.
    /// </summary>
    private void ValidateLocalizedComponents()
    {
        var localizedComponents = FindObjectsOfType<Localize>();
        if (localizedComponents.Length == 0)
        {
            Debug.LogWarning("No localized components found in the scene. Ensure all UI elements have a 'Localize' component attached.");
        }
        else
        {
            foreach (var component in localizedComponents)
            {
                Debug.Log($"Localized Component: {component.name} is using term: {component.Term}");
            }
        }
    }
}
