namespace _.Scripts.Food
{
    public class Apple : Food // INHERITANCE
    {
        protected override void DoOnCollision(PlayerController player) // POLYMORPHISM
        {
            player.AddBody();
        }
    }
}
