using HutongGames.PlayMaker;

namespace neapolis.tarzerind.eu.actions.logic
{
	/// <summary>
	///     <author> Tar Zerind Kft. - Nea Polisz Game Development Division.</author>
	///     <see cref="https://github.com/neapolisz/PlayMakerCustomActions_U5"/>
	/// </summary>
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Compares two integer values. " +
	                     "Takes the base integer compares with the first integer value and with AND relation compares with the second integer value.")]
	public class IntAndIntCompare : FsmStateAction
	{
		
		public enum Operation
		{
			ExtremeValueExclusive,
			ExtremeValueInclusive
		}
		
		[RequiredField, UIHint(UIHint.Variable), UnityEngine.Tooltip("The int variable should be tested.")]
		public FsmInt BaseValue;
		
		[RequiredField, UnityEngine.Tooltip("The tester int variable.")]
		public FsmInt FirstTesterValue;
		
		[RequiredField, UnityEngine.Tooltip("The tester int variable.")]
		public FsmInt SecondTesterValue;
		
		[RequiredField, UnityEngine.Tooltip("Extreme value exclusive: on both sides use compare with only. Extreme value inclusive: on both sides use compare with and equals.")]
		public Operation CompareOperator = Operation.ExtremeValueInclusive;
		
		[UnityEngine.Tooltip("Event to send if the Bool variable is True.")]
		public FsmEvent IsTrue;

		[UnityEngine.Tooltip("Event to send if the Bool variable is False.")]
		public FsmEvent IsFalse;

		public override void OnEnter()
		{
			Fsm.Event(AreTheyTrue().Value ? IsTrue : IsFalse);
			Finish();
		}

		private FsmBool AreTheyTrue()
		{
			var baseValue = BaseValue.Value;
			var first = FirstTesterValue.Value;
			var second = SecondTesterValue.Value;
			var result = false;
			
			switch (CompareOperator)
			{
				case Operation.ExtremeValueExclusive :
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
			}

			return new FsmBool(result);
		}

		public override void Reset()
		{
			BaseValue = null;
			FirstTesterValue = null;
			SecondTesterValue = null;
			IsTrue = null;
			IsFalse = null;
		}
	}
}
