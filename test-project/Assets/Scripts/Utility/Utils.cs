using UnityEngine;

namespace Utility {
    
    public static class Utils {
        
        // returns a random direction
        public static Vector2 RandomDirection() {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

    }

}
