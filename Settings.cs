using Satchel.BetterMenus;

namespace FuryAlwaysOn
{
	public static class Settings
	{
		public static bool audioDisabled, particleDisabled, burstDisabled, vignetteDisabled;
		public static Menu MenuRef;

		public static Menu PrepareMenu()
		{
			return new Menu("Visual Effects", new Element[]
			{
				new HorizontalOption(
					"Disable audio effects",
					"Disables the audio effects that activate with Fury of the Fallen",
					new string[] { "True", "False" },
					(setting) =>
					{
						audioDisabled = setting == 0;
					},
					() => audioDisabled ? 0 : 1
				),
				new HorizontalOption(
					"Disable particle effects effects",
					"Disables the particle effects that activate with Fury of the Fallen",
					new string[] { "True", "False" },
					(setting) =>
					{
						particleDisabled = setting == 0;
					},
					() => particleDisabled ? 0 : 1
				),
				new HorizontalOption(
					"Disable burst effect",
					"Disables the burst effect that activate with Fury of the Fallen",
					new string[] { "True", "False" },
					(setting) =>
					{
						burstDisabled = setting == 0;
					},
					() => burstDisabled ? 0 : 1
				),
				new HorizontalOption(
					"Disable vignette",
					"Disables the screen vignette that activate with Fury of the Fallen",
					new string[] { "True", "False" },
					(setting) =>
					{
						vignetteDisabled = setting == 0;
					},
					() => vignetteDisabled ? 0 : 1
				)
			});
		}

		public static MenuScreen GetMenu(MenuScreen lastMenu)
		{
			if (MenuRef == null)
			{
				MenuRef = PrepareMenu();
			}
			return MenuRef.GetMenuScreen(lastMenu);
		}
	}
}