using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PixelRivals.Player;

public class MVPBootstrap : MonoBehaviour
{
    private static bool _bootstrapped;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoBootstrap()
    {
        if (_bootstrapped) return;
        var go = new GameObject("MVPBootstrap");
        Object.DontDestroyOnLoad(go);
        go.AddComponent<MVPBootstrap>();
        _bootstrapped = true;
    }

    private void Start()
    {
        // Ground
        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "ArenaFloor";
        ground.transform.localScale = new Vector3(4f, 1f, 4f);
        ground.GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        // Light
        var lightGO = new GameObject("Directional Light");
        var light = lightGO.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        lightGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // Camera
        var camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        cam.tag = "MainCamera";
        camGO.transform.position = new Vector3(0f, 15f, -12f);
        camGO.transform.rotation = Quaternion.Euler(60f, 0f, 0f);

        // Zone
        var zoneGO = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        zoneGO.name = "CaptureZone";
        zoneGO.transform.position = Vector3.zero;
        zoneGO.transform.localScale = new Vector3(5f, 0.05f, 5f);
        Object.DestroyImmediate(zoneGO.GetComponent<Collider>());
        zoneGO.AddComponent<CaptureZone>();

        // Projectile template
        var projTemplate = new GameObject("ProjectileTemplate");
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.SetParent(projTemplate.transform, false);
        sphere.transform.localScale = Vector3.one * 0.2f;
        var sc = sphere.GetComponent<SphereCollider>();
        sc.isTrigger = true;
        var rb = projTemplate.AddComponent<Rigidbody>();
        rb.useGravity = false;
        projTemplate.AddComponent<Projectile>();

        // Player
        var player = CreateUnit("Player", Color.cyan);
        var pcc = player.AddComponent<CharacterController>();
        pcc.center = new Vector3(0, 1, 0);
        pcc.height = 2f;

        player.AddComponent<Health>();
        var pStats = player.AddComponent<StatsRuntime>();

        var shootOrigin = new GameObject("ShootOrigin");
        shootOrigin.transform.SetParent(player.transform, false);
        shootOrigin.transform.localPosition = new Vector3(0, 1.2f, 0.8f);

        var pCtrl = player.AddComponent<PlayerController>();
        pCtrl.shootOrigin = shootOrigin.transform;
        pCtrl.projectilePrefab = projTemplate;

        // Class (in-memory)
        var classSO = ScriptableObject.CreateInstance<PlayerClassSO>();
        classSO.maxHP = 100; classSO.moveSpeed = 8f;
        classSO.attackDamage = 25f; classSO.attackRate = 0.3f; classSO.attackRange = 10f; classSO.projectileSpeed = 18f;
        classSO.ultimateDamage = 40f; classSO.ultimateRadius = 4f; classSO.ultimateCooldown = 6f;

        var loadout = player.AddComponent<PlayerLoadout>();
        loadout.playerClass = classSO;
        pStats.SetClass(classSO);

        // Bots
        for (int i = 0; i < 3; i++)
        {
            var bot = CreateUnit("Bot_" + (i + 1), Color.red);
            bot.transform.position = new Vector3(6f + i * 1.5f, 0f, 4f + i * 1.5f);

            bot.AddComponent<Health>();
            var bStats = bot.AddComponent<StatsRuntime>();
            var bCtrl = bot.AddComponent<BotController>();

            var bShootOrigin = new GameObject("ShootOrigin");
            bShootOrigin.transform.SetParent(bot.transform, false);
            bShootOrigin.transform.localPosition = new Vector3(0, 1.2f, 0.8f);

            bCtrl.target = player.transform;
            bCtrl.shootOrigin = bShootOrigin.transform;
            bCtrl.projectilePrefab = projTemplate;

            // class: leggermente più lenti
            var botClass = ScriptableObject.CreateInstance<PlayerClassSO>();
            botClass.maxHP = 100; botClass.moveSpeed = 6.5f;
            botClass.attackDamage = 22f; botClass.attackRate = 0.6f; botClass.attackRange = 9f; botClass.projectileSpeed = 16f;
            botClass.ultimateDamage = 35f; botClass.ultimateRadius = 3.5f; botClass.ultimateCooldown = 8f;
            bStats.SetClass(botClass);
        }

        // Position player
        player.transform.position = new Vector3(-6f, 0f, -4f);

        // UI Canvas + Joystick + Buttons
        var canvasGO = new GameObject("HUD", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // EventSystem
        if (FindObjectOfType<EventSystem>() == null)
        {
            var es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            Object.DontDestroyOnLoad(es);
        }

        // Joystick ring
        var ringGO = new GameObject("Joystick", typeof(RectTransform), typeof(Image), typeof(PixelRivals.Inputs.VirtualJoystick));
        ringGO.transform.SetParent(canvasGO.transform, false);
        var ringRT = ringGO.GetComponent<RectTransform>();
        ringRT.anchorMin = new Vector2(0, 0);
        ringRT.anchorMax = new Vector2(0, 0);
        ringRT.pivot = new Vector2(0.5f, 0.5f);
        ringRT.anchoredPosition = new Vector2(200, 200);
        ringRT.sizeDelta = new Vector2(200, 200);
        var ringImg = ringGO.GetComponent<Image>();
        ringImg.color = new Color(1, 1, 1, 0.2f);

        // Joystick knob
        var knobGO = new GameObject("Knob", typeof(RectTransform), typeof(Image));
        knobGO.transform.SetParent(ringGO.transform, false);
        var knobRT = knobGO.GetComponent<RectTransform>();
        knobRT.sizeDelta = new Vector2(100, 100);
        var knobImg = knobGO.GetComponent<Image>();
        knobImg.color = new Color(0.6f, 0.9f, 1f, 0.8f);

        var vj = ringGO.GetComponent<PixelRivals.Inputs.VirtualJoystick>();
        vj.ring = ringRT;
        vj.knob = knobRT;

        // Attack Button
        var atkGO = MakeButton(canvasGO.transform, "AttackBtn", new Vector2(1720, 220), new Vector2(140, 140), new Color(1f, 0.7f, 0.3f, 0.8f), "ATK");
        atkGO.GetComponent<Button>().onClick.AddListener(() =>
        {
            var pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.Attack();
        });

        // Ultimate Button
        var ultGO = MakeButton(canvasGO.transform, "UltimateBtn", new Vector2(1720, 390), new Vector2(120, 120), new Color(0.9f, 0.4f, 1f, 0.8f), "ULT");
        ultGO.GetComponent<Button>().onClick.AddListener(() =>
        {
            var pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.Ultimate();
        });

        // Wire joystick to PlayerController
        var playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.joystick = vj;
    }

    private GameObject CreateUnit(string name, Color color)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        go.name = name;
        var rend = go.GetComponent<Renderer>();
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = color;
        rend.sharedMaterial = mat;

        // Collider configurato per CharacterController; rimuovi CapsuleCollider
        var col = go.GetComponent<CapsuleCollider>();
        if (col != null) Object.Destroy(col);

        return go;
    }

    private GameObject MakeButton(Transform parent, string name, Vector2 anchoredPos, Vector2 size, Color color, string text)
    {
        var btnGO = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        btnGO.transform.SetParent(parent, false);
        var rt = btnGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0, 0);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;
        btnGO.GetComponent<Image>().color = color;

        var labelGO = new GameObject("Label", typeof(RectTransform), typeof(Text));
        labelGO.transform.SetParent(btnGO.transform, false);
        var lrt = labelGO.GetComponent<RectTransform>();
        lrt.anchorMin = lrt.anchorMax = new Vector2(0.5f, 0.5f);
        lrt.pivot = new Vector2(0.5f, 0.5f);
        lrt.anchoredPosition = Vector2.zero;
        lrt.sizeDelta = size;
        var txt = labelGO.GetComponent<Text>();
        txt.text = text;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = Color.black;
        txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        txt.resizeTextForBestFit = true;

        return btnGO;
    }
}