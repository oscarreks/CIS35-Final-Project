using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldDragPlaceSpell : HoldDragPlaceUnit {

    public GameObject _spell;

    protected override void setBounds()
    {
        _right_bounds = 22;
        _left_bounds = 0;
    }

	protected override void spawnUnit()
    {
        SoundManager.instance.Play(spawn_sound);

        GameManager.instance.mana[(int)_team] -= _cost;
        manaUpdate();

        GameObject temp = Instantiate(_spell, snapToGrid(CurrentTouchPosition), Quaternion.identity);
        temp.GetComponent<Spell>().startDamage(_team, snapToGrid(CurrentTouchPosition));
    }

}
