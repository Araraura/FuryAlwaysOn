using Modding;
using USceneManager = UnityEngine.SceneManagement.SceneManager;
using UObject = UnityEngine.Object;
using System.Collections.Generic;
using Satchel.BetterMenus;

namespace FuryAlwaysOn
{
	public class FuryAlwaysOn : Mod, ICustomMenuMod, ITogglableMod
	{
		public static FuryAlwaysOn Instance;

		public override string GetVersion()
		{
			return "0.0.0.0";
		}

		public override void Initialize()
		{
			Instance = this;
			Log("Initializing.");

			Unload();                                            // Ensures two instances of this mod are not working at the same time
			ModHooks.AfterSavegameLoadHook += AfterSaveGameLoad; // Runs when a save file is chosen
			ModHooks.NewGameHook           += AddComponent;
		}

		public bool ToggleButtonInsideMenu => true;
		public Menu MenuRef;

		private void AfterSaveGameLoad(SaveGameData data) => AddComponent();

		private void AddComponent()
		{
			GameManager.instance.gameObject.AddComponent<EffectFixer>(); // Begins class EffectFixer when a new save file loads
		}

		public void Unload()
		{
			ModHooks.AfterSavegameLoadHook -= AfterSaveGameLoad;
			ModHooks.NewGameHook           -= AddComponent;

			var x = GameManager.instance?.gameObject.GetComponent<EffectFixer>();
			if (x == null)
				return;
			UObject.Destroy(x);
		}

		public Menu PrepareMenu(ModToggleDelegates toggleDelegates)
		{
			return new Menu("FuryAlwaysOn Settings", new Element[]
			{
				toggleDelegates.CreateToggle("Mod Toggle", "Turn this setting off to disable the mod entirely"),
				new MenuRow(
					new List<Element>
					{
						Blueprints.NavigateToMenu(
							"Visual Effects",
							"Toggle Fury of the Fallen's visual effects",
							() => Settings.GetMenu(MenuRef.menuScreen)
						)
					},
					Id: "group1"
				) { XDelta = 500f }
			});
		}

		public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
		{
			if (MenuRef == null)
			{
				MenuRef = PrepareMenu((ModToggleDelegates)toggleDelegates);
			}
			return MenuRef.GetMenuScreen(modListMenu);
		}
	}
}