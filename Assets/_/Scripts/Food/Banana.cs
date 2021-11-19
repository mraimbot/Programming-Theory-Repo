namespace _.Scripts.Food
{
    public class Banana : Food // INHERITANCE
    {
        protected override void DoOnCollision(PlayerController player) // POLYMORPHISM
        {
            player.AddBody();
        }
    }
}
