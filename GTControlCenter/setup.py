from setuptools import setup, find_packages

setup(
    name="gt-controlcenter-linux",
    version="1.0.0",
    packages=find_packages(),
    install_requires=[
        "pyusb",
        "psutil",
        "pyserial",
        "nvidia-ml-py"
    ],
    entry_points={
        'console_scripts': [
            'gt-controlcenter = controlcenter.app:main'
        ]
    }
)
