using System.Collections.Generic;
using UnityEngine;

namespace ASSIGNMENTS.Minecraft.Scripts {
    public enum ZombieState { Wandering, Chasing }

    public class ZombieAI : MonoBehaviour {
        [Header("player ref")]
        public Transform target;

        [Header("detection settings")]
        public float chaseRange = 10f;
        public float loseRange = 15f;
        public bool useLineOfSight = true;

        [Header("movement settings")]
        public float moveSpeed = 3f;
        public float waypointReachDist = 0.3f;

        [Header("wandering settings")]
        public float wanderRadius = 8f;
        public float wanderPause = 2f;

        [Header("path refresh speed")]
        public float pathRefreshRate = 0.5f;

        [Header("gizmos for pathfinder")]
        public Color pathColor = Color.cyan;
        public Color currentNodeColor = Color.red;
        public Color wanderTargetColor = Color.yellow;

        ZombieState state = ZombieState.Wandering;
        List<Vector3Int> path;
        int pathIndex;
        float pathTimer;
        float wanderTimer;
        bool worldDirty;

        void onEnable() {
            WorldObserver.OnWorldChanged += onWorldChanged;
        }
        void onDisable() {
            WorldObserver.OnWorldChanged -= onWorldChanged;
        }
        void onWorldChanged() {
            worldDirty = true;
        }

        void Update() {
            if (target == null) return;
            float dist = Vector3.Distance(transform.position, target.position);
            switch (state) {
                case ZombieState.Wandering:
                    if (detectionRange(dist))
                        enterChase();
                    else
                        updateWander();
                    break;
                case ZombieState.Chasing:
                    if (dist > loseRange)
                        enterWander();
                    else
                        updateChase();
                    break;
            }
            followPath();
        }

        // transitions
        void enterChase() {
            state = ZombieState.Chasing;
            recalcPath2(getTargetCell());
        }
        void enterWander() {
            state = ZombieState.Wandering;
            path = null;
            wanderTimer = wanderPause;
        }

        // wandering
        void updateWander() {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer > 0) return;

            if (path == null || pathIndex >= path.Count) {
                Vector3Int wanderTarget = findWanderTarget();
                recalcPath2(wanderTarget);
                wanderTimer = wanderPause;
            }
        }

        Vector3Int findWanderTarget() {
            Vector3Int myCell = WorldPos(transform.position);
            for (int i = 0; i < 20; i++) {
                int rx = myCell.x + Random.Range(-(int)wanderRadius, (int)wanderRadius + 1);
                int rz = myCell.z + Random.Range(-(int)wanderRadius, (int)wanderRadius + 1);
                // scan from top down to find a walkable spot
                for (int y = myCell.y + 3; y >= myCell.y - 3; y--) {
                    Vector3Int candidate = new Vector3Int(rx, y, rz);
                    if (isStandable(candidate))
                        return candidate;
                }
            }
            return myCell;
        }

        // chase
        void updateChase() {
            pathTimer -= Time.deltaTime;
            bool pathDone = path == null || pathIndex >= path.Count;
            if (pathTimer <= 0 || worldDirty || pathDone) {
                recalcPath2(getTargetCell());
                pathTimer = pathRefreshRate;
                worldDirty = false;
            }
        }
        
        // zombie movement
        void followPath() {
            if (path == null || pathIndex >= path.Count) return;

            float distToTarget = Vector3.Distance(transform.position, target.position);
            if (distToTarget < 3f) return;

            float yOffset = 0.5f;
            Vector3 targetPos = new Vector3(path[pathIndex].x, path[pathIndex].y + yOffset, path[pathIndex].z);
            Vector3 dir = targetPos - transform.position;

            float hDist = new Vector2(dir.x, dir.z).magnitude;

            if (hDist < waypointReachDist) {
                pathIndex++;
                return;
            }

            Vector3 move = dir.normalized * moveSpeed * Time.deltaTime;
            transform.position += move;

            if (dir.sqrMagnitude > 0.01f) {
                Quaternion look = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, look, 10f * Time.deltaTime);
            }
        }
        
        void recalcPath2(Vector3Int goal) {
            Vector3Int raw = WorldPos(transform.position);
            Vector3Int start = raw;
            for (int y = raw.y + 2; y >= raw.y - 3; y--) {
                Vector3Int candidate = new Vector3Int(raw.x, y, raw.z);
                if (isStandable(candidate)) {
                    start = candidate;
                    break;
                }
            }
            path = Pathfinding.findPath(start, goal);
            pathIndex = (path != null && path.Count > 1) ? 1 : 0;
        }
        
        Vector3Int getTargetCell() {
            Vector3Int raw = WorldPos(target.position);
            // scanning where the player is standing on
            for (int y = raw.y + 2; y >= raw.y - 3; y--) {
                Vector3Int candidate = new Vector3Int(raw.x, y, raw.z);
                if (isStandable(candidate))
                    return candidate;
            }
            return raw;
        }

        Vector3Int WorldPos(Vector3 pos) {
            return new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
        }

        bool detectionRange(float dist) {
            if (dist > chaseRange) return false;
            if (!useLineOfSight) return true;

            Vector3 dir = (target.position - transform.position).normalized;
            float rayDist = Vector3.Distance(transform.position, target.position);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, rayDist,
                    LayerMask.GetMask("Block")))
                return false;
            return true;
        }

        bool isStandable(Vector3Int pos) {
            Vector3 center = new Vector3(pos.x, pos.y, pos.z);
            Vector3 below = center + Vector3.down;
            bool hasGround = Physics.OverlapBox(below, Vector3.one * 0.4f, Quaternion.identity,
                LayerMask.GetMask("Block")).Length > 0;
            bool openFeet = Physics.OverlapBox(center, Vector3.one * 0.4f, Quaternion.identity,
                LayerMask.GetMask("Block")).Length == 0;
            return hasGround && openFeet;
        }
        
        void OnDrawGizmos() {
            if (path == null || path.Count == 0) return;
            for (int i = 0; i < path.Count; i++) {
                Vector3 pos = new Vector3(path[i].x, path[i].y, path[i].z);
                if (i == pathIndex) {
                    Gizmos.color = currentNodeColor;
                    Gizmos.DrawCube(pos, Vector3.one * 0.5f);
                } else {
                    Gizmos.color = pathColor;
                    Gizmos.DrawCube(pos, Vector3.one * 0.3f);
                }
                if (i < path.Count - 1) {
                    Vector3 next = new Vector3(path[i + 1].x, path[i + 1].y, path[i + 1].z);
                    Gizmos.DrawLine(pos, next);
                }
            }
        }
    }
}