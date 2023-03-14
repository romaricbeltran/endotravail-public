using UnityEngine;

namespace StarterAssets
{
    public class MyUICanvasControllerInput : MonoBehaviour
    {
        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;
        public float speedLookRotation = 1.0f;

        public void VirtualMoveUpInput(bool virtualMoveUpState)
        {
            starterAssetsInputs.MoveInput(virtualMoveUpState ? new Vector2(0, 1) : Vector2.zero);
            starterAssetsInputs.LookInput(virtualMoveUpState ? new Vector2(0, 1) * speedLookRotation : Vector2.zero);
        }

        public void VirtualMoveDownInput(bool virtualMoveDownState)
        {
            starterAssetsInputs.MoveInput(virtualMoveDownState ? new Vector2(0, -1) : Vector2.zero);
            starterAssetsInputs.LookInput(virtualMoveDownState ? new Vector2(0, -1) * speedLookRotation : Vector2.zero);
        }

        public void VirtualMoveLeftInput(bool virtualMoveLeftState)
        {
            starterAssetsInputs.MoveInput(virtualMoveLeftState ? new Vector2(-1, 0) : Vector2.zero);
            starterAssetsInputs.LookInput(virtualMoveLeftState ? new Vector2(-1, 0) * speedLookRotation : Vector2.zero);
        }

        public void VirtualMoveRightInput(bool virtualMoveRightState)
        {
            starterAssetsInputs.MoveInput(virtualMoveRightState ? new Vector2(1, 0) : Vector2.zero);
            starterAssetsInputs.LookInput(virtualMoveRightState ? new Vector2(1, 0) * speedLookRotation : Vector2.zero);
        }

    }
}