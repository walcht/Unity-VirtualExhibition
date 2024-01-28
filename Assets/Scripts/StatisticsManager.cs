using System.Runtime.InteropServices;
using Common;
using DesignPatterns;

/// <summary>
///     Deals with everything related to statistics.
///     Use this singleton to send statistics data to frontEnd React application.
/// </summary>
public class StatisticsManager : Singleton<StatisticsManager>
{
    [DllImport("__Internal")]
    private static extern void StandActionPerformedEvent(string actionJSON);

    [DllImport("__Internal")]
    private static extern void AuditoriumEnteredEvent();

    /// <summary>
    ///     Call this function when a stand action is performed to send statistics about that action.
    ///     For actions requiring time, call this function when that action ends.
    /// </summary>
    /// <param name="stand_id">ID of the stand from which this action came.</param>
    /// <param name="action">Type of action to send statistics for.</param>
    /// <param name="duration">Duration of action. Set by default to 0.</param>
    public void OnStandActionPerformed(string stand_id, StandAction action, float duration = 0.0f)
    {
        // generate JSON
        string JSONToSend = string.Format(
            "{{ \"stand_id\" : \"{0}\", \"visitor_id\"  : \"{1}\", \"action\"    : \"{2}\", \"duration\" : {3} }}",
            stand_id,
            CustomizationManager.Instance.id,
            action,
            duration
        );

#if UNITY_WEBGL == true && UNITY_EDITOR == false
                StandActionPerformedEvent(JSONToSend);
#endif
    }

    /// <summary>
    ///     Call this function when the user enters the Auditorium (webinar)
    /// </summary>
    public void OnAuditoriumEnterPerformed()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                AuditoriumEnteredEvent();
#endif
    }
}
