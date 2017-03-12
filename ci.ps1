# Continuous integration script.

# Restore solution-packages.
.\restore.ps1

# Create packages for Release configuration.
.\make.ps1 -t pack -v normal -Release