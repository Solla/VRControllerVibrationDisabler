using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Valve.VR;
//Sources: https://github.com/BOLL7708/OpenVRInputTest
namespace OpenVRInputTest
{
    class Program
    {
        static ulong mActionSetHandle;
        static VRActiveActionSet_t[] mActionSetArray;
        const int MAX_FPS = 144;
        // # items are referencing this list of actions: https://github.com/ValveSoftware/openvr/wiki/SteamVR-Input#getting-started
        static void Main(string[] args)
        {
            // Initializing connection to OpenVR
            var error = EVRInitError.None;
            OpenVR.Init(ref error, EVRApplicationType.VRApplication_Background); // Had this as overlay before to get it working, but changing it back is now fine?
            var workerThread = new Thread(Worker);
            if (error != EVRInitError.None)
                Utils.PrintError($"OpenVR initialization errored: {Enum.GetName(typeof(EVRInitError), error)}");
            else
            {
                Utils.PrintInfo("OpenVR initialized successfully.");

                // Load app manifest, I think this is needed for the application to show up in the input bindings at all
                Utils.PrintVerbose("Loading app.vrmanifest");
                var appError = OpenVR.Applications.AddApplicationManifest(Path.GetFullPath("./app.vrmanifest"), false);
                if (appError != EVRApplicationError.None)
                    Utils.PrintError($"Failed to load Application Manifest: {Enum.GetName(typeof(EVRApplicationError), appError)}");
                else 
                    Utils.PrintInfo("Application manifest loaded successfully.");

                // #3 Load action manifest
                Utils.PrintVerbose("Loading actions.json");
                var ioErr = OpenVR.Input.SetActionManifestPath(Path.GetFullPath("./actions.json"));
                if (ioErr != EVRInputError.None) 
                    Utils.PrintError($"Failed to load Action Manifest: {Enum.GetName(typeof(EVRInputError), ioErr)}");
                else 
                    Utils.PrintInfo("Action Manifest loaded successfully.");

                // #4 Get action handles
                Utils.PrintVerbose("Getting action handles");
                
                
                // #5 Get action set handle
                Utils.PrintVerbose("Getting action set handle");
                var errorAS = OpenVR.Input.GetActionSetHandle("/actions/default", ref mActionSetHandle);
                if (errorAS != EVRInputError.None) 
                    Utils.PrintError($"GetActionSetHandle Error: {Enum.GetName(typeof(EVRInputError), errorAS)}");
                Utils.PrintDebug($"Action Set Handle default: {mActionSetHandle}");

                // Starting worker
                Utils.PrintDebug("Starting worker thread.");
                if (!workerThread.IsAlive) 
                    workerThread.Start();
                else 
                    Utils.PrintError("Could not start worker thread.");
            }
            Console.ReadLine();
            workerThread.Abort();
            OpenVR.Shutdown();
        }

        private static void Worker()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Thread.CurrentThread.IsBackground = true;
            ulong LeftVibration = 0, RightVibration = 0;
            OpenVR.Input.GetActionHandle("/actions/default/out/haptic_left", ref LeftVibration); 
            OpenVR.Input.GetActionHandle("/actions/default/out/haptic_right", ref RightVibration);
            // #6 Update action set
            if (mActionSetArray == null)
            {
                var actionSet = new VRActiveActionSet_t
                {
                    ulActionSet = mActionSetHandle,
                    ulRestrictedToDevice = OpenVR.k_ulInvalidActionSetHandle,
                    nPriority = 0
                };
                mActionSetArray = new VRActiveActionSet_t[] { actionSet };
            }
            while (true)
            {
                var errorUAS = OpenVR.Input.UpdateActionState(mActionSetArray, (uint)Marshal.SizeOf(typeof(VRActiveActionSet_t)));
                if (errorUAS != EVRInputError.None) 
                    Utils.PrintError($"UpdateActionState Error: {Enum.GetName(typeof(EVRInputError), errorUAS)}");

                var errorLeftVibration = OpenVR.Input.TriggerHapticVibrationAction(LeftVibration, 0, 1000, 0, 0, OpenVR.k_ulInvalidInputValueHandle);
                if (errorLeftVibration != EVRInputError.None)
                    Utils.PrintError($"Left Vibration Error: {Enum.GetName(typeof(EVRInputError), errorLeftVibration)}");
                
                var errorRightVibration = OpenVR.Input.TriggerHapticVibrationAction(RightVibration, 0, 1000, 0, 0, OpenVR.k_ulInvalidInputValueHandle);
                if (errorRightVibration != EVRInputError.None)
                    Utils.PrintError($"Right Vibration Error: {Enum.GetName(typeof(EVRInputError), errorRightVibration)}");
                Thread.Sleep(1000 / MAX_FPS);
            }
        }
    }
}

