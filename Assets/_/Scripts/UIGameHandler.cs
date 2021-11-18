using UnityEngine;

namespace _.Scripts
{
    public class UIGameHandler : MonoBehaviour
    {
        public void OnBackToMenuClicked()
        {
            GameManager.LoadMenuScene();
        }
    }
}
