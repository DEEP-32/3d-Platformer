namespace Platformer {
    public class EntitySpawner<T> where T : Entity {
        IEntityFactory<T> entityFactory;
        ISpawnPointStratergy spawnPointStratergy;

        public EntitySpawner(IEntityFactory<T> entityFactory, ISpawnPointStratergy spawnPointStratergy) {
            this.entityFactory = entityFactory;
            this.spawnPointStratergy = spawnPointStratergy;
        }


        public T Spawn() {
            return entityFactory.Create(spawnPointStratergy.NextSpawnPoint());
        }
    }
}
