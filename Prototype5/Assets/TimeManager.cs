using UnityEngine;

public class TimeManager : MonoBehaviour
{
    enum TimeMode
    {
        Slow,
        Fast,
        Normal
    }

    [SerializeField, Range(0.01f, 3f)] private float defaultScale = 0.1f;
    private float resetLength = 0f;
    private TimeMode mode = TimeMode.Normal;

    private void Start()
    {
        ResetTimescale();
    }

    // Gradually resets timescale to normal
    private void Update()
    {
        if (resetLength == 0f) return;

        if (mode == TimeMode.Slow)
        {
            Time.timeScale += (1f / resetLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0.01f, 1f);
        }
        else if (mode == TimeMode.Fast)
        {
            Time.timeScale -= (1f / resetLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 1f, 3f);
        }

        if (Time.timeScale == 1f)
        {
            resetLength = 0f;
            mode = TimeMode.Normal;
        }
    }

    // Enters slow mo for length seconds; or until reset if length == 0f
    // Supports custom scale, or uses default scale if scale == 0f
    public void ChangeTimescale(float scale, float length)
    {
        if (scale == 0f) scale = defaultScale;

        if (scale > 1f)
        {
            mode = TimeMode.Fast;
        }
        else if (scale < 1f)
        {
            mode = TimeMode.Slow;
        }
        else
        {
            mode = TimeMode.Normal;
        }

        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        resetLength = length;
    }

    // Resets time scale to normal immediately
    public void ResetTimescale()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        resetLength = 0f;
        mode = TimeMode.Normal;
    }
}
