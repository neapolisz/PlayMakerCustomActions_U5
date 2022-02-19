using HutongGames.PlayMaker;

namespace neapolis.tarzerind.eu.actions
{
    /// <summary>
    ///     <author> Tar Zerind Kft. - Nea Polisz Game Development Division.</author>
    ///     <see cref="https://github.com/neapolisz/PlayMakerCustomActions_U5"/>
    /// </summary>
    [Tooltip(
        "Compares the targets and testers. There is AND relationship between the float relation and integer relation.")]
    public class IntAndFloatCompare : FsmStateAction
    {
        public enum Operation
        {
            Greater,
            Lower,
            Equals,
            GreaterThenEquals,
            LowerThenEquals
        }

        [RequiredField] [UIHint(UIHint.Variable)] [Tooltip("The float variable should be tested.")]
        public FsmFloat targetFloatVar;

        [RequiredField] [Tooltip("The tester float variable.")]
        public FsmFloat testerFloatVar;

        [RequiredField] [Tooltip("Operation which is used between the target and tester.")]
        public Operation floatOperation = Operation.Equals;

        [RequiredField] [UIHint(UIHint.Variable)] [Tooltip("The int variable should be tested.")]
        public FsmInt targetIntVar;

        [RequiredField] [Tooltip("The tester int variable.")]
        public FsmInt testerIntVar;

        [RequiredField] [Tooltip("Operation which is used between the target and tester.")]
        public Operation intOperation = Operation.Equals;

        [Tooltip("Event to send if the Bool variable is True.")]
        public FsmEvent isTrue;

        [Tooltip("Event to send if the Bool variable is False.")]
        public FsmEvent isFalse;

        public bool everyFrame;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            Fsm.Event(areTheyTrue().Value ? isTrue : isFalse);

            if (!everyFrame)
                Finish();
        }

        // Code that runs every frame.
        public override void OnUpdate()
        {
            Fsm.Event(areTheyTrue().Value ? isTrue : isFalse);
        }

        public override void Reset()
        {
            targetFloatVar = null;
            testerFloatVar = null;
            targetIntVar = null;
            testerIntVar = null;
            isTrue = null;
            isFalse = null;
            everyFrame = false;
        }

        private FsmBool areTheyTrue()
        {
            return decideFloat().Value && decideInt().Value;
        }

        private FsmBool decideFloat()
        {
            FsmBool result = new FsmBool();

            switch (floatOperation)
            {
                case Operation.Equals:
                    result = targetFloatVar.Value.Equals(testerFloatVar.Value);
                    break;

                case Operation.Greater:
                    result = targetFloatVar.Value > testerFloatVar.Value;
                    break;

                case Operation.Lower:
                    result = targetFloatVar.Value < testerFloatVar.Value;
                    break;

                case Operation.GreaterThenEquals:
                    result = targetFloatVar.Value >= testerFloatVar.Value;
                    break;

                case Operation.LowerThenEquals:
                    result = targetFloatVar.Value <= testerFloatVar.Value;
                    break;
            }

            return result;
        }

        private FsmBool decideInt()
        {
            FsmBool result = new FsmBool();

            switch (intOperation)
            {
                case Operation.Equals:
                    result = targetIntVar.Value.Equals(testerIntVar.Value);
                    break;

                case Operation.Greater:
                    result = targetIntVar.Value > testerIntVar.Value;
                    break;

                case Operation.Lower:
                    result = targetIntVar.Value < testerIntVar.Value;
                    break;

                case Operation.GreaterThenEquals:
                    result = targetIntVar.Value >= testerIntVar.Value;
                    break;

                case Operation.LowerThenEquals:
                    result = targetIntVar.Value <= testerIntVar.Value;
                    break;
            }

            return result;
        }
    }
}