using System.Collections;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    public GameObject[] GrowthStages; // Include Sprout, Growing, and Harvestable stages
    public GameObject harvestablePrefab; // Reference to the harvestable prefab

    public float GrowthTime = 3f; // Time between growth stages
    private int currentStage = 0;

    public void WaterPlant()
    {
        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        while (currentStage < GrowthStages.Length - 1)
        {
            yield return new WaitForSeconds(GrowthTime);

            currentStage++;
            UpdateGrowthStage(); // Update visual for current growth stage
        }

        // Spawn the harvestable prefab when the plant is fully grown
        if (harvestablePrefab != null && transform.parent != null)
        {
            // Instantiate the harvestable prefab at the soil's position
            GameObject harvestable = Instantiate(harvestablePrefab, transform.parent.position, Quaternion.identity, transform.parent);

            // adjust y-coord of plant so not covered by soil
            Vector3 newPos = harvestable.transform.position;
            newPos.y += 0.3f;
            harvestable.transform.position = newPos;
            Debug.Log($"Harvestable prefab spawned: {harvestablePrefab.name}");

            if (transform.parent != null && transform.parent.GetComponent<SoilInteraction>() != null)
            {
                
                transform.parent.GetComponent<SoilInteraction>().CurrentState = SoilInteraction.SoilState.Harvestable;
                transform.parent.GetComponent<SoilInteraction>().SetHarvestable(harvestable);
            }


        }
        else
        {
            Debug.LogError("Harvestable prefab or soil parent is missing!");
        }
        
    }


    private void UpdateGrowthStage()
    {
        if (currentStage == 1){
            GrowthStages[0].SetActive(false);
            GrowthStages[1].SetActive(true);
            Debug.Log("Updated from stage 0 to 1.");
        }
        else if (currentStage == 2){
            GrowthStages[1].SetActive(false);
            GrowthStages[2].SetActive(true);
            Debug.Log("Updated from stage 1 to 2. Harvest time!");
        }
        else{
            Debug.LogError($"GrowthStages is null! Check the prefab assignments.");
        }
    }
    //     Debug.Log($"Updating to growth stage {currentStage}. Total stages: {GrowthStages.Length}");

    //     for (int i = 0; i < GrowthStages.Length; i++)
    //     {
    //         if (GrowthStages[i] != null)
    //         {
    //             // Activate only the current stage
    //             GrowthStages[i].SetActive(i == currentStage);
    //             Debug.Log($"Stage {i} - {(i == currentStage ? "Active" : "Inactive")}");
    //         }
    //         else
    //         {
    //             Debug.LogError($"GrowthStages[{i}] is null! Check the prefab assignments.");
    //         }
    //     }



}
