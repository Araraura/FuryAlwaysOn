using System.Collections;
using UnityEngine;
using Logger = Modding.Logger;
using USceneManager = UnityEngine.SceneManagement.SceneManager;
using Vasi;
using static FuryAlwaysOn.Settings;

namespace FuryAlwaysOn
{
	public class EffectFixer : MonoBehaviour
	{
		PlayMakerFSM fsm;

		private void Start()
		{
			StartCoroutine(WaitForPlayer());
		}

		IEnumerator WaitForPlayer()
		{
			yield return new WaitWhile(() => HeroController.instance == null); // Waits for Hero to be loaded
			yield return null;
			fsm = GameObject.Find("Charm Effects").LocateMyFSM("Fury");
			fsm.ChangeTransition("Check HP", "CANCEL", "Get Ref");            // Ensures the FSM does not cancel when HP is not at 1
			fsm.GetState("Activate").RemoveTransition("HERO DAMAGED");        // Ensures HP is not checked when hit
			fsm.GetState("Activate").RemoveTransition("HERO HEALED");         // Ensures HP is not checked when healed
			fsm.GetState("Activate").RemoveTransition("HERO HEALED FULL");    // Ensures HP is not checked when at bench
			fsm.GetState("Activate").RemoveTransition("ADD BLUE HEALTH");     // Ensures HP is not checked when Lifeblood is received
			fsm.GetState("Activate").RemoveAction(1);                         // Required(?) to disable the rest of the effects
			fsm.GetState("Activate").Actions[0].Enabled  = !audioDisabled;    // Disables Fury audio effects
			fsm.GetState("Activate").Actions[2].Enabled  = !particleDisabled; // Disables Fury particle effects
			fsm.GetState("Activate").Actions[20].Enabled = !burstDisabled;    // Disables Fury burst effect
			fsm.GetState("Activate").Actions[21].Enabled = !vignetteDisabled; // Disables Fury vignette

			// Gives Grubberfly beams their Fury color
			HeroController.instance.grubberFlyBeamPrefabL = HeroController.instance.grubberFlyBeamPrefabL_fury;
			HeroController.instance.grubberFlyBeamPrefabR = HeroController.instance.grubberFlyBeamPrefabR_fury;
			HeroController.instance.grubberFlyBeamPrefabU = HeroController.instance.grubberFlyBeamPrefabU_fury;
			HeroController.instance.grubberFlyBeamPrefabD = HeroController.instance.grubberFlyBeamPrefabD_fury;

			while (true)
			{
				if (PlayerData.instance.equippedCharm_6 && fsm.ActiveStateName == "Idle") // Turns on Fury when equipped
				{
					fsm.SendEvent("HERO DAMAGED"); // Starts
				}
				if (!PlayerData.instance.equippedCharm_6 && fsm.ActiveStateName == "Activate") // Turns off Fury when not equipped
				{
					fsm.SetState("Deactivate");
				}
				yield return null;
			}
		}

		public static void Log(object o)
		{
			Logger.Log("[EffectFixer] " + o);
		}
	}
}