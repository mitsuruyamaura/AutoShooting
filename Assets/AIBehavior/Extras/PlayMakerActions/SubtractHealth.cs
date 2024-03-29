﻿#if USE_PLAYMAKER
using HutongGames.PlayMaker;

namespace AIBehavior
{
	public class SubtractHealth : AIPlayMakerActionBase
	{
		[RequiredField]
		[UIHint(UIHint.FsmFloat)]
		public FsmFloat healthAmount;


		public override void Reset()
		{
			base.Reset();
			healthAmount = null;
		}


		protected override void DoAIAction ()
		{
			ai.SubtractHealthValue(healthAmount.Value);
		}
	}
}
#endif