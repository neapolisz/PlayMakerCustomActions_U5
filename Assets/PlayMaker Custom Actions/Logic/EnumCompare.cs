using HutongGames.PlayMaker;

namespace neapolis.tarzerind.eu.actions
{
    /// <summary>
    ///     <author> Tar Zerind Kft. - Nea Polisz Game Development Division.</author>
    ///     <see cref="https://github.com/neapolisz/PlayMakerCustomActions_U5"/>
    /// </summary>
    [ActionCategory(ActionCategory.Logic)]
    [Tooltip("Compares 2 Enum values and sends Events based on the result.")]
    public class EnumCompare : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [UnityEngine.Tooltip("The first Enum Variable.")]
        public FsmEnum enumVariable;

        [MatchFieldType("enumVariable")]
        [UnityEngine.Tooltip("The second Enum Variable.")]
        public FsmEnum compareTo;

        [UnityEngine.Tooltip("Event to send if the values are equal.")]
        public FsmEvent equalEvent;

        [UnityEngine.Tooltip("Event to send if the values are not equal.")]
        public FsmEvent notEqualEvent;
        
        [UIHint(UIHint.Variable)]
        [UnityEngine.Tooltip("Store the true/false result in a bool variable.")]
        public FsmBool storeResult;
        
        [UnityEngine.Tooltip("Repeat every frame. Useful if the enum is changing over time.")]
        public bool everyFrame;

        public override void Reset()
        {
            enumVariable = null;
            compareTo = null;
            equalEvent = null;
            notEqualEvent = null;
            storeResult = null;
            everyFrame = false;
        }

        public override void OnEnter()
        {
            DoEnumCompare();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoEnumCompare();
        }

        void DoEnumCompare()
        {
            if (enumVariable == null || compareTo == null) return;

            var equal = Equals(enumVariable.Value, compareTo.Value);

            if (storeResult != null)
            {
                storeResult.Value = equal;
            }

            if (equal && equalEvent != null)
            {
                Fsm.Event(equalEvent);
                return;
            }

            if (!equal && notEqualEvent != null)
            {
                Fsm.Event(notEqualEvent);
            }

        }

    }
}
