using UnityEngine;

namespace _.Scripts
{
    public class BodyController : AutoMovement // INHERITANCE
    {
        [SerializeField] private GameObject prefabBody;
        [SerializeField] private GameObject nextTarget;
        
        [SerializeField] private float scaleBoundaryUp;
        [SerializeField] private float scaleBoundaryDown;
        [SerializeField] private float scaleSpeed;
        
        private bool isGrowing;

        private GameObject target;
        private BodyController nextBody;

        protected override void DoOnStart() // POLYMORPHISM
        {
            StartMovement();
        }

        protected override void DoOnUpdate() // POLYMORPHISM
        {
            UpdateScaling();
            LookAtTarget();
        }

        private void LookAtTarget() // ABSTRACTION
        {
            if (target == null)
            {
                target = GameObject.Find("Target Player");
                return;
            }
            transform.LookAt(target.transform, Vector3.up);
        }

        private void UpdateScaling() // ABSTRACTION
        {
            var tf = transform;
            var scale = tf.localScale.x;

            if (scale > scaleBoundaryUp)
            {
                scale = scaleBoundaryUp;
                isGrowing = false;
            }
            else if (scale < scaleBoundaryDown)
            {
                scale = scaleBoundaryDown;
                isGrowing = true;
            }

            var scaleAdjustment = scaleSpeed * Time.deltaTime;
            var newScale = scale + (isGrowing ? scaleAdjustment : -scaleAdjustment);
            tf.localScale = new Vector3(newScale, 1.0f, newScale);
        }
        
        public void AddBody() // ABSTRACTION
        {
            if (nextBody == null)
            {
                nextBody = Instantiate(prefabBody, nextTarget.transform.position, Quaternion.identity).GetComponent<BodyController>();
                nextBody.target = nextTarget;
            }
            else
            {
                nextBody.AddBody();
            }
        }

        public void RemoveBody() // ABSTRACTION
        {
            if (nextBody == null)
            {
                Destroy(gameObject);
            }
            else
            {
                nextBody.RemoveBody();
            }
        }

        protected override void DoOnStopMovement() // POLYMORPHISM
        {
            if (nextBody != null)
            {
                nextBody.StopMovement();
            }
        }
    }
}
