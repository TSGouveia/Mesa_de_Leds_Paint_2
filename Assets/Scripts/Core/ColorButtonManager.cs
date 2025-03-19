using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ColorButtonManager : MonoBehaviour
{
    public string paletteFilePath = "Assets/palette.txt"; // Path to the .txt palette file
    public GameObject buttonPrefab; // Button prefab to be instantiated
    public Transform buttonPanel; // Panel where buttons will be added

    private List<Color> colorPalette = new List<Color>(); // List to hold the color values

    void Start()
    {
        // Load colors from the palette file
        LoadPalette(paletteFilePath);

        // Create buttons for each color in the palette
        CreateButtonsWithColors();
    }

    // Function to load the palette from the .txt file
    void LoadPalette(string filePath)
    {
        if (File.Exists(filePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                // Skip comments (lines starting with ;)
                if (line.StartsWith(";") || string.IsNullOrWhiteSpace(line))
                    continue;

                // Parse the HEX color string (first 2 characters are alpha, next 6 are RGB)
                string hexColor = line.Trim(); // Remove any extra spaces or newlines
                if (hexColor.Length == 8) // Ensure it's in the correct format: AARRGGBB
                {
                    // Extract alpha (AA), red (RR), green (GG), blue (BB) from the string
                    byte alpha = byte.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    byte red = byte.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    byte green = byte.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    byte blue = byte.Parse(hexColor.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

                    // Convert to Unity's Color (where each component is between 0 and 1)
                    Color color = new Color(red / 255f, green / 255f, blue / 255f, alpha / 255f);
                    colorPalette.Add(color); // Add the color to the list
                }
                else
                {
                    Debug.LogWarning($"Invalid color code (must be AARRGGBB): {line}");
                }
            }
        }
        else
        {
            Debug.LogError("Palette file not found!");
        }
    }

    // Function to create buttons and apply colors from the palette
    void CreateButtonsWithColors()
    {
        foreach (var color in colorPalette)
        {
            // Instantiate the button prefab
            GameObject newButton = Instantiate(buttonPrefab, buttonPanel);

            // Get the Image component of the button to apply the color
            Image buttonImage = newButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = color; // Set the button color
            }

            // Optionally, set button text (e.g., display the color name or HEX)
            Text buttonText = newButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = ColorUtility.ToHtmlStringRGBA(color); // Display the color in RGBA format (e.g., #RRGGBBAA)
            }
        }
    }
}
