using UnityEngine;

namespace BigRookGames.Weapons
{
    public class GunfireController : MonoBehaviour
    {
        // --- Audio ---
        [Header("Audio")]
        public AudioClip GunShotClip;
        public AudioSource source;
        public Vector2 audioPitch = new Vector2(.9f, 1.1f);

        // --- Muzzle ---
        [Header("Muzzle")]
        public GameObject muzzlePrefab;
        public Transform muzzlePosition;

        // --- Shooting ---
        [Header("Shooting")]
        public float shotDelay = 0.5f;
        public Transform player;
        public bool enemyIsAttacking = false;

        // --- Damage ---
        [Header("Damage")]
        public float range = 60f;
        public float damage = 10f;

        // --- Internal ---
        private float timeLastFired;

        private void Update()
        {
            if (!enemyIsAttacking || player == null || muzzlePosition == null)
                return;

            if (Time.time >= timeLastFired + shotDelay)
            {
                FireWeapon();
            }
        }

        public void FireWeapon()
        {
            timeLastFired = Time.time;

            // --- Muzzle flash ---
            if (muzzlePrefab)
            {
                Instantiate(muzzlePrefab, muzzlePosition.position, muzzlePosition.rotation, muzzlePosition);
            }

            // --- Audio ---
            if (source && GunShotClip)
            {
                source.pitch = Random.Range(audioPitch.x, audioPitch.y);
                source.PlayOneShot(GunShotClip);
            }

            // --- Raycast toward player ---
            Vector3 direction = (player.position + Vector3.up * 1.5f) - muzzlePosition.position;
            direction.Normalize();

            RaycastHit hit;
            if (Physics.Raycast(muzzlePosition.position, direction, out hit, range))
            {
                Debug.DrawLine(muzzlePosition.position, hit.point, Color.red, 0.2f);

                // Check if we hit the player (by tag)
                if (hit.collider.CompareTag("Player"))
                {
                    CharacterStats stats = hit.collider.GetComponent<CharacterStats>();
                    if (stats != null)
                    {
                        stats.changeHealth(-damage);
                        Debug.Log($"Hit player! Health reduced by {damage}");
                    }
                }
                else
                {
                    Debug.Log($"Hit non-player object: {hit.collider.name}");
                }
            }
            else
            {
                Debug.Log("Missed player");
            }
        }
    }
}
