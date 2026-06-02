import json
import os
import logging
from pathlib import Path

logger = logging.getLogger(__name__)

class ConfigManager:
    def __init__(self, filename="GTControl.txt"):
        self.config_dir = Path.home() / ".config" / "GTControlCenter"
        self.config_dir.mkdir(parents=True, exist_ok=True)
        self.filepath = self.config_dir / filename
        self.config = self.get_default_config()
        self.load()

    def get_default_config(self):
        return {
            "performance": {
                "mode": 2,
                "max_fan": False
            },
            "keyboard": {
                "zone": 0,
                "mode": 1,
                "color": "#FF0000",
                "brightness": 100,
                "audio_device": 0,
                "sens": 35,
                "smooth": 0
            },
            "backzone": {
                "mode": 1,
                "color": "#FF0000",
                "brightness": 100,
                "audio_device": 0,
                "sens": 35,
                "smooth": 0
            }
        }

    def load(self):
        if self.filepath.exists():
            try:
                with open(self.filepath, 'r') as f:
                    data = json.load(f)
                    for k, v in data.items():
                        if k in self.config and isinstance(v, dict):
                            self.config[k].update(v)
                        else:
                            self.config[k] = v
            except Exception as e:
                logger.error(f"Failed to load config: {e}")

    def save(self):
        try:
            with open(self.filepath, 'w') as f:
                json.dump(self.config, f, indent=4)
        except Exception as e:
            logger.error(f"Failed to save config: {e}")

    def reset(self):
        self.config = self.get_default_config()
        self.save()
