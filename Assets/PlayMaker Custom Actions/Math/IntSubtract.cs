using HutongGames.PlayMaker;
using UnityEngine;

namespace neapolis.tarzerind.eu.actions.math
{
    /// <summary>
    ///     <author> Tar Zerind Kft. - Nea Polisz Game Development Division.</author>
    ///     <see cref="https://github.com/neapolisz/PlayMakerCustomActions_U5"/>
    /// </summary>
    [ActionCategory(ActionCategory.Math)]
    [HutongGames.PlayMaker.Tooltip("Subtracts a value to an Integer Variable.")]
    public class IntSubtract : FsmStateAction
    {
        [RequiredField] [UIHint(UIHint.Variable)] [UnityEngine.Tooltip("The int variable to subtract from.")]
        public FsmInt IntVariable;

        [RequiredField] [UnityEngine.Tooltip("Value to subtract from the int variable.")]
        public FsmInt Subtract;

        [UnityEngine.Tooltip("Repeat every frame while the state is active.")]
        public bool EveryFrame;

        [UnityEngine.Tooltip(
            "Used with Every Frame. Subtracts the value over one second to make the operation frame rate independent.")]
        public bool PerSecond;

        private float _acc;

        public override void Reset()
        {
            IntVariable = null;
            Subtract = null;
            EveryFrame = false;
            PerSecond = false;
        }

        public override void OnEnter()
        {
            doSubtract();

            if (!EveryFrame)
                Finish();
        }

        public override void OnUpdate()
        {
            doSubtract();
        }

        void doSubtract()
        {
            if (PerSecond)
            {
                var _absSub = Mathf.Abs(Subtract.Value);
                _acc += (_absSub * Time.deltaTime);

                if (!(_acc >= _absSub)) return;
                _acc = 0f;
                IntVariable.Value -= Subtract.Value;
            }
            else
            {
                IntVariable.Value -= Subtract.Value;
            }
        }
    }
}