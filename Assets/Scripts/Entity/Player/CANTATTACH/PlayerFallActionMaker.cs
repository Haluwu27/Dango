using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PlayerFallActionMaker
{
    GameObject _player;
    GameObject _maker;
    CapsuleCollider _col;

    public PlayerFallActionMaker(GameObject player, GameObject maker, CapsuleCollider col)
    {
        _player = player;
        _maker = maker;
        _col = col;
    }

    public void Update()
    {
        var ray = new Ray(_player.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            _maker.transform.position = hit.point + new Vector3(0, 0.01f, 0);
            
            //突き刺しできるようになったら有効化
            if (!Physics.Raycast(ray, _col.height + _col.height / 2f))
                _maker.SetActive(true);
            else
                _maker.SetActive(false);
        }

    }

}