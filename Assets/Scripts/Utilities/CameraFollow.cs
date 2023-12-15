using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject RedCandlePrefab;
    public GameObject YellowCandlePrefab;
    public GameObject BlueCandlePrefab;
    public GameObject LeftSpawner;
    public GameObject RightSpawner;

    public GameObject LevelPrefab;
    public GameObject LastLevelChunk;
    private Bounds _lastLevelBounds;

    public PlayerCharacterController Target;
    public Vector2 FocusAreaSize = new Vector2(3, 5);
    public float LookAheadX = 2;
    public float LookSmoothTimeX = 0.2f;
    public float VerticalSmoothTime = 0.2f;
    public float VerticalOffset = 1;

    private FocusArea _focusArea;

    private float _currentLookAheadX;
    private float _targetLookAheadX;
    private float _lookAheadDirX;
    private float _smooth_look_velocity_x;// don't touch, this is for Mathf.SmoothDamp to keep track of

    private bool _lookAheadStopped;

    private bool _climbUp = false;

    private Camera _camera;

    private float cameraTopY => _camera.ScreenToWorldPoint(new Vector3(0, Screen.height)).y;

    private float _lastGeneratedLevelTopY => _lastLevelBounds.max.y;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _focusArea = new FocusArea(Target.Collider.bounds, FocusAreaSize);
        _lastLevelBounds = LastLevelChunk.GetComponent<BoxCollider2D>().bounds;
        StartCoroutine(DelayAndClimb());
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator DelayAndClimb()
    {
        yield return new WaitForSeconds(3);
        _climbUp = true;
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            var pos = Random.Range(0, 2) > 0 ? LeftSpawner.transform.position : RightSpawner.transform.position;
            var tmpObj = Random.Range(0, 3) switch
            {
                0 => Instantiate(RedCandlePrefab, pos, Quaternion.identity),
                1 => Instantiate(YellowCandlePrefab, pos, Quaternion.identity),
                2 => Instantiate(BlueCandlePrefab, pos, Quaternion.identity),
                _ => throw new System.NotImplementedException()
            };
            var p = tmpObj.transform.position;
            p.z = 10;
            tmpObj.transform.position = p;
            yield return new WaitForSeconds(3);
        }
    }

    private void LateUpdate()
    {
        _focusArea.Update(Target.Collider.bounds);


        if (_focusArea.Velocity.x != 0)
        {
            _lookAheadDirX = Mathf.Sign(_focusArea.Velocity.x);
            var input = InputManager.CurrentDirectionalInput();

            if (input.x != 0 && Mathf.Sign(input.x) == Mathf.Sign(_focusArea.Velocity.x))
            {
                _lookAheadStopped = false;
                _targetLookAheadX = _lookAheadDirX * LookAheadX;
            }
            else
            {
                if (!_lookAheadStopped)
                {
                    _lookAheadStopped = true;
                    _targetLookAheadX = _currentLookAheadX + (_lookAheadDirX * LookAheadX - _currentLookAheadX) / 4f;
                }
            }
        }

        _currentLookAheadX = Mathf.SmoothDamp(_currentLookAheadX, _targetLookAheadX, ref _smooth_look_velocity_x, LookSmoothTimeX);

        if (_climbUp)
        {
            transform.position += Vector3.up * 0.0005f;

            if (cameraTopY > _lastGeneratedLevelTopY)
            {
                Debug.Log("generate moreplatform");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_focusArea.Center, FocusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 Center;

        public Vector2 Velocity;

        private float _left, _right, _top, _bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            _left = targetBounds.center.x - (size.x / 2);
            _right = targetBounds.center.x + (size.x / 2);
            _bottom = targetBounds.min.y;
            _top = targetBounds.min.y + size.y;

            Velocity = Vector2.zero;
            Center = new Vector2((_left + _right) / 2, (_top + _bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            var shiftX = 0f;

            if (targetBounds.min.x < _left)
            {
                shiftX = targetBounds.min.x - _left;
            }
            else if (targetBounds.max.x > _right)
            {
                shiftX = targetBounds.max.x - _right;
            }

            _left += shiftX;
            _right += shiftX;

            var shiftY = 0f;

            if (targetBounds.min.y < _bottom)
            {
                shiftY = targetBounds.min.y - _bottom;
            }
            else if (targetBounds.max.y > _top)
            {
                shiftY = targetBounds.max.y - _top;
            }

            _top += shiftY;
            _bottom += shiftY;

            Center = new Vector2((_left + _right) / 2, (_top + _bottom) / 2);
            Velocity = new Vector2(shiftX, shiftY);
        }
    }
}
