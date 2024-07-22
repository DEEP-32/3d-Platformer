using UnityEngine;
using Utils;

namespace Platformer {
    public class CollectibleSpawnManager : EntitySpawnManager {

        [SerializeField] CollectableData[] collectableData;
        [SerializeField] float spawnInterval = 1f;


        EntitySpawner<Collectable> spawner;

        CountdownTimer spawnTimer;
        int counter;


        protected override void Awake() {
            base.Awake();

            spawner = new EntitySpawner<Collectable>(
                new EntityFactory<Collectable>(collectableData),
                spawnPointStratergy
           );

            spawnTimer = new CountdownTimer(spawnInterval);
            spawnTimer.OnTimerStop += () => {

                if(counter ++ > collectableData.Length) {
                    spawnTimer.Stop();
                    return;
                }

                Spawn();
                counter++;
                spawnTimer.Start();
            };
        }

        private void Update() {
            spawnTimer.Tick(Time.deltaTime);
        }

        private void Start() {
            spawnTimer.Start();
        }

        public override void Spawn() {
            spawner.Spawn();
        }

    }
}
