using System;
using UnityEngine;
using UnityModManagerNet;

/*
	Pitstop Trashcans Modification for Derail Valley
	Copyright [2022] [Crypto [Neo]]

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

	    http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
 */

namespace DVPitstopTrashcans
{
	[EnableReloading]
    public class PitstopTrashcans
    {
		static UnityModManager.ModEntry mod;
		static AssetBundle bundle;

		static GameObject trashcan_instance;
		// List of all the pitstops in Derail Valley
		static String[] pitstops = new String[]
		{
			"PitstopSteelMill",
			"PitstopCityShouthWest",
			"PitstopOilWellNorth",
			"PitstopHarbor",
			"PitstopIronOreMineEast",
			"PitstopIronOreMineWest",
			"PitstopMachineFactoryTown",
			"PitstopCoalMine",
			"PitstopOilWellCentral",
			"PitstopGoodsFactory",
			"PitstopFoodFactory"
		};
		static GameObject[] trashcans = new GameObject[pitstops.Length];
		static bool isInitialized = false;
		static bool isEnabled = false;

		// Setup the hooks
		static void Load(UnityModManager.ModEntry modEntry)
		{
			mod = modEntry;
			mod.OnToggle = OnToggle;
			mod.OnUpdate = OnUpdate;			
		}

		// Toggles when the mod is enabled/disabled in the UMM menu
		static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
		{
			isEnabled = !isEnabled;
			if (!isEnabled)
			{
				isInitialized = false;
				foreach (GameObject go in trashcans)
					GameObject.Destroy(go);
				GameObject.Destroy(trashcan_instance);
			}
			return true;
		}

		static void OnUpdate(UnityModManager.ModEntry modEntry, float value)
		{
			// Return if disabled
			if (!isEnabled) return;

			if(!isInitialized)
			{
				// Check if our instance is null
				if (trashcan_instance == null)
				{
					// See if a trashcan instance already exists					
					if (trashcan_instance == null) // No?
					{
						// Verify the bundle is loaded and if it isn't, load it
						if (bundle == null)
							bundle = AssetBundle.LoadFromFile(Application.dataPath + "/../Mods/DVPitstopTrashcans/trashcan.assets");						
						// Spawn a brand new instance of the trashcan
						trashcan_instance = (GameObject)bundle.LoadAsset("Trashcan");											
					}
				}
				
				// Loop through each pitstop on the map and try to spawn a trashcan there
				for(int i = 0; i < pitstops.Length; i++)
				{
					// Get pitstop instance
					GameObject pitstop_instance = GameObject.Find(pitstops[i]);
					if (pitstop_instance == null) return;
					// Spawn a new trashcan and move it beside the pitstop register
					trashcans[i] = GameObject.Instantiate(trashcan_instance);
					trashcans[i].transform.parent = pitstop_instance.transform;
					trashcans[i].transform.localPosition = Vector3.zero + new Vector3(0, 0, 4.70f);
					trashcans[i].transform.localRotation = Quaternion.identity;
					trashcans[i].transform.Rotate(Vector3.up, -90.0f);
					trashcans[i].name = pitstops[i] + "_Trashbin";
				}

				// Declare initialized so we only do this once
				isInitialized = true;
				mod.Logger.Log("Pitstop Trashcans active! Have a nice day!");
			}
		}
    }
}
