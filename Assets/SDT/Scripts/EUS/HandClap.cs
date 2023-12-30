using UnityEngine;


public class HandClap : MonoBehaviour
{
    //public Theremin.Theremin theremin;
    public Transform transform1;
    public Transform transform2;


    private int points = 50;
    public float amplitude = 1;
    public Vector2 xLimits = Vector2.zero;
    //public float movementSpeed = 3;
    private float y = 0;
    private float y1 = 0;
    private float y2 = 0;
    private float _freq = 0.6f;
    private float _ampl = 0.3f;
    private float timer = 0f;
    public bool _start = false;
    public bool _isDrawing = false;

    private LineRenderer myLineRenderer;

    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
    }

    private void Draw()
    {
        //float xStart = 0;
        float Tau = 2 * Mathf.PI;
        //float xFinish = Vector3.Distance(transform1.position, transform2.position);
        Vector3 center = (transform1.position + transform2.position) / 2;
        float radius = 3f;


        myLineRenderer.positionCount = points;

        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float angle = Tau * currentPoint / (points - 1);
            float x = center.x + _ampl * radius * Mathf.Cos(angle);
            float y = center.y + _ampl * radius * Mathf.Sin(angle);
            float z = center.z;
            Vector3 newPosition = new Vector3(x, y, z);
            myLineRenderer.SetPosition(currentPoint, newPosition);
        }
    }

    private void Delete()
    {
        myLineRenderer.positionCount = 0;
    }

    void Update()
    {
        float _distance = Vector3.Distance(transform1.position, transform2.position);
        
        if (_distance < 0.2f && !_isDrawing)
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
