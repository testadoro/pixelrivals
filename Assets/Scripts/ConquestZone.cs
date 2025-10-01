using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Conquest zone that players can capture
/// </summary>
public class ConquestZone : MonoBehaviour
{
    [SerializeField] private float captureSpeed = 1f;
    [SerializeField] private Material neutralMaterial;
    [SerializeField] private Material playerMaterial;
    [SerializeField] private Material enemyMaterial;

    private float captureProgress = 0f; // -100 to 100, negative = enemy, positive = player
    private int playersInZone = 0;
    private int enemiesInZone = 0;
    private MeshRenderer meshRenderer;

    public float CaptureProgress => captureProgress;
    public string ControllingTeam
    {
        get
        {
            if (captureProgress > 50f) return "Player";
            if (captureProgress < -50f) return "Enemy";
            return "Neutral";
        }
    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        // Update capture progress based on who's in the zone
        if (playersInZone > enemiesInZone)
        {
            captureProgress += captureSpeed * Time.deltaTime;
        }
        else if (enemiesInZone > playersInZone)
        {
            captureProgress -= captureSpeed * Time.deltaTime;
        }

        captureProgress = Mathf.Clamp(captureProgress, -100f, 100f);

        // Update visual
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (meshRenderer == null) return;

        if (captureProgress > 50f && playerMaterial != null)
        {
            meshRenderer.material = playerMaterial;
        }
        else if (captureProgress < -50f && enemyMaterial != null)
        {
            meshRenderer.material = enemyMaterial;
        }
        else if (neutralMaterial != null)
        {
            meshRenderer.material = neutralMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInZone++;
        }
        else if (other.GetComponent<BotAI>() != null)
        {
            enemiesInZone++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInZone = Mathf.Max(0, playersInZone - 1);
        }
        else if (other.GetComponent<BotAI>() != null)
        {
            enemiesInZone = Mathf.Max(0, enemiesInZone - 1);
        }
    }
}
