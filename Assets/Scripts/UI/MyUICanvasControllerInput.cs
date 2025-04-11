using UnityEngine;

namespace StarterAssets
{
    public class MyUICanvasControllerInput : UICanvasControllerInput
    {
        public float speedLookRotation = 0.25f;
        public float magnitudeMinToMove = 0.5f;

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

        public new void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
	        Debug.Log(virtualMoveDirection.magnitude);

	        starterAssetsInputs.LookInput(Vector2.ClampMagnitude(virtualMoveDirection * speedLookRotation, 1f));

	        if (virtualMoveDirection.magnitude > magnitudeMinToMove)
	        {
		        starterAssetsInputs.MoveInput(Vector2.ClampMagnitude(virtualMoveDirection, 1f));
	        }
	        else
	        {
		        starterAssetsInputs.MoveInput(Vector2.zero);
	        }
        }

        public void VirtualResetMove()
        {
            starterAssetsInputs.MoveInput(Vector2.zero);
            starterAssetsInputs.LookInput(Vector2.zero);
        }
    }
}
