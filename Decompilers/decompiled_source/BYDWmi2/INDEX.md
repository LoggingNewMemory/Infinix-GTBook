# BYDWmi2.dll - Decompiled Source Index

- **Assembly**: BYDWmi2.dll
- **Types**: 13
- **Total Methods**: 114

## Namespace: `<global>`

- **Struct** `__StaticArrayInitTypeSize=20` - 1 methods, 0 properties
- **Struct** `__StaticArrayInitTypeSize=24` - 1 methods, 0 properties
- **Class** `BYDWmi2_ProcessedByFody` - 0 methods, 0 properties

## Namespace: `BYDWmi`

- **Delegate** `HookProc` - 4 methods, 0 properties
  - `Invoke(Int32 nCode, Int32 wParam, IntPtr lParam)`
  - `BeginInvoke(Int32 nCode, Int32 wParam, IntPtr lParam, AsyncCallback callback, Object object)`
  - `EndInvoke(IAsyncResult result)`
- **Class** `KeyBoardHookStruct` - 1 methods, 0 properties
- **Class** `Win32` - 30 methods, 0 properties
  - `GetBatteryRemaining()`
  - `GetBatteryChargingStatus()`
  - `GetCPUName()`
  - `GetCPUOP()`
  - `GetGPUName()`
  - `GetGPUOP()`
  - `GetDiskName()`
  - `GetDiskOP()`
  - `GetDiskUsed()`
  - `GetDiskTotalSize()`
  - `GetDiskTotalSizeWmi()`
  - `GetMemoryName()`
  - `GetMemoryOP()`
  - `GetMemoryUsed()`
  - `GetMemoryAvailable()`
  - `GetMemoryTotalSize()`
  - `GetBIOSVersion()`
  - `GetSoundVersion()`
  - `GetCpuFrequency()`
  - `GetCpuMaxFrequency()`
  - `GetNVVbiosVersion()`
  - `GetNVGpuFrequency()`
  - `GetNVGpuMaxFrequency()`
  - `GetNVTem()`
  - `ToGB(Double size, Double mod)`
  - `RestartComputer()`
  - `SetWinkeyLock(Boolean flag)`
  - `SetDMicMute(Boolean flag)`
  - `getCom()`
- **Class** `Win32Control` - 8 methods, 0 properties
  - `SetWindowsHookEx(Int32 idHook, HookProc lpfn, IntPtr hInstance, Int32 threadId)`
  - `UnhookWindowsHookEx(Int32 idHook)`
  - `CallNextHookEx(Int32 idHook, Int32 nCode, Int32 wParam, IntPtr lParam)`
  - `GetModuleHandle(String name)`
  - `KeyBoardHookProc(Int32 nCode, Int32 wParam, IntPtr lParam)`
  - `Hook_Start()`
  - `Hook_Clear()`
- **Class** `Wmi` - 50 methods, 0 properties
  - `inMS(Boolean intoMS)`
  - `SMI(UInt32 cmd, Int32 rw, Int32 length, Object data)`
  - `IfNvReady()`
  - `GetSSID()`
  - `GetSkuId()`
  - `GetPhase()`
  - `GetECStatus()`
  - `GetCPUTem()`
  - `GetGPUTem()`
  - `SetAirplaneMode()`
  - `SetSoundMute()`
  - `GetECVersion()`
  - `SetPerformanceMode(Int32 mode)`
  - `GetPerformanceMode()`
  - `getIgpuTem()`
  - `KeyLight(Boolean isOpen)`
  - `BackLight(Boolean isOpen)`
  - `getLightPageStatus()`
  - `setLightPageStatus(Boolean isDefault)`
  - `getLight1()`
  - `getLight2()`
  - `getLightVer()`
  - `GetGBoxPower()`
  - `GetGBoxFanTable()`
  - `SetGBoxLight(Boolean flag)`
  - `GetGBoxLight()`
  - `SetGBoxTurbo(Boolean flag)`
  - `GetGBoxTurbo()`
  - `GetGBoxCustomMode()`
  - `GetGBoxCustomLevel()`
  - `SetGBoxCustomMode()`
  - `SetGBoxCustomLevel(Int32 flag)`
  - `SetGBoxMode(Int32 flag)`
  - `GetGBoxChargeMode()`
  - `GetUSBCharging()`
  - `SetUSBCharging(Boolean isEnable)`
  - `SetGPUMode(Int32 mode)`
  - `GetGPUMode()`
  - `SetFanFullMode(Boolean flag)`
  - `GetFanFullMode()`
  - `GetCPUFanMax()`
  - `GetGPUFanMax()`
  - `GetCPUFan()`
  - `GetGPUFan()`
  - `ECWriteRamCMD(Byte address, Byte value)`
  - `ECReadRamCMD(Byte address)`
  - `Memory(UInt32 address, Int32 rw, Int32 length, Object data)`
  - `IO(UInt32 address, Int32 rw, Byte index, Byte data)`
  - `IOPort(UInt32 address, Int32 rw, Object data)`
- **Class** `WmiServer` - 5 methods, 0 properties
  - `WmiMethod(String methodName, Object inData)`
  - `WmiEvent_Start(String eScope, String eClassName)`
  - `WmiEvent_Stop()`
  - `WmiEvent_Handler(Object sender, EventArrivedEventArgs e)`

## Namespace: `Costura`

- **Class** `AssemblyLoader` - 10 methods, 0 properties
  - `CultureToString(CultureInfo culture)`
  - `ReadExistingAssembly(AssemblyName name)`
  - `CopyTo(Stream source, Stream destination)`
  - `LoadStream(String fullName)`
  - `LoadStream(Dictionary resourceNames, String name)`
  - `ReadStream(Stream stream)`
  - `ReadFromEmbeddedResources(Dictionary assemblyNames, Dictionary symbolNames, AssemblyName requestedAssemblyName)`
  - `ResolveAssembly(Object sender, ResolveEventArgs e)`
  - `Attach()`

## Namespace: `Microsoft.CodeAnalysis`

- **Class** `EmbeddedAttribute` - 1 methods, 0 properties

## Namespace: `System.Runtime.CompilerServices`

- **Class** `NullableAttribute` - 2 methods, 0 properties
- **Class** `NullableContextAttribute` - 1 methods, 0 properties

