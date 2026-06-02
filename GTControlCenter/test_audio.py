import os
import sounddevice as sd
import subprocess
out = subprocess.check_output(["pactl", "info"], text=True)
default_sink = next(line.split(":", 1)[1].strip() for line in out.splitlines() if line.startswith("Default Sink:"))
os.environ["PULSE_SOURCE"] = default_sink + ".monitor"
print("PULSE_SOURCE:", os.environ["PULSE_SOURCE"])

stream = sd.InputStream(device="pulse", blocksize=1024)
print("Device opened:", stream)
