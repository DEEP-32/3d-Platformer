using UnityEngine;

namespace Platformer {
    public abstract class EntitySpawnManager : MonoBehaviour {
        [SerializeField] protected SpawnPointStratergyType spawnPointStratergyType = SpawnPointStratergyType.Linear;
        [SerializeField] protected Transform[] spawnPoints;

        protected ISpawnPointStratergy spawnPointStratergy;

        public enum SpawnPointStratergyType {
            Linear,
            Random
        }

        protected virtual void Awake() {
            spawnPointStratergy = spawnPointStratergyType switch {
                SpawnPointStratergyType.Linear => new LinearSpawnPointStratery(spawnPoints),
                SpawnPointStratergyType.Random => new RandomSpawnPointStratergy(spawnPoints),
                _ => spawnPointStratergy
            };
        }

        public abstract void Spawn();
    }
}
