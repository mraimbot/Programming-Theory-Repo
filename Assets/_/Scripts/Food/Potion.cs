using UnityEngine;

namespace _.Scripts.Food
{
    public class Potion : Food
    {
        protected override void DoOnCollision(PlayerController player)
        {
            player.RemoveBody();
        }
    }
}
