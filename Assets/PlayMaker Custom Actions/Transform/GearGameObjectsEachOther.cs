using System;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;

namespace neapolis.tarzerind.eu.actions.transform
{
    /// <summary>
    ///     <author> Tar Zerind Kft. - Nea Polisz Game Development Division.</author>
    ///     <see cref="https://github.com/neapolisz/PlayMakerCustomActions_U5"/>
    /// </summary>
    [ActionCategory(ActionCategory.Transform)]
    [HutongGames.PlayMaker.Tooltip(
        "Gears two gameobjects to each other, using by their 'Y' rotation. This Action is useful if the targetEuler gameobject is looking in the Z axis direction.")]
    public class GearGameObjectsEachOther : FsmStateAction
    {
        private const int BaseObjectsMinYEulerDegree = 0;
        private const int BaseObjectsMaxYEulerDegree = 90;
        private const int BaseObjectsOppositeMinYEulerDegree = 270;
        private const int BaseObjectsOppositeMaxYEulerDegree = 359;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Base GameObject which 'Y' rotation's value is taken as base value.")]
        public FsmGameObject BaseGameObject;

        [RequiredField]
        [HutongGames.PlayMaker.Tooltip(
            "Target GameObject which 'Y' rotation's value equals with base gameobject's Y rotation value.")]
        public FsmOwnerDefault TargetGameObject;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip(
            "Turning speed of the targetEuler gameobject. How much time will the targetEuler game object will before it takes the next turning portion. Negative value will be turned to its absolute value.")]
        public FsmFloat TurningSpeed;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip(
            "Turning degree of the targetEuler gameobject. How much value will be added to the previous rotation value? Negative value will be turned to its absolute value.")]
        public FsmFloat TurningDegree;


        public override void Reset()
        {
            BaseGameObject = null;
            TargetGameObject = null;
            TurningSpeed = null;
            TurningDegree = null;
        }

        public override void OnEnter()
        {
            GearEachOther();
        }

        private void GearEachOther()
        {
            var targetGameObjectEuler = Fsm.GetOwnerDefaultTarget(TargetGameObject).gameObject.transform.localEulerAngles;
            var baseGameObjectEuler = BaseGameObject.Value.transform.localEulerAngles;
            StartCoroutine(TurnTargetToBaseRotation(baseGameObjectEuler, targetGameObjectEuler));
        }

        private IEnumerator TurnTargetToBaseRotation(Vector3 baseEuler, Vector3 targetEuler)
        {
            var x = targetEuler.x;
            var z = targetEuler.z;
            var targetY = targetEuler.y;

            var absTurningValue = Math.Abs(TurningDegree.Value);
            var absTurningSpeed = Math.Abs(TurningSpeed.Value);

            if (baseEuler.y >= BaseObjectsMinYEulerDegree && baseEuler.y <= BaseObjectsMaxYEulerDegree)
            {
                if (targetEuler.y < baseEuler.y)
                {
                    for (var k = targetY; k < Math.Abs(baseEuler.y - BaseObjectsMaxYEulerDegree); k++)
                    {
                        Fsm.GetOwnerDefaultTarget(TargetGameObject).gameObject.transform.Rotate(GETDestinationVector3(targetEuler, absTurningValue, x, z), Space.Self);
                        var res = GETRotationResult(baseEuler, Fsm.GetOwnerDefaultTarget(TargetGameObject).gameObject.transform.localEulerAngles.y);
                        if (FinishTurning(res, absTurningValue)) yield break;
                        yield return new WaitForSeconds(absTurningSpeed);
                    }
                }

                if (targetEuler.y > baseEuler.y)
                {
                    for (var k = targetY; k >= baseEuler.y + BaseObjectsMaxYEulerDegree; k++)
                    {
                        Fsm.GetOwnerDefaultTarget(TargetGameObject).gameObject.transform.Rotate(GETDestinationVector3(targetEuler, absTurningValue, x, z), Space.Self);
                        var res = GETRotationResult(baseEuler, Fsm.GetOwnerDefaultTarget(TargetGameObject).gameObject.transform.localEulerAngles.y);
                        if (FinishTurning(res, absTurningValue)) yield break;
                        yield return new WaitForSeconds(absTurningSpeed);
                    }
                }
            }

            if (baseEuler.y >= BaseObjectsOppositeMinYEulerDegree && baseEuler.y <= BaseObjectsOppositeMaxYEulerDegree)
            {
                if (targetEuler.y < baseEuler.y)
                {
                    for (var k = targetY; k + BaseObjectsMaxYEulerDegree < baseEuler.y; k--)
                    {
                        Fsm.GetOwnerDefaultTarget(TargetGameObject).gameObject.transform.Rotate(GETDestinationVector3(targetEuler, absTurningValue*-1, x, z), Space.Self);
                        var res = GETRotationResult(baseEuler, Fsm.GetOwnerDefaultTarget(TargetGameObject).gameObject.transform.localEulerAngles.y);
                        if (FinishTurning(res, absTurningValue)) yield break;
                        yield return new WaitForSeconds(absTurningSpeed);
                    }
                }

                if (targetEuler.y > baseEuler.y)
                {
                    for (var k = targetY; k + BaseObjectsMaxYEulerDegree > baseEuler.y; k--)
                    {
                        Fsm.GetOwnerDefaultTarget(TargetGameObject).gameObject.transform.Rotate(GETDestinationVector3(targetEuler, absTurningValue*-1, x, z), Space.Self);
                        var res = GETRotationResult(baseEuler, Fsm.GetOwnerDefaultTarget(TargetGameObject).gameObject.transform.localEulerAngles.y);
                        if (FinishTurning(res, absTurningValue)) yield break;
                        yield return new WaitForSeconds(absTurningSpeed);
                    }
                }
            }
        }

        private bool FinishTurning(int res, float absTurningValue)
        {
            if (res <= BaseObjectsMaxYEulerDegree + absTurningValue && res >= BaseObjectsMaxYEulerDegree - absTurningValue)
            {
                Fsm.Event(FsmEvent.Finished);
                return true;
            }

            return false;
        }

        private static Vector3 GETDestinationVector3(Vector3 targetEuler, float absTurningValue, float x, float z)
        {
            return new Vector3(x, targetEuler.y + absTurningValue, z);
        }

        private int GETRotationResult(Vector3 baseEuler, float y)
        {
            return Math.Abs(Convert.ToInt32(baseEuler.y) - Convert.ToInt32(y));
        }
    }
}