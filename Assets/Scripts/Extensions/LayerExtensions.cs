using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public static class LayerExtension
{
	public static bool IsMatch(this LayerMask mask, int layer)
	{
		return mask.value == (mask.value | (1 << layer));
	}

	public static void IgnoreCollision(this LayerMask mask, int layer, bool ignore)
	{
		uint bitstring = (uint)mask.value;
		for (int i = 31; bitstring > 0; i--)
			if ((bitstring >> i) > 0)
			{
				bitstring = ((bitstring << 32 - i) >> 32 - i);
				Physics2D.IgnoreLayerCollision(layer, i);
			}
	}

	public static List<int> MaskToLayers(this LayerMask mask)
	{
		var layers = new List<int>();
		uint bitstring = (uint)mask.value;
		for (int i = 31; bitstring > 0; i--)
			if ((bitstring >> i) > 0)
			{
				bitstring = ((bitstring << 32 - i) >> 32 - i);
				layers.Add(i);
			}

		return layers;
	}
}