namespace _.Scripts.Food
{
    public class Banana : Food
    {
        protected override void DoOnCollision(PlayerController player)
        {
            player.AddBodyTile();
        }
    }
}
