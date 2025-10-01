# PixelRivals — Press PLAY

Questo branch include un Bootstrap che genera automaticamente una scena di test all’avvio dell’Editor:
- Arena base, luce e camera top-down
- Player con joystick virtuale, attacco base e ultimate
- 3 Bot nemici
- Proiettile e salute minimi

Istruzioni:
1) Apri il progetto in Unity (URP, 2022.3+ consigliato).
2) Premi direttamente PLAY. Non serve nessun setup manuale.

Note:
- I dati di classe/skin sono creati in memoria per il test. Gli ScriptableObject e le Skin asset possono essere salvati successivamente in `Assets/Data/`.
- Per build mobile, aggiungi Vulkan/Metal in Player Settings e controlla i permessi touch.