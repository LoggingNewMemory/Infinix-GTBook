# GT Control Center for Linux - Tutorial

This guide explains how to install, configure, and run the GT Control Center on your Linux system **without requiring `sudo`** every time you launch the GUI.

## 1. Prerequisites

Make sure you have the following packages installed on your system:
- `python3` and `python3-venv`
- `python3-gi` (PyGObject for the GTK 4 / Libadwaita UI)
- `systemd` and `udev` (Standard on most Linux distributions)

*Note: The `gi` module must be installed system-wide for the GTK 4 interface to work properly.*

## 2. System Permissions Installation

To make the application run without `root` permissions, you need to set up rules for the hardware interfaces. This is handled automatically by the `install.sh` script.

1. Open your terminal in the `GTControlCenter` directory.
2. Make the install script executable (if it isn't already):
   ```bash
   chmod +x install.sh
   ```
3. Run the installer with root privileges:
   ```bash
   sudo ./install.sh
   ```

**What this script does behind the scenes:**
- It copies `99-byd-keyboard.rules` to `/etc/udev/rules.d/` granting standard users access to the USB keyboard and the Back Zone serial port (`/dev/ttyS4`).
- It creates and enables a lightweight systemd service (`acpi-call-perms.service`) that automatically changes the permissions of `/proc/acpi/call` to `0666` at boot, allowing the app to read temperatures and control the fan mode safely without a root password.

## 3. Setting Up the Python Environment

To keep dependencies isolated and clean, it is recommended to run the app in a Python Virtual Environment. 

Run the following commands inside the `GTControlCenter` folder:

```bash
# Create a virtual environment with access to system site-packages (required for 'gi' GTK module)
python3 -m venv venv --clear --system-site-packages

# Install the app and its dependencies via pip
./venv/bin/pip install -e .
```

## 4. Running the Application

Once installed, you can launch the app directly using the executable inside the virtual environment:

```bash
./venv/bin/gt-controlcenter
```

*Tip: You can create a `.desktop` shortcut pointing to this exact path to launch it seamlessly from your desktop environment's app menu!*

---

## Troubleshooting

- **`ModuleNotFoundError: No module named 'gi'`**: This happens if your virtual environment cannot access system packages. Make sure you included the `--system-site-packages` flag when creating the `venv` and that `python3-gi` (or `python-gobject` depending on your distro) is installed on your OS.
- **USB/TTY Permission Denied errors**: Ensure the `install.sh` ran successfully. In rare cases, a system reboot might be required for the new `udev` and `systemd` rules to fully apply.
- **Fan control fails**: Ensure the `acpi_call` kernel module is installed and loaded on your system.


## 5. New Features: System Tray, Hardware Keys & CLI

### System Tray Integration
The application now features a persistent background system tray icon! When you close the main GT Control Center window, the tray process keeps running in the background. You can right-click the tray icon to:
- Show the GT Control Center UI
- **Toggle Max Fan**
- Switch Performance Modes (Office, Balanced, Gaming)
- Completely Quit the application

### Hardware Key Integration
Thanks to the upgraded `udev` hardware permissions, the background tray process now natively intercepts the special **F13 (Office)**, **F14 (Balanced)**, and **F15 (Gaming)** physical keyboard keys. Pressing these keys will instantly sync your laptop's hardware fan speed and seamlessly update the GT Control Center GUI and tray menu without any manual KDE shortcut configuration!

### Command Line Interface (CLI)
You can now trigger actions via terminal commands (or custom desktop shortcuts). When executed, these commands instantly synchronize via DBus with the background process and update the UI in real-time:
```bash
# Change Performance Mode
gt-controlcenter --mode office
gt-controlcenter --mode balanced
gt-controlcenter --mode gaming

# Max Fan Control
gt-controlcenter --max-fan toggle
gt-controlcenter --max-fan on
gt-controlcenter --max-fan off
```
