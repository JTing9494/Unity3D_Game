using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStuff : MonoBehaviour, IDoEStuff
{
	[SerializeField] int id = 0;
	
	public void DoE()
	{
		SaveManager.instance.playerData.�K�[�D��(id);

		Destroy(this.gameObject);
	}
}
