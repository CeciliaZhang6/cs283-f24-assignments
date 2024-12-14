using UnityEngine;
using System.Collections;

public class SoilInteraction : MonoBehaviour
{
    public enum SoilState { Empty, Planted, Dry, Watered, Harvestable }
    public SoilState CurrentState = SoilState.Empty;

    public GameObject ApplePlantPrefab;
    public GameObject GrapePlantPrefab;
    public GameObject BananaPlantPrefab;
    public GameObject GoldenBananaPlantPrefab;

    private GameObject plantedPlant; // instantiated plant
    private GameObject harvestable; // spawned harvestable prefab
    public float dryTime = 3.0f; 

    public SubtitleManager subtitleManager;

    public void SetHarvestable(GameObject instance)
    {
        harvestable = instance;
    }

    private IEnumerator StartDryingTimer()
    {
        yield return new WaitForSeconds(dryTime);

        if (CurrentState == SoilState.Planted)
        {
            CurrentState = SoilState.Dry;

            // subtitle message
            subtitleManager.AddSubtitle("A soil has dried out.");
            Debug.Log("Soil has dried out.");
        }
    }

    private void OnMouseDown()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        switch (CurrentState)
        {
            case SoilState.Empty:
                PlantSeed();
                break;
            case SoilState.Planted:
                WaterSoil();
                break;
            case SoilState.Harvestable:
                Debug.Log("in harvest");
                HarvestPlant();
                break;
        }
    }

    private void PlantSeed()
    {
        if (CurrentState == SoilState.Empty)
        {
            GameObject selectedPlantPrefab = ChooseRandomPlant();
            if (selectedPlantPrefab != null)
            {
                // instantiate the plant
                plantedPlant = Instantiate(selectedPlantPrefab, transform.position, Quaternion.identity);

                plantedPlant.transform.parent = this.transform;

                // adjust y-coord of plant so not covered by soil
                Vector3 newPos = plantedPlant.transform.position;
                newPos.y += 0.3f;
                plantedPlant.transform.position = newPos;

                CurrentState = SoilState.Planted;
                subtitleManager.AddSubtitle("You planted a seed.");
                Debug.Log($"Planted a {selectedPlantPrefab.name}!");
            }
        }
    }

    private void WaterSoil()
    {
        if (CurrentState == SoilState.Planted && plantedPlant != null)
        {
            // growth logic in PlantGrowth
            plantedPlant.GetComponent<PlantGrowth>().WaterPlant();
            CurrentState = SoilState.Watered;
            subtitleManager.AddSubtitle("You watered the soil.");
            Debug.Log("Watered the soil!");
        }
    }

    private void HarvestPlant()
    {
        if (CurrentState == SoilState.Harvestable && transform.childCount > 0)
        {   
            Destroy(plantedPlant); // remove the growing plant
            Debug.Log($"Harvested a {harvestable.name}!");

            HarvestUI ui = FindObjectOfType<HarvestUI>();
            if (ui != null)
            {
                // find out the type of fruit and update the UI
                if (harvestable.name.Contains("apple"))
                    ui.AddApple();
                else if (harvestable.name.Contains("grape"))
                    ui.AddGrape();
                else if (harvestable.name.Contains("golden"))
                    ui.AddGoldenBanana();
                else if (harvestable.name.Contains("banana"))
                    ui.AddBanana();
            }
            else
            {
                Debug.LogError("HarvestUI not found in the scene!");
            }

            subtitleManager.AddSubtitle("You harvested a plant.");
            // destroy the first child (harvestable prefab)
            Destroy(harvestable);

            CurrentState = SoilState.Empty;
        }
    }


    private GameObject ChooseRandomPlant()
    {
        float randomValue = Random.Range(0f, 100f);
        if (randomValue <= 50f) return ApplePlantPrefab; // 50%
        if (randomValue <= 85f) return GrapePlantPrefab; // 35%
        if (randomValue <= 95f) return BananaPlantPrefab; // 10%
        return GoldenBananaPlantPrefab;                  // 5%
    }
}
