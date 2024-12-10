using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarvestUI : MonoBehaviour
{
    public Text fruitCountsText; // Reference to the Text UI element

    // Counters for each type of fruit
    private int appleCount = 0;
    private int grapeCount = 0;
    private int bananaCount = 0;
    private int goldenBananaCount = 0;

    // Update the UI text with current counts
    private void UpdateUI()
    {
        fruitCountsText.text = $"Apple: {appleCount}\n" +
                               $"Grape: {grapeCount}\n" +
                               $"Banana: {bananaCount}\n" +
                               $"Golden Banana: {goldenBananaCount}";
    }

    // Methods to increment fruit counts and update the UI
    public void AddApple()
    {
        appleCount++;
        UpdateUI();
    }

    public void AddGrape()
    {
        grapeCount++;
        UpdateUI();
    }

    public void AddBanana()
    {
        bananaCount++;
        UpdateUI();
    }

    public void AddGoldenBanana()
    {
        goldenBananaCount++;
        UpdateUI();
    }
}
