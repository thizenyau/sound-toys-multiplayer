using UnityEngine;


public class STW : MonoBehaviour
{
    //public Theremin.Theremin theremin;
    public Transform transform1;
    public Transform transform2;
    public Transform leftFoot;
    public Transform rightFoot;

    private int points = 12;
    public float amplitude = 1;
    public Vector2 xLimits = Vector2.zero;
    public float movementSpeed = 3;
    private float y = 0;
    private float y1 = 0;
    private float y2 = 0;
    private float _freq = 0.6f;
    private float _ampl = 0.3f;
    private float timer = 0f;
    public bool _start = false;
    private bool _isDrawing = false;

    private LineRenderer myLineRenderer;

    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
    }

    private void Draw()
    {
        float xStart = 0;
        float Tau = 2 * Mathf.PI;
        float xFinish = Vector3.Distance(transform1.position, transform2.position);

        myLineRenderer.positionCount = points;

        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            System.Random random = new System.Random();
            double randomNumber = random.NextDouble() - 0.5;
            float randomFloat = (float)randomNumber;
            float y = _ampl * Mathf.Sin((Tau * _freq * 5 * x) + (Time.timeSinceLevelLoad * movementSpeed)) + randomFloat * 0.1f;
            
            Vector3 _vector = Cross();
            Vector3 y_vector = _vector * y;
            Vector3 newPosition = transform1.position + (transform2.position - transform1.position).normalized * x + y_vector;
            myLineRenderer.SetPosition(currentPoint, newPosition);
            //y1 = y;
        }
    }

    private void Delete()
    {
        myLineRenderer.positionCount = 0;
    }

    void Update()
    {
        y = transform1.position.y;
        
        if (y - y1 < -0.09f && !_isDrawing)
        {
            _start = true;
        }

        
        if (_start)
        {
            timer = 100f;
            _ampl = 0f;
            _start = false;
            _isDrawing = true;
        }
        if (timer > 0)
        {
            _ampl += 0.012f;
            if (timer > 50f)
                _ampl -= 0.024f;
            Draw();
            timer -= 1f;
            if (timer == 0f)
                _isDrawing = false;
        }
        else
        {
            Delete();
        }


        y1 = y;

    }

    public Vector3 Cross()
    {
        Vector3 x = transform2.position - transform1.position;
        Vector3 xProjection = new Vector3(x.x, 0, x.z);
        Vector3 y = Vector3.Cross(x, xProjection);
        Vector3 z = Vector3.Cross(x, y);
        Vector3 z_n = z.normalized;
        return z_n;
    }
}
