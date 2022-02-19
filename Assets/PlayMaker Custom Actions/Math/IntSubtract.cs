using HutongGames.PlayMaker;
using UnityEngine;

namespace neapolis.tarzerind.eu.actions
{
    /// <summary>
    ///     <author> Tar Zerind Kft. - Nea Polisz Game Development Division.</author>
    ///     <see cref="https://github.com/neapolisz/PlayMakerCustomActions_U5"/>
    /// </summary>
    [ActionCategory(ActionCategory.Math)]
    [HutongGames.PlayMaker.Tooltip("Subtracts a value to an Integer Variable.")]
    public class IntSubtract : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [UnityEngine.Tooltip("The int variable to subtract from.")]
        public FsmInt intVariable;
		
        [RequiredField]
        [UnityEngine.Tooltip("Value to subtract from the int variable.")]
        public FsmInt subtract;
		
        [UnityEngine.Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;
		
        [UnityEngine.Tooltip("Used with Every Frame. Subtracts the value over one second to make the operation frame rate independent.")]
        public bool perSecond;
		
        float _acc = 0f;
		
        public override void Reset()
        {
            intVariable = null;
            subtract = null;
            everyFrame = false;
            perSecond = false;
        }

        public override void OnEnter()
        {
            doSubtract();
			
            if (!everyFrame)
                Finish();
        }

        public override void OnUpdate()
        {
            doSubtract();
			
        }
		
        void doSubtract()
        {
            if (perSecond)
            {
                int _absSub = Mathf.Abs(subtract.Value);
                _acc += ( _absSub* Time.deltaTime);
                if (_acc>=_absSub)
                {
                    _acc = 0f;
                    intVariable.Value -= subtract.Value;
                }
            }else{
                intVariable.Value -= subtract.Value;
            }
        }
    }
}