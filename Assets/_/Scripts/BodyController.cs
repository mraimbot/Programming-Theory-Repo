using System.Collections;
using UnityEngine;

namespace _.Scripts
{
    public class BodyController : AutoMovement
    {
        [SerializeField] private GameObject prefabBody;
        [SerializeField] private GameObject nextTarget;
        
        [SerializeField] private float nextBodySpawnDelay;
        [SerializeField] private float scaleBoundaryUp;
        [SerializeField] private float scaleBoundaryDown;
        [SerializeField] private float scaleSpeed;
        
        private bool doGrow;

        private GameObject target;
        private BodyController nextBody;

        protected override void DoOnStart()
        {
            StartMovement();
        }

        protected override void DoOnUpdate()
        {
            UpdateScaling();
            LookAtTarget();
        }

        private void LookAtTarget()
        {
            if (target == null)
            {
                target = GameObject.Find("Target Player");
                return;
            }
            transform.LookAt(target.transform, Vector3.up);
        }

        private void UpdateScaling()
        {
            var tf = transform;
            var scale = tf.localScale.x;

            if (scale > scaleBoundaryUp)
            {
                scale = scaleBoundaryUp;
                doGrow = false;
            }
            else if (scale < scaleBoundaryDown)
            {
                scale = scaleBoundaryDown;
                doGrow = true;
            }

            if (doGrow)
            {
                var newScale = scale + scaleSpeed * Time.deltaTime;
                tf.localScale = new Vector3(newScale, 1.0f, newScale);
            }
            else
            {
                var newScale = scale - scaleSpeed * Time.deltaTime;
                tf.localScale = new Vector3(newScale, 1.0f, newScale);
            }
        }
        
        public void AddBody()
        {
            if (nextBody == null)
            {
                nextBody = Instantiate(prefabBody, nextTarget.transform.position, prefabBody.transform.rotation).GetComponent<BodyController>();
                nextBody.target = nextTarget;
            }
            else
            {
                nextBody.AddBody();
            }
        }

        public void RemoveBody()
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

        protected override void DoOnStopMovement()
        {
            if (nextBody != null)
            {
                nextBody.StopMovement();
            }
        }
    }
}
