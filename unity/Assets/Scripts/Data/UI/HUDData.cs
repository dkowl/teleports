using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDData {

	[SerializeField] private GameObject healthbar;
    [SerializeField] private GameObject targetMarker;
    [SerializeField] private GameObject floatingText;
    [SerializeField] private GameObject enemyIndicator;
    [SerializeField] private GameObject hudPrefab;
    [SerializeField] private GameObject endScreen;

    public GameObject Healthbar => healthbar;
    public GameObject TargetMarker => targetMarker;
    public GameObject FloatingText => floatingText;
    public GameObject EnemyIndicator => enemyIndicator;
    public GameObject HUDPrefab => hudPrefab;
    public GameObject EndScreen => endScreen;
}
