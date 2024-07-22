using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platformer {
    public class RandomSpawnPointStratergy : ISpawnPointStratergy {

        List<Transform> unusedSpawnPoints;
        Transform[] spawnPoint;

        public RandomSpawnPointStratergy(Transform[] spawnPoint ) {
            this.spawnPoint = spawnPoint;
            unusedSpawnPoints = new List<Transform>(spawnPoint);
        }

        public Transform NextSpawnPoint() {
            if (!unusedSpawnPoints.Any()) {
                unusedSpawnPoints = new List<Transform>(spawnPoint);
            }

            var randomIndex = Random.Range(0, unusedSpawnPoints.Count);
            Transform result = unusedSpawnPoints[randomIndex];
            unusedSpawnPoints.RemoveAt(randomIndex);
            return result;

        }
    }
}
