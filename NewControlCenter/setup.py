from setuptools import setup, find_packages

setup(
    name="byd-controlcenter-linux",
    version="1.0.0",
    packages=find_packages(),
    install_requires=[
        "pyusb",
        "psutil",
        "pyserial"
    ],
    entry_points={
        'console_scripts': [
            'byd-controlcenter = controlcenter.app:main'
        ]
    }
)
