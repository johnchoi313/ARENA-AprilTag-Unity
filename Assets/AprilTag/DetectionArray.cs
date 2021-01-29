using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace AprilTag {

public sealed class DetectionArray : SafeHandleZeroOrMinusOneIsInvalid
{
    #region SafeHandle implementation

    DetectionArray() : base(true) {}

    protected override bool ReleaseHandle()
    {
        _Destroy(handle);
        return true;
    }

    #endregion

    #region zarray representation

    unsafe ref ZArray<IntPtr> AsPointerArray
      => ref Unsafe.AsRef<ZArray<IntPtr>>((void*)handle);

    #endregion

    #region Public methods

    public int Length
      => AsPointerArray.AsSpan.Length;

    public unsafe ref Detection this[int i]
      => ref Unsafe.AsRef<Detection>((void*)AsPointerArray.AsSpan[i]);

    #endregion

    #region Unmanaged interface

    [DllImport("AprilTag", EntryPoint="apriltag_detections_destroy")]
    static extern void _Destroy(IntPtr detections);

    #endregion
}

} // namespace AprilTag
