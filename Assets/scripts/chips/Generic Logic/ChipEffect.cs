using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChipEffect {
	public Sprite[] effectAnim;

	public virtual ChipEffect clone() {
		return (ChipEffect)this.MemberwiseClone();
	}

	public abstract void initAnim(Navi navi, ChipLogic c);
	public abstract void OnSyncedUpdate(Navi navi, ChipLogic c);

	public virtual void onHit(Navi navi, int dmg, int stun) {  // for use by "active chips" that provide some effect when navi is hit, such as shielding
		throw new NotImplementedException();
	}
	public virtual void deactivate(Navi navi, ChipLogic c) {
		if(c.chip_renderObj != null) {
			UnityEngine.Object.Destroy(c.chip_renderObj); // removes the chip's sprite
		}
		navi.running_chips.Remove(c);    // removes self from running chips list to no longer be called on synced update
	}
}
