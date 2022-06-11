﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace Jewelcrafting.GemEffects;

public static class ShadowHit
{
	[HarmonyPatch(typeof(Character), nameof(Character.Damage))]
	private class SpawnShadowSword
	{
		private static void Postfix(Character __instance, HitData hit)
		{
			if (hit.GetAttacker() is Player player && Random.value < player.GetEffect(Effect.Shadowhit) / 100f)
			{
				List<Character> characters = Object.FindObjectsOfType<Character>().Where(c => c != __instance && !c.IsPlayer() && Vector3.Distance(player.transform.position, c.transform.position) < 5f).ToList();
				if (characters.Count > 0)
				{
					Character character = characters[Random.Range(0, characters.Count)];
					GameObject sword = Object.Instantiate(Jewelcrafting.swordFall, character.gameObject.transform);
					sword.transform.localPosition = new Vector3(0, 2, 0);
					character.StartCoroutine(DamageEnemy(character, hit)); 
				}
			}
		}

		private static IEnumerator DamageEnemy(Character character, HitData hit)
		{
			yield return new WaitForSeconds(1.3f);
			hit = hit.Clone();
			hit.m_point = character.transform.position;
			character.Damage(hit);
		}
	}
}
