using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject teleportEffectUI2;

    // Function to open the Earring link
    public void OnEarringButtonClick()
    {
        string earringUrl = "https://somsara.art/products/eclipse-earrings";
        Application.OpenURL(earringUrl);
    }

    // Function to open the Pendant 1 link
    public void OnPendant1ButtonClick()
    {
        string pendant1Url = "https://somsara.art/products/eclipse-pendant-zero";
        Application.OpenURL(pendant1Url);
    }

    // Function to open the Pendant 2 link
    public void OnPendant2ButtonClick()
    {
        string pendant2Url = "https://somsara.art/products/eclipse-pendant-penta";
        Application.OpenURL(pendant2Url);
    }

    // Function to open the Ring link
    public void OnRingButtonClick()
    {
        string ringUrl = "https://somsara.art/products/eclipse-ring";
        Application.OpenURL(ringUrl);
    }

    // Functions for teleport UI effect
    public void ShowTeleportEffect()
    {
        if (teleportEffectUI2) teleportEffectUI2.SetActive(true);
    }

    public void HideTeleportEffect()
    {
        if (teleportEffectUI2) teleportEffectUI2.SetActive(false);
    }
}
