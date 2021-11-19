namespace _.Scripts.Food
{
    public class Potion : Food // INHERITANCE
    {
        protected override void DoOnCollision(PlayerController player) // POLYMORPHISM
        {
            player.RemoveBody();
        }
    }
}
