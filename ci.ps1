# Continuous integration script.

# Restore solution-packages.
.\restore.ps1

# Run tests for the Debug configuration.
.\make.ps1 test -v normal