using System;
using HutongGames.PlayMaker;

namespace neapolis.tarzerind.eu.actions.logic
{
    /// <summary>
    ///     <author> Tar Zerind Kft. - Nea Polisz Game Development Division.</author>
    ///     <see cref="https://github.com/neapolisz/PlayMakerCustomActions_U5"/>
    /// </summary>
    [ActionCategory(ActionCategory.Logic)]
    [Tooltip("Compares the given value with a higher and with a lower values using AND logical binding.")]
    public class ComplexFloatSwitch : FsmStateAction
    {
        public enum Operation
        {
            ExtremeValueExclusive,
            ExtremeValueInclusive
        }


        [RequiredField
         , UIHint(UIHint.Variable)
         , Tooltip("The float value to be tested.")]
        public FsmFloat BaseValue;

        [RequiredField
         , Tooltip("Extreme value exclusive: on both sides use compare with only. Extreme value inclusive: on both sides use compare with and equals.")]
        public Operation CompareOperator = Operation.ExtremeValueInclusive;

        [UIHint(UIHint.Tag)]
        [Tooltip("The lower and higher value that test the base value.")]
        [CompoundArray("Tester Value Switch", "Lower Value", "Higher Value")]
        public FsmFloat[] LowerTesterValue;

        [UIHint(UIHint.Tag)]
        [Tooltip("Is the Additional tag name optional or necessary?")]
        public FsmFloat[] HigherTesterValue;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Possible results")]
        public FsmString[] ResultsStrings;
        
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the value in an Int variable in this FSM.")]
        public FsmString Result;

        public override void OnEnter()
        {
            Finish();
        }

        private void DecideWhichPathToUse()
        {
            for (var i = 0; i < LowerTesterValue.Length; i++)
            {
                var baseValue = BaseValue.Value;
                var lower = LowerTesterValue[i].Value;
                var higher = HigherTesterValue[i].Value;

                switch (CompareOperator)
                {
                    case Operation.ExtremeValueInclusive:
                        if (baseValue >= lower && baseValue <= higher)
                        {
                            Result = ResultsStrings[i].Value;
                        }

                        break;

                    case Operation.ExtremeValueExclusive:
                        if (baseValue > lower && baseValue < higher)
                        {
                            Result = ResultsStrings[i].Value;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override void Reset()
        {
            BaseValue = null;
            LowerTesterValue = null;
            HigherTesterValue = null;
            ResultsStrings = null;
            Result = null;
        }
    }
}