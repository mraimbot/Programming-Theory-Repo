namespace _.Scripts.Food
{
    public class Apple : Food
    {
        protected override void DoOnCollision(PlayerController player)
        {
            player.AddBodyTile();
        }
    }
}
