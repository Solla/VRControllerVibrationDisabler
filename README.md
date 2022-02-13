# Acknowledgement
This is a variant from BOLL7708's project [OpenVRInputTest](https://github.com/BOLL7708/OpenVRInputTest) .

## Compatibility
Verified : 
- [x] HTC Vive Controller
- [X] Valve Index Controller
- [X] Oculus Touch Controller

Supported, but not verified yet:
- [ ] HTC Vive Cosmos Controller
- [ ] Holographic Controller

# Controller Disabler
Lots of people feel annoyed at the controller vibrating.

I have found several posts on the internet about how to disable the vibration of controllers.

Example:

[Is there a way to disable controller vibration?](https://www.reddit.com/r/WindowsMR/comments/8obiml/is_there_a_way_to_disable_controller_vibration/)

[Its possible to disable controller vibration at all?](https://www.reddit.com/r/Vive/comments/94noui/its_possible_to_disable_controller_vibration_at/e3n8e7d/)

[How to disable all haptics?](https://www.reddit.com/r/WindowsMR/comments/ds7qna/how_to_disable_all_haptics/)

[How to disable rumble/vibrate feature in controllers?](https://www.reddit.com/r/WindowsMR/comments/9gtafh/how_to_disable_rumblevibrate_feature_in/)

[How do I turn off Oculus Touch controller vibration?](https://forums.oculusvr.com/community/discussion/69874/how-do-i-turn-off-oculus-touch-controller-vibration)

## How It Works
The program is based on OpenVR API, and it keeps sending vibration signals with ``amplitude: 0``, which means stop the vibration immediately.

Even if games send a rumble request to controllers, this program will let the vibration stop without delay.

Sometimes the controllers are clattering, but the vibration is imperceptible.

# How To Use

Just open **VibrationDisable.exe**.
