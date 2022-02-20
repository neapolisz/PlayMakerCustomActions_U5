using System;
using HutongGames.PlayMaker;

namespace neapolis.tarzerind.eu.actions.logic
{
    /// <summary>
    ///     <author> Tar Zerind Kft. - Nea Polisz Game Development Division.</author>
    ///     <see cref="https://github.com/neapolisz/PlayMakerCustomActions_U5"/>
    /// </summary>
    [ActionCategory(ActionCategory.Logic)]
    [Tooltip(
        "Takes the base value and compares it with the lower value and also with the greater value. If it is between them, returns true.")]
    public class FloatAndFloatCompare : FsmStateAction
    {
        public enum Operation
        {
            ExtremeValueExclusive,
            ExtremeValueInclusive
        }

        [RequiredField, UIHint(UIHint.Variable), Tooltip("The int variable should be tested.")]
        public FsmFloat BaseValue;

        [RequiredField, Tooltip("The tester int variable.")]
        public FsmFloat LowerTester;

        [RequiredField, Tooltip("The tester int variable.")]
        public FsmFloat GreaterTester;

        [RequiredField, Tooltip("Extreme value exclusive: on both sides use compare with only. Extreme value inclusive: on both sides use compare with and equals.")]
        public readonly Operation CompareOperator = Operation.ExtremeValueInclusive;

        [Tooltip("Event to send if the Bool variable is True.")]
        public FsmEvent IsTrue;

        [Tooltip("Event to send if the Bool variable is False.")]
        public FsmEvent IsFalse;
        
        [Tooltip("Runs every frame.")]
        public bool everyFrame;

        public override void OnUpdate()
        {
            var baseValue = BaseValue.Value;
            var first = LowerTester.Value;
            var second = GreaterTester.Value;

            var result = false;

            switch (CompareOperator)
            {
                case Operation.ExtremeValueExclusive:
                    if (baseValue > first && baseValue < second)
                    {
                        result = true;
                    }

                    break;

                case Operation.ExtremeValueInclusive:
                    if (baseValue >= first && baseValue <= second)
                    {
                        result = true;
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Fsm.Event(new FsmBool(result).Value ? IsTrue : IsFalse);

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void Reset()
        {
            BaseValue = null;
            LowerTester = null;
            GreaterTester = null;
            IsTrue = null;
            IsFalse = null;
            everyFrame = false;
        }
    }
}