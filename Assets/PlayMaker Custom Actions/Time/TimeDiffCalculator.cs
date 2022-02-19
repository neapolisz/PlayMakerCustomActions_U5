using System;
using HutongGames.PlayMaker;

namespace neapolis.tarzerind.eu.actions
{
    /// <summary>
    ///     <author> Tar Zerind Kft. - Nea Polisz Game Development Division.</author>
    ///     <see cref="https://github.com/neapolisz/PlayMakerCustomActions_U5"/>
    /// </summary>
    [ActionCategory(ActionCategory.Time)]
    [Tooltip(
        "Compares two time values (given in String datatype in HH:mm:ss format) and gives back its difference converted into Float datatype.")]
    public class TimeDiffCalculator : FsmStateAction
    {
        [Tooltip("Starting time")] 
        public FsmString StartTime;

        [Tooltip("Ending time")] 
        public FsmString EndTime;

        [Tooltip("Target difference: time, in seconds, that is the maximum for waiting for.")]
        public FsmFloat TargetDifference;

        [Tooltip("The difference of ending time subtracting starting time, stored in milliseconds (float datatype).")]
        public FsmFloat Result;

        [Tooltip("Stores whether the set time is reached.")]
        public FsmBool ReachedTargetDifference;

        public override void OnEnter()
        {
            if (!FsmString.IsNullOrEmpty(StartTime))
            {
                if (!FsmString.IsNullOrEmpty(EndTime))
                {
                    var start = DateTime.Parse(StartTime.ToString());
                    var end = DateTime.Parse(EndTime.ToString());
                    var difference = end.Subtract(start).Milliseconds;

                    Result.Value = difference;
                    if (!NamedVariable.IsNullOrNone(TargetDifference))
                    {
                        ReachedTargetDifference.Value = difference >= TargetDifference.Value;
                    }
                }
            }

            Finish();
        }

        public override void Reset()
        {
            StartTime = string.Empty;
            EndTime = string.Empty;
            TargetDifference = 0.0f;
            Result = float.NaN;
            ReachedTargetDifference = false;
        }
    }
}