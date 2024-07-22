using UnityEngine;

namespace Platformer {
    public class LinearSpawnPointStratery : ISpawnPointStratergy {

        int index = 0;

        Transform[] spawnPoint;

        public LinearSpawnPointStratery(Transform[] spawnPoint) { 
            this.spawnPoint = spawnPoint;
        }

        public Transform NextSpawnPoint() {
            Transform result = spawnPoint[index];
            index = (index + 1) % spawnPoint.Length;
            return result;
        }
    }
}
